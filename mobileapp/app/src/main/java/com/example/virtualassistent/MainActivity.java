package com.example.virtualassistent;

import androidx.appcompat.app.AppCompatActivity;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;

import com.example.virtualassistent.recievers.SpeechResultReciever;
import com.example.virtualassistent.services.SpeechIntentService;

import org.w3c.dom.Text;

import java.lang.ref.WeakReference;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class MainActivity extends AppCompatActivity {

    ExecutorService executorService = Executors.newFixedThreadPool(4);

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }

    /** Run the speech recognition service */
    public void runSpeechRecognizer(View view) {
//        TextView listenedText = findViewById(R.id.listenedText);
//        String message = listenedText.getText().toString();
        SpeechIntentService.startServiceForRecognizer(this, new RecognizeSpeechResultReceiver(this));
//        SpeechIntentService.startRecognize(view.getContext());

    }

    private void showMessage(String msg) {
        TextView textView = findViewById(R.id.listenedText);
        textView.setText(msg);
    }

    private static class RecognizeSpeechResultReceiver implements SpeechResultReciever.ResultReceiverCallBack<String> {
        private final WeakReference<MainActivity> activityRef;
        public RecognizeSpeechResultReceiver(MainActivity activity){
            activityRef = new WeakReference<MainActivity>(activity);
        }

        @Override
        public void onSuccess(String data) {
            if(activityRef.get() != null) {
                activityRef.get().showMessage(data);
            }
        }

        @Override
        public void onError(Exception exception) {
            activityRef.get().showMessage("Account info failed");
        }
    }
}