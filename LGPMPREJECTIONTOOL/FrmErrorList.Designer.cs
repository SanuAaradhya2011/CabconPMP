namespace LGPMPREJECTIONTOOL
{
    partial class frmErrorList
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuUserID = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.displayRejectionListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewErrorTypesTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertNewErrorRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateErrorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteRejectionRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savePDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItemExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.liveChartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.labelTo = new System.Windows.Forms.Label();
            this.chkShowZeroCountRecords = new System.Windows.Forms.CheckBox();
            this.txtRecordCount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtParamName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtErrorName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtProbDesc = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtRejectCause = new System.Windows.Forms.TextBox();
            this.panelDates = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.chkListShift = new System.Windows.Forms.CheckedListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rdbActionDate = new System.Windows.Forms.RadioButton();
            this.rdbErrorDate = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.chkUniquePCBAID = new System.Windows.Forms.CheckBox();
            this.groupBoxErrDetails = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtRejectAction = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtErrorDate = new System.Windows.Forms.TextBox();
            this.txtActionDate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLoggedUserID = new System.Windows.Forms.TextBox();

            this.labelHorzDiv = new System.Windows.Forms.Label();
            this.lblSearchBy = new System.Windows.Forms.Label();
            this.lblErrorState = new System.Windows.Forms.Label();
            this.lblMeterType = new System.Windows.Forms.Label();
            this.txtSearchPCBAID = new System.Windows.Forms.TextBox();
            this.txtSearchCustomer = new System.Windows.Forms.TextBox();
            this.chkListProdStage = new System.Windows.Forms.CheckedListBox();
            this.chkListMeterType = new System.Windows.Forms.CheckedListBox();
            this.chkListErrorStatus = new System.Windows.Forms.CheckedListBox();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.labelHorzDiv2 = new System.Windows.Forms.Label();
            this.lblVertDiv = new System.Windows.Forms.Label();
            this.labelHorzDiv3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCollapse = new System.Windows.Forms.Button();
            this.panelSearchText = new System.Windows.Forms.Panel();
            this.chkListSource = new System.Windows.Forms.CheckedListBox();
            this.lblProdStage = new System.Windows.Forms.Label();
            this.lblPCBSource = new System.Windows.Forms.Label();
            this.lblLoggedUserID = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblQueryDateTime = new System.Windows.Forms.Label();
            this.dgvRejectionList = new System.Windows.Forms.DataGridView();
            this.chkListSearchBy = new MyCheckedListBox();
            this.menuStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panelDates.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBoxErrDetails.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelFilters.SuspendLayout();
            this.panelSearchText.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRejectionList)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuUserID
            // 
            this.contextMenuUserID.Name = "contextMenuUserID";
            this.contextMenuUserID.ShowImageMargin = false;
            this.contextMenuUserID.Size = new System.Drawing.Size(36, 4);
            this.contextMenuUserID.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuUserID_ItemClicked);
            this.contextMenuUserID.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuUserID_Opening);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayRejectionListToolStripMenuItem,
            this.viewErrorTypesTableToolStripMenuItem,
            this.updateEntryToolStripMenuItem,
            this.printToolStripMenuItem,
            this.liveChartToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1095, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // displayRejectionListToolStripMenuItem
            // 
            this.displayRejectionListToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayRejectionListToolStripMenuItem.Name = "displayRejectionListToolStripMenuItem";
            this.displayRejectionListToolStripMenuItem.Size = new System.Drawing.Size(131, 20);
            this.displayRejectionListToolStripMenuItem.Text = "Refresh Rejection List";
            this.displayRejectionListToolStripMenuItem.Click += new System.EventHandler(this.displayRejectionListToolStripMenuItem_Click);
            // 
            // viewErrorTypesTableToolStripMenuItem
            // 
            this.viewErrorTypesTableToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewErrorTypesTableToolStripMenuItem.Name = "viewErrorTypesTableToolStripMenuItem";
            this.viewErrorTypesTableToolStripMenuItem.Size = new System.Drawing.Size(145, 20);
            this.viewErrorTypesTableToolStripMenuItem.Text = "Refresh ErrorTypes table";
            this.viewErrorTypesTableToolStripMenuItem.Click += new System.EventHandler(this.viewErrorTypesTableToolStripMenuItem_Click);
            // 
            // updateEntryToolStripMenuItem
            // 
            this.updateEntryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertToolStripMenuItem,
            this.updateRecordToolStripMenuItem,
            this.insertNewErrorRecordToolStripMenuItem,
            this.updateErrorToolStripMenuItem,
            this.deleteRejectionRecordToolStripMenuItem});
            this.updateEntryToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateEntryToolStripMenuItem.Name = "updateEntryToolStripMenuItem";
            this.updateEntryToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.updateEntryToolStripMenuItem.Text = "Records";
            // 
            // insertToolStripMenuItem
            // 
            this.insertToolStripMenuItem.Name = "insertToolStripMenuItem";
            this.insertToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.insertToolStripMenuItem.Text = "Insert New Rejection Record";
            this.insertToolStripMenuItem.Click += new System.EventHandler(this.insertToolStripMenuItem_Click);
            // 
            // updateRecordToolStripMenuItem
            // 
            this.updateRecordToolStripMenuItem.Name = "updateRecordToolStripMenuItem";
            this.updateRecordToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.updateRecordToolStripMenuItem.Text = "Update Rejection Record";
            this.updateRecordToolStripMenuItem.Click += new System.EventHandler(this.updateRecordToolStripMenuItem_Click);
            // 
            // insertNewErrorRecordToolStripMenuItem
            // 
            this.insertNewErrorRecordToolStripMenuItem.Enabled = false;
            this.insertNewErrorRecordToolStripMenuItem.Name = "insertNewErrorRecordToolStripMenuItem";
            this.insertNewErrorRecordToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.insertNewErrorRecordToolStripMenuItem.Text = "Insert New ErrorType Record";
            this.insertNewErrorRecordToolStripMenuItem.Visible = false;
            // 
            // updateErrorToolStripMenuItem
            // 
            this.updateErrorToolStripMenuItem.Enabled = false;
            this.updateErrorToolStripMenuItem.Name = "updateErrorToolStripMenuItem";
            this.updateErrorToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.updateErrorToolStripMenuItem.Text = "Update ErrorType Record";
            this.updateErrorToolStripMenuItem.Visible = false;
            // 
            // deleteRejectionRecordToolStripMenuItem
            // 
            this.deleteRejectionRecordToolStripMenuItem.Name = "deleteRejectionRecordToolStripMenuItem";
            this.deleteRejectionRecordToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.deleteRejectionRecordToolStripMenuItem.Text = "Delete Rejection Record";
            this.deleteRejectionRecordToolStripMenuItem.Click += new System.EventHandler(this.deleteRejectionRecordToolStripMenuItem_Click);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.savePDFToolStripMenuItem,
            this.saveToolStripMenuItemExcel});
            this.printToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.printToolStripMenuItem.Text = "Export As";
            // 
            // savePDFToolStripMenuItem
            // 
            this.savePDFToolStripMenuItem.Name = "savePDFToolStripMenuItem";
            this.savePDFToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.savePDFToolStripMenuItem.Text = "PDF";
            this.savePDFToolStripMenuItem.Click += new System.EventHandler(this.asPDFToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItemExcel
            // 
            this.saveToolStripMenuItemExcel.Name = "saveToolStripMenuItemExcel";
            this.saveToolStripMenuItemExcel.Size = new System.Drawing.Size(101, 22);
            this.saveToolStripMenuItemExcel.Text = "Excel";
            this.saveToolStripMenuItemExcel.Click += new System.EventHandler(this.saveToolStripMenuItemExcel_Click);
            // 
            // liveChartToolStripMenuItem
            // 
            this.liveChartToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.liveChartToolStripMenuItem.Name = "liveChartToolStripMenuItem";
            this.liveChartToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.liveChartToolStripMenuItem.Text = "Live Chart";
            this.liveChartToolStripMenuItem.Click += new System.EventHandler(this.liveChartToolStripMenuItem_Click);
            // 
            // dtpStart
            // 
            this.dtpStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpStart.CustomFormat = "dd/MM/yyyy hh:mm:ss tt";
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(6, 4);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(163, 20);
            this.dtpStart.TabIndex = 2;
            // 
            // dtpEnd
            // 
            this.dtpEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpEnd.CustomFormat = "dd/MM/yyyy hh:mm:ss tt";
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(200, 4);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(163, 20);
            this.dtpEnd.TabIndex = 3;
            // 
            // labelTo
            // 
            this.labelTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTo.AutoSize = true;
            this.labelTo.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.labelTo.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTo.Location = new System.Drawing.Point(174, 7);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(23, 16);
            this.labelTo.TabIndex = 4;
            this.labelTo.Text = "TO";
            // 
            // chkShowZeroCountRecords
            // 
            this.chkShowZeroCountRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkShowZeroCountRecords.AutoSize = true;
            this.chkShowZeroCountRecords.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowZeroCountRecords.Location = new System.Drawing.Point(788, 7);
            this.chkShowZeroCountRecords.Name = "chkShowZeroCountRecords";
            this.chkShowZeroCountRecords.Size = new System.Drawing.Size(156, 19);
            this.chkShowZeroCountRecords.TabIndex = 9;
            this.chkShowZeroCountRecords.Text = "Show Zero Count Entries";
            this.chkShowZeroCountRecords.UseVisualStyleBackColor = true;
            this.chkShowZeroCountRecords.Visible = false;
            // 
            // txtRecordCount
            // 
            this.txtRecordCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRecordCount.HideSelection = false;
            this.txtRecordCount.Location = new System.Drawing.Point(53, 4);
            this.txtRecordCount.Name = "txtRecordCount";
            this.txtRecordCount.ReadOnly = true;
            this.txtRecordCount.Size = new System.Drawing.Size(84, 21);
            this.txtRecordCount.TabIndex = 2;
            this.txtRecordCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(15, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Total :";
            // 
            // txtParamName
            // 
            this.txtParamName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtParamName.Location = new System.Drawing.Point(107, 104);
            this.txtParamName.Multiline = true;
            this.txtParamName.Name = "txtParamName";
            this.txtParamName.ReadOnly = true;
            this.txtParamName.Size = new System.Drawing.Size(244, 27);
            this.txtParamName.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(5, 111);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Parameter Name";
            // 
            // txtErrorName
            // 
            this.txtErrorName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtErrorName.Location = new System.Drawing.Point(107, 5);
            this.txtErrorName.Multiline = true;
            this.txtErrorName.Name = "txtErrorName";
            this.txtErrorName.ReadOnly = true;
            this.txtErrorName.Size = new System.Drawing.Size(244, 25);
            this.txtErrorName.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(5, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Error Date";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(5, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Error Name";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox3, 2);
            this.groupBox3.Controls.Add(this.txtProbDesc);
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(356, 2);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox3.Name = "groupBox3";
            this.tableLayoutPanel1.SetRowSpan(this.groupBox3, 3);
            this.groupBox3.Size = new System.Drawing.Size(443, 97);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Problem Description";
            // 
            // txtProbDesc
            // 
            this.txtProbDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProbDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProbDesc.Location = new System.Drawing.Point(3, 18);
            this.txtProbDesc.Multiline = true;
            this.txtProbDesc.Name = "txtProbDesc";
            this.txtProbDesc.ReadOnly = true;
            this.txtProbDesc.Size = new System.Drawing.Size(437, 76);
            this.txtProbDesc.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtRejectCause);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(804, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.SetRowSpan(this.groupBox4, 2);
            this.groupBox4.Size = new System.Drawing.Size(286, 58);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Rejection Cause";
            // 
            // txtRejectCause
            // 
            this.txtRejectCause.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRejectCause.Location = new System.Drawing.Point(0, 15);
            this.txtRejectCause.Multiline = true;
            this.txtRejectCause.Name = "txtRejectCause";
            this.txtRejectCause.ReadOnly = true;
            this.txtRejectCause.Size = new System.Drawing.Size(286, 43);
            this.txtRejectCause.TabIndex = 0;
            // 
            // panelDates
            // 
            this.panelDates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDates.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panelDates.Controls.Add(this.dtpStart);
            this.panelDates.Controls.Add(this.dtpEnd);
            this.panelDates.Controls.Add(this.labelTo);
            this.panelDates.Location = new System.Drawing.Point(724, -2);
            this.panelDates.Name = "panelDates";
            this.panelDates.Size = new System.Drawing.Size(370, 26);
            this.panelDates.TabIndex = 8;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(912, 47);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 15);
            this.label11.TabIndex = 17;
            this.label11.Text = "Shift";
            // 
            // chkListShift
            // 
            this.chkListShift.BackColor = System.Drawing.Color.AliceBlue;
            this.chkListShift.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chkListShift.CheckOnClick = true;
            this.chkListShift.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkListShift.FormattingEnabled = true;
            this.chkListShift.IntegralHeight = false;
            this.chkListShift.Location = new System.Drawing.Point(862, 70);
            this.chkListShift.Name = "chkListShift";
            this.chkListShift.Size = new System.Drawing.Size(146, 164);
            this.chkListShift.TabIndex = 4;
            this.chkListShift.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkListSource_ItemCheck);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel2.Controls.Add(this.rdbActionDate);
            this.panel2.Controls.Add(this.rdbErrorDate);
            this.panel2.Location = new System.Drawing.Point(6, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(206, 26);
            this.panel2.TabIndex = 12;
            // 
            // rdbActionDate
            // 
            this.rdbActionDate.AutoSize = true;
            this.rdbActionDate.Location = new System.Drawing.Point(84, 6);
            this.rdbActionDate.Name = "rdbActionDate";
            this.rdbActionDate.Size = new System.Drawing.Size(125, 17);
            this.rdbActionDate.TabIndex = 0;
            this.rdbActionDate.Text = "Update (Action) Date";
            this.rdbActionDate.UseVisualStyleBackColor = true;
            // 
            // rdbErrorDate
            // 
            this.rdbErrorDate.AutoSize = true;
            this.rdbErrorDate.Checked = true;
            this.rdbErrorDate.Location = new System.Drawing.Point(6, 6);
            this.rdbErrorDate.Name = "rdbErrorDate";
            this.rdbErrorDate.Size = new System.Drawing.Size(73, 17);
            this.rdbErrorDate.TabIndex = 0;
            this.rdbErrorDate.TabStop = true;
            this.rdbErrorDate.Text = "Error Date";
            this.rdbErrorDate.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(387, 5);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 16);
            this.label9.TabIndex = 17;
            this.label9.Text = "▬▬";
            // 
            // chkUniquePCBAID
            // 
            this.chkUniquePCBAID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkUniquePCBAID.AutoSize = true;
            this.chkUniquePCBAID.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.chkUniquePCBAID.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUniquePCBAID.Location = new System.Drawing.Point(713, 7);
            this.chkUniquePCBAID.Name = "chkUniquePCBAID";
            this.chkUniquePCBAID.Size = new System.Drawing.Size(232, 19);
            this.chkUniquePCBAID.TabIndex = 9;
            this.chkUniquePCBAID.Text = "Show Unique PCBAID (Manual Search)";
            this.chkUniquePCBAID.UseVisualStyleBackColor = false;
            this.chkUniquePCBAID.CheckedChanged += new System.EventHandler(this.chkUniquePCBAID_CheckedChanged);
            // 
            // groupBoxErrDetails
            // 
            this.groupBoxErrDetails.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.groupBoxErrDetails.Controls.Add(this.tableLayoutPanel1);
            this.groupBoxErrDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxErrDetails.Location = new System.Drawing.Point(0, 515);
            this.groupBoxErrDetails.Name = "groupBoxErrDetails";
            this.groupBoxErrDetails.Size = new System.Drawing.Size(1095, 136);
            this.groupBoxErrDetails.TabIndex = 11;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtParamName, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtErrorName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtErrorDate, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtActionDate, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtLoggedUserID, 3, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1095, 136);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtRejectAction);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(804, 71);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.SetRowSpan(this.groupBox1, 2);
            this.groupBox1.Size = new System.Drawing.Size(286, 60);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rejection Action";
            // 
            // txtRejectAction
            // 
            this.txtRejectAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRejectAction.Location = new System.Drawing.Point(0, 15);
            this.txtRejectAction.Multiline = true;
            this.txtRejectAction.Name = "txtRejectAction";
            this.txtRejectAction.ReadOnly = true;
            this.txtRejectAction.Size = new System.Drawing.Size(286, 45);
            this.txtRejectAction.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Last Action Date";
            // 
            // txtErrorDate
            // 
            this.txtErrorDate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtErrorDate.Location = new System.Drawing.Point(107, 38);
            this.txtErrorDate.Multiline = true;
            this.txtErrorDate.Name = "txtErrorDate";
            this.txtErrorDate.ReadOnly = true;
            this.txtErrorDate.Size = new System.Drawing.Size(244, 25);
            this.txtErrorDate.TabIndex = 0;
            // 
            // txtActionDate
            // 
            this.txtActionDate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtActionDate.Location = new System.Drawing.Point(107, 71);
            this.txtActionDate.Multiline = true;
            this.txtActionDate.Name = "txtActionDate";
            this.txtActionDate.ReadOnly = true;
            this.txtActionDate.Size = new System.Drawing.Size(244, 25);
            this.txtActionDate.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(359, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(144, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Operator / Logged User ID";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLoggedUserID
            // 
            this.txtLoggedUserID.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLoggedUserID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLoggedUserID.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoggedUserID.Location = new System.Drawing.Point(511, 104);
            this.txtLoggedUserID.Multiline = true;
            this.txtLoggedUserID.Name = "txtLoggedUserID";
            this.txtLoggedUserID.ReadOnly = true;
            this.txtLoggedUserID.Size = new System.Drawing.Size(285, 27);
            this.txtLoggedUserID.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel2.Controls.Add(this.rdbActionDate);
            this.panel2.Controls.Add(this.rdbErrorDate);
            this.panel2.Location = new System.Drawing.Point(516, -2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(206, 26);
            this.panel2.TabIndex = 12;
            // 
            // rdbActionDate
            // 
            this.rdbActionDate.AutoSize = true;
            this.rdbActionDate.Location = new System.Drawing.Point(84, 6);
            this.rdbActionDate.Name = "rdbActionDate";
            this.rdbActionDate.Size = new System.Drawing.Size(125, 17);
            this.rdbActionDate.TabIndex = 0;
            this.rdbActionDate.Text = "Update (Action) Date";
            this.rdbActionDate.UseVisualStyleBackColor = true;
            // 
            // rdbErrorDate
            // 
            this.rdbErrorDate.AutoSize = true;
            this.rdbErrorDate.Checked = true;
            this.rdbErrorDate.Location = new System.Drawing.Point(6, 6);
            this.rdbErrorDate.Name = "rdbErrorDate";
            this.rdbErrorDate.Size = new System.Drawing.Size(73, 17);
            this.rdbErrorDate.TabIndex = 0;
            this.rdbErrorDate.TabStop = true;
            this.rdbErrorDate.Text = "Error Date";
            this.rdbErrorDate.UseVisualStyleBackColor = true;
            // 
            // labelHorzDiv
            // 
            this.labelHorzDiv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHorzDiv.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelHorzDiv.Location = new System.Drawing.Point(-3, 26);
            this.labelHorzDiv.Name = "labelHorzDiv";
            this.labelHorzDiv.Size = new System.Drawing.Size(1106, 2);
            this.labelHorzDiv.TabIndex = 13;
            // 
            // lblSearchBy
            // 
            this.lblSearchBy.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchBy.Location = new System.Drawing.Point(595, 34);
            this.lblSearchBy.Name = "lblSearchBy";
            this.lblSearchBy.Size = new System.Drawing.Size(246, 38);
            this.lblSearchBy.TabIndex = 0;
            this.lblSearchBy.Text = "Search Criteria";
            this.lblSearchBy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblErrorState
            // 
            this.lblErrorState.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorState.Location = new System.Drawing.Point(330, 34);
            this.lblErrorState.Name = "lblErrorState";
            this.lblErrorState.Size = new System.Drawing.Size(123, 40);
            this.lblErrorState.TabIndex = 0;
            this.lblErrorState.Text = "Error Status";
            this.lblErrorState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMeterType
            // 
            this.lblMeterType.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMeterType.Location = new System.Drawing.Point(156, 34);
            this.lblMeterType.Name = "lblMeterType";
            this.lblMeterType.Size = new System.Drawing.Size(169, 38);
            this.lblMeterType.TabIndex = 0;
            this.lblMeterType.Text = "Meter Type";
            this.lblMeterType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSearchPCBAID
            // 
            this.txtSearchPCBAID.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchPCBAID.Location = new System.Drawing.Point(-1, 22);
            this.txtSearchPCBAID.Name = "txtSearchPCBAID";
            this.txtSearchPCBAID.Size = new System.Drawing.Size(147, 23);
            this.txtSearchPCBAID.TabIndex = 3;
            this.txtSearchPCBAID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSearchPCBAID.TextChanged += new System.EventHandler(this.txtSearchPCBAID_TextChanged);
            // 
            // txtSearchCustomer
            // 
            this.txtSearchCustomer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchCustomer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchCustomer.Location = new System.Drawing.Point(-1, 44);
            this.txtSearchCustomer.Name = "txtSearchCustomer";
            this.txtSearchCustomer.Size = new System.Drawing.Size(147, 23);
            this.txtSearchCustomer.TabIndex = 3;
            this.txtSearchCustomer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSearchCustomer.TextChanged += new System.EventHandler(this.txtSearchPCBAID_TextChanged);
            // 
            // chkListProdStage
            // 
            this.chkListProdStage.BackColor = System.Drawing.Color.AliceBlue;
            this.chkListProdStage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chkListProdStage.CheckOnClick = true;
            this.chkListProdStage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkListProdStage.FormattingEnabled = true;
            this.chkListProdStage.IntegralHeight = false;
            this.chkListProdStage.Location = new System.Drawing.Point(22, 70);
            this.chkListProdStage.Name = "chkListProdStage";
            this.chkListProdStage.Size = new System.Drawing.Size(128, 164);
            this.chkListProdStage.TabIndex = 4;
            this.chkListProdStage.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkListSource_ItemCheck);
            // 
            // chkListMeterType
            // 
            this.chkListMeterType.BackColor = System.Drawing.Color.AliceBlue;
            this.chkListMeterType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chkListMeterType.CheckOnClick = true;
            this.chkListMeterType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkListMeterType.FormattingEnabled = true;
            this.chkListMeterType.IntegralHeight = false;
            this.chkListMeterType.Location = new System.Drawing.Point(156, 70);
            this.chkListMeterType.Name = "chkListMeterType";
            this.chkListMeterType.Size = new System.Drawing.Size(169, 164);
            this.chkListMeterType.TabIndex = 4;
            this.chkListMeterType.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkListSource_ItemCheck);
            // 
            // chkListErrorStatus
            // 
            this.chkListErrorStatus.BackColor = System.Drawing.Color.AliceBlue;
            this.chkListErrorStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chkListErrorStatus.CheckOnClick = true;
            this.chkListErrorStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkListErrorStatus.FormattingEnabled = true;
            this.chkListErrorStatus.IntegralHeight = false;
            this.chkListErrorStatus.Location = new System.Drawing.Point(330, 70);
            this.chkListErrorStatus.Name = "chkListErrorStatus";
            this.chkListErrorStatus.Size = new System.Drawing.Size(123, 164);
            this.chkListErrorStatus.TabIndex = 4;
            this.chkListErrorStatus.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkListSource_ItemCheck);
            // 
            // panelFilters
            // 
            this.panelFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFilters.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panelFilters.Controls.Add(this.label11);
            this.panelFilters.Controls.Add(this.chkListShift);
            this.panelFilters.Controls.Add(this.labelHorzDiv2);
            this.panelFilters.Controls.Add(this.lblVertDiv);
            this.panelFilters.Controls.Add(this.chkListSearchBy);
            this.panelFilters.Controls.Add(this.labelHorzDiv3);
            this.panelFilters.Controls.Add(this.label1);
            this.panelFilters.Controls.Add(this.btnCollapse);
            this.panelFilters.Controls.Add(this.panelSearchText);
            this.panelFilters.Controls.Add(this.chkListSource);
            this.panelFilters.Controls.Add(this.txtRecordCount);
            this.panelFilters.Controls.Add(this.chkListErrorStatus);
            this.panelFilters.Controls.Add(this.chkListMeterType);
            this.panelFilters.Controls.Add(this.chkListProdStage);
            this.panelFilters.Controls.Add(this.lblProdStage);
            this.panelFilters.Controls.Add(this.lblMeterType);
            this.panelFilters.Controls.Add(this.lblErrorState);
            this.panelFilters.Controls.Add(this.lblSearchBy);
            this.panelFilters.Controls.Add(this.lblPCBSource);
            this.panelFilters.Controls.Add(this.lblLoggedUserID);
            this.panelFilters.Controls.Add(this.label2);
            this.panelFilters.Controls.Add(this.lblQueryDateTime);
            this.panelFilters.Controls.Add(this.label5);
            this.panelFilters.Controls.Add(this.chkUniquePCBAID);
            this.panelFilters.Controls.Add(this.chkShowZeroCountRecords);
            this.panelFilters.Location = new System.Drawing.Point(-4, 26);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Size = new System.Drawing.Size(1103, 254);
            this.panelFilters.TabIndex = 15;
            // 
            // labelHorzDiv2
            // 
            this.labelHorzDiv2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHorzDiv2.BackColor = System.Drawing.Color.SlateGray;
            this.labelHorzDiv2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelHorzDiv2.Location = new System.Drawing.Point(-1, 247);
            this.labelHorzDiv2.Name = "labelHorzDiv2";
            this.labelHorzDiv2.Size = new System.Drawing.Size(1105, 8);
            this.labelHorzDiv2.TabIndex = 14;
            // 
            // lblVertDiv
            // 
            this.lblVertDiv.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblVertDiv.Location = new System.Drawing.Point(468, 33);
            this.lblVertDiv.Name = "lblVertDiv";
            this.lblVertDiv.Size = new System.Drawing.Size(2, 214);
            this.lblVertDiv.TabIndex = 16;
            // 
            // labelHorzDiv3
            // 
            this.labelHorzDiv3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHorzDiv3.BackColor = System.Drawing.Color.SlateGray;
            this.labelHorzDiv3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelHorzDiv3.Location = new System.Drawing.Point(-1, 31);
            this.labelHorzDiv3.Name = "labelHorzDiv3";
            this.labelHorzDiv3.Size = new System.Drawing.Size(1105, 2);
            this.labelHorzDiv3.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(962, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 15);
            this.label1.TabIndex = 11;
            this.label1.Text = "Filters -";
            // 
            // btnCollapse
            // 
            this.btnCollapse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollapse.BackColor = System.Drawing.Color.SteelBlue;
            this.btnCollapse.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCollapse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCollapse.ForeColor = System.Drawing.Color.LightCyan;
            this.btnCollapse.Location = new System.Drawing.Point(1010, 5);
            this.btnCollapse.Name = "btnCollapse";
            this.btnCollapse.Size = new System.Drawing.Size(80, 22);
            this.btnCollapse.TabIndex = 10;
            this.btnCollapse.Text = "---------▼---------";
            this.btnCollapse.UseVisualStyleBackColor = false;
            this.btnCollapse.Click += new System.EventHandler(this.btnCollapse_Click);
            // 
            // panelSearchText
            // 
            this.panelSearchText.BackColor = System.Drawing.Color.AliceBlue;
            this.panelSearchText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSearchText.Controls.Add(this.txtSearchCustomer);
            this.panelSearchText.Controls.Add(this.txtSearchPCBAID);
            this.panelSearchText.Location = new System.Drawing.Point(696, 70);
            this.panelSearchText.Name = "panelSearchText";
            this.panelSearchText.Size = new System.Drawing.Size(145, 164);
            this.panelSearchText.TabIndex = 5;
            // 
            // chkListSource
            // 
            this.chkListSource.BackColor = System.Drawing.Color.AliceBlue;
            this.chkListSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chkListSource.CheckOnClick = true;
            this.chkListSource.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkListSource.FormattingEnabled = true;
            this.chkListSource.IntegralHeight = false;
            this.chkListSource.Location = new System.Drawing.Point(485, 70);
            this.chkListSource.Name = "chkListSource";
            this.chkListSource.Size = new System.Drawing.Size(104, 164);
            this.chkListSource.TabIndex = 4;
            this.chkListSource.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkListSource_ItemCheck);
            // 
            // lblProdStage
            // 
            this.lblProdStage.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProdStage.Location = new System.Drawing.Point(22, 34);
            this.lblProdStage.Name = "lblProdStage";
            this.lblProdStage.Size = new System.Drawing.Size(128, 40);
            this.lblProdStage.TabIndex = 0;
            this.lblProdStage.Text = "Production Stage";
            this.lblProdStage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPCBSource
            // 
            this.lblPCBSource.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPCBSource.Location = new System.Drawing.Point(485, 34);
            this.lblPCBSource.Name = "lblPCBSource";
            this.lblPCBSource.Size = new System.Drawing.Size(104, 38);
            this.lblPCBSource.TabIndex = 0;
            this.lblPCBSource.Text = "PCB Source";
            this.lblPCBSource.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLoggedUserID
            // 
            this.lblLoggedUserID.AutoSize = true;
            this.lblLoggedUserID.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblLoggedUserID.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblLoggedUserID.Location = new System.Drawing.Point(497, 7);
            this.lblLoggedUserID.Name = "lblLoggedUserID";
            this.lblLoggedUserID.Size = new System.Drawing.Size(29, 17);
            this.lblLoggedUserID.TabIndex = 0;
            this.lblLoggedUserID.Text = "ALL";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(311, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(192, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "LoggedUserID / LastRepairID : ";
            // 
            // lblQueryDateTime
            // 
            this.lblQueryDateTime.AutoSize = true;
            this.lblQueryDateTime.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblQueryDateTime.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblQueryDateTime.Location = new System.Drawing.Point(142, 6);
            this.lblQueryDateTime.Name = "lblQueryDateTime";
            this.lblQueryDateTime.Size = new System.Drawing.Size(169, 20);
            this.lblQueryDateTime.TabIndex = 0;
            this.lblQueryDateTime.Text = "12/12/2020 00:00:00 AM";
            // 
            // dgvRejectionList
            // 
            this.dgvRejectionList.AllowUserToAddRows = false;
            this.dgvRejectionList.AllowUserToDeleteRows = false;
            this.dgvRejectionList.AllowUserToOrderColumns = true;
            this.dgvRejectionList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRejectionList.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRejectionList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRejectionList.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvRejectionList.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.dgvRejectionList.Location = new System.Drawing.Point(2, 60);
            this.dgvRejectionList.MultiSelect = false;
            this.dgvRejectionList.Name = "dgvRejectionList";
            this.dgvRejectionList.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRejectionList.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvRejectionList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRejectionList.Size = new System.Drawing.Size(1091, 454);
            this.dgvRejectionList.TabIndex = 0;
            this.dgvRejectionList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRejectionList_CellDoubleClick);
            this.dgvRejectionList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvRejectionList_MouseClick);
            this.dgvRejectionList.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvRejectionList_CellPainting);
            // 
            // chkListSearchBy
            // 
            this.chkListSearchBy.BackColor = System.Drawing.Color.AliceBlue;
            this.chkListSearchBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chkListSearchBy.CheckOnClick = true;
            this.chkListSearchBy.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkListSearchBy.FormattingEnabled = true;
            this.chkListSearchBy.IntegralHeight = false;
            this.chkListSearchBy.Location = new System.Drawing.Point(595, 70);
            this.chkListSearchBy.Name = "chkListSearchBy";
            this.chkListSearchBy.Size = new System.Drawing.Size(102, 164);
            this.chkListSearchBy.TabIndex = 15;
            this.chkListSearchBy.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkListSource_ItemCheck);
            // 
            // frmErrorList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1095, 651);
            this.Controls.Add(this.groupBoxErrDetails);
            this.Controls.Add(this.labelHorzDiv);
            this.Controls.Add(this.panelFilters);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelDates);
            this.Controls.Add(this.dgvRejectionList);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(870, 690);
            this.Name = "frmErrorList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Meter Rejection List";
            this.Load += new System.EventHandler(this.FrmErrorList_Load);
            this.ResizeEnd += new System.EventHandler(this.frmErrorList_ResizeEnd);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panelDates.ResumeLayout(false);
            this.panelDates.PerformLayout();
            this.groupBoxErrDetails.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.panelSearchText.ResumeLayout(false);
            this.panelSearchText.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRejectionList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem displayRejectionListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewErrorTypesTableToolStripMenuItem;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.TextBox txtRecordCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtProbDesc;
        private System.Windows.Forms.TextBox txtParamName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripMenuItem updateEntryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savePDFToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ToolStripMenuItem insertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateRecordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertNewErrorRecordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateErrorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItemExcel;
        private System.Windows.Forms.ToolStripMenuItem liveChartToolStripMenuItem;
        private System.Windows.Forms.Panel panelDates;
        private System.Windows.Forms.ToolStripMenuItem deleteRejectionRecordToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkShowZeroCountRecords;
        protected System.Windows.Forms.TextBox txtErrorName;
        private System.Windows.Forms.CheckBox chkUniquePCBAID;
        private System.Windows.Forms.Panel groupBoxErrDetails;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rdbActionDate;
        private System.Windows.Forms.RadioButton rdbErrorDate;
        private System.Windows.Forms.Label labelHorzDiv;
        private System.Windows.Forms.Label lblSearchBy;
        private System.Windows.Forms.Label lblErrorState;
        private System.Windows.Forms.Label lblMeterType;
        private System.Windows.Forms.TextBox txtSearchPCBAID;
        private System.Windows.Forms.TextBox txtSearchCustomer;
        private System.Windows.Forms.CheckedListBox chkListProdStage;
        private System.Windows.Forms.CheckedListBox chkListMeterType;
        private System.Windows.Forms.CheckedListBox chkListErrorStatus;
        private System.Windows.Forms.CheckedListBox chkListShift;
        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.CheckedListBox chkListSource;
        private System.Windows.Forms.Label lblPCBSource;
        private System.Windows.Forms.Panel panelSearchText;
        private System.Windows.Forms.Button btnCollapse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelHorzDiv2;
        private System.Windows.Forms.Label labelHorzDiv3;
        private System.Windows.Forms.Label lblQueryDateTime;
        private System.Windows.Forms.ContextMenuStrip contextMenuUserID;
        private System.Windows.Forms.Label lblLoggedUserID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvRejectionList;
        private MyCheckedListBox chkListSearchBy;
        private System.Windows.Forms.Label lblVertDiv;
        private System.Windows.Forms.Label lblProdStage;
        private System.Windows.Forms.TextBox txtRejectCause;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtRejectAction;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label3;
        protected System.Windows.Forms.TextBox txtErrorDate;
        protected System.Windows.Forms.TextBox txtActionDate;
        private System.Windows.Forms.Label label4;
        protected System.Windows.Forms.TextBox txtLoggedUserID;
    }
}