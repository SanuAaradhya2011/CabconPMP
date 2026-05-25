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
    public partial class frmProgramListOpen : Form
    {
        EntityPrograms objentityprog = new EntityPrograms();
        BALPrograms objbalprog = new BALPrograms();
        BALDBConnectionTest objdbcon = new BALDBConnectionTest();
        EntityUserManagement objentyUM;
        public frmProgramListOpen(EntityUserManagement objetyusermgtref)
        {
            InitializeComponent();
            if (objetyusermgtref.LoginTypeIndex == (int)StaticVariables.userCategory.Useradministrator || objetyusermgtref.LoginTypeIndex == (int)StaticVariables.userCategory.UserPoweradministrator) btnDelete.Enabled = true;
            else btnDelete.Enabled = false;

            objentyUM = objetyusermgtref;
        }

        private void frmProgramListOpen_Load(object sender, EventArgs e)
        {
            GetProgramList();
        }

        private void GetProgramList()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (!objdbcon.IsDBConnected())
                {
                    MessageBox.Show("Unable To Connect DataBase, Server May Down" + "\n" + "Plase Contact System Administrator !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.DoEvents();
                    this.Close();
                    Application.DoEvents();
                    return;
                }
                DataSet ds = objbalprog.SelectDistinctFromTabProgramMaster();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    lstProgramName.Items.Clear();
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        lstProgramName.Items.Add(dr[0].ToString());
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnopen_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lstProgramName.SelectedIndex >= 0)
                {

                    if (MessageBox.Show("Do You Want To open Selected Program Details ?? ", "Cabcon PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                    frmProgramLists objfrmprog = new frmProgramLists(lstProgramName.SelectedItem.ToString(), objentyUM);
                    objfrmprog.ShowDialog();

                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstProgramName.SelectedIndex >= 0)
                {

                    if (MessageBox.Show("Do You Want To Delete Selected Program ?? ", "Cabcon PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                    objentityprog.ProgramName = lstProgramName.SelectedItem.ToString();
                    string resultmsg = objbalprog.DeleteTabProgramMaster_onProgramNameOnly(objentityprog);
                    if (resultmsg != "")
                    {
                        MessageBox.Show("Unable To Delete !" + "\n" + resultmsg, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else lstProgramName.Items.RemoveAt(lstProgramName.SelectedIndex);
                    MessageBox.Show("Selected Program Deleted !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Unable To Delete Selected Program !" + "\n" + Ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetProgramList();
        }
    }
}
