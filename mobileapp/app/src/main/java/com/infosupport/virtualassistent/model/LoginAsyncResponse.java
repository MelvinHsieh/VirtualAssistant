package com.infosupport.virtualassistent.model;

public interface LoginAsyncResponse {
    void processFinished(boolean correctCredentials);
}