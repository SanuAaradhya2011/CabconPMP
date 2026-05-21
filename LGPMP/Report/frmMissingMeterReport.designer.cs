namespace LGPMP
{
    partial class frmMissingMeterReport
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
            this.lblPrint = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.lblClose = new System.Windows.Forms.ToolStripLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblMissingCount = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblPassCount = new System.Windows.Forms.Label();
            this.lblFailCount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbMidDegits = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFixString = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMeterIDTo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbMeterType = new System.Windows.Forms.ComboBox();
            this.txtmeterIDFrom = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tabMeterStatus = new System.Windows.Forms.TabControl();
            this.tabPagePass = new System.Windows.Forms.TabPage();
            this.DGVPassMeterList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageFail = new System.Windows.Forms.TabPage();
            this.DGVFailMeterList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageMissing = new System.Windows.Forms.TabPage();
            this.DGVMissingMeterList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkmeterlistMissing = new System.Windows.Forms.ListBox();
            this.ts_Menu.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabMeterStatus.SuspendLayout();
            this.tabPagePass.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVPassMeterList)).BeginInit();
            this.tabPageFail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVFailMeterList)).BeginInit();
            this.tabPageMissing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVMissingMeterList)).BeginInit();
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
            this.lblPrint,
            this.toolStripSeparator2,
            this.lblClose});
            this.ts_Menu.Location = new System.Drawing.Point(0, 0);
            this.ts_Menu.Name = "ts_Menu";
            this.ts_Menu.Size = new System.Drawing.Size(504, 25);
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
            this.lblFind.Size = new System.Drawing.Size(96, 22);
            this.lblFind.Text = "Get Meters List";
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
            this.lblPrint.Size = new System.Drawing.Size(48, 22);
            this.lblPrint.Text = "Report";
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cmbMidDegits);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtFixString);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtMeterIDTo);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cmbMeterType);
            this.groupBox2.Controls.Add(this.txtmeterIDFrom);
            this.groupBox2.Location = new System.Drawing.Point(7, 69);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(255, 206);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanel1.Controls.Add(this.lblMissingCount, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label15, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label13, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPassCount, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblFailCount, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(30, 148);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.55556F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.44444F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(213, 38);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // lblMissingCount
            // 
            this.lblMissingCount.AutoSize = true;
            this.lblMissingCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMissingCount.Location = new System.Drawing.Point(133, 21);
            this.lblMissingCount.Name = "lblMissingCount";
            this.lblMissingCount.Size = new System.Drawing.Size(75, 15);
            this.lblMissingCount.TabIndex = 5;
            this.lblMissingCount.Text = "---";
            this.lblMissingCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(133, 2);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(75, 17);
            this.label15.TabIndex = 4;
            this.label15.Text = "MISSING";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(69, 2);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 17);
            this.label13.TabIndex = 1;
            this.label13.Text = "FAIL";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(5, 2);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 17);
            this.label12.TabIndex = 0;
            this.label12.Text = "PASS";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPassCount
            // 
            this.lblPassCount.AutoSize = true;
            this.lblPassCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPassCount.Location = new System.Drawing.Point(5, 21);
            this.lblPassCount.Name = "lblPassCount";
            this.lblPassCount.Size = new System.Drawing.Size(56, 15);
            this.lblPassCount.TabIndex = 3;
            this.lblPassCount.Text = "---";
            this.lblPassCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFailCount
            // 
            this.lblFailCount.AutoSize = true;
            this.lblFailCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFailCount.Location = new System.Drawing.Point(69, 21);
            this.lblFailCount.Name = "lblFailCount";
            this.lblFailCount.Size = new System.Drawing.Size(56, 15);
            this.lblFailCount.TabIndex = 2;
            this.lblFailCount.Text = "---";
            this.lblFailCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Total MID Digit\'s";
            // 
            // cmbMidDegits
            // 
            this.cmbMidDegits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMidDegits.FormattingEnabled = true;
            this.cmbMidDegits.Items.AddRange(new object[] {
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
            this.cmbMidDegits.Location = new System.Drawing.Point(128, 38);
            this.cmbMidDegits.Name = "cmbMidDegits";
            this.cmbMidDegits.Size = new System.Drawing.Size(115, 21);
            this.cmbMidDegits.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Pre-Fix Digit";
            // 
            // txtFixString
            // 
            this.txtFixString.Location = new System.Drawing.Point(128, 63);
            this.txtFixString.MaxLength = 10;
            this.txtFixString.Name = "txtFixString";
            this.txtFixString.Size = new System.Drawing.Size(115, 20);
            this.txtFixString.TabIndex = 2;
            this.txtFixString.Text = "SM110";
            this.txtFixString.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFixString_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(64, 117);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "To";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Meter ID From";
            // 
            // txtMeterIDTo
            // 
            this.txtMeterIDTo.Location = new System.Drawing.Point(128, 110);
            this.txtMeterIDTo.MaxLength = 16;
            this.txtMeterIDTo.Name = "txtMeterIDTo";
            this.txtMeterIDTo.Size = new System.Drawing.Size(115, 20);
            this.txtMeterIDTo.TabIndex = 4;
            this.txtMeterIDTo.Text = "999";
            this.txtMeterIDTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMeterIDTo_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Meter Type";
            // 
            // cmbMeterType
            // 
            this.cmbMeterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMeterType.FormattingEnabled = true;
            this.cmbMeterType.Location = new System.Drawing.Point(73, 14);
            this.cmbMeterType.Name = "cmbMeterType";
            this.cmbMeterType.Size = new System.Drawing.Size(170, 21);
            this.cmbMeterType.TabIndex = 0;
            // 
            // txtmeterIDFrom
            // 
            this.txtmeterIDFrom.Location = new System.Drawing.Point(128, 86);
            this.txtmeterIDFrom.MaxLength = 16;
            this.txtmeterIDFrom.Name = "txtmeterIDFrom";
            this.txtmeterIDFrom.Size = new System.Drawing.Size(115, 20);
            this.txtmeterIDFrom.TabIndex = 3;
            this.txtmeterIDFrom.Text = "100";
            this.txtmeterIDFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtmeterIDFrom_KeyPress);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tabMeterStatus);
            this.groupBox4.Location = new System.Drawing.Point(263, 28);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(231, 310);
            this.groupBox4.TabIndex = 23;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Meter ID List";
            // 
            // tabMeterStatus
            // 
            this.tabMeterStatus.Controls.Add(this.tabPagePass);
            this.tabMeterStatus.Controls.Add(this.tabPageFail);
            this.tabMeterStatus.Controls.Add(this.tabPageMissing);
            this.tabMeterStatus.Location = new System.Drawing.Point(5, 19);
            this.tabMeterStatus.Name = "tabMeterStatus";
            this.tabMeterStatus.SelectedIndex = 0;
            this.tabMeterStatus.Size = new System.Drawing.Size(219, 283);
            this.tabMeterStatus.TabIndex = 5;
            // 
            // tabPagePass
            // 
            this.tabPagePass.Controls.Add(this.DGVPassMeterList);
            this.tabPagePass.Location = new System.Drawing.Point(4, 22);
            this.tabPagePass.Name = "tabPagePass";
            this.tabPagePass.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePass.Size = new System.Drawing.Size(211, 257);
            this.tabPagePass.TabIndex = 0;
            this.tabPagePass.Text = "     PASS     ";
            this.tabPagePass.UseVisualStyleBackColor = true;
            // 
            // DGVPassMeterList
            // 
            this.DGVPassMeterList.AllowUserToAddRows = false;
            this.DGVPassMeterList.AllowUserToDeleteRows = false;
            this.DGVPassMeterList.AllowUserToResizeColumns = false;
            this.DGVPassMeterList.AllowUserToResizeRows = false;
            this.DGVPassMeterList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DGVPassMeterList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            this.DGVPassMeterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGVPassMeterList.Location = new System.Drawing.Point(3, 3);
            this.DGVPassMeterList.MultiSelect = false;
            this.DGVPassMeterList.Name = "DGVPassMeterList";
            this.DGVPassMeterList.ReadOnly = true;
            this.DGVPassMeterList.Size = new System.Drawing.Size(205, 251);
            this.DGVPassMeterList.TabIndex = 21;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "PASS Meters List";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tabPageFail
            // 
            this.tabPageFail.Controls.Add(this.DGVFailMeterList);
            this.tabPageFail.Location = new System.Drawing.Point(4, 22);
            this.tabPageFail.Name = "tabPageFail";
            this.tabPageFail.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFail.Size = new System.Drawing.Size(211, 319);
            this.tabPageFail.TabIndex = 1;
            this.tabPageFail.Text = "     FAIL     ";
            this.tabPageFail.UseVisualStyleBackColor = true;
            // 
            // DGVFailMeterList
            // 
            this.DGVFailMeterList.AllowUserToAddRows = false;
            this.DGVFailMeterList.AllowUserToDeleteRows = false;
            this.DGVFailMeterList.AllowUserToResizeColumns = false;
            this.DGVFailMeterList.AllowUserToResizeRows = false;
            this.DGVFailMeterList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DGVFailMeterList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3});
            this.DGVFailMeterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGVFailMeterList.Location = new System.Drawing.Point(3, 3);
            this.DGVFailMeterList.MultiSelect = false;
            this.DGVFailMeterList.Name = "DGVFailMeterList";
            this.DGVFailMeterList.ReadOnly = true;
            this.DGVFailMeterList.Size = new System.Drawing.Size(205, 313);
            this.DGVFailMeterList.TabIndex = 21;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "FAIL Meters List";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tabPageMissing
            // 
            this.tabPageMissing.Controls.Add(this.DGVMissingMeterList);
            this.tabPageMissing.Controls.Add(this.chkmeterlistMissing);
            this.tabPageMissing.Location = new System.Drawing.Point(4, 22);
            this.tabPageMissing.Name = "tabPageMissing";
            this.tabPageMissing.Size = new System.Drawing.Size(211, 319);
            this.tabPageMissing.TabIndex = 2;
            this.tabPageMissing.Text = "   Missing   ";
            this.tabPageMissing.UseVisualStyleBackColor = true;
            // 
            // DGVMissingMeterList
            // 
            this.DGVMissingMeterList.AllowUserToAddRows = false;
            this.DGVMissingMeterList.AllowUserToDeleteRows = false;
            this.DGVMissingMeterList.AllowUserToResizeColumns = false;
            this.DGVMissingMeterList.AllowUserToResizeRows = false;
            this.DGVMissingMeterList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DGVMissingMeterList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2});
            this.DGVMissingMeterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGVMissingMeterList.Location = new System.Drawing.Point(0, 0);
            this.DGVMissingMeterList.MultiSelect = false;
            this.DGVMissingMeterList.Name = "DGVMissingMeterList";
            this.DGVMissingMeterList.ReadOnly = true;
            this.DGVMissingMeterList.Size = new System.Drawing.Size(211, 319);
            this.DGVMissingMeterList.TabIndex = 20;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "MISSING Meters List";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // chkmeterlistMissing
            // 
            this.chkmeterlistMissing.FormattingEnabled = true;
            this.chkmeterlistMissing.Location = new System.Drawing.Point(5, 21);
            this.chkmeterlistMissing.Name = "chkmeterlistMissing";
            this.chkmeterlistMissing.Size = new System.Drawing.Size(194, 212);
            this.chkmeterlistMissing.TabIndex = 0;
            // 
            // frmMissingMeterReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 345);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ts_Menu);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMissingMeterReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Serialization Missing Meters Report";
            this.Load += new System.EventHandler(this.frmResultsReport_Load);
            this.ts_Menu.ResumeLayout(false);
            this.ts_Menu.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.tabMeterStatus.ResumeLayout(false);
            this.tabPagePass.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGVPassMeterList)).EndInit();
            this.tabPageFail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGVFailMeterList)).EndInit();
            this.tabPageMissing.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGVMissingMeterList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip ts_Menu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel lblFind;
        private System.Windows.Forms.ToolStripLabel lblClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel lblPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbMeterType;
        private System.Windows.Forms.TextBox txtmeterIDFrom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMeterIDTo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFixString;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbMidDegits;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblMissingCount;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblPassCount;
        private System.Windows.Forms.Label lblFailCount;
        private System.Windows.Forms.TabControl tabMeterStatus;
        private System.Windows.Forms.TabPage tabPagePass;
        private System.Windows.Forms.TabPage tabPageFail;
        private System.Windows.Forms.TabPage tabPageMissing;
        private System.Windows.Forms.DataGridView DGVMissingMeterList;
        private System.Windows.Forms.ListBox chkmeterlistMissing;
        private System.Windows.Forms.DataGridView DGVPassMeterList;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridView DGVFailMeterList;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}