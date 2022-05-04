package com.infosupport.virtualassistent.receivers;

import com.infosupport.virtualassistent.MainActivity;

import java.lang.ref.WeakReference;

public class DetectionResultReceiver implements SpeechResultReceiver.ResultReceiverCallBack<String> {
    private final WeakReference<MainActivity> activityRef;

    public DetectionResultReceiver(MainActivity activity) {
        activityRef = new WeakReference<MainActivity>(activity);
    }

    @Override
    public void onSuccess(String data) {
        if (activityRef.get() != null) {
            activityRef.get().runSpeechRecognizer();
        }
    }

    @Override
    public void onError(Exception exception) {
        activityRef.get().showMessage("Account info failed", false);
    }
}
