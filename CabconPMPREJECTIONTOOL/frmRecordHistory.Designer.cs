namespace CabconPMPREJECTIONTOOL
{
    partial class frmRecordHistory
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
            this.dgvRecordHistory = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newHWErrorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkPCBAID = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecordHistory)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvRecordHistory
            // 
            this.dgvRecordHistory.AllowUserToAddRows = false;
            this.dgvRecordHistory.AllowUserToDeleteRows = false;
            this.dgvRecordHistory.AllowUserToOrderColumns = true;
            this.dgvRecordHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRecordHistory.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgvRecordHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecordHistory.Location = new System.Drawing.Point(4, 27);
            this.dgvRecordHistory.Name = "dgvRecordHistory";
            this.dgvRecordHistory.ReadOnly = true;
            this.dgvRecordHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecordHistory.Size = new System.Drawing.Size(977, 358);
            this.dgvRecordHistory.TabIndex = 0;
            this.dgvRecordHistory.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRecordHistory_CellDoubleClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.saveExcelToolStripMenuItem,
            this.newHWErrorToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(65, 21);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(53, 21);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(52, 21);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // saveExcelToolStripMenuItem
            // 
            this.saveExcelToolStripMenuItem.Name = "saveExcelToolStripMenuItem";
            this.saveExcelToolStripMenuItem.Size = new System.Drawing.Size(82, 21);
            this.saveExcelToolStripMenuItem.Text = "Save Excel";
            this.saveExcelToolStripMenuItem.Click += new System.EventHandler(this.saveExcelToolStripMenuItem_Click);
            // 
            // newHWErrorToolStripMenuItem
            // 
            this.newHWErrorToolStripMenuItem.Name = "newHWErrorToolStripMenuItem";
            this.newHWErrorToolStripMenuItem.Size = new System.Drawing.Size(108, 21);
            this.newHWErrorToolStripMenuItem.Text = "New HW Error";
            this.newHWErrorToolStripMenuItem.Click += new System.EventHandler(this.newHWErrorToolStripMenuItem_Click);
            // 
            // chkPCBAID
            // 
            this.chkPCBAID.AutoSize = true;
            this.chkPCBAID.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.chkPCBAID.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPCBAID.Location = new System.Drawing.Point(776, 4);
            this.chkPCBAID.Name = "chkPCBAID";
            this.chkPCBAID.Size = new System.Drawing.Size(188, 21);
            this.chkPCBAID.TabIndex = 2;
            this.chkPCBAID.Text = "All History for this PCBAID";
            this.chkPCBAID.UseVisualStyleBackColor = false;
            this.chkPCBAID.CheckedChanged += new System.EventHandler(this.chkPCBAID_CheckedChanged);
            // 
            // frmRecordHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 389);
            this.Controls.Add(this.chkPCBAID);
            this.Controls.Add(this.dgvRecordHistory);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1000, 400);
            this.Name = "frmRecordHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PCBA Record Rejection History";
            this.Shown += new System.EventHandler(this.frmRecordHistory_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecordHistory)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvRecordHistory;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkPCBAID;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newHWErrorToolStripMenuItem;
    }
}