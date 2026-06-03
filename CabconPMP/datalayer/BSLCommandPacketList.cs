using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntegratedCalibration.DataLayer
{
    public class BSLCommandPacketList
    {
        public List<byte> BSLCommandData { get; set; }
        public enum BSLResponseCommandID { DataBlock = 0x3A, BSLVersion = 0x3A, CRCValue = 0x3A, BSLResponseMessage = 0x3B };
        public enum BSLResponseMessage { OperationSuccessful = 0x00, Flashwritecheckfailed = 0x01, BSLlocked = 0x04, BSLpassworderror = 0x05, Unknowncommand = 0x07 }
        public enum BSLCoreCommandID { RXDataBlock = 0x10, RXDataBlockFast = 0x1B, RXPassword = 0x11, CRCCheck = 0x16, LoadPC = 0x17, TXDataBlock = 0x18 };
        public static byte URTcommandHeader = 0x80;

        public string GetBSLResponseMEssage(byte ResponseCode)
        {
            Dictionary<byte, string> ResponseMessage = new Dictionary<byte, string>();
            ResponseMessage.Add(0x00, "Operation Successful");
            ResponseMessage.Add(0x01, "Flash Write Check Failed");
            ResponseMessage.Add(0x04, "BSL Locked");
            ResponseMessage.Add(0x05, "BSL Password Error");
            ResponseMessage.Add(0x07, "Unknown Command");
            return ResponseMessage[ResponseCode];
        }
    }
}
