package com.infosupport.virtualassistent.bot.websocket;

import com.infosupport.virtualassistent.AssistantActivity;
import com.infosupport.virtualassistent.bot.models.Schedule;
import com.infosupport.virtualassistent.services.LoggingService;

import org.java_websocket.client.WebSocketClient;
import org.java_websocket.handshake.ServerHandshake;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.lang.ref.WeakReference;
import java.net.URI;

public class BotWebSocketClient extends WebSocketClient {
    private final WeakReference<AssistantActivity> activityRef;

    public BotWebSocketClient(URI serverURI, AssistantActivity activity) {
        super(serverURI);
        activityRef = new WeakReference<AssistantActivity>(activity);
    }

    @Override
    public void onOpen(ServerHandshake handshakedata) {
//        System.out.println("WEBSOCKET CONNECTION OPENED!" + handshakedata.getHttpStatusMessage());
    }

    @Override
    public void onMessage(String message) {
        if (message.isEmpty()) return;
        try {
            JSONObject jObject = new JSONObject(message);
            if (!jObject.has("activities") || jObject.isNull("activities")) return;
            JSONArray jArray = jObject.getJSONArray("activities");
            for (int i=0; i < jArray.length(); i++)
            {
                JSONObject msg = jArray.getJSONObject(i);
                // When a serviceUrl is in the json, it means it should not be shown to the user
                if(msg.has("serviceUrl")) continue;
                String type = msg.getString("type");
                if (type.equalsIgnoreCase("message")) {
                    if(msg.has("text")) {
                        String text = msg.getString("text");
                        activityRef.get().runOnUiThread(() -> activityRef.get().showMessage(text, false, false));
                    }
                }
                else if (type.equalsIgnoreCase("image")) {
                    if(msg.has("value")) {
                        String image_url = msg.getString("value");
                        activityRef.get().runOnUiThread(() -> activityRef.get().showMessage(image_url, false, true));
                    }
                }
                else if (type.equalsIgnoreCase("OPEN_SCHEDULE")) {
                    activityRef.get().runOnUiThread(() -> activityRef.get().showMessage(Schedule.fromJSON(msg), false, false));
                }
                if (msg.has("inputHint") && msg.getString("inputHint").equals("expectingInput")) {
                    // This would activate the speech whenever input is expected again.
                    // Right now it causes way too many requests, because nearly every reply - EVEN ERRORS - expect reply
                    // And it detects internal TTS as input as well

                    activityRef.get().runOnUiThread(() -> activityRef.get().turnMicOnAfterCurrentUtterance());
                }
            }
        } catch (JSONException e) {
            LoggingService.Log(e.getMessage());
        }

    }

    @Override
    public void onClose(int code, String reason, boolean remote) {
//        System.out.println("WEBSOCKET CONNECTION CLOSED!" + reason);
    }

    @Override
    public void onError(Exception ex) {
        System.out.println("Error: " + ex.toString());
    }
}
