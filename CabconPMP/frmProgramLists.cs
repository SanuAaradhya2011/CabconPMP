using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using COMMONENTITY;
using BALLAYER;
namespace CabconPMP
{
    public partial class frmProgramLists : Form
    {
        EntityPrograms objentityprog = new EntityPrograms();
        BALPrograms objbalprog = new BALPrograms();
        DataTable ProgramMasterClone;
        public frmProgramLists(string prgName, EntityUserManagement objetyusermgtref)
        {
            InitializeComponent(); COMMONENTITY.FormStyleHelper.Apply(this);
            objentityprog.ProgramName = prgName;
            DataTable ProgramMasterClone = new DataTable();
            if (objetyusermgtref.LoginTypeIndex == (int)StaticVariables.userCategory.Useradministrator || objetyusermgtref.LoginTypeIndex == (int)StaticVariables.userCategory.UserPoweradministrator) { cmbMeterType.Enabled = true; lblDelete.Enabled = true; txtProgramName.Enabled = true; }
            else { lblDelete.Enabled = false; cmbMeterType.Enabled = false; txtProgramName.Enabled = false; }
           
        }
         
        private void lblAddUpdate_Click(object sender, EventArgs e)
        {
            AddParameterDetails();
            txtProgramCounts.Text = lstParameterLists.Items.Count.ToString();
            
        }

        private void lblDelete_Click(object sender, EventArgs e)
        {
            if (lstParameterLists.SelectedIndex >= 0)
            {
                if (MessageBox.Show("Do You Want To Delete Selected Parameter ?? ", "Cabcon PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                objentityprog.ProgramParameterName = lstParameterLists.SelectedItem.ToString().Trim();
                DataSet ds = objbalprog.SelecttabProgramMaster_onParametersName(objentityprog);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                  MessageBox.Show("You Cann't Delete The Selected Parameter That is Already Used in" + "\n" + "Meter Type : " + ds.Tables[0].Rows[0][1].ToString() + "\n" + "Test Procedure : " + ds.Tables[0].Rows[0][3], "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                  return;
                }
                lstParameterLists.Items.Remove(lstParameterLists.SelectedItem);
                txtProgramCounts.Text = lstParameterLists.Items.Count.ToString();
            }
                   
        }

        private void txtProgramName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetterOrDigit(e.KeyChar) ){}
            else if (e.KeyChar == (char)Keys.Enter) txtParametersName.Focus();
            else e.Handled = e.KeyChar != (char)Keys.Back; 
        }
        private void txtParametersName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) AddParameterDetails();
            txtParametersName.Focus();
        }

        private void AddParameterDetails()
        {
            if (txtProgramName.Text.Trim().Length <= 2)
            {
                MessageBox.Show("Please Enter Valid Program Name !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtParametersName.Text.Trim().Length <= 2)
            {
                MessageBox.Show("Please Enter Valid Parameter Name !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!lstParameterLists.Items.Contains(txtParametersName.Text.Trim())) lstParameterLists.Items.Add(txtParametersName.Text.ToUpper().Trim());
            else
            {
                MessageBox.Show("Parameter Name Already Exist !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void lblSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (MessageBox.Show("Do You Want To Save the Program Details ?? ", "Cabcon PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                Application.DoEvents();
                this.Cursor = Cursors.WaitCursor;
                string resultmsg = string.Empty;
                if (txtProgramName.Text.Trim().Length <= 2)
                {
                    MessageBox.Show("Please Enter Valid Program Name !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (lstParameterLists.Items.Count <= 0)
                {
                    MessageBox.Show("Please Add Valid Program Name !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Application.DoEvents();
                    return;
                }
               
                BALPrograms objbalprog = new BALPrograms();
                objentityprog.ProgramName = txtProgramName.Text.Trim();

                int gridrCnr = 0;
                DataRow dr;
                ProgramMasterClone = ProgramMasterClone.Clone();
                while (gridrCnr < lstParameterLists.Items.Count)
                {
                    dr = ProgramMasterClone.NewRow();
                    dr["ProgramName"] = objentityprog.ProgramName;
                    dr["ParameterName"] = lstParameterLists.Items[gridrCnr].ToString(); ;
                    dr["TestType"] = cmbMeterType.Text;
                    ProgramMasterClone.Rows.Add(dr);
                    gridrCnr++;
                }               
                resultmsg = objbalprog.InsertinToTabProgramMasterByBatch(objentityprog, ProgramMasterClone);
                if (resultmsg != "")
                {
                    MessageBox.Show(resultmsg, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                
                MessageBox.Show("Program Details Saved !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                     
                Application.DoEvents();
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

        private void frmProgramLists_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                cmbMeterType.DataSource = StaticVariables.GetMeterType();
                txtProgramName.Text = objentityprog.ProgramName;
                DataSet ds = objbalprog.SelectFromTabProcedureMasteronProcedureNameonProgramName(objentityprog);
                ProgramMasterClone = ds.Tables[0].Clone();
                if (objentityprog.ProgramName.Length > 0) DisplayListItem(ds);            
                if (cmbMeterType.SelectedIndex < 0 &&  cmbMeterType.Items.Count > 0) cmbMeterType.SelectedIndex = 0;     
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

        private void lblClose_Click(object sender, EventArgs e)
        {
            if (lstParameterLists.Items.Count >= 1) lblSave_Click(this, e);           
            this.Close();
        }

        private void lstParameterLists_DoubleClick(object sender, EventArgs e)
        {
            if (lstParameterLists.SelectedIndex >= 0) txtParametersName.Text = lstParameterLists.Items[lstParameterLists.SelectedIndex].ToString();
        }

        private void lstParameterLists_Click(object sender, EventArgs e)
        {
            txtParametersName.Text = "";
        }
 
        private void DisplayListItem(DataSet ds)
        {             
            txtProgramName.Text = "";
            lstParameterLists.Items.Clear();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cmbMeterType.Text = ds.Tables[0].Rows[0][2].ToString();
                txtProgramName.Text = cmbMeterType.Text = ds.Tables[0].Rows[0][0].ToString();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lstParameterLists.Items.Add(dr[1].ToString());
                    Application.DoEvents();
                }
            }
            txtProgramCounts.Text = lstParameterLists.Items.Count.ToString();
        }

        private void cmbMeterType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbMeterType.SelectedIndex >= 0)
            {
                objentityprog.TestType = cmbMeterType.Text;
                DataSet ds = objbalprog.Pro_Select_tabProgramMaster_onMeterType(objentityprog);
                DisplayListItem(ds);
            }
        }

              
    }
}
