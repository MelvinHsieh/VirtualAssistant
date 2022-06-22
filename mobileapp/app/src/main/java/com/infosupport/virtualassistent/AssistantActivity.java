package com.infosupport.virtualassistent;

import android.Manifest;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.content.pm.PackageManager;
import android.os.Build;
import android.os.Bundle;
import android.os.Handler;
import android.os.IBinder;
import android.os.Messenger;
import android.os.RemoteException;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
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
import com.infosupport.virtualassistent.receivers.DetectionResultReceiver;
import com.infosupport.virtualassistent.receivers.RecognizeSpeechResultReceiver;
import com.infosupport.virtualassistent.receivers.SpeechResultReceiver;
import com.infosupport.virtualassistent.services.AuthService;
import com.infosupport.virtualassistent.services.SpeechIntentService;
import com.infosupport.virtualassistent.services.TextToSpeechService;
import com.infosupport.virtualassistent.services.WakeWordService;
import com.infosupport.virtualassistent.storage.AppDatabase;
import com.infosupport.virtualassistent.storage.MessageDAO;

import java.util.Calendar;
import java.util.LinkedList;
import java.util.List;

public class AssistantActivity extends AppCompatActivity {

    public static final Integer RecordAudioRequestCode = 1;
    private MessageListAdapter messageAdapter;
    private List<Message> messageList;
    private RecyclerView messageRecycler;
    private MessageDAO msgDao;
    private Bot bot;
    public Boolean isListening;
    public Messenger speechServiceMessenger;
    public SpeechIntentService speechIntentService;
    public TextToSpeechService textToSpeech;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_assistant);
        isListening = false;

        dbInit();
        textToSpeech = new TextToSpeechService(this);

        // Start the speechIntentService and bind it to the serviceConnection for internal communication purposes.
        speechIntentService = new SpeechIntentService();
        bindService(new Intent(this, SpeechIntentService.class), serviceConnection,
                Context.BIND_AUTO_CREATE);

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
        startWakeWordService();
    }

    private void dbInit() {
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
    }

    private final ServiceConnection serviceConnection = new ServiceConnection() {
        public void onServiceConnected(ComponentName className, IBinder iBinder) {
            speechServiceMessenger = new Messenger(iBinder);
        }
        public void onServiceDisconnected(ComponentName className) {
        }
    };

    private void startWakeWordService() {
        SpeechResultReceiver speechResultReceiver = new SpeechResultReceiver(new Handler(getApplicationContext().getMainLooper()));
        speechResultReceiver.setReceiver(new DetectionResultReceiver(this));

        Intent serviceIntent = new Intent(this, WakeWordService.class);
        serviceIntent.putExtra(WakeWordService.RESULT_RECEIVER, speechResultReceiver);
        ContextCompat.startForegroundService(this, serviceIntent);
    }

    private void stopWakeWordService() {
        Intent serviceIntent = new Intent(this, WakeWordService.class);
        stopService(serviceIntent);
    }

    public void runSpeechRecognizer() {
        FloatingActionButton fab = findViewById(R.id.fab);
        runSpeechRecognizer(fab);
    }

    /** Run the speech recognition service */
    private void runSpeechRecognizer(FloatingActionButton fab) {
        if(isListening) {
            //stop listening
            Bundle bundle = new Bundle();
            bundle.putBoolean("stopListening", true);
            android.os.Message message = android.os.Message.obtain();
            message.setData(bundle);
            try {
                speechServiceMessenger.send(message);
            } catch (RemoteException e) {
                e.printStackTrace();
            }
            isListening = false;
        } else {
            textToSpeech.Stop();
            Toast.makeText(getApplicationContext(), "Aan het luisteren...", Toast.LENGTH_SHORT).show();
            fab.setImageResource(R.drawable.mic_active);
            speechIntentService.startServiceForRecognizer(this, new RecognizeSpeechResultReceiver(this));
            isListening = true;
        }
    }

    public void turnMicOnAfterCurrentUtterance() {
        textToSpeech.turnOnMicAfter = textToSpeech.latestId;
    }

    public void showMessage(String msg, boolean isUser, boolean isImage) {
        FloatingActionButton fab = findViewById(R.id.fab);
        fab.setImageResource(R.drawable.mic_inactive);
        Message message = new Message(msg, isUser, isImage, Calendar.getInstance().getTimeInMillis());
        messageList.add(message);
        int pos = messageList.size() - 1;
        messageAdapter.notifyItemInserted(pos);
        messageRecycler.scrollToPosition(pos);

        // Save new message to the database
        new Thread(() -> {
            msgDao.insertAll(message);
        }).start();

        if(!isUser && !isImage) {
            textToSpeech.Speak(msg);
        }
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

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        MenuInflater inflater = getMenuInflater();
        inflater.inflate(R.menu.menu, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle item selection
        if (item.getItemId() == R.id.logout) {
            AuthService as = new AuthService(this);
            as.logOut();
            stopWakeWordService();
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    public Bot getBot() {return bot;}

    @Override
    protected void onStop() {
        super.onStop();
        textToSpeech.Stop();
    }
}