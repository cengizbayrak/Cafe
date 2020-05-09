package com.bdy.cafe.utility;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.content.res.Configuration;
import android.graphics.Typeface;
import android.graphics.drawable.Drawable;
import android.media.MediaPlayer;
import android.os.Build;
import android.os.StrictMode;
import android.preference.PreferenceManager;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.design.widget.Snackbar;
import android.support.graphics.drawable.VectorDrawableCompat;
import android.support.v4.content.ContextCompat;
import android.support.v4.graphics.drawable.DrawableCompat;
import android.support.v4.view.WindowCompat;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatDelegate;
import android.support.v7.widget.AppCompatImageView;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.TextView;

import com.bdy.cafe.R;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.util.Locale;

/**
 * Created by cngz on 21.12.2016.
 * <p>
 * Utils
 */
public class MyUtil {

    // region locale
    private static final Locale mLocale = new Locale("tr", "TR");

    public static Locale getLocale() {
        return mLocale;
    }
    // endregion

    public static class Global {
        private static final String TAG = "Global";

        public static Snackbar getSnackbar(Context context, View view, int message, infoType type) {
            String m = context.getString(message);
            return getSnackbar(context, view, m, type);
        }

        static Snackbar getSnackbar(Context context, View view, String message, infoType type) {
            Snackbar snackbar = Snackbar.make(view, message, Snackbar.LENGTH_LONG);
            View snackbarView = snackbar.getView();
            snackbarView.setPadding(10, 0, 10, 0);
            Drawable drawable = Helper.getDrawable(context, type);
            int white = ContextCompat.getColor(context, R.color.white);
            drawable = Helper.colorizeDrawable(drawable, white);
            int color = Helper.getColor(context, type);
            snackbarView.setBackgroundColor(color);
            TextView snackbarText = snackbarView.findViewById(android.support.design.R.id.snackbar_text);
            snackbarText.setCompoundDrawablesWithIntrinsicBounds(drawable, null, null, null);
            snackbarText.setCompoundDrawablePadding(5);
            snackbarText.setTextColor(white);
            snackbarText.setTextSize(Helper.getTextSize(context));
            snackbarText.setTypeface(Typeface.DEFAULT_BOLD);
            snackbarText.setGravity(Gravity.CENTER_VERTICAL | Gravity.CENTER_HORIZONTAL);
            return snackbar;
        }

        /**
         * <b color="red">This method first show alertdialogs then returns its reference</b>
         * <p>
         * Because of layout width and height configuration
         *
         * @param context         app context
         * @param titleResource   title resource
         * @param messageResource message resource
         * @return customized alertdialog
         */
        public static AlertDialog getAlertDialog(Context context, int titleResource, int messageResource, infoType type) {
            String title = context.getString(titleResource);
            String message = context.getString(messageResource);
            return getAlertDialog(context, title, message, type);
        }

        /**
         * Custom alert dialog
         * <b color="red">This method first show alertdialogs then returns its reference</b>
         * <p>
         * Because of layout width and height configuration
         *
         * @param context app context
         * @param title   title resource
         * @param message message resource
         * @return customized alertdialog
         */
        public static AlertDialog getAlertDialog(Context context, String title, String message, infoType type) {
            Drawable drawable = Helper.getDrawable(context, type);
            int color = Helper.getColor(context, type);
            drawable = Helper.colorizeDrawable(drawable, color);
            AppCompatDelegate.setCompatVectorFromResourcesEnabled(true);
            AlertDialog.Builder builder = new AlertDialog.Builder(context)
                    .setCancelable(false);
            LayoutInflater layoutInflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            View view = layoutInflater.inflate(R.layout.popup_info, null);
            AppCompatImageView icon = view.findViewById(R.id.appCompatImageView_popupInfo_icon);
            TextView txtTitle = view.findViewById(R.id.textView_popupInfo_title);
            TextView txtMessage = view.findViewById(R.id.textView_popupInfo_message);
//            txtTitle.setTextColor(color);
            txtMessage.setText(title);
            txtMessage.setText(message);
            builder.setView(view);
            drawable = DrawableCompat.wrap(drawable);
            DrawableCompat.setTint(drawable, color);
            icon.setImageDrawable(drawable);
            AlertDialog alertDialog = builder.create();
            WindowManager.LayoutParams layoutParams = new WindowManager.LayoutParams();
            layoutParams.copyFrom(alertDialog.getWindow().getAttributes());
            layoutParams.width = WindowManager.LayoutParams.MATCH_PARENT;
            layoutParams.height = WindowManager.LayoutParams.WRAP_CONTENT;
            alertDialog.getWindow().setFlags(WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE, WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE);
            alertDialog.getWindow().getDecorView().setSystemUiVisibility(((Activity) context).getWindow().getDecorView().getSystemUiVisibility());
            alertDialog.show();
            alertDialog.getWindow().setAttributes(layoutParams);
            // not focused
            Window window = alertDialog.getWindow();
            if (window != null) {
                window.setFlags(WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE, WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE);
            }
//            alertDialog.show();
            window = alertDialog.getWindow();
            if (window != null && context instanceof Activity) {
                window.getDecorView().setSystemUiVisibility(((Activity) context).getWindow().getDecorView().getSystemUiVisibility());
            }
            window = alertDialog.getWindow();
            if (window != null) {
                window.clearFlags(WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE);
            }
//            alertDialog.getWindow().clearFlags(WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE);
            alertDialog = Helper.makeDialogNotFocusable(context, alertDialog);
            window = alertDialog.getWindow();
            if (window != null) {
                window.addFlags(WindowManager.LayoutParams.FLAG_DIM_BEHIND);
                window.setDimAmount(0.6f);
            }
//            alertDialog.getWindow().addFlags(WindowManager.LayoutParams.FLAG_BLUR_BEHIND);
//            alertDialog.getWindow().addFlags(WindowManager.LayoutParams.FLAG_DIM_BEHIND);
//            alertDialog.getWindow().setFlags(WindowManager.LayoutParams.FLAG_DIM_BEHIND, 0);
            return alertDialog;
        }

