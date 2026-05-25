using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Web.UI.WebControls;
//using CrystalDecisions.CrystalReports.Engine;
using Excel = Microsoft.Office.Interop.Excel;
using Utilities;
using COMMONENTITY;

namespace CabconPMP.Report
{
    public partial class frmRoutineTestReport : Form
    {
        COMMONENTITY.EntityRoutineTestReport routinetestentity = new COMMONENTITY.EntityRoutineTestReport();
        string refFileFormat ="";
        CheckedListBox chklst;
        List<string> SelectedRangedlistofMeters;
        DataTable dtResult = new DataTable();
        
        public enum EXCELCELL
        {
            NAMEOFCUSTOMER_ROW = 12,
            NAMEOFCUSTOMER_COL = 5,
            ORDERNO_ROW = 14,
            ORDERNO_COL = 5,
            ORDERDATE_ROW = 14,
            ORDERDATE_COL = 6,
            METERRATING_ROW = 16,
            METERRATING_COL = 5,
            TESTPOINTNAME_ROW = 51,
            TESTPOINTNAME_COL = 3,
            TESTPOINTDATA_ROW = 55,
            TESTPOINTDATA_COL = 2,
                

        }

        public frmRoutineTestReport(EntityUserManagement objetyusermgtref)
        {
            InitializeComponent(); COMMONENTITY.FormStyleHelper.Apply(this);
            if (objetyusermgtref.LoginTypeIndex == (int)StaticVariables.userCategory.Usersupervisor || objetyusermgtref.LoginTypeIndex == (int)StaticVariables.userCategory.Useradministrator || objetyusermgtref.LoginTypeIndex == (int)StaticVariables.userCategory.UserPoweradministrator) chkRangedReport.Enabled = true;
            else chkRangedReport.Enabled = false;
        }

