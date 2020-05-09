package com.bdy.cafe;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.res.Configuration;
import android.graphics.drawable.ColorDrawable;
import android.graphics.drawable.Drawable;
import android.os.Build;
import android.os.PowerManager;
import android.support.v4.content.ContextCompat;
import android.support.v4.graphics.drawable.DrawableCompat;
import android.support.v7.app.ActionBar;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.app.AppCompatDelegate;
import android.support.v7.widget.AppCompatImageView;
import android.support.v7.widget.CardView;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.KeyEvent;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import com.bdy.cafe.handler.FindTableHandler;
import com.bdy.cafe.handler.InitConnectionHandler;
import com.bdy.cafe.handler.SqlHandler;
import com.bdy.cafe.model.User;
import com.bdy.cafe.receiver.CheckConnectionReceiver;
import com.bdy.cafe.utility.MyUtil;

import java.util.Timer;
import java.util.TimerTask;

/**
 * Main
 */
public class MainActivity extends AppCompatActivity {
    private static final String TAG = "MainActivity";

    private static boolean startedAlready = false;

    // region operations
    private int OPERATION_TYPE = 0;
    // operation types
    private static final int OPERATION_ADDITION_NUMBER = 1;
    private static final int OPERATION_TABLE_NUMBER = 2;
    private static final int OPERATION_SEARCH_TABLE = 3;
    // operation codes
    private static final int CODE_APP_EXIST = 1453;
    private static final int CODE_CONNECTION_SETTINGS = 1454;
    private static final int CODE_RETRY_CONNECTION = 1455;
    private static final int CODE_TABLE_SEARCH = 9999;
    // endregion

    private int number_addition;
    private int number_table;

    private TextView input_info;
    private TextView input;

    private TextView next_ok_text;

    private MenuItem goBackToAddition;

    private View.OnClickListener cardClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {
            MainActivity.this.onClick(v);
        }
    };

    // region handle device idle
    private IntentFilter filter = new IntentFilter() {
        {
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
                this.addAction(PowerManager.ACTION_DEVICE_IDLE_MODE_CHANGED);
            }
        }
    };
    private BroadcastReceiver idleModeReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            Log.d(TAG, "onReceive: Received");
            PowerManager powerManager = (PowerManager) context.getSystemService(Context.POWER_SERVICE);
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
                if (powerManager != null) {
                    onDeviceIdleChanged(powerManager.isDeviceIdleMode());
                    Log.d(TAG, "onReceive: idleModeReceiver - isDeviceIdleMode: " + String.valueOf(powerManager.isDeviceIdleMode()));
                }
            }
        }
    };
    // endregion

    private Timer timerConnection = new Timer();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

