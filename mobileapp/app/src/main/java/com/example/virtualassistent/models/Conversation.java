package com.example.virtualassistent.models;

import org.json.JSONException;
import org.json.JSONObject;

public class Conversation {
    public Conversation() {}

    public Conversation(String jsonString) throws JSONException {
        this(new JSONObject(jsonString));
    }

    public Conversation(JSONObject json) throws JSONException {
        if (json.isNull("conversationId") == false) {
            conversationId = json.getString("conversationId");
        }

        if (json.isNull("eTag") == false) {
            eTag = json.getString("eTag");
        }

        if (json.isNull("token") == false) {
            token = json.getString("token");
        }
    }

    public String conversationId = null;
    public String eTag = null;
    public String token = null;
}