        public static AlertDialog getAlertDialog(Context context,
                                                 int title,
                                                 int message,
                                                 boolean cancelable,
                                                 infoType type,
                                                 int positiveText,
                                                 DialogInterface.OnClickListener positive,
                                                 int negativeText,
                                                 DialogInterface.OnClickListener negative) {
            String t = context.getString(title);
            String m = context.getString(message);
            String pt = context.getString(positiveText);
            String nt = context.getString(negativeText);
            return getAlertDialog(context, t, m, cancelable, type, pt, positive, nt, negative);
        }

        public static AlertDialog getAlertDialog(Context context,
                                                 String title,
                                                 String message,
                                                 boolean cancelable,
                                                 infoType type,
                                                 String positiveText,
                                                 DialogInterface.OnClickListener positive,
                                                 String negativeText,
                                                 DialogInterface.OnClickListener negative) {
            Drawable icon = Helper.getDrawable(context, type);
            int color = Helper.getColor(context, type);
            icon = Helper.colorizeDrawable(icon, color);

            AlertDialog.Builder builder = new AlertDialog.Builder(context)
                    .setTitle(title)
                    .setMessage(message)
                    .setCancelable(cancelable)
                    .setIcon(icon)
                    .setPositiveButton(positiveText, positive)
                    .setNegativeButton(negativeText, negative);
            AlertDialog dialog = builder.create();
            dialog = Helper.makeDialogNotFocusable(context, dialog);
            return dialog;
        }

        public static ProgressDialog getProgressDialog(Context context, int title, int message, boolean cancelable) {
            String t = context.getString(title);
            String m = context.getString(message);
            return getProgressDialog(context, t, m, cancelable);
        }

        public static ProgressDialog getProgressDialog(Context context, String title, String message, boolean cancelable) {
            ProgressDialog dialog = new ProgressDialog(context);
            dialog.setCancelable(cancelable);
            dialog.setTitle(title);
            dialog.setMessage(message);
            dialog = Helper.makeDialogNotFocusable(context, dialog);
            return dialog;
        }

