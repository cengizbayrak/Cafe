package com.bdy.cafe;

import android.app.Application;
import android.content.Intent;

import com.bdy.cafe.service.KioskService;

/**
 * Created by cngz on 28.12.2016.
 * <p>
 * App
 */
public class AppContext extends Application {

    /*private AppContext instance;
    private PowerManager.WakeLock wakeLock;
    private OnScreenOffReceiver onScreenOffReceiver;*/

    @Override
    public void onCreate() {
        super.onCreate();
        /*instance = this;
        registerKioskModeScreenOffReceiver();
        startKioskService();    // add this*/
    }

    private void startKioskService() {   // ... and this method
        startService(new Intent(this, KioskService.class));
    }

    // register screen off receiver
    private void registerKioskModeScreenOffReceiver() {
        /*final IntentFilter filter = new IntentFilter(Intent.ACTION_SCREEN_OFF);
        onScreenOffReceiver = new OnScreenOffReceiver();
        registerReceiver(onScreenOffReceiver, filter);*/
    }

    /*public PowerManager.WakeLock getWakeLock() {
        if (wakeLock == null) {
            // lazy loading: first call, create wakeLock via PowerManager
            PowerManager pm = (PowerManager) getSystemService(Context.POWER_SERVICE);
            wakeLock = pm.newWakeLock(PowerManager.FULL_WAKE_LOCK | PowerManager.ACQUIRE_CAUSES_WAKEUP, "wakeup");
        }
        return wakeLock;
    }*/
}
