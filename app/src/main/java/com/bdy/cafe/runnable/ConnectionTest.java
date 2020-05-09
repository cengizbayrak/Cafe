package com.bdy.cafe.runnable;

import android.util.Log;

import com.bdy.cafe.utility.MyUtil;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.Socket;

/**
 * Created by cngz on 30.12.2016.
 * <p>
 * Test the SQL server connection via socket during time {@link ConnectionTest#timeout}
 * <p>
 * Get result via {@link ConnectionTest#getResult()}
 */
class ConnectionTest implements Runnable {
    private static final String TAG = "ConnectionTest";

    private boolean result = false;
    private int timeout = 10000;

    @Override
    public void run() {
        try {
            Socket socket = new Socket();
            socket.connect(new InetSocketAddress(MyUtil.UserPrefs.Values.HOST, Integer.parseInt(MyUtil.UserPrefs.Values.PORT)), timeout);
            if (result = socket.isConnected()) {
                socket.close();
            }
        } catch (IOException e) {
            result = false;
            e.printStackTrace();
            Log.e(TAG, "run: Exception: " + e.getMessage());
        }
    }

    /**
     * Get operation result
     *
     * @return true if connection established, false otherwise
     */
    boolean getResult() {
        return result;
    }
}