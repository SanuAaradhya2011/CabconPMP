namespace CabconPMPREJECTIONTOOL
{
    partial class frmHardError
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
            this.listBoxErrorItems = new System.Windows.Forms.ListBox();
            this.groupMeterDetails = new System.Windows.Forms.GroupBox();
            this.groupStage = new System.Windows.Forms.GroupBox();
            this.rdbFT = new System.Windows.Forms.RadioButton();
            this.rdbSealing = new System.Windows.Forms.RadioButton();
            this.rdbAssembly = new System.Windows.Forms.RadioButton();
            this.rdbCalibration = new System.Windows.Forms.RadioButton();
            this.rdbSerialization = new System.Windows.Forms.RadioButton();
            this.groupMeterType = new System.Windows.Forms.GroupBox();
            this.rdb3PHSMART = new System.Windows.Forms.RadioButton();
            this.rdbSAPPHIRE = new System.Windows.Forms.RadioButton();
            this.rdbMSNDLMS = new System.Windows.Forms.RadioButton();
            this.rdb1PHSMART = new System.Windows.Forms.RadioButton();
            this.rdbMSDLMS = new System.Windows.Forms.RadioButton();
            this.panelCustomer = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCustomerID = new System.Windows.Forms.TextBox();
            this.panelPCBAID = new System.Windows.Forms.Panel();
            this.txtPCBAID = new System.Windows.Forms.TextBox();
            this.btnGetPCBID = new System.Windows.Forms.Button();
            this.rtbStatus = new System.Windows.Forms.RichTextBox();
            this.panelRejectDetails = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRejectAction = new System.Windows.Forms.TextBox();
            this.txtRejectCause = new System.Windows.Forms.TextBox();
            this.txtProbDesc = new System.Windows.Forms.TextBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAddNewErrorType = new System.Windows.Forms.Button();
            this.txtNewErrorType = new System.Windows.Forms.TextBox();
            this.groupStatus = new System.Windows.Forms.GroupBox();
            this.panelErrorType = new System.Windows.Forms.Panel();
            this.groupMeterDetails.SuspendLayout();
            this.groupStage.SuspendLayout();
            this.groupMeterType.SuspendLayout();
            this.panelCustomer.SuspendLayout();
            this.panelPCBAID.SuspendLayout();
            this.panelRejectDetails.SuspendLayout();
            this.groupStatus.SuspendLayout();
            this.panelErrorType.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxErrorItems
            // 
            this.listBoxErrorItems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxErrorItems.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.listBoxErrorItems.ItemHeight = 22;
            this.listBoxErrorItems.Location = new System.Drawing.Point(12, 12);
            this.listBoxErrorItems.Name = "listBoxErrorItems";
            this.listBoxErrorItems.Size = new System.Drawing.Size(377, 620);
            this.listBoxErrorItems.TabIndex = 2;
            this.listBoxErrorItems.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox1_DrawItem);
            // 
            // groupMeterDetails
            // 
            this.groupMeterDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupMeterDetails.Controls.Add(this.groupStage);
            this.groupMeterDetails.Controls.Add(this.groupMeterType);
            this.groupMeterDetails.Controls.Add(this.panelCustomer);
            this.groupMeterDetails.Controls.Add(this.panelPCBAID);
            this.groupMeterDetails.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupMeterDetails.Location = new System.Drawing.Point(395, 79);
            this.groupMeterDetails.Name = "groupMeterDetails";
            this.groupMeterDetails.Size = new System.Drawing.Size(230, 478);
            this.groupMeterDetails.TabIndex = 1;
            this.groupMeterDetails.TabStop = false;
            this.groupMeterDetails.Text = "Meter Details";
            // 
            // groupStage
            // 
            this.groupStage.Controls.Add(this.rdbFT);
            this.groupStage.Controls.Add(this.rdbSealing);
            this.groupStage.Controls.Add(this.rdbAssembly);
            this.groupStage.Controls.Add(this.rdbCalibration);
            this.groupStage.Controls.Add(this.rdbSerialization);
            this.groupStage.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupStage.Location = new System.Drawing.Point(0, 81);
            this.groupStage.Name = "groupStage";
            this.groupStage.Size = new System.Drawing.Size(230, 160);
            this.groupStage.TabIndex = 5;
            this.groupStage.TabStop = false;
            this.groupStage.Text = "PRODUCTION STAGE";
            // 
            // rdbFT
            // 
            this.rdbFT.AutoSize = true;
            this.rdbFT.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbFT.Location = new System.Drawing.Point(6, 29);
            this.rdbFT.Name = "rdbFT";
            this.rdbFT.Size = new System.Drawing.Size(117, 20);
            this.rdbFT.TabIndex = 4;
            this.rdbFT.TabStop = true;
            this.rdbFT.Text = "Functional Test";
            this.rdbFT.UseVisualStyleBackColor = true;
            this.rdbFT.CheckedChanged += new System.EventHandler(this.rdbFT_CheckedChanged);
            // 
            // rdbSealing
            // 
            this.rdbSealing.AutoSize = true;
            this.rdbSealing.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbSealing.Location = new System.Drawing.Point(6, 107);
            this.rdbSealing.Name = "rdbSealing";
            this.rdbSealing.Size = new System.Drawing.Size(72, 20);
            this.rdbSealing.TabIndex = 4;
            this.rdbSealing.TabStop = true;
            this.rdbSealing.Text = "Sealing";
            this.rdbSealing.UseVisualStyleBackColor = true;
            this.rdbSealing.CheckedChanged += new System.EventHandler(this.rdbFT_CheckedChanged);
            // 
            // rdbAssembly
            // 
            this.rdbAssembly.AutoSize = true;
            this.rdbAssembly.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbAssembly.Location = new System.Drawing.Point(6, 133);
            this.rdbAssembly.Name = "rdbAssembly";
            this.rdbAssembly.Size = new System.Drawing.Size(86, 20);
            this.rdbAssembly.TabIndex = 4;
            this.rdbAssembly.TabStop = true;
            this.rdbAssembly.Text = "Assembly";
            this.rdbAssembly.UseVisualStyleBackColor = true;
            this.rdbAssembly.CheckedChanged += new System.EventHandler(this.rdbFT_CheckedChanged);
            // 
            // rdbCalibration
            // 
            this.rdbCalibration.AutoSize = true;
            this.rdbCalibration.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbCalibration.Location = new System.Drawing.Point(6, 55);
            this.rdbCalibration.Name = "rdbCalibration";
            this.rdbCalibration.Size = new System.Drawing.Size(90, 20);
            this.rdbCalibration.TabIndex = 4;
            this.rdbCalibration.TabStop = true;
            this.rdbCalibration.Text = "Calibration";
            this.rdbCalibration.UseVisualStyleBackColor = true;
            this.rdbCalibration.CheckedChanged += new System.EventHandler(this.rdbFT_CheckedChanged);
            // 
            // rdbSerialization
            // 
            this.rdbSerialization.AutoSize = true;
            this.rdbSerialization.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbSerialization.Location = new System.Drawing.Point(6, 81);
            this.rdbSerialization.Name = "rdbSerialization";
            this.rdbSerialization.Size = new System.Drawing.Size(99, 20);
            this.rdbSerialization.TabIndex = 4;
            this.rdbSerialization.TabStop = true;
            this.rdbSerialization.Text = "Serialization";
            this.rdbSerialization.UseVisualStyleBackColor = true;
            this.rdbSerialization.CheckedChanged += new System.EventHandler(this.rdbFT_CheckedChanged);
            // 
            // groupMeterType
            // 
            this.groupMeterType.Controls.Add(this.rdb3PHSMART);
            this.groupMeterType.Controls.Add(this.rdbSAPPHIRE);
            this.groupMeterType.Controls.Add(this.rdbMSNDLMS);
            this.groupMeterType.Controls.Add(this.rdb1PHSMART);
            this.groupMeterType.Controls.Add(this.rdbMSDLMS);
            this.groupMeterType.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupMeterType.Location = new System.Drawing.Point(0, 248);
            this.groupMeterType.Name = "groupMeterType";
            this.groupMeterType.Size = new System.Drawing.Size(230, 158);
            this.groupMeterType.TabIndex = 4;
            this.groupMeterType.TabStop = false;
            this.groupMeterType.Text = "METER TYPE";
            // 
            // rdb3PHSMART
            // 
            this.rdb3PHSMART.AutoSize = true;
            this.rdb3PHSMART.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb3PHSMART.Location = new System.Drawing.Point(6, 132);
            this.rdb3PHSMART.Name = "rdb3PHSMART";
            this.rdb3PHSMART.Size = new System.Drawing.Size(173, 20);
            this.rdb3PHSMART.TabIndex = 4;
            this.rdb3PHSMART.TabStop = true;
            this.rdb3PHSMART.Text = "SM310-3PH-Smart Meter";
            this.rdb3PHSMART.UseVisualStyleBackColor = true;
            this.rdb3PHSMART.CheckedChanged += new System.EventHandler(this.rdbMSNDLMS_CheckedChanged);
            // 
            // rdbSAPPHIRE
            // 
            this.rdbSAPPHIRE.AutoSize = true;
            this.rdbSAPPHIRE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbSAPPHIRE.Location = new System.Drawing.Point(6, 80);
            this.rdbSAPPHIRE.Name = "rdbSAPPHIRE";
            this.rdbSAPPHIRE.Size = new System.Drawing.Size(165, 20);
            this.rdbSAPPHIRE.TabIndex = 4;
            this.rdbSAPPHIRE.TabStop = true;
            this.rdbSAPPHIRE.Text = "DLMS-3PH-SAPPHIRE";
            this.rdbSAPPHIRE.UseVisualStyleBackColor = true;
            this.rdbSAPPHIRE.CheckedChanged += new System.EventHandler(this.rdbMSNDLMS_CheckedChanged);
            // 
            // rdbMSNDLMS
            // 
            this.rdbMSNDLMS.AutoSize = true;
            this.rdbMSNDLMS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbMSNDLMS.Location = new System.Drawing.Point(6, 28);
            this.rdbMSNDLMS.Name = "rdbMSNDLMS";
            this.rdbMSNDLMS.Size = new System.Drawing.Size(147, 20);
            this.rdbMSNDLMS.TabIndex = 4;
            this.rdbMSNDLMS.TabStop = true;
            this.rdbMSNDLMS.Text = "E150-1PH-MicroStar";
            this.rdbMSNDLMS.UseVisualStyleBackColor = true;
            this.rdbMSNDLMS.CheckedChanged += new System.EventHandler(this.rdbMSNDLMS_CheckedChanged);
            // 
            // rdb1PHSMART
            // 
            this.rdb1PHSMART.AutoSize = true;
            this.rdb1PHSMART.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb1PHSMART.Location = new System.Drawing.Point(6, 106);
            this.rdb1PHSMART.Name = "rdb1PHSMART";
            this.rdb1PHSMART.Size = new System.Drawing.Size(173, 20);
            this.rdb1PHSMART.TabIndex = 4;
            this.rdb1PHSMART.TabStop = true;
            this.rdb1PHSMART.Text = "SM110-1PH-Smart Meter";
            this.rdb1PHSMART.UseVisualStyleBackColor = true;
            this.rdb1PHSMART.CheckedChanged += new System.EventHandler(this.rdbMSNDLMS_CheckedChanged);
            // 
            // rdbMSDLMS
            // 
            this.rdbMSDLMS.AutoSize = true;
            this.rdbMSDLMS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbMSDLMS.Location = new System.Drawing.Point(6, 54);
            this.rdbMSDLMS.Name = "rdbMSDLMS";
            this.rdbMSDLMS.Size = new System.Drawing.Size(188, 20);
            this.rdbMSDLMS.TabIndex = 4;
            this.rdbMSDLMS.TabStop = true;
            this.rdbMSDLMS.Text = "E150-1PH-MicroStar-DLMS";
            this.rdbMSDLMS.UseVisualStyleBackColor = true;
            this.rdbMSDLMS.CheckedChanged += new System.EventHandler(this.rdbMSNDLMS_CheckedChanged);
            // 
            // panelCustomer
            // 
            this.panelCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCustomer.Controls.Add(this.label4);
            this.panelCustomer.Controls.Add(this.txtCustomerID);
            this.panelCustomer.Location = new System.Drawing.Point(6, 20);
            this.panelCustomer.Name = "panelCustomer";
            this.panelCustomer.Size = new System.Drawing.Size(218, 51);
            this.panelCustomer.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "CUSTOMER";
            // 
            // txtCustomerID
            // 
            this.txtCustomerID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomerID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCustomerID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerID.Location = new System.Drawing.Point(3, 27);
            this.txtCustomerID.Name = "txtCustomerID";
            this.txtCustomerID.Size = new System.Drawing.Size(212, 22);
            this.txtCustomerID.TabIndex = 1;
            // 
            // panelPCBAID
            // 
            this.panelPCBAID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPCBAID.Controls.Add(this.txtPCBAID);
            this.panelPCBAID.Controls.Add(this.btnGetPCBID);
            this.panelPCBAID.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelPCBAID.Location = new System.Drawing.Point(6, 408);
            this.panelPCBAID.Name = "panelPCBAID";
            this.panelPCBAID.Size = new System.Drawing.Size(218, 60);
            this.panelPCBAID.TabIndex = 2;
            // 
            // txtPCBAID
            // 
            this.txtPCBAID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPCBAID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPCBAID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPCBAID.Location = new System.Drawing.Point(3, 35);
            this.txtPCBAID.Name = "txtPCBAID";
            this.txtPCBAID.Size = new System.Drawing.Size(212, 22);
            this.txtPCBAID.TabIndex = 1;
            // 
            // btnGetPCBID
            // 
            this.btnGetPCBID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetPCBID.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetPCBID.Location = new System.Drawing.Point(3, 5);
            this.btnGetPCBID.Name = "btnGetPCBID";
            this.btnGetPCBID.Size = new System.Drawing.Size(212, 29);
            this.btnGetPCBID.TabIndex = 0;
            this.btnGetPCBID.Text = "GET METER PCBAID";
            this.btnGetPCBID.UseVisualStyleBackColor = true;
            this.btnGetPCBID.Click += new System.EventHandler(this.button15_Click);
            // 
            // rtbStatus
            // 
            this.rtbStatus.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.rtbStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbStatus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbStatus.ForeColor = System.Drawing.Color.SeaGreen;
            this.rtbStatus.Location = new System.Drawing.Point(3, 17);
            this.rtbStatus.Name = "rtbStatus";
            this.rtbStatus.ReadOnly = true;
            this.rtbStatus.Size = new System.Drawing.Size(437, 43);
            this.rtbStatus.TabIndex = 4;
            this.rtbStatus.Text = "";
            this.rtbStatus.WordWrap = false;
            // 
            // panelRejectDetails
            // 
            this.panelRejectDetails.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelRejectDetails.Controls.Add(this.label6);
            this.panelRejectDetails.Controls.Add(this.label5);
            this.panelRejectDetails.Controls.Add(this.label1);
            this.panelRejectDetails.Controls.Add(this.txtRejectAction);
            this.panelRejectDetails.Controls.Add(this.txtRejectCause);
            this.panelRejectDetails.Controls.Add(this.txtProbDesc);
            this.panelRejectDetails.Location = new System.Drawing.Point(635, 86);
            this.panelRejectDetails.Name = "panelRejectDetails";
            this.panelRejectDetails.Size = new System.Drawing.Size(203, 472);
            this.panelRejectDetails.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 326);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "REJECT ACTION";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 164);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "REJECT CAUSE";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "PROBLEM DESCRIPTION";
            // 
            // txtRejectAction
            // 
            this.txtRejectAction.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtRejectAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRejectAction.Location = new System.Drawing.Point(0, 342);
            this.txtRejectAction.Multiline = true;
            this.txtRejectAction.Name = "txtRejectAction";
            this.txtRejectAction.Size = new System.Drawing.Size(203, 130);
            this.txtRejectAction.TabIndex = 1;
            // 
            // txtRejectCause
            // 
            this.txtRejectCause.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtRejectCause.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRejectCause.Location = new System.Drawing.Point(0, 181);
            this.txtRejectCause.Multiline = true;
            this.txtRejectCause.Name = "txtRejectCause";
            this.txtRejectCause.Size = new System.Drawing.Size(203, 124);
            this.txtRejectCause.TabIndex = 1;
            // 
            // txtProbDesc
            // 
            this.txtProbDesc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtProbDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProbDesc.Location = new System.Drawing.Point(0, 27);
            this.txtProbDesc.Multiline = true;
            this.txtProbDesc.Name = "txtProbDesc";
            this.txtProbDesc.Size = new System.Drawing.Size(203, 115);
            this.txtProbDesc.TabIndex = 1;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(635, 568);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(203, 68);
            this.btnUpdate.TabIndex = 5;
            this.btnUpdate.Text = "UPDATE";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnAddNewErrorType
            // 
            this.btnAddNewErrorType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNewErrorType.Location = new System.Drawing.Point(3, 3);
            this.btnAddNewErrorType.Name = "btnAddNewErrorType";
            this.btnAddNewErrorType.Size = new System.Drawing.Size(220, 35);
            this.btnAddNewErrorType.TabIndex = 5;
            this.btnAddNewErrorType.Text = "Add New Error Type";
            this.btnAddNewErrorType.UseVisualStyleBackColor = true;
            this.btnAddNewErrorType.Click += new System.EventHandler(this.btnAddNewErrorType_Click);
            // 
            // txtNewErrorType
            // 
            this.txtNewErrorType.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtNewErrorType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNewErrorType.Location = new System.Drawing.Point(3, 39);
            this.txtNewErrorType.Name = "txtNewErrorType";
            this.txtNewErrorType.Size = new System.Drawing.Size(220, 22);
            this.txtNewErrorType.TabIndex = 1;
            // 
            // groupStatus
            // 
            this.groupStatus.Controls.Add(this.rtbStatus);
            this.groupStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupStatus.Location = new System.Drawing.Point(395, 10);
            this.groupStatus.Name = "groupStatus";
            this.groupStatus.Size = new System.Drawing.Size(443, 63);
            this.groupStatus.TabIndex = 6;
            this.groupStatus.TabStop = false;
            this.groupStatus.Text = "Status";
            // 
            // panelErrorType
            // 
            this.panelErrorType.Controls.Add(this.txtNewErrorType);
            this.panelErrorType.Controls.Add(this.btnAddNewErrorType);
            this.panelErrorType.Location = new System.Drawing.Point(395, 568);
            this.panelErrorType.Name = "panelErrorType";
            this.panelErrorType.Size = new System.Drawing.Size(230, 68);
            this.panelErrorType.TabIndex = 7;
            // 
            // frmHardError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(850, 645);
            this.Controls.Add(this.panelErrorType);
            this.Controls.Add(this.listBoxErrorItems);
            this.Controls.Add(this.groupStatus);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.groupMeterDetails);
            this.Controls.Add(this.panelRejectDetails);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmHardError";
            this.Text = "Record Meter Rejection";
            this.Load += new System.EventHandler(this.frmHardError_Load);
            this.Shown += new System.EventHandler(this.frmHardError_Shown);
            this.groupMeterDetails.ResumeLayout(false);
            this.groupStage.ResumeLayout(false);
            this.groupStage.PerformLayout();
            this.groupMeterType.ResumeLayout(false);
            this.groupMeterType.PerformLayout();
            this.panelCustomer.ResumeLayout(false);
            this.panelCustomer.PerformLayout();
            this.panelPCBAID.ResumeLayout(false);
            this.panelPCBAID.PerformLayout();
            this.panelRejectDetails.ResumeLayout(false);
            this.panelRejectDetails.PerformLayout();
            this.groupStatus.ResumeLayout(false);
            this.panelErrorType.ResumeLayout(false);
            this.panelErrorType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupMeterDetails;
        private System.Windows.Forms.Button btnGetPCBID;
        private System.Windows.Forms.Panel panelPCBAID;
        private System.Windows.Forms.TextBox txtPCBAID;
        private System.Windows.Forms.Panel panelCustomer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCustomerID;
        private System.Windows.Forms.Panel panelRejectDetails;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProbDesc;
        private System.Windows.Forms.RadioButton rdb3PHSMART;
        private System.Windows.Forms.RadioButton rdb1PHSMART;
        private System.Windows.Forms.RadioButton rdbSAPPHIRE;
        private System.Windows.Forms.RadioButton rdbMSDLMS;
        private System.Windows.Forms.RadioButton rdbMSNDLMS;
        private System.Windows.Forms.RadioButton rdbFT;
        private System.Windows.Forms.RadioButton rdbSerialization;
        private System.Windows.Forms.RadioButton rdbCalibration;
        private System.Windows.Forms.RadioButton rdbSealing;
        private System.Windows.Forms.RichTextBox rtbStatus;
        private System.Windows.Forms.ListBox listBoxErrorItems;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRejectCause;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRejectAction;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAddNewErrorType;
        private System.Windows.Forms.TextBox txtNewErrorType;
        private System.Windows.Forms.RadioButton rdbAssembly;
        private System.Windows.Forms.GroupBox groupMeterType;
        private System.Windows.Forms.GroupBox groupStage;
        private System.Windows.Forms.GroupBox groupStatus;
        private System.Windows.Forms.Panel panelErrorType;
    }
}