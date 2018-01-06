namespace BinaryFormatter {
    partial class FrmBinaryFormatter {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent() {
            this.btnToBinary = new System.Windows.Forms.Button();
            this.lblPlain = new System.Windows.Forms.Label();
            this.lblBinary = new System.Windows.Forms.Label();
            this.rtxtPlain = new System.Windows.Forms.RichTextBox();
            this.rtxtBinary = new System.Windows.Forms.RichTextBox();
            this.btnToPlain = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnToBinary
            // 
            this.btnToBinary.Location = new System.Drawing.Point(553, 216);
            this.btnToBinary.Name = "btnToBinary";
            this.btnToBinary.Size = new System.Drawing.Size(75, 23);
            this.btnToBinary.TabIndex = 0;
            this.btnToBinary.Text = "To Binary";
            this.btnToBinary.UseVisualStyleBackColor = true;
            // 
            // lblPlain
            // 
            this.lblPlain.AutoSize = true;
            this.lblPlain.Location = new System.Drawing.Point(12, 9);
            this.lblPlain.Name = "lblPlain";
            this.lblPlain.Size = new System.Drawing.Size(30, 13);
            this.lblPlain.TabIndex = 3;
            this.lblPlain.Text = "Plain";
            // 
            // lblBinary
            // 
            this.lblBinary.AutoSize = true;
            this.lblBinary.Location = new System.Drawing.Point(12, 117);
            this.lblBinary.Name = "lblBinary";
            this.lblBinary.Size = new System.Drawing.Size(36, 13);
            this.lblBinary.TabIndex = 4;
            this.lblBinary.Text = "Binary";
            // 
            // rtxtPlain
            // 
            this.rtxtPlain.Location = new System.Drawing.Point(53, 12);
            this.rtxtPlain.Name = "rtxtPlain";
            this.rtxtPlain.Size = new System.Drawing.Size(575, 96);
            this.rtxtPlain.TabIndex = 5;
            this.rtxtPlain.Text = "";
            // 
            // rtxtBinary
            // 
            this.rtxtBinary.Location = new System.Drawing.Point(53, 114);
            this.rtxtBinary.Name = "rtxtBinary";
            this.rtxtBinary.Size = new System.Drawing.Size(575, 96);
            this.rtxtBinary.TabIndex = 6;
            this.rtxtBinary.Text = "";
            // 
            // btnToPlain
            // 
            this.btnToPlain.Location = new System.Drawing.Point(53, 216);
            this.btnToPlain.Name = "btnToPlain";
            this.btnToPlain.Size = new System.Drawing.Size(75, 23);
            this.btnToPlain.TabIndex = 7;
            this.btnToPlain.Text = "To Plain";
            this.btnToPlain.UseVisualStyleBackColor = true;
            // 
            // frmBinaryFormatter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 250);
            this.Controls.Add(this.btnToPlain);
            this.Controls.Add(this.rtxtBinary);
            this.Controls.Add(this.rtxtPlain);
            this.Controls.Add(this.lblBinary);
            this.Controls.Add(this.lblPlain);
            this.Controls.Add(this.btnToBinary);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(656, 289);
            this.MinimumSize = new System.Drawing.Size(656, 289);
            this.Name = "frmBinaryFormatter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Binary Formatter Converter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnToBinary;
        private System.Windows.Forms.Label lblPlain;
        private System.Windows.Forms.Label lblBinary;
        private System.Windows.Forms.RichTextBox rtxtPlain;
        private System.Windows.Forms.RichTextBox rtxtBinary;
        private System.Windows.Forms.Button btnToPlain;
    }
}

