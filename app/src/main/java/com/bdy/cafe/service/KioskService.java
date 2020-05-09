package com.bdy.cafe.service;

import android.app.Service;
import android.content.Intent;
import android.os.IBinder;
import android.support.annotation.Nullable;

/**
 * Created by cngz on 28.12.2016.
 *
 */
public class KioskService extends Service {

    /*private static final long INTERVAL = TimeUnit.SECONDS.toMillis(2);  // periodic interval to check in seconds -> 2 seconds
    private static final String TAG = "KioskService";
    private static final String PREF_KIOSK_MODE = "pref_kiosk_mode";

    private Thread t = null;
    private Context ctx = null;
    private boolean running = false;*/

    @Override
    public void onDestroy() {
        /*Log.i(TAG, "onDestroy: Stopping service 'KioskService'");
        running = false;
        super.onDestroy();*/
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        /*Log.i(TAG, "onStartCommand: Starting service 'KioskService'");
        running = true;
        ctx = this;

        // start a thread that periodically checks if your app is in the foreground
        t = new Thread(new Runnable() {
            @Override
            public void run() {
                do {
                    handleKioskMode();
                    try {
                        Thread.sleep(INTERVAL);
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                        Log.i(TAG, "run: Thread interrupted: 'KioskService'");
                    }
                } while (running);
                stopSelf();
            }
        });

        t.start();
        return Service.START_NOT_STICKY;*/
        return super.onStartCommand(intent, flags, startId);
    }

    private void handleKioskMode() {
        // is Kiosk Mode active?
        /*if (isKioskModeActive(ctx)) {
            // is App in background?
            if (isInBackground()) {
                restoreApp();   // restore
            }
        }*/
    }

    /*private boolean isInBackground() {
        ActivityManager am = (ActivityManager) ctx.getSystemService(Context.ACTIVITY_SERVICE);

        List<ActivityManager.RunningTaskInfo> taskInfo = am.getRunningTasks(1);
        ComponentName componentInfo = taskInfo.get(0).topActivity;
        return (!ctx.getApplicationContext().getPackageName().equals(componentInfo.getPackageName()));
    }*/

    private void restoreApp() {
        // Restart acivity
        /*Intent i = new Intent(ctx, MainActivity.class);
        i.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        ctx.startActivity(i);*/
    }

   /*public boolean isKioskModeActive(final Context context) {
        SharedPreferences sp = PreferenceManager.getDefaultSharedPreferences(context);
        return sp.getBoolean(PREF_KIOSK_MODE, false);
    }*/

    @Nullable
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }
}