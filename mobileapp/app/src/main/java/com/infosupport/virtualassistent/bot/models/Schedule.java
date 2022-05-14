package com.infosupport.virtualassistent.bot.models;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.time.LocalTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;
import java.util.Objects;

public class Schedule {

    public static String fromJSON(JSONObject msg) {
        StringBuilder value = new StringBuilder("U moet de volgende medicijnen nemen:\n");
        try {
            String medicine = msg.getString("value");
            JSONArray array = new JSONArray(medicine);
            HashMap<LocalTime, ArrayList<Medicine>> hashMap = new HashMap<>();
            int len = ((JSONArray)array).length();
            for (int i=0;i<len;i++){
                Medicine m = new Medicine(((JSONArray)array).getJSONObject(i));
                if (!hashMap.containsKey(m.intakeStart)) {
                    hashMap.put(m.intakeStart, new ArrayList<>());
                }
                Objects.requireNonNull(hashMap.get(m.intakeStart)).add(m);
            }

            for(Map.Entry<LocalTime, ArrayList<Medicine>> entry : hashMap.entrySet()) {
                LocalTime time = entry.getKey();
                ArrayList<Medicine> medicines = entry.getValue();
                value.append("Om ")
                        .append(time)
                        .append(" neemt u: \n");
                for (Medicine med : medicines) {
                    value.append(med.amount)
                            .append(" ")
                            .append(med.color)
                            .append(" ")
                            .append(med.type)
                            .append(" ")
                            .append(med.name)
                            .append(".\n");
                }
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
        return value.toString();
    }
}
