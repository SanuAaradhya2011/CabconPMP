using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using System.IO;
using ApplicationInterface; 
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Data;
using System.Windows.Forms;
namespace COMMONENTITY
{
   public class CommonCommandMethods
    {
       enum SmartMeter1PCalibrationIndex { Voltage = 0, PhaseCurrent = 1, NeutralCurrent = 2, PhaseActivePower = 3, NeutralActivePower = 4, PhaseReactivePower = 5, NeutralReactivePower = 6, PhaseDelay = 7, NeutralDelay = 8 };

        LayerInterface objLI = new LayerInterface();
       AppSettings objappSettings = new AppSettings();
       TextBox[] txtboxobject = new TextBox[] { };
       CommonMethods objcomnMethod = new CommonMethods();     
       decimal objdec;
       public bool IsTravellerWriteSkip = false;
       public string ReadPCBAID()
       {
           try
           {
               string ClientSAP = Convert.ToInt32(objappSettings.GetClientSAP(), 10).ToString("X");
                string unloacmeterResponse = "";
                if (!objLI.ReadByteFromMeter(DLMSDataStracture.PCBAIDDataStracture.PCBAIDOBIS, txtboxobject, "0", 1M, DLMSDataStracture.PCBAIDDataStracture.PCBAIDClassID, DLMSDataStracture.PCBAIDDataStracture.PCBAIDValueAttribute)) { return  StaticVariables.ERRORPreFix + "COMM Failed."; }
                string readpcbaresponse = DisplayDatainControl(GlobalObjects.objSerialComm.ReceiveBuffer, txtboxobject, "0", 1M);
                if (readpcbaresponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return readpcbaresponse;
                //------------Unlock the meter , It may Lock--------------     
                if (objappSettings.GetMeterMode() == (int)StaticVariables.ExecutedMeterType.Smart_Meter_1PH || objappSettings.GetMeterMode() == (int)StaticVariables.ExecutedMeterType.MicroStar_DLMS)
                {
                    if (IsTravellerWriteSkip == false && (ClientSAP == "40" || ClientSAP == "7E")) unloacmeterResponse = LockingMeter(0x00); //--If association is only FS mode
                }
                // else if (objappSettings.GetMeterMode() == (int)StaticVariables.ExecutedMeterType.Smart_Meter_3PH) unloacmeterResponse = LockingMeter_3Phase(0x00); //---As discussed with 3P FW team Lock is not supported in Falcon2
                else return readpcbaresponse;
                if (unloacmeterResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return StaticVariables.ERRORPreFix + unloacmeterResponse + ", Unable To Unlock"; 
                return readpcbaresponse;      
                 
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;               
           }
       }

       public string VerifyPCBAIDwithReadID(string refPCBA, string readPcba, string defaultVal, string refminVal, string refmaxVal)
       {
           string lenVerification="";
           int objint;
           if (Int32.TryParse(defaultVal, out objint) || Int32.TryParse(refminVal, out objint) || Int32.TryParse(refmaxVal, out objint)) { }
           else return StaticVariables.ERRORPreFix + "Invalid PCBA ID Length Input.";
           if (readPcba.IndexOf(StaticVariables.ERRORPreFix) < 0)
           {
             lenVerification = objcomnMethod.CheckingRangeValueForDecimal("Meter PCBA ID Length =",defaultVal, refminVal, refmaxVal,readPcba.Length.ToString());
           }
           if (readPcba.IndexOf(StaticVariables.ERRORPreFix) >= 0) return "Unable To Verify PCBA ID, " + lenVerification;
           else if (lenVerification.IndexOf(StaticVariables.ERRORPreFix) >= 0) return "PCBA ID =" + readPcba + ", " + lenVerification ;
           else if (refPCBA.Trim() != readPcba.Trim()) return StaticVariables.ERRORPreFix + "PCBA ID =" + readPcba + ", PCBA ID Not Match, " + lenVerification;
           else return "PCBA ID =" + readPcba + ", " + lenVerification;          
               
       }

       public string WritePCBAID(string scanPCBAID, byte IDbyteLength)
       {
           try
           {
               if (!objLI.WriteDataToMeter(DLMSDataStracture.PCBAIDDataStracture.PCBAIDValueAttribute, DLMSDataStracture.PCBAIDDataStracture.PCBAIDOBIS, DLMSDataStracture.PCBAIDDataStracture.PCBAIDClassID, DLMSDataStracture.PCBAIDDataStracture.PCBAIDDataType, IDbyteLength, GetMeterIDByte(scanPCBAID, IDbyteLength), DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public string FSModeCommunicationLocking(byte lockingByte)
       {
           try
           {
               //------------------------------------------Read FS Mode Locking Struructure Byte------------------
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.FSModeLockingDataStructure.FSModeLockingOBIS, txtboxobject, "0", 1M, DLMSDataStracture.FSModeLockingDataStructure.FSModeLockingClassID, DLMSDataStracture.FSModeLockingDataStructure.FSModeLockingValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               int startDataindx = 18;
               string[] datavalue = DLMSDataStracture.DLMSDataFormator(GlobalObjects.objSerialComm.ReceiveBuffer, startDataindx, true);
               if (datavalue.Length < 2) return StaticVariables.ERRORPreFix + "Invalid Configuration";
               
               string receivedData = Convert.ToString(Convert.ToInt32(datavalue[0], 16), 2);
               receivedData = receivedData.PadLeft(8, '0');
               receivedData = receivedData.Substring(0, 7) + lockingByte.ToString();
               receivedData = (Convert.ToInt32(receivedData, 2)).ToString("X");
             //----------------------------------------------------------------------------------------------------
               List<byte> ResetBytes = new List<byte> { Convert.ToByte(receivedData, 16) };
               if (!objLI.WriteDataToMeter(DLMSDataStracture.FSModeLockingDataStructure.FSModeLockingValueAttribute, DLMSDataStracture.FSModeLockingDataStructure.FSModeLockingOBIS, DLMSDataStracture.FSModeLockingDataStructure.FSModeLockingClassID, DLMSDataStracture.FSModeLockingDataStructure.FSModeLockingDataType, DLMSDataStracture.FSModeLockingDataStructure.FSModeLockingDataLength, ResetBytes, DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public string CheckingFirmwareVersion(string RefFWVersion)
       {
           try
           {
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadMeterInfoDataStracture.ReadMeterInfoOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadMeterInfoDataStracture.ReadMeterInfoClassID, DLMSDataStracture.ReadMeterInfoDataStracture.ReadMeterInfoValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return (IsValidFirmware(GlobalObjects.objSerialComm.ReceiveBuffer, RefFWVersion));               
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public string CheckingFirmwareVersion_SmartMeter(string RefFWVersion)
       {
           try
           {
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.BuildVersionDataStracture.BuildVersionOBIS, txtboxobject, "0", 100M, DLMSDataStracture.BuildVersionDataStracture.BuildVersionClassID, DLMSDataStracture.BuildVersionDataStracture.BuildVersionValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return (IsValidFirmware_SmartMeter(GlobalObjects.objSerialComm.ReceiveBuffer, RefFWVersion));
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public string CheckingFirmwareVersion_MicroStarDLMS(string RefFWVersion)
       {
           try
           {
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadMeterInfoDataStracture.ReadMeterInfoOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadMeterInfoDataStracture.ReadMeterInfoClassID, DLMSDataStracture.ReadMeterInfoDataStracture.ReadMeterInfoValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return (IsValidFirmware_MicroStarDLMS(GlobalObjects.objSerialComm.ReceiveBuffer, RefFWVersion));
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string CheckingPCBASTATUS_3Phase(int meterExecutionType)
       {
           try
           {
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadFlashStatusDataStracture.ReadFlashStatusOBIS, txtboxobject, "0", 1M, DLMSDataStracture.ReadFlashStatusDataStracture.ReadFlashStatusClassID, DLMSDataStracture.ReadFlashStatusDataStracture.ReadFlashStatusValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               if (meterExecutionType == (int)StaticVariables.ExecutedMeterType.Smart_Meter_3PH) return (FillPCBAStatusSmartMeter(GlobalObjects.objSerialComm.ReceiveBuffer));
               else if (meterExecutionType == (int)StaticVariables.ExecutedMeterType.DLMS_3PH) return (FillPCBAStatus(GlobalObjects.objSerialComm.ReceiveBuffer));
               else if (meterExecutionType == (int)StaticVariables.ExecutedMeterType.SAPPHIRE) return (FillPCBAStatusSapphire(GlobalObjects.objSerialComm.ReceiveBuffer));
               else if (meterExecutionType == (int)StaticVariables.ExecutedMeterType.SAPPHIRE_S2) return (FillPCBAStatus_SapphireS2(GlobalObjects.objSerialComm.ReceiveBuffer));
               else {return StaticVariables.ERRORPreFix + "Invalid Method Call"; }
            }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }
       public string CheckingPCBASTATUS_3PhaseSapphireLTCTC(int meterExecutionType)
       {
           try
           {
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadFlashStatusDataStracture.ReadFlashStatusOBIS, txtboxobject, "0", 1M, DLMSDataStracture.ReadFlashStatusDataStracture.ReadFlashStatusClassID, DLMSDataStracture.ReadFlashStatusDataStracture.ReadFlashStatusValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return (FillPCBAStatusSapphireLTCT(GlobalObjects.objSerialComm.ReceiveBuffer));
                
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }
       public string CheckingCase_SM310(bool IsFrontTested)
       {
           int defcmdtimeout = GlobalObjects.objSerialComm.CommandTimeout;
           try
           {
               string caseStatus = GetCaseStatus_SM310();
               if (caseStatus.IndexOf(StaticVariables.ERRORPreFix) >= 0) return caseStatus;
               if (IsFrontTested && Convert.ToByte(caseStatus) == 0) return "Case Flag =" + caseStatus.ToString();
               if (!IsFrontTested && Convert.ToByte(caseStatus) != 1) return StaticVariables.ERRORPreFix + "Case Not Detected, Switch May Damage =" + caseStatus.ToString();
               List<byte> ResetBytes = new List<byte> { 0x00, 0x01 };
               GlobalObjects.objSerialComm.CommandTimeout = 10000;// 6000;//--Setting Tamper Reset timeouts
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ResetDataStracture.ResetValueAttribute, DLMSDataStracture.ResetDataStracture.ResetTamperOBIS, DLMSDataStracture.ResetDataStracture.ResetClassID, DLMSDataStracture.ResetDataStracture.ResetDataType_Falcon2, DLMSDataStracture.ResetDataStracture.ResetDataLength_Falcon2, ResetBytes, DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               caseStatus = GetCaseStatus_SM310();
               if (caseStatus.IndexOf(StaticVariables.ERRORPreFix) >= 0) return caseStatus;
               if (Convert.ToByte(caseStatus) == 0) return "Case Flag =" + caseStatus.ToString();
               else return StaticVariables.ERRORPreFix + "Case Detected =" + caseStatus.ToString();

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
           finally
           {
               GlobalObjects.objSerialComm.CommandTimeout = defcmdtimeout;
           }

       }
       private string GetCaseStatus_SM310()
       {
           if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadFlashStatusDataStracture.ReadFlashStatusOBIS, txtboxobject, "0", 1M, DLMSDataStracture.ReadFlashStatusDataStracture.ReadFlashStatusClassID, DLMSDataStracture.ReadFlashStatusDataStracture.ReadFlashStatusValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
           string coveropenTest = Convert.ToString(GlobalObjects.objSerialComm.ReceiveBuffer[40], 2).ToString().PadLeft(8, '0');
           byte caseStatus = Convert.ToByte(coveropenTest.Substring(7, 1) == "1");
           return caseStatus.ToString();
       }
       public string CheckingMagnetSensorTest(int mgtSensorTest)
       {
           try
           {
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadFlashStatusDataStracture.ReadFlashStatusOBIS, txtboxobject, "0", 1M, DLMSDataStracture.ReadFlashStatusDataStracture.ReadFlashStatusClassID, DLMSDataStracture.ReadFlashStatusDataStracture.ReadFlashStatusValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return (VerifyMagnetSensor(GlobalObjects.objSerialComm.ReceiveBuffer, mgtSensorTest));
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }

       private string VerifyMagnetSensor(byte[] receivedData, int mgtSensorTest)
       {
           string pcbaststus = string.Empty;       
           string magneticSensorTest = Convert.ToString(receivedData[39], 2).ToString().PadLeft(8, '0');
           int mgtflagStatus = Convert.ToInt16(magneticSensorTest.Substring(7, 1));
           if (mgtSensorTest == (int)StaticVariables.MagnetTest.LeftSensor) mgtflagStatus = Convert.ToInt16(magneticSensorTest.Substring(6, 1));
           if (mgtSensorTest == 1) pcbaststus = "Sensor Value =" + mgtflagStatus.ToString();
           else pcbaststus = StaticVariables.ERRORPreFix + "Sensor Value =" + mgtflagStatus.ToString();
           return pcbaststus;
       }
       public string DIDOCircuitTest(int testStage)
       {
           try
           {
               if (testStage == 0)
               {
                   List<byte> ResetBytes = new List<byte> { 0x01 };
                   if (!objLI.WriteDataToMeter(DLMSDataStracture.DigitalIOConfigurationDataStracture.DigitalIOConfigurationValueAttribute, DLMSDataStracture.DigitalIOConfigurationDataStracture.DigitalIOConfigurationOBIS, DLMSDataStracture.DigitalIOConfigurationDataStracture.DigitalIOConfigurationClassID, DLMSDataStracture.DigitalIOConfigurationDataStracture.DigitalIOConfigurationDataType, DLMSDataStracture.DigitalIOConfigurationDataStracture.DigitalIOConfigurationDataLength, ResetBytes, DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
                   
               }
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadFlashStatusDataStracture.ReadFlashStatusOBIS, txtboxobject, "0", 1M, DLMSDataStracture.ReadFlashStatusDataStracture.ReadFlashStatusClassID, DLMSDataStracture.ReadFlashStatusDataStracture.ReadFlashStatusValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return (VerifyDIDIFlag(GlobalObjects.objSerialComm.ReceiveBuffer, testStage));
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       private string VerifyDIDIFlag(byte[] receivedData, int testStage)
       {
           string DidiFlagBits = Convert.ToString(receivedData[0x29], 2).ToString().PadLeft(8, '0');
           byte DidiFlagValue = receivedData[0x29];
           char[] bitstruct = DidiFlagBits.ToCharArray();
           string retStatus = "";
           switch (testStage)
           {
            case 0: //----------1st with Reset Case------
               if (DidiFlagValue != 0x01 /*bitstruct[7] != '1'*/) { retStatus = StaticVariables.ERRORPreFix + receivedData[0x29].ToString("X2"); }
               else retStatus = receivedData[0x29].ToString("x2");
               break;
            case 1:
               if (DidiFlagValue != 0x03 /*bitstruct[6] != '1'*/) { retStatus = StaticVariables.ERRORPreFix + receivedData[0x29].ToString("X2"); }
               else retStatus = receivedData[0x29].ToString("x2");
               break;
            case 2:
               if (DidiFlagValue != 0x07 /*bitstruct[5] != '1'*/) { retStatus = StaticVariables.ERRORPreFix + receivedData[0x29].ToString("X2"); }
               else retStatus = receivedData[0x29].ToString("x2");
                break;
            case 3:
                if (DidiFlagValue != 0x0F /*bitstruct[4] != '1'*/) { retStatus = StaticVariables.ERRORPreFix + receivedData[0x29].ToString("X2"); }
               else retStatus = receivedData[0x29].ToString("x2");
                break;
             case 4:
                if (DidiFlagValue != 0x1F /*bitstruct[3] != '1'*/) { retStatus = StaticVariables.ERRORPreFix + receivedData[0x29].ToString("X2"); }
                else retStatus = receivedData[0x29].ToString("x2");
                break;
             
           
           }
           return retStatus;
       }
       private string FillPCBAStatusSmartMeter(byte[] receivedData)
       {
           string rtcBatteryVoltage = "";
           string mainSupplyVoltage = "";         
           string mainBatteryVoltage = "";
           List<string> ParametersListValue = new List<string>(){
                "FLASH 1",
                "FLASH 2",
                "FRAM",
                "UP Key Status",
                "Down Key Status",
                "MD Key Status",
                "",//FCO Button Pin Status",
                "",//Comms Card Removal Button ",
                "",//R Phase Relay Malfunction Circuit",
                "",//Y Phase Relay Malfunction Circuit",
                "",//B Phase Relay Malfunction Circuit ",
                "SMPS -- > Power Supply",
                "RTC Battery Low",
                "RTC Stop",
                "RTC Bad",
                "RTC Battery Voltage",
                "DVcc voltage",
                "Main Battery Voltage",
                "",//Sensor 1",
                "",//Sensor 2",
                };
       /* ---Below Position No need to Compare during FT
       * 6-FC Button
       * 7-Comms Card Removal Button
       * 18-Magnetic Sensor - Right
       * 19-Magnetic Sensor - Left  
       *20-Cover open Tamper Status - Added as seperate seperate Steps as Case Tamper Test
       */
           float RecevData = 0;
           Int16 TempCounter = 0;
           int ListCnt = 0;
           List<string> statusValue = new List<string>();
           int paracnt = 0;
           while (paracnt++ < ParametersListValue.Count) statusValue.Add("FAIL");
           TempCounter = 20;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) statusValue[0] = "PASS";
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) statusValue[1] = "PASS";
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) statusValue[2] = "PASS";
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) statusValue[3] = "PASS";
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) statusValue[4] = "PASS";
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) statusValue[5] = "PASS";
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) statusValue[6] = "PASS"; //---Front open set default pass
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) statusValue[7] = "PASS";//---COM Card Removal Status set default pass
           TempCounter++;

           string relaymalfunstatus = Convert.ToString(receivedData[TempCounter], 2).ToString().PadLeft(8, '0');//---All Pass by Default as not available with meter
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" ||relaymalfunstatus.Substring(7, 1) == "1") 
               statusValue[8] = "PASS";
            if (ParametersListValue[ListCnt++] == "" ||relaymalfunstatus.Substring(6, 1) == "1") 
               statusValue[9] = "PASS";
            if (ParametersListValue[ListCnt++] == "" ||relaymalfunstatus.Substring(5, 1) == "1") 
               statusValue[10] = "PASS";
           


            if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) statusValue[11] = "PASS";
            TempCounter++;
            if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) statusValue[12] = "PASS";
            TempCounter++;
            if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) statusValue[13] = "PASS";
            TempCounter++;
            if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) statusValue[14] = "PASS";
            TempCounter++;

            if (ParametersListValue[ListCnt++] == "") statusValue[11] = "PASS";
            else
            {
                RecevData = Convert.ToInt32((receivedData[TempCounter] << 8) | receivedData[TempCounter + 1]);
                TempCounter++;
                TempCounter++;
                RecevData /= 1000;
                //statusValue[15] = RecevData.ToString();
                if (Convert.ToDecimal(RecevData) > 2.5M) statusValue[15] = "PASS";
                rtcBatteryVoltage = ", RTC Battery Voltage =" + RecevData.ToString();
            }

           if (ParametersListValue[ListCnt++] == "") statusValue[11] = "PASS";
            else
            {
               RecevData = Convert.ToInt32((receivedData[TempCounter] << 8) | receivedData[TempCounter + 1]);
               TempCounter++;
               TempCounter++;
               RecevData /= 1000;
               //statusValue[16] = RecevData.ToString();
               if (Convert.ToDecimal(RecevData) > 2.5M) statusValue[16] = "PASS";
                mainSupplyVoltage = ", DVcc voltage =" + RecevData.ToString();
            }

            if (ParametersListValue[ListCnt++] == "") statusValue[11] = "PASS";
            else
            {
               RecevData = Convert.ToInt32((receivedData[TempCounter] << 8) | receivedData[TempCounter + 1]);
               TempCounter++;
               TempCounter++;
               RecevData /= 1000;
               //statusValue[17] = RecevData.ToString();
               if (Convert.ToDecimal(RecevData) > 2.5M) statusValue[17] = "PASS";
               mainBatteryVoltage = ", Main Battery Voltage =" + RecevData.ToString();
            }

           string magneticSensorTest = Convert.ToString(receivedData[TempCounter], 2).ToString().PadLeft(8, '0');
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" || magneticSensorTest.Substring(7, 1) == "1")//---magnetic Sensor Status set default pass
               statusValue[18] = "PASS";
           if (ParametersListValue[ListCnt++] == "" || magneticSensorTest.Substring(6, 1) == "1")
               statusValue[19] = "PASS";




           paracnt = 0;
           bool isnotok = false;
           string pcbaststus = string.Empty;
           while (paracnt < statusValue.Count)
           {
               if (statusValue[paracnt] == "FAIL") { isnotok = true; pcbaststus += ParametersListValue[paracnt] + ":" + statusValue[paracnt] + ", "; }
               paracnt++;
           }
           if (isnotok) pcbaststus = StaticVariables.ERRORPreFix + pcbaststus;
           return pcbaststus + rtcBatteryVoltage + mainSupplyVoltage + mainBatteryVoltage;
       }

       private string FillPCBAStatusSapphire(byte[] receivedData)
       {
           string rtcBatteryVoltage = "";
           string mainBatteryVoltage = "";
           List<string> ParametersListValue = new List<string>(){
            "FLASH",       
            "FRAM", 
            "UP Button",  
            "Down Button",  
            "MD Button",  
            "",//"Front Cover", 
            "", //Terminal Cover
            "Power Supply",  
            "RTC Battery Low",  
            "RTC Stop",  
            "RTC Time Bad",  
            "RTC Battery Voltage",           
            "", //Reserve -1
            "", //Reserve -2
            "Main Battery Voltage",
            "",//"Magnet",  
            "Calibration Status",
            "RTC Battery Calibration Values",
            "MAIN Battery Calibration Values ",
           };           
           float RecevData = 0;
           Int16 TempCounter = 0;

           List<string> statusValue = new List<string>();
           int paracnt = 0;
           while (paracnt++ < ParametersListValue.Count) statusValue.Add("FAIL");
           TempCounter = 20;
           int ListCnt=0;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) { statusValue[0] = "PASS"; }
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) { statusValue[1] = "PASS"; }
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) { statusValue[2] = "PASS"; }
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) { statusValue[3] = "PASS"; }
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) { statusValue[4] = "PASS"; }
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) { statusValue[5] = "PASS"; }
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) { statusValue[6] = "PASS"; }
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) { statusValue[7] = "PASS"; }
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) { statusValue[8] = "PASS"; }
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) { statusValue[9] = "PASS"; }
           TempCounter++;
           if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) { statusValue[10] = "PASS";}
           TempCounter++;


            if (ParametersListValue[ListCnt++] == "") statusValue[11] = "PASS";
            else
            {
                RecevData = Convert.ToInt32((receivedData[TempCounter] << 8) | receivedData[TempCounter + 1]);
                TempCounter++;
                TempCounter++;
                RecevData /= 1000;
                if (Convert.ToDecimal(RecevData) > 3.0M ) statusValue[11] = "PASS";
                rtcBatteryVoltage ="RTC Battery Voltage =" +  RecevData.ToString();
            }
            if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) statusValue[12] = "PASS";//---Reserved 1
            TempCounter++;
            if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) statusValue[13] = "PASS";//---Reserved 2
            TempCounter++;

