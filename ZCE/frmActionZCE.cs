using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using System.Collections;
using System.IO.Ports;
using System.Globalization;
using System.Diagnostics;
using COMMONENTITY;
using BALLAYER;
using ApplicationInterface;
using Utilities;
using SystemSecurityLibrary;
namespace ZCE
{
    public partial class frmActionZCE : Form
    {         
        int test_index = 0;
        IECLayerInterface objIECLI = new IECLayerInterface();
        BALExecutionResults objexere = new BALExecutionResults();
        EntityExecutionResult objexeresult = new EntityExecutionResult();
        LayerInterface objLI = new LayerInterface();        
        AppSettings objappSettings = new AppSettings();
        MyCrypro objcrypt = new MyCrypro();
        CommonCommandMethods objccmdmethod = new CommonCommandMethods();
         IECCommonCommandMethods objIECccmdmethod = new IECCommonCommandMethods();
        public delegate void UpdateMainMsgHandler(object sender, UpdateEventArgs e);
       // TextBox[] txtboxobject = new TextBox[] { };
        List<string> paraNameList;
        //GlobalMethods StaticVariablesvar = new GlobalMethods();
         
        string TestProcedureName = string.Empty;
        string TestProcedureID = string.Empty;
        string[] IPParalist;
        string testtype = string.Empty;
        bool isProcedureExecuted = false;
        DataTable proceduredt = new DataTable();
        string testCategory = string.Empty;
        string ProgrammLabelMsg = "PCBA";
        //string travelerbyte = string.Empty;
        string logedUserID = string.Empty;
        DataTable executionResultClone;
        int logedUserTypeIndex;
        bool ExecutionWithManualScan = true;
        public frmActionZCE(string[] ipPara)
        {
            InitializeComponent();
            testtype = ipPara[0];
            TestProcedureName = ipPara[1];
            logedUserID = ipPara[32];
            if (!int.TryParse(ipPara[33], out logedUserTypeIndex)) logedUserTypeIndex = 0;
            txtCustomer.Text = TestProcedureName;
            IPParalist = ipPara;
            TestProcedureID = TestProcedureName;
            
        }

