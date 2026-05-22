namespace CabconPMP
{
    partial class frmProductionStausReport
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
            this.ts_Menu = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.cmbMeterType = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cmbSortBy = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.txtFrom = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.txtTo = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblFind = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.lblPrint = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.lblClose = new System.Windows.Forms.ToolStripLabel();
            this.DGVTestResult = new System.Windows.Forms.DataGridView();
            this.colPCBAID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMeterType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colexecutionEMS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCalibration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSerialization = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ts_Menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVTestResult)).BeginInit();
            this.SuspendLayout();
            // 
            // ts_Menu
            // 
            this.ts_Menu.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ts_Menu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ts_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator7,
            this.toolStripLabel3,
            this.cmbMeterType,
            this.toolStripSeparator6,
            this.toolStripLabel1,
            this.cmbSortBy,
            this.toolStripLabel4,
            this.txtFrom,
            this.toolStripLabel2,
            this.txtTo,
            this.toolStripSeparator1,
            this.lblFind,
            this.toolStripSeparator3,
            this.lblPrint,
            this.toolStripSeparator2,
            this.lblClose});
            this.ts_Menu.Location = new System.Drawing.Point(0, 0);
            this.ts_Menu.Name = "ts_Menu";
            this.ts_Menu.Size = new System.Drawing.Size(923, 25);
            this.ts_Menu.TabIndex = 18;
            this.ts_Menu.Text = "toolStrip1";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(43, 22);
            this.toolStripLabel3.Text = "Meter";
            // 
            // cmbMeterType
            // 
            this.cmbMeterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMeterType.Name = "cmbMeterType";
            this.cmbMeterType.Size = new System.Drawing.Size(190, 25);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(68, 22);
            this.toolStripLabel1.Text = "Search By ";
            // 
            // cmbSortBy
            // 
            this.cmbSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSortBy.Items.AddRange(new object[] {
            "Execution Date",
            "PCBA ID"});
            this.cmbSortBy.Name = "cmbSortBy";
            this.cmbSortBy.Size = new System.Drawing.Size(120, 25);
            this.cmbSortBy.SelectedIndexChanged += new System.EventHandler(this.cmbSortBy_SelectedIndexChanged);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(38, 22);
            this.toolStripLabel4.Text = "From";
            // 
            // txtFrom
            // 
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(23, 22);
            this.toolStripLabel2.Text = "To";
            // 
            // txtTo
            // 
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // lblFind
            // 
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new System.Drawing.Size(32, 22);
            this.lblFind.Text = "Find";
            this.lblFind.ToolTipText = "Click To Display Analysis Report As Per Selected Condition.";
            this.lblFind.Click += new System.EventHandler(this.lblFind_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // lblPrint
            // 
            this.lblPrint.Name = "lblPrint";
            this.lblPrint.Size = new System.Drawing.Size(34, 22);
            this.lblPrint.Text = "Print";
            this.lblPrint.ToolTipText = "Click To Generate Execution Status Report.";
            this.lblPrint.Click += new System.EventHandler(this.lblPrint_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // lblClose
            // 
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(40, 22);
            this.lblClose.Text = "Close";
            this.lblClose.ToolTipText = "Click To Close The Report Window";
            this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
            // 
            // DGVTestResult
            // 
            this.DGVTestResult.AllowUserToAddRows = false;
            this.DGVTestResult.AllowUserToDeleteRows = false;
            this.DGVTestResult.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.DGVTestResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPCBAID,
            this.colMeterType,
            this.colexecutionEMS,
            this.colFT,
            this.colCalibration,
            this.colSerialization});
            this.DGVTestResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGVTestResult.Location = new System.Drawing.Point(0, 25);
            this.DGVTestResult.Name = "DGVTestResult";
            this.DGVTestResult.ReadOnly = true;
            this.DGVTestResult.Size = new System.Drawing.Size(923, 436);
            this.DGVTestResult.TabIndex = 19;
            // 
            // colPCBAID
            // 
            this.colPCBAID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colPCBAID.HeaderText = "PCBA ID";
            this.colPCBAID.Name = "colPCBAID";
            this.colPCBAID.ReadOnly = true;
            this.colPCBAID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colPCBAID.Width = 55;
            // 
            // colMeterType
            // 
            this.colMeterType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colMeterType.HeaderText = "Meter Type";
            this.colMeterType.Name = "colMeterType";
            this.colMeterType.ReadOnly = true;
            this.colMeterType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colMeterType.Width = 67;
            // 
            // colexecutionEMS
            // 
            this.colexecutionEMS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colexecutionEMS.HeaderText = "EMS Test Status";
            this.colexecutionEMS.Name = "colexecutionEMS";
            this.colexecutionEMS.ReadOnly = true;
            this.colexecutionEMS.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colexecutionEMS.Width = 93;
            // 
            // colFT
            // 
            this.colFT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colFT.HeaderText = "FT Status";
            this.colFT.Name = "colFT";
            this.colFT.ReadOnly = true;
            this.colFT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colFT.Width = 59;
            // 
            // colCalibration
            // 
            this.colCalibration.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colCalibration.HeaderText = "Calibration Status";
            this.colCalibration.Name = "colCalibration";
            this.colCalibration.ReadOnly = true;
            this.colCalibration.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCalibration.Width = 95;
            // 
            // colSerialization
            // 
            this.colSerialization.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colSerialization.HeaderText = "Serialization Status";
            this.colSerialization.Name = "colSerialization";
            this.colSerialization.ReadOnly = true;
            this.colSerialization.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSerialization.Width = 102;
            // 
            // frmProductionStausReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 461);
            this.Controls.Add(this.DGVTestResult);
            this.Controls.Add(this.ts_Menu);
            this.MinimizeBox = false;
            this.Name = "frmProductionStausReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Production Status Report";
            this.Load += new System.EventHandler(this.frmProductionStausReport_Load);
            this.ts_Menu.ResumeLayout(false);
            this.ts_Menu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVTestResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip ts_Menu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cmbSortBy;
        private System.Windows.Forms.ToolStripLabel lblFind;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel lblClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.DataGridView DGVTestResult;
        private System.Windows.Forms.ToolStripLabel lblPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox cmbMeterType;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPCBAID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMeterType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colexecutionEMS;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCalibration;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSerialization;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripTextBox txtFrom;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox txtTo;
    }
}