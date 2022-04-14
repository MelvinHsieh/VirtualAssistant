package com.example.virtualassistent.models;

public class Conversation {
    public String conversationId;
    public String token;
    public int expires_in;

    @Override
    public String toString() {
        return "Conversation{" +
                "conversationId='" + conversationId + '\'' +
                ", token='" + token + '\'' +
                ", expires_in=" + expires_in +
                '}';
    }
}
