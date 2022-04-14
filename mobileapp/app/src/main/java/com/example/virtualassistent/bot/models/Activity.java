package com.example.virtualassistent.bot.models;

import com.google.gson.Gson;

public class Activity {
    public String locale;
    public String type;
    public From from;
    public String text;

    public String ToJSON() {
        Gson gson = new Gson();
        return gson.toJson(this);
    }
}
