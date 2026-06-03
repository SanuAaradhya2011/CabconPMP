using IntegratedCalibration.Communication;
using IntegratedCalibration.Constants;
using IntegratedCalibration.DataLayer;
using LNG.Communication.SerialCommunication;
using SmartCalibration.Communication;
using SmartCalibration.Constants;
using SmartCalibration.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SmartCalibration.Communication
{
    public class NonDLMSManager
    {
        public NonDLMSSerialComm serialcom;
        public List<byte> ValidData { get; set; }
        //private byte nCMDByte = 0;

        public NonDLMSManager(NonDLMSSerialComm _serialComm)
        {
            // Assign the value as per meter type
            serialcom = _serialComm;
        }
        /// <summary>
        /// Write Image File
        /// </summary>
        /// <param name="filedata"></param>
        /// <returns></returns>
        public Constants.GlobalConstants.Result WriteBotFile(List<BSLCommandPacketList> BSLCommandList, bool isnotbslinitrequired)
        {
            byte[] readValueList = new byte[3];

            try
            {
                if (BSLCommandList == null || BSLCommandList.Count < 1)
                {
                    Logger.Logger.WriteCalibLog("BSLCommandList not found", serialcom.mpos);
                    return Constants.GlobalConstants.Result.Fail;
                }

                string commandforBSLEntrance = "EE0101080000000000000000";//BSL Mote Initiation Hard core command as per design document.
                commandforBSLEntrance = commandforBSLEntrance + CheckSum.GetCheckSum(commandforBSLEntrance, false);
                List<byte> commandforBSLEntranceList = GenericMethods.ConvertStrToByte(commandforBSLEntrance);

                if (!serialcom.OpenPort())
                {
                    Logger.Logger.WriteCalibLog("Port open error", serialcom.mpos);
                    return GlobalConstants.Result.Fail;
                }

                SPS2BootMot sps2bootmot = new SPS2BootMot();

                if (Constants.GlobalConstants.IsRetryMode == false)
                {
                    WriteNonProtocolPacket(commandforBSLEntranceList, 0x00, (byte)BSLCommandPacketList.URTcommandHeader);

                    if (serialcom.bufferIndex <= 0)
                    {
                        Logger.Logger.WriteCalibLog("Unable to switch", serialcom.mpos);
                        return Constants.GlobalConstants.Result.Fail;
                    }

                    Logger.Logger.WriteCalibLog("Switched Successffully", serialcom.mpos);


                    /*-------------------------Step 2: Erasing the Device---------------------------------------*/
                    

                    //List<byte> commandforBSLWrongPassword = sps2bootmot.CreateBSLCoreCommand("", "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000", (byte)BSLCommandPacketList.BSLCoreCommandID.RXPassword);//Any 32 Byte wrong Password
                    List<byte> commandforBSLCorrectPassword = sps2bootmot.CreateBSLCoreCommand("", GlobalConstants.BSLPassword.Trim(), (byte)BSLCommandPacketList.BSLCoreCommandID.RXPassword);

                    //WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(commandforBSLWrongPassword), 0x00, (byte)BSLCommandPacketList.URTcommandHeader);

                    //if (!VerifyBSLResponse(0, ref readValueList)) { /*return;*/ }//response will come fail always as wrong password sended for erasing the memory
                    //Thread.Sleep(100);
                    WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(commandforBSLCorrectPassword), 0x00, (byte)BSLCommandPacketList.URTcommandHeader);
                    Thread.Sleep(100);
                    if (!VerifyBSLResponse(0, ref readValueList)) return GlobalConstants.Result.Fail;
                    Thread.Sleep(100);
                }
                /*-------------------------Step 3: Downloading the Firmware Image File---------------------------------------*/

                int VerifyByteCounts = 0;

                Logger.Logger.WriteCalibLog("Downloading Start -" +  BSLCommandList.Count.ToString() + " ...", serialcom.mpos);

                int icount = 1;

                foreach (var item in BSLCommandList)
                {
                    //lblElapsedTime.Text = "FW Upload Time: " + String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                    serialcom.ReceiveBuffer[0] = 0xFF;//---Default wrong Value
                    if (VerifyByteCounts == -1)
                        WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(item.BSLCommandData), 0x00, (byte?)null);
                    else
                        WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(item.BSLCommandData), 0x00, (byte)BSLCommandPacketList.URTcommandHeader);

                    if (!VerifyBSLResponse(VerifyByteCounts, ref readValueList))
                    {
                        serialcom.ReceiveBuffer[0] = 0xFF;//---Default wrong Value

                        if (VerifyByteCounts == -1)
                            WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(item.BSLCommandData), 0x00, (byte?)null);
                        else
                            WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(item.BSLCommandData), 0x00, (byte)BSLCommandPacketList.URTcommandHeader);

                        if (!VerifyBSLResponse(VerifyByteCounts, ref readValueList))
                        {
                            File.WriteAllText("BSL" + serialcom.mpos.ToString() + ".txt", "Test");
                            return GlobalConstants.Result.Fail;
                        }
                    }

                    Logger.Logger.WriteCalibLog("Block No - " + icount.ToString() + " -> Pass", serialcom.mpos);

                    icount++;
                }

                /*-------------------------Step 4: Verify Firmware Image File---------------------------------------*/
                //Under development at firmware side
                /*-------------------------Step 5: Executing the Application---------------------------------------*/
                //----------------Get BSL Activation Address------------------
                Logger.Logger.WriteCalibLog("Activation...", serialcom.mpos);

                List<byte> commandforBSLActivationAddress = sps2bootmot.CreateBSLCoreCommand("65534", "0200", (byte)BSLCommandPacketList.BSLCoreCommandID.TXDataBlock);// Address = 0x00FFFE;
                WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(commandforBSLActivationAddress), 0x00, (byte)BSLCommandPacketList.URTcommandHeader);
                if (!VerifyBSLResponse(2, ref readValueList)) { /*return;*/ }//response will come fail always as wrong password sended for erasing the memory
                                                                             //--------------------Activate firmware---------------------
                List<byte> activationAddress = new List<byte>();
                activationAddress.AddRange(readValueList);
                activationAddress.Add(0x00); // Add one more byte as 0x00 to make it 3 Byte address 
                activationAddress.Reverse();
                List<byte> commandforBSLActivate = sps2bootmot.CreateBSLCoreCommand(GenericMethods.FormatData(activationAddress.ToArray(), false), "", (byte)BSLCommandPacketList.BSLCoreCommandID.LoadPC);// Address = 0x00FFFE;
                WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(commandforBSLActivate), 0x00, (byte)BSLCommandPacketList.URTcommandHeader);
                if (!VerifyBSLResponse(0, ref readValueList)) { /*return;*/ }//No Responce/Grabage response may come as meter switched to application mode.

                Logger.Logger.WriteCalibLog("Block Completed Successffully", serialcom.mpos);

                return Constants.GlobalConstants.Result.Pass;
            }
            catch (Exception ex)
            {
                return Constants.GlobalConstants.Result.Fail;
            }
            finally
            {
                serialcom.ClosePort();
            }
        }


        /// <summary>
        /// Write Image File
        /// </summary>
        /// <param name="filedata"></param>
        /// <returns></returns>
        public Constants.GlobalConstants.Result WriteBotFileRepair(List<BSLCommandPacketList> BSLCommandList, bool isnotbslinitrequired)
        {
            byte[] readValueList = new byte[3];
            
            try
            {
                if (BSLCommandList == null || BSLCommandList.Count < 1)
                {
                    Logger.Logger.WriteCalibLog("BSLCommandList not found", serialcom.mpos);
                    return Constants.GlobalConstants.Result.Fail;
                }

                string commandforBSLEntrance = "EE0101080000000000000000";//BSL Mote Initiation Hard core command as per design document.
                commandforBSLEntrance = commandforBSLEntrance + CheckSum.GetCheckSum(commandforBSLEntrance, false);
                List<byte> commandforBSLEntranceList = GenericMethods.ConvertStrToByte(commandforBSLEntrance);

                if (!serialcom.OpenPort())
                {
                    Logger.Logger.WriteCalibLog("Port open error", serialcom.mpos);
                    return GlobalConstants.Result.Fail;
                }

                SPS2BootMot sps2bootmot = new SPS2BootMot();

                serialcom.ReceiveBuffer[0] = 0x00;
                WriteNonProtocolPacket(commandforBSLEntranceList, 0x00, (byte)BSLCommandPacketList.URTcommandHeader);

                if (serialcom.bufferIndex <= 0)
                {
                    Logger.Logger.WriteCalibLog("Unable to switch", serialcom.mpos);
                }

                Logger.Logger.WriteCalibLog("Switched Successffully", serialcom.mpos);

                /*-------------------------Step 2: Erasing the Device---------------------------------------*/
                List<byte> commandforBSLWrongPassword = sps2bootmot.CreateBSLCoreCommand("", "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000", 
                    (byte)BSLCommandPacketList.BSLCoreCommandID.RXPassword);//Any 32 Byte wrong Password

                GlobalConstants.BSLPassword = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";
                List<byte> commandforBSLCorrectPassword = sps2bootmot.CreateBSLCoreCommand("", GlobalConstants.BSLPassword.Trim(), (byte)BSLCommandPacketList.BSLCoreCommandID.RXPassword);
                WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(commandforBSLWrongPassword), 0x00, (byte)BSLCommandPacketList.URTcommandHeader);
                if (!VerifyBSLResponse(0, ref readValueList)) { /*return;*/ }//response will come fail always as wrong password sended for erasing the memory
                Thread.Sleep(100);
                WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(commandforBSLCorrectPassword), 0x00, (byte)BSLCommandPacketList.URTcommandHeader);
                Thread.Sleep(100);
                if (!VerifyBSLResponse(0, ref readValueList)) return GlobalConstants.Result.Fail;
                Thread.Sleep(100);
                
                /*-------------------------Step 3: Downloading the Firmware Image File---------------------------------------*/

                int VerifyByteCounts = 0;

                Logger.Logger.WriteCalibLog("Downloading Start -" + BSLCommandList.Count.ToString() + " ...", serialcom.mpos);

                int icount = 1;

                foreach (var item in BSLCommandList)
                {
                    //lblElapsedTime.Text = "FW Upload Time: " + String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                    serialcom.ReceiveBuffer[0] = 0xFF;//---Default wrong Value
                    if (VerifyByteCounts == -1) WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(item.BSLCommandData), 0x00, (byte?)null);
                    else WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(item.BSLCommandData), 0x00, (byte)BSLCommandPacketList.URTcommandHeader);

                    if (!VerifyBSLResponse(VerifyByteCounts, ref readValueList))
                    {
                        //File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "BSL" + serialcom.mpos.ToString() + ".txt", "Test");
                        return GlobalConstants.Result.Fail;
                    }

                    Logger.Logger.WriteCalibLog("Block No - " + icount.ToString() + " -> Pass", serialcom.mpos);

                    icount++;
                }

                /*-------------------------Step 4: Verify Firmware Image File---------------------------------------*/
                //Under development at firmware side
                /*-------------------------Step 5: Executing the Application---------------------------------------*/
                //----------------Get BSL Activation Address------------------
                Logger.Logger.WriteCalibLog("Activation...", serialcom.mpos);

                List<byte> commandforBSLActivationAddress = sps2bootmot.CreateBSLCoreCommand("65534", "0200", (byte)BSLCommandPacketList.BSLCoreCommandID.TXDataBlock);// Address = 0x00FFFE;
                WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(commandforBSLActivationAddress), 0x00, (byte)BSLCommandPacketList.URTcommandHeader);
                if (!VerifyBSLResponse(2, ref readValueList)) { /*return;*/ }//response will come fail always as wrong password sended for erasing the memory
                                                                             //--------------------Activate firmware---------------------
                List<byte> activationAddress = new List<byte>();
                activationAddress.AddRange(readValueList);
                activationAddress.Add(0x00); // Add one more byte as 0x00 to make it 3 Byte address 
                activationAddress.Reverse();
                List<byte> commandforBSLActivate = sps2bootmot.CreateBSLCoreCommand(GenericMethods.FormatData(activationAddress.ToArray(), false), "", (byte)BSLCommandPacketList.BSLCoreCommandID.LoadPC);// Address = 0x00FFFE;
                WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(commandforBSLActivate), 0x00, (byte)BSLCommandPacketList.URTcommandHeader);
                if (!VerifyBSLResponse(0, ref readValueList)) { /*return;*/ }//No Responce/Grabage response may come as meter switched to application mode.

                Logger.Logger.WriteCalibLog("Block Completed Successffully", serialcom.mpos);

                return Constants.GlobalConstants.Result.Pass;
            }
            catch (Exception ex)
            {
                return Constants.GlobalConstants.Result.Fail;
            }
            finally
            {
                serialcom.ClosePort();
            }
        }


        /// <summary>
        /// Write Image File
        /// </summary>
        /// <param name="filedata"></param>
        /// <returns></returns>
        public Constants.GlobalConstants.Result WriteBotFileRepairRetry(List<BSLCommandPacketList> BSLCommandList, bool isnotbslinitrequired)
        {
            byte[] readValueList = new byte[3];

            try
            {
                if (BSLCommandList == null || BSLCommandList.Count < 1)
                {
                    Logger.Logger.WriteCalibLog("BSLCommandList not found", serialcom.mpos);
                    return Constants.GlobalConstants.Result.Fail;
                }

                string commandforBSLEntrance = "EE0101080000000000000000";//BSL Mote Initiation Hard core command as per design document.
                commandforBSLEntrance = commandforBSLEntrance + CheckSum.GetCheckSum(commandforBSLEntrance, false);
                List<byte> commandforBSLEntranceList = GenericMethods.ConvertStrToByte(commandforBSLEntrance);

                if (!serialcom.OpenPort())
                {
                    Logger.Logger.WriteCalibLog("Port open error", serialcom.mpos);
                    return GlobalConstants.Result.Fail;
                }

                SPS2BootMot sps2bootmot = new SPS2BootMot();
                Constants.GlobalConstants.IsRetryMode = true;

                if (Constants.GlobalConstants.IsRetryMode == false)
                {
                    WriteNonProtocolPacket(commandforBSLEntranceList, 0x00, (byte)BSLCommandPacketList.URTcommandHeader);

                    if (serialcom.bufferIndex <= 0)
                    {
                        Logger.Logger.WriteCalibLog("Unable to switch", serialcom.mpos);
                        return Constants.GlobalConstants.Result.Fail;
                    }

                    Logger.Logger.WriteCalibLog("Switched Successffully", serialcom.mpos);

                    /*-------------------------Step 2: Erasing the Device---------------------------------------*/

                    List<byte> commandforBSLWrongPassword = sps2bootmot.CreateBSLCoreCommand("", "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000", (byte)BSLCommandPacketList.BSLCoreCommandID.RXPassword);//Any 32 Byte wrong Password
                    GlobalConstants.BSLPassword = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";
                    List<byte> commandforBSLCorrectPassword = sps2bootmot.CreateBSLCoreCommand("", GlobalConstants.BSLPassword.Trim(), (byte)BSLCommandPacketList.BSLCoreCommandID.RXPassword);

                    WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(commandforBSLWrongPassword), 0x00, (byte)BSLCommandPacketList.URTcommandHeader);

                    if (!VerifyBSLResponse(0, ref readValueList)) { /*return;*/ }//response will come fail always as wrong password sended for erasing the memory
                    Thread.Sleep(100);
                    WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(commandforBSLCorrectPassword), 0x00, (byte)BSLCommandPacketList.URTcommandHeader);
                    Thread.Sleep(100);
                    if (!VerifyBSLResponse(0, ref readValueList)) return GlobalConstants.Result.Fail;
                    Thread.Sleep(100);
                }
                /*-------------------------Step 3: Downloading the Firmware Image File---------------------------------------*/

                int VerifyByteCounts = 0;

                Logger.Logger.WriteCalibLog("Downloading Start -" + BSLCommandList.Count.ToString() + " ...", serialcom.mpos);

                int icount = 1;

                foreach (var item in BSLCommandList)
                {
                    //lblElapsedTime.Text = "FW Upload Time: " + String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                    serialcom.ReceiveBuffer[0] = 0xFF;//---Default wrong Value
                    if (VerifyByteCounts == -1) WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(item.BSLCommandData), 0x00, (byte?)null);
                    else WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(item.BSLCommandData), 0x00, (byte)BSLCommandPacketList.URTcommandHeader);

                    if (!VerifyBSLResponse(VerifyByteCounts, ref readValueList))
                    {
                        //File.WriteAllText("BSL" + serialcom.mpos.ToString() + ".txt", "Test");
                        return GlobalConstants.Result.Fail;
                    }

                    Logger.Logger.WriteCalibLog("Block No - " + icount.ToString() + " -> Pass", serialcom.mpos);

                    icount++;
                }

                /*-------------------------Step 4: Verify Firmware Image File---------------------------------------*/
                //Under development at firmware side
                /*-------------------------Step 5: Executing the Application---------------------------------------*/
                //----------------Get BSL Activation Address------------------
                Logger.Logger.WriteCalibLog("Activation...", serialcom.mpos);

                List<byte> commandforBSLActivationAddress = sps2bootmot.CreateBSLCoreCommand("65534", "0200", (byte)BSLCommandPacketList.BSLCoreCommandID.TXDataBlock);// Address = 0x00FFFE;
                WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(commandforBSLActivationAddress), 0x00, (byte)BSLCommandPacketList.URTcommandHeader);
                if (!VerifyBSLResponse(2, ref readValueList)) { /*return;*/ }//response will come fail always as wrong password sended for erasing the memory
                                                                             //--------------------Activate firmware---------------------
                List<byte> activationAddress = new List<byte>();
                activationAddress.AddRange(readValueList);
                activationAddress.Add(0x00); // Add one more byte as 0x00 to make it 3 Byte address 
                activationAddress.Reverse();
                List<byte> commandforBSLActivate = sps2bootmot.CreateBSLCoreCommand(GenericMethods.FormatData(activationAddress.ToArray(), false), "", (byte)BSLCommandPacketList.BSLCoreCommandID.LoadPC);// Address = 0x00FFFE;
                WriteNonProtocolPacket(sps2bootmot.CreateUARTBSLCommand(commandforBSLActivate), 0x00, (byte)BSLCommandPacketList.URTcommandHeader);
                if (!VerifyBSLResponse(0, ref readValueList)) { /*return;*/ }//No Responce/Grabage response may come as meter switched to application mode.

                Logger.Logger.WriteCalibLog("Block Completed Successffully", serialcom.mpos);

                return Constants.GlobalConstants.Result.Pass;
            }
            catch (Exception ex)
            {
                return Constants.GlobalConstants.Result.Fail;
            }
            finally
            {
                serialcom.ClosePort();
            }
        }

        private bool WriteNonProtocolPacket(List<byte> commandDataByte, byte CommandResponseStopByte, byte? CommandResponseStopByte2nd)
        {
            try
            {
                serialcom._CommandResponseStopByte = CommandResponseStopByte;

                if (CommandResponseStopByte2nd != null)
                {
                    serialcom._2NdCommandResponseStopByte = (byte)CommandResponseStopByte2nd;
                }

                bool bret =  serialcom.SendDataToPort(commandDataByte.ToArray(), commandDataByte.Count());

                Thread.Sleep(200);

                return bret;

            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        private bool VerifyBSLResponse(int ReadbyteLength, ref byte[] ReadValue)
        {
            if (ReadbyteLength == -1) //--To Check Fast mode response only
            {
                if (serialcom.ReceiveBuffer[0] != 0x00)
                {
                   // objIECLI.DisplayStatusMsg("Invalid Meter Response!", true);
                    return false;
                }
                return true;
            }
            if (serialcom.ReceiveBuffer[0] != 0x00 || serialcom.ReceiveBuffer[1] != BSLCommandPacketList.URTcommandHeader)
            {
                //objIECLI.DisplayStatusMsg("Invalid Meter Response!", true);
                return false;
            }
            if (serialcom.ReceiveBuffer[4] == (byte)BSLCommandPacketList.BSLResponseCommandID.BSLResponseMessage)
            {
                if (serialcom.ReceiveBuffer[5] == (byte)BSLCommandPacketList.BSLResponseMessage.OperationSuccessful) return true;
                else
                {
                    //objIECLI.DisplayStatusMsg("Error : " + objBSLcommand.GetBSLResponseMEssage(GlobalObjects.objIECMeterSerialComm.ReceiveBuffer[5]), true);
                }
            }
            else if (serialcom.ReceiveBuffer[4] == (byte)BSLCommandPacketList.BSLResponseCommandID.DataBlock)
            {
                ReadValue = new byte[ReadbyteLength];
                Array.Copy(serialcom.ReceiveBuffer, 5, ReadValue, 0, ReadbyteLength);
                return true;
            }
            return false;
        }

    }
}