// kiosk
//        getWindow().addFlags(WindowManager.LayoutParams.FLAG_DISMISS_KEYGUARD);

        final View decorView = this.getWindow().getDecorView();
        decorView.setOnSystemUiVisibilityChangeListener(new View.OnSystemUiVisibilityChangeListener() {
            @Override
            public void onSystemUiVisibilityChange(int visibility) {
                setUIOptions();
            }
        });

        setContentView(R.layout.activity_main);

        // marşmelovsa başlat
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            registerReceiver(idleModeReceiver, filter);
            Log.d(TAG, "onCreate: CheckConnectionReceiver registered");
        }

        onDeviceIdleChanged(true);

        setCheckConnection(connectionOp.activate);

        // region connection parameters
        MyUtil.UserPrefs.setContext(MainActivity.this);
        String host = MyUtil.UserPrefs.getPreference(MyUtil.UserPrefs.Key.host);
        if (host != null) {
            MyUtil.UserPrefs.Values.HOST = host;
        }
        String port = MyUtil.UserPrefs.getPreference(MyUtil.UserPrefs.Key.port);
        if (port != null) {
            MyUtil.UserPrefs.Values.PORT = port;
        }
        String instance = MyUtil.UserPrefs.getPreference(MyUtil.UserPrefs.Key.instance);
        if (instance != null) {
            MyUtil.UserPrefs.Values.INSTANCE = instance;
        }
        String database = MyUtil.UserPrefs.getPreference(MyUtil.UserPrefs.Key.database);
        if (database != null) {
            MyUtil.UserPrefs.Values.DATABASE = database;
        }
        String username = MyUtil.UserPrefs.getPreference(MyUtil.UserPrefs.Key.username);
        if (username != null) {
            MyUtil.UserPrefs.Values.USERNAME = username;
        }
        String password = MyUtil.UserPrefs.getPreference(MyUtil.UserPrefs.Key.password);
        if (password != null) {
            MyUtil.UserPrefs.Values.PASSWORD = password;
        }
        // endregion

        if (!startedAlready) {
            InitConnectionHandler initConnectionHandler = new InitConnectionHandler(MainActivity.this, true);
            initConnectionHandler.execute();
        }

        OPERATION_TYPE = OPERATION_ADDITION_NUMBER;

        Toolbar toolbar = findViewById(R.id.toolbar_mainActivity);
        setSupportActionBar(toolbar);
        ActionBar actionBar = getSupportActionBar();
        if (actionBar != null) {
            actionBar.setTitle(R.string.arjantin_cafe);
            actionBar.setBackgroundDrawable(new ColorDrawable(ContextCompat.getColor(MainActivity.this, R.color.colorPrimary)));
            actionBar.setLogo(R.mipmap.ic_launcher);
        }

        input_info = findViewById(R.id.textView_mainActivity_inputInfo);
        input = findViewById(R.id.textView_mainActivity_input);

        CardView one = findViewById(R.id.cardView_mainActivity_one);
        CardView two = findViewById(R.id.cardView_mainActivity_two);
        CardView three = findViewById(R.id.cardView_mainActivity_three);
        CardView four = findViewById(R.id.cardView_mainActivity_four);
        CardView five = findViewById(R.id.cardView_mainActivity_five);
        CardView six = findViewById(R.id.cardView_mainActivity_six);
        CardView seven = findViewById(R.id.cardView_mainActivity_seven);
        CardView eight = findViewById(R.id.cardView_mainActivity_eight);
        CardView nine = findViewById(R.id.cardView_mainActivity_nine);
        CardView zero = findViewById(R.id.cardView_mainActivity_zero);
        CardView backspace = findViewById(R.id.cardView_mainActivity_backspace);
        CardView next_ok = findViewById(R.id.cardView_mainActivity_nextAndOk);

        next_ok_text = findViewById(R.id.textView_mainActivity_nextAndOk);

        AppCompatImageView oneImage = (AppCompatImageView) one.getChildAt(0);
        AppCompatImageView twoImage = (AppCompatImageView) two.getChildAt(0);
        AppCompatImageView threeImage = (AppCompatImageView) three.getChildAt(0);
        AppCompatImageView fourImage = (AppCompatImageView) four.getChildAt(0);
        AppCompatImageView fiveImage = (AppCompatImageView) five.getChildAt(0);
        AppCompatImageView sixImage = (AppCompatImageView) six.getChildAt(0);
        AppCompatImageView sevenImage = (AppCompatImageView) seven.getChildAt(0);
        AppCompatImageView eightImage = (AppCompatImageView) eight.getChildAt(0);
        AppCompatImageView nineImage = (AppCompatImageView) nine.getChildAt(0);
        AppCompatImageView zeroImage = (AppCompatImageView) zero.getChildAt(0);
        AppCompatImageView backspaceImage = (AppCompatImageView) backspace.getChildAt(0);

        oneImage.setFocusable(false);
        twoImage.setFocusable(false);
        threeImage.setFocusable(false);
        fourImage.setFocusable(false);
        fiveImage.setFocusable(false);
        sixImage.setFocusable(false);
        sevenImage.setFocusable(false);
        eightImage.setFocusable(false);
        nineImage.setFocusable(false);
        zeroImage.setFocusable(false);
        backspaceImage.setFocusable(false);

        input_info.setFocusable(false);
        input.setFocusable(false);
        next_ok_text.setFocusable(false);

        one.setOnClickListener(cardClickListener);
        two.setOnClickListener(cardClickListener);
        three.setOnClickListener(cardClickListener);
        four.setOnClickListener(cardClickListener);
        five.setOnClickListener(cardClickListener);
        six.setOnClickListener(cardClickListener);
        seven.setOnClickListener(cardClickListener);
        eight.setOnClickListener(cardClickListener);
        nine.setOnClickListener(cardClickListener);
        zero.setOnClickListener(cardClickListener);
        backspace.setOnClickListener(cardClickListener);
        next_ok.setOnClickListener(cardClickListener);

        backspace.setOnLongClickListener(new View.OnLongClickListener() {
            @Override
            public boolean onLongClick(View view) {
                input.setText("");
                Toast.makeText(MainActivity.this, R.string.all_input_cleared, Toast.LENGTH_SHORT).show();
                return true;
            }
        });

        if (goBackToAddition != null) {
            goBackToAddition.setVisible(OPERATION_TYPE != OPERATION_ADDITION_NUMBER);
        }

        startedAlready = true;
    }

    @Override
    protected void onResume() {
        super.onResume();
    }

    @Override
    protected void onPause() {
        super.onPause();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_main, menu);

        AppCompatDelegate.setCompatVectorFromResourcesEnabled(true);
        for (int i = 0; i < menu.size(); i++) {
            Drawable drawable = menu.getItem(i).getIcon();
            if (drawable != null) {
                drawable = DrawableCompat.wrap(drawable);
                DrawableCompat.setTint(drawable, ContextCompat.getColor(MainActivity.this, R.color.white));
            }
        }

        goBackToAddition = menu.getItem(0);
        if (goBackToAddition != null) {
            goBackToAddition.setVisible(OPERATION_TYPE != OPERATION_ADDITION_NUMBER);
        }
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case R.id.menuItem_main_goBack:
                if (OPERATION_TYPE == OPERATION_ADDITION_NUMBER) {
                    return true;
                }

                number_addition = 0;
                number_table = 0;
                switchMode(enumOperation.addition);

                item.setVisible(false);
                break;
            /*case R.id.menuItem_main_about:
                // TODO: 11.01.2017 implement about here
                break;*/
        }
        return true;
