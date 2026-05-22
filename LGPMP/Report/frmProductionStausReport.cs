using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using COMMONENTITY;
using BALLAYER;
using Utilities;
using SystemSecurityLibrary;
namespace CabconPMP
{
    public partial class frmProductionStausReport : Form
    {
        StandardDateTime std = new StandardDateTime();
        BALExecutionResults objER = new BALExecutionResults();
        EntityExecutionResult objettER = new EntityExecutionResult();
        BALDBConnectionTest objdbcon = new BALDBConnectionTest();
        DataSet ds = new DataSet();
        public frmProductionStausReport()
        {
            InitializeComponent();
        }

         
        private void GetDistinctMeterType()
        {
            List<string> meterTypeList = StaticVariables.GetMeterType();
            cmbMeterType.Items.Clear();
            foreach (string mtype in meterTypeList)
            {
                cmbMeterType.Items.Add(mtype);
            }
            if (cmbMeterType.Items.Count >= 0) cmbMeterType.SelectedIndex = 0;            
        
        }

        private void lblFind_Click(object sender, EventArgs e)
        {

            if (cmbSortBy.SelectedIndex <0)
            {
                MessageBox.Show("Please Select A Valid Search Condition !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cmbSortBy.Focus();
                return;
            }
            if (txtFrom.Text.Trim().Length <= 0)
            {
                MessageBox.Show("Plase Provide Valid Input For Data Search !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFrom.Focus();
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            BindingSource SBind = new BindingSource();
            ts_Menu.Enabled = false;
            try
            {
                if (!objdbcon.IsDBConnected())
                {
                    MessageBox.Show("Unable To Connect To DataBase, Server May Down" + "\n" + "Plase Contact System Administrator !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                bool isvalidStartDateTime = true;
                bool isvalidEndDateTime = true;
                int objint;
                
                objettER.ExecutionMeterType = cmbMeterType.Text.Trim();
                // setDefaultResets();--Only For Testing
                
                if (cmbSortBy.SelectedIndex == 1) 
                {
                    objettER.ExecutionStatusStartPCBA = txtFrom.Text.Trim();
                    objettER.ExecutionstatusEndPCBA = txtTo.Text.Trim();
                    ds = objER.SelectFromtabExecutionResult_onPCBARang_ProductStatus(objettER); 
                }
                    else if (cmbSortBy.SelectedIndex == 0)
                    {
                        if (txtFrom.Text.Length >= 6 && txtTo.Text.Length >= 6)
                        {
                            string[] dtStart = txtFrom.Text.Trim().Split('/');
                            string[] dtEnd = txtTo.Text.Trim().Split('/');                            

                            if (dtStart.Length >=3 && int.TryParse(dtStart[0], out objint) && int.TryParse(dtStart[1], out objint) && int.TryParse(dtStart[2], out objint)) { }
                            else isvalidStartDateTime = false;

                            if (dtEnd.Length >= 3 && int.TryParse(dtEnd[0], out objint) && int.TryParse(dtEnd[1], out objint) && int.TryParse(dtEnd[2], out objint)) { }
                            else isvalidEndDateTime = false;

                            if (isvalidStartDateTime && isvalidEndDateTime)
                            {
                                

                                objettER.ExecutionStatusStart = new DateTime(Convert.ToInt16(dtStart[2]),Convert.ToInt16(dtStart[1]),Convert.ToInt16(dtStart[0]),0,0,0);
                                objettER.ExecutionstatusEnd = new DateTime(Convert.ToInt16(dtEnd[2]), Convert.ToInt16(dtEnd[1]), Convert.ToInt16(dtEnd[0]), 23, 59, 59);
                                TimeSpan ts = objettER.ExecutionstatusEnd - objettER.ExecutionStatusStart;
                                if (ts.Ticks <= 0)
                                {
                                    MessageBox.Show("From Date Should be Less than (<=) To Date ! , Please Provide Correct Input in DD/MM/YYYY Format Only !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    return;
                                }
                                ds = objER.SelectFromtabExecutionResult_onDateRang_ProductStatus(objettER);
                            }
                        }
                        else { isvalidStartDateTime = false; isvalidEndDateTime = false; }
                        if (!isvalidStartDateTime || !isvalidEndDateTime)
                        {
                            MessageBox.Show("Invalid Date Format, Please Provide Valid 10 Digit Date in DD/MM/YYYY Format Only  !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }

                    }
                    if (ds != null)
                    {
                        SBind.Clear();
                        DGVTestResult.DataSource = SBind;
                        if (DGVTestResult.RowCount > 0) DGVTestResult.Rows.Clear();
                        if (DGVTestResult.ColumnCount > 0) DGVTestResult.Columns.Clear();
                        SBind.DataSource = ds.Tables[0];
                        DGVTestResult.DataSource = SBind;
                        this.Text = "Production Status Report   [ Total Records : " + DGVTestResult.Rows.Count.ToString() + " ]";
                    }
                    else
                    {
                        MessageBox.Show("Data Not Found OR Unable to Retrive Data !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            DataTable dt = ServiceClass.ServiceInstance.GetGridDataTable("Execution Result", DGVTestResult);
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

        private void cmbSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSortBy.SelectedIndex == 0) { txtFrom.Text = String.Format("{0:dd/MM/yyyy}", DateTime.Now.AddDays(-(DateTime.Now.Day - 1))); txtTo.Text = String.Format("{0:dd/MM/yyyy}", DateTime.Now); txtTo.Enabled = true; }
            else { txtFrom.Text = ""; txtTo.Text = ""; txtTo.Enabled = false; }
        }

        private void DGVTestResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblState_Click(object sender, EventArgs e)
        {
             
            string meterID = string.Empty;
            string pcbaID = string.Empty;
            string meterType = string.Empty;
            string testType = string.Empty;
            string testID = string.Empty;
                  
            ts_Menu.Enabled = false;
            try
            {
                if (!objdbcon.IsDBConnected())
                {
                    MessageBox.Show("Unable To Connect DataBase, Server May Down" + "\n" + "Plase Contact System Administrator !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (DGVTestResult.CurrentRow == null)
                {
                    MessageBox.Show("No Records Selected To Generate Product Status Graph !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                    
                if (DGVTestResult.Rows[DGVTestResult.CurrentRow.Index].Cells[0].Value == null) return;
                    objettER.ExecutionMeterType = DGVTestResult.Rows[DGVTestResult.CurrentRow.Index].Cells[7].Value.ToString();
                    objettER.PCBAID = DGVTestResult.Rows[DGVTestResult.CurrentRow.Index].Cells[0].Value.ToString();
                    ds = objER.SelectFromtabExecutionReport_onMeterTypeandPCBAID(objettER);
                
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<string> TestTypeList = new List<string>();
                    List<bool> FinalStatusList = new List<bool>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (!TestTypeList.Contains(dr[1].ToString()))
                        {
                            TestTypeList.Add(dr[1].ToString());
                            if (dr[2].ToString().ToUpper() == "PASS" ) FinalStatusList.Add(true);
                            else FinalStatusList.Add(false);
                        }
                    }

                    frmMeterExecutionState objexeState = new frmMeterExecutionState(TestTypeList, FinalStatusList, objettER.PCBAID);
                    objexeState.ShowDialog();
                      
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                
                ts_Menu.Enabled = true;
            }
         
        
        }

        private void frmProductionStausReport_Load(object sender, EventArgs e)
        {
            GetDistinctMeterType();
        }
    }
}
