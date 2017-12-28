using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Cafe {
    public partial class ConnectionParameters : Form {
        private string server;
        private string database;
        private string user;
        private string password;

        public ConnectionParameters() {
            InitializeComponent();

            txtServer.TextChanged += (tss, tse) => server = txtServer.Text;
            txtDatabase.TextChanged += (tds, tde) => database = txtDatabase.Text;
            txtUser.TextChanged += (tus, tue) => user = txtUser.Text;
            txtPassword.TextChanged += (tps, tpe) => password = txtPassword.Text;

            btnBaglanti.Click += btnBaglanti_Click;
            btnKaydet.Click += btnKaydet_Click;
        }

        private void ConnectionParameters_Load(object sender, EventArgs e) {
            var path = Util.AppSettings.getValue(Util.AppSettings.Key.serverFileName);
            if (File.Exists(path)) {
                using (StreamReader reader = new StreamReader(path)) {
                    string line = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line)) {
                        string[] parts = line.Split('|');
                        txtServer.Text = parts[0];
                        txtDatabase.Text = parts[1];
                        txtUser.Text = parts[2];
                        txtPassword.Text = parts[3];
                    }
                }
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e) {
            var filename = Util.AppSettings.getValue(Util.AppSettings.Key.serverFileName);
            using (StreamWriter writer = new StreamWriter(filename)) {
                string content = server + "|" + database + "|" + user + "|" + password;
                writer.WriteLine(content);
                var appname = Util.AppSettings.getValue(Util.AppSettings.Key.appName);
                MessageBox.Show("Parametreler kaydedildi.", appname, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnBaglanti_Click(object sender, EventArgs e) {
            Util.Connection.setParameters(server, database, user, password);
            using (SqlConnection connection = Util.Connection.getConnection()) {
                string message = "Bağlantı açılamadı!";
                MessageBoxIcon icon = MessageBoxIcon.Warning;
                try {
                    connection.Open();
                    if (connection.State == ConnectionState.Open) {
                        icon = MessageBoxIcon.Information;
                        message = "Bağlantı denemesi başarılı.";
                    }
                } catch (Exception ex) {
                    icon = MessageBoxIcon.Error;
                    message = "Hata: " + ex.Message;
                }
                var appname = Util.AppSettings.getValue(Util.AppSettings.Key.appName);
                MessageBox.Show(message, appname, MessageBoxButtons.OK, icon);
            }
        }
    }
}
