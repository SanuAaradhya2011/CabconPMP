namespace PMPE150MICROSTARDLMS
{
    partial class frmActionMicroStarDLMS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmActionMicroStarDLMS));
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
            this.btnStart.Location = new System.Drawing.Point(19, 264);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(93, 42);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "START TEST";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DGVParaLists);
            this.groupBox1.Location = new System.Drawing.Point(12, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(419, 549);
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
            this.DGVParaLists.Location = new System.Drawing.Point(3, 16);
            this.DGVParaLists.MultiSelect = false;
            this.DGVParaLists.Name = "DGVParaLists";
            this.DGVParaLists.ReadOnly = true;
            this.DGVParaLists.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGVParaLists.Size = new System.Drawing.Size(413, 530);
            this.DGVParaLists.TabIndex = 0;
            // 
            // colSNO
            // 
            this.colSNO.HeaderText = "S. No.";
            this.colSNO.Name = "colSNO";
            this.colSNO.ReadOnly = true;
            this.colSNO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSNO.Width = 45;
            // 
            // colParaName
            // 
            this.colParaName.HeaderText = "Test Parameters Name";
            this.colParaName.Name = "colParaName";
            this.colParaName.ReadOnly = true;
            this.colParaName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colParaName.Width = 230;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colDefaultValue
            // 
            this.colDefaultValue.HeaderText = "Default Value";
            this.colDefaultValue.Name = "colDefaultValue";
            this.colDefaultValue.ReadOnly = true;
            this.colDefaultValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColMinVal
            // 
            this.ColMinVal.HeaderText = "Min. Range";
            this.ColMinVal.Name = "ColMinVal";
            this.ColMinVal.ReadOnly = true;
            this.ColMinVal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColMinVal.Width = 70;
            // 
            // ColMaxValue
            // 
            this.ColMaxValue.HeaderText = "Max. Range";
            this.ColMaxValue.Name = "ColMaxValue";
            this.ColMaxValue.ReadOnly = true;
            this.ColMaxValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColMaxValue.Width = 70;
            // 
            // colRemarks
            // 
            this.colRemarks.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colRemarks.HeaderText = "Execution Remarks";
            this.colRemarks.Name = "colRemarks";
            this.colRemarks.ReadOnly = true;
            this.colRemarks.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colRemarks.Width = 95;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(118, 264);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(93, 42);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // DLMSStas
            // 
            this.DLMSStas.AutoSize = false;
            this.DLMSStas.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stsReady,
            this.dlmsCommStatusmsh,
            this.lblversion});
            this.DLMSStas.Location = new System.Drawing.Point(0, 554);
            this.DLMSStas.Name = "DLMSStas";
            this.DLMSStas.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DLMSStas.Size = new System.Drawing.Size(674, 22);
            this.DLMSStas.TabIndex = 20;
            this.DLMSStas.Text = "statusStrip1";
            // 
            // stsReady
            // 
            this.stsReady.BackColor = System.Drawing.SystemColors.Control;
            this.stsReady.Name = "stsReady";
            this.stsReady.Size = new System.Drawing.Size(118, 17);
            this.stsReady.Text = "toolStripStatusLabel1";
            // 
            // dlmsCommStatusmsh
            // 
            this.dlmsCommStatusmsh.AutoSize = false;
            this.dlmsCommStatusmsh.BackColor = System.Drawing.SystemColors.Control;
            this.dlmsCommStatusmsh.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dlmsCommStatusmsh.ForeColor = System.Drawing.Color.Green;
            this.dlmsCommStatusmsh.Name = "dlmsCommStatusmsh";
            this.dlmsCommStatusmsh.Size = new System.Drawing.Size(43, 17);
            this.dlmsCommStatusmsh.Text = "Space";
            // 
            // lblversion
            // 
            this.lblversion.BackColor = System.Drawing.SystemColors.Control;
            this.lblversion.Name = "lblversion";
            this.lblversion.Size = new System.Drawing.Size(118, 17);
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
            this.grpinputs.Controls.Add(this.txtMeterPCBAID);
            this.grpinputs.Controls.Add(this.lblParaonTestType);
            this.grpinputs.Controls.Add(this.txtPCBAID);
            this.grpinputs.Controls.Add(this.btnStart);
            this.grpinputs.Controls.Add(this.btnClose);
            this.grpinputs.Location = new System.Drawing.Point(437, 1);
            this.grpinputs.Name = "grpinputs";
            this.grpinputs.Size = new System.Drawing.Size(223, 322);
            this.grpinputs.TabIndex = 21;
            this.grpinputs.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 15);
            this.label1.TabIndex = 62;
            this.label1.Text = "Test Type";
            // 
            // txtTestType
            // 
            this.txtTestType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTestType.Location = new System.Drawing.Point(6, 36);
            this.txtTestType.MaxLength = 25;
            this.txtTestType.Name = "txtTestType";
            this.txtTestType.ReadOnly = true;
            this.txtTestType.Size = new System.Drawing.Size(205, 21);
            this.txtTestType.TabIndex = 61;
            // 
            // lblLastScan
            // 
            this.lblLastScan.AutoSize = true;
            this.lblLastScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastScan.Location = new System.Drawing.Point(9, 212);
            this.lblLastScan.Name = "lblLastScan";
            this.lblLastScan.Size = new System.Drawing.Size(110, 15);
            this.lblLastScan.TabIndex = 60;
            this.lblLastScan.Text = "Last Scan PCBA ID";
            // 
            // txtLastScanID
            // 
            this.txtLastScanID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastScanID.Location = new System.Drawing.Point(6, 232);
            this.txtLastScanID.MaxLength = 16;
            this.txtLastScanID.Name = "txtLastScanID";
            this.txtLastScanID.ReadOnly = true;
            this.txtLastScanID.Size = new System.Drawing.Size(205, 26);
            this.txtLastScanID.TabIndex = 59;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 56;
            this.label3.Text = "Customer Name";
            // 
            // txtCustomer
            // 
            this.txtCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomer.Location = new System.Drawing.Point(6, 79);
            this.txtCustomer.MaxLength = 25;
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(205, 20);
            this.txtCustomer.TabIndex = 55;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Meter PCBA ID";
            // 
            // txtMeterPCBAID
            // 
            this.txtMeterPCBAID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMeterPCBAID.Location = new System.Drawing.Point(6, 182);
            this.txtMeterPCBAID.MaxLength = 16;
            this.txtMeterPCBAID.Name = "txtMeterPCBAID";
            this.txtMeterPCBAID.ReadOnly = true;
            this.txtMeterPCBAID.Size = new System.Drawing.Size(205, 22);
            this.txtMeterPCBAID.TabIndex = 21;
            // 
            // lblParaonTestType
            // 
            this.lblParaonTestType.AutoSize = true;
            this.lblParaonTestType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParaonTestType.Location = new System.Drawing.Point(8, 104);
            this.lblParaonTestType.Name = "lblParaonTestType";
            this.lblParaonTestType.Size = new System.Drawing.Size(126, 20);
            this.lblParaonTestType.TabIndex = 20;
            this.lblParaonTestType.Text = "Scan PCBA ID";
            // 
            // txtPCBAID
            // 
            this.txtPCBAID.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPCBAID.Location = new System.Drawing.Point(6, 127);
            this.txtPCBAID.MaxLength = 13;
            this.txtPCBAID.Name = "txtPCBAID";
            this.txtPCBAID.Size = new System.Drawing.Size(205, 29);
            this.txtPCBAID.TabIndex = 0;
            this.txtPCBAID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPCBAID_KeyPress);
            this.txtPCBAID.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMeterID_KeyUp);
            // 
            // pbExecutionStatus
            // 
            this.pbExecutionStatus.Image = global::MICROSTARDLMS.Properties.Resources.ExecutionWait;
            this.pbExecutionStatus.Location = new System.Drawing.Point(491, 329);
            this.pbExecutionStatus.Name = "pbExecutionStatus";
            this.pbExecutionStatus.Size = new System.Drawing.Size(113, 115);
            this.pbExecutionStatus.TabIndex = 25;
            this.pbExecutionStatus.TabStop = false;
            // 
            // frmActionMicroStarDLMS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(674, 576);
            this.Controls.Add(this.pbExecutionStatus);
            this.Controls.Add(this.grpinputs);
            this.Controls.Add(this.DLMSStas);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmActionMicroStarDLMS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test Run";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmActionMicroStarDLMS_FormClosing);
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