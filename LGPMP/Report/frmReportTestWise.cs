using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
 
using System.Data.SqlClient;
using COMMONENTITY;
using BALLAYER;
using Utilities;
using SystemSecurityLibrary;
namespace LGPMP
{
    public partial class frmReportTestWise : Form
    {
        StandardDateTime std = new StandardDateTime();
        BALExecutionResults objER = new BALExecutionResults();
        EntityExecutionResult objettER = new EntityExecutionResult();
        BALDBConnectionTest objdbcon = new BALDBConnectionTest();
        DataSet ds = new DataSet();
        EntityPrograms objentityprog = new EntityPrograms();
        BALPrograms objbalprog = new BALPrograms();
        public frmReportTestWise()
        {
            InitializeComponent();
        }

        private void frmResultsReport_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (!objdbcon.IsDBConnected())
                {
                    MessageBox.Show("Unable To Connect DataBase, Server May Down" + "\n" + "Plase Contact System Administrator !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.DoEvents();
                    this.Close();
                    Application.DoEvents();
                    return;
                }

                XMLExportImport objexpimp = new XMLExportImport();               
                
                GetDistinctMeterType();              
                cmbStatus.SelectedIndex = 0;                
                txtFindfrom.Text = String.Format("{0:dd/MM/yyyy}", DateTime.Now.AddDays(-(DateTime.Now.Day - 1))); txtfindto.Text = String.Format("{0:dd/MM/yyyy}", DateTime.Now); 
                txtFindfrom.Focus();
                List<string> mtypeList = StaticVariables.GetTestType();
                cmbTestType.DataSource = mtypeList;
                if (cmbTestType.Items.Count >= 0) cmbTestType.SelectedIndex = 0;
                cmbSearchBy.SelectedIndex = 0;
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
        private void GetDistinctMeterType()
        {
            cmbMeterType.DataSource = StaticVariables.GetMeterType();
            if (cmbMeterType.Items.Count >= 0) cmbMeterType.SelectedIndex = 0;            
        }
         
        private void lblFind_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            DGVTestResult.DataSource = null;
            BindingSource SBind = new BindingSource();
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
            ts_Menu.Enabled = false;
            try
            {
                if (!objdbcon.IsDBConnected())
                {
                    MessageBox.Show("Unable To Connect To DataBase, Server May Down" + "\n" + "Plase Contact System Administrator !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtFindfrom.Text.Trim().Length <= 0)
                {
                    MessageBox.Show("Plase Provide Valid Input For Data Search !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtFindfrom.Focus();
                    return;
                }
                if (txtfindto.Text.Trim().Length <= 0)
                {
                    MessageBox.Show("Plase Provide Valid Input For Data Search !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtfindto.Focus();
                    return;
                }
                if (cmbSearchBy.SelectedIndex > 0 && txtFixString.Text.Trim().Length <= 0)
                {
                    MessageBox.Show("Plase Provide Valid Input For range Prefix !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtFixString.Focus();
                    return;
                }
                if (cmbSearchBy.SelectedIndex == 0) ds = GetDataByDateRange();
                else if (cmbSearchBy.SelectedIndex == 1) ds = GetDataByMeterIDRange();
                else if (cmbSearchBy.SelectedIndex == 2) ds = GetDataByPCBAIDRange();
                else
                {
                    MessageBox.Show("Plase Select a Valid Search Type !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cmbSearchBy.Focus();
                    return;
                }
                if (ds != null)
                {
                    DataView dv = ds.Tables[0].DefaultView;
                    dv.Sort = "PCBAID asc";
                    DataTable sortedDT = dv.ToTable();

                    SBind.Clear();
                    DGVTestResult.DataSource = SBind;
                    if (DGVTestResult.RowCount > 0) DGVTestResult.Rows.Clear();
                    if (DGVTestResult.ColumnCount > 0) DGVTestResult.Columns.Clear();
                    SBind.DataSource = sortedDT;
                    DGVTestResult.DataSource = SBind;
                    txttotalrecCount.Text = sortedDT.Rows.Count.ToString();
                    
                    for(int colcount=0;colcount <DGVTestResult.ColumnCount;colcount++){this.DGVTestResult.Columns[colcount].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;}
                }
                else
                {
                    MessageBox.Show("Unable To Connect To DataBase, Server May Down" + "\n" + "Plase Contact System Administrator !" + "\n" + "Data Not Found OR Unable to Retrive Data ! !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable To Connect To DataBase, Invalid Response !" + "\n\n" + ex.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                ts_Menu.Enabled = true;
            }
        }
        private System.Data.DataSet GetDataByDateRange()
        {
           
            try
            {
                bool isvalidStartDateTime = true;
                bool isvalidEndDateTime = true;
                int objint;

                objettER.ExecutionMeterType = cmbMeterType.Text.Trim();
                objettER.ExecutionProgramName = cmbparametersName.Text;
                objettER.ExecutionProcedureType = cmbTestType.Text.Trim();
                objettER.FinalResult = cmbStatus.Text.Trim().ToUpper();  //LatestStatus
                if (cmbStatus.SelectedIndex <= 0) objettER.FinalResult = "";//-------Show All              
                if (txtFindfrom.Text.Length >= 6 && txtfindto.Text.Length >= 6)
                {
                    string[] dtStart = txtFindfrom.Text.Trim().Split('/');
                    string[] dtEnd = txtfindto.Text.Trim().Split('/');

                    if (dtStart.Length >= 3 && int.TryParse(dtStart[0], out objint) && int.TryParse(dtStart[1], out objint) && int.TryParse(dtStart[2], out objint)) { }
                    else isvalidStartDateTime = false;

                    if (dtEnd.Length >= 3 && int.TryParse(dtEnd[0], out objint) && int.TryParse(dtEnd[1], out objint) && int.TryParse(dtEnd[2], out objint)) { }
                    else isvalidEndDateTime = false;

                    if (isvalidStartDateTime && isvalidEndDateTime)
                    {
                        objettER.ExecutionStatusStart = new DateTime(Convert.ToInt16(dtStart[2]), Convert.ToInt16(dtStart[1]), Convert.ToInt16(dtStart[0]), 0, 0, 0);
                        objettER.ExecutionstatusEnd = new DateTime(Convert.ToInt16(dtEnd[2]), Convert.ToInt16(dtEnd[1]), Convert.ToInt16(dtEnd[0]), 23, 59, 59);
                        TimeSpan ts = objettER.ExecutionstatusEnd - objettER.ExecutionStatusStart;
                        if (ts.Ticks <= 0)
                        {
                            MessageBox.Show("From Date Should be Less than (<=) To Date ! , Please Provide Correct Input in DD/MM/YYYY Format Only !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return null;
                        }

                        ds = objER.SelectFromtabExecutionResult_ParameterWise(objettER);
                        return ds;
                    }
                }
                else { isvalidStartDateTime = false; isvalidEndDateTime = false; }
                if (!isvalidStartDateTime || !isvalidEndDateTime)
                {
                    MessageBox.Show("Invalid Date Time Format, Please Provide Input in DD/MM/YYYY Format Only !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    
                }
               return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return null;
            }
        }

        private System.Data.DataSet GetDataByMeterIDRange()
        {

            try
            {
                objettER.ExecutionMeterType = cmbMeterType.Text.Trim();
                objettER.MissingMeterAlphaFileld = txtFixString.Text.Trim();
                objettER.ExecutionParametersName = cmbparametersName.Text;
                objettER.ExecutionProcedureType = cmbTestType.Text.Trim();
                objettER.FinalResult = cmbStatus.Text.Trim().ToUpper();  //LatestStatus
                if (cmbStatus.SelectedIndex <= 0) objettER.FinalResult = "";//-------Show All              
                if (txtFindfrom.Text.Length > 1 && txtfindto.Text.Length > 1)
                {
                    objettER.MissingMeterRangeFrom = txtFindfrom.Text;
                    objettER.MissingMeterRangeTo = txtfindto.Text;
                    ds = objER.SelectFromtabExecutionResult_ParaMeterWiseReports(objettER, EntityExecutionResult.SelectionType.MeterID);
                    return ds;
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return null;
            }
        }

        private System.Data.DataSet GetDataByPCBAIDRange()
        {

            try
            {
                objettER.ExecutionMeterType = cmbMeterType.Text.Trim();
                objettER.MissingMeterAlphaFileld = txtFixString.Text.Trim();
                objettER.ExecutionParametersName = cmbparametersName.Text;
                objettER.ExecutionProcedureType = cmbTestType.Text.Trim();
                objettER.FinalResult = cmbStatus.Text.Trim().ToUpper();  //LatestStatus
                if (cmbStatus.SelectedIndex <= 0) objettER.FinalResult = "";//-------Show All     
                if (txtFindfrom.Text.Length > 1 && txtfindto.Text.Length > 1)
                {
                    objettER.MissingMeterRangeFrom = txtFindfrom.Text;
                    objettER.MissingMeterRangeTo = txtfindto.Text;
                    ds = objER.SelectFromtabExecutionResult_ParaMeterWiseReports(objettER, EntityExecutionResult.SelectionType.PCBAID);
                    return ds;
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return null;
            }
        }


        private void txtFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) lblFind_Click(this,e);
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      

        private void lblPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (DGVTestResult.RowCount <= 0)
                {
                    MessageBox.Show("No Records Selected To Generate Report !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                ts_Menu.Enabled = false;
                if (DGVTestResult.CurrentRow.Index >= 0)
                {
                    if (DGVTestResult.Rows[0].Cells[2].Value == null) return;
                }
                else return;
            DataSet EXCELLDS = new DataSet();
            DataTable dt = ServiceClass.ServiceInstance.GetGridDataTable("Parameters Wise Report", DGVTestResult);
            if (dt != null && dt.Rows.Count > 0)
            {
                EXCELLDS.Tables.Add(dt);
                ServiceClass.ServiceInstance.ExportDataSetToExcell(EXCELLDS);
            }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                ts_Menu.Enabled = true;
            }
        }
       private void cmbMeterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMeterType.SelectedIndex >= 0)
            {
                objentityprog.TestType = cmbMeterType.Text;
                DataSet ds = objbalprog.Pro_Select_tabProgramMaster_onMeterType(objentityprog);

                cmbparametersName.Items.Clear();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        cmbparametersName.Items.Add(dr[1].ToString());
                        Application.DoEvents();
                    }
                    if (cmbparametersName.Items.Count > 0) cmbparametersName.SelectedIndex = 0;
                }
            }
        }

       private void cmbSearchBy_SelectedIndexChanged(object sender, EventArgs e)
       {
           if (cmbSearchBy.SelectedIndex == 0) { txtFindfrom.Text = String.Format("{0:dd/MM/yyyy}", DateTime.Now.AddDays(-(DateTime.Now.Day - 1))); txtfindto.Text = String.Format("{0:dd/MM/yyyy}", DateTime.Now); txtFixString.Enabled = false; }
           else { txtFixString.Enabled = true; txtFindfrom.Text = ""; txtfindto.Text = ""; }
       }
        
    }
}
