package com.example.virtualassistent.storage;

import androidx.room.Database;
import androidx.room.RoomDatabase;

import com.example.virtualassistent.model.Message;

@Database(entities = {Message.class}, version = 1)
public abstract class AppDatabase extends RoomDatabase {
    public abstract MessageDAO messageDao();
}
