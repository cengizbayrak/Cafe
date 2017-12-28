using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cafe {
    public partial class frmAdisyon : Form {
        /// <summary>
        /// string.format ile çağır, adisyon numarasını parametre olarak gönder
        /// </summary>
        private const string adisyonIslenmis = "{0} numaralı adisyon işleme alınmış görünüyor! Lütfen adisyon numaranızı kontrol edin!";

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

            Util.Animation animation = new Util.Animation();
            Load += (s, e) => {
                StartPosition = FormStartPosition.CenterParent;
                animation.fadeIn(this, 1500, 100, Opacity, 1.0, false, false);
            };
            FormClosing += (s, e) => {
                animation.fadeOut(this, 1500, 100, Opacity, 0.0, false);
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
            btnVazgec.Click += (bvs, bve) => Close();
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
            }

            SqlConnection connection = Util.Connection.getConnection();
            if (connection == null) {
                string message = "Veri tabanı bağlantısı sağlanamadı!";
                new MessageBoxForm(message).ShowDialog();
                return;
            }
            using (connection) {
                string sql = @"IF NOT EXISTS(SELECT * FROM AdditionTable WHERE AdditionNumber = " + adisyon + " AND DATEADD(DAY, 0, DATEDIFF(DAY, 0, MatchDate)) = DATEADD(DAY, 0, DATEDIFF(DAY, 0, GETDATE()))) " +
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
                        message = string.Format("{0} nolu adisyon, {1} nolu masa bilginiz iletildi. Siparişiniz masanıza gelecektir." + enter + "Afiyet olsun.", adisyon, masa);
                        var timer = new System.Windows.Forms.Timer() {
                            Interval = 3000
                        };
                        try {
                            NotifyIcon notify = new NotifyIcon() {
                                Icon = new Icon(Icon, 40, 40),
                                Visible = true,
                                Text = "Adisyon ve masa bilginiz iletildi. Afiyet olsun.",
                                BalloonTipTitle = "Cafe Arjantin",
                                BalloonTipText = "Adisyon ve masa bilginiz iletildi. Afiyet olsun.",
                                BalloonTipIcon = ToolTipIcon.Info
                            };
                            notify.ShowBalloonTip(2000);
                        } catch (Exception ex) {
                            Util.Logger.log(ex.Message);
                        }
                    }
                    if (inserted) {
                        Close();
                    }
                    new MessageBoxForm(message).ShowDialog();
                } catch (Exception ex) {
                    string enter = Environment.NewLine;
                    new MessageBoxForm("Veri tabanı hatası sonucu işlem gerçekleştirilemedi!" + enter + enter + "Hata kodu: 101").ShowDialog();
                    Util.Logger.log(ex.Message);
                }
            }
        }
    }
}