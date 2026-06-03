using IntegratedCalibration.Constants;
using IntegratedCalibration.DataLayer;
using SmartCalibration.Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IntegratedCalibration.Communication
{
    public class SPS2BootMot
    {
        public List<BSLCommandPacketList> GetBSLCoreCommand(string strfilename, int maxBlocksize, bool chkbslFastUpgrade)
        {
            try
            {
                StreamReader streamReader = new StreamReader(strfilename, Encoding.UTF8);
                string text = streamReader.ReadToEnd();
                string text1 = text;
                text = text.Replace("q", "");//remove the last char from the file most of the time it will be q, unused charachter
                text = text.Replace("\r\n", "\n");
                streamReader.Close();
                List<string> datalist = new List<string>();
                List<BSLCommandPacketList> BSLCommandList = new List<BSLCommandPacketList>();
                datalist.AddRange(text.Split('@'));

                foreach (string item in datalist)
                {
                    if (item.Length <= 0) continue;
                    BSLCommandPacketList objbl = new BSLCommandPacketList();
                    string addressBytes = GenericMethods.FormatData(GenericMethods.ConvertStrToByte(item.Substring(0, item.IndexOf('\n'))).ToArray(), false);

                    string dataBytes = item.Substring(item.IndexOf('\n'));
                    List<string> datalistAddresWise = new List<string>();
                    dataBytes = dataBytes.Replace("\n", "");

                    dataBytes = dataBytes.Replace(" ", "");
                    int dataByteIndex = 0;
                    while (dataByteIndex < dataBytes.Length)
                    {
                        objbl = new BSLCommandPacketList();
                        string datapacket = "";
                        if (dataBytes.Length - dataByteIndex > maxBlocksize * 2) datapacket = dataBytes.Substring(dataByteIndex, maxBlocksize * 2);
                        else datapacket = dataBytes.Substring(dataByteIndex);
                        if (chkbslFastUpgrade) objbl.BSLCommandData = CreateBSLCoreCommand(addressBytes, datapacket, (byte)BSLCommandPacketList.BSLCoreCommandID.RXDataBlockFast);
                        else objbl.BSLCommandData = CreateBSLCoreCommand(addressBytes, datapacket, (byte)BSLCommandPacketList.BSLCoreCommandID.RXDataBlock);

                        addressBytes = (Convert.ToInt32(addressBytes) + datapacket.Length / 2).ToString();
                        dataByteIndex += datapacket.Length;
                        BSLCommandList.Add(objbl);
                    }
                }

                return BSLCommandList;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<byte> CreateBSLCoreCommand(string addressBytes, string dataBytes, byte bsclCommandType)
        {
            List<byte> commandPcket = new List<byte>();
            commandPcket.Add(bsclCommandType);
            if (addressBytes.Length > 0)
            {
                List<byte> Address = GenericMethods.ConvertStrToByte(Convert.ToInt32(addressBytes).ToString("X6"));
                Address.Reverse();
                commandPcket.AddRange(Address);
            }
            List<byte> Data = GenericMethods.ConvertStrToByte(dataBytes);
            commandPcket.AddRange(Data);
            CRC16 objcrc16 = new CRC16(CRC16.Crc16Mode.CcittKermit);
            string CRCbyte = objcrc16.CalcCRC16(commandPcket.ToArray());
            List<byte> commandCRC = GenericMethods.ConvertStrToByte(CRCbyte);//FB91
            commandCRC.Reverse();
            commandPcket.AddRange(commandCRC);
            return commandPcket;
        }

        public List<byte> CreateUARTBSLCommand(List<byte> bslCommand)
        {
            List<byte> urtBslCommand = new List<byte>();
            urtBslCommand.Add(BSLCommandPacketList.URTcommandHeader);//UART BSL Command Header
            string lengthofBSLCoreCommand = (bslCommand.Count - 2).ToString("X4");//BSL Command - CRC length
            List<byte> DataLength = GenericMethods.ConvertStrToByte(lengthofBSLCoreCommand);
            DataLength.Reverse();
            urtBslCommand.AddRange(DataLength);// BSL Command Length
            urtBslCommand.AddRange(bslCommand);// BSL Command 
            return urtBslCommand;
        }
    }
}
