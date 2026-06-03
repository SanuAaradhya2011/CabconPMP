using LNG.Communication.SerialCommunication;
using SmartCalibration.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCalibration.Communication
{
    public class DLMSManager
    {
        public SerialComm serialcom;
        public List<byte> ValidData { get; set; }
        public byte nTestCMDByte = 0;

        public DLMSManager(SerialComm _serialComm)
        {
            // Assign the value as per meter type
            serialcom = _serialComm;
        }

        /// <summary>
        /// Connect to DLMS Smart Meter
        /// </summary>
        /// <returns></returns>
        public Constants.GlobalConstants.Result ConnectToMeter()
        {
            Constants.GlobalConstants.Result result = Constants.GlobalConstants.Result.Fail;

            if (!serialcom.OpenPort())
            {
                Logger.Logger.WriteCalibLog("Meter Connection Failed -> Port Error", serialcom.mpos);
                return result;
            }

            DisConnect();

            Thread.Sleep(20);

            if (!SendSNRM())
            {
                Logger.Logger.WriteCalibLog("Meter Connection Failed -> SNRM Failed", serialcom.mpos);
                return result;
            }

            if (!SendAARQ())
            {
                Logger.Logger.WriteCalibLog("Meter Connection Failed -> AARQ Failed", serialcom.mpos);
                return result;
            }
            
            if ( !SendHighPassSecurity())
            {
                Logger.Logger.WriteCalibLog("Meter Connection Failed -> HighPass Failed", serialcom.mpos);
                return result;
            }

            Logger.Logger.WriteCalibLog("Association Established -> Pass", serialcom.mpos);

            return Constants.GlobalConstants.Result.Pass;
        }

        /// <summary>
        /// Disconnect association and close the port
        /// </summary>
        /// <returns></returns>
        public Constants.GlobalConstants.Result DisConnect()
        {
            Constants.GlobalConstants.Result result = Constants.GlobalConstants.Result.Fail;

            basehdlc hdlc = DLMSFactory.HDLCObjectMapper[GlobalConstants.GlobalMeterType];

            byte nCMDByte = 0;

            try
            {
                if (!serialcom.OpenPort())
                    return result;

                List<byte> datapkt = hdlc.GetDisConnect();

                if (!serialcom.SendDataToPort(datapkt.ToArray(), datapkt.Count))
                    return result;

                if (!hdlc.dlmsFrameValidation(serialcom.ReceiveBuffer.ToList(), hdlc.nCMDByte))
                    return result;

                nCMDByte = serialcom.ReceiveBuffer[8];


                Logger.Logger.WriteCalibLog("Association Disconnected -> Pass", serialcom.mpos);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                serialcom.ClosePort();
                Logger.Logger.WriteCalibLog("PortClosed-------------------------------------------", serialcom.mpos);
            }

            return Constants.GlobalConstants.Result.Pass;
        }

        /// <summary>
        /// Send SNRM Command
        /// </summary>
        /// <returns></returns>
        private bool SendSNRM()
        {
            basehdlc hdlc = DLMSFactory.HDLCObjectMapper[GlobalConstants.GlobalMeterType];

            List<byte> datapkt = hdlc.GetCmdSNRM();

            if (!serialcom.SendDataToPort(datapkt.ToArray(), datapkt.Count))
                return false;

            if (!hdlc.dlmsFrameValidation(serialcom.ReceiveBuffer.ToList(), hdlc.nCMDByte))
                return false;

            //nCMDByte = serialcom.ReceiveBuffer[8];
            
            Logger.Logger.WriteCalibLog("SNRM -> Pass", serialcom.mpos);

            return true;
        }
        
        /// <summary>
        /// Send AARQ Command
        /// </summary>
        /// <returns></returns>
        private bool SendAARQ()
        {
            basehdlc hdlc = DLMSFactory.HDLCObjectMapper[GlobalConstants.GlobalMeterType];

            List<byte> cosemdatapkt = DLMSFactory.CosemObjectMapper[Constants.DLMSConstants.DLMSCommand.AARQ].GetPacket(null);

            // Initialise nCMDByte Here
            hdlc.nCMDByte = 0x10;

            List<byte> hdlcdatapkt = hdlc.GetAARQ(cosemdatapkt);

            if (!serialcom.SendDataToPort(hdlcdatapkt.ToArray(), hdlcdatapkt.Count))
                return false;

            if (!hdlc.dlmsFrameValidation(serialcom.ReceiveBuffer.ToList(), hdlc.nCMDByte))
                return false;

            //nCMDByte = serialcom.ReceiveBuffer[8];

            Logger.Logger.WriteCalibLog("AARQ -> Pass", serialcom.mpos);

            return true;
        }

        /// <summary>
        /// Send High Pass Security Command
        /// </summary>
        /// <returns></returns>
        private bool SendHighPassSecurity()
        {
            if (GlobalConstants.GlobalMeterType != GlobalConstants.MeterType.SM110 && GlobalConstants.GlobalMeterType != GlobalConstants.MeterType.SM310) return true;

            basehdlc hdlc = DLMSFactory.HDLCObjectMapper[GlobalConstants.GlobalMeterType];

            List<byte> cosemdatapkt = DLMSFactory.CosemObjectMapper[Constants.DLMSConstants.DLMSCommand.HIGHLEVELSECURITYPASS].GetPacket(null);

            List<byte> hdlcdatapkt = hdlc.GetNormalHDLCPacket(cosemdatapkt, serialcom.ReceiveBuffer[8]);

            if (!serialcom.SendDataToPort(hdlcdatapkt.ToArray(), hdlcdatapkt.Count))
                return false;

            if (!hdlc.dlmsFrameValidation(serialcom.ReceiveBuffer.ToList(), hdlc.nCMDByte))
                return false;

            return true;
           
        }

        /// <summary>
        /// Command to make Normal Set Packet
        /// </summary>
        /// <param name="cosempkt"></param>
        /// <param name="_commandname"></param>
        /// <returns></returns>
        public Constants.GlobalConstants.Result NormalSetPacket(List<byte> cosempkt, Constants.DLMSConstants.DLMSCommand _commandname)
        {
            int datastartindex = 17;

            basehdlc hdlc = DLMSFactory.HDLCObjectMapper[GlobalConstants.GlobalMeterType];

            ConfigureData configdat = new ConfigureData() { setdata = cosempkt, commandname = _commandname };

            List<byte> cosemdatapkt = DLMSFactory.CosemObjectMapper[Constants.DLMSConstants.DLMSCommand.NORMALSET].GetPacket(configdat);

            if (!hdlc.cmddicbyte.ContainsKey(serialcom.mpos))
                hdlc.cmddicbyte.Add(serialcom.mpos, serialcom.cmddict[serialcom.mpos]);
            else
                hdlc.cmddicbyte[serialcom.mpos] = serialcom.cmddict[serialcom.mpos];

            //List<byte> hdlcdatapkt = hdlc.GetNormalHDLCPacket(cosemdatapkt, serialcom.cmddict[serialcom.mpos]);
            List<byte> hdlcdatapkt = hdlc.GetNormalHDLCPacket(cosemdatapkt, serialcom.mpos);

            nTestCMDByte = hdlc.nCMDByte;

            if (!serialcom.SendDataToPort(hdlcdatapkt.ToArray(), hdlcdatapkt.Count))
                return Constants.GlobalConstants.Result.Fail;

            
            if (!hdlc.dlmsFrameValidation(serialcom.ReceiveBuffer.ToList(), hdlc.nCMDByte))
            {
                Logger.Logger.WriteCalibLog("Cosem Packet Error", serialcom.mpos);
                return Constants.GlobalConstants.Result.Fail;
            }

            if(serialcom.bufferIndex <= (datastartindex+1))
                return Constants.GlobalConstants.Result.Fail;

            if (serialcom.bufferIndex > datastartindex && serialcom.ReceiveBuffer[datastartindex] == 0x00)
            return Constants.GlobalConstants.Result.Pass;

            Logger.Logger.WriteCalibLog("Cosem Packet Error", serialcom.mpos);
            return Constants.GlobalConstants.Result.Fail;

        }

        /// <summary>
        /// Command to get Normal Packet
        /// </summary>
        /// <param name="cosempkt"></param>
        /// <param name="_commandname"></param>
        /// <returns></returns>
        public Constants.GlobalConstants.Result NormalGetPacket(List<byte> cosempkt, Constants.DLMSConstants.DLMSCommand _commandname)
        {
            int datastartindex = 17;

            basehdlc hdlc = DLMSFactory.HDLCObjectMapper[GlobalConstants.GlobalMeterType];

            ConfigureData configdat = new ConfigureData() { setdata = cosempkt, commandname = _commandname  };

            List<byte> cosemdatapkt = DLMSFactory.CosemObjectMapper[Constants.DLMSConstants.DLMSCommand.NORMALGET].GetPacket(configdat);

            List<byte> hdlcdatapkt = hdlc.GetNormalHDLCPacket(cosemdatapkt, serialcom.ReceiveBuffer[8]);

            Thread.Sleep(100);

            if (!serialcom.SendDataToPort(hdlcdatapkt.ToArray(), hdlcdatapkt.Count))
                return Constants.GlobalConstants.Result.Fail;

            if (!hdlc.dlmsFrameValidation(serialcom.ReceiveBuffer.ToList(), hdlc.nCMDByte))
            {
                Logger.Logger.WriteCalibLog("Cosem Packet Error", serialcom.mpos);
                return Constants.GlobalConstants.Result.Fail;
            }

            if(serialcom.bufferIndex < datastartindex)
            {
                Logger.Logger.WriteCalibLog("Cosem Packet Error", serialcom.mpos);
                return Constants.GlobalConstants.Result.Fail;
            }

            if (serialcom.ReceiveBuffer.Length > datastartindex && serialcom.ReceiveBuffer[datastartindex] == 0x00)
            {
                ValidData = new List<byte>();
                ValidData.AddRange(serialcom.ReceiveBuffer);
            }

            Logger.Logger.WriteCalibLog("Normal Packet Get -> Pass", serialcom.mpos);

            return Constants.GlobalConstants.Result.Pass;

        }

        public Constants.GlobalConstants.Result BlockGetPacket(List<byte> cosempkt, Constants.DLMSConstants.DLMSCommand _commandname)
        {
            int datastartindex = 17;

            basehdlc hdlc = DLMSFactory.HDLCObjectMapper[GlobalConstants.GlobalMeterType];

            ConfigureData configdat = new ConfigureData() { setdata = cosempkt, commandname = _commandname };

            List<byte> cosemdatapkt = DLMSFactory.CosemObjectMapper[Constants.DLMSConstants.DLMSCommand.NORMALGET].GetPacket(configdat);

            List<byte> hdlcdatapkt = hdlc.GetNormalHDLCPacket(cosemdatapkt, serialcom.ReceiveBuffer[8]);

            Thread.Sleep(100);

            if (!serialcom.SendDataToPort(hdlcdatapkt.ToArray(), hdlcdatapkt.Count))
                return Constants.GlobalConstants.Result.Fail;

            hdlc.fIncRecieve();
            if (!hdlc.dlmsFrameValidation(serialcom.ReceiveBuffer.ToList(), hdlc.nCMDByte))
            {
                Logger.Logger.WriteCalibLog("Cosem Packet Error", serialcom.mpos);
                return Constants.GlobalConstants.Result.Fail;
            }

            int ret = fCheckCOSEMResponse(serialcom.ReceiveBuffer,ref configdat);
                if (ret == 0x01) return Constants.GlobalConstants.Result.Pass;
            else if (ret == 0x05) return Constants.GlobalConstants.Result.Fail;
            else if (ret == 0x07) return Constants.GlobalConstants.Result.Fail;
            else if (ret != 0x02) return Constants.GlobalConstants.Result.Fail;
            while (true)
            {
                cosempkt = new List<byte>();
                cosempkt.Add(0xC0);
                cosempkt.Add(0x02);
                cosempkt.Add(0x81);
                cosempkt.Add(0x00);
                cosempkt.Add(0x00);
                cosempkt.Add(Convert.ToByte(configdat.nBlockNumber >> 8));
                cosempkt.Add(Convert.ToByte(configdat.nBlockNumber & 0xFF));
                configdat.setdata = cosempkt;
                cosemdatapkt = DLMSFactory.CosemObjectMapper[Constants.DLMSConstants.DLMSCommand.NORMALGET].GetPacket(configdat);
                    hdlcdatapkt = hdlc.GetBlockHDLCPacket(cosemdatapkt, serialcom.ReceiveBuffer[8]);
                    if (!serialcom.SendDataToPort(hdlcdatapkt.ToArray(), hdlcdatapkt.Count))
                            return Constants.GlobalConstants.Result.Fail;
                        hdlc.fIncRecieve();
                        if (!hdlc.dlmsFrameValidation(serialcom.ReceiveBuffer.ToList(), hdlc.nCMDByte))
                        {
                            Logger.Logger.WriteCalibLog("Cosem Packet Error", serialcom.mpos);
                            return Constants.GlobalConstants.Result.Fail;
                        }

                        ret = fCheckCOSEMResponse(serialcom.ReceiveBuffer, ref configdat);
                        if (ret == 0x01) break;
                            else if (ret == 0x02) continue;
             }
                return Constants.GlobalConstants.Result.Pass;
        }
        public int fCheckCOSEMResponse(byte[] Buffer, ref ConfigureData configdat)
        {
            int nCosemIndex = 14;
            if (Buffer[nCosemIndex] != 0xC4) return 0x03;//Tag Mismatch  //Get.response
            nCosemIndex = nCosemIndex + 1;
            if (Buffer[nCosemIndex] == 0x02)   //Get.response.Next
            {
                nCosemIndex = nCosemIndex + 2;
                if (Buffer[nCosemIndex] == 0x00)   //Get.response.Next
                {
                    nCosemIndex = nCosemIndex + 3;


                    configdat.nBlockNumber = 0;
                    configdat.nBlockNumber = configdat.nBlockNumber | (int)Buffer[nCosemIndex] << 8;
                    nCosemIndex++;
                    configdat.nBlockNumber = configdat.nBlockNumber | (int)Buffer[nCosemIndex];

                    nCosemIndex = nCosemIndex + 2;
                    int nBlockByteCount = Buffer[nCosemIndex];
                    if (Buffer[nCosemIndex] == 0x82) { nBlockByteCount = ((byte)(Buffer[nCosemIndex + 1] & 0x1F) * 0x100 + (byte)(Buffer[nCosemIndex + 2])); nCosemIndex += 2; }
                    else if (Buffer[nCosemIndex] == 0x81) { nBlockByteCount = ((byte)(Buffer[nCosemIndex + 1])); nCosemIndex += 1; }
                    nCosemIndex = nCosemIndex + 1;  //Skipping the block Count Byte
                    for (int i = 0; i < nBlockByteCount; i++)
                    {
                        serialcom.BlockBuffer[serialcom.nBlockIndex++] = Buffer[nCosemIndex++];
                    }
                    return 0x02;
                }
                else
                {
                    nCosemIndex = nCosemIndex + 6;
                    int nBlockByteCount = Buffer[nCosemIndex];
                    if (Buffer[nCosemIndex] == 0x82) { nBlockByteCount = ((byte)(Buffer[nCosemIndex + 1] & 0x1F) * 0x100 + (byte)(Buffer[nCosemIndex + 2])); nCosemIndex += 2; }
                    else if (Buffer[nCosemIndex] == 0x81) { nBlockByteCount = ((byte)(Buffer[nCosemIndex + 1])); nCosemIndex += 1; }
                    nCosemIndex = nCosemIndex + 1;  //Skipping the block Count Byte
                    for (int i = 0; i < nBlockByteCount; i++)
                    {
                        serialcom.BlockBuffer[serialcom.nBlockIndex++] = Buffer[nCosemIndex++];
                    }

                    configdat.nBlockTotalByteCount = serialcom.nBlockIndex;
                    configdat.nBlockNumber = 0x00;
                    serialcom.nBlockIndex = 0x00;
                    return 0x01; ///Block Trasfer Completed
                }
            }
            else
            {
                nCosemIndex = nCosemIndex + 2;
                if (Buffer[nCosemIndex] == 0x00)   //Get.response.Normal
                {
                    //code written to handle tamper compartment readout when data comes in normal transfer 
                    int nBlockByteCount = Buffer[nCosemIndex + 2];
                    if (nBlockByteCount == 0x00)
                    {

                        return 0x07; ///Get.Request.normal
                    }
                    else
                    {

                        nBlockByteCount = (byte)(Buffer[nCosemIndex - 16] & 0x1F) * 0x100 + (byte)(Buffer[nCosemIndex - 15]);
                        nBlockByteCount = nBlockByteCount - 20;

                        if (Buffer[nCosemIndex] == 0x82) { nBlockByteCount = ((byte)(Buffer[nCosemIndex + 1] & 0x1F) * 0x100 + (byte)(Buffer[nCosemIndex + 2])); nCosemIndex += 2; }
                        else if (Buffer[nCosemIndex] == 0x81) { nBlockByteCount = ((byte)(Buffer[nCosemIndex + 1])); nCosemIndex += 1; }

                        nCosemIndex = nCosemIndex + 1;
                        for (int i = 0; i <= nBlockByteCount; i++)  //included = in the condition for tamper compartment readout 
                        {
                            serialcom.BlockBuffer[serialcom.nBlockIndex++] = Buffer[nCosemIndex++];
                        }

                        configdat.nBlockTotalByteCount = serialcom.nBlockIndex;
                        configdat.nBlockNumber = 0x00;
                        serialcom.nBlockIndex = 0x00;
                        return 0x01; ///Get.Request.normal
                    }

                }
                else
                {
                    return 0x05; ///Get.Request.normal
                }
            }

        }
        /// <summary>
        /// Write Block data in meter
        /// </summary>
        /// <param name="filedata"></param>
        /// <returns></returns>
        public Constants.GlobalConstants.Result WriteBlock(string filedata)
        {
            int itoltallength = filedata.Length / 2;
            int iDiv = itoltallength / 2;
            int iRem = itoltallength % 2;
            int itotalpkt = iDiv + iRem;
            int datastartindex = 17;
            string[] strarr = filedata.Split(',');

            if (strarr == null || strarr.Count() < 3) return GlobalConstants.Result.Fail;
           
            List<string> mlist = splitmystring(strarr[1], Constants.DLMSConstants.PacketSize * 2);

            Logger.Logger.WriteCalibLog("Block Transfer Started...", serialcom.mpos);

            basehdlc hdlc = DLMSFactory.HDLCObjectMapper[GlobalConstants.GlobalMeterType];

            for (int i = 0; i < mlist.Count; i++)
            {
                byte[] barr = convertStringtobyteArr(mlist[i]);
                byte blastpkt = 0;
                if (i == mlist.Count - 1) blastpkt = 1;
                ConfigureData configdata = new ConfigureData();
                configdata.packetnumber = i;
                configdata.setdata = new List<byte>();
                List<byte> cosemdatapkt = new List<byte>();
                configdata.lastpacket = blastpkt;
                configdata.setdata = new List<byte>();
                configdata.setdata.AddRange(barr);

                if (i == 0)
                  cosemdatapkt = DLMSFactory.CosemObjectMapper[Constants.DLMSConstants.DLMSCommand.WRITEFIRSTBLOCK].GetPacket(configdata);
                else
                  cosemdatapkt = DLMSFactory.CosemObjectMapper[Constants.DLMSConstants.DLMSCommand.WRITENEXTBLOCK].GetPacket(configdata);
                
                List<byte> hdlcdatapkt = hdlc.GetNormalHDLCPacket(cosemdatapkt, serialcom.ReceiveBuffer[8]);

                if (!serialcom.SendDataToPort(hdlcdatapkt.ToArray(), hdlcdatapkt.Count))
                    return Constants.GlobalConstants.Result.Fail;
                hdlc.nCMDByte = serialcom.ReceiveBuffer[8];//----------Config Issue
                if (!hdlc.dlmsFrameValidation(serialcom.ReceiveBuffer.ToList(), hdlc.nCMDByte))
                {
                    Logger.Logger.WriteCalibLog("Cosem Packet Error", serialcom.mpos);
                    return Constants.GlobalConstants.Result.Fail;
                }

                if (serialcom.bufferIndex <= datastartindex || serialcom.ReceiveBuffer[datastartindex] != 0x00)
                {
                    Logger.Logger.WriteCalibLog("Cosem Packet Error", serialcom.mpos);
                    return Constants.GlobalConstants.Result.Fail;
                }
                //Logger.Logger.WriteCalibLog("Received response Byte -> " + serialcom.ReceiveBuffer[17].ToString("X2"), serialcom.mpos); //--7063 Debug Code
                Logger.Logger.WriteCalibLog("Packet No " + (i+1).ToString() + " -> Pass", serialcom.mpos);

            }
           
            Logger.Logger.WriteCalibLog("Block Completed Successffully", serialcom.mpos);

            return Constants.GlobalConstants.Result.Pass; 

        }


        /// <summary>
        /// WriteSetAesTable
        /// </summary>
        /// <param name="filedata"></param>
        /// <returns></returns>
        public Constants.GlobalConstants.Result WriteSetAesTable(string filedata)
        {
            int itoltallength = filedata.Length / 2;
            int iDiv = itoltallength / 2;
            int iRem = itoltallength % 2;
            int itotalpkt = iDiv + iRem;
            string[] strarr = filedata.Split(',');

            if (strarr == null || strarr.Count() < 3) return GlobalConstants.Result.Fail;

            List<string> mlist = splitmystring(strarr[1], 0x40 * 2);

            Logger.Logger.WriteCalibLog("AES Transfer Started...", serialcom.mpos);

            basehdlc hdlc = DLMSFactory.HDLCObjectMapper[GlobalConstants.GlobalMeterType];

            for (int icount = 0; icount < mlist.Count; icount++)
            {
                byte[] barr = convertStringtobyteArr(mlist[icount]);
                byte blastpkt = 0;
                if (icount == mlist.Count - 1) blastpkt = 1;
                ConfigureData configdata = new ConfigureData();
                configdata.packetnumber = icount;
                configdata.setdata = new List<byte>();
                List<byte> cosemdatapkt = new List<byte>();
                configdata.lastpacket = blastpkt;
                configdata.setdata = new List<byte>();
                configdata.setdata.AddRange(barr);

                cosemdatapkt = DLMSConstants.GlobalNonAMI1PMeterObject[Constants.DLMSConstants.DLMSCommand.WRITEAES].mclobisatt.ToList();

                UInt16 wdataaddress = (UInt16)(0x3D + (64 * icount));

                cosemdatapkt.Add((byte)(wdataaddress >> 8));
                cosemdatapkt.Add((byte)wdataaddress);

                cosemdatapkt.Add( 0x09);
                cosemdatapkt.Add((byte)barr.Count());

                cosemdatapkt.AddRange(barr);
                
                List<byte> hdlcdatapkt = hdlc.GetNormalHDLCPacket(cosemdatapkt, serialcom.ReceiveBuffer[8]);

                if (!serialcom.SendDataToPort(hdlcdatapkt.ToArray(), hdlcdatapkt.Count))
                    return Constants.GlobalConstants.Result.Fail;

                if (!hdlc.dlmsFrameValidation(serialcom.ReceiveBuffer.ToList(), hdlc.nCMDByte))
                {
                    Logger.Logger.WriteCalibLog("Cosem Packet Error", serialcom.mpos);
                    return Constants.GlobalConstants.Result.Fail;
                }

                if (serialcom.ReceiveBuffer[17] != 0x00)
                {
                    Logger.Logger.WriteCalibLog("Cosem Packet Error", serialcom.mpos);
                    return Constants.GlobalConstants.Result.Fail;
                }

                Logger.Logger.WriteCalibLog("Packet No " + (icount + 1).ToString() + " -> Pass", serialcom.mpos);

            }

            Logger.Logger.WriteCalibLog("Block Completed Successffully", serialcom.mpos);

            return Constants.GlobalConstants.Result.Pass;

        }


        /// <summary>
        /// Set Packet in  meter 
        /// </summary>
        /// <param name="strdata"></param>
        /// <returns></returns>
        public Constants.GlobalConstants.Result SetWritePacket(string strdata)
        {
            int datastartindex = 17;

            basehdlc hdlc = DLMSFactory.HDLCObjectMapper[GlobalConstants.GlobalMeterType];

            byte[] barr = convertStringtobyteArr(strdata);

            if(barr == null || barr.Count() < 1)
                return Constants.GlobalConstants.Result.Fail;

            ConfigureData configdata = new ConfigureData();

            configdata.setdata = new List<byte>();

            configdata.setdata.AddRange(barr);

            List<byte> cosemdatapkt = DLMSFactory.CosemObjectMapper[Constants.DLMSConstants.DLMSCommand.WRITESET].GetPacket(configdata);

            List<byte> hdlcdatapkt = hdlc.GetNormalHDLCPacket(cosemdatapkt, serialcom.ReceiveBuffer[8]);

            if (!serialcom.SendDataToPort(hdlcdatapkt.ToArray(), hdlcdatapkt.Count))
                return Constants.GlobalConstants.Result.Fail;
          
            if (!hdlc.dlmsFrameValidation(serialcom.ReceiveBuffer.ToList(), hdlc.nCMDByte))
            {
                Logger.Logger.WriteCalibLog("Cosem Packet Error", serialcom.mpos);
                return Constants.GlobalConstants.Result.Fail;
            }
            hdlc.nCMDByte = serialcom.ReceiveBuffer[8];//----------Config Issue
            if (serialcom.bufferIndex <= datastartindex || serialcom.ReceiveBuffer[datastartindex] != 0x00)
            {
                Logger.Logger.WriteCalibLog("Cosem Packet Error", serialcom.mpos);
                return Constants.GlobalConstants.Result.Fail;
            }
            //Logger.Logger.WriteCalibLog("Received response Byte -> " + serialcom.ReceiveBuffer[datastartindex].ToString("X2"), serialcom.mpos); //--7063 Debug Code

            Logger.Logger.WriteCalibLog("Normal Set Command -> Pass", serialcom.mpos);

            return Constants.GlobalConstants.Result.Pass;

        }

        /// <summary>
        /// Method Name : convertStringtobyteArr
        /// Purpose : This method is used to convert string data hex format to byte array
        /// </summary>
        /// <param name="strdata"></param>
        /// <returns></returns>
        public byte[] convertStringtobyteArr(string strdata)
        {
            List<byte> bretdata = new List<byte>();
            if (strdata == null || strdata.Length == 0) return null;
            if ((strdata.Length % 2) != 0) return null;
            int icount = 0;
            while (true)
            {
                string str = strdata.Substring(icount, 2);
                icount += 2;
                bretdata.Add(byte.Parse(str, System.Globalization.NumberStyles.AllowHexSpecifier));
                if (icount >= strdata.Length) break;
            }

            return bretdata.ToArray();

        }

        /// <summary>
        /// split strin based on size
        /// </summary>
        /// <param name="strdata"></param>
        /// <param name="isize"></param>
        /// <returns></returns>
        public List<string> splitmystring(string strdata, int isize)
        {
            List<string> a = strdata.ToCharArray().Select((c, i) => new { Char = c, Index = i }).GroupBy(o => o.Index / isize).Select(g => new String(g.Select(o => o.Char).ToArray())).ToList();
            return a;
        }

    }
}
