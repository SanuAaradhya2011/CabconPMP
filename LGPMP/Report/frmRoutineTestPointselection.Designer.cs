namespace LGPMP.Report
{
    partial class frmRoutineTestPointselection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRoutineTestPointselection));
            this.gpparams = new System.Windows.Forms.GroupBox();
            this.chklist = new System.Windows.Forms.CheckedListBox();
            this.btnselect = new System.Windows.Forms.Button();
            this.btnclose = new System.Windows.Forms.Button();
            this.btnUpScroll = new System.Windows.Forms.Button();
            this.btnDownScroll = new System.Windows.Forms.Button();
            this.lblSelectedCount = new System.Windows.Forms.Label();
            this.gpparams.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpparams
            // 
            this.gpparams.Controls.Add(this.chklist);
            this.gpparams.Location = new System.Drawing.Point(9, 11);
            this.gpparams.Name = "gpparams";
            this.gpparams.Size = new System.Drawing.Size(318, 239);
            this.gpparams.TabIndex = 0;
            this.gpparams.TabStop = false;
            this.gpparams.Text = "Parameters";
            // 
            // chklist
            // 
            this.chklist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chklist.FormattingEnabled = true;
            this.chklist.Location = new System.Drawing.Point(3, 16);
            this.chklist.Name = "chklist";
            this.chklist.Size = new System.Drawing.Size(312, 214);
            this.chklist.TabIndex = 0;
            this.chklist.DoubleClick += new System.EventHandler(this.chklist_DoubleClick);
            this.chklist.SelectedValueChanged += new System.EventHandler(this.chklist_SelectedValueChanged);
            // 
            // btnselect
            // 
            this.btnselect.Location = new System.Drawing.Point(197, 256);
            this.btnselect.Name = "btnselect";
            this.btnselect.Size = new System.Drawing.Size(75, 38);
            this.btnselect.TabIndex = 1;
            this.btnselect.Text = "OK";
            this.btnselect.UseVisualStyleBackColor = true;
            this.btnselect.Click += new System.EventHandler(this.btnselect_Click);
            // 
            // btnclose
            // 
            this.btnclose.Location = new System.Drawing.Point(283, 256);
            this.btnclose.Name = "btnclose";
            this.btnclose.Size = new System.Drawing.Size(75, 38);
            this.btnclose.TabIndex = 2;
            this.btnclose.Text = "Close";
            this.btnclose.UseVisualStyleBackColor = true;
            this.btnclose.Click += new System.EventHandler(this.btnclose_Click);
            // 
            // btnUpScroll
            // 
            this.btnUpScroll.BackColor = System.Drawing.Color.White;
            this.btnUpScroll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpScroll.Image = ((System.Drawing.Image)(resources.GetObject("btnUpScroll.Image")));
            this.btnUpScroll.Location = new System.Drawing.Point(333, 66);
            this.btnUpScroll.Name = "btnUpScroll";
            this.btnUpScroll.Size = new System.Drawing.Size(31, 62);
            this.btnUpScroll.TabIndex = 3;
            this.btnUpScroll.UseVisualStyleBackColor = false;
            this.btnUpScroll.Click += new System.EventHandler(this.btnUpScroll_Click);
            // 
            // btnDownScroll
            // 
            this.btnDownScroll.BackColor = System.Drawing.Color.White;
            this.btnDownScroll.Image = ((System.Drawing.Image)(resources.GetObject("btnDownScroll.Image")));
            this.btnDownScroll.Location = new System.Drawing.Point(333, 134);
            this.btnDownScroll.Name = "btnDownScroll";
            this.btnDownScroll.Size = new System.Drawing.Size(31, 62);
            this.btnDownScroll.TabIndex = 4;
            this.btnDownScroll.UseVisualStyleBackColor = false;
            this.btnDownScroll.Click += new System.EventHandler(this.btnDownScroll_Click);
            // 
            // lblSelectedCount
            // 
            this.lblSelectedCount.AutoSize = true;
            this.lblSelectedCount.Location = new System.Drawing.Point(12, 269);
            this.lblSelectedCount.Name = "lblSelectedCount";
            this.lblSelectedCount.Size = new System.Drawing.Size(117, 13);
            this.lblSelectedCount.TabIndex = 5;
            this.lblSelectedCount.Text = "Total Selected Points : ";
            // 
            // frmRoutineTestPointselection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 306);
            this.Controls.Add(this.lblSelectedCount);
            this.Controls.Add(this.btnDownScroll);
            this.Controls.Add(this.btnUpScroll);
            this.Controls.Add(this.btnclose);
            this.Controls.Add(this.btnselect);
            this.Controls.Add(this.gpparams);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRoutineTestPointselection";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Execution Report Parameters Selection";
            this.Load += new System.EventHandler(this.frmreportcolselection_Load);
            this.gpparams.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gpparams;
        private System.Windows.Forms.CheckedListBox chklist;
        private System.Windows.Forms.Button btnselect;
        private System.Windows.Forms.Button btnclose;
        private System.Windows.Forms.Button btnUpScroll;
        private System.Windows.Forms.Button btnDownScroll;
        private System.Windows.Forms.Label lblSelectedCount;
    }
}