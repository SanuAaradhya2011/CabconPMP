using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using DLMSLIB;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Text.RegularExpressions;
using Utilities;
using System.Windows.Forms;
namespace ApplicationInterface
{
   
  public  class LayerInterface
    {
        int HDLCIndex = 0;
        public delegate void UpdateHandler(object sender, UpdateEventArgs e);
        public event UpdateHandler UpdatedLed;
        AppSettings objappsettings = new AppSettings();
        UpdateEventArgs args = null;
        public int errormsgStstus = 0;
        public string AESstr = "";
        byte[] HDLCCommand = new byte[1024];
        public int getWriteResponseCode = 0;
        public bool IsMeterConnected = false;
        public string AppDirectoryLocalPath = AppDomain.CurrentDomain.BaseDirectory + "\\Configuration";
        public string MeterInfoValue = "";
        public string currentMeterID;
        private string secureModeLLSPassword = string.Empty;
        private string secureModeHLSPassword = string.Empty;
        private string secureModeEncruptionKey = string.Empty;
        private string secureModeTempEncruptionKey = string.Empty;
        public static string MeterSignature = string.Empty;
        public string RSAPrivateKey
        {
            get;
            set;
        }

        public List<string> ListSecurityKeys
        {
            get;
            set;
        }

        #region Enums

        public enum ProgrammingCode
        {
            Success,
            Fail,
            AccessDenied,
            DataUnavailable,
            TimeOut,
            SignOnFailed,
            CosemConnectionFailed,
            MeterIDMismatch

        }
       
        public enum ApplicationContext : byte
        {
            ShortMode = 2,
            LogicalModeWithoutCiphering = 1,
            LogicalModeWithCiphering = 3
        }


        public enum MeterTypeInfo { Smart_Meter_1PH = 0, MicroStar_DLMS = 1, Smart_Meter_3PH = 2, DLMS_3PH = 3, SAPPHIRE = 4, DLMS_3PH_RUBY = 5, Non_DLMS_1PH = 6, SAPPHIRE_S2 = 7 };

        public enum DisplayProgrammingTypes
        {
            OneByte = 0,
            TwoByte = 1
        }
        #endregion

        public  List<string> GetMeterTypeList()
        {
            List<string> meterTypeList = new List<string>();
            meterTypeList.Add("1Phase-Smart Meter");
            meterTypeList.Add("1Phase -DLMS");
            meterTypeList.Add("3Phase-Smart Meter");
            //meterTypeList.Add("3Phase-DLMS-PUMA");
            //meterTypeList.Add("3Phase-Sapphire");
            //meterTypeList.Add("3Phase-RUBY");
            //meterTypeList.Add("1Phase-NON-DLMS");
            //meterTypeList.Add("3Phase-Sapphire-S2");
            return meterTypeList;
        }

        public  string GetSelectedMeterType()
        {
             AppSettings objappSettings = new AppSettings();
             List<string> meterTypelist=GetMeterTypeList();            
             return meterTypelist[objappSettings.GetMeterMode()];

        }
        public List<string> GetSelectedMeterTypeValue()
        {
            AppSettings objappSettings = new AppSettings();
            List<string> meterTypelist = GetMeterTypeList();
            List<string> meterTypeValue = new List<string>();
            switch (objappSettings.GetMeterMode())
            {
                case (int)MeterTypeInfo.Smart_Meter_1PH:
                    meterTypeValue.Add(meterTypelist[objappSettings.GetMeterMode()]);
                    break;
                case (int)MeterTypeInfo.MicroStar_DLMS:
                    meterTypeValue.Add(meterTypelist[objappSettings.GetMeterMode()]);
                    break;
                case (int)MeterTypeInfo.Smart_Meter_3PH:
                    meterTypeValue.Add(meterTypelist[objappSettings.GetMeterMode()]);
                    break;
                case (int)MeterTypeInfo.DLMS_3PH:
                    meterTypeValue.Add(meterTypelist[objappSettings.GetMeterMode()]);
                    break;
                case (int)MeterTypeInfo.SAPPHIRE:
                    meterTypeValue.Add(meterTypelist[objappSettings.GetMeterMode()]);
                    break;
                case (int)MeterTypeInfo.DLMS_3PH_RUBY:
                    meterTypeValue.Add(meterTypelist[objappSettings.GetMeterMode()]);
                    break;
                case (int)MeterTypeInfo.Non_DLMS_1PH:
                    meterTypeValue.Add(meterTypelist[objappSettings.GetMeterMode()]);
                    break;
                case (int)MeterTypeInfo.SAPPHIRE_S2:
                    meterTypeValue.Add(meterTypelist[objappSettings.GetMeterMode()]);
                    break;
                default:
                    break;
            }
            return meterTypeValue;

        }
        
        public void DisplayStatusMsg(string msgString,bool isError)
        {
            try
            {
                args = new UpdateEventArgs(msgString, isError);
                UpdatedLed(this, args);
            }
            catch (Exception)
            {
            }
        }

        public bool ConnectToMeter()
        {
            currentMeterID = string.Empty;
            if (SerialPortSettings.Default.ApplicationContext == (byte)ApplicationContext.LogicalModeWithCiphering)
            {
               
                if (!ReadAssociationForInvocationCounter()) return false;
                if (currentMeterID.Trim().Length <= 0) { DisplayStatusMsg("Invalid Meter ID Detected!", true); return false; }
                if (objappsettings.GetAssociationMode() == 0) objappsettings.SetCipheredSecurityResponse(secureModeLLSPassword,secureModeHLSPassword,secureModeEncruptionKey); //--Secure Mode Association --> 0, Used Define Mode Association-->1
            }
                IsMeterConnected = false;
                MeterInfoValue = string.Empty;
                AppSettings objappSettings = new AppSettings();
                DisplayStatusMsg("  Physical Layer Communication...", false);
                if (!PhysicalLayerConnect()) { DisplayStatusMsg("Physical Layer Connection Failed!", true); return false; }
                DisplayStatusMsg("HDLC Layer Communication...", false);
                if (!HDLCLayerConnect()) { DisplayStatusMsg("HDLC Layer Connection Failed/ Busy !", true); return false; }
                DisplayStatusMsg("Establishing Association...", false);
                IsMeterConnected = true;
                // MohsinRaza: Add Logic For Dual Key
                if (!AssociationStablish())
                {
                    if (secureModeTempEncruptionKey != null && secureModeTempEncruptionKey.Length > 1)
                    {
                        objappsettings.SetCipheredSecurityResponse(secureModeLLSPassword, secureModeHLSPassword, secureModeTempEncruptionKey);
                        if (!PhysicalLayerConnect()) { DisplayStatusMsg("Physical Layer Connection Failed!", true); return false; }
                        if (!HDLCLayerConnect()) { DisplayStatusMsg("HDLC Layer Connection Failed/ Busy !", true); return false; }
                        if (!AssociationStablish())
                        {
                            DisplayStatusMsg("Unable To Establish Association!", true); return false;
                        }

                        secureModeEncruptionKey = secureModeTempEncruptionKey;
                    }
                    else
                    {
                        DisplayStatusMsg("Unable To Establish Association!", true); return false;
                    }
                }
                if (GlobalObjects.objCOSEMLIB.DedKeystr != "")//Dedicated
                {
                    GlobalObjects.objHDLCLIB.InitializationCounter = 0;
                }
                DisplayStatusMsg("Checking Meter Type Info...", false);
                string ClientSAP = Convert.ToInt32(objappSettings.GetClientSAP(), 10).ToString("X");
                if (ClientSAP != "10") if (!ValidMeterTypeInfo()) { DisplayStatusMsg("Invalid Meter Detected, Check Meter Variant !", true); return false; }//--If Not is PC Mode
                DisplayStatusMsg("Data Transferring Please Wait...", false);
                return true;
              }

        public bool ReadAssociationForInvocationCounter()
        {
            byte oldSecurityMechanism = objappsettings.GetSecurityMachanism();
            string oldClientSAP = objappsettings.GetClientSAP();
            byte oldAppctx = objappsettings.GetApplicationContext();
            try
            {
                IsMeterConnected = false;
                MeterInfoValue = string.Empty;
               
                //********In cyphering mode switch to PC mode for find invocation counter ***********
                objappsettings.SetSecurityMachanism(0x00);//---PC Mode
                objappsettings.SetClientSAP(0x10);       //---HLS for PC Mode 
                objappsettings.SetApplicationContext(0x01);       //---Set Application Context logical name wo ciphering
                DisplayStatusMsg("Physical Layer Communication...", false);
                if (!PhysicalLayerConnect()) { DisplayStatusMsg("Physical Layer Connection Failed!", true); return false; }
                DisplayStatusMsg("HDLC Layer Communication...", false);
                if (!HDLCLayerConnect()) { DisplayStatusMsg("HDLC Layer Connection Failed/ Busy !", true); return false; }
                DisplayStatusMsg("Establishing Association...", false);
                IsMeterConnected = true;
                GlobalObjects.objGlobalFunctions.fSendAARQ(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP, SerialPortSettings.Default.SecurityMechanism, SerialPortSettings.Default.Password, SerialPortSettings.Default.HLSKey, SerialPortSettings.Default.HLSPWD, SerialPortSettings.Default.ConformanceBlock, SerialPortSettings.Default.PDUSize, SerialPortSettings.Default.ApplicationContext);
                int writeResponse = 0;
                byte ClsCode = 0x01;
                byte AttCode = 0x02;
                //---------------------------Read Meter ID in PC Mode-----------------------------------------------------
                writeResponse = ReadDataCommand(DLMSDataStracture.MeterIDDataStracture.MeterIDOBIS, ClsCode, AttCode);
                string[] datavalue = DLMSDataStracture.DLMSDataFormator(GlobalObjects.objSerialComm.ReceiveBuffer, 18, true);
                if (datavalue != null) currentMeterID = datavalue[0];
                string SelectedClientType = oldClientSAP;

                if (objappsettings.GetAssociationMode() == 0) //---If Association Mode is Secure, This is for HHU & CC Security File
                {
                    List<string> securityKeyValues = null;
                    
                    if (RSAPrivateKey.Length < 1)
                    {
                        securityKeyValues = ServiceClass.ServiceInstance.GetSecurityKeys(objappsettings.GetScaleXMLPath(), currentMeterID);
                    }
                    else
                    {
                        securityKeyValues = ServiceClass.ServiceInstance.GetSecurityKeys(AppDomain.CurrentDomain.BaseDirectory + "EndDeviceSecurityResponse.xml", currentMeterID, RSAPrivateKey);

                        
                    }

                    ListSecurityKeys = securityKeyValues;

                    if (securityKeyValues != null && securityKeyValues.Count >= 1)
                    {
                        if (securityKeyValues[1].Length >= 16) 
                        { 
                            secureModeHLSPassword = securityKeyValues[1]; SelectedClientType = "48";
                        }
                        else
                        {
                            secureModeLLSPassword = securityKeyValues[1]; SelectedClientType = "32"; 
                        }
                        secureModeEncruptionKey = securityKeyValues[2];
                        // MohsinRaza: Handle Dual Global key Logic for CC
                        if(securityKeyValues.Count > 3 && securityKeyValues[3] != null)
                            secureModeTempEncruptionKey = securityKeyValues[3];
                    }
                }
                //--------------------------------------------------------------------------------------------------------
                byte[] InvoCounterOBIS = new byte[] { 0x00, 0x00, 0x2B, 0x01, 0x00, 0xFF };
                if (SelectedClientType == "32")//--MR Mode
                    InvoCounterOBIS = new byte[] { 0x00, 0x00, 0x2B, 0x01, 0x02, 0xFF };
                else if (SelectedClientType == "48")//--US Mode
                    InvoCounterOBIS = new byte[] { 0x00, 0x00, 0x2B, 0x01, 0x03, 0xFF };
                else if (SelectedClientType == "80")//---FU mode - Firmware Upgrade
                    InvoCounterOBIS = new byte[] { 0x00, 0x00, 0x2B, 0x01, 0x05, 0xFF };
                writeResponse = ReadDataCommand(InvoCounterOBIS, ClsCode, AttCode);
                long InvoCountValue = 0;
                 datavalue = new string[2];
                datavalue = DLMSDataStracture.DLMSDataFormator(GlobalObjects.objSerialComm.ReceiveBuffer, 18, false);
                if (datavalue != null) InvoCountValue = Convert.ToInt64(datavalue[0]);
                GlobalObjects.objHDLCLIB.InitializationCounter = InvoCountValue + 1;
               
                AssociationDisconnect();
                
                return true;
            }
            catch (Exception ex)
            {
               return false;
            }
            finally
            {
                //********Restore previous setting of Cyphering MR/US mode **************

                objappsettings.SetSecurityMachanism(oldSecurityMechanism);//--- MR/US Mode
                objappsettings.SetClientSAP(Convert.ToInt16(oldClientSAP));      //--- MR/US Mode 
                objappsettings.SetApplicationContext(oldAppctx);      //--- MR/US Mode 
            }

        }

        //private bool SetCipheredAssociation()
        //{
        //    try
        //    {
               
        //        List<string> securityKeyValues = ServiceClass.ServiceInstance.GetSecurityKeys(currentMeterID);
        //        if (securityKeyValues != null)
        //        {
        //            objappsettings.SetCipheredSecurityResponse(securityKeyValues);
        //        }
        //        return true;
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
        //}

        public bool PhysicalLayerConnect()
        {
            try
            {
               
               // GlobalObjects.objSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, SerialPortSettings.Default.CommandBaudRate, SerialPortSettings.Default.Parity, SerialPortSettings.Default.DataBits, SerialPortSettings.Default.StopBits, SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                GlobalObjects.objSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, SerialPortSettings.Default.CommandBaudRate, "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                if (GlobalObjects.objSerialComm.OpenPort()) return true;
                else return false;
                 
            }
            catch (Exception)
            {
               return false;
            }

        }

