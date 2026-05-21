namespace PMP3PHDLMS
{
    partial class frmLPRScan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_LPR = new System.Windows.Forms.Label();
            this.textBox_LPR = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label_LPR
            // 
            this.label_LPR.AutoSize = true;
            this.label_LPR.Location = new System.Drawing.Point(48, 80);
            this.label_LPR.Name = "label_LPR";
            this.label_LPR.Size = new System.Drawing.Size(103, 13);
            this.label_LPR.TabIndex = 0;
            this.label_LPR.Text = "Enter/Scan LPR ID:";
            // 
            // textBox_LPR
            // 
            this.textBox_LPR.Location = new System.Drawing.Point(158, 77);
            this.textBox_LPR.MaxLength = 7;
            this.textBox_LPR.Name = "textBox_LPR";
            this.textBox_LPR.Size = new System.Drawing.Size(100, 20);
            this.textBox_LPR.TabIndex = 1;
            this.textBox_LPR.Text = "000434";
            this.textBox_LPR.TextChanged += new System.EventHandler(this.textBox_LPR_TextChanged);
            // 
            // frmLPRScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 216);
            this.Controls.Add(this.textBox_LPR);
            this.Controls.Add(this.label_LPR);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLPRScan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Scan LPR ID";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_LPR;
        private System.Windows.Forms.TextBox textBox_LPR;
    }
}