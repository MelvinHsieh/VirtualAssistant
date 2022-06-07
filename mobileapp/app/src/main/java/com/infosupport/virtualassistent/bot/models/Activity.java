package com.infosupport.virtualassistent.bot.models;

import com.google.gson.Gson;

import java.util.Map;

public class Activity {
    public String locale;
    public String type;
    public From from;
    public String text;
    public Map<String, String> properties;

    public String ToJSON() {
        Gson gson = new Gson();
        return gson.toJson(this);
    }
}
