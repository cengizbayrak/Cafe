package com.bdy.cafe.handler;

import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.os.AsyncTask;

import com.bdy.cafe.R;
import com.bdy.cafe.runnable.InitialConnectionRunnable;
import com.bdy.cafe.utility.MyUtil;

import java.lang.ref.WeakReference;

/**
 * Created by cngz on 22.12.2016.
 * <p>
 * Check SQL connection and get settings on opening of the app
 */
public class InitConnectionHandler extends AsyncTask<Void, Void, Boolean> {
    private static final String TAG = "InitConnectionHandler";

    private WeakReference<Context> weakContext;
    private boolean showProgress;
    private ProgressDialog progressDialog;

    public InitConnectionHandler(Context context, boolean showProgress) {
        this.weakContext = new WeakReference<>(context);
        this.showProgress = showProgress;
    }

    @Override
    protected void onPreExecute() {
        if (progressDialog != null) {
            progressDialog.dismiss();
        }
        if (showProgress) {
            progressDialog = MyUtil.Global.getProgressDialog(weakContext.get(), R.string.server_control, R.string.server_being_controlled, false);
            progressDialog.show();
        }
    }

    @Override
    protected Boolean doInBackground(Void... params) {
        InitialConnectionRunnable connection = new InitialConnectionRunnable();
        connection.run();
        return connection.getResult();
    }

    @Override
    protected void onPostExecute(Boolean aBoolean) {
        if (progressDialog != null) {
            progressDialog.dismiss();
        }
        if (!aBoolean) {
            DialogInterface.OnClickListener positive = new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialogInterface, int i) {
                    InitConnectionHandler handler = new InitConnectionHandler(weakContext.get(), true);
                    handler.execute();
                }
            };
            DialogInterface.OnClickListener negative = new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialogInterface, int i) {
                    dialogInterface.dismiss();
                }
            };
            MyUtil.Global.getAlertDialog(weakContext.get(),
                    R.string.server_control,
                    R.string.server_connection_failed_contact_supervisor,
                    true,
                    MyUtil.Global.infoType.errorDialog,
                    R.string.retry_UPPER,
                    positive,
                    R.string.cancel_UPPER,
                    negative).show();
        }
    }
}