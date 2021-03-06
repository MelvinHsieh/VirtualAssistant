package com.infosupport.virtualassistent.bot.models;

import com.infosupport.virtualassistent.services.LoggingService;

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
        if(!json.isNull("id")) {
            this.id = json.getString("id");
        }

        if(!json.isNull("conversationId")) {
            this.conversationId = json.getString("conversationId");
        }

        if(!json.isNull("created")) {
            SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss");
            try {
                this.created = dateFormat.parse(json.getString("created"));
            } catch (ParseException e) {
                LoggingService.Log(e.getMessage());
            }
        }

        if(!json.isNull("from")) {
            this.from = json.getString("from");
        }

        if(!json.isNull("text")) {
            this.text = json.getString("text");
        }

        if(!json.isNull("eTag")) {
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
            LoggingService.Log(e.getMessage());
        }

        return json;
    }

    public String ToJSON() {
        return this.ToJSONObject().toString();
    }
}