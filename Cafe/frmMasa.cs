using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Cafe {
    public partial class frmMasa : Form {
        private bool open = true;

        public frmMasa() {
            InitializeComponent();

            setupConnectionSettings();
        }

        private void frmMasa_Load(object sender, EventArgs e) {
            FormClosing += (fcs, fce) => open = false;

            setupLayout();
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

                            }
                        }
                    } catch (Exception ex) {
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

            for (int i = 0; i < rows * columns; i++) {
                var btn = new CustomButton((i + 1).ToString(), (s, e) => {
                    Button b = s as Button;
                    int masa = int.Parse(b.Text);
                    var frm = new frmAdisyon(masa);
                    Opacity = .9;
                    frm.ShowDialog();
                    if (open) {
                        Opacity = 1;
                    }
                });
                table.Controls.Add(btn);
            }
        }
    }
}