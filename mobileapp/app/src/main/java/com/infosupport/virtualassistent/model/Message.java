package com.infosupport.virtualassistent.model;

import androidx.room.ColumnInfo;
import androidx.room.Entity;
import androidx.room.PrimaryKey;


@Entity(tableName = "message")
public class Message {

    public Message(String message, boolean isUser, boolean isImage, long createdAt) {
        this.message = message;
        this.isUser = isUser;
        this.isImage = isImage;
        this.createdAt = createdAt;
    }

    @PrimaryKey(autoGenerate = true)
    public int uid;

    @ColumnInfo(name = "message")
    public String message;

    @ColumnInfo(name = "is_image")
    public boolean isImage;

    @ColumnInfo(name = "is_user")
    public boolean isUser;

    @ColumnInfo(name = "created_at")
    public long createdAt;
}
