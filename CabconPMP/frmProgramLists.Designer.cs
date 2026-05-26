namespace CabconPMP
{
    partial class frmProgramLists
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
            this.cmbMeterType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtProgramCounts = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lstParameterLists = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtParametersName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtProgramName = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblAddUpdate = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.lblDelete = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblSave = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.lblClose = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbMeterType);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtProgramCounts);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lstParameterLists);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtParametersName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtProgramName);
            this.groupBox1.Location = new System.Drawing.Point(18, 43);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(976, 349);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // cmbMeterType
            // 
            this.cmbMeterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMeterType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbMeterType.FormattingEnabled = true;
            this.cmbMeterType.Location = new System.Drawing.Point(188, 89);
            this.cmbMeterType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbMeterType.Name = "cmbMeterType";
            this.cmbMeterType.Size = new System.Drawing.Size(332, 28);
            this.cmbMeterType.TabIndex = 24;
            this.cmbMeterType.SelectionChangeCommitted += new System.EventHandler(this.cmbMeterType_SelectionChangeCommitted);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 94);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Meter Type";
            // 
            // txtProgramCounts
            // 
            this.txtProgramCounts.Location = new System.Drawing.Point(188, 218);
            this.txtProgramCounts.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtProgramCounts.MaxLength = 35;
            this.txtProgramCounts.Name = "txtProgramCounts";
            this.txtProgramCounts.ReadOnly = true;
            this.txtProgramCounts.Size = new System.Drawing.Size(332, 26);
            this.txtProgramCounts.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 223);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Total Program Counts";
            // 
            // lstParameterLists
            // 
            this.lstParameterLists.FormattingEnabled = true;
            this.lstParameterLists.ItemHeight = 20;
            this.lstParameterLists.Location = new System.Drawing.Point(531, 29);
            this.lstParameterLists.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lstParameterLists.Name = "lstParameterLists";
            this.lstParameterLists.Size = new System.Drawing.Size(432, 304);
            this.lstParameterLists.TabIndex = 4;
            this.lstParameterLists.Click += new System.EventHandler(this.lstParameterLists_Click);
            this.lstParameterLists.DoubleClick += new System.EventHandler(this.lstParameterLists_DoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 182);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Parameters Name";
            // 
            // txtParametersName
            // 
            this.txtParametersName.Location = new System.Drawing.Point(188, 177);
            this.txtParametersName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtParametersName.MaxLength = 35;
            this.txtParametersName.Name = "txtParametersName";
            this.txtParametersName.Size = new System.Drawing.Size(332, 26);
            this.txtParametersName.TabIndex = 2;
            this.txtParametersName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtParametersName_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 142);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Program Name (*.EXE)";
            // 
            // txtProgramName
            // 
            this.txtProgramName.Location = new System.Drawing.Point(188, 137);
            this.txtProgramName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtProgramName.MaxLength = 15;
            this.txtProgramName.Name = "txtProgramName";
            this.txtProgramName.Size = new System.Drawing.Size(332, 26);
            this.txtProgramName.TabIndex = 0;
            this.txtProgramName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtProgramName_KeyPress);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblAddUpdate,
            this.toolStripSeparator8,
            this.lblDelete,
            this.toolStripSeparator1,
            this.lblSave,
            this.toolStripSeparator4,
            this.lblClose,
            this.toolStripSeparator3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1012, 33);
            this.toolStrip1.TabIndex = 18;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lblAddUpdate
            // 
            this.lblAddUpdate.Name = "lblAddUpdate";
            this.lblAddUpdate.Size = new System.Drawing.Size(49, 28);
            this.lblAddUpdate.Text = "Add";
            this.lblAddUpdate.ToolTipText = "Open Configuration File *.cfg";
            this.lblAddUpdate.Click += new System.EventHandler(this.lblAddUpdate_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 33);
            // 
            // lblDelete
            // 
            this.lblDelete.Name = "lblDelete";
            this.lblDelete.Size = new System.Drawing.Size(68, 28);
            this.lblDelete.Text = "Delete";
            this.lblDelete.Click += new System.EventHandler(this.lblDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // lblSave
            // 
            this.lblSave.Name = "lblSave";
            this.lblSave.Size = new System.Drawing.Size(53, 28);
            this.lblSave.Text = "Save";
            this.lblSave.ToolTipText = "Save Configuration File As *.cfg";
            this.lblSave.Click += new System.EventHandler(this.lblSave_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 33);
            // 
            // lblClose
            // 
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(59, 28);
            this.lblClose.Text = "Close";
            this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 33);
            // 
            // frmProgramLists
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 402);
            this.ControlBox = false;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProgramLists";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Program List";
            this.Load += new System.EventHandler(this.frmProgramLists_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstParameterLists;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtParametersName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProgramName;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel lblAddUpdate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripLabel lblDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel lblSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel lblClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.TextBox txtProgramCounts;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbMeterType;
    }
}