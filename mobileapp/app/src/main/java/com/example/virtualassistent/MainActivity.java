package com.example.virtualassistent;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;

import com.example.virtualassistent.chat.MessageListAdapter;
import com.example.virtualassistent.model.Message;
import com.example.virtualassistent.recievers.SpeechResultReciever;
import com.example.virtualassistent.services.SpeechIntentService;
import com.google.android.material.floatingactionbutton.FloatingActionButton;
import com.google.android.material.snackbar.Snackbar;

import org.w3c.dom.Text;

import java.lang.ref.WeakReference;
import java.util.Arrays;
import java.util.Calendar;
import java.util.LinkedList;
import java.util.List;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class MainActivity extends AppCompatActivity {

    private RecyclerView messageRecycler;
    private MessageListAdapter messageAdapter;
    private List<Message> messageList;
    ExecutorService executorService = Executors.newFixedThreadPool(4);

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        messageList = new LinkedList<>();
        messageList.add(new Message("Hey, hoe kan ik je helpen?", false, Calendar.getInstance().getTimeInMillis()));
        messageList.add(new Message("Niet, hou je bek nou eens", true, Calendar.getInstance().getTimeInMillis() + 1000));

        messageRecycler = (RecyclerView) findViewById(R.id.recycler_gchat);
        messageAdapter = new MessageListAdapter(messageList);
        messageRecycler.setLayoutManager(new LinearLayoutManager(this));
        messageRecycler.setAdapter(messageAdapter);

        FloatingActionButton fab = findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Snackbar.make(view, "Aan het luisteren...", Snackbar.LENGTH_LONG)
                        .setAction("Action", null).show();
                fab.setImageResource(R.drawable.mic_active);
                runSpeechRecognizer();
            }
        });
    }

    /** Run the speech recognition service */
    public void runSpeechRecognizer() {
//        TextView listenedText = findViewById(R.id.listenedText);
//        String message = listenedText.getText().toString();
        SpeechIntentService.startServiceForRecognizer(this, new RecognizeSpeechResultReceiver(this));
//        SpeechIntentService.startRecognize(view.getContext());

    }

    private void showMessage(String msg) {
        FloatingActionButton fab = findViewById(R.id.fab);
        fab.setImageResource(R.drawable.mic_inactive);
        Message message = new Message(msg, true, Calendar.getInstance().getTimeInMillis());
        messageList.add(message);
        messageAdapter.notifyItemInserted(messageList.size() - 1);
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