        private void frmroutinetest_Load(object sender, EventArgs e)
        {
            try
            {
                cmbMeterType.DataSource = StaticVariables.GetMeterType();
                if (cmbMeterType.Items.Count >= 0) cmbMeterType.SelectedIndex = 0;
                txtorderdate.Text = String.Format("{0:dd-MM-yyyy}", DateTime.Now);
                cmbMidDegits.SelectedIndex = 1;
                chklst = chkmeterlistPass;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }            
        }
              
       
  
        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + "\n" + ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
              
            }
            finally
            {
                GC.Collect();
            }
        }

        private void GenerateReport(DataTable _dt, List<string> _icolindex, string strFileName,CheckedListBox midList )
        {
            try
            {
               //this.Cursor = Cursors.WaitCursor;
                //---------------------
                Excel.Application xlApp = null;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;
               
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(refFileFormat, 0, true, misValue, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                                
                List<string> strparamlist = new List<string>();

                for (int icount = 0; icount < midList.CheckedItems.Count; icount++)
                {                 

                    var selectedMeterTest = (from myRow in _dt.AsEnumerable()
                                             where myRow.Field<string>("MeterID") == midList.CheckedItems[icount].ToString()
                                             select myRow).ToList();

                    COMMONENTITY.MeterReportData _meterdata = new COMMONENTITY.MeterReportData();
                    _meterdata.mmtereid = midList.CheckedItems[icount].ToString();//Meter ID
                    bool isPass = false;
                    if (selectedMeterTest.Count > 0 && selectedMeterTest[0].ItemArray[2].ToString().Length > 1)
                    {
                        isPass = true;
                        if (chkRangedReport.Checked)
                        {    //-----------Show Only in Case of Padding------------- 
                            if (selectedMeterTest[0].ItemArray[3].ToString().IndexOf("<<") >= 0 || selectedMeterTest[0].ItemArray[3].ToString().IndexOf(">>") >= 0) isPass = false;
                        }
                    }
                    if (isPass)
                    {  
                        strparamlist = selectedMeterTest[0].ItemArray[2].ToString().Split(',').ToList();//ParaList
                        string strval = selectedMeterTest[0].ItemArray[3].ToString();//Result List
                        string[] _arr = strval.Split(',');

                        int errorListCnt = 1;
                        if (_arr == null || _arr.Count() < _icolindex.Count)
                        {
                            if (!chkRangedReport.Checked) continue; 
                            //-----------Show Only in Case of Padding-------------                            
                            if (_icolindex.Count() >= 1 ) errorListCnt = _icolindex.Count();
                            for (int icol = 0; icol < errorListCnt; icol++) _meterdata.mcolvalues.Add("--");

                        }
                        else
                        {
                            for (int icol = 0; icol < _icolindex.Count(); icol++)
                            {
                                int errorIdx = strparamlist.IndexOf(_icolindex[icol]);
                                if (errorIdx >= 0) _meterdata.mcolvalues.Add(_arr[errorIdx]);
                                else _meterdata.mcolvalues.Add("");
                            }
                        }
                    }
                    else 
                    {
                        for (int icol = 0; icol < _icolindex.Count(); icol++) _meterdata.mcolvalues.Add("");                  
                         
                    }

                    routinetestentity.mdatalist.Add(_meterdata);

                }
                if (routinetestentity.mdatalist.Count <= 0)
                {
                    MessageBox.Show("No Data To Generate The Report !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                CabconPMP.PMPDataSet _dataset = new PMPDataSet();

                for (int icount = 0; icount < routinetestentity.mdatalist.Count; icount++)
                {
                    DataRow _dr = _dataset.Tables[0].NewRow();
                    _dr["CustomerName"] = routinetestentity.mcustomername;
                    _dr["OrderNo"] = routinetestentity.morderno;
                    _dr["MeterRating"] = routinetestentity.mmeterrating;
                    _dr["Standard"] = routinetestentity.mstandard;
                    _dr["MeterType"] = routinetestentity.mmetertype;
                    _dr["Class"] = routinetestentity.mclass;

                    for (int icolcount = 0; icolcount < _icolindex.Count; icolcount++)
                    {

                        _dr["TestPoint" + (icolcount + 1).ToString()] = routinetestentity.mdatalist[icount].mcolvalues[icolcount];
                        _dr["MeterNo"] = routinetestentity.mdatalist[icount].mmtereid;
                    }

                    _dataset.Tables[0].Rows.Add(_dr);
                }


                int icolumncount = _icolindex.Count + 1;

                xlWorkSheet.Cells[EXCELCELL.NAMEOFCUSTOMER_ROW, EXCELCELL.NAMEOFCUSTOMER_COL] = routinetestentity.mcustomername;
                xlWorkSheet.Cells[EXCELCELL.NAMEOFCUSTOMER_ROW, EXCELCELL.NAMEOFCUSTOMER_COL + 14] = routinetestentity.mcustomername;

                xlWorkSheet.Cells[EXCELCELL.ORDERNO_ROW, EXCELCELL.ORDERNO_COL] = routinetestentity.morderno;
                xlWorkSheet.Cells[EXCELCELL.ORDERNO_ROW, EXCELCELL.ORDERNO_COL + 14] = routinetestentity.morderno;

                xlWorkSheet.Cells[EXCELCELL.ORDERDATE_ROW, EXCELCELL.ORDERDATE_COL + 1] ="Dt. " +  routinetestentity.morderdate;
                xlWorkSheet.Cells[EXCELCELL.ORDERDATE_ROW, EXCELCELL.ORDERDATE_COL + 1 + 14] = "Dt. " + routinetestentity.morderdate;

                xlWorkSheet.Cells[EXCELCELL.METERRATING_ROW, EXCELCELL.METERRATING_COL] = routinetestentity.mmeterrating;
                xlWorkSheet.Cells[EXCELCELL.METERRATING_ROW, EXCELCELL.METERRATING_COL + 14] = routinetestentity.mmeterrating;


                for (int icolcount = 0; icolcount < _icolindex.Count; icolcount++)
                {
                    string testpointname = _icolindex[icolcount];
                    xlWorkSheet.Cells[EXCELCELL.TESTPOINTNAME_ROW, EXCELCELL.TESTPOINTNAME_COL + icolcount] = testpointname;
                    xlWorkSheet.Cells[EXCELCELL.TESTPOINTNAME_ROW, EXCELCELL.TESTPOINTNAME_COL + icolcount + 14] = testpointname;
                }

                for (int idatacount = 0, idisplaycount = 0; idatacount < _dataset.Tables[0].Rows.Count; idatacount++, idisplaycount++)
                {

                    xlWorkSheet.Cells[EXCELCELL.TESTPOINTDATA_ROW + idisplaycount, EXCELCELL.TESTPOINTDATA_COL] = _dataset.Tables[0].Rows[idatacount][6].ToString();

                    for (int icelldata = 7, j = 1; icelldata < _icolindex.Count + 7; icelldata++, j++)
                    {
                        xlWorkSheet.Cells[EXCELCELL.TESTPOINTDATA_ROW + idisplaycount, EXCELCELL.TESTPOINTDATA_COL + j] = _dataset.Tables[0].Rows[idatacount][icelldata].ToString();
                    }

                    idatacount++;

                    if (idatacount >= _dataset.Tables[0].Rows.Count) break;

                    xlWorkSheet.Cells[EXCELCELL.TESTPOINTDATA_ROW + idisplaycount, EXCELCELL.TESTPOINTDATA_COL + 14] = _dataset.Tables[0].Rows[idatacount][6].ToString();

                    for (int icelldata = 7, j = 1; icelldata < _icolindex.Count + 7; icelldata++, j++)
                    {
                        xlWorkSheet.Cells[EXCELCELL.TESTPOINTDATA_ROW + idisplaycount, EXCELCELL.TESTPOINTDATA_COL + j + 14] = _dataset.Tables[0].Rows[idatacount][icelldata].ToString();
                    }
                }

                xlWorkBook.Close(true, strFileName, misValue);                
                xlApp.Quit();
                toolStriplbstatus.Text = "Report Generated Successfully ";
                ReleaseObject(xlWorkSheet);
                ReleaseObject(xlWorkBook);
                ReleaseObject(xlApp);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void ExportMissingMeterReport()
        {
            try
            {                
                DataSet EXCELLDS = new DataSet();
                DataTable dt = ServiceClass.ServiceInstance.GetGridDataTable("Missing Meter ID List", DGVMissingMeterList);
                if (dt != null && dt.Rows.Count > 0)
                {
                    EXCELLDS.Tables.Add(dt);
                    ServiceClass.ServiceInstance.ExportDataSetToExcell(EXCELLDS);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        
      

        private void UpdateTestIDs(DataTable _dt)
        {
            BindingSource SBind = new BindingSource();
            SBind.Clear();
            DGVMissingMeterList.DataSource = SBind;
            long objlong;
            SelectedRangedlistofMeters = new List<string>();             
            if (!long.TryParse(txtmeterIDFrom.Text.Trim(), out objlong))
            {
                MessageBox.Show("Meter ID Range From Value Should be Valiad Numeric only !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtmeterIDFrom.Focus();
                return ;
            }
            if (!long.TryParse(txtMeterIDTo.Text.Trim(), out objlong))
            {
                MessageBox.Show("Meter ID Range From Value Should be Valiad Numeric only !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMeterIDTo.Focus();
                return ;
            }
            long mIDcnt = Convert.ToInt64(txtmeterIDFrom.Text.Trim());
            long maxIDcnt = Convert.ToInt64(txtMeterIDTo.Text.Trim());
            if (maxIDcnt - mIDcnt >= 100000)
            {
                MessageBox.Show("Meter ID Range Diffrence Should Not Be > 1 Lac !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtmeterIDFrom.Focus();
                return ;
            }
            int maxlen = Convert.ToInt32(cmbMidDegits.Text.Trim()) - txtFixString.Text.Trim().Length;
           
            Application.DoEvents();
            Application.DoEvents();
            this.Cursor = Cursors.WaitCursor;
           
            
            try
            {

               
                DataTable DT = new DataTable();
                DT.Columns.Add("Missing Meter ID List");
                long midCount = mIDcnt;
                while (midCount <= maxIDcnt)
                {
                    SelectedRangedlistofMeters.Add(txtFixString.Text.ToUpperInvariant() + (midCount++).ToString().Trim().PadLeft(maxlen, '0'));
                }
                int mListCount = 0;
                while (mListCount < SelectedRangedlistofMeters.Count)
                {
                    if (chkRangedReport.Checked)
                    {
                        chkmeterlistPass.Items.Add(SelectedRangedlistofMeters[mListCount++]);
                        continue;
                    }
                    DataRow[] result = _dt.Select("MeterID ='" + SelectedRangedlistofMeters[mListCount] + "'");//"MeterID =" + );
                    if (result.Length > 0)
                    {
                        toolStriplbstatus.Text = "";
                        Application.DoEvents();
                        string errorList = result[0].ItemArray[3].ToString();
                        if (errorList.IndexOf(">>") >= 0 || errorList.IndexOf("<<") >= 0) chkmeterlistFail.Items.Add(result[0].ItemArray[0].ToString());
                        else if (errorList.IndexOf(",,") >= 0) chkmeterlistFail.Items.Add(result[0].ItemArray[0].ToString());
                        else chkmeterlistPass.Items.Add(result[0].ItemArray[0].ToString());
                    }
                    else
                    {
                        toolStriplbstatus.Text = "Geting Meter's ID List, Please Wait . . . . . .";
                        Application.DoEvents();
                        DT.Rows.Add(SelectedRangedlistofMeters[mListCount]);
                    }
                    mListCount++;
                }
                if (chkmeterlistPass.Items.Count <= 0 && chkmeterlistFail.Items.Count <= 0)
                {
                    MessageBox.Show("No Meter Serialized For Selected Condition !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);                  
                }
                if (DGVMissingMeterList.ColumnCount > 0) DGVMissingMeterList.Columns.Clear();
                SBind.DataSource = DT;
                DGVMissingMeterList.DataSource = SBind;

                lblPassCount.Text = chkmeterlistPass.Items.Count.ToString();
                lblFailCount.Text = chkmeterlistFail.Items.Count.ToString();
                lblMissingCount.Text = DT.Rows.Count.ToString();
 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable To Connect To DataBase, Invalid Response !" + "\n\n" + ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                this.Cursor = Cursors.Default;
              
            }     
          
        }      
        
        private void chkselectall_CheckedChanged(object sender, EventArgs e)
        {
            for (int icount = 0; icount < chkmeterlistPass.Items.Count; icount++)
                chkmeterlistPass.SetItemChecked(icount, chkselectall.Checked);            
        }

            

        private void chkAllFailed_CheckedChanged(object sender, EventArgs e)
        {

            for (int icount = 0; icount < chkmeterlistFail.Items.Count; icount++)
                chkmeterlistFail.SetItemChecked(icount, chkAllFailed.Checked);
        }      

        private void tabMeterStatus_Selected(object sender, TabControlEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (tabMeterStatus.SelectedIndex == 0) chklst = chkmeterlistPass;
            else if (tabMeterStatus.SelectedIndex == 1) chklst = chkmeterlistFail;
            else chklst = null;
            this.Cursor = Cursors.Default;
        }

        private void chkRangedReport_CheckedChanged(object sender, EventArgs e)
        {
            lblFind_Click(this, e);
            if (chkRangedReport.Checked) { tabMeterStatus.SelectedTab = tabMeterStatus.TabPages[0]; chkselectall.Checked = true; tabMeterStatus.Enabled = false; }
            else tabMeterStatus.Enabled = true;

        }

        private void lblFind_Click(object sender, EventArgs e)
        {
               
            try
            {
                this.Cursor = Cursors.WaitCursor;
                toolStriplbstatus.Text = "Geting Meter's ID List, Please Wait . . . . . .";
                Application.DoEvents();
                lblFind.Enabled = false;
                chkselectall.Checked = false;
                BALLAYER.BALRoutineTestReport _balroutinetest = new BALLAYER.BALRoutineTestReport();
                routinetestentity.mcustomername = txtcustomername.Text;
                routinetestentity.morderno = txtorderno.Text;
                routinetestentity.mmeterrating = txtmeterrating.Text;
                routinetestentity.mstandard = "";
                routinetestentity.mmetertype = cmbMeterType.Text;
                routinetestentity.mclass = "";
                routinetestentity.mfromdate = new DateTime(dtfrom.Value.Year, dtfrom.Value.Month, dtfrom.Value.Day, 0, 0, 0);
                routinetestentity.mtodate = new DateTime(dtto.Value.Year, dtto.Value.Month, dtto.Value.Day, 23, 59, 59);

                TimeSpan ts = routinetestentity.mtodate - routinetestentity.mfromdate;
                if (ts.Ticks <= 0)
                {
                    MessageBox.Show("From Date Should be Less than (<=) To Date ! , Please Provide Correct Input in DD/MM/YYYY Format Only !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                routinetestentity.morderdate = txtorderdate.Text;
                chkmeterlistPass.Items.Clear();
                chkmeterlistFail.Items.Clear();
                chkmeterlistMissing.Items.Clear();
                DataTable dt = _balroutinetest.getExecutionResult(routinetestentity);

                chkmeterlistPass.Items.Clear();
                var dataView = new DataView(dt);
                dataView.Sort = "MeterID ASC";
                dataView.RowFilter = "MeterID <> ''";
                dtResult = dataView.ToTable();

                if (dtResult == null || dtResult.Rows.Count < 1)
                {
                    MessageBox.Show("No Data Found !" + "\n" + "Verify the Selected Condition OR Pre-Fix Digit Then Try Again !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                UpdateTestIDs(dtResult);
                chkselectall.Checked = true;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                lblFind.Enabled = true;
                toolStriplbstatus.Text = "";
                this.Cursor = Cursors.Default;
            }

        
        }

        private void lblReport_Click(object sender, EventArgs e)
        {
           
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int cycleTime = 0;
                if (chklst == null)
                {
                    if(MessageBox.Show("Do You Want To Generate MISSING Meter Report ?? ", "Cabcon PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)==DialogResult.No)return;
                    toolStriplbstatus.Text = "Generating Report, Please Wait . . . . . .";
                    Application.DoEvents();
                    Application.DoEvents();
                    Thread mmr = new Thread(ExportMissingMeterReport);
                    mmr.Start();                  
                   
                    while (mmr.IsAlive)
                    {

                        toolStriplbstatus.Text = "Generating Report Time (Sec.) : " + (cycleTime / 1000).ToString();
                        Application.DoEvents();
                        Thread.Sleep(10);
                        cycleTime += 10;
                        this.Cursor = Cursors.WaitCursor;
                    }
                    Application.DoEvents();
                    Application.DoEvents();                   
                    toolStriplbstatus.ForeColor = Color.Green;
                    toolStriplbstatus.Text = "Report Generated Successfully !";
                    MessageBox.Show("Report Generated Successfully !" , "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   return;
                }
                else if (tabMeterStatus.SelectedIndex == 0)
                {
                    if (MessageBox.Show("Do You Want To Generate Routine Test PASS Meter Report ?? ", "Cabcon PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                }
                else if (tabMeterStatus.SelectedIndex == 1)
                {
                    if (MessageBox.Show("Do You Want To Generate Routine Test FAIL Meter Report ?? ", "Cabcon PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                }
                if (chklst.Items.Count < 1)
                {
                    MessageBox.Show("Meter List Should Not Be Empty", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (chklst.CheckedItems.Count < 1)
                {
                    MessageBox.Show("Please Select Meter ID from List !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                pabelRoutineTest.Enabled = false;
                refFileFormat = AppDomain.CurrentDomain.BaseDirectory + "\\Configuration\\" + StaticVariables.FilePrefixRoutineTestReportFileFormat + cmbMeterType.Text.Trim() + ".xls";
                if (!File.Exists(refFileFormat))
                {
                    MessageBox.Show("Routine Test Report Refrence File Format (*.xls) Not Found For Selected Meter Type !" + "\n" + "Please Import Valid Reference File !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                routinetestentity.mdatalist.Clear();
                
                gpreportset.Enabled = false;
                toolStriplbstatus.ForeColor = Color.Green;
                toolStriplbstatus.Text = "Fetching Data, Please Wait . . . . . .";
                Application.DoEvents(); 
                BALLAYER.BALRoutineTestReport _balroutinetest = new BALLAYER.BALRoutineTestReport();
                routinetestentity.mcustomername = txtcustomername.Text;
                routinetestentity.morderno = txtorderno.Text;
                routinetestentity.mmeterrating = txtmeterrating.Text;
                routinetestentity.mstandard = "";
                routinetestentity.mmetertype = cmbMeterType.Text;
                routinetestentity.mclass = "";
                routinetestentity.mfromdate = new DateTime(dtfrom.Value.Year, dtfrom.Value.Month, dtfrom.Value.Day, 0, 0, 0);
                routinetestentity.mtodate = new DateTime(dtto.Value.Year, dtto.Value.Month, dtto.Value.Day, 23, 59, 59);
                routinetestentity.morderdate = txtorderdate.Text;

                
                    DataView view = new DataView(dtResult);
                    DataTable distinctValues = view.ToTable(true, "ParamList");
                    List<string> _DistinctPointList = new List<string>();
                    foreach (DataRow dr in distinctValues.Rows)
                    {
                        List<string> _testPoints = dr["ParamList"].ToString().Split(',').ToList();
                        foreach (string testPoint in _testPoints)
                        {
                            if (!_DistinctPointList.Contains(testPoint) && testPoint.Trim().Length > 0) _DistinctPointList.Add(testPoint);                            
                            Application.DoEvents();
                             
                        }
                    }
                    _DistinctPointList.Sort();
                    _DistinctPointList.Reverse();
               
                if (_DistinctPointList == null || _DistinctPointList.Count < 1)
                {                     
                        toolStriplbstatus.ForeColor = Color.Red;
                        toolStriplbstatus.Text = "No Test Points Available To Genrate The Report !";
                        return;                   
                }
                toolStriplbstatus.Text = "Generating Report, Please Wait . . . . . .";
                Application.DoEvents();
                frmRoutineTestPointselection _reportcolselection = new frmRoutineTestPointselection(_DistinctPointList);

                if (_reportcolselection.ShowDialog() != DialogResult.OK)
                {
                    toolStriplbstatus.ForeColor = Color.Red;
                    toolStriplbstatus.Text = "Report Generation is Cancelled !";
                    return;
                }
                //---------------------Exporting File ----------------------------

                string datetime = "RTR_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls";
                SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
                SaveFileDialog1.Filter = "Files (*.xls)|*.xls";//|All Files (*.*)|*.*";
                SaveFileDialog1.FileName = datetime;
                DialogResult result = SaveFileDialog1.ShowDialog();

                if (result != DialogResult.OK) return;
                string strFileName = SaveFileDialog1.FileName;
                
                
                Application.DoEvents();
                Thread t = new Thread(() => GenerateReport(dtResult, _reportcolselection.mparamindexlist, strFileName, chklst));
                t.Start();
                
                cycleTime = 0;               
                while (t.IsAlive)
                {

                    toolStriplbstatus.Text = "Generating Report Time (Sec.) : " + (cycleTime/1000).ToString();                    
                    Application.DoEvents();
                    Thread.Sleep(10);
                    cycleTime += 10;
                    this.Cursor = Cursors.WaitCursor;
                }
                Application.DoEvents();
                Application.DoEvents();                
                
                toolStriplbstatus.ForeColor = Color.Green;
                toolStriplbstatus.Text = "Report Generated Successfully !";
                MessageBox.Show("Report Generated Successfully !" + "\n File Path : " + strFileName, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }            
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } 
            finally
            {
                pabelRoutineTest.Enabled = true;
                gpreportset.Enabled = true;
                this.Cursor = Cursors.Default;
            }

        
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
             
            this.Close();
          
        }
 
         
    }
}
