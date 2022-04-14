package com.example.virtualassistent;

import android.os.Bundle;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.virtualassistent.bot.Bot;
import com.example.virtualassistent.chat.MessageListAdapter;
import com.example.virtualassistent.model.Message;
import com.example.virtualassistent.receivers.RecognizeSpeechResultReceiver;
import com.example.virtualassistent.services.SpeechIntentService;
import com.example.virtualassistent.storage.MessageDAO;
import com.google.android.material.floatingactionbutton.FloatingActionButton;

import java.util.Calendar;
import java.util.LinkedList;
import java.util.List;

public class MainActivity extends AppCompatActivity {

    private MessageListAdapter messageAdapter;
    private List<Message> messageList;
    private MessageDAO msgDao;
    private Bot bot;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

//        AppDatabase db = Room.databaseBuilder(getApplicationContext(),
//                AppDatabase.class, "message-history").build();
//        msgDao = db.messageDao();
//        List<Message> messages = msgDao.getAll();

        messageList = new LinkedList<>();
//        messageList.addAll(messages);

        RecyclerView messageRecycler = (RecyclerView) findViewById(R.id.recycler_gchat);
        messageAdapter = new MessageListAdapter(messageList);
        messageRecycler.setLayoutManager(new LinearLayoutManager(this));
        messageRecycler.setAdapter(messageAdapter);

        FloatingActionButton fab = findViewById(R.id.fab);
        fab.setOnClickListener(view -> {
            runSpeechRecognizer(fab);
        });
        bot = new Bot(this);
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
        FloatingActionButton fab = findViewById(R.id.fab);
        fab.setImageResource(R.drawable.mic_inactive);
        Message message = new Message(msg, isUser, Calendar.getInstance().getTimeInMillis());
        messageList.add(message);
        messageAdapter.notifyItemInserted(messageList.size() - 1);

        //Save new message to the database
//        msgDao.insertAll(message);
    }

    public Bot getBot() {return bot;}

}