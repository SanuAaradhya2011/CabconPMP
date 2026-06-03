using SmartCalibration.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCalibration.Constants
{
    public class GlobalConstants
    {
        public static int ActiveTask = 0;
        public static UInt16 MAXSM110SAMPLES = 10;
        public static byte MAXSM310SAMPLES = 10;
        public static byte MAXMETERPOSITION;
        public static UInt16 MAXCMDTIMEOUTSM110 = 8000;
        public static UInt16 MAXCMDTIMEOUTOTHERSCMD = 15000;
        public static UInt16 MAXCMDTIMEOUTSM310RESET = 60000;
        public static string metersignature;
        public static MeterType GlobalMeterType;
        public static string IMPORTFILE = "Imp.res";
        public static string CONFIGFILE = "config.ini";
        public static string AEMCALSAMPLEFILE = "3pcalvalues.txt";
        public static string BSLPASSWORDFILE = "BSLPassword.txt";
        public static string CONFIGURE = "CONFIGURE";
        public static string FWUPGRADE = "BOOTLOADER";
        public static string SETPWD = "SETPWD";
        public static string GETMSN = "GETMSN";
        public static string CALIBTEMP = "CALIBTEMP";
        public static string BSLPassword = "";//"ffff9E734A732671CE74AA749C73ffffD0711A73ffffffffffffffffffffF670";//"FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";//

        public static string[] MSNdata = new string[100];
        public static Dictionary<int, string> MeterPortMap = new Dictionary<int, string>();

        public static int SM310_MAXSAMPLECOLLECTIONTIME = 12000;
        public static int SM110_MAXSAMPLECOLLECTIONTIME = 10000;
        public static bool IsRetryMode = false;

        static GlobalConstants()
        {
            MAXMETERPOSITION = 100;
        }

        public static Dictionary<GlobalConstants.MeterType, GenericAction> GlobalActionMapper = new Dictionary<GlobalConstants.MeterType, GenericAction>()
        {
            { GlobalConstants.MeterType.SM110, new SM110Action()},
            { GlobalConstants.MeterType.SM310, new SM310Action()},
            { GlobalConstants.MeterType.NONAMI1P, new NonAMI1PAction()},
            { GlobalConstants.MeterType.NONAMI3P, new NonAMI3PAction()},
            { GlobalConstants.MeterType.SPS2, new SPS2Action()},

        };

        public static Dictionary<string, GlobalConstants.MeterType> GlobalMeterTypeMapper = new Dictionary<string, GlobalConstants.MeterType>()
        {   
            { "SM110", GlobalConstants.MeterType.SM110},
            { "SM310",GlobalConstants.MeterType.SM310},
            { "1PDLMS",GlobalConstants.MeterType.NONAMI1P},
            { "3PDLMS",GlobalConstants.MeterType.NONAMI3P},
            { "3PDLMSSPS2",GlobalConstants.MeterType.SPS2},
        };

        public static Dictionary<GlobalConstants.MeterType, byte> SourceAddress = new Dictionary<GlobalConstants.MeterType, byte>()
        {
           
            { GlobalConstants.MeterType.SM110, 0xFD},
            { GlobalConstants.MeterType.SM310, 0xFD},
            { GlobalConstants.MeterType.NONAMI1P, 0x81},
            { GlobalConstants.MeterType.NONAMI3P, 0x81},
            { GlobalConstants.MeterType.SPS2, 0x81},
        };

        public static Dictionary<Result, string> GlobalErrorTable = new Dictionary<Result, string>()
        {
            { Result.Init, "Init"},
            { Result.Pass, " [Ok]"},
            { Result.Fail, " [Fail]"},
            { Result.OUT, "  [Out]"},
            { Result.NA,   " [NA]"},
            { Result.Retry, " [Retrying...]"},
            { Result.HF, " [HF]"},
            { Result.OTHER, " [OT]"},
            { Result.RWD, " [RWD]"},
            { Result.UNDEFINED, " [Fail]"},
            { Result.ERRFILE, " [ErrFIle]"},
        };

        public enum MeterType
        {
            SM110,
            SM310,
            NONAMI1P,
            NONAMI3P,
            SPS2,
            
        }

        public enum Result
        {
            Init,
            Pass,
            Fail,
            OUT,
            NA,
            Retry,
            HF,
            RWD,
            OTHER,
            UNDEFINED,
            ERRFILE,

        }


       

    }
}
