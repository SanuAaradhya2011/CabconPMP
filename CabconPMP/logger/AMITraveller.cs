using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCalibration.Logger
{
    public class AMITraveller : basetraveller
    {
        public bool WriteTravellerStatus(int pos, string status)
        {
            string filename = AppDomain.CurrentDomain.BaseDirectory + "traveller" + pos.ToString("d2") + ".log";

            try
            {
                File.WriteAllText(filename, status.ToUpper());
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
           
        }

        public bool ReadTravellerStatus(int pos)
        {
            string filename = AppDomain.CurrentDomain.BaseDirectory + "traveller" + pos.ToString("d2") + ".log";

            try
            {
                string status = File.ReadAllText(filename);

                if (status.ToUpper().Contains("PASS"))
                     return true;
            }
            catch (Exception ex)
            {
                
            }

            return false;
        }
    }
}
