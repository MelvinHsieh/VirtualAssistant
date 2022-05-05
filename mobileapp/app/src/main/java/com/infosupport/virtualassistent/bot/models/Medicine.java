package com.infosupport.virtualassistent.bot.models;

import android.os.Build;

import org.json.JSONException;
import org.json.JSONObject;
import java.time.LocalTime;

public class Medicine {
    public int id;
    public LocalTime intakeStart;
    public LocalTime intakeEnd;
    public int amount;
    public String name;
    public String indication;
    public int dose;
    public String doseUnit;
    public String shape;
    public String color;
    public String type;
    public String status;

    public Medicine(){}

    public Medicine(JSONObject jsonObject) {
        try {
            this.id = jsonObject.getInt("medicineId");
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
                this.intakeStart = LocalTime.parse(jsonObject.getString("intakeStart"));
                this.intakeEnd = LocalTime.parse(jsonObject.getString("intakeEnd"));
            }
            this.amount = jsonObject.getInt("amount");
            JSONObject med = jsonObject.getJSONObject("medicine");
            this.name = med.getString("name");
            this.indication = med.getString("indication");
            this.dose = med.getInt("dose");
            this.doseUnit = med.getString("doseUnit");
            this.shape = med.getString("shape");
            this.color = med.getString("color");
            this.type = med.getString("type");
            this.status = med.getString("status");
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }


}
