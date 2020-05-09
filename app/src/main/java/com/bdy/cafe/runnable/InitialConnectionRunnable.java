package com.bdy.cafe.runnable;

import android.util.Log;

import com.bdy.cafe.model.User;
import com.bdy.cafe.utility.MyUtil;

import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

/**
 * Created by cngz on 27.12.2016.
 * <p>
 * <b>Checks initial connection</b>
 * <ul>
 * <b color="blue">Operations:</b>
 * <li>Get max table number defined</li>
 * <li>Get max addition number defined (TODO)</li>
 * </ul>
 * <ul>
 * <b color="#B40404">Notes:</b>
 * <li>Run in background</li>
 * <li>Get result via {@link InitialConnectionRunnable#getResult()}</li>
 * </ul>
 */
public class InitialConnectionRunnable implements Runnable {
    private static final String TAG = InitialConnectionRunnable.class.getSimpleName();

    private boolean result;

    @Override
    public void run() {
        result = true;

        ConnectionTest connectionTest = new ConnectionTest();
        connectionTest.run();

        if (result = connectionTest.getResult()) {
            if (User.connection == null) {
                try {
                    User.connection = MyUtil.MsSql.getConnection_Default(false);
                    result = true;
                    Log.d(TAG, "run: Connection: " + User.connection.toString());
                } catch (ClassNotFoundException | SQLException e) {
                    result = false;
                    e.printStackTrace();
                    Log.e(TAG, "run: Exception: " + e.getMessage());
                }
            }
            if (result) {
                try {
                    String query = "SELECT MAX(Number) AS MAX_TABLE_NUMBER FROM Tables";
                    Statement statement = User.connection.createStatement();
                    ResultSet resultSet = statement.executeQuery(query);
                    if (resultSet.next()) {
                        User.maxTableNumber = resultSet.getInt("MAX_TABLE_NUMBER");
                        Log.d(TAG, "run: Max table number: " + User.maxTableNumber);
                    }
                } catch (SQLException e) {
                    e.printStackTrace();
                    Log.e(TAG, "run: Exception: " + e.getMessage());

                    // isValid is abstract method
                    /*try {
                        result = !User.connection.isValid(10000);
                    } catch (SQLException e1) {
                        result = false;
                        e.printStackTrace();
                        Log.e(TAG, "run: isValid Exception: " + e1.getMessage());
                    }*/
                }
            }
        }
    }

    /**
     * Get the result of the operation
     *
     * @return true if connection is valid, false otherwise
     */
    public boolean getResult() {
        return result;
    }
}