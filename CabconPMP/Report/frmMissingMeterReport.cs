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
namespace CabconPMP
{
    public partial class frmMissingMeterReport : Form
    {
        StandardDateTime std = new StandardDateTime();
        BALExecutionResults objER = new BALExecutionResults();
        EntityExecutionResult objettER = new EntityExecutionResult();
        BALDBConnectionTest objdbcon = new BALDBConnectionTest();
        DataSet ds = new DataSet();
        public frmMissingMeterReport()
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
                    MessageBox.Show("Unable To Connect DataBase, Server May Down" + "\n" + "Plase Contact System Administrator !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.DoEvents();
                    this.Close();
                    Application.DoEvents();
                    return;
                }

                 
                GetDistinctMeterType();                
                txtmeterIDFrom.Focus();
                cmbMidDegits.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (MessageBox.Show("Do You Want To Get Missing Meter ID list For : " + cmbMeterType.Text + '\n' + "**Please Select A Valid Total Meter ID Digit's From Available List ","Cabcon PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
            long objlong;
            if (!long.TryParse(txtmeterIDFrom.Text.Trim(),out objlong))
            {
                MessageBox.Show("Meter ID Range From Value Should be Valiad Numeric only !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtmeterIDFrom.Focus();
                return;
            }
            if (!long.TryParse(txtMeterIDTo.Text.Trim(), out objlong))
            {
                MessageBox.Show("Meter ID Range From Value Should be Valiad Numeric only !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMeterIDTo.Focus();
                return;
            }
            long mIDcnt = Convert.ToInt64(txtmeterIDFrom.Text.Trim());
            long maxIDcnt = Convert.ToInt64(txtMeterIDTo.Text.Trim());
            if (maxIDcnt - mIDcnt >= 100000)
            {
                MessageBox.Show("Meter ID Range Diffrence Should Not Be > 1 Lac !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtmeterIDFrom.Focus();
                return;
            }
            int maxlen =Convert.ToInt32(cmbMidDegits.Text.Trim()) -  txtFixString.Text.Trim().Length;
            txtmeterIDFrom.Text = mIDcnt.ToString().Trim().PadLeft(maxlen, '0');
            txtMeterIDTo.Text = maxIDcnt.ToString().Trim().PadLeft(maxlen, '0');
            Application.DoEvents();
            Application.DoEvents();
            this.Cursor = Cursors.WaitCursor;
            BindingSource SBind = new BindingSource();
            ts_Menu.Enabled = false;
            try
            {
                if (!objdbcon.IsDBConnected())
                {
                    MessageBox.Show("Unable To Connect To DataBase, Server May Down" + "\n" + "Plase Contact System Administrator !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                objettER.MissingMeterAlphaFileld = txtFixString.Text.Trim();
                objettER.MissingMeterRangeFrom = txtmeterIDFrom.Text.Trim().PadLeft(maxlen,'0');
                objettER.MissingMeterRangeTo = txtMeterIDTo.Text.Trim().PadLeft(maxlen, '0');
                objettER.ExecutionMeterType = cmbMeterType.Text.Trim();

                ds = objER.SelectFromtabExecutionResult_MissingMeterList(objettER);
                if (ds == null || ds.Tables[0].Rows.Count < 1)
                {
                    MessageBox.Show("No Data Found !" + "\n" + "Verify the Selected Condition or Pre-Fix Digit Then Try Again !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DataTable DT = new DataTable();
                DT.Columns.Add("PASS Meter ID List");

                DataTable DT1 = new DataTable();
                DT1.Columns.Add("FAIL Meter ID List");

                DataTable DT2 = new DataTable();
                DT2.Columns.Add("Missing Meter ID List");
                
                 while (mIDcnt <= maxIDcnt)
                 {
                     string expvar = txtFixString.Text.Trim() + mIDcnt.ToString().PadLeft(maxlen,'0');
                     DataRow[] result = ds.Tables[0].Select("MeterID ='" + expvar + "'");//"MeterID =" + );
                     if (result.Length > 0)
                     {
                         if (result[0].ItemArray[1].ToString().ToUpperInvariant() == "PASS")
                         {
                             DataRow row = DT.NewRow();
                             row[0] = expvar.ToUpperInvariant();
                             DT.Rows.Add(row[0]);
                         }
                         else
                         {
                             DataRow row = DT1.NewRow();
                             row[0] = expvar.ToUpperInvariant();
                             DT1.Rows.Add(row[0]);
                         }
                     }
                     else
                     {
                         DataRow row = DT2.NewRow();
                         row[0] = expvar.ToUpperInvariant();
                         DT2.Rows.Add(row[0]);
                     }
                     mIDcnt++;
                 }
                                        
                    if (ds != null)
                    {                        
                        if (DT != null)
                        {
                            SBind.Clear();
                            DGVPassMeterList.DataSource = SBind;
                            if (DGVPassMeterList.RowCount > 0) DGVPassMeterList.Rows.Clear();
                            if (DGVPassMeterList.ColumnCount > 0) DGVPassMeterList.Columns.Clear();
                            if (DGVPassMeterList.ColumnCount > 0) DGVPassMeterList.Columns.Clear();
                            SBind.DataSource = DT;
                            DGVPassMeterList.DataSource = SBind;
                            lblPassCount.Text = DT.Rows.Count.ToString();
                        }
                        if (DT1 != null)
                        {
                            BindingSource SBind1 = new BindingSource();
                            SBind1.Clear();
                            DGVFailMeterList.DataSource = SBind1;
                            if (DGVFailMeterList.RowCount > 0) DGVFailMeterList.Rows.Clear();
                            if (DGVFailMeterList.ColumnCount > 0) DGVFailMeterList.Columns.Clear();
                            if (DGVFailMeterList.ColumnCount > 0) DGVFailMeterList.Columns.Clear();
                            SBind1.DataSource = DT1;
                            DGVFailMeterList.DataSource = SBind1;
                            lblFailCount.Text = DT1.Rows.Count.ToString();
                        }
                        if (DT2 != null)
                        {
                            BindingSource SBind2 = new BindingSource();
                            SBind2.Clear();
                            DGVMissingMeterList.DataSource = SBind2;
                            if (DGVPassMeterList.RowCount > 0) DGVMissingMeterList.Rows.Clear();
                            if (DGVFailMeterList.ColumnCount > 0) DGVMissingMeterList.Columns.Clear();
                            if (DGVMissingMeterList.ColumnCount > 0) DGVMissingMeterList.Columns.Clear();
                            SBind2.DataSource = DT2;
                            DGVMissingMeterList.DataSource = SBind2;
                            lblMissingCount.Text = DT2.Rows.Count.ToString();
                        }                       
                         
                        
                    }
                    else
                    {
                        MessageBox.Show("Unable To Connect To DataBase, Server May Down" + "\n" + "Plase Contact System Administrator !" + "\n" + "Data Not Found OR Unable to Retrive Data ! !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable To Connect To DataBase, Invalid Response !" + "\n\n" + ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                
            this.Cursor = Cursors.WaitCursor;
            ts_Menu.Enabled = false;
                
            DataSet EXCELLDS = new DataSet();
            DataTable dt = ServiceClass.ServiceInstance.GetGridDataTable("Available_PASS Meter ID List", DGVPassMeterList);
            DataTable dt1 = ServiceClass.ServiceInstance.GetGridDataTable("Available_FAIL Meter ID List", DGVFailMeterList);
            DataTable dt2 = ServiceClass.ServiceInstance.GetGridDataTable("Missing Meter ID List", DGVMissingMeterList);
            if (dt != null && dt.Rows.Count > 0)
            {
                EXCELLDS.Tables.Add(dt);
                EXCELLDS.Tables.Add(dt1);
                EXCELLDS.Tables.Add(dt2);
                ServiceClass.ServiceInstance.ExportDataSetToExcell(EXCELLDS);
            }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                ts_Menu.Enabled = true;
            }
        }

        private void txtFixString_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetterOrDigit(e.KeyChar)) { }  
            else e.Handled = e.KeyChar != (char)Keys.Back; 
        }

        private void txtmeterIDFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar)) { }
            else e.Handled = e.KeyChar != (char)Keys.Back; 
        }

        private void txtMeterIDTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar)) { }
            else e.Handled = e.KeyChar != (char)Keys.Back; 
        }
  
     
        
    }
}
