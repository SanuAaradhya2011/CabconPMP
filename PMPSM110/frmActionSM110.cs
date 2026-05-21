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
using ApplicationInterface;
using Utilities;
using SerialCommunication;
using COMMONENTITY;
using BALLAYER;
using SystemSecurityLibrary;
namespace PMPSM110
{
    public partial class frmActionSM110 : Form
    { 
        int test_index = 0;
        //public static SerialComm objSerialComm = new SerialComm();
        BALExecutionResults objexere = new BALExecutionResults();
        EntityExecutionResult objexeresult = new EntityExecutionResult();
        LayerInterface objLI = new LayerInterface();
        IECLayerInterface objIECLI = new IECLayerInterface();        
        AppSettings objappSettings = new AppSettings();
        MyCrypro objcrypt = new MyCrypro();
        CommonCommandMethods objccmdmethod = new CommonCommandMethods();
        public delegate void UpdateMainMsgHandler(object sender, UpdateEventArgs e);
       // TextBox[] txtboxobject = new TextBox[] { };
        List<string> paraNameList ;
        GlobalMethods StaticVariablesvar = new GlobalMethods();
        //enum ExecutionReurnStatus { Pass = 0, Fail = 1, ComFail=2 };
        int TravelerStage = 0;
        string TestProcedureName = string.Empty;
        string TestProcedureID = string.Empty;
        string[] IPParalist;
        string testtype = string.Empty;
        bool isProcedureExecuted = false;
        DataTable proceduredt = new DataTable();
        string testCategory = string.Empty;
        string ProgrammLabelMsg = "PCBA";
       // string travelerbyte = string.Empty;
        string logedUserID = string.Empty;
        DataTable executionResultClone;
        int logedUserTypeIndex;
        bool ExecutionWithManualScan = true;
        bool ExecutionWithOutTravellerStage = false;
        public frmActionSM110(string[] ipPara)
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
            TravelerStage =objccmdmethod.SetTravelerStage(testCategory);
            if (TravelerStage < 0 && testCategory != "Other Test")
            {

                MessageBox.Show("Invalid Test Type !, Error: Setting Product Stage.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
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
                if (finalExecutionStatus) pbExecutionStatus.Image = PMPSM110.Properties.Resources.ExecutionPass;
                else pbExecutionStatus.Image = PMPSM110.Properties.Resources.ExecutionFail;

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
                objccmdmethod.IsTravellerWriteSkip = ExecutionWithOutTravellerStage;
                int testExecutionStatus = -1;
                isProcedureExecuted = true;
                this.Cursor = Cursors.WaitCursor;
                pbExecutionStatus.Image = PMPSM110.Properties.Resources.ExecutionWait;
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
                int selectedControlIDX = 0;                
                string[] PreTestPara = new string[] { "INITMETER", "LEDTEST", "SRPFTEST"};
                
                string_length = test_index;
                objLI.DisplayStatusMsg("", false);
                txtMeterPCBAID.Text = "";
               
                while (selectedControlIDX < paraNameList.Count)
                {
                    string ParaToBeExecute = paraNameList[selectedControlIDX];
                    if (!PreTestPara.Contains(ParaToBeExecute)) { selectedControlIDX++; continue; }
                    DGVParaLists.Rows[selectedControlIDX].Selected = true;
                    DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Wait...";
                    if (selectedControlIDX % 10 == 0) DGVParaLists.FirstDisplayedScrollingRowIndex = selectedControlIDX;
                    Application.DoEvents();
                    testExecutionStatus = -1;
                    switch (ParaToBeExecute)
                    {
                        case "INITMETER":
                            if (!InitMeter()) { DGVParaLists.Rows[0].Cells["colStatus"].Value = "INIT Fail"; DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Style.BackColor = Color.Red; return false; }
                            else { DGVParaLists.Rows[0].Cells["colStatus"].Value = "Pass"; }
                            break;
                        case "LEDTEST":
                            if (DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString().IndexOf("Manual") >= 0)
                            {
                               
                                fromLEDTest objledtest = new fromLEDTest(StaticVariablesvar);
                                objledtest.ShowDialog();
                                Application.DoEvents();
                                string LEDStatus = StaticVariablesvar.LEDTestResponse;
                                DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = LEDStatus;
                                if (LEDStatus.IndexOf(StaticVariables.DisplayNotWorking) >= 0) { DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Fail"; DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Style.BackColor = Color.Red; return false; }
                                else { DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Pass"; }
                            }
                            break;
                        case "SRPFTEST":
                            selectedControlIDX = paraNameList.IndexOf("SRPFTEST");
                            string srfpResponse = SRPFTest(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = srfpResponse;
                            if (srfpResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) { DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Fail"; DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Style.BackColor = Color.Red; return false; }
                            else { DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Pass"; }
                            break;                        
                            
                    }
                   selectedControlIDX++  ;
                }
                objappSettings.SetMeterMode((int)LayerInterface.MeterTypeInfo.Smart_Meter_1PH);
                if (!objLI.ConnectToMeter()) return false; 
                selectedControlIDX = 0;
                //-----------------------Read PCBA ID-------------------------------------------
                string getpcbaResponse = objccmdmethod.ReadPCBAID();
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
                    objLI.DisplayStatusMsg("Checking Database ...", false);
                    ds = objexere.VerifyExecutionStatusInDatabase(objexeresult);
                    if (ds == null) return false;
                    executionResultClone = ds.Tables[0].Clone();
                    if (executionResultClone == null || executionResultClone.Columns.Count <= 0) return false;
                    objLI.DisplayStatusMsg("Tests In Execution, Please Wait ...", false);
                }
                //-----------------------------------------------------------------------------
                while (selectedControlIDX < paraNameList.Count)
                {
                    string ParaToBeExecute = paraNameList[selectedControlIDX];                     
                    if (PreTestPara.Contains(ParaToBeExecute)) { selectedControlIDX++; continue; }
                    DGVParaLists.Rows[selectedControlIDX].Selected = true;
                    if (selectedControlIDX > 0 && DGVParaLists.Rows[selectedControlIDX - 1].Cells["colRemarks"].Value.ToString() == (StaticVariables.ERRORPreFix + "COMM Failed.")) break;
                    DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Wait...";
                    if (selectedControlIDX % 10 == 0) DGVParaLists.FirstDisplayedScrollingRowIndex = selectedControlIDX;
                    Application.DoEvents();
                    testExecutionStatus = -1;
                    switch (ParaToBeExecute)
                    {
                        case "VERIFYPRODUCTSTAGE":
                            string tavelerreadResponse = objccmdmethod.GetTravelerData_3Phase(TravelerStage, (int)StaticVariables.ExecutedMeterType.Smart_Meter_1PH);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = tavelerreadResponse;
                            if (VerifyProductStage(tavelerreadResponse)){ testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass; break;}
                            else 
                            {
                                DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Fail";
                                DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Style.BackColor = Color.Red;
                                testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail; 
                                return false; 
                            }
                           
                        case "VERIFYPCBAID":
                            string meterpcbaID = objccmdmethod.ReadPCBAID();
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
                            string fwVersionResponse = objccmdmethod.CheckingFirmwareVersion_SmartMeter(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = fwVersionResponse;
                            if (fwVersionResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass; break; }
                            else
                            {
                                DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Fail";
                                DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Style.BackColor = Color.Red;
                                testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                                return false;
                            }
                        case "VERIFYDLMSFWVERSION":
                            string dlmsfwVersionResponse = objccmdmethod.CheckingDLMSFirmwareVersion(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = dlmsfwVersionResponse;
                            if (dlmsfwVersionResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass; break; }
                            else
                            {
                                DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Fail";
                                DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Style.BackColor = Color.Red;
                                testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                                return false;
                            }
                        case "VERIFYMETERPASSWORD":
                            string readusPwdesponse = objccmdmethod.VerifyUSPassword(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = readusPwdesponse;
                            if (readusPwdesponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "WRITEPCBAID":
                            string writePCBAResponse = objccmdmethod.WritePCBAID(txtPCBAID.Text.Trim(), DLMSDataStracture.PCBAIDDataStracture.PCBAIDDataLength); 
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = writePCBAResponse;
                            if (writePCBAResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYCONFIGURATION":
                            string readconfigResponse = objccmdmethod.VerifyDisplayConfigData_1Phase(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = readconfigResponse;
                            if (readconfigResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYENERGY":
                            string readenergyResponse = objccmdmethod.VerifyEnergy(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = readenergyResponse;
                            if (readenergyResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "RELAYCIRCUITTEST":
                            string relayCircuitsTestResponse = objccmdmethod.RelayCircuitTest_1Phase(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = relayCircuitsTestResponse;
                            if (relayCircuitsTestResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "RELAYCIRCUITCONNECT":
                            string relayConnectResponse = objccmdmethod.RelayCircuitConnect();
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = relayConnectResponse;
                            if (relayConnectResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "RELAYCIRCUITDISCONNECT":
                            string relayDisConnectResponse = objccmdmethod.RelayCircuitDisConnect();
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = relayDisConnectResponse;
                            if (relayDisConnectResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "SETMETERRTC":
                            string SetRTCResponse = objccmdmethod.SetMeterRTCFalcon2SM();
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = SetRTCResponse;
                            if (SetRTCResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "PUSHBUTTONTEST":
                            frmLCDDisplayTest objlcttest = new frmLCDDisplayTest(StaticVariablesvar, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            objlcttest.ShowDialog();
                            Application.DoEvents();
                            string pushstusResponse = objccmdmethod.Read1PHMeterBuffer((int)StaticVariables.MeterBuffer1PHStatusFlg.PushButtonPressCounter, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = pushstusResponse + ",   " + StaticVariablesvar.LCDSegmentTestResponse;
                            if (pushstusResponse.IndexOf(StaticVariables.ERRORPreFix) < 0 && StaticVariablesvar.LCDSegmentTestResponse.IndexOf(StaticVariables.DisplayNotWorking) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "MAINBATTERYVOLTAGETEST":
                            string MainBatterystusResponse = objccmdmethod.Read1PHMeterBuffer((int)StaticVariables.MeterBuffer1PHStatusFlg.MainBatteryVoltage, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = MainBatterystusResponse;
                            if (MainBatterystusResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "RTCBATTERYVOLTAGETEST":
                            string RTCBatteryResponse = objccmdmethod.Read1PHMeterBuffer((int)StaticVariables.MeterBuffer1PHStatusFlg.RTCBatteryVoltage, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = RTCBatteryResponse;
                            if (RTCBatteryResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "METROLOGYTESTPHASE":
                            MessageBox.Show("Press Phase Current Switch And Hit OK To Continue.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Application.DoEvents();
                            string mmiResponse = objccmdmethod.MetrologyTest_1PhaseFalcon(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString(), (int)StaticVariables.MMITestParameters.PhaseCurrentTest);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = mmiResponse;
                            if (mmiResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "METROLOGYTESTNEUTRAL":
                            MessageBox.Show("Press Neutral Current Switch And Hit OK To Continue.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Application.DoEvents();
                            string mmiResponseNeu = objccmdmethod.MetrologyTest_1PhaseFalcon(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString(), (int)StaticVariables.MMITestParameters.NeutralCurrentTest);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = mmiResponseNeu;
                            if (mmiResponseNeu.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "RELAYMALFUNCTIONTEST"://---Not Supported in Falcon 2
                            string RelaymalfunstusResponse = objccmdmethod.Read1PHMeterBuffer((int)StaticVariables.MeterBuffer1PHStatusFlg.RelayMalFunctionStatus, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = RelaymalfunstusResponse;
                            if (RelaymalfunstusResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "COMMUNICATIONTESTONBATTERY":
                            objLI.AssociationDisconnect();
                            MessageBox.Show("Power-on Meter on Battery Mode And Hit OK To Continue.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Application.DoEvents();
                            Thread.Sleep(100);
                            if (!objLI.ConnectToMeter()) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail; DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = StaticVariables.ERRORPreFix + "COMM Failed."; break; }
                            Application.DoEvents();
                            string getBatteryModeVoltage = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.ReadVoltageDataStracture_1Phase.ReadVoltageOBIS, DLMSDataStracture.ReadVoltageDataStracture_1Phase.ReadVoltageClassID, 0, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = "Voltage =" + getBatteryModeVoltage;
                            if (getBatteryModeVoltage.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            objLI.AssociationDisconnect();
                            Thread.Sleep(100);
                            MessageBox.Show("Power-on Meter on Main Supply Mode And Hit OK To Continue.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Thread.Sleep(1500);
                            if (!objLI.ConnectToMeter()) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail; DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = StaticVariables.ERRORPreFix + "COMM Failed."; break; }
                            Application.DoEvents();
                            break;
                        case "COMMUNICATIONTESTONBATTERYWOMAINS":
                            //Application.DoEvents();
                            //objLI.AssociationDisconnect();
                            //Application.DoEvents();
                            //Thread.Sleep(100);
                            //if (!objLI.ConnectToMeter()) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail; DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = StaticVariables.ERRORPreFix + "COMM Failed."; break; }
                            Application.DoEvents();
                            string getvoltageonBattery = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.ReadVoltageDataStracture_1Phase.ReadVoltageOBIS, DLMSDataStracture.ReadVoltageDataStracture_1Phase.ReadVoltageClassID, 0, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = "Voltage =" + getvoltageonBattery;
                            if (getvoltageonBattery.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break; 
                        case "COMMUNICATIONTEST":
                            string CommtestResponse = objccmdmethod.CommunicationTest(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = CommtestResponse;
                            if (CommtestResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            Thread.Sleep(500);
                            break;
                        case "CASETAMPERSWITCHTEST":
                            string casetampertstusResponse = objccmdmethod.Read1PHMeterBuffer((int)StaticVariables.MeterBuffer1PHStatusFlg.CaseTamperCount, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = casetampertstusResponse;
                            if (casetampertstusResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "ESDTAMPERTEST":
                            string[] ReferenceValue = new string[] { "120321", "120322" }; /*--ESD Event Code 801 & 802--*/
                            string esdTamperResponse = objccmdmethod.ReadTamperCompartment(DLMSDataStracture.ReadoutDataStracture.TamperReadoutOBIS_Compartment5, ReferenceValue, "ESD");//--Tamper - Others Compartment i.e. 5
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = esdTamperResponse;
                            if (esdTamperResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "ACMAGNETTEST":
                            string ACMagnetstusResponse = objccmdmethod.Read1PHMeterBuffer((int)StaticVariables.MeterBuffer1PHStatusFlg.ACMagnetFieldCount, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = ACMagnetstusResponse;
                            if (ACMagnetstusResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;                        
                        case "RTCDRIFTTEST":
                            string rtcDriftResponse = objccmdmethod.TestRTCDrift(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = rtcDriftResponse;
                            if (rtcDriftResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYTEMPERATURE":
                            string tempratureResponse = objccmdmethod.VerifyTemprature(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = tempratureResponse;
                            if (tempratureResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;     
                        case "VERIFYCALIBRATIONDATA":
                            string caliRespResponse = objccmdmethod.TestCalibrationData();
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = caliRespResponse;
                            if (caliRespResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "CASETAMPERTEST":
                            string caseResponse = objccmdmethod.CheckingCaseTamper();
                            if (caseResponse.IndexOf(StaticVariables.ERRORPreFix) < 0)
                            {
                                if (caseResponse.Substring(caseResponse.IndexOf("=") + 1).Trim() == "0") testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                                else caseResponse = "Case Detected =" + caseResponse;
                            }
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = caseResponse;
                            break;
                        case "RESETBILLINGDATA":
                            string resetBillingResponse = objccmdmethod.ResetBilling(objappSettings.GetMeterMode());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = resetBillingResponse;
                            if (resetBillingResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "RESETALLDATA":
                            string resetallResponse = objccmdmethod.ResetAllData(objappSettings.GetMeterMode());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = resetallResponse;
                            if (resetallResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            Thread.Sleep(1000);
                            break;
                        case "RELAYCONFIGURATION":
                            string relayConfigResponse = objccmdmethod.SetLoadControlDefaultConfigurations(objappSettings.GetMeterMode());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = relayConfigResponse;
                            if (relayConfigResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "READPCBAID":
                            string pcbaResponse = objccmdmethod.ReadPCBAID();                           
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = pcbaResponse.Trim();
                            if (pcbaResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) { txtMeterPCBAID.Text = pcbaResponse; testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass; }
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;  
                            break;
                        case "WRITEMETERID":
                            string writemIDResponse = objccmdmethod.WrittingMeterID_SmartMeter(txtPCBAID.Text.Trim());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = writemIDResponse.Trim();
                            if (writemIDResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "WRITEMANUFACTURINGYEAR":
                            string mfgResponse = objccmdmethod.WrittingManufacturingYear(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = mfgResponse.Trim();
                            if (mfgResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "WRITECURRENTRATING":
                            string mfgdataWritrResponse = objccmdmethod.WriteCuttentRating(DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString(),DLMSDataStracture.CurrentRatingDataStracture.CurrentRatingDataLength);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = mfgdataWritrResponse;
                            if (mfgdataWritrResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYMANUFACTURINGYEAR":
                            string readmfgResponse = objccmdmethod.VerifyManufacturingYear(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = readmfgResponse.Trim();
                            if (readmfgResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYMETERID":
                            string readmidResponse = objccmdmethod.VerifyMeterID(txtPCBAID.Text.Trim());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = readmidResponse.Trim();
                            if (readmidResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "RESCANMETERID":
                            inputBox objinputform = new inputBox("Scan Meter ID");
                            objinputform.ShowDialog();
                            string rescanmidResponse = objccmdmethod.VerifyMeterID(objinputform.txtInputBox.Text.Trim());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = rescanmidResponse.Trim() + ", Scanned =" + objinputform.txtInputBox.Text.Trim();
                            if (rescanmidResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYRS485DEVICEADDRESS":
                            string readrs485Response = objccmdmethod.VerifyRS485Address(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = readrs485Response.Trim();
                            if (readrs485Response.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "LOCKFSMODE":
                            string fsmodeLockResponse = objccmdmethod.FSModeCommunicationLocking(0x01);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = fsmodeLockResponse;
                            if (fsmodeLockResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "UNLOCKFSMODE":
                            string fsmodeUnLockResponse = objccmdmethod.FSModeCommunicationLocking(0x00);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = fsmodeUnLockResponse;
                            if (fsmodeUnLockResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "SETDEFAULTPASSWORD":
                            string defPwdesponse = objccmdmethod.WrittingDefaultMeterPassword_SmartMeter(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());//--Reference parameters only needed if meter default passwords modified.
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = defPwdesponse;
                            if (defPwdesponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "LOCKINGMETER":
                            if (objccmdmethod.GetExecutionStatus(DGVParaLists.Rows.Count - 1, DGVParaLists))
                            {
                                string lockResponse = objccmdmethod.LockingMeter(0xFF);
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
                        if (DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value.ToString().IndexOf("COMM Failed") >= 0) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.ComFail; isComError = true; }
                        if (testExecutionStatus == (int)StaticVariables.ExecutionReurnStatus.Fail)
                        {
                            DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Fail"; DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Style.BackColor = Color.Red;
                            if (logedUserTypeIndex == (int)StaticVariables.userCategory.UserValidation) { selectedControlIDX++; continue; } //----Continue for validation only

                        }
                        else if (testExecutionStatus == (int)StaticVariables.ExecutionReurnStatus.ComFail) { DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Fail"; DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Style.BackColor = Color.Red; }
                        else { DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Not Executed"; DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Style.BackColor = Color.Yellow; }
                        selectedControlIDX++;//--If break here then need to increase index for traveller write
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
                    if (!isComError && !ExecutionWithOutTravellerStage)
                    {
                        if (!objccmdmethod.SetTravelerData_SmartMeter1Phase(selectedControlIDX, TravelerStage, DGVParaLists)) { MessageBox.Show("Unable To Set Production Stage !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
                    }
                    //return (ExportResultsByBatch());
                    if (ExportResultsByBatch())
                        return true;
                    else if (executionsts && (new ExcecutionResultImportExport().SaveExecutionData(DGVParaLists, executionResultClone, objexeresult)))
                    {
                        objLI.DisplayStatusMsg("Saved Results to File!", true);
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
                objLI.AssociationDisconnect();
                this.Cursor = Cursors.Default;                
            }

        }

        private bool VerifyProductStage(string tavelerreadResponse)
        {
            if (TravelerStage > 0)
            {                
                if (tavelerreadResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0)
                { 
                    MessageBox.Show("Product are At Invalid Production Stage, Return To Previous Production Stage !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }                 
            }
            return true;
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
                    objLI.DisplayStatusMsg("Unable To Save Results !", true);
                    MessageBox.Show("Unable To Save Results !" + "\n" + resultmsg, "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                DataSet ds = objexere.Select_GetExecutionResult_onPCBAID_ProType_ExeDate(objexeresult);
                objLI.DisplayStatusMsg("Test Executed !", false);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["FinalStatus"].ToString().ToUpperInvariant() != "PASS") return false;
                    
                }
                else
                {
                    objLI.DisplayStatusMsg("Scan Meter Record Not Found in Database !", true);
                    MessageBox.Show("Scan Meter Record Not Found in Database !, Re-Scan and Try Again !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
               
                return true;
            }
            catch (Exception Ex)
            {
                resultmsg = Ex.Message;
                objLI.DisplayStatusMsg("Unable To Save Results !", true);
                MessageBox.Show("Unable To Save Results !" + "\n" + Ex.Message, "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            finally
            {
                if (resultmsg != "") CommonMethods.LogPMPMessage("Unable To Save Results : " + resultmsg);

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
                dlmsCommStatusmsh.Width = (DLMSStas.Width / 2) + 50;
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
                else if (ClientSAP == "40" || ClientSAP == "7E")
                    dlmscommmode += " FS ";

                dlmscommmode += ",  " + SerialPort;
                //--------------------------------------------------------------------


                string Communication = string.Empty;
                string Company_Profile = string.Empty;
                string filename = AppDomain.CurrentDomain.BaseDirectory + @"\SMPMP.exe";
                string strbuilton = "Built On: " + File.GetCreationTime(filename).ToShortDateString().ToString();
                bool Flg_ComSetting = true; 
                if (Flg_ComSetting)
                {
                    Communication = dlmscommmode;

                }
                string Prduct_Version = SystemInfo.ProductVersion();
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
               int rowcnt=0;
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
              else { txtPCBAID.MaxLength = 13; lblParaonTestType.Text = "Scan PCBA ID"; ProgrammLabelMsg = "PCBA"; lblLastScan.Text = "Last Scan PCB ID"; }
            }
        }       

        private bool InitMeter()
        {
            try
            {
                bool isCheckFlash=true;
                int waitTime=4;
                byte[] InitCmd = new byte[] { 0x7E, 0xAA, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x09, 0x7E };
                if (isCheckFlash) InitCmd = new byte[] { 0x7E, 0xAA, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x7E };
                objLI.DisplayStatusMsg("Meter Initializing ...", false);
                Application.DoEvents();
                if (!objLI.PhysicalLayerConnect()) return false;
                GlobalObjects.objSerialComm.fSendDataToPort(InitCmd, InitCmd.Length);
                while (waitTime-- > 0)
                {
                    if (GlobalObjects.objSerialComm.ReceiveBuffer[0] != 0x00) break;
                    objLI.DisplayStatusMsg("Meter Initializing Wait :" + waitTime.ToString() + " Sec.", false);
                    Application.DoEvents();
                    Thread.Sleep(1000);
                    Application.DoEvents();
                }                
                if (GlobalObjects.objSerialComm.ReceiveBuffer.Length >= 1)
                {
                    byte ststusflg = GlobalObjects.objSerialComm.ReceiveBuffer[0];
                    string bitsting = Convert.ToString(ststusflg, 2).PadLeft(8, '0');
                    string FailMsg="";

                    string EEPROMTEST = bitsting.Substring(6, 2);
                    if (EEPROMTEST == "00") FailMsg = "EEPROM Not Tested";
                    else if (EEPROMTEST == "01") FailMsg = "EEPROM Erase Failed";
                    else if (EEPROMTEST == "10") FailMsg = "EEPROM Verification Failed";

                    if (isCheckFlash) if (bitsting.Substring(5, 1) == "0") FailMsg = FailMsg + "\n" + "Flash Read Write Status Failed";
                    if (FailMsg.Length > 0)
                    {
                        objLI.DisplayStatusMsg("Meter Initializing Test Fail!", true);
                        MessageBox.Show("Meter Initializing Test Fail!" + "\n" + FailMsg, "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception Ex)
            {
             
                MessageBox.Show(Ex.ToString());
                return false;
            }
            finally
            {
                GlobalObjects.objSerialComm.OpenPort();
            }
        }

        private string SRPFTest(string Defaultpara)
        {
            string MeterReadVal =string.Empty;
            try
            {         
        
                byte[] InitCmd = new byte[] { 0x01, 0x52, 0x31, 0x02, 0x45, 0x30, 0x30, 0x38, 0x28, 0x30, 0x36, 0x29, 0x03, 0x15 };
                objLI.DisplayStatusMsg("Checking SR/PF ...", false);
                Application.DoEvents();
                //----------------Set Port----------------------
                string[] comlist = Defaultpara.Split(',');
                if (comlist.Length < 11 ) return StaticVariables.ERRORPreFix + "Invalid Argument Passed !";
                string portName = comlist[0];                  
                //----------------------------------------------

                if (!objIECLI.ConnectToRefMeter(portName)) return StaticVariables.ERRORPreFix + "COM Error";
                string[] Command =new string[4];// "01523102" + objIECLI.GetStrToHexCmd("E008(06)") + "0315";
                Command[0] = "01 52 31 02 45 30 30 36 28 30 32 29 03 12"; //.R1.E006(02)..    "\x03"
                Command[1] = "01 57 31 02 45 30 30 41 28 30 31 29 03 63";// .W1.E00A(01).c    "\x06"
                Command[2] = "01 52 31 02 45 30 30 38 28 30 36 29 03 15";// .R1.E008(06)..    "\x03"
                Command[3] = "01 52 31 02 45 30 30 37 28 30 36 29 03 15";// .R1.E007(06)..    "\x03"
                List<string> responseList = new List<string>();
                int cmdCnt = 0;
                string cmdResponse = string.Empty;
                while (cmdCnt < Command.Length)
                {                    
                    if (cmdCnt != 1) cmdResponse = objIECLI.WriteDataToMeter(Command[cmdCnt].Replace(" ", ""), "\x29");
                    else cmdResponse = objIECLI.WriteDataToMeter(Command[cmdCnt].Replace(" ", ""), "\x06");
                    if (cmdResponse == "") return "";
                    responseList.Add(cmdResponse);
                    cmdCnt++;
                }

                if (responseList.Count > 0)
                {
                    bool srpfTest = true;
                    decimal RefVoltageMin = Convert.ToDecimal(comlist[1]);
                    decimal RefVoltageMax = Convert.ToDecimal(comlist[2]);

                    string DVCCVoltage =objccmdmethod.GetMeterResponseValuePart(responseList[0]);
                    string DVCCVoltageValuu = DLMSDataStracture.HexToDecimalConversion(DVCCVoltage);
                    decimal calvoltage = Convert.ToDecimal(DVCCVoltageValuu) / 275M; //---1.8  >> 1.5 to 1.9 
                    if (calvoltage < RefVoltageMin || calvoltage > RefVoltageMax) srpfTest = false;

                    decimal RefmalfunctionMin = Convert.ToDecimal(comlist[3]);
                    decimal RefmalfunctionMax = Convert.ToDecimal(comlist[4]);
                    string Eggstruct = objccmdmethod.GetMeterResponseValuePart(responseList[2]);                          //2byte:malfunction,2byte:powerfail to rf, 2byte:tamper req.
                    string malfunctionValue = DLMSDataStracture.HexToDecimalConversion(Eggstruct.Substring(0, 4)); //== 8-12
                    if (Convert.ToInt32(malfunctionValue) < RefmalfunctionMin || Convert.ToInt32(malfunctionValue) > RefmalfunctionMax) srpfTest = false;

                    decimal RefpowerfailMin = Convert.ToDecimal(comlist[5]);
                    decimal RefpowerfailMax = Convert.ToDecimal(comlist[6]);
                    string powerfail = DLMSDataStracture.HexToDecimalConversion(Eggstruct.Substring(4, 4));  // ==40 - 60
                    if (Convert.ToInt32(powerfail) < RefpowerfailMin || Convert.ToInt32(powerfail) > RefpowerfailMax) srpfTest = false;

                    decimal RefserviceReqMin = Convert.ToDecimal(comlist[7]);
                    decimal RefserviceReqMax = Convert.ToDecimal(comlist[8]);
                    string serviceReq = DLMSDataStracture.HexToDecimalConversion(Eggstruct.Substring(8, 4)); // ==40 - 60
                    if (Convert.ToInt32(serviceReq) < RefserviceReqMin || Convert.ToInt32(serviceReq) > RefserviceReqMax) srpfTest = false;

                    decimal RefFrequencyMin = Convert.ToDecimal(comlist[9]);
                    decimal RefFrequencyMax = Convert.ToDecimal(comlist[10]);
                    string EggFrequency  = objccmdmethod.GetMeterResponseValuePart(responseList[3]); //1st 8 char * 0x10000 + 2nd 8 char
                    string EggFrequencyValue1st = DLMSDataStracture.HexToDecimalConversion(EggFrequency.Substring(0, 8));
                    string EggFrequencyValue2nd = DLMSDataStracture.HexToDecimalConversion(EggFrequency.Substring(8, 8));
                    long frqVal = (Convert.ToInt64(EggFrequencyValue1st) * 0x10000) + Convert.ToInt64(EggFrequencyValue2nd);
                    if (frqVal < RefFrequencyMin || frqVal > RefFrequencyMax) srpfTest = false;

                     MeterReadVal = "DVCC Voltage =" + calvoltage.ToString("0.00") + ", Malfunction Count =" + malfunctionValue + ", PF Count =" + powerfail + ", SR Count =" + serviceReq + ", 1hz RTC pulse value =" + frqVal.ToString();
                    if(!srpfTest) MeterReadVal = StaticVariables.ERRORPreFix + MeterReadVal;
                }
                return MeterReadVal;
            }
            catch (Exception Ex)
            {

                 return StaticVariables.ERRORPreFix + Ex.ToString();
                 
            }
            finally
            {
                objIECLI.AssociationDisconnect();
                
            }
        }
      
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
        private void txtMeterID_KeyUp(object sender, KeyEventArgs e)
        {
            int maxlen = 13;
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
                if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Space)
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
