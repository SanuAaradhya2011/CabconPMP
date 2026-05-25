namespace CabconPMP
{
    partial class AccessPassword
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
        /// Required method for Designer support.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.picLock = new System.Windows.Forms.PictureBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtBoxPassword = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pnlMain.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLock)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.Controls.Add(this.pnlButtons);
            this.pnlMain.Controls.Add(this.txtBoxPassword);
            this.pnlMain.Controls.Add(this.lblPassword);
            this.pnlMain.Controls.Add(this.picLock);
            this.pnlMain.Controls.Add(this.pnlHeader);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(20);
            this.pnlMain.Size = new System.Drawing.Size(420, 240);
            this.pnlMain.TabIndex = 0;

            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.pnlHeader.Controls.Add(this.lblSubTitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(20, 20);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(380, 70);
            this.pnlHeader.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(187, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Secure Login Area";

            // 
            // lblSubTitle
            // 
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubTitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblSubTitle.Location = new System.Drawing.Point(22, 43);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(217, 15);
            this.lblSubTitle.TabIndex = 1;
            this.lblSubTitle.Text = "Enter your password to continue access";

            // 
            // picLock
            // 
            this.picLock.BackColor = System.Drawing.Color.FromArgb(239, 246, 255);
            this.picLock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLock.Location = new System.Drawing.Point(40, 115);
            this.picLock.Name = "picLock";
            this.picLock.Size = new System.Drawing.Size(56, 56);
            this.picLock.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picLock.TabIndex = 1;
            this.picLock.TabStop = false;

            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblPassword.Location = new System.Drawing.Point(120, 118);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(73, 19);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password";

            // 
            // txtBoxPassword
            // 
            this.txtBoxPassword.BackColor = System.Drawing.Color.White;
            this.txtBoxPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxPassword.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtBoxPassword.Location = new System.Drawing.Point(124, 143);
            this.txtBoxPassword.MaxLength = 8;
            this.txtBoxPassword.Name = "txtBoxPassword";
            this.txtBoxPassword.PasswordChar = '●';
            this.txtBoxPassword.Size = new System.Drawing.Size(240, 27);
            this.txtBoxPassword.TabIndex = 0;
            this.txtBoxPassword.Text = "lng123#";

            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(20, 180);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(380, 40);
            this.pnlButtons.TabIndex = 4;

            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(172, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 34);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = " Login";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);

            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnCancel.Location = new System.Drawing.Point(274, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 34);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = " Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;

            // 
            // AccessPassword
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(420, 240);
            this.Controls.Add(this.pnlMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AccessPassword";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cabcon PMP - Secure Access";
            this.Load += new System.EventHandler(this.AccessPassword_Load);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLock)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.PictureBox picLock;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtBoxPassword;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}