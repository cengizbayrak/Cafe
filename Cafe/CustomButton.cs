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
            var tooltip = new ToolTip();

            //btn.MouseHover += (s, e) => {
            //    tooltip.SetToolTip(btn, Width.ToString() + "/" + Height.ToString() + "\n" +
            //        btn.Width.ToString() + "/" + btn.Height.ToString() + "\n" +
            //        btn.Font.ToString());
            //};
           
            SizeChanged += (s, e) => {
                btn.Size = Size;
            };
            btn.SizeChanged += (s, e) => {
                btn.Font = sizeTextToControl(btn, btn.CreateGraphics(), padding: 25);
            };
            btn.FontChanged += (s, e) => {
                btn.Font = sizeTextToControl(btn, btn.CreateGraphics(), padding: 25);
            };
        }

        /// <summary>
        /// Size control text to control size
        /// </summary>
        /// <param name="control">The control to find the font size that should be used.</param>
        /// <param name="graphic">The graphics context.</param>
        /// <param name="padding">The padding around the text.</param>
        /// <returns>The font the control should use.</returns>
        private Font sizeTextToControl(Control control, Graphics graphic, int padding) {
            // Create a small font
            Font font;
            font = new Font(control.Font.FontFamily, 6.0f, control.Font.Style);
            SizeF textSize = graphic.MeasureString(control.Text, font);
            // Loop until it fits perfect
            while ((textSize.Height < control.Height - padding) && (textSize.Width < control.Width - padding) && (textSize.Height < control.Height - padding)) {
                font = new Font(font.FontFamily, font.Size + 0.5f, font.Style);
                textSize = graphic.MeasureString(control.Text, font);
            }
            font = new Font(font.FontFamily, font.Size - 0.5f, font.Style);
            return font;
        }

        private float newFontSize(Graphics graphics, Size size, Font font, string text) {
            SizeF stringSize = graphics.MeasureString(text, font);
            float wRatio = size.Width / stringSize.Width;
            float hRatio = size.Height / stringSize.Height;
            float ratio = Math.Min(hRatio, wRatio);
            return font.Size * ratio;
        }

        public Font GetAdjustedFont(Graphics GraphicRef, string GraphicString, Font OriginalFont, int ContainerWidth, int MaxFontSize, int MinFontSize, bool SmallestOnFail) {
            Font testFont = null;
            // We utilize MeasureString which we get via a control instance           
            for (int AdjustedSize = MaxFontSize; AdjustedSize >= MinFontSize; AdjustedSize--) {
                testFont = new Font(OriginalFont.Name, AdjustedSize, OriginalFont.Style);

                // Test the string with the new size
                SizeF AdjustedSizeNew = GraphicRef.MeasureString(GraphicString, testFont);

                if (ContainerWidth > Convert.ToInt32(AdjustedSizeNew.Width)) {
                    // Good font, return it
                    return testFont;
                }
            }

            // If you get here there was no fontsize that worked
            // return MinimumSize or Original?
            if (SmallestOnFail) {
                return testFont;
            } else {
                return OriginalFont;
            }
        }
    }
}
