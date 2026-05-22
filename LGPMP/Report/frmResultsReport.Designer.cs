namespace CabconPMP
{
    partial class frmResultsReport
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
            this.lblFind = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.lblReports = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.lblPrint = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.lblState = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.lblClose = new System.Windows.Forms.ToolStripLabel();
            this.DGVTestResult = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txttotalrecCount = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtfindto = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbSortBy = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbMeterType = new System.Windows.Forms.ComboBox();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbTestType = new System.Windows.Forms.ComboBox();
            this.colPCBAID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMeterID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWorkStationID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTestType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTestID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colstatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMeterType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colexecutionDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpdationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ts_Menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVTestResult)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ts_Menu
            // 
            this.ts_Menu.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ts_Menu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ts_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator7,
            this.lblFind,
            this.toolStripSeparator3,
            this.lblReports,
            this.toolStripSeparator4,
            this.lblPrint,
            this.toolStripSeparator2,
            this.lblState,
            this.toolStripSeparator9,
            this.lblClose});
            this.ts_Menu.Location = new System.Drawing.Point(0, 0);
            this.ts_Menu.Name = "ts_Menu";
            this.ts_Menu.Size = new System.Drawing.Size(1089, 25);
            this.ts_Menu.TabIndex = 18;
            this.ts_Menu.Text = "toolStrip1";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
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
            // lblReports
            // 
            this.lblReports.Name = "lblReports";
            this.lblReports.Size = new System.Drawing.Size(48, 22);
            this.lblReports.Text = "Report";
            this.lblReports.ToolTipText = "Click To Generate Execution Detailed Report For Selected Items.";
            this.lblReports.Click += new System.EventHandler(this.lblReports_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
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
            // lblState
            // 
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(37, 22);
            this.lblState.Text = "State";
            this.lblState.ToolTipText = "Click here to view the graphical state of the selected meter";
            this.lblState.Click += new System.EventHandler(this.lblState_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
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
            this.colMeterID,
            this.colWorkStationID,
            this.colTestType,
            this.colTestID,
            this.colstatus,
            this.colCustomerName,
            this.colMeterType,
            this.colexecutionDate,
            this.colUpdationDate});
            this.DGVTestResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGVTestResult.Location = new System.Drawing.Point(3, 16);
            this.DGVTestResult.Name = "DGVTestResult";
            this.DGVTestResult.ReadOnly = true;
            this.DGVTestResult.Size = new System.Drawing.Size(1064, 442);
            this.DGVTestResult.TabIndex = 19;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DGVTestResult);
            this.groupBox1.Location = new System.Drawing.Point(12, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1070, 461);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txttotalrecCount);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtfindto);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cmbSortBy);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.cmbStatus);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cmbMeterType);
            this.groupBox2.Controls.Add(this.txtFind);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cmbTestType);
            this.groupBox2.Location = new System.Drawing.Point(12, 28);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1070, 64);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            // 
            // txttotalrecCount
            // 
            this.txttotalrecCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.txttotalrecCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txttotalrecCount.Location = new System.Drawing.Point(903, 28);
            this.txttotalrecCount.Name = "txttotalrecCount";
            this.txttotalrecCount.Size = new System.Drawing.Size(140, 20);
            this.txttotalrecCount.TabIndex = 15;
            this.txttotalrecCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(920, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Total Records Count";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(804, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "To";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(628, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "From";
            // 
            // txtfindto
            // 
            this.txtfindto.Location = new System.Drawing.Point(736, 28);
            this.txtfindto.Name = "txtfindto";
            this.txtfindto.Size = new System.Drawing.Size(160, 20);
            this.txtfindto.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(474, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Search By ";
            // 
            // cmbSortBy
            // 
            this.cmbSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSortBy.FormattingEnabled = true;
            this.cmbSortBy.Items.AddRange(new object[] {
            "Test ID",
            "Customer Name",
            "Execution Date",
            "PCBA ID",
            "Meter ID",
            "User ID"});
            this.cmbSortBy.Location = new System.Drawing.Point(443, 28);
            this.cmbSortBy.Name = "cmbSortBy";
            this.cmbSortBy.Size = new System.Drawing.Size(121, 21);
            this.cmbSortBy.TabIndex = 7;
            this.cmbSortBy.SelectedIndexChanged += new System.EventHandler(this.cmbSortBy_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(342, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Meter Status";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "All",
            "Pass",
            "Fail"});
            this.cmbStatus.Location = new System.Drawing.Point(327, 29);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(99, 21);
            this.cmbStatus.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(174, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Meter Type";
            // 
            // cmbMeterType
            // 
            this.cmbMeterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMeterType.FormattingEnabled = true;
            this.cmbMeterType.Location = new System.Drawing.Point(143, 28);
            this.cmbMeterType.Name = "cmbMeterType";
            this.cmbMeterType.Size = new System.Drawing.Size(170, 21);
            this.cmbMeterType.TabIndex = 3;
            // 
            // txtFind
            // 
            this.txtFind.Location = new System.Drawing.Point(570, 28);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(160, 20);
            this.txtFind.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Test Type";
            // 
            // cmbTestType
            // 
            this.cmbTestType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTestType.FormattingEnabled = true;
            this.cmbTestType.Location = new System.Drawing.Point(6, 28);
            this.cmbTestType.Name = "cmbTestType";
            this.cmbTestType.Size = new System.Drawing.Size(115, 21);
            this.cmbTestType.TabIndex = 0;
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
            // colMeterID
            // 
            this.colMeterID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colMeterID.HeaderText = "Meter ID";
            this.colMeterID.Name = "colMeterID";
            this.colMeterID.ReadOnly = true;
            this.colMeterID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colMeterID.Width = 54;
            // 
            // colWorkStationID
            // 
            this.colWorkStationID.HeaderText = "Work Station ID";
            this.colWorkStationID.Name = "colWorkStationID";
            this.colWorkStationID.ReadOnly = true;
            // 
            // colTestType
            // 
            this.colTestType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colTestType.HeaderText = "Test Type";
            this.colTestType.Name = "colTestType";
            this.colTestType.ReadOnly = true;
            this.colTestType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colTestType.Width = 61;
            // 
            // colTestID
            // 
            this.colTestID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colTestID.HeaderText = "Test ID";
            this.colTestID.Name = "colTestID";
            this.colTestID.ReadOnly = true;
            this.colTestID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colTestID.Width = 48;
            // 
            // colstatus
            // 
            this.colstatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colstatus.HeaderText = "Result";
            this.colstatus.Name = "colstatus";
            this.colstatus.ReadOnly = true;
            this.colstatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colstatus.Width = 43;
            // 
            // colCustomerName
            // 
            this.colCustomerName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colCustomerName.HeaderText = "Customer Name";
            this.colCustomerName.Name = "colCustomerName";
            this.colCustomerName.ReadOnly = true;
            this.colCustomerName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCustomerName.Width = 88;
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
            // colexecutionDate
            // 
            this.colexecutionDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colexecutionDate.HeaderText = "Execution Date";
            this.colexecutionDate.Name = "colexecutionDate";
            this.colexecutionDate.ReadOnly = true;
            this.colexecutionDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colexecutionDate.Width = 86;
            // 
            // colUpdationDate
            // 
            this.colUpdationDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colUpdationDate.HeaderText = "User ID";
            this.colUpdationDate.Name = "colUpdationDate";
            this.colUpdationDate.ReadOnly = true;
            this.colUpdationDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colUpdationDate.Width = 49;
            // 
            // frmResultsReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1089, 549);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ts_Menu);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmResultsReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Detailed Report";
            this.Load += new System.EventHandler(this.frmResultsReport_Load);
            this.ts_Menu.ResumeLayout(false);
            this.ts_Menu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVTestResult)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip ts_Menu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel lblFind;
        private System.Windows.Forms.ToolStripLabel lblReports;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel lblClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.DataGridView DGVTestResult;
        private System.Windows.Forms.ToolStripLabel lblPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel lblState;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbSortBy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbMeterType;
        private System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbTestType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtfindto;
        private System.Windows.Forms.TextBox txttotalrecCount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPCBAID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMeterID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWorkStationID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTestType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTestID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colstatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMeterType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colexecutionDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUpdationDate;
    }
}