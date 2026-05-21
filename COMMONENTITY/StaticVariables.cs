using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMMONENTITY
{
  public  class StaticVariables
    {
      public enum ExecutedMeterType { Smart_Meter_1PH = 0, MicroStar_DLMS = 1, Smart_Meter_3PH = 2, DLMS_3PH = 3, SAPPHIRE = 4, DLMS_3PH_RUBY = 5, Non_DLMS_1PH = 6, SAPPHIRE_S2 = 7 };
        public enum userCategory { Useroperator = 0, Uservendor = 1, Usersupervisor = 2, Userreader = 3, Useradministrator = 4, UserPoweradministrator = 5, UserValidation=6, UserRework=7 };
        public enum MagnetTest { LeftSensor = 0, RightSensor = 1};
        public enum ProduTraveler { EMSTest = 0, FunctionTest = 1, CalibrationTest = 2, SirializationTest = 3 };
        public enum MeterBuffer1PHStatusFlg {  RelayMalFunctionStatus = 0, MainBatteryVoltage = 1, RTCBatteryVoltage = 2, PushButtonPressCounter = 3, ACMagnetFieldCount = 4, CaseTamperCount = 5 };
        public enum MMITestParameters { VoltageTest = 0, PhaseCurrentTest = 1, NeutralCurrentTest = 2, FrequencyTest=3 };
        public enum ExecutionReurnStatus { Pass = 0, Fail = 1, ComFail = 2 };
        public enum MeterCurrentType { PhaseCurrent = 0, NeutralCurrent = 1 };
         public const string _SOH = "01";
        public const string _STX = "02";
        public const string _ETX = "03";
        public const string _EOT = "04";
        public const string _ACK = "06";
        public const string _NACK = "15";
        public const string _LF = "0A";
        public const string _CR = "0D";
        public const string _NAK = "15";
        public const string _ZERO = "30";
        public const string _R1 = "5231";
        public const string _R2 = "5232";
        public const string _R4 = "5234";
        public const string _W1 = "5731";
        public const string _W2 = "5732";
        public const string _W4 = "5734";
        public const string _D40 = "443430";
        public const string _E00 = "4530";
        public const string _IEC_READ = "30";
        public const string _IEC_WRITE = "31";
        public const string _IEC_MANF = "36";
        public const string _IEC_PRG = "63";
        public const string _BStart = "28";
        public const string _BEnd = "29";

        public const string Traveler_EMS_Add = "0015";
        public const string Traveler_FT_Add = "001D";
        public const string Traveler_CALI_Add = "0025";
        public const string Traveler_SERIAL_Add = "002D";

        public const string Traveler_EMS_3PhAdd = "0000";
        public const string Traveler_FT_3PhAdd = "0008";
        public const string Traveler_CALI_3PhAdd = "0010";
        public const string Traveler_SERIAL_3PhAdd = "0018";

        public const string  MeterType_EcoStar = "LGC110";        
        public const string  MeterType_SmartMeter1Phase = "SM110";
        public const string  MeterType_SmartMeter3Phase = "SM310";
        public const string MeterType_ZCE = "ZCE";
        //------------------For Falcon2 Smart Meter 1Phase Varients -----------------------
        public const string PhaseP1WH_SM110 = "315331";
        public const string NeutralP1WH_SM110 = "278125";
        public const string NeutralPMagSlopeLow_SM110 = "0";
        public const string SWTPhaseCompFactor_SM110 = "0";//"17170";//
        public const string SWTNeutralCompFactor_SM110 = "0";
        //------------------For Mico Star 5-30 Varients -----------------------
        public const string PhaseP1WH_Eco = "249966";
        public const string NeutralP1WH_Eco = "177875";
        public const string NeutralPMagSlopeLow_Eco = "0";
        public const string SWTPhaseCompFactor_Eco = "0";
        public const string SWTNeutralCompFactor_Eco = "0";
        //------------------For Mico Star 5-30 Varients -----------------------
       public const string PhaseP1WH_Micro1 = "7100740";
       public const string NeutralP1WH_Micro1  = "5397722";
       public const string Phase_P_1VARrH_Micro1 = "182801";
       public const string Neutral_P_1VArH_Micro1 = "139016";
       
      //------------------For Mico Star 10-60 Varients -----------------------
       //Limits Changed after Pawan Mail  24/11/2014 15:37
       public const string PhaseP1WH_Micro2 = "3557105";
       public const string NeutralP1WH_Micro2 = "3586334";
       public const string Phase_P_1VARrH_Micro2 = "91250";
       public const string Neutral_P_1VArH_Micro2 = "91760";

       //------------------For VIM 5-30 Varients -----------------------
       public const string PhaseP1WH_VIM1 = "5834630";
       public const string NeutralP1WH_VIM1_DC_IMMUNE_CT = "5193200";
       public const string NeutralP1WH_VIM1_DC_NONIMMUNE_CT = "4366860";
       public const string Phase_P_1VARrH_VIM1 = "149770";
       public const string Neutral_P_1VArH_VIM1 = "122775";

       //------------------For VIM 10-60 Varients -----------------------

       public const string PhaseP1WH_VIM2 = "2826230";
       public const string NeutralP1WH_VIM2 = "2840740";
       public const string Phase_P_1VARrH_VIM2 = "72500";
       public const string Neutral_P_1VArH_VIM2 = "72900";
      //-------------------------------------------------------------------
       public const string MMIWH10Value_ZCE = "0001DEE8";
       public const string PhCaliFactor_ZCE = "0400";


       public const string ERRORPreFix = "Error : ";
       public const string ERRORCOMMFailed = "COMM Failed ! ";
           

       public const string DataBaseConnectionFile = "DbConnection";

       public const string DisplayNotWorking = "Not-Working";
       public const string DisplayWorking = "Working";

       public const string PublicKeyEncryption = "09080706050403020109080706050403";

        public const string MeterType_1PH_E150_ES =  "E150-1PH-EcoStar";
        public const string MeterType_1PH_E150_MS = "E150-1PH-MicroStar";
        public const string MeterType_1PH_E150_MSD = "E150-1PH-MicroStar-DLMS";
        public const string MeterType_3PH_DLMS = "DLMS-3PH-Ruby-Puma"; //---Back Slash cann't assigned as creating path error
        public const string MeterType_3PH_Sapphire ="DLMS-3PH-SAPPHIRE";
        public const string MeterType_1PH_SM ="SM110-1PH-Smart Meter";
        public const string MeterType_3PH_SM ="SM310-3PH-Smart Meter";
        public const string MeterType_1PH_ZCE ="ZCE-1PH-Superstatic";  
    

        public const string TestType_FT = "Functional Test";
        public const string TestType_SR = "Serialization";
        public const string TestType_Cal = "Calibration";
        public const string TestType_EMS = "EMS Test";
        public const string TestType_OT = "Other Test";

        public const string FilePrefixMeterIDList = "MeterIDList";
        public const string FilePrefixRoutineTestReportFileFormat = "RTRFileFormat";


        private const string UserType_operator = "operator";
        private const string UserType_vendor = "vendor";
        private const string UserType_supervisor = "supervisor";
        private const string UserType_reader = "reader";
        private const string UserType_administrator = "administrator";
        private const string UserType_PowerAdministrator = "PowerAdministrator";
        private const string UserType_Validation = "Validation";
        private const string UserType_Rework = "Rework";

        public static List<string> ApplicationUserType = new List<string> { UserType_operator, UserType_vendor, UserType_supervisor, UserType_reader, UserType_administrator, UserType_PowerAdministrator, UserType_Validation, UserType_Rework };

        public enum ENMDisplayConfigStruct_1Phase
        {
            AutoScroll = 100,
            PushButton = 101,
            DisplayConfig = 102,
            TamperConfig = 103,
            TamperPersistance = 104,
            DemandIP = 105,
            BillingDateTime = 106,
            BillingCycle = 107,
            LoadControl = 108,
            TOUPriceSlab = 109,
            Resolution = 110,
            TamperThresholdOverVoltage = 111,
            TamperThresholdLowVoltage = 112,
            TamperThresholdOverLoad = 113,
            TamperThresholdOverCurrent = 114,
            EventLog = 115,
            EventAlarm = 116,
            SURVEYIP = 117,
            ARMBUTTON = 118,
            MagneticThreshold = 119,
            RS485DeviceStatus = 120,
            RS485DeviceAddress = 121,
            DisplayHR = 122,
            LSCaptureobject = 123,
            ESWFConfig= 124,
            LoadLimitValuekW= 125,
            PaymentMode= 126,
            MeteringMode= 127,
            OpticalPort= 128,
            RJPort= 129,


            THROverVoltagePersistence = 130,
            THROverVoltageThreshold = 131,
            THRLowVoltagePersistence = 132,
            THRLowVoltageThreshold = 133,
            THROverLoadPersistence = 134,
            THROverCurrentPersistence = 135,
            THROverCurrentThreshold = 136,
            THRTemperatureRisePersistence = 137,
            THRTemperatureRiseThreshold = 138,
            THRCurrentReversalPersistence = 139,
            THRCurrentReversalThreshold = 140,

            BatteryMode = 141, //----1P 128K DLMS
            FSModeLock = 142, 

            FutureDayProfile = 192,
            WeekProfile = 193,
            SeasonProfile = 194,
            FutureActivationDate = 195,
        }

        public enum ENMDisplayConfigStruct_3Ph
        {
            INTERFRAMETIMEOUT = 300,
            INACTIVITYTIMEOUT = 301,
            KVAHSELECTION = 302,
            REFVOLT = 303,
            RESOLUTION = 304,
            BILLINGDATETIME = 305,
            BILLINGCYCLE = 306,
            MDRESETLOCKOUTTIME = 307,
            LSCAPTUREOBJECT = 308,
            LSCAPTUREPERIOD = 309,
            INTEGRATIONPERIOD = 310,
            DISPPUSH = 311,
            DISPSCROLL = 312,
            DISPHR = 313,
            DISPTIMEOUT = 314,
            TAMPCOMPARTEMENT = 315,
            TAMPTHRESHOULD = 316,
            NOOFBILLONLCD = 317,
            MFGYEAR = 318,
            LOADCONTROL = 319,
            COPENSTATUS = 320,
            OVERVOLTAGE = 321,
            LOWVOLTAGE = 322,
            OVERLOAD = 323,
            TOUSLAB = 324,
            EVENTLOG = 325,
            ALARMLOG = 326,
            VERYLOWPF = 327,
            MISSINGPOTENTIAL = 328,
            NEUTRALDISTURBANCE = 329,
            MAGNETICINFLUENCE = 330,
            CURRENTREVERSAL = 331,
            CURRENTIMBALANCE = 332,
            OVERCURRENT = 333,
            CTOPEN = 334,
            CTSHORT = 335,
            VOLTAGEUNB = 336,
            POWERONOFF = 337,
            ARMBUTTON = 338,
            RS485DeviceAddress = 339,
            DisConnectOnMagnet = 340,
            TAMPCOMPARTEMENTCONFIG = 341,
            MeterType = 342,
            LCDBackLight = 343,
            CTRatio = 344,
            PTRatio = 345,
            AutoBilling = 346,
            RS232LockUnlock = 347,
            FactoryCongurabelity = 348,
            DemandIPSliding = 349,
            RJPortConfiguration = 350,
            OpticalPortConfiguration = 351,
            METERINGMODE = 352,
            ESWFCONFIG = 353,
            LOADLIMITVALUEKW = 354,
            DEMANDMETHOD = 355,
            PAYMENTMODE = 356,

            THRMissingPotentioalPersistence = 357,
            THRMissingPotentioalThreshold = 358,
            THRVoltageUnbalancePersistence = 359,
            THRVoltageUnbalanceThreshold = 360,
            THROverVoltagePersistence = 361,
            THROverVoltageThreshold = 362,
            THRLowVoltagePersistence = 363,
            THRLowVoltageThreshold = 364,
            THRCurrentReversalPersistence = 365,
            THRCurrentReversalThreshold = 366,
            THRCTOpenPersistence = 367,
            THRCTOpenThreshold = 368,
            THRCTByPassPersistence = 369,
            THRCTByPassThreshold = 370,
            THROverCurrentPersistence = 371,
            THROverCurrentThreshold = 372,
            THRCurrentUnbalancePersistence = 373,
            THRCurrentUnbalanceThreshold = 374,
            THRPowerONoffPersistence = 375,
            THRMangeneticInfluence = 376,
            THRNEUTRALDISTURBANCE = 377,
            THRVeryLowPFPersistence = 378,
            THRVeryLowPFThreshold = 379,
            THROverLoad = 380,
            THRHighNeutralCurrentPersistence = 381,
            THRHighNeutralCurrentThreshold = 382,
            THRTemperatureRisePersistence = 383,
            THRTemperatureRiseThreshold = 384,
            THRInvalidPhaseAssociationPersistence = 385,
          
            FutureDayProfile = 391,
            WeekProfile = 392,
            SeasonProfile = 393,
            SpecialDaysProfile = 394,
            FutureActivationDate = 395,

            FSModeLock =396,
        }

        public static Dictionary<byte, string> Get3PHDLMSConfigParaName()
        {
            Dictionary<byte, string> dictionaryPara = new Dictionary<byte, string>();
            dictionaryPara.Add(0x03, "Inter Frame Timeout");
            dictionaryPara.Add(0x04, "Inactivity Timeout");
            dictionaryPara.Add(0x05, "integration Period");
            dictionaryPara.Add(0x06, "KVAh Selection");
            dictionaryPara.Add(0x07, "CT Ratio");
            dictionaryPara.Add(0x08, "Resolution");
            dictionaryPara.Add(0x09, "Billing Date");
            dictionaryPara.Add(0x0A, "LS Capture object");
            dictionaryPara.Add(0x0B, "Tamper Limit");
            dictionaryPara.Add(0x0C, "LS Capture Period");
            dictionaryPara.Add(0x0D, "Display Para Push");
            dictionaryPara.Add(0x0E, "Display Para Scroll");
            dictionaryPara.Add(0x0F, "Display Param HR");
            dictionaryPara.Add(0x10, "Display Timeout");
            dictionaryPara.Add(0x11, "LCDBacklight");
            dictionaryPara.Add(0x12, "MDRestLockOut");
            dictionaryPara.Add(0x13, "TOU Season Profile");
            dictionaryPara.Add(0x14, "TOU Week Profile");
            dictionaryPara.Add(0x15, "TOU Day Profile");
            dictionaryPara.Add(0x16, "TOU Future Activation Date");
            dictionaryPara.Add(0x17, "Tamper Threshold");
            dictionaryPara.Add(0x18, "Manufacture Year");

            dictionaryPara.Add(0x19, "Number of Billing in Auto Scroll");
            dictionaryPara.Add(0x1A, "PT Ratio");
            dictionaryPara.Add(0x1B, "Sliding Integration Period");
            dictionaryPara.Add(0x1C, "Auto Billing Enable/Disable");
            dictionaryPara.Add(0x1D, "RS232 Lock/Unlock option");
            dictionaryPara.Add(0x1E, "Over Load Tamper Threshold Values");
            dictionaryPara.Add(0x1F, "Factory MMP Sheet");
            dictionaryPara.Add(0x20, "Unknown 1");
            dictionaryPara.Add(0x21, "Unknown 2");
            dictionaryPara.Add(0x22, "Unknown 3");
            dictionaryPara.Add(0x23, "Unknown 4");
            dictionaryPara.Add(0x24, "Unknown 5");
            return dictionaryPara;
        }

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

        public static List<string> GetTestType()
        {
            List<string> tType = new List<string>();
            tType.Add(TestType_FT);
            tType.Add(TestType_SR);
            tType.Add(TestType_Cal);
            tType.Add(TestType_EMS);
            tType.Add(TestType_OT);
            return tType;

        }

        public static List<string> SapphireCalibrationCofficient = new List<string>
        {
            "0000000000000000",//"to check non zero's value, If all zero the fail the test point
            "AB69B15FA4A12431",//"SapphireWCMNET24Mhz10601040"--It is for Sapphire WCM NET 24Mhz 10-60 ,10-40 Variants
            "AB69B15FA4A19B18",//"SapphireWCMNET20Mhz10601040"--It is same for Sapphire & Sapphire NET Meter 20Mhz 10-60 ,10-40 WCM Variants
            "7E69BB7632C8621E",//"SapphireWCMNET20Mhz20100"--It is same for Sapphire 20Mhz & Sapphire NET ( 20 Mhz) Meter 20-100 WCM Variants
            "D069107772C93B3D",//new-"SapphireWCMNET24Mhz20100"--It is for Sapphire NET  24  Meter 20-100 WCM Variants
            "27693F2BBB480D0B",//"SapphireWCMNET20Mhz0530"--It is same for Sapphire 20Mhz & Sapphire NET (24 & 20 Mhz) Meter 05-30 WCM Variants
            "1D6AA72B124A8416",//"SapphireWCMNET24Mhz0530"--It is same for Sapphire 24Mhz & Sapphire NET  Meter 05-30 WCM Variants


           //OLD "F9685811201DEB08",//"SapphireLTCTNET24Mhz05100506"--It is for Sapphire LTCT NET 24Mhz 05-10, 05-06 Variants
            "546A6C11CE1D1009",//"new-SapphireLTCTNET24Mhz05100506"--It is for Sapphire LTCT NET 24Mhz 05-10, 05-06 Variants
            "F9685811201D6C04",//"SapphireLTCTNET20Mhz05100506"--It is same for Sapphire LTCT NET 20Mhz & Sapphire 20Mhz 05-10, 05-06 Variants

            "38369E113C0F5802",//"SapphireHTCTNET"-- It is for Sapphire HTCT NET
            "5A3C9C0E1B0E2402",//"SapphireHTCT"-- It is for Sapphire HTCT 

            "3836CC5150468C0A",//"SapphireOLD_1"
            "3836652C8426DC05",//"SapphireOLD_2"
            "38361F6DAF5D350E"//"SapphireOLD_3"
        };
        public static List<List<string>> SmartMeter3PSMCalibrationCofficient = new List<List<string>>
        {
            //---------------Default Voltage Cofficient List---------------------- 
             new List<string>{"26836",//--SmartMeterWCM05-30
                              "27006",//--SmartMeterLTCT05-10, 05-06
                              "26900"},//--SmartMeterWCM10-60, 10-40 and 20-100
             //---------------Default Current Cofficient List--------------------- 
             new List<string>{"22222",//--SmartMeterWCM 20MHz 10-60, 10-40
                              "27250",//--SmartMeterWCM 24MHz 10-60, 10-40, Changed during Rating variants development
                              "27453",//--SmartMeterWCM 24MHz 20-100
                              "27100",//--SmartMeterWCM 20MHz 20-100
                              "11122",//--SmartMeterWCM 20 & 24 MHz 05-30
                              "4434",//--SmartMeterLTCT20Mhz 05-10, 05-06
                              "4464"},//--SmartMeterLTCT24Mhz 05-10, 05-06
              //---------------Default Active Power Cofficient List----------------
             new List<string>{"37350",//--SmartMeterWCM 20MHz 10-60, 10-40
                              "46121",//--SmartMeterWCM 24MHz 10-60, 10-40, 20-100
                              "45500",//--SmartMeterWCM 20MHz 20-100
                              "18651",//--SmartMeterWCM 20 & 24 MHz 05-30
                              "7481",//--SmartMeterLTCT20Mhz 05-10, 05-06
                              "7530"},//--SmartMeterLTCT24Mhz 05-10, 05-06
               //---------------Default Reactive Power Cofficient List--------------- 
             new List<string>{"5670",//--SmartMeterWCM 20MHz 10-60, 10-40
                              "14000",//--SmartMeterWCM 24MHz 10-60, 10-40,20-100
                              "6900",//--SmartMeterWCM 20MHz 20-100
                              "2843",//--SmartMeterWCM 20 & 24 MHz 05-30
                              "1135",//--SmartMeterLTCT20Mhz 05-10, 05-06
                              "2283"},//--SmartMeterLTCT24Mhz 05-10, 05-06
            
        };
    }

 
  public class GlobalMethods
  {
      private string lcdSegmentTestResponse;
      private string ledtestResponse;
      public string[] DayOfWeek = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
      public string[] ProgramNameList = new string[] { "E150ECOSTAR", "E150MICROSTAR","MICROSTARDLMS", "PMP3PHDLMS", "PMPSAPPHIRE", "PMPSM110", "PMPSM310", "ZCE",  };
      
      public string LEDTestResponse
      {
          get { return ledtestResponse; }
          set { ledtestResponse = value; }
      }

      public string LCDSegmentTestResponse
      {
          get { return lcdSegmentTestResponse; }
          set { lcdSegmentTestResponse = value; }
      }

   

     
  }
}