        private void EMS_TEST_Load(object sender, EventArgs e)
        {
            try
            {

                pbExecutionStatus.Image = null;
                FillTestLists();
                if (testCategory.Trim().Length <= 0)
                {
                    MessageBox.Show("Invalid Test Type !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
                txtTestType.Text = testCategory;
                IPParalist[6] = "300";//-------Hard Codded For ZCE
                objccmdmethod.SetDefaultSettings(IPParalist);
                DisplayMainStatus();
                txtPCBAID.Text = "";
                txtPCBAID.Focus();

            }
            catch (Exception Ex)
            {
                MessageBox.Show("Unable To Run Test !" + "\n" + Ex.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

        }
      
        public void AddressForm_PingLed(object sender, UpdateEventArgs e)
        {
            if (e.isError) dlmsCommStatusmsh.ForeColor = Color.Red;
            else dlmsCommStatusmsh.ForeColor = Color.Green;
            dlmsCommStatusmsh.Text = e.msg;
            Application.DoEvents(); 
        }

        public void DisplayMainStatus()
        {
            try
            {
                dlmsCommStatusmsh.Text = "";
                dlmsCommStatusmsh.Width = (DLMSStas.Width / 2)  + 50;
                //------------------------------DLMS Mode Settings----------------------
                string ClientSAP = Convert.ToInt32(objappSettings.GetClientSAP(), 10).ToString("X");
                string SerialPort = objappSettings.GetPortName();
                string dlmscommmode = "";

                if (ClientSAP == "10")
                    dlmscommmode += " PC ";
                else if (ClientSAP == "20")
                    dlmscommmode += " MR ";
                else if (ClientSAP == "30")
                    dlmscommmode += " US ";
                else if (ClientSAP == "40")
                    dlmscommmode += " FS ";
               
                //dlmscommmode += ",  " + SerialPort;
                dlmscommmode = " " + SerialPort;
                //--------------------------------------------------------------------


                string Communication = string.Empty;
                string Company_Profile = string.Empty;
                string filename = AppDomain.CurrentDomain.BaseDirectory + @"\SMPMP.exe";
                string strbuilton = "Built On: " + File.GetCreationTime(filename).ToShortDateString().ToString();


                //SerialComms objSerialComm = new SerialComms();
                bool Flg_ComSetting = true;// objSerialComm.GetPortSettings();
                if (Flg_ComSetting)
                {
                    Communication = dlmscommmode;

                }
                string Prduct_Version = SystemInfo.ProductVersion();
                //lbl_Background.Text = "Smart Meter Product Management Programm Ver " + Prduct_Version;
                string strpath = AppDomain.CurrentDomain.BaseDirectory + "SMPMP.exe";
                Company_Profile = SystemInfo.CopyRightsDetail();
                Prduct_Version = Application.ProductName.ToString() + " Ver. " + Prduct_Version;

                stsReady.Text = "Association : " + dlmscommmode;
                lblversion.Text = Prduct_Version;// +" | " + strbuilton;
               
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Setting Main Form Status Failed !" + "\r\n" + "\r\n" + Ex.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
            }
        }

        private void FillTestLists()
        {
            XMLExportImport objexpxml = new XMLExportImport();
            DataTable dt = objexpxml.ImportXMLToDatatable(TestProcedureName);
            if (dt != null && dt.Rows.Count > 0)
            {
                proceduredt = dt;
                int rowcnt = 0;
                paraNameList = new List<string>();
                foreach (DataRow rw in dt.Rows)
                {
                    testCategory = rw[2].ToString();
                    if (rw[4].ToString() == "0") { btnStart.Enabled = false; ExecutionWithManualScan = true; } //---if Serialization Windows ==0
                    else { ExecutionWithManualScan = false; btnStart.Enabled = true; }
                    if (rw[10].ToString().ToUpper() == "TRUE")
                    {
                        DGVParaLists.Rows.Add();
                        DGVParaLists.Rows[rowcnt].Cells[0].Value = (rowcnt + 1).ToString();
                        string paraname = rw[6].ToString();
                        DGVParaLists.Rows[rowcnt].Cells[1].Value = paraname;
                        paraname = paraname.Replace(" ", "");
                        paraNameList.Add(paraname.ToUpper());
                        DGVParaLists.Rows[rowcnt].Cells["colDefaultValue"].Value = rw[7].ToString();
                        DGVParaLists.Rows[rowcnt].Cells["ColMinVal"].Value = rw[8].ToString();
                        DGVParaLists.Rows[rowcnt].Cells["ColMaxValue"].Value = rw[9].ToString();
                        rowcnt++;
                    }
                }
                if (testCategory.IndexOf("Serialization") >= 0) { txtPCBAID.MaxLength = 16; lblParaonTestType.Text = "Scan Meter ID"; ProgrammLabelMsg = "Meter"; lblLastScan.Text = "Last Scan Meter ID"; }
                else { txtPCBAID.MaxLength = 10; lblParaonTestType.Text = "Scan PCBA ID"; ProgrammLabelMsg = "PCBA"; lblLastScan.Text = "Last Scan PCBA ID"; }
            }
        }       

        private void btnStart_Click(object sender, EventArgs e)
        { 
           
            try
            {
                grpinputs.Enabled = false;
                DataSet ds = new DataSet();
                this.Cursor = Cursors.WaitCursor;
                if (!ProgrammLabelMsg.ToUpperInvariant().Contains("PCBA") && txtPCBAID.Text.Trim().Length <= 0)
                {
                    MessageBox.Show("Please Scan Valid " + ProgrammLabelMsg + " ID !, ID Should Not Blank!", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                objLI.UpdatedLed += new LayerInterface.UpdateHandler(AddressForm_PingLed);
                objIECLI.UpdatedLed += new IECLayerInterface.UpdateHandler(AddressForm_PingLed);
                if (DGVParaLists.Rows.Count <= 0)
                {
                    MessageBox.Show("No Active Procedure to Execute Test!", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                objexeresult.PCBAID = txtPCBAID.Text.Trim();
                objexeresult.MeterID = txtPCBAID.Text.Trim();//-In Case of Serialization
                objexeresult.ExecutionProcedureType = testCategory;
                objexeresult.ExecutionMeterType = proceduredt.Rows[0]["MeterType"].ToString();
                objexeresult.ExecutionTestID = TestProcedureName;
                //--------------------------------Execution ----------------------------------------------------------
                bool finalExecutionStatus = ExecuteProcedure();
                if (finalExecutionStatus) pbExecutionStatus.Image = ZCE.Properties.Resources.ExecutionPass;
                else pbExecutionStatus.Image = ZCE.Properties.Resources.ExecutionFail;

                txtLastScanID.Text = txtPCBAID.Text.Trim();
                if (objexeresult.FinalResult == "PASS" && finalExecutionStatus) { txtPCBAID.Text = ""; }
                 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                isProcedureExecuted = false;
                this.Cursor = Cursors.Default;
                grpinputs.Enabled = true;
                if (ExecutionWithManualScan) txtPCBAID.Focus();
                else btnStart.Focus();
            }
                
        }      

        private bool ExecuteProcedure()
        {
            try
            {
                int testExecutionStatus = -1;
                isProcedureExecuted = true;
                this.Cursor = Cursors.WaitCursor;
                pbExecutionStatus.Image = ZCE.Properties.Resources.ExecutionWait;
                Application.DoEvents();
                int string_length = 0;
                string MeterID = string.Empty;             
                while (string_length < DGVParaLists.Rows.Count)
                {
                    DGVParaLists.Rows[string_length].Cells["colRemarks"].Value = "";
                    DGVParaLists.Rows[string_length].Cells["colStatus"].Value = "";
                    DGVParaLists.Rows[string_length].Cells["colStatus"].Style.BackColor = Color.White;
                    string_length++;
                }
                int selectedControlIDX = 0;
                
                string_length = test_index;
                objLI.DisplayStatusMsg("", false);
                txtMeterPCBAID.Text = "";

                if (!objIECLI.ConnectToMeter())
                {
                    MessageBox.Show("Unable To Communicate, Meter Should Compatible to Selected Test Procedure !" + "\n" + "Please Verify Communication Settings .", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                } 
                string meterSignonResponse = objIECLI.MeterSignonResponse;
                if (meterSignonResponse.IndexOf(StaticVariables.MeterType_ZCE) < 0)
                {
                    MessageBox.Show("Invalid Test/Meter Type Selected !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                selectedControlIDX = 0;
                //-----------------------Read PCBA ID-------------------------------------------
                 string getpcbaResponse = objIECccmdmethod.ReadPCBAID_ZCE();
                 if (objexeresult.ExecutionProcedureType.IndexOf(StaticVariables.TestType_EMS) < 0)
                 {
                    // txtMeterPCBAID.Text = getpcbaResponse;
                     if (getpcbaResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0 || getpcbaResponse.Trim().Length < 7) { MessageBox.Show("Invalid PCBA ID, Length Should be >= 7 " + "\n" + getpcbaResponse, "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop); return false; }
                 }
                 //-----------------------New Implementation for WO Scan-------------------
                 txtMeterPCBAID.Text = getpcbaResponse;
                 if (objexeresult.ExecutionProcedureType.IndexOf(StaticVariables.TestType_SR) < 0)
                 {
                     if (ExecutionWithManualScan == true && getpcbaResponse != txtPCBAID.Text.Trim())
                     {
                         MessageBox.Show("Scan PCBA ID and Meter PCBA ID Not Match !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                         return false;
                     }
                     if (ExecutionWithManualScan == false) txtPCBAID.Text = getpcbaResponse;
                 }
                 objexeresult.PCBAID = txtPCBAID.Text.Trim();
                 objexeresult.MeterID = txtPCBAID.Text.Trim();//-In Case of Serialization
                 if (objexeresult.ExecutionProcedureType.IndexOf(StaticVariables.TestType_OT) < 0)
                 {
                     DataSet ds = new DataSet();
                     objIECLI.DisplayStatusMsg("Checking Database ...", false);
                     ds = objexere.VerifyExecutionStatusInDatabase(objexeresult);
                     if (ds == null) return false;
                     executionResultClone = ds.Tables[0].Clone();
                     if (executionResultClone == null || executionResultClone.Columns.Count <= 0) return false;
                     objIECLI.DisplayStatusMsg("Tests In Execution, Please Wait ...", false);
                 }
                 //-----------------------------------------------------------------------------
                while (selectedControlIDX < paraNameList.Count)
                {
                    string ParaToBeExecute = paraNameList[selectedControlIDX];
                     
                    DGVParaLists.Rows[selectedControlIDX].Selected = true;
                    if (selectedControlIDX > 0 && DGVParaLists.Rows[selectedControlIDX - 1].Cells["colRemarks"].Value.ToString() == (StaticVariables.ERRORPreFix + "COMM Failed.")) break;
                    DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Wait...";
                    if (selectedControlIDX % 10 == 0) DGVParaLists.FirstDisplayedScrollingRowIndex = selectedControlIDX;
                    Application.DoEvents();
                    testExecutionStatus = -1;
                    switch (ParaToBeExecute)
                    {                        

                        case "VERIFYPCBAID":
                            string meterpcbaID = objIECccmdmethod.ReadPCBAID_ZCE();
                            string isvalidPCBA = objccmdmethod.VerifyPCBAIDwithReadID(txtPCBAID.Text, meterpcbaID, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = isvalidPCBA;
                            if (isvalidPCBA.IndexOf(StaticVariables.ERRORPreFix) < 0) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass; break; }
                            else
                            {
                                DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Fail";
                                DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Style.BackColor = Color.Red;
                                testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                                return false;
                            }
                        case "VERIFYMETERFWVERSION":                            
                            string fwVersionResponse =  DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString();
                            if (meterSignonResponse.Length >= 14) DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = meterSignonResponse.Substring(11, 7);
                            else DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = meterSignonResponse;
                            if (fwVersionResponse.IndexOf(StaticVariables.ERRORPreFix) < 0 && meterSignonResponse.IndexOf(fwVersionResponse) >= 0) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass; break; }
                            else 
                            {
                                DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Fail";
                                DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Style.BackColor = Color.Red;
                                testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                                return false;
                            }


                        case "VERIFYCALIBRATIONDATA":
                            string caliRespResponse = objIECccmdmethod.VerifyCalibrationDataZCE();
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = caliRespResponse;
                            if (caliRespResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                         
                        case "WRITEPCBAID":
                            string writePCBAResponse = objIECccmdmethod.WritePCBAIDZCE(txtPCBAID.Text.Trim());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = writePCBAResponse;
                            if (writePCBAResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "WRITEMETERID":
                            string writemIDResponse = objIECccmdmethod.WriteMeterIDZCE(txtPCBAID.Text.Trim());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = writemIDResponse.Trim();
                            if (writemIDResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        
                                           
                        case "SETMETERRTC":
                            string SetRTCResponse = objIECccmdmethod.SetMeterRTCZCE();
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = SetRTCResponse;
                            if (SetRTCResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;                    
                                                                                     
                        case "RTCDRIFTTEST":
                            string rtcDriftResponse = objIECccmdmethod.TestRTCDriftZCE(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = rtcDriftResponse;
                            if (rtcDriftResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;                        
                        case "CASETAMPERTEST":
                            string caseResponse = objIECccmdmethod.CheckingCaseTamperZCE();
                            if (caseResponse.IndexOf(StaticVariables.ERRORPreFix) < 0)testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;                            
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = caseResponse;
                            break;                        
                        case "READPCBAID":
                            string pcbaResponse = objIECccmdmethod.ReadPCBAID_ZCE();                          
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = pcbaResponse.Trim();
                            if (pcbaResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) { txtMeterPCBAID.Text = pcbaResponse; testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass; }
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;  
                            break;                     
                        
                        case "LOCKINGMETER":
                            if (objccmdmethod.GetExecutionStatus(DGVParaLists.Rows.Count - 1, DGVParaLists))
                            {
                                string lockResponse = objIECccmdmethod.LockingMeterZCE("FF");
                                DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = lockResponse.Trim();
                                if (lockResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                                else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            }
                            break;
                        case "LOCKINGMETERWOTOU":
                            if (objccmdmethod.GetExecutionStatus(DGVParaLists.Rows.Count - 1, DGVParaLists))
                            {
                                string lockResponse = objIECccmdmethod.LockingMeterZCE("BF");
                                DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = lockResponse.Trim();
                                if (lockResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                                else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            }
                            break;
                        case "MANUALMESSAGE":
                            if (DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString().Trim().Length > 0)
                            {
                                MessageBox.Show(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            }
                            else { DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = "No Message To Display !"; testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail; }
                            break;

                    }
                    if (testExecutionStatus == (int)StaticVariables.ExecutionReurnStatus.Pass) { DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Pass"; }
                    else
                    {
                        if (DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value.ToString().IndexOf("COMM Failed") >= 0) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.ComFail; }
                        if (DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value.ToString().IndexOf("ER20") >= 0) { DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = "Meter Locked >> " + DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value.ToString(); testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.ComFail; }//----If Meter is Lock Case
                        if (DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value.ToString().IndexOf("ER28") >= 0) { DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = "Meter Locked >> " + DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value.ToString(); testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.ComFail; }//----If Meter is Lock Case

                        if (testExecutionStatus == (int)StaticVariables.ExecutionReurnStatus.Fail)
                        {
                            DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Fail"; DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Style.BackColor = Color.Red;
                            if (logedUserTypeIndex == (int)StaticVariables.userCategory.UserValidation) { selectedControlIDX++; continue; } //----Continue for validation only

                        }
                        else if (testExecutionStatus == (int)StaticVariables.ExecutionReurnStatus.ComFail) { DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Fail"; DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Style.BackColor = Color.Red; }
                        else { DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Not Executed"; DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Style.BackColor = Color.Yellow; }
                        break;
                    }
                    Application.DoEvents();
                    Application.DoEvents();
                    selectedControlIDX++;
                }
                //----------------------------End of Test Points---------------------------------------------              
                bool executionsts = objccmdmethod.GetExecutionStatus(DGVParaLists.Rows.Count, DGVParaLists);
                if (!executionsts) objexeresult.FinalResult = "FAIL";
                else objexeresult.FinalResult = "PASS";
                if (objexeresult.ExecutionProcedureType.IndexOf(StaticVariables.TestType_OT) < 0)
                {
                    //return (ExportResultsByBatch());
                    if (ExportResultsByBatch())
                        return true;
                    else if (executionsts && (new ExcecutionResultImportExport().SaveExecutionData(DGVParaLists, executionResultClone, objexeresult)))
                    {
                        objIECLI.DisplayStatusMsg("Saved Results to File!", true);
                        //return false;
                    }
                    else
                        return false;
                }
                
                if (selectedControlIDX >= 1) DGVParaLists.Rows[selectedControlIDX - 1].Selected = false;
                return executionsts;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                objIECLI.AssociationDisconnect();
                this.Cursor = Cursors.Default;
                 
            }

        }

        private bool ExportResultsByBatch()
        {
            string resultmsg = "";
            dlmsCommStatusmsh.Text = "Saving Test Result...";
            Application.DoEvents();
            if (lblParaonTestType.Text.IndexOf("Meter") >= 0) objexeresult.MeterID = txtPCBAID.Text.Trim();
            else objexeresult.MeterID = "";
            if (lblParaonTestType.Text.IndexOf("PCBA") >= 0) objexeresult.PCBAID = txtPCBAID.Text.Trim();
            else objexeresult.PCBAID = txtMeterPCBAID.Text.Trim();
            objexeresult.CustomerName = txtCustomer.Text.Trim();
            objexeresult.ExecutionMeterType = proceduredt.Rows[0]["MeterType"].ToString();
            objexeresult.ExecutionProcedureType = proceduredt.Rows[0]["ProceduteType"].ToString();
            objexeresult.ExecutionTestID = TestProcedureID;
            objexeresult.ExecutionProgramName = proceduredt.Rows[0]["ProgramName"].ToString();
            objexeresult.ExecutionDate = DateTime.Now;
            objexeresult.LogedUserID = logedUserID;
            try
            {
                resultmsg = objexere.InsertinToTabExecutionResults(DGVParaLists, executionResultClone, objexeresult);
                if (resultmsg != "")
                {
                    objIECLI.DisplayStatusMsg("Unable To Save Results !", true);
                    MessageBox.Show("Unable To Save Results !" + "\n" + resultmsg, "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                DataSet ds = objexere.Select_GetExecutionResult_onPCBAID_ProType_ExeDate(objexeresult);
                objIECLI.DisplayStatusMsg("Test Executed !", false);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["FinalStatus"].ToString().ToUpperInvariant() != "PASS") return false;

                }
                else
                {
                    objIECLI.DisplayStatusMsg("Scan Meter Record Not Found in Database !", true);
                    MessageBox.Show("Scan Meter Record Not Found in Database !, Re-Scan and Try Again !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
               
                return true;
            }
            catch (Exception Ex)
            {
                resultmsg = Ex.Message;
                objIECLI.DisplayStatusMsg("Unable To Save Results !", true);
                MessageBox.Show("Unable To Save Results !" + "\n" + Ex.Message, "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            finally
            {
                if (resultmsg != "") CommonMethods.LogPMPMessage("Unable To Save Results : " + resultmsg);

            }
        }                
     
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtMeterID_KeyUp(object sender, KeyEventArgs e)
        {
            int maxlen = 10;
            if (lblParaonTestType.Text.ToUpper().IndexOf("METER ID") >= 0) maxlen = 16;
            if (isProcedureExecuted) return;
            if (txtPCBAID.Text.Trim().Length == maxlen)
            {
                if (txtPCBAID.Text.Trim().Length > 0) btnStart_Click(sender, e);
            }          
        }

        private void txtPCBAID_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    if (txtPCBAID.Text.Trim().Length <= 0)
                    {
                        MessageBox.Show("Please Scan Valid " + ProgrammLabelMsg + " ID !, ID Should Not Blank!", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    btnStart_Click(sender, e);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }        
        }

     
 
    }
}
