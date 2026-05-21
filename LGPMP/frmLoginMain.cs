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
namespace LGPMP
{
    public partial class frmLoginMain : Form
    {
        GlobalMethods objsv = new GlobalMethods();
        EntityUserManagement objetyusermgt = new EntityUserManagement();
        BALDBConnectionTest objdbcon = new BALDBConnectionTest();
        int loginLimit = 3;
         public frmLoginMain()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            BALUserManagement objum = new BALUserManagement();
            try
            {
                objetyusermgt.LoginuserID = txtuserID.Text.Trim();
                objetyusermgt.Loginpassword = txtPassword.Text.Trim();

                if (!objdbcon.IsDBFileExist())
                {
                    frmServerSettings objserversett = new frmServerSettings(false);
                    objserversett.ShowDialog();
                }
               
                if (!objdbcon.IsDBConnected())
                {
                    MessageBox.Show("Unable To Connect DataBase, Server May Down" + "\n" + "Please Contact System Administrator !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                 
                 DataSet ds = objum.Select_LoginUseronUserIDandPWD(objetyusermgt);
                 if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                 {
                    objetyusermgt.LogType = ds.Tables[0].Rows[0][2].ToString();
                    frmMain objmain = new frmMain(objetyusermgt);
                    objmain.Show();
                    this.Hide();
                   }
                    else
                    {
                        if (loginLimit-- <= 0)
                        {
                            MessageBox.Show("Unauthorized Access, Please Contact Administrator !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            Application.Exit();
                        }
                        MessageBox.Show("Invalid User , Please Enter Valid Password !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtPassword.Focus();
                        return;

                    }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            
        }

        private void frmLoginMain_Load(object sender, EventArgs e)
        {
            txtuserID.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtuserID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) { txtPassword.Focus(); }
            
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) { btnLogin_Click(this, e); }
        }

        
    }
}
