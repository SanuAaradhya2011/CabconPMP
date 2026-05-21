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
namespace PMPE150MICROSTARDLMS
{
    public partial class frmActionMicroStarDLMS : Form
    { 
        int test_index = 0;
        // public static SerialComm objSerialComm = new SerialComm();
        SystemSettingsAccess objsysSetting = new SystemSettingsAccess();
         BALExecutionResults objexere = new BALExecutionResults();
        EntityExecutionResult objexeresult = new EntityExecutionResult();
        LayerInterface objLI = new LayerInterface();
        IECLayerInterface objIECLI = new IECLayerInterface();        
        AppSettings objappSettings = new AppSettings();
        MyCrypro objcrypt = new MyCrypro();
        CommonCommandMethods objccmdmethod = new CommonCommandMethods();
        public delegate void UpdateMainMsgHandler(object sender, UpdateEventArgs e);
        //TextBox[] txtboxobject = new TextBox[] { };
        List<string> paraNameList ;
        //GlobalMethods StaticVariablesvar = new GlobalMethods();
        //enum ExecutionReurnStatus { Pass = 0, Fail = 1, ComFail=2 };       
        string TestProcedureName = string.Empty;
        string TestProcedureID = string.Empty;
        string[] IPParalist;
        int TravelerStage = 0;
        string testtype = string.Empty;
        bool isProcedureExecuted = false;
        DataTable proceduredt = new DataTable();
        string testCategory = string.Empty;
        string ProgrammLabelMsg = "PCBA";
        string travelerbyte = string.Empty;
        string logedUserID = string.Empty;
        DataTable executionResultClone;
        int logedUserTypeIndex;
        bool ExecutionWithManualScan = true;
        bool ExecutionWithOutTravellerStage = false;
        public frmActionMicroStarDLMS(string[] ipPara)
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
                objsysSetting.IS15959PART1Amendment5Clock = true;
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
                if (finalExecutionStatus) pbExecutionStatus.Image = MICROSTARDLMS.Properties.Resources.ExecutionPass;
                else pbExecutionStatus.Image = MICROSTARDLMS.Properties.Resources.ExecutionFail;

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
                pbExecutionStatus.Image = MICROSTARDLMS.Properties.Resources.ExecutionWait;
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
                objLI.DisplayStatusMsg("", false);
                txtMeterPCBAID.Text = "";               
                
