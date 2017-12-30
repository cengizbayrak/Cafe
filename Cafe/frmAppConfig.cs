using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cafe {
    public partial class frmAppConfig : Form {
        public frmAppConfig() {
            InitializeComponent();

            int oldTables = 0;

            Load += (s, e) => {
                string rows = Util.AppSettings.getValue(Util.AppSettings.Key.rows);
                string columns = Util.AppSettings.getValue(Util.AppSettings.Key.columns);
                string tables = Util.AppSettings.getValue(Util.AppSettings.Key.tables);
                string duration = Util.AppSettings.getValue(Util.AppSettings.Key.messageBoxDuration);
                string sound = Util.AppSettings.getValue(Util.AppSettings.Key.sound);
                string notify = Util.AppSettings.getValue(Util.AppSettings.Key.notifyIcon);

                int deger = 0;
                int.TryParse(rows, out deger);
                nudSatir.Value = deger;

                deger = 0;
                int.TryParse(columns, out deger);
                nudSutun.Value = deger;

                deger = 0;
                int.TryParse(tables, out deger);
                nudMasa.Value = deger;
                oldTables = deger;

                deger = 0;
                int.TryParse(duration, out deger);
                nudMesajSuresi.Value = deger;
                nudMesajSuresi.Minimum = 5;
                nudMesajSuresi.Maximum = 30;

                bool ses = false;
                bool.TryParse(sound, out ses);
                switchSesler.Checked = ses;

                bool bildirimler = false;
                bool.TryParse(notify, out bildirimler);
                switchBildirimler.Checked = bildirimler;
            };

            nudSatir.ValueChanged += (s, e) => {
                nudMasa.Maximum = (int)nudSatir.Value * (int)nudSutun.Value;
            };
            nudSutun.ValueChanged += (s, e) => {
                nudMasa.Maximum = (int)nudSutun.Value * (int)nudSatir.Value;
            };

            btnKaydet.Click += (s, e) => {
                string rows = Convert.ToString(nudSatir.Value);
                string columns = Convert.ToString(nudSutun.Value);
                string tables = Convert.ToString(nudMasa.Value);
                string duration = Convert.ToString(nudMesajSuresi.Value);
                string sound = Convert.ToString(switchSesler.Checked).ToLower();
                string notify = Convert.ToString(switchBildirimler.Checked).ToLower();

                Util.AppSettings.setValue(Util.AppSettings.Key.rows, rows);
                Util.AppSettings.setValue(Util.AppSettings.Key.columns, columns);
                Util.AppSettings.setValue(Util.AppSettings.Key.tables, tables);
                Util.AppSettings.setValue(Util.AppSettings.Key.messageBoxDuration, duration);
                Util.AppSettings.setValue(Util.AppSettings.Key.sound, sound);
                Util.AppSettings.setValue(Util.AppSettings.Key.notifyIcon, notify);

                int newTables = 0;
                int.TryParse(Util.AppSettings.getValue(Util.AppSettings.Key.tables), out newTables);
                if (newTables != oldTables) {
                    Process.Start(Application.ExecutablePath);
                    Application.Exit();
                    return;
                }
                Close();
            };
        }
    }
}
