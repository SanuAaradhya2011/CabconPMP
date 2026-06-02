using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class SystemSettingsAccess
    {
        public bool IS15959PART1Amendment5Clock 
        { 
            get { return SystemSettings.Default.IS15959Part1Amd5RTCFormat; } 
            set 
            { 
                SystemSettings.Default.IS15959Part1Amd5RTCFormat = value;
                SystemSettings.Default.Save();
            } 
        }
     
    }
}