                objappSettings.SetMeterMode((int)LayerInterface.MeterTypeInfo.MicroStar_DLMS);
                if (!objLI.ConnectToMeter()) return false;
                
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
                            string tavelerreadResponse = objccmdmethod.VerifyTraveler_VIMDLMS(TravelerStage);// objIECccmdmethod.VerifyTraveler_VIMNONDLMS(TravelerStage);
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
                            string fwVersionResponse = objccmdmethod.CheckingFirmwareVersion_MicroStarDLMS(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
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
                            string writePCBAResponse = objccmdmethod.WritePCBAID(txtPCBAID.Text.Trim(), DLMSDataStracture.PCBAIDDataStracture.PCBAIDDataLength_E150DLMS); 
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
                        case "VERIFYMAGNETCOUNTS":
                            string magTamperCountResponse = "";
                            string defaultVal = DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString();
                            string refminVal = DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString();
                            string refmaxVal = DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString();
                            if (defaultVal.Trim().Length <= 0 && refminVal.Trim().Length <= 0 && refmaxVal.Trim().Length <= 0)
                            {
                                //if (objLI.MeterInfoValue.Contains("VB"))
                                if (objccmdmethod.ISSinglePhaseDLMS64KMeterVariants(objLI.MeterInfoValue)) magTamperCountResponse = objccmdmethod.CheckingMagnetTamperStatus64K();//------For VIM 64K Specefic
                                else magTamperCountResponse = objccmdmethod.CheckingMagnetTamperStatus();//------For VIM Specefic
                            }
                            else magTamperCountResponse = objccmdmethod.CheckingMagnetTamperStatus(defaultVal, refminVal, refmaxVal);//-----For MicroStar Specefic
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = magTamperCountResponse;
                            if (magTamperCountResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYCTWIRE":
                            string readshuntwireResponse="";
                            //------------------In case of 64K Bescom Specefic----------------------------
                            //if (objLI.MeterInfoValue.Contains("VB"))
                            if (objccmdmethod.ISSinglePhaseDLMS64KMeterVariants(objLI.MeterInfoValue))
                            {
                                //----Message commented as Neutral current ON continiously in production Jig (confirmed by Production on 31st Mar'21 by mail) so no need to popup message
                                //MessageBox.Show("Press Neutral Current Switch And Hit OK To Continue.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //Application.DoEvents();
                                readshuntwireResponse = objccmdmethod.VerifyCT_ShuntWire64KSpecefic(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            }
                            //------------------In case of 1P DLMS Generic along with 128K variants ----------------------------
                            else readshuntwireResponse = objccmdmethod.VerifyCT_ShuntWire(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = readshuntwireResponse.Trim();
                            if (readshuntwireResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYPHASECURRENT":
                            //MessageBox.Show("Press Phase Current Switch And Hit OK To Continue.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Application.DoEvents();
                            string phaseCurrentResponse = objccmdmethod.VerifyMeterCurrentTest_1Phase(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString(), (int)StaticVariables.MeterCurrentType.PhaseCurrent);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = phaseCurrentResponse;
                            if (phaseCurrentResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYNEUTRALCURRENT":
                            //MessageBox.Show("Press Neutral Current Switch And Hit OK To Continue.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Application.DoEvents();
                            string neuCurrentResponse = objccmdmethod.VerifyMeterCurrentTest_1Phase(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString(), (int)StaticVariables.MeterCurrentType.NeutralCurrent);
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
                        case "VERIFYBATTERYSTATUS":
                            string readbatflagResponse = objccmdmethod.VerifyBatteryUsingFlag();
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = readbatflagResponse.Trim();
                            if (readbatflagResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYSINGLEWIRECOUNTLIMITS":
                            string readCountResponse = objccmdmethod.GetSingleWireCountLimits(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = readCountResponse.Trim();
                            if (readCountResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "SETMETERRTC":
                            string SetRTCResponse = objccmdmethod.SetMeterRTC(true, 0x00, 0);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = SetRTCResponse;
                            if (SetRTCResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;

                        case "SETMETERRTCAHEAD30MINUTES":
                            string SetRTCResponseBhutan = objccmdmethod.SetMeterRTC(true, 0x00, 1800);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = SetRTCResponseBhutan;
                            if (SetRTCResponseBhutan.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;  
                            //---------------Main Battery & RTC Battery Voltage Not Used in 1P DLMS Non-Smart Meters-----------------------------
                        //case "MAINBATTERYVOLTAGETEST":
                        //    string MainBatterystusResponse = objccmdmethod.Read1PHMeterBuffer((int)StaticVariables.MeterBuffer1PHStatusFlg.MainBatteryVoltage, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                        //    DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = MainBatterystusResponse;
                        //    if (MainBatterystusResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                        //    else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                        //    break;
                        //case "RTCBATTERYVOLTAGETEST":                            
                        //    string RTCBatteryResponse = objccmdmethod.Read1PHMeterBuffer((int)StaticVariables.MeterBuffer1PHStatusFlg.RTCBatteryVoltage, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                        //    DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = RTCBatteryResponse;
                        //    if (RTCBatteryResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                        //    else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;                            
                        //    break;
                        case "METROLOGYTESTPHASE":
                            MessageBox.Show("Press Phase Current Switch And Hit OK To Continue.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Application.DoEvents();
                            string mmiResponse = objccmdmethod.MMITEST(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString(), (int)StaticVariables.MMITestParameters.PhaseCurrentTest);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = mmiResponse;
                            if (mmiResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "METROLOGYTESTNEUTRAL":
                            MessageBox.Show("Press Neutral Current Switch And Hit OK To Continue.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Application.DoEvents();
                            string mmiResponseNeu = objccmdmethod.MMITEST(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString(), (int)StaticVariables.MMITestParameters.NeutralCurrentTest);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = mmiResponseNeu;
                            if (mmiResponseNeu.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                      
                        case "COMMUNICATIONTESTONBATTERY":
                            objLI.AssociationDisconnect();
                            MessageBox.Show("Power-on Meter on Battery Mode And Hit OK To Continue.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Application.DoEvents();
                            Thread.Sleep(100);
                            if (!objLI.ConnectToMeter()) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail; DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = StaticVariables.ERRORPreFix + "COMM Failed."; break; }
                            Application.DoEvents();
                            string getBatteryModeVoltage = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.ReadVoltageDataStracture_1Phase.ReadVoltageOBIS, DLMSDataStracture.ReadVoltageDataStracture_1Phase.ReadVoltageClassID, 0, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), "0"); //Volatge on Battery Must be <=0
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = "Voltage " + getBatteryModeVoltage;
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
                            Application.DoEvents();
                            string getBatteryModeVoltageWOMains = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.ReadVoltageDataStracture_1Phase.ReadVoltageOBIS, DLMSDataStracture.ReadVoltageDataStracture_1Phase.ReadVoltageClassID, 0, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), "0"); //Volatge on Battery Must be <=0
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = "Voltage " + getBatteryModeVoltageWOMains;
                            if (getBatteryModeVoltageWOMains.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;  
                        case "COMMUNICATIONTEST":
                            string CommtestResponse = objccmdmethod.CommunicationTest(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = CommtestResponse;
                            if (CommtestResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;                       
                        //case "ACMAGNETTEST": //---Not Used in Production
                        //    string ACMagnetstusResponse = objccmdmethod.Read1PHMeterBuffer((int)StaticVariables.MeterBuffer1PHStatusFlg.ACMagnetFieldCount, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                        //    DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = ACMagnetstusResponse;
                        //    if (ACMagnetstusResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                        //    else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                        //    break;                        
                        case "RTCDRIFTTEST":
                            string rtcDriftResponse = objccmdmethod.TestRTCDrift(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = rtcDriftResponse;
                            if (rtcDriftResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "RTCDRIFTTESTAHEAD30MINUTES":
                            string rtcDriftResponseBhutan = objccmdmethod.TestRTCDrift(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString(), 1800);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = rtcDriftResponseBhutan;
                            if (rtcDriftResponseBhutan.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYCALIBRATIONDATA":
                            string caliRespResponse = objccmdmethod.TestCalibrationData_MicroStarDLMS(objLI.MeterInfoValue);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = caliRespResponse;
                            if (caliRespResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "CASETAMPERTEST":
                            string caseResponse = "";
                            //if (objLI.MeterInfoValue.ToUpperInvariant().Contains("VB")) { }
                            if (objccmdmethod.ISSinglePhaseDLMS64KMeterVariants(objLI.MeterInfoValue)) caseResponse = objccmdmethod.CheckingCaseTamper_MicrostarDLMS64K(DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString());
                            else caseResponse = objccmdmethod.CheckingCaseTamper_MicrostarDLMS(DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = caseResponse.Trim();
                            if (caseResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;                           
                            break;
                        case "VERIFYCASETAMPER":
                            string verifycaseResponse ="";
                            //if (objLI.MeterInfoValue.Contains("VB"))
                            if (objccmdmethod.ISSinglePhaseDLMS64KMeterVariants(objLI.MeterInfoValue)) verifycaseResponse = objccmdmethod.ReadCaseTamperCounter_MicrostarDLMS64K(DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString());
                            else verifycaseResponse = objccmdmethod.ReadCaseTamperCounter_MicrostarDLMS();
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = verifycaseResponse.Trim();
                            if (verifycaseResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
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
                        case "RESETLOWBATT":
                            string lowBatResponse = objccmdmethod.ResetLowBatt();
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = lowBatResponse;
                            if (lowBatResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "READPCBAID":
                            string pcbaResponse = objccmdmethod.ReadPCBAID();                            
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = pcbaResponse.Trim();
                            if (pcbaResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) { txtMeterPCBAID.Text = pcbaResponse; testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass; }
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;  
                            break;
                        case "WRITEMETERID":
                            string writemIDResponse = objccmdmethod.WrittingMeterID(txtPCBAID.Text.Trim());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = writemIDResponse.Trim();
                            if (writemIDResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "WRITEREFVOLTAGE":
                            string voltageResponse = objccmdmethod.WriteRefVoltage(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = voltageResponse.Trim();
                            if (voltageResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "WRITEMANUFACTURINGYEAR":
                            string mfgResponse = objccmdmethod.WrittingManufacturingYear(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = mfgResponse.Trim();
                            if (mfgResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "WRITECURRENTRATING":
                            string mfgdataWritrResponse = objccmdmethod.WriteCuttentRating(DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString(), DLMSDataStracture.CurrentRatingDataStracture.CurrentRatingDataLength);
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
                        case "WRITEMANUFACTURINGMONTH":
                            string mfgMonthResponse = objccmdmethod.WriteManufacturingMonth(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = mfgMonthResponse.Trim();
                            if (mfgMonthResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYMANUFACTURINGMONTH":
                            string readmfgMonthResponse = objccmdmethod.VerifyManufacturingMonth(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = readmfgMonthResponse.Trim();
                            if (readmfgMonthResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "WRITEPMAX":
                            string pmaxResponse = objccmdmethod.WritePmaxValue(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = pmaxResponse.Trim();
                            if (pmaxResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
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
                    if (!isComError && !ExecutionWithOutTravellerStage)
                    {
                        if (objexeresult.FinalResult == "PASS")
                        {
                            if (!objccmdmethod.SetTravelerData_VIMDLMS(TravelerStage)) { MessageBox.Show("Unable To Set Production Stage Flag, Meter May at Invalid Production Stage !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
                        }
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
                dlmsCommStatusmsh.Width = (DLMSStas.Width / 2);
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
              else { txtPCBAID.MaxLength = 10; lblParaonTestType.Text = "Scan PCBA ID"; ProgrammLabelMsg = "PCBA"; lblLastScan.Text = "Last Scan PCBA ID"; }
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

        private string PushButtonTest(string defaultVal, string minVal, string maxVal)
        {
            try
            {
                int pushCount;
                int responsepushCount = 0; ;
                if (!int.TryParse(defaultVal, out pushCount))
                {
                    if (!int.TryParse(minVal, out pushCount))
                    {
                        if (!int.TryParse(maxVal, out pushCount)) { return StaticVariables.ERRORPreFix + "Input Parameters Can't be Blank !"; }
                    }
                }
                //-------------------Get Default flag and then push buttun and deduct default for calculation--------------------
                //---------Deduction case not working so that commented
                ////string pushButtonCountResponse = objccmdmethod.PushButtonTest_1Phase(defaultVal, minVal, maxVal, -1);
                ////if (!int.TryParse(pushButtonCountResponse, out responsepushCount)) return StaticVariables.ERRORPreFix + pushButtonCountResponse;
                MessageBox.Show("Press Push Button Switch " + pushCount.ToString() + " Time And Hit OK To Continue.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.DoEvents();
               // if (objLI.MeterInfoValue.Contains("VB"))
                if (objccmdmethod.ISSinglePhaseDLMS64KMeterVariants(objLI.MeterInfoValue)) return objccmdmethod.PushButtonTest_1Phase64KVIM(defaultVal, minVal, maxVal, responsepushCount);
                else return objccmdmethod.PushButtonTest_1Phase(defaultVal, minVal, maxVal, responsepushCount);
            }
            catch (Exception Ex)
            {
                return StaticVariables.ERRORPreFix + Ex.ToString();
            }
        }

        private void FrmActionMicroStarDLMS_FormClosing(object sender, FormClosingEventArgs e)
        {
            objsysSetting.IS15959PART1Amendment5Clock = false; //restoring global setting to default
        }
    }
}
