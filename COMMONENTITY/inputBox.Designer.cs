namespace COMMONENTITY
{
    partial class inputBox
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
            this.txtInputBox = new System.Windows.Forms.TextBox();
            this.bthOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtInputBox
            // 
            this.txtInputBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInputBox.Location = new System.Drawing.Point(7, 12);
            this.txtInputBox.MaxLength = 25;
            this.txtInputBox.Name = "txtInputBox";
            this.txtInputBox.Size = new System.Drawing.Size(266, 38);
            this.txtInputBox.TabIndex = 1;
            this.txtInputBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInputBox_KeyPress);
            // 
            // bthOK
            // 
            this.bthOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bthOK.Location = new System.Drawing.Point(274, 12);
            this.bthOK.Name = "bthOK";
            this.bthOK.Size = new System.Drawing.Size(65, 39);
            this.bthOK.TabIndex = 2;
            this.bthOK.Text = "OK";
            this.bthOK.UseVisualStyleBackColor = true;
            this.bthOK.Click += new System.EventHandler(this.bthOK_Click);
            // 
            // inputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 67);
            this.ControlBox = false;
            this.Controls.Add(this.bthOK);
            this.Controls.Add(this.txtInputBox);
            this.Name = "inputBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "inputBox";
            this.Load += new System.EventHandler(this.inputBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bthOK;
        public System.Windows.Forms.TextBox txtInputBox;
    }
}