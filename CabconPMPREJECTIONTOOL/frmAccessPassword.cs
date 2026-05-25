using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using COMMONENTITY;

namespace CabconPMPREJECTIONTOOL
{
    /// <summary>
    /// Class for passwords as well as entry for PCBAID.
    /// </summary>
    public partial class AccessPassword : Form
    {
        private string _password="" ;
        private bool bMode = false;
        EntityUserManagement objetyusermgt = null;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        /// <summary>
        /// Default constructor.Creates an instance of this class as entry form for password.
        /// </summary>
        public AccessPassword()
        {
            InitializeComponent(); COMMONENTITY.FormStyleHelper.Apply(this);
        }
        /// <summary>
        /// Creates an instance of this class as entry form for PCBAID.
        /// </summary>
        /// <param name="objetyusermgt"></param>
        public AccessPassword(EntityUserManagement objetyusermgt)
        {
            InitializeComponent(); COMMONENTITY.FormStyleHelper.Apply(this);

            bMode = true;
            this.Text = "Enter PCBA ID";
            this.txtBoxPassword.PasswordChar = '\0';
            txtBoxPassword.Text = "";
            label1.Text = "PCBA ID";
            this.objetyusermgt = objetyusermgt;
        }


        /// <summary>
        /// Action to take on click of "OK" button. If form is for pcbaid entry, and entry exists in database, frmHardError class is instantiated, otherwise frmHardError class is instantiated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (bMode)
            {
                string pcbaid = txtBoxPassword.Text = txtBoxPassword.Text.Trim();
                if (!VerifyPcbaID(pcbaid)) return;
                var table = new BALLAYER.BALErrorOperations().GetHistoryTable_PCBID(pcbaid);
                if (table != null && table.Rows.Count > 0)
                {
                    frmRecordHistory frmobj = new frmRecordHistory(pcbaid, table.Rows[table.Rows.Count - 1][RejectionTable.colProductionStage].ToString(), objetyusermgt);
                    frmobj.Show();
                }
                else
                {
                    frmHardError frmobj = new frmHardError(objetyusermgt, pcbaid);
                    frmobj.Show();
                }
            }
            else
	        {
                 this.Password = txtBoxPassword.Text;
	        }
            this.Close();
        }

        private void AccessPassword_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtBoxPassword.Text = string.Empty;
            this.Close();
        }
        /// <summary>
        /// Simulates click of "OK" button when "Enter" key is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AccessPassword_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter)
            {
                if (VerifyPcbaID(txtBoxPassword.Text))
                    btnOK_Click(new object(), EventArgs.Empty);
            }
        }
        /// <summary>
        /// Verifies if PCBAID is alphanumeric.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>boolean</returns>
        private bool VerifyPcbaID(string p)
        {
            if (string.IsNullOrEmpty(p)
                || !p.All(s => Char.IsLetterOrDigit(s))
                || p.Length < 6
                )
            {
                MessageBox.Show("One or more of the following conditions were not satisfied.\n\n1. PCBAID can only be Alphabets/Numbers!\n2. PCBAID must be 6 characters or more.");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
