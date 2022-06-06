package com.infosupport.virtualassistent.services;


import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.os.Build;

import androidx.annotation.NonNull;
import androidx.core.app.NotificationManagerCompat;

import com.google.firebase.messaging.FirebaseMessagingService;
import com.google.firebase.messaging.RemoteMessage;
import com.infosupport.virtualassistent.R;

import java.net.HttpURLConnection;
import java.net.URL;

public class NotificationService extends FirebaseMessagingService {
    public static final String CHANNEL_ID = "HANS Notification Channel";

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
        try {
            URL url = new URL("http://exampleurl.com/");
            HttpURLConnection client = null;
            client = (HttpURLConnection) url.openConnection();
            client.setRequestMethod("POST");
            client.setRequestProperty("Key","Value");
            client.setDoOutput(true);
        } catch (Exception e) {

        }
    }
}
