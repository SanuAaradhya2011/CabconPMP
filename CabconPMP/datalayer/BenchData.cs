using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCalibration.DataLayer
{
    public class BenchData
    {
        public static UInt32 rvoltage;
        public static UInt32 yvoltage;
        public static UInt32 bvoltage;
        public static UInt32 rcurrent;
        public static UInt32 ycurrent;
        public static UInt32 bcurrent;
        public static double rpowerfactor;
        public static double ypowerfactor;
        public static double bpowerfactor;
        public static UInt32 ractivepower;
        public static UInt32 yactivepower;
        public static UInt32 bactivepower;
        public static UInt32 rreactivepower;
        public static UInt32 yreactivepower;
        public static UInt32 breactivepower;

        public enum SAMPLEINDEX
        {
            VR = 0,
            VY,//1
            VB,//2
            IR,//3
            IY,//4
            IB,//5
            RANGLE = 9,//5
            YANGLE = 10,//6
            BANGLE = 11,//7
            RACTIVEPOWER = 12,//8
            YACTIVEPOWER = 13,//9
            BACTIVEPOWER = 14,//10
            RREACTPOWER = 15,//11
            YREACTPOWER = 16,//12
            BREACTPOWER = 17,//13
        }
    }
}
