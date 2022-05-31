package com.infosupport.virtualassistent.services;

import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;
import android.util.Base64;
import android.widget.Toast;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.auth0.jwt.JWT;
import com.auth0.jwt.JWTVerifier;
import com.auth0.jwt.algorithms.Algorithm;
import com.auth0.jwt.exceptions.JWTVerificationException;
import com.auth0.jwt.interfaces.DecodedJWT;
import com.infosupport.virtualassistent.MainActivity;
import com.infosupport.virtualassistent.R;
import com.infosupport.virtualassistent.model.LoginAsyncResponse;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;

public class AuthService {

    private final RequestQueue queue;
    private final Activity activity;
    private final SharedPreferences preferences;

    public AuthService(Activity activity) {
        this.activity = activity;
        queue = Volley.newRequestQueue(activity);
        preferences = PreferenceManager.getDefaultSharedPreferences(activity.getApplicationContext());
    }

    public void sendLoginInfo(String username, String password, LoginAsyncResponse responseCallback) {
        String url = activity.getApplicationContext().getString(R.string.login_url);

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
        prefEditor.remove("userId");
        prefEditor.apply();
        Toast.makeText(activity, "Je bent succesvol uitgelogd", Toast.LENGTH_SHORT).show();

        // Start the Main activity again
        Intent intent = new Intent(activity, MainActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        activity.startActivity(intent);
        activity.finish();
    }

    private void Login(boolean correctPassword, String response, LoginAsyncResponse responseCallback) {
        response = response.substring(1, response.length() - 1);
        if(!correctPassword) {
            Toast.makeText(activity, response, Toast.LENGTH_LONG).show();
            responseCallback.processFinished(false);
            return;
        }

        DecodedJWT jwt;
        String secret = activity.getApplicationContext().getString(R.string.auth_secret_key);
        String issuer = activity.getApplicationContext().getString(R.string.auth_issuer);

        try {
            Algorithm algorithm = Algorithm.HMAC256(secret);
            JWTVerifier verifier = JWT.require(algorithm)
                    .withIssuer(issuer)
                    .acceptLeeway(5)
                    .build();
            jwt = verifier.verify(response);
        } catch (JWTVerificationException exception){
            System.out.println(exception.getMessage());
            Toast.makeText(activity, "De secret key is incorrect.", Toast.LENGTH_SHORT).show();
            return;
        }

        // Get the user id from the encoded response
        String jsonString = new String(Base64.decode(jwt.getPayload(), Base64.DEFAULT));
        System.out.println(jsonString);
        String userId = "";
        try {
            JSONObject json = new JSONObject(jsonString);
            JSONObject userJson = json.getJSONObject("user");
            if(userJson.has("patientId")) {
                userId = userJson.getString("patientId");
            } else {
                Toast.makeText(activity, "Alleen patienten kunnen inloggen in de app.", Toast.LENGTH_SHORT).show();
                return;
            }

        } catch (JSONException e) {
            Toast.makeText(activity, "Je accountgegevens konden niet worden opgehaald.", Toast.LENGTH_SHORT).show();
            return;
        }

        Toast.makeText(activity, "De inloggegevens zijn correct!", Toast.LENGTH_SHORT).show();

        // Save the user auth token in the sharedPreferences
        SharedPreferences.Editor prefEditor = preferences.edit();
        prefEditor.putString("authToken", response);
        prefEditor.putString("userId", userId);
        prefEditor.apply();

        responseCallback.processFinished(true);
    }

}