            if (ParametersListValue[ListCnt++] == "") statusValue[14] = "PASS";
            else
            {
                RecevData = Convert.ToInt32((receivedData[TempCounter] << 8) | receivedData[TempCounter + 1]);
                TempCounter++;
                TempCounter++;
                RecevData /= 1000;
                if (Convert.ToDecimal(RecevData) > 2.65M) statusValue[14] = "PASS";
                mainBatteryVoltage = ", Main Battery Voltage =" + RecevData.ToString();
            }
            if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) { statusValue[15] = "PASS"; }
            TempCounter++;
            if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] == 1) { statusValue[16] = "PASS"; }
            TempCounter++;
            if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] <= 0x3C) { statusValue[17] = "PASS"; } // calibration value <= 60
            TempCounter++;
            if (ParametersListValue[ListCnt++] == "" || receivedData[TempCounter] <= 0x3C) { statusValue[18] = "PASS"; } // calibration value <= 60
            TempCounter++;

           paracnt = 0;
           bool isnotok = false;
           string pcbaststus = string.Empty;
           while (paracnt < statusValue.Count)
           {                
               if (statusValue[paracnt] == "FAIL") { isnotok = true; pcbaststus += ParametersListValue[paracnt] + ":" + statusValue[paracnt] + ", "; }
               paracnt++;
           }
           if (isnotok) pcbaststus = StaticVariables.ERRORPreFix + pcbaststus;
           return pcbaststus + rtcBatteryVoltage + mainBatteryVoltage;
       }

       private string FillPCBAStatus_SapphireS2(byte[] receivedData)
       {
           // receivedData = DLMSDataStracture.GetByteFromHexStringPattern("7ea055810002040152dc4fe6e700c401c1000940ff2128000000000068014a012c01f0d50100030002000000000000000000000000000000000000000000000000000000000000000000000000000000000000009aee7e");
           List<string> ParametersListValue = new List<string>();
               ParametersListValue.Add("Flash 1");
               ParametersListValue.Add("Flash 2");
               ParametersListValue.Add("EEPROM 1");
               ParametersListValue.Add("EEPROM 2");
               ParametersListValue.Add("SMPS");
               ParametersListValue.Add("RTC Bat Low");
               ParametersListValue.Add("RTC Stop");
               ParametersListValue.Add("RTC Bad");
               ParametersListValue.Add("RTC Battery Remove");
               ParametersListValue.Add("UP Key Status");
               ParametersListValue.Add("Down Key Status");
               ParametersListValue.Add("MD Key Status");
               ParametersListValue.Add("FCO Button Pin Status");
               ParametersListValue.Add("PCBA Test LCD Indication");
               ParametersListValue.Add("Magnet Sensor Indication");
               ParametersListValue.Add("DVcc voltage");
               ParametersListValue.Add("Main battery Voltage");
               ParametersListValue.Add("RTC battery Voltage");
               ParametersListValue.Add("Magnet Interrupt Count");
               ParametersListValue.Add("Current status of Production Traveler");
               ParametersListValue.Add("");
               ParametersListValue.Add("");
               ParametersListValue.Add("");
               ParametersListValue.Add("");
               ParametersListValue.Add("");

           float RecevData = 0;
           Int16 indexCounter = 0;
           byte[] tempBuffer = new byte[2];
           List<string> statusValue = new List<string>();
           int paracnt = 0;
           while (paracnt++ < ParametersListValue.Count)
               statusValue.Add("FAIL");


           indexCounter = 20;
           List<char> meterFlag = new List<char>();
           List<object> meterStatusValue = new List<object>();
           char[] Byte1Value = Convert.ToString(Convert.ToInt32(receivedData[indexCounter].ToString()), 2).PadLeft(8, '0').ToCharArray();
           char[] Byte2Value = Convert.ToString(Convert.ToInt32(receivedData[indexCounter + 1].ToString()), 2).PadLeft(8, '0').ToCharArray();
           char[] Byte3Value = Convert.ToString(Convert.ToInt32(receivedData[indexCounter + 2].ToString()), 2).PadLeft(8, '0').ToCharArray();
           Array.Reverse(Byte1Value);
           Array.Reverse(Byte2Value);
           Array.Reverse(Byte3Value);

           int itemCount = 0;
           foreach (var item in Byte1Value)
           {
               meterStatusValue.Add(item == '1' ? true : false);
               itemCount++;
           }
           itemCount = 0;
           foreach (var item in Byte2Value)
           {
               if (itemCount < 7) meterStatusValue.Add(item == '1' ? true : false);
               itemCount++;
           }
           //----------------Byte 3 is free----------------

           indexCounter += 3;//Reserve Bytes
           indexCounter += 5;//Reserve Bytes
           //--------------------------Numeric data coming in reverse order---------------------
           Array.Copy(receivedData, indexCounter, tempBuffer, 0, tempBuffer.Length);
           Array.Reverse(tempBuffer);
           decimal DVccvoltage = (decimal.Parse(DLMSDataStracture.FormatData(tempBuffer, false)) / 100);//DVcc voltage
           meterStatusValue.Add(DVccvoltage >= 2.5M ? "PASS" : "FAIL");
           //meterStatusValue.Add(DVccvoltage.ToString("0.00"));
           indexCounter += 2;

           Array.Copy(receivedData, indexCounter, tempBuffer, 0, tempBuffer.Length);
           Array.Reverse(tempBuffer);
           decimal mainBatteryVoltage = (decimal.Parse(DLMSDataStracture.FormatData(tempBuffer, false)) / 100);//Main Battery voltage
           meterStatusValue.Add(mainBatteryVoltage >= 2.5M ? "PASS" : "FAIL");
           indexCounter += 2;

           Array.Copy(receivedData, indexCounter, tempBuffer, 0, tempBuffer.Length);
           Array.Reverse(tempBuffer);
           decimal rtcBatteryVoltage = (decimal.Parse(DLMSDataStracture.FormatData(tempBuffer, false)) / 100);//RTC Battery voltage
           meterStatusValue.Add(rtcBatteryVoltage >= 2.5M ? "PASS" : "FAIL");
           indexCounter += 2;

           //--------Magnet Interrupt Count--------------------------------------
           tempBuffer = new byte[4];
           Array.Copy(receivedData, indexCounter, tempBuffer, 0, tempBuffer.Length);
           Array.Reverse(tempBuffer);
           meterStatusValue.Add(DLMSDataStracture.FormatData(tempBuffer, false));
           indexCounter += 4;
           //-------------Current status of Production Traveller------------------
           RecevData = Convert.ToInt32(receivedData[indexCounter]);
           meterStatusValue.Add(RecevData.ToString());
           indexCounter += 1;

           paracnt = 0;
           string pcbaststus="";
           bool isWorking = false;
           foreach (var item in meterStatusValue)
           {
               if (bool.TryParse(item.ToString(), out isWorking))
               {
                   if ((bool)item == false) pcbaststus += ParametersListValue[paracnt] + ":FAIL," ;
               }
               paracnt++;
           }
           if (pcbaststus.Length > 0 ) pcbaststus = StaticVariables.ERRORPreFix + pcbaststus;
           return pcbaststus + "DVccVoltage=" + DVccvoltage + ",RTCBatteryVoltage=" + rtcBatteryVoltage + ",MainBatteryVoltage=" + mainBatteryVoltage ;

       }

       private string FillPCBAStatusSapphireLTCT(byte[] receivedData)
       {
           string rtcBatteryVoltage = "";         
           string mainBatteryVoltage = "";
           List<string> ParametersListValue = new List<string>(){
            "FLASH 1",        
             "",/*"FLASH 2" - Reserve*/  
            "EEPROM", 
            "UP Button",  
            "Down Button",  
            "MD Button",  
            "",//"FC Button  ",  
            "",//"Comms Card Removal Button  ",  
            "",//"R Phase Relay Malfunction Circuit",
            "",//"Y Phase Relay Malfunction Circuit",
            "",//"B Phase Relay Malfunction Circuit ",  
            "Power Supply",  
            "RTC Battery Low",  
            "RTC Stop",  
            "RTC Bad",  
            "RTC Battery Voltage",
            "",//"Main Supply Voltage",
            "Main Battery Voltage", 
            "",//"Magnetic Sensor - Right",            
            "",//"Cover open Tamper Status"
            "",//"RTC Battery Removal Flag"
           };
           
           float RecevData = 0;
           Int16 TempCounter = 0;

           List<string> statusValue = new List<string>();
           int paracnt = 0;
           while (paracnt++ < ParametersListValue.Count) statusValue.Add("FAIL");
           TempCounter = 20;

           if (receivedData[TempCounter++] == 1) statusValue[0] = "PASS";
           TempCounter++; statusValue[1] = "PASS";//---Reserved
           
           if (receivedData[TempCounter++] == 1) statusValue[2] = "PASS";
           if (receivedData[TempCounter++] == 1) statusValue[3] = "PASS";
           if (receivedData[TempCounter++] == 1) statusValue[4] = "PASS";
           if (receivedData[TempCounter++] == 1) statusValue[5] = "PASS";
           TempCounter++; statusValue[6] = "PASS";
           TempCounter++; statusValue[7] = "PASS";
           //------------Bitwise Reserved---------------
             TempCounter++;
             statusValue[8] = "PASS";
             statusValue[9] = "PASS";
             statusValue[10] = "PASS";
           if (receivedData[TempCounter++] == 1) statusValue[11] = "PASS";
           if (receivedData[TempCounter++] == 1) statusValue[12] = "PASS";
           if (receivedData[TempCounter++] == 1) statusValue[13] = "PASS";
           if (receivedData[TempCounter++] == 1) statusValue[14] = "PASS";

           RecevData = Convert.ToInt32((receivedData[TempCounter] << 8) | receivedData[TempCounter + 1]);
           TempCounter++;
           TempCounter++;
           RecevData /= 1000;
           statusValue[15] = RecevData.ToString();
           rtcBatteryVoltage = "RTC Battery Voltage =" + RecevData.ToString();

            
           TempCounter++;
           TempCounter++;

           statusValue[16] = "PASS";
           

           RecevData = Convert.ToInt32((receivedData[TempCounter] << 8) | receivedData[TempCounter + 1]);
           TempCounter += 2;
           RecevData /= 1000;
           statusValue[17] = RecevData.ToString();
           mainBatteryVoltage = ", Main Battery Voltage =" + RecevData.ToString();

           statusValue[18] = "PASS";
           statusValue[19] = "PASS";
           statusValue[20] = "PASS";
          
           paracnt = 0;
           bool isnotok = false;
           string pcbaststus = string.Empty;
           while (paracnt < statusValue.Count)
           {
               if (ParametersListValue[paracnt].IndexOf("Voltage") >= 0)
               {
                   if (Convert.ToDecimal(statusValue[paracnt]) > 2.5M) statusValue[paracnt] = "PASS";
                   else statusValue[paracnt] = "FAIL";
               }
               if (statusValue[paracnt] == "FAIL") { isnotok = true; pcbaststus += ParametersListValue[paracnt] + ":" + statusValue[paracnt] + ","; }
               paracnt++;
           }
           if (isnotok) pcbaststus = StaticVariables.ERRORPreFix + pcbaststus;
           return pcbaststus + rtcBatteryVoltage + mainBatteryVoltage;
       }

       private string FillPCBAStatus(byte[] receivedData)
       {
           List<string> AnomalyParaList = new List<string>(){
            "FLASH",        
            "EEPROM",            
            "UP Switch",  
            "Down Switch",  
            "MD Switch",               
            "PSM Volt" };
           try
           {
            
           Int16 TempCounter = 0;
           List<string> statusValue = new List<string>();
           int paracnt = 0;
           while (paracnt++ < 6) statusValue.Add("FAIL");
           TempCounter = 20;

           if (receivedData[TempCounter++] == 1) statusValue[0] = "PASS";
           if (receivedData[TempCounter++] == 1) statusValue[1] = "PASS";
           if (receivedData[TempCounter++] == 1) statusValue[2] = "PASS";
           if (receivedData[TempCounter++] == 1) statusValue[3] = "PASS";
           if (receivedData[TempCounter++] == 1) statusValue[4] = "PASS";
           TempCounter++; //FC - Index 25
           TempCounter++; //TC - Index 26
           if (receivedData[TempCounter++] == 1) statusValue[5] = "PASS";
           
 
            paracnt = 0;
           bool isnotok = false;
           string pcbaststus = string.Empty;
           while (paracnt < statusValue.Count)
           {
               
               if (statusValue[paracnt] == "FAIL") { isnotok = true; pcbaststus += AnomalyParaList[paracnt] + ":" + statusValue[paracnt] + ","; }
               paracnt++;
           }
           if (isnotok) pcbaststus = StaticVariables.ERRORPreFix + pcbaststus;
           return pcbaststus;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string IsValidFirmware(byte[] Blockdata, string RefFWVersion)
       {
           try
           {
               string data = string.Empty;
               string[] datavalue = new string[2];
               Application.DoEvents();
               datavalue = DLMSDataStracture.DLMSDataFormator(Blockdata, 18, true);
               if (datavalue == null) { }
               else data = datavalue[0];
               if (data.Length < 3) return StaticVariables.ERRORPreFix + "Invalid Meter FW Version Data!";
               string[] datavaluefw = data.Split('-');//--- case : 1Phase Smart Meter
               string msgmefwv = "";
               string meterfwversion = "";
               if (datavaluefw.Length >= 2)meterfwversion = datavaluefw[1];
               else meterfwversion = datavaluefw[0];

               if (RefFWVersion == meterfwversion) msgmefwv = "Meter FW Version is =" + meterfwversion;
               else msgmefwv = StaticVariables.ERRORPreFix + "Invalid Meter FW Version =" + meterfwversion;               
               return msgmefwv;           
            }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string IsValidFirmware_SmartMeter(byte[] Blockdata, string RefFWVersion)
       {
           try
           {
               string data = string.Empty;
               string[] datavalue = new string[2];
               Application.DoEvents();
               datavalue = DLMSDataStracture.DLMSDataFormator(Blockdata, 18, true);
               if (datavalue == null) { }
               else data = datavalue[0];
               if (data.Length < 4) return StaticVariables.ERRORPreFix + "Invalid Meter FW Version Data!";
               string[] datavaluefw = data.Split('-');//--- case : 1Phase Smart Meter
               string msgmefwv = "";
               string meterfwversion = "";
               if (datavaluefw.Length >= 4) meterfwversion = datavaluefw[3];
               else meterfwversion = datavaluefw[0];

               if (RefFWVersion == meterfwversion) msgmefwv = "Meter FW Version is =" + meterfwversion;
               else msgmefwv = StaticVariables.ERRORPreFix + "Invalid Meter FW Version =" + meterfwversion;
               return msgmefwv;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string IsValidFirmware_MicroStarDLMS(byte[] Blockdata, string RefFWVersion)
       {
           try
           {
               string data = string.Empty;
               string[] datavalue = new string[2];
               Application.DoEvents();
               datavalue = DLMSDataStracture.DLMSDataFormator(Blockdata, 18, true);
               if (datavalue == null) { }
               else data = datavalue[0];
               if (data.Length < 6) return StaticVariables.ERRORPreFix + "Invalid FW Version Data!";
               string datavaluefw = data ;
               if (!datavaluefw.Contains(StaticVariables.MeterType_EcoStar)) { return StaticVariables.ERRORPreFix + "Invalid Meter Type =" + datavaluefw; }
               datavaluefw = datavaluefw.Substring(datavaluefw.IndexOf(StaticVariables.MeterType_EcoStar) + (StaticVariables.MeterType_EcoStar.Length+1));
               string mefwv = "";
               if (datavaluefw.Length >= 2)
               {
                   if (RefFWVersion == datavaluefw) mefwv = "FW Version is =" + datavaluefw;
                   else mefwv = StaticVariables.ERRORPreFix + "Invalid FW Version =" + datavaluefw;
               }
               return mefwv;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
               
       public string CheckingDLMSFirmwareVersion(string RefFWVersion)
       {
           try
           {
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.MeterFWVersionDataStracture.MeterFWVersionOBIS, txtboxobject, "0", 100M, DLMSDataStracture.MeterFWVersionDataStracture.MeterFWVersionClassID, DLMSDataStracture.MeterFWVersionDataStracture.MeterFWVersionValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return (IsValidDLMSFirmware(GlobalObjects.objSerialComm.ReceiveBuffer, RefFWVersion));
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }
     
       public string IsValidDLMSFirmware(byte[] Blockdata, string RefFWVersion)
       {
           string data = string.Empty;
           string[] datavalue = new string[2];
           Application.DoEvents();
           datavalue = DLMSDataStracture.DLMSDataFormator(Blockdata, 18, true);
           if (datavalue == null) { }
           else data = datavalue[0];
           if (data.Length < 2) return StaticVariables.ERRORPreFix + "Invalid DLMS FW Version Data!";
           string mefwv = "DLMS FW Version is =" + data;
           if (data == RefFWVersion) return mefwv;
           return StaticVariables.ERRORPreFix + mefwv;

       }

       public string VerifyMeterLock()
       {
           try
           {
               byte lockByte = 0xFF;
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.MeterSoftwareLockDataStracture.MeterSoftwareLockOBIS, txtboxobject, "0", 100M, DLMSDataStracture.MeterSoftwareLockDataStracture.MeterSoftwareLockClassID, DLMSDataStracture.MeterSoftwareLockDataStracture.MeterSoftwareLockValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return (IsMatchMeterLockValue(GlobalObjects.objSerialComm.ReceiveBuffer, lockByte));
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }

       private string IsMatchMeterLockValue(byte[] receivedData, byte lockByte)
       {
           try
           {
               int startDataindx = 18;
               byte configByte;
               if (receivedData[startDataindx++] == 0x11) //srtact
               {
                   configByte = receivedData[startDataindx++];
                   if (configByte == 0xFF) { return  "Verified Meter Locked !"; }
                   else { return "Verification " + StaticVariables.ERRORPreFix + " Meter Not Locked! "; }
               }
               else { return StaticVariables.ERRORPreFix + " Unable to parse received data";  }
           }
           catch (Exception ex)
           {
              return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string VerifyMRPassword(string RefMRPassword)
       {
           try
           {
              // byte[] OBISCode = DLMSDataStracture.LLSKeyDataStracture.LLSKeyOBIS; //---Old 
               byte[] OBISCode = DLMSDataStracture.LNAssociationDataStracture.LNAssociationOBIS_MR;//---OBIS is modified for Smart meter falcon2 & same are working for Non AMI DLMS, Old OBIS is : DLMSDataStracture.LLSKeyDataStracture.LLSKeyOBIS
               if (!objLI.ReadByteFromMeter(OBISCode, txtboxobject, "0", 100M, DLMSDataStracture.LLSKeyDataStracture.LLSKeyClassID, DLMSDataStracture.LLSKeyDataStracture.LLSKeyValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return (IsValidMRPassword(GlobalObjects.objSerialComm.ReceiveBuffer, RefMRPassword));
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }

       public string IsValidMRPassword(byte[] Blockdata, string RefMRPassword)
       {
           string data = string.Empty;
           string[] datavalue = new string[2];
           Application.DoEvents();
           datavalue = DLMSDataStracture.DLMSDataFormator(Blockdata, 18, true);
           if (datavalue == null) { }
           else data = datavalue[0];
           if (data.Length < 2) return StaticVariables.ERRORPreFix + "MR Password : Invalid";           
           if (data == RefMRPassword) return "MR Password =" + data; ;
           return StaticVariables.ERRORPreFix + "MR Password Failed =" + data; ;

       }

       public string VerifyUSPassword(string RefPassword)
       {
           byte oldSecurityMechanism = objappSettings.GetSecurityMachanism();
           string oldClientSAP = objappSettings.GetClientSAP();
           byte oldappContext = objappSettings.GetApplicationContext();
           string oldLLS = objappSettings.GetLLSPassword();
           string oldHLS = objappSettings.GetHLSPassword();
           string testCaseMessage = string.Empty;
           try
           {
               string[] refPWD = RefPassword.Split(',');
               string RefUSPassword = "32323232323232323232323232323232"; //Default US Password in 16 Byte HES
               string RefFUPassword = "0102030405060708090A0B0C0D0E0F"; //Default FW Upgrade mode Password in 16 Byte HES
               string refMRPassword = "11111111"; //Default MR Password in 8 Byte ASCII
               if (refPWD.Length >= 1) RefUSPassword = refPWD[0];
               if (refPWD.Length >= 2) refMRPassword = refPWD[1];
               if (refPWD.Length >= 3) RefFUPassword = refPWD[2];
               
               //======================Verify US mode Password================================================
               if (!VerifyMeterAssociation(RefUSPassword, refMRPassword, 0x02, 48)) testCaseMessage = " US: Failed,"; // return StaticVariables.ERRORPreFix + " US Association: Failed , MR Password: Not Verified.";
               else testCaseMessage = "US,"; 
               //======================Verify FU mode Password================================================
               if (objappSettings.GetMeterMode() == (int)StaticVariables.ExecutedMeterType.Smart_Meter_3PH || objappSettings.GetMeterMode() == (int)StaticVariables.ExecutedMeterType.Smart_Meter_1PH)
               {
                   if (!VerifyMeterAssociation(RefFUPassword, refMRPassword, 0x02, 80)) testCaseMessage += "FU: Failed,"; // return StaticVariables.ERRORPreFix + " FU Association: Failed , MR Password: Not Verified.";
                   else testCaseMessage += "FU,"; 
               }
               //======================Verify MR mode Password================================================
               if (!VerifyMeterAssociation(RefUSPassword, refMRPassword, 0x01, 32)) testCaseMessage += "MR: Failed,"; // return StaticVariables.ERRORPreFix + " MR Association: Failed.";
               else testCaseMessage += "MR,";  
               //======================Restore Previous Association i.e. FS mode (WO Ciphering)==========================
               objLI.AssociationDisconnect();
               Application.DoEvents();
               System.Threading.Thread.Sleep(100);
               objappSettings.SetSecurityMachanism(oldSecurityMechanism);
               objappSettings.SetClientSAP(Convert.ToInt16(oldClientSAP));
               objappSettings.SetApplicationContext(oldappContext);
               objappSettings.SetLLSPassword(oldLLS);
               objappSettings.SetHLSPWD(oldHLS);
               if (!objLI.ConnectToMeter()) { objLI.AssociationDisconnect(); testCaseMessage += "FS Restore: Failed";/* return StaticVariables.ERRORPreFix + "COMM Failed. >> Unable To Restore Default Association."; */}
               else testCaseMessage += "FS";

               if(testCaseMessage.Contains("Failed"))testCaseMessage =StaticVariables.ERRORPreFix + testCaseMessage;
               else testCaseMessage += " - Association Verified.";
               return testCaseMessage;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
           finally
           {
               objappSettings.SetLLSPassword(oldLLS);
               objappSettings.SetHLSPWD(oldHLS);
               objappSettings.SetSecurityMachanism(oldSecurityMechanism);
               objappSettings.SetClientSAP(Convert.ToInt16(oldClientSAP));
               objappSettings.SetApplicationContext(oldappContext);
           }
             
       }
       public bool VerifyMeterAssociation(string RefHLSPassword,string refLLSPassword, byte refSecurityMachanism, int refClientSAP)
       {
            objLI.IsMeterConnected = true;
            objLI.AssociationDisconnect();
           
            if (objappSettings.GetMeterMode() == (int)StaticVariables.ExecutedMeterType.Smart_Meter_3PH || objappSettings.GetMeterMode() == (int)StaticVariables.ExecutedMeterType.Smart_Meter_1PH)
            {
                objappSettings.SetCipheredSecurityResponse(refLLSPassword, RefHLSPassword, objappSettings.GetGlobalEncryptionKey());
            }
            objappSettings.SetHLSPWD(RefHLSPassword);
            objappSettings.SetLLSPassword(refLLSPassword);
            objappSettings.SetSecurityMachanism(refSecurityMachanism);
            objappSettings.SetClientSAP(refClientSAP);
            Application.DoEvents();
            System.Threading.Thread.Sleep(100);
            if (!objLI.ConnectToMeter())
            {
                //----Incase of Association Failed in FU mode, try disconnect for 3 times, if disconnect success then retry FU aassociation again otherwise return as FAIL.
                if (!objLI.AssociationDisconnect())
                {
                    if (!objLI.AssociationDisconnect()) { objLI.AssociationDisconnect(); }
                }
                objappSettings.SetHLSPWD(RefHLSPassword);
                objappSettings.SetLLSPassword(refLLSPassword);
                objappSettings.SetSecurityMachanism(refSecurityMachanism);
                objappSettings.SetClientSAP(refClientSAP);
                if (!objLI.ConnectToMeter())
                {
                    return false;
                }
            }
            return true;
       }
       /// <summary>
       /// Not in use, Was used in Falcon1 : dt. 31.10.2018
       /// </summary>
       /// <returns></returns>
       public string RelayCircuitTest()
       {
           try
           {
               
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ConnectControlDataStracture.ConnectControlOBIS, txtboxobject, "0", 1M, DLMSDataStracture.ConnectControlDataStracture.ConnectControlClassID, DLMSDataStracture.ConnectControlDataStracture.DisconnectControlControlStateValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               string readflg = GlobalObjects.objSerialComm.ReceiveBuffer[19].ToString();
               Application.DoEvents();
               if (readflg == "0") //---------- Disconnected state connect and read
               {
                   if (!objLI.WriteDataToMeter(DLMSDataStracture.ConnectControlDataStracture.ConnectControlValueAttribute, DLMSDataStracture.ConnectControlDataStracture.ConnectControlOBIS, DLMSDataStracture.ConnectControlDataStracture.ConnectControlClassID, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataType, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataLength, GetDisconnectControlBytes(), DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               }
               else if (GlobalObjects.objSerialComm.ReceiveBuffer[19].ToString() == "1") //----------Connected state disconnect and read
               {
                   if (!objLI.WriteDataToMeter(DLMSDataStracture.ConnectControlDataStracture.DisconnectControlValueAttribute, DLMSDataStracture.ConnectControlDataStracture.ConnectControlOBIS, DLMSDataStracture.ConnectControlDataStracture.ConnectControlClassID, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataType, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataLength, GetDisconnectControlBytes(), DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               }
               CommandExecutionWaitTimer(500);
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ConnectControlDataStracture.ConnectControlOBIS, txtboxobject, "0", 1M, DLMSDataStracture.ConnectControlDataStracture.ConnectControlClassID, DLMSDataStracture.ConnectControlDataStracture.DisconnectControlControlStateValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               string readflgNext = GlobalObjects.objSerialComm.ReceiveBuffer[19].ToString();
               if (readflgNext != readflg)
               {
                   if (readflgNext == "0")
                   {
                      CommandExecutionWaitTimer(4000);
                      if (!objLI.WriteDataToMeter(DLMSDataStracture.ConnectControlDataStracture.ConnectControlValueAttribute, DLMSDataStracture.ConnectControlDataStracture.ConnectControlOBIS, DLMSDataStracture.ConnectControlDataStracture.ConnectControlClassID, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataType, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataLength, GetDisconnectControlBytes(), DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
                   }
                   return "Before and After Flag are Same.";
               }
               else
               {
                   return StaticVariables.ERRORPreFix + "Before and After Flag are Not Same.";

               }
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       /// <summary>
       /// Method for 3Phase Smart meter only, Relay Circuit test
       /// </summary>
       /// <param name="refdefault"></param>
       /// <param name="refMin"></param>
       /// <param name="refMax"></param>
       /// <returns></returns>
       public string RelayCircuitTest_3Phase(string refdefault,string refMin,string refMax)
       {
           try
           {
               CommandExecutionWaitTimer(10000);
               //---------------------Disconnect & Read V--------------------------------------------------------
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ConnectControlDataStracture.DisconnectControlValueAttribute, DLMSDataStracture.ConnectControlDataStracture.ConnectControlOBIS, DLMSDataStracture.ConnectControlDataStracture.ConnectControlClassID, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataType, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataLength, GetDisconnectControlBytes(), DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) 
               { 
                   //--If disconnect command Fail then try to connect and again disconnect to Ensure the actual relay faulty----------------
                   CommandExecutionWaitTimer(10000);
                   if (!objLI.WriteDataToMeter(DLMSDataStracture.ConnectControlDataStracture.ConnectControlValueAttribute, DLMSDataStracture.ConnectControlDataStracture.ConnectControlOBIS, DLMSDataStracture.ConnectControlDataStracture.ConnectControlClassID, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataType, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataLength, GetDisconnectControlBytes(), DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { /*---No Return--*/}
                   CommandExecutionWaitTimer(10000);
                   if (!objLI.WriteDataToMeter(DLMSDataStracture.ConnectControlDataStracture.DisconnectControlValueAttribute, DLMSDataStracture.ConnectControlDataStracture.ConnectControlOBIS, DLMSDataStracture.ConnectControlDataStracture.ConnectControlClassID, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataType, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataLength, GetDisconnectControlBytes(), DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { /*---No Return--*/}
               }
               CommandExecutionWaitTimer(10000);//new delay
               //---------------------Read Verify Voltage in Disconnected mode--------------------------------------------------------
              string VValue = RelayWireSwapTest(refdefault, refMin, refMax);
              if (VValue.IndexOf(StaticVariables.ERRORPreFix) >= 0)
               {
                   MessageBox.Show("Phase Wire Not Connected !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                   return VValue;
               }
              //---------------------Read and Verify V I PF in Disconnected mode--------------------------------------------------------
               //CommandExecutionWaitTimer(1000);
               //MessageBox.Show("Switch ON Current ! Then Hit OK To Continue !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               //Application.DoEvents();
               //CommandExecutionWaitTimer(1000);
               string VIPFValueDis = "Dis =" + ReadVIPF_RelayTest(false,refdefault,refMin,refMax);
               if (VIPFValueDis.IndexOf(StaticVariables.ERRORPreFix) >= 0)
               {
                   MessageBox.Show("Relay Malfunction !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                   return VIPFValueDis;
               }
               //---------------------Read and Verify V I PF in in Connected mode--------------------------------------------------------
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ConnectControlDataStracture.ConnectControlValueAttribute, DLMSDataStracture.ConnectControlDataStracture.ConnectControlOBIS, DLMSDataStracture.ConnectControlDataStracture.ConnectControlClassID, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataType, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataLength, GetDisconnectControlBytes(), DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               CommandExecutionWaitTimer(2000);
               string VIPFValueCon = "Con =" + ReadVIPF_RelayTest(true,refdefault,refMin,refMax);
               if (VIPFValueCon.IndexOf(StaticVariables.ERRORPreFix) >= 0)
               {
                   MessageBox.Show("CT Wire Swaped !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
               }
               return VIPFValueCon + VIPFValueDis + VValue;
                
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       /// <summary>
       /// Method for 1Phase Smart meter only, relay Circuit test
       /// </summary>
       /// <param name="refdefault"></param>
       /// <param name="refMin"></param>
       /// <param name="refMax"></param>
       /// <returns></returns>
       public string RelayCircuitTest_1Phase(string refdefault, string refMin, string refMax)
       {
           try
           {
               MessageBox.Show("Press Phase Current Switch And Hit OK To Continue.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               Application.DoEvents();
               //CommandExecutionWaitTimer(10000);
               //---------------------Disconnect--------------------------------------------------------
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ConnectControlDataStracture.DisconnectControlValueAttribute, DLMSDataStracture.ConnectControlDataStracture.ConnectControlOBIS, DLMSDataStracture.ConnectControlDataStracture.ConnectControlClassID, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataType, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataLength, GetDisconnectControlBytes(), DLMSDataStracture.DataStractureRequest.ActionRequest_Normal))
               {
                   //--If disconnect command Fail then try to connect and again disconnect to Ensure the actual relay faulty----------------
                   //CommandExecutionWaitTimer(5000);
                   if (!objLI.WriteDataToMeter(DLMSDataStracture.ConnectControlDataStracture.ConnectControlValueAttribute, DLMSDataStracture.ConnectControlDataStracture.ConnectControlOBIS, DLMSDataStracture.ConnectControlDataStracture.ConnectControlClassID, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataType, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataLength, GetDisconnectControlBytes(), DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { /*---No Return--*/}
                   CommandExecutionWaitTimer(5000);
                   if (!objLI.WriteDataToMeter(DLMSDataStracture.ConnectControlDataStracture.DisconnectControlValueAttribute, DLMSDataStracture.ConnectControlDataStracture.ConnectControlOBIS, DLMSDataStracture.ConnectControlDataStracture.ConnectControlClassID, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataType, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataLength, GetDisconnectControlBytes(), DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { /*---No Return--*/}
               }
               CommandExecutionWaitTimer(10000);
               //--Verify Meter Current Value, Ph& Neu Current,it should be <=0 incase of relay disconnected---------------------------------------------
               string CurrentValueDis = "Dis: " + ReadandVerifyCurrentFromBufferSinglePhaseFalcon("", "", "0.0");  
               if (CurrentValueDis.IndexOf(StaticVariables.ERRORPreFix) >= 0)
               {
                   MessageBox.Show("Realy Dis-Connection Issue !" + "\n" + "Current Detected After Disconnect.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                   return CurrentValueDis;
               }
               //-------------------------------Connect Relay and verify Ph Current Value, It should be within range-------------------------------------
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ConnectControlDataStracture.ConnectControlValueAttribute, DLMSDataStracture.ConnectControlDataStracture.ConnectControlOBIS, DLMSDataStracture.ConnectControlDataStracture.ConnectControlClassID, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataType, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataLength, GetDisconnectControlBytes(), DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               CommandExecutionWaitTimer(2000);
               //string CurrentValueConn = "  Con: Ph=" + ReadandVerifyMeterBufferIndivisualFlagSinglePhaseFalcon(1, refdefault, refMin, refMax);//--Phase Current
               string CurrentValueConn =", Con: " + MetrologyTest_1PhaseFalcon(refdefault, refMin, refMax, (int)StaticVariables.MMITestParameters.PhaseCurrentTest);
               if (CurrentValueConn.IndexOf(StaticVariables.ERRORPreFix) >= 0)
               {
                   MessageBox.Show("Realy Connection issue !" + "\n" + "Metrology Error in Phase.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                   return CurrentValueDis + CurrentValueConn;;
               }
               //------------------------------------Read and verify Neu Current Value, It should be within range ----------------------------------------
               MessageBox.Show("Press Neu Current Switch And Hit OK To Continue.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               Application.DoEvents();

               CommandExecutionWaitTimer(2000);
               CurrentValueConn += ", " + MetrologyTest_1PhaseFalcon(refdefault, refMin, refMax, (int)StaticVariables.MMITestParameters.NeutralCurrentTest);
               if (CurrentValueConn.IndexOf(StaticVariables.ERRORPreFix) >= 0)
               {
                   MessageBox.Show("Realy Connection issue !" + "\n" + "Metrology Error in Neutral.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
               }
               return CurrentValueDis + CurrentValueConn;

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string RelayCircuitConnect()
       {
           try
           {
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ConnectControlDataStracture.ConnectControlValueAttribute, DLMSDataStracture.ConnectControlDataStracture.ConnectControlOBIS, DLMSDataStracture.ConnectControlDataStracture.ConnectControlClassID, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataType, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataLength, GetDisconnectControlBytes(), DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "Unable To Connect"; }
               return "Relay Status: Connected";

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public string RelayCircuitDisConnect()
       {
           try
           {
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ConnectControlDataStracture.DisconnectControlValueAttribute, DLMSDataStracture.ConnectControlDataStracture.ConnectControlOBIS, DLMSDataStracture.ConnectControlDataStracture.ConnectControlClassID, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataType, DLMSDataStracture.ConnectControlDataStracture.ConnectControlDataLength, GetDisconnectControlBytes(), DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "Unable To Dis-Connect"; }
               return "Relay Status: Dis-Connected";

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       // strvalue[0]-->Ref Voltage  //strvalue[1]-->Ref Phase Current // strvalue[2]-->Ref Neu Current //strvalue[9]-->Ref Frequency
       private string ReadandVerifyCurrentFromBufferSinglePhaseFalcon( string refdefault, string refMin, string refMax)
       {
           string phasecurrent="";
           string NeuCurrent="";
           if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferClassID, DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
           List<string> strvalue = FillMeterBufferStructure(GlobalObjects.objSerialComm.ReceiveBuffer);
           if (strvalue[1].IndexOf('.') >= 0) phasecurrent = strvalue[1].Substring(0, strvalue[1].IndexOf('.') + 2);
           else phasecurrent = strvalue[1];
           if (strvalue[2].IndexOf('.') >= 0) NeuCurrent = strvalue[2].Substring(0, strvalue[2].IndexOf('.') + 2);
           else NeuCurrent=strvalue[2];
           string currentVal=" Ph I=" +  objcomnMethod.CheckingRangeValueForDecimal("", refdefault, refMin, refMax, phasecurrent);//--Phase Current
           currentVal += ", Neu I=" + objcomnMethod.CheckingRangeValueForDecimal("", refdefault, refMin, refMax, NeuCurrent);//--Neu Current

           return currentVal;
       }
       public string ReadVIPF_RelayTest(bool isConnected, string refdefault, string refMin, string refMax)
       {
           try
           {                 
                string[] refDefaultList = refdefault.Split(',');
                string[] refMinList = refMin.Split(',');
                string[] refMaxList = refMax.Split(',');

                string refdefVoltage = "";
                string refdefCurrent = "";
                string refdefPF = "";
                string refminVoltage = "";
                string refminCurrent = "";
                string refminPF = "";
                string refmaxVoltage = "";
                string refmaxCurrent = "";
                string refmaxPF = "";
                if (refDefaultList.Length >= 2) { refdefVoltage = refDefaultList[0]; refdefCurrent = refDefaultList[1]; refdefPF = refDefaultList[2]; };
                if (refMinList.Length >= 2) { refminVoltage = refMinList[0]; refminCurrent = refMinList[1]; refminPF = refMinList[2]; };
                if (refMaxList.Length >= 2) { refmaxVoltage = refMaxList[0]; refmaxCurrent = refMaxList[1]; refmaxPF = refMaxList[2]; };

                string readValuesPara=string.Empty;
                List<string> datavalue = new List<string>();
                List<string> datavalueList = new List<string>();

                datavalue.Add(ReadFormattedValue(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.VoltageOBIS_RPhase,DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID));
                datavalue.Add(ReadFormattedValue(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.VoltageOBIS_YPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID));
                datavalue.Add(ReadFormattedValue(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.VoltageOBIS_BPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID));

                datavalue.Add(ReadFormattedValue(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.CurrentOBIS_RPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID));
                datavalue.Add(ReadFormattedValue(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.CurrentOBIS_YPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID));
                datavalue.Add(ReadFormattedValue(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.CurrentOBIS_BPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID));

                datavalue.Add(ReadFormattedValue(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.PFOBIS_RPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID));
                datavalue.Add(ReadFormattedValue(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.PFOBIS_YPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID));
                datavalue.Add(ReadFormattedValue(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.PFOBIS_BPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID));
             
                string tempRes= "";
                if (isConnected)
                {
                    int valCnt = 0;
                    while (valCnt < datavalue.Count)
                    {

                        if (valCnt < 3) tempRes = objcomnMethod.CheckingRangeValueForDecimal("", refdefVoltage, refminVoltage, refmaxVoltage, datavalue[valCnt]);
                        else if (valCnt >= 3 && valCnt < 6) tempRes = objcomnMethod.CheckingRangeValueForDecimal("", refdefCurrent, refminCurrent, refmaxCurrent, datavalue[valCnt]);
                        else if (valCnt >= 6 && valCnt < 9) tempRes = objcomnMethod.CheckingRangeValueForDecimal("", refdefPF, refminPF, refmaxPF, datavalue[valCnt]);

                        readValuesPara += tempRes + ",";
                        valCnt++;
                    }
                }
                else
                {
                    int valCnt = 0;
                    while (valCnt < datavalue.Count)
                    {

                        if (valCnt < 3) tempRes = objcomnMethod.CheckingRangeValueForDecimal("", refdefVoltage, refminVoltage, refmaxVoltage, datavalue[valCnt]);
                            else if (valCnt >= 3 && valCnt < 6) tempRes = objcomnMethod.CheckingRangeValueForDecimal("", "", "0", "0", datavalue[valCnt]);
                            else if (valCnt >= 6 && valCnt < 9) tempRes = objcomnMethod.CheckingRangeValueForDecimal("", "", "0", "0", datavalue[valCnt]);

                        readValuesPara += tempRes + ",";
                        valCnt++;
                    }
                }
                return readValuesPara;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string RelayWireSwapTest(string refdefault, string refMin, string refMax)
       {
           try
           {
               string tempRes = "";
               int valCnt = 0;

               string[] refDefaultList = refdefault.Split(',');
               string[] refMinList = refMin.Split(',');
               string[] refMaxList = refMax.Split(',');

               string refdefVoltage = "";
               string refdefCurrent = "";
               string refdefPF = "";
               string refminVoltage = "";
               string refminCurrent = "";
               string refminPF = "";
               string refmaxVoltage = "";
               string refmaxCurrent = "";
               string refmaxPF = "";
               if (refDefaultList.Length >= 2) { refdefVoltage = refDefaultList[0]; refdefCurrent = refDefaultList[1]; refdefPF = refDefaultList[2]; };
               if (refMinList.Length >= 2) { refminVoltage = refMinList[0]; refminCurrent = refMinList[1]; refminPF = refMinList[2]; };
               if (refMaxList.Length >= 2) { refmaxVoltage = refMaxList[0]; refmaxCurrent = refMaxList[1]; refmaxPF = refMaxList[2]; };
               
               string readValuesPara = string.Empty;
               List<string> datavalue = new List<string>();
               List<string> datavalueList = new List<string>();

               datavalue.Add(ReadFormattedValue(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.VoltageOBIS_RPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID));
               datavalue.Add(ReadFormattedValue(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.VoltageOBIS_YPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID));
               datavalue.Add(ReadFormattedValue(DLMSDataStracture.EngineeringCommandDataStracture_3Phase.VoltageOBIS_BPhase, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID));
               while (valCnt < datavalue.Count)
               {
                   tempRes = objcomnMethod.CheckingRangeValueForDecimal("", refdefVoltage, refminVoltage, refmaxVoltage, datavalue[valCnt]);
                   readValuesPara += tempRes + ",";
                   valCnt++;
               }               
               return "Phase Wire =" + readValuesPara;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private string ReadFormattedValue(byte[] meterStatusOBIS, byte objectclassID)
       {
           if (!objLI.ReadByteFromMeter(meterStatusOBIS, txtboxobject, "0", 100M, objectclassID, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_Attribute_Value)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
           List<string> datavalue = FormatIndivisualData(GlobalObjects.objSerialComm.ReceiveBuffer);
           if (!objLI.ReadByteFromMeter(meterStatusOBIS, txtboxobject, "0", 100M, objectclassID, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_Attribute_Scalar)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
           string formattedValue = ApplyScalarUnits(GlobalObjects.objSerialComm.ReceiveBuffer, datavalue[0]);
           return formattedValue;
       }

       private void CommandExecutionWaitTimer(Int64 waitduration)
       {
           while (waitduration > 0)
           {
               Application.DoEvents();
               System.Threading.Thread.Sleep(100);
               waitduration -= 100;
           }
       }

       private List<byte> GetDisconnectControlBytes()
       {
           int disconnectStst = Convert.ToInt32("00");
           List<byte> disconnectStstbyte = new List<byte>();
           disconnectStstbyte.Add(Convert.ToByte(disconnectStst));
           return disconnectStstbyte;
       }
       /// <summary>
       /// NON AMI Meter Set RTC
       /// </summary>
       /// <param name="IsValidWeekDay"></param> 
       /// <param name="clockStatus"></param> // Value of clock status will be 0xFF for sapphire S2 and 0x00 for others
       /// <param name="diffSeconds"></param>
       /// <returns></returns>
       public string SetMeterRTC(bool IsValidWeekDay, byte clockStatus, int diffSeconds)
       {
           int defcmdtimeout = GlobalObjects.objSerialComm.CommandTimeout;
           try
           {
               GlobalObjects.objSerialComm.CommandTimeout = 30000;//----All Data Resets beacuse of DLMS Amendment-3 IF RTC drift > IP then meter will reset All Data and response time will increase
               if (!objLI.WriteDataToMeter(DLMSDataStracture.MeterRTCDataStracture.MeterRTCValueAttribute, DLMSDataStracture.MeterRTCDataStracture.MeterRTCOBIS, DLMSDataStracture.MeterRTCDataStracture.MeterRTCClassID, DLMSDataStracture.MeterRTCDataStracture.MeterRTCDataType, DLMSDataStracture.MeterRTCDataStracture.MeterRTCDataLength, GetRTCBytes(IsValidWeekDay, clockStatus, diffSeconds), DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
           finally
           {
               GlobalObjects.objSerialComm.CommandTimeout = defcmdtimeout;
           }

       }

       //public string SetMeterRTC(bool IsValidWeekDay, int diffSeconds)
       //{
       //    int defcmdtimeout = GlobalObjects.objSerialComm.CommandTimeout;
       //    try
       //    {
       //        GlobalObjects.objSerialComm.CommandTimeout = 30000;//----All Data Resets beacuse of DLMS Amendment-3 IF RTC drift > IP then meter will reset All Data and response time will increase
       //        if (!objLI.WriteDataToMeter(DLMSDataStracture.MeterRTCDataStracture.MeterRTCValueAttribute, DLMSDataStracture.MeterRTCDataStracture.MeterRTCOBIS, DLMSDataStracture.MeterRTCDataStracture.MeterRTCClassID, DLMSDataStracture.MeterRTCDataStracture.MeterRTCDataType, DLMSDataStracture.MeterRTCDataStracture.MeterRTCDataLength, GetRTCBytes(IsValidWeekDay, diffSeconds), DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
       //        return "";
       //    }
       //    catch (Exception ex)
       //    {
       //        return StaticVariables.ERRORPreFix + ex.Message;
       //    }
       //    finally
       //    {
       //        GlobalObjects.objSerialComm.CommandTimeout = defcmdtimeout;
       //    }

       //}

       public string SetMeterRTCFalcon2SM()
       {
           int defcmdtimeout = GlobalObjects.objSerialComm.CommandTimeout;
           try
           {
               GlobalObjects.objSerialComm.CommandTimeout = 30000;//----All Data Resets beacuse of DLMS Amendment-3 IF RTC drift > IP then meter will reset All Data and response time will increase
               if (!objLI.WriteDataToMeter(DLMSDataStracture.MeterRTCDataStracture.MeterRTCValueAttribute, DLMSDataStracture.MeterRTCDataStracture.MeterRTCOBIS, DLMSDataStracture.MeterRTCDataStracture.MeterRTCClassID, DLMSDataStracture.MeterRTCDataStracture.MeterRTCDataType, DLMSDataStracture.MeterRTCDataStracture.MeterRTCDataLength, GetRTCBytesFalcon2SM(), DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
           finally
           {
               GlobalObjects.objSerialComm.CommandTimeout = defcmdtimeout;
           }

       }            

       private List<byte> GetRTCBytesFalcon2SM()
       {

           List<byte> RTCbyte = new List<byte>();

           DateTime.Today.ToString("MM/dd/yyyy");
           DateTime setdatetime = System.DateTime.Now.AddSeconds(1);
            
           RTCbyte.Add(Convert.ToByte((setdatetime.Year & 0xFF00) >> 8));
           RTCbyte.Add(Convert.ToByte(setdatetime.Year & 0x00FF));

           RTCbyte.Add(Convert.ToByte(setdatetime.Month));
           RTCbyte.Add(Convert.ToByte(setdatetime.Day));
          
           if (setdatetime.DayOfWeek == DayOfWeek.Sunday) RTCbyte.Add(0x07);
           else RTCbyte.Add(Convert.ToByte(setdatetime.DayOfWeek));              
           
           RTCbyte.Add(Convert.ToByte(setdatetime.Hour));
           RTCbyte.Add(Convert.ToByte(setdatetime.Minute));
           RTCbyte.Add(Convert.ToByte(setdatetime.Second));

           RTCbyte.Add(0xFF);
           RTCbyte.Add(0x80);
           RTCbyte.Add(0x00);
           RTCbyte.Add(0xFF);
           return RTCbyte;
       }

       private List<byte> GetRTCBytes(bool IsValidWeekDay, byte clockStatus, int diffSeconds)
       {

            List<byte> RTCbyte = new List<byte>();

            DateTime.Today.ToString("MM/dd/yyyy");
            DateTime setdatetime = System.DateTime.Now.AddSeconds(1 + diffSeconds);

            RTCbyte.Add(Convert.ToByte((setdatetime.Year & 0xFF00) >> 8));
            RTCbyte.Add(Convert.ToByte(setdatetime.Year & 0x00FF));

            RTCbyte.Add(Convert.ToByte(setdatetime.Month));
            RTCbyte.Add(Convert.ToByte(setdatetime.Day));
            RTC_ISFormat rtc = ApplicationInterface.GenericRTC.ClockWRITEBytes(objappSettings.GetMeterMode(), setdatetime.Year, setdatetime.Month, setdatetime.Day, setdatetime.Hour, setdatetime.Minute);
            RTCbyte.Add(rtc.dayofweek);
            RTCbyte.Add(Convert.ToByte(setdatetime.Hour));
            RTCbyte.Add(Convert.ToByte(setdatetime.Minute));
            RTCbyte.Add(Convert.ToByte(setdatetime.Second));

            RTCbyte.Add(0xFF);
            RTCbyte.Add(0x80);
            RTCbyte.Add(0x00);
            RTCbyte.Add(rtc.clockstatus);
            return RTCbyte;
           /*
           if (!IsValidWeekDay) RTCbyte.Add(0xff);//----Condition is applicable For 3Phase DLMS & Sapphire Meter
           else
           {
               if (setdatetime.DayOfWeek == DayOfWeek.Sunday) RTCbyte.Add(0x07);
               else RTCbyte.Add(Convert.ToByte(setdatetime.DayOfWeek));
           }
           RTCbyte.Add(Convert.ToByte(setdatetime.Hour));
           RTCbyte.Add(Convert.ToByte(setdatetime.Minute));
           RTCbyte.Add(Convert.ToByte(setdatetime.Second));

           RTCbyte.Add(0xFF);
           RTCbyte.Add(0x80);
           RTCbyte.Add(0x00);
           RTCbyte.Add(clockStatus); //if Sapphire S2 it will be 0xFF else 0x00;
           return RTCbyte;
            */
       }

       //private List<byte> GetRTCBytes(bool IsValidWeekDay, int diffSeconds)
       //{

       //    List<byte> RTCbyte = new List<byte>();

       //    DateTime.Today.ToString("MM/dd/yyyy");
       //    DateTime setdatetime = System.DateTime.Now.AddSeconds(1+diffSeconds);

       //    RTCbyte.Add(Convert.ToByte((setdatetime.Year & 0xFF00) >> 8));
       //    RTCbyte.Add(Convert.ToByte(setdatetime.Year & 0x00FF));

       //    RTCbyte.Add(Convert.ToByte(setdatetime.Month));
       //    RTCbyte.Add(Convert.ToByte(setdatetime.Day));

       //    if (!IsValidWeekDay) RTCbyte.Add(0xff);//----Condition is applicable For 3Phase DLMS & Sapphire Meter
       //    else
       //    {
       //        if (setdatetime.DayOfWeek == DayOfWeek.Sunday) RTCbyte.Add(0x07);
       //        else RTCbyte.Add(Convert.ToByte(setdatetime.DayOfWeek));
       //    }
       //    RTCbyte.Add(Convert.ToByte(setdatetime.Hour));
       //    RTCbyte.Add(Convert.ToByte(setdatetime.Minute));
       //    RTCbyte.Add(Convert.ToByte(setdatetime.Second));

       //    RTCbyte.Add(0xFF);
       //    RTCbyte.Add(0x80);
       //    RTCbyte.Add(0x00);
       //    RTCbyte.Add(0x00);
       //    return RTCbyte;
       //}

       public string ReadTamperCompartment(byte[] OBIS, string[] ReferenceValue,string TamperType)
       {
           try
           {
               TextBox[] lstboxobject = new TextBox[] { };
               if (!objLI.ReadBlockFromMeter(OBIS, lstboxobject, "0", 1M, DLMSDataStracture.ReadoutDataStracture.ReadoutClassID, DLMSDataStracture.ReadoutDataStracture.ReadoutValueAttribute_Data, DLMSDataStracture.DataStractureAccessSelector.Null_descriptor, null)) 
               {
                   if (objLI.errormsgStstus == (int)LayerInterface.ProgrammingCode.DataUnavailable) return "Data Not Available";
                   else if (objLI.errormsgStstus != (int)LayerInterface.ProgrammingCode.Success) return StaticVariables.ERRORPreFix + "COMM Failed."; 
               }
               string hexValue = BitConverter.ToString(GlobalObjects.objCOSEMLIB.BlockBuffer, 0, GlobalObjects.objCOSEMLIB.nBlockTotalByteCount).Replace("-", String.Empty);
               bool ismatchFound=false;
               foreach (string item in ReferenceValue)
               {
                   if (hexValue.ToUpperInvariant().IndexOf(item.ToUpperInvariant()) > 0) ismatchFound = true;
               }
               if (ismatchFound)
               {
                   return StaticVariables.ERRORPreFix + TamperType + " Detected !";
               }
               return TamperType + " Not Detected !";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public string Read1PHMeterBuffer(int msFlg, string defaultVal, string minVal, string maxVal)
       {
           try
           {

               //if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadEnggBufferDataStracture.ReadEnggBufferOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadStatusFlagDataStracture.MeterHealthClassID, DLMSDataStracture.ReadStatusFlagDataStracture.MeterHealthAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }//--Falcon 1 implementation
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferClassID, DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               List<string> strvalue = FillMeterBufferStructure(GlobalObjects.objSerialComm.ReceiveBuffer);

               List<string> datavalueList = new List<string>();
               //public enum MeterBuffer1PHStatusFlg {  RelayMalFunctionStatus = 0, MainBatteryVoltage = 1, RTCBatteryVoltage = 2, PushButtonPressCounter = 3, ACMagnetFieldCount = 4, CaseTamperCount = 5 };
               datavalueList.Add("0"); //----RelayMalFunctionStatus No Flag in new Structure--
               datavalueList.Add(strvalue[strvalue.Count - 6]);//----MainBatteryVoltage
               datavalueList.Add(strvalue[strvalue.Count - 5]);//----RTCBatteryVoltage
               datavalueList.Add(strvalue[strvalue.Count - 4]);//----PushButtonPressCounter
               datavalueList.Add(strvalue[strvalue.Count - 3]);//----ACMagnetFieldCount
               datavalueList.Add(strvalue[strvalue.Count - 1]);//----CaseTamperCount

               string MemoryStstFlg = "";
               switch (msFlg)
               {
                   case (int)StaticVariables.MeterBuffer1PHStatusFlg.RelayMalFunctionStatus://---This feature is not supported in Falcon2 as No Compatible HW
                       string bitsting = Convert.ToString(Convert.ToInt32(datavalueList[msFlg]), 2).PadLeft(8, '0');
                       if (bitsting.Substring(4, 1) == "0") MemoryStstFlg = "";
                       else MemoryStstFlg = StaticVariables.ERRORPreFix +  "Relay Malfunction Flag Is Non-Zero ";
                       break;
                   case (int)StaticVariables.MeterBuffer1PHStatusFlg.MainBatteryVoltage:
                   case (int)StaticVariables.MeterBuffer1PHStatusFlg.RTCBatteryVoltage:
                       MemoryStstFlg = "Main Battery Voltage = ";
                       if (msFlg == (int)StaticVariables.MeterBuffer1PHStatusFlg.RTCBatteryVoltage) MemoryStstFlg = "RTC Battery Status = " + datavalueList[msFlg];
                       else MemoryStstFlg =objcomnMethod.CheckingRangeValueForDecimal(MemoryStstFlg, defaultVal, minVal, maxVal, (Convert.ToDecimal(datavalueList[msFlg])).ToString("0.00"));
                       break;                  
                   case (int)StaticVariables.MeterBuffer1PHStatusFlg.PushButtonPressCounter:
                   case (int)StaticVariables.MeterBuffer1PHStatusFlg.CaseTamperCount:
                   case (int)StaticVariables.MeterBuffer1PHStatusFlg.ACMagnetFieldCount:
                        MemoryStstFlg = "Meter Push Button Count = ";
                        if (msFlg == (int)StaticVariables.MeterBuffer1PHStatusFlg.CaseTamperCount) MemoryStstFlg = "Case Tamper Switch Count = ";
                        else if(msFlg == (int)StaticVariables.MeterBuffer1PHStatusFlg.ACMagnetFieldCount) MemoryStstFlg = "AC Magnet Count = ";
                        MemoryStstFlg = objcomnMethod.CheckingRangeValueForDecimal(MemoryStstFlg, defaultVal, minVal, maxVal, datavalueList[msFlg]);                        
                        break;   
 
               }
               return MemoryStstFlg;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private List<string> FillMeterBufferStructure(byte[] Blockdata)
       {
           try
           {
               string data = string.Empty;
               int capture_object_definition;
               Decimal number;
               int nRowIndex = 0;
               int nByteIndex = 18;
               int byteCnt = nByteIndex + 2;
               List<string> strvalue = new List<string>();
               capture_object_definition = Blockdata[nByteIndex + 1];

               byte[] buffer = new byte[capture_object_definition];

               while (nRowIndex < capture_object_definition)
               {
                   buffer[nRowIndex++] = Blockdata[byteCnt++];
               }

               string strcmdresponse = DLMSDataStracture.GetHexStringPatternByte(buffer);
               if (strcmdresponse.Length >= 80)
               {
                   int startIndex = 0;
                   string StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 8));
                   if (Decimal.TryParse(StrData, out number)) { strvalue.Add((Convert.ToDecimal(StrData) / 100).ToString("0.00")); }
                   else strvalue.Add(StrData);
                   startIndex += 8;
                   StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 8));
                   if (Decimal.TryParse(StrData, out number)) { strvalue.Add((Convert.ToDecimal(StrData) / 1000).ToString("0.000")); }
                   else strvalue.Add(StrData);
                   startIndex += 8;
                   StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 8));
                   if (Decimal.TryParse(StrData, out number)) { strvalue.Add((Convert.ToDecimal(StrData) / 1000).ToString("0.000")); }
                   else strvalue.Add(StrData);
                   startIndex += 8;
                   StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 8));
                   if (Decimal.TryParse(StrData, out number)) { strvalue.Add((Convert.ToDecimal(StrData) / 1000).ToString("0.000")); }
                   else strvalue.Add(StrData);
                   startIndex += 8;
                   StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 8));
                   if (Decimal.TryParse(StrData, out number)) { strvalue.Add((Convert.ToDecimal(StrData) / 1000).ToString("0.000")); }
                   else strvalue.Add(StrData);
                   startIndex += 8;
                   StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 8));
                   if (Decimal.TryParse(StrData, out number)) { strvalue.Add((Convert.ToDecimal(StrData) / 1000).ToString("0.000")); }
                   else strvalue.Add(StrData);
                   startIndex += 8;
                   StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 8));
                   if (Decimal.TryParse(StrData, out number)) { strvalue.Add((Convert.ToDecimal(StrData) / 1000).ToString("0.000")); }
                   else strvalue.Add(StrData);
                   startIndex += 8;
                   StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 8));
                   if (Decimal.TryParse(StrData, out number)) { strvalue.Add((Convert.ToDecimal(StrData) / 1000).ToString("0.000")); }
                   else strvalue.Add(StrData);
                   startIndex += 8;
                   StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 8));
                   if (Decimal.TryParse(StrData, out number)) { strvalue.Add((Convert.ToDecimal(StrData) / 1000).ToString("0.000")); }
                   else strvalue.Add(StrData);
                   startIndex += 8;
                   StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 4));
                   if (Decimal.TryParse(StrData, out number)) { strvalue.Add((Convert.ToDecimal(StrData) / 100).ToString("0.00")); }
                   else strvalue.Add(StrData);
                   startIndex += 4;
                   StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 4));
                   if (Decimal.TryParse(StrData, out number)) { strvalue.Add((Convert.ToDecimal(StrData) / 10).ToString("0.0")); }
                   else strvalue.Add(StrData);
                   startIndex += 4;
                   StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 4));
                   if (Decimal.TryParse(StrData, out number)) { strvalue.Add(Convert.ToDecimal(StrData).ToString("0")); }
                   else strvalue.Add(StrData);
                   startIndex += 4;
                   StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 4));
                   if (Decimal.TryParse(StrData, out number)) { strvalue.Add(Convert.ToDecimal(StrData).ToString("0")); }
                   else strvalue.Add(StrData);
                   startIndex += 4;
                   StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 4));
                   if (Decimal.TryParse(StrData, out number)) { strvalue.Add(Convert.ToDecimal(StrData).ToString("0")); }
                   else strvalue.Add(StrData);
                   startIndex += 4;
                   if (strcmdresponse.Length > 92) //-------------For 64K BESCOM Specefic
                   {
                       //----------Test Byte Status - 1 Bytet-----------------------
                       StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 2));
                       if (Decimal.TryParse(StrData, out number)) { strvalue.Add(Convert.ToDecimal(StrData).ToString("0")); }
                       else strvalue.Add(StrData);
                       startIndex += 2;
                       //----------Main Battery Voltage - 2 Bytet-----------------------
                       StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 4));
                       if (Decimal.TryParse(StrData, out number)) { strvalue.Add((Convert.ToDecimal(StrData) / 100).ToString("0.00")); }
                       else strvalue.Add(StrData);
                       startIndex += 4;
                       //----------RTC Battery Voltage - 2 Bytet-----------------------

                       StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 2));
                       if (Decimal.TryParse(StrData, out number))
                       {
                           if (number == 0) strvalue.Add("Not OK");
                           else if (number == 1) strvalue.Add("OK");
                           else strvalue.Add("Unknown");
                       }
                       else strvalue.Add(StrData);
                       startIndex += 2;
                       startIndex += 2;//----1Byte Reserved

                       //StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 4));
                       //if (Decimal.TryParse(StrData, out number)) { strvalue.Add((Convert.ToDecimal(StrData) / 100).ToString("0.00")); }
                       //else strvalue.Add(StrData);
                       //startIndex += 4;
                       //----------Push Button Counter - 1 Bytet-----------------------
                       StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 2));
                       if (Decimal.TryParse(StrData, out number)) { strvalue.Add(Convert.ToDecimal(StrData).ToString("0")); }
                       else strvalue.Add(StrData);
                       startIndex += 2;
                       //----------AC magnet Field count - 2 Bytet-----------------------
                       StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 4));
                       if (Decimal.TryParse(StrData, out number)) { strvalue.Add(Convert.ToDecimal(StrData).ToString("0")); }
                       else strvalue.Add(StrData);
                       startIndex += 4;
                       //----------Magnet Tamper count - 2 Bytet-----------------------
                       StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 4));
                       if (Decimal.TryParse(StrData, out number)) { strvalue.Add(Convert.ToDecimal(StrData).ToString("0")); }
                       else strvalue.Add(StrData);
                       startIndex += 4;
                       //----------Case Tamper count - 2 Bytet-----------------------
                       StrData = DLMSDataStracture.HexToDecimalConversion(strcmdresponse.Substring(startIndex, 4));
                       if (Decimal.TryParse(StrData, out number)) { strvalue.Add(Convert.ToDecimal(StrData).ToString("0")); }
                       else strvalue.Add(StrData);
                       startIndex += 4;
                   }
               }
                
                return strvalue;
           }
           catch (Exception)
           {
              // MessageBox.Show(ex.Message, "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Error);
               return null;
           }

       }

       public string PushButtonTest_1Phase(string defaultVal, string minVal, string maxVal,int initFlag)
       {
           try
           {
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadEnggBufferDataStracture.ReadEnggBufferOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadStatusFlagDataStracture.ReadStatusFlagClassID, DLMSDataStracture.ReadStatusFlagDataStracture.ReadStatusFlagValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               List<string> datavalueList = FillDataInList(GlobalObjects.objSerialComm.ReceiveBuffer);
               if (initFlag >= 0) initFlag = Convert.ToInt32(datavalueList[(int)StaticVariables.MeterBuffer1PHStatusFlg.PushButtonPressCounter]) - initFlag;
               else return datavalueList[(int)StaticVariables.MeterBuffer1PHStatusFlg.PushButtonPressCounter];
               string MemoryStstFlg = objcomnMethod.CheckingRangeValueForDecimal("Push Button Count =", defaultVal, minVal, maxVal, initFlag.ToString());
               return MemoryStstFlg;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string PushButtonTest_1Phase64KVIM(string defaultVal, string minVal, string maxVal, int initFlag)
       {
           try
           {
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferClassID, DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
              // List<string> datavalueList = FillDataInList(GlobalObjects.objSerialComm.ReceiveBuffer);
               if (initFlag >= 0) initFlag = Convert.ToInt32(GlobalObjects.objSerialComm.ReceiveBuffer[63]) - initFlag;
               else return GlobalObjects.objSerialComm.ReceiveBuffer[63].ToString();
               string MemoryStstFlg = objcomnMethod.CheckingRangeValueForDecimal("Push Button Count =", defaultVal, minVal, maxVal, initFlag.ToString());
               return MemoryStstFlg;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       /// <summary>
       /// This is MicoStar Specefic and Argument Required to Verify the Value
       /// </summary>
       /// <param name="defaultVal"></param>
       /// <param name="minVal"></param>
       /// <param name="maxVal"></param>
       /// <returns></returns>
       public string CheckingMagnetTamperStatus(string defaultVal, string minVal, string maxVal)
       {
           try
           {
               int statusValue = 0;
               string validatedResponse = "";
               string acCountdefaultVal = "";
               string acCountrefminVal = "";
               string acCountrefmaxVal = "";
               string[] defValueList = defaultVal.Split(',');
               string[] minValueList = minVal.Split(',');
               string[] maxValueList = maxVal.Split(',');
               int enggdataindex = 30;
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadEnggBufferDataStracture.ReadEnggBufferOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadStatusFlagDataStracture.ReadStatusFlagClassID, DLMSDataStracture.ReadStatusFlagDataStracture.ReadStatusFlagValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               if (GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex] == 0x12)
               {
                   //------------------AC Count Test----------------------------------------
                   if (defValueList.Length >= 1) acCountdefaultVal = defValueList[0];
                   if (minValueList.Length >= 1) acCountrefminVal = minValueList[0];
                   if (maxValueList.Length >= 1) acCountrefmaxVal = maxValueList[0];

                   string[] dataValue = DLMSDataStracture.DLMSDataFormator(GlobalObjects.objSerialComm.ReceiveBuffer, enggdataindex,false);
                   if (dataValue.Length > 0) statusValue = Convert.ToInt32(dataValue[0]);
                   if (objcomnMethod.isValidReadParameters(acCountdefaultVal, acCountrefminVal, acCountrefmaxVal, statusValue)) validatedResponse = " AC Count =" + statusValue.ToString();
                   else validatedResponse = StaticVariables.ERRORPreFix + " AC Count =" + statusValue.ToString();


                   //------------------------DC Count Test------------------------------------                   
                   if (defValueList.Length >= 2) acCountdefaultVal = defValueList[1];
                   if (minValueList.Length >= 2) acCountrefminVal = minValueList[1];
                   if (maxValueList.Length >= 2) acCountrefmaxVal = maxValueList[1];
                   enggdataindex = 77;
                    dataValue = DLMSDataStracture.DLMSDataFormator(GlobalObjects.objSerialComm.ReceiveBuffer, enggdataindex, false);
                   if (dataValue.Length > 0) statusValue = Convert.ToInt32(dataValue[0]);
                   if (objcomnMethod.isValidReadParameters(acCountdefaultVal, acCountrefminVal, acCountrefmaxVal, statusValue)) validatedResponse += " ,DC Count =" + statusValue.ToString();
                   else validatedResponse += "," + StaticVariables.ERRORPreFix + " DC Count =" + statusValue.ToString();

                   return validatedResponse;

               }
               else return StaticVariables.ERRORPreFix + "Invalid Response !";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       /// <summary>
       /// This is VIM Specefic and No Argument Required as only Need to Check Flag
       /// </summary>
       /// <returns></returns>
       public string CheckingMagnetTamperStatus()
       {
           try
           {
               int enggdataindex = 30;
               string validatedResponse = "";
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadEnggBufferDataStracture.ReadEnggBufferOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadStatusFlagDataStracture.ReadStatusFlagClassID, DLMSDataStracture.ReadStatusFlagDataStracture.ReadStatusFlagValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               if (GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex] == 0x12)
               {
                   //------------------------AC Sensor 1 Flag Test------------------------------------         
                   if (GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex + 1] > 0) validatedResponse = " AC HALL Sensor-1 Flag =" + GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex + 1].ToString();
                   else validatedResponse = StaticVariables.ERRORPreFix + " AC HALL Sensor-1 Flag =" + GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex + 1].ToString();

                   //------------------------AC Sensor 2 Flag Test------------------------------------         
                   if (GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex + 2] > 0) validatedResponse += ", Sensor-2 Flag =" + GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex + 2].ToString();
                   else validatedResponse += "," + StaticVariables.ERRORPreFix + " Sensor-2 Flag =" + GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex + 2].ToString();

                   //------------------------DC Flag Test------------------------------------                   
                   enggdataindex = 77;
                   if (GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex + 2] > 0) validatedResponse += " ,DC HALL Sensor Flag =" + GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex + 2].ToString();
                   else validatedResponse += "," + StaticVariables.ERRORPreFix + " DC HALL Sensor Flag =" + GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex + 2].ToString();


                   return validatedResponse;
               }
               else return StaticVariables.ERRORPreFix + "Invalid Response !";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       /// <summary>
       /// This is 64K VIM Specefic and No Argument Required as only Need to Check Flag
       /// ReadEnggBufferDataStracture is removed from meter and flag is come with meter buffer command 
       /// </summary>
       /// <returns></returns>
       public string CheckingMagnetTamperStatus64K()
       {
           try
           {
               int enggdataindex = 18;
               string validatedResponse = "";
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferClassID, DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               if (GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex] == 0x09)
               {
                   //------------------------AC Sensor 1 Flag Test Index 64-1Byte ------------------------------------    
                   enggdataindex = 64;
                   if (GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex] > 0) validatedResponse = " AC HALL Sensor-1 Flag =" + GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex].ToString();
                   else validatedResponse = StaticVariables.ERRORPreFix + " AC HALL Sensor-1 Flag =" + GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex].ToString();
                   enggdataindex++;
                   //------------------------AC Sensor 2 Flag Test 65-1Byte ------------------------------------         
                   if (GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex] > 0) validatedResponse += ", Sensor-2 Flag =" + GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex].ToString();
                   else validatedResponse += "," + StaticVariables.ERRORPreFix + " Sensor-2 Flag =" + GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex].ToString();
                   enggdataindex++;
                   //------------------------DC Flag Test 67-1Byte ------------------------------------                   
                   enggdataindex++;//---Reverse                  
                   if (GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex] > 0) validatedResponse += " ,DC HALL Sensor Flag =" + GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex ].ToString();
                   else validatedResponse += "," + StaticVariables.ERRORPreFix + " DC HALL Sensor Flag =" + GlobalObjects.objSerialComm.ReceiveBuffer[enggdataindex].ToString();


                   return validatedResponse;
               }
               else return StaticVariables.ERRORPreFix + "Invalid Response !";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string ReadMeterParameters_3Phase(byte[] meterStatusOBIS, byte objectclassID, int msFlg, string defaultVal, string refminVal, string refmaxVal)
       { 
           try
           {
              
               string formattedValue = string.Empty;
               if (!objLI.ReadByteFromMeter(meterStatusOBIS, txtboxobject, "0", 100M, objectclassID, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_Attribute_Value)) 
               {
                   if (meterStatusOBIS == DLMSDataStracture.ReadNeutralCurrentDataStracture.ReadNeutralCurrentOBIS)
                   {
                       //---------New Neu Current read Implementation with Class ID 3 --------------------------
                       objectclassID = DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_ClassID;
                       if (!objLI.ReadByteFromMeter(meterStatusOBIS, txtboxobject, "0", 100M, objectclassID, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_Attribute_Value)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
                   }
                   else return StaticVariables.ERRORPreFix + "COMM Failed."; 
               }
               List<string> datavalueList = FormatIndivisualData(GlobalObjects.objSerialComm.ReceiveBuffer);
               if (datavalueList == null) return StaticVariables.ERRORPreFix + "COMM Failed.";
                     //---Below Condition is removed as only Scalar is supported by Class 3 Objects---------------          
                   //if (meterStatusOBIS != DLMSDataStracture.ReadNeutralCurrentDataStracture.ReadNeutralCurrentOBIS && meterStatusOBIS != DLMSDataStracture.CTDirectionDataStracture.CTDirectionOBIS)
               if (objectclassID == 0x03)
               {
                   if (!objLI.ReadByteFromMeter(meterStatusOBIS, txtboxobject, "0", 100M, objectclassID, DLMSDataStracture.EngineeringCommandDataStracture_3Phase.EngineeringCommand_3Phase_Attribute_Scalar)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
                   formattedValue = ApplyScalarUnits(GlobalObjects.objSerialComm.ReceiveBuffer, datavalueList[0]);
               }
               else if (meterStatusOBIS == DLMSDataStracture.ReadNeutralCurrentDataStracture.ReadNeutralCurrentOBIS)
               {
                   int RecevData = 0;
                   if (GlobalObjects.objSerialComm.ReceiveBuffer[19] == 0x03)
                   {
                       RecevData = (RecevData | (((int)GlobalObjects.objSerialComm.ReceiveBuffer[20]) << 16));
                       RecevData = (RecevData | (((int)GlobalObjects.objSerialComm.ReceiveBuffer[21]) << 8));
                       RecevData = (RecevData | (int)GlobalObjects.objSerialComm.ReceiveBuffer[22]);
                   }
                   else
                   {
                       // RecevData = (RecevData | (((int)GlobalObjects.objSerialComm.ReceiveBuffer[20]) << 16));
                       RecevData = (RecevData | (((int)GlobalObjects.objSerialComm.ReceiveBuffer[20]) << 8));
                       RecevData = (RecevData | (int)GlobalObjects.objSerialComm.ReceiveBuffer[21]);
                   }
                   formattedValue = (Convert.ToDecimal(RecevData) / 1000M).ToString("0.000");

                   // formattedValue = (Convert.ToDecimal(datavalueList[0]) / 1000M).ToString("0.000");
               }
               else if (meterStatusOBIS == DLMSDataStracture.CTDirectionDataStracture.CTDirectionOBIS)
               {
                   formattedValue = "Meter Value : R Phase =" + GlobalObjects.objSerialComm.ReceiveBuffer[20] + ",  Y Phase =" + GlobalObjects.objSerialComm.ReceiveBuffer[21] + ",  B Phase =" + GlobalObjects.objSerialComm.ReceiveBuffer[22];
                   if (GlobalObjects.objSerialComm.ReceiveBuffer[20] != 0x00 || GlobalObjects.objSerialComm.ReceiveBuffer[21] != 0x00 || GlobalObjects.objSerialComm.ReceiveBuffer[22] != 0x00) formattedValue = StaticVariables.ERRORPreFix + formattedValue;
                   return formattedValue;
               }
               else formattedValue = datavalueList[0];
                   //---------------------------------------Checking Conditions------------------------------------------------
                   return objcomnMethod.CheckingRangeValueForDecimal("Meter Value =", defaultVal, refminVal, refmaxVal, formattedValue);                   
                
                
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       /*Specefic 3Phase Smart meter CD Direction check implemented on R Y B Phase power value due to non- availebeliy of CT direction command in meter-*/
       public string ReadCTDirectionFalconSM310()
       {
           try
           {
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ActivePowerDataStracture.ActivePowerOBIS_R, txtboxobject, "0", 100M, DLMSDataStracture.ActivePowerDataStracture.ActivePowerClassID, DLMSDataStracture.ActivePowerDataStracture.ActivePowerValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               List<string> datavalueRphase = FormatIndivisualData(GlobalObjects.objSerialComm.ReceiveBuffer);
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ActivePowerDataStracture.ActivePowerOBIS_Y, txtboxobject, "0", 100M, DLMSDataStracture.ActivePowerDataStracture.ActivePowerClassID, DLMSDataStracture.ActivePowerDataStracture.ActivePowerValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               List<string> datavalueYphase = FormatIndivisualData(GlobalObjects.objSerialComm.ReceiveBuffer);
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ActivePowerDataStracture.ActivePowerOBIS_B, txtboxobject, "0", 100M, DLMSDataStracture.ActivePowerDataStracture.ActivePowerClassID, DLMSDataStracture.ActivePowerDataStracture.ActivePowerValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               List<string> datavalueBphase = FormatIndivisualData(GlobalObjects.objSerialComm.ReceiveBuffer);
               string formattedValue = "Power R=" + datavalueRphase[0] + ", Y=" + datavalueYphase[0] + ", B=" + datavalueBphase[0];
               if (Convert.ToInt64(datavalueRphase[0]) < 0 || Convert.ToInt64(datavalueYphase[0]) < 0 || Convert.ToInt64(datavalueBphase[0]) < 0) return StaticVariables.ERRORPreFix + formattedValue;
               return formattedValue;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       /*Specefic Sapphire S2 meter CD Direction check implemented on R Y B Phase power value due to non- availebeliy of CT direction command in meter-*/
       public string ReadCTDirectionSapphireS2()
       {
           try
           {
               TextBox[] lstboxobject = new TextBox[] { };
               if (!objLI.ReadBlockFromMeter(DLMSDataStracture.ReadoutDataStracture.PhasorProfileOBIS, lstboxobject, "0", 1M, DLMSDataStracture.ReadoutDataStracture.ReadoutClassID, DLMSDataStracture.ReadoutDataStracture.ReadoutValueAttribute_Object, DLMSDataStracture.DataStractureAccessSelector.Null_descriptor, null)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               List<string> ProfileObjectList = GetProfileObjectList(GlobalObjects.objCOSEMLIB.BlockBuffer);
               if (!objLI.ReadBlockFromMeter(DLMSDataStracture.ReadoutDataStracture.PhasorProfileOBIS, lstboxobject, "0", 1M, DLMSDataStracture.ReadoutDataStracture.ReadoutClassID, DLMSDataStracture.ReadoutDataStracture.ReadoutValueAttribute_Data, DLMSDataStracture.DataStractureAccessSelector.Null_descriptor, null)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               List<string> ProfileDataList = GetProfileDataList(GlobalObjects.objCOSEMLIB.BlockBuffer);
               if (ProfileObjectList == null || ProfileDataList == null) return StaticVariables.ERRORPreFix + "Invalid Data.";
               List<string> ReferencePhaseWisePowerObjectList = new List<string>();// Class, OBIS, Att
               ReferencePhaseWisePowerObjectList.Add("3,1.0.21.7.0.255,2");//Power R Phase
               ReferencePhaseWisePowerObjectList.Add("3,1.0.41.7.0.255,2");//Power Y Phase
               ReferencePhaseWisePowerObjectList.Add("3,1.0.61.7.0.255,2");//Power B Phase
               List<string> listofFormattedValue = new List<string>();
               foreach (var item in ReferencePhaseWisePowerObjectList)
               {
                  listofFormattedValue.Add(ProfileDataList[ProfileObjectList.IndexOf(item)]);
               }
               if (listofFormattedValue.Count <=2) return StaticVariables.ERRORPreFix + "Object Not Found.";
               string formattedValue = "Power R=" + listofFormattedValue[0] + ", Y=" + listofFormattedValue[1] + ", B=" + listofFormattedValue[2];
               if (Convert.ToInt64(listofFormattedValue[0]) < 0 || Convert.ToInt64(listofFormattedValue[1]) < 0 || Convert.ToInt64(listofFormattedValue[2]) < 0) return StaticVariables.ERRORPreFix + formattedValue;
               return formattedValue;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       private List<string> GetProfileObjectList(byte[] Blockdata)
       {
           try
           {
               string data = string.Empty;
               int capture_object_definition;
               int attribute_index;
               int i = 0;
               int nRowIndex = 0;
               int nClassID = 0;
               int nByteIndex = 0;
               List<string> ProfilObjectList = new List<string>();
               nByteIndex++;       // Array 01
               capture_object_definition = Blockdata[nByteIndex];
               for (i = 0; i < capture_object_definition; i++)
               {
                   nByteIndex++;       // Structure 02
                   nByteIndex++;       // Structure of 04

                   nByteIndex++;       // Long unsigned
                   nByteIndex++;       // Class ID
                   nByteIndex++;       // Class ID
                   nClassID = Blockdata[nByteIndex++];

                   nByteIndex++;       // Octet String 09
                   nByteIndex++;       // length 06
                   data = Blockdata[nByteIndex++].ToString();
                   data = data + "." + Blockdata[nByteIndex++].ToString();
                   data = data + "." + Blockdata[nByteIndex++].ToString();
                   data = data + "." + Blockdata[nByteIndex++].ToString();
                   data = data + "." + Blockdata[nByteIndex++].ToString();
                   data = data + "." + Blockdata[nByteIndex++].ToString();
                   nByteIndex++;       // integer
                   attribute_index = Blockdata[nByteIndex++];        // attribute index
                   nByteIndex++;       // long unsigned
                   nByteIndex++;       // data index
                   //nByteIndex++;       //data index

                   //dgvPhasorProfile.Rows[nRowIndex].Cells["colSNo"].Value = i + 1;
                   //dgvPhasorProfile.Rows[nRowIndex].Cells["colClass"].Value = nClassID;
                   //dgvPhasorProfile.Rows[nRowIndex].Cells["colObis"].Value = data;

                   //dgvPhasorProfile.Rows[nRowIndex].Cells["colAttribute"].Value = attribute_index;
                   ProfilObjectList.Add(nClassID + "," + data + "," + attribute_index);
                   data = ServiceClass.ServiceInstance.ConvertObisCode(data, 10);
                   //dgvPhasorProfile.Rows[nRowIndex].Cells["colParameter"].Value = ServiceClass.ServiceInstance.GetObisNameFromObisCode(nClassID, data, attribute_index);

                   nRowIndex++;
               }
               return ProfilObjectList;
           }
           catch (Exception)
           {
               return null;
           }
       }

       private List<string> GetProfileDataList(byte[] Blockdata)
       {
           try
           {
               List<string> ProfileDataList = new List<string>();
               string data = string.Empty;
               int capture_object_definition;
               string[] datavalue = new string[2];
               //int attribute_index;
               int i = 0;
               int nRowIndex = 0;
               //int nClassID = 0;
               int nByteIndex = 0;
               nByteIndex++;       // Array 01
               nByteIndex++;       // Array 01
               nByteIndex++;       // Structure 02
               capture_object_definition = Blockdata[nByteIndex];
               nByteIndex++;       // Structure 02
               i = 0;

               while (i < capture_object_definition)
               {
                   datavalue = DLMSDataStracture.DLMSDataFormator(Blockdata, nByteIndex, false);
                   if (datavalue == null) { i++; continue; }
                   ProfileDataList.Add(datavalue[0]);
                   nByteIndex = Convert.ToInt32(datavalue[1]);
                   nRowIndex++;
                   i++;
               }
               return ProfileDataList;
           }
           catch (Exception)
           {
               return null;
           }

       }

       private string ApplyScalarUnits(byte[] Blockdata, string data)
       {

           int arraySize;
           int nByteIndex = 18;
           String strTemp = "";
           if (Blockdata[nByteIndex] == 0x01)
           {
               nByteIndex++;
               nByteIndex++;
               arraySize = Blockdata[nByteIndex++];
           }
           else
           {
               arraySize = Blockdata[nByteIndex++];
           }


           if (Blockdata[nByteIndex] == 0x02)
           {
               nByteIndex++;
               nByteIndex++;
               int bScale = Blockdata[nByteIndex++];
               nByteIndex++;
               if ((Blockdata[nByteIndex] >= 0x1B) && (Blockdata[nByteIndex] <= 0x20))
               {
                   bScale = (Byte)(bScale - 3);
               }

               if (bScale > 127)
               {
                   bScale = 255 - bScale + 1;
                   strTemp = "-" + bScale.ToString();
               }
               else
               {
                   strTemp = bScale.ToString();
               }
           }
           string formatedData = ServiceClass.ServiceInstance.AllignData(data, Convert.ToInt16(strTemp));
           return formatedData;

       }

       private List<string> FormatIndivisualData(byte[] Blockdata)
       {
           try
           {
               string data = string.Empty;
               string[] datavalue = new string[2];
               List<string> datavalueList = new List<string>();
               int nRowIndex = 0;
               int nByteIndex = 18;
               datavalue = DLMSDataStracture.DLMSDataFormator(Blockdata, nByteIndex, false);
               if (datavalue == null) return datavalueList;
               data = datavalue[0];
               nByteIndex = Convert.ToInt32(datavalue[1]);
               datavalueList.Add(data);
               nRowIndex++;
               return datavalueList;
           }
           catch (Exception)
           {
               return null;
           }
       }

       public string VerifyMeterCurrentTest_1Phase(string refdefaultVal, string refminVal, string refmaxVal, int CurrentTestType)
       {

           try
           {
               decimal number;
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferClassID, DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               byte[] test_res_buff = GlobalObjects.objSerialComm.ReceiveBuffer;
               int nByteIndex = 18;
               int capture_object_definition = test_res_buff[nByteIndex + 1];
               byte[] buffer = new byte[capture_object_definition];
               int nRowIndex = 0;
               nByteIndex += 2;
               while (nRowIndex < capture_object_definition)
               {
                   buffer[nRowIndex++] = test_res_buff[nByteIndex++];
               }
               decimal[] str_res = new decimal[10];

               string cmd_response = DLMSDataStracture.GetHexStringPatternByte(buffer);
               //---------------------------------Voltage-------------------------------------------------
               string StrData = DLMSDataStracture.HexToDecimalConversion(cmd_response.Substring(0, 4));
               if (Decimal.TryParse(StrData, out number)) { str_res[0] = (Convert.ToDecimal(StrData) / 100); }
               else str_res[0] = Convert.ToDecimal(StrData);
               //---------------------------------Phase Current---------------------------------------------
               StrData = DLMSDataStracture.HexToDecimalConversion(cmd_response.Substring(4, 8));
               if (Decimal.TryParse(StrData, out number)) { str_res[1] = (Convert.ToDecimal(StrData) / 1000); }
               else str_res[1] = Convert.ToDecimal(StrData);
               //---------------------------------Neutral Current---------------------------------------------
               StrData = DLMSDataStracture.HexToDecimalConversion(cmd_response.Substring(12, 8));
               if (Decimal.TryParse(StrData, out number)) { str_res[2] = (Convert.ToDecimal(StrData) / 1000); }
               else str_res[2] = Convert.ToDecimal(StrData);
               //-----------------------------------------Frequency------------------------------------------
               StrData = DLMSDataStracture.HexToDecimalConversion(cmd_response.Substring(68, 4));
               if (Decimal.TryParse(StrData, out number)) { str_res[3] = (Convert.ToDecimal(StrData) / 100); }
               else str_res[3] = Convert.ToDecimal(StrData);

                string responseMessgae = "Meter Neutral Current =";
               decimal meterValue ;
               if (CurrentTestType == (int)StaticVariables.MeterCurrentType.NeutralCurrent) meterValue = str_res[2];
               else { responseMessgae = "Meter Phase Current ="; meterValue = str_res[1]; }

               if (!objcomnMethod.isValidReadParameters(refdefaultVal, refminVal, refmaxVal, meterValue)) return StaticVariables.ERRORPreFix + responseMessgae + meterValue.ToString();
               return responseMessgae + meterValue.ToString();

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }
       public string MetrologyTest_1PhaseFalcon(string refdefaultVal, string refminVal, string refmaxVal, int CurrentTestType)
       {
           try
           {

               //if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadEnggBufferDataStracture.ReadEnggBufferOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadStatusFlagDataStracture.MeterHealthClassID, DLMSDataStracture.ReadStatusFlagDataStracture.MeterHealthAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }//--Falcon 1 implementation
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferClassID, DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               List<string> strvalue = FillMeterBufferStructure(GlobalObjects.objSerialComm.ReceiveBuffer);

               List<string> str_res = new List<string>();
               //public enum MeterBuffer1PHStatusFlg {  RelayMalFunctionStatus = 0, MainBatteryVoltage = 1, RTCBatteryVoltage = 2, PushButtonPressCounter = 3, ACMagnetFieldCount = 4, CaseTamperCount = 5 };
               str_res.Add(strvalue[0]);//----Ref Voltage
               str_res.Add(strvalue[1]);//----Ref Phase Current
               str_res.Add(strvalue[2]);//----Ref Neu Current
               str_res.Add(strvalue[9]);//----Ref Frequency
              // str_res.Add(strvalue[strvalue.Count - 1]);//----CaseTamperCount

               //------------------------------Ref Value---------------------------
               string[] minValueList = refminVal.Trim().Split(',');
               string[] maxValueList = refmaxVal.Trim().Split(',');
               string[] defaultValueList = refdefaultVal.Trim().Split(',');

               int pralistCnt = defaultValueList.Length;
               if (minValueList.Length > pralistCnt) pralistCnt = minValueList.Length;
               if (maxValueList.Length > pralistCnt) pralistCnt = maxValueList.Length;
               int rCnt = 0;
               string currentcoff = string.Empty;
               bool finalResult = true;
               while (rCnt < pralistCnt)
               {

                   string defformattedValue = "";
                   string minformattedValue = "";
                   string maxformattedValue = "";
                   if (rCnt < defaultValueList.Length && defaultValueList[rCnt] != "")
                   {
                       defformattedValue = defaultValueList[pralistCnt];
                   }
                   if (rCnt < minValueList.Length && minValueList[rCnt] != "")
                   {
                       if (!Decimal.TryParse(minValueList[rCnt], out objdec)) { }
                       else minformattedValue = minValueList[rCnt];
                   }
                   if (rCnt < maxValueList.Length && maxValueList[rCnt] != "")
                   {
                       if (!Decimal.TryParse(maxValueList[rCnt], out objdec)) { }
                       else maxformattedValue = maxValueList[rCnt];
                   }


                   switch (rCnt)
                   {
                       case 0://Voltage
                           if (!objcomnMethod.isValidReadParameters(defformattedValue, minformattedValue, maxformattedValue,Convert.ToDecimal(str_res[rCnt]))) finalResult = false;
                           break;
                       case 1:
                           if (CurrentTestType == (int)StaticVariables.MMITestParameters.PhaseCurrentTest)//phase Current
                           {
                               if (!objcomnMethod.isValidReadParameters(defformattedValue, minformattedValue, maxformattedValue, Convert.ToDecimal(str_res[rCnt]))) finalResult = false;
                               currentcoff = "Ph I =" + str_res[1].ToString();
                           }
                           else if (CurrentTestType == (int)StaticVariables.MMITestParameters.NeutralCurrentTest)//Neutral Current
                           {
                               if (!objcomnMethod.isValidReadParameters(defformattedValue, minformattedValue, maxformattedValue, Convert.ToDecimal(str_res[rCnt + 1]))) finalResult = false;
                               currentcoff = "Neu I =" + str_res[2].ToString();
                           }
                           break;
                       case 2://Frequency
                           if (!objcomnMethod.isValidReadParameters(defformattedValue, minformattedValue, maxformattedValue, Convert.ToDecimal(str_res[rCnt + 1]))) finalResult = false;
                           break;
                   }
                   rCnt++;

               }
               string mmiResponse = currentcoff + ",V =" + str_res[0].ToString() + ", F =" + str_res[3].ToString();
               if (!finalResult) mmiResponse = StaticVariables.ERRORPreFix + mmiResponse;
               return mmiResponse;

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public string MMITEST(string refdefaultVal, string refminVal, string refmaxVal, int CurrentTestType)
       {

           try
           {
               decimal number;
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferClassID, DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               byte[] test_res_buff = GlobalObjects.objSerialComm.ReceiveBuffer;
               int nByteIndex = 18;
               int capture_object_definition = test_res_buff[nByteIndex + 1];
               byte[] buffer = new byte[capture_object_definition];
               int nRowIndex = 0;
               nByteIndex += 2;
               while (nRowIndex < capture_object_definition)
               {
                   buffer[nRowIndex++] = test_res_buff[nByteIndex++];
               }
               decimal[] str_res = new decimal[10];

               string cmd_response = DLMSDataStracture.GetHexStringPatternByte(buffer);
               //---------------------------------Voltage-------------------------------------------------
               string StrData = DLMSDataStracture.HexToDecimalConversion(cmd_response.Substring(0, 4));
               if (Decimal.TryParse(StrData, out number)) { str_res[0] = (Convert.ToDecimal(StrData) / 100); }
               else str_res[0] = Convert.ToDecimal(StrData);
               //---------------------------------Phase Current---------------------------------------------
               StrData = DLMSDataStracture.HexToDecimalConversion(cmd_response.Substring(4, 8));
               if (Decimal.TryParse(StrData, out number)) { str_res[1] = (Convert.ToDecimal(StrData) / 1000); }
               else str_res[1] = Convert.ToDecimal(StrData);
               //---------------------------------Neutral Current---------------------------------------------
               StrData = DLMSDataStracture.HexToDecimalConversion(cmd_response.Substring(12, 8));
               if (Decimal.TryParse(StrData, out number)) { str_res[2] = (Convert.ToDecimal(StrData) / 1000); }
               else str_res[2] = Convert.ToDecimal(StrData);
               //-----------------------------------------Frequency------------------------------------------
               StrData = DLMSDataStracture.HexToDecimalConversion(cmd_response.Substring(68, 4));
               if (Decimal.TryParse(StrData, out number)) { str_res[3] = (Convert.ToDecimal(StrData) / 100); }
               else str_res[3] = Convert.ToDecimal(StrData);

               //------------------------------Ref Value---------------------------
               string[] minValueList = refminVal.Trim().Split(',');
               string[] maxValueList = refmaxVal.Trim().Split(',');
               string[] defaultValueList = refdefaultVal.Trim().Split(',');

             int pralistCnt = defaultValueList.Length;
             if(minValueList.Length > pralistCnt) pralistCnt = minValueList.Length;
             if(maxValueList.Length > pralistCnt) pralistCnt = maxValueList.Length;
             int rCnt=0;           
             string currentcoff = string.Empty;
             bool finalResult = true;
               while(rCnt < pralistCnt)
               {
                   
                   string defformattedValue="";
                   string minformattedValue="";
                   string maxformattedValue="";
                       if (rCnt < defaultValueList.Length && defaultValueList[rCnt] != "")
                       {
                           defformattedValue = defaultValueList[pralistCnt];
                       }                    
                       if (rCnt < minValueList.Length && minValueList[rCnt] != "")
                       {
                           if (!Decimal.TryParse(minValueList[rCnt], out objdec)) {}
                           else minformattedValue = minValueList[rCnt];
                       }
                       if (rCnt < maxValueList.Length && maxValueList[rCnt] != "")
                       {
                           if (!Decimal.TryParse(maxValueList[rCnt], out objdec)) {}
                           else maxformattedValue = maxValueList[rCnt];
                       }
                       
                   
                   switch (rCnt)
                   {
                       case 0://Voltage
                           if (!objcomnMethod.isValidReadParameters(defformattedValue, minformattedValue, maxformattedValue, str_res[rCnt])) finalResult = false;
                           break;
                       case 1:
                           if (CurrentTestType == (int)StaticVariables.MMITestParameters.PhaseCurrentTest)//phase Current
                           {
                               if (!objcomnMethod.isValidReadParameters(defformattedValue, minformattedValue, maxformattedValue, str_res[rCnt])) finalResult = false;
                               currentcoff = " , Phase Current =" + str_res[1].ToString();
                           }
                           else if (CurrentTestType == (int)StaticVariables.MMITestParameters.NeutralCurrentTest)//Neutral Current
                           {
                               if (!objcomnMethod.isValidReadParameters(defformattedValue, minformattedValue, maxformattedValue, str_res[rCnt + 1])) finalResult = false;
                               currentcoff = ", Neutral Current =" + str_res[2].ToString();
                           }
                               break;
                       case 2://Frequency
                               if (!objcomnMethod.isValidReadParameters(defformattedValue, minformattedValue, maxformattedValue, str_res[rCnt + 1])) finalResult = false;
                           break;
                   }
                   rCnt++;

               }
               string mmiResponse = "Voltage =" + str_res[0].ToString() + currentcoff + ", Frequency =" + str_res[3].ToString();
               if (!finalResult) mmiResponse = StaticVariables.ERRORPreFix + mmiResponse;
               return mmiResponse;

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }

       public string CommunicationTest(string Defaultpara)
       {
            string returnmsg = "";
           string CurrentCOM = objappSettings.GetPortName();
           try
           {
               string[] comlist = Defaultpara.Split(',');
               if (comlist == null) return  StaticVariables.ERRORPreFix + "No COM Port List To Test"; ;
               if (Defaultpara.Trim().Length <= 0)return  StaticVariables.ERRORPreFix + "No COM Port List To Test";
               int porttestcnt = 0;
               while (porttestcnt < comlist.Length)
               {
                   string portName = comlist[porttestcnt++];
                   if (portName.Trim().Length <= 0) continue;
                   if (CurrentCOM == portName) continue;
                   objLI.IsMeterConnected = true;
                   objLI.AssociationDisconnect();//---Diconnect for optical communication port
                   objappSettings.SetPortName(portName);//---Setting RJ port for communication check
                   if (!objLI.ConnectToMeter())//----Retry incase of failure
                   {
                       objLI.IsMeterConnected = true;
                       objLI.AssociationDisconnect();
                       if (!objLI.ConnectToMeter()) return StaticVariables.ERRORPreFix + portName;
                   }
               }
                //---------------------Restoring orginal Connection--------------------------
                objLI.AssociationDisconnect();
                objappSettings.SetPortName(CurrentCOM);
                if (!objLI.ConnectToMeter())//----Retry incase of failure
                {
                    objLI.IsMeterConnected = true;
                    objLI.AssociationDisconnect();
                    if (!objLI.ConnectToMeter()) returnmsg = StaticVariables.ERRORPreFix + "Unable To Restore Association";
                }
            }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
           return returnmsg;
       }

       public string TestRTCDrift(string defaultVal, string minVal, string maxVal)
       {
           string meter_datetime = string.Empty;
           try
           {
               objLI.DisplayStatusMsg("", false);
               TextBox[] txtboxobject = new TextBox[] { };
               Application.DoEvents();
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.MeterRTCDataStracture.MeterRTCOBIS, txtboxobject, "0", 1M, DLMSDataStracture.MeterRTCDataStracture.MeterRTCClassID, DLMSDataStracture.MeterRTCDataStracture.MeterRTCValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               DateTime system_dt = DateTime.Now;
               int month = GlobalObjects.objSerialComm.ReceiveBuffer[22];
               meter_datetime = month.ToString() + "/";
               int date = GlobalObjects.objSerialComm.ReceiveBuffer[23];
               meter_datetime += date.ToString() + "/";
               int year = 0;// receivedData[21];
               year = (year | (int)GlobalObjects.objSerialComm.ReceiveBuffer[20]) << 8;
               year = (year | (int)GlobalObjects.objSerialComm.ReceiveBuffer[21]);
               meter_datetime += year.ToString() + " ";
               int hour = GlobalObjects.objSerialComm.ReceiveBuffer[25];
               meter_datetime += hour.ToString() + ":";
               int minute = GlobalObjects.objSerialComm.ReceiveBuffer[26];
               meter_datetime += minute.ToString() + ":";
               int second = GlobalObjects.objSerialComm.ReceiveBuffer[27];
               meter_datetime += second.ToString();
               DateTime mdt = new DateTime(year, month, date, hour, minute, second);               
               TimeSpan get_diff = system_dt - mdt;
               string calDrift = objcomnMethod.CheckingRangeValueForDecimal("Meter Drift (Sec.) = ", defaultVal, minVal, maxVal,  ((long)get_diff.TotalSeconds).ToString());
               return calDrift + ",  Meter Stamp =" + string.Format("{0:dd/MM/yyyy HH:mm:ss}", mdt) + ",  System Stamp =" + string.Format("{0:dd/MM/yyyy HH:mm:ss}", system_dt);

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }

       public string TestRTCDrift(string defaultVal, string minVal, string maxVal, int diffSeconds)
       {
           string meter_datetime = string.Empty;
           try
           {
               objLI.DisplayStatusMsg("", false);
               TextBox[] txtboxobject = new TextBox[] { };
               Application.DoEvents();
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.MeterRTCDataStracture.MeterRTCOBIS, txtboxobject, "0", 1M, DLMSDataStracture.MeterRTCDataStracture.MeterRTCClassID, DLMSDataStracture.MeterRTCDataStracture.MeterRTCValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               DateTime system_dt = DateTime.Now.AddSeconds(diffSeconds);
               int month = GlobalObjects.objSerialComm.ReceiveBuffer[22];
               meter_datetime = month.ToString() + "/";
               int date = GlobalObjects.objSerialComm.ReceiveBuffer[23];
               meter_datetime += date.ToString() + "/";
               int year = 0;// receivedData[21];
               year = (year | (int)GlobalObjects.objSerialComm.ReceiveBuffer[20]) << 8;
               year = (year | (int)GlobalObjects.objSerialComm.ReceiveBuffer[21]);
               meter_datetime += year.ToString() + " ";
               int hour = GlobalObjects.objSerialComm.ReceiveBuffer[25];
               meter_datetime += hour.ToString() + ":";
               int minute = GlobalObjects.objSerialComm.ReceiveBuffer[26];
               meter_datetime += minute.ToString() + ":";
               int second = GlobalObjects.objSerialComm.ReceiveBuffer[27];
               meter_datetime += second.ToString();
               DateTime mdt = new DateTime(year, month, date, hour, minute, second);
               TimeSpan get_diff = system_dt - mdt;
               string calDrift = objcomnMethod.CheckingRangeValueForDecimal("Meter Drift (Sec.) = ", defaultVal, minVal, maxVal, ((long)get_diff.TotalSeconds).ToString());
               return calDrift + ",  Meter Stamp =" + string.Format("{0:dd/MM/yyyy HH:mm:ss}", mdt) + ",  System Stamp =" + string.Format("{0:dd/MM/yyyy HH:mm:ss}", system_dt);

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }       

       private List<string> FillDataInList(byte[] Blockdata)
       {
           try
           {
               string data = string.Empty;
               int capture_object_definition;
               string[] datavalue = new string[2];
               List<string> datavalueList = new List<string>();
               //int attribute_index;
               int i = 0;
               int nRowIndex = 0;
               //int nClassID = 0;
               int nByteIndex = 18;
               nByteIndex++;
               capture_object_definition = Blockdata[nByteIndex++];
               nByteIndex++;       // Structure 02
               i = 0;

               while (i < capture_object_definition)
               {
                   if (Blockdata[nByteIndex] == 0x09)
                   {
                       nByteIndex++;
                       datavalue[0] = (Blockdata[nByteIndex + 22] * 16 + Blockdata[nByteIndex + 23]).ToString();
                       nByteIndex += Convert.ToInt32(Blockdata[nByteIndex]);
                       // nByteIndex++;
                       datavalue[1] = nByteIndex.ToString();
                   }
                   else
                   {
                       datavalue = DLMSDataStracture.DLMSDataFormator(Blockdata, nByteIndex, false);
                       if (datavalue == null) { i++; continue; }

                   }
                   data = datavalue[0];
                   nByteIndex = Convert.ToInt32(datavalue[1]);
                   datavalueList.Add(data);
                   nRowIndex++;
                   i++;
               }

               return datavalueList;
           }
           catch (Exception)
           {
               return null;
           }
       }

       private string DisplayDatainControl(byte[] receivedData, TextBox[] txtboxobject, string displayFormat, decimal emf)
       {
           try
           {
               int startDataindx = 18;
               string MeterIDReadbuff = "";
               string MID = string.Empty;
               if (receivedData[startDataindx] == 0x09 || receivedData[startDataindx] == 0x0A) //srtact
               {
                   startDataindx++;
                   int stractcount = 0;
                   int lengthodstruct = receivedData[startDataindx++];//length of stract
                   while (stractcount < lengthodstruct)
                   {
                       MeterIDReadbuff = receivedData[startDataindx++].ToString("X");
                       char idchar = ((char)(Convert.ToInt32((MeterIDReadbuff), 16)));
                       if (idchar != '\0') MID = MID + idchar.ToString();
                       else break;
                       stractcount++;
                   }
               }
               if (MID.Trim().Length <= 0) return StaticVariables.ERRORPreFix + "PCBA ID is Blank/Invalid";
               else return MID.Trim();
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public void SetDefaultSettings(string[] IPParalist)
       {
           try
           {
               int maxlen = IPParalist.Length;// As last element is signon baudrate and 2nd lase is user ID and 
               int elementCount = 33;
               AppSettings objApps = new AppSettings();
               List<string> DataValueList = new List<string>();
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);
               DataValueList.Add(IPParalist[maxlen - (elementCount--)]);              
               DataValueList.Add(IPParalist[maxlen-1]);  //---------default File Path
               // DataValueList.Add(IPParalist[maxlen - 2]);//----------Loged USED Type Index
               // DataValueList.Add(IPParalist[maxlen - 3]);//----------Loged USED ID No Need To Save in Settings
               DataValueList.Add(IPParalist[maxlen - 9]);//---------Signon Baudrate
               DataValueList.Add(IPParalist[maxlen - 8]);//---------ClientSystemTitle
               DataValueList.Add(IPParalist[maxlen - 7]);//---------Securitysuit
               DataValueList.Add(IPParalist[maxlen - 6]);//---------GlobalEncryptionKey
               DataValueList.Add(IPParalist[maxlen - 5]);//---------DedicatedKey
               DataValueList.Add(IPParalist[maxlen - 4]);//---------AuthenticationKey
               //SerialPortSettings.Default.ClientSystemTitle = valueList[ValueIDX++];// for Smart meter

               //SerialPortSettings.Default.Securitysuit = Convert.ToInt32(valueList[ValueIDX++]);//security suit for Smart meter 
               //SerialPortSettings.Default.GlobalEncryptionKey = valueList[ValueIDX++];//Encryption Key
               //SerialPortSettings.Default.DedicatedKey = Convert.ToInt16(valueList[ValueIDX++]);//Dedicated Key
               //SerialPortSettings.Default.AuthenticationKey = valueList[ValueIDX++];//Authentication Key
               if (!objApps.SaveSettings(DataValueList))
               {
                   return;
               }

                
           }
           catch (Exception Ex)
           {
               MessageBox.Show(Ex.ToString() + "\n" + "Unable To Save Settings!", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
           }
       }

       //------------------------Smart Meter Traveller---------------------------------------------
       
       public bool SetTravelerData_SmartMeter1Phase(int gdidIDX, int travelerType, DataGridView DGVParaLists)
       {
           try
           {
               TextBox[] texboxobject = new TextBox[] { };
               List<byte> travelerBytes = GetTravelerWritebytes_1Phase(travelerType, gdidIDX, DGVParaLists);
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMValueAttribute, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMOBIS, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMClassID, 0x09, (byte)travelerBytes.Count, travelerBytes, DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return false; }
               return true;
           }
           catch (Exception)
           {
               return false;
           }
       }
       public bool SetTravelerData_3Phase(int gdidIDX, int travelerType, DataGridView DGVParaLists)
       {
           try
           {
               TextBox[] texboxobject = new TextBox[] { };
       
               List<byte> travelerBytes = GetTravelerWritebytes_3Phase(travelerType, gdidIDX, DGVParaLists);
               if (objappSettings.GetMeterMode() == (int)LayerInterface.MeterTypeInfo.SAPPHIRE_S2) travelerBytes = GetTravelerWritebytes_SapphireS2(travelerType, gdidIDX, DGVParaLists);
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMValueAttribute, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMOBIS, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMClassID, 0x09 , (byte)travelerBytes.Count , travelerBytes, DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return false; }
               return true;
           }
           catch (Exception)
           {
               return false;
           }
       }
     
       public string GetTravelerData(int travelerType)
       {
           try
           {
               TextBox[] texboxobject = new TextBox[] { };
               List<byte> parabyte = GetReadTravelerStructByte(travelerType);
               if (!objLI.ReadBlockFromMeter(DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMOBIS, texboxobject, "0", 1M, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMClassID, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMValueAttribute, DLMSDataStracture.DataStractureAccessSelector.Range_descriptor, parabyte)) return StaticVariables.ERRORPreFix + "COMM Failed.";
               return (IsValidTravelerData(GlobalObjects.objSerialComm.ReceiveBuffer, travelerType));
           }
           catch (Exception Ex)
           {
               return StaticVariables.ERRORPreFix + Ex;
           }
       }

       public string GetTravelerData_3Phase(int travelerType, int meterExecutionType)
       {
           try
           {
               TextBox[] texboxobject = new TextBox[] { };             
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMOBIS, texboxobject, "0", 1M, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMClassID, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               if (meterExecutionType == (int)LayerInterface.MeterTypeInfo.SAPPHIRE_S2) return (IsValidTravelerData_SapphireS2(GlobalObjects.objSerialComm.ReceiveBuffer, travelerType));
               else return(IsValidTravelerData_3PhaseSmartMeter(GlobalObjects.objSerialComm.ReceiveBuffer, travelerType, meterExecutionType));
           }
           catch (Exception Ex)
           {
               return StaticVariables.ERRORPreFix + Ex;
           }
       }
        /// <summary>
        ///In VIM DLMS Cost Reduction project - we are using small battery and we have restricted single wire limit.
        ///Single wire limit counter readout has been implemented in eeprom read command, for checking battery condition during production stage.
        ///Command type – EEPROM READ (Obis- 0x00,0x00,0x60,0x02,0x9B,0xFF, Attribute - 2)
        ///psudeo Address – 0xFFFD
        ///Data Size – 4
       /// </summary>
       /// memory health status. Read Meter Buffer OBIS
       /// 0x00 --> PASS, Healthy
       /// 0x01 --> FAIL, Corrupted
       /// <returns></returns>
       public string GetSingleWireCountLimits(string defaultVal, string refminVal, string refmaxVal)
       {
           try
           {
               if (defaultVal == "" && refminVal == "" && refmaxVal == "") refmaxVal = "86400"; //---Set the default Value to Max if no arg given, default value will be two month * Minutes
               TextBox[] texboxobject = new TextBox[] { };
               List<byte> parabyte = GetReadStructByte(0xFFFD, 4);
               //------------------Read Meter Buffer to check the memory health status--------------
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferOBIS, texboxobject, "0", 100M, DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferClassID, DLMSDataStracture.ReadMeterBufferDataStracture.ReadMeterBufferValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               byte memoryHealthStatus = GlobalObjects.objSerialComm.ReceiveBuffer[66];//---Base index (18) + Datatype index(1Byte) + Data Length index (1Byte) + Byte Index (46)
               if (memoryHealthStatus >= 1) 
               {
                   return StaticVariables.ERRORPreFix + "Memory Corrupted, Status =" + memoryHealthStatus.ToString();
               }
               //------------------Read Single Wire Count to verify the limits----------------------
               if (!objLI.ReadBlockFromMeter(DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMOBIS, texboxobject, "0", 1M, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMClassID, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMValueAttribute, DLMSDataStracture.DataStractureAccessSelector.Range_descriptor, parabyte)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               int formattedValue = 0;
               formattedValue = (formattedValue | (int)GlobalObjects.objSerialComm.ReceiveBuffer[20]);
               formattedValue = (formattedValue | (((int)GlobalObjects.objSerialComm.ReceiveBuffer[21]) << 8));
               formattedValue = (formattedValue | (((int)GlobalObjects.objSerialComm.ReceiveBuffer[22]) << 16));
               formattedValue = (formattedValue | (((int)GlobalObjects.objSerialComm.ReceiveBuffer[23]) << 24));
               return objcomnMethod.CheckingRangeValueForDecimal("Single Wire Counts =", defaultVal, refminVal, refmaxVal, formattedValue.ToString()) + ", Memory Status =" + memoryHealthStatus.ToString();
           }
           catch (Exception Ex)
           {
               return StaticVariables.ERRORPreFix + Ex;
           }
       }

       private List<byte> GetReadStructByte(int CurrentReadAddress, int BytestoRead)
       {
           int noofBytes = BytestoRead;
           string StartAddress = CurrentReadAddress.ToString("X");
           StartAddress = StartAddress.PadLeft(4, '0');

           List<byte> EEPROMbyte = new List<byte>();
           EEPROMbyte.Add(DLMSDataStracture.DataStractureAccessSelector.Range_descriptor);
           EEPROMbyte.Add(DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMDataType);
           EEPROMbyte.Add(DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMDataLength);

           EEPROMbyte.Add(0x12);//double-long-unsigned
           EEPROMbyte.Add((byte)Convert.ToInt32(StartAddress.Substring(0, 2), 16));
           EEPROMbyte.Add((byte)Convert.ToInt32(StartAddress.Substring(2, 2), 16));

           EEPROMbyte.Add(0x12);//double-long-unsigned
           EEPROMbyte.Add(Convert.ToByte((noofBytes & 0xFF00) >> 8));
           EEPROMbyte.Add(Convert.ToByte(noofBytes & 0x00FF));

           return EEPROMbyte;
       }


       public string IsValidTravelerData(byte[] Blockdata, int TravelerType)
        {
            string metertravelerstatus="";
            int testPosition = 20;
            try
            {
                string dtFlag = GetTravellerDateTime_SmartMeter(Blockdata, testPosition + 2);
                metertravelerstatus = "Meter Product Stage =" + Blockdata[testPosition].ToString() + ", Execution Status =" + Blockdata[testPosition + 1].ToString();
                if (Blockdata[testPosition] == TravelerType && Blockdata[testPosition + 1] == 0x01) return metertravelerstatus + ", " + dtFlag;
                else if (Blockdata[testPosition] == TravelerType && Blockdata[testPosition + 1] == 0x11) return metertravelerstatus + ", " + dtFlag;
                return StaticVariables.ERRORPreFix + metertravelerstatus + ", " + dtFlag;  
            }
            catch (Exception Ex)
            {
                return StaticVariables.ERRORPreFix + Ex;
            }
        }

       public string IsValidTravelerData_3PhaseSmartMeter(byte[] Blockdata, int TravelerType, int meterExecutionType)
       {
           int testPosition  =0;
           if (TravelerType == (int)StaticVariables.ProduTraveler.FunctionTest) testPosition = 20;
           else if (TravelerType == (int)StaticVariables.ProduTraveler.CalibrationTest) testPosition = 28;
           else if (TravelerType == (int)StaticVariables.ProduTraveler.SirializationTest) testPosition = 36;
           if (meterExecutionType == (int)StaticVariables.ExecutedMeterType.Smart_Meter_1PH) testPosition += 21;
           string metertravelerstatus = "";
           try
           {
               string dtFlag = GetTravellerDateTime_SmartMeter(Blockdata, testPosition + 2);
               metertravelerstatus = "Meter Product Stage =" + Blockdata[testPosition].ToString() + ", Execution Status =" + Blockdata[testPosition + 1].ToString();
               if (Blockdata[testPosition] == TravelerType && Blockdata[testPosition + 1] == 0x01) return metertravelerstatus + ", " + dtFlag;//PASS
               else if (Blockdata[testPosition] == TravelerType && Blockdata[testPosition + 1] == 0x11) return metertravelerstatus + ", " + dtFlag;//Re-Works
               return StaticVariables.ERRORPreFix + metertravelerstatus + ", " + dtFlag;//FAIL

           }
           catch (Exception Ex)
           {
               return StaticVariables.ERRORPreFix + Ex;
           }
       }
       //------------------------3Phase Non-Smart Meter traveller-------------------------------------------

       public bool SetTravelerData_3PhaseDLMS(int gdidIDX, int travelerType, DataGridView DGVParaLists)
       {
           try
           {
               //-------------Smart Meter RefReferenceVoltageDataStracture are same as 3Phase DLMS Meter Traveller object and here we are using the same
               TextBox[] texboxobject = new TextBox[] { };
               List<byte> travelerBytes = GetTravelerWritebytes_3PhaseDLMS(travelerType, gdidIDX, DGVParaLists);
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ReferenceVoltageDataStracture.ReadRefVoltageValueAttribute, DLMSDataStracture.ReferenceVoltageDataStracture.ReferenceVoltageOBIS, DLMSDataStracture.ReferenceVoltageDataStracture.RefVoltageClassID, 0x09, (byte)travelerBytes.Count, travelerBytes, DLMSDataStracture.DataStractureRequest.SetRequest_Normal))
               {
                   if (objLI.getWriteResponseCode == 2) return true; //(int)objLI.ProgrammingCode.AccessDenied
                   return false; 
               }
               return true;
           }
           catch (Exception)
           {
               return false;
           }
       }

       public List<byte> GetTravelerWritebytes_3PhaseDLMS(int TravelerType, int gdidIDX, DataGridView DGVParaLists)
       {
           CommonMethods objcmnmethods = new CommonMethods();           
           List<byte> travelerbyte = new List<byte>();
           travelerbyte.Add((byte)(TravelerType + 1));//---Stage Number 1,2,3 or 4
           if (GetExecutionStatus(gdidIDX, DGVParaLists)) travelerbyte.Add(0x01);//---Status Fail:0x00, Pass:0x01, ReworkPass 0x11
           else travelerbyte.Add(0x00);
           travelerbyte.Add((byte)DateTime.Now.Day);
           travelerbyte.Add((byte)DateTime.Now.Month);
           travelerbyte.Add((byte)(DateTime.Now.Year - 2000));
           travelerbyte.Add((byte)DateTime.Now.Hour);
           travelerbyte.Add((byte)DateTime.Now.Minute);
           travelerbyte.Add((byte)DateTime.Now.Second);
           return travelerbyte;
       }      

       public string GetTravelerData_3PhaseDLMS(int travelerType)
       {
           try
           {
               //--------Smart Meter Ref Meter Voltage and 3Phase DLMS Meter OBIS are Same----------------------------
               TextBox[] texboxobject = new TextBox[] { };
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReferenceVoltageDataStracture.ReferenceVoltageOBIS, texboxobject, "0", 1M, DLMSDataStracture.ReferenceVoltageDataStracture.RefVoltageClassID, DLMSDataStracture.ReferenceVoltageDataStracture.ReadRefVoltageValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return (IsValidTravelerData_3PhaseDLMS(GlobalObjects.objSerialComm.ReceiveBuffer, travelerType));
           }
           catch (Exception Ex)
           {
               return StaticVariables.ERRORPreFix + Ex;
           }
       }

       public string IsValidTravelerData_3PhaseDLMS(byte[] Blockdata, int TravelerType)
       {
           int testPosition = 0;
           if (Blockdata[15] == 0x02)
           {
               int idx = Blockdata.ToList().IndexOf(0x09);
               if (idx <= 0) testPosition = 23;
               if (TravelerType == (int)StaticVariables.ProduTraveler.FunctionTest) testPosition = idx + 2;
               else if (TravelerType == (int)StaticVariables.ProduTraveler.CalibrationTest) testPosition = idx + 10;
               else if (TravelerType == (int)StaticVariables.ProduTraveler.SirializationTest) testPosition = idx + 18;
           }
           else
           {
               if (TravelerType == (int)StaticVariables.ProduTraveler.FunctionTest) testPosition = 20;
               else if (TravelerType == (int)StaticVariables.ProduTraveler.CalibrationTest) testPosition = 28;
               else if (TravelerType == (int)StaticVariables.ProduTraveler.SirializationTest) testPosition = 36;
           }
           string dtFlag = "";
           string metertravelerstatus = "";
           try
           {
               metertravelerstatus = "Meter Product Stage =" + Blockdata[testPosition].ToString() + ", Execution Status =" + Blockdata[testPosition + 1].ToString();

               dtFlag = "Date & Time =" + String.Format("{0:dd/MM/yy HH:mm:ss}", new DateTime(Blockdata[testPosition + 4]
                       , Blockdata[testPosition + 3]
                       , Blockdata[testPosition + 2]
                       , Blockdata[testPosition + 5]
                       , Blockdata[testPosition + 6]
                       , Blockdata[testPosition + 7]));
               if (Blockdata[testPosition] == TravelerType && (Blockdata[testPosition + 1] == 0x01 || Blockdata[testPosition + 1] == 0x11)) return metertravelerstatus + ", " + dtFlag;
               return StaticVariables.ERRORPreFix + metertravelerstatus + ", " + dtFlag;

           }
           catch (Exception)
           {
               return StaticVariables.ERRORPreFix + metertravelerstatus + ", Invalid Date & Time =" + dtFlag;
           }
       }

       public string IsValidTravelerData_SapphireS2(byte[] Blockdata, int TravelerType)
       {
           int testPosition = 0;
           if (TravelerType == (int)StaticVariables.ProduTraveler.FunctionTest) testPosition = 21;
           else if (TravelerType == (int)StaticVariables.ProduTraveler.CalibrationTest) testPosition = 29;
           else if (TravelerType == (int)StaticVariables.ProduTraveler.SirializationTest) testPosition = 37;
           string dtFlag = "";
           string metertravelerstatus = "";
           try
           {
               metertravelerstatus = "Current Flag =" + Blockdata[testPosition-1].ToString() + ", Meter Product Stage =" + Blockdata[testPosition].ToString() + ", Execution Status =" + Blockdata[testPosition + 1].ToString();
               dtFlag = "Date & Time =" + String.Format("{0:dd/MM/yy HH:mm:ss}", new DateTime(Blockdata[testPosition + 4]
                       , Blockdata[testPosition + 3]
                       , Blockdata[testPosition + 2]
                       , Blockdata[testPosition + 5]
                       , Blockdata[testPosition + 6]
                       , Blockdata[testPosition + 7]));
               if (Blockdata[testPosition] == TravelerType && (Blockdata[testPosition + 1] == 0x01 || Blockdata[testPosition + 1] == 0x03)) return metertravelerstatus + ", " + dtFlag;
               return StaticVariables.ERRORPreFix + metertravelerstatus + ", " + dtFlag;

           }
           catch (Exception)
           {
               return StaticVariables.ERRORPreFix + metertravelerstatus + ", Invalid Date & Time =" + dtFlag;
           }
       }

       private string GetTravellerDateTime_SmartMeter(byte[] Blockdata, int testPosition)
       {
           string travellerstatusDT = "";
           try
           {
               byte[] dataValuearr = new byte[8];
               dataValuearr[0] = 0x00;
               dataValuearr[1] = 0x00;
               Array.Copy(Blockdata, testPosition, dataValuearr, 2, 6);
               Array.Reverse(dataValuearr);
               Int64 compValue = BitConverter.ToInt64(dataValuearr, 0);
               travellerstatusDT = compValue.ToString("d14");

               string dtFlag = "Date & Time =" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", new DateTime(Convert.ToInt16(travellerstatusDT.Substring(0, 4))
                        , Convert.ToInt16(travellerstatusDT.Substring(4, 2))
                        , Convert.ToInt16(travellerstatusDT.Substring(6, 2))
                        , Convert.ToInt16(travellerstatusDT.Substring(8, 2))
                        , Convert.ToInt16(travellerstatusDT.Substring(10, 2))
                        , Convert.ToInt16(travellerstatusDT.Substring(12, 2))));
               return dtFlag;

           }
           catch (Exception)
           {
               return StaticVariables.ERRORPreFix + "Invalid Date & Time =" + travellerstatusDT;
           }
       }
        
       //---------------------------------------------------------------------------------------------------
       
       public string IsSerialIDinRange(string GerSerialID,string MeterTypeasFileName)
       {
           try
           {           
               string xmlFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Configuration\\" + StaticVariables.FilePrefixMeterIDList + MeterTypeasFileName + ".xml";
               if (!File.Exists(xmlFilePath)) return "Meter ID Range Definition Not Found !" + "\n" + "Please Define Valid Meter ID Range In Selected Procedure OR Contact Your Supervisor !";
               string resultArr = ServiceClass.ServiceInstance.GetDisplayParametersValue("MeterIDList", "MeterID", GerSerialID, "MeterID", xmlFilePath);
               if (resultArr.Length > 0)
               {
                   if (GerSerialID == resultArr.Trim()) return "";
                   else return "Meter ID : " + GerSerialID + " Is Out of Defined Range, " + "\n" + "Please Define Valid Meter ID Range In Selected Procedure OR Contact Your Supervisor !";
               }
               else return "Meter ID is out of Define Range !" + "\n" + "Please Define Valid Meter ID Range In Selected Procedure !";

           }
           catch (Exception ex)
           {
               return ex.ToString();
           }
       }

       public int SetTravelerStage(string testCategory)
       {
           int TravelerStage = -1;
           if (testCategory.IndexOf("EMS") >= 0) TravelerStage = 0;
           else if (testCategory.IndexOf("Functional") >= 0) TravelerStage = 1;
           else if (testCategory.IndexOf("Calibration") >= 0) TravelerStage = 2;
           else if (testCategory.IndexOf("Serialization") >= 0) TravelerStage = 3;
           else if (testCategory.IndexOf("OtherTest") >= 0) TravelerStage = 4;
           return TravelerStage;
       }

       public List<byte> GetReadTravelerStructByte(int travelerType)
        {
            CommonMethods objcmnmethods = new CommonMethods();
            string StartAddress = objcmnmethods.GetTravelerAddress(travelerType - 1);
            StartAddress = StartAddress.PadLeft(4, '0');

            List<byte> EEPROMbyte = new List<byte>();
            EEPROMbyte.Add(DLMSDataStracture.DataStractureAccessSelector.Range_descriptor);
            EEPROMbyte.Add(DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMDataType);
            EEPROMbyte.Add(DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMDataLength);


            EEPROMbyte.Add(0x12);//double-long-unsigned
            EEPROMbyte.Add((byte)Convert.ToInt32(StartAddress.Substring(0, 2), 16));
            EEPROMbyte.Add((byte)Convert.ToInt32(StartAddress.Substring(2, 2), 16));

            EEPROMbyte.Add(0x12);//double-long-unsigned
            EEPROMbyte.Add(0x00);
            EEPROMbyte.Add(0x08);
            return EEPROMbyte;
        }


       public List<byte> GetTravelerWritebytes_SapphireS2(int TravelerType, int gdidIDX, DataGridView DGVParaLists)
       {
           CommonMethods objcmnmethods = new CommonMethods();
           byte totalLen = objcmnmethods.GetTravelerBytes(TravelerType);
           List<byte> travelerbyte = new List<byte>();

           travelerbyte.Add((byte)(TravelerType + 1));//---Stage Number 1,2,3 or 4

          if (GetExecutionStatus(gdidIDX, DGVParaLists)) travelerbyte.Add(0x01);//---Status Fail:0, Pass:1
           else travelerbyte.Add(0x00);
           //----------------DD MM YY hh mm ss-------------------------------
           travelerbyte.Add(Convert.ToByte(DateTime.Now.Day));
           travelerbyte.Add(Convert.ToByte(DateTime.Now.Month));
           travelerbyte.Add(Convert.ToByte(String.Format("{0:yy}", DateTime.Now)));
           travelerbyte.Add(Convert.ToByte(DateTime.Now.Hour));
           travelerbyte.Add(Convert.ToByte(DateTime.Now.Minute));
           travelerbyte.Add(Convert.ToByte(DateTime.Now.Second));
           //while (travelerbyte.Count < totalLen ) { travelerbyte.Add(0x00); }
           return travelerbyte;
       }

       public List<byte> GetTravelerWritebytes_3Phase(int TravelerType, int gdidIDX, DataGridView DGVParaLists)
       {
           //--Command Struct 0x09 len (2Byte EEPROM Address) (8 Byte Data - 1Byte-Stage,1Byte-Status,6-Byte Date & Time - DD MM YY HH MM SS)
           CommonMethods objcmnmethods = new CommonMethods();
           string StartAddress = objcmnmethods.GetTravelerAddress_3Phase(TravelerType);          
           List<byte> travelerbyte = new List<byte>();
           StartAddress = StartAddress.PadLeft(4, '0');          
           byte totalLen = objcmnmethods.GetTravelerBytes(TravelerType);            
           travelerbyte.Add((byte)Convert.ToInt32(StartAddress.Substring(0, 2), 16));
           travelerbyte.Add((byte)Convert.ToInt32(StartAddress.Substring(2, 2), 16));         
                  
           travelerbyte.Add((byte)(TravelerType + 1));//---Stage Number 1,2,3 or 4
           if (GetExecutionStatus(gdidIDX, DGVParaLists)) travelerbyte.Add(0x01);//---Status Fail:0, Pass:1
           else travelerbyte.Add(0x00);
           long dtbyte = Convert.ToInt64(String.Format("{0:yyyyMMddHHmmss}", DateTime.Now));
           travelerbyte.Add(Convert.ToByte((dtbyte & 0xFF0000000000) >> 40));
           travelerbyte.Add(Convert.ToByte((dtbyte & 0xFF00000000) >> 32));
           travelerbyte.Add(Convert.ToByte((dtbyte & 0xFF000000) >> 24));
           travelerbyte.Add(Convert.ToByte((dtbyte & 0xFF0000) >> 16));
           travelerbyte.Add(Convert.ToByte((dtbyte & 0xFF00) >> 8));
           travelerbyte.Add(Convert.ToByte(dtbyte & 0x00FF));
           while (travelerbyte.Count < totalLen + 2) { travelerbyte.Add(0x00); } //---2 Bytes for Address
           return travelerbyte;
       }

       public List<byte> GetTravelerWritebytes_1Phase(int TravelerType, int gdidIDX, DataGridView DGVParaLists)
       {
           //--Command Struct 0x09 len (2Byte EEPROM Address) (8 Byte Data - 1Byte-Stage,1Byte-Status,6-Byte Date & Time - DD MM YY HH MM SS)
           CommonMethods objcmnmethods = new CommonMethods();
           string StartAddress = objcmnmethods.GetTravelerAddress(TravelerType);
           List<byte> travelerbyte = new List<byte>();
           StartAddress = StartAddress.PadLeft(4, '0');
           byte totalLen = objcmnmethods.GetTravelerBytes(TravelerType);
           travelerbyte.Add((byte)Convert.ToInt32(StartAddress.Substring(0, 2), 16));
           travelerbyte.Add((byte)Convert.ToInt32(StartAddress.Substring(2, 2), 16));

           travelerbyte.Add((byte)(TravelerType + 1));//---Stage Number 1,2,3 or 4
           if (GetExecutionStatus(gdidIDX, DGVParaLists)) travelerbyte.Add(0x01);//---Status Fail:0, Pass:1
           else travelerbyte.Add(0x00);
           long dtbyte = Convert.ToInt64(String.Format("{0:yyyyMMddHHmmss}", DateTime.Now));
           travelerbyte.Add(Convert.ToByte((dtbyte & 0xFF0000000000) >> 40));
           travelerbyte.Add(Convert.ToByte((dtbyte & 0xFF00000000) >> 32));
           travelerbyte.Add(Convert.ToByte((dtbyte & 0xFF000000) >> 24));
           travelerbyte.Add(Convert.ToByte((dtbyte & 0xFF0000) >> 16));
           travelerbyte.Add(Convert.ToByte((dtbyte & 0xFF00) >> 8));
           travelerbyte.Add(Convert.ToByte(dtbyte & 0x00FF));
           while (travelerbyte.Count < totalLen + 2) { travelerbyte.Add(0x00); } //---2 Bytes for Address
           return travelerbyte;
       }

       public bool GetExecutionStatus(int executionRecCnt, DataGridView DGVParaLists)
       {           
           int string_length = 0;
           bool isNotAllTestPass = true;

           if (DGVParaLists.Rows[DGVParaLists.Rows.Count - 1].Cells["colParaName"].Value.ToString().Replace(" ", "") == "VERIFYMETERLOCK")
           {
               executionRecCnt--;
           }
         
           while (string_length < executionRecCnt)
           {
               string gridVal = DGVParaLists.Rows[string_length].Cells["colStatus"].Value.ToString();
               if (gridVal != "Pass") { isNotAllTestPass = false; break; }
               string_length++;
           }
           return isNotAllTestPass;
       }
       public bool ISMeterLockExecuted(int executionRecCnt, DataGridView DGVParaLists)
       {
           int string_length = 0;          
           while (string_length < executionRecCnt)
           {
               string gridVal = DGVParaLists.Rows[string_length].Cells[1].Value.ToString();
               if (gridVal.ToUpperInvariant().IndexOf("LOCKING") >= 0)
               {
                   if (DGVParaLists.Rows[string_length].Cells["colStatus"].Value.ToString().ToUpperInvariant() == "PASS") return true;
                   else return false;
               }
               string_length++;
           }
           return false;
       }
       
       public string ResettingCaseTamperCount()
       {
           try
           {
             List<byte> ResetBytes = new List<byte> { 0x00 };
             if (!objLI.WriteDataToMeter(DLMSDataStracture.ResetDataStracture.ResetValueAttribute, DLMSDataStracture.ResetDataStracture.ResetTamperOBIS, DLMSDataStracture.ResetDataStracture.ResetClassID, DLMSDataStracture.ResetDataStracture.ResetDataType, DLMSDataStracture.ResetDataStracture.ResetDataLength, ResetBytes, DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
             return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }     
       }

       public string CheckingCaseTamper()
       {
            int defcmdtimeout = GlobalObjects.objSerialComm.CommandTimeout;
            try
           {
                GlobalObjects.objSerialComm.CommandTimeout = 10000;
                List<byte> ResetBytes = new List<byte> { 0x00,0x01 };
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ResetDataStracture.ResetValueAttribute, DLMSDataStracture.ResetDataStracture.ResetTamperOBIS, DLMSDataStracture.ResetDataStracture.ResetClassID, DLMSDataStracture.ResetDataStracture.ResetDataType_Falcon2, DLMSDataStracture.ResetDataStracture.ResetDataLength_Falcon2, ResetBytes, DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
                   return (Read1PHMeterBuffer((int)StaticVariables.MeterBuffer1PHStatusFlg.CaseTamperCount, "0", "0", "0"));
               
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
            finally
            {
                GlobalObjects.objSerialComm.CommandTimeout =  defcmdtimeout;

            }
       }

       public string TestCalibrationData()
       {
           try
           {
               byte InitBasedCalibrationMethodCofficientLength = 0x30; //---New Calibration method for 1PSM falcon Cast Down meter variant Calibration cofficient Byte Length to detect new calibration method
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadCalibrationDataStracture.ReadCalibrationOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadCalibrationDataStracture.ReadCalibrationClassID, DLMSDataStracture.ReadCalibrationDataStracture.ReadCalibrationValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               if (GlobalObjects.objSerialComm.ReceiveBuffer[19] == InitBasedCalibrationMethodCofficientLength) return (IsValidCalibrationData_1PSMCostDown());
               else return(IsValidCalibrationData(GlobalObjects.objSerialComm.ReceiveBuffer));
               
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string VerifyCalibrationData()
       {
           try
           {
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.CalibrationDataStracture.CalibrationSetOBIS, txtboxobject, null, 1, DLMSDataStracture.CalibrationDataStracture.CalibrationClassID, DLMSDataStracture.CalibrationDataStracture.CalibrationValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               if (objappSettings.GetMeterMode() == (int)LayerInterface.MeterTypeInfo.SAPPHIRE_S2) return (IsValidCalibrationData_SapphireS2(GlobalObjects.objSerialComm.ReceiveBuffer));
               else return (IsValidCalibrationData_3Phase(GlobalObjects.objSerialComm.ReceiveBuffer));

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       /// <summary>
       /// Verify Meter Calibration data based on Standard DLMS command for 3Phase shappire meters
       /// </summary>
       /// <returns></returns>
       public string VerifyCalibrationDataShappireDLMSCommand()
       {
           try
           {
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.CalibrationDataStracture.CalibrationSetOBIS, txtboxobject, null, 1, DLMSDataStracture.CalibrationDataStracture.CalibrationClassID, DLMSDataStracture.CalibrationDataStracture.CalibrationValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return (IsValidCalibrationData_3PhaseShappireDLMSCommand(GlobalObjects.objSerialComm.ReceiveBuffer));

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       /// <summary>
       ///  Verify Meter Calibration data based on Non-DLMS command for 3Phase Non-AMI meters
       /// </summary>
       /// <param name="meterType"></param>
       /// <param name="isPumaOldMeter"></param>
       /// <returns></returns>
       public string TestCalibrationData_3PhaseDLMS(int meterType,bool isPumaOldMeter)
       {
            
           try
           {
               byte[] rec_pkt;
               System.Threading.Thread.Sleep(3000);
               if (meterType == (int)StaticVariables.ExecutedMeterType.DLMS_3PH)
               {
                   if (isPumaOldMeter) GlobalObjects.objSerialComm.NoOfBytesToBeReceive3PHDLMSCalibCoeff = 115;
                   else GlobalObjects.objSerialComm.NoOfBytesToBeReceive3PHDLMSCalibCoeff = 139;
               }
               if (meterType == (int)StaticVariables.ExecutedMeterType.DLMS_3PH_RUBY) GlobalObjects.objSerialComm.NoOfBytesToBeReceive3PHDLMSCalibCoeff = 589;
               if (meterType == (int)StaticVariables.ExecutedMeterType.SAPPHIRE)
               {
                   if (isPumaOldMeter) GlobalObjects.objSerialComm.NoOfBytesToBeReceive3PHDLMSCalibCoeff = 181;//for OLD Sapphire 181 
                   else GlobalObjects.objSerialComm.NoOfBytesToBeReceive3PHDLMSCalibCoeff = 145;
               }
               
               if (meterType == (int)StaticVariables.ExecutedMeterType.DLMS_3PH_RUBY)
               {
                   rec_pkt = objLI.Read3PHDLMSCalibCoeff(DLMSDataStracture.CalibrationSpeceficDataStruct_3Phase.Calibration_DLMSRUBYOBIS);
                   if (rec_pkt == null) { rec_pkt = objLI.Read3PHDLMSCalibCoeff(DLMSDataStracture.CalibrationSpeceficDataStruct_3Phase.Calibration_DLMSRUBYOBIS); }//retry Again
                   if (rec_pkt == null) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
                   return IsValidCalibrationData_3PHDlmsRUBY(rec_pkt, meterType);

               }
               else
               {
                   rec_pkt = objLI.Read3PHDLMSCalibCoeff(DLMSDataStracture.CalibrationSpeceficDataStruct_3Phase.Calibration_DLMSOBIS);
                   if (rec_pkt == null) { rec_pkt = objLI.Read3PHDLMSCalibCoeff(DLMSDataStracture.CalibrationSpeceficDataStruct_3Phase.Calibration_DLMSOBIS); }//retry Again
                   if (rec_pkt == null) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
                   return IsValidCalibrationData_3PHDlms(rec_pkt, meterType);

               }
              
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
           finally
           {
               GlobalObjects.objSerialComm.NoOfBytesToBeReceive3PHDLMSCalibCoeff = 0;                
           }
       }

       public string ReadCaseTamperCounter_MicrostarDLMS()
       {
           try
           {              

               TextBox[] texboxobject = new TextBox[] { /*txtPCBID*/ };
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadCaseTamperCounterDataStracture.ReadCaseTamperCounterOBIS, texboxobject, "0", 1M, DLMSDataStracture.ReadCaseTamperCounterDataStracture.ReadCaseTamperCounterClassID, DLMSDataStracture.ReadCaseTamperCounterDataStracture.ReadCaseTamperCounterValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }

               string[] datavalue = new string[2];
               string data = string.Empty;
               datavalue = DLMSDataStracture.DLMSDataFormator(GlobalObjects.objSerialComm.ReceiveBuffer, 18, true);
               if (datavalue == null) { }
               else data = datavalue[0];
               int iTamperCounter = Convert.ToInt32(data);
               if (iTamperCounter == 0) //Pass
               {
                   return "Case Tamper Counter =" + data;
               }
               else //Fail
               {
                   return StaticVariables.ERRORPreFix + "Case Tamper Counter =" + data;
               }
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string ReadCaseTamperCounter_MicrostarDLMS64K(string ipVerifyflagdelay)
       {
           int verifyingFlagDelayCount;
           try
           {
               if (!int.TryParse(ipVerifyflagdelay, out verifyingFlagDelayCount)) verifyingFlagDelayCount = 1;
               System.Threading.Thread.Sleep(verifyingFlagDelayCount * 1000);
               TextBox[] texboxobject = new TextBox[] { /*txtPCBID*/ };
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.CasteTamperDataStracture.CasteTamperOBIS, txtboxobject, "0", 100M, DLMSDataStracture.CasteTamperDataStracture.CasteTamperClassID, DLMSDataStracture.CasteTamperDataStracture.CasteTamperValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
              
               string[] datavalue = new string[2];
               string data = string.Empty;
               datavalue = DLMSDataStracture.DLMSDataFormator(GlobalObjects.objSerialComm.ReceiveBuffer, 18, true);
               if (datavalue == null) { }
               else data = datavalue[0];
               int iTamperCounter = Convert.ToInt32(data);
               if (iTamperCounter != 251) //Pass
               {
                   return "Case Tamper Event Code =" + data;
               }
               else //Fail
               {
                   return StaticVariables.ERRORPreFix + "Case Tamper Event Code =" + data;
               }
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string CheckingCaseTamper_MicrostarDLMS(string ipVerifyflagdelay)
       {
           try
           {
               int verifyingFlagDelayCount;
               string resetResp = ResettingCaseTamperCount();
               if (resetResp.Length <= 0)
               {
                   if (!int.TryParse(ipVerifyflagdelay, out verifyingFlagDelayCount)) verifyingFlagDelayCount = 1;
                   System.Threading.Thread.Sleep(verifyingFlagDelayCount * 1000);
                   return (ReadCaseTamperCounter_MicrostarDLMS());
               }
               else return resetResp;

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string CheckingCaseTamper_MicrostarDLMS64K(string ipVerifyflagdelay)
       {
           try
           {
               int verifyingFlagDelayCount;
               string resetResp = ResettingCaseTamperCount();
               if (resetResp.Length <= 0)
               {
                   if (!int.TryParse(ipVerifyflagdelay, out verifyingFlagDelayCount)) verifyingFlagDelayCount = 1;
                   System.Threading.Thread.Sleep(verifyingFlagDelayCount * 1000);
                   return (ReadCaseTamperCounter_MicrostarDLMS64K(ipVerifyflagdelay));
               }
               else return resetResp;

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private string IsValidCalibrationData_3PHDlms(byte[] receivedData,int meterType)
       {
           try
           {
               string strCalibCoeff = "";
               bool isdefaultCofficientExist = false;
               uint dwChar = 0;               
               int iIndex1 = 0, iIndex2 = 0, iIndex3 = 0;
               for (dwChar = 0; dwChar < GlobalObjects.objSerialComm.NoOfBytesToBeReceive3PHDLMSCalibCoeff; dwChar++)
               {
                   if (receivedData[dwChar] > 0x0F)
                   {
                       strCalibCoeff = strCalibCoeff + Convert.ToString(receivedData[dwChar], 16).ToUpper();
                   }
                   else
                   {
                       strCalibCoeff = strCalibCoeff + "0" + Convert.ToString(receivedData[dwChar], 16).ToUpper();
                   }
               }

               if (meterType == (int)StaticVariables.ExecutedMeterType.DLMS_3PH || meterType == (int)StaticVariables.ExecutedMeterType.SAPPHIRE)//GlobalVariable.Instance.g_bIsWCM_PUMA_DLMS_E250_3P4W_Selected) //WCM PUMA --BG
               {
                   iIndex1 = GlobalObjects.objSerialComm.NoOfBytesToBeReceive3PHDLMSCalibCoeff - 1;
                   iIndex1 = iIndex1 / 3;
                   iIndex2 = iIndex1 * 2;                  
                   iIndex3 = iIndex1 * 3;

                   //strPUMACalibCoeff = strPUMACalibCoeff + "38361F6DAF5D350E";
                   if ((receivedData[iIndex1 - 1] == receivedData[iIndex2 - 1]) && (receivedData[iIndex2 - 1] == receivedData[iIndex3 - 1]) && (receivedData[iIndex1 - 2] == receivedData[iIndex2 - 2]) && (receivedData[iIndex2 - 2] == receivedData[iIndex3 - 2]))
                   {
                       foreach (var item in StaticVariables.SapphireCalibrationCofficient)
                       {
                           if (strCalibCoeff.Contains(item)) { isdefaultCofficientExist = true; break; }
                       }
                       if (!isdefaultCofficientExist) return "Calibration Response >> " + strCalibCoeff; //---If default Cofficient Not Found then only PASS
                       
                       ////-----------PUMA WCM---------------------------
                       //if (!(strCalibCoeff.Contains("3836CC5150468C0A") || strCalibCoeff.Contains("3836652C8426DC05") || strCalibCoeff.Contains("38361F6DAF5D350E"))) //Not found - Pass
                       //{
                       //    return "Calibration Response >> " + strCalibCoeff; //Pass
                       //}
                       
                   }
               }
               else
               {

                   if (!(strCalibCoeff.Contains("9C2C9E11940CF401") || strCalibCoeff.Contains("38369E11940CF401"))) //Not found - Pass
                   {
                       return "Calibration Response >> " + strCalibCoeff; //Pass
                   }
               }
               return StaticVariables.ERRORPreFix + "Calibration Response >> " + strCalibCoeff; //FAIL

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

      
       private string IsValidCalibrationData_3PHDlmsRUBY(byte[] rec_pkt, int meterType)
       {
           try
           {
               uint uiChecksum = 0xFFFF;
               uint temp2Byte = 0;
               uint dwChar = 0;

               for (dwChar = 0; dwChar < 194; dwChar += 2)
               {
                   temp2Byte = (uint)((rec_pkt[dwChar + 1] << 8) | (rec_pkt[dwChar]));
                   uiChecksum = uiChecksum ^ temp2Byte;
               }
               temp2Byte = (uint)((rec_pkt[195] << 8) | (rec_pkt[194])); //--BCC Byte
               if (uiChecksum != temp2Byte) return StaticVariables.ERRORPreFix + "Calibration Response BCC =" + temp2Byte.ToString("X"); //Fail
               else  return "Calibration Response BCC =" + temp2Byte.ToString("X"); //Pass
               
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       
       public string TestCalibrationData_MicroStarDLMS(string meterSignInfo)
       {
           try
           {
               TextBox[] texboxobject = new TextBox[] { };
          //     if (meterSignInfo.Contains("VB")) //-------VIM 64K Specefic
               if (ISSinglePhaseDLMS64KMeterVariants(meterSignInfo))
               {
                   List<byte> parabyte = GetReadEEPROMStructByte(0xFFFE, 49);
                   if (!objLI.ReadBlockFromMeter(DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMOBIS, texboxobject, "0", 1M, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMClassID, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMValueAttribute, DLMSDataStracture.DataStractureAccessSelector.Range_descriptor, parabyte)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }

               }
               else
               {
                   if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadCalibrationDataStracture.ReadCalibrationOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadCalibrationDataStracture.ReadCalibrationClassID, DLMSDataStracture.ReadCalibrationDataStracture.ReadCalibrationValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               }
                   return (IsValidCalibrationData_MicroStarDLMS(GlobalObjects.objSerialComm.ReceiveBuffer));

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public string VerifyTraveler_VIMDLMS(int defaultVal)
       {
           try
           {
                  TextBox[] texboxobject = new TextBox[] { };
                  List<byte> parabyte;
                   string StatusValues = "";
                   int stageFlag = -1;                   
                   if (defaultVal == 1) //---Verify only for FT
                   {
                       parabyte = GetReadEEPROMStructByte(0x000F, 1);
                       if (!objLI.ReadBlockFromMeter(DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMOBIS, texboxobject, "0", 1M, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMClassID, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMValueAttribute, DLMSDataStracture.DataStractureAccessSelector.Range_descriptor, parabyte)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
                        if(GlobalObjects.objSerialComm.ReceiveBuffer[20] == 0x31) StatusValues = "Status=" + (GlobalObjects.objSerialComm.ReceiveBuffer[20]-0x30).ToString() + ", ";
                        else StatusValues = StaticVariables.ERRORPreFix + "Invalid Status=" + GlobalObjects.objSerialComm.ReceiveBuffer[20].ToString() + ", ";
                   }
                   parabyte = GetReadEEPROMStructByte(0x0028, 1);
                   if (!objLI.ReadBlockFromMeter(DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMOBIS, texboxobject, "0", 1M, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMClassID, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMValueAttribute, DLMSDataStracture.DataStractureAccessSelector.Range_descriptor, parabyte)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }

                   stageFlag = GlobalObjects.objSerialComm.ReceiveBuffer[20];
                   if (defaultVal == 1) //-----FT Stage
                   {
                       if (stageFlag == 0x39)
                       {
                           if (stageFlag >= 0x30) return StatusValues + StaticVariables.ERRORPreFix + "Invalid Stage=" + (stageFlag - 0x30).ToString();//--Fail only if found 9, Specefic Case
                           else return StatusValues + StaticVariables.ERRORPreFix + "Invalid Stage=" + (stageFlag).ToString();//--Fail only if found 9, Specefic Case

                       }
                   }
                   else if (defaultVal == 2)//----Calibration Stage
                   {
                       if (stageFlag < 0x33)
                       {
                           if (stageFlag >= 0x30) return StatusValues + StaticVariables.ERRORPreFix + "Invalid Stage=" + (stageFlag - 0x30).ToString();
                           else return StatusValues + StaticVariables.ERRORPreFix + "Invalid Stage=" + (stageFlag).ToString();
                       }
                   }
                   else if (defaultVal == 3)//----Serialization Stage
                   {
                       if (stageFlag < 0x35)
                       {
                           if (stageFlag >= 0x30) return StatusValues + StaticVariables.ERRORPreFix + "Invalid Stage=" + (stageFlag - 0x30).ToString();
                           else return StatusValues + StaticVariables.ERRORPreFix + "Invalid Stage=" + (stageFlag).ToString();
                       }
                   }
                  if(stageFlag >= 0x30) return StatusValues + "Stage=" + (stageFlag-0x30).ToString();
                  else return StatusValues + "Stage=" + (stageFlag).ToString();

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public bool SetTravelerData_VIMDLMS(int defaultVal)
       {
            try
           {

               List<byte> ResetBytes = new List<byte> { 0x00};
               if (defaultVal == 0) ResetBytes = WriteEEPROMStructByte(0x0028, 0x31);      //---EMS Test
               else if (defaultVal == 1) ResetBytes = WriteEEPROMStructByte(0x0028, 0x33);  //---FT Test
               else if (defaultVal == 2) ResetBytes = WriteEEPROMStructByte(0x0028, 0x35);  //---Calibration Test
               else if (defaultVal == 3) ResetBytes = WriteEEPROMStructByte(0x0028, 0x37);  //---Serialization Test
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMValueAttribute, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMOBIS, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMClassID, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMDataType, DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMDataLength, ResetBytes, DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return false; }
               else return true;
           }
           catch (Exception)
           {
               return false;
           }

       }
       private List<byte> GetReadEEPROMStructByte(int CurrentReadAddress, int BytestoRead)
       {

           int noofBytes = BytestoRead;
           string StartAddress = CurrentReadAddress.ToString("X");
           StartAddress = StartAddress.PadLeft(4, '0');

           List<byte> EEPROMbyte = new List<byte>();
           EEPROMbyte.Add(DLMSDataStracture.DataStractureAccessSelector.Range_descriptor);
           EEPROMbyte.Add(DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMDataType);
           EEPROMbyte.Add(DLMSDataStracture.ReadEEPROMDataStracture.ReadEEPROMDataLength);


           EEPROMbyte.Add(0x12);//double-long-unsigned
           EEPROMbyte.Add((byte)Convert.ToInt32(StartAddress.Substring(0, 2), 16));
           EEPROMbyte.Add((byte)Convert.ToInt32(StartAddress.Substring(2, 2), 16));

           EEPROMbyte.Add(0x12);//double-long-unsigned
           EEPROMbyte.Add(Convert.ToByte((noofBytes & 0xFF00) >> 8));
           EEPROMbyte.Add(Convert.ToByte(noofBytes & 0x00FF));

           return EEPROMbyte;


       }
       private List<byte> WriteEEPROMStructByte(int CurrentReadAddress, int BytestoRead)
       {

           int noofBytes = BytestoRead;
           string StartAddress = CurrentReadAddress.ToString("X");
           StartAddress = StartAddress.PadLeft(4, '0');

           List<byte> EEPROMbyte = new List<byte>();
           
           EEPROMbyte.Add(0x12);//double-long-unsigned
           EEPROMbyte.Add((byte)Convert.ToInt32(StartAddress.Substring(0, 2), 16));
           EEPROMbyte.Add((byte)Convert.ToInt32(StartAddress.Substring(2, 2), 16));

           EEPROMbyte.Add(0x09);//double-long-unsigned
           EEPROMbyte.Add(0x01);//double-long-unsigned
           EEPROMbyte.Add(Convert.ToByte(noofBytes));
        
           return EEPROMbyte;


       }
       public string ResetBilling(int meterType)
       {
           byte ResetDataType = DLMSDataStracture.ResetDataStracture.ResetDataType;
           byte ResetDataLength = DLMSDataStracture.ResetDataStracture.ResetDataLength;
           List<byte> ResetBytes = new List<byte> { 0x00 };
           try
           {
               if (meterType == (int)StaticVariables.ExecutedMeterType.Smart_Meter_1PH || meterType == (int)StaticVariables.ExecutedMeterType.Smart_Meter_3PH)
               {
                   ResetBytes = new List<byte> { 0x00, 0x01 };
                   ResetDataType = DLMSDataStracture.ResetDataStracture.ResetDataType_Falcon2;
                   ResetDataLength = DLMSDataStracture.ResetDataStracture.ResetDataLength_Falcon2;
               }
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ResetDataStracture.ResetValueAttribute, DLMSDataStracture.ResetDataStracture.ResetBillingOBIS, DLMSDataStracture.ResetDataStracture.ResetClassID, ResetDataType, ResetDataLength, ResetBytes, DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string ResetAllData(int meterType)
       {
           int defcmdtimeout = GlobalObjects.objSerialComm.CommandTimeout;
           byte ResetDataType = DLMSDataStracture.ResetDataStracture.ResetDataType;
           byte ResetDataLength = DLMSDataStracture.ResetDataStracture.ResetDataLength;
           List<byte> ResetBytes = new List<byte> { 0x00 };
           try
           {
               GlobalObjects.objSerialComm.CommandTimeout = 60000;//----All Data Resets 30000
               if (meterType == (int)StaticVariables.ExecutedMeterType.Smart_Meter_1PH || meterType == (int)StaticVariables.ExecutedMeterType.Smart_Meter_3PH)
               {
                   
                   ResetBytes = new List<byte> { 0x00, 0x01 };
                   ResetDataType = DLMSDataStracture.ResetDataStracture.ResetDataType_Falcon2;
                   ResetDataLength = DLMSDataStracture.ResetDataStracture.ResetDataLength_Falcon2;
               }
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ResetDataStracture.ResetValueAttribute, DLMSDataStracture.ResetDataStracture.ResetALLOBIS, DLMSDataStracture.ResetDataStracture.ResetClassID, ResetDataType, ResetDataLength, ResetBytes, DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
           finally
           {
               GlobalObjects.objSerialComm.CommandTimeout = defcmdtimeout;
           }
       }
       /// <summary>
       /// Specefic to Sapphire S2 To Reset All Data in quick reset mode
       /// </summary>
       /// <returns></returns>
       public string QuickResetAllData()
       {
           byte ResetDataType = DLMSDataStracture.ResetDataStracture.ResetDataType;
           byte ResetDataLength = DLMSDataStracture.ResetDataStracture.ResetDataLength;
           List<byte> ResetBytes = new List<byte> { 0x02 };
           try
           {
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ResetDataStracture.ResetValueAttribute, DLMSDataStracture.ResetDataStracture.ResetALLOBIS, DLMSDataStracture.ResetDataStracture.ResetClassID, ResetDataType, ResetDataLength, ResetBytes, DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               //--Due to Firmware issue , Reset must be done two time to ensure all data reset using this fast command for Sapphire S2
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ResetDataStracture.ResetValueAttribute, DLMSDataStracture.ResetDataStracture.ResetALLOBIS, DLMSDataStracture.ResetDataStracture.ResetClassID, ResetDataType, ResetDataLength, ResetBytes, DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="IsLocking" --> TRUE-->1/FALSE-->0></param>
       /// <returns></returns>
       public string JTagLocking(bool IsLocking)
       {
           List<byte> JtagLockingBytes = new List<byte> { 0x00 };
           try
           {
               if (IsLocking) JtagLockingBytes = new List<byte> { 0x01 }; //---Locking Request
               if (!objLI.WriteDataToMeter(DLMSDataStracture.JTAGLockingDataStructure.JTAGLockingValueAttribute, DLMSDataStracture.JTAGLockingDataStructure.JTAGLockingOBIS, DLMSDataStracture.JTAGLockingDataStructure.JTAGLockingClassID, DLMSDataStracture.JTAGLockingDataStructure.JTAGLockingDataType, DLMSDataStracture.JTAGLockingDataStructure.JTAGLockingDataLength, JtagLockingBytes, DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
           
       }
        public string SetLoadControlDefaultConfigurations(int meterType)
        {
            List<byte> loadControlBytes;
            try
            {
                if(meterType == (int)StaticVariables.ExecutedMeterType.Smart_Meter_1PH) loadControlBytes = new List<byte> { 0x04, 0x20, 0x00, 0x00, 0x00, 0x00, 0x11, 0x05, 0x11, 0x01, 0x11, 0x1E, 0x11, 0x00, 0x11, 0x0A }; //---01 00 00 60 03 80 FF 02 00 0206 04 20 00 00 00 00 11 05 11 01 11 1E 11 00 11 0A
                else return StaticVariables.ERRORPreFix + "Non Supported Test point";
                if (!objLI.WriteDataToMeter(DLMSDataStracture.LoadControlDataStracture.LoadControlValueAttribute, DLMSDataStracture.LoadControlDataStracture.LoadControlOBIS, DLMSDataStracture.LoadControlDataStracture.LoadControlClassID, DLMSDataStracture.LoadControlDataStracture.LoadControlDataType, DLMSDataStracture.LoadControlDataStracture.LoadControlDataLength, loadControlBytes, DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
                return "";
            }
            catch (Exception ex)
            {
                return StaticVariables.ERRORPreFix + ex.Message;
            }

        }
        public string ResetBatteryCounter()
       {
            
           try
           {              
               List<byte> ResetBytes = new List<byte> { 0x00 };
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ResetDataStracture.ResetValueAttribute, DLMSDataStracture.ResetDataStracture.ResetBatteryCounterResetOBIS, DLMSDataStracture.ResetDataStracture.ResetClassID, DLMSDataStracture.ResetDataStracture.ResetDataType, DLMSDataStracture.ResetDataStracture.ResetDataLength, ResetBytes, DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
           
       }
       public string ResetBatteryCounterS2()
       {

           try
           {
               byte ResetDataType = DLMSDataStracture.ResetDataStracture.ResetDataType;
               byte ResetDataLength = DLMSDataStracture.ResetDataStracture.ResetDataLength;
               List<byte> ResetBytes = new List<byte> { 0x09 };
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ResetDataStracture.ResetValueAttribute, DLMSDataStracture.ResetDataStracture.ResetALLOBIS, DLMSDataStracture.ResetDataStracture.ResetClassID, ResetDataType, ResetDataLength, ResetBytes, DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }
       public string ResetLowBatt()
       {
           
           try
           {               
               List<byte> ResetBytes = new List<byte> { 0x00 };
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ResetDataStracture.ResetValueAttribute, DLMSDataStracture.ResetDataStracture.ResetLowBattOBIS, DLMSDataStracture.ResetDataStracture.ResetClassID, DLMSDataStracture.ResetDataStracture.ResetDataType, DLMSDataStracture.ResetDataStracture.ResetDataLength, ResetBytes, DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
           
       }
       public string ResettingCaseTamper(bool IsFrontTested)
       {
           int defcmdtimeout = GlobalObjects.objSerialComm.CommandTimeout;
           string caseRes = "";
           try
           {
              if (!objLI.ReadByteFromMeter(DLMSDataStracture.CasteTamperDataStracture.CasteTamperOBIS, txtboxobject, "0", 1M, DLMSDataStracture.CasteTamperDataStracture.CasteTamperClassID, DLMSDataStracture.CasteTamperDataStracture.CasteTamperValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
              caseRes = IsValidCaseFlag(GlobalObjects.objSerialComm.ReceiveBuffer, false);
              if (!IsFrontTested && caseRes.IndexOf(StaticVariables.ERRORPreFix) >= 0) return caseRes; //-------If Already Tested and is PASS No Need To Pre Test
                
               GlobalObjects.objSerialComm.CommandTimeout = 30000;//----All Data Resets
               List<byte> ResetBytes = new List<byte> { 0x00 };
               byte[] resetOBIS = DLMSDataStracture.ResetDataStracture.ResetTamperOBIS;
               if (objappSettings.GetMeterMode() == (int)StaticVariables.ExecutedMeterType.SAPPHIRE_S2) { ResetBytes = new List<byte> { 0x02 }; resetOBIS = DLMSDataStracture.ResetDataStracture.ResetALLOBIS; }// DATA RESET FOR PRODUCTION (New Implementation)for Sapphire S2 with same Reaset All OBIS
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ResetDataStracture.ResetValueAttribute, resetOBIS, DLMSDataStracture.ResetDataStracture.ResetClassID, DLMSDataStracture.ResetDataStracture.ResetDataType, DLMSDataStracture.ResetDataStracture.ResetDataLength, ResetBytes, DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               System.Threading.Thread.Sleep(1000);
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.CasteTamperDataStracture.CasteTamperOBIS, txtboxobject, "0", 1M, DLMSDataStracture.CasteTamperDataStracture.CasteTamperClassID, DLMSDataStracture.CasteTamperDataStracture.CasteTamperValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               caseRes = IsValidCaseFlag(GlobalObjects.objSerialComm.ReceiveBuffer, true);
               return caseRes;                
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
           finally
           {
               GlobalObjects.objSerialComm.CommandTimeout = defcmdtimeout;
           }
       }
       
       private string IsValidCaseFlag(byte[] receivedData, bool isafterReset)
       {
           try
           {
               string verificationResponse = "";
               int compValue = 0;
               compValue = (compValue | (int)receivedData[19]) << 8;
               compValue = (compValue | (int)receivedData[20]);
               if (isafterReset)
               {
                   if (compValue <= 0) verificationResponse = "Case Counter =" + compValue.ToString();
                   else verificationResponse = StaticVariables.ERRORPreFix + "Case Detected =" + compValue.ToString();
               }
               else 
               {
                   if (compValue > 0) verificationResponse = "Case Counter =" + compValue.ToString();
                   else
                   {
                       verificationResponse = StaticVariables.ERRORPreFix + "Case Not Detected, Switch May Damage =" + compValue.ToString();
                   }
               }
               return verificationResponse;
               
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message + "Case Flag =" + receivedData[20];
           }
       }

       public string WrittingMeterID(string meterID)
       {
           try
           { 
               TextBox[] texboxobject = new TextBox[] { };
               if (!objLI.WriteDataToMeter(DLMSDataStracture.MeterIDDataStracture.MeterIDValueAttribute, DLMSDataStracture.MeterIDDataStracture.MeterIDOBIS, DLMSDataStracture.MeterIDDataStracture.MeterIDClassID, DLMSDataStracture.MeterIDDataStracture.MeterIDDataType, DLMSDataStracture.MeterIDDataStracture.MeterIDDataLength, GetMeterIDByte(meterID, 16), DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }
       public string WrittingMeterID_SmartMeter(string meterID)
       {
           try
           {
               TextBox[] texboxobject = new TextBox[] { };
               List<byte> meterIDBytes = GetMeterIDByte(meterID, meterID.Length);
               if (!objLI.WriteDataToMeter(DLMSDataStracture.MeterIDDataStracture.MeterIDValueAttribute, DLMSDataStracture.MeterIDDataStracture.MeterIDOBIS, DLMSDataStracture.MeterIDDataStracture.MeterIDClassID, DLMSDataStracture.MeterIDDataStracture.MeterIDDataType, (byte)meterIDBytes.Count, meterIDBytes, DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }

       public string WrittingMeterID_3Phase(string meterID)
       {
           try
           {
               TextBox[] texboxobject = new TextBox[] { };
               if (!objLI.WriteDataToMeter(DLMSDataStracture.MeterIDDataStracture.MeterIDValueAttribute, DLMSDataStracture.MeterIDDataStracture.MeterIDOBIS, DLMSDataStracture.MeterIDDataStracture.MeterIDClassID, DLMSDataStracture.MeterIDDataStracture.MeterIDDataType_3Phase, (byte)meterID.Length, GetMeterIDByte(meterID, meterID.Length), DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed, Meter ID Length May Invalid."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }

       public string WrittingMeterID_3PhaseSapphire(string meterID)
       {
           try
           {
               TextBox[] texboxobject = new TextBox[] { };
               if (!objLI.WriteDataToMeter(DLMSDataStracture.MeterIDDataStracture.MeterIDValueAttribute, DLMSDataStracture.MeterIDDataStracture.MeterIDOBIS, DLMSDataStracture.MeterIDDataStracture.MeterIDClassID, DLMSDataStracture.MeterIDDataStracture.MeterIDDataType, (byte)meterID.Length, GetMeterIDByte(meterID, meterID.Length), DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed, Meter ID Length May Invalid."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }        

       public string WrittingManufacturingYear(string mfyear)
       {
           try
           {
               TextBox[] texboxobject = new TextBox[] { };
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ManufactureYearDataStracture.ManufactureYearValueAttribute, DLMSDataStracture.ManufactureYearDataStracture.ManufactureYearOBIS, DLMSDataStracture.ManufactureYearDataStracture.ManufactureYearClassID, DLMSDataStracture.ManufactureYearDataStracture.ManufactureYearDataType, DLMSDataStracture.ManufactureYearDataStracture.ManufactureYearDataLength, GetManufactureYearBytes( mfyear), DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string VerifyManufacturingYear(string mfyear)
       {
           try
           {
               TextBox[] texboxobject = new TextBox[] { };
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ManufactureYearDataStracture.ManufactureYearOBIS, txtboxobject, "0", 1M, DLMSDataStracture.ManufactureYearDataStracture.ManufactureYearClassID, DLMSDataStracture.ManufactureYearDataStracture.ManufactureYearValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return IsValidManufactureYear(GlobalObjects.objSerialComm.ReceiveBuffer, mfyear);  
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string WriteManufacturingMonth(string mfgMonth)
       {
           try
           {
               int result=-1;
               if(!(int.TryParse(mfgMonth, out result) && result > 0 && result < 13))
                 return StaticVariables.ERRORPreFix + " Please provide Valid Month!";
               TextBox[] texboxobject = new TextBox[] { };
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ManufactureMonthDataStracture.ManufactureMonthValueAttribute, DLMSDataStracture.ManufactureMonthDataStracture.ManufactureMonthOBIS, DLMSDataStracture.ManufactureMonthDataStracture.ManufactureMonthClassID, DLMSDataStracture.ManufactureMonthDataStracture.ManufactureMonthDataType, DLMSDataStracture.ManufactureMonthDataStracture.ManufactureMonthDataLength, GetManufactureMonthBytes(mfgMonth), DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; };
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string VerifyManufacturingMonth(string mfgMonth)
       {
           try
           {
               int result = -1;
               if (!(int.TryParse(mfgMonth, out result) && result > 0 && result < 13))
                   return StaticVariables.ERRORPreFix + " Please provide Valid Month!";
               TextBox[] texboxobject = new TextBox[] { };
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ManufactureMonthDataStracture.ManufactureMonthOBIS, txtboxobject, "0", 1M, DLMSDataStracture.ManufactureMonthDataStracture.ManufactureMonthClassID, DLMSDataStracture.ManufactureMonthDataStracture.ManufactureMonthValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; };
               return IsValidManufactureMonth(GlobalObjects.objSerialComm.ReceiveBuffer, mfgMonth);
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string VerifyCurrentRating(string currentRating)
       {
           try
           {
               TextBox[] texboxobject = new TextBox[] { };
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.CurrentRatingDataStracture.CurrentRatingOBIS, txtboxobject, "0", 1M, DLMSDataStracture.CurrentRatingDataStracture.CurrentRatingClassID, DLMSDataStracture.CurrentRatingDataStracture.CurrentRatingValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return IsValidCurrentRating(GlobalObjects.objSerialComm.ReceiveBuffer, currentRating);
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public string VerifyRS485Address(string deviceAddress)
       {
           try
           {
               TextBox[] texboxobject = new TextBox[] { };
               //---------------------------Checking that Meter Must me RS485 Type-------------------------
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.MeterTypeDataStructure.MeterTypeOBIS, txtboxobject, "0", 1M, DLMSDataStracture.MeterTypeDataStructure.MeterTypeClassID, DLMSDataStracture.MeterTypeDataStructure.MeterTypeValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               if (GlobalObjects.objSerialComm.ReceiveBuffer[19] != 0x01) return StaticVariables.ERRORPreFix + "Non-RS485 Meter Configuration=" + GlobalObjects.objSerialComm.ReceiveBuffer[19].ToString();
               //---------------------------Verifying RS485 Device address---------------------------------
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.RS485DeviceAddressDataStracture.RS485DeviceaddressOBIS, txtboxobject, "0", 1M, DLMSDataStracture.RS485DeviceAddressDataStracture.RS485DeviceaddressClassID, DLMSDataStracture.RS485DeviceAddressDataStracture.RS485DeviceaddressValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return IsValidRS485DeviceAddress(GlobalObjects.objSerialComm.ReceiveBuffer, deviceAddress);
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public string VerifyDeviceType(string deviceType)
       {
           try
           {
               string deviceVariants = "RS232";
               TextBox[] texboxobject = new TextBox[] { };               
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.MeterTypeDataStructure.MeterTypeOBIS, txtboxobject, "0", 1M, DLMSDataStracture.MeterTypeDataStructure.MeterTypeClassID, DLMSDataStracture.MeterTypeDataStructure.MeterTypeValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               if (GlobalObjects.objSerialComm.ReceiveBuffer[19] == 0x01) deviceVariants = "RS485";
               if (deviceType.ToUpperInvariant() == deviceVariants) return "Device Is=" + deviceVariants + ", Meter Flag=" + GlobalObjects.objSerialComm.ReceiveBuffer[19].ToString();
               return StaticVariables.ERRORPreFix + "Device Is=" + deviceVariants + ", Meter Flag=" + GlobalObjects.objSerialComm.ReceiveBuffer[19].ToString();
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public string WritePmaxValue(string PmaxValue)
       {
          
           try
           {
               Int32 pMax;
               if (!Int32.TryParse(PmaxValue, out pMax)) return StaticVariables.ERRORPreFix + "Pmax Value Should be Numeric Only !";
               if (pMax < 20 || pMax > 90) return StaticVariables.ERRORPreFix + "Pmax Value Should be 20-90"; 

               TextBox[] texboxobject = new TextBox[] { };
               List<byte> pmaxbytes=new List<byte>{Convert.ToByte(pMax)};
               if (!objLI.WriteDataToMeter(DLMSDataStracture.PmaxDataStracture.PmaxValueAttribute, DLMSDataStracture.PmaxDataStracture.PmaxOBIS, DLMSDataStracture.PmaxDataStracture.PmaxClassID, DLMSDataStracture.PmaxDataStracture.PmaxDataType, DLMSDataStracture.PmaxDataStracture.PmaxDataLength, pmaxbytes, DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public string WrittingDefaultMeterPassword_SmartMeter(string RefMasterKey)
       {
           try
           {
               string RefUSPassword = "000102030405060708090A0B0C0D0E0F"; //Default US Password in 16 Byte HES
               string refMRPassword = "3131313131313131"; //Default MR Password in 8 Byte ASCII
               string refEncryptionKey = "000102030405060708090A0B0C0D0E0F"; //Default MR Password in 8 Byte ASCII
               string RefFUPassword = "000102030405060708090A0B0C0D0E0F"; //Default FU Password in 16 Byte HES
               string refMasterKey = RefMasterKey;
              
               TextBox[] texboxobject = new TextBox[] { };
               //--MR
               if (!objLI.WriteDataToMeter(DLMSDataStracture.LNAssociationDataStracture.LNAssociationValueAttribute, DLMSDataStracture.LNAssociationDataStracture.LNAssociationOBIS_MR, DLMSDataStracture.LNAssociationDataStracture.LNAssociationClassID, DLMSDataStracture.LNAssociationDataStracture.LNAssociationDataType_LLS, DLMSDataStracture.LNAssociationDataStracture.LNAssociationDataLength_LLS, DLMSDataStracture.GetPasswordBytes(refMRPassword, "", ""), DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "Writting LLS Failed."; }
               //--US
               if (!objLI.WriteDataToMeter(DLMSDataStracture.LNAssociationDataStracture.LNAssociationValueAttribute_Method, DLMSDataStracture.LNAssociationDataStracture.LNAssociationOBIS_US, DLMSDataStracture.LNAssociationDataStracture.LNAssociationClassID, DLMSDataStracture.LNAssociationDataStracture.LNAssociationDataType_HLS, DLMSDataStracture.LNAssociationDataStracture.LNAssociationDataLength_HLS, DLMSDataStracture.GetPasswordBytes(RefUSPassword, "", ""), DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "Writting HLS Failed."; }
               //--FU
               if (!objLI.WriteDataToMeter(DLMSDataStracture.LNAssociationDataStracture.LNAssociationValueAttribute_Method, DLMSDataStracture.LNAssociationDataStracture.LNAssociationOBIS_FU, DLMSDataStracture.LNAssociationDataStracture.LNAssociationClassID, DLMSDataStracture.LNAssociationDataStracture.LNAssociationDataType_HLS, DLMSDataStracture.LNAssociationDataStracture.LNAssociationDataLength_HLS, DLMSDataStracture.GetPasswordBytes(RefFUPassword, "", ""), DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "Writting FU Failed."; }
               //--Encryption Key
               if (!objLI.WriteDataToMeter(DLMSDataStracture.SecuritySetupDataStracture.SecuritySetupValueAttribute_Write, DLMSDataStracture.SecuritySetupDataStracture.SecuritySetupOBIS_US, DLMSDataStracture.SecuritySetupDataStracture.SecuritySetupClassID, DLMSDataStracture.SecuritySetupDataStracture.SecuritySetupDataType, DLMSDataStracture.SecuritySetupDataStracture.SecuritySetupDataLength, DLMSDataStracture.GetPasswordBytes("", refEncryptionKey, refMasterKey), DLMSDataStracture.DataStractureRequest.ActionRequest_Normal)) { return StaticVariables.ERRORPreFix + "Writting EK Failed."; }

               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }


       private string IsValidManufactureYear(byte[] receivedData,string refMfgYear)
       {
           try
           {
               int compValue = 0;
               compValue = (compValue | (int)receivedData[19]) << 8;
               compValue = (compValue | (int)receivedData[20]);
               if (compValue.ToString("d4") == refMfgYear) return "Meter Mfg Year =" + compValue.ToString("d4");
               else return StaticVariables.ERRORPreFix + "Meter Mfg Year =" + compValue.ToString("d4");
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private string IsValidManufactureMonth(byte[] receivedData, string refMfgMonth)
       {
           try
           {
               int compValue = 0;
               compValue = (int)receivedData[19];
               if (compValue == int.Parse(refMfgMonth)) return "Meter Mfg Month =" + compValue.ToString("D2");
               else return StaticVariables.ERRORPreFix + "Meter Mfg Month =" + compValue.ToString("D2");
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private string IsValidCurrentRating(byte[] receivedData, string currentRating)
       {
           try
           {
               
               string datavalue = string.Empty;
               string[] datavaluearr = new string[2];
               datavaluearr = DLMSDataStracture.DLMSDataFormator(receivedData, 18, true);
               if (datavalue != null)
               {
                   datavalue = datavaluearr[0].Trim();
                   if (datavalue == currentRating) return "Meter Rating =" + datavalue;
                   else return StaticVariables.ERRORPreFix + "Meter Rating =" + datavalue;
               }
               return StaticVariables.ERRORPreFix + "Invalid Response";
                
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       private string IsValidRS485DeviceAddress(byte[] receivedData, string deviceAddress)
       {
           try
           {

               int compValue = 0;
               compValue = (compValue | (int)receivedData[19]) << 8;
               compValue = (compValue | (int)receivedData[20]);
               string RS485DeviceAddress = Convert.ToString(compValue);
               if (RS485DeviceAddress == deviceAddress) return "RS485 Device Address =" + RS485DeviceAddress;
               else return StaticVariables.ERRORPreFix + "Invalid RS485 Device Address =" + RS485DeviceAddress;

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public string VerifyMeterID(string scanMeterID)
       {
           try
           {
               TextBox[] texboxobject = new TextBox[] { };
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.MeterIDDataStracture.MeterIDOBIS, texboxobject, "0", 1M, DLMSDataStracture.MeterIDDataStracture.MeterIDClassID, DLMSDataStracture.MeterIDDataStracture.MeterIDValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return IsValidMeterID(GlobalObjects.objSerialComm.ReceiveBuffer, scanMeterID);
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private string IsValidMeterID(byte[] receivedData, string scanMeterID)
       {
           try
           {
               string datavalue = string.Empty;
               string[] datavaluearr = new string[2];
               datavaluearr = DLMSDataStracture.DLMSDataFormator(receivedData, 18, true);
               if (datavalue != null)
               {
                   datavalue = datavaluearr[0];
                   if (datavalue == scanMeterID) return "Meter ID =" + datavalue;
                   else return StaticVariables.ERRORPreFix + "Meter ID =" + datavalue;
               }
               return StaticVariables.ERRORPreFix + "Invalid Response.";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string WriteRefVoltage(string refVoltage)
       {
           try
           {
               TextBox[] texboxobject = new TextBox[] { };
               if (!objLI.WriteDataToMeter(DLMSDataStracture.ReferenceVoltageDataStracture.ReadRefVoltageValueAttribute, DLMSDataStracture.ReferenceVoltageDataStracture.ReferenceVoltageOBIS, DLMSDataStracture.ReferenceVoltageDataStracture.RefVoltageClassID, DLMSDataStracture.ReferenceVoltageDataStracture.ReferenceVoltageDataType, DLMSDataStracture.ReferenceVoltageDataStracture.ReferenceVoltageDataLength, RefVoltageByte(refVoltage), DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
              return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

        public List<byte> RefVoltageByte(string refVoltage)
        {
            List<byte> RefVolbyte = new List<byte>();
            RefVolbyte.Add(byte.Parse(refVoltage));
            return RefVolbyte;
        }

       public string WriteCuttentRating(string CurrentRatingIB, string CurrentRatingIMax,byte parameterLength)
       {
           try
           {
               TextBox[] texboxobject = new TextBox[] { };
               if (!objLI.WriteDataToMeter(DLMSDataStracture.CurrentRatingDataStracture.CurrentRatingValueAttribute, DLMSDataStracture.CurrentRatingDataStracture.CurrentRatingOBIS, DLMSDataStracture.CurrentRatingDataStracture.CurrentRatingClassID, DLMSDataStracture.CurrentRatingDataStracture.CurrentRatingDataType, parameterLength, GetCurrentRatingByte(CurrentRatingIB, CurrentRatingIMax, parameterLength), DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string LockingMeter(byte lockByte )
       {
           try
           {
               if (!objLI.WriteDataToMeter(DLMSDataStracture.MeterSoftwareLockDataStracture.MeterSoftwareLockValueAttribute, DLMSDataStracture.MeterSoftwareLockDataStracture.MeterSoftwareLockOBIS, DLMSDataStracture.MeterSoftwareLockDataStracture.MeterSoftwareLockClassID, DLMSDataStracture.MeterSoftwareLockDataStracture.MeterSoftwareLockDataType, DLMSDataStracture.MeterSoftwareLockDataStracture.MeterSoftwareLockDataLength, GetMeterLockConfigByte(lockByte), DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               if (lockByte == 0xFF)
               {
                   return VerifyMeterLock();
               }
               else
               {
                   return "";
               }
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string LockingMeter_3Phase(byte lockByte)
       {
           try
           {               
               if (!objLI.WriteDataToMeter(DLMSDataStracture.MeterLOCKDataStracture.MeterLOCKValueAttribute, DLMSDataStracture.MeterLOCKDataStracture.MeterLOCKOBIS, DLMSDataStracture.MeterLOCKDataStracture.MeterLOCKClassID, DLMSDataStracture.MeterLOCKDataStracture.MeterLOCKDataType, DLMSDataStracture.MeterLOCKDataStracture.MeterLOCKDataLength, GetMeterLockConfigByte(lockByte), DLMSDataStracture.DataStractureRequest.SetRequest_Normal)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }

       private List<byte> GetMeterLockConfigByte(byte lockByte)
       {
           List<byte> MeterLockConfigbyte = new List<byte>();
           MeterLockConfigbyte.Add(lockByte);
           return MeterLockConfigbyte;
       }

       private List<byte> GetManufactureYearBytes(string  refMfyear)
       {
           int mfyear = Convert.ToInt32(refMfyear);
           List<byte> MfgYearbyte = new List<byte>();
           MfgYearbyte.Add(Convert.ToByte((mfyear & 0xFF00) >> 8));
           MfgYearbyte.Add(Convert.ToByte(mfyear & 0x00FF));
           return MfgYearbyte;
       }

       private List<byte> GetManufactureMonthBytes(string refMfgMonth)
       {
           int month = Convert.ToInt32(refMfgMonth);
           List<byte> monthByte = new List<byte>();
           monthByte.Add(Convert.ToByte(month));
           return monthByte;
       }

       private List<byte> GetCurrentRatingByte(string CurrentRatingIB, string CurrentRatingIMax, byte parameterLength)
       {
           string minval = Convert.ToInt32(CurrentRatingIB.Trim()).ToString();//.PadLeft(3, '0');
           string maxval = Convert.ToInt32(CurrentRatingIMax.Trim()).ToString();//.PadLeft(3, '0');
           char[] ratingValueMin = minval.ToCharArray();
           char[] ratingValueMax = maxval.ToCharArray();
           List<byte> IRatingbyte = new List<byte>();
           int datalen = 0;
           while (datalen < ratingValueMin.Length) IRatingbyte.Add(Convert.ToByte(ratingValueMin[datalen++]));
           IRatingbyte.Add(0x2D);
           datalen = 0;
           while (datalen < ratingValueMax.Length) IRatingbyte.Add(Convert.ToByte(ratingValueMax[datalen++]));
           IRatingbyte.Add(0x20);//---Space
           IRatingbyte.Add(0x41);//---Unit A
           while (IRatingbyte.Count < parameterLength) IRatingbyte.Add(0x20);
           return IRatingbyte;
       }

       public List<byte> GetMeterIDByte(string meterID, int charLen)
       {
           List<byte> MeterID = new List<byte>();
           byte len = Convert.ToByte(meterID.Length);
           foreach (char ch in meterID)
           {
               MeterID.Add(Convert.ToByte(ch));
           }
           while (MeterID.Count < charLen) MeterID.Add(Convert.ToByte(' '));
           return MeterID;
       }
       /// <summary>
       /*
        ----- Parameters--------------Default---------------Deviation(%)-------------DataLength----
              Voltage	                45628	                10                  4Byte Data
              Phase Current	            49000	                20                  4Byte Data
              Neutral Current	        69488	                10                  4Byte Data
              Phase Active Power	    4474	                20                  4Byte Data
              Neutral Active Power	    3154	                10                  4Byte Data
              Phase Reactive Power	    4474	                20                  4Byte Data
              Neutral Reactive Power	3154	                10                  4Byte Data
              Phase Delay	            0	    Min Value 1 -- Max Value 417         2Byte Data
              Neutral Delay	            0	    Min Value 1 -- Max Value 417         2Byte Data
         -------------------------------------------------------------------*/
       /// </summary>
       /// <param></param>
       /// <returns></returns>
       private string IsValidCalibrationData_1PSMCostDown()
       {
           
           try
           {
               int startindex = 20;
               int NoOfFourByteParamaters = 6;
               bool isMerterVariationOut = false;
               bool isMeterUnCalibrated = false;
               List<double> parsedMeterData = new List<double>();
               List<double> DefaultConstant = new List<double>() { 45628, 49000, 69488, 4474, 3154, 4474, 3154, 0, 0 };
               List<double> PercentageVariation = new List<double>() { 10, 20, 10, 20, 10, 20, 10, 0, 0 };
               int minCorrectionDelayValue = 1;
               int maxCorrectionDelayValue = 417;
               double MinLimit =0;
               double MaxLimit = 0;
               for (int icount = 0; icount < DefaultConstant.Count; icount++)
               {
                   if (icount <= NoOfFourByteParamaters)
                   {
                       UInt32 uitemp = BitConverter.ToUInt32(new byte[] { GlobalObjects.objSerialComm.ReceiveBuffer[startindex], GlobalObjects.objSerialComm.ReceiveBuffer[startindex + 1], GlobalObjects.objSerialComm.ReceiveBuffer[startindex + 2], GlobalObjects.objSerialComm.ReceiveBuffer[startindex + 3] }, 0); startindex += 4;
                       parsedMeterData.Add(uitemp);
                   }
                   else
                   {
                       UInt16 uitemp = BitConverter.ToUInt16(new byte[] { GlobalObjects.objSerialComm.ReceiveBuffer[startindex], GlobalObjects.objSerialComm.ReceiveBuffer[startindex + 1] }, 0); startindex += 2;
                       parsedMeterData.Add(uitemp);
                   }
                   if (DefaultConstant[icount] == 0) { MinLimit = minCorrectionDelayValue; MaxLimit = maxCorrectionDelayValue; } //--To Handle Corrections cofficient
                   else
                   {
                       MinLimit = (DefaultConstant[icount] - (DefaultConstant[icount] * PercentageVariation[icount] / 100));
                       MaxLimit = (DefaultConstant[icount] + (DefaultConstant[icount] * PercentageVariation[icount] / 100));
                   }
                    //--------------------Check Min/Max Range-----------------------------------------------------------------------
                   if ((parsedMeterData[icount] < MinLimit) || (parsedMeterData[icount] > MaxLimit)) isMerterVariationOut = true;
               }
                //----------------------------------Check Phase Default-------------------------------------------------------
                if (DefaultConstant[(int)SmartMeter1PCalibrationIndex.Voltage] == parsedMeterData[(int)SmartMeter1PCalibrationIndex.Voltage] && 
                    DefaultConstant[(int)SmartMeter1PCalibrationIndex.PhaseCurrent] == parsedMeterData[(int)SmartMeter1PCalibrationIndex.PhaseCurrent] && 
                    DefaultConstant[(int)SmartMeter1PCalibrationIndex.PhaseActivePower] == parsedMeterData[(int)SmartMeter1PCalibrationIndex.PhaseActivePower] && 
                    DefaultConstant[(int)SmartMeter1PCalibrationIndex.PhaseReactivePower] == parsedMeterData[(int)SmartMeter1PCalibrationIndex.PhaseReactivePower]
                    )
                    {
                        isMeterUnCalibrated = true;
                    }
                //----------------------------------Check Neutral Default-------------------------------------------------------
                if (DefaultConstant[(int)SmartMeter1PCalibrationIndex.NeutralCurrent] == parsedMeterData[(int)SmartMeter1PCalibrationIndex.NeutralCurrent] &&
                   DefaultConstant[(int)SmartMeter1PCalibrationIndex.NeutralActivePower] == parsedMeterData[(int)SmartMeter1PCalibrationIndex.NeutralActivePower] &&
                   DefaultConstant[(int)SmartMeter1PCalibrationIndex.NeutralReactivePower] == parsedMeterData[(int)SmartMeter1PCalibrationIndex.NeutralReactivePower]
                   )
                   {
                    isMeterUnCalibrated = true;
                   }
                string combindedString = "";
                foreach (double item in parsedMeterData) { combindedString += "," + item; }
                if (isMeterUnCalibrated ) return StaticVariables.ERRORPreFix + "Uncalibrated Meter >> " + combindedString;
                if (isMerterVariationOut) return StaticVariables.ERRORPreFix + "Cofficient Out Of Range >> " + combindedString;
                return combindedString;
            }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       private string IsValidCalibrationData(byte[] receivedData)
       {
           try
           {
               int startDataindx = 18;

               string strResp = "";
               if (receivedData[startDataindx++] == 0x09) //String
               {
                   int stractcount = 0;
                   int lengthodstruct = receivedData[startDataindx++];//length of stract

                   byte[] buffer = new byte[lengthodstruct];

                   while (stractcount < lengthodstruct)
                   {
                       buffer[stractcount++] = receivedData[startDataindx++];
                   }
                   strResp = DLMSDataStracture.GetHexStringPatternByte(buffer);
               }
               
               string txtPhaseP1WH = ReverseString(strResp.Substring(24, 8), 0);
               string txtNeutralP1WH = ReverseString(strResp.Substring(32, 8), 0);
               
               string txtNeutralPMagSlopeLow = GetData(ReverseString(strResp.Substring(44, 4), 1));
               string txtNeutralPMagSlopeHigh = ReverseString(strResp.Substring(48, 4), 0);
               
               string txtSWTPhaseCompFactor = GetData(ReverseString(strResp.Substring(88, 4), 1));
               string txtSWTNeutralCompFactor = GetData(ReverseString(strResp.Substring(92, 4), 1));

               bool isFail = true;
               string txtCRCbyte = ReverseString(strResp.Substring(96, 2), 0);
               if (txtPhaseP1WH == StaticVariables.PhaseP1WH_SM110 && txtNeutralP1WH == StaticVariables.NeutralP1WH_SM110 && txtNeutralPMagSlopeLow == StaticVariables.NeutralPMagSlopeLow_SM110 && txtSWTPhaseCompFactor == StaticVariables.SWTPhaseCompFactor_SM110 && txtSWTNeutralCompFactor == StaticVariables.SWTNeutralCompFactor_SM110) isFail = true;
               else isFail = false;
               string resultResponse = "PhaseP1WH =" + txtPhaseP1WH + ", NeutralP1WH =" + txtNeutralP1WH + ", NeutralPMagSlopeLow =" + txtNeutralPMagSlopeLow + ", SWTPhaseCompFactor =" + txtSWTPhaseCompFactor + ", SWTNeutralCompFactor =" + txtSWTNeutralCompFactor;
               if (isFail) return StaticVariables.ERRORPreFix + "Uncalibrated Meter >> " + resultResponse;
               else return resultResponse;

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       private string IsValidCalibrationData_3PhaseShappireDLMSCommand(byte[] Blockdata)
       {
           string meterCofficient = "";
           try
           {
               List<string> parsedDataList = new List<string>();
               int startDataindx = 18;
               int paraCode = 0;
               if (Blockdata[startDataindx++] == 0x09) //Oct String
               {
                   int lengthodstruct = Blockdata[startDataindx++];//length of String
                   int objCnt = 0;
                   while (startDataindx < lengthodstruct)
                   {
                       parsedDataList = new List<string>();
                       paraCode = Blockdata[startDataindx++];//------Para Code
                       startDataindx++; //------Coff Structure
                       int coffStructLen = Blockdata[startDataindx++]; //-----Coff Len
                       //------------------------R Phase Value--------------------
                       startDataindx++; //------data Type
                       string data = Blockdata[startDataindx + 1].ToString("X2") + Blockdata[startDataindx].ToString("X2");
                       parsedDataList.Add(data);
                       startDataindx += 2;
                       //------------------------Y Phase Value--------------------
                       if (coffStructLen > 1)
                       {
                           startDataindx++; //------data Type
                           data = Blockdata[startDataindx + 1].ToString("X2") + Blockdata[startDataindx].ToString("X2");
                           parsedDataList.Add(data);
                           startDataindx += 2;
                           //------------------------B Phase Value--------------------
                           startDataindx++; //------data Type
                           data = Blockdata[startDataindx + 1].ToString("X2") + Blockdata[startDataindx].ToString("X2");
                           parsedDataList.Add(data);
                           startDataindx += 2;
                       }
                       objCnt++;
                       if (paraCode == 6) meterCofficient +=  parsedDataList[0];  //Voltage
                       if (paraCode == 7) meterCofficient += parsedDataList[0]; ; //Current
                       if (paraCode == 8) meterCofficient += parsedDataList[0];   //Active Power
                       if (paraCode == 9) meterCofficient += parsedDataList[0];   //ReActive Power
                   }
               }
               foreach (var item in StaticVariables.SapphireCalibrationCofficient)
               {
                   if (meterCofficient == item) { return StaticVariables.ERRORPreFix + "Uncalibrated Meter >> " + meterCofficient; }
               }
               return "Calibration Response >> " + meterCofficient; //---If default Cofficient Not Found then only PASS
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message + meterCofficient;
           }
       }
       private string IsValidCalibrationData_3Phase(byte[] Blockdata)
       {
           string resultResponse = "";
            try
            {
                
                List<string> parsedDataList = new List<string>();
                int startDataindx = 18;             
                string[] datavalue = new string[2];
                int textValIndex = 0;
                 int paraCode =0;
                 byte Resultflag = 0;
                if (Blockdata[startDataindx++] == 0x02) //srtact
                {
                    int lengthodstruct = Blockdata[startDataindx++];//length of stract
                    int objCnt = 0;
                    while (objCnt < lengthodstruct)
                    {
                        parsedDataList = new List<string>();
                        startDataindx++; //------Coff Structure
                        int coffStructLen = Blockdata[startDataindx++]; //-----Coff Len
                        startDataindx++; //-----Para data type
                        paraCode = Blockdata[startDataindx++];//------Para Code
                        textValIndex = 0;

                        int byteIndex = startDataindx;
                        //------------------------R Phase Value--------------------
                        datavalue = DLMSDataStracture.DLMSDataFormator(Blockdata, byteIndex, false);
                        if (datavalue == null) { parsedDataList.Add(""); objCnt++; continue; }
                        string data = datavalue[0];
                        parsedDataList.Add(data);
                        textValIndex++;
                        byteIndex = Convert.ToInt32(datavalue[1]);
                        //------------------------Y Phase Value--------------------
                        if (coffStructLen == 2) break;
                        datavalue = DLMSDataStracture.DLMSDataFormator(Blockdata, byteIndex, false);
                        if (datavalue == null) { parsedDataList.Add(""); objCnt++; continue; }
                        data = datavalue[0];
                        parsedDataList.Add(data);
                        textValIndex++;
                        byteIndex = Convert.ToInt32(datavalue[1]);
                        //------------------------B Phase Value--------------------
                        datavalue = DLMSDataStracture.DLMSDataFormator(Blockdata, byteIndex, false);
                        if (datavalue == null) { parsedDataList.Add(""); objCnt++; continue; }
                        data = datavalue[0];
                        parsedDataList.Add(data);
                        textValIndex++;
                        startDataindx = Convert.ToInt32(datavalue[1]);
                        objCnt++;



                        if (paraCode == 1)
                        {
                            string tempRes = "Voltage R =" + parsedDataList[0] + ",Y=" + parsedDataList[1] + ",B=" + parsedDataList[2];
                            string[] Distvalue = parsedDataList.Distinct().ToArray();
                            //if (Distvalue.Length <= 1 && (parsedDataList[0] == "26900" || parsedDataList[0] == "27006")) resultResponse = StaticVariables.ERRORPreFix + tempRes;
                            if (Distvalue.Length <= 1 && (StaticVariables.SmartMeter3PSMCalibrationCofficient[0].Contains(parsedDataList[0]))) resultResponse = StaticVariables.ERRORPreFix + tempRes;
                            else { resultResponse = tempRes; Resultflag++;}
                        }
                        if (paraCode == 2)
                        {
                            string tempRes = ", Current R=" + parsedDataList[0] + ",Y=" + parsedDataList[1] + ",B=" + parsedDataList[2];
                            string[] Distvalue = parsedDataList.Distinct().ToArray();
                           // if (Distvalue.Length <= 1 && (parsedDataList[0] == "27100" || parsedDataList[0] == "4434")) resultResponse += StaticVariables.ERRORPreFix + tempRes;
                            if (Distvalue.Length <= 1 && (StaticVariables.SmartMeter3PSMCalibrationCofficient[1].Contains(parsedDataList[0]))) resultResponse += StaticVariables.ERRORPreFix + tempRes;
                            else{ resultResponse += tempRes; Resultflag++;}
                        }
                        if (paraCode == 3)
                        {
                            string tempRes = ", Active Power R=" + parsedDataList[0] + ",Y=" + parsedDataList[1] + ",B=" + parsedDataList[2];
                            string[] Distvalue = parsedDataList.Distinct().ToArray();
                           // if (Distvalue.Length <= 1 && (parsedDataList[0] == "45500" || parsedDataList[0] == "7481")) resultResponse += StaticVariables.ERRORPreFix + tempRes;
                            if (Distvalue.Length <= 1 && (StaticVariables.SmartMeter3PSMCalibrationCofficient[2].Contains(parsedDataList[0]))) resultResponse += StaticVariables.ERRORPreFix + tempRes;
                            else {resultResponse += tempRes; Resultflag++;}
                        }
                        if (paraCode == 4)
                        {
                            string tempRes = ", ReActive Power R=" + parsedDataList[0] + ",Y=" + parsedDataList[1] + ",B=" + parsedDataList[2];
                            string[] Distvalue = parsedDataList.Distinct().ToArray();
                            //if (Distvalue.Length <= 1 && (parsedDataList[0] == "6900" || parsedDataList[0] == "1135")) resultResponse += StaticVariables.ERRORPreFix + tempRes;
                            if (Distvalue.Length <= 1 && (StaticVariables.SmartMeter3PSMCalibrationCofficient[3].Contains(parsedDataList[0]))) resultResponse += StaticVariables.ERRORPreFix + tempRes;
                            else { resultResponse += tempRes; Resultflag++;}
                        }
                    }
                    
                }
               
                if (Resultflag <= 0) return "Uncalibrated Meter >> " + resultResponse;
                else
                {
                    resultResponse = resultResponse.Replace(StaticVariables.ERRORPreFix, "Default:");
                    return resultResponse;
                }
            }
            catch (Exception ex)
            {
                return StaticVariables.ERRORPreFix + ex.Message + resultResponse;

            }
       }
       /// <Calibration Read Structure>
       /// Calibration Parameters	     Tag	Tag	Parameter Code	Tag	R-Phase	    Tag	Y-Phase	    Tag	B-Phase
        ///Voltage	                    02 04	0x11	Code - 1	0x12	Value	0x12	Value	0x12	Value
        ///Current	                    02 04	0x11	Code - 2	0x12	Value	0x12	Value	0x12	Value
        ///P-Active	                    02 04	0x11	Code - 3	0x12	Value	0x12	Value	0x12	Value
        ///P-Reactive	                02 04	0x11	Code - 4	0x12	Value	0x12	Value	0x12	Value
        ///Fundamental Voltage	        02 04	0x11	Code - 5	0x12	Value	0x12	Value	0x12	Value
        ///Fundamental P-Active	        02 04	0x11	Code - 6	0x12	Value	0x12	Value	0x12	Value
        ///Fundamental P-Reactive	    02 04	0x11	Code - 7	0x12	Value	0x12	Value	0x12	Value
        ///Phase Compensation	        02 02	0x11	Code - 8	0x12	Value				
        ///I-Offset	                    02 02	0x11	Code -9	    0x12	Value	 	 	 	 
        ///I-Neutral	                02 02	0x11	Code -10	0x12	Value	 	 	 	 
        ///I-Neutral Offset	            02 02	0x11	Code -11	0x12	Value	
       /// </summary>
       /// <param name="receivedData"></param>
       /// <returns></returns>
       private string IsValidCalibrationData_SapphireS2(byte[] receivedData)
       {
           string MeterCalibrationCofficient="";
           string DefaultCalibrationCofficient = "020a020411011234411234411234410204110212253a12253a12253a02041103121f13121f13121f13020411041278b41278b41278b402041108120161120161120161020411091200001200001200000202110a12be780202110b1200000202110c120000".ToUpperInvariant();
           List<string> listDefaultCalibrationCofficient = new List<string>();
           listDefaultCalibrationCofficient.Add("02041101123441123441123441");//Voltage default cofficient
           listDefaultCalibrationCofficient.Add("0204110212253a12253a12253a");//Current default cofficient
           listDefaultCalibrationCofficient.Add("02041103121f13121f13121f13");//Active Power default cofficient
           listDefaultCalibrationCofficient.Add("020411041278b41278b41278b4");//Reactive Power default cofficient
           //listDefaultCalibrationCofficient.Add("02041108120161120161120161");//Phase Compansetion default cofficient
           //listDefaultCalibrationCofficient.Add("02041109120000120000120000");//I-offset default cofficient
           listDefaultCalibrationCofficient.Add("0202110a12be78");//Neu. Current default cofficient
           //listDefaultCalibrationCofficient.Add("0202110b120000");//I-Neu. offset default cofficient
           //listDefaultCalibrationCofficient.Add("0202110c120000");//Temprature default
           try
           {
               Application.DoEvents();
               Application.DoEvents();
               int startDataindx = 18;
               string[] datavalue = new string[2];
               int strctureStartIndex = startDataindx;
               if (receivedData[startDataindx] == 0x09) //srtact
               {
                   int lengthodstruct = receivedData[startDataindx + 1];//length of stract
                   byte[] parameterlist = new byte[lengthodstruct + 2];
                   Array.Copy(receivedData, startDataindx, parameterlist, 0, parameterlist.Length);
                   MeterCalibrationCofficient = DLMSDataStracture.GetByteToHexString(parameterlist);
               }
               //---Verify the complete string received from meter with default cofficient--------------
               if (MeterCalibrationCofficient.Contains(DefaultCalibrationCofficient)) return StaticVariables.ERRORPreFix + "Uncalibrated >>" + MeterCalibrationCofficient;
               //---Re-Verify Parameters Wise Default Value with default cofficient list---------------------    
               foreach (var item in listDefaultCalibrationCofficient)
               {
                   if (MeterCalibrationCofficient.Contains(item.ToUpperInvariant())) return StaticVariables.ERRORPreFix + "Uncalibrated >>" + MeterCalibrationCofficient;
               }
               return MeterCalibrationCofficient;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message + MeterCalibrationCofficient;

           }
       }

       private string IsValidCalibrationData_MicroStarDLMS(byte[] receivedData)
       {
           try
           {
               int startDataindx = 18;

               string strResp = "";
               if (receivedData[startDataindx++] == 0x09) //String
               {
                   int stractcount = 0;
                   int lengthodstruct = receivedData[startDataindx++];//length of stract

                   byte[] buffer = new byte[lengthodstruct];

                   while (stractcount < lengthodstruct)
                   {
                       buffer[stractcount++] = receivedData[startDataindx++];
                   }
                   strResp = DLMSDataStracture.GetHexStringPatternByte(buffer);
               }

               string txtPhaseP1WH = ReverseString(strResp.Substring(24, 8), 0);
               string txtNeutralP1WH = ReverseString(strResp.Substring(32, 8), 0);

               string txtPhase_P_1VARrH = Convert.ToInt32(ReverseString(strResp.Substring(44, 8), 1), 16).ToString();//GetData(ReverseString(strResp.Substring(44, 8), 1));
               string txtNeutral_P_1VArH = ReverseString(strResp.Substring(52, 8), 0);

               bool isFail = true;
               //-------------- 05-30 Microstar ----------------------------------------------------------------------------------------------------------------------------------------
               if ((txtPhaseP1WH == StaticVariables.PhaseP1WH_Micro1 && txtPhase_P_1VARrH == StaticVariables.Phase_P_1VARrH_Micro1) || (txtNeutralP1WH == StaticVariables.NeutralP1WH_Micro1 && txtNeutral_P_1VArH == StaticVariables.Neutral_P_1VArH_Micro1)) isFail = true;
               //-------------- 10-60 Microstar ----------------------------------------------------------------------------------------------------------------------------------------
               else if ((txtPhaseP1WH == StaticVariables.PhaseP1WH_Micro2 && txtPhase_P_1VARrH == StaticVariables.Phase_P_1VARrH_Micro2) || (txtNeutralP1WH == StaticVariables.NeutralP1WH_Micro2 && txtNeutral_P_1VArH == StaticVariables.Neutral_P_1VArH_Micro2)) isFail = true;
               //-------------- 05-30 VIM ----------------------------------------------------------------------------------------------------------------------------------------
               else if ((txtPhaseP1WH == StaticVariables.PhaseP1WH_VIM1 && txtPhase_P_1VARrH == StaticVariables.Phase_P_1VARrH_VIM1) || ((txtNeutralP1WH == StaticVariables.NeutralP1WH_VIM1_DC_IMMUNE_CT || txtNeutralP1WH == StaticVariables.NeutralP1WH_VIM1_DC_NONIMMUNE_CT) && txtNeutral_P_1VArH == StaticVariables.Neutral_P_1VArH_VIM1)) isFail = true;
               //-------------- 10-60 VIM ----------------------------------------------------------------------------------------------------------------------------------------
               else if ((txtPhaseP1WH == StaticVariables.PhaseP1WH_VIM2 && txtPhase_P_1VARrH == StaticVariables.Phase_P_1VARrH_VIM2) || (txtNeutralP1WH == StaticVariables.NeutralP1WH_VIM2 && txtNeutral_P_1VArH == StaticVariables.Neutral_P_1VArH_VIM2)) isFail = true;
               //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
               else isFail = false;
               string resultResponse = "PhaseP1WH =" + txtPhaseP1WH + ", NeutralP1WH =" + txtNeutralP1WH + ", Phase_P_1VARrH =" + txtPhase_P_1VARrH + ", Neutral_P_1VArH =" + txtNeutral_P_1VArH;
               if (isFail) return StaticVariables.ERRORPreFix + "Uncalibrated Meter >> " + resultResponse;
               else return resultResponse;


           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private string GetData(string data)
       {
           string result = "";
           string binDataValue = Convert.ToString(Convert.ToInt32(data, 16), 2);
           while (binDataValue.Length < 16) { binDataValue = "0" + binDataValue; }
           if (binDataValue.Substring(0, 1) == "1")
           {
               int decData = Convert.ToInt32(data, 16);
               decData = 0 - decData;
               result = decData.ToString("X").Substring(decData.ToString("X").Length - 4, 4);
               result = "-" + Convert.ToInt32(result, 16).ToString();
           }
           else
           {
               result = Convert.ToInt32(data, 16).ToString(); ;
           }
           return result;
       }

       private string ReverseString(string response, int format)
       {
           int count = 0;
           string revResponse = "";

           for (count = response.Length - 2; count >= 0; count -= 2)
           {
               revResponse += response.Substring(count, 2);
           }
           if (format == 0)
           {
               revResponse = Convert.ToInt32(revResponse, 16).ToString();
           }
           return revResponse;
       }

       public string GetMeterResponseValuePart(string CmdResponse)
       {
           try
           {
               const string regexReadbuffer = @"(\(([\w\W]*?)\))";
               MatchCollection matches = Regex.Matches(CmdResponse, regexReadbuffer, RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
               string[] Bufferdata = new string[matches.Count];
               int rcnt = 0;
               foreach (Match match in matches)
               {
                   GroupCollection groups = match.Groups;
                   Bufferdata[rcnt] = groups["0"].Value;
                   rcnt++;
               }

               CmdResponse = Bufferdata[0];
               CmdResponse = CmdResponse.Substring(1, CmdResponse.Length - 2);
               return CmdResponse;
           }
           catch (Exception)
           {
               return "";
           }
       }
       
               
       public string VerifyEnergy(string defaultVal,string refminVal,string refmaxVal)
       {
           try
           {               
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadInstantkWhDataStracture.ReadInstantkWhOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadInstantkWhDataStracture.ReadInstantkWhClassID, DLMSDataStracture.ReadInstantkWhDataStracture.ReadInstantkWhValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               List<string> formattedEnergy = FormatIndivisualData(GlobalObjects.objSerialComm.ReceiveBuffer);
               if (formattedEnergy.Count <= 0) { return StaticVariables.ERRORPreFix + "Invalid Data Read."; }
               string formattedValue = formattedEnergy[0];
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadInstantkWhDataStracture.ReadInstantkWhOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadInstantkWhDataStracture.ReadInstantkWhClassID, DLMSDataStracture.ReadInstantkWhDataStracture.ReadInstantkWhValueAttributeScalar)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               formattedValue = ApplyScalarUnits(GlobalObjects.objSerialComm.ReceiveBuffer, formattedValue);
               return objcomnMethod.CheckingRangeValueForDecimal("Meter Value =", defaultVal, refminVal, refmaxVal, formattedValue);
               
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
        
       public string VerifyTemprature(string defaultVal, string minVal, string maxVal)
       {
           try
           {
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.CalibrationDataStracture.CalibrationTemperatureOBIS, txtboxobject, "0", 100M, DLMSDataStracture.CalibrationDataStracture.CalibrationNonPowerReadClassID, DLMSDataStracture.CalibrationDataStracture.CalibrationValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               List<string> formattedEnergy = FormatIndivisualData(GlobalObjects.objSerialComm.ReceiveBuffer);
               if (formattedEnergy.Count <= 0) { return StaticVariables.ERRORPreFix + "Invalid Data Read."; }
               string formattedValue = formattedEnergy[0];
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.CalibrationDataStracture.CalibrationTemperatureOBIS, txtboxobject, "0", 100M, DLMSDataStracture.CalibrationDataStracture.CalibrationNonPowerReadClassID, DLMSDataStracture.CalibrationDataStracture.CalibrationValueAttribute_Scalar)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               formattedValue = ApplyScalarUnits(GlobalObjects.objSerialComm.ReceiveBuffer, formattedValue);
               return objcomnMethod.CheckingRangeValueForDecimal("Meter Value =", defaultVal, minVal, maxVal, formattedValue);
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       //------------------------------Read & Verify  Meter Configuration File -----------------------------------------------------
       public string VerifyDisplayConfigData_3PhaseDLMS(string srcFileName)
       {
           try
           {
               if (!File.Exists(srcFileName)) { return StaticVariables.ERRORPreFix + "Invalid File Path/File Not Exist"; }
               Dictionary<byte, string> configPara = StaticVariables.Get3PHDLMSConfigParaName();               
               string strFileData = DLMSDataStracture.ReadUserFileData(srcFileName);
               string[] dataPacket = strFileData.Split('\n');
               if (dataPacket.Length < 1)
               {
                   return StaticVariables.ERRORPreFix + "Invalid/ Blank File";
               }
               if (dataPacket.Length >= 1 && dataPacket[0].Length <=0 )
               {
                   return StaticVariables.ERRORPreFix + "Invalid/ Blank File";
               }
               int paraCount = 0;

               while (paraCount < dataPacket.Length - 1)
               {
                   string paraLabel = "";
                   byte keyCode = Convert.ToByte(Convert.ToInt32(dataPacket[paraCount].ToString().Substring(0, 2),16));
                   string[] keysByValue = configPara.Where(x => x.Key == keyCode).Select(pair => pair.Value).ToArray();
                   if (keysByValue.Length > 0) paraLabel = keysByValue[0].ToString();
                   else return StaticVariables.ERRORPreFix + "invalid Command in file =" + dataPacket[paraCount].ToString();
                   string commandParaValue = dataPacket[paraCount].ToString().Substring(0, dataPacket[paraCount].ToString().Length - 1);
                   if (commandParaValue.Length <= 18) return StaticVariables.ERRORPreFix + " Command Not Executed =" + paraLabel;
                   string getParafromMeter = ReadConfiguration_3PHDLMS(commandParaValue);
                   if (getParafromMeter.Length == StaticVariables.ERRORPreFix.Length) return StaticVariables.ERRORPreFix + paraLabel;
                   else if (getParafromMeter.IndexOf(StaticVariables.ERRORPreFix) >= 0) return getParafromMeter + " : "+ paraLabel;  
                   paraCount++;
               }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string VerifyDisplayConfigData_3Phase(string srcFileName)
       {
           try
           {
               if (!File.Exists(srcFileName)) { return StaticVariables.ERRORPreFix + "Invalid File Path/File Not Exist"; }
               string strFileData = DLMSDataStracture.ReadUserFileData(srcFileName);
               string[] dataPacket = strFileData.Split(',');
               if (dataPacket.Length <= 1)
               {
                   return StaticVariables.ERRORPreFix + "Invalid/ Blank File";
               }
               if (dataPacket[1].IndexOf("E450") < 0 && dataPacket[1].IndexOf("E250") < 0)
               {
                   return StaticVariables.ERRORPreFix + "Invalid/ Blank File";
               }
               if (dataPacket.Length >= 1 && dataPacket[0].Length <= 0)
               {
                   return StaticVariables.ERRORPreFix + "Invalid/ Blank File";
               }
               int paraCount = 2;
              
               while (paraCount < dataPacket.Length - 1)
               {                  

                   bool IsValidLabel = false;
                   string paraLabel = dataPacket[paraCount].ToString();
                   if (paraLabel.LastIndexOf("<") >= 0) { IsValidLabel = true; paraLabel = paraLabel.Substring(paraLabel.LastIndexOf("<")); }
                   int paraIDX = GetParaIndex_3Phase(dataPacket[paraCount++].ToString());
                   string filedata = dataPacket[paraCount].ToString();
                   if (paraLabel.IndexOf("<Info>") >= 0) continue; //-----------Skip if tag is Info or Active Day Profile for TOU otherwise return command not found 
                   if (paraLabel.IndexOf("<ActiveDayProfile>") >= 0) continue;
                   if (paraIDX < 0 && !IsValidLabel) continue;
                   else if (paraIDX < 0 && IsValidLabel) return StaticVariables.ERRORPreFix + " Command Not Executed =" + paraLabel;

                   string getParafromMeter = ReadConfiguration(paraIDX, filedata);
                   if (getParafromMeter.Length == StaticVariables.ERRORPreFix.Length) return StaticVariables.ERRORPreFix + paraLabel;
                   else if (getParafromMeter.IndexOf(StaticVariables.ERRORPreFix) >= 0) return getParafromMeter + " : " + paraLabel; 
               }             
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
      

       private string ReadConfiguration(int ReadoutParaIndex,string FileData)
       {
           try
           {
               byte[] receviedData=null;
               int rs485addlen = 0;
               List<byte> paradata = DLMSDataStracture.GetByteFromHexStringPattern(FileData).ToList();               
               byte objectClassID = paradata[0];
               byte[] objectOBIS = new byte[6];
               Array.Copy(paradata.ToArray(), 1, objectOBIS, 0, 6);
               byte objectattribute = paradata[7];
               List<string> CongigReadoutList = new List<string>();
               List<byte> selectedAccessByteList = new List<byte>();
               bool isBlock = false;
               if ((int)StaticVariables.ENMDisplayConfigStruct_1Phase.RS485DeviceStatus == ReadoutParaIndex) { rs485addlen = 6; selectedAccessByteList = RS485ByteAddress_SM110(paradata); }
               byte accessSelector = DLMSDataStracture.DataStractureAccessSelector.Null_descriptor;
               if(selectedAccessByteList.Count > 0)accessSelector = DLMSDataStracture.DataStractureAccessSelector.Range_descriptor;
               if (   (int)StaticVariables.ENMDisplayConfigStruct_3Ph.DISPPUSH == ReadoutParaIndex
                   || (int)StaticVariables.ENMDisplayConfigStruct_3Ph.DISPSCROLL == ReadoutParaIndex
                   || (int)StaticVariables.ENMDisplayConfigStruct_3Ph.DISPHR == ReadoutParaIndex
                   || (int)StaticVariables.ENMDisplayConfigStruct_3Ph.TAMPTHRESHOULD == ReadoutParaIndex                   
                   || (int)StaticVariables.ENMDisplayConfigStruct_3Ph.FutureDayProfile == ReadoutParaIndex 
                   || (int)StaticVariables.ENMDisplayConfigStruct_3Ph.WeekProfile == ReadoutParaIndex 
                   || (int)StaticVariables.ENMDisplayConfigStruct_3Ph.SeasonProfile == ReadoutParaIndex 
                   || (int)StaticVariables.ENMDisplayConfigStruct_3Ph.SpecialDaysProfile == ReadoutParaIndex
                   || (int)StaticVariables.ENMDisplayConfigStruct_3Ph.FutureActivationDate == ReadoutParaIndex
                   || (int)StaticVariables.ENMDisplayConfigStruct_1Phase.FutureDayProfile == ReadoutParaIndex
                   || (int)StaticVariables.ENMDisplayConfigStruct_1Phase.WeekProfile == ReadoutParaIndex
                   || (int)StaticVariables.ENMDisplayConfigStruct_1Phase.SeasonProfile == ReadoutParaIndex                   
                   || (int)StaticVariables.ENMDisplayConfigStruct_1Phase.FutureActivationDate == ReadoutParaIndex
                   || (int)StaticVariables.ENMDisplayConfigStruct_1Phase.RS485DeviceStatus == ReadoutParaIndex 
                  )
                   {
                      // if (!objLI.ReadBlockFromMeter(objectOBIS, txtboxobject, "0", 1M, objectClassID, GetReadoutAttribute(ReadoutParaIndex, objectattribute), accessSelector, selectedAccessByteList)) { { return StaticVariables.ERRORPreFix + "COMM Failed."; } }
                    if (!objLI.ReadBlockFromMeter(objectOBIS, txtboxobject, "0", 1M, objectClassID, objectattribute, accessSelector, selectedAccessByteList)) { { return StaticVariables.ERRORPreFix + "COMM Failed."; } }
                    receviedData =   GlobalObjects.objCOSEMLIB.BlockBuffer;
                        isBlock = true;
                  }
                else
                   {
                        if (!objLI.ReadByteFromMeter(objectOBIS, txtboxobject, "0", 1M, objectClassID, objectattribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
                        receviedData = GlobalObjects.objSerialComm.ReceiveBuffer;    
               }

                int statIDX = 18 + rs485addlen;
              
                int endIDX = statIDX + ((FileData.Length - statIDX) / 2);
                if (isBlock) 
                {
                    endIDX = ((FileData.Length - statIDX) / 2); statIDX = 0;
                    if (receviedData[0] == 0x09) endIDX -= 2;
                }
                if ((int)StaticVariables.ENMDisplayConfigStruct_3Ph.FutureDayProfile == ReadoutParaIndex
                        || (int)StaticVariables.ENMDisplayConfigStruct_3Ph.WeekProfile == ReadoutParaIndex
                        || (int)StaticVariables.ENMDisplayConfigStruct_3Ph.SeasonProfile == ReadoutParaIndex                 
                        || (int)StaticVariables.ENMDisplayConfigStruct_3Ph.FutureActivationDate == ReadoutParaIndex
                        || (int)StaticVariables.ENMDisplayConfigStruct_1Phase.FutureDayProfile == ReadoutParaIndex
                        || (int)StaticVariables.ENMDisplayConfigStruct_1Phase.WeekProfile == ReadoutParaIndex
                        || (int)StaticVariables.ENMDisplayConfigStruct_1Phase.SeasonProfile == ReadoutParaIndex
                        || (int)StaticVariables.ENMDisplayConfigStruct_1Phase.FutureActivationDate == ReadoutParaIndex 
                    ) endIDX -= 4;
                else if ((int)StaticVariables.ENMDisplayConfigStruct_3Ph.SpecialDaysProfile == ReadoutParaIndex) endIDX -= 2;
                string readPara = string.Empty;
                List<byte> meterReadouts = new List<byte>();

                string resValue = BitConverter.ToString(receviedData);
                resValue = resValue.Replace("-", "");
                readPara = resValue.Substring(statIDX * 2, endIDX * 2 - statIDX * 2);

               //while (statIDX < endIDX)
               //{
               //    readPara += receviedData[statIDX].ToString("X").PadLeft(2, '0');
               //    statIDX++;                
               //}
               if ((int)StaticVariables.ENMDisplayConfigStruct_3Ph.ALARMLOG == ReadoutParaIndex)
               {
                   //----------------No Need To Check Last Byte as some bits are used by firmware and updated at run time
                   readPara = readPara.Substring(0, readPara.Length - 2);
               }
               if ((int)StaticVariables.ENMDisplayConfigStruct_3Ph.DISPTIMEOUT == ReadoutParaIndex)
               {
                   //----------------No Need To Check 2nd last structure Bytes as it is part of FCS command which is used by firmware and updated during FCS write.
                   if (objappSettings.GetMeterMode() == (int)StaticVariables.ExecutedMeterType.Smart_Meter_3PH)
                   { readPara = readPara.Substring(0, readPara.Length - 10) + FileData.Substring(46, 4) + readPara.Substring(readPara.Length - 6,6); }
                   
               }
               if (FileData.Contains(readPara)) return "";
               else if (objectOBIS.SequenceEqual(DLMSDataStracture.KVAHSelectionDataStracture.KVAHSelectionOBIS) &&
                   (CompareKVahFailPacket(FileData, readPara))) return "";//-----For KVah Selection Packet as meter accept data type "09" in write and send "0x11" in read response from old MMP
               else if (objectOBIS.SequenceEqual(DLMSDataStracture.ManufactureYearDataStracture.ManufactureYearOBIS) &&
                   (CompareMfgYearFailPacket(FileData, readPara))) return "";
               else return StaticVariables.ERRORPreFix + "Packet Mismatch >>";            
               
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
            
       }
       /// <summary>
       /// To read Active TOU profile and ensure that it must be activated in factory so that read active tou profile and compare file future TOU profile
       /// </summary>
       /// <param name="ProfileNameIndex"></param>
       /// <param name="refAtt"></param>
       /// <returns></returns>
       private byte GetReadoutAttribute(int ProfileNameIndex, byte refAtt)
       {
           byte tempatt = refAtt;
           switch (ProfileNameIndex)
           {
               case (int)StaticVariables.ENMDisplayConfigStruct_3Ph.FutureDayProfile:
               case (int)StaticVariables.ENMDisplayConfigStruct_1Phase.FutureDayProfile:
                   tempatt = 0x05;
                   break;
               case (int)StaticVariables.ENMDisplayConfigStruct_3Ph.WeekProfile:
               case (int)StaticVariables.ENMDisplayConfigStruct_1Phase.WeekProfile:
                   tempatt = 0x04;
                   break;
               case (int)StaticVariables.ENMDisplayConfigStruct_3Ph.SeasonProfile:
               case (int)StaticVariables.ENMDisplayConfigStruct_1Phase.SeasonProfile:
                   tempatt = 0x03;
                   break;
           }
           return tempatt;
       }
       private List<byte> RS485ByteAddress_SM110(List<byte> paradata)
       {
           List<byte> rs485AddresByte = new List<byte>();
           byte[] objectdatalist = new byte[9];
           objectdatalist[0] = 0x01;// Access Selector
           Array.Copy(paradata.ToArray(), 9, objectdatalist, 1, 5);
           objectdatalist[6] = 0x12;
           objectdatalist[7] = 0x00;
           objectdatalist[8] = 0x01;
           rs485AddresByte =  objectdatalist.ToList();
           return rs485AddresByte;
       }

       private string ReadConfiguration_3PHDLMS(string FileData)
       {
           try
           {
               byte[] receviedData = null;
               List<byte> paradata = DLMSDataStracture.GetByteFromHexStringPattern(FileData).ToList();
               byte objectClassID = paradata[1];
               byte[] objectOBIS = new byte[6];
               Array.Copy(paradata.ToArray(), 2, objectOBIS, 0, 6);
               byte objectattribute = paradata[8];
               List<string> CongigReadoutList = new List<string>();
               bool isBlock = false;
               if (paradata.Count > 100 || objectClassID == 0x14)
               {
                   if (!objLI.ReadBlockFromMeter(objectOBIS, txtboxobject, "0", 1M, objectClassID, objectattribute, DLMSDataStracture.DataStractureAccessSelector.Null_descriptor, null)) { { return StaticVariables.ERRORPreFix + "COMM Failed."; } }
                   receviedData = GlobalObjects.objCOSEMLIB.BlockBuffer;
                   isBlock = true;
               }
               else
               {
                   if (!objLI.ReadByteFromMeter(objectOBIS, txtboxobject, "0", 1M, objectClassID, objectattribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
                   receviedData = GlobalObjects.objSerialComm.ReceiveBuffer;
               }

               int statIDX = 18;
               int endIDX = statIDX + ((FileData.Length - statIDX) / 2);
               if (isBlock) { endIDX = ((FileData.Length - statIDX) / 2); statIDX = 4; }               
               endIDX -= 1;
               string readPara = string.Empty;
               List<byte> meterReadouts = new List<byte>();

               string resValue = BitConverter.ToString(receviedData);
               resValue = resValue.Replace("-","");
               readPara = resValue.Substring(statIDX * 2, endIDX * 2 - statIDX * 2);
                
               if (FileData.Contains(readPara)) return "";
               else if (objectOBIS.SequenceEqual(DLMSDataStracture.TamperThresholdDataStracture.TamperThresholdOBIS) &&
                   (CompareFailPacket(statIDX, endIDX, receviedData, FileData))) return "";//-----For Thamper threshold Stack issue at fixed location
               else if (objectOBIS.SequenceEqual(DLMSDataStracture.KVAHSelectionDataStracture.KVAHSelectionOBIS) &&
                   (CompareKVahFailPacket(FileData, readPara))) return "";//-----For KVah Selection Packet as meter accept data type "09" in write and send "0x11" in read response from old MMP
               else if (objectOBIS.SequenceEqual(DLMSDataStracture.ManufactureYearDataStracture.ManufactureYearOBIS) &&
                   (CompareMfgYearFailPacket(FileData, readPara))) return "";
               else return StaticVariables.ERRORPreFix + "Packet Mismatch >>";

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }

       private bool CompareFailPacket(int statIDX, int endIDX, byte[] receviedData, string FileData)
       {
            try
           {
           string readPara = "";
           int replaceCount = 0;
           while (statIDX < endIDX)
           {
                 readPara += receviedData[statIDX].ToString("X").PadLeft(2, '0');
                 if (!FileData.Contains(readPara)) { readPara = ""; replaceCount++; }
                 statIDX++;
                 if (replaceCount >= 2) return false;
           }
           return true;
           }
            catch (Exception)
            {
                return false;
            }
       }
       private bool CompareKVahFailPacket( string FileData, string recData)
       {
           try
           {
               if (FileData.Contains("0901") && recData.Contains("11")) { recData = recData.Replace("11", "0901"); recData=recData.Substring(0, 6); }
               if (FileData.Contains(recData)) return true;
               return false;
           }
           catch (Exception )
           {
               return false;
           }
       }
       private bool CompareMfgYearFailPacket(string FileData, string recData)
       {
           try
           {
               string systemYear = DateTime.Now.Year.ToString("X4");
               if (recData.Contains(systemYear)) return true; //---Pass the test case if Meter Manufacturing Year match with System year (current Date)
               return false;
           }
           catch (Exception)
           {
               return false;
           }
       }

       public string VerifyDisplayConfigData_1Phase(string srcFileName)
       {
           try
           {
               if (!File.Exists(srcFileName)) { return StaticVariables.ERRORPreFix + "Invalid File Path/File Not Exist"; }
               string strFileData = DLMSDataStracture.ReadUserFileData(srcFileName);
               string[] dataPacket = strFileData.Split(',');
               if (dataPacket.Length <= 1)
               {
                   return StaticVariables.ERRORPreFix + "Invalid/ Blank File";
               }
               if (dataPacket.Length >= 1 && dataPacket[0].Length <= 0)
               {
                   return StaticVariables.ERRORPreFix + "Invalid/ Blank File";
               }
               if (dataPacket[1].IndexOf("E350") < 0 && dataPacket[1].IndexOf("E150DLMS") < 0)
               {
                  return StaticVariables.ERRORPreFix + "Invalid File Selected";
               }
               int paraCount = 2;

               while (paraCount < dataPacket.Length - 1)
               { 
                   bool IsValidLabel = false;
                   string paraLabel = dataPacket[paraCount].ToString();
                   if (paraLabel.LastIndexOf("<") >= 0) { IsValidLabel = true; paraLabel = paraLabel.Substring(paraLabel.LastIndexOf("<")); }
                   int paraIDX = GetParaIndex_1Phase(dataPacket[paraCount++].ToString());
                   string filedata = dataPacket[paraCount].ToString();
                   if (paraLabel.IndexOf("<Info>") >= 0) continue; //-----------Skip if tag is Info or Active Day Profile for TOU otherwise return command not found 
                   if (paraLabel.IndexOf("<ActiveDayProfile>") >= 0) continue;
                   if (paraIDX < 0 && !IsValidLabel) continue;
                   else if (paraIDX < 0 && IsValidLabel) return StaticVariables.ERRORPreFix + " Command Not Executed =" + paraLabel;

                   string getParafromMeter = ReadConfiguration(paraIDX, filedata);
                   if (getParafromMeter.Length == StaticVariables.ERRORPreFix.Length) return StaticVariables.ERRORPreFix + paraLabel;
                   else if (getParafromMeter.IndexOf(StaticVariables.ERRORPreFix) >= 0) return getParafromMeter + " : " + paraLabel; 
               }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private int GetParaIndex_1Phase(string inpPra)
       {
           try
           {
               if (inpPra.IndexOf("<AutoScroll>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.AutoScroll;
               else if (inpPra.IndexOf("<PushButton>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.PushButton;
               else if (inpPra.IndexOf("<DispHR>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.DisplayHR;
               else if (inpPra.IndexOf("<DisplayConfig>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.DisplayConfig;
               else if (inpPra.IndexOf("<TamperConfig>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.TamperConfig;
               else if (inpPra.IndexOf("<TamperPersistance>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.TamperPersistance;
               else if (inpPra.IndexOf("<DemandIP>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.DemandIP;
               else if (inpPra.IndexOf("<SurveyIP>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.SURVEYIP;
               else if (inpPra.IndexOf("<BillingDateTime>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.BillingDateTime;
               else if (inpPra.IndexOf("<BillingCycle>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.BillingCycle;
               else if (inpPra.IndexOf("<LoadControl>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.LoadControl;
               else if (inpPra.IndexOf("<TOUPriceSlab>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.TOUPriceSlab;
               else if (inpPra.IndexOf("<Resolution>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.Resolution;
               else if (inpPra.IndexOf("<OverVoltage>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.TamperThresholdOverVoltage;
               else if (inpPra.IndexOf("<LowVoltage>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.TamperThresholdLowVoltage;
               else if (inpPra.IndexOf("<OverLoad>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.TamperThresholdOverLoad;
               else if (inpPra.IndexOf("<OverCurrent>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.TamperThresholdOverCurrent;
               else if (inpPra.IndexOf("<EventLog>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.EventLog;
               else if (inpPra.IndexOf("<EventAlarm>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.EventAlarm;
               else if (inpPra.IndexOf("<ARMBUTTON>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.ARMBUTTON;
               else if (inpPra.IndexOf("<MagneticThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.MagneticThreshold;
               else if (inpPra.IndexOf("<RS485DeviceStatus>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.RS485DeviceStatus;
               else if (inpPra.IndexOf("<RS485DeviceAddress>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.RS485DeviceAddress;
               
               else if (inpPra.IndexOf("<LSCaptureobject>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.LSCaptureobject;
               else if (inpPra.IndexOf("<ESWFConfig>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.ESWFConfig;
               else if (inpPra.IndexOf("<LoadLimitValuekW>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.LoadLimitValuekW;
               else if (inpPra.IndexOf("<PaymentMode>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.PaymentMode;
               else if (inpPra.IndexOf("<MeteringMode>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.MeteringMode;
               else if (inpPra.IndexOf("<OpticalPort>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.OpticalPort;
               else if (inpPra.IndexOf("<RJPort>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.RJPort;

               else if (inpPra.IndexOf("<BatteryMode>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.BatteryMode;
               else if (inpPra.IndexOf("<FSModeLock>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.FSModeLock;

               else if (inpPra.IndexOf("<NodeOverVoltagePersistence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.THROverVoltagePersistence;
               else if (inpPra.IndexOf("<NodeOverVoltageThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.THROverVoltageThreshold;
               else if (inpPra.IndexOf("<NodeLowVoltagePersistence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.THRLowVoltagePersistence;
               else if (inpPra.IndexOf("<NodeLowVoltageThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.THRLowVoltageThreshold;
               else if (inpPra.IndexOf("<NodeOverLoad>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.THROverLoadPersistence;
               else if (inpPra.IndexOf("<NodeOverCurrentPersistence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.THROverCurrentPersistence;
               else if (inpPra.IndexOf("<NodeOverCurrentThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.THROverCurrentThreshold;
               else if (inpPra.IndexOf("<NodeTemperatureRisePersistence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.THRTemperatureRisePersistence;
               else if (inpPra.IndexOf("<NodeTemperatureRiseThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.THRTemperatureRiseThreshold;
               else if (inpPra.IndexOf("<NodeCurrentReversalPersistence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.THRCurrentReversalPersistence;
               else if (inpPra.IndexOf("<NodeCurrentReversalThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.THRCurrentReversalThreshold;


               //else if (inpPra.IndexOf("<ActiveDayProfile>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.ActiveDayProfile;//-Need to Check only Future TOU
               else if (inpPra.IndexOf("<FutureDayProfile>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.FutureDayProfile;
               else if (inpPra.IndexOf("<WeekProfile>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.WeekProfile;
               else if (inpPra.IndexOf("<SeasonProfile>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.SeasonProfile;
               else if (inpPra.IndexOf("<FutureActivationDate>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_1Phase.FutureActivationDate;
               else return -1;
           }
           catch (Exception)
           {
               return -1;
           }
       }

       private int GetParaIndex_3Phase(string inpPra)
       {
           try
           {

               if (inpPra.IndexOf("<InterFrameTimeout>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.INTERFRAMETIMEOUT;
               else if (inpPra.IndexOf("<InActivityTimeout>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.INACTIVITYTIMEOUT;
               else if (inpPra.IndexOf("<kVAhSelection>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.KVAHSELECTION;
               else if (inpPra.IndexOf("<RefVoltage>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.REFVOLT;
               else if (inpPra.IndexOf("<Resolution>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.RESOLUTION;
               else if (inpPra.IndexOf("<BillingDateTime>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.BILLINGDATETIME;
               else if (inpPra.IndexOf("<BillingCycle>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.BILLINGCYCLE;
               else if (inpPra.IndexOf("<MDResetLockoutTime>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.MDRESETLOCKOUTTIME;
               else if (inpPra.IndexOf("<SurveyIP>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.LSCAPTUREPERIOD;
               else if (inpPra.IndexOf("<LSCaptureobject>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.LSCAPTUREOBJECT;
               else if (inpPra.IndexOf("<DemandIP>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.INTEGRATIONPERIOD;
               else if (inpPra.IndexOf("<DemandIPSliding>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.DemandIPSliding;
               else if (inpPra.IndexOf("<DispPushButton>") >= 0 || inpPra.IndexOf("<PushButton>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.DISPPUSH;
               else if (inpPra.IndexOf("<DispAutoScroll>") >= 0 || inpPra.IndexOf("<AutoScroll>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.DISPSCROLL;
               else if (inpPra.IndexOf("<DispHR>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.DISPHR;
               else if (inpPra.IndexOf("<DispTimeout>") >= 0 ) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.DISPTIMEOUT;
               else if (inpPra.IndexOf("<TamperComp>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.TAMPCOMPARTEMENT;
               else if (inpPra.IndexOf("<TamperCompConfig>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.TAMPCOMPARTEMENTCONFIG;
               else if (inpPra.IndexOf("<TamperThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.TAMPTHRESHOULD;
               else if (inpPra.IndexOf("<BillingonLCD>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.NOOFBILLONLCD;
               else if (inpPra.IndexOf("<MfgYear>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.MFGYEAR;
               else if (inpPra.IndexOf("<LoadControl>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.LOADCONTROL;
               else if (inpPra.IndexOf("<TOUSlab>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.TOUSLAB;
               else if (inpPra.IndexOf("<EventLog>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.EVENTLOG;
               else if (inpPra.IndexOf("<AlarmLog>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.ALARMLOG;
               else if (inpPra.IndexOf("<DispCOpen>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.COPENSTATUS;

               else if (inpPra.IndexOf("<DemandMethod>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.DEMANDMETHOD;
               else if (inpPra.IndexOf("<LCDBacklight>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.LCDBackLight;
               else if (inpPra.IndexOf("<CTRatio>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.CTRatio;
               else if (inpPra.IndexOf("<PTRatio>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.PTRatio;
               else if (inpPra.IndexOf("<AutoBilling>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.AutoBilling;
               else if (inpPra.IndexOf("<RS232>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.RS232LockUnlock;
               else if (inpPra.IndexOf("<FactoryConfigurabelity>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.FactoryCongurabelity;
               
               // Tamper Threshold New Implementation
               else if (inpPra.IndexOf("<OverVoltage>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.OVERVOLTAGE;
               else if (inpPra.IndexOf("<LowVoltage>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.LOWVOLTAGE;
               else if (inpPra.IndexOf("<OverLoad>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.OVERLOAD;
               else if (inpPra.IndexOf("<VeryLowPF>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.VERYLOWPF;
               else if (inpPra.IndexOf("<MissingPotential>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.MISSINGPOTENTIAL;
               else if (inpPra.IndexOf("<NeutralDisturbance>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.NEUTRALDISTURBANCE;
               else if (inpPra.IndexOf("<MagneticInfluence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.MAGNETICINFLUENCE;
               else if (inpPra.IndexOf("<CurrentReversal>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.CURRENTREVERSAL;
               else if (inpPra.IndexOf("<CurrentImbalance>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.CURRENTIMBALANCE;
               else if (inpPra.IndexOf("<OverCurrent>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.OVERCURRENT;
               else if (inpPra.IndexOf("<CTOpen>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.CTOPEN;
               else if (inpPra.IndexOf("<CTShort>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.CTSHORT;
               else if (inpPra.IndexOf("<VoltageUnb>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.VOLTAGEUNB;
               else if (inpPra.IndexOf("<PowerOnOff>") >= 0 || inpPra.IndexOf("<NodePowerOnOff>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.POWERONOFF;
               else if (inpPra.IndexOf("<ARMBUTTON>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.ARMBUTTON;
               else if (inpPra.IndexOf("<RS485DeviceAddress>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.RS485DeviceAddress;
               else if (inpPra.IndexOf("<MeterType>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.MeterType;
               else if (inpPra.IndexOf("<DisConnectonMagnet>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.DisConnectOnMagnet;

               else if (inpPra.IndexOf("<MeterPortType>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.MeterType;
               else if (inpPra.IndexOf("<RS485DeviceAddress>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.RS485DeviceAddress;
               else if (inpPra.IndexOf("<RJPORTLOCKS>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.RJPortConfiguration;
               else if (inpPra.IndexOf("<OPTICALPORTLOCKS>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.OpticalPortConfiguration;
               else if (inpPra.IndexOf("<MeteringMode>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.METERINGMODE;
               else if (inpPra.IndexOf("<ESWFConfig>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.ESWFCONFIG;
               else if (inpPra.IndexOf("<LoadLimitValuekW>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.LOADLIMITVALUEKW;
               else if (inpPra.IndexOf("<PaymentMode>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.PAYMENTMODE;

               else if (inpPra.IndexOf("<FSModeLock>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.FSModeLock;

               else if (inpPra.IndexOf("<NodeMissingPotentioalPersistence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRMissingPotentioalPersistence;
               else if (inpPra.IndexOf("<NodeMissingPotentioalThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRMissingPotentioalThreshold;
               else if (inpPra.IndexOf("<NodeVoltageUnbalancePersistence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRVoltageUnbalancePersistence;
               else if (inpPra.IndexOf("<NodeVoltageUnbalanceThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRVoltageUnbalanceThreshold;
               else if (inpPra.IndexOf("<NodeOverVoltagePersistence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THROverVoltagePersistence;
               else if (inpPra.IndexOf("<NodeOverVoltageThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THROverVoltageThreshold;
               else if (inpPra.IndexOf("<NodeLowVoltagePersistence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRLowVoltagePersistence;
               else if (inpPra.IndexOf("<NodeLowVoltageThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRLowVoltageThreshold;
               else if (inpPra.IndexOf("<NodeCurrentReversalPersistence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRCurrentReversalPersistence;
               else if (inpPra.IndexOf("<NodeCurrentReversalThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRCurrentReversalThreshold;
               else if (inpPra.IndexOf("<NodeCTOpenPersistence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRCTOpenPersistence;
               else if (inpPra.IndexOf("<NodeCTOpenThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRCTOpenThreshold;
               else if (inpPra.IndexOf("<NodeCTByPassPersistence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRCTByPassPersistence;
               else if (inpPra.IndexOf("<NodeCTByPassThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRCTByPassThreshold;
               else if (inpPra.IndexOf("<NodeOverCurrentPersistence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THROverCurrentPersistence;
               else if (inpPra.IndexOf("<NodeOverCurrentThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THROverCurrentThreshold;
               else if (inpPra.IndexOf("<NodeCurrentUnbalancePersistence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRCurrentUnbalancePersistence;
               else if (inpPra.IndexOf("<NodeCurrentUnbalanceThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRCurrentUnbalanceThreshold;
               else if (inpPra.IndexOf("<NodePowerONoffPersistence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRPowerONoffPersistence;
               else if (inpPra.IndexOf("<NodeMangeneticInfluence>") >= 0 || inpPra.IndexOf("<NodeMagneticInfluence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRMangeneticInfluence;
               else if (inpPra.IndexOf("<NodeNeutralDisturbance>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRNEUTRALDISTURBANCE;
               else if (inpPra.IndexOf("<NodeVeryLowPFPersistence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRVeryLowPFPersistence;
               else if (inpPra.IndexOf("<NodeVeryLowPFThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRVeryLowPFThreshold;
               else if (inpPra.IndexOf("<NodeOverLoad>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THROverLoad;
               else if (inpPra.IndexOf("<NodeHighNeutralCurrentPersistence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRHighNeutralCurrentPersistence;
               else if (inpPra.IndexOf("<NodeHighNeutralCurrentThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRHighNeutralCurrentThreshold;
               else if (inpPra.IndexOf("<NodeTemperatureRisePersistence>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRTemperatureRisePersistence;
               else if (inpPra.IndexOf("<NodeTemperatureRiseThreshold>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRTemperatureRiseThreshold;
               else if (inpPra.IndexOf("<NodeInvalidPhaseAssociation>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.THRInvalidPhaseAssociationPersistence;
                

               else if (inpPra.IndexOf("<FutureDayProfile>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.FutureDayProfile;
               else if (inpPra.IndexOf("<WeekProfile>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.WeekProfile;
               else if (inpPra.IndexOf("<SeasonProfile>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.SeasonProfile;
               else if (inpPra.IndexOf("<SpecialDaysProfile>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.SpecialDaysProfile;
               else if (inpPra.IndexOf("<FutureActivationDate>") >= 0) return (int)StaticVariables.ENMDisplayConfigStruct_3Ph.FutureActivationDate;

               else return -1;
           }
           catch (Exception)
           {
               return -1;
           }
       }

       public string VerifyCT_ShuntWire(string defaultVal, string refminVal, string refmaxVal)
       {
           try
           {
              if (!objLI.ReadByteFromMeter( DLMSDataStracture.ReadCTWireDataStracture.ReadCTWireOBIS, txtboxobject, "0", 1M, DLMSDataStracture.ReadCTWireDataStracture.ReadCTWireClassID, DLMSDataStracture.ReadCTWireDataStracture.ReadCTWireValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
              string[] datavalue = DLMSDataStracture.DLMSDataFormator(GlobalObjects.objSerialComm.ReceiveBuffer, 18, false);
              if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadCTWireDataStracture.ReadCTWireOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadCTWireDataStracture.ReadCTWireClassID, 0x03)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
              string formattedValue = ApplyScalarUnits(GlobalObjects.objSerialComm.ReceiveBuffer, datavalue[0]);
              if (Convert.ToDecimal(formattedValue) < 0) return StaticVariables.ERRORPreFix + "CT Fail: Reverse , Meter Power = " + formattedValue;
              return objcomnMethod.CheckingRangeValueForDecimal("Active Power =", defaultVal, refminVal, refmaxVal, formattedValue);
               
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string VerifyCT_ShuntWire64KSpecefic(string defaultVal, string refminVal, string refmaxVal)
       {
           try
           {
               //-----------------------------------Read Neutral Current-----------------------------------------------
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.CalibrationDataStracture.CalibrationNeutralCurrentReadOBIS, txtboxobject, "0", 1M, DLMSDataStracture.ReadCTWireDataStracture.ReadCTWireClassID, DLMSDataStracture.ReadCTWireDataStracture.ReadCTWireValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               string[] datavalueNI = DLMSDataStracture.DLMSDataFormator(GlobalObjects.objSerialComm.ReceiveBuffer, 18, false);
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.CalibrationDataStracture.CalibrationNeutralCurrentReadOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadCTWireDataStracture.ReadCTWireClassID, 0x03)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               string formattedValueNeuI = ApplyScalarUnits(GlobalObjects.objSerialComm.ReceiveBuffer, datavalueNI[0]);
               //-----------------------------------Read Phase Current-----------------------------------------------
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.CalibrationDataStracture.CalibrationPhaseCurrentReadOBIS, txtboxobject, "0", 1M, DLMSDataStracture.ReadCTWireDataStracture.ReadCTWireClassID, DLMSDataStracture.ReadCTWireDataStracture.ReadCTWireValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               string[] datavaluePI = DLMSDataStracture.DLMSDataFormator(GlobalObjects.objSerialComm.ReceiveBuffer, 18, false);
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.CalibrationDataStracture.CalibrationPhaseCurrentReadOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadCTWireDataStracture.ReadCTWireClassID, 0x03)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               string formattedValuePhI = ApplyScalarUnits(GlobalObjects.objSerialComm.ReceiveBuffer, datavaluePI[0]);
               //-----------------------------------Compare Phase & Neu Cuttent--------------------------------------
               string formattedCurrentmsg="Current: Neu =" + formattedValueNeuI + ", " + "Ph =" + formattedValuePhI;
               if (Convert.ToDecimal(formattedValuePhI) >= (Convert.ToDecimal(formattedValueNeuI) * 20M / 100M))
                   return StaticVariables.ERRORPreFix + "Invalid " + formattedCurrentmsg;
               //-------------------------------------Read Active Power-----------------------------------------------
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadSignedActivePowerDataStracture.SignedActivePowerOBIS, txtboxobject, "0", 1M, DLMSDataStracture.ReadSignedActivePowerDataStracture.SignedActivePowerClassID, DLMSDataStracture.ReadSignedActivePowerDataStracture.SignedActivePowerValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               string[] datavalue = DLMSDataStracture.DLMSDataFormator(GlobalObjects.objSerialComm.ReceiveBuffer, 18, false);
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadSignedActivePowerDataStracture.SignedActivePowerOBIS, txtboxobject, "0", 100M, DLMSDataStracture.ReadSignedActivePowerDataStracture.SignedActivePowerClassID, 0x03)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               string formattedValue = ApplyScalarUnits(GlobalObjects.objSerialComm.ReceiveBuffer, datavalue[0]);
               if (Convert.ToDecimal(formattedValue) < 0) return StaticVariables.ERRORPreFix + "CT Fail: Reverse , Meter Power = " + formattedValue + ", " +formattedCurrentmsg;
               return objcomnMethod.CheckingRangeValueForDecimal("Active Power =", defaultVal, refminVal, refmaxVal, formattedValue) + ", " + formattedCurrentmsg; ;

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string VerifyBatteryUsingFlag()
       {
           try
           {
               if (!objLI.ReadByteFromMeter(DLMSDataStracture.ReadBatteryStatusDataStracture.ReadBatteryStatusOBIS, txtboxobject, "0", 1M, DLMSDataStracture.ReadBatteryStatusDataStracture.ReadBatteryStatusClassID, DLMSDataStracture.ReadBatteryStatusDataStracture.ReadBatteryStatusValueAttribute)) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               return BatteryOfRTCandMainPass(GlobalObjects.objSerialComm.ReceiveBuffer);
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }            

       private string BatteryOfRTCandMainPass(byte[] receivedData)
       {
           try
           {
               //RTC Battery (18 + 1 + 4) Main Battery (18 + 1 + 9)
               if ((receivedData[18 + 1 + 4] == 0x01) && (receivedData[18 + 1 + 9] == 0x01)) //Both RTC & Main Pass
               {
                   return "Battery Status >> " + "RTC Value =" + GlobalObjects.objSerialComm.ReceiveBuffer[18 + 1 + 4]
                                                                      + ", Main Value =" + GlobalObjects.objSerialComm.ReceiveBuffer[18 + 1 + 9];
               }
               else if ((receivedData[18 + 1 + 4] != 0x01) && (receivedData[18 + 1 + 9] != 0x01)) //Both RTC & Main Fail
               {
                   return StaticVariables.ERRORPreFix + "Battery Status >> " + "RTC Value =" + GlobalObjects.objSerialComm.ReceiveBuffer[18 + 1 + 4]
                                                                       + ", Main Value =" + GlobalObjects.objSerialComm.ReceiveBuffer[18 + 1 + 9];
               }
               else if (receivedData[18 + 1 + 4] != 0x01) //RTC Fail
               {
                   return StaticVariables.ERRORPreFix + "Battery Status >> " + "RTC Value =" + GlobalObjects.objSerialComm.ReceiveBuffer[18 + 1 + 4]
                                                        + ", Main Value =" + GlobalObjects.objSerialComm.ReceiveBuffer[18 + 1 + 9];
               }
               else //if (receivedData[18 + 1 + 9] != 0x01) //Main Fail
               {
                   return StaticVariables.ERRORPreFix + "Battery Status >> " + "RTC Value =" + GlobalObjects.objSerialComm.ReceiveBuffer[18 + 1 + 4]
                                                         + ", Main Value =" + GlobalObjects.objSerialComm.ReceiveBuffer[18 + 1 + 9];
               }
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       //-----------------------------------------LPR Module Testing------------------------------------------------------------------------

       public string LPRTestAnalogic(string strMeterID,string lprID,string defPortName)
       {
           string mIDresponse = StaticVariables.ERRORPreFix;
           if (defPortName == objappSettings.GetPortName()) return StaticVariables.ERRORPreFix + "Opto & LPR COM Ports Can Not Be Same !";
             try
               {                 

                   for (int nRetry = 0; nRetry < 2; nRetry++)
                   { 
                       GlobalObjects.objSerialComm.SetSerialPortSettings(defPortName, "9600", "None", "8", "1", 3000, 500);
                       GlobalObjects.objSerialComm.OpenPort();

                       byte[] LPRSend = new byte[500];
                       byte[] LPRSend1 = new byte[500];
                       string[] words = new string[] { };
                       
                       char[] delimiterChars = { '!' };
                       words = lprID.Split(delimiterChars);
                       //long i = words.Length;
                       for (int i = 1; i < words.Length - 1; i = i + 2)
                       {
                           LPRSend[0] = Convert.ToByte('+');
                           LPRSend[1] = Convert.ToByte('+');
                           LPRSend[2] = Convert.ToByte('+');
                           LPRSend[3] = 0x0A;
                           GlobalObjects.objSerialComm.fSendDataToPort(LPRSend, 4);
                           System.Threading.Thread.Sleep(100);

                           Array.Clear(LPRSend, 0, 10);
                           LPRSend[0] = Convert.ToByte('A');
                           LPRSend[1] = Convert.ToByte('T');
                           LPRSend[2] = Convert.ToByte('+');
                           LPRSend[3] = Convert.ToByte('T');
                           LPRSend[4] = Convert.ToByte('O');
                           LPRSend[5] = Convert.ToByte('=');
                           LPRSend[6] = Convert.ToByte('1');
                           LPRSend[7] = Convert.ToByte('0');
                           LPRSend[8] = 0x0A;
                           GlobalObjects.objSerialComm.fSendDataToPort(LPRSend, 9);
                           System.Threading.Thread.Sleep(100);

                           Array.Clear(LPRSend, 0, 10);
                           LPRSend1 = System.Text.Encoding.UTF8.GetBytes(words[i]);
                           LPRSend[0] = Convert.ToByte('A');
                           LPRSend[1] = Convert.ToByte('T');
                           LPRSend[2] = Convert.ToByte('D');
                           LPRSend[3] = Convert.ToByte('T');
                           Array.Copy(LPRSend1, 0, LPRSend, 4, words[i].Length);
                           LPRSend[4 + words[i].Length] = 0x0A;
                           byte nLength = Convert.ToByte(words[i].Length + 5);
                           //Connect LPR moule using LPR ID
                           GlobalObjects.objSerialComm.fSendDataToPort(LPRSend, nLength);
                           System.Threading.Thread.Sleep(3000);
                           string  strReceived = Encoding.UTF8.GetString(GlobalObjects.objSerialComm.ReceiveBuffer, 0, GlobalObjects.objSerialComm.ReceiveBuffer.Length);
                           if (!strReceived.Contains("CONNECTED SUCCESSFULLY"))
                               continue;

                           if (objLI.HDLCLayerConnect() && objLI.AssociationStablish()) //Pass 
                           {
                               lprID = lprID.Replace("!", "");
                               mIDresponse = "PCBAID:" + VerifyMeterID(strMeterID) + ", LPRID:" + lprID;                               
                           }
                           objLI.AssociationDisconnect();
                           System.Threading.Thread.Sleep(50);
                           LPRSend[0] = Convert.ToByte('+');
                           LPRSend[1] = Convert.ToByte('+');
                           LPRSend[2] = Convert.ToByte('+');
                           LPRSend[3] = 0x0A;
                           GlobalObjects.objSerialComm.fSendDataToPort(LPRSend, 4);
                           System.Threading.Thread.Sleep(100);
                           strReceived = Encoding.UTF8.GetString(GlobalObjects.objSerialComm.ReceiveBuffer, 0, GlobalObjects.objSerialComm.ReceiveBuffer.Length);
                           if (strReceived.Contains("DISCONNECTED")) break;
                           CommandExecutionWaitTimer(10000);
                           break; 
                       }
                       GlobalObjects.objSerialComm.ClosePort();
                       if (mIDresponse.IndexOf(StaticVariables.ERRORPreFix) < 0) break;
                   }
                   if (mIDresponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return mIDresponse + "Unable to Connect with LPR Module";
                   //if (mIDresponse == "Error : ") return StaticVariables.ERRORPreFix + "Unable to Connect with LPR Module";
                   else return mIDresponse;
               }
               catch (Exception ex)
               {
                   GlobalObjects.objSerialComm.ClosePort();                  
                   return StaticVariables.ERRORPreFix + ex.Message;
               }
               
       }


       public string LPRTestMilange(string strMeterID, string lprID, string defPortName)
       {
           
           if (defPortName == objappSettings.GetPortName()) return StaticVariables.ERRORPreFix + "Opto & LPR COM Ports Can Not Be Same !";
           try
           {
               bool isConnectedStage = false;
              
              
                   GlobalObjects.objSerialComm.SetSerialPortSettings(defPortName, "115200", "None", "8", "1", 2200, 500);
                   GlobalObjects.objSerialComm.OpenPort();

                   byte[] LPRSend = new byte[500];
                   byte[] LPRDisconnect = new byte[] { 0x2A, 0x05, 0x02, 0x01, 0x00, 0x61, 0x3C, 0xC1, 0xF6,0x01 };
                   string[] words = new string[] { };                  
                       
                       int commandByteCount = 0;
                       int retryCounts = 0;

                       if (!isConnectedStage)
                       {
                           //------------------LPR ID Command-----------------------
                           commandByteCount = 0;
                           
                           LPRSend[commandByteCount++] = 0x2A;
                           LPRSend[commandByteCount++] = 0x0D;                       
                           LPRSend[commandByteCount++] = 0x02;
                           LPRSend[commandByteCount++] = 0x01;
                           LPRSend[commandByteCount++] = 0x00;
                           LPRSend[commandByteCount++] = 0x60;
                           LPRSend[commandByteCount++] = 0x3C;
                           LPRSend[commandByteCount++] = 0xC1;
                           LPRSend[commandByteCount++] = 0xF6;
                           LPRSend[commandByteCount++] = 0x01;
                           int lprIDCount = 0;

                           byte[] inputBytes = Encoding.UTF8.GetBytes(lprID);

                           while (lprIDCount < lprID.Length)
                           {
                               LPRSend[commandByteCount++] = Convert.ToByte(((char)(Convert.ToInt32((lprID.Substring(lprIDCount, 2)), 16))));
                               lprIDCount += 2;
                           }
                           retryCounts = 0;
                           while (!isConnectedStage)
                           {
                               GlobalObjects.objSerialComm.fSendDataToPort(LPRSend, commandByteCount);
                               if (GlobalObjects.objSerialComm.ReceiveBuffer[2] == 0x82 && GlobalObjects.objSerialComm.ReceiveBuffer[4] == 0x60 && GlobalObjects.objSerialComm.ReceiveBuffer[5] == 0x00) isConnectedStage = true;
                               if (retryCounts++ > 2) break;
                           }
                       }
                       if (!isConnectedStage) return StaticVariables.ERRORPreFix + "Unable to Connect with LPR Module";
                       //--------------------Meter SignOn------------------------------------------
                       GlobalObjects.objSerialComm.InterchatracterDelay = 2500;
                       GlobalObjects.objSerialComm.CommandTimeout = 3500;
                       if (!objLI.HDLCLayerConnect()) return StaticVariables.ERRORPreFix + "HDLC Layer Connection Failed Over LPR !";
                       if (!objLI.AssociationStablish()) return StaticVariables.ERRORPreFix + "Unable To Stablish Association Over LPR!"; 
                       string getpcbaIDoverLPR = ReadPCBAID();
                       if (getpcbaIDoverLPR.IndexOf(StaticVariables.ERRORPreFix) >= 0) return getpcbaIDoverLPR;
                       //------------------------LPR Disconnect-------------------------
                           retryCounts = 0;
                           while (retryCounts++ < 2)
                           {
                               GlobalObjects.objSerialComm.fSendDataToPort(LPRDisconnect, 6);
                               if (GlobalObjects.objSerialComm.ReceiveBuffer[2] == 0x82 && GlobalObjects.objSerialComm.ReceiveBuffer[4] == 0x61 && GlobalObjects.objSerialComm.ReceiveBuffer[5] == 0x00) break;
                           }
                           GlobalObjects.objSerialComm.ClosePort();
                           return "PCBAID:" + getpcbaIDoverLPR + ", LPRID:" + lprID;
                      
           }
           catch (Exception ex)
           {
               GlobalObjects.objSerialComm.ClosePort();
               return StaticVariables.ERRORPreFix + ex.Message;
           }

       }

       public bool ISSinglePhaseDLMS64KMeterVariants(string metersignature)
       {
           try
           {
               List<string> listofMeterVariants = new List<string>();
               listofMeterVariants.Add("VB");
               listofMeterVariants.Add("vb");
               listofMeterVariants.Add("VF");
               listofMeterVariants.Add("CF");
               listofMeterVariants.Add("CB");
               foreach (var item in listofMeterVariants)
               {
                   if (metersignature.Contains(item)) return true;
               }
               return false;
           }
           catch (Exception)
           {
               return false;
           }
       }

       //-------------------------------------End of Configuration Verification-------------------------------------------------------------
        
    }
}
