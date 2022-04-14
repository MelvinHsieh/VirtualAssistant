package com.example.virtualassistent.bot.websocket;

import org.java_websocket.client.WebSocketClient;
import org.java_websocket.handshake.ServerHandshake;

import java.net.InetSocketAddress;
import java.net.URI;
import java.net.URISyntaxException;
import java.nio.ByteBuffer;
import java.nio.charset.Charset;

public class BotWebSocketClient extends WebSocketClient {
    public BotWebSocketClient(URI serverURI) {
        super(serverURI);
    }
    @Override
    public void onOpen(ServerHandshake handshakedata) {
        System.out.println("WEBSOCKET CONNECTION OPENED!" + handshakedata.getHttpStatusMessage());
    }

    @Override
    public void onMessage(String message) {
        System.out.println("Message:" + message);
    }

    @Override
    public void onMessage(ByteBuffer message) {
        String parsed = new String(message.array(), Charset.defaultCharset());
        System.out.println("Byte buffer:" + parsed);
    }

    @Override
    public void onClose(int code, String reason, boolean remote) {
        System.out.println("WEBSOCKET CONNECTION CLOSED!" + reason);
    }

    @Override
    public void onError(Exception ex) {
        System.out.println(ex.toString());
    }
}
