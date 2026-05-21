namespace PMP3PHDLMS
{
    partial class frmActionPMP3PHDLMS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmActionPMP3PHDLMS));
            this.btnStart = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DGVParaLists = new System.Windows.Forms.DataGridView();
            this.colSNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colParaName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDefaultValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColMinVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColMaxValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.DLMSStas = new System.Windows.Forms.StatusStrip();
            this.stsReady = new System.Windows.Forms.ToolStripStatusLabel();
            this.dlmsCommStatusmsh = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblversion = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpinputs = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTestType = new System.Windows.Forms.TextBox();
            this.lblLastScan = new System.Windows.Forms.Label();
            this.txtLastScanID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMeterPCBAID = new System.Windows.Forms.TextBox();
            this.lblParaonTestType = new System.Windows.Forms.Label();
            this.txtPCBAID = new System.Windows.Forms.TextBox();
            this.pbExecutionStatus = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVParaLists)).BeginInit();
            this.DLMSStas.SuspendLayout();
            this.grpinputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbExecutionStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.SystemColors.Control;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(28, 412);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(140, 65);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "START TEST";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DGVParaLists);
            this.groupBox1.Location = new System.Drawing.Point(18, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(628, 845);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            // 
            // DGVParaLists
            // 
            this.DGVParaLists.AllowUserToAddRows = false;
            this.DGVParaLists.AllowUserToDeleteRows = false;
            this.DGVParaLists.AllowUserToResizeColumns = false;
            this.DGVParaLists.AllowUserToResizeRows = false;
            this.DGVParaLists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVParaLists.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSNO,
            this.colParaName,
            this.colStatus,
            this.colDefaultValue,
            this.ColMinVal,
            this.ColMaxValue,
            this.colRemarks});
            this.DGVParaLists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGVParaLists.Location = new System.Drawing.Point(4, 24);
            this.DGVParaLists.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DGVParaLists.MultiSelect = false;
            this.DGVParaLists.Name = "DGVParaLists";
            this.DGVParaLists.ReadOnly = true;
            this.DGVParaLists.RowHeadersWidth = 62;
            this.DGVParaLists.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGVParaLists.Size = new System.Drawing.Size(620, 816);
            this.DGVParaLists.TabIndex = 0;
            // 
            // colSNO
            // 
            this.colSNO.HeaderText = "S. No.";
            this.colSNO.MinimumWidth = 8;
            this.colSNO.Name = "colSNO";
            this.colSNO.ReadOnly = true;
            this.colSNO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSNO.Width = 50;
            // 
            // colParaName
            // 
            this.colParaName.HeaderText = "Test Parameters Name";
            this.colParaName.MinimumWidth = 8;
            this.colParaName.Name = "colParaName";
            this.colParaName.ReadOnly = true;
            this.colParaName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colParaName.Width = 230;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Status";
            this.colStatus.MinimumWidth = 8;
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colStatus.Width = 150;
            // 
            // colDefaultValue
            // 
            this.colDefaultValue.HeaderText = "Default Value";
            this.colDefaultValue.MinimumWidth = 8;
            this.colDefaultValue.Name = "colDefaultValue";
            this.colDefaultValue.ReadOnly = true;
            this.colDefaultValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDefaultValue.Width = 150;
            // 
            // ColMinVal
            // 
            this.ColMinVal.HeaderText = "Min. Range";
            this.ColMinVal.MinimumWidth = 8;
            this.ColMinVal.Name = "ColMinVal";
            this.ColMinVal.ReadOnly = true;
            this.ColMinVal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColMinVal.Width = 70;
            // 
            // ColMaxValue
            // 
            this.ColMaxValue.HeaderText = "Max. Range";
            this.ColMaxValue.MinimumWidth = 8;
            this.ColMaxValue.Name = "ColMaxValue";
            this.ColMaxValue.ReadOnly = true;
            this.ColMaxValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColMaxValue.Width = 70;
            // 
            // colRemarks
            // 
            this.colRemarks.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colRemarks.HeaderText = "Execution Remarks";
            this.colRemarks.MinimumWidth = 8;
            this.colRemarks.Name = "colRemarks";
            this.colRemarks.ReadOnly = true;
            this.colRemarks.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colRemarks.Width = 138;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(177, 412);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(140, 65);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // DLMSStas
            // 
            this.DLMSStas.AutoSize = false;
            this.DLMSStas.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.DLMSStas.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stsReady,
            this.dlmsCommStatusmsh,
            this.lblversion});
            this.DLMSStas.Location = new System.Drawing.Point(0, 860);
            this.DLMSStas.Name = "DLMSStas";
            this.DLMSStas.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.DLMSStas.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DLMSStas.Size = new System.Drawing.Size(1005, 34);
            this.DLMSStas.TabIndex = 20;
            this.DLMSStas.Text = "statusStrip1";
            // 
            // stsReady
            // 
            this.stsReady.BackColor = System.Drawing.SystemColors.Control;
            this.stsReady.Name = "stsReady";
            this.stsReady.Size = new System.Drawing.Size(179, 27);
            this.stsReady.Text = "toolStripStatusLabel1";
            // 
            // dlmsCommStatusmsh
            // 
            this.dlmsCommStatusmsh.AutoSize = false;
            this.dlmsCommStatusmsh.BackColor = System.Drawing.SystemColors.Control;
            this.dlmsCommStatusmsh.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dlmsCommStatusmsh.ForeColor = System.Drawing.Color.Green;
            this.dlmsCommStatusmsh.Name = "dlmsCommStatusmsh";
            this.dlmsCommStatusmsh.Size = new System.Drawing.Size(43, 27);
            this.dlmsCommStatusmsh.Text = "Space";
            // 
            // lblversion
            // 
            this.lblversion.BackColor = System.Drawing.SystemColors.Control;
            this.lblversion.Name = "lblversion";
            this.lblversion.Size = new System.Drawing.Size(179, 27);
            this.lblversion.Text = "toolStripStatusLabel2";
            // 
            // grpinputs
            // 
            this.grpinputs.Controls.Add(this.label1);
            this.grpinputs.Controls.Add(this.txtTestType);
            this.grpinputs.Controls.Add(this.lblLastScan);
            this.grpinputs.Controls.Add(this.txtLastScanID);
            this.grpinputs.Controls.Add(this.label3);
            this.grpinputs.Controls.Add(this.txtCustomer);
            this.grpinputs.Controls.Add(this.label2);
            this.grpinputs.Controls.Add(this.btnClose);
            this.grpinputs.Controls.Add(this.btnStart);
            this.grpinputs.Controls.Add(this.txtMeterPCBAID);
            this.grpinputs.Controls.Add(this.lblParaonTestType);
            this.grpinputs.Controls.Add(this.txtPCBAID);
            this.grpinputs.Location = new System.Drawing.Point(656, 2);
            this.grpinputs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpinputs.Name = "grpinputs";
            this.grpinputs.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpinputs.Size = new System.Drawing.Size(334, 495);
            this.grpinputs.TabIndex = 21;
            this.grpinputs.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 22);
            this.label1.TabIndex = 60;
            this.label1.Text = "Test Type";
            // 
            // txtTestType
            // 
            this.txtTestType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTestType.Location = new System.Drawing.Point(9, 62);
            this.txtTestType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTestType.MaxLength = 25;
            this.txtTestType.Name = "txtTestType";
            this.txtTestType.ReadOnly = true;
            this.txtTestType.Size = new System.Drawing.Size(306, 28);
            this.txtTestType.TabIndex = 59;
            // 
            // lblLastScan
            // 
            this.lblLastScan.AutoSize = true;
            this.lblLastScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastScan.Location = new System.Drawing.Point(14, 332);
            this.lblLastScan.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLastScan.Name = "lblLastScan";
            this.lblLastScan.Size = new System.Drawing.Size(166, 22);
            this.lblLastScan.TabIndex = 58;
            this.lblLastScan.Text = "Last Scan PCBA ID";
            // 
            // txtLastScanID
            // 
            this.txtLastScanID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastScanID.Location = new System.Drawing.Point(9, 363);
            this.txtLastScanID.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtLastScanID.MaxLength = 16;
            this.txtLastScanID.Name = "txtLastScanID";
            this.txtLastScanID.ReadOnly = true;
            this.txtLastScanID.Size = new System.Drawing.Size(306, 35);
            this.txtLastScanID.TabIndex = 57;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 98);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 22);
            this.label3.TabIndex = 56;
            this.label3.Text = "Customer Name";
            // 
            // txtCustomer
            // 
            this.txtCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomer.Location = new System.Drawing.Point(9, 137);
            this.txtCustomer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCustomer.MaxLength = 25;
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(306, 28);
            this.txtCustomer.TabIndex = 55;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 265);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 22);
            this.label2.TabIndex = 22;
            this.label2.Text = "Meter PCBA ID";
            // 
            // txtMeterPCBAID
            // 
            this.txtMeterPCBAID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMeterPCBAID.Location = new System.Drawing.Point(9, 292);
            this.txtMeterPCBAID.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMeterPCBAID.MaxLength = 16;
            this.txtMeterPCBAID.Name = "txtMeterPCBAID";
            this.txtMeterPCBAID.ReadOnly = true;
            this.txtMeterPCBAID.Size = new System.Drawing.Size(306, 28);
            this.txtMeterPCBAID.TabIndex = 21;
            // 
            // lblParaonTestType
            // 
            this.lblParaonTestType.AutoSize = true;
            this.lblParaonTestType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParaonTestType.Location = new System.Drawing.Point(12, 180);
            this.lblParaonTestType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblParaonTestType.Name = "lblParaonTestType";
            this.lblParaonTestType.Size = new System.Drawing.Size(178, 29);
            this.lblParaonTestType.TabIndex = 20;
            this.lblParaonTestType.Text = "Scan PCBA ID";
            // 
            // txtPCBAID
            // 
            this.txtPCBAID.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPCBAID.Location = new System.Drawing.Point(9, 215);
            this.txtPCBAID.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPCBAID.MaxLength = 13;
            this.txtPCBAID.Name = "txtPCBAID";
            this.txtPCBAID.Size = new System.Drawing.Size(306, 40);
            this.txtPCBAID.TabIndex = 0;
            this.txtPCBAID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPCBAID_KeyPress);
            this.txtPCBAID.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMeterID_KeyUp);
            // 
            // pbExecutionStatus
            // 
            this.pbExecutionStatus.Image = global::PMP3PHDLMS.Properties.Resources.ExecutionWait;
            this.pbExecutionStatus.Location = new System.Drawing.Point(736, 506);
            this.pbExecutionStatus.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbExecutionStatus.Name = "pbExecutionStatus";
            this.pbExecutionStatus.Size = new System.Drawing.Size(170, 177);
            this.pbExecutionStatus.TabIndex = 23;
            this.pbExecutionStatus.TabStop = false;
            // 
            // frmActionPMP3PHDLMS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1005, 894);
            this.Controls.Add(this.pbExecutionStatus);
            this.Controls.Add(this.grpinputs);
            this.Controls.Add(this.DLMSStas);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmActionPMP3PHDLMS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test Run";
            this.Load += new System.EventHandler(this.EMS_TEST_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGVParaLists)).EndInit();
            this.DLMSStas.ResumeLayout(false);
            this.DLMSStas.PerformLayout();
            this.grpinputs.ResumeLayout(false);
            this.grpinputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbExecutionStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView DGVParaLists;
        private System.Windows.Forms.StatusStrip DLMSStas;
        private System.Windows.Forms.ToolStripStatusLabel dlmsCommStatusmsh;
        private System.Windows.Forms.ToolStripStatusLabel stsReady;
        private System.Windows.Forms.ToolStripStatusLabel lblversion;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox grpinputs;
        private System.Windows.Forms.Label lblParaonTestType;
        private System.Windows.Forms.TextBox txtPCBAID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMeterPCBAID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParaName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDefaultValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMinVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMaxValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemarks;
        private System.Windows.Forms.PictureBox pbExecutionStatus;
        private System.Windows.Forms.Label lblLastScan;
        private System.Windows.Forms.TextBox txtLastScanID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTestType;
    }
}