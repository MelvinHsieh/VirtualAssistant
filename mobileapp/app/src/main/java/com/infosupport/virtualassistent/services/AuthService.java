package com.infosupport.virtualassistent.services;

import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;
import android.widget.Toast;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.google.gson.Gson;
import com.infosupport.virtualassistent.MainActivity;
import com.infosupport.virtualassistent.bot.models.LoginAsyncResponse;

import java.util.HashMap;
import java.util.Map;

public class AuthService {

    private final RequestQueue queue;
    private final Gson gson;
    private final Activity activity;
    private final SharedPreferences preferences;

    public AuthService(Activity activity) {
        this.activity = activity;
        queue = Volley.newRequestQueue(activity);
        gson = new Gson();
        preferences = PreferenceManager.getDefaultSharedPreferences(activity.getApplicationContext());
    }

    public void sendLoginInfo(String username, String password, LoginAsyncResponse responseCallback) {
        String url = "http://192.168.2.2:3001/login";

        StringRequest req = new StringRequest(Request.Method.POST, url,
                response -> Login(true, response, responseCallback),
                error -> Login(false, "De gebruikersnaam of het wachtwoord is incorrect.", responseCallback)) {
            @Override
            protected Map<String, String> getParams() {
                Map<String, String> params = new HashMap<String, String>();

                params.put("username", username);
                params.put("password", password);

                return params;
            }
        };
        queue.add(req);
    }

    public void logOut() {
        SharedPreferences.Editor prefEditor = preferences.edit();
        prefEditor.remove("authToken");
        prefEditor.apply();
        Toast.makeText(activity, "Je bent succesvol uitgelogd", Toast.LENGTH_SHORT).show();

        // Start the Main activity again
        Intent intent = new Intent(activity, MainActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        activity.startActivity(intent);
        activity.finish();
    }

    private void Login(boolean correctPassword, String response, LoginAsyncResponse responseCallback) {
        if(!correctPassword) {
            Toast.makeText(activity, response, Toast.LENGTH_LONG).show();
            responseCallback.processFinished(false);
            return;
        }
        Toast.makeText(activity, "De inloggegevens zijn correct!", Toast.LENGTH_SHORT).show();

        // Save the user auth token in the sharedPreferences
        SharedPreferences.Editor prefEditor = preferences.edit();
        prefEditor.putString("authToken", response);
        prefEditor.apply();

        responseCallback.processFinished(true);
    }

}
