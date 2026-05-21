namespace PMPSAPPHIRE
{
    partial class frmLCDDisplayTest
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
            this.label5 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.rdLedReActiveFail = new System.Windows.Forms.RadioButton();
            this.rdLedReActiveOK = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.rdLedActiveFail = new System.Windows.Forms.RadioButton();
            this.rdLedActiveOK = new System.Windows.Forms.RadioButton();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.rdOddFail = new System.Windows.Forms.RadioButton();
            this.rdOddOK = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.rdEvenFail = new System.Windows.Forms.RadioButton();
            this.rdEvenOK = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblphasetest = new System.Windows.Forms.Label();
            this.rdPhaseFail = new System.Windows.Forms.RadioButton();
            this.rdPhaseOK = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.panel5);
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(7, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(534, 330);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(-2, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(536, 24);
            this.label5.TabIndex = 8;
            this.label5.Text = "Press UP, Down, MD Switch ! Then Hit OK To Continue !";
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.label4);
            this.panel5.Controls.Add(this.rdLedReActiveFail);
            this.panel5.Controls.Add(this.rdLedReActiveOK);
            this.panel5.Location = new System.Drawing.Point(8, 233);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(519, 40);
            this.panel5.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(1, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(221, 25);
            this.label4.TabIndex = 2;
            this.label4.Text = "LED Re-Active";
            // 
            // rdLedReActiveFail
            // 
            this.rdLedReActiveFail.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdLedReActiveFail.Location = new System.Drawing.Point(348, -1);
            this.rdLedReActiveFail.Name = "rdLedReActiveFail";
            this.rdLedReActiveFail.Size = new System.Drawing.Size(166, 35);
            this.rdLedReActiveFail.TabIndex = 1;
            this.rdLedReActiveFail.TabStop = true;
            this.rdLedReActiveFail.Text = "Not-Working";
            this.rdLedReActiveFail.UseVisualStyleBackColor = true;
            this.rdLedReActiveFail.CheckedChanged += new System.EventHandler(this.rdLedReActiveFail_CheckedChanged);
            // 
            // rdLedReActiveOK
            // 
            this.rdLedReActiveOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdLedReActiveOK.Location = new System.Drawing.Point(228, -2);
            this.rdLedReActiveOK.Name = "rdLedReActiveOK";
            this.rdLedReActiveOK.Size = new System.Drawing.Size(119, 35);
            this.rdLedReActiveOK.TabIndex = 0;
            this.rdLedReActiveOK.TabStop = true;
            this.rdLedReActiveOK.Text = "Working";
            this.rdLedReActiveOK.UseVisualStyleBackColor = true;
            this.rdLedReActiveOK.CheckedChanged += new System.EventHandler(this.rdLedReActiveOK_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.rdLedActiveFail);
            this.panel4.Controls.Add(this.rdLedActiveOK);
            this.panel4.Location = new System.Drawing.Point(8, 187);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(519, 40);
            this.panel4.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "LED Active";
            // 
            // rdLedActiveFail
            // 
            this.rdLedActiveFail.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdLedActiveFail.Location = new System.Drawing.Point(348, -1);
            this.rdLedActiveFail.Name = "rdLedActiveFail";
            this.rdLedActiveFail.Size = new System.Drawing.Size(166, 35);
            this.rdLedActiveFail.TabIndex = 1;
            this.rdLedActiveFail.TabStop = true;
            this.rdLedActiveFail.Text = "Not-Working";
            this.rdLedActiveFail.UseVisualStyleBackColor = true;
            this.rdLedActiveFail.CheckedChanged += new System.EventHandler(this.rdLedActiveFail_CheckedChanged);
            // 
            // rdLedActiveOK
            // 
            this.rdLedActiveOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdLedActiveOK.Location = new System.Drawing.Point(228, -2);
            this.rdLedActiveOK.Name = "rdLedActiveOK";
            this.rdLedActiveOK.Size = new System.Drawing.Size(119, 35);
            this.rdLedActiveOK.TabIndex = 0;
            this.rdLedActiveOK.TabStop = true;
            this.rdLedActiveOK.Text = "Working";
            this.rdLedActiveOK.UseVisualStyleBackColor = true;
            this.rdLedActiveOK.CheckedChanged += new System.EventHandler(this.rdLedActiveOK_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(279, 279);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 44);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(191, 279);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(82, 44);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.rdOddFail);
            this.panel3.Controls.Add(this.rdOddOK);
            this.panel3.Location = new System.Drawing.Point(6, 141);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(519, 40);
            this.panel3.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(221, 29);
            this.label3.TabIndex = 2;
            this.label3.Text = "LCD odd Segment";
            // 
            // rdOddFail
            // 
            this.rdOddFail.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdOddFail.Location = new System.Drawing.Point(348, -1);
            this.rdOddFail.Name = "rdOddFail";
            this.rdOddFail.Size = new System.Drawing.Size(166, 35);
            this.rdOddFail.TabIndex = 1;
            this.rdOddFail.TabStop = true;
            this.rdOddFail.Text = "Not-Working";
            this.rdOddFail.UseVisualStyleBackColor = true;
            this.rdOddFail.CheckedChanged += new System.EventHandler(this.rdActiveFail_CheckedChanged);
            // 
            // rdOddOK
            // 
            this.rdOddOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdOddOK.Location = new System.Drawing.Point(228, -2);
            this.rdOddOK.Name = "rdOddOK";
            this.rdOddOK.Size = new System.Drawing.Size(119, 35);
            this.rdOddOK.TabIndex = 0;
            this.rdOddOK.TabStop = true;
            this.rdOddOK.Text = "Working";
            this.rdOddOK.UseVisualStyleBackColor = true;
            this.rdOddOK.CheckedChanged += new System.EventHandler(this.rdActiveOK_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.rdEvenFail);
            this.panel2.Controls.Add(this.rdEvenOK);
            this.panel2.Location = new System.Drawing.Point(6, 95);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(519, 40);
            this.panel2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(221, 28);
            this.label2.TabIndex = 2;
            this.label2.Text = "LCD Even Segment";
            // 
            // rdEvenFail
            // 
            this.rdEvenFail.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdEvenFail.Location = new System.Drawing.Point(348, 3);
            this.rdEvenFail.Name = "rdEvenFail";
            this.rdEvenFail.Size = new System.Drawing.Size(166, 35);
            this.rdEvenFail.TabIndex = 1;
            this.rdEvenFail.TabStop = true;
            this.rdEvenFail.Text = "Not-Working";
            this.rdEvenFail.UseVisualStyleBackColor = true;
            this.rdEvenFail.CheckedChanged += new System.EventHandler(this.rdRelayFail_CheckedChanged);
            // 
            // rdEvenOK
            // 
            this.rdEvenOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdEvenOK.Location = new System.Drawing.Point(228, 2);
            this.rdEvenOK.Name = "rdEvenOK";
            this.rdEvenOK.Size = new System.Drawing.Size(119, 35);
            this.rdEvenOK.TabIndex = 0;
            this.rdEvenOK.TabStop = true;
            this.rdEvenOK.Text = "Working";
            this.rdEvenOK.UseVisualStyleBackColor = true;
            this.rdEvenOK.CheckedChanged += new System.EventHandler(this.rdRelayOK_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblphasetest);
            this.panel1.Controls.Add(this.rdPhaseFail);
            this.panel1.Controls.Add(this.rdPhaseOK);
            this.panel1.Location = new System.Drawing.Point(6, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(519, 40);
            this.panel1.TabIndex = 0;
            // 
            // lblphasetest
            // 
            this.lblphasetest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblphasetest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblphasetest.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblphasetest.Location = new System.Drawing.Point(1, 9);
            this.lblphasetest.Name = "lblphasetest";
            this.lblphasetest.Size = new System.Drawing.Size(221, 28);
            this.lblphasetest.TabIndex = 2;
            this.lblphasetest.Text = "LCD All Segment";
            // 
            // rdPhaseFail
            // 
            this.rdPhaseFail.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdPhaseFail.Location = new System.Drawing.Point(348, 3);
            this.rdPhaseFail.Name = "rdPhaseFail";
            this.rdPhaseFail.Size = new System.Drawing.Size(166, 35);
            this.rdPhaseFail.TabIndex = 1;
            this.rdPhaseFail.TabStop = true;
            this.rdPhaseFail.Text = "Not-Working";
            this.rdPhaseFail.UseVisualStyleBackColor = true;
            this.rdPhaseFail.CheckedChanged += new System.EventHandler(this.rdPhaseFail_CheckedChanged);
            // 
            // rdPhaseOK
            // 
            this.rdPhaseOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdPhaseOK.Location = new System.Drawing.Point(228, 3);
            this.rdPhaseOK.Name = "rdPhaseOK";
            this.rdPhaseOK.Size = new System.Drawing.Size(119, 35);
            this.rdPhaseOK.TabIndex = 0;
            this.rdPhaseOK.TabStop = true;
            this.rdPhaseOK.Text = "Working";
            this.rdPhaseOK.UseVisualStyleBackColor = true;
            this.rdPhaseOK.CheckedChanged += new System.EventHandler(this.rdPhaseOK_CheckedChanged);
            // 
            // frmLCDDisplayTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ClientSize = new System.Drawing.Size(547, 341);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLCDDisplayTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rdOddFail;
        private System.Windows.Forms.RadioButton rdOddOK;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdEvenFail;
        private System.Windows.Forms.RadioButton rdEvenOK;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblphasetest;
        private System.Windows.Forms.RadioButton rdPhaseFail;
        private System.Windows.Forms.RadioButton rdPhaseOK;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rdLedReActiveFail;
        private System.Windows.Forms.RadioButton rdLedReActiveOK;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdLedActiveFail;
        private System.Windows.Forms.RadioButton rdLedActiveOK;
        private System.Windows.Forms.Label label5;
    }
}