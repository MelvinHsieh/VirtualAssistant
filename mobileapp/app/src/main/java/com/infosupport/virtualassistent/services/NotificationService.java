package com.infosupport.virtualassistent.services;

import com.infosupport.virtualassistent.R;

import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.content.SharedPreferences;
import android.os.Build;
import android.preference.PreferenceManager;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.core.app.NotificationManagerCompat;

import com.google.firebase.messaging.FirebaseMessagingService;
import com.google.firebase.messaging.RemoteMessage;

import java.util.Objects;

public class NotificationService extends FirebaseMessagingService {
    private static SharedPreferences sp;

    public NotificationService() {
        System.out.print("NotificationService Aangeroepen");
        /*sp = PreferenceManager.getDefaultSharedPreferences(this);*/
    }

    @Override
    public void onNewToken(@NonNull String token) {
        // Log new token
        /*sendRegistrationToServer(token);*/
        System.out.print("Refreshed token: " + token);
        /*SharedPreferences.Editor prefEditor = sp.edit(); //
        prefEditor.putString("deviceToken", token);
        prefEditor.apply();*/
        super.onNewToken(token);
    }

    // Notification messages are only received here in onMessageReceived when the app
    // is in the foreground. When the app is in the background an automatically generated
    // notification is displayed.
    @Override
    public void onMessageReceived(@NonNull RemoteMessage message) {
        System.out.println(message);
        // Also if you intend on generating your own notifications as a result of a received FCM
        // message, here is where that should be initiated. See sendNotification method below.
        String title = Objects.requireNonNull(message.getNotification()).getTitle();
        String text = message.getNotification().getBody();
        createNotificationChannel(title, text);
        super.onMessageReceived(message);
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
}
