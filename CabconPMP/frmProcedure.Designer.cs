namespace CabconPMP
{
    partial class frmProcedure
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbParametersName = new System.Windows.Forms.ComboBox();
            this.chkStatus = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMaxValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMinValue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDefaultValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DGVProcedure = new System.Windows.Forms.DataGridView();
            this.SNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParametersName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDefaultValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColMinVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaxValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblNew = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.lblAddUpdate = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.lblEdit = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.lblInsert = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.lblDelete = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblSave = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.lblClose = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtProcedureName = new System.Windows.Forms.TextBox();
            this.cmbProcedureType = new System.Windows.Forms.ComboBox();
            this.cmbProgramList = new System.Windows.Forms.ComboBox();
            this.cmbMeterType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblProcedureName = new System.Windows.Forms.Label();
            this.grpMeterIDRange = new System.Windows.Forms.GroupBox();
            this.cmbMidDegits = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.txtMeterIDTO = new System.Windows.Forms.TextBox();
            this.txtMeterIDFrom = new System.Windows.Forms.TextBox();
            this.txtMeterIDPrefix = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.grpOtherSettings = new System.Windows.Forms.GroupBox();
            this.chkExecutionWithoutTraveler = new System.Windows.Forms.CheckBox();
            this.chkDisablemanualScan = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVProcedure)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpMeterIDRange.SuspendLayout();
            this.grpOtherSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbParametersName);
            this.groupBox1.Controls.Add(this.chkStatus);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtMaxValue);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtMinValue);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtDefaultValue);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(6, 109);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(938, 57);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // cmbParametersName
            // 
            this.cmbParametersName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParametersName.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbParametersName.FormattingEnabled = true;
            this.cmbParametersName.Location = new System.Drawing.Point(6, 31);
            this.cmbParametersName.Name = "cmbParametersName";
            this.cmbParametersName.Size = new System.Drawing.Size(269, 21);
            this.cmbParametersName.TabIndex = 23;
            this.cmbParametersName.SelectedIndexChanged += new System.EventHandler(this.cmbParametersName_SelectedIndexChanged);
            this.cmbParametersName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbParametersName_KeyPress);
            // 
            // chkStatus
            // 
            this.chkStatus.AutoSize = true;
            this.chkStatus.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chkStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStatus.Location = new System.Drawing.Point(889, 19);
            this.chkStatus.Name = "chkStatus";
            this.chkStatus.Size = new System.Drawing.Size(47, 31);
            this.chkStatus.TabIndex = 10;
            this.chkStatus.Text = "Status";
            this.chkStatus.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(752, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Max. Value";
            // 
            // txtMaxValue
            // 
            this.txtMaxValue.Location = new System.Drawing.Point(684, 31);
            this.txtMaxValue.MaxLength = 49;
            this.txtMaxValue.Name = "txtMaxValue";
            this.txtMaxValue.Size = new System.Drawing.Size(201, 20);
            this.txtMaxValue.TabIndex = 8;
            this.txtMaxValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMaxValue_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(540, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Min. Value";
            // 
            // txtMinValue
            // 
            this.txtMinValue.Location = new System.Drawing.Point(482, 31);
            this.txtMinValue.MaxLength = 49;
            this.txtMinValue.Name = "txtMinValue";
            this.txtMinValue.Size = new System.Drawing.Size(201, 20);
            this.txtMinValue.TabIndex = 6;
            this.txtMinValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMinValue_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(351, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Default Value";
            // 
            // txtDefaultValue
            // 
            this.txtDefaultValue.Location = new System.Drawing.Point(279, 31);
            this.txtDefaultValue.MaxLength = 140;
            this.txtDefaultValue.Name = "txtDefaultValue";
            this.txtDefaultValue.Size = new System.Drawing.Size(201, 20);
            this.txtDefaultValue.TabIndex = 4;
            this.txtDefaultValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDefaultValue_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(76, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Parameter Name";
            // 
            // DGVProcedure
            // 
            this.DGVProcedure.AllowUserToAddRows = false;
            this.DGVProcedure.AllowUserToDeleteRows = false;
            this.DGVProcedure.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVProcedure.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SNO,
            this.ParametersName,
            this.colDefaultValue,
            this.ColMinVal,
            this.colMaxValue,
            this.colStatus});
            this.DGVProcedure.Location = new System.Drawing.Point(6, 170);
            this.DGVProcedure.MultiSelect = false;
            this.DGVProcedure.Name = "DGVProcedure";
            this.DGVProcedure.ReadOnly = true;
            this.DGVProcedure.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGVProcedure.Size = new System.Drawing.Size(934, 329);
            this.DGVProcedure.TabIndex = 1;
            // 
            // SNO
            // 
            this.SNO.HeaderText = "S. No.";
            this.SNO.Name = "SNO";
            this.SNO.ReadOnly = true;
            this.SNO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SNO.Width = 50;
            // 
            // ParametersName
            // 
            this.ParametersName.HeaderText = "Parameters Name";
            this.ParametersName.Name = "ParametersName";
            this.ParametersName.ReadOnly = true;
            this.ParametersName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ParametersName.Width = 250;
            // 
            // colDefaultValue
            // 
            this.colDefaultValue.HeaderText = "Default Value";
            this.colDefaultValue.Name = "colDefaultValue";
            this.colDefaultValue.ReadOnly = true;
            this.colDefaultValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDefaultValue.Width = 160;
            // 
            // ColMinVal
            // 
            this.ColMinVal.HeaderText = "Min Value";
            this.ColMinVal.Name = "ColMinVal";
            this.ColMinVal.ReadOnly = true;
            this.ColMinVal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColMinVal.Width = 180;
            // 
            // colMaxValue
            // 
            this.colMaxValue.HeaderText = "Max Value";
            this.colMaxValue.Name = "colMaxValue";
            this.colMaxValue.ReadOnly = true;
            this.colMaxValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colMaxValue.Width = 180;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 50;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblNew,
            this.toolStripSeparator7,
            this.lblAddUpdate,
            this.toolStripSeparator8,
            this.lblEdit,
            this.toolStripSeparator2,
            this.lblInsert,
            this.toolStripSeparator5,
            this.lblDelete,
            this.toolStripSeparator1,
            this.lblSave,
            this.toolStripSeparator4,
            this.lblClose,
            this.toolStripSeparator3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(947, 25);
            this.toolStrip1.TabIndex = 17;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lblNew
            // 
            this.lblNew.Name = "lblNew";
            this.lblNew.Size = new System.Drawing.Size(34, 22);
            this.lblNew.Text = "New";
            this.lblNew.ToolTipText = "To Create New Test Point";
            this.lblNew.Click += new System.EventHandler(this.lblNew_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // lblAddUpdate
            // 
            this.lblAddUpdate.Name = "lblAddUpdate";
            this.lblAddUpdate.Size = new System.Drawing.Size(80, 22);
            this.lblAddUpdate.Text = "Add/Update";
            this.lblAddUpdate.ToolTipText = "To Add or Update Test Point";
            this.lblAddUpdate.Click += new System.EventHandler(this.lblAddUpdate_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // lblEdit
            // 
            this.lblEdit.Name = "lblEdit";
            this.lblEdit.Size = new System.Drawing.Size(71, 22);
            this.lblEdit.Text = "Get Details";
            this.lblEdit.ToolTipText = "To Get Test point Details for Edit ";
            this.lblEdit.Click += new System.EventHandler(this.lblEdit_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // lblInsert
            // 
            this.lblInsert.Name = "lblInsert";
            this.lblInsert.Size = new System.Drawing.Size(40, 22);
            this.lblInsert.Text = "Insert";
            this.lblInsert.ToolTipText = "To Insert Test point above the Selected point";
            this.lblInsert.Click += new System.EventHandler(this.lblInsert_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // lblDelete
            // 
            this.lblDelete.Name = "lblDelete";
            this.lblDelete.Size = new System.Drawing.Size(45, 22);
            this.lblDelete.Text = "Delete";
            this.lblDelete.ToolTipText = "To Delete Selected Test point";
            this.lblDelete.Click += new System.EventHandler(this.lblDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // lblSave
            // 
            this.lblSave.Name = "lblSave";
            this.lblSave.Size = new System.Drawing.Size(35, 22);
            this.lblSave.Text = "Save";
            this.lblSave.ToolTipText = "To Save Test Procedure";
            this.lblSave.Click += new System.EventHandler(this.lblSave_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // lblClose
            // 
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(40, 22);
            this.lblClose.Text = "Close";
            this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Meter Type";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(207, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Test Type";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtProcedureName);
            this.groupBox2.Controls.Add(this.cmbProcedureType);
            this.groupBox2.Controls.Add(this.cmbProgramList);
            this.groupBox2.Controls.Add(this.cmbMeterType);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(6, 23);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(624, 57);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(371, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "Procedure Name";
            // 
            // txtProcedureName
            // 
            this.txtProcedureName.Location = new System.Drawing.Point(304, 29);
            this.txtProcedureName.MaxLength = 40;
            this.txtProcedureName.Name = "txtProcedureName";
            this.txtProcedureName.Size = new System.Drawing.Size(180, 20);
            this.txtProcedureName.TabIndex = 24;
            this.txtProcedureName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtProcedureName_KeyPress);
            // 
            // cmbProcedureType
            // 
            this.cmbProcedureType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProcedureType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbProcedureType.FormattingEnabled = true;
            this.cmbProcedureType.Location = new System.Drawing.Point(174, 28);
            this.cmbProcedureType.Name = "cmbProcedureType";
            this.cmbProcedureType.Size = new System.Drawing.Size(124, 21);
            this.cmbProcedureType.TabIndex = 24;
            this.cmbProcedureType.SelectedIndexChanged += new System.EventHandler(this.cmbProcedureType_SelectedIndexChanged);
            this.cmbProcedureType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbProsedureName_KeyPress);
            // 
            // cmbProgramList
            // 
            this.cmbProgramList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProgramList.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbProgramList.FormattingEnabled = true;
            this.cmbProgramList.Location = new System.Drawing.Point(490, 27);
            this.cmbProgramList.Name = "cmbProgramList";
            this.cmbProgramList.Size = new System.Drawing.Size(124, 21);
            this.cmbProgramList.TabIndex = 23;
            this.cmbProgramList.SelectionChangeCommitted += new System.EventHandler(this.cmbProgramList_SelectionChangeCommitted);
            this.cmbProgramList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbProgramList_KeyPress);
            // 
            // cmbMeterType
            // 
            this.cmbMeterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMeterType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbMeterType.FormattingEnabled = true;
            this.cmbMeterType.Location = new System.Drawing.Point(6, 27);
            this.cmbMeterType.Name = "cmbMeterType";
            this.cmbMeterType.Size = new System.Drawing.Size(162, 21);
            this.cmbMeterType.TabIndex = 22;
            this.cmbMeterType.SelectionChangeCommitted += new System.EventHandler(this.cmbMeterType_SelectionChangeCommitted);
            this.cmbMeterType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbMeterType_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(490, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(124, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Program File List (*. EXE)";
            // 
            // lblProcedureName
            // 
            this.lblProcedureName.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lblProcedureName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcedureName.Location = new System.Drawing.Point(800, 2);
            this.lblProcedureName.Name = "lblProcedureName";
            this.lblProcedureName.Size = new System.Drawing.Size(129, 18);
            this.lblProcedureName.TabIndex = 22;
            this.lblProcedureName.Text = "Procedure Name";
            this.lblProcedureName.Visible = false;
            // 
            // grpMeterIDRange
            // 
            this.grpMeterIDRange.Controls.Add(this.cmbMidDegits);
            this.grpMeterIDRange.Controls.Add(this.label10);
            this.grpMeterIDRange.Controls.Add(this.label11);
            this.grpMeterIDRange.Controls.Add(this.lblFrom);
            this.grpMeterIDRange.Controls.Add(this.txtMeterIDTO);
            this.grpMeterIDRange.Controls.Add(this.txtMeterIDFrom);
            this.grpMeterIDRange.Controls.Add(this.txtMeterIDPrefix);
            this.grpMeterIDRange.Controls.Add(this.label8);
            this.grpMeterIDRange.Location = new System.Drawing.Point(636, 23);
            this.grpMeterIDRange.Name = "grpMeterIDRange";
            this.grpMeterIDRange.Size = new System.Drawing.Size(308, 90);
            this.grpMeterIDRange.TabIndex = 23;
            this.grpMeterIDRange.TabStop = false;
            this.grpMeterIDRange.Text = "Meter ID Range";
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
            this.cmbMidDegits.Location = new System.Drawing.Point(6, 32);
            this.cmbMidDegits.Name = "cmbMidDegits";
            this.cmbMidDegits.Size = new System.Drawing.Size(45, 21);
            this.cmbMidDegits.TabIndex = 37;
            this.cmbMidDegits.SelectedIndexChanged += new System.EventHandler(this.cmbMidDegits_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.DarkRed;
            this.label10.Location = new System.Drawing.Point(9, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 13);
            this.label10.TabIndex = 36;
            this.label10.Text = "Length";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.DarkRed;
            this.label11.Location = new System.Drawing.Point(243, 18);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(20, 13);
            this.label11.TabIndex = 35;
            this.label11.Text = "To";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.ForeColor = System.Drawing.Color.DarkRed;
            this.lblFrom.Location = new System.Drawing.Point(146, 18);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(30, 13);
            this.lblFrom.TabIndex = 34;
            this.lblFrom.Text = "From";
            // 
            // txtMeterIDTO
            // 
            this.txtMeterIDTO.BackColor = System.Drawing.Color.White;
            this.txtMeterIDTO.Location = new System.Drawing.Point(224, 32);
            this.txtMeterIDTO.MaxLength = 16;
            this.txtMeterIDTO.Name = "txtMeterIDTO";
            this.txtMeterIDTO.Size = new System.Drawing.Size(80, 20);
            this.txtMeterIDTO.TabIndex = 33;
            this.txtMeterIDTO.Leave += new System.EventHandler(this.txtMeterIDTO_Leave);
            this.txtMeterIDTO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMeterIDTO_KeyPress);
            // 
            // txtMeterIDFrom
            // 
            this.txtMeterIDFrom.BackColor = System.Drawing.Color.White;
            this.txtMeterIDFrom.Location = new System.Drawing.Point(140, 33);
            this.txtMeterIDFrom.MaxLength = 16;
            this.txtMeterIDFrom.Name = "txtMeterIDFrom";
            this.txtMeterIDFrom.Size = new System.Drawing.Size(80, 20);
            this.txtMeterIDFrom.TabIndex = 32;
            this.txtMeterIDFrom.Leave += new System.EventHandler(this.txtMeterIDFrom_Leave);
            this.txtMeterIDFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMeterIDFrom_KeyPress);
            // 
            // txtMeterIDPrefix
            // 
            this.txtMeterIDPrefix.BackColor = System.Drawing.Color.White;
            this.txtMeterIDPrefix.Location = new System.Drawing.Point(54, 34);
            this.txtMeterIDPrefix.MaxLength = 12;
            this.txtMeterIDPrefix.Name = "txtMeterIDPrefix";
            this.txtMeterIDPrefix.Size = new System.Drawing.Size(82, 20);
            this.txtMeterIDPrefix.TabIndex = 31;
            this.txtMeterIDPrefix.Leave += new System.EventHandler(this.txtMeterIDPrefix_Leave);
            this.txtMeterIDPrefix.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMeterIDPrefix_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.DarkRed;
            this.label8.Location = new System.Drawing.Point(66, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 30;
            this.label8.Text = "Pre-Fix";
            // 
            // grpOtherSettings
            // 
            this.grpOtherSettings.Controls.Add(this.chkExecutionWithoutTraveler);
            this.grpOtherSettings.Controls.Add(this.chkDisablemanualScan);
            this.grpOtherSettings.Location = new System.Drawing.Point(6, 80);
            this.grpOtherSettings.Name = "grpOtherSettings";
            this.grpOtherSettings.Size = new System.Drawing.Size(624, 34);
            this.grpOtherSettings.TabIndex = 24;
            this.grpOtherSettings.TabStop = false;
            this.grpOtherSettings.Text = "others Setting";
            // 
            // chkExecutionWithoutTraveler
            // 
            this.chkExecutionWithoutTraveler.AutoSize = true;
            this.chkExecutionWithoutTraveler.Location = new System.Drawing.Point(304, 11);
            this.chkExecutionWithoutTraveler.Name = "chkExecutionWithoutTraveler";
            this.chkExecutionWithoutTraveler.Size = new System.Drawing.Size(194, 17);
            this.chkExecutionWithoutTraveler.TabIndex = 1;
            this.chkExecutionWithoutTraveler.Text = "Enable Execution Without Traveler ";
            this.chkExecutionWithoutTraveler.UseVisualStyleBackColor = true;
            this.chkExecutionWithoutTraveler.Click += new System.EventHandler(this.chkExecutionWithoutTraveler_Click);
            // 
            // chkDisablemanualScan
            // 
            this.chkDisablemanualScan.AutoSize = true;
            this.chkDisablemanualScan.Location = new System.Drawing.Point(94, 11);
            this.chkDisablemanualScan.Name = "chkDisablemanualScan";
            this.chkDisablemanualScan.Size = new System.Drawing.Size(180, 17);
            this.chkDisablemanualScan.TabIndex = 0;
            this.chkDisablemanualScan.Text = "Enable Execution Without Scan ";
            this.chkDisablemanualScan.UseVisualStyleBackColor = true;
            // 
            // frmProcedure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 511);
            this.ControlBox = false;
            this.Controls.Add(this.grpOtherSettings);
            this.Controls.Add(this.grpMeterIDRange);
            this.Controls.Add(this.lblProcedureName);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.DGVProcedure);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProcedure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Procedure";
            this.Load += new System.EventHandler(this.SM110frmProcedure_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVProcedure)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpMeterIDRange.ResumeLayout(false);
            this.grpMeterIDRange.PerformLayout();
            this.grpOtherSettings.ResumeLayout(false);
            this.grpOtherSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView DGVProcedure;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMaxValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMinValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDefaultValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel lblNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel lblAddUpdate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripLabel lblSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel lblClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbMeterType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbParametersName;
        private System.Windows.Forms.ComboBox cmbProcedureType;
        private System.Windows.Forms.ComboBox cmbProgramList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel lblDelete;
        private System.Windows.Forms.CheckBox chkStatus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtProcedureName;
        private System.Windows.Forms.Label lblProcedureName;
        private System.Windows.Forms.ToolStripLabel lblEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel lblInsert;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.DataGridViewTextBoxColumn SNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParametersName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDefaultValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMinVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaxValue;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colStatus;
        private System.Windows.Forms.GroupBox grpMeterIDRange;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.TextBox txtMeterIDTO;
        private System.Windows.Forms.TextBox txtMeterIDFrom;
        private System.Windows.Forms.TextBox txtMeterIDPrefix;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbMidDegits;
        private System.Windows.Forms.GroupBox grpOtherSettings;
        private System.Windows.Forms.CheckBox chkDisablemanualScan;
        private System.Windows.Forms.CheckBox chkExecutionWithoutTraveler;
    }
}