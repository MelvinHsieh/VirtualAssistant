package com.infosupport.virtualassistent.services;

import android.os.AsyncTask;

import com.rabbitmq.client.Channel;
import com.rabbitmq.client.Connection;
import com.rabbitmq.client.ConnectionFactory;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.util.concurrent.TimeoutException;

public class LoggingService {

    private static final String EXCHANGE_NAME = "storeErrorLog";
    private static final String QUEUE_NAME = "storeErrorLogQueue";

    public static void Log(String message) {
        ConnectionFactory factory = new ConnectionFactory();
        factory.setUsername("guest");
        factory.setPassword("guest");
        factory.setHost("192.168.2.2");
        factory.setPort(5672);

        AsyncTask.execute(() -> {
            try {
                Connection conn = factory.newConnection();
                Channel channel = conn.createChannel();

                channel.exchangeDeclare(EXCHANGE_NAME, "direct", true);
                channel.queueDeclareNoWait(QUEUE_NAME, true, false, false, null);
                channel.queueBind(QUEUE_NAME, EXCHANGE_NAME, QUEUE_NAME);

                JSONObject json = new JSONObject();
                json.put("service", "mobileApp");
                json.put("error", message);

                byte[] messageBodyBytes = json.toString().getBytes(StandardCharsets.UTF_8);

                channel.basicPublish(EXCHANGE_NAME, QUEUE_NAME, null, messageBodyBytes);

                channel.close();
                conn.close();
            } catch (IOException | TimeoutException | JSONException e) {
                e.printStackTrace();
            }
        });

    }
}
