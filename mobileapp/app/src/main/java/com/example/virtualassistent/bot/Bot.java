package com.example.virtualassistent.bot;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.example.virtualassistent.MainActivity;
import com.example.virtualassistent.bot.models.Activity;
import com.example.virtualassistent.bot.models.Conversation;
import com.example.virtualassistent.bot.models.From;
import com.example.virtualassistent.bot.websocket.BotWebSocketClient;
import com.google.gson.Gson;

import org.json.JSONObject;

import java.net.URI;
import java.net.URISyntaxException;
import java.nio.charset.StandardCharsets;
import java.util.HashMap;
import java.util.Map;

public class Bot {

    Conversation conversation = null;
    String secretCode = "xP92GZX8--c.Cx-sJT1V-hbGz2_nkSaC_5pQvPd4anvBpBm7mOwhmYc"; //TODO ergens opbergen in een kluisje
    RequestQueue queue = null;
    Gson gson = null;

    public Bot(MainActivity activity) {
        gson = new Gson();

        queue = Volley.newRequestQueue(activity.getApplicationContext());
        startConversation(activity);
    }

    public void startConversation(MainActivity mainActivity) {
        String conversationURL = "https://directline.botframework.com/v3/directline/conversations";

        JsonObjectRequest startConversationRequest = new JsonObjectRequest(Request.Method.POST, conversationURL, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject response) {
                conversation = gson.fromJson(response.toString(), Conversation.class);
                try {
                    URI serverURI = new URI(conversation.streamUrl);
                    BotWebSocketClient client = new BotWebSocketClient(serverURI, mainActivity);
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

    public void sendMessage(String message, String type) {
        Activity activity = new Activity();
        activity.type = type;
        activity.text = message;
        activity.from = new From("eenID");
        activity.locale = "nl-NL";
        sendActivity(activity);
    }

    public void sendConversationUpdate() {
        sendMessage("Update", "conversationUpdate");
    }

    public void sendMessage(String message) {
        sendMessage(message, "message");
    }

    public void sendActivity(Activity activity) {
        String postActivityURL = "https://directline.botframework.com/v3/directline/conversations/" + conversation.conversationId + "/activities";

        StringRequest messagePostRequest = new StringRequest(Request.Method.POST, postActivityURL, new Response.Listener<String>() {
            @Override
            public void onResponse(String response) {
                System.out.println("POST activity response: " + response);
            }
        }, error -> System.out.println("Error: " + error.toString())) {
            @Override
            public Map<String, String> getHeaders() {
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
            public byte[] getBody() {
                return activity.ToJSON().getBytes(StandardCharsets.UTF_8);
            }
        };

        queue.add(messagePostRequest);
    }
}
