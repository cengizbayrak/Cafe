package com.bdy.cafe.receiver;

import android.app.AlarmManager;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.util.Log;

import com.bdy.cafe.model.User;
import com.bdy.cafe.runnable.InitialConnectionRunnable;
import com.bdy.cafe.utility.MyUtil;

import java.sql.SQLException;
import java.util.Calendar;

/**
 * Created by cngz on 28.02.2017.
 * <p>
 * Cihaz boş duruma geçtiğinde
 */
public class CheckConnectionReceiver extends BroadcastReceiver {
    // max 23 chars
    private static final String TAG = "CheckConnectionReceiver";

    @Override
    public void onReceive(Context context, Intent intent) {
        try {
            Calendar calendar = Calendar.getInstance(MyUtil.getLocale());
            Log.d(TAG, "onReceive: Received at " + calendar.getTime().toString());
        } catch (Exception e) {
            e.printStackTrace();
            Log.e(TAG, "onReceive: Exception: " + e.getMessage());
        }

        // already trying to execute a transaction to db server
        if (User.executingTransaction) {
            Log.d(TAG, "onReceive: Already executing transaction.");
            return;
        }

        // connection alive
        try {
            if (User.connection != null && !User.connection.isClosed()) {
                Log.d(TAG, "onReceive: Connection is active and valid.");
                return;
            }
        } catch (SQLException e) {
            e.printStackTrace();
            Log.e(TAG, "onReceive: Exception: " + e.getMessage());
        }

        // revive connection
        try {
            new Thread(new InitialConnectionRunnable()).start();
            Log.d(TAG, "onReceive: InitialConnectionRunnable execution started");
        } catch (Exception e) {
            e.printStackTrace();
            Log.e(TAG, "onReceive: Exception: " + e.getMessage());
        }
    }

    public void setAlarm(Context context) {
        final int alarmInterval = 60 * 1000;  // miliseconds

        try {
            AlarmManager alarmManager = (AlarmManager) context.getSystemService(Context.ALARM_SERVICE);
            Intent intent = new Intent(context, CheckConnectionReceiver.class);
            PendingIntent pendingIntent = PendingIntent.getBroadcast(context, 0, intent, 0);
            alarmManager.setInexactRepeating(AlarmManager.RTC_WAKEUP, System.currentTimeMillis(), alarmInterval, pendingIntent);
            Log.d(TAG, "setAlarm: Alarm setted");
        } catch (Exception e) {
            e.printStackTrace();
            Log.e(TAG, "setAlarm: Exception: " + e.getMessage());
        }
    }

    public void cancelAlarm(Context context) {
        try {
            Intent intent = new Intent(context, CheckConnectionReceiver.class);
            PendingIntent pendingIntent = PendingIntent.getBroadcast(context, 0, intent, 0);
            AlarmManager alarmManager = (AlarmManager) context.getSystemService(Context.ALARM_SERVICE);
            alarmManager.cancel(pendingIntent);
            Log.d(TAG, "cancelAlarm: Alarm cancelled");
        } catch (Exception e) {
            e.printStackTrace();
            Log.e(TAG, "cancelAlarm: Exception: " + e.getMessage());
        }
    }
}