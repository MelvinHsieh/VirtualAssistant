package com.infosupport.virtualassistent.bot.models;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

public class MessageSet {
    public MessageSet() {}

    public MessageSet(String jsonString) throws JSONException {
        this(new JSONObject(jsonString));
    }

    public MessageSet(JSONObject json) throws JSONException {
        if (json.has("watermark") && !json.isNull("watermark")) {
            this.watermark = json.getString("watermark");
        }

        if (json.has("eTag") && !json.isNull("eTag")) {
            this.eTag = json.getString("eTag");
        }

        if (json.has("messages") && !json.isNull("messages")) {
            JSONArray array = json.getJSONArray("messages");
            int arrayLength = array.length();
            if (arrayLength > 0) {
                this.messages = new ArrayList<Message>();

                for (int i = 0; i < arrayLength; ++i) {
                    this.messages.add(new Message(array.getJSONObject(i)));
                }
            }
        }
    }

    public List<Message> messages;
    public String watermark;
    public String eTag;
}
