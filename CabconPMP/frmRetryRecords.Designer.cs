namespace CabconPMP
{
    partial class frmRetryRecords
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblOperationName;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.FlowLayoutPanel flpCards;
        internal RetryRecordCard templateCard;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.Label lblDetailTitle;
        private System.Windows.Forms.Label lblPositionCaption;
        private System.Windows.Forms.Label lblPcbaCaption;
        private System.Windows.Forms.Label lblCalibrationCaption;
        private System.Windows.Forms.Label lblThreadCaption;
        private System.Windows.Forms.Label lblSelectedPositionValue;
        private System.Windows.Forms.Label lblSelectedPcbaValue;
        private System.Windows.Forms.Label lblSelectedCalibrationValue;
        private System.Windows.Forms.Label lblSelectedThreadValue;
        private System.Windows.Forms.TextBox txtDetail;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblOperationName = new System.Windows.Forms.Label();
            this.lblSummary = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.flpCards = new System.Windows.Forms.FlowLayoutPanel();
            this.templateCard = new CabconPMP.RetryRecordCard();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.txtDetail = new System.Windows.Forms.TextBox();
            this.lblSelectedThreadValue = new System.Windows.Forms.Label();
            this.lblThreadCaption = new System.Windows.Forms.Label();
            this.lblSelectedCalibrationValue = new System.Windows.Forms.Label();
            this.lblCalibrationCaption = new System.Windows.Forms.Label();
            this.lblSelectedPcbaValue = new System.Windows.Forms.Label();
            this.lblPcbaCaption = new System.Windows.Forms.Label();
            this.lblSelectedPositionValue = new System.Windows.Forms.Label();
            this.lblPositionCaption = new System.Windows.Forms.Label();
            this.lblDetailTitle = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSummary);
            this.pnlHeader.Controls.Add(this.btnRefresh);
            this.pnlHeader.Controls.Add(this.btnClose);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1200, 93);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(21, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(239, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Retry Records";
            // 
            // lblOperationName
            // 
            this.lblOperationName.AutoSize = true;
            this.lblOperationName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOperationName.ForeColor = System.Drawing.Color.Black;
            this.lblOperationName.Location = new System.Drawing.Point(24, 35);
            this.lblOperationName.Name = "lblOperationName";
            this.lblOperationName.Size = new System.Drawing.Size(150, 30);
            this.lblOperationName.TabIndex = 5;
            this.lblOperationName.Text = "Method: <none>";
            // 
            // lblSummary
            // 
            this.lblSummary.AutoEllipsis = true;
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSummary.ForeColor = System.Drawing.Color.Black;
            this.lblSummary.Location = new System.Drawing.Point(24, 50);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(834, 38);
            this.lblSummary.TabIndex = 1;
            this.lblSummary.Text = "Loading bench cards...";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.Black;
            this.btnRefresh.Location = new System.Drawing.Point(1008, 24);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(84, 34);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Reload";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Location = new System.Drawing.Point(1104, 24);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(84, 34);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Hide";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 93);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainerMain.Panel1.Controls.Add(this.flpCards);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.splitContainerMain.Panel2.Controls.Add(this.pnlDetails);
            this.splitContainerMain.Size = new System.Drawing.Size(1200, 607);
            this.splitContainerMain.SplitterDistance = 850;
            this.splitContainerMain.TabIndex = 1;
            // 
            // flpCards
            // 
            this.flpCards.AutoScroll = true;
            this.flpCards.BackColor = System.Drawing.Color.White;
            this.flpCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpCards.Location = new System.Drawing.Point(0, 0);
            this.flpCards.Name = "flpCards";
            this.flpCards.Padding = new System.Windows.Forms.Padding(12);
            this.flpCards.Size = new System.Drawing.Size(850, 607);
            this.flpCards.TabIndex = 0;
            this.flpCards.WrapContents = true;
            // hidden template card used at runtime for cloning
            this.templateCard.Location = new System.Drawing.Point(0, 0);
            this.templateCard.Name = "templateCard";
            this.templateCard.Size = new System.Drawing.Size(290, 240);
            this.templateCard.TabIndex = 1;
            this.templateCard.Visible = false;
            // 
            // pnlDetails
            // 
            this.pnlDetails.BackColor = System.Drawing.Color.White;
            this.pnlDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetails.Controls.Add(this.txtDetail);
            this.pnlDetails.Controls.Add(this.lblSelectedThreadValue);
            this.pnlDetails.Controls.Add(this.lblThreadCaption);
            this.pnlDetails.Controls.Add(this.lblSelectedCalibrationValue);
            this.pnlDetails.Controls.Add(this.lblCalibrationCaption);
            this.pnlDetails.Controls.Add(this.lblSelectedPcbaValue);
            this.pnlDetails.Controls.Add(this.lblPcbaCaption);
            this.pnlDetails.Controls.Add(this.lblSelectedPositionValue);
            this.pnlDetails.Controls.Add(this.lblPositionCaption);
            this.pnlDetails.Controls.Add(this.lblDetailTitle);
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetails.Location = new System.Drawing.Point(0, 0);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Padding = new System.Windows.Forms.Padding(14);
            this.pnlDetails.Size = new System.Drawing.Size(346, 607);
            this.pnlDetails.TabIndex = 0;
            // 
            // txtDetail
            // 
            this.txtDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDetail.BackColor = System.Drawing.Color.White;
            this.txtDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetail.Location = new System.Drawing.Point(18, 292);
            this.txtDetail.Multiline = true;
            this.txtDetail.Name = "txtDetail";
            this.txtDetail.ReadOnly = true;
            this.txtDetail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetail.Size = new System.Drawing.Size(300, 280);
            this.txtDetail.TabIndex = 9;
            // 
            // lblSelectedThreadValue
            // 
            this.lblSelectedThreadValue.AutoEllipsis = true;
            this.lblSelectedThreadValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedThreadValue.Location = new System.Drawing.Point(18, 253);
            this.lblSelectedThreadValue.Name = "lblSelectedThreadValue";
            this.lblSelectedThreadValue.Size = new System.Drawing.Size(292, 24);
            this.lblSelectedThreadValue.TabIndex = 8;
            this.lblSelectedThreadValue.Text = "-";
            // 
            // lblThreadCaption
            // 
            this.lblThreadCaption.AutoSize = true;
            this.lblThreadCaption.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThreadCaption.Location = new System.Drawing.Point(18, 228);
            this.lblThreadCaption.Name = "lblThreadCaption";
            this.lblThreadCaption.Size = new System.Drawing.Size(92, 25);
            this.lblThreadCaption.TabIndex = 7;
            this.lblThreadCaption.Text = "ThreadID";
            // 
            // lblSelectedCalibrationValue
            // 
            this.lblSelectedCalibrationValue.AutoEllipsis = true;
            this.lblSelectedCalibrationValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedCalibrationValue.Location = new System.Drawing.Point(18, 197);
            this.lblSelectedCalibrationValue.Name = "lblSelectedCalibrationValue";
            this.lblSelectedCalibrationValue.Size = new System.Drawing.Size(292, 24);
            this.lblSelectedCalibrationValue.TabIndex = 6;
            this.lblSelectedCalibrationValue.Text = "-";
            // 
            // lblCalibrationCaption
            // 
            this.lblCalibrationCaption.AutoSize = true;
            this.lblCalibrationCaption.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCalibrationCaption.Location = new System.Drawing.Point(18, 172);
            this.lblCalibrationCaption.Name = "lblCalibrationCaption";
            this.lblCalibrationCaption.Size = new System.Drawing.Size(124, 25);
            this.lblCalibrationCaption.TabIndex = 5;
            this.lblCalibrationCaption.Text = "Calibration";
            // 
            // lblSelectedPcbaValue
            // 
            this.lblSelectedPcbaValue.AutoEllipsis = true;
            this.lblSelectedPcbaValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedPcbaValue.Location = new System.Drawing.Point(18, 141);
            this.lblSelectedPcbaValue.Name = "lblSelectedPcbaValue";
            this.lblSelectedPcbaValue.Size = new System.Drawing.Size(292, 24);
            this.lblSelectedPcbaValue.TabIndex = 4;
            this.lblSelectedPcbaValue.Text = "-";
            // 
            // lblPcbaCaption
            // 
            this.lblPcbaCaption.AutoSize = true;
            this.lblPcbaCaption.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPcbaCaption.Location = new System.Drawing.Point(18, 116);
            this.lblPcbaCaption.Name = "lblPcbaCaption";
            this.lblPcbaCaption.Size = new System.Drawing.Size(83, 25);
            this.lblPcbaCaption.TabIndex = 3;
            this.lblPcbaCaption.Text = "PCBAID";
            // 
            // lblSelectedPositionValue
            // 
            this.lblSelectedPositionValue.AutoEllipsis = true;
            this.lblSelectedPositionValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedPositionValue.Location = new System.Drawing.Point(18, 85);
            this.lblSelectedPositionValue.Name = "lblSelectedPositionValue";
            this.lblSelectedPositionValue.Size = new System.Drawing.Size(292, 24);
            this.lblSelectedPositionValue.TabIndex = 2;
            this.lblSelectedPositionValue.Text = "-";
            // 
            // lblPositionCaption
            // 
            this.lblPositionCaption.AutoSize = true;
            this.lblPositionCaption.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPositionCaption.Location = new System.Drawing.Point(18, 60);
            this.lblPositionCaption.Name = "lblPositionCaption";
            this.lblPositionCaption.Size = new System.Drawing.Size(82, 25);
            this.lblPositionCaption.TabIndex = 1;
            this.lblPositionCaption.Text = "Position";
            // 
            // lblDetailTitle
            // 
            this.lblDetailTitle.AutoSize = true;
            this.lblDetailTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailTitle.ForeColor = System.Drawing.Color.Black;
            this.lblDetailTitle.Location = new System.Drawing.Point(14, 12);
            this.lblDetailTitle.Name = "lblDetailTitle";
            this.lblDetailTitle.Size = new System.Drawing.Size(177, 36);
            this.lblDetailTitle.TabIndex = 0;
            this.lblDetailTitle.Text = "Card Details";
            // 
            // frmRetryRecords
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmRetryRecords";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Retry Records";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
