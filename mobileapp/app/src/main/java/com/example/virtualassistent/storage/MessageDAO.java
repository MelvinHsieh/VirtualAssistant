package com.example.virtualassistent.storage;

import androidx.room.Dao;
import androidx.room.Delete;
import androidx.room.Insert;
import androidx.room.Query;

import com.example.virtualassistent.model.Message;

import java.util.List;

@Dao
public interface MessageDAO {
    @Query("SELECT * FROM message")
    List<Message> getAll();

    @Query("SELECT * FROM message WHERE uid IN (:msgIds)")
    List<Message> loadAllByIds(int[] msgIds);

    @Insert
    void insertAll(Message... messages);

    @Delete
    void delete(Message message);
}
