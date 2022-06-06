package com.infosupport.virtualassistent.services;


import android.app.Activity;
import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.os.Build;

import androidx.annotation.NonNull;
import androidx.core.app.NotificationManagerCompat;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.google.firebase.messaging.FirebaseMessagingService;
import com.google.firebase.messaging.RemoteMessage;
import com.infosupport.virtualassistent.R;

import java.io.BufferedOutputStream;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.HashMap;
import java.util.Map;

public class NotificationService extends FirebaseMessagingService {
    private final RequestQueue queue;
    private final Activity activity;

    public static final String CHANNEL_ID = "HANS Notification Channel";

    NotificationService(Activity activity) {
        this.activity = activity;
        this.queue = Volley.newRequestQueue(activity);
    }

    @Override
    public void onNewToken(@NonNull String token) {
        // Log new token
        sendRegistrationToServer(token);
        super.onNewToken(token);
    }

    @Override
    public void onMessageReceived(@NonNull RemoteMessage message) {
        String title = message.getNotification().getTitle();
        String text = message.getNotification().getBody();
        createNotificationChannel(title, text);
        super.onMessageReceived(message);
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

    private void sendRegistrationToServer(String token) {
        String url = activity.getApplicationContext().getString(R.string.dataservice);

        StringRequest req = new StringRequest(Request.Method.POST, url,
                response -> {},
                error -> {}) {
            @Override
            protected Map<String, String> getParams() {
                Map<String, String> params = new HashMap<String, String>();

                params.put("token", token);

                return params;
            }
        };
        queue.add(req);
    }

}
