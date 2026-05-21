namespace PMPSM110
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
            this.lblPushButton = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.rdActiveFail = new System.Windows.Forms.RadioButton();
            this.rdActiveOK = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.rdRelayFail = new System.Windows.Forms.RadioButton();
            this.rdRelayOK = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblphasetest = new System.Windows.Forms.Label();
            this.rdPhaseFail = new System.Windows.Forms.RadioButton();
            this.rdPhaseOK = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.lblPushButton);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(7, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(534, 278);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // lblPushButton
            // 
            this.lblPushButton.AutoSize = true;
            this.lblPushButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPushButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblPushButton.Location = new System.Drawing.Point(12, 20);
            this.lblPushButton.Name = "lblPushButton";
            this.lblPushButton.Size = new System.Drawing.Size(91, 16);
            this.lblPushButton.TabIndex = 5;
            this.lblPushButton.Text = "All Segment";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(272, 228);
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
            this.btnOK.Location = new System.Drawing.Point(184, 228);
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
            this.panel3.Controls.Add(this.rdActiveFail);
            this.panel3.Controls.Add(this.rdActiveOK);
            this.panel3.Location = new System.Drawing.Point(6, 168);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(519, 54);
            this.panel3.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(199, 29);
            this.label3.TabIndex = 2;
            this.label3.Text = "All Segment";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rdActiveFail
            // 
            this.rdActiveFail.AutoSize = true;
            this.rdActiveFail.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdActiveFail.Location = new System.Drawing.Point(354, 12);
            this.rdActiveFail.Name = "rdActiveFail";
            this.rdActiveFail.Size = new System.Drawing.Size(160, 29);
            this.rdActiveFail.TabIndex = 1;
            this.rdActiveFail.TabStop = true;
            this.rdActiveFail.Text = "Not-Working";
            this.rdActiveFail.UseVisualStyleBackColor = true;
            this.rdActiveFail.CheckedChanged += new System.EventHandler(this.rdActiveFail_CheckedChanged);
            // 
            // rdActiveOK
            // 
            this.rdActiveOK.AutoSize = true;
            this.rdActiveOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdActiveOK.Location = new System.Drawing.Point(232, 12);
            this.rdActiveOK.Name = "rdActiveOK";
            this.rdActiveOK.Size = new System.Drawing.Size(116, 29);
            this.rdActiveOK.TabIndex = 0;
            this.rdActiveOK.TabStop = true;
            this.rdActiveOK.Text = "Working";
            this.rdActiveOK.UseVisualStyleBackColor = true;
            this.rdActiveOK.CheckedChanged += new System.EventHandler(this.rdActiveOK_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.rdRelayFail);
            this.panel2.Controls.Add(this.rdRelayOK);
            this.panel2.Location = new System.Drawing.Point(6, 108);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(519, 54);
            this.panel2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(199, 29);
            this.label2.TabIndex = 2;
            this.label2.Text = "Relay ON/OFF";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rdRelayFail
            // 
            this.rdRelayFail.AutoSize = true;
            this.rdRelayFail.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdRelayFail.Location = new System.Drawing.Point(354, 9);
            this.rdRelayFail.Name = "rdRelayFail";
            this.rdRelayFail.Size = new System.Drawing.Size(160, 29);
            this.rdRelayFail.TabIndex = 1;
            this.rdRelayFail.TabStop = true;
            this.rdRelayFail.Text = "Not-Working";
            this.rdRelayFail.UseVisualStyleBackColor = true;
            this.rdRelayFail.CheckedChanged += new System.EventHandler(this.rdRelayFail_CheckedChanged);
            // 
            // rdRelayOK
            // 
            this.rdRelayOK.AutoSize = true;
            this.rdRelayOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdRelayOK.Location = new System.Drawing.Point(232, 9);
            this.rdRelayOK.Name = "rdRelayOK";
            this.rdRelayOK.Size = new System.Drawing.Size(116, 29);
            this.rdRelayOK.TabIndex = 0;
            this.rdRelayOK.TabStop = true;
            this.rdRelayOK.Text = "Working";
            this.rdRelayOK.UseVisualStyleBackColor = true;
            this.rdRelayOK.CheckedChanged += new System.EventHandler(this.rdRelayOK_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblphasetest);
            this.panel1.Controls.Add(this.rdPhaseFail);
            this.panel1.Controls.Add(this.rdPhaseOK);
            this.panel1.Location = new System.Drawing.Point(6, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(519, 54);
            this.panel1.TabIndex = 0;
            // 
            // lblphasetest
            // 
            this.lblphasetest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblphasetest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblphasetest.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblphasetest.Location = new System.Drawing.Point(3, 10);
            this.lblphasetest.Name = "lblphasetest";
            this.lblphasetest.Size = new System.Drawing.Size(199, 29);
            this.lblphasetest.TabIndex = 2;
            this.lblphasetest.Text = "Push Button Icon";
            this.lblphasetest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rdPhaseFail
            // 
            this.rdPhaseFail.AutoSize = true;
            this.rdPhaseFail.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdPhaseFail.Location = new System.Drawing.Point(354, 12);
            this.rdPhaseFail.Name = "rdPhaseFail";
            this.rdPhaseFail.Size = new System.Drawing.Size(160, 29);
            this.rdPhaseFail.TabIndex = 1;
            this.rdPhaseFail.TabStop = true;
            this.rdPhaseFail.Text = "Not-Working";
            this.rdPhaseFail.UseVisualStyleBackColor = true;
            this.rdPhaseFail.CheckedChanged += new System.EventHandler(this.rdPhaseFail_CheckedChanged);
            // 
            // rdPhaseOK
            // 
            this.rdPhaseOK.AutoSize = true;
            this.rdPhaseOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdPhaseOK.Location = new System.Drawing.Point(232, 12);
            this.rdPhaseOK.Name = "rdPhaseOK";
            this.rdPhaseOK.Size = new System.Drawing.Size(116, 29);
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
            this.ClientSize = new System.Drawing.Size(547, 298);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLCDDisplayTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rdActiveFail;
        private System.Windows.Forms.RadioButton rdActiveOK;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdRelayFail;
        private System.Windows.Forms.RadioButton rdRelayOK;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblphasetest;
        private System.Windows.Forms.RadioButton rdPhaseFail;
        private System.Windows.Forms.RadioButton rdPhaseOK;
        private System.Windows.Forms.Label lblPushButton;
    }
}