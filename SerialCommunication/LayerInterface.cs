using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using DLMSLIB;
 //using Utilities;
 using System.Windows.Forms;
using SerialCommunication;
namespace App_1Phase
{
   
  public  class LayerInterface
    {
        byte HDLCIndex = 0;
        public delegate void UpdateHandler(object sender, UpdateEventArgs e);
        public event UpdateHandler UpdatedLed;
        UpdateEventArgs args = null;
        byte[] HDLCCommand = new byte[200];
        SerialComm objSerialComm = new SerialComm();
      

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

        #endregion

        public void DisplayStatusMsg(string msgString,bool isError)
        {
            args = new UpdateEventArgs(msgString, isError);
            UpdatedLed(this, args);
        }
        public bool ConnectToMeter()
        {
                DisplayStatusMsg("  Physical Layer Communication...",false);
                if (!PhysicalLayerConnect()) { DisplayStatusMsg("Physical Layer Connection Failed!", true); return false; }
                DisplayStatusMsg("HDLC Layer Communication...", false);
                if (!HDLCLayerConnect()) { DisplayStatusMsg("HDLC Layer Connection Failed/ Busy !", true); return false; }
                DisplayStatusMsg("Stablizing Association...", false);
                if (!AssociationStablish()) { DisplayStatusMsg("Unable To Stablish Association!", true); return false; }
                DisplayStatusMsg("Data Transfering Please Wait...", false);
                return true;               
        }

        public bool PhysicalLayerConnect()
        {
            try
            {
                objSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, SerialPortSettings.Default.CommandBaudRate, SerialPortSettings.Default.Parity, SerialPortSettings.Default.DataBits, SerialPortSettings.Default.StopBits, SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                if (objSerialComm.OpenPort()) return true;
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
               
                 if (GlobalObjects.objGlobalFunctions.fSendSNRM(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP))return true;
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
                if (GlobalObjects.objGlobalFunctions.fSendAARQ(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP, SerialPortSettings.Default.SecurityMechanism, SerialPortSettings.Default.Password, SerialPortSettings.Default.HLSKey,SerialPortSettings.Default.HLSPWD, SerialPortSettings.Default.ConformanceBlock)) return true;
                else return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void AssociationDisconnect()
        {
            try
            {
                if (GlobalObjects.objGlobalFunctions.fSendDISC(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP)) return;

            }
            catch (Exception)
            {
                return;
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
            while (stractcount < controllist.Length) controllist[stractcount++].Text = "";
            int writeResponse = ReadDataCommand(ParameterOBIS, classCode, AttCode);
            if (writeResponse == (int)ProgrammingCode.Success) { /*DisplayStatusMsg("Reading Succesfull.", false);*/ return true; }
            else if (writeResponse == (int)ProgrammingCode.AccessDenied) { DisplayStatusMsg("Access Denied!", true); MessageBox.Show("Access Denied!", "Landis+Gyr", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); return false; }
            else if (writeResponse == (int)ProgrammingCode.DataUnavailable) { DisplayStatusMsg("Data Not Available!", true); MessageBox.Show("Data Not Available!", "Landis+Gyr", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1); return false; }
            else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed) { DisplayStatusMsg("Cosem Connection Failed!", true); MessageBox.Show("Cosem Connection Failed!", "Landis+Gyr", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); return false; }
            else { DisplayStatusMsg("Communication Failed!", true); return false; }
        }

        public bool ReadBlockFromMeter(byte[] ParameterOBIS, TextBox[] controllist, string displayFormat, decimal emf, byte classCode, byte AttCode, byte Access_Selector, List<byte> DescriptorByteList)
        {
            int stractcount = 0;
            while (stractcount < controllist.Length) controllist[stractcount++].Text = "";
            int writeResponse = ReadDataBlockCommand(ParameterOBIS, classCode, AttCode, Access_Selector, DescriptorByteList);
            if (writeResponse == (int)ProgrammingCode.Success) { /*DisplayStatusMsg("Reading Succesfull.", false);*/ return true; }
            else if (writeResponse == (int)ProgrammingCode.AccessDenied) { DisplayStatusMsg("Access Denied!", true); MessageBox.Show("Access Denied!", "Landis+Gyr", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); return false; }
            else if (writeResponse == (int)ProgrammingCode.DataUnavailable) { DisplayStatusMsg("Data Not Available!", true); MessageBox.Show("Data Not Available!", "Landis+Gyr", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);  return false; }
            else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed) { DisplayStatusMsg("Cosem Connection Failed!", true); MessageBox.Show("Cosem Connection Failed!", "Landis+Gyr", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); return false; }
            else { DisplayStatusMsg("Communication Failed!", true); return false; }
        }
       
        public int ReadAllTamperBlockFromMeter(byte[] ParameterOBIS, TextBox[] controllist, string displayFormat, decimal emf, byte classCode, byte AttCode, byte Access_Selector, List<byte> DescriptorByteList)
        {
            int stractcount = 0;
            while (stractcount < controllist.Length) controllist[stractcount++].Text = "";
            int writeResponse = ReadDataBlockCommand(ParameterOBIS, classCode, AttCode, Access_Selector, DescriptorByteList);
            return writeResponse;
        }
        
        public bool WriteDataToMeter(byte attributeID, byte[] ParameterOBIS, byte paraClassID, byte typeofStruct, byte lengthofStruct, List<byte> ParameterBytes, byte[] DataRequestType)
        {
            int writeResponse = WritParameterToMeter(ParameterBytes, attributeID, ParameterOBIS, paraClassID, typeofStruct, lengthofStruct, DataRequestType);
            if (writeResponse == (int)ProgrammingCode.Success) {/* DisplayStatusMsg("Parameter Written Successfully.", false); */return true; }
            else if (writeResponse == (int)ProgrammingCode.AccessDenied) { DisplayStatusMsg("Access Denied.Please Change The Mode!", true); /*MessageBox.Show("Access Denied.Please Change The Mode!", "Landis+Gyr", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); */return false; }
            else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed) { DisplayStatusMsg("Cosem Connection Failed!", true); /*MessageBox.Show("Cosem Connection Failed!", "Landis+Gyr", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);*/ return false; }
            else { DisplayStatusMsg("Communication Failed!", true); return false; }
        }

