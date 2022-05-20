package com.infosupport.virtualassistent;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.infosupport.virtualassistent.services.AuthService;

public class MainActivity extends AppCompatActivity {

    private EditText usernameEditText;
    private EditText passwordEditText;
    private Button loginButton;
    private AuthService authService;
    private SharedPreferences preferences;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        preferences = PreferenceManager.getDefaultSharedPreferences(getApplicationContext());
        if(!preferences.getString("authToken", "").isEmpty()) {
            startAssistantActivity();
            return;
        }
        setContentView(R.layout.activity_main);

        authService = new AuthService(this);

        usernameEditText = findViewById(R.id.activity_main_usernameEditText);
        passwordEditText = findViewById(R.id.activity_main_passwordEditText);
        loginButton = findViewById(R.id.activity_main_loginButton);

        loginButton.setOnClickListener(v -> {
            if (usernameEditText.getText().length() > 0 && passwordEditText.getText().length() > 0) {
                String username = usernameEditText.getText().toString();
                String password = passwordEditText.getText().toString();
                authService.sendLoginInfo(username, password, bool -> {
                    if(bool) startAssistantActivity();
                });
            } else {
                String toastMessage = "Vul je gebruikersnaam en wachtwoord in";
                Toast.makeText(getApplicationContext(), toastMessage, Toast.LENGTH_LONG).show();
            }
        });
    }

    private void startAssistantActivity() {
        Intent intent = new Intent(this, AssistantActivity.class);
        startActivity(intent);
    }

    /** Run the speech recognition service */
    private void runSpeechRecognizer(FloatingActionButton fab) {
        textToSpeech.stop();
        Toast.makeText(getApplicationContext(),"Aan het luisteren...", Toast.LENGTH_SHORT).show();
        fab.setImageResource(R.drawable.mic_active);
        SpeechIntentService.startServiceForRecognizer(this, new RecognizeSpeechResultReceiver(this));
    }

    public void showMessage(String msg, boolean isUser) {
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

        if(!isUser) {
            textToSpeech.speak(msg, TextToSpeech.QUEUE_FLUSH, null, null);
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

    public Bot getBot() {return bot;}

}