package com.example.virtualassistent.bot.models;

public class Conversation {
    public String conversationId;
    public String token;
    public int expires_in;
    public String streamUrl;
    public String referenceGrammarId;

    @Override
    public String toString() {
        return "Conversation{" +
                "conversationId='" + conversationId + '\'' +
                ", token='" + token + '\'' +
                ", expires_in=" + expires_in +
                '}';
    }
}
