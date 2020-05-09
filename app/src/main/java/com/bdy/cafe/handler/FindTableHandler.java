package com.bdy.cafe.handler;

import android.app.ProgressDialog;
import android.content.Context;
import android.os.AsyncTask;
import android.os.Handler;
import android.support.v7.app.AlertDialog;

import com.bdy.cafe.R;
import com.bdy.cafe.runnable.FindTable;
import com.bdy.cafe.utility.MyUtil;

import java.lang.ref.WeakReference;

/**
 * Created by cngz on 10.01.2017.
 * <p>
 * For finding table number of addition number
 */
public class FindTableHandler extends AsyncTask<Void, Void, Boolean> {
    private static final String TAG = "FindTableHandler";

    private WeakReference<Context> weakContext;
    private int additionNumber;
    private int tableNumber;
    private ProgressDialog progress;

    /**
     * Find table of an addition
     *
     * @param context  app context
     * @param addition addition number
     */
    public FindTableHandler(Context context, int addition) {
        this.weakContext = new WeakReference<>(context);
        this.additionNumber = addition;
    }

    @Override
    protected void onPreExecute() {
        super.onPreExecute();

        progress = MyUtil.Global.getProgressDialog(weakContext.get(), R.string.table_search, R.string.table_searching, false);
        progress.show();
    }

    @Override
    protected Boolean doInBackground(Void... params) {
        FindTable findTable = new FindTable(additionNumber);
        findTable.run();
        tableNumber = findTable.getTableNumber();
        return tableNumber > 0;
    }

    @Override
    protected void onPostExecute(Boolean aBoolean) {
        if (progress != null) {
            progress.dismiss();
        }

        String title = weakContext.get().getString(R.string.table_search);
        String message = weakContext.get().getString(R.string.addition_number_table_number);
        MyUtil.Global.infoType infoType = MyUtil.Global.infoType.infoDialog;
        message = String.format(message, tableNumber, additionNumber);
        if (!aBoolean) {
            message = weakContext.get().getString(R.string.table_number_not_found);
            infoType = MyUtil.Global.infoType.errorDialog;
        } else {
            MyUtil.Global.playSound(weakContext.get(), MyUtil.Global.soundType.success, true);
        }

        final AlertDialog dialog = MyUtil.Global.getAlertDialog(weakContext.get(), title, message, infoType);
        dialog.setCancelable(true);

        int messageDuration = 5000;
        new Handler().postDelayed(new Runnable() {
            @Override
            public void run() {
                dialog.dismiss();
            }
        }, messageDuration);
    }
}