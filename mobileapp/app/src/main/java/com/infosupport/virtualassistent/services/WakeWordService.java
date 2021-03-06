package com.infosupport.virtualassistent.services;

import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.content.res.Resources;
import android.os.Build;
import android.os.Bundle;
import android.os.IBinder;
import android.os.ResultReceiver;
import android.util.Log;

import androidx.annotation.Nullable;
import androidx.core.app.NotificationCompat;

import com.infosupport.virtualassistent.AssistantActivity;
import com.infosupport.virtualassistent.R;
import com.infosupport.virtualassistent.receivers.SpeechResultReceiver;

import ai.picovoice.porcupine.PorcupineActivationException;
import ai.picovoice.porcupine.PorcupineActivationLimitException;
import ai.picovoice.porcupine.PorcupineActivationRefusedException;
import ai.picovoice.porcupine.PorcupineActivationThrottledException;
import ai.picovoice.porcupine.PorcupineException;
import ai.picovoice.porcupine.PorcupineInvalidArgumentException;
import ai.picovoice.porcupine.PorcupineManager;
import ai.picovoice.porcupine.PorcupineManagerCallback;

public class WakeWordService extends Service {
    private static String CHANNEL_ID;
    private static String ACCESS_KEY;
    private static String KEYWORD_PATH;
    public static final String RESULT_RECEIVER = "com.infosupport.virtualassistent.services.extra.WWE_RESULT_RECEIVER";

    private PorcupineManager porcupineManager;
    private ResultReceiver resultReceiver;
    private final PorcupineManagerCallback porcupineManagerCallback = (keywordIndex) -> {
        resultReceiver.send(SpeechResultReceiver.RESULT_CODE_OK, new Bundle());
    };

    @Override
    public void onCreate() {
        super.onCreate();
        CHANNEL_ID = this.getString(R.string.wakeword_service_channel_id);
        ACCESS_KEY = this.getString(R.string.porcupine_key);
        KEYWORD_PATH = this.getString(R.string.wakeword_keyword_path);
    }

    private void createNotificationChannel() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            NotificationChannel notificationChannel = new NotificationChannel(
                    CHANNEL_ID,
                    "WakeWordEngine",
                    NotificationManager.IMPORTANCE_HIGH);

            NotificationManager manager = getSystemService(NotificationManager.class);
            manager.createNotificationChannel(notificationChannel);
        }
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        resultReceiver = intent.getParcelableExtra(RESULT_RECEIVER);

        createNotificationChannel();

        try {
            porcupineManager = new PorcupineManager.Builder()
                    .setAccessKey(ACCESS_KEY)
                    .setKeywordPath(KEYWORD_PATH)
                    .setSensitivity(0.7f).build(
                            getApplicationContext(),
                            porcupineManagerCallback);
            porcupineManager.start();

        } catch (PorcupineInvalidArgumentException e) {
            onPorcupineInitError(
                    String.format("%s\nEnsure your accessKey '%s' is a valid access key.", e.getMessage(), ACCESS_KEY)
            );
        } catch (PorcupineActivationException e) {
            onPorcupineInitError("AccessKey activation error");
        } catch (PorcupineActivationLimitException e) {
            onPorcupineInitError("AccessKey reached its device limit");
        } catch (PorcupineActivationRefusedException e) {
            onPorcupineInitError("AccessKey refused");
        } catch (PorcupineActivationThrottledException e) {
            onPorcupineInitError("AccessKey has been throttled");
        } catch (PorcupineException e) {
            onPorcupineInitError("Failed to initialize Porcupine " + e.getMessage());
        }

        Notification notification = porcupineManager == null ?
                getNotification("Porcupine init failed", "Service will be shut down") :
                getNotification("Wake word", "Service running");
        startForeground(1234, notification);

        return super.onStartCommand(intent, flags, startId);
    }

    private void onPorcupineInitError(String message) {
        Intent i = new Intent("PorcupineInitError");
        i.putExtra("errorMessage", message);
        sendBroadcast(i);
        LoggingService.Log(message);
    }

    private Notification getNotification(String title, String message) {
        PendingIntent pendingIntent = null;
        if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.S) {
            pendingIntent = PendingIntent.getActivity(
                    this,
                    0,
                    new Intent(this, AssistantActivity.class),
                    PendingIntent.FLAG_MUTABLE);
        }

        return new NotificationCompat.Builder(this, CHANNEL_ID)
                .setContentTitle(title)
                .setContentText(message)
                .setSmallIcon(R.drawable.ic_launcher_foreground)
                .setContentIntent(pendingIntent)
                .build();
    }

    @Nullable
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    @Override
    public void onDestroy() {
        if (porcupineManager != null) {
            try {
                porcupineManager.stop();
                porcupineManager.delete();
            } catch (PorcupineException e) {
                LoggingService.Log(e.getMessage());
                Log.e("PORCUPINE", e.toString());
            }
        }

        super.onDestroy();
    }
}