        public static void setUIOptions(Activity activity) {
            try {
                final View decorView = activity.getWindow().getDecorView();
                int uiOptions = View.SYSTEM_UI_FLAG_HIDE_NAVIGATION;
                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN) {
                    uiOptions |= View.SYSTEM_UI_FLAG_LAYOUT_STABLE
                            | View.SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION
                            | View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN
                            | View.SYSTEM_UI_FLAG_FULLSCREEN
                            | WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON;
                }
                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.KITKAT) {
                    uiOptions |= View.SYSTEM_UI_FLAG_IMMERSIVE;
                }
                decorView.setSystemUiVisibility(uiOptions);
            } catch (Exception e) {
                e.printStackTrace();
                Log.e(TAG, "setUIOptions: Exception: " + e.getMessage());
            }
        }

        /**
         * Play sound according to sound type
         *
         * @param context   current context
         * @param type      {@link soundType} sound type
         * @param forcePlay whether to force to play the sound or not
         */
        public static void playSound(final Context context, final soundType type, final boolean forcePlay) {
            Log.d(TAG, "playSound: soundType: " + String.valueOf(type));
            if (!forcePlay) {
                return;
            }
            int raw = 0;
            switch (type) {
                case app:
                    raw = R.raw.button20;
                    break;
                case dialog:
                    raw = R.raw.pop;
                    break;
                case click:
                    raw = R.raw.button16;
                    break;
                case longClick:
                    raw = R.raw.button4;
                    break;
                case backClick:
                    raw = R.raw.button2;
                    break;
                case success:
                    raw = R.raw.tone_beep;
                    break;
            }
            final MediaPlayer player = MediaPlayer.create(context, raw);
            player.setOnCompletionListener(new MediaPlayer.OnCompletionListener() {
                @Override
                public void onCompletion(MediaPlayer mediaPlayer) {
                    mediaPlayer.release();
                }
            });
            player.setOnPreparedListener(new MediaPlayer.OnPreparedListener() {
                @Override
                public void onPrepared(MediaPlayer mediaPlayer) {
                    mediaPlayer.start();
                }
            });
        }

        /**
         * Sound types
         */
        public enum soundType {
            app,
            dialog,
            click,
            longClick,
            backClick,
            success
        }

        /**
         * Message type
         * <ul>
         * <li>{@link infoType#infoDialog} - infoDialog & success messages</li>
         * <li>{@link infoType#errorDialog} - errorDialog & fail messages</li>
         * </ul>
         */
        public enum infoType {
            infoDialog,
            errorDialog,
            infoSnackbar,
            errorSnackbar
        }

        private static final class Helper {
            @Nullable
            private static Drawable getDrawable(Context context, infoType type) {
                AppCompatDelegate.setCompatVectorFromResourcesEnabled(true);
                int resource = 0;
                switch (type) {
                    case infoDialog:
                    case infoSnackbar:
                        resource = R.drawable.ic_check_circle_black_24dp;
                        break;
                    case errorDialog:
                    case errorSnackbar:
                        resource = R.drawable.ic_error_black_24dp;
                        break;
                }
                return VectorDrawableCompat.create(context.getResources(), resource, null);
            }

            private static int getColor(Context context, infoType type) {
                int resource = 0;
                switch (type) {
                    case infoDialog:
                        resource = R.color.colorPrimary;
                        break;
                    case errorDialog:
                        resource = R.color.colorError;
                        break;
                    case infoSnackbar:
                        resource = R.color.gray_light;
                        break;
                    case errorSnackbar:
                        resource = R.color.colorError_light;
                        break;
                }
                return ContextCompat.getColor(context, resource);
            }

            private static Drawable colorizeDrawable(Drawable drawable, int color) {
                drawable = DrawableCompat.wrap(drawable);
                DrawableCompat.setTint(drawable, color);
                return drawable;
            }

            /**
             * Get text size according to screen size for snackbar
             *
             * @param context app context
             * @return text size
             */
            private static float getTextSize(Context context) {
                int screenLayout = context.getResources().getConfiguration().screenLayout;
                screenLayout &= Configuration.SCREENLAYOUT_SIZE_MASK;

                float textSize;
                switch (screenLayout) {
                    case Configuration.SCREENLAYOUT_SIZE_SMALL:
                        textSize = context.getResources().getDimension(R.dimen.textSize_popupTitle);
                        textSize = 12;
                        break;
                    case Configuration.SCREENLAYOUT_SIZE_NORMAL:
                        textSize = context.getResources().getDimension(R.dimen.textSize_popupTitle);
                        textSize = 14;
                        break;
                    case Configuration.SCREENLAYOUT_SIZE_LARGE:
                        textSize = context.getResources().getDimension(R.dimen.textSize_popupTitle_sw600);
                        textSize = 24;
                        break;
                    case Configuration.SCREENLAYOUT_SIZE_XLARGE:
                        textSize = context.getResources().getDimension(R.dimen.textSize_popupTitle_sw720);
                        textSize = 36;
                        break;
                    default:
                        textSize = context.getResources().getDimension(R.dimen.textSize_popupTitle);
                        textSize = 10;
                }
                return textSize;
            }

            private static ProgressDialog makeDialogNotFocusable(@NonNull Context context, @NonNull ProgressDialog progressDialog) {
                Window window = progressDialog.getWindow();
                if (window != null) {
                    // animation
                    window.getAttributes().windowAnimations = R.style.dialog_animation_bottom_bottom;
                }
                window = progressDialog.getWindow();
                if (window != null) {
                    // here's the magic
                    // set the dialog to not focusable (makes navigation ignore us adding the window)
                    window.setFlags(WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE, WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE);
                }
                progressDialog.show();
                window = progressDialog.getWindow();
                if (window != null && context instanceof Activity) {
                    window.getDecorView().setSystemUiVisibility(((Activity) context).getWindow().getDecorView().getSystemUiVisibility());
                }
                window = progressDialog.getWindow();
                if (window != null) {
                    window.clearFlags(WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE);
                }
                return progressDialog;
            }

            private static AlertDialog makeDialogNotFocusable(@NonNull Context context, @NonNull AlertDialog alertDialog) {
                Window window = alertDialog.getWindow();
                if (window != null) {
                    // animation
                    window.getAttributes().windowAnimations = R.style.dialog_animation_bottom_bottom;
                }
                window = alertDialog.getWindow();
                if (window != null) {
                    // here's the magic
                    // set the dialog to not focusable (makes navigation ignore us adding the window)
                    window.setFlags(WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE, WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE);
                }
                alertDialog.show();
                window = alertDialog.getWindow();
                if (window != null && context instanceof Activity) {
                    window.getDecorView().setSystemUiVisibility(((Activity) context).getWindow().getDecorView().getSystemUiVisibility());
                }
                window = alertDialog.getWindow();
                if (window != null) {
                    window.clearFlags(WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE);
                }
                return alertDialog;
            }
        }
    }

    public static final class UserPrefs {
        private final class Keys {
            private static final String HOST = "HOST";
            private static final String PORT = "PORT";
            private static final String INSTANCE = "INSTANCE";
            private static final String DATABASE = "DATABASE";
            private static final String USERNAME = "USERNAME";
            private static final String PASSWORD = "PASSWORD";
        }

        public static final class Values {
            public static String HOST = "192.168.1.106";
            public static String PORT = "1433";
            public static String INSTANCE = "NEUROOGLE";
            public static String DATABASE = "BDY_CAFE";
            public static String USERNAME = "SA";
            public static String PASSWORD = "1453";
        }

        private static SharedPreferences preferences = null;

        private UserPrefs() {
        }

        /**
         * Set application context into
         *
         * @param context app context
         */
        public static void setContext(Context context) {
            UserPrefs.preferences = PreferenceManager.getDefaultSharedPreferences(context);
        }

        /**
         * <p color="red"><b>First call {@link UserPrefs#setContext(Context)}</b></p>
         * Check if SharedPreferences has key
         *
         * @param key {@link Key}
         * @return true if exists, false otherwise
         */
        public static boolean hasPreference(Key key) {
            String keyName;
            switch (key) {
                case host:
                    keyName = Keys.HOST;
                    break;
                case port:
                    keyName = Keys.PORT;
                    break;
                case instance:
                    keyName = Keys.INSTANCE;
                    break;
                case database:
                    keyName = Keys.DATABASE;
                    break;
                case username:
                    keyName = Keys.USERNAME;
                    break;
                case password:
                    keyName = Keys.PASSWORD;
                    break;
                default:
                    return false;
            }
            return UserPrefs.preferences.contains(keyName);
        }

        /**
         * Get value of a specific user preference key
         *
         * @param key user preference key {@link Key}
         * @return object containing value if ok, null otherwise
         */
        public static String getPreference(Key key) {
            String value = null;
            switch (key) {
                case host:
                    value = UserPrefs.preferences.getString(Keys.HOST, null);
                    break;
                case port:
                    value = UserPrefs.preferences.getString(Keys.PORT, null);
                    break;
                case instance:
                    value = UserPrefs.preferences.getString(Keys.INSTANCE, null);
                    break;
                case database:
                    value = UserPrefs.preferences.getString(Keys.DATABASE, null);
                    break;
                case username:
                    value = UserPrefs.preferences.getString(Keys.USERNAME, null);
                    break;
                case password:
                    value = UserPrefs.preferences.getString(Keys.PASSWORD, null);
                    break;
            }
            return value;
        }

        /**
         * Set value for a specific user preference key {@link Key}
         *
         * @param value value to be set
         * @param key   key of user preference
         * @return true if ok, false otherwise
         */
        public static boolean setPreference(String value, Key key) {
            String keyName;
            switch (key) {
                case host:
                    keyName = Keys.HOST;
                    break;
                case port:
                    keyName = Keys.PORT;
                    break;
                case instance:
                    keyName = Keys.INSTANCE;
                    break;
                case database:
                    keyName = Keys.DATABASE;
                    break;
                case username:
                    keyName = Keys.USERNAME;
                    break;
                case password:
                    keyName = Keys.PASSWORD;
                    break;
                default:
                    return false;
            }
            return preferences.edit().putString(keyName, value).commit();
        }

        public enum Key {
            host,
            port,
            instance,
            database,
            username,
            password
        }
    }

    /**
     * MS SQL connection utils
     */
    public static final class MsSql {
        private static final String TAG = "MsSql";

        // sql connection string constants
        private static final String DRIVER = "net.sourceforge.jtds.jdbc.Driver";
        private static final String PREFIX = "jdbc:jtds:sqlserver://";
        private static final String PREFIX_DATABASE = "databaseName";
        private static final String PREFIX_INSTANCE = "INSTANCE";
        private static final String PREFIX_ENCRYPT = "encrypt";

        private MsSql() {

        }

        public static final class Constants {
            public static final String DRIVER = MsSql.DRIVER;
        }

        // connection string example
        // "jdbc:jtds:sqlserver://192.168.0.11:1433;databaseName=Neuroogle_AHBYS;encrypt=false;INSTANCE=EXPRESS2014;"

        /**
         * Get connection string
         *
         * @param host     ip
         * @param port     port
         * @param instance instance
         * @param database database
         * @return connection string if ok, null otherwise
         */
        public static String getConnectionString(@NonNull String host, @NonNull String port, @NonNull String instance, @NonNull String database) {
            if (host.isEmpty() || port.isEmpty() || instance.isEmpty() || database.isEmpty()) {
                return null;
            }

            return PREFIX + host + ":" + port + ";" +
                    PREFIX_DATABASE + "=" + database + ";" +
                    PREFIX_INSTANCE + "=" + instance + ";" +
                    PREFIX_ENCRYPT + "=false;";
        }

        /**
         * Get connection string with encrypt, send encrypt as "false"
         *
         * @param host     ip
         * @param port     port
         * @param instance instance
         * @param database database
         * @param encrypt  false default, send true
         * @return connection string if ok, null otherwise
         */
        public static String getConnectionString(@NonNull String host, @NonNull String port, @NonNull String instance, @NonNull String database, @NonNull String encrypt) {
            if (host.isEmpty() || port.isEmpty() || instance.isEmpty() || database.isEmpty() || encrypt.isEmpty()) {
                return null;
            }

            return PREFIX + host + ":" + port + ";" +
                    PREFIX_DATABASE + "=" + database + ";" +
                    PREFIX_INSTANCE + "=" + instance + ";" +
                    PREFIX_ENCRYPT + "=" + encrypt + ";";
        }

        /**
         * Get connection string
         *
         * @return connection string with default values
         */
        private static String getConnectionString_WithDefaultValues() {

            return PREFIX + UserPrefs.Values.HOST + ":" + UserPrefs.Values.PORT + ";" +
                    PREFIX_DATABASE + "=" + UserPrefs.Values.DATABASE + ";" +
                    PREFIX_INSTANCE + "=" + UserPrefs.Values.INSTANCE + ";" +
                    PREFIX_ENCRYPT + "=false;";
        }

        /**
         * Get connection
         *
         * @param connectionString connection string without username & password
         * @param username         username
         * @param password         password
         * @param allowOnUIThread  allow on ui
         * @return connection
         * @throws ClassNotFoundException driver class not found
         * @throws SQLException           sql exception
         */
        public static Connection getConnection(String connectionString, String username, String password, boolean allowOnUIThread) throws ClassNotFoundException, SQLException {
            if (allowOnUIThread) {
                StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
                StrictMode.setThreadPolicy(policy);
            }
            Class.forName(DRIVER);
            return DriverManager.getConnection(connectionString, username, password);
        }

        public static Connection getConnection_Default(boolean allowOnUIThread) throws ClassNotFoundException, SQLException {
            if (allowOnUIThread) {
                StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
                StrictMode.setThreadPolicy(policy);
            }

            Class.forName(DRIVER);
            return DriverManager.getConnection(getConnectionString_WithDefaultValues(), UserPrefs.Values.USERNAME, UserPrefs.Values.PASSWORD);
        }
    }
}