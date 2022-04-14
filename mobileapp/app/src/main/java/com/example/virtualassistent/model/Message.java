package com.example.virtualassistent.model;

import androidx.room.ColumnInfo;
import androidx.room.Entity;
import androidx.room.PrimaryKey;


@Entity
public class Message {

    public Message(String message, boolean isSender, long createdAt) {
        this.message = message;
        this.isSender = isSender;
        this.createdAt = createdAt;
    }

    @PrimaryKey
    public int uid;

    @ColumnInfo(name = "message")
    public String message;

    @ColumnInfo(name = "is_sender")
    public boolean isSender;

    @ColumnInfo(name = "created_at")
    public long createdAt;
}
