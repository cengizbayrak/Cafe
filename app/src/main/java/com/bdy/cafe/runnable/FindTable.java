package com.bdy.cafe.runnable;

import android.util.Log;

import com.bdy.cafe.model.User;
import com.bdy.cafe.utility.MyUtil;

import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

/**
 * Created by cngz on 10.01.2017.
 * <p>
 * Find table of addition
 */
public class FindTable implements Runnable {
    private static final String TAG = "FindTable";

    private int additionNumber;
    private int tableNumber;

    /**
     * Find table of an addition
     *
     * @param addition addition number
     */
    public FindTable(int addition) {
        this.additionNumber = addition;
    }

    @Override
    public void run() {
        ConnectionTest connectionTest = new ConnectionTest();
        connectionTest.run();
        if (connectionTest.getResult()) {
            if (User.connection == null) {
                try {
                    User.connection = MyUtil.MsSql.getConnection_Default(false);
                } catch (ClassNotFoundException | SQLException e) {
                    e.printStackTrace();
                    Log.d(TAG, "run: Exception: " + e.getMessage());
                }
            }

            if (User.connection == null) {
                tableNumber = -1;
                return;
            }

            try {
                String query = "SElECT TableNumber FROM AdditionTable WHERE AdditionNumber=" + additionNumber +
                        " AND DATEADD(DAY, 0, DATEDIFF(DAY, 0, MatchDate))=DATEADD(DAY, 0, DATEDIFF(DAY, 0, GETDATE()))";
                Statement statement = User.connection.createStatement();
                ResultSet resultSet = statement.executeQuery(query);
                if (resultSet.next()) {
                    tableNumber = resultSet.getInt("TableNumber");
                }
            } catch (SQLException e) {
                tableNumber = -1;
                e.printStackTrace();
                Log.d(TAG, "run: Exception: " + e.getMessage());
            }
        }
    }

    /**
     * Get table number of relevant addition number
     *
     * @return table number if found, -1 otherwise
     */
    public int getTableNumber() {
        return tableNumber;
    }
}