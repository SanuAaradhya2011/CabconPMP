using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCalibration.Actions
{
    /// <summary>
    /// Class for events
    /// </summary>
    public class UpdateEventArgs : System.EventArgs
    {
        // member variables
        public string mmsg;
        public bool misError;
        public int mipos;
        public Constants.GlobalConstants.Result miResult;
        public int miAct;
        public string meterdetail;
        public string argument; 

        // constructor
        public UpdateEventArgs(string msg, bool isError, int pos, Constants.GlobalConstants.Result iResult, int iAct, object _meterdetails, string cmdargument)
        {
            mmsg = msg;
            misError = isError;
            mipos = pos;
            miResult = iResult;
            miAct = iAct;
            meterdetail = (string)_meterdetails;
            argument = cmdargument;
        }
    }
}
