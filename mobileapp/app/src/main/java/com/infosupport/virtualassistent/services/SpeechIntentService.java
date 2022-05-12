package com.infosupport.virtualassistent.services;

import android.app.IntentService;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.ResultReceiver;

import androidx.annotation.NonNull;

import com.infosupport.virtualassistent.R;
import com.infosupport.virtualassistent.receivers.SpeechResultReceiver;
import com.microsoft.cognitiveservices.speech.CancellationDetails;
import com.microsoft.cognitiveservices.speech.ResultReason;
import com.microsoft.cognitiveservices.speech.SpeechConfig;
import com.microsoft.cognitiveservices.speech.SpeechRecognitionResult;
import com.microsoft.cognitiveservices.speech.SpeechRecognizer;
import com.microsoft.cognitiveservices.speech.audio.AudioConfig;

import java.util.concurrent.ExecutionException;
import java.util.concurrent.Future;

public class SpeechIntentService extends IntentService {

    private String SubscriptionKey;
    private static final String ServiceRegion = "westeurope";

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
        System.out.println("Dit is een test");
        Bundle bundle = new Bundle();
        SpeechConfig speechConfig = SpeechConfig.fromSubscription(SubscriptionKey, ServiceRegion);
        speechConfig.setSpeechRecognitionLanguage("nl-NL");

        AudioConfig audioConfig = AudioConfig.fromDefaultMicrophoneInput();
        SpeechRecognizer speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        Future<SpeechRecognitionResult> task = speechRecognizer.recognizeOnceAsync();
        SpeechRecognitionResult speechRecognitionResult = task.get();

        if (speechRecognitionResult.getReason() == ResultReason.RecognizedSpeech) {
            bundle.putSerializable(SpeechResultReceiver.PARAM_RESULT, speechRecognitionResult.getText());
        }
        else if (speechRecognitionResult.getReason() == ResultReason.NoMatch) {
            bundle.putSerializable(SpeechResultReceiver.PARAM_RESULT, "Sorry, dat kon ik niet verstaan.");
        }
        else if (speechRecognitionResult.getReason() == ResultReason.Canceled) {
            CancellationDetails cancellation = CancellationDetails.fromResult(speechRecognitionResult);
            bundle.putSerializable(SpeechResultReceiver.PARAM_RESULT, "Er is helaas iets fout gegaan met de spraakherkenner.");
        }

        if(resultReceiver != null){
            resultReceiver.send(SpeechResultReceiver.RESULT_CODE_OK, bundle);
        }
    }

    public static void startServiceForRecognizer(@NonNull Context context, SpeechResultReceiver.ResultReceiverCallBack resultReceiverCallBack){
        SpeechResultReceiver speechResultReceiver = new SpeechResultReceiver(new Handler(context.getMainLooper()));
        speechResultReceiver.setReceiver(resultReceiverCallBack);

        Intent intent = new Intent(context, SpeechIntentService.class);
        intent.setAction(ACTION_RECOGNIZE);
        intent.putExtra(RESULT_RECEIVER, speechResultReceiver);
        context.startService(intent);
    }

}