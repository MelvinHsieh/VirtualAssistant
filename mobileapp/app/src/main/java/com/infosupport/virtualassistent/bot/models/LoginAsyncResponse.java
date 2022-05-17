package com.infosupport.virtualassistent.bot.models;

public interface LoginAsyncResponse {
    void processFinished(boolean correctCredentials);
}