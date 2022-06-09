package com.infosupport.virtualassistent.storage;

import androidx.room.Database;
import androidx.room.RoomDatabase;

import com.infosupport.virtualassistent.model.Message;

@Database(entities = {Message.class}, version = 1, exportSchema = false)
public abstract class AppDatabase extends RoomDatabase {
    public abstract MessageDAO messageDao();
}
