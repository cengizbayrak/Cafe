using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace BinaryFormatterTutorial {
    public partial class frmBinaryFormatter : Form {
        public frmBinaryFormatter() {
            InitializeComponent();

            rtxtPlain.Clear();
            rtxtBinary.Clear();

            btnToPlain.Click += (s, e) => {
                if (rtxtBinary.TextLength == 0) {
                    return;
                }
                byte[] binData = Convert.FromBase64String(rtxtBinary.Text);
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(binData);
                var bytes = (byte[])formatter.Deserialize(ms);
                rtxtPlain.Text = Encoding.Unicode.GetString(bytes);
            };
            btnToBinary.Click += (s, e) => {
                if (rtxtPlain.TextLength == 0) {
                    return;
                }
                MemoryStream streamMemory = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                var bytes = Encoding.Unicode.GetBytes(rtxtPlain.Text);
                formatter.Serialize(streamMemory, bytes);
                rtxtBinary.Text = Convert.ToBase64String(streamMemory.GetBuffer());
            };
        }
    }
}