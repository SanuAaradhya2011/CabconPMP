namespace CabconPMP.Report
{
    partial class frmRoutineTestReport
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
            this.pabelRoutineTest = new System.Windows.Forms.Panel();
            this.ts_Menu = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.lblFind = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.lblReport = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.lblClose = new System.Windows.Forms.ToolStripLabel();
            this.chkRangedReport = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblMissingCount = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblPassCount = new System.Windows.Forms.Label();
            this.lblFailCount = new System.Windows.Forms.Label();
            this.tabMeterStatus = new System.Windows.Forms.TabControl();
            this.tabPagePass = new System.Windows.Forms.TabPage();
            this.chkmeterlistPass = new System.Windows.Forms.CheckedListBox();
            this.chkselectall = new System.Windows.Forms.CheckBox();
            this.tabPageFail = new System.Windows.Forms.TabPage();
            this.chkAllFailed = new System.Windows.Forms.CheckBox();
            this.chkmeterlistFail = new System.Windows.Forms.CheckedListBox();
            this.tabPageMissing = new System.Windows.Forms.TabPage();
            this.DGVMissingMeterList = new System.Windows.Forms.DataGridView();
            this.colMeterID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkmeterlistMissing = new System.Windows.Forms.ListBox();
            this.gpreportset = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbMidDegits = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtFixString = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMeterIDTo = new System.Windows.Forms.TextBox();
            this.txtmeterIDFrom = new System.Windows.Forms.TextBox();
            this.cmbMeterType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtto = new System.Windows.Forms.DateTimePicker();
            this.dtfrom = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtorderdate = new System.Windows.Forms.TextBox();
            this.txtmeterrating = new System.Windows.Forms.TextBox();
            this.txtorderno = new System.Windows.Forms.TextBox();
            this.txtcustomername = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.statusContaner = new System.Windows.Forms.StatusStrip();
            this.toolStriplbstatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pabelRoutineTest.SuspendLayout();
            this.ts_Menu.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabMeterStatus.SuspendLayout();
            this.tabPagePass.SuspendLayout();
            this.tabPageFail.SuspendLayout();
            this.tabPageMissing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVMissingMeterList)).BeginInit();
            this.gpreportset.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusContaner.SuspendLayout();
            this.SuspendLayout();
            // 
            // pabelRoutineTest
            // 
            this.pabelRoutineTest.Controls.Add(this.ts_Menu);
            this.pabelRoutineTest.Controls.Add(this.chkRangedReport);
            this.pabelRoutineTest.Controls.Add(this.groupBox3);
            this.pabelRoutineTest.Controls.Add(this.gpreportset);
            this.pabelRoutineTest.Controls.Add(this.groupBox1);
            this.pabelRoutineTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pabelRoutineTest.Location = new System.Drawing.Point(0, 0);
            this.pabelRoutineTest.Name = "pabelRoutineTest";
            this.pabelRoutineTest.Size = new System.Drawing.Size(568, 390);
            this.pabelRoutineTest.TabIndex = 0;
            // 
            // ts_Menu
            // 
            this.ts_Menu.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ts_Menu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ts_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator7,
            this.lblFind,
            this.toolStripSeparator3,
            this.lblReport,
            this.toolStripSeparator2,
            this.lblClose});
            this.ts_Menu.Location = new System.Drawing.Point(0, 0);
            this.ts_Menu.Name = "ts_Menu";
            this.ts_Menu.Size = new System.Drawing.Size(568, 25);
            this.ts_Menu.TabIndex = 19;
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
            // lblReport
            // 
            this.lblReport.Name = "lblReport";
            this.lblReport.Size = new System.Drawing.Size(48, 22);
            this.lblReport.Text = "Report";
            this.lblReport.ToolTipText = "Click To Generate Execution Status Report.";
            this.lblReport.Click += new System.EventHandler(this.lblReport_Click);
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
            // chkRangedReport
            // 
            this.chkRangedReport.AutoSize = true;
            this.chkRangedReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRangedReport.Location = new System.Drawing.Point(44, 337);
            this.chkRangedReport.Name = "chkRangedReport";
            this.chkRangedReport.Size = new System.Drawing.Size(256, 17);
            this.chkRangedReport.TabIndex = 11;
            this.chkRangedReport.Text = "Combined Report With Meter ID Padding";
            this.chkRangedReport.UseVisualStyleBackColor = true;
            this.chkRangedReport.CheckedChanged += new System.EventHandler(this.chkRangedReport_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel1);
            this.groupBox3.Controls.Add(this.tabMeterStatus);
            this.groupBox3.Location = new System.Drawing.Point(333, 28);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(224, 333);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Meter ID List";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 87F));
            this.tableLayoutPanel1.Controls.Add(this.lblMissingCount, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label15, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label13, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPassCount, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblFailCount, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 288);
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
            this.lblMissingCount.Location = new System.Drawing.Point(127, 21);
            this.lblMissingCount.Name = "lblMissingCount";
            this.lblMissingCount.Size = new System.Drawing.Size(81, 15);
            this.lblMissingCount.TabIndex = 5;
            this.lblMissingCount.Text = "---";
            this.lblMissingCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(127, 2);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(81, 17);
            this.label15.TabIndex = 4;
            this.label15.Text = "MISSING";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(66, 2);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 17);
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
            this.label12.Size = new System.Drawing.Size(53, 17);
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
            this.lblPassCount.Size = new System.Drawing.Size(53, 15);
            this.lblPassCount.TabIndex = 3;
            this.lblPassCount.Text = "---";
            this.lblPassCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFailCount
            // 
            this.lblFailCount.AutoSize = true;
            this.lblFailCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFailCount.Location = new System.Drawing.Point(66, 21);
            this.lblFailCount.Name = "lblFailCount";
            this.lblFailCount.Size = new System.Drawing.Size(53, 15);
            this.lblFailCount.TabIndex = 2;
            this.lblFailCount.Text = "---";
            this.lblFailCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabMeterStatus
            // 
            this.tabMeterStatus.Controls.Add(this.tabPagePass);
            this.tabMeterStatus.Controls.Add(this.tabPageFail);
            this.tabMeterStatus.Controls.Add(this.tabPageMissing);
            this.tabMeterStatus.Location = new System.Drawing.Point(5, 19);
            this.tabMeterStatus.Name = "tabMeterStatus";
            this.tabMeterStatus.SelectedIndex = 0;
            this.tabMeterStatus.Size = new System.Drawing.Size(213, 263);
            this.tabMeterStatus.TabIndex = 5;
            this.tabMeterStatus.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabMeterStatus_Selected);
            // 
            // tabPagePass
            // 
            this.tabPagePass.Controls.Add(this.chkmeterlistPass);
            this.tabPagePass.Controls.Add(this.chkselectall);
            this.tabPagePass.Location = new System.Drawing.Point(4, 22);
            this.tabPagePass.Name = "tabPagePass";
            this.tabPagePass.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePass.Size = new System.Drawing.Size(205, 237);
            this.tabPagePass.TabIndex = 0;
            this.tabPagePass.Text = "     PASS     ";
            this.tabPagePass.UseVisualStyleBackColor = true;
            // 
            // chkmeterlistPass
            // 
            this.chkmeterlistPass.FormattingEnabled = true;
            this.chkmeterlistPass.Location = new System.Drawing.Point(5, 21);
            this.chkmeterlistPass.Name = "chkmeterlistPass";
            this.chkmeterlistPass.Size = new System.Drawing.Size(194, 214);
            this.chkmeterlistPass.TabIndex = 1;
            // 
            // chkselectall
            // 
            this.chkselectall.AutoSize = true;
            this.chkselectall.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkselectall.Location = new System.Drawing.Point(7, 3);
            this.chkselectall.Name = "chkselectall";
            this.chkselectall.Size = new System.Drawing.Size(80, 17);
            this.chkselectall.TabIndex = 4;
            this.chkselectall.Text = "Select All";
            this.chkselectall.UseVisualStyleBackColor = true;
            this.chkselectall.CheckedChanged += new System.EventHandler(this.chkselectall_CheckedChanged);
            // 
            // tabPageFail
            // 
            this.tabPageFail.Controls.Add(this.chkAllFailed);
            this.tabPageFail.Controls.Add(this.chkmeterlistFail);
            this.tabPageFail.Location = new System.Drawing.Point(4, 22);
            this.tabPageFail.Name = "tabPageFail";
            this.tabPageFail.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFail.Size = new System.Drawing.Size(205, 237);
            this.tabPageFail.TabIndex = 1;
            this.tabPageFail.Text = "     FAIL     ";
            this.tabPageFail.UseVisualStyleBackColor = true;
            // 
            // chkAllFailed
            // 
            this.chkAllFailed.AutoSize = true;
            this.chkAllFailed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAllFailed.Location = new System.Drawing.Point(7, 3);
            this.chkAllFailed.Name = "chkAllFailed";
            this.chkAllFailed.Size = new System.Drawing.Size(80, 17);
            this.chkAllFailed.TabIndex = 5;
            this.chkAllFailed.Text = "Select All";
            this.chkAllFailed.UseVisualStyleBackColor = true;
            this.chkAllFailed.CheckedChanged += new System.EventHandler(this.chkAllFailed_CheckedChanged);
            // 
            // chkmeterlistFail
            // 
            this.chkmeterlistFail.FormattingEnabled = true;
            this.chkmeterlistFail.Location = new System.Drawing.Point(5, 21);
            this.chkmeterlistFail.Name = "chkmeterlistFail";
            this.chkmeterlistFail.Size = new System.Drawing.Size(194, 214);
            this.chkmeterlistFail.TabIndex = 2;
            // 
            // tabPageMissing
            // 
            this.tabPageMissing.Controls.Add(this.DGVMissingMeterList);
            this.tabPageMissing.Controls.Add(this.chkmeterlistMissing);
            this.tabPageMissing.Location = new System.Drawing.Point(4, 22);
            this.tabPageMissing.Name = "tabPageMissing";
            this.tabPageMissing.Size = new System.Drawing.Size(205, 237);
            this.tabPageMissing.TabIndex = 2;
            this.tabPageMissing.Text = "   Missing   ";
            this.tabPageMissing.UseVisualStyleBackColor = true;
            // 
            // DGVMissingMeterList
            // 
            this.DGVMissingMeterList.AllowUserToDeleteRows = false;
            this.DGVMissingMeterList.AllowUserToResizeColumns = false;
            this.DGVMissingMeterList.AllowUserToResizeRows = false;
            this.DGVMissingMeterList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DGVMissingMeterList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMeterID});
            this.DGVMissingMeterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGVMissingMeterList.Location = new System.Drawing.Point(0, 0);
            this.DGVMissingMeterList.MultiSelect = false;
            this.DGVMissingMeterList.Name = "DGVMissingMeterList";
            this.DGVMissingMeterList.ReadOnly = true;
            this.DGVMissingMeterList.Size = new System.Drawing.Size(205, 237);
            this.DGVMissingMeterList.TabIndex = 20;
            // 
            // colMeterID
            // 
            this.colMeterID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colMeterID.HeaderText = "Missing Meter ID List";
            this.colMeterID.Name = "colMeterID";
            this.colMeterID.ReadOnly = true;
            this.colMeterID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colMeterID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // chkmeterlistMissing
            // 
            this.chkmeterlistMissing.FormattingEnabled = true;
            this.chkmeterlistMissing.Location = new System.Drawing.Point(5, 21);
            this.chkmeterlistMissing.Name = "chkmeterlistMissing";
            this.chkmeterlistMissing.Size = new System.Drawing.Size(194, 212);
            this.chkmeterlistMissing.TabIndex = 0;
            // 
            // gpreportset
            // 
            this.gpreportset.Controls.Add(this.label4);
            this.gpreportset.Controls.Add(this.cmbMidDegits);
            this.gpreportset.Controls.Add(this.label9);
            this.gpreportset.Controls.Add(this.txtFixString);
            this.gpreportset.Controls.Add(this.label10);
            this.gpreportset.Controls.Add(this.label11);
            this.gpreportset.Controls.Add(this.txtMeterIDTo);
            this.gpreportset.Controls.Add(this.txtmeterIDFrom);
            this.gpreportset.Controls.Add(this.cmbMeterType);
            this.gpreportset.Controls.Add(this.label6);
            this.gpreportset.Controls.Add(this.dtto);
            this.gpreportset.Controls.Add(this.dtfrom);
            this.gpreportset.Controls.Add(this.label8);
            this.gpreportset.Controls.Add(this.label7);
            this.gpreportset.Location = new System.Drawing.Point(9, 164);
            this.gpreportset.Name = "gpreportset";
            this.gpreportset.Size = new System.Drawing.Size(318, 156);
            this.gpreportset.TabIndex = 1;
            this.gpreportset.TabStop = false;
            this.gpreportset.Text = "Selection Criteria";
            this.gpreportset.UseCompatibleTextRendering = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 30;
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
            this.cmbMidDegits.Location = new System.Drawing.Point(115, 70);
            this.cmbMidDegits.Name = "cmbMidDegits";
            this.cmbMidDegits.Size = new System.Drawing.Size(71, 21);
            this.cmbMidDegits.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(192, 73);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.label9.TabIndex = 28;
            this.label9.Text = "Pre-Fix";
            // 
            // txtFixString
            // 
            this.txtFixString.Location = new System.Drawing.Point(230, 70);
            this.txtFixString.MaxLength = 10;
            this.txtFixString.Name = "txtFixString";
            this.txtFixString.Size = new System.Drawing.Size(80, 20);
            this.txtFixString.TabIndex = 8;
            this.txtFixString.Text = "SM110";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(204, 106);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(20, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "To";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 106);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(74, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "Meter ID From";
            // 
            // txtMeterIDTo
            // 
            this.txtMeterIDTo.Location = new System.Drawing.Point(230, 103);
            this.txtMeterIDTo.MaxLength = 16;
            this.txtMeterIDTo.Name = "txtMeterIDTo";
            this.txtMeterIDTo.Size = new System.Drawing.Size(80, 20);
            this.txtMeterIDTo.TabIndex = 10;
            this.txtMeterIDTo.Text = "999";
            // 
            // txtmeterIDFrom
            // 
            this.txtmeterIDFrom.Location = new System.Drawing.Point(115, 103);
            this.txtmeterIDFrom.MaxLength = 16;
            this.txtmeterIDFrom.Name = "txtmeterIDFrom";
            this.txtmeterIDFrom.Size = new System.Drawing.Size(80, 20);
            this.txtmeterIDFrom.TabIndex = 9;
            this.txtmeterIDFrom.Text = "100";
            // 
            // cmbMeterType
            // 
            this.cmbMeterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMeterType.FormattingEnabled = true;
            this.cmbMeterType.Location = new System.Drawing.Point(117, 17);
            this.cmbMeterType.Name = "cmbMeterType";
            this.cmbMeterType.Size = new System.Drawing.Size(193, 21);
            this.cmbMeterType.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Meter Type";
            // 
            // dtto
            // 
            this.dtto.CustomFormat = "dd/MM/yyyy";
            this.dtto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtto.Location = new System.Drawing.Point(232, 44);
            this.dtto.Name = "dtto";
            this.dtto.Size = new System.Drawing.Size(78, 20);
            this.dtto.TabIndex = 6;
            // 
            // dtfrom
            // 
            this.dtfrom.CustomFormat = "dd/MM/yyyy";
            this.dtfrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtfrom.Location = new System.Drawing.Point(117, 44);
            this.dtfrom.Name = "dtfrom";
            this.dtfrom.Size = new System.Drawing.Size(78, 20);
            this.dtfrom.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(206, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "To";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Date From";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtorderdate);
            this.groupBox1.Controls.Add(this.txtmeterrating);
            this.groupBox1.Controls.Add(this.txtorderno);
            this.groupBox1.Controls.Add(this.txtcustomername);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(9, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(318, 129);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Report Details";
            // 
            // txtorderdate
            // 
            this.txtorderdate.Location = new System.Drawing.Point(114, 98);
            this.txtorderdate.Name = "txtorderdate";
            this.txtorderdate.Size = new System.Drawing.Size(198, 20);
            this.txtorderdate.TabIndex = 3;
            // 
            // txtmeterrating
            // 
            this.txtmeterrating.Location = new System.Drawing.Point(114, 46);
            this.txtmeterrating.Name = "txtmeterrating";
            this.txtmeterrating.Size = new System.Drawing.Size(200, 20);
            this.txtmeterrating.TabIndex = 1;
            this.txtmeterrating.Text = "240V/05-30A";
            // 
            // txtorderno
            // 
            this.txtorderno.AcceptsReturn = true;
            this.txtorderno.Location = new System.Drawing.Point(114, 72);
            this.txtorderno.Name = "txtorderno";
            this.txtorderno.Size = new System.Drawing.Size(198, 20);
            this.txtorderno.TabIndex = 2;
            this.txtorderno.Text = "LGIND05-2015";
            // 
            // txtcustomername
            // 
            this.txtcustomername.Location = new System.Drawing.Point(114, 20);
            this.txtcustomername.Name = "txtcustomername";
            this.txtcustomername.Size = new System.Drawing.Size(200, 20);
            this.txtcustomername.TabIndex = 0;
            this.txtcustomername.Text = "L+G INDIA";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Order Date ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Meter Rating ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Order No ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name of Customer";
            // 
            // statusContaner
            // 
            this.statusContaner.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStriplbstatus});
            this.statusContaner.Location = new System.Drawing.Point(0, 368);
            this.statusContaner.Name = "statusContaner";
            this.statusContaner.Size = new System.Drawing.Size(568, 22);
            this.statusContaner.TabIndex = 1;
            this.statusContaner.Text = "statusStrip1";
            // 
            // toolStriplbstatus
            // 
            this.toolStriplbstatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStriplbstatus.Name = "toolStriplbstatus";
            this.toolStriplbstatus.Size = new System.Drawing.Size(41, 17);
            this.toolStriplbstatus.Text = "Ready";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // frmRoutineTestReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 390);
            this.Controls.Add(this.statusContaner);
            this.Controls.Add(this.pabelRoutineTest);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRoutineTestReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Calibration Routine Test Report";
            this.Load += new System.EventHandler(this.frmroutinetest_Load);
            this.pabelRoutineTest.ResumeLayout(false);
            this.pabelRoutineTest.PerformLayout();
            this.ts_Menu.ResumeLayout(false);
            this.ts_Menu.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabMeterStatus.ResumeLayout(false);
            this.tabPagePass.ResumeLayout(false);
            this.tabPagePass.PerformLayout();
            this.tabPageFail.ResumeLayout(false);
            this.tabPageFail.PerformLayout();
            this.tabPageMissing.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGVMissingMeterList)).EndInit();
            this.gpreportset.ResumeLayout(false);
            this.gpreportset.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusContaner.ResumeLayout(false);
            this.statusContaner.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pabelRoutineTest;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtmeterrating;
        private System.Windows.Forms.TextBox txtorderno;
        private System.Windows.Forms.TextBox txtcustomername;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gpreportset;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DateTimePicker dtto;
        private System.Windows.Forms.DateTimePicker dtfrom;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkselectall;
        private System.Windows.Forms.TextBox txtorderdate;
        private System.Windows.Forms.ComboBox cmbMeterType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.StatusStrip statusContaner;
        private System.Windows.Forms.ToolStripStatusLabel toolStriplbstatus;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbMidDegits;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtFixString;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtMeterIDTo;
        private System.Windows.Forms.TextBox txtmeterIDFrom;
        private System.Windows.Forms.TabControl tabMeterStatus;
        private System.Windows.Forms.TabPage tabPagePass;
        private System.Windows.Forms.CheckedListBox chkmeterlistPass;
        private System.Windows.Forms.TabPage tabPageFail;
        private System.Windows.Forms.TabPage tabPageMissing;
        private System.Windows.Forms.CheckedListBox chkmeterlistFail;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblPassCount;
        private System.Windows.Forms.Label lblFailCount;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblMissingCount;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ListBox chkmeterlistMissing;
        private System.Windows.Forms.CheckBox chkAllFailed;
        private System.Windows.Forms.DataGridView DGVMissingMeterList;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMeterID;
        private System.Windows.Forms.CheckBox chkRangedReport;
        private System.Windows.Forms.ToolStrip ts_Menu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel lblFind;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel lblReport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel lblClose;
    }
}