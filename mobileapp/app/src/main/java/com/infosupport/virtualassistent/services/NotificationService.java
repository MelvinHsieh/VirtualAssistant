package com.infosupport.virtualassistent.services;


import android.app.Activity;
import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
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
import com.google.firebase.messaging.FirebaseMessaging;
import com.google.firebase.messaging.FirebaseMessagingService;
import com.google.firebase.messaging.RemoteMessage;
import com.infosupport.virtualassistent.R;

import java.util.HashMap;
import java.util.Map;
import java.util.Objects;

public class NotificationService extends FirebaseMessagingService {
    /*private final String URL_DEVICE;
    private final RequestQueue queue;
    private final SharedPreferences preferences;*/
    private final String CHANNEL_ID = "HANS Notification Channel";

    /*NotificationService(Activity activity) {
        activity = (activity == null) ? (Activity) this.getApplicationContext() : activity;
        URL_DEVICE = activity.getApplicationContext().getString(R.string.dataservice_url) + "/api/PatientDevice";
        queue = Volley.newRequestQueue(activity);
        preferences = PreferenceManager.getDefaultSharedPreferences(activity.getApplicationContext());
    }*/

    @Override
    public void onNewToken(@NonNull String token) {
        // Log new token
        /*sendRegistrationToServer(token);*/
        Log.d("Token", "Refreshed token: " + token);
        super.onNewToken(token);
    }

    @Override
    public void onMessageReceived(@NonNull RemoteMessage message) {
        // ...

        // TODO(developer): Handle FCM messages here.
        // Not getting messages here? See why this may be: https://goo.gl/39bRNJ
        Log.d("MEssage", "From: " + message.getFrom());

        // Check if message contains a data payload.
        if (message.getData().size() > 0) {
            Log.d("Payload", "Message data payload: " + message.getData());

            /*if (*//* Check if data needs to be processed by long running job *//* true) {
                // For long-running tasks (10 seconds or more) use Firebase Job Dispatcher.
                scheduleJob();
            } else {
                // Handle message within 10 seconds
                handleNow();
            }*/

        }

        // Check if message contains a notification payload.
        if (message.getNotification() != null) {
            Log.d("Body", "Message Notification Body: " + message.getNotification().getBody());
        }

        // Also if you intend on generating your own notifications as a result of a received FCM
        // message, here is where that should be initiated. See sendNotification method below.
        /*String title = Objects.requireNonNull(message.getNotification()).getTitle();
        String text = message.getNotification().getBody();
        createNotificationChannel(title, text);*/
    }

    /** Creates notification channel **/
    private void createNotificationChannel(String title, String text) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
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
