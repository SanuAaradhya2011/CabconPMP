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
namespace PMPSAPPHIRE
{
    public partial class frmActionSAPPHIRE : Form
    {
        int test_index = 0;
        //public static SerialComm objSerialComm = new SerialComm();
        BALExecutionResults objexere = new BALExecutionResults();
        EntityExecutionResult objexeresult = new EntityExecutionResult();
        LayerInterface objLI = new LayerInterface();
        //IECLayerInterface objIECLI = new IECLayerInterface();
        AppSettings objappSettings = new AppSettings();
        MyCrypro objcrypt = new MyCrypro();
        CommonCommandMethods objccmdmethod = new CommonCommandMethods();
        public delegate void UpdateMainMsgHandler(object sender, UpdateEventArgs e);
        //TextBox[] txtboxobject = new TextBox[] { };
        List<string> paraNameList;
        GlobalMethods StaticVariablesvar = new GlobalMethods();
        //enum ExecutionReurnStatus { Pass = 0, Fail = 1, ComFail = 2 };
       // enum MeterStatusFlg { MemoryandRelayStatus = 0, MainBatteryVoltage = 1, RTCBatteryVoltage = 2, PushButtonPressCounter = 3, CaseTamperCount = 4, ACMagnetFieldCount = 5 };
        enum MeterParaRead { Voltage_RPhase = 0, Voltage_YPhase = 1, Voltage_BPhase = 2, Current_RPhase = 3, Current_YPhase = 4, Current_BPhase = 5, PF_RPhase = 6, PF_YPhase = 7, PF_BPhase = 8, NeutralCurrent = 9, CTDirection = 10,ReadDefault=255 };
        string ProgrammLabelMsg = "PCBA";
        int TravelerStage = 0;
        string TestProcedureName = string.Empty;
        string TestProcedureID = string.Empty;
        string[] IPParalist;
        string testtype = string.Empty;
        bool isProcedureExecuted = false;
        DataTable proceduredt = new DataTable();
        string testCategory = string.Empty;
        string logedUserID = string.Empty;
        DataTable executionResultClone;
        bool IsFrontTested = false;
        int logedUserTypeIndex;
        bool ExecutionWithManualScan = true;
        bool ExecutionWithOutTravellerStage = false;
        public frmActionSAPPHIRE(string[] ipPara)
        {
            InitializeComponent(); COMMONENTITY.FormStyleHelper.Apply(this);
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
                    MessageBox.Show("Invalid Test Type !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
                txtTestType.Text = testCategory;
                TravelerStage = objccmdmethod.SetTravelerStage(testCategory);
                if (TravelerStage < 0 && testCategory != "Other Test")
                {

                    MessageBox.Show("Invalid Test Type !, Error: Setting Product Stage.", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
                objccmdmethod.SetDefaultSettings(IPParalist);
                DisplayMainStatus();
                txtPCBAID.Text = "";
                txtPCBAID.Focus();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Unable To Run Test !" + "\n" + Ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                grpinputs.Enabled = false;
                DataSet ds = new DataSet();
                this.Cursor = Cursors.WaitCursor;
                if (!ProgrammLabelMsg.ToUpperInvariant().Contains("PCBA") && txtPCBAID.Text.Trim().Length <= 0)
                {
                    MessageBox.Show("Please Scan Valid " + ProgrammLabelMsg + " ID !, ID Should Not Blank!", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                objLI.UpdatedLed += new LayerInterface.UpdateHandler(AddressForm_PingLed);

                if (DGVParaLists.Rows.Count <= 0)
                {
                    MessageBox.Show("No Active Procedure to Execute Test!", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                objexeresult.PCBAID = txtPCBAID.Text.Trim();
                objexeresult.MeterID = txtPCBAID.Text.Trim();//-In Case of Serialization
                objexeresult.ExecutionProcedureType = testCategory;
                objexeresult.ExecutionMeterType = proceduredt.Rows[0]["MeterType"].ToString();
                objexeresult.ExecutionTestID = TestProcedureName;
                //--------------------------------Execution ----------------------------------------------------------
                bool finalExecutionStatus = ExecuteProcedure();
                if (finalExecutionStatus) pbExecutionStatus.Image = PMPSAPPHIRE.Properties.Resources.ExecutionPass;
                else pbExecutionStatus.Image = PMPSAPPHIRE.Properties.Resources.ExecutionFail;

                txtLastScanID.Text = txtPCBAID.Text.Trim();
                if (objexeresult.FinalResult == "PASS" && finalExecutionStatus)  { txtPCBAID.Text = ""; }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                pbExecutionStatus.Image = PMPSAPPHIRE.Properties.Resources.ExecutionWait;
                Application.DoEvents();
                int string_length = 0;
                string MeterID = string.Empty;
                bool isComError = false;
                string productionTravelerSupport = "";
                while (string_length < DGVParaLists.Rows.Count)
                {
                    DGVParaLists.Rows[string_length].Cells["colRemarks"].Value = "";
                    DGVParaLists.Rows[string_length].Cells["colStatus"].Value = "";
                    DGVParaLists.Rows[string_length].Cells["colStatus"].Style.BackColor = Color.White;
                    string_length++;
                }
                int selectedControlIDX = 0;
                string[] PreTestPara = new string[] { "INITMETER", "LEDTEST", "SRPFTEST" };

                string_length = test_index;
                objLI.DisplayStatusMsg("", false);
                txtMeterPCBAID.Text = "";
                objappSettings.SetMeterMode((int)LayerInterface.MeterTypeInfo.SAPPHIRE);
                if (!objLI.ConnectToMeter()) return false;
                if(objLI.MeterInfoValue.Trim().Length<=0)
                {
                /*--------------Check if meter is Sapphire S2 then set meter model as Sapphire S2 to Continue-----------------*/
                    objappSettings.SetMeterMode((int)LayerInterface.MeterTypeInfo.SAPPHIRE_S2);
                    objLI.ValidMeterTypeInfo();
                    if (!objLI.MeterInfoValue.Contains("SPS2")) objappSettings.SetMeterMode((int)LayerInterface.MeterTypeInfo.SAPPHIRE);
                }
                selectedControlIDX = 0;
                //-----------------------Read PCBA ID-------------------------------------------
                string getpcbaResponse = objccmdmethod.ReadPCBAID();
                if (objexeresult.ExecutionProcedureType.IndexOf(StaticVariables.TestType_EMS) < 0)
                {
                    //txtMeterPCBAID.Text = getpcbaResponse;
                    if (getpcbaResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0 || getpcbaResponse.Trim().Length < 7) { MessageBox.Show("Invalid PCBA ID, Length Should be >= 7 " + "\n" + getpcbaResponse, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop); return false; }
                }
                //-----------------------New Implementation for WO Scan-------------------
                txtMeterPCBAID.Text = getpcbaResponse;
                if (objexeresult.ExecutionProcedureType.IndexOf(StaticVariables.TestType_SR) < 0)
                {
                    if (ExecutionWithManualScan == true && getpcbaResponse != txtPCBAID.Text.Trim())
                    {
                        MessageBox.Show("Scan PCBA ID and Meter PCBA ID Not Match !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    IsFrontTested = objexere.IsCaseTamper_3Phase_AlreadyPass(objexeresult);
                    objLI.DisplayStatusMsg("Tests In Execution, Please Wait ...", false);
                }
                //-----------------------------------------------------------------------------
                while (selectedControlIDX < paraNameList.Count)
                {
                    string ParaToBeExecute = paraNameList[selectedControlIDX];
                     
                    if (PreTestPara.Contains(ParaToBeExecute)) { selectedControlIDX++; continue; }
                    DGVParaLists.Rows[selectedControlIDX].Selected = true;
                    if (selectedControlIDX > 0 && DGVParaLists.Rows[selectedControlIDX - 1].Cells[6].Value.ToString() == (StaticVariables.ERRORPreFix + "COMM Failed.")) break;
                    DGVParaLists.Rows[selectedControlIDX].Cells["colStatus"].Value = "Wait...";
                    if (selectedControlIDX % 10 == 0) DGVParaLists.FirstDisplayedScrollingRowIndex = selectedControlIDX;
                    Application.DoEvents();
                    testExecutionStatus = -1;
                    switch (ParaToBeExecute)
                    {
                        case "VERIFYPRODUCTSTAGE":
                            productionTravelerSupport = DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString();
                            string tavelerreadResponse ="";
                            if (productionTravelerSupport.ToUpperInvariant().IndexOf("OLD") >= 0) tavelerreadResponse = objccmdmethod.GetTravelerData_3PhaseDLMS(TravelerStage);//---For Old Sapphire
                            else tavelerreadResponse = objccmdmethod.GetTravelerData_3Phase(TravelerStage, objappSettings.GetMeterMode()); //---For New LTCT Sapphire
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = tavelerreadResponse;
                            if (VerifyProductStage(tavelerreadResponse)) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass; break; }
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
                            string fwVersionResponse ="";
                            if (objappSettings.GetMeterMode() == (int)LayerInterface.MeterTypeInfo.SAPPHIRE_S2) fwVersionResponse = objccmdmethod.CheckingFirmwareVersion_SmartMeter(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            else  fwVersionResponse = objccmdmethod.CheckingFirmwareVersion(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
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
                            string configFile = DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString();;
                            string readconfigResponse = "";
                            if (!configFile.ToUpperInvariant().Contains(".CSV")) readconfigResponse = objccmdmethod.VerifyDisplayConfigData_3PhaseDLMS(configFile);//Keep till old cfh support once replace by csv then only can be remove
                            else readconfigResponse = objccmdmethod.VerifyDisplayConfigData_3Phase(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());//For New CSV File Support    
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
                        case "VERIFYCTRATIO":
                            string getCTratioResponse = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.CTRatioDataStracture.CTRatioOBIS, DLMSDataStracture.CTRatioDataStracture.CTRatioClassID, (int)MeterParaRead.ReadDefault, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = getCTratioResponse;
                            if (getCTratioResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYPTRATIO":
                            string getPTratioResponse = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.PTRatioDataStracture.PTRatioOBIS, DLMSDataStracture.PTRatioDataStracture.PTRatioClassID, (int)MeterParaRead.ReadDefault, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = getPTratioResponse;
                            if (getPTratioResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;        
                        case "SETMETERRTC":
                            string SetRTCResponse = "";
                            if (objappSettings.GetMeterMode() == (int)LayerInterface.MeterTypeInfo.SAPPHIRE_S2) SetRTCResponse = objccmdmethod.SetMeterRTC(true, 0xFF,0);
                            else SetRTCResponse = objccmdmethod.SetMeterRTC(false, 0x00,0);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = SetRTCResponse;
                            if (SetRTCResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "SETMETERRTCAHEAD30MINUTES":
                            string SetRTCResponseBhutan = "";
                            if (objappSettings.GetMeterMode() == (int)LayerInterface.MeterTypeInfo.SAPPHIRE_S2) objccmdmethod.SetMeterRTC(true,0xFF, 1800);
                            else objccmdmethod.SetMeterRTC(false, 0x00, 1800);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = SetRTCResponseBhutan;
                            if (SetRTCResponseBhutan.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        //---------------For OLD Sapphire WCM-----------------------------
                        case "PCBASTATUSTEST":
                            frmLCDDisplayTest objledtest = new frmLCDDisplayTest(StaticVariablesvar);
                            objledtest.ShowDialog();
                            Application.DoEvents();
                            string displayStatus = StaticVariablesvar.LCDSegmentTestResponse;
                            if (displayStatus.IndexOf(StaticVariables.ERRORPreFix) >= 0) { DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = displayStatus; testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail; break; }
                            string pcbastatusResponse = objccmdmethod.CheckingPCBASTATUS_3Phase(objappSettings.GetMeterMode());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = pcbastatusResponse + displayStatus;
                            if (pcbastatusResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                            //---------------For New Sapphire LTCT-----------------------------
                        case "PCBASTATUSTESTLTCT":
                            frmLCDDisplayTest objledtestltct = new frmLCDDisplayTest(StaticVariablesvar);
                            objledtestltct.ShowDialog();
                            Application.DoEvents();
                            string displayStatusltct = StaticVariablesvar.LCDSegmentTestResponse;
                            if (displayStatusltct.IndexOf(StaticVariables.ERRORPreFix) >= 0) { DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = displayStatusltct; testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail; break; }
                            string pcbastatusResponseltct = objccmdmethod.CheckingPCBASTATUS_3PhaseSapphireLTCTC((int)StaticVariables.ExecutedMeterType.SAPPHIRE);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = pcbastatusResponseltct + displayStatusltct;
                            if (pcbastatusResponseltct.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "MAGNETSENSORTESTLEFT":
                            MessageBox.Show("Apply Magnet To Left Sensor ! Then Hit OK To Continue !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Application.DoEvents();
                            string mgtleftResponse = objccmdmethod.CheckingMagnetSensorTest((int)StaticVariables.MagnetTest.LeftSensor);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = mgtleftResponse;
                            if (mgtleftResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "MAGNETSENSORTESTRIGHT":
                            MessageBox.Show("Apply Magnet To Right Sensor ! Then Hit OK To Continue !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Application.DoEvents();
                            string mgtrightResponse = objccmdmethod.CheckingMagnetSensorTest((int)StaticVariables.MagnetTest.RightSensor);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = mgtrightResponse;
                            if (mgtrightResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;   
                        case "VOLTAGETESTRPHASE":
                            string getVoltageRphaseResponse = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.VoltageOBIS_RPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID, (int)MeterParaRead.Voltage_RPhase, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = getVoltageRphaseResponse;
                            if (getVoltageRphaseResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VOLTAGETESTYPHASE":
                            string getVoltageYphaseResponse = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.VoltageOBIS_YPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID, (int)MeterParaRead.Voltage_RPhase, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = getVoltageYphaseResponse;
                            if (getVoltageYphaseResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;

                        case "VOLTAGETESTBPHASE":
                            string getVoltageBphaseResponse = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.VoltageOBIS_BPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID, (int)MeterParaRead.Voltage_RPhase, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = getVoltageBphaseResponse;
                            if (getVoltageBphaseResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;

                        case "CURRENTTESTRPHASE":
                            string getCurrentRphaseResponse = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.CurrentOBIS_RPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID, (int)MeterParaRead.Voltage_RPhase, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = getCurrentRphaseResponse;
                            if (getCurrentRphaseResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;

                        case "CURRENTTESTYPHASE":
                            string getCurrentYphaseResponse = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.CurrentOBIS_YPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID, (int)MeterParaRead.Voltage_RPhase, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = getCurrentYphaseResponse;
                            if (getCurrentYphaseResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;

                        case "CURRENTTESTBPHASE":
                            string getCurrentBphaseResponse = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.CurrentOBIS_BPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID, (int)MeterParaRead.Voltage_RPhase, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = getCurrentBphaseResponse;
                            if (getCurrentBphaseResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;

                        case "POWERFACTORRPHASE":
                            string getPowerRphaseResponse = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.PFOBIS_RPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID, (int)MeterParaRead.Voltage_RPhase, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = getPowerRphaseResponse;
                            if (getPowerRphaseResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;

                        case "POWERFACTORYPHASE":
                            string getPowerYphaseResponse = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.PFOBIS_YPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID, (int)MeterParaRead.Voltage_RPhase, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = getPowerYphaseResponse;
                            if (getPowerYphaseResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;

                        case "POWERFACTORBPHASE":
                            string getPowerBphaseResponse = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.PFOBIS_BPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID, (int)MeterParaRead.Voltage_RPhase, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = getPowerBphaseResponse;
                            if (getPowerBphaseResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "NEUTRALCURRENTTEST":
                            byte[] neuCurrentOBIS = DLMSDataStracture.ReadNeutralCurrentDataStracture.ReadNeutralCurrentOBIS;
                            byte neuCurrentClassID=DLMSDataStracture.ReadNeutralCurrentDataStracture.ReadNeutralCurrentClassID;
                            if (objappSettings.GetMeterMode() == (int)LayerInterface.MeterTypeInfo.SAPPHIRE_S2) { neuCurrentOBIS = DLMSDataStracture.CalibrationDataStracture.CalibrationNeutralCurrentReadOBIS; neuCurrentClassID = DLMSDataStracture.CalibrationDataStracture.CalibrationNonPowerReadClassID; }
                            string getneuCurrenetResponse = objccmdmethod.ReadMeterParameters_3Phase(neuCurrentOBIS, neuCurrentClassID, (int)MeterParaRead.NeutralCurrent, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = getneuCurrenetResponse;
                            if (getneuCurrenetResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;

                        case "CTDIRECTIONTEST":
                            string getctDirectionResponse = "";
                            if (objappSettings.GetMeterMode() == (int)LayerInterface.MeterTypeInfo.SAPPHIRE_S2) getctDirectionResponse = objccmdmethod.ReadCTDirectionSapphireS2();
                            else getctDirectionResponse = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.CTDirectionDataStracture.CTDirectionOBIS, DLMSDataStracture.CTDirectionDataStracture.CTDirectionClassID, (int)MeterParaRead.CTDirection, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = getctDirectionResponse;
                            if (getctDirectionResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "COMMUNICATIONTESTONBATTERY":
                            objLI.AssociationDisconnect();
                            MessageBox.Show("Power-on Meter on Battery Mode And Hit OK To Continue.", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Application.DoEvents();
                            Thread.Sleep(100);
                            if (!objLI.ConnectToMeter()) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail; DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = StaticVariables.ERRORPreFix + "COMM Failed."; break; }
                             Application.DoEvents();
                            string getBatteryModeVoltageRPH = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.VoltageOBIS_RPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID, (int)MeterParaRead.Voltage_RPhase, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            string getBatteryModeVoltageYPH= objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.VoltageOBIS_YPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID, (int)MeterParaRead.Voltage_RPhase, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            string getBatteryModeVoltageBPH = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.VoltageOBIS_BPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID, (int)MeterParaRead.Voltage_RPhase, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = "Voltage >>> R Phase =" + getBatteryModeVoltageRPH + ",  " + " Y Phase =" + getBatteryModeVoltageYPH + ",  " + " B Phase =" + getBatteryModeVoltageBPH;
                            if (getBatteryModeVoltageRPH.IndexOf(StaticVariables.ERRORPreFix) < 0 && getBatteryModeVoltageYPH.IndexOf(StaticVariables.ERRORPreFix) < 0 && getBatteryModeVoltageBPH.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            objLI.AssociationDisconnect();
                            Thread.Sleep(100);
                            MessageBox.Show("Power-on Meter on Main Supply Mode And Hit OK To Continue.", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Thread.Sleep(1500);
                            if (!objLI.ConnectToMeter()) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail; DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = StaticVariables.ERRORPreFix + "COMM Failed."; break; }
                            Application.DoEvents();
                            break;
                        case "COMMUNICATIONTESTONBATTERYWOMAINS":
                            string getBatteryModeVoltageRPHWOMAINS = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.VoltageOBIS_RPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID, (int)MeterParaRead.Voltage_RPhase, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            string getBatteryModeVoltageYPHWOMAINS = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.VoltageOBIS_YPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID, (int)MeterParaRead.Voltage_RPhase, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            string getBatteryModeVoltageBPHWOMAINS = objccmdmethod.ReadMeterParameters_3Phase(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.VoltageOBIS_BPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID, (int)MeterParaRead.Voltage_RPhase, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = "Voltage >>> R Phase =" + getBatteryModeVoltageRPHWOMAINS + ",  " + " Y Phase =" + getBatteryModeVoltageYPHWOMAINS + ",  " + " B Phase =" + getBatteryModeVoltageBPHWOMAINS;
                            if (getBatteryModeVoltageRPHWOMAINS.IndexOf(StaticVariables.ERRORPreFix) < 0 && getBatteryModeVoltageYPHWOMAINS.IndexOf(StaticVariables.ERRORPreFix) < 0 && getBatteryModeVoltageBPHWOMAINS.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;  
                        case "COMMUNICATIONTEST":
                            string CommtestResponse = objccmdmethod.CommunicationTest(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = CommtestResponse;
                            if (CommtestResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "CASETAMPERTEST":
                            string caseResponse = objccmdmethod.ResettingCaseTamper(IsFrontTested);
                            if (caseResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = caseResponse;
                            break;
                        case "RTCDRIFTTEST":
                            string rtcDriftResponse = objccmdmethod.TestRTCDrift(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = rtcDriftResponse;
                            if (rtcDriftResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "RTCDRIFTTESTAHEAD30MINUTES":
                            string rtcDriftResponseBhutan = objccmdmethod.TestRTCDrift(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString(),1800);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = rtcDriftResponseBhutan;
                            if (rtcDriftResponseBhutan.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYCALIBRATIONDATA":
                            string refValue = DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString();
                            bool IsSapphireOldMeter = false;
                            string caliRespResponse="";
                            if (objappSettings.GetMeterMode() == (int)LayerInterface.MeterTypeInfo.SAPPHIRE_S2) caliRespResponse = objccmdmethod.VerifyCalibrationData();
                            else
                            {
                                if (refValue.ToUpperInvariant() == "OLD") IsSapphireOldMeter = true;
                                //----Verify Calibration Data based on DLMS structured command for calibration cofficient read
                                if (refValue.ToUpperInvariant() == "DLMS") caliRespResponse = objccmdmethod.VerifyCalibrationDataShappireDLMSCommand();
                                //----Verify Calibration Data based on Manufacture specefic non-DLMS command for calibration cofficient read
                                else caliRespResponse = objccmdmethod.TestCalibrationData_3PhaseDLMS((int)StaticVariables.ExecutedMeterType.SAPPHIRE, IsSapphireOldMeter);
                            }
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = caliRespResponse;
                            if (caliRespResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                       case "WRITECURRENTRATING":
                            string mfgdataWritrResponse = objccmdmethod.WriteCuttentRating(DGVParaLists.Rows[selectedControlIDX].Cells["ColMinVal"].Value.ToString(), DGVParaLists.Rows[selectedControlIDX].Cells["ColMaxValue"].Value.ToString(), DLMSDataStracture.CurrentRatingDataStracture.CurrentRatingDataLength_3Phase);
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = mfgdataWritrResponse;
                            if (mfgdataWritrResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "WRITEMANUFACTURINGYEAR":
                            string mfgResponse = objccmdmethod.WrittingManufacturingYear(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = mfgResponse.Trim();
                            if (mfgResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYMANUFACTURINGYEAR":                        
                            string readmfgResponse = objccmdmethod.VerifyManufacturingYear(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = readmfgResponse.Trim();
                            if (readmfgResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYCURRENTRATING":
                            string readIratingResponse = objccmdmethod.VerifyCurrentRating(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = readIratingResponse.Trim();
                            if (readIratingResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYMETERID":
                            string readmidResponse = objccmdmethod.VerifyMeterID(txtPCBAID.Text.Trim());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = readmidResponse.Trim();
                            if (readmidResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYRS485DEVICEADDRESS":
                            string readrs485Response = objccmdmethod.VerifyRS485Address(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = readrs485Response.Trim();
                            if (readrs485Response.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYDEVICETYPE":
                            string readdeviceType = objccmdmethod.VerifyDeviceType(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = readdeviceType.Trim();
                            if (readdeviceType.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
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
                        case "QUICKRESETALLDATA":
                            string quickresetallResponse = objccmdmethod.QuickResetAllData();
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = quickresetallResponse;
                            if (quickresetallResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "RESETBATTERYCOUNTER":
                            string resetbatteryCounterResponse = "";
                            if (objappSettings.GetMeterMode() == (int)LayerInterface.MeterTypeInfo.SAPPHIRE_S2) resetbatteryCounterResponse = objccmdmethod.ResetBatteryCounterS2();
                            else resetbatteryCounterResponse = objccmdmethod.ResetBatteryCounter();
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = resetbatteryCounterResponse;
                            if (resetbatteryCounterResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "READPCBAID":
                            string pcbaResponse = objccmdmethod.ReadPCBAID();                            
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = pcbaResponse.Trim();
                            if (pcbaResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) { txtMeterPCBAID.Text = pcbaResponse; testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass; }
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;                            
                            break;
                        case "WRITEMETERID":
                            string writemIDResponse = objccmdmethod.WrittingMeterID_3PhaseSapphire(txtPCBAID.Text.Trim());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = writemIDResponse.Trim();
                            if (writemIDResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            break;
                        case "VERIFYLPR":
                            objLI.AssociationDisconnect();
                            frmLPRScan ObjLprScan = new frmLPRScan();
                            ObjLprScan.ShowDialog();
                            if (ObjLprScan.strLPR_ID.Length != 5)
                            {
                                DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = StaticVariables.ERRORPreFix + "Invalid LPR ID Entered !";
                                testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                                break;
                            }
                            else ObjLprScan.strLPR_ID = "!" + ObjLprScan.strLPR_ID + "!";
                            string lprResponse = objccmdmethod.LPRTestAnalogic(txtPCBAID.Text.Trim(), ObjLprScan.strLPR_ID, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = lprResponse.Trim();
                            if (lprResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail; break; }
                            Thread.Sleep(100);
                            if (!objLI.ConnectToMeter()) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail; DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = StaticVariables.ERRORPreFix + "COMM Failed."; break; }
                            Application.DoEvents();
                            break;
                        case "VERIFYMLPR":
                            objLI.AssociationDisconnect();
                            frmMLPRScan ObjmLprScan = new frmMLPRScan();
                            ObjmLprScan.ShowDialog();
                            if (ObjmLprScan.strLPR_ID.Length != 8)
                            {
                                DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = StaticVariables.ERRORPreFix + "Invalid LPR ID Entered !";
                                testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                                break;
                            }
                            string lprmResponse = objccmdmethod.LPRTestMilange(txtPCBAID.Text.Trim(), ObjmLprScan.strLPR_ID, DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString());
                            DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = lprmResponse.Trim();
                            if (lprmResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                            else { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail; break; }
                            Thread.Sleep(100);
                            if (!objLI.ConnectToMeter()) { testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail; DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = StaticVariables.ERRORPreFix + "COMM Failed."; break; }
                            Application.DoEvents();
                            break;
                        case "LOCKINGMETER":
                            if (objccmdmethod.GetExecutionStatus(DGVParaLists.Rows.Count - 1, DGVParaLists))
                            {
                                string lockResponse = objccmdmethod.LockingMeter_3Phase(0x01);
                                DGVParaLists.Rows[selectedControlIDX].Cells["colRemarks"].Value = lockResponse.Trim();
                                if (lockResponse.IndexOf(StaticVariables.ERRORPreFix) < 0) testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Pass;
                                else testExecutionStatus = (int)StaticVariables.ExecutionReurnStatus.Fail;
                            }
                            break;
                        case "MANUALMESSAGE":
                            if (DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString().Trim().Length > 0)
                            {
                                MessageBox.Show(DGVParaLists.Rows[selectedControlIDX].Cells["colDefaultValue"].Value.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        if (productionTravelerSupport.ToUpperInvariant().IndexOf("OLD") >= 0)
                        {
                            //--------------------For OLD Sapphire-------------------------------------
                            if (!objccmdmethod.SetTravelerData_3PhaseDLMS(selectedControlIDX, TravelerStage, DGVParaLists)) { MessageBox.Show("Unable To Set Production Stage Flag, Meter May at Invalid Production Stage !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
                        }
                        else
                        {
                            if (!objccmdmethod.SetTravelerData_3Phase(selectedControlIDX, TravelerStage, DGVParaLists)) { MessageBox.Show("Unable To Set Production Stage Flag, Meter May at Invalid Production Stage !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
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
                MessageBox.Show(ex.Message, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("Product are At Invalid Production Stage, Return To Previous Production Stage !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("Unable To Save Results !" + "\n" + resultmsg, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                    MessageBox.Show("Scan Meter Record Not Found in Database !, Re-Scan and Try Again !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
              
                return true;
            }
            catch (Exception Ex)
            {
                resultmsg = Ex.Message;
                objLI.DisplayStatusMsg("Unable To Save Results !", true);
                MessageBox.Show("Unable To Save Results !" + "\n" + Ex.Message, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                dlmsCommStatusmsh.Width = (DLMSStas.Width / 2) ;
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
                string filename = AppDomain.CurrentDomain.BaseDirectory + @"\PMPSAPPHIRE.exe";
                string strbuilton = "Built On: " + File.GetCreationTime(filename).ToShortDateString().ToString();
                bool Flg_ComSetting = true;// objSerialComm.GetPortSettings();
                if (Flg_ComSetting)
                {
                    Communication = dlmscommmode;
                }
                string Prduct_Version = SystemInfo.ProductVersion();
                string strpath = AppDomain.CurrentDomain.BaseDirectory + "PMPSAPPHIRE.exe";
                Company_Profile = SystemInfo.CopyRightsDetail();
                Prduct_Version = Application.ProductName.ToString() + " Ver. " + Prduct_Version;
                stsReady.Text = "Association : " + dlmscommmode;
                lblversion.Text = Prduct_Version;// +" | " + strbuilton;
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Setting Main Form Status Failed !" + "\r\n" + "\r\n" + Ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
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
                else { txtPCBAID.MaxLength = 13; lblParaonTestType.Text = "Scan PCBA ID"; ProgrammLabelMsg = "PCBA"; lblLastScan.Text = "Last Scan PCBA ID"; }
            }
        }       
     
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtMeterID_KeyUp(object sender, KeyEventArgs e)
        {            
            if (isProcedureExecuted) return;
            if (txtPCBAID.Text.Trim().Length == txtPCBAID.MaxLength)
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
                        MessageBox.Show("Please Scan Valid " + ProgrammLabelMsg + " ID !, ID Should Not Blank!", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    btnStart_Click(sender, e);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

    }
}
