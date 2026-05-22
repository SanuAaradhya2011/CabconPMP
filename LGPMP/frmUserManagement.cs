using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BALLAYER;
using COMMONENTITY;
namespace CabconPMP
{
    public partial class frmUserManagement : Form
    {
        GlobalMethods objsv = new GlobalMethods();
        EntityUserManagement objetyusermgt = new EntityUserManagement();
        BALDBConnectionTest objdbcon = new BALDBConnectionTest();
        BALUserManagement objum = new BALUserManagement();
        string useractionType = string.Empty;
        public frmUserManagement(string userAction, EntityUserManagement objgetum)
        {
            InitializeComponent();
            useractionType = userAction;
            objetyusermgt.LoginuserID = objgetum.LoginuserID;
            if (userAction == "PWD") { cmbUserType.Enabled = false; txtUserID.Enabled = false; this.Text = "Change Password"; DGVUsermgt.Visible = false; this.Height = 215; }
            else { this.Text = "User Management"; this.Height = 420; }
            
        }

        private void frmUserManagement_Load(object sender, EventArgs e)
        {
            int usercnt = 0;          
            cmbUserType.Items.Clear();
            while (usercnt < StaticVariables.ApplicationUserType.Count) cmbUserType.Items.Add(StaticVariables.ApplicationUserType[usercnt++]);
            if (useractionType != "PWD") FillUserDetails();       
            else
            {
                DataSet ds = objum.Select_LoginUseronUserID(objetyusermgt);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    txtUserID.Text = ds.Tables[0].Rows[0][0].ToString();
                    cmbUserType.Text = ds.Tables[0].Rows[0][2].ToString();
                    txtnewPassword.Focus();
                }
            }
            cmbUserType.Focus();
        }
        private void FillUserDetails()
        {
               this.Cursor = Cursors.WaitCursor;
               BindingSource SBind = new BindingSource();
               DataSet dsall = objum.Select_UserDetails();
               SBind.Clear();
               DGVUsermgt.DataSource = SBind;
               if (DGVUsermgt.RowCount > 0) DGVUsermgt.Rows.Clear();
               if (DGVUsermgt.ColumnCount > 0) DGVUsermgt.Columns.Clear();
               SBind.DataSource = dsall.Tables[0];
               DGVUsermgt.DataSource = SBind;
               this.Cursor = Cursors.Default;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValidUserDetails()) return;
            objetyusermgt.LoginuserID = txtUserID.Text.Trim();
            objetyusermgt.Loginpassword= txtnewPassword.Text.Trim();
            objetyusermgt.LogType = cmbUserType.Text.Trim();

            if (MessageBox.Show("Do You Want To Save/ Update the User Details ? ", "L+G PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            if (objum.Insert_UserDetails(objetyusermgt) != "")
            {
                MessageBox.Show("Unable To Save/ Update Records !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FillUserDetails();
            MessageBox.Show("User Details Saved !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DGVUsermgt_DoubleClick(object sender, EventArgs e)
        {
            if (DGVUsermgt.Rows[DGVUsermgt.CurrentRow.Index].Cells[0].Value == null) return;            
            if (MessageBox.Show("Do You Want To View/ Edit the Selected User Details ? ", "L+G PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            txtUserID.Text = DGVUsermgt.Rows[DGVUsermgt.CurrentRow.Index].Cells[0].Value.ToString();
            txtnewPassword.Text = DGVUsermgt.Rows[DGVUsermgt.CurrentRow.Index].Cells[1].Value.ToString();
            txtConfirmnewPassword.Text = txtnewPassword.Text.Trim();
            cmbUserType.Text = DGVUsermgt.Rows[DGVUsermgt.CurrentRow.Index].Cells[2].Value.ToString();            
            
        }
        private bool IsValidUserDetails()
        {
            if (cmbUserType.SelectedIndex < 0)
            {
                MessageBox.Show("Please Select Valid User Type !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbUserType.Focus();
                return false;
            }
            if (txtUserID.Text.Trim().Length < 3)
            {
                MessageBox.Show("Please Enter Valid User ID, Length Should be >= 3  !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserID.Focus();
                return false;
            }
            if (txtnewPassword.Text.Trim().Length < 6)
            {
                MessageBox.Show("Please Enter Valid Password, Length Should be >= 6  !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtnewPassword.Focus();
                return false;
            }
            if (txtnewPassword.Text.Trim() != txtConfirmnewPassword.Text.Trim())
            {
                MessageBox.Show("New Password and Confirm Password Not Matched, Try Again !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtnewPassword.Focus();
                return false;
            }
            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbUserType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) { txtUserID.Focus(); }
        }

        private void txtUserID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) { txtnewPassword.Focus(); }
        }

        private void txtnewPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) { txtConfirmnewPassword.Focus(); }
        }

        private void txtConfirmnewPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) { btnSave_Click(this,e); }
        }
    }
}
