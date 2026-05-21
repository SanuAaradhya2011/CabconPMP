using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using ApplicationInterface;
using Utilities;
using SerialCommunication;
using COMMONENTITY;
using BALLAYER;
using SystemSecurityLibrary;
namespace LGPMP
{

    public partial class frmProcedure : Form
    {
        public delegate void UpdateMainMsgHandler(object sender, UpdateEventArgs e);
        public event UpdateMainMsgHandler UpdateMsg;
        UpdateEventArgs args = null;
        StandardDateTime stddt = new StandardDateTime();
        EntityProcedure objentitypro = new EntityProcedure();        
        BALProcedure objbalprocedure = new BALProcedure();
        XMLExportImport objexpimp = new XMLExportImport();
        EntityPrograms objentityprog = new EntityPrograms();
        BALPrograms objbalprog = new BALPrograms();
        DataTable executionResultClone;
        int procedureRindex = -1;
        decimal objdec;
        string procedureName;
        public frmProcedure(string pName, EntityUserManagement objetyusermgtref)
        {
            InitializeComponent();
            Application.DoEvents();
            Application.DoEvents();
            procedureName = pName;
            lblProcedureName.Text = pName;

            if (objetyusermgtref.LoginTypeIndex == (int)StaticVariables.userCategory.Useradministrator || objetyusermgtref.LoginTypeIndex == (int)StaticVariables.userCategory.UserPoweradministrator) { lblDelete.Enabled = true; cmbProgramList.Enabled = true; }
            else { lblDelete.Enabled = false; cmbProgramList.Enabled = false; }
        }
        public void AddressForm_PingLed(object sender, UpdateEventArgs e)
        {
            args = new UpdateEventArgs(e.msg, e.isError);
            UpdateMsg(this, args);
        } 
      
        private void lblNew_Click(object sender, EventArgs e)
        {
            ResetAll();
        }

        private void ResetAll()
        {
            grpMeterIDRange.Enabled = false;
            chkStatus.Checked = true;
            txtDefaultValue.Text = string.Empty;
            txtMinValue.Text = string.Empty;
            txtMaxValue.Text = string.Empty;
            DGVProcedure.DataSource = null;
            cmbMidDegits.SelectedIndex = 1;

        }
        private void SM110frmProcedure_Load(object sender, EventArgs e)
        {
            ResetAll();
            FillDefaultData();
            ViewProcedureDetails();
            FillProgramParameterList();
        }
        private void FillDefaultData()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;               
                cmbMeterType.DataSource = StaticVariables.GetMeterType();
                if (cmbMeterType.Items.Count >= 0) cmbMeterType.SelectedIndex = 0;
                
                cmbProcedureType.DataSource = StaticVariables.GetTestType();
                if (cmbProcedureType.Items.Count >= 0) cmbProcedureType.SelectedIndex = 0;
                
