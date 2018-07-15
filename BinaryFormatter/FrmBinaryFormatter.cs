using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BinaryFormatter {
    public partial class FrmBinaryFormatter : Form {
        public FrmBinaryFormatter() {
            InitializeComponent();

            rtxtPlain.Clear();
            rtxtBinary.Clear();

            btnToPlain.Click += (s, e) => {
                if (rtxtBinary.TextLength == 0) {
                    return;
                }
                byte[] binData = Convert.FromBase64String(rtxtBinary.Text);
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                MemoryStream ms = new MemoryStream(binData);
                var bytes = (byte[])formatter.Deserialize(ms);
                rtxtPlain.Text = Encoding.Unicode.GetString(bytes);
            };
            btnToBinary.Click += (s, e) => {
                if (rtxtPlain.TextLength == 0) {
                    return;
                }
                MemoryStream streamMemory = new MemoryStream();
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                var bytes = Encoding.Unicode.GetBytes(rtxtPlain.Text);
                formatter.Serialize(streamMemory, bytes);
                rtxtBinary.Text = Convert.ToBase64String(streamMemory.GetBuffer());
            };
        }
    }
}