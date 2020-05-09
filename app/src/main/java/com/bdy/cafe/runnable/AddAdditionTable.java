package com.bdy.cafe.runnable;

import android.util.Log;

import com.bdy.cafe.model.User;
import com.bdy.cafe.utility.MyUtil;

import java.sql.SQLException;
import java.sql.Statement;

/**
 * Created by cngz on 27.12.2016.
 * <p>
 * <b>Adds addition & table match infoDialog</b>
 * <ul>
 * <b color="#B40404">Notes:</b>
 * <li>Run in background</li>
 * <li>Get result via {@link AddAdditionTable#getResult()}</li>
 * </ul>
 */
public class AddAdditionTable implements Runnable {
    private static final String TAG = "AddAdditionTable";

    private int additionNumber;
    private int tableNumber;
    private boolean result;

    /**
     * Default constructor
     *
     * @param additionNumber addition number
     * @param tableNumber    table number
     */
    public AddAdditionTable(int additionNumber, int tableNumber) {
        this.additionNumber = additionNumber;
        this.tableNumber = tableNumber;
    }

    @Override
    public void run() {
        ConnectionTest connectionTest = new ConnectionTest();
        connectionTest.run();
        if (result = connectionTest.getResult()) {
            if (User.connection == null) {
                try {
                    User.connection = MyUtil.MsSql.getConnection_Default(false);
                } catch (ClassNotFoundException | SQLException e) {
                    result = false;
                    e.printStackTrace();
                    Log.d(TAG, "run: Exception: " + e.getMessage());
                }
            }

            try {
                String query = "IF NOT EXISTS(SELECT * FROM AdditionTable WHERE AdditionNumber=" + additionNumber + " AND DATEADD(DAY, 0, DATEDIFF(DAY, 0, MatchDate))=DATEADD(DAY, 0, DATEDIFF(DAY, 0, GETDATE()))) " +
                        "BEGIN " +
                        "INSERT INTO AdditionTable(AdditionNumber,TableNumber,MatchDate) VALUES(" + additionNumber + "," + tableNumber + ",GETDATE()) " +
                        "END";
                Statement statement = User.connection.createStatement();
                int insert = statement.executeUpdate(query);
                result = insert > 0;
            } catch (SQLException e) {
                result = false;
                e.printStackTrace();
                Log.d(TAG, "run: Exception: " + e.getMessage());
            }
        }
    }

    /**
     * Get the result of the operation
     *
     * @return true if added, false otherwise
     */
    public boolean getResult() {
        return result;
    }
}