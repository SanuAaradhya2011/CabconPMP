namespace CabconPMP
{
    partial class frmUserManagement
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
            this.panelUserInput = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtConfirmnewPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.cmbUserType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtnewPassword = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.DGVUsermgt = new System.Windows.Forms.DataGridView();
            this.panelUserInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVUsermgt)).BeginInit();
            this.SuspendLayout();
            // 
            // panelUserInput
            // 
            this.panelUserInput.Controls.Add(this.label4);
            this.panelUserInput.Controls.Add(this.txtConfirmnewPassword);
            this.panelUserInput.Controls.Add(this.label1);
            this.panelUserInput.Controls.Add(this.txtUserID);
            this.panelUserInput.Controls.Add(this.cmbUserType);
            this.panelUserInput.Controls.Add(this.label3);
            this.panelUserInput.Controls.Add(this.label2);
            this.panelUserInput.Controls.Add(this.txtnewPassword);
            this.panelUserInput.Location = new System.Drawing.Point(12, 12);
            this.panelUserInput.Name = "panelUserInput";
            this.panelUserInput.Size = new System.Drawing.Size(355, 160);
            this.panelUserInput.TabIndex = 0;
            this.panelUserInput.BackColor = System.Drawing.Color.White;
            this.panelUserInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(22, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 33;
            this.label4.Text = "Confirm Password";
            // 
            // txtConfirmnewPassword
            // 
            this.txtConfirmnewPassword.Location = new System.Drawing.Point(155, 114);
            this.txtConfirmnewPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtConfirmnewPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConfirmnewPassword.MaxLength = 16;
            this.txtConfirmnewPassword.Name = "txtConfirmnewPassword";
            this.txtConfirmnewPassword.Size = new System.Drawing.Size(168, 20);
            this.txtConfirmnewPassword.TabIndex = 3;
            this.txtConfirmnewPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtConfirmnewPassword_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "User Type";
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(155, 61);
            this.txtUserID.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtUserID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserID.MaxLength = 16;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(168, 20);
            this.txtUserID.TabIndex = 1;
            this.txtUserID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUserID_KeyPress);
            // 
            // cmbUserType
            // 
            this.cmbUserType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUserType.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbUserType.FormattingEnabled = true;
            this.cmbUserType.Items.AddRange(new object[] {
            "Operator",
            "Vendor",
            "Supervisor",
            "Reader",
            "Administrator",
            "PowerAdministrator",
            "Validation",
            "Rework"});
            this.cmbUserType.Location = new System.Drawing.Point(155, 34);
            this.cmbUserType.Name = "cmbUserType";
            this.cmbUserType.Size = new System.Drawing.Size(168, 23);
            this.cmbUserType.TabIndex = 0;
            this.cmbUserType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbUserType_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(22, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(22, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "User ID";
            // 
            // txtnewPassword
            // 
            this.txtnewPassword.Location = new System.Drawing.Point(155, 88);
            this.txtnewPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtnewPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtnewPassword.MaxLength = 16;
            this.txtnewPassword.Name = "txtnewPassword";
            this.txtnewPassword.Size = new System.Drawing.Size(168, 20);
            this.txtnewPassword.TabIndex = 2;
            this.txtnewPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtnewPassword_KeyPress);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(384, 55);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 30);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(384, 94);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 30);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(232, 17, 35);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // DGVUsermgt
            // 
            this.DGVUsermgt.AllowUserToAddRows = false;
            this.DGVUsermgt.AllowUserToDeleteRows = false;
            this.DGVUsermgt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVUsermgt.Location = new System.Drawing.Point(12, 184);
            this.DGVUsermgt.Name = "DGVUsermgt";
            this.DGVUsermgt.ReadOnly = true;
            this.DGVUsermgt.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGVUsermgt.Size = new System.Drawing.Size(448, 186);
            this.DGVUsermgt.TabIndex = 6;
            this.DGVUsermgt.BackgroundColor = System.Drawing.Color.White;
            this.DGVUsermgt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGVUsermgt.GridColor = System.Drawing.Color.LightGray;
            this.DGVUsermgt.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.DGVUsermgt.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.DGVUsermgt.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.DGVUsermgt.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.DGVUsermgt.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.DGVUsermgt.DoubleClick += new System.EventHandler(this.DGVUsermgt_DoubleClick);
            // 
            // frmUserManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(471, 390);
            this.Controls.Add(this.DGVUsermgt);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panelUserInput);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUserManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Management";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Load += new System.EventHandler(this.frmUserManagement_Load);
            this.panelUserInput.ResumeLayout(false);
            this.panelUserInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVUsermgt)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelUserInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.ComboBox cmbUserType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtnewPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtConfirmnewPassword;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView DGVUsermgt;
    }
}