using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCalibration.DataLayer
{
    public class ConfigHandler
    {
        public string baudrate { get; set; }
        public string defualtpath { get; set; }
        public string meterpassword { get; set; }
        public string defualtimagepath { get; set; }

        public bool FillConfuguration()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + Constants.GlobalConstants.CONFIGFILE))
                return false;

            string[] dataarray = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + Constants.GlobalConstants.CONFIGFILE);

            if (dataarray == null) return false;

            if(dataarray.Count() > 0)
               baudrate = dataarray[0];
            
            if (dataarray.Count() > 1)
               defualtpath = dataarray[1];
           
            if (dataarray.Count() > 2)
                meterpassword = dataarray[2];

            if (dataarray.Count() > 3)
                defualtimagepath = dataarray[3];

            return true;
        
        }
    }
}
