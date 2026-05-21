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
namespace E150MICROSTAR
{
    public partial class frmActionMicroStar : Form
    {         
        int test_index = 0;
        IECLayerInterface objIECLI = new IECLayerInterface();
        BALExecutionResults objexere = new BALExecutionResults();
        EntityExecutionResult objexeresult = new EntityExecutionResult();
        //LayerInterface objLI = new LayerInterface();        
        AppSettings objappSettings = new AppSettings();
        MyCrypro objcrypt = new MyCrypro();
        CommonCommandMethods objccmdmethod = new CommonCommandMethods();
         IECCommonCommandMethods objIECccmdmethod = new IECCommonCommandMethods();
        public delegate void UpdateMainMsgHandler(object sender, UpdateEventArgs e);
        //TextBox[] txtboxobject = new TextBox[] { };
        List<string> paraNameList;
       // GlobalMethods StaticVariablesvar = new GlobalMethods();
        //enum ExecutionReurnStatus { Pass = 0, Fail = 1, ComFail = 2 };       
        string TestProcedureName = string.Empty;
        string TestProcedureID = string.Empty;
        string[] IPParalist;
        string testtype = string.Empty;
        bool isProcedureExecuted = false;
        DataTable proceduredt = new DataTable();
        string testCategory = string.Empty;
        string ProgrammLabelMsg = "PCBA";
       // string travelerbyte = string.Empty;
        int TravelerStage = 0;
        string logedUserID = string.Empty;
        DataTable executionResultClone;
        byte configChecFlag = 0;
        string ConfigFilePath = "";
        int logedUserTypeIndex;
        bool ExecutionWithManualScan = true;
        bool ExecutionWithOutTravellerStage = false;
        public frmActionMicroStar(string[] ipPara)
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
                TravelerStage = objccmdmethod.SetTravelerStage(testCategory);
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
                dlmsCommStatusmsh.Width = (DLMSStas.Width / 2)  + 30;
                //------------------------------DLMS Mode Settings----------------------
                string ClientSAP = Convert.ToInt32(objappSettings.GetClientSAP(), 10).ToString("X");
                string SerialPort = objappSettings.GetPortName();
                string dlmscommmode = "";

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
                testCategory = dt.Rows[0]["ProceduteType"].ToString();
                BitArray otherSettings = new BitArray(new int[] { Convert.ToInt32(dt.Rows[0]["SerializationWindow"].ToString()) });
                ExecutionWithManualScan = !otherSettings[0];
                btnStart.Enabled = otherSettings[0];
                ExecutionWithOutTravellerStage = otherSettings[1];
                foreach (DataRow rw in dt.Rows)
                {
                    //testCategory = rw[2].ToString();
                    //if (rw[4].ToString() == "0") { btnStart.Enabled = false; ExecutionWithManualScan = true; } //---if Serialization Windows ==0
                    //else { ExecutionWithManualScan = false; btnStart.Enabled = true; }
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
                if (finalExecutionStatus) pbExecutionStatus.Image = E150MICROSTAR.Properties.Resources.ExecutionPass;
                else pbExecutionStatus.Image = E150MICROSTAR.Properties.Resources.ExecutionFail; 
                
                txtLastScanID.Text = txtPCBAID.Text.Trim();
                if (objexeresult.FinalResult == "PASS" && finalExecutionStatus)  { txtPCBAID.Text = ""; }
              
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
                objIECccmdmethod.IsTravellerWriteSkip = ExecutionWithOutTravellerStage;
                int testExecutionStatus = -1;
                isProcedureExecuted = true;
                this.Cursor = Cursors.WaitCursor;
                pbExecutionStatus.Image = E150MICROSTAR.Properties.Resources.ExecutionWait;
                Application.DoEvents();
                int string_length = 0;
                string MeterID = string.Empty;
                bool isComError = false;
                while (string_length < DGVParaLists.Rows.Count)
                {
                    DGVParaLists.Rows[string_length].Cells["colRemarks"].Value = "";
                    DGVParaLists.Rows[string_length].Cells["colStatus"].Value = "";
                    DGVParaLists.Rows[string_length].Cells["colStatus"].Style.BackColor = Color.White;
                    string_length++;
                }
                
                
                string_length = test_index;
                objIECLI.DisplayStatusMsg("", false);
                txtMeterPCBAID.Text = "";

                if (!objIECLI.ConnectToMeter())
                {
                    MessageBox.Show("Unable To Communicate, Meter Should Compatible to Selected Test Procedure !" + "\n" + "Please Verify Communication Settings .", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                } 
                string meterSignonResponse = objIECLI.MeterSignonResponse;
                if (meterSignonResponse.IndexOf(StaticVariables.MeterType_EcoStar) < 0)
                {
                    MessageBox.Show("Invalid Test/Meter Type Selected !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                
                //-----------------------Read PCBA ID-------------------------------------------
                string getpcbaResponse = objIECccmdmethod.ReadPCBAID();
                if (objexeresult.ExecutionProcedureType.IndexOf(StaticVariables.TestType_EMS) < 0)
                {
                    //txtMeterPCBAID.Text = getpcbaResponse;
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
                    if (ds == null) return false; ;
                    executionResultClone = ds.Tables[0].Clone();
                    if (executionResultClone == null || executionResultClone.Columns.Count <= 0) return false; ;
                    objIECLI.DisplayStatusMsg("Tests In Execution, Please Wait ...", false);
                }
                //-----------------------------------------------------------------------------
                int selectedControlIDX = 0;
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
                        case "VERIFYPRODUCTSTAGE":
                            string tavelerreadResponse = objIECccmdmethod.VerifyTraveler_VIMNONDLMS(TravelerStage);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = tavelerreadResponse;
                            if (tavelerreadResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass; break; }
                            else
                            {
                                DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Fail";
                                DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Style.BackColor = Color.Red;
                                testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                                return false;
                            }
                        case "VERIFYPCBAID":
                            string meterpcbaID = objIECccmdmethod.ReadPCBAID();
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
                            string fwVersionResponse =MeterFirmwareVersionTest(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(),meterSignonResponse);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = fwVersionResponse;
                            if (fwVersionResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else 
                            {
                                DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Fail";
                                DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Style.BackColor = Color.Red;
                                testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail; return false;                                  
                            }
                            break;
                        case "VERIFYCTWIRE":
                            string ctWireResponse = objIECccmdmethod.VerifyCTWire(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = ctWireResponse;
                            if (ctWireResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYPHASECURRENT":
                            MessageBox.Show("Press Phase Current Switch And Hit OK To Continue.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Application.DoEvents();
                            string phaseCurrentResponse = objIECccmdmethod.VerifyMeterCurrent(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString(),(int)StaticVariables.MeterCurrentType.PhaseCurrent);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = phaseCurrentResponse;
                            if (phaseCurrentResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYNEUTRALCURRENT":
                            MessageBox.Show("Press Neutral Current Switch And Hit OK To Continue.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Application.DoEvents();
                            string neuCurrentResponse = objIECccmdmethod.VerifyMeterCurrent(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString(), (int)StaticVariables.MeterCurrentType.NeutralCurrent);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = neuCurrentResponse;
                            if (neuCurrentResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "PUSHBUTTONTEST":                            
                            string pushButtonResponse = PushButtonTest(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = pushButtonResponse;
                            if (pushButtonResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYENERGY":
                            string readenergyResponse = objIECccmdmethod.VerifyEnergy(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = readenergyResponse;
                            if (readenergyResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYMAGNETCOUNTS":
                            string magTamperCountResponse = "";
                            string defaultVal = DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString();
                            string refminVal = DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString();
                            string refmaxVal = DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString();
                            if (defaultVal.Trim().Length <= 0 && refminVal.Trim().Length <= 0 && refmaxVal.Trim().Length <= 0) magTamperCountResponse = objIECccmdmethod.CheckingMagnetTamperStatus();//------For VIM Specefic
                            else magTamperCountResponse = objIECccmdmethod.CheckingMagnetTamperStatus(defaultVal, refminVal, refmaxVal);//-----For MicroStar Specefic
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = magTamperCountResponse;
                            if (magTamperCountResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYMETERPASSWORD":
                            string meterpwdResponse = objIECccmdmethod.VerifyMeterPassword(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = meterpwdResponse;
                            if (meterpwdResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYCALIBRATIONDATA":
                            string caliRespResponse = objIECccmdmethod.VerifyCalibrationData_MicroStar();
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = caliRespResponse;
                            if (caliRespResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "WRITEPCBAID":
                            string writePCBAResponse = objIECccmdmethod.WritePCBAID(txtPCBAID.Text.Trim());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = writePCBAResponse;
                            if (writePCBAResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "WRITEMETERID":
                            string writemIDResponse = objIECccmdmethod.WriteMeterID(txtPCBAID.Text.Trim());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = writemIDResponse.Trim();
                            if (writemIDResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYCONFIGURATION":
                            ConfigFilePath = DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString();
                            if (configChecFlag < 100) configChecFlag++;
                            byte configreadFlag = 0;
                            if (Byte.TryParse(DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), out configreadFlag)) { }
                            else if (Byte.TryParse(DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString(), out configreadFlag)) { }
                            if (configreadFlag > 0 && configChecFlag < configreadFlag) { DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = "Test Skip by Software"; testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass; break; }
                            string readconfigResponse = objIECccmdmethod.VerifyDisplayConfigData(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = readconfigResponse;
                            if (readconfigResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass; configChecFlag = 0; }
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "RESETBILLINGDATA":
                            string resetBillingResponse = objIECccmdmethod.ResetingFrauds("01","F001");
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = resetBillingResponse;
                            if (resetBillingResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "RESETALLDATA":
                            string resetallResponse = objIECccmdmethod.ResetingFrauds("FF","F001");
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = resetallResponse;
                            if (resetallResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            Thread.Sleep(1000);
                            break;
                        case "RESETLOWBATT":
                            string lowBatResponse = objIECccmdmethod.ResetingFrauds("08", "F001");
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = lowBatResponse;
                            if (lowBatResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "SETMETERRTC":
                            string SetRTCResponse = objIECccmdmethod.SetMeterRTC();
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = SetRTCResponse;
                            if (SetRTCResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYTEMPERATURE":
                            string tempratureResponse = objIECccmdmethod.VerifyTemprature_VIM(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = tempratureResponse;
                            if (tempratureResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;  
                        case "VERIFYBATTERYSTATUS":
                            string batterystatusResponse = objIECccmdmethod.CheckingBatteryStatus();
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = batterystatusResponse;
                            if (batterystatusResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;                                                                 
                        case "RTCDRIFTTEST":
                            string rtcDriftResponse = objIECccmdmethod.TestRTCDrift(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = rtcDriftResponse;
                            if (rtcDriftResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        
                        case "CASETAMPERTEST":
                            if (ConfigFilePath.Length <= 0) ConfigFilePath = DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString();//---If Verify Configuration No Required Then Pass CFG File Path Here
                            string caseResponse = objIECccmdmethod.CheckingCaseTamper(ConfigFilePath, DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString());
                            if (caseResponse.IndexOf(StaticVariables.ERRORPreFix) < 0)testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;                            
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = caseResponse;
                            break;
                        case "VERIFYCASETAMPER":
                            string verifycaseResponse = objIECccmdmethod.VerifyCaseTamper();
                            if (verifycaseResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = verifycaseResponse;
                            break;
                        case "READPCBAID":
                            string pcbaResponse = objIECccmdmethod.ReadPCBAID();                             
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = pcbaResponse.Trim();
                            if (pcbaResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) { txtMeterPCBAID.Text = pcbaResponse; testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass; }
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;  
                            break;
                     
                        case "WRITEMANUFACTURINGDATA":
                            string mfgResponse = objIECccmdmethod.WriteManufacturingData(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = mfgResponse.Trim();
                            if (mfgResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "WRITEPMAX":
                            string pmaxResponse = objIECccmdmethod.WritePmaxValue(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = pmaxResponse;
                            if (pmaxResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "COMMUNICATIONTESTONBATTERY":
                            objIECLI.PortDisconnect();
                            MessageBox.Show("Power-on Meter on Battery Mode And Hit OK To Continue.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Application.DoEvents();
                            Thread.Sleep(100);

                            string signonResponse = objIECccmdmethod.IsValidVoltage(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), "0");//-Max Value Must be 0 as meter voltage must be <=0 in battery mode                          
                            objIECLI.PortDisconnect();
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = signonResponse;
                            if (signonResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;break;}
                            
                            Thread.Sleep(100);
                            MessageBox.Show("Power-on Meter on Main Supply Mode And Hit OK To Continue.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Thread.Sleep(1500);
                            if (!objIECLI.ConnectToMeter()) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail; DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = StaticVariables.ERRORPreFix + "COMM Failed."; break; }
                            Application.DoEvents();
                            break;
                        case "COMMUNICATIONTESTONBATTERYWOMAINS":
                            Application.DoEvents();
                            string signonResponseonBattery = objIECccmdmethod.IsValidVoltage(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), "0");//-Max Value Must be 0 as meter voltage must be <=0 in battery mode                          
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = signonResponseonBattery;
                            if (signonResponseonBattery.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "COMMUNICATIONTEST":                           
                            string CommtestResponse = CommunicationTest(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = CommtestResponse;
                            if (CommtestResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "METERUNLOCK":
                            string unloacmeterResponse = objIECccmdmethod.LockingMeter("00");
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = unloacmeterResponse.Trim();
                            if (unloacmeterResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "LOCKINGMETERALL":
                            if (objccmdmethod.GetExecutionStatus(DGVParaLists.Rows.Count - 1, DGVParaLists))
                            {
                                string lockResponse = objIECccmdmethod.LockingMeter("FF");
                                DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = lockResponse.Trim();
                                if (lockResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                                else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            }
                            break;
                        case "LOCKINGMETERWOTOU":
                            if (objccmdmethod.GetExecutionStatus(DGVParaLists.Rows.Count - 1, DGVParaLists))
                            {
                                string lockResponse = objIECccmdmethod.LockingMeter("BF");
                                DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = lockResponse.Trim();
                                if (lockResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                                else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            }
                            break;
                        case "LOCKINGMETERWOTOUANDDISPLAY":
                            if (objccmdmethod.GetExecutionStatus(DGVParaLists.Rows.Count - 1, DGVParaLists))
                            {
                                string lockResponse = objIECccmdmethod.LockingMeter("BD");
                                DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = lockResponse.Trim();
                                if (lockResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                                else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            }
                            break;
                        case "SETPRODUCTSTAGE": //---SETPRODUCTSTAGE Must be 2nd Last for Serialization before LOCKING Meter and last for other type of test 
                            int testPointsCount = 1;
                            if (objexeresult.ExecutionProcedureType.IndexOf(StaticVariables.TestType_SR) >= 0) testPointsCount = 2;
                            if (objccmdmethod.GetExecutionStatus(DGVParaLists.Rows.Count - testPointsCount, DGVParaLists))
                            {
                                if (objexeresult.ExecutionProcedureType.IndexOf(StaticVariables.TestType_OT) < 0)
                                {
                                    if (!isComError && !ExecutionWithOutTravellerStage)
                                    {
                                       if (objIECccmdmethod.SetTravelerData_VIMNONDLMS(TravelerStage))testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                                       break;
                                    }
                                }
                            }
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = "Unable To Set Production Stage Flag, Meter May at Invalid Production Stage !";
                            testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
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
                        if (DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value.ToString().IndexOf("COMM Failed") >= 0) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.ComFail; isComError = true; }
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
                    //if (!isComError)
                    //{
                    //    if (objexeresult.FinalResult == "PASS")
                    //    {
                    //        if (!objIECccmdmethod.SetTravelerData_VIMNONDLMS(TravelerStage)) { MessageBox.Show("Unable To Set Production Stage Flag, Meter May at Invalid Production Stage !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
                    //    }
                    //}
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
            
            //if (lblParaonTestType.Text.ToUpper().IndexOf("METER ID") >= 0) maxlen = 16;
            if (isProcedureExecuted) return;
            if (txtPCBAID.Text.Trim().Length == txtPCBAID.MaxLength)
            {
                if (txtPCBAID.Text.Trim().Length > 0) { btnStart_Click(sender, e); }
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

        private string PushButtonTest(string defaultVal, string minVal, string maxVal)
        {
            try
            {
                int pushCount;
                
                if (!int.TryParse(defaultVal, out pushCount))
                {
                    if (!int.TryParse(minVal, out pushCount))
                    {
                        if (!int.TryParse(maxVal, out pushCount)) { return StaticVariables.ERRORPreFix + "Input Parameters Can't be Blank !"; }
                    }
                }
                string pushButtonCountResponse = objIECccmdmethod.StartPushButtonRest();
                if (pushButtonCountResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return pushButtonCountResponse;//-----If Fail the return
                
                MessageBox.Show("Press Push Button Switch " + pushCount.ToString() + " Time And Hit OK To Continue.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.DoEvents();
                return objIECccmdmethod.VerifyPushButtonCount(defaultVal, minVal, maxVal);
            }
            catch (Exception Ex)
            {
                return StaticVariables.ERRORPreFix + Ex.ToString();
            }
        }

        private string MeterFirmwareVersionTest(string DefaultfwValue, string meterSignonResponse)
        {     
            try
            {
                string tempMeterfwVersion = "";
                if (meterSignonResponse.Length >= 14) tempMeterfwVersion = meterSignonResponse.Substring(8, 6);
                else tempMeterfwVersion = meterSignonResponse;
                if (DefaultfwValue.IndexOf(StaticVariables.ERRORPreFix) >= 0 || meterSignonResponse.IndexOf(DefaultfwValue) < 0) return StaticVariables.ERRORPreFix + tempMeterfwVersion;
                {
                    string emsFwResponse = objIECccmdmethod.VerifyPCBAFirmwareVersion(DefaultfwValue);
                    tempMeterfwVersion += emsFwResponse;
                    if (emsFwResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) return tempMeterfwVersion; 
                    return StaticVariables.ERRORPreFix + tempMeterfwVersion;
                }
            }
            catch (Exception Ex)
            {
                return StaticVariables.ERRORPreFix + Ex.ToString();
            }
        }

        private string CommunicationTest(string Defaultpara)
        {
            string CurrentCOM = objappSettings.GetPortName();
            try
            {
                string[] comlist = Defaultpara.Split(',');
                if (comlist == null) return StaticVariables.ERRORPreFix + "No COM Port List To Test"; ;
                if (Defaultpara.Trim().Length <= 0) return StaticVariables.ERRORPreFix + "No COM Port List To Test";
                int porttestcnt = 0;
                while (porttestcnt < comlist.Length)
                {
                    string portName = comlist[porttestcnt++];
                    if (portName.Trim().Length <= 0) continue;
                    if (CurrentCOM == portName) continue;
                    objIECLI.AssociationDisconnect();
                    objappSettings.SetPortName(portName);
                    if (!objIECLI.ConnectToMeter()) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
                    Application.DoEvents();

                }
                return "";
            }
            catch (Exception ex)
            {
                return StaticVariables.ERRORPreFix + ex.Message;
            }
            finally
            {
                objIECLI.AssociationDisconnect();
                System.Threading.Thread.Sleep(200);
                objappSettings.SetPortName(CurrentCOM);
                objIECLI.ConnectToMeter();
            }
        }

        
    }
}
