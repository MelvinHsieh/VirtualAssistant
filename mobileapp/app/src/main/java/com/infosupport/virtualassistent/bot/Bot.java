package com.infosupport.virtualassistent.bot;

import android.content.SharedPreferences;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.google.gson.Gson;
import com.infosupport.virtualassistent.AssistantActivity;
import com.infosupport.virtualassistent.R;
import com.infosupport.virtualassistent.bot.models.Activity;
import com.infosupport.virtualassistent.bot.models.Conversation;
import com.infosupport.virtualassistent.bot.models.From;
import com.infosupport.virtualassistent.bot.websocket.BotWebSocketClient;
import com.infosupport.virtualassistent.services.LoggingService;

import org.json.JSONObject;

import java.net.URI;
import java.net.URISyntaxException;
import java.nio.charset.StandardCharsets;
import java.util.HashMap;
import java.util.Map;

public class Bot {
    String directLineURL;
    Conversation conversation = null;
    String secretCode;
    RequestQueue queue = null;
    Gson gson = null;
    SharedPreferences preferences;

    public Bot(AssistantActivity activity) {
        secretCode = activity.getApplicationContext().getString(R.string.bot_secret_code);
        gson = new Gson();
        queue = Volley.newRequestQueue(activity.getApplicationContext());
        startConversation(activity);
    }

    public void startConversation(AssistantActivity assistantActivity) {
        directLineURL = assistantActivity.getApplicationContext().getString(R.string.direct_line_url);

        JsonObjectRequest startConversationRequest = new JsonObjectRequest(Request.Method.POST, directLineURL, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject response) {
                conversation = gson.fromJson(response.toString(), Conversation.class);
                try {
                    URI serverURI = new URI(conversation.streamUrl);
                    BotWebSocketClient client = new BotWebSocketClient(serverURI, assistantActivity);
                    client.connect();
                    sendConversationUpdate();
                } catch (URISyntaxException e) {
                    LoggingService.Log(e.getMessage());
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
        activity.from = new From(preferences.getString("userId", "NoId"));
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
        String postActivityURL = directLineURL + "/" + conversation.conversationId + "/activities";

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
