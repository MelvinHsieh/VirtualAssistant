package com.infosupport.virtualassistent.services;


import android.app.Activity;
import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.content.Context;
import android.content.SharedPreferences;
import android.os.Build;
import android.preference.PreferenceManager;
import android.util.Log;

import androidx.annotation.NonNull;
import androidx.core.app.NotificationManagerCompat;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.google.firebase.installations.FirebaseInstallations;
import com.google.firebase.messaging.FirebaseMessaging;
import com.google.firebase.messaging.FirebaseMessagingService;
import com.google.firebase.messaging.RemoteMessage;
import com.infosupport.virtualassistent.MainActivity;
import com.infosupport.virtualassistent.R;

import java.util.HashMap;
import java.util.Map;
import java.util.Objects;

public class NotificationService extends FirebaseMessagingService {
    private final String URL_DEVICE;

    NotificationService() {
        URL_DEVICE = R.string.dataservice_url + "/api/PatientDevice";
        FirebaseInstallations.getInstance().getId().addOnCompleteListener(
                task -> {
                    if (task.isSuccessful()) {
                        String token = task.getResult();
                        Log.i("token ---->>", token);

                        // store the token in shared preferences
                        PrefUtils.getInstance(getApplicationContext()).setValue(PrefKeys.FCM_TOKEN, token);
                    }
                }
        );
    }

    @Override
    public void onNewToken(@NonNull String token) {
        // Log new token
        /*sendRegistrationToServer(token);*/
        System.out.print("Refreshed token: " + token);
        super.onNewToken(token);
    }

    // either use below function to get the token or directly get from the shared preferences
    public static String getToken(Context context) {
        return PrefUtils.getInstance(context).getStringValue(PrefKeys.FCM_TOKEN, "");
    }

    /** Creates notification channel **/
    private void createNotificationChannel(String title, String text) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            String CHANNEL_ID = "HANS Notification Channel";
            NotificationChannel serviceChannel = new NotificationChannel(
                    CHANNEL_ID,
                    "HANS Notification",
                    NotificationManager.IMPORTANCE_DEFAULT
            );

            NotificationManager manager = getSystemService(NotificationManager.class);
            manager.createNotificationChannel(serviceChannel);
            Notification.Builder notification = new Notification.Builder(this, CHANNEL_ID)
                    .setContentTitle(title)
                    .setContentText(text)
                    .setSmallIcon(R.drawable.ic_launcher_background)
                    .setAutoCancel(false);
            NotificationManagerCompat.from(this).notify(1, notification.build());
        }
    }

    /*private void sendRegistrationToServer() {
        FirebaseMessaging.getInstance().getToken()
                .addOnCompleteListener(task -> {
                    if (task.isSuccessful() && task.getResult() != null) {
                        sendRegistrationToServer(task.getResult());
                    }
                });
    }*/

    /*private void sendRegistrationToServer(String token) {
        String userId = preferences.getString("userId", null);

        if (URL_DEVICE == null || queue == null || userId == null) return;

        StringRequest req = new StringRequest(Request.Method.POST, URL_DEVICE,
                response -> {},
                error -> {}) {
            @Override
            protected Map<String, String> getParams() {
                Map<String, String> params = new HashMap<>();

                params.put("deviceId", token);
                params.put("userId", token);

                return params;
            }
        };
        queue.add(req);
    }*/
}
