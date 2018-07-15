using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Cafe {
    public partial class frmMasa : Form {
        private bool open = true;

        public frmMasa() {
            InitializeComponent();

            setupConnectionSettings();

            Opacity = 0.0;

            Util.Animation animation = new Util.Animation();
            Load += (s, e) => {
                bool licenseValid = Util.License.licenseValid();
                if (!licenseValid) {
                    var app = Util.AppSettings.getValue(Util.AppSettings.Key.appName);
                    string message = "Kullanmış olduğunuz yazılımın lisans bilgisi bulunamadı!";
                    Util.Logger.log(message);
                    MessageBox.Show(message, app, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    try {
                        NotifyIcon icon = new NotifyIcon() {
                            Icon = Icon,
                            BalloonTipIcon = ToolTipIcon.Warning,
                            BalloonTipTitle = app,
                            Text = "Lisanssız yazılım kullanmak yasalara aykırıdır!",
                            Visible = true,
                            BalloonTipText = "Lisanssız yazılım kullanmak yasalara aykırıdır!"
                        };
                        icon.ShowBalloonTip(2000);
                    } catch (Exception ex) {
                        Util.Logger.log(ex.Message);
                    }
                    Close();
                    Application.Exit();
                    return;
                }
                setupLayout();
                animation.fadeIn(this, 1500, 100, Opacity, 1.0, false, false);
            };
            FormClosing += (s, e) => {
                open = false;
                animation.fadeOut(this, 1500, 100, Opacity, 0.0, false);
            };
        }

        private void setupConnectionSettings() {
            var appName = Util.AppSettings.getValue(Util.AppSettings.Key.appName);

            var _server = Util.AppSettings.getValue(Util.AppSettings.Key.server);
            var _database = Util.AppSettings.getValue(Util.AppSettings.Key.database);
            var _user = Util.AppSettings.getValue(Util.AppSettings.Key.user);
            var _password = Util.AppSettings.getValue(Util.AppSettings.Key.password);

            var path = Util.AppSettings.getValue(Util.AppSettings.Key.serverFileName);
            bool exist = File.Exists(path);
            if (exist) {
                try {
                    using (StreamReader reader = new StreamReader(path)) {
                        String line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line)) {
                            var frm = new ConnectionParameters();
                            frm.ShowDialog();
                            return;
                        }
                        String[] conParams = line.Split('|');
                        var server = conParams[0];
                        var database = conParams[1];
                        var user = conParams[2];
                        var password = conParams[3];
                        if (!_server.Equals(server)) {
                            Util.AppSettings.setValue(Util.AppSettings.Key.server, server);
                        }
                        if (!_database.Equals(database)) {
                            Util.AppSettings.setValue(Util.AppSettings.Key.database, database);
                        }
                        if (!_user.Equals(user)) {
                            Util.AppSettings.setValue(Util.AppSettings.Key.user, user);
                        }
                        if (!_password.Equals(password)) {
                            Util.AppSettings.setValue(Util.AppSettings.Key.password, password);
                        }
                    }
                    Util.Connection.setParameters(_server, _database, _user, _password);
                    try {
                        using (SqlConnection connection = Util.Connection.getConnection()) {
                            connection.Open();
                            if (connection.State != System.Data.ConnectionState.Open) {
                                MessageBox.Show("Veri tabanı bağlantısı sağlanamadı! Lütfen ayarları kontrol edin!", appName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    } catch (Exception ex) {
                        Util.Logger.log(ex.Message);
                    }
                } catch (Exception) {
                }
            } else {
                var dialog = MessageBox.Show("Bağlantı ayarları için gerekli dosya bulunamadı! Varsayılan ayarlarla devam etmek istiyor musunuz?", appName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog != DialogResult.Yes) {
                    // todo giriş ayarları
                    var frm = new ConnectionParameters();
                    frm.ShowDialog();
                }
            }
        }

        private void setupLayout() {
            var rows = int.Parse(Util.AppSettings.getValue(Util.AppSettings.Key.rows));
            var columns = int.Parse(Util.AppSettings.getValue(Util.AppSettings.Key.columns));
            var tables = int.Parse(Util.AppSettings.getValue(Util.AppSettings.Key.tables));

            table.RowCount = rows;
            table.ColumnCount = columns;

            table.RowStyles.Clear();
            table.ColumnStyles.Clear();

            for (int i = 0; i < columns; i++) {
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / columns));
            }

            for (int i = 0; i < rows; i++) {
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / rows));
            }

            var workingArea = Screen.PrimaryScreen.WorkingArea;
            //var workingArea = Screen.AllScreens[1].WorkingArea;

            var cells = rows * columns;
            var size = (workingArea.Width * workingArea.Height) / (cells);
            var dimen = Math.Sqrt(size);
            int side = (int)Math.Ceiling(dimen);
            side -= 1;

            for (int i = 0; i < rows * columns; i++) {
                if (i >= tables) {
                    break;
                }
                var btn = new CustomButton((i + 1).ToString(),
                    (s, e) => {
                        Util.Sound.play(Util.Sound.Type.click);

                        Button b = s as Button;
                        int masa = int.Parse(b.Text);
                        var frm = new frmAdisyon(masa);
                        Util.Animation animation = new Util.Animation();
                        animation.fadeOut(this, 2500, 100, 1.0, 0.55, false);
                        frm.ShowDialog();
                        if (open) {
                            animation.fadeIn(this, 2500, 100, Opacity, 1.0, false, false);
                        }
                    });
                btn.Width = side;
                btn.Height = side;
                table.Controls.Add(btn);
            }
        }
    }
}