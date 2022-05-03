package com.infosupport.virtualassistent;

import android.Manifest;
import android.content.pm.PackageManager;
import android.os.Build;
import android.os.Bundle;
import android.speech.tts.TextToSpeech;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import androidx.room.Room;

import com.google.android.material.floatingactionbutton.FloatingActionButton;
import com.infosupport.virtualassistent.bot.Bot;
import com.infosupport.virtualassistent.chat.MessageListAdapter;
import com.infosupport.virtualassistent.model.Message;
import com.infosupport.virtualassistent.receivers.RecognizeSpeechResultReceiver;
import com.infosupport.virtualassistent.services.SpeechIntentService;
import com.infosupport.virtualassistent.storage.AppDatabase;
import com.infosupport.virtualassistent.storage.MessageDAO;

import java.util.Calendar;
import java.util.LinkedList;
import java.util.List;
import java.util.Locale;

public class MainActivity extends AppCompatActivity {

    public static final Integer RecordAudioRequestCode = 1;
    private MessageListAdapter messageAdapter;
    private List<Message> messageList;
    private RecyclerView messageRecycler;
    private MessageDAO msgDao;
    private Bot bot;
    private TextToSpeech textToSpeech;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        AppDatabase db = Room.databaseBuilder(getApplicationContext(),
                AppDatabase.class, "message-history").build();
        msgDao = db.messageDao();

        messageList = new LinkedList<>();

        new Thread(() -> {
            List<Message> messages = msgDao.getAll();

            if(messages != null) {
                messageList.addAll(messages);
            }
        }).start();


        messageRecycler = (RecyclerView) findViewById(R.id.recycler_gchat);
        messageAdapter = new MessageListAdapter(messageList);
        LinearLayoutManager llm = new LinearLayoutManager(this);
        llm.setStackFromEnd(true);
        messageRecycler.setLayoutManager(llm);
        messageRecycler.setAdapter(messageAdapter);

        FloatingActionButton fab = findViewById(R.id.fab);
        fab.setOnClickListener(view -> {
            runSpeechRecognizer(fab);
        });
        bot = new Bot(this);

        if (ContextCompat.checkSelfPermission(this, Manifest.permission.RECORD_AUDIO) != PackageManager.PERMISSION_GRANTED) {
            checkMicPermission();
        }

        textToSpeech=new TextToSpeech(getApplicationContext(), status -> {
            if(status != TextToSpeech.ERROR) {
                textToSpeech.setLanguage(new Locale("nl_NL"));
                textToSpeech.setSpeechRate(0.5f);
            }
        });

    }

    // DO NOT REMOVE, WILL BE USED AGAIN
    public void runSpeechRecognizer() {
        FloatingActionButton fab = findViewById(R.id.fab);
        runSpeechRecognizer(fab);
    }

    /** Run the speech recognition service */
    public void runSpeechRecognizer(FloatingActionButton fab) {
        Toast.makeText(getApplicationContext(),"Aan het luisteren...", Toast.LENGTH_SHORT).show();
        fab.setImageResource(R.drawable.mic_active);
        SpeechIntentService.startServiceForRecognizer(this, new RecognizeSpeechResultReceiver(this));
    }

    public void showMessage(String msg, boolean isUser) {
        if(!isUser) {
            textToSpeech.speak(msg, TextToSpeech.QUEUE_FLUSH, null, null);
        }
        FloatingActionButton fab = findViewById(R.id.fab);
        fab.setImageResource(R.drawable.mic_inactive);
        Message message = new Message(msg, isUser, Calendar.getInstance().getTimeInMillis());
        messageList.add(message);
        int pos = messageList.size() - 1;
        messageAdapter.notifyItemInserted(pos);
        messageRecycler.scrollToPosition(pos);

        // Save new message to the database
        new Thread(() -> {
            msgDao.insertAll(message);
        }).start();
    }

    private void checkMicPermission() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.RECORD_AUDIO}, RecordAudioRequestCode);
        }
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        if (requestCode == RecordAudioRequestCode && grantResults.length > 0) {
            if (grantResults[0] == PackageManager.PERMISSION_GRANTED)
                Toast.makeText(this, "Microphone permission Granted", Toast.LENGTH_SHORT).show();
        }
    }

    public Bot getBot() {return bot;}

}