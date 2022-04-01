package com.avans.virtualassistant;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.Volley;

import java.net.MalformedURLException;
import java.net.URL;

public class MainActivity extends AppCompatActivity {

    private Button ask;
    private TextView question;
    private TextView answer;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        ask = findViewById(R.id.button_ask);
        ask.setOnClickListener(view -> {
            AskQuestion(view);
        });

        question = findViewById(R.id.question);
        question.setText("Hoeveel paracetamollen moet ik slikken?");

        answer = findViewById(R.id.answer);
    }

    //POST
    public void AskQuestion(View view) {
        //speech to text
        //textview om het erin te verwerken

        RequestQueue queue = Volley.newRequestQueue(this);
        URL url = null;

        try {
            url = new URL("http://time.jsontest.com");
        } catch (MalformedURLException e) {
            e.printStackTrace();
        }

        JsonObjectRequest req = new JsonObjectRequest(Request.Method.GET, url.toString(), null, response -> {
            answer.setText(response.toString());
        }, error -> {
            answer.setText("20 pillen");
        });

        queue.add(req);

        question.setText("");
    }
}