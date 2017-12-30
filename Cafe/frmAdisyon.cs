using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cafe {
    public partial class frmAdisyon : Form {
        /// <summary>
        /// string.format ile çağır, adisyon numarasını parametre olarak gönder
        /// </summary>
        private const string adisyonIslenmis = "{0} numaralı adisyon işleme alınmış görünüyor! Lütfen adisyon numaranızı kontrol edin!";
        /// <summary>
        /// string.format ile çağır, masa ve adisyon numarasını parametre olarak gönder
        /// </summary>
        private const string bulunanMasa = "{0} masa - {1} adisyon";

        private enum enOperation {
            addition,
            table
        }

        private enOperation operation = enOperation.addition;

        private int masa;
        private int adisyon;

        private enum buttonValue {
            sifir = 0,
            bir = 1,
            iki = 2,
            uc = 3,
            dort = 4,
            bes = 5,
            alti = 6,
            yedi = 7,
            sekiz = 8,
            dokuz = 9,
            sil = -1
        }

        private frmAdisyon() {
            InitializeComponent();

            var color = BackColor;
            TransparencyKey = color;
            BackColor = color;
            //Opacity = .5;

            //var w = pnl.Width;
            //var h = pnl.Height;
            //var csize = ClientSize;
            //var cw = csize.Width;
            //var ch = csize.Height;
            //pnl.Location = new Point(cw / 2 - w / 2, ch / 2 - h / 2);

            pnl.Anchor = AnchorStyles.None;

            btnSifir.Tag = buttonValue.sifir;
            btnBir.Tag = buttonValue.bir;
            btnIki.Tag = buttonValue.iki;
            btnUc.Tag = buttonValue.uc;
            btnDort.Tag = buttonValue.dort;
            btnBes.Tag = buttonValue.bes;
            btnAlti.Tag = buttonValue.alti;
            btnYedi.Tag = buttonValue.yedi;
            btnSekiz.Tag = buttonValue.sekiz;
            btnDokuz.Tag = buttonValue.dokuz;
            btnSil.Tag = buttonValue.sil;

            Opacity = 0.0;

            Load += (s, e) => {
                StartPosition = FormStartPosition.CenterParent;
                fadeIn();
            };
            FormClosing += (s, e) => {
                fadeOut(0.0);
            };

            txtAdisyon.KeyPress += (tas, tae) => {
                tae.Handled = !char.IsDigit(tae.KeyChar) && !char.IsControl(tae.KeyChar);
            };
            txtAdisyon.TextChanged += (tas, tae) => {
                bool parsed = int.TryParse(txtAdisyon.Text, out int _adisyon);
                Debug.WriteLine("parsed: " + parsed.ToString());
                if (parsed) {
                    adisyon = _adisyon;
                }
                Debug.WriteLine("adisyon: " + adisyon.ToString());
            };
            txtAdisyon.GotFocus += (tas, tae) => btnTamam.Focus();
            btnVazgec.Click += (bvs, bve) => {
                Util.Sound.playClick(); Close();
            };
            btnTamam.Click += btnTamam_Click;

            btnSifir.Click += btnNumber_Click;
            btnBir.Click += btnNumber_Click;
            btnIki.Click += btnNumber_Click;
            btnUc.Click += btnNumber_Click;
            btnDort.Click += btnNumber_Click;
            btnBes.Click += btnNumber_Click;
            btnAlti.Click += btnNumber_Click;
            btnYedi.Click += btnNumber_Click;
            btnSekiz.Click += btnNumber_Click;
            btnDokuz.Click += btnNumber_Click;
            btnSil.Click += btnNumber_Click;

            var tip = new ToolTip();
            tip.SetToolTip(btnSil, "Sil");
        }

        public frmAdisyon(int masa) : this() {
            this.masa = masa;
        }

        private void btnNumber_Click(object sender, EventArgs e) {
            Util.Sound.playClick();

            Button btn = sender as Button;
            buttonValue val = (buttonValue)btn.Tag;
            if (val == buttonValue.sifir && txtAdisyon.TextLength == 0) {
                return;
            }
            if (txtAdisyon.TextLength > 3 && val != buttonValue.sil) {
                return;
            }
            if (val != buttonValue.sil) {
                int v = (int)val;
                txtAdisyon.AppendText(v.ToString());
            } else {
                if (txtAdisyon.TextLength != 0) {
                    txtAdisyon.Text = txtAdisyon.Text.Substring(0, txtAdisyon.TextLength - 1);
                }
            }
        }

        private void btnTamam_Click(object sender, EventArgs e) {
            Util.Sound.playClick();

            if (txtAdisyon.TextLength == 0) {
                btnTamam.Focus();
                Close();
                return;
            }
            if (adisyon == 1453) {
                txtAdisyon.Clear();
                Application.Exit();
                return;
            } else if (adisyon == 1454) {
                txtAdisyon.Clear();
                var frm = new ConnectionParameters();
                frm.ShowDialog();
                return;
            } else if (adisyon == 1455) {
                txtAdisyon.Clear();
                var frm = new frmAppConfig();
                frm.ShowDialog();
                return;
            } else if (adisyon == 1456) {
                Process.Start(Application.ExecutablePath);
                Application.Exit();
                return;
            } else if (adisyon == 1469) {
                txtAdisyon.Clear();
                string log = Util.Logger.read();
                var txt = new RichTextBox();
                txt.Text = log;
                txt.ReadOnly = true;
                txt.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
                txt.Dock = DockStyle.Fill;
                if (!string.IsNullOrWhiteSpace(txt.Text)) {
                    txt.Select(txt.Text.Length - 1, 0);
                }
                var frm = new Form();
                frm.Text = "Log";
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                frm.Controls.Add(txt);
                frm.KeyPreview = true;
                frm.KeyPress += (fs, fe) => {
                    if (fe.KeyChar == (char)Keys.Escape) {
                        frm.Close();
                    }
                };
                frm.ShowDialog();
                return;
            } else if (adisyon == 9999) {
                if (operation == enOperation.addition) {
                    operation = enOperation.table;
                    txtAdisyon.Clear();
                    return;
                }
            }
            if (operation == enOperation.table) {
                txtAdisyon.Clear();
                SqlConnection conn = Util.Connection.getConnection();
                if (conn != null) {
                    using (conn) {
                        try {
                            string sql = "SElECT TableNumber FROM AdditionTable WHERE AdditionNumber = " + adisyon +
                                    " AND DATEADD(DAY, 0, DATEDIFF(DAY, 0, MatchDate))=DATEADD(DAY, 0, DATEDIFF(DAY, 0, GETDATE()))";
                            conn.Open();
                            SqlCommand command = new SqlCommand(sql, conn);
                            SqlDataReader reader = command.ExecuteReader();
                            if (!reader.HasRows) {
                                string msg = "Masa bilgisi bulunamadı! Doğru adisyon numarası girdiğinize emin olun!";
                                fadeOut(0.55);
                                new MessageBoxForm(msg).ShowDialog();
                            } else if (reader.Read()) {
                                int table = (int)reader.GetSqlInt32(0);
                                string msg = string.Format(bulunanMasa, table, adisyon);
                                fadeOut(0.55);
                                new MessageBoxForm(msg).ShowDialog();
                                fadeIn();
                            }
                        } catch (TableException ex) {
                            Util.Logger.log(ex.Message);
                        } catch (Exception ex) {
                            Util.Logger.log(ex.Message);
                        }
                    }
                }
                operation = enOperation.addition;
                return;
            }

            SqlConnection connection = Util.Connection.getConnection();
            if (connection == null) {
                fadeOut(0.55);
                string message = "Veri tabanı bağlantısı sağlanamadı!";
                new MessageBoxForm(message).ShowDialog();
                fadeIn();
                return;
            }
            using (connection) {
                string sql = @"IF EXISTS(SELECT * FROM Tables WHERE Number = " + masa +
                    ") AND NOT EXISTS(SELECT * FROM AdditionTable WHERE AdditionNumber = " + adisyon +
                    " AND DATEADD(DAY, 0, DATEDIFF(DAY, 0, MatchDate)) = DATEADD(DAY, 0, DATEDIFF(DAY, 0, GETDATE()))) " +
                            "BEGIN; " +
                            "INSERT INTO AdditionTable(AdditionNumber,TableNumber,MatchDate) VALUES(" + adisyon + "," + masa + ",GETDATE()) ;" +
                            "END; ";
                try {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    bool inserted = command.ExecuteNonQuery() > 0;
                    connection.Close();
                    String message = string.Format(adisyonIslenmis, adisyon);
                    if (inserted) {
                        string enter = Environment.NewLine;
                        message = string.Format("{0} nolu adisyon, {1} nolu masa bilginiz iletildi. Siparişiniz masanıza getirilecektir." + enter + "Afiyet olsun.", adisyon, masa);
                        var timer = new System.Windows.Forms.Timer() {
                            Interval = 3000
                        };
                        Util.Sound.playBeep();
                        Util.Notify.notify(this, "Adisyon ve masa bilginiz iletildi. Afiyet olsun.");
                    }
                    if (inserted) {
                        Close();
                    } else {
                        fadeOut(0.55);
                    }
                    new MessageBoxForm(message).ShowDialog();
                    fadeIn();
                } catch (AdditionException ex) {
                    string enter = Environment.NewLine;
                    new MessageBoxForm("Veri tabanı hatası sonucu işlem gerçekleştirilemedi!").ShowDialog();
                    Util.Logger.log(ex.Message);
                } catch (Exception ex) {
                    string enter = Environment.NewLine;
                    new MessageBoxForm("Veri tabanı hatası sonucu işlem gerçekleştirilemedi!").ShowDialog();
                    Util.Logger.log(ex.Message);
                }
            }
        }

        private void fadeIn() {
            Util.Animation animation = new Util.Animation();
            animation.fadeIn(this, 1500, 100, Opacity, 1.0, false);
        }

        private void fadeOut(double opacity) {
            Util.Animation animation = new Util.Animation();
            animation.fadeOut(this, 1500, 100, Opacity, opacity, false);
        }

        protected class CustomException : Exception {
            protected CustomException(string message) : base(message) { }
            protected CustomException(string guid, string message) : base(message) { }
        }

        protected class AdditionException : CustomException {
            protected AdditionException(string message) : base(message) { }
            protected AdditionException(string guid, string message) : base(guid, message) {
                _guid = guid;
            }

            protected string _guid;

            public override string Message {
                get {
                    string guid = "(ExceptionCode: 101)";
                    if (!string.IsNullOrWhiteSpace(_guid)) {
                        guid = _guid;
                    }
                    return $"{Message} {guid}";
                }
            }
        }

        protected class TableException : CustomException {
            protected TableException(string message) : base(message) { }
            protected TableException(string guid, string message) : base(guid, message) {
                _guid = guid;
            }

            protected string _guid;

            public override string Message {
                get {
                    string guid = "(ExceptionCode: 102)";
                    if (!string.IsNullOrWhiteSpace(_guid)) {
                        guid = _guid;
                    }
                    return $"{Message} {guid}";
                }
            }
        }
    }
}