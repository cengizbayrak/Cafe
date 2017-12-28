using System.Windows.Forms;

namespace Cafe {
    public partial class MessageBoxForm : Form {
        public MessageBoxForm() {
            InitializeComponent();

            txtMesaj.GotFocus += (taos, taoe) => btnTamam.Focus();
            btnTamam.Focus();
            btnTamam.Click += (bts, bte) => Close();

            Opacity = 0.0;

            Util.Animation animation = new Util.Animation();
            const string note = "Bu mesaj {0} saniye sonra kaybolacaktır! Hemen yok etmek için herhangi bir yere tıklayın!";
            int duration = int.Parse(Util.AppSettings.getValue(Util.AppSettings.Key.messageBoxDuration));
            lblNote.Text = string.Format(note, duration);
            Load += (s, e) => {
                animation.fadeIn(this, 1500, 100, Opacity, 1.0, false, false);
                Timer timer = new Timer {
                    Interval = 1000
                };
                timer.Tick += (ts, te) => {
                    if (duration <= 1) {
                        timer.Stop();
                        timer.Dispose();
                        Close();
                    }
                    duration -= 1;
                    lblNote.Text = string.Format(note, duration);
                };
                timer.Start();
            };
            FormClosing += (s, e) => {
                animation.fadeOut(this, duration: 1500, steps: 100, currentOpacity: Opacity, targetOpacity: 0.0, close: false);
            };
            foreach (Control control in Controls) {
                control.MouseDown += (s, e) => Close();
            }
        }

        public MessageBoxForm(string text) : this() {
            if (!string.IsNullOrWhiteSpace(text)) {
                txtMesaj.Text = text;
            }
        }
    }
}