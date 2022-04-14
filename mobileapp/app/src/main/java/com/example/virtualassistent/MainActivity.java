package com.example.virtualassistent;

import android.os.Bundle;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.android.volley.AuthFailureError;
import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.Volley;
import com.example.virtualassistent.models.Conversation;
import com.example.virtualassistent.models.Message;
import com.example.virtualassistent.recievers.SpeechResultReciever;
import com.example.virtualassistent.services.SpeechIntentService;
import com.google.gson.Gson;

import org.json.JSONObject;

import java.lang.ref.WeakReference;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class MainActivity extends AppCompatActivity {

    ExecutorService executorService = Executors.newFixedThreadPool(4);
    Conversation conversation = null;
    String secretCode = "xP92GZX8--c.Cx-sJT1V-hbGz2_nkSaC_5pQvPd4anvBpBm7mOwhmYc"; //TODO ergens opbergen in een kluisje
    RequestQueue queue = null;
    Gson gson = null;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        gson = new Gson();
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        queue = Volley.newRequestQueue(this);
        startConversation();
    }

    /**
     * Run the speech recognition service
     */
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

        public RecognizeSpeechResultReceiver(MainActivity activity) {
            activityRef = new WeakReference<MainActivity>(activity);
        }

        @Override
        public void onSuccess(String data) {
            if (activityRef.get() != null) {
                activityRef.get().showMessage(data);
            }
        }

        @Override
        public void onError(Exception exception) {
            activityRef.get().showMessage("Account info failed");
        }
    }

    public void startConversation() {
        URL directLineUrl = null;

        try {
            directLineUrl = new URL("https://directline.botframework.com/api/conversations/");
        } catch (MalformedURLException e) {
            e.printStackTrace();
        }

        JsonObjectRequest req = new JsonObjectRequest(Request.Method.POST, directLineUrl.toString(), null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject response) {
                conversation = gson.fromJson(response.toString(), Conversation.class);
                sendMessage();
            }
        }, error -> System.out.println(error.toString())) { //????
            @Override
            public Map<String, String> getHeaders() throws AuthFailureError {
                HashMap<String, String> headers = new HashMap<String, String>();
                headers.put("Authorization", "BotConnector " + secretCode);
                headers.put("Content-Type", "application/json");
                return headers;
            }
        };

        queue.add(req);
    }

    public void sendMessage() {
        Message message = new Message();
        message.text = "[SOME TEXT TO SEND TO YOUR BOT]";
        String botUrl = "https://directline.botframework.com/api/conversations/" + conversation.conversationId + "/messages/";
        boolean messageSent = false;
        URL messageUrl = null;

        try {
            messageUrl = new URL(botUrl);
        } catch (MalformedURLException e) {
            e.printStackTrace();
        }

        JsonObjectRequest req = new JsonObjectRequest(Request.Method.POST, botUrl.toString(), message.toJsonObject(), new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject response) {
                System.out.println("massage: " + response.toString());
            }
        }, error -> System.out.println(error.toString())) { //????
            @Override
            public Map<String, String> getHeaders() throws AuthFailureError {
                HashMap<String, String> headers = new HashMap<String, String>();
                headers.put("Authorization", "BotConnector " + secretCode);
                headers.put("Content-Type", "application/json");
                return headers;
            }
        };

        queue.add(req);
    }
}