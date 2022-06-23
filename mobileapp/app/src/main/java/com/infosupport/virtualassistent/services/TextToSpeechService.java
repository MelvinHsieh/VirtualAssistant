package com.infosupport.virtualassistent.services;

import android.os.AsyncTask;

import com.infosupport.virtualassistent.AssistantActivity;
import com.infosupport.virtualassistent.R;
import com.microsoft.cognitiveservices.speech.CancellationReason;
import com.microsoft.cognitiveservices.speech.ResultReason;
import com.microsoft.cognitiveservices.speech.SpeechConfig;
import com.microsoft.cognitiveservices.speech.SpeechSynthesisCancellationDetails;
import com.microsoft.cognitiveservices.speech.SpeechSynthesisResult;
import com.microsoft.cognitiveservices.speech.SpeechSynthesizer;

import java.lang.ref.WeakReference;
import java.util.concurrent.ExecutionException;

public class TextToSpeechService {
    private final WeakReference<AssistantActivity> activityRef;
    private static final String serviceRegion = "westeurope";
    private final SpeechSynthesizer speechSynthesizer;
    public int turnOnMicAfter = -1;
    public int latestId = 0;

    public TextToSpeechService(AssistantActivity activity) {
        activityRef = new WeakReference<AssistantActivity>(activity);
        String subscriptionKey = activity.getApplicationContext().getString(R.string.microsoft_speech_key);
        SpeechConfig speechConfig = SpeechConfig.fromSubscription(subscriptionKey, serviceRegion);

        speechConfig.setSpeechSynthesisVoiceName("nl-NL-MaartenNeural");

        speechSynthesizer = new SpeechSynthesizer(speechConfig);
    }

    public void Speak(String message) {
        latestId++;
        int id = latestId;
        AsyncTask.execute(() -> {
            if (message.isEmpty()) return;

            SpeechSynthesisResult speechRecognitionResult = null;
            try {
                speechRecognitionResult = speechSynthesizer.SpeakTextAsync(message).get();
            } catch (ExecutionException | InterruptedException e) {
                LoggingService.Log(e.getMessage());
            }

            if (speechRecognitionResult != null) {
                if (speechRecognitionResult.getReason() == ResultReason.SynthesizingAudioCompleted) {
                    if (turnOnMicAfter == id) {
                        activityRef.get().runOnUiThread(() -> activityRef.get().runSpeechRecognizer());
                    }
                } else if (speechRecognitionResult.getReason() == ResultReason.Canceled) {
                    SpeechSynthesisCancellationDetails cancellation = SpeechSynthesisCancellationDetails.fromResult(speechRecognitionResult);
                    LoggingService.Log("TextToSpeech CANCELED: Reason=" + cancellation.getReason());

                    if (cancellation.getReason() == CancellationReason.Error) {
                        LoggingService.Log("TextToSpeech CANCELED: ErrorCode=" + cancellation.getErrorCode());
                        LoggingService.Log("TextToSpeech CANCELED: ErrorDetails=" + cancellation.getErrorDetails());
                        LoggingService.Log("TextToSpeech CANCELED: Did you set the speech resource key and region values?");
                    }
                }
            }
        });
    }

    public void Stop() {
        speechSynthesizer.StopSpeakingAsync();
    }
}
