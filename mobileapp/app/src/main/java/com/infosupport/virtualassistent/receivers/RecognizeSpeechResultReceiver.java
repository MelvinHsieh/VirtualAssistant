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
            if(!data.isEmpty()) {
                activityRef.get().showMessage(data, true, false);
                activityRef.get().getBot().sendMessage(data);
                activityRef.get().isListening = false;
            } else {
                activityRef.get().isListening = false;
                // currently commented out, when no result is received it will just do nothing.
                //activityRef.get().showMessage("Sorry dat heb ik niet verstaan.", false, false);
            }
        }
    }

    @Override
    public void onError(Exception exception) {
        activityRef.get().showMessage("Account info failed", false, false);
    }
}
