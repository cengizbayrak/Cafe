using System;
using System.Diagnostics;
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
        }

        public frmAdisyon(int masa) : this() {
            this.masa = masa;
        }

        private void frmAdisyon_Load(object sender, EventArgs e) {
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
                Application.Exit();
            } else if (adisyon == 1454) {
                var frm = new ConnectionParameters();
                frm.ShowDialog();
                return;
            }
            Opacity = 0;
            Close();
            string text = string.Format(adisyonIslenmis, adisyon);
            new MessageBoxForm(text).ShowDialog();
        }
    }
}