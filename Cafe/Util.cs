﻿using System;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Media;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Cafe {
    class Util {
        private const string tagUtil = "Util";

        public static class Notify {
            public static void notify(Form form, string text) {
                bool on = false;
                string value = AppSettings.getValue(key: AppSettings.Key.notifyIcon);
                bool.TryParse(value, out on);
                if (!on) {
                    return;
                }
                try {
                    NotifyIcon notify = new NotifyIcon() {
                        Icon = new Icon(form.Icon, 40, 40),
                        Visible = true,
                        Text = text,
                        BalloonTipTitle = "Cafe Arjantin",
                        BalloonTipText = text,
                        BalloonTipIcon = ToolTipIcon.Info
                    };
                    notify.ShowBalloonTip(2000);
                } catch (Exception ex) {
                    Logger.log(ex.Message);
                }
            }
        }

        public static class AppSettings {
            public enum Key {
                [Description("appName")]
                appName,
                [Description("serverFileName")]
                serverFileName,
                [Description("server")]
                server,
                [Description("database")]
                database,
                [Description("user")]
                user,
                [Description("password")]
                password,
                [Description("rows")]
                rows,
                [Description("columns")]
                columns,
                [Description("tables")]
                tables,
                [Description("messageBoxDuration")]
                messageBoxDuration,
                [Description("sound")]
                sound,
                [Description("notifyIcon")]
                notifyIcon
            }

            public static string getValue(Key key) {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])key.GetType().GetField(key.ToString()).
                    GetCustomAttributes(typeof(DescriptionAttribute), false);
                string description = attributes[0].Description;
                try {
                    return ConfigurationManager.AppSettings[description];
                } catch (Exception ex) {
                    Logger.log(ex.Message);
                    return "";
                }
            }

            public static void setValue(Key key, string value) {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = config.AppSettings.Settings;
                DescriptionAttribute[] attributes = (DescriptionAttribute[])key.GetType().GetField(key.ToString())
                    .GetCustomAttributes(typeof(DescriptionAttribute), false);
                string description = attributes[0].Description;
                if (settings[description] == null) {
                    settings.Add(description, value);
                } else {
                    settings[description].Value = value;
                }
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
            }
        }

        public static class Connection {
            private const string tagConnection = tagUtil + " - " + "Connection";

            private const string formatConnectionString = "Data Source={0};" +
                "Initial Catalog={1};" +
                "User Id={2};" +
                "Password={3};";
            private static string connectionString;

            private static SqlConnection connection;

            private static string _server;
            private static string _database;
            private static string _user;
            private static string _password;

            public static void setParameters(string server, string database, string user, string password) {
                _server = server;
                _database = database;
                _user = user;
                _password = password;

                constructString();
            }

            private static void constructString() {
                connectionString = string.Format(formatConnectionString, _server, _database, _user, _password);
            }

            public static string getConnectionString() => connectionString;

            public static SqlConnection getConnection() {
                const string tag = tagConnection + " - getConnection";
                if (connection == null || connection.State != System.Data.ConnectionState.Open) {
                    try {
                        connection = new SqlConnection(connectionString);
                    } catch (Exception ex) {
                        Debug.WriteLine(message: tag + " exception: " + ex.Message);
                    }
                }
                return connection;
            }
        }

        public class Animation {

            public void fadeIn(Form form, int duration = 1500, int steps = 100, double currentOpacity = 0.0, double targetOpacity = 1.0, bool show = true, bool dialog = true) {
                form.Opacity = currentOpacity;

                Timer timer = new Timer();
                timer.Interval = duration / steps;

                int currentStep = 0;
                timer.Tick += (arg1, arg2) => {
                    form.Opacity += ((double)currentStep) / steps;
                    currentStep++;

                    if (form.Opacity >= targetOpacity || currentStep >= steps) {
                        timer.Stop();
                        timer.Dispose();
                        if (show && dialog) {
                            form.ShowDialog();
                        } else if (show) {
                            form.Show();
                        }
                    }
                };
                timer.Start();
            }

            public void fadeOut(Form form, int duration = 1500, int steps = 100, double currentOpacity = 1.0, double targetOpacity = 0.0, bool close = true) {
                form.Opacity = currentOpacity;

                Timer timer = new Timer();
                timer.Interval = duration / steps;

                int currentStep = 0;
                timer.Tick += (arg1, arg2) => {
                    form.Opacity -= ((double)currentStep) / steps;
                    currentStep++;

                    if (form.Opacity <= targetOpacity || currentStep >= steps) {
                        timer.Stop();
                        timer.Dispose();
                        if (close) {
                            form.Close();
                        }
                    }
                };
                timer.Start();
            }




        }

        public static class Logger {
            private static string logFile = "log.txt";
            private static string startupPath = Application.StartupPath;

            private static string filePath() => startupPath + "\\" + logFile;

            public static void log(string log) {
                string date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                log = date + ": " + log;
                string enter = Environment.NewLine;
                try {
                    File.AppendAllText(filePath(), log + enter);
                } catch (Exception ex) {
                    Debug.WriteLine(ex.Message);
                }
            }

            public static string read() {
                try {
                    using (StreamReader reader = new StreamReader(filePath())) {
                        return reader.ReadToEnd();
                    }
                } catch (Exception ex) {
                    log(ex.Message);
                }
                return "";
            }
        }

        public static class FirebaseLicense {
            private const string url = "https://cafe-18.firebaseio.com/";
            private const string license_validation_date = "license_validation_date";

            //public static async Task<string> LicenseDateAsync() {
            //    var db = new Firebase.Database.FirebaseClient(url, null);
            //    var child = db.Child(license_validation_date);
            //    var value = await db.Child(license_validation_date).OnceAsync<System.Collections.Generic.Dictionary<string, string>>();
            //    return value.ToString();
            //}

            //public static string LicenseDateAsync() {
            //    try {
            //        var fb = new FirebaseNet.Database.FirebaseDB(url);
            //        var node = fb.Node(license_validation_date);
            //        var response = fb.Get();
            //        var json = JObject.Parse(response.JSONContent);
            //        string value = json.Value<string>(license_validation_date);
            //        return value;
            //    } catch (Exception ex) {
            //        Logger.log(ex.Message);
            //    }
            //    return "";
            //}

            //public static DateTime LicenseValidationDate(string base64Date) {
            //    var binData = Convert.FromBase64String(base64Date);
            //    using (var stream = new MemoryStream(binData)) {
            //        var formatter = new BinaryFormatter();
            //        var bytes = (byte[])formatter.Deserialize(stream);
            //        string key = Encoding.Unicode.GetString(bytes);
            //        var date = DateTime.Parse(key);
            //        return date;
            //    }
            //}
        }

        public static class License {
            private static string licenseFile = "license.dat";
            //private static string licenseKey = "aHR0cHM6Ly9nb28uZ2wvM0RmN3E4";
            private static string licenseKey = DateTime.Today.ToString("yyyyMM") + " " + "https://goo.gl/3Df7q8";

            private static string path() => Application.StartupPath + "\\" + licenseFile;

            public static bool licenseValid() {
                string path = License.path();
                if (!File.Exists(path)) {
                    return false;
                }
                string content = "";
                try {
                    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                        using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read))) {
                            content = reader.ReadToEnd();
                        }
                    }
                } catch (Exception ex) {
                    Logger.log(ex.Message);
                    return true;
                }
                if (string.IsNullOrWhiteSpace(content)) {
                    return false;
                }
                try {
                    var binData = Convert.FromBase64String(content);
                    using (MemoryStream stream = new MemoryStream(binData)) {
                        BinaryFormatter formatter = new BinaryFormatter();
                        byte[] bytes = (byte[])formatter.Deserialize(stream);
                        string key = Encoding.Unicode.GetString(bytes);
                        // 2018-05-01 dat
                        return DateTime.Now <= DateTime.Parse(key);
                        //return !string.IsNullOrWhiteSpace(key) && key.Equals(licenseKey);
                    }
                } catch (Exception ex) {
                    Logger.log(ex.Message);
                    return true;
                }
            }
        }

        public static class Sound {
            private static bool playOn() {
                bool play = false;
                bool.TryParse(AppSettings.getValue(AppSettings.Key.sound), out play);
                return play;
            }

            public static void play(Type type) {
                if (!playOn())
                    return;
                string soundLocation = soundFile(type);
                if (string.IsNullOrWhiteSpace(soundLocation)) {
                    return;
                }
                try {
                    SoundPlayer player = new SoundPlayer(soundLocation);
                    player.Play();
                } catch (Exception ex) {
                    Logger.log(ex.Message);
                }
            }

            private static string soundFile(Type type) {
                string path = Application.StartupPath;
                string file;
                switch (type) {
                    case Type.click:
                        file = "button16.wav";
                        break;
                    case Type.beep:
                        file = "tone_beep.wav";
                        break;
                    default:
                        return "";
                }
                return path + "\\" + file;
            }

            public enum Type {
                click,
                beep
            }
        }

        public static class Mail {
            public static void Send(string msg) {
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Interval = 10 * 60 * 1000;
                timer.Enabled = true;
                timer.Tick += (s, e) => {
                    var fromAddress = new System.Net.Mail.MailAddress("from@gmail.com", "From Name");
                    var toAddress = new System.Net.Mail.MailAddress("to@example.com", "To Name");
                    const string fromPassword = "fromPassword";
                    const string subject = "Subject";
                    const string body = "Body";
                    var smtp = new System.Net.Mail.SmtpClient {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new System.Net.Mail.MailMessage(fromAddress, toAddress) {
                        Subject = subject,
                        Body = body
                    }) {
                        System.Threading.Thread thread = new System.Threading.Thread(delegate () {
                            try {
                                smtp.Send(message);
                            } catch (Exception ex) {
                                Util.Logger.log(ex.Message);
                            }
                        }) {
                            IsBackground = true
                        };
                        thread.Start();
                    }
                };
            }
        }
    }
}