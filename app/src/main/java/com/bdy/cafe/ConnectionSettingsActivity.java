package com.bdy.cafe;

import android.content.Intent;
import android.content.pm.PackageManager;
import android.hardware.Camera;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.design.widget.TextInputEditText;
import android.support.v7.app.ActionBar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

import com.bdy.cafe.utility.MyUtil;
import com.google.zxing.integration.android.IntentIntegrator;
import com.google.zxing.integration.android.IntentResult;

/**
 * Created by cngz on 30.12.2016.
 * <p>
 * Connection settings
 */
public class ConnectionSettingsActivity extends AppCompatActivity {
    private static final String TAG = ConnectionSettingsActivity.class.getSimpleName();

    private TextInputEditText host;
    private TextInputEditText port;
    private TextInputEditText instance;
    private TextInputEditText database;
    private TextInputEditText username;
    private TextInputEditText password;

    private Button save;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_connection_settings);

        Toolbar toolbar = findViewById(R.id.toolbar_connectionSettings);
        setSupportActionBar(toolbar);
        ActionBar actionBar = getSupportActionBar();
        if (actionBar != null) {
            actionBar.setTitle(R.string.connection_settings);
            actionBar.setDisplayHomeAsUpEnabled(true);
        }

        host = findViewById(R.id.textInputEditText_connectionSettings_host);
        port = findViewById(R.id.textInputEditText_connectionSettings_port);
        instance = findViewById(R.id.textInputEditText_connectionSettings_instance);
        database = findViewById(R.id.textInputEditText_connectionSettings_database);
        username = findViewById(R.id.textInputEditText_connectionSettings_username);
        password = findViewById(R.id.textInputEditText_connectionSettings_password);
        Button qr = findViewById(R.id.button_connectionSettings_qrCode);
        save = findViewById(R.id.button_connectionSettings_save);

        host.setText(MyUtil.UserPrefs.Values.HOST);
        port.setText(MyUtil.UserPrefs.Values.PORT);
        instance.setText(MyUtil.UserPrefs.Values.INSTANCE);
        database.setText(MyUtil.UserPrefs.Values.DATABASE);
        username.setText(MyUtil.UserPrefs.Values.USERNAME);
        password.setText(MyUtil.UserPrefs.Values.PASSWORD);

        qr.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                PackageManager packageManager = getPackageManager();
                if (!packageManager.hasSystemFeature(PackageManager.FEATURE_CAMERA_ANY)) {
                    Toast.makeText(ConnectionSettingsActivity.this, R.string.feature_needs_camera, Toast.LENGTH_SHORT).show();
                    return;
                }

                IntentIntegrator intentIntegrator = new IntentIntegrator(ConnectionSettingsActivity.this);
                intentIntegrator.setDesiredBarcodeFormats(IntentIntegrator.QR_CODE_TYPES);
                intentIntegrator.setPrompt(getString(R.string.bdy));
                // TODO: 30.12.2016
                int camId = 0;
                if (!packageManager.hasSystemFeature(PackageManager.FEATURE_CAMERA)) {
                    if (!packageManager.hasSystemFeature(PackageManager.FEATURE_CAMERA_FRONT)) {
                        camId = 1;
                    }
                }

                int cameraId = -1;
                for (int i = 0; i < Camera.getNumberOfCameras(); i++) {
                    Camera.CameraInfo cameraInfo = new Camera.CameraInfo();
                    Camera.getCameraInfo(i, cameraInfo);
                    if (cameraInfo.facing == Camera.CameraInfo.CAMERA_FACING_BACK) {
                        cameraId = i;
                        break;
                    } else if (cameraInfo.facing == Camera.CameraInfo.CAMERA_FACING_FRONT) {
                        cameraId = i;
                        break;
                    }
                }

                intentIntegrator.setCameraId(cameraId);
                intentIntegrator.setOrientationLocked(false);
                intentIntegrator.setBeepEnabled(false);
                intentIntegrator.setBarcodeImageEnabled(false);
                intentIntegrator.initiateScan();
            }
        });

        save.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (host.length() == 0) {
                    host.requestFocus();
                    host.setError(getString(R.string.can_not_be_empty));
                    return;
                } else if (port.length() == 0) {
                    port.requestFocus();
                    port.setError(getString(R.string.can_not_be_empty));
                    return;
                } else if (instance.length() == 0) {
                    instance.requestFocus();
                    instance.setError(getString(R.string.can_not_be_empty));
                    return;
                } else if (database.length() == 0) {
                    database.requestFocus();
                    database.setError(getString(R.string.can_not_be_empty));
                    return;
                } else if (username.length() == 0) {
                    username.requestFocus();
                    username.setError(getString(R.string.can_not_be_empty));
                    return;
                } else if (password.length() == 0) {
                    password.requestFocus();
                    password.setError(getString(R.string.can_not_be_empty));
                    return;
                }

                // ok
                MyUtil.UserPrefs.setContext(ConnectionSettingsActivity.this);
                if (MyUtil.UserPrefs.setPreference(host.getText().toString(), MyUtil.UserPrefs.Key.host)) {
                    MyUtil.UserPrefs.Values.HOST = host.getText().toString();
                }
                if (MyUtil.UserPrefs.setPreference(port.getText().toString(), MyUtil.UserPrefs.Key.port)) {
                    MyUtil.UserPrefs.Values.PORT = port.getText().toString();
                }
                if (MyUtil.UserPrefs.setPreference(instance.getText().toString(), MyUtil.UserPrefs.Key.instance)) {
                    MyUtil.UserPrefs.Values.INSTANCE = instance.getText().toString();
                }
                if (MyUtil.UserPrefs.setPreference(database.getText().toString(), MyUtil.UserPrefs.Key.database)) {
                    MyUtil.UserPrefs.Values.DATABASE = database.getText().toString();
                }
                if (MyUtil.UserPrefs.setPreference(username.getText().toString(), MyUtil.UserPrefs.Key.username)) {
                    MyUtil.UserPrefs.Values.USERNAME = username.getText().toString();
                }
                if (MyUtil.UserPrefs.setPreference(password.getText().toString(), MyUtil.UserPrefs.Key.password)) {
                    MyUtil.UserPrefs.Values.PASSWORD = password.getText().toString();
                }

                Toast.makeText(ConnectionSettingsActivity.this, R.string.connection_settings_saved, Toast.LENGTH_SHORT).show();
            }
        });
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        IntentResult result = IntentIntegrator.parseActivityResult(requestCode, resultCode, data);
        if (result == null) {
            super.onActivityResult(requestCode, resultCode, data);
            return;
        }
        if (result.getContents() == null) {
            Toast.makeText(ConnectionSettingsActivity.this, R.string.youCancelledTheScanOperation, Toast.LENGTH_LONG).show();
            return;
        }
        String contents = result.getContents();
        if (!contents.startsWith("bdy")) {
            Toast.makeText(ConnectionSettingsActivity.this, R.string.please_read_qr_code_from_application, Toast.LENGTH_LONG).show();
            return;
        }

        String[] props = contents.split("\\|");
        host.setText(props[1]);
        port.setText(props[2]);
        instance.setText(props[3]);
        database.setText(props[4]);
        username.setText(props[5]);
        password.setText(props[6]);
        Toast.makeText(ConnectionSettingsActivity.this, R.string.connection_settings_got, Toast.LENGTH_LONG).show();
        boolean clicked = save.performClick();
        Log.d(TAG, "onActivityResult: save button clicked: " + String.valueOf(clicked));
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case android.R.id.home:
                ConnectionSettingsActivity.this.finish();
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }
}