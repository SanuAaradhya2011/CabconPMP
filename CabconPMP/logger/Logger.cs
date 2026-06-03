using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCalibration.Logger
{
    public class Logger
    {
        // enum to identify the actions
        public enum ActionMode
        {
            TX,
            RX,
            Open,
            Close,
        }

        public static void WriteCalibLog(string msg, int pos)
        {
            string strfilename =  AppDomain.CurrentDomain.BaseDirectory +  "Caliblog" + pos.ToString("D2") + ".log";
            string logdata = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss ") + " --> ";
             File.AppendAllText(strfilename, logdata + msg + "\n");
        }

        public static void WriteComLog(byte[] buffer, int count, int pos, ActionMode mode)
        {
            string strfilename = AppDomain.CurrentDomain.BaseDirectory + "act" + pos.ToString("D2") + ".log";
            string logdata = string.Empty;

            try
            {
                string strmode = ">> ";
                if (mode == ActionMode.RX) strmode = "<< ";
                logdata = strmode + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss ") + "Total Bytes: " + count.ToString("d2") + " --------------------------------";
                // In one line 96 chars will be shown for better visibility
                logdata = logdata.PadRight(96, '-');

                for (int _count = 0; _count < count; _count++)
                {
                    if (_count % 32 == 0) logdata += "\n";
                    logdata += buffer[_count].ToString("X2") + " ";
                }

                logdata += "\n";

                File.AppendAllText(strfilename, logdata);
            }
            catch (Exception ex)
            {
                WriteCalibLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + "->"
                                        + System.Reflection.MethodBase.GetCurrentMethod().Name + "->" + ex.Message, pos);
            }
        }


        public static void LogNeutralCurrent(string arg_strMsg, int mpos)
        {
            //File open
            string strfilename = AppDomain.CurrentDomain.BaseDirectory + "Neutral Current" + mpos.ToString("D2") + ".lg";
            string strTime; //, strMsg; //, strBytes, strTmp, strMsg, strData((LPCTSTR) Data, Len);
            strTime = @DateTime.Now.Day.ToString() + "/" +
                         DateTime.Now.Month.ToString() + "/" +
                         DateTime.Now.Year.ToString() + "-" +
                         DateTime.Now.Hour.ToString("d2") + ":" +
                         DateTime.Now.Minute.ToString("d2") + ":" +
                         DateTime.Now.Second.ToString("d2") + "-> Position: " + mpos.ToString();


            File.WriteAllText(strfilename, strTime + " -> Neutral Current:" + arg_strMsg + "\n");
           

        }

        public static void UpdateLogNeutralCurrent(string arg_strMsg)
        {
            //File open
           // string strfilename = AppDomain.CurrentDomain.BaseDirectory + "Neutral Current.log";
            

           // File.AppendAllText(strfilename, arg_strMsg);


        }
    }
}
