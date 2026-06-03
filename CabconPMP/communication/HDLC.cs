using LNG.Communication.SerialCommunication;
using SmartCalibration.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCalibration.Communication
{
    public class HDLC : basehdlc
    {
        private GlobalConstants.MeterType metertype;
      
        public HDLC(GlobalConstants.MeterType _metertype)
        {
            metertype = _metertype;
            sourceaddress = Constants.GlobalConstants.SourceAddress[metertype];
        }

    }
}
