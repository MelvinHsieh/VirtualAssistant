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

    private static final String EXCHANGE_NAME = "storeError";
    private static final String QUEUE_NAME = "storeErrorQueue";

    public static void Log(String message) {
        ConnectionFactory factory = new ConnectionFactory();
        factory.setUsername("vaadmin");
        factory.setPassword("MessageBus12345!@");
        factory.setHost("va-messagebus.westeurope.cloudapp.azure.com");
        factory.setPort(5672);

        AsyncTask.execute(() -> {
            try {
                Connection conn = factory.newConnection();
                Channel channel = conn.createChannel();

                channel.exchangeDeclare(EXCHANGE_NAME, "direct", true);
                channel.queueDeclareNoWait(QUEUE_NAME, true, false, false, null);
                channel.queueBind(QUEUE_NAME, EXCHANGE_NAME, QUEUE_NAME);

                JSONObject json = new JSONObject();
                json.put("ServiceName", "mobileApp");
                json.put("ErrorMessage", message);

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
