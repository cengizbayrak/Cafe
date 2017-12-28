using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cafe {
    public partial class CustomButton : UserControl {
        public string title { get; set; }
        public EventHandler click { get; set; }

        public CustomButton(string title, EventHandler click) {
            InitializeComponent();
            this.title = title;
            this.click = click;
            btn.Text = title;
            btn.Click += click;
            btn.Refresh();
        }
    }
}
