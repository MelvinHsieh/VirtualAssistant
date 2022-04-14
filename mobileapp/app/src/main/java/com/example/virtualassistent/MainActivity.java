package com.example.virtualassistent;

import android.os.Bundle;
import android.view.View;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;

import com.android.volley.AuthFailureError;
import com.android.volley.NetworkResponse;
import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.example.virtualassistent.bot.models.Activity;
import com.example.virtualassistent.bot.models.Conversation;
import com.example.virtualassistent.bot.models.From;
import com.example.virtualassistent.bot.models.Message;
import com.example.virtualassistent.bot.models.MessageSet;
import com.example.virtualassistent.bot.websocket.BotWebSocketClient;
import com.example.virtualassistent.recievers.SpeechResultReciever;
import com.example.virtualassistent.services.SpeechIntentService;
import com.google.gson.Gson;

import org.json.JSONObject;

import java.io.UnsupportedEncodingException;
import java.lang.ref.WeakReference;
import java.net.URI;
import java.net.URISyntaxException;
import java.util.Date;
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
        String conversationURL = "https://directline.botframework.com/v3/directline/conversations";

        JsonObjectRequest startConversationRequest = new JsonObjectRequest(Request.Method.POST, conversationURL, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject response) {
                conversation = gson.fromJson(response.toString(), Conversation.class);
                try {
                    URI serverURI = new URI(conversation.streamUrl);
                    BotWebSocketClient client = new BotWebSocketClient(serverURI);
                    client.connect();
                    sendConversationUpdate();
                } catch (URISyntaxException e) {
                    e.printStackTrace();
                }
            }
        }, error -> System.out.println(error.toString())) {
            @Override
            public Map<String, String> getHeaders() {
                HashMap<String, String> headers = new HashMap<String, String>();
                headers.put("Authorization", "Bearer " + secretCode);
                headers.put("Content-Type", "application/json");
                return headers;
            }
        };

        queue.add(startConversationRequest);
    }

    public void sendConversationUpdate() {
        Activity activity = new Activity();
        activity.type = "conversationUpdate";
    }

    public void sendMessage(String message) {
        Activity activity = new Activity();
        activity.type = "message";
        activity.text = message;
        activity.from = new From("eenID");
        activity.locale = "nl-NL";
    }

    public void sendActivity(Activity activity) {
        String postActivityURL = "https://directline.botframework.com/v3/directline/conversations/" + conversation.conversationId + "/activities";

        StringRequest messagePostRequest = new StringRequest(Request.Method.POST, postActivityURL, new Response.Listener<String>() {
            @Override
            public void onResponse(String response) {
                System.out.println("POST activity response: " + response);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                System.out.println("Error: " + error.toString());
            }
        }) {
            @Override
            public Map<String, String> getHeaders() throws AuthFailureError {
                Map<String, String>  params = new HashMap<String, String>();
                params.put("Content-Type", "application/json");
                params.put("Authorization", "Bearer " + secretCode);

                return params;
            }

            @Override
            public String getBodyContentType() {
                return "application/json; charset=utf-8";
            }

            @Override
            public byte[] getBody() throws AuthFailureError {
                try {
                    return activity.ToJSON().getBytes("utf-8");
                } catch (UnsupportedEncodingException uee) {
                    return null;
                }
            }
        };

        queue.add(messagePostRequest);
    }

    public void readMessage() {
        MessageSet messageSet = null;
        String messageSetPath = "https://directline.botframework.com/api/conversations/" + conversation.conversationId + "/messages/";

        StringRequest messagePostRequest = new StringRequest(Request.Method.GET, messageSetPath, new Response.Listener<String>() {
            @Override
            public void onResponse(String response) {
                System.out.println(response);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                System.out.println("Error: " + error.toString());
            }
        }) {
            @Override
            public Map<String, String> getHeaders() throws AuthFailureError {
                Map<String, String>  params = new HashMap<String, String>();
                params.put("Content-Type", "application/json");
                params.put("Authorization", "BotConnector " + secretCode);

                return params;
            }
        };

        queue.add(messagePostRequest);
    }
}