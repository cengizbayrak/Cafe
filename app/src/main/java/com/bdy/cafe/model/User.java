package com.bdy.cafe.model;

import java.sql.Connection;

/**
 * Created by cngz on 22.12.2016.
 * <p>
 * Current user of the app
 */
public final class User {

    // region fields
    /**
     * max table number of the cafe
     */
    public static int maxTableNumber = -1;

    /**
     * current sql connection to DB Server
     */
    public static Connection connection = null;

    /**
     * flag for if customers trying to send infoDialog to db server
     */
    public static boolean executingTransaction = false;

    /*public static SQLStructure sqlStructure;
    public static int lastHandledAdditionNumber = -1;*/
    // endregion

    private User() {

    }
}
