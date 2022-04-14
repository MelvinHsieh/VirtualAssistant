package com.example.virtualassistent.model;

public class Message {

    public Message(String message, boolean isSender, long createdAt) {
        this.message = message;
        this.isSender = isSender;
        this.createdAt = createdAt;
    }

    public String message;
    public boolean isSender;
    public long createdAt;
}
