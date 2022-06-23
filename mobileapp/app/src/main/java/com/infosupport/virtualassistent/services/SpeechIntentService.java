package com.infosupport.virtualassistent.services;

import android.app.IntentService;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.IBinder;
import android.os.Message;
import android.os.Messenger;
import android.os.ResultReceiver;

import androidx.annotation.NonNull;

import com.infosupport.virtualassistent.R;
import com.infosupport.virtualassistent.receivers.SpeechResultReceiver;
import com.microsoft.cognitiveservices.speech.CancellationReason;
import com.microsoft.cognitiveservices.speech.ResultReason;
import com.microsoft.cognitiveservices.speech.SpeechConfig;
import com.microsoft.cognitiveservices.speech.SpeechRecognizer;
import com.microsoft.cognitiveservices.speech.audio.AudioConfig;

import java.util.concurrent.ExecutionException;
import java.util.concurrent.Semaphore;

public class SpeechIntentService extends IntentService {

    private String SubscriptionKey;
    private static final String ServiceRegion = "westeurope";
    private static Semaphore stopTranslationSemaphore;
    final Messenger messenger = new Messenger(new ServiceHandler());
    private SpeechRecognizer speechRecognizer;

    private static final String ACTION_RECOGNIZE = "com.infosupport.virtualassistent.services.action.RECOGNIZE";
    private static final String RESULT_RECEIVER = "com.infosupport.virtualassistent.services.extra.RESULT_RECEIVER";

    public SpeechIntentService() {
        super("SpeechIntentService");
    }

    @Override
    protected void onHandleIntent(Intent intent) {
        SubscriptionKey = getApplicationContext().getString(R.string.microsoft_speech_key);
        ResultReceiver resultReceiver = intent.getParcelableExtra(RESULT_RECEIVER);

        final String action = intent.getAction();
        if (ACTION_RECOGNIZE.equals(action)) {
            try {
                handleActionRecognize(resultReceiver);
            } catch (ExecutionException | InterruptedException e) {
                e.printStackTrace();
            }
        }
    }

    /**
     * Handle the recognizer in a worker thread
     */
    private void handleActionRecognize(ResultReceiver resultReceiver) throws ExecutionException, InterruptedException {
        Bundle bundle = new Bundle();
        SpeechConfig speechConfig = SpeechConfig.fromSubscription(SubscriptionKey, ServiceRegion);
        speechConfig.setSpeechRecognitionLanguage("nl-NL");

        AudioConfig audioConfig = AudioConfig.fromDefaultMicrophoneInput();
        speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        stopTranslationSemaphore = new Semaphore(0);

        speechRecognizer.startContinuousRecognitionAsync().get();

        speechRecognizer.recognized.addEventListener((s, e) -> {
            if (e.getResult().getReason() == ResultReason.RecognizedSpeech) {
                // Speech is recognized
                bundle.putSerializable(SpeechResultReceiver.PARAM_RESULT, e.getResult().getText());
            }
            else if (e.getResult().getReason() == ResultReason.NoMatch) {
                // No speech was detected (so it was silent)
                bundle.putSerializable(SpeechResultReceiver.PARAM_RESULT, "");
            }
            speechRecognizer.stopContinuousRecognitionAsync();
            if(resultReceiver != null) {
                resultReceiver.send(SpeechResultReceiver.RESULT_CODE_OK, bundle);
            }
        });

        speechRecognizer.canceled.addEventListener((s, e) -> {

            if (e.getReason() == CancellationReason.Error) {
                // An error occurred
                bundle.putSerializable(SpeechResultReceiver.PARAM_RESULT, "Er is helaas iets fout gegaan met de spraakherkenner.");
                if(resultReceiver != null) {
                    resultReceiver.send(SpeechResultReceiver.RESULT_CODE_OK, bundle);
                }
            }

            stopTranslationSemaphore.release();
        });

        speechRecognizer.sessionStopped.addEventListener((s, e) -> {
            stopTranslationSemaphore.release();
        });
    }

    public void startServiceForRecognizer(@NonNull Context context, SpeechResultReceiver.ResultReceiverCallBack resultReceiverCallBack){
        SpeechResultReceiver speechResultReceiver = new SpeechResultReceiver(new Handler(context.getMainLooper()));
        speechResultReceiver.setReceiver(resultReceiverCallBack);

        Intent intent = new Intent(context, SpeechIntentService.class);
        intent.setAction(ACTION_RECOGNIZE);
        intent.putExtra(RESULT_RECEIVER, speechResultReceiver);
        context.startService(intent);
    }

    @Override
    public IBinder onBind(Intent intent) {
        return messenger.getBinder();
    }

    class ServiceHandler extends Handler {
        @Override
        public void handleMessage(Message msg) {
            if(msg.getData().getBoolean("stopListening")) {
                speechRecognizer.stopContinuousRecognitionAsync();
            }
            super.handleMessage(msg);
        }
    }

}