package com.example.virtualassistent.bot.models;

import org.json.JSONException;
import org.json.JSONObject;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;

public class Message {
    public Message() {
    }

    public Message(String jsonString) throws JSONException {
        this(new JSONObject(jsonString));
    }

    public Message(JSONObject json) throws JSONException {
        if(json.isNull("id") == false) {
            this.id = json.getString("id");
        }

        if(json.isNull("conversationId") == false) {
            this.conversationId = json.getString("conversationId");
        }

        if(json.isNull("created") == false) {
            SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss");
            try {
                this.created = dateFormat.parse(json.getString("created"));
            } catch (ParseException e) {
                e.printStackTrace();
            }
        }

        if(json.isNull("from") == false) {
            this.from = json.getString("from");
        }

        if(json.isNull("text") == false) {
            this.text = json.getString("text");
        }

        if(json.isNull("eTag") == false) {
            this.eTag = json.getString("eTag");
        }
    }


    public String id = null;
    public String conversationId = null;
    public Date created = null;
    public String from = null;
    public String text = null;
    public Object channelData = null;
    public List<String> images = null;
    public List<Attachment> attachments = null;
    public String eTag = null;

    public JSONObject ToJSONObject() {
        JSONObject json = new JSONObject();

        try {

            json.put("id", this.id);
            json.put("conversationId", this.conversationId);
            if(this.created != null) {
                json.put("created", this.created.toString());
            }

            json.put("from", this.from);
            json.put("text", this.text);
            json.put("eTag", this.eTag);
        } catch (JSONException e) {
            e.printStackTrace();
        }

        return json;
    }

    public String ToJSON() {
        return this.ToJSONObject().toString();
    }
}