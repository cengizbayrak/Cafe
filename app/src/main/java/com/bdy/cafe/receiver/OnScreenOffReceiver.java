package com.bdy.cafe.receiver;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

import com.bdy.cafe.AppContext;

/**
 * Created by cngz on 28.12.2016.
 *
 */
public class OnScreenOffReceiver extends BroadcastReceiver {
//    private static final String PREF_KIOSK_MODE = "pref_kiosk_mode";

    @Override
    public void onReceive(Context context, Intent intent) {
        /*if (Intent.ACTION_SCREEN_OFF.equals(intent.getAction())) {
            AppContext ctx = (AppContext) context.getApplicationContext();
            // is Kiosk Mode active?
            if (isKioskModeActive(ctx)) {
                wakeUpDevice(ctx);
            }
        }*/
    }

    private void wakeUpDevice(AppContext context) {
        /*PowerManager.WakeLock wakeLock = context.getWakeLock(); // get WakeLock reference via AppContext
        if (wakeLock.isHeld()) {
            wakeLock.release();
        }

        // create a new wake lock
        wakeLock.acquire();

        // ... and release again
        wakeLock.release();*/
    }

    /*private boolean isKioskModeActive(final Context context) {
        SharedPreferences sp = PreferenceManager.getDefaultSharedPreferences(context);
        return sp.getBoolean(PREF_KIOSK_MODE, false);
    }*/
}