using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationInterface;
using System.Text.RegularExpressions;
using System.Threading;
using COMMONENTITY;
using Utilities;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Forms;
using SystemSecurityLibrary;
namespace COMMONENTITY
{
   public class IECCommonCommandMethods
    {
       IECLayerInterface objIECLI = new IECLayerInterface();     
       CommonMethods objcomnMethod = new CommonMethods();
       StandardDateTime stddt = new StandardDateTime();
       GlobalMethods objstst = new GlobalMethods();
       AppSettings objappSettings = new AppSettings();
       public bool IsTravellerWriteSkip = false;
       public string ReadPCBAID()
       {
           string CmdResponse = "";
           try
           {
               CmdResponse = ReadDataFromMeter("3033(0A)", StaticVariables._R1, "\x03");
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               CmdResponse = CmdResponse.Substring(1, CmdResponse.Length - 2);
               string meterPCBAID = FormatASCIIData(CmdResponse).Trim();
               if (meterPCBAID.Trim().Length <= 0) return StaticVariables.ERRORPreFix + "PCBA ID is Blank";
               else
               {
                   string  unloacmeterResponse ="";
                   if (IsTravellerWriteSkip == false) unloacmeterResponse = LockingMeter("00");//------------Unlock the meter , It may Lock--------------
                   if (unloacmeterResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return StaticVariables.ERRORPreFix + unloacmeterResponse;
                   return meterPCBAID;
               }
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message + "Meter Response :" + CmdResponse;
           }
            
       }

       public string ReadPCBAID_ZCE()
       {
           try
           {
               Thread.Sleep(200);
               string CmdResponse = ReadDataFromMeter("108E(0A)", StaticVariables._R1, "\x03");
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               CmdResponse = CmdResponse.Substring(1, CmdResponse.Length - 2);
               string meterPCBAID = FormatASCIIData(CmdResponse).Trim();
               if (meterPCBAID.Trim().Length <= 0) return StaticVariables.ERRORPreFix + "PCBA ID is Blank";
               else
               {
                   string unloacmeterResponse = LockingMeterZCE("00");//------------Unlock the meter , It may Lock--------------
                   if (unloacmeterResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return StaticVariables.ERRORPreFix + unloacmeterResponse;
                   return meterPCBAID;
               }
               
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string VerifySHUNTWire(string defaultVal, string minVal, string maxVal)
       {
           try
           {
               Thread.Sleep(200);
               //-------------------------Verify CT or Shunt Current--------------------------------
               string CmdResponse = ReadDataFromMeter("E001(16)", StaticVariables._R1, "\x03");
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               CmdResponse = CmdResponse.Substring(1, CmdResponse.Length - 2);
               string resultResponse = IsValidSHUNTCurrent(CmdResponse, defaultVal, minVal, maxVal);
               if (resultResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return resultResponse;
               //-------------------------Verify CT or Shunt Flag--------------------------------
               Thread.Sleep(200);
               string CmdResponseShunt = ReadDataFromMeter("E004(01)", StaticVariables._R1, "\x03");
               if (CmdResponseShunt.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponseShunt;
               CmdResponseShunt = CmdResponseShunt.Substring(1, CmdResponseShunt.Length - 2);
               string ctFlagResponse = IsValidCTorSHUNTFlag(CmdResponseShunt);
               return resultResponse + ", " + ctFlagResponse;               
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string VerifyCTWire(string defaultVal, string minVal, string maxVal)
       {
           try
           {
               //-------------------------Verify CT or Shunt Neu Current--------------------------------               
               string resultResponse = VerifyMeterCurrent(defaultVal, minVal, maxVal, (int)StaticVariables.MeterCurrentType.NeutralCurrent);
               if (resultResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return resultResponse;
               //-------------------------Verify CT or Shunt Flag--------------------------------
               string CmdResponseShunt = ReadDataFromMeter("E004(01)", StaticVariables._R1, "\x03");
               if (CmdResponseShunt.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponseShunt;
               CmdResponseShunt = CmdResponseShunt.Substring(1, CmdResponseShunt.Length - 2);
               string ctFlagResponse = IsValidCTorSHUNTFlag(CmdResponseShunt);
               return resultResponse + ", " + ctFlagResponse;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string VerifyMeterCurrent(string defaultVal, string minVal, string maxVal,int MeterCurrentType)
       {
           try
           {
               //-------------------------Verify CT or Shunt Current--------------------------------
               string CmdResponse = ReadDataFromMeter("E001(16)", StaticVariables._R1, "\x03");
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               CmdResponse = CmdResponse.Substring(1, CmdResponse.Length - 2);
               string resultResponse = IsValidCTCurrent(CmdResponse, defaultVal, minVal, maxVal, MeterCurrentType);
               return resultResponse;               
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public string VerifyMeterVoltage(string defaultVal, string minVal, string maxVal)
       {
           try
           {
               //-------------------------Verify Meter Voltage--------------------------------
               string CmdResponse = ReadDataFromMeter("E001(16)", StaticVariables._R1, "\x03");
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               CmdResponse = CmdResponse.Substring(1, CmdResponse.Length - 2);
               string resultResponse = IsValidVoltage(CmdResponse, defaultVal, minVal, maxVal);
               return resultResponse;
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
               string CmdResponse = ReadDataFromMeter("E002(14)", StaticVariables._R1, "\x03");
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               string resultResponse = IsValidTemprature(CmdResponse.Substring(33, 4), defaultVal, minVal, maxVal);
               return resultResponse;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public string VerifyTemprature_VIM(string defaultVal, string minVal, string maxVal)
       {
           try
           {
               string CmdResponse = ReadDataFromMeter("E01A(02)", StaticVariables._R1, "\x03");
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               string resultResponse = IsValidTemprature(CmdResponse.Substring(1,4), defaultVal, minVal, maxVal);
               return resultResponse;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public string VerifyTraveler_VIMNONDLMS(int defaultVal)
       {
           try
           {
                string CmdResponse="";
                string StatusValues = "";
                int stageFlag = -1;
                int statusFlag = -1;
                if (defaultVal == 1) //---Verify only for FT
                {
                    CmdResponse = ReadDataFromMeter("300F(01)", StaticVariables._R1, "\x03"); //---3000 (Schudo address) + 0F 
                    if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return "Invalid Status=" + CmdResponse;
                    if (CmdResponse.Length >= 4) statusFlag = Convert.ToInt16(CmdResponse.Substring(1, 2)) - 30;
                    if (statusFlag == 1) StatusValues = "Status=" + statusFlag + ", ";
                    else StatusValues = StaticVariables.ERRORPreFix + "Invalid Status=" + StatusValues + ", ";
                    
                }
                CmdResponse = ReadDataFromMeter("3028(01)", StaticVariables._R1, "\x03"); //---3000 (Schudo address) + 0F 
                if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return "Invalid Stage=" + CmdResponse;
                if (CmdResponse.Length >= 4) stageFlag = Convert.ToInt16(CmdResponse.Substring(1, 2)) - 30;// objcomnMethod.HexToDecimalConversion(CmdResponse.Substring(1, 2));

                if (defaultVal == 1) //-----FT Stage
                {
                    if (stageFlag == 9) return StatusValues + StaticVariables.ERRORPreFix + "Invalid Stage=" + stageFlag.ToString(); ;//--Fail only if found 9, Specefic Case
                }
                else if (defaultVal == 2)//----Calibration Stage
                {
                    if (stageFlag < 3) return StatusValues + StaticVariables.ERRORPreFix + "Invalid Stage=" + stageFlag.ToString(); ;
                }
                else if (defaultVal == 3)//----Serialization Stage
                {
                    if (stageFlag < 5) return StatusValues + StaticVariables.ERRORPreFix + "Invalid Stage=" + stageFlag.ToString(); ;
                }
                return StatusValues + "Stage=" + stageFlag.ToString();

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public bool SetTravelerData_VIMNONDLMS(int defaultVal)
       {
           try
           {
               string CmdResponse = StaticVariables.ERRORPreFix;
               if (defaultVal == 0) CmdResponse = WriteDataToMeter("3028(31)", StaticVariables._W1);  //---EMS Test
               else if (defaultVal == 1) CmdResponse = WriteDataToMeter("3028(33)", StaticVariables._W1); //---FT Test
               else if (defaultVal == 2) CmdResponse = WriteDataToMeter("3028(35)", StaticVariables._W1); //---Calibration Test
               else if (defaultVal == 3) CmdResponse = WriteDataToMeter("3028(37)", StaticVariables._W1); //---Serialization Test
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return false;
               else return true;
           }
           catch (Exception)
           {
               return false;
           }
       }
       public string StartPushButtonRest()
       {
           try
           {
               string CmdResponse = WriteDataToMeter("E018(01)", StaticVariables._W1);
               return CmdResponse;  
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       public string VerifyPushButtonCount(string defaultVal, string minVal, string maxVal)
       {
           try
           {
               string CmdResponse = ReadDataFromMeter("E013(01)", StaticVariables._R1, "\x03");
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               string resultResponse = IsValidPushCount(CmdResponse, defaultVal, minVal, maxVal);
               return resultResponse;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       private string IsValidPushCount(string CmdResponse, string defaultVal, string minVal, string maxVal)
       {
           string validatedResponse = string.Empty;
           try
           {
               CmdResponse = objcomnMethod.ReverseString(CmdResponse.Substring(1, 2), 0);
               decimal calCulatedTemp = Convert.ToDecimal(CmdResponse);
               validatedResponse = "Push Button Count =" + calCulatedTemp.ToString("0");
               if (objcomnMethod.isValidReadParameters(defaultVal, minVal, maxVal, calCulatedTemp)) return validatedResponse;
               else  return StaticVariables.ERRORPreFix + validatedResponse;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       private string IsValidTemprature(string CmdResponse, string defaultVal, string minVal, string maxVal)
       {
           string validatedResponse = string.Empty;
           try
           {               
               CmdResponse = objcomnMethod.ReverseString(CmdResponse,0);
               decimal calCulatedTemp = Convert.ToDecimal(CmdResponse)/10M;
               if (objcomnMethod.isValidReadParameters(defaultVal, minVal, maxVal,calCulatedTemp )) validatedResponse = calCulatedTemp.ToString("0.00");
               else validatedResponse = StaticVariables.ERRORPreFix + calCulatedTemp.ToString("0.00");
               return validatedResponse;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
      

       public string TestRTCDrift(string defaultVal, string minVal, string maxVal)
       {
           try
           {
               string CmdResponse = ReadDataFromMeter("C001()", StaticVariables._R2, "\x03");
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               CmdResponse = CmdResponse.Substring(1, CmdResponse.Length - 2);
               string resultResponse = IsValidMeterRTCDrift(CmdResponse, defaultVal, minVal,  maxVal);
               return resultResponse;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string TestRTCDriftZCE(string defaultVal, string minVal, string maxVal)
       {
           try
           {

               string CmdResponse = ReadDataFromMeter("C001()", StaticVariables._R2, "\x03");
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               CmdResponse = CmdResponse.Substring(1, CmdResponse.Length - 2);
               string resultResponse = IsValidMeterRTCDriftZCE(CmdResponse, defaultVal, minVal, maxVal);
               return resultResponse;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private string IsValidMeterRTCDrift(string commandResponse, string defaultVal, string minVal, string maxVal)
       {
           string validatedResponse = string.Empty;
           try
           {
               DateTime systemtimeonreadout = DateTime.Now;
               string ReadindDT = commandResponse;
               if (ReadindDT == "00000000000000") return StaticVariables.ERRORPreFix + ReadindDT;
               //----------------Check RTC Drift-------
               DateTime meterDatetime = objcomnMethod.ConvertIECMeterResponseTodateZCE(ReadindDT);
               if (meterDatetime == new DateTime()) return StaticVariables.ERRORPreFix + string.Format("{0:ddMMyyHHmmss}", meterDatetime);               
               TimeSpan get_diff = systemtimeonreadout - meterDatetime;
               string calDrift =objcomnMethod.CheckingRangeValueForDecimal("Meter Drift (Sec.) = ", defaultVal, minVal, maxVal, ((long)get_diff.TotalSeconds).ToString());
               return calDrift + ",  Meter Stamp =" + string.Format("{0:dd/MM/yyyy HH:mm:ss}", meterDatetime) + ",  System Stamp =" + string.Format("{0:dd/MM/yyyy HH:mm:ss}", systemtimeonreadout) ;

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private string IsValidMeterRTCDriftZCE(string commandResponse, string defaultVal, string minVal, string maxVal)
       {
           string validatedResponse = string.Empty;
           try
           {
               DateTime systemtimeonreadout = DateTime.Now;
               string ReadindDT = commandResponse;
               if (ReadindDT == "00000000000000") return StaticVariables.ERRORPreFix + ReadindDT;
               //----------------Check RTC Drift-------
               DateTime meterDatetime = objcomnMethod.ConvertIECMeterResponseTodate(ReadindDT);
               if (meterDatetime == new DateTime()) return StaticVariables.ERRORPreFix + string.Format("{0:ddMMyyHHmmss}", meterDatetime);
               TimeSpan get_diff = systemtimeonreadout - meterDatetime;
               string calDrift = objcomnMethod.CheckingRangeValueForDecimal("Meter Drift (Sec.) = ", defaultVal, minVal, maxVal, ((long)get_diff.TotalSeconds).ToString());
               return calDrift + ",  Meter Stamp =" + string.Format("{0:dd/MM/yyyy HH:mm:ss}", meterDatetime) + ",  System Stamp =" + string.Format("{0:dd/MM/yyyy HH:mm:ss}", systemtimeonreadout);

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private string IsValidSHUNTCurrent(string CmdResponse, string defaultVal, string minVal, string maxVal)
       {
           string validatedResponse = string.Empty;
           try
           {
               CmdResponse = objcomnMethod.HexToDecimalConversion(CmdResponse.Substring(4, 8));
               CmdResponse = (Convert.ToDecimal(CmdResponse) / 1000M).ToString();
               decimal current = Convert.ToDecimal(CmdResponse);
               if (!objcomnMethod.isValidReadParameters(defaultVal, minVal, maxVal, current)) return StaticVariables.ERRORPreFix + "Failure CT/Shunt Current =" + CmdResponse;
               return "Meter Current =" + CmdResponse;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + "Failure CT/Shunt Current=" + CmdResponse + ",  " + ex;
           }
       }

       private string IsValidCTCurrent(string CmdResponse, string defaultVal, string minVal, string maxVal, int MeterCurrentType)
       {
           string validatedResponse = string.Empty;
           string responseMessgae = "Meter Neutral Current =";
           try
           {
               if (MeterCurrentType == (int)StaticVariables.MeterCurrentType.NeutralCurrent) CmdResponse = objcomnMethod.HexToDecimalConversion(CmdResponse.Substring(12, 8));
               else { responseMessgae = "Meter Phase Current ="; CmdResponse = objcomnMethod.HexToDecimalConversion(CmdResponse.Substring(4, 8)); }
               CmdResponse = (Convert.ToDecimal(CmdResponse) / 1000M).ToString();
               int current = (int)Convert.ToDecimal(CmdResponse);
               if (!objcomnMethod.isValidReadParameters(defaultVal, minVal, maxVal, current)) return StaticVariables.ERRORPreFix + responseMessgae + CmdResponse;
               return responseMessgae + CmdResponse;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + "Failure CT/Shunt Current=" + CmdResponse + ",  " + ex;
           }
       }
       private string IsValidVoltage(string CmdResponse, string defaultVal, string minVal, string maxVal)
       {
           string validatedResponse = string.Empty;
           
           try
           {
              string responseMessgae = "Meter Phase Current ="; CmdResponse = objcomnMethod.HexToDecimalConversion(CmdResponse.Substring(4, 8));  
               CmdResponse = (Convert.ToDecimal(CmdResponse) / 1000M).ToString();
               int current = (int)Convert.ToDecimal(CmdResponse);
               if (!objcomnMethod.isValidReadParameters(defaultVal, minVal, maxVal, current)) return StaticVariables.ERRORPreFix + responseMessgae + CmdResponse;
               return responseMessgae + CmdResponse;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + "Failure CT/Shunt Current=" + CmdResponse + ",  " + ex;
           }
       }
       private string IsValidCTorSHUNTFlag(string CmdResponse)
       {
           string validatedResponse = string.Empty;
           try
           {
               CmdResponse = objcomnMethod.HexToDecimalConversion(CmdResponse.Substring(0, 2));
               if (CmdResponse.Length <= 0) return "Invalid Meter Response =" + CmdResponse;
               CmdResponse = Convert.ToString(Convert.ToInt32(CmdResponse), 2);
               CmdResponse = CmdResponse.PadLeft(8, '0');
               if (CmdResponse.Substring(0, 1) == "0") return  "CT/Shunt Wire OK =" + CmdResponse.Substring(0, 1);
               else return StaticVariables.ERRORPreFix + "  CT/Shunt Wire Reverse =" + CmdResponse.Substring(0, 1);
               
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + "Failure CT/Shunt =" + CmdResponse + ",  " + ex;
           }
       }

       public string VerifyEnergy(string defaultVal, string refminVal, string refmaxVal)
       {
               string CmdResponse = "";
               try
               {
                   CmdResponse = ReadDataFromMeter("F00B(11)", StaticVariables._R1, "\x03");                    
                   if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
                   return IsValidEnergy(CmdResponse, defaultVal, refminVal, refmaxVal);
               }
               catch (Exception ex)
               {
                   return StaticVariables.ERRORPreFix + ex.Message + "Meter Response :" + CmdResponse;
               }

            
       }

       private string IsValidEnergy(string CmdResponse, string defaultVal, string minVal, string maxVal)
       {
           string validatedResponse = string.Empty;           
           try
           {
               if (CmdResponse.Length < 10) return StaticVariables.ERRORPreFix + CmdResponse;
               string ValueCmdResponse = objcomnMethod.HexToDecimalConversion(CmdResponse.Substring(1, 8));                
               decimal IR_kWh = (Convert.ToDecimal(ValueCmdResponse) / 1000M);
               if (objcomnMethod.isValidReadParameters(defaultVal, minVal, maxVal, IR_kWh)) validatedResponse = " Meter Energy Is  =" + IR_kWh.ToString();
               else validatedResponse = StaticVariables.ERRORPreFix + " Meter Energy Is  =" + IR_kWh.ToString() + ", Response =" + CmdResponse;
               return validatedResponse;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message + ", Response =" + CmdResponse;  
           }
       }
       public string IsValidVoltage(string defaultVal, string minVal, string maxVal)
       {
           string validatedResponse = string.Empty;
           string meterVoltage = string.Empty;
           try
           {
               if (!objIECLI.PhysicalLayerConnect(false)) { return StaticVariables.ERRORPreFix + "Signon Failed on Battery mode !"; }
               if (!objIECLI.HDLCLayerConnect(out validatedResponse)) { return StaticVariables.ERRORPreFix + "Signon Failed on Battery mode !"; }
               string tempReadCommand = GlobalObjects.objIECSerialComm._IEC_SIGNON;//------Keep Backup of Orginal Signon Command
               string tempStopResponseByte = GlobalObjects.objIECSerialComm.StopResponseByte;//------Keep Backup of Orginal Stop Response Byte Command
               GlobalObjects.objIECSerialComm.StopResponseByte = "\x21";
               GlobalObjects.objIECSerialComm._IEC_SIGNON = "063035300D0A";                  //------Assign Readout Command to Signon For General data readout
               GlobalObjects.objIECSerialComm.SignOn() ;                                     //------Read, Readout Data via signon Genric Command
               GlobalObjects.objIECSerialComm._IEC_SIGNON = tempReadCommand;                 //------Re-Assign orginal Signon Command
               GlobalObjects.objIECSerialComm.StopResponseByte = tempStopResponseByte;       //------Re-Assign orginal Stop Response Byte
               validatedResponse = GlobalObjects.objIECSerialComm.strOutBuff;    
               if (validatedResponse.Contains("12.7("))                                      //-----Verify Voltage if Available
               {
                   meterVoltage = validatedResponse.Substring(validatedResponse.IndexOf("12.7(")+5);
                   meterVoltage = meterVoltage.Substring(0, meterVoltage.IndexOf(")") -1);
               }
               else return StaticVariables.ERRORPreFix + "Voltage Not Available in Readout Response";
               validatedResponse = objcomnMethod.CheckingRangeValueForDecimal("Meter Voltage =", defaultVal, minVal, maxVal, meterVoltage);
               return validatedResponse;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message + ", Response =" + validatedResponse;
           }
       }

       public string VerifyMeterPassword(string defaultVal)
       {
           string CmdResponse = "";
           try
           {
                   string validatedResponse = string.Empty;
                   CmdResponse = ReadDataFromMeter("304E(08)", StaticVariables._R1, "\x03");
                   if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
                   if (CmdResponse.Length < 8) return StaticVariables.ERRORPreFix + CmdResponse;
                   CmdResponse = CmdResponse.Substring(1, CmdResponse.Length - 2);
                   string meterpwd= FormatASCIIData(CmdResponse);
                   if ( meterpwd.Length > 0 && meterpwd == defaultVal) validatedResponse = " Meter Password Is  =" +  meterpwd ;
                   else validatedResponse = StaticVariables.ERRORPreFix + " Meter Password Is  =" +  meterpwd ;
                   return validatedResponse;                 

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message + "Meter Response :" + CmdResponse;
           }
       }
        
       public string VerifyPCBAFirmwareVersion(string defaultVal)
       {
           try
           {
               defaultVal = defaultVal.Replace(".", "");
               string validatedResponse = string.Empty;
               string CmdResponse = ReadDataFromMeter("3000(10)", StaticVariables._R1, "\x03");               
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               CmdResponse = CmdResponse.Substring(21, 10);
               string emsfwversion = FormatASCIIData(CmdResponse).Trim();
               if (emsfwversion.Trim().Length > 0 && emsfwversion == defaultVal) validatedResponse = ",  EMS FW =" + emsfwversion;
               else validatedResponse = ", " + StaticVariables.ERRORPreFix + " EMS FW =" + emsfwversion;
               return validatedResponse;

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       
       public string VerifyCalibrationData()
       {
           string[] CmdResponse = new string[] { "", "", "" };
           try
           {
               
                CmdResponse[0] = ReadDataFromMeter("313F(1E)", StaticVariables._R1, "\x03");
                if (CmdResponse[0].IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse[0];
                Thread.Sleep(300);
                CmdResponse[1] = ReadDataFromMeter("315D(1E)", StaticVariables._R1, "\x03");
                if (CmdResponse[1].IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse[1];               
                if (CmdResponse.Length >= 2) return IsValidCalibrationData(CmdResponse);
                else return StaticVariables.ERRORPreFix + " Invalid Meter Response";                
               
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message + "Meter Response :" + CmdResponse[0] + ", " + CmdResponse[1];
           }
       }

       public string VerifyCalibrationDataZCE()
       {
           try
           {
               string CmdResponse ="";
               CmdResponse = ReadDataFromMeter("1098(18)", StaticVariables._R1, "\x03");
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               return IsValidCalibrationDataZCE(CmdResponse);               

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private string IsValidCalibrationDataZCE(string commandResponse)
       {
           try
           {
               commandResponse = commandResponse.Substring(commandResponse.IndexOf("(") + 1);
               string MMIWH10Value = commandResponse.Substring(0, 8);
               string PhCaliFactor = commandResponse.Substring(16, 4);               

               string resultResponse = "MMIWH10Value = " + MMIWH10Value + ", PhCaliFactor = " + PhCaliFactor;
               if (MMIWH10Value == StaticVariables.MMIWH10Value_ZCE && PhCaliFactor == StaticVariables.PhCaliFactor_ZCE) return StaticVariables.ERRORPreFix + "Uncalibrated Meter >> " + resultResponse;
               else return resultResponse;

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string VerifyRfPayload(string refCOMPort,string pcbaID)
       {
           try
           {
               objIECLI.PortDisconnect();
               if(!objIECLI.PhysicalLayerConnectRFMeter("COM1"))return StaticVariables.ERRORPreFix + "Port Open Error";
                string[] CmdResponse = new string[] { "", "", "" };
                byte[] rfPayLoadCommand =  GetRFPayLoadCommand(pcbaID);
                List<byte> RecievRFPayload = objIECLI.WriteBytesToMeter(rfPayLoadCommand,16,5);
                if (CmdResponse.Length >= 2) return IsValidCalibrationData(CmdResponse);
                else return StaticVariables.ERRORPreFix + " Invalid Meter Response";                
               
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private byte[] GetRFPayLoadCommand(string payloadstring)
       {
           try
           {
               

               int payloadByteCnt = 0;
               byte[] RequestCommand = new byte[18];
               //SOF 4 Bytes 0x55 0xAA 0x55 0xAA 
               RequestCommand[payloadByteCnt++] = 0x55;
               RequestCommand[payloadByteCnt++] = 0xAA;
               RequestCommand[payloadByteCnt++] = 0x55;
               RequestCommand[payloadByteCnt++] = 0xAA;
               //Command ID 1 Bytes 0x03
               RequestCommand[payloadByteCnt++] = 0x03;
               //Data Sesuence Number (DSN) 2 Bytes 0x01 0x02
               RequestCommand[payloadByteCnt++] = 0x00;
               RequestCommand[payloadByteCnt++] = 0x01;
               //Pay Load Lenth ID 1 Bytes 
               string palyLoadCommand = payloadstring;

               RequestCommand[payloadByteCnt++] = (byte)(palyLoadCommand.Length + 1);
               RequestCommand[payloadByteCnt++] = (byte)(palyLoadCommand.Length);
               int loadlen = 0;
               System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
               byte[] bytesVal = enc.GetBytes(palyLoadCommand);

               while (loadlen < bytesVal.Length)
               {
                   RequestCommand[payloadByteCnt++] = bytesVal[loadlen];
                   loadlen += 1;
               }
               RequestCommand[payloadByteCnt++] = RFCRC(RequestCommand, payloadByteCnt);

               string commandResponse = string.Empty;

               return RequestCommand;

           }
           catch (Exception)
           {
               return null;
           }

       }

       public byte RFCRC(byte[] HHUPacket, int PacketLength)
       {
           int Iterator = 0;
           uint CRC = 0x00;
           uint ZeroCount = 0x00;
           CRC = 0;
           /* XOR entire packet except checksum filled*/
           for (Iterator = 0; Iterator < PacketLength - 1; Iterator++)
           {
               if (HHUPacket[Iterator] == 0x00)
                   ZeroCount++;

               CRC = CRC ^ HHUPacket[Iterator];
           }
           CRC = CRC ^ ZeroCount;
           //LOG_DEBUG("CALC[%x]\r\n", CRC);
           return (byte)CRC;
       }

       private string IsValidCalibrationData(string[] data)
       {
           try
           {
           string strResp = "";
           bool isFail = false;
           strResp = data[0].Replace("(", "");
           strResp = strResp.Replace(")", "");

           string txtPhaseP1WH =objcomnMethod.ReverseString(strResp.Substring(24, 8), 0);
           string txtNeutralP1WH = objcomnMethod.ReverseString(strResp.Substring(32, 8), 0);
           
           string txtNeutralPMagSlopeLow = objcomnMethod.GetData(objcomnMethod.ReverseString(strResp.Substring(44, 4), 1));
           string txtNeutralPMagSlopeHigh = objcomnMethod.ReverseString(strResp.Substring(48, 4), 0);
           

           strResp = data[1].Replace("(", "");
           strResp = strResp.Replace(")", "");

           string txtSWTPhaseCompFactor = objcomnMethod.GetData(objcomnMethod.ReverseString(strResp.Substring(28, 4), 1));
           string txtSWTNeutralCompFactor = objcomnMethod.GetData(objcomnMethod.ReverseString(strResp.Substring(32, 4), 1));

           if (txtPhaseP1WH == StaticVariables.PhaseP1WH_Eco && txtNeutralP1WH == StaticVariables.NeutralP1WH_Eco && txtNeutralPMagSlopeLow == StaticVariables.NeutralPMagSlopeLow_Eco && txtSWTPhaseCompFactor == StaticVariables.SWTPhaseCompFactor_Eco && txtSWTNeutralCompFactor == StaticVariables.SWTNeutralCompFactor_Eco) isFail = true;
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

       public string VerifyCalibrationData_MicroStar()
       {
           try
           {
               string[] CmdResponse = new string[] { "", "", "" };
               CmdResponse[0] = ReadDataFromMeter("313F(1E)", StaticVariables._R1, "\x03");
               if (CmdResponse[0].IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse[0];
               Thread.Sleep(300);
               CmdResponse[1] = ReadDataFromMeter("315D(1E)", StaticVariables._R1, "\x03");
               if (CmdResponse[1].IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse[1];
               if (CmdResponse.Length >= 2) return IsValidCalibrationData_MicroStar(CmdResponse);
               else return StaticVariables.ERRORPreFix + " Invalid Meter Response";

           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private string IsValidCalibrationData_MicroStar(string[] data)
       {
           try
           {
               string strResp = "";
               bool isFail = false;
               strResp = data[0].Replace("(", "");
               strResp = strResp.Replace(")", "");

               string txtPhaseP1WH = objcomnMethod.ReverseString(strResp.Substring(24, 8), 0);
               string txtNeutralP1WH = objcomnMethod.ReverseString(strResp.Substring(32, 8), 0);

               string txtPhase_P_1VARrH = Convert.ToInt32(objcomnMethod.ReverseString(strResp.Substring(44, 8), 1), 16).ToString();
               string txtNeutral_P_1VArH = objcomnMethod.ReverseString(strResp.Substring(52, 8), 0);


               strResp = data[1].Replace("(", "");
               strResp = strResp.Replace(")", "");

               
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

       private string ReadDataFromMeter(string ipCommand, string readCommandType,string responseStopByte)
       {
           try
           {
               Thread.Sleep(100);
               string CmdResponse = "";
               string Command = StaticVariables._SOH + readCommandType + StaticVariables._STX + objIECLI.GetStrToHexCmd(ipCommand) + StaticVariables._ETX;
               string bcc = objIECLI.GetCalculatedBCC(Command.Substring(2));
               Command += bcc;

               if (readCommandType == StaticVariables._ACK) CmdResponse = objIECLI.WriteDataToMeter(StaticVariables._ACK, responseStopByte);//"\x04"
               else CmdResponse = objIECLI.WriteDataToMeter(Command, responseStopByte);//"\x03"
               if (CmdResponse.Length < 5) { return StaticVariables.ERRORPreFix + "COMM Failed."; }

               const string regexReadbuffer = @"(\(([\w\W]*?)\))";
               MatchCollection matches = Regex.Matches(CmdResponse, regexReadbuffer, RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
               string[] Bufferdata = new string[matches.Count];
               int rcnt = 0;
               string opCommand = string.Empty;
               foreach (Match match in matches)
               {
                   GroupCollection groups = match.Groups;
                   Bufferdata[rcnt] = groups["0"].Value;
                   opCommand = opCommand + Bufferdata[rcnt];
                   rcnt++;
               }
               if (opCommand.Length <= 0) { return StaticVariables.ERRORPreFix + "In Valid Response :" + opCommand; }
               return opCommand;
            }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string WritePCBAID( string meterPCBAID)
       {
           meterPCBAID = meterPCBAID.PadRight(10, ' ');
           string CmdResponse = WriteDataToMeter("3033(" + objcomnMethod.ASCIIToHexForSerial(meterPCBAID) + ")", StaticVariables._W1);
           return CmdResponse;       
       }

       public string WritePCBAIDZCE( string meterPCBAID)
       {
           meterPCBAID = meterPCBAID.PadRight(10, ' ');
           string CmdResponse = WriteDataToMeter("108E(" + objcomnMethod.ASCIIToHexForSerial(meterPCBAID) + ")", StaticVariables._W1);
           return CmdResponse;       
       }

       public string WriteMeterID(string meterPCBAID)
       {
           meterPCBAID = meterPCBAID.PadRight(16, ' ');
           string CmdResponse = WriteDataToMeter("3057(" + objcomnMethod.ASCIIToHexForSerial(meterPCBAID) + ")", StaticVariables._W1);
           return CmdResponse;
       }

       public string WriteMeterIDZCE(string meterPCBAID)
       {
           meterPCBAID = meterPCBAID.PadRight(16, ' ');
           string CmdResponse = WriteDataToMeter("10BA(" + objcomnMethod.ASCIIToHexForSerial(meterPCBAID) + ")", StaticVariables._W1);
           return CmdResponse;
       }

       public string WriteManufacturingData(string defaultVal, string minVal, string maxVal)
       {
           string mfgData =objcomnMethod.GetManufacturingDataWriteCommand(defaultVal,  minVal,  maxVal, 1);
           string CmdResponse = WriteDataToMeter("31F7(" + mfgData + ")", StaticVariables._W1);
           return CmdResponse;
       }
      
       public string ResetingFrauds(string resetflg,string resettingAddress)
       {
           int iCommandTimeout = GlobalObjects.objIECSerialComm.InterchatracterDelay;
           try
           {
               if (resetflg == "FF") GlobalObjects.objIECSerialComm.InterchatracterDelay = 30000;//----All Data Resets
               string CmdResponse = WriteDataToMeter(resettingAddress + "(" + resetflg + ")", StaticVariables._W1);
               return CmdResponse;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
           finally
           {
               GlobalObjects.objIECSerialComm.InterchatracterDelay = iCommandTimeout;
           }
       }
       public string WritePmaxValue(string PmaxValue)
       {
           
           try
           {
               int pMax;
               if (!int.TryParse(PmaxValue, out pMax)) return StaticVariables.ERRORPreFix + "Pmax Value Should be Numeric Only !";
               if (pMax < 20 || pMax > 90) return StaticVariables.ERRORPreFix + "Pmax Value Should be 20-90";            

               string valueofpMax = pMax.ToString("X").PadRight(4,'0');
               string CmdResponse = WriteDataToMeter("3181" + "(" + valueofpMax + ")", StaticVariables._W1);
               return CmdResponse;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
           
       }
       private string WriteDataToMeter(string ipCommand, string writeCommandType)
       {
           string CmdResponse = "";
           try
           {
               Thread.Sleep(200);
               string Command = StaticVariables._SOH + writeCommandType + StaticVariables._STX + objIECLI.GetStrToHexCmd(ipCommand) + StaticVariables._ETX;
               string bcc = objIECLI.GetCalculatedBCC(Command.Substring(2));
               Command += bcc;
              
               CmdResponse = objIECLI.WriteDataToMeter(Command, "\x06");
               if (CmdResponse.Length < 1) { return StaticVariables.ERRORPreFix + "COMM Failed."; }
               else if (CmdResponse.IndexOf("\x06") >= 0) return "";
               else { return StaticVariables.ERRORPreFix + "Invalid Response =" + CmdResponse; }
               
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message + "Meter Response :" + CmdResponse;
           }
       }

       public string SetMeterRTC()
       {           
           try
           {
               string CmdResponse = WriteDataToMeter("C001(" + GetSystemRtc() + ")", StaticVariables._W2);               
               return CmdResponse;                             
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string SetMeterRTCZCE()
       {           
           try
           {
               string CmdResponse = WriteDataToMeter("C001(" + GetSystemRtcZCE() + ")", StaticVariables._W2);               
               return CmdResponse;                             
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }      

       public string CheckingCaseTamper(string ConfigFilePath,string ipVerifyflagdelay)
       {
           try
           {
               int verifyingFlagDelayCount;
               Thread.Sleep(100);
               string CmdResponse = ResetingFrauds("FF", "C161");
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;               
             
               //---------------Writing LDSM if in Configuration--------------------------
               string ldsmCommand = GetLDSMCommandFromFile(ConfigFilePath);              
               if (ldsmCommand.IndexOf(StaticVariables.ERRORPreFix) >= 0) return ldsmCommand;
               if (ldsmCommand.Length > 0)
               {
                   CmdResponse = WriteDataToMeter(ldsmCommand, StaticVariables._W1);
                   if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               }
                //--------------------Reading Case Flag-------------------------------------
               if(!int.TryParse(ipVerifyflagdelay,out verifyingFlagDelayCount)) verifyingFlagDelayCount = 1;
               Thread.Sleep(verifyingFlagDelayCount * 1000);
               return VerifyCaseTamper();
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string VerifyCaseTamper()
       {
           try
           {               
               string  CmdResponse = ReadDataFromMeter("E004(19)", StaticVariables._R1, "\x03");
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               if (CmdResponse.Length >= 9) return IsCaseDetect(CmdResponse);
               else return StaticVariables.ERRORPreFix + " Invalid Meter Response";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private string GetLDSMCommandFromFile(string ConfigFilePath)
       {
           try
           {
               string ldsmBytes = "";
               if (!File.Exists(ConfigFilePath)) { return StaticVariables.ERRORPreFix + "Invalid Config. File Path/File Not Exist"; }
               string strFileData = DLMSDataStracture.ReadUserFileData(ConfigFilePath);
               if (strFileData.IndexOf("0x331D") >= 0)
               {
                   ldsmBytes = strFileData.Substring(strFileData.IndexOf("0x331D"));
                   string[] paracmd = ldsmBytes.Split(',');
                   if (paracmd.Length > 1) ldsmBytes = paracmd[0].Substring(2) + "(" + paracmd[1] + ")";
               }             
               return ldsmBytes;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string CheckingCaseTamperZCE()
       {
           try
           {
               string CmdResponse = ResetingFrauds("FF", "C161");
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               Thread.Sleep(1000);
               CmdResponse = ReadDataFromMeter("000E()", StaticVariables._R2, "\x03");
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               if (CmdResponse.Length >= 4) return IsCaseDetectZCE(CmdResponse);
               else return StaticVariables.ERRORPreFix + " Invalid Meter Response";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private string IsCaseDetect(string CmdResponse)
       {
           try
           {
               //byte casestatus = (byte)Convert.ToInt16((CmdResponse.Substring(3, 2)), 16);
               string caseCounter =objcomnMethod.HexToDecimalConversion(CmdResponse.Substring(27, 4));
               if (Convert.ToInt16(caseCounter) == 0) return "";//----Nothing return in case of Pass for automation req. Case Counter =" + casestatus.ToString();
               return StaticVariables.ERRORPreFix + "Case Detected Counter=" + caseCounter.ToString(); 
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private string IsCaseDetectZCE(string CmdResponse)
       {
           try
           {
               if (CmdResponse.Substring(1, 2) == "00") return "";//----Nothing return in case of Pass for automation req. "Case Flag =" + CmdResponse.Substring(1, 2);
               else return StaticVariables.ERRORPreFix + "Case Detected =" + CmdResponse.Substring(1, 2);
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string CheckingBatteryStatus()
       {
           try
           {
               string CmdResponse = ReadDataFromMeter("E004(19)", StaticVariables._R1, "\x03");
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               if (CmdResponse.Length >= 9) return IsValidBatteryStatus(CmdResponse);
               else return StaticVariables.ERRORPreFix + " Invalid Meter Response";
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
       /// <param name="refminVal"></param>
       /// <param name="refmaxVal"></param>
       /// <returns></returns>

       public string CheckingMagnetTamperStatus(string defaultVal, string refminVal, string refmaxVal)
       {
           try
           {
               
               string validatedResponse = "";
               string acCountdefaultVal = "";
               string acCountrefminVal = "";
               string acCountrefmaxVal = "";
               string[] defValueList = defaultVal.Split(',');
               string[] minValueList = refminVal.Split(',');
               string[] maxValueList = refmaxVal.Split(',');
               string CmdResponse = ReadDataFromMeter("E002(14)", StaticVariables._R1, "\x03");
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               if (CmdResponse.Length >= 42)
               {
                    
                   string acCount = objcomnMethod.HexToDecimalConversion(CmdResponse.Substring(33, 4));
                   string dcCount = objcomnMethod.HexToDecimalConversion(CmdResponse.Substring(37, 4));
                 
                   //------------------AC Count Test----------------------------------------
                   if (defValueList.Length >= 1) acCountdefaultVal = defValueList[0];
                   if (minValueList.Length >= 1) acCountrefminVal = minValueList[0];
                   if (maxValueList.Length >= 1) acCountrefmaxVal = maxValueList[0];

                   if (objcomnMethod.isValidReadParameters(acCountdefaultVal, acCountrefminVal, acCountrefmaxVal, Convert.ToDecimal(acCount))) validatedResponse = " AC Count =" + acCount.ToString();
                   else validatedResponse = StaticVariables.ERRORPreFix + " AC Count =" + acCount.ToString();

                   //------------------------DC Count Test------------------------------------                   
                   if (defValueList.Length >= 2) acCountdefaultVal = defValueList[1];
                   if (minValueList.Length >= 2) acCountrefminVal = minValueList[1];
                   if (maxValueList.Length >= 2) acCountrefmaxVal = maxValueList[1];
                   if (objcomnMethod.isValidReadParameters(acCountdefaultVal, acCountrefminVal, acCountrefmaxVal, Convert.ToDecimal(dcCount))) validatedResponse += " ,DC Count =" + dcCount.ToString();
                   else validatedResponse += "," + StaticVariables.ERRORPreFix + " DC Count =" + dcCount.ToString();

                   return validatedResponse;
                   
               }
               else return StaticVariables.ERRORPreFix + " Invalid Response" + CmdResponse;
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
               string validatedResponse = "";
               string CmdResponse = ReadDataFromMeter("E002(14)", StaticVariables._R1, "\x03");
               if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
               if (CmdResponse.Length >= 42)
               {
                   
                   string acCount1 = objcomnMethod.HexToDecimalConversion(CmdResponse.Substring(37, 2));
                   string acCount2 = objcomnMethod.HexToDecimalConversion(CmdResponse.Substring(39, 2));
                   string dcCount = objcomnMethod.HexToDecimalConversion(CmdResponse.Substring(35, 2));

                   //------------------------AC Sensor 1 Flag Test------------------------------------         
                   if (Convert.ToDecimal(acCount1) > 0) validatedResponse = " AC HALL Sensor-1 Flag =" + acCount1.ToString();
                   else validatedResponse = StaticVariables.ERRORPreFix + " AC HALL Sensor-1 Flag =" + acCount1.ToString();

                   //------------------------AC Sensor 2 Flag Test------------------------------------         
                   if (Convert.ToDecimal(acCount2) > 0) validatedResponse += " AC HALL Sensor-2 Flag =" + acCount2.ToString();
                   else validatedResponse += "," + StaticVariables.ERRORPreFix + " AC HALL Sensor-2 Flag =" + acCount2.ToString();

                   //------------------------DC Flag Test------------------------------------                   

                   if (Convert.ToDecimal(dcCount) > 0) validatedResponse += " ,DC HALL Sensor Flag =" + dcCount.ToString();
                   else validatedResponse += "," + StaticVariables.ERRORPreFix + " DC HALL Sensor Flag =" + dcCount.ToString();

                   return validatedResponse;

               }
               else return StaticVariables.ERRORPreFix + " Invalid Response" + CmdResponse;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private string IsValidBatteryStatus(string CmdResponse)
       {
           try
           {
               string rtcStatus = "";
               string mainStatus = "";
               string batteryStatus = CmdResponse.Substring(5, 2);
               string Batterychk = Convert.ToString(Convert.ToInt32(batteryStatus, 16), 2);
               Batterychk = Batterychk.PadLeft(8, '0');
               if (Batterychk.Substring(0, 1) == "1") rtcStatus = StaticVariables.ERRORPreFix + "RTC Battery Failed =" + Batterychk.Substring(0, 1);
               else rtcStatus = "RTC Battery =" + Batterychk.Substring(0, 1);
               if (Batterychk.Substring(1, 1) == "1") mainStatus = StaticVariables.ERRORPreFix + "Main Battery Failed =" + Batterychk.Substring(1, 1);
               else mainStatus = "Main Battery =" + Batterychk.Substring(1, 1);
               return rtcStatus +  ", " + mainStatus;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message + " Meter Response =" + CmdResponse;
           }
       }
       
       public string LockingMeter(string lockByte)
       {
           try
           {
               string CmdResponse = WriteDataToMeter("0008(" + lockByte + ")", StaticVariables._W1);
               return CmdResponse;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string LockingMeterZCE(string lockByte)
       {
           try
           {
               string CmdResponse = WriteDataToMeter("0003(" + lockByte + ")", StaticVariables._W1);
               return CmdResponse;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }
       
       public string GetSystemRtc()
       {

           int weekdayindex = 0;
           string sysrtc = string.Empty;
           string strrtc = string.Empty;
           DateTime DTPdatetime = DateTime.Now.AddSeconds(2);
           /*Second*/
           strrtc = DTPdatetime.Second.ToString("00");// DateTime.Now.Second.ToString("00");
           sysrtc += strrtc;
           //sysrtc += ((Convert.ToInt16(strrtc.Substring(0, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(1, 1)) + 30).ToString());
           /*Minut*/
           strrtc = DTPdatetime.Minute.ToString("00");
           sysrtc += strrtc;
          // sysrtc += ((Convert.ToInt16(strrtc.Substring(0, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(1, 1)) + 30).ToString());
           /*Hour*/
           strrtc = DTPdatetime.Hour.ToString("00");
           sysrtc += strrtc;
          // sysrtc += ((Convert.ToInt16(strrtc.Substring(0, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(1, 1)) + 30).ToString());
           /*Day*/
           strrtc = DTPdatetime.Day.ToString("00");
           sysrtc += strrtc;
           //sysrtc += ((Convert.ToInt16(strrtc.Substring(0, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(1, 1)) + 30).ToString());
           /*Month*/
           strrtc = DTPdatetime.Month.ToString("00");
           sysrtc += strrtc;
          // sysrtc += ((Convert.ToInt16(strrtc.Substring(0, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(1, 1)) + 30).ToString());
           /*Year*/
           strrtc = DTPdatetime.Year.ToString("00");
           if (strrtc.Length >= 4) strrtc = strrtc.Substring(2, 2);
           sysrtc += strrtc;
           //sysrtc += ((Convert.ToInt16(strrtc.Substring(0, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(1, 1)) + 30).ToString());
           ///*Week Day*/
           string WeekDay = DTPdatetime.DayOfWeek.ToString();
           while (objstst.DayOfWeek[weekdayindex] != WeekDay) { weekdayindex++; }
           sysrtc += "0" + ((weekdayindex).ToString());
           return sysrtc;


       }

       public string GetSystemRtcZCE()
       {

           int weekdayindex = 0;
           string sysrtc = string.Empty;
           string strrtc = string.Empty;
           DateTime DTPdatetime = DateTime.Now.AddSeconds(2);
           
           strrtc = DTPdatetime.Year.ToString("00");
           if (strrtc.Length >= 4) strrtc = strrtc.Substring(2, 2);
           sysrtc += strrtc;
           /*Month*/
           strrtc = DTPdatetime.Month.ToString("00");
           sysrtc += strrtc;
           /*Day*/
           strrtc = DTPdatetime.Day.ToString("00");
           sysrtc += strrtc;
           /*Hour*/
           strrtc = DTPdatetime.Hour.ToString("00");
           sysrtc += strrtc;
           /*Minut*/
           strrtc = DTPdatetime.Minute.ToString("00");
           sysrtc += strrtc;
           /*Second*/
           strrtc = DTPdatetime.Second.ToString("00");// DateTime.Now.Second.ToString("00");
           sysrtc += strrtc;

           //sysrtc += ((Convert.ToInt16(strrtc.Substring(0, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(1, 1)) + 30).ToString());
           ///*Week Day*/
           string WeekDay = DTPdatetime.DayOfWeek.ToString();
           while (objstst.DayOfWeek[weekdayindex] != WeekDay) { weekdayindex++; }
           sysrtc += "00" + ((weekdayindex).ToString());
           return sysrtc;


       }

       private string FormatASCIIData(string buffer)
       {
           string dataVal = string.Empty;
           string asciival = string.Empty;
           int stractcount = 0;
           int lengthodstruct = buffer.Length;//length of stract
           while (stractcount < lengthodstruct)
           {
               dataVal = buffer.Substring(stractcount, 2);
               char idchar = ((char)(Convert.ToInt32((dataVal), 16)));
               if (idchar != '\0') asciival = asciival + idchar.ToString();
               else break;
               stractcount += 2; 
           }
           return asciival;           
       }

       //------------------------------Read & Verify  Meter Configuration File -----------------------------------------------------
       public string VerifyDisplayConfigData(string srcFileName)
       {
           try
           {                
               string resStopByte = "";
               if (!File.Exists(srcFileName)) { return StaticVariables.ERRORPreFix + "Invalid File Path/File Not Exist"; }
               string strFileData = DLMSDataStracture.ReadUserFileData(srcFileName);
               string[] dataPacket = strFileData.Split('\n');
               if (dataPacket.Length <= 1)
               {
                   return StaticVariables.ERRORPreFix + "Invalid/ Blank File";
               }
               if (dataPacket.Length >= 1 && dataPacket[0].Length <= 0)
               {
                   return StaticVariables.ERRORPreFix + "Invalid/ Blank File";
               }
               string fileHeder = dataPacket[0];
               fileHeder = fileHeder.Replace(" ", "");
               if (fileHeder.IndexOf("NVMapCode=0001") < 0)
               {
                   return StaticVariables.ERRORPreFix + "Invalid/ Blank File";
               }
               int paraCount = 2;

               while (paraCount < dataPacket.Length - 1)
               {
                   string paraLabel = dataPacket[paraCount].ToString();
                   if (paraLabel.IndexOf("SOFTWARE LOCKS") >= 0) break;
                   if (paraLabel.IndexOf("\"") >= 0) paraLabel = paraLabel.Substring(paraLabel.IndexOf("\""));
                   string[] fileCommandData =  dataPacket[paraCount++].ToString().Split(',');
                   if (fileCommandData.Length <= 2) break;
                   if (fileCommandData[0].IndexOf("D4") >= 0 || fileCommandData[0].Length <= 0) resStopByte = "\x04";  //--For TOU Table Readout                   
                   else resStopByte = "\x03";
                   if (dataPacket[paraCount].IndexOf("D4") >= 0) resStopByte = "\x03";                               //--For TOU Table Readout
                  
                   string getParafromMeter = ReadConfiguration(fileCommandData, resStopByte);
                   if (getParafromMeter.Length == StaticVariables.ERRORPreFix.Length)    return StaticVariables.ERRORPreFix + paraLabel;
                   else if (getParafromMeter.IndexOf(StaticVariables.ERRORPreFix) >= 0) return getParafromMeter + " " + paraLabel;
                   Thread.Sleep(50);
               }
               return "";
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       private string ReadConfiguration(string[] FileData,string resStopByte)
       {
           try
           {
               string CmdResponse = "";
               string addresscmd = "";
               if (FileData.Length < 2) return StaticVariables.ERRORPreFix;
               
                   if(FileData[0].Length >= 2) addresscmd = FileData[0].Substring(2);
                   if (addresscmd.Length > 0)
                   {
                       string bytelentoRead = (FileData[1].Length / 2).ToString("X2");
                       if (addresscmd.IndexOf("D4") < 0) CmdResponse = ReadDataFromMeter(addresscmd + "(" + bytelentoRead + ")", StaticVariables._R1, resStopByte);
                       else CmdResponse = ReadDataFromMeter(addresscmd + "()", StaticVariables._R4, resStopByte);
                   }
                   else
                   {
                       CmdResponse = ReadDataFromMeter(addresscmd + "()", StaticVariables._ACK, resStopByte);
                   }
                   if (CmdResponse.IndexOf(StaticVariables.ERRORPreFix) >= 0) return CmdResponse;
                   if (CmdResponse.IndexOf("(") >= 0) CmdResponse = CmdResponse.Substring(CmdResponse.IndexOf("(")+1);
                   if (CmdResponse.IndexOf(")") >= 0) CmdResponse = CmdResponse.Substring(0,CmdResponse.IndexOf(")"));
                   if (FileData[1].ToString().IndexOf(CmdResponse) >= 0) return "";
                   return StaticVariables.ERRORPreFix + CmdResponse;
                
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

      //-------------------------------------End of Configuration Verification-------------------------------------------------------------








    }
}
