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

}