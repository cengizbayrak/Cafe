namespace Cafe {
    partial class frmAppConfig {
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
            this.lblSatir = new System.Windows.Forms.Label();
            this.lblMasa = new System.Windows.Forms.Label();
            this.lblSutun = new System.Windows.Forms.Label();
            this.lblMesajSuresi = new System.Windows.Forms.Label();
            this.nudMesajSuresi = new System.Windows.Forms.NumericUpDown();
            this.switchSesler = new SwitchBox();
            this.lblSesler = new System.Windows.Forms.Label();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.nudSatir = new System.Windows.Forms.NumericUpDown();
            this.nudSutun = new System.Windows.Forms.NumericUpDown();
            this.nudMasa = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudMesajSuresi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSatir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSutun)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMasa)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSatir
            // 
            this.lblSatir.AutoSize = true;
            this.lblSatir.Location = new System.Drawing.Point(12, 15);
            this.lblSatir.Name = "lblSatir";
            this.lblSatir.Size = new System.Drawing.Size(31, 13);
            this.lblSatir.TabIndex = 0;
            this.lblSatir.Text = "Satır:";
            // 
            // lblMasa
            // 
            this.lblMasa.AutoSize = true;
            this.lblMasa.Location = new System.Drawing.Point(12, 67);
            this.lblMasa.Name = "lblMasa";
            this.lblMasa.Size = new System.Drawing.Size(36, 13);
            this.lblMasa.TabIndex = 1;
            this.lblMasa.Text = "Masa:";
            // 
            // lblSutun
            // 
            this.lblSutun.AutoSize = true;
            this.lblSutun.Location = new System.Drawing.Point(12, 41);
            this.lblSutun.Name = "lblSutun";
            this.lblSutun.Size = new System.Drawing.Size(38, 13);
            this.lblSutun.TabIndex = 5;
            this.lblSutun.Text = "Sütun:";
            // 
            // lblMesajSuresi
            // 
            this.lblMesajSuresi.AutoSize = true;
            this.lblMesajSuresi.Location = new System.Drawing.Point(12, 93);
            this.lblMesajSuresi.Name = "lblMesajSuresi";
            this.lblMesajSuresi.Size = new System.Drawing.Size(68, 13);
            this.lblMesajSuresi.TabIndex = 6;
            this.lblMesajSuresi.Text = "Mesaj süresi:";
            // 
            // nudMesajSuresi
            // 
            this.nudMesajSuresi.Location = new System.Drawing.Point(98, 91);
            this.nudMesajSuresi.Name = "nudMesajSuresi";
            this.nudMesajSuresi.Size = new System.Drawing.Size(92, 20);
            this.nudMesajSuresi.TabIndex = 7;
            // 
            // switchSesler
            // 
            this.switchSesler.AutoSize = true;
            this.switchSesler.Location = new System.Drawing.Point(98, 117);
            this.switchSesler.Name = "switchSesler";
            this.switchSesler.Padding = new System.Windows.Forms.Padding(6);
            this.switchSesler.Size = new System.Drawing.Size(92, 29);
            this.switchSesler.TabIndex = 8;
            this.switchSesler.Text = "switchBox1";
            this.switchSesler.UseVisualStyleBackColor = true;
            // 
            // lblSesler
            // 
            this.lblSesler.AutoSize = true;
            this.lblSesler.Location = new System.Drawing.Point(12, 123);
            this.lblSesler.Name = "lblSesler";
            this.lblSesler.Size = new System.Drawing.Size(39, 13);
            this.lblSesler.TabIndex = 9;
            this.lblSesler.Text = "Sesler:";
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(115, 152);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(75, 23);
            this.btnKaydet.TabIndex = 10;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            // 
            // nudSatir
            // 
            this.nudSatir.Location = new System.Drawing.Point(98, 13);
            this.nudSatir.Name = "nudSatir";
            this.nudSatir.Size = new System.Drawing.Size(92, 20);
            this.nudSatir.TabIndex = 11;
            // 
            // nudSutun
            // 
            this.nudSutun.Location = new System.Drawing.Point(98, 39);
            this.nudSutun.Name = "nudSutun";
            this.nudSutun.Size = new System.Drawing.Size(92, 20);
            this.nudSutun.TabIndex = 12;
            // 
            // nudMasa
            // 
            this.nudMasa.Location = new System.Drawing.Point(98, 65);
            this.nudMasa.Name = "nudMasa";
            this.nudMasa.Size = new System.Drawing.Size(92, 20);
            this.nudMasa.TabIndex = 13;
            // 
            // frmAppConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(204, 185);
            this.Controls.Add(this.nudMasa);
            this.Controls.Add(this.nudSutun);
            this.Controls.Add(this.nudSatir);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.lblSesler);
            this.Controls.Add(this.switchSesler);
            this.Controls.Add(this.nudMesajSuresi);
            this.Controls.Add(this.lblMesajSuresi);
            this.Controls.Add(this.lblSutun);
            this.Controls.Add(this.lblMasa);
            this.Controls.Add(this.lblSatir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(220, 224);
            this.MinimumSize = new System.Drawing.Size(220, 224);
            this.Name = "frmAppConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "App Config";
            ((System.ComponentModel.ISupportInitialize)(this.nudMesajSuresi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSatir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSutun)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMasa)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSatir;
        private System.Windows.Forms.Label lblMasa;
        private System.Windows.Forms.Label lblSutun;
        private System.Windows.Forms.Label lblMesajSuresi;
        private System.Windows.Forms.NumericUpDown nudMesajSuresi;
        private SwitchBox switchSesler;
        private System.Windows.Forms.Label lblSesler;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.NumericUpDown nudSatir;
        private System.Windows.Forms.NumericUpDown nudSutun;
        private System.Windows.Forms.NumericUpDown nudMasa;
    }
}