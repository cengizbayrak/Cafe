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
            this.lblNote = new System.Windows.Forms.Label();
            this.lblNoteTag = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtMesaj
            // 
            this.txtMesaj.BackColor = System.Drawing.SystemColors.ControlDark;
            this.txtMesaj.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMesaj.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtMesaj.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtMesaj.Font = new System.Drawing.Font("Calibri", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtMesaj.ForeColor = System.Drawing.Color.White;
            this.txtMesaj.Location = new System.Drawing.Point(3, 3);
            this.txtMesaj.Multiline = true;
            this.txtMesaj.Name = "txtMesaj";
            this.txtMesaj.ReadOnly = true;
            this.txtMesaj.Size = new System.Drawing.Size(561, 188);
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
            // lblNote
            // 
            this.lblNote.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblNote.ForeColor = System.Drawing.Color.White;
            this.lblNote.Location = new System.Drawing.Point(44, 230);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(361, 33);
            this.lblNote.TabIndex = 2;
            this.lblNote.Text = "Bu mesaj 10 saniye sonra kaybolacaktır! Hemen yok etmek için herhangi bir yere tı" +
    "klayın!";
            // 
            // lblNoteTag
            // 
            this.lblNoteTag.AutoSize = true;
            this.lblNoteTag.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblNoteTag.ForeColor = System.Drawing.Color.White;
            this.lblNoteTag.Location = new System.Drawing.Point(6, 230);
            this.lblNoteTag.Name = "lblNoteTag";
            this.lblNoteTag.Size = new System.Drawing.Size(34, 15);
            this.lblNoteTag.TabIndex = 3;
            this.lblNoteTag.Text = "NOT:";
            // 
            // MessageBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(567, 272);
            this.Controls.Add(this.lblNoteTag);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.btnTamam);
            this.Controls.Add(this.txtMesaj);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MessageBoxForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MessageBoxForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMesaj;
        private System.Windows.Forms.Button btnTamam;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Label lblNoteTag;
    }
}