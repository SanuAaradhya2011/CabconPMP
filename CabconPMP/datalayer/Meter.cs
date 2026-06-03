using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
    public class Meter
    {
        public int ID { get; set; }
        public int mpos { get; set; }
        public int mact { get; set; }
        public string margument { get; set; }
        public string PortName { get; set; }
        public bool IsConnected { get; set; }
        public string RTCValue { get; set; }
        public string PCBAId { get; set; }
        public bool mstatus;
        public double Result_Error
        { get; set; }
        public Meter()
        {
            mstatus = false;
            //ActionResult = SmartCalibration.Constants.GlobalConstants.Result.Fail;
        }
    }
}
