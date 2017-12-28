using System.Windows.Forms;

namespace Cafe {
    public partial class MessageBoxForm : Form {
        public MessageBoxForm() {
            InitializeComponent();

            txtMesaj.GotFocus += (taos, taoe) => btnTamam.Focus();
            btnTamam.Focus();
            btnTamam.Click += (bts, bte) => Close();
        }

        public MessageBoxForm(string text) : this() {
            txtMesaj.Text = text;
        }
    }
}