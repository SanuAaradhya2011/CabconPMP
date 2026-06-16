namespace CabconPMP
{
    partial class RetryRecordCard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // ─── Designer-visible child controls ──────────────────────────────────────
        //
        // All sizing/colour values here were extracted directly from the runtime
        // CreateCard() method in frmRetryRecords.cs.  Developers can now tweak
        // them visually inside the VS WinForms designer.

        internal System.Windows.Forms.Label   lblPosition;
        internal System.Windows.Forms.Label   lblPcbaId;
        internal System.Windows.Forms.Label   lblPort;
        internal System.Windows.Forms.Label   lblThread;
        internal System.Windows.Forms.Panel   pnlButtons;
        internal System.Windows.Forms.Button  btnDetails;
        internal System.Windows.Forms.Button  btnRetry;

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support — do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblPosition = new System.Windows.Forms.Label();
            this.lblPcbaId = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblThread = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnDetails = new System.Windows.Forms.Button();
            this.btnRetry = new System.Windows.Forms.Button();
            this.pnlStatusPill = new System.Windows.Forms.Panel();
            this.lblCalibration = new System.Windows.Forms.Label();
            this.pnlButtons.SuspendLayout();
            this.pnlStatusPill.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPosition
            // 
            this.lblPosition.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPosition.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPosition.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(64)))), ((int)(((byte)(175)))));
            this.lblPosition.Location = new System.Drawing.Point(12, 12);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(266, 37);
            this.lblPosition.TabIndex = 5;
            this.lblPosition.Text = "Position 1";
            this.lblPosition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPcbaId
            // 
            this.lblPcbaId.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPcbaId.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPcbaId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblPcbaId.Location = new System.Drawing.Point(12, 49);
            this.lblPcbaId.Name = "lblPcbaId";
            this.lblPcbaId.Size = new System.Drawing.Size(266, 30);
            this.lblPcbaId.TabIndex = 4;
            this.lblPcbaId.Text = "1234567";
            this.lblPcbaId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPort
            // 
            this.lblPort.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPort.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblPort.Location = new System.Drawing.Point(12, 105);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(266, 25);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Port: 1234567";
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblThread
            // 
            this.lblThread.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblThread.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThread.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblThread.Location = new System.Drawing.Point(12, 79);
            this.lblThread.Name = "lblThread";
            this.lblThread.Size = new System.Drawing.Size(266, 26);
            this.lblThread.TabIndex = 3;
            this.lblThread.Text = "Thread: 2001";
            this.lblThread.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnDetails);
            this.pnlButtons.Controls.Add(this.btnRetry);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(12, 200);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(266, 28);
            this.pnlButtons.TabIndex = 1;
            // 
            // btnDetails
            // 
            this.btnDetails.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDetails.Location = new System.Drawing.Point(73, 0);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size(123, 28);
            this.btnDetails.TabIndex = 0;
            this.btnDetails.Text = "See Details";
            this.btnDetails.Click += new System.EventHandler(this.btnDetails_Click);
            // 
            // btnRetry
            // 
            this.btnRetry.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRetry.Location = new System.Drawing.Point(196, 0);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(70, 28);
            this.btnRetry.TabIndex = 1;
            this.btnRetry.Text = "Retry";
            this.btnRetry.Visible = false;
            this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
            // 
            // pnlStatusPill
            // 
            this.pnlStatusPill.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(252)))), ((int)(((byte)(231)))));
            this.pnlStatusPill.Controls.Add(this.lblCalibration);
            this.pnlStatusPill.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatusPill.Location = new System.Drawing.Point(12, 149);
            this.pnlStatusPill.Name = "pnlStatusPill";
            this.pnlStatusPill.Padding = new System.Windows.Forms.Padding(6);
            this.pnlStatusPill.Size = new System.Drawing.Size(266, 51);
            this.pnlStatusPill.TabIndex = 0;
            // 
            // lblCalibration
            // 
            this.lblCalibration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCalibration.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCalibration.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(128)))), ((int)(((byte)(61)))));
            this.lblCalibration.Location = new System.Drawing.Point(6, 6);
            this.lblCalibration.Margin = new System.Windows.Forms.Padding(0);
            this.lblCalibration.Name = "lblCalibration";
            this.lblCalibration.Size = new System.Drawing.Size(254, 39);
            this.lblCalibration.TabIndex = 0;
            this.lblCalibration.Text = "Status: Pass";
            this.lblCalibration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RetryRecordCard
            // 
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pnlStatusPill);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblThread);
            this.Controls.Add(this.lblPcbaId);
            this.Controls.Add(this.lblPosition);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "RetryRecordCard";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.Size = new System.Drawing.Size(290, 240);
            this.pnlButtons.ResumeLayout(false);
            this.pnlStatusPill.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel pnlStatusPill;
        internal System.Windows.Forms.Label lblCalibration;
    }
}
