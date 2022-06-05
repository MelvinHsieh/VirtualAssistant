package com.infosupport.virtualassistent.receivers;

import com.infosupport.virtualassistent.AssistantActivity;

import java.lang.ref.WeakReference;

public class RecognizeSpeechResultReceiver implements SpeechResultReceiver.ResultReceiverCallBack<String> {
    private final WeakReference<AssistantActivity> activityRef;

    public RecognizeSpeechResultReceiver(AssistantActivity activity) {
        activityRef = new WeakReference<AssistantActivity>(activity);
    }

    @Override
    public void onSuccess(String data) {
        if (activityRef.get() != null) {
            activityRef.get().showMessage(data, true);
            activityRef.get().getBot().sendMessage(data);
        }
    }

    @Override
    public void onError(Exception exception) {
        activityRef.get().showMessage("Account info failed", false);
    }
}
