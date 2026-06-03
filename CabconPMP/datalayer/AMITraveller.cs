using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCalibration.DataLayer
{
    /// <summary>
    /// Traveller Command Handler
    /// Read Travelelr Status and Write Traveller Status
    /// Author: Mohsin Raza
    /// </summary>
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
                Logger.Logger.WriteCalibLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + "->"
                                       + System.Reflection.MethodBase.GetCurrentMethod().Name + "->" + ex.Message, pos);
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
                Logger.Logger.WriteCalibLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + "->"
                                      + System.Reflection.MethodBase.GetCurrentMethod().Name + "->" + ex.Message, pos);
            }

            return false;
        }
    }
}