        public bool HDLCLayerConnect()
        {
            try
            {
               
                // if (GlobalObjects.objGlobalFunctions.fSendSNRM(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP))return true;
                 if (GlobalObjects.objGlobalFunctions.fSendSNRM(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP, SerialPortSettings.Default.CosemBufferSize, SerialPortSettings.Default.CosemBufferSize, SerialPortSettings.Default.WindowSize, SerialPortSettings.Default.WindowSize))
                {
                //    byte[] destination = new byte[hdlcIndex];
                //    Array.Copy(hdlcBuffer, destination, hdlcIndex);

                //    string hexString = BitConverter.ToString(destination).Replace("-", " ");
                //    string filePath = "output.txt";

                //    File.AppendAllText(filePath, "Send " + DateTime.Now.ToShortDateString() + ":->" + hexString + "\n");
                //    result = Serial.Send(hdlcBuffer, hdlcIndex);
                //    hexString = BitConverter.ToString(result.RecieveDataBuffer.ToArray()).Replace("-", " ");
                //    File.AppendAllText(filePath, "Receive " + DateTime.Now.ToShortTimeString() + ":->" + hexString + "\n");
                    return true;
                }

                else
                {
                  return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool AssociationStablish()
        {
            try
            {
              //  if (SerialPortSettings.Default.AESEncryption == "Cyphering")
                if (SerialPortSettings.Default.ApplicationContext == (byte)ApplicationContext.LogicalModeWithCiphering)
                {
                    //AESstr = SerialPortSettings.Default.AESEncryption;
                    AESstr = "Cyphering";
                    if (!GlobalObjects.objGlobalFunctions.fSendAARQ_Cyphered(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP, SerialPortSettings.Default.SecurityMechanism, SerialPortSettings.Default.Password, SerialPortSettings.Default.HLSKey, SerialPortSettings.Default.HLSPWD, SerialPortSettings.Default.ConformanceBlock, SerialPortSettings.Default.PDUSize, SerialPortSettings.Default.ClientSystemTitle, SerialPortSettings.Default.Securitysuit, SerialPortSettings.Default.GlobalEncryptionKey, SerialPortSettings.Default.AuthenticationKey, SerialPortSettings.Default.DedicatedKey)) return false;
                   return true;
                  
                   
                }
                else
                { 
                 if (GlobalObjects.objGlobalFunctions.fSendAARQ(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP, SerialPortSettings.Default.SecurityMechanism, SerialPortSettings.Default.Password, SerialPortSettings.Default.HLSKey, SerialPortSettings.Default.HLSPWD, SerialPortSettings.Default.ConformanceBlock, SerialPortSettings.Default.PDUSize, SerialPortSettings.Default.ApplicationContext)) return true;
                 else return false;
                
                }
                return false;
              
                
                
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ValidMeterTypeInfo()
        {
            try
            {
                if (SerialPortSettings.Default.MeterMode == (int)MeterTypeInfo.DLMS_3PH_RUBY) return true;//---Meter Info Command Not Supported in Ruby
                byte[] infoOBIS = DLMSDataStracture.ReadMeterInfoDataStracture.ReadMeterInfoOBIS_3PHDLMS;
                byte infoattribute=DLMSDataStracture.ReadMeterInfoDataStracture.ReadMeterInfoValueAttribute;
                byte infoclassID = DLMSDataStracture.ReadMeterInfoDataStracture.ReadMeterInfoClassID ;
                if (SerialPortSettings.Default.MeterMode == (int)MeterTypeInfo.Smart_Meter_1PH || SerialPortSettings.Default.MeterMode == (int)MeterTypeInfo.Smart_Meter_3PH || SerialPortSettings.Default.MeterMode == (int)MeterTypeInfo.SAPPHIRE_S2)
                {
                    infoOBIS = DLMSDataStracture.BuildVersionDataStracture.BuildVersionOBIS;
                    infoattribute = DLMSDataStracture.BuildVersionDataStracture.BuildVersionValueAttribute;
                    infoclassID = DLMSDataStracture.BuildVersionDataStracture.BuildVersionClassID;
                }
                //if (SerialPortSettings.Default.MeterMode == (int)MeterTypeInfo.Smart_Meter_1PH ) infoOBIS = DLMSDataStracture.ReadMeterInfoDataStracture.ReadMeterInfoOBIS;
                if (!ReadByteFromMeter(infoOBIS, null, "0", 1M,infoclassID ,infoattribute ))
                {
                    if (getWriteResponseCode == (int)ProgrammingCode.AccessDenied) return true;
                    return false;
                }
                if (IsValisMeterTypeInfo(GlobalObjects.objSerialComm.ReceiveBuffer, null, "0", 1M, SerialPortSettings.Default.MeterMode)) return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool IsValisMeterTypeInfo(byte[] receivedData, TextBox[] txtboxobject, string displayFormat, decimal emf, int MeterTypeinfo)
        {
            try
            {
                int startDataindx = 18;
                int startIDX = 15;
                int EndIDX = 2;
                string infodata = string.Empty;
                if (SerialPortSettings.Default.MeterMode == (int)MeterTypeInfo.Smart_Meter_1PH || SerialPortSettings.Default.MeterMode == (int)MeterTypeInfo.Smart_Meter_3PH || SerialPortSettings.Default.MeterMode == (int)MeterTypeInfo.SAPPHIRE_S2) { startIDX = 4; EndIDX = 6; }
                else if (receivedData[startDataindx + 1] >= 0x3F) { startIDX = 17; EndIDX = 2; } //--------For Meters like Net Metering having 64 Byte Signature Info
                else if (receivedData[startDataindx + 1] >= 0x1E) { startIDX = 16; EndIDX = 2; } //--------For Meters like HTCT Variants with voltage 63.5V having 30 Byte Signature Info
                if (receivedData[startDataindx] == 0x09 || receivedData[startDataindx] == 0x0A)
                {
                   string[] dataval = DLMSDataStracture.DLMSDataFormator(receivedData, startDataindx, true);
                   if (dataval.Length >= 1) { MeterSignature = MeterInfoValue = dataval[0]; infodata = dataval[0]; }
                }
                else { return true; }//---Ruby Old Meters No Meter Info
                if (infodata.Trim().Length < startIDX + EndIDX) return false;
                else
                {                   
                    string mtinfo = infodata.Substring(startIDX, EndIDX).ToUpperInvariant();
                    Dictionary<string, int> configPara = GetMeterTypeCode();
                    int[] keysByValue = configPara.Where(x => x.Key == mtinfo).Select(pair => pair.Value).ToArray();
                    if (!keysByValue.Contains(MeterTypeinfo))
                    {
                        //if (mtinfo == "WB" & MeterTypeinfo == 4) return true; //---If meter Type is Sapphire and Meter model is "WB" then PASS, we can't define "WB" for sapphire as already defined for Ruby so check required here to pass.
                        //if (mtinfo == "TN" & MeterTypeinfo == 3) return true; //---Special case for TNEB TN defined for Saphhire & Ruby
                        if (mtinfo == "CC" & MeterTypeinfo == 1) return true;
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetDisplayProgrammingVariant()
        {
            try
            {
                if (SerialPortSettings.Default.MeterMode == (int)MeterTypeInfo.DLMS_3PH_RUBY) return (int)DisplayProgrammingTypes.OneByte;//---Meter Info Command Not Supported in Ruby
                byte[] infoOBIS = DLMSDataStracture.ReadMeterInfoDataStracture.ReadMeterInfoOBIS_3PHDLMS;
                byte infoattribute = DLMSDataStracture.ReadMeterInfoDataStracture.ReadMeterInfoValueAttribute;
                byte infoclassID = DLMSDataStracture.ReadMeterInfoDataStracture.ReadMeterInfoClassID;
                if (SerialPortSettings.Default.MeterMode == (int)MeterTypeInfo.Smart_Meter_1PH || SerialPortSettings.Default.MeterMode == (int)MeterTypeInfo.Smart_Meter_3PH)
                {
                    infoOBIS = DLMSDataStracture.BuildVersionDataStracture.BuildVersionOBIS;
                    infoattribute = DLMSDataStracture.BuildVersionDataStracture.BuildVersionValueAttribute;
                    infoclassID = DLMSDataStracture.BuildVersionDataStracture.BuildVersionClassID;
                }
                if (SerialPortSettings.Default.MeterMode == (int)MeterTypeInfo.SAPPHIRE_S2) return (int)DisplayProgrammingTypes.TwoByte;//Default
                if (ReadByteFromMeter(infoOBIS, null, "0", 1M, infoclassID, infoattribute))
                {
                    return GetDisplayProgrammingVariant(GlobalObjects.objSerialComm.ReceiveBuffer, null, "0", 1M, SerialPortSettings.Default.MeterMode);
                }
                else
                {
                    return (int)DisplayProgrammingTypes.OneByte;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int GetDisplayProgrammingVariant(byte[] receivedData, TextBox[] txtboxobject, string displayFormat, decimal emf, int MeterTypeinfo)
        {
            try
            {
                int startDataindx = 18;
                int startIDX = 23;
                int EndIDX = 1;
                string infodata = string.Empty;
                if (SerialPortSettings.Default.MeterMode == (int)MeterTypeInfo.Smart_Meter_1PH || SerialPortSettings.Default.MeterMode == (int)MeterTypeInfo.Smart_Meter_3PH) { return (int)DisplayProgrammingTypes.OneByte; }
                else if (receivedData[startDataindx + 1] >= 0x3F) { startIDX = 25; EndIDX = 1; } //--------For Meters like Net Metering having 64 Byte Signature Info
                else if (receivedData[startDataindx + 1] >= 0x1E) { startIDX = 24; EndIDX = 1; } //--------For Meters like HTCT Variants with voltage 63.5V having 30 Byte Signature Info
                if (receivedData[startDataindx] == 0x09 || receivedData[startDataindx] == 0x0A)
                {
                    string[] dataval = DLMSDataStracture.DLMSDataFormator(receivedData, startDataindx, true);
                    if (dataval.Length >= 1) { MeterInfoValue = dataval[0]; infodata = dataval[0]; }
                }
                else { return (int)DisplayProgrammingTypes.OneByte; }//---Ruby Old Meters No Meter Info
                if (infodata.Trim().Length < startIDX + EndIDX) return (int)DisplayProgrammingTypes.OneByte;
                else
                {
                    string dispVariant = infodata.Substring(startIDX, EndIDX);
                    return (dispVariant == "2") ? (int)DisplayProgrammingTypes.TwoByte : (int)DisplayProgrammingTypes.OneByte;
                }
            }
            catch (Exception)
            {
                return (int)DisplayProgrammingTypes.OneByte;
            }
        }  

        public static Dictionary<string, int> GetMeterTypeCode()
        {
            Dictionary<string, int> dictionaryPara = new Dictionary<string, int>();

            dictionaryPara.Add("HM", (int)MeterTypeInfo.DLMS_3PH);
            dictionaryPara.Add("HK", (int)MeterTypeInfo.DLMS_3PH);
            dictionaryPara.Add("LT", (int)MeterTypeInfo.DLMS_3PH);
            dictionaryPara.Add("WC", (int)MeterTypeInfo.DLMS_3PH);
            dictionaryPara.Add("LC", (int)MeterTypeInfo.DLMS_3PH);
            dictionaryPara.Add("HC", (int)MeterTypeInfo.DLMS_3PH);
            dictionaryPara.Add("UK", (int)MeterTypeInfo.DLMS_3PH);
            dictionaryPara.Add("WB", (int)MeterTypeInfo.DLMS_3PH);
            dictionaryPara.Add("BW", (int)MeterTypeInfo.DLMS_3PH);
            dictionaryPara.Add("uk", (int)MeterTypeInfo.DLMS_3PH);
            dictionaryPara.Add("Ht", (int)MeterTypeInfo.DLMS_3PH);
            dictionaryPara.Add("SC", (int)MeterTypeInfo.SAPPHIRE);              //---WCM
            dictionaryPara.Add("ST", (int)MeterTypeInfo.SAPPHIRE);              //---LTCT
            dictionaryPara.Add("W0", (int)MeterTypeInfo.SAPPHIRE);              //---WCM : IS15959 Amendment 5 changes (Value is 'W' & '0'(Zero))
            dictionaryPara.Add("L0", (int)MeterTypeInfo.SAPPHIRE);              //---LTCT: IS15959 Amendment 5 changes (Value is 'L' & '0'(Zero))
            dictionaryPara.Add("SM", (int)MeterTypeInfo.SAPPHIRE);              //---HTCT Mega Variant
            dictionaryPara.Add("SH", (int)MeterTypeInfo.SAPPHIRE);              //---HTCT Kilo Variant
            dictionaryPara.Add("sm", (int)MeterTypeInfo.SAPPHIRE);              //---HTCT Mega Variant with 2 TOU
            dictionaryPara.Add("sh", (int)MeterTypeInfo.SAPPHIRE);              //---HTCT Kilo Variant with 2 TOU
            dictionaryPara.Add("TN", (int)MeterTypeInfo.SAPPHIRE);              //---WCM-TNEB customer specific     
            dictionaryPara.Add("LGC110", (int)MeterTypeInfo.MicroStar_DLMS);    //---MicroStar DLMS
            dictionaryPara.Add("SK", (int)MeterTypeInfo.MicroStar_DLMS);        //---VIM 128K DLMS
            dictionaryPara.Add("SF", (int)MeterTypeInfo.MicroStar_DLMS);        //---VIM 128K DLMS with FD Mode Readouts 
            dictionaryPara.Add("VB", (int)MeterTypeInfo.MicroStar_DLMS);        //---VIM 64K 1P VIM DLMS
            dictionaryPara.Add("VF", (int)MeterTypeInfo.MicroStar_DLMS);        //---VIM 64K 1P VIM DLMS with FD Mode Readouts
            dictionaryPara.Add("SM_110", (int)MeterTypeInfo.Smart_Meter_1PH);   //---with Specefic meter info command
            dictionaryPara.Add("FS", (int)MeterTypeInfo.Smart_Meter_1PH);       //---Same as above smart Meter 1Phase with generic meter info command
            dictionaryPara.Add("SM_310", (int)MeterTypeInfo.Smart_Meter_3PH);   //---WCM---with Specefic meter info command
            dictionaryPara.Add("SM_405", (int)MeterTypeInfo.Smart_Meter_3PH);   //---LTCT---with Specefic meter info command
            dictionaryPara.Add("SM0110", (int)MeterTypeInfo.Smart_Meter_1PH);   //---Falcon2-1PSM---with Specefic Build Version command
            dictionaryPara.Add("SM0405", (int)MeterTypeInfo.Smart_Meter_3PH);   //---Falcon2-LTCT---with Specefic meter info command
            dictionaryPara.Add("SM0310", (int)MeterTypeInfo.Smart_Meter_3PH);   //---Falcon2-WCM---with Specefic meter info command
            dictionaryPara.Add("FU", (int)MeterTypeInfo.Smart_Meter_3PH);       //---WCM ---Same as above smart Meter 3Phase with generic meter info command
            dictionaryPara.Add("FL", (int)MeterTypeInfo.Smart_Meter_3PH);       //---LTCT---Same as above smart Meter 3Phase with generic meter info command
            dictionaryPara.Add("FH", (int)MeterTypeInfo.Smart_Meter_3PH);       //---HTCT---Same as above smart Meter 3Phase with generic meter info command
            dictionaryPara.Add("BF", (int)MeterTypeInfo.MicroStar_DLMS);        //---VIM 128K DLMS with FD Mode Readouts with 7 slot TOU
            dictionaryPara.Add("BK", (int)MeterTypeInfo.MicroStar_DLMS);        //---VIM 128K DLMS with 7 slot TOU
            dictionaryPara.Add("CF", (int)MeterTypeInfo.MicroStar_DLMS);        //---VIM 64K DLMS with 7 slot TOU & new FD-Data Compression implementation
            dictionaryPara.Add("RF", (int)MeterTypeInfo.MicroStar_DLMS);        //---VIM 128K DLMS with 7 slot TOU with FD LS Resolution change
            dictionaryPara.Add("CB", (int)MeterTypeInfo.MicroStar_DLMS);        //---VIM 64K DLMS with 7 slot TOU and without FD
            dictionaryPara.Add("SPS201", (int)MeterTypeInfo.SAPPHIRE_S2);        //---Product Name for Sapphire S2 Optima : not in supply only in tender
            dictionaryPara.Add("SPS202", (int)MeterTypeInfo.SAPPHIRE_S2);        //---Product Name for Sapphire S2 Optima : IS15959 Amendment 5 changes
            return dictionaryPara;

        }

        public bool GetHTMeterTyps()
        {
            AppSettings objaps = new AppSettings();
            if (objaps.GetMeterMode() != (int)LayerInterface.MeterTypeInfo.Smart_Meter_3PH)
            {
                if (MeterInfoValue.Contains("SM") || MeterInfoValue.Contains("FH")) return true;
            }
            return false;
        }

        public bool AssociationDisconnect()
        {
            try
            {
                if (IsMeterConnected == false) return true;
                if (!GlobalObjects.objGlobalFunctions.fSendDISC(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP))
                { DisplayStatusMsg("Unable To Close Current Association!", true); return false; }
                else return true;               
                
            }
            catch (Exception)
            {
                DisplayStatusMsg("Unable To Close Current Association!", true); 
                return false; 
                
            }
            finally
            {
                PhysicalLayerDisconnect();
            }
         }

        public void PhysicalLayerDisconnect()
        {
            try
            {
                GlobalObjects.objSerialComm.ClosePort();
                return;
            }
            catch (Exception)
            {
                 return;
            }
        }      

        public bool fCheckHDLCResponse(byte[] Buffer)
        {
            try 
            {
                if (!GlobalObjects.objHDLCLIB.fCheckStartEndTag(Buffer)) { DisplayStatusMsg("   Invalid Start or end Tag", false); return false; }
                if (!GlobalObjects.objHDLCLIB.fCheckFCS(Buffer)) { DisplayStatusMsg("  Invalid HDLC FCS", false); return false; }
                if (!GlobalObjects.objHDLCLIB.fCheckServerSAP(Buffer, SerialPortSettings.Default.ClientSAP)) { DisplayStatusMsg("  Invalid Destination Address", false); return false; }
                if (!GlobalObjects.objHDLCLIB.fCheckCommand(Buffer, GlobalObjects.objHDLCLIB.nCMDByte)) { DisplayStatusMsg("  Invalid Response Byte", false); return false; }
                return true ;                       
            }
            catch (Exception)
            {
                DisplayStatusMsg("   Invalid Data", false);
                return false;
            }  
        }       

        public bool GenerateXML(int ScalarCommandType,DataGridView dtViewControl)
        {
            try
            {
                string datasetName = string.Empty;
                string XMLFileName = string.Empty;

                if (ScalarCommandType == 0)
                {
                    datasetName = "InstantScalar";
                    XMLFileName = @"\TempInstantScalarProfile.xml";
                }
                else if (ScalarCommandType == 1)
                {
                    datasetName = "BillingScalar";
                    XMLFileName = @"\TempBillingScalarProfile.xml";
                }
                else if (ScalarCommandType == 2)
                {
                    datasetName = "LoadSurveyScalar";
                    XMLFileName = @"\TempLoadSurveyScalarProfile.xml";
                }
                else if (ScalarCommandType == 3)
                {
                    datasetName = "TamperScalar";
                    XMLFileName = @"\TempTamperScalarProfile.xml";
                }
                else if (ScalarCommandType == 4)
                {
                    datasetName = "DailySurveyScalar";
                    XMLFileName = @"\TempDailySurveyScalarProfile.xml";
                }

                DataSet ds = new DataSet(datasetName);
                DataTable dt = new DataTable("DLMS");
                dt.Columns.Add("Class");
                dt.Columns.Add("ObisCode");
                dt.Columns.Add("Attribute");
                dt.Columns.Add("Scale");
                dt.Columns.Add("Unit");

                ds.Tables.Add(dt);
                DataRow row;

                foreach (DataGridViewRow rowDGV in dtViewControl.Rows)
                {
                    row = dt.NewRow();
                    row["Class"] = rowDGV.Cells["colClass"].Value;
                    row["ObisCode"] = rowDGV.Cells["colObis"].Value;
                    row["Attribute"] = rowDGV.Cells["colAttribute"].Value;
                    row["Scale"] = rowDGV.Cells["colScale"].Value;
                    row["Unit"] = rowDGV.Cells["colUnit"].Value;
                    dt.Rows.Add(row);
                }               
                ds.Tables[0].WriteXml(SerialPortSettings.Default.ScaleXMLPath + XMLFileName, XmlWriteMode.IgnoreSchema);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }      
       
        public bool ReadByteFromMeter(byte[] ParameterOBIS, TextBox[] controllist, string displayFormat, decimal emf,byte classCode, byte AttCode)
        {
            int stractcount = 0;
            int writeResponse = 0;
            if (controllist != null)
            {
                while (stractcount < controllist.Length) controllist[stractcount++].Text = "";
            }
            if (SerialPortSettings.Default.ApplicationContext == (byte)ApplicationContext.LogicalModeWithCiphering)
            {
                writeResponse = ReadDataCommand_Cyphered(ParameterOBIS, classCode, AttCode);
            }
            else
            {
                writeResponse = ReadDataCommand(ParameterOBIS, classCode, AttCode);
            }

           
            getWriteResponseCode = writeResponse;
            if (writeResponse == (int)ProgrammingCode.Success) { /*DisplayStatusMsg("Reading Succesfull.", false);*/ return true; }
            else if (writeResponse == (int)ProgrammingCode.AccessDenied) { DisplayStatusMsg("Access Denied!", true);/* MessageBox.Show("Access Denied!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);*/ return false; }
            else if (writeResponse == (int)ProgrammingCode.DataUnavailable) { DisplayStatusMsg("Data Not Available!", true); MessageBox.Show("Data Not Available!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1); return false; }
            else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed) { DisplayStatusMsg("Cosem Connection Failed!", true); MessageBox.Show("Cosem Connection Failed!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); return false; }
            else { DisplayStatusMsg("Communication Failed!", true); return false; }
        }

        public bool ReadBlockFromMeter(byte[] ParameterOBIS, TextBox[] controllist, string displayFormat, decimal emf, byte classCode, byte AttCode, byte Access_Selector, List<byte> DescriptorByteList)
        {
           // Thread waitwindowTh = null;
           // waitwindowTh = new Thread(new ThreadStart(DoSplash));
           // waitwindowTh.Start();
            int stractcount = 0;
            int writeResponse = 0;
            while (stractcount < controllist.Length) controllist[stractcount++].Text = "";

            if (SerialPortSettings.Default.ApplicationContext == (byte)ApplicationContext.LogicalModeWithCiphering)
            {
                writeResponse = ReadDataBlockCommand_Cyphered(ParameterOBIS, classCode, AttCode, Access_Selector, DescriptorByteList);
                errormsgStstus = writeResponse;
            }
            else
            {
                writeResponse = ReadDataBlockCommand(ParameterOBIS, classCode, AttCode, Access_Selector, DescriptorByteList);
                errormsgStstus = writeResponse;
            }
                 
           // waitwindowTh.Abort();
            if (writeResponse == (int)ProgrammingCode.Success) { /*DisplayStatusMsg("Reading Succesfull.", false);*/ return true; }
            else if (writeResponse == (int)ProgrammingCode.AccessDenied)
            {
                DisplayStatusMsg("Access Denied/ Not Supported!", true); /*MessageBox.Show("Access Denied!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);*/ return false;
            }
            else if (writeResponse == (int)ProgrammingCode.DataUnavailable) { DisplayStatusMsg("Data Not Available!", true);/* MessageBox.Show("Data Not Available!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1); */ return false; }
            else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed) { DisplayStatusMsg("Cosem Connection Failed!", true); MessageBox.Show("Cosem Connection Failed!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); return false; }
            else { DisplayStatusMsg("Communication Failed!", true); return false; }
        }
          
      /// <summary>
      /// Only For splash screen
      /// </summary>
        public void DoSplash()
        {
            ApplicationInterface.Form_Waitwindow.frmsplash sp = new ApplicationInterface.Form_Waitwindow.frmsplash();
            sp.ShowDialog();
        }

        public int ReadAllTamperBlockFromMeter(byte[] ParameterOBIS, TextBox[] controllist, string displayFormat, decimal emf, byte classCode, byte AttCode, byte Access_Selector, List<byte> DescriptorByteList)
        {
            int stractcount = 0;
            while (stractcount < controllist.Length) controllist[stractcount++].Text = "";

            if (SerialPortSettings.Default.ApplicationContext == (byte)ApplicationContext.LogicalModeWithCiphering)
            {
                int writeResponse = ReadDataBlockCommand_Cyphered(ParameterOBIS, classCode, AttCode, Access_Selector, DescriptorByteList);
                return writeResponse;
            }
            else
            {
                int writeResponse = ReadDataBlockCommand(ParameterOBIS, classCode, AttCode, Access_Selector, DescriptorByteList);
                return writeResponse;
            }
          
        }
        
        public bool WriteDataToMeter(byte attributeID, byte[] ParameterOBIS, byte paraClassID, byte typeofStruct, byte lengthofStruct, List<byte> ParameterBytes, byte[] DataRequestType)
        {
            int writeResponse = WritParameterToMeter(ParameterBytes, attributeID, ParameterOBIS, paraClassID, typeofStruct, lengthofStruct, DataRequestType);
            getWriteResponseCode = writeResponse;
            if (writeResponse == (int)ProgrammingCode.Success) {/* DisplayStatusMsg("Parameter Written Successfully.", false); */return true; }
            else if (writeResponse == (int)ProgrammingCode.AccessDenied) { DisplayStatusMsg("Access Denied !", true); /*MessageBox.Show("Access Denied.Please Change The Mode!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); */return false; }
            else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed) { DisplayStatusMsg("Cosem Connection Failed!", true); /*MessageBox.Show("Cosem Connection Failed!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);*/ return false; }
            else { DisplayStatusMsg("Communication Failed!", true); return false; }
        }
           
        public bool WriteMethodToMeter(byte attributeID, byte[] ParameterOBIS, byte paraClassID, byte typeofStruct, byte lengthofStruct, List<byte> ParameterBytes, byte[] DataRequestType,byte AccessSelector)
        {
            int writeResponse = WritMethodParameterToMeter(ParameterBytes, attributeID, ParameterOBIS, paraClassID, typeofStruct, lengthofStruct, DataRequestType,AccessSelector);
            if (writeResponse == (int)ProgrammingCode.Success) { /* DisplayStatusMsg("Parameter Written Successfully.", false); */return true; }
            else if (writeResponse == (int)ProgrammingCode.AccessDenied) { DisplayStatusMsg("Access Denied !", true); /*MessageBox.Show("Access Denied.Please Change The Mode!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); */return false; }
            else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed) { DisplayStatusMsg("Cosem Connection Failed!", true); /*MessageBox.Show("Cosem Connection Failed!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);*/ return false; }
            else { DisplayStatusMsg("Communication Failed!", true); return false; }
        }

        public bool WriteBlockDataToMeter(byte attributeID, byte[] ParameterOBIS, byte paraClassID, byte typeofStruct, int lengthofStruct, List<byte> ParameterBytes, byte[] DataRequestType)
        {
            int writeResponse = WritBlockToMeter(ParameterBytes, attributeID, ParameterOBIS, paraClassID, typeofStruct, lengthofStruct, DataRequestType);
            if (writeResponse == (int)ProgrammingCode.Success) { /* DisplayStatusMsg("Parameter Written Successfully.", false); */return true; }
            else if (writeResponse == (int)ProgrammingCode.AccessDenied) { DisplayStatusMsg("Access Denied !", true); /*MessageBox.Show("Access Denied.Please Change The Mode!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); */return false; }
            else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed) { DisplayStatusMsg("Cosem Connection Failed!", true); /*MessageBox.Show("Cosem Connection Failed!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);*/ return false; }
            else { DisplayStatusMsg("Communication Failed!", true); return false; }
        }

        public bool WriteImageBlockDataToMeter(byte attributeID, byte[] ParameterOBIS, byte paraClassID, byte typeofStruct, int lengthofStruct, List<byte> ParameterBytes, byte[] DataRequestType, List<byte> imgcrc, List<byte> imgfooter)
        {

            int writeResponse = WritImageBlockToMeter(ParameterBytes, attributeID, ParameterOBIS, paraClassID, typeofStruct, lengthofStruct, DataRequestType, imgcrc, imgfooter);
            if (writeResponse == (int)ProgrammingCode.Success) { /* DisplayStatusMsg("Parameter Written Successfully.", false); */return true; }
            else if (writeResponse == (int)ProgrammingCode.AccessDenied) { DisplayStatusMsg("Access Denied !", true); /*MessageBox.Show("Access Denied.Please Change The Mode!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); */return false; }
            else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed) { DisplayStatusMsg("Cosem Connection Failed!", true); /*MessageBox.Show("Cosem Connection Failed!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);*/ return false; }
            else { DisplayStatusMsg("Communication Failed!", true); return false; }
        }
        
        private int WritParameterToMeter(List<byte> DataByte, byte attributeID, byte[] ParameterOBIS, byte ParaClassID, byte typeodData, byte lengthofData, byte[] DataRequestType)
        {
            
            try
            {
                if (SerialPortSettings.Default.ApplicationContext == (byte)ApplicationContext.LogicalModeWithCiphering)
                {
                    HDLCIndex = 0;
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ClientSAP);
                    GlobalObjects.objHDLCLIB.fIncSend();
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);

                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = DedicatedCommand(HDLCCommand, (byte)HDLCIndex, "Write", DataByte.Count);//Dedicated/Without Dedicated
                    int cipherindex = HDLCIndex;
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteToMeter(DataByte, HDLCCommand, (byte)HDLCIndex, attributeID, ParameterOBIS, ParaClassID, typeodData, lengthofData, DataRequestType);

                    HDLCIndex = GlobalObjects.objHDLCLIB.FillWriteParameters(HDLCCommand, (byte)HDLCIndex, DataByte);
                    //*******************AES GCM Encrypt ********************************
                    byte[] plaintextcommandbyte = new byte[HDLCIndex - cipherindex];
                    System.Buffer.BlockCopy(HDLCCommand, cipherindex, plaintextcommandbyte, 0, plaintextcommandbyte.Length);
                    HDLCIndex = cipherindex;
                    HDLCIndex = GlobalObjects.objGlobalFunctions.CreateCipherCommand(plaintextcommandbyte, HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                    GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, (byte)HDLCIndex);
                   // HDLCCommand[15] = (byte)(HDLCIndex - 18); //--FILL Cipher len
                    //-----------------------------Filling Length As BEER Encoding---------------
                    UInt16 itemplength = Convert.ToUInt16(HDLCIndex);
                    itemplength -= 18;
                    if (itemplength < 128)
                    {
                        HDLCCommand[15] = (byte)itemplength;
                    }
                    else if (itemplength > 127 && itemplength < 256)
                    {
                        itemplength -= 1;
                        HDLCCommand[15] = 0x81;
                        HDLCCommand[16] = (byte)(itemplength);

                    }
                    else
                    {
                        itemplength -= 2;
                        HDLCCommand[15] = 0x82;
                        HDLCCommand[16] = (byte)(itemplength >> 8);
                        HDLCCommand[17] = (byte)(itemplength);
                    }
                    //-------------------------------------------------------------------------------------

                    GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                    GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                    GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, (byte)HDLCIndex - 3);
                    GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, (byte)HDLCIndex - 2, (byte)HDLCIndex - 1);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);

                    if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, (byte)HDLCIndex)) return (int)ProgrammingCode.CosemConnectionFailed;
                  
                    byte[] plaintextResponse = GlobalObjects.objGlobalFunctions.GetPlainTextFromCipheredTest(17);
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer)) return (int)ProgrammingCode.CosemConnectionFailed;
                    System.Buffer.BlockCopy(plaintextResponse, 0, GlobalObjects.objSerialComm.ReceiveBuffer, 14, plaintextResponse.Length);
                    int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                    if (ret == 0x01) return (int)ProgrammingCode.Success;
                    else if (ret == 0x02 || ret == 0x04) return (int)ProgrammingCode.AccessDenied;
                    else return (int)ProgrammingCode.CosemConnectionFailed;
               
                }

                else
                {
                HDLCIndex = 0;
                HDLCCommand = new byte[1024];
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, (byte)HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteToMeter(DataByte, HDLCCommand, (byte)HDLCIndex, attributeID, ParameterOBIS, ParaClassID, typeodData, lengthofData, DataRequestType);

                HDLCIndex = GlobalObjects.objHDLCLIB.FillWriteParameters(HDLCCommand, (byte)HDLCIndex, DataByte);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, (byte)HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, (byte)HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, (byte)HDLCIndex - 2, (byte)HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);

                if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, (byte)HDLCIndex)) return (int)ProgrammingCode.CosemConnectionFailed;
                //////Application.DoEvents();
                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer)) return (int)ProgrammingCode.CosemConnectionFailed;
                int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                if (ret == 0x01) return (int)ProgrammingCode.Success;
                else if (ret == 0x02 || ret == 0x04) return (int)ProgrammingCode.AccessDenied;
                else return (int)ProgrammingCode.CosemConnectionFailed;
                }
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
            catch (Exception)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }

        private int WritMethodParameterToMeter(List<byte> DataByte, byte attributeID, byte[] ParameterOBIS, byte ParaClassID, byte typeodData, byte lengthofData, byte[] DataRequestType, byte AccessSelector)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, (byte)HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteMethodToMeter(HDLCCommand, (byte)HDLCIndex, attributeID, ParameterOBIS, ParaClassID, typeodData, lengthofData, DataRequestType, AccessSelector);

                HDLCIndex = GlobalObjects.objHDLCLIB.FillWriteParameters(HDLCCommand, (byte)HDLCIndex, DataByte);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, (byte)HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, (byte)HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, (byte)HDLCIndex - 2, (byte)HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);

                if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, (byte)HDLCIndex)) return (int)ProgrammingCode.CosemConnectionFailed;
                //////Application.DoEvents();
                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer)) return (int)ProgrammingCode.CosemConnectionFailed;
                int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                if (ret == 0x01) return (int)ProgrammingCode.Success;
                else if (ret == 0x02 || ret == 0x04) return (int)ProgrammingCode.AccessDenied;
                else return (int)ProgrammingCode.CosemConnectionFailed;

            }
            catch (Exception)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }

        public int ReadDataCommand(byte[] OBISCode, byte ClassCode, byte AttID)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, (byte)HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadByClassOBIS(HDLCCommand, (byte)HDLCIndex, AttID, OBISCode, ClassCode);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, (byte)HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, (byte)HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, (byte)HDLCIndex - 2, (byte)HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);

                if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, (byte)HDLCIndex)) return (int)ProgrammingCode.CosemConnectionFailed;
                //////Application.DoEvents();
                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer))
                    return (int)ProgrammingCode.CosemConnectionFailed;
                int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);

                if (ret == 0x01) return (int)ProgrammingCode.Success;
                else if (ret == 0x0E) return (int)ProgrammingCode.DataUnavailable; //Data block unavailable
                else if (ret == 0x02) return (int)ProgrammingCode.AccessDenied; //Access denied
                else if (ret == 0x03) return (int)ProgrammingCode.AccessDenied; //Access denied
                else return (int)ProgrammingCode.CosemConnectionFailed;
            }
            catch (Exception)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }
                   
        public int ReadDataCommand_Cyphered(byte[] OBISCode, byte ClassCode,byte AttID)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = DedicatedCommand(HDLCCommand, (byte)HDLCIndex, "Read",0);//Dedicated/Without Dedicated
                int cipherindex = HDLCIndex;
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadByClassOBIS(HDLCCommand, (byte)HDLCIndex, AttID, OBISCode, ClassCode);
               byte[] plaintextcommandbyte = new byte[HDLCIndex-cipherindex];
               System.Buffer.BlockCopy(HDLCCommand, cipherindex, plaintextcommandbyte, 0, plaintextcommandbyte.Length);
               HDLCIndex = cipherindex;
               //*******************AES GCM Encrypt ********************************
              HDLCIndex = GlobalObjects.objGlobalFunctions.CreateCipherCommand(plaintextcommandbyte, HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, (byte)HDLCIndex);
                HDLCCommand[15] = (byte)(HDLCIndex - 18); //--FILL Cipher len
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, (byte)HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, (byte)HDLCIndex - 2, (byte)HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);

                if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, (byte)HDLCIndex)) return (int)ProgrammingCode.CosemConnectionFailed;
                //////Application.DoEvents();
                //*******************AES GCM Decrypt ********************************
                byte[] plaintextResponse = GlobalObjects.objGlobalFunctions.GetPlainTextFromCipheredTest(17);
                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer)) return (int)ProgrammingCode.CosemConnectionFailed;
                System.Buffer.BlockCopy(plaintextResponse, 0, GlobalObjects.objSerialComm.ReceiveBuffer, 14, plaintextResponse.Length);

                int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);

               
                if (ret == 0x01) return (int)ProgrammingCode.Success;
                else if (ret == 0x0E) return (int)ProgrammingCode.DataUnavailable; //Data block unavailable
                else if (ret == 0x02) return (int)ProgrammingCode.AccessDenied; //Access denied
                else if (ret == 0x03) return (int)ProgrammingCode.AccessDenied; //Access denied
                else return (int)ProgrammingCode.CosemConnectionFailed;
            }
            catch (Exception)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }

        public int ReadDataBlockCommand_Cyphered(byte[] OBISCode, byte ClassCode, byte AttID, byte Access_Selector, List<byte> DescriptorByteList)
        {
            try
            {
                GlobalObjects.objCOSEMLIB.nBlockIndex = 0x00;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0x00;

                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = DedicatedCommand(HDLCCommand, (byte)HDLCIndex, "Read",0);//Dedicated/Without Dedicated
                int cipherindex = HDLCIndex;
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadByClassOBIS(HDLCCommand, (byte)HDLCIndex, AttID, OBISCode, ClassCode);
                //----------------------------For Selective Access--------------------------------------------------------------------------
                if (Access_Selector != 0x00)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.FillCommandData(HDLCCommand, (byte)(--HDLCIndex), DescriptorByteList);
                }
                //*******************AES GCM Encrypt ********************************
                byte[] plaintextcommandbyte = new byte[HDLCIndex - cipherindex];
                System.Buffer.BlockCopy(HDLCCommand, cipherindex, plaintextcommandbyte, 0, plaintextcommandbyte.Length);
                HDLCIndex = cipherindex;
              
                HDLCIndex = GlobalObjects.objGlobalFunctions.CreateCipherCommand(plaintextcommandbyte, HDLCCommand, (byte)HDLCIndex);
          
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, (byte)HDLCIndex);
                HDLCCommand[15] = (byte)(HDLCIndex - 18); //--FILL Cipher len
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, (byte)HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, (byte)HDLCIndex - 2, (byte)HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);

                if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, (byte)HDLCIndex))return (int)ProgrammingCode.CosemConnectionFailed;
            
                //*******************AES GCM Decrypt ********************************
                int IvCountindex = 17;
                if (GlobalObjects.objSerialComm.ReceiveBuffer[15] == 0x81) IvCountindex++;
                else if (GlobalObjects.objSerialComm.ReceiveBuffer[15] == 0x82) IvCountindex += 2;
                byte[] plaintextResponse = GlobalObjects.objGlobalFunctions.GetPlainTextFromCipheredTest(IvCountindex);
                GlobalObjects.objHDLCLIB.fIncRecieve();
                if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer)) return (int)ProgrammingCode.CosemConnectionFailed;
                System.Buffer.BlockCopy(plaintextResponse, 0, GlobalObjects.objSerialComm.ReceiveBuffer, 14, plaintextResponse.Length); 
                int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
               
                if (ret == 0x01) return (int)ProgrammingCode.Success;
                else if (ret == 0x05)return (int)ProgrammingCode.AccessDenied;               
                else if (ret == 0x07) return (int)ProgrammingCode.DataUnavailable;
                else if (ret != 0x02) return (int)ProgrammingCode.CosemConnectionFailed;
                while (true)
                {
                    HDLCIndex = 0;
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ClientSAP);
                    GlobalObjects.objHDLCLIB.fIncSend();
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, (byte)HDLCIndex);
                    if (GlobalObjects.objCOSEMLIB.DedKeystr != "")
                        HDLCCommand[HDLCIndex++] = 0xD0;
                    else
                        HDLCCommand[HDLCIndex++] = 0xC8;
                    
                    HDLCCommand[HDLCIndex++] = 0x00;
                    HDLCCommand[HDLCIndex]   = 0x00;//security suit added "0"
                    AppSettings objappSettings = new AppSettings();
                    byte oldSecurityMechanism = objappSettings.GetSecurityMachanism();
                    if (oldSecurityMechanism == 0x01)
                    {
                        GlobalObjects.objHDLCLIB.SecuritysuitByte = 0x20;
                        HDLCCommand[HDLCIndex++] = (byte)GlobalObjects.objHDLCLIB.SecuritysuitByte;
                    }
                    else
                        HDLCCommand[HDLCIndex++] = (byte)GlobalObjects.objHDLCLIB.SecuritysuitByte;//Security suit actual value
                    if (GlobalObjects.objCOSEMLIB.DedKeystr != "")
                    {
                       HDLCCommand[HDLCIndex++] = Convert.ToByte((GlobalObjects.objHDLCLIB.InitializationCounter & 0xFF000000) >> 24);
                        HDLCCommand[HDLCIndex++] = Convert.ToByte((GlobalObjects.objHDLCLIB.InitializationCounter & 0xFF0000) >> 16);
                        HDLCCommand[HDLCIndex++] = Convert.ToByte((GlobalObjects.objHDLCLIB.InitializationCounter & 0xFF00) >> 8);
                        HDLCCommand[HDLCIndex++] = Convert.ToByte(GlobalObjects.objHDLCLIB.InitializationCounter & 0x00FF);
                       
                    }
                    else
                    {
                        HDLCCommand[HDLCIndex++] = Convert.ToByte((GlobalObjects.objHDLCLIB.InitializationCounter & 0xFF000000) >> 24);
                        HDLCCommand[HDLCIndex++] = Convert.ToByte((GlobalObjects.objHDLCLIB.InitializationCounter & 0xFF0000) >> 16);
                        HDLCCommand[HDLCIndex++] = Convert.ToByte((GlobalObjects.objHDLCLIB.InitializationCounter & 0xFF00) >> 8);
                        HDLCCommand[HDLCIndex++] = Convert.ToByte(GlobalObjects.objHDLCLIB.InitializationCounter & 0x00FF);
                    }
                   
                   int cipherindexs = HDLCIndex;
                   HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, (byte)HDLCIndex);

                    //******************* AES GCM Encrypt ********************************
                    byte[] plaintextbyte = new byte[HDLCIndex - cipherindex];
                    System.Buffer.BlockCopy(HDLCCommand, cipherindexs, plaintextbyte, 0, plaintextbyte.Length);
                    HDLCIndex = cipherindexs;
                    HDLCIndex = GlobalObjects.objGlobalFunctions.CreateCipherCommand(plaintextbyte, HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                    GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, (byte)HDLCIndex);
                    HDLCCommand[15] = (byte)(HDLCIndex - 18); //--FILL Cipher len
                    GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                    GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                    GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, (byte)HDLCIndex - 3);
                    GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, (byte)HDLCIndex - 2, (byte)HDLCIndex - 1);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);
                   if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, (byte)HDLCIndex)) return (int)ProgrammingCode.CosemConnectionFailed;

                    //*******************AES GCM Decrypt ********************************
                    IvCountindex = 17;
                    if (GlobalObjects.objSerialComm.ReceiveBuffer[15] == 0x81) IvCountindex++;
                    else if (GlobalObjects.objSerialComm.ReceiveBuffer[15] == 0x82) IvCountindex += 2;
                    byte[] plainResponsetext = GlobalObjects.objGlobalFunctions.GetPlainTextFromCipheredTest(IvCountindex);
                    GlobalObjects.objHDLCLIB.fIncRecieve();

                    if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer)) return (int)ProgrammingCode.CosemConnectionFailed;
                    System.Buffer.BlockCopy(plainResponsetext, 0, GlobalObjects.objSerialComm.ReceiveBuffer, 14, plainResponsetext.Length); 

                    ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                    if (ret == 0x01) break;
                    else if (ret == 0x02) continue;                                        
                  }
                   return (int)ProgrammingCode.Success;
            }
            catch (Exception)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
               // throw;
            }
        }
              
        public int ReadDataBlockCommand(byte[] OBISCode, byte ClassCode, byte AttID, byte Access_Selector, List<byte> DescriptorByteList)
        {
            try
            {
                GlobalObjects.objCOSEMLIB.nBlockIndex = 0x00;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0x00;
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, (byte)HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadByClassOBIS(HDLCCommand, (byte)HDLCIndex, AttID, OBISCode, ClassCode);
                //----------------------------For Selective Access--------------------------------------------------------------------------
                if (Access_Selector != 0x00)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.FillCommandData(HDLCCommand, (byte)(--HDLCIndex), DescriptorByteList);
                }
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, (byte)HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, (byte)HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, (byte)HDLCIndex - 2, (byte)HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);

                if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, (byte)HDLCIndex)) return (int)ProgrammingCode.CosemConnectionFailed;

                GlobalObjects.objHDLCLIB.fIncRecieve();
                if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer)) return (int)ProgrammingCode.CosemConnectionFailed;

                int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                if (ret == 0x01) return (int)ProgrammingCode.Success;
                else if (ret == 0x05) return (int)ProgrammingCode.AccessDenied;
                else if (ret == 0x07) return (int)ProgrammingCode.DataUnavailable;
                else if (ret != 0x02) return (int)ProgrammingCode.CosemConnectionFailed;
                while (true)
                {

                    HDLCIndex = 0;
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ClientSAP);
                    GlobalObjects.objHDLCLIB.fIncSend();
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                    GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, (byte)HDLCIndex);
                    GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                    GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                    GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, (byte)HDLCIndex - 3);
                    GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, (byte)HDLCIndex - 2, (byte)HDLCIndex - 1);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    //7EA014022321766E17E6E600C002C100000002CA8C7E
                    if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, (byte)HDLCIndex)) return (int)ProgrammingCode.CosemConnectionFailed;
                    if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer)) return (int)ProgrammingCode.CosemConnectionFailed;
                    ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                    if (ret == 0x01) break;
                    else if (ret == 0x02) continue;
                }
                return (int)ProgrammingCode.Success;
            }
            catch (Exception)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
                // throw;
            }
        }
      
        public bool WriteImageActionNormalToMeter(byte attributeID, byte[] ParameterOBIS, byte paraClassID, byte typeofStruct, int lengthofStruct, List<byte> ParameterBytes, byte[] DataRequestType, List<byte> imgcrc, List<byte> imgfooter)
        {
            int writeResponse = WritActionNormalToMeter(ParameterBytes, attributeID, ParameterOBIS, paraClassID, typeofStruct, lengthofStruct, DataRequestType, imgcrc, imgfooter);
            if (writeResponse == (int)ProgrammingCode.Success) { /* DisplayStatusMsg("Parameter Written Successfully.", false); */return true; }
            else if (writeResponse == (int)ProgrammingCode.AccessDenied) { DisplayStatusMsg("Access Denied !", true); /*MessageBox.Show("Access Denied.Please Change The Mode!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); */return false; }
            else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed) { DisplayStatusMsg("Cosem Connection Failed!", true); /*MessageBox.Show("Cosem Connection Failed!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);*/ return false; }
            else { DisplayStatusMsg("Communication Failed!", true); return false; }
        }

        private int WritActionNormalToMeter(List<byte> DataByte, byte attributeID, byte[] ParameterOBIS, byte ParaClassID, byte typeodData, int lengthofData, byte[] DataRequestType, List<byte> imgcrc, List<byte> imgfooter)//(byte[] nDataArray, int nLength, byte atb)
        {
            try
            {
                if (SerialPortSettings.Default.ApplicationContext == (byte)ApplicationContext.LogicalModeWithCiphering)
                {
                    HDLCCommand = new byte[1024];
                    HDLCIndex = 0;
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ClientSAP);
                    GlobalObjects.objHDLCLIB.fIncSend();
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, (byte)HDLCIndex);

                    HDLCIndex = DedicatedCommand(HDLCCommand, (byte)HDLCIndex, "Write", DataByte.Count);//Dedicated/Without Dedicated
                    int cipherindex = HDLCIndex;
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryToWriteBlockToMeterWithoutAccessSelector(HDLCCommand, (byte)HDLCIndex, attributeID, ParameterOBIS, ParaClassID, typeodData, lengthofData, DataRequestType);

                    HDLCIndex = GlobalObjects.objCOSEMLIB.fActionNormalImgBlockTransferPacket(HDLCCommand, HDLCIndex, DataByte.ToArray(), null);

                    //*******************AES GCM Encrypt ********************************
                    byte[] plaintextcommandbyte = new byte[HDLCIndex - cipherindex];
                    System.Buffer.BlockCopy(HDLCCommand, cipherindex, plaintextcommandbyte, 0, plaintextcommandbyte.Length);
                    HDLCIndex = cipherindex;
                    HDLCIndex = GlobalObjects.objGlobalFunctions.CreateCipherCommand(plaintextcommandbyte, HDLCCommand, (byte)HDLCIndex);

                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                    GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                   // HDLCCommand[15] = (byte)(HDLCIndex - 18); //--FILL Cipher len
                    //-----------------------------Filling Length As BEER Encoding---------------
                    UInt16 itemplength =Convert.ToUInt16(HDLCIndex) ;
                    itemplength -= 18;
                    if (itemplength < 128)
                    {
                        HDLCCommand[15] = (byte)itemplength;
                    }
                    else if (itemplength > 127 && itemplength < 256)
                    {
                        itemplength -= 1;
                        HDLCCommand[15] = 0x81;
                        HDLCCommand[16] = (byte)(itemplength);

                    }
                    else
                    {
                        itemplength -= 2;
                        HDLCCommand[15] = 0x82;
                        HDLCCommand[16] = (byte)(itemplength >> 8);
                        HDLCCommand[17] = (byte)(itemplength);
                    }
                    //-------------------------------------------------------------------------------------
                    GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                    GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                    GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                    GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                    if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex)) return (int)ProgrammingCode.CosemConnectionFailed;

                    byte[] plaintextResponse = GlobalObjects.objGlobalFunctions.GetPlainTextFromCipheredTest(17);
                    //////Application.DoEvents();

                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer)) return (int)ProgrammingCode.CosemConnectionFailed;
                    System.Buffer.BlockCopy(plaintextResponse, 0, GlobalObjects.objSerialComm.ReceiveBuffer, 14, plaintextResponse.Length);
                    int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForImageBlockSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                    if (ret == 0x01) return (int)ProgrammingCode.Success;
                    else if (ret == 0x02) return (int)ProgrammingCode.AccessDenied;
                    else if (ret == 0x03) return (int)ProgrammingCode.Fail;
                    else return (int)ProgrammingCode.CosemConnectionFailed;
                }

                else
                {
                    HDLCCommand = new byte[600];
                    HDLCIndex = 0;
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ClientSAP);
                    GlobalObjects.objHDLCLIB.fIncSend();
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, (byte)HDLCIndex);

                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryToWriteBlockToMeterWithoutAccessSelector(HDLCCommand, (byte)HDLCIndex, attributeID, ParameterOBIS, ParaClassID, typeodData, lengthofData, DataRequestType);

                    HDLCIndex = GlobalObjects.objCOSEMLIB.fActionNormalImgBlockTransferPacket(HDLCCommand, HDLCIndex, DataByte.ToArray(), null);
                                      
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                    GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                    GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                    GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                    GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                    GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                    if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex)) return (int)ProgrammingCode.CosemConnectionFailed;
                     GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer)) return (int)ProgrammingCode.CosemConnectionFailed;
                   int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForImageBlockSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                    if (ret == 0x01) return (int)ProgrammingCode.Success;
                    else if (ret == 0x02) return (int)ProgrammingCode.AccessDenied;
                    else if (ret == 0x03) return (int)ProgrammingCode.Fail;
                    else return (int)ProgrammingCode.CosemConnectionFailed;
                }
               
            }
            catch (Exception)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }
        
        private int WritBlockToMeter(List<byte> DataByte, byte attributeID, byte[] ParameterOBIS, byte ParaClassID, byte typeodData, int lengthofData, byte[] DataRequestType)//(byte[] nDataArray, int nLength, byte atb)
        {
            try
            {
               // int nErrorCode = 0x00;
                bool nBlkTransfer = false;               
                while (true)
                {
                    if (SerialPortSettings.Default.ApplicationContext == (byte)ApplicationContext.LogicalModeWithCiphering)
                    {
                        HDLCCommand = new byte[200];
                        HDLCIndex = 0;
                        HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);
                        HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, (byte)HDLCIndex);
                        HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                        HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ClientSAP);
                        GlobalObjects.objHDLCLIB.fIncSend();
                        HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, (byte)HDLCIndex);
                        HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                        HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, (byte)HDLCIndex);
                        HDLCIndex = DedicatedCommand(HDLCCommand, (byte)HDLCIndex, "Write", 0);//Dedicated/Without Dedicated
                        int cipherindex = HDLCIndex;
                        if (nBlkTransfer == false)
                        {
                            GlobalObjects.objCOSEMLIB.nTotalPacketSize = lengthofData;
                            HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryToWriteBlockToMeter(HDLCCommand, (byte)HDLCIndex, attributeID, ParameterOBIS, ParaClassID, typeodData, lengthofData, DataRequestType);
                        }
                        else
                        {
                            HDLCCommand[(byte)HDLCIndex++] = 0xC1;
                            HDLCCommand[(byte)HDLCIndex++] = 0x03;           
                            HDLCCommand[(byte)HDLCIndex++] = 0xC1;
                        }
                        HDLCIndex = GlobalObjects.objCOSEMLIB.fSetBlockTransferPacket(HDLCCommand, (byte)HDLCIndex, DataByte.ToArray(), nBlkTransfer);

                        //*******************AES GCM Encrypt ********************************
                        byte[] plaintextcommandbyte = new byte[HDLCIndex - cipherindex];
                        System.Buffer.BlockCopy(HDLCCommand, cipherindex, plaintextcommandbyte, 0, plaintextcommandbyte.Length);
                        HDLCIndex = cipherindex;
                        HDLCIndex = GlobalObjects.objGlobalFunctions.CreateCipherCommand(plaintextcommandbyte, HDLCCommand, (byte)HDLCIndex);
                        
                        HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                        GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, (byte)HDLCIndex);
                        HDLCCommand[15] = (byte)(HDLCIndex - 18); //--FILL Cipher len
                        //-----------------------------Filling Length As BEER Encoding---------------
                        //UInt16 itemplength = Convert.ToUInt16(HDLCIndex);
                        //itemplength -= 18;
                        //if (itemplength < 128)
                        //{
                        //    HDLCCommand[15] = (byte)itemplength;
                        //}
                        //else if (itemplength > 127 && itemplength < 256)
                        //{
                        //    HDLCCommand[15] = 0x81;
                        //    HDLCCommand[16] = (byte)(itemplength);

                        //}
                        //else
                        //{
                        //    HDLCCommand[15] = 0x82;
                        //    HDLCCommand[16] = (byte)(itemplength >> 8);
                        //    HDLCCommand[17] = (byte)(itemplength);
                        //}
                        //-------------------------------------------------------------------------------------
                        GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                        GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                        GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, (byte)HDLCIndex - 3);
                        GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, (byte)HDLCIndex - 2, (byte)HDLCIndex - 1);
                        HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);

                        if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, (byte)HDLCIndex))return (int)ProgrammingCode.CosemConnectionFailed;
                        byte[] plaintextResponse = GlobalObjects.objGlobalFunctions.GetPlainTextFromCipheredTest(17);

                        GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                        if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer))return (int)ProgrammingCode.CosemConnectionFailed;
                        System.Buffer.BlockCopy(plaintextResponse, 0, GlobalObjects.objSerialComm.ReceiveBuffer, 14, plaintextResponse.Length);
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForImageBlockSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)return (int)ProgrammingCode.Success;                             
                        else if (ret == 0x02)return (int)ProgrammingCode.AccessDenied;
                        else if (ret == 0x03) return (int)ProgrammingCode.Fail;
                        else if (ret == 0x04) nBlkTransfer = true;                            
                        else return (int)ProgrammingCode.CosemConnectionFailed;
                                            
                    }
                    else
                    {
                        HDLCCommand = new byte[200];
                        HDLCIndex = 0;
                        HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);
                        HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, (byte)HDLCIndex);
                        HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                        HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ClientSAP);
                        GlobalObjects.objHDLCLIB.fIncSend();
                        HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, (byte)HDLCIndex);
                        HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                        HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, (byte)HDLCIndex);

                        if (nBlkTransfer == false)
                        {
                            GlobalObjects.objCOSEMLIB.nTotalPacketSize = lengthofData;
                            HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryToWriteBlockToMeter(HDLCCommand, (byte)HDLCIndex, attributeID, ParameterOBIS, ParaClassID, typeodData, lengthofData, DataRequestType);
                        }
                        else
                        {
                            HDLCCommand[(byte)HDLCIndex++] = 0xC1;
                            HDLCCommand[(byte)HDLCIndex++] = 0x03;           
                            HDLCCommand[(byte)HDLCIndex++] = 0xC1;
                        }
                        HDLCIndex = GlobalObjects.objCOSEMLIB.fSetBlockTransferPacket(HDLCCommand, (byte)HDLCIndex, DataByte.ToArray(), nBlkTransfer);
                        HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                        GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, (byte)HDLCIndex);
                        GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                        GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                        GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, (byte)HDLCIndex - 3);
                        GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, (byte)HDLCIndex - 2, (byte)HDLCIndex - 1);
                        HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);
                        if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, (byte)HDLCIndex))return (int)ProgrammingCode.CosemConnectionFailed;
                        GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                        //GlobalObjects.objSerialComm.ReceiveBuffer = DLMSDataStracture.GetHexStringToByteList("7EA013FD0002040196CAA4E6E700C503C103730E7E");//Debug Code
                        if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer))return (int)ProgrammingCode.CosemConnectionFailed;
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForImageBlockSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)return (int)ProgrammingCode.Success;                             
                        else if (ret == 0x02)return (int)ProgrammingCode.AccessDenied;
                        else if (ret == 0x03) return (int)ProgrammingCode.Fail;
                        else if (ret == 0x04) nBlkTransfer = true;                            
                        else return (int)ProgrammingCode.CosemConnectionFailed;
                    }
                }
                
            }
            catch (Exception)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }

        private int WritImageBlockToMeter(List<byte> DataByte, byte attributeID, byte[] ParameterOBIS, byte ParaClassID, byte typeodData, int lengthofData, byte[] DataRequestType, List<byte> imgcrc, List<byte> imgfooter)//(byte[] nDataArray, int nLength, byte atb)
        {
            try
            {
                // int nErrorCode = 0x00;
                bool nBlkTransfer = false;
                while (true)
                {
                    int nmaxbuffersize = GlobalObjects.objCOSEMLIB.nMaxBufferSize;
                    HDLCCommand = new byte[600];
                    HDLCIndex = 0;
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, (byte)HDLCIndex, SerialPortSettings.Default.ClientSAP);
                    GlobalObjects.objHDLCLIB.fIncSend();
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, (byte)HDLCIndex);
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, (byte)HDLCIndex);

                    if (nBlkTransfer == false)
                    {
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = lengthofData + imgcrc.Count;
                        lengthofData += imgcrc.Count;
                        HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryToWriteBlockToMeterWithoutAccessSelector(HDLCCommand, (byte)HDLCIndex, attributeID, ParameterOBIS, ParaClassID, typeodData, lengthofData, DataRequestType);
                        GlobalObjects.objCOSEMLIB.nMaxBufferSize += imgcrc.Count;

                        for (int i = 0; i < imgcrc.Count; i++)
                            DataByte.Insert(i, imgcrc[i]);

                    }
                    else
                    {
                        HDLCCommand[(byte)HDLCIndex++] = 0xC3;
                        HDLCCommand[(byte)HDLCIndex++] = 0x06;
                        HDLCCommand[(byte)HDLCIndex++] = 0xC1;
                       
                    }


                    HDLCIndex = GlobalObjects.objCOSEMLIB.fSetImgBlockTransferPacket(HDLCCommand, HDLCIndex, DataByte.ToArray(), nBlkTransfer, null);
                    //GlobalObjects.objCOSEMLIB.nMaxBufferSize = nmaxbuffersize;
                    // if last packet
                    if (HDLCCommand[17] == 0x01)
                    {
                        for (int i = 0; i < imgfooter.Count; i++)
                        {
                            HDLCCommand[(byte)HDLCIndex++] = imgfooter[i];
                        }
                        GlobalObjects.objCOSEMLIB.nMaxBufferSize += imgfooter.Count;
                        if (GlobalObjects.objCOSEMLIB.nMaxBufferSize < 128)
                        {
                            HDLCCommand[22] = (byte)(GlobalObjects.objCOSEMLIB.nMaxBufferSize);
                        }
                        else if (GlobalObjects.objCOSEMLIB.nMaxBufferSize > 127 && GlobalObjects.objCOSEMLIB.nMaxBufferSize < 256)
                        {
                            UInt16 itemplength = (UInt16)(GlobalObjects.objCOSEMLIB.nMaxBufferSize);

                            //HDLCCommand[23] = (byte)(itemplength >> 8);
                            //HDLCCommand[24] = (byte)(itemplength);

                            HDLCCommand[23] = (byte)(GlobalObjects.objCOSEMLIB.nMaxBufferSize);

                        }
                        else
                        {
                            UInt16 itemplength = (UInt16)(GlobalObjects.objCOSEMLIB.nMaxBufferSize);
                            //  HDLCCommand[23] = 0x82;
                            HDLCCommand[23] = (byte)(itemplength >> 8);
                            HDLCCommand[24] = (byte)(itemplength);
                        }
                    }

                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                    GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                    GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                    GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                    GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                    GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                    if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex)) return (int)ProgrammingCode.CosemConnectionFailed;

                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer)) return (int)ProgrammingCode.CosemConnectionFailed;
                    GlobalObjects.objCOSEMLIB.nMaxBufferSize = nmaxbuffersize;
                    int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForImageBlockSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                    if (ret == 0x01) return (int)ProgrammingCode.Success;
                    else if (ret == 0x02) return (int)ProgrammingCode.AccessDenied;
                    else if (ret == 0x03) return (int)ProgrammingCode.Fail;
                    else if (ret == 0x04) nBlkTransfer = true;
                    else return (int)ProgrammingCode.CosemConnectionFailed;


                }
               
            }
            catch (Exception)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }
            
        public List<byte> GetByteByEntry_ValueType(long fromEntry, long toEntry)
        {
            List<byte> EnrtyByValue = new List<byte>();           
            EnrtyByValue.Add(0x01);
            EnrtyByValue.Add(0x02);
            EnrtyByValue.Add(0x02);      
            EnrtyByValue.Add(0x04);
            EnrtyByValue.Add(0x06);
            EnrtyByValue.Add(Convert.ToByte((fromEntry & 0xFF000000) >> 24));
            EnrtyByValue.Add(Convert.ToByte((fromEntry & 0xFF0000) >> 16));
            EnrtyByValue.Add(Convert.ToByte((fromEntry & 0xFF00) >> 8));
            EnrtyByValue.Add(Convert.ToByte(fromEntry & 0x00FF));          
            EnrtyByValue.Add(0x06);
            EnrtyByValue.Add(Convert.ToByte((toEntry & 0xFF000000) >> 24));
            EnrtyByValue.Add(Convert.ToByte((toEntry & 0xFF0000) >> 16));
            EnrtyByValue.Add(Convert.ToByte((toEntry & 0xFF00) >> 8));
            EnrtyByValue.Add(Convert.ToByte(toEntry & 0x00FF));
            EnrtyByValue.Add(0x12);
            EnrtyByValue.Add(0x00);
            EnrtyByValue.Add(0x01);
            EnrtyByValue.Add(0x12);
            EnrtyByValue.Add(0x00);
            EnrtyByValue.Add(0x00);
            return EnrtyByValue;
        }

        public List<byte> GetByteByEntry_DateType(DateTime fromDate, DateTime toDate)
        {
            List<byte> EnrtyByDate = new List<byte>();     
            
            EnrtyByDate.Add(0x01);
            EnrtyByDate.Add(0x01);
            EnrtyByDate.Add(0x02);
            EnrtyByDate.Add(0x04);
            EnrtyByDate.Add(0x02);
            EnrtyByDate.Add(0x04);

            EnrtyByDate.Add(0x12);
            EnrtyByDate.Add(0x00);
            EnrtyByDate.Add(0x08);

            EnrtyByDate.Add(0x09);
            EnrtyByDate.Add(0x06);

            EnrtyByDate.Add(0x00); //obis code
            EnrtyByDate.Add(0x00);
            EnrtyByDate.Add(0x01);
            EnrtyByDate.Add(0x00);
            EnrtyByDate.Add(0x00);
            EnrtyByDate.Add(0xFF);

            EnrtyByDate.Add(0x0F);
            EnrtyByDate.Add(0x02);
            EnrtyByDate.Add(0x12);

            EnrtyByDate.Add(0x00);
            EnrtyByDate.Add(0x00);

            EnrtyByDate.Add(0x09);
            EnrtyByDate.Add(0x0C);

            EnrtyByDate.Add(Convert.ToByte((fromDate.Year & 0xFF00) >> 8));
            EnrtyByDate.Add(Convert.ToByte(fromDate.Year & 0x00FF));
            //EnrtyByDate.Add(Convert.ToByte((fromDate.Year / 100) % 20)); //year
            //EnrtyByDate.Add(Convert.ToByte(fromDate.Year % 100));

            EnrtyByDate.Add(Convert.ToByte(fromDate.Month)); //month

            EnrtyByDate.Add(Convert.ToByte(fromDate.Day));
            EnrtyByDate.Add(0xFF);

            EnrtyByDate.Add(Convert.ToByte(fromDate.Hour));
            EnrtyByDate.Add(Convert.ToByte(fromDate.Minute));
            EnrtyByDate.Add(Convert.ToByte(fromDate.Second));

            EnrtyByDate.Add(0xFF);

            EnrtyByDate.Add(0x80);
            EnrtyByDate.Add(0x00);

            EnrtyByDate.Add(0x00);

            EnrtyByDate.Add(0x09);
            EnrtyByDate.Add(0x0C);

            EnrtyByDate.Add(Convert.ToByte((toDate.Year & 0xFF00) >> 8));
            EnrtyByDate.Add(Convert.ToByte(toDate.Year & 0x00FF));

            //EnrtyByDate.Add(Convert.ToByte((toDate.Year / 100) % 20)); //year
            //EnrtyByDate.Add(Convert.ToByte(toDate.Year % 100));

            EnrtyByDate.Add(Convert.ToByte(toDate.Month)); //month

            EnrtyByDate.Add(Convert.ToByte(toDate.Day));
            EnrtyByDate.Add(0xFF);

            EnrtyByDate.Add(Convert.ToByte(toDate.Hour));
            EnrtyByDate.Add(Convert.ToByte(toDate.Minute));
            EnrtyByDate.Add(Convert.ToByte(toDate.Second));

            EnrtyByDate.Add(0xFF);

            EnrtyByDate.Add(0x80);
            EnrtyByDate.Add(0x00);
            EnrtyByDate.Add(0x00);

            EnrtyByDate.Add(0x01);
            EnrtyByDate.Add(0x00);

            return EnrtyByDate;
        }

        public byte[] Read3PHDLMSCalibCoeff(byte[] caliOBIS)
        {
            int iCommandTimeout = GlobalObjects.objSerialComm.CommandTimeout;
            int iInterchatracterDelay = GlobalObjects.objSerialComm.InterchatracterDelay;
            byte[] rec_pkt = new byte[GlobalObjects.objSerialComm.NoOfBytesToBeReceive3PHDLMSCalibCoeff + 1];
            try
            {
                //HDLCIndex = 0;  
                //HDLCCommand[HDLCIndex++] = 0xEE;
                //HDLCCommand[HDLCIndex++] = 0x25;
                //HDLCCommand[HDLCIndex++] = 0x00;
                //HDLCCommand[HDLCIndex++] = 0x00;
                //HDLCCommand[HDLCIndex++] = 0x13;

             
                //GlobalObjects.objSerialComm.CommandTimeout = 24000;
                //GlobalObjects.objSerialComm.InterchatracterDelay = 2500 * 3;

                if (!GlobalObjects.objSerialComm.fSendDataToPort(caliOBIS, caliOBIS.Length)) { return null; }
                if (GlobalObjects.objSerialComm.bufferIndex < GlobalObjects.objSerialComm.NoOfBytesToBeReceive3PHDLMSCalibCoeff) return null;
                for (int i = 0; i < GlobalObjects.objSerialComm.bufferIndex; i++)
                {
                    rec_pkt[i] = GlobalObjects.objSerialComm.ReceiveBuffer[i];
                }
                return rec_pkt;                    
                 
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                //GlobalObjects.objSerialComm.CommandTimeout = iCommandTimeout;
                //GlobalObjects.objSerialComm.InterchatracterDelay = iInterchatracterDelay;
            }
        }

        public int DedicatedCommand(byte[] Buffer, int nBufferIndex, string InputType, int bytelength)
        {
         
          if (GlobalObjects.objCOSEMLIB.DedKeystr != "" && InputType == "Read")
              Buffer[nBufferIndex++] = 0xD0;
          else if (GlobalObjects.objCOSEMLIB.DedKeystr == "" && InputType == "Read")
              Buffer[nBufferIndex++] = 0xC8;
          else if (GlobalObjects.objCOSEMLIB.DedKeystr != "" && InputType == "Write")
              Buffer[nBufferIndex++] = 0xD1;
          else if (GlobalObjects.objCOSEMLIB.DedKeystr == "" && InputType == "Write")
              Buffer[nBufferIndex++] = 0xC9;


          if (bytelength < 128)
          {
              Buffer[nBufferIndex++] = 0x00;//Length
          }
          else if (bytelength > 127 && bytelength < 256)
          {
              Buffer[nBufferIndex++] = 0x00;//Length
              Buffer[nBufferIndex++] = 0x00;//Length
          }
          else
          {
              Buffer[nBufferIndex++] = 0x00;//Length
              Buffer[nBufferIndex++] = 0x00;//Length
              Buffer[nBufferIndex++] = 0x00;//Length
          }

         // Buffer[nBufferIndex++] = 0x00;//Length
          Buffer[nBufferIndex] = 0x00;//security suit added "0"

          AppSettings objappSettings = new AppSettings();
          byte oldSecurityMechanism = objappSettings.GetSecurityMachanism();
          if (oldSecurityMechanism == 0x01)
          {
              GlobalObjects.objHDLCLIB.SecuritysuitByte = 0x20;
              Buffer[nBufferIndex++] = (byte)GlobalObjects.objHDLCLIB.SecuritysuitByte;
          }
          else
              Buffer[nBufferIndex++] = (byte)GlobalObjects.objHDLCLIB.SecuritysuitByte;//Security suit actual value
          //if (GlobalObjects.objCOSEMLIB.DedKeystr != "")//Dedicated
          //{
          //    Buffer[nBufferIndex++] = 0x00;
          //    Buffer[nBufferIndex++] = 0x00;
          //    Buffer[nBufferIndex++] = 0x00;
          //    GlobalObjects.objHDLCLIB.InitializationCounter = 0;
          //    Buffer[nBufferIndex++] = (byte)GlobalObjects.objHDLCLIB.InitializationCounter;
          //}
          //else if (GlobalObjects.objCOSEMLIB.DedKeystr == "")//outDedicated
          //{
              Buffer[nBufferIndex++] = Convert.ToByte((GlobalObjects.objHDLCLIB.InitializationCounter & 0xFF000000) >> 24);
              Buffer[nBufferIndex++] = Convert.ToByte((GlobalObjects.objHDLCLIB.InitializationCounter & 0xFF0000) >> 16);
              Buffer[nBufferIndex++] = Convert.ToByte((GlobalObjects.objHDLCLIB.InitializationCounter & 0xFF00) >> 8);
              Buffer[nBufferIndex++] = Convert.ToByte(GlobalObjects.objHDLCLIB.InitializationCounter & 0x00FF);
          //}

          //else
          //{
          //    Buffer[nBufferIndex++] = Convert.ToByte((GlobalObjects.objHDLCLIB.InitializationCounter & 0xFF000000) >> 24);
          //    Buffer[nBufferIndex++] = Convert.ToByte((GlobalObjects.objHDLCLIB.InitializationCounter & 0xFF0000) >> 16);
          //    Buffer[nBufferIndex++] = Convert.ToByte((GlobalObjects.objHDLCLIB.InitializationCounter & 0xFF00) >> 8);
          //    Buffer[nBufferIndex++] = Convert.ToByte(GlobalObjects.objHDLCLIB.InitializationCounter & 0x00FF);
          //}
          return nBufferIndex;
        
            
      }

        #region Methods For Validation Automation that Supports Byte Array instead of List bytes
        
        public bool ReadBlockFromMeter_Bytes(byte[] ParameterOBIS, TextBox[] controllist, string displayFormat, decimal emf, byte classCode, byte AttCode, byte Access_Selector, byte[] DescriptorByteList)
        {
            return ReadBlockFromMeter(ParameterOBIS, controllist, displayFormat, emf, classCode, AttCode, Access_Selector, DescriptorByteList.ToList<byte>());
        }

        public bool WriteDataToMeter_Bytes(byte attributeID, byte[] ParameterOBIS, byte paraClassID, byte typeofStruct, byte lengthofStruct, byte[] ParameterBytes, byte[] DataRequestType)
        {
            return WriteDataToMeter(attributeID, ParameterOBIS, paraClassID, typeofStruct, lengthofStruct, ParameterBytes.ToList<byte>(), DataRequestType);
        }

        public bool WriteBlockDataToMeter_Bytes(byte attributeID, byte[] ParameterOBIS, byte paraClassID, byte typeofStruct, int lengthofStruct, byte[] ParameterBytes, byte[] DataRequestType)
        {
            return WriteBlockDataToMeter(attributeID, ParameterOBIS, paraClassID, typeofStruct, lengthofStruct, ParameterBytes.ToList<byte>(), DataRequestType);
        }

        public int ReadDataBlockCommand_Cyphered_Byte(byte[] OBISCode, byte ClassCode, byte AttID, byte Access_Selector, byte[] DescriptorByteList)
        {
            return ReadDataBlockCommand_Cyphered(OBISCode, ClassCode, AttID, Access_Selector, DescriptorByteList.ToList<byte>());
        }

        public string[] DLMSDataFormatorLabView(byte[] blockdata, int index, bool inascii)
        {
            try
            {
               return DLMSDataStracture.DLMSDataFormator(blockdata, index, inascii);

            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
      
    }

  public class UpdateEventArgs : System.EventArgs
  {
       public string msg;
       public bool isError;
       public UpdateEventArgs(string smsg, bool isError)
      {
          this.msg = smsg;
          this.isError = isError;

      }
  }

  public class IECLayerInterface
  {
      public delegate void UpdateHandler(object sender, UpdateEventArgs e);
      public event UpdateHandler UpdatedLed;
      UpdateEventArgs args = null;
      public string MeterSignonResponse = string.Empty;
      public string MeterReadoutnResponse = string.Empty;
      #region Enums

      private enum ProgrammingCode
      {
          Success,
          Fail,
          AccessDenied,
          DataUnavailable,
          TimeOut,
          SignOnFailed,
          CosemConnectionFailed,
          MeterIDMismatch

      }

      public enum IECSignOnMode
      {
        _IEC_READ,      
        _IEC_MANUFACURER,
        _IEC_PRGRAMING,

      }

      #endregion
      public void DisplayStatusMsg(string msgString, bool isError)
      {
          args = new UpdateEventArgs(msgString, isError);
          UpdatedLed(this, args);
      }

      public string TestString()
      {
          return "";
      }
      //---------------------IEC Meter Communication New implementation------------------------------
      public bool ConnectToIECMeter(int IECSignOnModeType)
      {
          
          DisplayStatusMsg("Connecting...", false);
          if (!IECPhysicalLayerConnect(false)) { DisplayStatusMsg("Connecting Failed!", true); return false; }

          DisplayStatusMsg("Signon...", false);
          SetCommandProperties("MeterSignon");
          if (!ReadMeterData()) { MeterSignonResponse = GlobalObjects.objIECMeterSerialComm.strOutBuff;  DisplayStatusMsg("Signon Failed!", true); return false; }
          else MeterSignonResponse = GlobalObjects.objIECMeterSerialComm.strOutBuff; 

          DisplayStatusMsg("Checking Association...", false);
          if (IECSignOnModeType == (int)IECLayerInterface.IECSignOnMode._IEC_PRGRAMING) SetCommandProperties("ProgrammingAssociation");   
          else if (IECSignOnModeType == (int)IECLayerInterface.IECSignOnMode._IEC_MANUFACURER) SetCommandProperties("ManufacurerReadAssociation");
          else SetCommandProperties("IECReadoutAssociation");
          DisplayStatusMsg("Reading Data, Please Wait...", false);
          if (!IECCheckingAssociation()) { DisplayStatusMsg("Unable To Stablish Association!", true); return false; }
          MeterReadoutnResponse = GlobalObjects.objIECMeterSerialComm.strOutBuff; 
          if (IECSignOnModeType == (int)IECLayerInterface.IECSignOnMode._IEC_READ) return true;  //----For IEC Billing Read


          SetCommandProperties("AccessAssociation");
          DisplayStatusMsg("Stablizing Association...", false);

            GlobalObjects.objIECMeterSerialComm.TeaAlgorithm(GlobalObjects.objIECMeterSerialComm.strOutBuff);
          
          if (!ReadMeterData()) { DisplayStatusMsg("Unable To Stablish Association!", true); return false; }
          DisplayStatusMsg("Data Transferring Please Wait...", false);
          return true;

      }
     
      public bool IECPhysicalLayerConnect(bool isIECSettings)
      {

          try
          {
               GlobalObjects.objIECMeterSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, SerialPortSettings.Default.CommandBaudRate, SerialPortSettings.Default.Parity, SerialPortSettings.Default.DataBits, SerialPortSettings.Default.StopBits, SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay, SerialPortSettings.Default.SignOnBaudRate);
              if (GlobalObjects.objIECMeterSerialComm.OpenPort()) return true;
              else return false;
          }
          catch (Exception)
          {
              return false;
          }
      }
      public string ReadDataBuffer(string TabgoLable)
      {
          try
          {
              DisplayStatusMsg("Reading " + TabgoLable + " ...", false);
              SetCommandProperties(TabgoLable);
              if (!ReadMeterData()) { MeterSignonResponse ="Error:" +  GlobalObjects.objIECMeterSerialComm.strOutBuff; DisplayStatusMsg("Communication Failed!", true); }
              else MeterSignonResponse = GlobalObjects.objIECMeterSerialComm.strOutBuff;
              return MeterSignonResponse;
          }
          catch (Exception)
          {
              return "";
          }
      }

      public string ReadProfileBuffer(string TabgoLable,int noOfEvents)
      {
          try
          {
              string MeterreadoutResponse = "";
              int packetCount = 1;
              SetCommandProperties(TabgoLable);
              char[] daysBytes =  noOfEvents.ToString("00").ToCharArray();
              byte[] Databytes = GlobalObjects.objIECMeterSerialComm._CommandDataBytes;

              if (TabgoLable.Contains("Tamper") || TabgoLable.Contains("DailyProfileCommand") || TabgoLable.Contains("LoadProfileCommand"))
              {
                  int tempindex = 2;
                  int tempParalen = 0;
                  while (tempParalen < daysBytes.Length) { Databytes[tempindex++] = (byte)daysBytes[tempParalen++]; }
                  //Databytes[2] = (byte)daysBytes[0];
                  //Databytes[3] = (byte)daysBytes[1];
                  GlobalObjects.objIECMeterSerialComm._CommandDataBytes = Databytes;
              }
              else GlobalObjects.objIECMeterSerialComm._CommandDataBytes = Databytes;
              
              DisplayStatusMsg("Reading " + TabgoLable + " ...", false);
              string BufferData="";             
              do
              {
                  DisplayStatusMsg(TabgoLable + "   -    Reading Packet : " + packetCount++.ToString() + " ...", false);
                  if (!ReadMeterData())  {  DisplayStatusMsg("Reading Tamper Profile Failed!", true); break;  }
                  else MeterreadoutResponse = GlobalObjects.objIECMeterSerialComm.strOutBuff;
                  Thread.Sleep(50);
                  BufferData += ExtractDataFromResponse(MeterreadoutResponse);                  
                  SetCommandProperties("ACKCommand");                 
              } while (GlobalObjects.objIECMeterSerialComm.ReceiveBuffer.ToList().Contains(0x04)); //-----Break on Last Packet response 0x03
              return   BufferData  ;
          }
          catch (Exception)
          {
              return "";
          }
      }

      public string WriteProfileBuffer(string[] WriteCommandLabel,List<string> commandDataByte)
      {
          try
          {
              string MeterreadoutResponse = "";
              int packetCount = 0;              
              while (WriteCommandLabel.Length > packetCount)
              {
                  byte[] tempDatabytes = commandDataByte[packetCount].Split('.').Select(s => Convert.ToByte(s, 16)).ToArray();
                  DisplayStatusMsg("Writing " + WriteCommandLabel[packetCount] + " ...", false);
                  SetCommandProperties(WriteCommandLabel[packetCount]);
                  List<byte> Databytes = GlobalObjects.objIECMeterSerialComm._CommandDataBytes.ToList();
                  int startIdx = Databytes.IndexOf(0x28);
                  Databytes.InsertRange(startIdx + 1, tempDatabytes);
                  GlobalObjects.objIECMeterSerialComm._CommandDataBytes = Databytes.ToArray();

                  DisplayStatusMsg("Writing " + WriteCommandLabel[packetCount] + " ...", false);
                  if (!ReadMeterData()) { DisplayStatusMsg("Writing " + WriteCommandLabel[packetCount] + " Profile Failed!", true); return "Error: " + GlobalObjects.objIECMeterSerialComm.strOutBuff; }
                  else MeterreadoutResponse = GlobalObjects.objIECMeterSerialComm.strOutBuff;
                  packetCount++;
              } 
              return "";
          }
          catch (Exception Ex)
          {
              return "Error: " +  Ex.Message;
          }
      }

      public string WriteIECBuffer(List<string> commandDataByte)
      {
          try
          {
              string MeterreadoutResponse = "";
              int packetCount = 0;
              
                  byte[] tempDatabytes = commandDataByte[packetCount].Split('.').Select(s => Convert.ToByte(s, 16)).ToArray();
                  GlobalObjects.objIECMeterSerialComm._CommandDataBytes = tempDatabytes;
                  GlobalObjects.objIECMeterSerialComm._CommandResponseStopByte =  0x06;
                  GlobalObjects.objIECMeterSerialComm._2NdCommandResponseStopByte = 0x03;
               

                 // DisplayStatusMsg("Writing " + WriteCommandLabel[packetCount] + " ...", false);
                  if (!ReadMeterData()) { return "Error: " + GlobalObjects.objIECMeterSerialComm.strOutBuff; }
                  else MeterreadoutResponse = GlobalObjects.objIECMeterSerialComm.strOutBuff;
                  packetCount++;
                  return GlobalObjects.objIECMeterSerialComm.strOutBuff;
          }
          catch (Exception Ex)
          {
              return Ex.ToString();
          }
      }

      public string WriteBootLoaderProfileBufferinitiate(string[] WriteCommandLabel, List<string> commandDataByte)
      {
          try
          {
              int length = WriteCommandLabel.Length - 1;
              string MeterreadoutResponse = "";
              int packetCount = 0;
              while (WriteCommandLabel.Length > packetCount)
              {
                  byte[] tempDatabytes = commandDataByte[packetCount].Split('.').Select(s => Convert.ToByte(s, 16)).ToArray();
                  DisplayStatusMsg("Writing " + WriteCommandLabel[packetCount] + " ...", false);
                  SetCommandProperties(WriteCommandLabel[0]);//---For Bootloader Specefic
                  // List<byte> Databytes = GlobalObjects.objIECMeterSerialComm._CommandDataBytes.ToList();
                  GlobalObjects.objIECMeterSerialComm._CommandDataBytes = tempDatabytes.ToArray();

                  DisplayStatusMsg("Writing " + WriteCommandLabel[packetCount] + " ... Total " + length + " Packets to Write.", false);
                  if (!WriteBootloader()) { DisplayStatusMsg("Writing " + WriteCommandLabel[packetCount] + " Profile Failed!", true); return "Error: " + GlobalObjects.objIECMeterSerialComm.strOutBuff; }
                  else if (packetCount == 0)
                  {
                      for (int i = 0; i <= 4; i++)
                      {
                          byte tembyte = GlobalObjects.objIECMeterSerialComm.ReceiveBuffer[i];
                          MeterreadoutResponse += (char)tembyte;

                      }
                      return MeterreadoutResponse;
                  }
                  else MeterreadoutResponse = GlobalObjects.objIECMeterSerialComm.ReceiveBuffer[0].ToString();// GlobalObjects.objIECMeterSerialComm.strOutBuff;
                  packetCount++;
              }
              return MeterreadoutResponse;
          }
          catch (Exception Ex)
          {
              return Ex.ToString();
          }
      }

      public string WriteBootLoaderProfileBuffer(string[] WriteCommandLabel, List<string> commandDataByte)
      {
          try
          {
              int length = WriteCommandLabel.Length - 1;
              string MeterreadoutResponse = "";
              int packetCount = 1;
              while (WriteCommandLabel.Length > packetCount)
              {
                  byte[] tempDatabytes = commandDataByte[packetCount].Split('.').Select(s => Convert.ToByte(s, 16)).ToArray();
                  DisplayStatusMsg("Writing " + WriteCommandLabel[packetCount] + " ...", false);
                  SetCommandProperties(WriteCommandLabel[0]);//---For Bootloader Specefic
                 // List<byte> Databytes = GlobalObjects.objIECMeterSerialComm._CommandDataBytes.ToList();
                  GlobalObjects.objIECMeterSerialComm._CommandDataBytes = tempDatabytes.ToArray();                 

                  DisplayStatusMsg("Writing " + WriteCommandLabel[packetCount] + " ... Total "+length+" Packets to Write." , false);
                  if (!WriteBootloader()) { DisplayStatusMsg("Writing " + WriteCommandLabel[packetCount] + " Profile Failed!", true); return "Error: " + GlobalObjects.objIECMeterSerialComm.strOutBuff; }
                 
                  else MeterreadoutResponse = GlobalObjects.objIECMeterSerialComm.ReceiveBuffer[0].ToString();// GlobalObjects.objIECMeterSerialComm.strOutBuff;
                  packetCount++;
              }
              return MeterreadoutResponse;
          }
          catch (Exception Ex)
          {
              return Ex.ToString();
          }
      }
      public string WriteNonProtocolPacket(List<byte> commandDataByte, byte CommandResponseStopByte, byte? CommandResponseStopByte2nd)
      {
          try
          {
                  int commadnWaitTime = 10;
                  GlobalObjects.objIECMeterSerialComm._CommandResponseStopByte = CommandResponseStopByte;
                  if (CommandResponseStopByte2nd != null) { commadnWaitTime = 100; GlobalObjects.objIECMeterSerialComm._2NdCommandResponseStopByte = (byte)CommandResponseStopByte2nd; }
                  GlobalObjects.objIECMeterSerialComm._CommandDataBytes = commandDataByte.ToArray();
                  WriteBootloader();
                  Thread.Sleep(commadnWaitTime);
                  return  GlobalObjects.objIECMeterSerialComm.strOutBuff;  
          }
          catch (Exception Ex)
          {
              return Ex.ToString();
          }
      }
      public string ExtractDataFromResponse(string MeterreadoutResponse)
      {
          const string regex = @"(\(([\w\W]*?)\))";
          MatchCollection matches = Regex.Matches(MeterreadoutResponse, regex, RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

          if (matches.Count >= 0) return matches[0].Value;
          else return "";
      }

      public bool ReadMeterData()
      {
          try
          {
              int retry=3;
              if (GlobalObjects.objIECMeterSerialComm._CommandDataBytes.Contains((byte)3) || GlobalObjects.objIECMeterSerialComm._CommandDataBytes.Contains((byte)4))
              {
                  byte bccByte = GlobalObjects.objIECMeterSerialComm.GetBcc(GlobalObjects.objIECMeterSerialComm._CommandDataBytes, 1, GlobalObjects.objIECMeterSerialComm._CommandDataBytes.Length-1);
                  GlobalObjects.objIECMeterSerialComm._CommandDataBytes[GlobalObjects.objIECMeterSerialComm._CommandDataBytes.Length - 1] = bccByte;
              }
              while(retry-- > 0)
              {
                  if (!GlobalObjects.objIECMeterSerialComm.fSendDataToPort())
                  {
                      if (GlobalObjects.objIECMeterSerialComm.strOutBuff.ToUpperInvariant().IndexOf("READY") >= 0) return true;//--For Bootloader Only
                      continue;
                  }
                  else if (GlobalObjects.objIECMeterSerialComm.strOutBuff.ToUpperInvariant().IndexOf("(ER") >= 0) { Thread.Sleep(200); continue; }

                return true;  
              }              
              return false; 
          }
          catch (Exception)
          {
               return false; 
          }

      }

      public bool WriteBootloader()
      {
          try
          {
              int retry = 1;              
              while (retry-- > 0)
              {
                  if (!GlobalObjects.objIECMeterSerialComm.fSendDataToPort()) return false;
                  return true;
              }
              return false;
          }
          catch (Exception)
          {
              return false;
          }

      }

      public bool IECHDLCLayerConnect(out string responseBytes)
      {
          try
          {             
              if (GlobalObjects.objIECMeterSerialComm.fSendDataToPort()) { responseBytes = GlobalObjects.objIECMeterSerialComm.strOutBuff; return true; }
              else { responseBytes = GlobalObjects.objIECSerialComm.strOutBuff; return false; }
          }
          catch (Exception)
          {
              { responseBytes = GlobalObjects.objIECSerialComm.strOutBuff; return false; }
          }

      }

      public bool IECCheckingAssociation()
      {
          try
          {
              
              if (GlobalObjects.objIECMeterSerialComm.ManfCommandAccess()) return true;
              else return false;
          }
          catch (Exception)
          {
              return false;
          }

      }

      public bool IECAssociationStablish()
      {
          try
          {
              GlobalObjects.objIECSerialComm.StopResponseByte = "\x06";
              if (GlobalObjects.objIECSerialComm.PasswordModeChecking(true)) return true;
              else return false;
          }
          catch (Exception)
          {
              return false;
          }

      }

      public void SetCommandProperties(string commandTango)
      {
          string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Configuration\" + "1PCommandRepository.xml";
          try
          {
              var xmlStr = File.ReadAllText(filePath);
              var str = XElement.Parse(xmlStr);
              var result = str.Elements("COMMAND").Where(x => x.Element("TAGNO").Value.Equals(commandTango)).ToList();            
              byte[] Databytes = result[0].Element("CommandDataBytes").Value.Split('.').Select(s => Convert.ToByte(s, 16)).ToArray();
              GlobalObjects.objIECMeterSerialComm._CommandDataBytes = Databytes;
              GlobalObjects.objIECMeterSerialComm._CommandResponseStopByte = (byte)Convert.ToInt32(result[0].Element("ResponseStopByte").Value,16);
              if (result[0].Element("ResponseStopByte_2").Value != "") GlobalObjects.objIECMeterSerialComm._2NdCommandResponseStopByte = (byte)Convert.ToInt32(result[0].Element("ResponseStopByte_2").Value, 16);
              else GlobalObjects.objIECMeterSerialComm._2NdCommandResponseStopByte = (byte)Convert.ToInt32(result[0].Element("ResponseStopByte").Value, 16);

          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public void IECAssociationDisconnect()
      {
          try
          {
               
              SetCommandProperties("DisconnectAssociation");
              if (!ReadMeterData()) { DisplayStatusMsg("Unable To Disconnect Meter Association !", true); }             
              GlobalObjects.objIECMeterSerialComm.ClosePort();
              return;

          }
          catch (Exception)
          {

          }

      }
      public void IECPortDisconnect()
      {
          try
          {

              GlobalObjects.objIECMeterSerialComm.ClosePort();
              return;

          }
          catch (Exception)
          {

          }

      }

      //---------------------------------------------------------------------------------------------
     /* public bool ConnectToMeter()
      {
          string signonResponse = "";
          DisplayStatusMsg("Connecting...", false);
          if (!PhysicalLayerConnect(false)) { DisplayStatusMsg("Connecting Failed!", true); return false; }
          DisplayStatusMsg("Signon...", false);
          if (!HDLCLayerConnect(out signonResponse)) { MeterSignonResponse = signonResponse; DisplayStatusMsg("Signon Failed!", true); return false; }
          else MeterSignonResponse = signonResponse;
          DisplayStatusMsg("Checking Association...", false);
          if (!CheckingAssociation()) { DisplayStatusMsg("Unable To Stablish Association!", true); return false; }
          DisplayStatusMsg("Stablizing Association...", false);
          if (!AssociationStablish()) { DisplayStatusMsg("Unable To Stablish Association!", true); return false; }
          DisplayStatusMsg("Data Transferring Please Wait...", false);
          return true;

      }*/

      public bool ConnectToRefMeter(string COMMPORT)
      {
          string signonResponse ="";
          if (!PhysicalLayerConnectRefMeter(COMMPORT)) { return false; }
          if (!HDLCLayerConnect(out signonResponse)) { return false; }
          return true;

      }

      public bool PhysicalLayerConnect(bool isIECSettings)
      {
          try
          {
              if (isIECSettings) GlobalObjects.objIECSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, SerialPortSettings.Default.CommandBaudRate, SerialPortSettings.Default.Parity, SerialPortSettings.Default.DataBits, SerialPortSettings.Default.StopBits, SerialPortSettings.Default.IntercharacterDelay, 200, SerialPortSettings.Default.BaudRateSelectedIndex);
              else GlobalObjects.objIECSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, SerialPortSettings.Default.SignOnBaudRate, SerialPortSettings.Default.Parity, SerialPortSettings.Default.DataBits, SerialPortSettings.Default.StopBits, SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.BaudRateSelectedIndex);
              if (GlobalObjects.objIECSerialComm.OpenPort()) return true;
              else return false;

          }
          catch (Exception)
          {
              return false;
          }

      }

      public bool PhysicalLayerConnectRefMeter(string COMMPORT)
      {
          try
          {
              //-------------------Parity EVEN-----------------------------
              GlobalObjects.objIECSerialComm.SetSerialPortSettings(COMMPORT, "9600", "Even", "8", "1", 5000, 2500, 5);
              if (GlobalObjects.objIECSerialComm.OpenPort()) return true;
              else return false;

          }
          catch (Exception)
          {
              return false;
          }

      }
      public bool PhysicalLayerConnectRFMeter(string COMMPORT)
      {
          try
          {
              //-------------------Parity NONE-----------------------------
              GlobalObjects.objIECSerialComm.SetSerialPortSettings(COMMPORT, "9600", "None", "8", "1", 5000, 2500, 5);
              if (GlobalObjects.objIECSerialComm.OpenPort()) return true;
              else return false;

          }
          catch (Exception)
          {
              return false;
          }

      }

      public bool HDLCLayerConnect(out string responseBytes)
      {
          try
          {
              GlobalObjects.objIECSerialComm.StopResponseByte = "\r\n";
              if (GlobalObjects.objIECSerialComm.SignOn()) { responseBytes = GlobalObjects.objIECSerialComm.strOutBuff; return true; }
              else {responseBytes = GlobalObjects.objIECSerialComm.strOutBuff;  return false;}
          }
          catch (Exception)
          {
              { responseBytes = GlobalObjects.objIECSerialComm.strOutBuff; return false; }
          }

      }

      public bool CheckingAssociation()
      {
          try
          {
              GlobalObjects.objIECSerialComm.StopResponseByte = "\x03";   
              if (GlobalObjects.objIECSerialComm.ManfCommand(SerialPortSettings.Default.CommandBaudRate)) return true;
              else return false;
          }
          catch (Exception)
          {
              return false;
          }

      }

      public bool AssociationStablish()
      {
          try
          {
              GlobalObjects.objIECSerialComm.StopResponseByte = "\x06";             
              if (GlobalObjects.objIECSerialComm.PasswordModeChecking(true)) return true;
              else return false;
          }
          catch (Exception)
          {
              return false;
          }

      }
      
      public string WriteDataToMeter(string CommandHexString,string stopByte)
      {
          try
          {

              GlobalObjects.objIECSerialComm.StopResponseByte = stopByte; //"\x06";
              return GlobalObjects.objIECSerialComm.SendDataToMeterandGetResponse(CommandHexString);
              
          }
          catch (Exception)
          {
              return "";
          }

      }
      public List<byte> WriteBytesToMeter(byte[] RequestCommand,int bytetoRead,int CommandIDX)
      {
          try
          {
              return GlobalObjects.objIECSerialComm.ReadByteResponse(RequestCommand, bytetoRead, CommandIDX);

          }
          catch (Exception)
          {
              return null;
          }

      }
      public string GetCalculatedBCC(string CommandHexString)
      {
          try
          {
              return GlobalObjects.objIECSerialComm.CalBcc(CommandHexString);

          }
          catch (Exception)
          {
              return "";
          }

      }

      public string GetStrToHexCmd(string CommandHexString)
      {
          try
          {
              return GlobalObjects.objIECSerialComm.StrToHexCmd(CommandHexString);

          }
          catch (Exception)
          {
              return "";
          }

      }

      public string WriteOTADataToMeter(string CommandHexString)
      {
          try
          {

              GlobalObjects.objIECSerialComm.StopResponseByte = "\x06";
              return GlobalObjects.objIECSerialComm.SendMotFileDataToMeter(CommandHexString);

          }
          catch (Exception)
          {
              return "";
          }

      }

      public void AssociationDisconnect()
      {
          try
          {
              GlobalObjects.objIECSerialComm.Command = GlobalObjects.objIECSerialComm._BreakCommand;
              if (GlobalObjects.objIECSerialComm.SendCommand()) { Thread.Sleep(200); }
              GlobalObjects.objIECSerialComm.ClosePort();
              return;
             
          }
          catch (Exception)
          {
             
          }

      }

      public void PortDisconnect()
      {
          try
          {
              GlobalObjects.objIECSerialComm.ClosePort();
              return;

          }
          catch (Exception)
          {

          }

      }
  }

  public class IrDALayerInterface
  {
      int HDLCIndex = 0;
      public delegate void UpdateHandler(object sender, UpdateEventArgs e);
      public event UpdateHandler UpdatedLed;
      UpdateEventArgs args = null;
      public int errormsgStstus = 0;
      byte[] HDLCCommand = new byte[1024];
      public int getWriteResponseCode = 0;
      public string AppDirectoryLocalPath = AppDomain.CurrentDomain.BaseDirectory + "\\Configuration";
      public string MeterInfoValue = "";

      #region Enums

      public enum ProgrammingCode
      {
          Success,
          Fail,
          AccessDenied,
          DataUnavailable,
          TimeOut,
          SignOnFailed,
          CosemConnectionFailed,
          MeterIDMismatch

      }

      public enum MeterTypeInfo { Smart_Meter_1PH = 0, MicroStar_DLMS = 1, Smart_Meter_3PH = 2, DLMS_3PH = 3, SAPPHIRE = 4, DLMS_3PH_RUBY = 5, Non_DLMS_1PH = 6 };

      public enum IrDACommandType {InitiationCommand = 0x96, BillingDataCommand =0x00 , ClosingCommand = 0x9E, MeterSerialNo = 0x41};
      #endregion

      public List<string> GetMeterTypeList()
      {
          List<string> meterTypeList = new List<string>();
          meterTypeList.Add("1Phase-Smart Meter");
          meterTypeList.Add("1Phase -DLMS");
          meterTypeList.Add("3Phase-Smart Meter");
          meterTypeList.Add("3Phase-DLMS-PUMA");
          meterTypeList.Add("3Phase-Sapphire");
          meterTypeList.Add("3Phase-RUBY");
          meterTypeList.Add("1Phase-NON-DLMS");
          return meterTypeList;
      }

      public string GetSelectedMeterType()
      {
          AppSettings objappSettings = new AppSettings();
          List<string> meterTypelist = GetMeterTypeList();
          return meterTypelist[objappSettings.GetMeterMode()];

      }

      public void DisplayStatusMsg(string msgString, bool isError)
      {
          try
          {
              args = new UpdateEventArgs(msgString, isError);
              UpdatedLed(this, args);
          }
          catch (Exception)
          {
          }
      }

      public bool ConnectToIrDAMeters()
      {
          MeterInfoValue = string.Empty;
          AppSettings objappSettings = new AppSettings();
          DisplayStatusMsg("  Physical Layer Communication...", false);
          if (!PhysicalLayerConnect()) { DisplayStatusMsg("Physical Layer Connection Failed!", true); return false; }
          DisplayStatusMsg("Device Is Connected, Please Wait...", false);
          return true;
      }

      public bool PhysicalLayerConnect()
      {
          try
          {
              GlobalObjects.objSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, SerialPortSettings.Default.CommandBaudRate, "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
              if (GlobalObjects.objSerialComm.OpenPort()) return true;
              else return false;

          }
          catch (Exception)
          {
              return false;
          }

      }
      
      public bool AssociationDisconnect()
      {
          try
          {
              PhysicalLayerDisconnect();
              return true;
          }
          catch (Exception)
          {
              DisplayStatusMsg("Unable To Close Current Association!", true);
              return false;

          }
           
      }

      public void PhysicalLayerDisconnect()
      {
          try
          {
              GlobalObjects.objSerialComm.ClosePort();
              return;
          }
          catch (Exception)
          {
              return;
          }
      }

      public bool ReadIrDAByteFromMeter(byte IrDAReadCommandType, int IrDAMeterid, int IrDAhhuID, string CommandData)
      {
          int writeResponse = ReadDIrDAataCommand(IrDAReadCommandType, IrDAMeterid, IrDAhhuID, CommandData);
          getWriteResponseCode = writeResponse;
          if (writeResponse == (int)ProgrammingCode.Success) { /*DisplayStatusMsg("Reading Succesfull.", false);*/ return true; }
          else if (writeResponse == (int)ProgrammingCode.AccessDenied) { DisplayStatusMsg("Access Denied!", true);/* MessageBox.Show("Access Denied!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);*/ return false; }
          else if (writeResponse == (int)ProgrammingCode.DataUnavailable) { DisplayStatusMsg("Data Not Available!", true); MessageBox.Show("Data Not Available!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1); return false; }
          else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed) { DisplayStatusMsg("Cosem Connection Failed!", true); MessageBox.Show("Cosem Connection Failed!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); return false; }
          else { DisplayStatusMsg("Communication Failed!", true); return false; }
      }

      public bool fChecIrDAResponse(byte[] Buffer, int IrDAMeterid, int IrDAhhuID, byte IrDAReadCommandbyte)
      {
          try
          {
              if (!GlobalObjects.objHDLCLIB.IrDACheckBCC(Buffer)) { DisplayStatusMsg("   BCC Not Match", false); return false; }
              if (!GlobalObjects.objHDLCLIB.IrDACheckSyncWord(Buffer)) { DisplayStatusMsg("   Invalid IrDA Sync Word", false); return false; }
              //Byte Position offset is 2,3,4
              if (Convert.ToInt32(DLMSDataStracture.HexToDecimalConversion(Buffer[4].ToString("X2") + Buffer[3].ToString("X2") + Buffer[2].ToString("X2"))) != IrDAhhuID) { DisplayStatusMsg("  Invalid Source ID Received", false); return false; }
               //if (DLMSDataStracture.HexToDecimalConversion(Buffer[4] + Buffer[3] + Buffer[2]) !GlobalObjects.objHDLCLIB.IrDACheckHHUIP(Buffer, IrDAhhuID)) { DisplayStatusMsg("  Invalid Source ID Received", false); return false; }
              if (!GlobalObjects.objHDLCLIB.IrDACheckCommandID(Buffer, IrDAReadCommandbyte)) { DisplayStatusMsg("  Invalid Command Received", false); return false; }
              //Byte Position offset is 7,8,9
              int resMeterID = Convert.ToInt32(DLMSDataStracture.HexToDecimalConversion(Buffer[9].ToString("X2") + Buffer[8].ToString("X2") + Buffer[7].ToString("X2")));
              if (resMeterID <= 0) { DisplayStatusMsg("  Invalid Destination ID Received", false); return false; }
              return true;
          }
          catch (Exception)
          {
              DisplayStatusMsg("   Invalid Data", false);
              return false;
          }
      }

      private int ReadDIrDAataCommand(byte IrDAReadCommandbyte, int IrDAMeterid, int IrDAhhuID, string CommandData)
      {
          try
          {
              HDLCIndex = 0;
              HDLCCommand[HDLCIndex++] = 0x95;
              HDLCCommand[HDLCIndex++] = 0x95;
              //---------------------------Meter IP---------------------------------
              HDLCCommand[HDLCIndex++] = Convert.ToByte((IrDAMeterid & 0xFF0000) >> 16);
              HDLCCommand[HDLCIndex++] = Convert.ToByte((IrDAMeterid & 0xFF00) >> 8);
              HDLCCommand[HDLCIndex++] = Convert.ToByte(IrDAMeterid & 0x00FF);
              //-----------Pay load Byte, Length of command data----------------------
              HDLCCommand[HDLCIndex++] = 0x00;// Convert.ToByte(CommandData.Length);
              //-----------Command Type----------------------
              HDLCCommand[HDLCIndex++] = IrDAReadCommandbyte;
              //---------------------------HHU IP---------------------------------
              HDLCCommand[HDLCIndex++] = Convert.ToByte((IrDAhhuID & 0xFF0000) >> 16);
              HDLCCommand[HDLCIndex++] = Convert.ToByte((IrDAhhuID & 0xFF00) >> 8);
              HDLCCommand[HDLCIndex++] = Convert.ToByte(IrDAhhuID & 0x00FF);
              byte bccByte = 0x00;
              if (CommandData.Length > 0) 
              {
                  int datacount = 0;
                  while (datacount < CommandData.Length)
                  {
                      HDLCCommand[HDLCIndex++] = Convert.ToByte(CommandData.Substring(datacount,2));
                      datacount += 2;
                  }

              }
              //---------------Calculate BCC---------------
               //-----No Data Command required so no need to calculate BCC for send command
              //-------------------------------------------
              HDLCCommand[HDLCIndex++] = bccByte; //---BCC
              HDLCCommand[5] = Convert.ToByte(HDLCIndex);
              if (!GlobalObjects.objSerialComm.fSendIrDADataToPort(HDLCCommand, (byte)HDLCIndex))
              {
                  if (IrDAReadCommandbyte == (byte)IrDALayerInterface.IrDACommandType.ClosingCommand) return (int)ProgrammingCode.Success;//-----For Closing Command No response will come from meter 
                  return (int)ProgrammingCode.CosemConnectionFailed;
              }
              if (!fChecIrDAResponse(GlobalObjects.objSerialComm.ReceiveBuffer, IrDAMeterid, IrDAhhuID, IrDAReadCommandbyte)) return (int)ProgrammingCode.CosemConnectionFailed;
              else return (int)ProgrammingCode.Success;
              
          }
          catch (Exception)
          {
              return (int)ProgrammingCode.CosemConnectionFailed;
          }
      }

      
      //-----------------------1P IrDA-------------------------------------------------
      public bool Read1P_IrDAByteFromMeter(byte[] IrDAReadCommand)
      {
          int writeResponse = Read1P_DIrDAataCommand(IrDAReadCommand);
          getWriteResponseCode = writeResponse;
          if (writeResponse == (int)ProgrammingCode.Success) { /*DisplayStatusMsg("Reading Succesfull.", false);*/ return true; }
          else if (writeResponse == (int)ProgrammingCode.AccessDenied) { DisplayStatusMsg("Access Denied!", true);/* MessageBox.Show("Access Denied!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);*/ return false; }
          else if (writeResponse == (int)ProgrammingCode.DataUnavailable) { DisplayStatusMsg("Data Not Available!", true); MessageBox.Show("Data Not Available!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1); return false; }
          else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed) { DisplayStatusMsg("Cosem Connection Failed!", true); MessageBox.Show("Cosem Connection Failed!", "L+G", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); return false; }
          else { DisplayStatusMsg("Communication Failed!", true); return false; }
      }

      public bool fChec1P_IrDAResponse(byte[] Buffer)
      {
          try
          {
              int getlen = GlobalObjects.objSerialComm.ASCIIHexToDecimalConversion(Buffer, 1, 2);
              if (!GlobalObjects.objHDLCLIB.IrDACheckBCC_1P(Buffer, getlen)) { DisplayStatusMsg("   BCC Not Match", false); return false; }
              if (!GlobalObjects.objHDLCLIB.IrDACheckSyncWord_1P(Buffer, getlen)) { DisplayStatusMsg("   Invalid IrDA Fram", false); return false; }
              return true;
          }
          catch (Exception)
          {
              DisplayStatusMsg("   Invalid Data", false);
              return false;
          }
      }

      private int Read1P_DIrDAataCommand(byte[] IrDAReadCommand)
      {
          try
          {
              Thread.Sleep(500);
              if (!GlobalObjects.objSerialComm.fSendIrDADataToPort_1P(IrDAReadCommand, (byte)IrDAReadCommand.Length)) return (int)ProgrammingCode.CosemConnectionFailed;

              if (!fChec1P_IrDAResponse(GlobalObjects.objSerialComm.ReceiveBuffer)) return (int)ProgrammingCode.CosemConnectionFailed;
              else return (int)ProgrammingCode.Success;

          }
          catch (Exception)
          {
              return (int)ProgrammingCode.CosemConnectionFailed;
          }
      }


  }

  public class AppSettings
  { 
                
      public bool SaveSettings(List<string> valueList)
      {
        int ValueIDX=0;
        SerialPortSettings.Default.SerialPort = valueList[ValueIDX++];
        SerialPortSettings.Default.Parity = valueList[ValueIDX++];
        SerialPortSettings.Default.DataBits = valueList[ValueIDX++];
        SerialPortSettings.Default.StopBits = valueList[ValueIDX++];
        SerialPortSettings.Default.CommandBaudRate = valueList[ValueIDX++];
        SerialPortSettings.Default.IntercharacterDelay = Convert.ToInt32(valueList[ValueIDX++]);
        SerialPortSettings.Default.CommandTimeOut = Convert.ToInt32(valueList[ValueIDX++]);

        SerialPortSettings.Default.InformationSize = Convert.ToInt32(valueList[ValueIDX++]);
        SerialPortSettings.Default.WindowSize = Convert.ToInt32(valueList[ValueIDX++]);
        SerialPortSettings.Default.AddressingSchem = Convert.ToInt32(valueList[ValueIDX++]);
        SerialPortSettings.Default.ServerPhysicalID = Convert.ToInt32(valueList[ValueIDX++]);
        SerialPortSettings.Default.CosemBufferSize = Convert.ToInt32(valueList[ValueIDX++]);
        SerialPortSettings.Default.DLLBufferSize = Convert.ToInt32(valueList[ValueIDX++]);

        SerialPortSettings.Default.BaudRateSelectedIndex = Convert.ToInt32(valueList[ValueIDX++]);

        SerialPortSettings.Default.ClientSAP = Convert.ToInt32(valueList[ValueIDX++]);

        SerialPortSettings.Default.ServerSAP = Convert.ToInt32(valueList[ValueIDX++]);
        SerialPortSettings.Default.ServerLowerMacAddress = Convert.ToInt32(valueList[ValueIDX++]);
        SerialPortSettings.Default.DLMSVersion = Convert.ToInt32(valueList[ValueIDX++]);
        SerialPortSettings.Default.ApplicationContext = (byte)Convert.ToInt32(valueList[ValueIDX++]);

        SerialPortSettings.Default.SecurityMechanism = (byte)Convert.ToInt32(valueList[ValueIDX++]);

        if (valueList[ValueIDX] != "") SerialPortSettings.Default.Password = valueList[ValueIDX++];
        else ValueIDX++;
        if (valueList[ValueIDX] != "") SerialPortSettings.Default.HLSPWD = valueList[ValueIDX++];
        else ValueIDX++;
        SerialPortSettings.Default.PDUSize = Convert.ToInt32(valueList[ValueIDX++]);               
        SerialPortSettings.Default.ConformanceBlock = valueList[ValueIDX++];
        SerialPortSettings.Default.ScaleXMLPath = valueList[ValueIDX++]; //Set as App Default Path
        SerialPortSettings.Default.SignOnBaudRate = valueList[ValueIDX++];
      //  SerialPortSettings.Default.AESEncryption  = valueList[ValueIDX++];// for Smart meter
        SerialPortSettings.Default.ClientSystemTitle = valueList[ValueIDX++];// for Smart meter

        SerialPortSettings.Default.Securitysuit = Convert.ToInt32(valueList[ValueIDX++]);//security suit for Smart meter 
        SerialPortSettings.Default.GlobalEncryptionKey = valueList[ValueIDX++];//Encryption Key
        SerialPortSettings.Default.DedicatedKey = Convert.ToInt16(valueList[ValueIDX++]);//Dedicated Key
        SerialPortSettings.Default.AuthenticationKey = valueList[ValueIDX++];//Authentication Key
        SerialPortSettings.Default.Save();
        return true;
      }

      public bool SaveSettingsByIndex(string[] valueList)
      {
          int ValueIDX = 0;
          SerialPortSettings.Default.SerialPort = valueList[ValueIDX++];
          SerialPortSettings.Default.Parity = valueList[ValueIDX++];
          SerialPortSettings.Default.DataBits = valueList[ValueIDX++];
          SerialPortSettings.Default.StopBits = valueList[ValueIDX++];
          SerialPortSettings.Default.CommandBaudRate = valueList[ValueIDX++];
          SerialPortSettings.Default.IntercharacterDelay = Convert.ToInt32(valueList[ValueIDX++]);
          SerialPortSettings.Default.CommandTimeOut = Convert.ToInt32(valueList[ValueIDX++]);

          SerialPortSettings.Default.InformationSize = Convert.ToInt32(valueList[ValueIDX++]);
          SerialPortSettings.Default.WindowSize = Convert.ToInt32(valueList[ValueIDX++]);
          SerialPortSettings.Default.AddressingSchem = Convert.ToInt32(valueList[ValueIDX++]);
          SerialPortSettings.Default.ServerPhysicalID = Convert.ToInt32(valueList[ValueIDX++]);
          SerialPortSettings.Default.CosemBufferSize = Convert.ToInt32(valueList[ValueIDX++]);
          SerialPortSettings.Default.DLLBufferSize = Convert.ToInt32(valueList[ValueIDX++]);

          SerialPortSettings.Default.BaudRateSelectedIndex = Convert.ToInt32(valueList[ValueIDX++]);

          SerialPortSettings.Default.ClientSAP = Convert.ToInt32(valueList[ValueIDX++]);

          SerialPortSettings.Default.ServerSAP = Convert.ToInt32(valueList[ValueIDX++]);
          SerialPortSettings.Default.ServerLowerMacAddress = Convert.ToInt32(valueList[ValueIDX++]);
          SerialPortSettings.Default.DLMSVersion = Convert.ToInt32(valueList[ValueIDX++]);
          SerialPortSettings.Default.ApplicationContext = (byte)Convert.ToInt32(valueList[ValueIDX++]);

          SerialPortSettings.Default.SecurityMechanism = (byte)Convert.ToInt32(valueList[ValueIDX++]);

          if (valueList[ValueIDX] != "") SerialPortSettings.Default.Password = valueList[ValueIDX++];
          else ValueIDX++;
          if (valueList[ValueIDX] != "") SerialPortSettings.Default.HLSPWD = valueList[ValueIDX++];
          else ValueIDX++;
          SerialPortSettings.Default.PDUSize = Convert.ToInt32(valueList[ValueIDX++]);
          SerialPortSettings.Default.ConformanceBlock = valueList[ValueIDX++];
          SerialPortSettings.Default.ScaleXMLPath = valueList[ValueIDX++]; //Set as App Default Path
          SerialPortSettings.Default.SignOnBaudRate = valueList[ValueIDX++];
          //  SerialPortSettings.Default.AESEncryption  = valueList[ValueIDX++];// for Smart meter
          SerialPortSettings.Default.ClientSystemTitle = valueList[ValueIDX++];// for Smart meter

          SerialPortSettings.Default.Securitysuit = Convert.ToInt32(valueList[ValueIDX++]);//security suit for Smart meter 
          SerialPortSettings.Default.GlobalEncryptionKey = valueList[ValueIDX++];//Encryption Key
          SerialPortSettings.Default.DedicatedKey = Convert.ToInt16(valueList[ValueIDX++]);//Dedicated Key
          SerialPortSettings.Default.AuthenticationKey = valueList[ValueIDX++];//Authentication Key
          SerialPortSettings.Default.Save();
          return true;
      }

     

      public void SetSecurityMachanism(byte smsm)
      {
          SerialPortSettings.Default.SecurityMechanism = smsm;
          SerialPortSettings.Default.Save();
          return;
      }

      public byte GetSecurityMachanism()
      {
          return SerialPortSettings.Default.SecurityMechanism;
          
      }

      public void SetApplicationContext(byte smsm)
      {
          SerialPortSettings.Default.ApplicationContext = smsm;
          SerialPortSettings.Default.Save();
          return;
      }

      public byte GetApplicationContext()
      {
          return SerialPortSettings.Default.ApplicationContext;

      }

      public List<string> GetSettings()
      {
            List<string> valueList = new List<string>();
          
            valueList.Add(SerialPortSettings.Default.SerialPort);
            valueList.Add(SerialPortSettings.Default.Parity);
            valueList.Add(SerialPortSettings.Default.DataBits);
            valueList.Add(SerialPortSettings.Default.StopBits);
            valueList.Add(SerialPortSettings.Default.CommandBaudRate);
            valueList.Add(SerialPortSettings.Default.IntercharacterDelay.ToString());
            valueList.Add(SerialPortSettings.Default.CommandTimeOut.ToString());

            valueList.Add(SerialPortSettings.Default.InformationSize.ToString());
            valueList.Add(SerialPortSettings.Default.WindowSize.ToString());
            valueList.Add(SerialPortSettings.Default.AddressingSchem.ToString());
            valueList.Add(SerialPortSettings.Default.ServerPhysicalID.ToString());
            valueList.Add(SerialPortSettings.Default.CosemBufferSize.ToString());
            valueList.Add(SerialPortSettings.Default.DLLBufferSize.ToString());

            valueList.Add(SerialPortSettings.Default.BaudRateSelectedIndex.ToString());

            valueList.Add(SerialPortSettings.Default.ClientSAP.ToString());

            valueList.Add(SerialPortSettings.Default.ServerSAP.ToString());
            valueList.Add(SerialPortSettings.Default.ServerLowerMacAddress.ToString());
            valueList.Add(SerialPortSettings.Default.DLMSVersion.ToString());
            valueList.Add(SerialPortSettings.Default.ApplicationContext.ToString());

            valueList.Add(SerialPortSettings.Default.SecurityMechanism.ToString());

            valueList.Add(SerialPortSettings.Default.Password);

            valueList.Add(SerialPortSettings.Default.HLSPWD);

            valueList.Add(SerialPortSettings.Default.PDUSize.ToString());
            valueList.Add(SerialPortSettings.Default.ConformanceBlock);
            valueList.Add(SerialPortSettings.Default.ScaleXMLPath );
            valueList.Add(SerialPortSettings.Default.SignOnBaudRate);
           // valueList.Add(SerialPortSettings.Default.AESEncryption.ToString());
            valueList.Add(SerialPortSettings.Default.ClientSystemTitle.ToString()); 
            valueList.Add(SerialPortSettings.Default.Securitysuit.ToString());//Security soot
            valueList.Add(SerialPortSettings.Default.GlobalEncryptionKey);
           // valueList.Add(SerialPortSettings.Default.AAD);
            valueList.Add(SerialPortSettings.Default.DedicatedKey.ToString());
            valueList.Add(SerialPortSettings.Default.AuthenticationKey.ToString());
          
          return valueList;
      }

      public string GetPortName()
      {
          return SerialPortSettings.Default.SerialPort;
      }

      public void SetPortName(string SPortName)
      {
           SerialPortSettings.Default.SerialPort = SPortName;
           SerialPortSettings.Default.Save();
      }

      public string GetBaudRate()
      {
          return SerialPortSettings.Default.CommandBaudRate;
      }

      public string GetSignonBaudRate()
      {
          return SerialPortSettings.Default.SignOnBaudRate;
      }

      public string GetParity()
      {
          return SerialPortSettings.Default.Parity;
      }

      public string GetDatabits()
      {
          return SerialPortSettings.Default.DataBits;
      }

      public string GetStopBits()
      {
          return SerialPortSettings.Default.StopBits;
      }

      public string GetClientSAP()
      {
          return SerialPortSettings.Default.ClientSAP.ToString();
      }

      public void SetClientSAP(int clintIP)
      {
          SerialPortSettings.Default.ClientSAP = clintIP;
          SerialPortSettings.Default.Save();
          
      }

      public string GetScaleXMLPath()
      {
          return SerialPortSettings.Default.ScaleXMLPath.ToString();
          
      }

      public void SetScaleXMLPath(string xmlPath)
      {
          SerialPortSettings.Default.ScaleXMLPath = xmlPath;
          SerialPortSettings.Default.Save();
      }

      public void SetMeterMode(int selectedMeterMode)
      {
          SerialPortSettings.Default.MeterMode = selectedMeterMode;
          SerialPortSettings.Default.Save();
      }

      public int GetMeterMode()
      {
          return SerialPortSettings.Default.MeterMode;
          
      }

      public string GetAppUser()
      {
          return SerialPortSettings.Default.AppUser;

      }

      public void SetAppUser(string appUserName)
      {
          SerialPortSettings.Default.AppUser = appUserName;
          SerialPortSettings.Default.Save();

      }

      public string GetApppwd()
      {
          return SerialPortSettings.Default.AppPwd;

      }

      public void SetApppwd(string apppwd)
      {
          SerialPortSettings.Default.AppPwd = apppwd;
          SerialPortSettings.Default.Save();

      }

      public bool GetAppUserRememberMe()
      {
          return SerialPortSettings.Default.AppUserRememberMe;

      }

      public void SetAppUserRememberMe(bool UserRememberme)
      {
          SerialPortSettings.Default.AppUserRememberMe = UserRememberme;
          SerialPortSettings.Default.Save();

      }

      public string[] GetReadoutCommandStracure()
      {
          string[] readoutCmd=new string[9];
          readoutCmd[0] = SerialPortSettings.Default.DefaultReadClassID;
          readoutCmd[1] = SerialPortSettings.Default.DefaultReadOBIS;
          readoutCmd[2] = SerialPortSettings.Default.DefaultReadAtt;
          readoutCmd[3] = SerialPortSettings.Default.DefaultReadDaraType;
          readoutCmd[4] = SerialPortSettings.Default.DefaultReadLen.ToString();
          readoutCmd[5] = SerialPortSettings.Default.DefaultReadAccSelector.ToString();
          readoutCmd[6] = SerialPortSettings.Default.DefaultReadCmdType.ToString();
          readoutCmd[7] = SerialPortSettings.Default.DefaultReadSelectiveAccessCommand;
          readoutCmd[8] = SerialPortSettings.Default.DefaultReadDataValueCommand;      
          return readoutCmd;
      }

      public void SetReadoutCommandStracure( string[] readoutCmd)
      {

          SerialPortSettings.Default.DefaultReadClassID = readoutCmd[0];
          SerialPortSettings.Default.DefaultReadOBIS = readoutCmd[1];
          SerialPortSettings.Default.DefaultReadAtt = readoutCmd[2];
          SerialPortSettings.Default.DefaultReadDaraType = readoutCmd[3];
          SerialPortSettings.Default.DefaultReadLen = readoutCmd[4];  
          SerialPortSettings.Default.DefaultReadAccSelector =Convert.ToInt16(readoutCmd[5]);
          SerialPortSettings.Default.DefaultReadCmdType = Convert.ToInt16(readoutCmd[6]);
          SerialPortSettings.Default.DefaultReadSelectiveAccessCommand = readoutCmd[7];
          SerialPortSettings.Default.DefaultReadDataValueCommand = readoutCmd[8];
          SerialPortSettings.Default.Save();
      }

      public string GetLLSPassword()
      {
          return SerialPortSettings.Default.Password;

      }
      public void SetLLSPassword(string llspwd)
      {
          SerialPortSettings.Default.Password = llspwd;
          SerialPortSettings.Default.Save();
          return;
      }
      public string GetHLSPassword()
      {
          return SerialPortSettings.Default.HLSPWD;

      }
      public void SetHLSPWD(string hlspwd)
      {
          SerialPortSettings.Default.HLSPWD = hlspwd;
          SerialPortSettings.Default.Save();
          return;
      }
      public void SetCipheredSecurityResponse(string secureModeLLSPassword,string secureModeHLSPassword,string secureModeEncruptionKey)
      {
          if (secureModeHLSPassword.Length >= 16) //---US Mode Settings
          {
              SerialPortSettings.Default.ClientSAP = Convert.ToInt32("48");//---MR
              SerialPortSettings.Default.SecurityMechanism = (byte)Convert.ToInt32("02");  //High Level security
              SerialPortSettings.Default.HLSPWD = secureModeHLSPassword;
          }
          else
          {
              SerialPortSettings.Default.ClientSAP = Convert.ToInt32("32");//---MR
              SerialPortSettings.Default.SecurityMechanism = (byte)Convert.ToInt16("01");//Low Level security
              SerialPortSettings.Default.Password = secureModeLLSPassword;
          }
              SerialPortSettings.Default.ApplicationContext = (byte)Convert.ToInt16("03"); //--Logical Name with Ciphering
              SerialPortSettings.Default.Securitysuit = Convert.ToInt16(0x30);//Authentication + Encryption
              SerialPortSettings.Default.GlobalEncryptionKey = secureModeEncruptionKey;//Encryption Key
              SerialPortSettings.Default.DedicatedKey = Convert.ToInt16("01");//Dedicated Key TRUE
              SerialPortSettings.Default.AuthenticationKey = secureModeEncruptionKey; //Authentication Key
              SerialPortSettings.Default.Save();

      }
      public void SetAssociationMode(int AssMeterMode)
      {
          SerialPortSettings.Default.AssociationMode = (byte)AssMeterMode;
          SerialPortSettings.Default.Save();
      }
      public int GetAssociationMode()
      {
          return (int)SerialPortSettings.Default.AssociationMode;
      }
      public string GetGlobalEncryptionKey()
      {
          return SerialPortSettings.Default.GlobalEncryptionKey;
      }
      public void SetAssociationType(int AssType)
      {
          SerialPortSettings.Default.AssociationType = (byte)AssType;
          SerialPortSettings.Default.Save();
      }
      public int GetAssociationType()
      {
          return (int)SerialPortSettings.Default.AssociationType;
      }
      public void SetAssociationAccess(string AssociationAccess)
      {
          SerialPortSettings.Default.AssociationAccess = AssociationAccess;
          SerialPortSettings.Default.Save();
      }
      public string GetAssociationAccess()
      {
          return SerialPortSettings.Default.AssociationAccess;
      }
    
   }
}