        public bool WriteBlockDataToMeter(byte attributeID, byte[] ParameterOBIS, byte paraClassID, byte typeofStruct, int lengthofStruct, List<byte> ParameterBytes, byte[] DataRequestType)
        {
            int writeResponse = WritBlockToMeter(ParameterBytes, attributeID, ParameterOBIS, paraClassID, typeofStruct, lengthofStruct, DataRequestType);
            if (writeResponse == (int)ProgrammingCode.Success) { /* DisplayStatusMsg("Parameter Written Successfully.", false); */return true; }
            else if (writeResponse == (int)ProgrammingCode.AccessDenied) { DisplayStatusMsg("Access Denied.Please Change The Mode!", true); /*MessageBox.Show("Access Denied.Please Change The Mode!", "Landis+Gyr", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); */return false; }
            else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed) { DisplayStatusMsg("Cosem Connection Failed!", true); /*MessageBox.Show("Cosem Connection Failed!", "Landis+Gyr", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);*/ return false; }
            else { DisplayStatusMsg("Communication Failed!", true); return false; }
        }

        private int WritParameterToMeter(List<byte> DataByte, byte attributeID, byte[] ParameterOBIS, byte ParaClassID, byte typeodData, byte lengthofData, byte[] DataRequestType)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteToMeter(HDLCCommand, HDLCIndex, attributeID, ParameterOBIS, ParaClassID, typeodData, lengthofData,DataRequestType);