                FillProgramList();

                
            }
            catch (Exception)
            {
               
            }            
            finally
            {
                this.Cursor = Cursors.Default;
            }
                       
        }
        private void FillProgramList()
        {           
            DataSet ds = objbalprog.SelectDistinctFromTabProgramMaster();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cmbProgramList.DataSource = ds.Tables[0].DefaultView;
                cmbProgramList.DisplayMember = "ProgramName";
                if (cmbProgramList.Items.Count >= 0) cmbProgramList.SelectedIndex = 0;
            }
           
        }

        private void FillProgramParameterList()
        {
            if (cmbProgramList.SelectedIndex >= 0)
            {
                objentityprog.ProgramName = cmbProgramList.Text.Trim();
                DataSet ds = objbalprog.SelectFromTabProcedureMasteronProcedureNameonProgramName(objentityprog);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cmbParametersName.DataSource = ds.Tables[0].DefaultView;
                    cmbParametersName.DisplayMember = "ParameterName";
                    if (cmbParametersName.Items.Count >= 0) cmbParametersName.SelectedIndex = 0;
                    cmbMeterType.Text = ds.Tables[0].Rows[0][2].ToString();
                }
            }
        }
        private void ViewProcedureDetails()
        {
            try
            {
                txtProcedureName.Text = procedureName;
                lblProcedureName.Text = procedureName;
                objentitypro.ProcedureName = procedureName;
                DataSet ds = objbalprocedure.SelectFromTabProcedureMasteronProcedureNAme(objentitypro);
                executionResultClone = ds.Tables[0].Clone();
                if (procedureName.Length > 0)
                {                                     
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        cmbMeterType.Text = dt.Rows[0][1].ToString();
                        cmbProcedureType.Text = dt.Rows[0][2].ToString();
                        txtProcedureName.Text = dt.Rows[0][3].ToString();
                        BitArray otherSettings = new BitArray(new int[] { Convert.ToInt32(dt.Rows[0][4].ToString()) });
                        chkDisablemanualScan.Checked = otherSettings[0];
                        chkExecutionWithoutTraveler.Checked = otherSettings[1];
                        //if (dt.Rows[0][4].ToString() == "0") chkDisablemanualScan.Checked = false;
                        //else chkDisablemanualScan.Checked = true;
                        cmbProgramList.Text = dt.Rows[0][5].ToString();
                        DisplayMeterIDRange(dt.Rows[0]["MeterIDRange"].ToString());
                        int rcnt = 0 ;
                        foreach (DataRow dr in dt.Rows)
                        {
                            DGVProcedure.Rows.Add();
                            DGVProcedure.Rows[rcnt].Cells[0].Value = (rcnt+1).ToString();
                            DGVProcedure.Rows[rcnt].Cells[1].Value = dr[6].ToString();
                            DGVProcedure.Rows[rcnt].Cells["colDefaultValue"].Value = dr[7].ToString();
                            DGVProcedure.Rows[rcnt].Cells["ColMinVal"].Value = dr[8].ToString();
                            DGVProcedure.Rows[rcnt].Cells["ColMaxValue"].Value = dr[9].ToString();
                            DGVProcedure.Rows[rcnt].Cells["colStatus"].Value = (bool)dr[10];
                            rcnt++;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
                       
        }    


        private void lblAddUpdate_Click(object sender, EventArgs e)
        {
            ProcedureAddUpdate();
        }

        private void ProcedureAddUpdate()
        {
            procedureRindex = DGVProcedure.RowCount;
            int totalProcedureCount = 0;
            
            while (totalProcedureCount < DGVProcedure.Rows.Count )
            {
                if (DGVProcedure.Rows[totalProcedureCount].Cells[1].Value.ToString() == cmbParametersName.Text){  break;}
                totalProcedureCount++;
            }
            if (!IsValidRangeParameters()) return;
            if (!IsParaAlreadyExist())
            {
                if (MessageBox.Show("Are You Sure To Add This Parameter ?? ", "L+G PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                DGVProcedure.Rows.Add();
                DGVProcedure.Rows[procedureRindex].Cells[0].Value = (DGVProcedure.RowCount).ToString();
            }
            else
            {
                if (MessageBox.Show("Are You Sure To Update Selected Parameter ?? ", "L+G PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                procedureRindex = totalProcedureCount;
            }
            //-----------------------------------------------------------------------------
           
            //-----------------------------------------------------------------------------
            DGVProcedure.Rows[procedureRindex].Cells[1].Value = cmbParametersName.Text;
            DGVProcedure.Rows[procedureRindex].Cells["colDefaultValue"].Value = txtDefaultValue.Text;
            DGVProcedure.Rows[procedureRindex].Cells["ColMinVal"].Value = txtMinValue.Text;
            DGVProcedure.Rows[procedureRindex].Cells["ColMaxValue"].Value = txtMaxValue.Text;
            DGVProcedure.Rows[procedureRindex].Cells["colStatus"].Value = chkStatus.Checked;
            procedureRindex = -1;
        }

        private bool IsValidRangeParameters()
        {
            string[] minValue = txtMinValue.Text.Trim().Split(',');
            string[] maxValue = txtMaxValue.Text.Trim().Split(',');
            int pralistCnt = 0;
            string minformattedValue = "";
            string maxformattedValue = "";
            bool isValidMinMaxRange;
            while (pralistCnt < minValue.Length)
            {
                minformattedValue = "";
                maxformattedValue = "";
                isValidMinMaxRange = true;
                if (pralistCnt < minValue.Length && minValue[pralistCnt] != "")
                {
                  if (!Decimal.TryParse(minValue[pralistCnt], out objdec)) isValidMinMaxRange = false;
                  else minformattedValue =  minValue[pralistCnt] ;
                }
                if (pralistCnt < maxValue.Length && maxValue[pralistCnt] != "")
                {
                  if (!Decimal.TryParse(maxValue[pralistCnt], out objdec)) isValidMinMaxRange = false;
                  else maxformattedValue =  maxValue[pralistCnt] ;
                }
                if (minformattedValue != "" && maxformattedValue != "")
                {
                    if (Convert.ToDecimal(minformattedValue) > Convert.ToDecimal(maxformattedValue)) isValidMinMaxRange = false;
                }
                if (!isValidMinMaxRange)
                {
                    MessageBox.Show("Invalid Min or Max Value Range !" + "\n" + "Min. Value < Max. Value", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                pralistCnt++;
            }
            return true;
        }

        private bool IsParaAlreadyExist()
        {
            int totalProcedureCount = 0;
            bool ValueAlreadyAdded = false;
            while (totalProcedureCount < DGVProcedure.Rows.Count)
            {
                if (DGVProcedure.Rows[totalProcedureCount].Cells[1].Value.ToString() == cmbParametersName.Text) { ValueAlreadyAdded = true; break; }
                totalProcedureCount++;
            }
            if (!ValueAlreadyAdded || procedureRindex <= 0) { return false; }
            else return true;
        }

        private void lblDelete_Click(object sender, EventArgs e)
        {
            procedureRindex = DGVProcedure.CurrentRow.Index;
            if (procedureRindex >= 0 && DGVProcedure.Rows[procedureRindex].Cells[0].Value != null)
            {
                if (MessageBox.Show("Are You Sure To Delete Selected Procedure ?? ", "L+G PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
               
                DGVProcedure.Rows.RemoveAt(procedureRindex);
                int totalProcedureCount = 0;
                while (totalProcedureCount < DGVProcedure.Rows.Count - 1)
                {
                    DGVProcedure.Rows[totalProcedureCount].Cells[0].Value = (totalProcedureCount + 1).ToString();
                    totalProcedureCount++;
                }
            }
            else
            {
              MessageBox.Show("Please Select Valid Row To Delete ?? ", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
            }
            procedureRindex = -1;

        }

        private void DisplayMeterIDRange(string listValue)
        {
            string[] meterIDlist = listValue.Split(',');
            if (meterIDlist != null && meterIDlist.Length > 3) { cmbMidDegits.Text = meterIDlist[0].Trim(); txtMeterIDPrefix.Text = meterIDlist[1].Trim(); txtMeterIDFrom.Text = meterIDlist[2].Trim(); txtMeterIDTO.Text = meterIDlist[3].Trim(); }
                        
        }
        private void lblSave_Click(object sender, EventArgs e)
        {
            try
            {
               // txtDefaultValue.Text = txtMinValue.Text = txtMaxValue.Text = "";
                if (!ISValidProcedure()) return;               
                if (MessageBox.Show("Are You Sure To Save the Procedure Details ?? ", "L+G PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                Application.DoEvents();
                this.Cursor = Cursors.WaitCursor;
                if (cmbProcedureType.SelectedIndex == 1)
                {
                    if (!VerifyMeterIDRangeGeneration()) return;
                    objentitypro.MeterIDList = cmbMidDegits.Text.Trim() + "," + txtMeterIDPrefix.Text.Trim() + "," + txtMeterIDFrom.Text.Trim() + "," + txtMeterIDTO.Text.Trim(); 
                 }               
                string resultmsg = string.Empty;               
                objentitypro.MeterType = cmbMeterType.Text;
                objentitypro.ProcedureType = cmbProcedureType.Text;
                objentitypro.ProcedureName = txtProcedureName.Text;
                //objentitypro.SerializationWindow = cmbSerialisationWindow.SelectedIndex;
                objentitypro.ProgramName = cmbProgramList.Text;
                objentitypro.ParaUpdatedDateTime = DateTime.Now;
               
                //---------------Fill Datatable--------------------
                executionResultClone = executionResultClone.Clone();
                executionResultClone.Clear();
                int recCount = 0;
                DataRow dr;
                BitArray myBitArr = new BitArray(2);
                myBitArr[0] = chkDisablemanualScan.Checked;
                myBitArr[1] = chkExecutionWithoutTraveler.Checked;
                int[] SerializationWindowValues = new int[1];
                myBitArr.CopyTo(SerializationWindowValues, 0);
              
                while (recCount < DGVProcedure.Rows.Count)
                {                     
                    dr = executionResultClone.NewRow();
                    dr["ProcedureID"] = recCount+1;
                    dr["MeterType"] = objentitypro.MeterType;
                    dr["ProceduteType"] =objentitypro.ProcedureType;
                    dr["ProcedureName"] = objentitypro.ProcedureName;
                    dr["SerializationWindow"] = SerializationWindowValues[0]; //---Manual Scan & Traveler Execution
                    //if (chkDisablemanualScan.Checked) dr["SerializationWindow"] = 1; //---Manual Scan Disable
                    //else dr["SerializationWindow"] = 0;
                    dr["ProgramName"] = objentitypro.ProgramName;                  
                    dr["ParametersName"] = DGVProcedure.Rows[recCount].Cells["ParametersName"].Value.ToString();
                    dr["ParaDefaultValue"] = DGVProcedure.Rows[recCount].Cells["colDefaultValue"].Value.ToString();
                    dr["ParaMinValue"] = DGVProcedure.Rows[recCount].Cells["ColMinVal"].Value.ToString();
                    dr["ParaMaxValue"] = DGVProcedure.Rows[recCount].Cells["ColMaxValue"].Value.ToString();
                    dr["ParaActivationStatus"] = (bool)DGVProcedure.Rows[recCount].Cells["colStatus"].Value;
                    dr["ParaUpdatedDateTime"] = objentitypro.ParaUpdatedDateTime;
                    dr["MeterIDRange"] = objentitypro.MeterIDList;
                    executionResultClone.Rows.Add(dr);
                    recCount++;
                }
               
                resultmsg = objbalprocedure.InsertinToTabProcedureMasterByBatch(objentitypro, executionResultClone);
                if (resultmsg != "")
                {
                    MessageBox.Show(resultmsg, "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                
                MessageBox.Show("Procedure Saved !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                  
                 
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void cmbMeterType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                
                cmbProcedureType.Focus();
            }
        }

        private void cmbProsedureName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) txtProcedureName.Focus();
        }

        private void cmbProgramList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) cmbParametersName.Focus();
        }

        private void cmbParametersName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) txtDefaultValue.Focus();
        }
       
        private void txtDefaultValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) txtMinValue.Focus();
        }

        private void txtMinValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) txtMaxValue.Focus();
        }
        private void txtMaxValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ProcedureAddUpdate();
                cmbParametersName.Focus();
            }

        }      

        private bool ISValidProcedure()
        {
            
            if (cmbMeterType.SelectedIndex < 0)
            {
                MessageBox.Show("Please Select Valid Meter Type ! ", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cmbMeterType.Focus();
                return false;
            }
            if (cmbProcedureType.SelectedIndex < 0)
            {
                MessageBox.Show("Please Select Valid Test type ! ", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cmbProcedureType.Focus();
                return false;
            }

            if (txtProcedureName.Text.Trim().Length <= 0) 
            {
                MessageBox.Show("Please Provide Valid Test Procedure Name ! ", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtProcedureName.Focus();
                return false; 
            }
           
            if (cmbProgramList.SelectedIndex < 0)
            {
                MessageBox.Show("Please Select Valid Program Name ! ", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cmbProgramList.Focus();
                return false;
            }
            if (cmbParametersName.SelectedIndex < 0)
            {
                MessageBox.Show("Please Select Valid Test Parameter Name ! ", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cmbParametersName.Focus();
                return false;
            }
            //bool isinvalid = true;
            //if (txtMinValue.Text.Trim() != "" && txtMaxValue.Text.Trim() != "")
            //{
            //    if (!Decimal.TryParse(txtMinValue.Text.Trim(), out objdec)) isinvalid = false;
            //    if (!Decimal.TryParse(txtMaxValue.Text.Trim(), out objdec)) isinvalid = false;
            //    if (Convert.ToDecimal(txtMinValue.Text.Trim()) > Convert.ToDecimal(txtMaxValue.Text.Trim())) isinvalid = false;
            //    if (!isinvalid)
            //    {
            //        MessageBox.Show("Please Provide Valid Selection or Range ! ", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        return false;
            //    }
            //}
            return true;
        }

        private void cmbParametersName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbParametersName.Text.IndexOf("Voltage") >= 0) { txtDefaultValue.Text = "240"; txtMinValue.Text = "210"; txtMaxValue.Text = "260"; }
            else if (cmbParametersName.Text.IndexOf("Current") >= 0) { txtDefaultValue.Text = "10"; txtMinValue.Text = "8"; txtMaxValue.Text = "12"; }
            else if (cmbParametersName.Text.IndexOf("Manuf") >= 0) { txtDefaultValue.Text = String.Format("{0:yyyy}", DateTime.Now); txtMinValue.Text = ""; txtMaxValue.Text = ""; }
            else { txtDefaultValue.Text = ""; txtMinValue.Text = ""; txtMaxValue.Text = ""; }
           
        }

       
        private void lblEdit_Click(object sender, EventArgs e)
        {
            procedureRindex = DGVProcedure.CurrentRow.Index;
            if (procedureRindex >= 0 && procedureRindex < DGVProcedure.RowCount)
            {
                cmbParametersName.Text = DGVProcedure.Rows[procedureRindex].Cells[1].Value.ToString();
                txtDefaultValue.Text = DGVProcedure.Rows[procedureRindex].Cells["colDefaultValue"].Value.ToString();
                txtMinValue.Text = DGVProcedure.Rows[procedureRindex].Cells["ColMinVal"].Value.ToString();
                txtMaxValue.Text = DGVProcedure.Rows[procedureRindex].Cells["ColMaxValue"].Value.ToString();
                chkStatus.Checked = (bool)DGVProcedure.Rows[procedureRindex].Cells["colStatus"].Value;
            }
            else
            {
                MessageBox.Show("Please Select Valid Row !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void txtProcedureName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) cmbProgramList.Focus();
        }

        private void cmbSerialisationWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) cmbProgramList.Focus();
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            if (DGVProcedure.Rows.Count >= 1)lblSave_Click(this, e);
            this.Close();
        }

        private void lblInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsValidRangeParameters()) return;
                if (IsParaAlreadyExist())
                {
                    MessageBox.Show("Parameter Already Exist Cann't Add Duplicate Parameter Name !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (DGVProcedure.CurrentRow == null)
                {
                    MessageBox.Show("Please Select Valid Row To Insert ?? ", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                procedureRindex = DGVProcedure.CurrentRow.Index;
                if (procedureRindex >= 0 && DGVProcedure.Rows[procedureRindex].Cells[0].Value != null)
                {
                    if (MessageBox.Show("Are You Sure To Insert Parameters Details Above Selected Location ?? ", "L+G PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                    DataGridViewRow row = (DataGridViewRow)DGVProcedure.Rows[0].Clone();
                    row.Cells[1].Value = "1";
                    row.Cells[1].Value = cmbParametersName.Text;
                    row.Cells[2].Value = txtDefaultValue.Text;
                    row.Cells[3].Value = txtMinValue.Text;
                    row.Cells[4].Value = txtMaxValue.Text;
                    row.Cells[5].Value = chkStatus.Checked;
                    DGVProcedure.Rows.InsertRange(procedureRindex, row);
                    int totalProcedureCount = 0;
                    while (totalProcedureCount < DGVProcedure.Rows.Count - 1)
                    {
                        DGVProcedure.Rows[totalProcedureCount].Cells[0].Value = (totalProcedureCount + 1).ToString();
                        totalProcedureCount++;
                    }
                    procedureRindex = -1;
                }
                else
                {
                    MessageBox.Show("Please Select Valid Row To Delete ?? ", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                procedureRindex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }
 
        private void cmbProgramList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                FillProgramParameterList();
            }
            catch (Exception)
            {

            }
        }

        private void cmbMeterType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbMeterType.SelectedIndex >= 0)
            {
                objentityprog.TestType = cmbMeterType.Text;
                DataSet ds = objbalprog.Pro_Select_tabProgramMaster_onMeterType(objentityprog);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cmbProgramList.Text = ds.Tables[0].Rows[0][0].ToString();
                }
                FillProgramParameterList();
            }
        }

        private void cmbProcedureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProcedureType.SelectedIndex == 1) { grpMeterIDRange.Enabled = true; chkDisablemanualScan.Checked = false; chkDisablemanualScan.Enabled = false; }
            else { txtMeterIDFrom.BackColor = Color.White; txtMeterIDTO.BackColor = Color.White; grpMeterIDRange.Enabled = false; chkDisablemanualScan.Enabled = true; }
        }

        private void txtMeterIDPrefix_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsLetter(e.KeyChar)) e.Handled = true;             
        }

        private void txtMeterIDFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void txtMeterIDTO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void txtMeterIDPrefix_Leave(object sender, EventArgs e)
        {
            FormatMeterIDLength();
        }

        private void txtMeterIDFrom_Leave(object sender, EventArgs e)
        {
            FormatMeterIDLength();
        }

        private void txtMeterIDTO_Leave(object sender, EventArgs e)
        {
            FormatMeterIDLength();
        }
        private bool FormatMeterIDLength()
        {
            try
            {
                int maxlen = Convert.ToInt32(cmbMidDegits.Text.Trim()) - txtMeterIDPrefix.Text.Trim().Length;
                if (txtMeterIDFrom.Text.Trim().Length > 0) txtMeterIDFrom.Text = Convert.ToInt64(txtMeterIDFrom.Text).ToString().Trim().PadLeft(maxlen, '0');
                if (txtMeterIDTO.Text.Trim().Length > 0) txtMeterIDTO.Text = Convert.ToInt64(txtMeterIDTO.Text).ToString().Trim().PadLeft(maxlen, '0');
                if (Convert.ToInt64(cmbMidDegits.Text.Trim()) < txtMeterIDFrom.Text.Trim().Length + txtMeterIDPrefix.Text.Trim().Length) 
                { 
                    txtMeterIDFrom.BackColor = Color.Red;
                    MessageBox.Show("Meter ID Length and From Range Digit Value Not Matched !" + "\n" + "Meter ID (Prefix + Range Disgit) Length Should be Equal To Total Meter ID Length !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                else txtMeterIDFrom.BackColor = Color.White;
                if (Convert.ToInt32(cmbMidDegits.Text.Trim()) < txtMeterIDTO.Text.Trim().Length + txtMeterIDPrefix.Text.Trim().Length)
                {
                    txtMeterIDTO.BackColor = Color.Red;
                    MessageBox.Show("Meter ID Length and To Range Digit Value Not Matched !" + "\n" + "Meter ID (Prefix + Range Disgit) Length Should be Equal To Total Meter ID Length !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                else txtMeterIDTO.BackColor = Color.White;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable To Generate Meter ID List !" + "\n\n" + ex.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            } 
            
        }

        private void cmbMidDegits_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormatMeterIDLength();
        }
        /// <summary>
        /// This Method is used only for Verification of Meter ID List Generation
        /// </summary>
        /// <returns></returns>
        private bool VerifyMeterIDRangeGeneration()
        { 
            long objlong;
            if (!FormatMeterIDLength()) return false;
            if (!long.TryParse(txtMeterIDFrom.Text.Trim(), out objlong))
            {
                MessageBox.Show("Meter ID Range From Value Should be Valid Numeric only !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMeterIDFrom.Focus();
                return false;
            }
            if (!long.TryParse(txtMeterIDTO.Text.Trim(), out objlong))
            {
                MessageBox.Show("Meter ID Range From Value Should be Valid Numeric only !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMeterIDTO.Focus();
                return false;
            }
            long mIDcnt = Convert.ToInt64(txtMeterIDFrom.Text.Trim());
            long maxIDcnt = Convert.ToInt64(txtMeterIDTO.Text.Trim());
            if (maxIDcnt - mIDcnt >= 100000)
            {
                MessageBox.Show("Meter ID Range Diffrence Should Not Be > 1 Lac !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMeterIDFrom.Focus();
                return false;
            }
            if (maxIDcnt - mIDcnt < 0)
            {
                MessageBox.Show("Meter ID Range Diffrence Should Not Be > 1 !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMeterIDFrom.Focus();
                return false;
            }
            int maxlen = Convert.ToInt32(cmbMidDegits.Text.Trim()) - txtMeterIDPrefix.Text.Trim().Length;
            txtMeterIDFrom.Text = mIDcnt.ToString().Trim().PadLeft(maxlen, '0');
            txtMeterIDTO.Text = maxIDcnt.ToString().Trim().PadLeft(maxlen, '0');            
            try
            {
                int count = 0;
                while (mIDcnt <= maxIDcnt)
                {
                    string temmID = txtMeterIDPrefix.Text.Trim() + (mIDcnt++).ToString().PadLeft(maxlen, '0');
                    Application.DoEvents();
                    if (count++ >= 5) break;
                }
                return true;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid Meter ID Range Parameters !" + "\n\n" + ex.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }            
        }
        private void chkExecutionWithoutTraveler_Click(object sender, EventArgs e)
        {
            if (chkExecutionWithoutTraveler.Checked)
                if (MessageBox.Show("Do you want to Disable the Production Traveler stage Write Command ?", "L+G PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) chkExecutionWithoutTraveler.Checked = false;
        }
    }
}
