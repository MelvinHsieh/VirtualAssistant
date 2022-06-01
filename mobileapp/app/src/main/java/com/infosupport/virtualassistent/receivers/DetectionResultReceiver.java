package com.infosupport.virtualassistent.receivers;

import android.media.MediaPlayer;

import com.infosupport.virtualassistent.AssistantActivity;
import com.infosupport.virtualassistent.R;

import java.lang.ref.WeakReference;

public class DetectionResultReceiver implements SpeechResultReceiver.ResultReceiverCallBack<String> {
    private final WeakReference<AssistantActivity> activityRef;

    public DetectionResultReceiver(AssistantActivity activity) {
        activityRef = new WeakReference<AssistantActivity>(activity);
    }

    @Override
    public void onSuccess(String data) {
        if (activityRef.get() != null) {
            MediaPlayer mediaPlayer = MediaPlayer.create(activityRef.get(), R.raw.bleep);
            mediaPlayer.start();
            activityRef.get().runSpeechRecognizer();
        }
    }

    @Override
    public void onError(Exception exception) {
        activityRef.get().showMessage("Account info failed", false);
    }
}
