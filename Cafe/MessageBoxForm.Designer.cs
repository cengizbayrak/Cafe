namespace Cafe {
    partial class MessageBoxForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.txtMesaj = new System.Windows.Forms.TextBox();
            this.btnTamam = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtMesaj
            // 
            this.txtMesaj.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMesaj.BackColor = System.Drawing.Color.White;
            this.txtMesaj.Font = new System.Drawing.Font("Calibri", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtMesaj.Location = new System.Drawing.Point(12, 12);
            this.txtMesaj.Multiline = true;
            this.txtMesaj.Name = "txtMesaj";
            this.txtMesaj.ReadOnly = true;
            this.txtMesaj.Size = new System.Drawing.Size(541, 179);
            this.txtMesaj.TabIndex = 0;
            this.txtMesaj.Text = "Masa ve adisyon bilginiz iletildi. Siparişiniz masanıza getirilecektir.\r\nAfiyet o" +
    "lsun.";
            // 
            // btnTamam
            // 
            this.btnTamam.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.btnTamam.Font = new System.Drawing.Font("Calibri", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnTamam.ForeColor = System.Drawing.Color.White;
            this.btnTamam.Location = new System.Drawing.Point(411, 197);
            this.btnTamam.Name = "btnTamam";
            this.btnTamam.Size = new System.Drawing.Size(144, 64);
            this.btnTamam.TabIndex = 1;
            this.btnTamam.Text = "Tamam";
            this.btnTamam.UseVisualStyleBackColor = false;
            // 
            // MessageBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(567, 272);
            this.Controls.Add(this.btnTamam);
            this.Controls.Add(this.txtMesaj);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MessageBoxForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MessageBoxForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMesaj;
        private System.Windows.Forms.Button btnTamam;
    }
}