//        return super.onOptionsItemSelected(item);
    }

    @Override
    protected void onSaveInstanceState(Bundle outState) {
        String info = input_info.getText().toString();
        String number = input.getText().toString();
        outState.putString("infoDialog", info);
        outState.putString("number", number);

        super.onSaveInstanceState(outState);
    }

    @Override
    protected void onRestoreInstanceState(Bundle savedInstanceState) {
        boolean portrait = getResources().getConfiguration().orientation == Configuration.ORIENTATION_PORTRAIT;
        int infoResource = 0;
        switch (OPERATION_TYPE) {
            case OPERATION_ADDITION_NUMBER:
                infoResource = portrait ? R.string.your_addition_no : R.string.your_addition_number;
                break;
            case OPERATION_TABLE_NUMBER:
                infoResource = portrait ? R.string.your_table_no : R.string.your_table_number;
                break;
            case OPERATION_SEARCH_TABLE:
                infoResource = R.string.table_search;
                break;
        }
        input_info.setText(infoResource);
        String number = savedInstanceState.getString("number");
        if (number != null) {
            input.setText(number);
        }

        super.onRestoreInstanceState(savedInstanceState);
    }

    @Override
    public void onBackPressed() {
//        super.onBackPressed();
    }

    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        return keyCode == KeyEvent.KEYCODE_HOME ||
                keyCode == KeyEvent.KEYCODE_APP_SWITCH ||
                super.onKeyDown(keyCode, event);
    }

    @Override
    public void onWindowFocusChanged(boolean hasFocus) {
        super.onWindowFocusChanged(hasFocus);
//        if (!hasFocus) {
        // close every kind of system dialog
            /*Intent closeDialog = new Intent(Intent.ACTION_CLOSE_SYSTEM_DIALOGS);
            sendBroadcast(closeDialog);*/
//        }

        if (hasFocus) {
            setUIOptions();
        }
        MyUtil.Global.setUIOptions(MainActivity.this);
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();

        try {
            setCheckConnection(connectionOp.deactivate);
        } catch (Exception e) {
            e.printStackTrace();
            Log.e(TAG, "onDestroy: Exception: " + e.getMessage());
        }

        try {
            unregisterReceiver(idleModeReceiver);
        } catch (Exception e) {
            e.printStackTrace();
            Log.e(TAG, "onDestroy: Exception: " + e.getMessage());
        }
    }

    public void onClick(View view) {
        int viewId = view.getId();

        if (input.length() == 4) {
            int value = Integer.parseInt(input.getText().toString());
            if (value == CODE_APP_EXIST) {
                if (viewId != R.id.cardView_mainActivity_nextAndOk && viewId != R.id.cardView_mainActivity_backspace) {
                    return;
                }
                if (viewId == R.id.cardView_mainActivity_nextAndOk) {
                    MainActivity.this.finish();
                    return;
                }
            } else if (value == CODE_CONNECTION_SETTINGS) {
                if (viewId != R.id.cardView_mainActivity_nextAndOk && viewId != R.id.cardView_mainActivity_backspace) {
                    return;
                }

                if (viewId == R.id.cardView_mainActivity_nextAndOk) {
                    Intent intent = new Intent(MainActivity.this, ConnectionSettingsActivity.class);
                    startActivity(intent);
                    return;
                }
            } else if (value == CODE_RETRY_CONNECTION) {
                if (viewId != R.id.cardView_mainActivity_nextAndOk && viewId != R.id.cardView_mainActivity_backspace) {
                    return;
                }

                if (viewId == R.id.cardView_mainActivity_nextAndOk) {
                    InitConnectionHandler initConnectionHandler = new InitConnectionHandler(MainActivity.this, true);
                    initConnectionHandler.execute();
                    return;
                }
            } else if (value == CODE_TABLE_SEARCH) {
                if (viewId != R.id.cardView_mainActivity_nextAndOk && viewId != R.id.cardView_mainActivity_backspace) {
                    return;
                }

                if (viewId == R.id.cardView_mainActivity_nextAndOk) {
                    switchMode(enumOperation.searchTable);
                    return;
                }
            } else {
                if (viewId != R.id.cardView_mainActivity_backspace && viewId != R.id.cardView_mainActivity_nextAndOk) {
                    return;
                }
            }
        }

        switch (viewId) {
            case R.id.cardView_mainActivity_one:
                input.append("1");
                break;
            case R.id.cardView_mainActivity_two:
                input.append("2");
                break;
            case R.id.cardView_mainActivity_three:
                input.append("3");
                break;
            case R.id.cardView_mainActivity_four:
                input.append("4");
                break;
            case R.id.cardView_mainActivity_five:
                input.append("5");
                break;
            case R.id.cardView_mainActivity_six:
                input.append("6");
                break;
            case R.id.cardView_mainActivity_seven:
                input.append("7");
                break;
            case R.id.cardView_mainActivity_eight:
                input.append("8");
                break;
            case R.id.cardView_mainActivity_nine:
                input.append("9");
                break;
            case R.id.cardView_mainActivity_zero:
                if (input.length() != 0) {
                    input.append("0");
                }
                break;
            case R.id.cardView_mainActivity_backspace:
                if (input.length() > 0) {
                    input.setText(input.getText().subSequence(0, input.length() - 1));
                }
                break;
            case R.id.cardView_mainActivity_nextAndOk:
                if (input.length() == 0) {
                    int messageResource = 0;
                    switch (OPERATION_TYPE) {
                        case OPERATION_ADDITION_NUMBER:
                            messageResource = R.string.please_enter_addition_number_UPPER;
                            break;
                        case OPERATION_TABLE_NUMBER:
                            messageResource = R.string.please_enter_table_number_UPPER;
                            break;
                    }
                    MyUtil.Global.getSnackbar(MainActivity.this, input, messageResource, MyUtil.Global.infoType.errorSnackbar).show();
                    return;
                } else {
                    switch (OPERATION_TYPE) {
                        case OPERATION_ADDITION_NUMBER:
                            number_addition = Integer.parseInt(input.getText().toString());
                            switchMode(enumOperation.table);
                            break;
                        case OPERATION_TABLE_NUMBER:
                            number_table = Integer.parseInt(input.getText().toString());
                            if (number_table > User.maxTableNumber) {
                                MyUtil.Global.getSnackbar(MainActivity.this, input, R.string.please_enter_valid_table_number_UPPER, MyUtil.Global.infoType.errorSnackbar).show();
                                return;
                            }
                            switchMode(enumOperation.addition);
                            SqlHandler handler = new SqlHandler(MainActivity.this, number_addition, number_table);
                            handler.execute();
                            break;
                        case OPERATION_SEARCH_TABLE:
                            int additionNo = Integer.parseInt(input.getText().toString());
                            switchMode(enumOperation.addition);
                            FindTableHandler findTableHandler = new FindTableHandler(MainActivity.this, additionNo);
                            findTableHandler.execute();
                            break;
                    }
                }
                break;
        }
    }

    // region modes
    private enum enumOperation {
        addition("addition"),
        table("table"),
        searchTable("searchTable");

        private final String _value;

        enumOperation(String value) {
            this._value = value;
        }

        private String getValue() {
            return _value;
        }
    }

    private void switchMode(enumOperation operation) {
        Log.d(TAG, "switchMode: enumOperation switched to: " + operation.getValue());
        boolean portrait = getResources().getConfiguration().orientation == Configuration.ORIENTATION_PORTRAIT;
        String orientation = String.format("screen orientation: %s", portrait ? "portrait" : "landscape");
        Log.d(TAG, "switchMode: " + orientation);
        switch (operation) {
            case addition:
                input_info.setText(portrait ? R.string.your_addition_no : R.string.your_addition_number);
                input.setText("");
                next_ok_text.setText(R.string.next);
                OPERATION_TYPE = OPERATION_ADDITION_NUMBER;
                if (goBackToAddition != null) {
                    goBackToAddition.setVisible(false);
                }
                break;
            case table:
                input_info.setText(portrait ? R.string.your_table_no : R.string.your_table_number);
                input.setText("");
                next_ok_text.setText(R.string.send);
                OPERATION_TYPE = OPERATION_TABLE_NUMBER;
                if (goBackToAddition != null) {
                    goBackToAddition.setVisible(true);
                }
                break;
            case searchTable:
                input_info.setText(R.string.table_search);
                input.setText("");
                next_ok_text.setText(R.string.find);
                OPERATION_TYPE = OPERATION_SEARCH_TABLE;
                if (goBackToAddition != null) {
                    goBackToAddition.setVisible(true);
                }
                break;
        }
    }
    // endregion

    private void setUIOptions() {
        final View decorView = MainActivity.this.getWindow().getDecorView();

        /*final View decorView = this.getWindow().getDecorView();
        int uiOptions = View.SYSTEM_UI_FLAG_HIDE_NAVIGATION;
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN) {
            uiOptions |= View.SYSTEM_UI_FLAG_LAYOUT_STABLE
                    | View.SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION
                    *//*| View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN*//*
                    | View.SYSTEM_UI_FLAG_FULLSCREEN;
        }
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.KITKAT) {
        uiOptions |= View.SYSTEM_UI_FLAG_IMMERSIVE_STICKY;
        }*/

        int uiOptions = View.SYSTEM_UI_FLAG_HIDE_NAVIGATION;
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN) {
            uiOptions |= View.SYSTEM_UI_FLAG_LAYOUT_STABLE
                    | View.SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION
                    | View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN
            /*|View.SYSTEM_UI_FLAG_FULLSCREEN*/;
        }
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.KITKAT) {
            uiOptions |= View.SYSTEM_UI_FLAG_IMMERSIVE;
        }
        decorView.setSystemUiVisibility(uiOptions);
    }

    /**
     * Aygıt idle moda girip çıktığında tetiklenecek
     *
     * @param isIdle idle mod mu
     */
    private void onDeviceIdleChanged(boolean isIdle) {
        if (isIdle) {
            setCheckConnection(connectionOp.activate);
        } else {
            setCheckConnection(connectionOp.deactivate);
        }
    }

    /**
     * Activate & deactivate connection
     *
     * @param op {@link connectionOp}
     */
    private void setCheckConnection(connectionOp op) {
        CheckConnectionReceiver checkConnectionReceiver = new CheckConnectionReceiver();
        switch (op) {
            case activate:
                checkConnectionReceiver.setAlarm(getApplicationContext());
                break;
            case deactivate:
                checkConnectionReceiver.cancelAlarm(getApplicationContext());
                break;
        }
    }

    private enum connectionOp {
        activate,
        deactivate
    }

    private void rescheduleTimers() {
        if (timerConnection != null) {
            timerConnection.cancel();
            timerConnection.purge();
        }
        timerConnection = new Timer();
        final int delay = 15 * 1000;
        final int interval = 60 * 1000;
        TimerTask task = new TimerTask() {
            @Override
            public void run() {
                final boolean transaction = User.executingTransaction;
                Log.d(TAG, "run: in transacton: " + String.valueOf(transaction));
                if (transaction) {
                    return;
                }
                InitConnectionHandler handler = new InitConnectionHandler(MainActivity.this, false);
                handler.execute();
            }
        };
        timerConnection.scheduleAtFixedRate(task, delay, interval);
    }

    private void clearTimers() {
        if (timerConnection != null) {
            timerConnection.cancel();
            timerConnection.purge();
        }
    }
}