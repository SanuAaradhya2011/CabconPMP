using SmartCalibration.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCalibration.Communication
{
    public class basecosem
    {
        public virtual List<byte> GetPacket(ConfigureData configureData)
        {
            return null;
        }


    }

    public class ConfigureData
    {
        public List<byte> setdata;
        public byte lastpacket { get; set; }
        public string blockdata { get; set; }
        public int packetnumber { get; set; }
        public Constants.DLMSConstants.DLMSCommand commandname;
        public string tagname { get; set; }
        public int delay { get; set; }
        public byte bCMDByte { get; set; }

        public int nBlockTotalByteCount = 0x00;
        public int nBlockNumber = 0x00;
    }

    
}
