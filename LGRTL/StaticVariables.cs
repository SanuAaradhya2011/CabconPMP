using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CabconRTL
{
    class StaticVariables
    {
        const string MeterType_1PH_E150_ES = "E150-1PH-EcoStar";
        const string MeterType_1PH_E150_MS = "E150-1PH-MicroStar";
        const string MeterType_1PH_E150_MSD = "E150-1PH-MicroStar-DLMS";
        const string MeterType_3PH_DLMS = "DLMS-3PH-Ruby-Puma";
        const string MeterType_3PH_Sapphire = "DLMS-3PH-SAPPHIRE";
        const string MeterType_1PH_SM = "SM110-1PH-Smart Meter";
        const string MeterType_3PH_SM = "SM310-3PH-Smart Meter";
        const string MeterType_1PH_ZCE = "ZCE-1PH-Superstatic"; 
        public const string DataBaseConnectionFile = "DbConnection";
        public const string PublicKeyEncryption = "09080706050403020109080706050403";
        public static List<string> GetMeterType()
        {
            List<string> mType = new List<string>();
            mType.Add(MeterType_1PH_E150_ES);
            mType.Add(MeterType_1PH_E150_MS);
            mType.Add(MeterType_1PH_E150_MSD);
            mType.Add(MeterType_3PH_DLMS);
            mType.Add(MeterType_3PH_Sapphire);
            mType.Add(MeterType_1PH_SM);
            mType.Add(MeterType_3PH_SM);
            mType.Add(MeterType_1PH_ZCE);
            return mType;

        }
    }
}
