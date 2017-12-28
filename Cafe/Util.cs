using System;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Cafe {
    class Util {
        private const string tagUtil = "Util";

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
                messageBoxDuration
            }

            public static string getValue(Key key) {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])key.GetType().GetField(key.ToString()).
                    GetCustomAttributes(typeof(DescriptionAttribute), false);
                string description = attributes[0].Description;
                try {
                    return ConfigurationManager.AppSettings[description];
                } catch (Exception ex) {
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
    }
}