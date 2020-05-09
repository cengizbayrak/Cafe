package com.bdy.cafe.handler;

import android.app.ProgressDialog;
import android.content.Context;
import android.os.AsyncTask;
import android.os.Handler;
import android.support.v7.app.AlertDialog;
import android.util.Log;

import com.bdy.cafe.R;
import com.bdy.cafe.model.User;
import com.bdy.cafe.runnable.AddAdditionTable;
import com.bdy.cafe.runnable.InitialConnectionRunnable;
import com.bdy.cafe.utility.MyUtil;

import java.lang.ref.WeakReference;

/**
 * Created by cngz on 26.12.2016.
 * <p>
 * <b>SQL operations</b>
 * <ul>
 * <b color="blue">Operations:</b>
 * <li>{@link InitialConnectionRunnable}</li>
 * <li>{@link AddAdditionTable}</li>
 * </ul>
 */
public class SqlHandler extends AsyncTask<Void, Void, Boolean> {
    private static final String TAG = "SqlHandler";

    private WeakReference<Context> weakReference;
    private int additionNumber = -1;
    private int tableNumber = -1;
    private ProgressDialog progressDialog;

    public SqlHandler(Context context, int additionNumber, int tableNumber) {
        this.weakReference = new WeakReference<>(context);
        this.additionNumber = additionNumber;
        this.tableNumber = tableNumber;
    }

    @Override
    protected void onPreExecute() {
        super.onPreExecute();

        User.executingTransaction = true;
        progressDialog = MyUtil.Global.getProgressDialog(weakReference.get(), R.string.addition_table_info, R.string.info_transferring, false);
        progressDialog.show();
    }

    @Override
    protected Boolean doInBackground(Void... params) {
        // params wrong
        if (additionNumber == -1 || tableNumber == -1) {
            Log.d(TAG, "doInBackground: additionNumber=" + additionNumber + ", tableNumber=" + tableNumber);
            return false;
        }
        // get max
        InitialConnectionRunnable connection = new InitialConnectionRunnable();
        connection.run();
        // add
        AddAdditionTable addition = new AddAdditionTable(additionNumber, tableNumber);
        addition.run();
        return addition.getResult();
    }

    @Override
    protected void onPostExecute(Boolean aBoolean) {
        if (progressDialog != null) {
            progressDialog.dismiss();
        }
        String title = weakReference.get().getString(R.string.addition_table_info);
        String message = weakReference.get().getString(R.string.addition_table_info_sent_successfully_order_brought_to_you_bon_appetit);
        MyUtil.Global.infoType infoType = MyUtil.Global.infoType.infoDialog;
        message = String.format(message, additionNumber, tableNumber);
        if (!aBoolean) {
            message = weakReference.get().getString(R.string.addition_number_sent_already_please_check_info_try_again);
            message = String.format(message, additionNumber);
            infoType = MyUtil.Global.infoType.errorDialog;
        } else {
            MyUtil.Global.playSound(weakReference.get(), MyUtil.Global.soundType.success, true);
        }
        final AlertDialog dialog = MyUtil.Global.getAlertDialog(weakReference.get(), title, message, infoType);
        dialog.setCancelable(true);
        // show message for duration
        final int duration = 8000;
        new Handler().postDelayed(new Runnable() {
            @Override
            public void run() {
                dialog.dismiss();
            }
        }, duration);
        User.executingTransaction = false;
    }
}