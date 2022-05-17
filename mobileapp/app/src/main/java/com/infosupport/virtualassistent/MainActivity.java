package com.infosupport.virtualassistent;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.infosupport.virtualassistent.services.AuthService;

public class MainActivity extends AppCompatActivity {

    private EditText usernameEditText;
    private EditText passwordEditText;
    private Button loginButton;
    public AuthService authService;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        authService = new AuthService(this);

        usernameEditText = findViewById(R.id.activity_main_usernameEditText);
        passwordEditText = findViewById(R.id.activity_main_passwordEditText);
        loginButton = findViewById(R.id.activity_main_loginButton);

        loginButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
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
            }
        });
    }

    private void startAssistantActivity() {
        Intent intent = new Intent(this, AssistantActivity.class);
        startActivity(intent);
    }

}