                HDLCIndex = GlobalObjects.objHDLCLIB.FillWriteParameters(HDLCCommand, HDLCIndex, DataByte);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex)) return (int)ProgrammingCode.CosemConnectionFailed;
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

        private int ReadDataCommand(byte[] OBISCode, byte ClassCode,byte AttID)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadByClassOBIS(HDLCCommand, HDLCIndex, AttID, OBISCode, ClassCode);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex)) return (int)ProgrammingCode.CosemConnectionFailed;
                //////Application.DoEvents();
                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer)) return (int)ProgrammingCode.CosemConnectionFailed;
                int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                if (ret == 0x01) return (int)ProgrammingCode.Success;
                else if (ret == 0x0E) return (int)ProgrammingCode.DataUnavailable; //Data block unavailable
                else if (ret == 0x03) return (int)ProgrammingCode.AccessDenied; //Access denied
                else return (int)ProgrammingCode.CosemConnectionFailed;
            }
            catch (Exception)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }

        public int ReadDataBlockCommand(byte[] OBISCode, byte ClassCode, byte AttID, byte Access_Selector, List<byte> DescriptorByteList)
        {
            try
            {
                GlobalObjects.objCOSEMLIB.nBlockIndex = 0x00;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0x00;

                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadByClassOBIS(HDLCCommand, HDLCIndex, AttID, OBISCode, ClassCode);
                //----------------------------For Selective Access--------------------------------------------------------------------------
                if (Access_Selector != 0x00)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.FillCommandData(HDLCCommand, --HDLCIndex, DescriptorByteList);
                }
                //if (isSelectiveAccess)
                //{
                //    if (ValueType == 0)
                //    {
                //        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, 1, RangeValue1);
                //    }
                //    else if (ValueType == 1)
                //    {
                //        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, RangeValue1, RangeValue2);
                //    }
                //    else if (ValueType == 2)
                //    {
                //        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, startDate, endDate);
                //    }
                //}

 
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex))return (int)ProgrammingCode.CosemConnectionFailed;

                GlobalObjects.objHDLCLIB.fIncRecieve();
                if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer)) return (int)ProgrammingCode.CosemConnectionFailed;

                int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                if (ret == 0x01) return (int)ProgrammingCode.Success;
                else if (ret == 0x05)return (int)ProgrammingCode.AccessDenied;               
                else if (ret == 0x07) return (int)ProgrammingCode.DataUnavailable;
                else if (ret != 0x02) return (int)ProgrammingCode.CosemConnectionFailed;
                while (true)
                {
                   
                    HDLCIndex = 0;
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                    GlobalObjects.objHDLCLIB.fIncSend();
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                    HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                    GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                    GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                    GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                    GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                    GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    //7EA014022321766E17E6E600C002C100000002CA8C7E
                    if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex)) return (int)ProgrammingCode.CosemConnectionFailed;
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

        
        private int WritBlockToMeter(List<byte> DataByte, byte attributeID, byte[] ParameterOBIS, byte ParaClassID, byte typeodData, int lengthofData, byte[] DataRequestType)//(byte[] nDataArray, int nLength, byte atb)
        {
            try
            {
               // int nErrorCode = 0x00;
                bool nBlkTransfer = false;               
                while (true)
                {
                    HDLCIndex = 0;
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                    GlobalObjects.objHDLCLIB.fIncSend();
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                     
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = lengthofData;
                        HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryToWriteBlockToMeter(HDLCCommand, HDLCIndex, attributeID, ParameterOBIS, ParaClassID, typeodData, lengthofData, DataRequestType);
                        //HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteTOUBlock(HDLCCommand, HDLCIndex, attributeID);
                    


                    HDLCIndex = GlobalObjects.objCOSEMLIB.fSetBlockTransferPacket(HDLCCommand, HDLCIndex, DataByte.ToArray(), nBlkTransfer);


                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                    GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                    GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                    GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                    GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                    GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                    if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex))return (int)ProgrammingCode.CosemConnectionFailed;
                     
                        GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                        if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer))return (int)ProgrammingCode.CosemConnectionFailed;
                         
                            int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                            if (ret == 0x01)return (int)ProgrammingCode.Success;                             
                            else if (ret == 0x02)return (int)ProgrammingCode.AccessDenied;
                            else if (ret != 0x04) return (int)ProgrammingCode.CosemConnectionFailed;                            
                            //nErrorCode = 0x4;
                            nBlkTransfer = true;                           
                        
                    
                }
                return (int)ProgrammingCode.Success; ; 
            }
            catch (Exception)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }

        public List<byte> GetByteByEntry_ValueType(byte fromEntry, byte toEntry)
        {
            List<byte> EnrtyByValue = new List<byte>();           
            EnrtyByValue.Add(0x01);
            EnrtyByValue.Add(0x02);
            EnrtyByValue.Add(0x02);      
            EnrtyByValue.Add(0x04);
            EnrtyByValue.Add(0x06);
            EnrtyByValue.Add(0x00);
            EnrtyByValue.Add(0x00);
            EnrtyByValue.Add(0x00);
            EnrtyByValue.Add(Convert.ToByte(fromEntry));
            EnrtyByValue.Add(0x06);
            EnrtyByValue.Add(0x00);
            EnrtyByValue.Add(0x00);
            EnrtyByValue.Add(0x00);
            EnrtyByValue.Add(Convert.ToByte(toEntry));
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

            EnrtyByDate.Add(Convert.ToByte((fromDate.Year / 100) % 20)); //year
            EnrtyByDate.Add(Convert.ToByte(fromDate.Year % 100));

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

            EnrtyByDate.Add(Convert.ToByte((toDate.Year / 100) % 20)); //year
            EnrtyByDate.Add(Convert.ToByte(toDate.Year % 100));

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

      #endregion
      public void DisplayStatusMsg(string msgString, bool isError)
      {
          args = new UpdateEventArgs(msgString, isError);
          UpdatedLed(this, args);
      }
      public bool ConnectToMeter()
      {
          DisplayStatusMsg("Connecting...", false);
          if (!PhysicalLayerConnect(false)) { DisplayStatusMsg("Connecting Failed!", true); return false; }
          DisplayStatusMsg("Signon...", false);
          if (!HDLCLayerConnect()) { DisplayStatusMsg("Signon Failed!", true); return false; }
          DisplayStatusMsg("Checking Association...", false);
          if (!CheckingAssociation()) { DisplayStatusMsg("Unable To Stablish Association!", true); return false; }
          DisplayStatusMsg("Stablizing Association...", false);
          if (!AssociationStablish()) { DisplayStatusMsg("Unable To Stablish Association!", true); return false; }
          DisplayStatusMsg("Data Transfering Please Wait...", false);
          return true;

      }
      public bool PhysicalLayerConnect(bool isIECSettings)
      {
          try
          {
              if (isIECSettings) GlobalObjects.objIECSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, "9600", "None", "8", "1", 5000, 1000, SerialPortSettings.Default.BaudRateSelectedIndex);
              else GlobalObjects.objIECSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, "300", "Even", "7", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.BaudRateSelectedIndex);
              if (GlobalObjects.objIECSerialComm.OpenPort()) return true;
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
              GlobalObjects.objIECSerialComm.StopResponseByte = "\r\n";   
              if (GlobalObjects.objIECSerialComm.SignOn()) return true;
              else return false;
          }
          catch (Exception)
          {
              return false;
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
      public string WriteDataToMeter(string CommandHexString)
      {
          try
          {
              
              GlobalObjects.objIECSerialComm.StopResponseByte = "\x06";
              return GlobalObjects.objIECSerialComm.SendDataToMeterandGetResponse(CommandHexString);
              
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
  
 
}
