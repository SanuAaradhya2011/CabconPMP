using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public static class StaticConstantsCommon
    {

        public const string TypeAConfigurationFile = "MeterConfigurationCategoryA";
        public const string TypeBConfigurationFile = "MeterConfigurationCategoryB";

        public static string ProgramCRC = "ProgramCRC";
        public static string ProgramDescription = "ProgramDescription";
        public static string ConfigDescription = "ConfigDescription";
        public static int FwVerLen = 9;
        public static int RfModFwVerLen = 9;
        public static int RfModFwVerLen_ProductionRelease = 5;
        
        public enum MeterConfigurationType { CategoryA = 0, CategoryB };
        public enum ToolMainMenuList { File = 0, Readouts = 1, Programming = 2, DataDebug = 3, Actions = 4, Settings = 5, Help = 6, Irda = 7 };
        public enum SelectedMeter {OnePhaseSmartMeter=0, OnePhaseDLMS=1, ThreePhaseSmartMeter=2,ThreePhaseDLMSPUMA=3,ThreePhaseSapphire=4,ThreePhaseRUBY=5, OnePhaseNONDLMS=6};

        public static string TamperThresholdParaName = "TamperThresholds";
        public static string TouParaName = "TimeofUse";
        public static string DemandIP = "DemandIP";
        public static string BillingDateTime = "BillingDateTime";
        public static string SurveyIP = "SurveyIP";        
        public static string textFwName = "textFwVerStr";

        public static string LoadLimit = "LoadLimitValuekW";
        public static string MeteringMode = "MeteringMode";
        public static string PaymentMode = "PaymentMode";
        public static string EventStatusWordFilter = "ESWFConfig";
        public static string DisplayAutoScroll = "AutoScroll";
        public static string DisplayPushButton = "PushButton";
        public static string DisplayHRmode = "DispHR";
        public static string CaptureObjects = "LSCaptureobject";
        public static string OpticalPortLocking = "OpticalPort";
        public static string RJPortLocking = "RJPort";
        public static string EventLog = "EventLog";
        public static string LoadControl = "LoadControl";
        public static string ARMButton = "ARMBUTTON";
        public static string DemandMethod = "DemandMethod";
        public static string RS485DeviceAddress = "RS485DeviceAddress";

        public static int RS485DeviceAddressMin = 16;
        public static int RS485DeviceAddressMax = 16381;
        public static int RS485DeviceAddressDefault = 256;

        public static int MeterTypeMin = 0;
        public static int MeterTypeMax = 1;
        public static int MeterTypeDefault = 0;

        public enum ProduTraveler { EMSTest = 0, FunctionTest = 1, CalibrationTest = 2, SirializationTest = 3 };

       
   }

    public static class StaticConstants_1Phase
    {

     public static int RS485DeviceAddressMin = 16;
     public static int RS485DeviceAddressMax = 16381;
     public static int RS485DeviceAddressDefault = 256;

     public static string ScalarFile_Instant=@"\E350_TempInstantScalarProfile.xml";
     public static string ScalarFile_Billing = @"\E350_TempBillingScalarProfile.xml";
     public static string ScalarFile_Tamper = @"\E350_TempTamperScalarProfile.xml";
     public static string ScalarFile_LoadSurvey = @"\E350_TempLoadSurveyScalarProfile.xml";
     public static string ScalarFile_DailySurvey = @"\E350_TempDailySurveyScalarProfile.xml";
     public static string Xml1pA = "1P MeterProgram_CategoryA";
     public static string Xml1pB = "1P MeterProgram_CategoryB";
     public static string MtrFwVer1Ph = "00.000.10";


     public static string ObjectFile_Instant = @"\E350_TempInstantObjectProfile.xml";
     public static string ObjectFile_Billing = @"\E350_TempBillingObjectProfile.xml";
     public static string ObjectFile_Tamper = @"\E350_TempTamperObjectProfile.xml";
     public static string ObjectFile_LoadSurvey = @"\E350_TempLoadSurveyObjectProfile.xml";
     public static string ObjectFile_DailySurvey = @"\E350_TempDailySurveyObjectProfile.xml";
     public static string ObjectFile_NamePlate = @"\E350_TempNamePlateObjectProfile.xml";
     public static string ObjectFile_DefaultConfig = @"\E350_Default.cfg";
     public static string ObjectFile_DefaultTOU = @"\E350_Default.tou";
     public static string ObjectFile_DefaultConfig_1P_IEC = @"\1P_IEC_Default.cfg";
     public static string ObjectFile_DefaultTOU_1P_IEC = @"\1P_IEC_Default.tou";
     public static string ObjectFile_DefaultTOU_1P_IEC_Rate8 = @"\1P_IEC_Default_Rate8.tou";
     public static string ObjectFile_tempConfig_1P_IEC = @"\1P_IEC_temp.cfg";
     public static string ObjectFile_tempTOU_1P_IEC = @"\1P_IEC_temp.tou";
     public static string ObjectFile_FileTag_1P_IEC = "1P-IEC";

     public static string E310_CompartmentWiseTamperCountsObjectProfile = @"\E310_CompartmentWiseTamperCountsObjectProfile.xml";

     public static string ObjectFile_DefaultConfig_E150DLMS = @"\E150DLMS_Default.cfg";
     public static string ObjectFile_DefaultTOU_E150DLMS = @"\E150DLMS_Default.tou";
     public static string ObjectFile_Licience =AppDomain.CurrentDomain.BaseDirectory +  @"\pteul.ini";

     public static string ObjectFile_DefaultConfig_EventCodeList = @"\EventCodeList.xml";


     public const string Traveler_EMS_Add = "0015";
     public const string Traveler_FT_Add = "001D";
     public const string Traveler_CALI_Add = "0025";
     public const string Traveler_SERIAL_Add = "002D";

     public static string[] ConfigCategoryA = new string[] 
     { 
        StaticConstantsCommon.DemandIP,
        StaticConstantsCommon.BillingDateTime,
        StaticConstantsCommon.SurveyIP,
        StaticConstantsCommon.LoadLimit,
        StaticConstantsCommon.MeteringMode,
        StaticConstantsCommon.PaymentMode,
        StaticConstantsCommon.EventStatusWordFilter,
        StaticConstantsCommon.DisplayAutoScroll,
        StaticConstantsCommon.DisplayPushButton,
        StaticConstantsCommon.DisplayHRmode,
        StaticConstantsCommon.CaptureObjects,
        StaticConstantsCommon.OpticalPortLocking,
        StaticConstantsCommon.RJPortLocking,
        StaticConstantsCommon.EventLog,
        StaticConstantsCommon.LoadControl,
        StaticConstantsCommon.ARMButton,
        StaticConstantsCommon.TamperThresholdParaName,
        StaticConstantsCommon.TouParaName
     };

     public static string[] ConfigCategoryB = new string[] { StaticConstantsCommon.TamperThresholdParaName, StaticConstantsCommon.TouParaName }; //-- Category B Not Used at Present 31 january 2014
     //public static string[] TamperThreshold = new string[] { "OverVoltage", "LowVoltage", "OverLoad", "OverCurrent" };
     public static string[] TouConfiguration = new string[] { "FutureDayProfile", "WeekProfile", "SeasonProfile", "FutureActivationDate" };
     public static string[] TamperThreshold = new string[] { "NodeOverVoltageThreshold", "NodeOverVoltagePersistence", "NodeLowVoltageThreshold", "NodeLowVoltagePersistence", "NodeOverCurrentThreshold", "NodeOverCurrentPersistence", "NodeOverLoad", "NodeTemperatureRiseThreshold", "NodeTemperatureRisePersistence", "NodeCurrentReversalThreshold", "NodeCurrentReversalPersistence" };//NodeCurrentReversal --> Current Missmatch
         
         
    }

    public static class StaticConstants_3Phase
    {
        public enum MeterTOUType { Type_Season1 = 1, Type_Season2=2 };

        public static string ScalarFile_Instant = @"\E450_TempInstantScalarProfile.xml";
        public static string ScalarFile_Billing = @"\E450_TempBillingScalarProfile.xml";
        public static string ScalarFile_Tamper = @"\E450_TempTamperScalarProfile.xml";
        public static string ScalarFile_LoadSurvey = @"\E450_TempLoadSurveyScalarProfile.xml";
        public static string ScalarFile_DailySurvey = @"\E450_TempDailySurveyScalarProfile.xml";
       public static string ScalarFile_LoadSwitch = @"\E450_TempLoadSwitchScalarProfile.xml";
       public static string ScalarFile_UtilitySpeceficEvent = @"\E450_TempTamperScalarProfile_UtilitySpeceficEvent.xml";
        public static string ScalarFile_Phasor = @"\E450_TempPhasorScalarProfile.xml";
        public static string ScalarFile_AccuracyCheck = @"\E450_TempAccuracyCheckScalarProfile.xml";
        public static string ScalarFile_HR = @"\E450_TempHRScalarProfile.xml";
        public static string Xml3pA = "3P MeterProgram_CategoryA";
        public static string Xml3pB = "3P MeterProgram_CategoryB";
        public static string MtrFwVer3Ph = "00.000.10";
            


        public static string ObjectFile_Instant = @"\E450_TempInstantObjectProfile.xml";
        public static string ObjectFile_Billing = @"\E450_TempBillingObjectProfile.xml";
        public static string ObjectFile_Tamper = @"\E450_TempTamperObjectProfile.xml";
        public static string ObjectFile_LoadSurvey = @"\E450_TempLoadSurveyObjectProfile.xml";
        public static string ObjectFile_DailySurvey = @"\E450_TempDailySurveyObjectProfile.xml";
        public static string ObjectFile_LoadSwitchProfile = @"\E450_TempLoadSwitchObjectProfile.xml";
        public static string ObjectFile_UtilitySpeceficEvent = @"\E450_TempTamperObjectProfile_UtilitySpeceficEvent.xml";
        public static string ObjectFile_NamePlate = @"\E450_TempNamePlateObjectProfile.xml";
        public static string ObjectFile_AccuracyCheck = @"\E450_TempAccuracyCheckObjectProfile.xml";
        public static string ObjectFile_Phasor = @"\E450_TempPhasor.xml";
        public static string ObjectFile_HR = @"\E450_TempHRObjectProfile.xml";

        public static string  File_DisplayParameters = @"\E450_DisplayParameters.xml";        
        public static string  File_TamperEventCode = @"\E450_TamperEventCode.xml";
        public static string  File_ProfileGeneric = @"\E450_ProfileGeneric.xml";
        public static string ObjectFile_DefaultConfig = @"\E450_Default.cfh";

        public static string ObjectFile_DefaultConfig_E250 = @"\E250_Default.cfh";
        public static string ObjectFile_DefaultConfig_E250S2 = @"\E250S2_Default.cfh";

        public static string File_DisplayParameters_E250 = @"\E250_DisplayParameters.xml";
        public static string File_TamperThresholdValidation_E250 = @"\E250_TamperThresholdValidation.xml";
        public static string File_TamperThresholdValidation_TPDDL_E250 = @"\E250_TamperThresholdValidation_TPDDL.xml";
        public static string File_Factory_Configurable_MMP_Sheet_Sample_E250 = @"\Factory_Configurable_MMP_Sheet_Sample.xlsx";
        public static string File_Factory_Configurable_MMP_Sheet_Sample_E250_S2 = @"\Factory_Configurable_MMP_Sheet_Sample_S2.xlsx";
        public static string File_Factory_Configurable_MMP_Sheet_Sample_E450 = @"\Factory_Configurable_MMP_Sheet_Sample_SmartMeter.xlsx";

        //----------------------------3Phase Non-Smart Meter------------------
        public static string File_TamperEventCode_E250 = @"\E250_TamperEventCode.xml";
        public static string File_ProfileGeneric_E250 = @"\E250_ProfileGeneric.xml";
        public static string ObjectFile_AdHocDisplayAutoScroll = @"\E250_AdHocDisplayAutoScrollObjectProfile.xml";
        public static string ObjectFile_AdHocDisplayPushButton = @"\E250_AdHocDisplayPushButtonObjectProfile.xml";
        //-------------------------------------------------------------------

        public const string Traveler_EMS_Add = "0000";
        public const string Traveler_FT_Add = "0008";
        public const string Traveler_CALI_Add = "0010";
        public const string Traveler_SERIAL_Add = "0018";

        public static string[] ConfigCategoryA = new string[] 
        { 
            StaticConstantsCommon.DemandIP,
            StaticConstantsCommon.BillingDateTime,
            StaticConstantsCommon.SurveyIP,
            StaticConstantsCommon.LoadLimit,
            StaticConstantsCommon.MeteringMode,
            StaticConstantsCommon.PaymentMode,
            StaticConstantsCommon.EventStatusWordFilter,
            StaticConstantsCommon.DisplayAutoScroll,
            StaticConstantsCommon.DisplayPushButton,
            StaticConstantsCommon.DisplayHRmode,
            StaticConstantsCommon.CaptureObjects,
            StaticConstantsCommon.OpticalPortLocking,
            StaticConstantsCommon.RJPortLocking,
            StaticConstantsCommon.EventLog,
            StaticConstantsCommon.LoadControl,
            StaticConstantsCommon.ARMButton,
            StaticConstantsCommon.DemandMethod,
            StaticConstantsCommon.RS485DeviceAddress,
            StaticConstantsCommon.TamperThresholdParaName,
            StaticConstantsCommon.TouParaName
        };
        public static string[] ConfigCategoryB = new string[] { StaticConstantsCommon.TamperThresholdParaName, StaticConstantsCommon.TouParaName }; //-- Category B Not Used at Present 31 january 2014
       // public static string[] TamperThreshold = new string[] { "OverVoltage", "LowVoltage", "OverLoad", "VeryLowPF", "MissingPotential", "NeutralDisturbance", "MagneticInfluence", "CurrentReversal", "CurrentImbalance", "OverCurrent", "CTOpen", "CTShort", "VoltageUnb", "PowerOnOff" };
        public static string[] TamperThreshold = new string[] { 
                                                                "NodeMissingPotentioalThreshold",
                                                                "NodeMissingPotentioalPersistence", 
                                                                "NodeOverVoltageThreshold", 
                                                                "NodeOverVoltagePersistence", 
                                                                "NodeLowVoltageThreshold", 
                                                                "NodeLowVoltagePersistence", 
                                                                "NodeVoltageUnbalanceThreshold", 
                                                                "NodeVoltageUnbalancePersistence", 
                                                                "NodeCurrentReversalThreshold", 
                                                                "NodeCurrentReversalPersistence", 
                                                                "NodeCTOpenThreshold", 
                                                                "NodeCTOpenPersistence", 
                                                                "NodeCurrentUnbalanceThreshold", 
                                                                "NodeCurrentUnbalancePersistence", 
                                                                "NodeOverCurrentThreshold", 
                                                                "NodeOverCurrentPersistence", 
                                                                "NodeCTByPassThreshold", 
                                                                "NodeCTByPassPersistence", 
                                                                "NodeHighNeutralCurrentThreshold", 
                                                                "NodeHighNeutralCurrentPersistence",
                                                                "NodeVeryLowPFThreshold", 
                                                                "NodeVeryLowPFPersistence", 
                                                                "NodeTemperatureRiseThreshold" ,
                                                                "NodeTemperatureRisePersistence", 
                                                                "NodePowerOnOff", 
                                                                "NodeMagneticInfluence", 
                                                                "NodeNeutralDisturbance", 
                                                                "NodeOverLoad",
                                                                "NodeInvalidPhaseAssociation"
                                                                };
        public static string[] TouConfiguration = new string[] { "FutureDayProfile", "WeekProfile", "SeasonProfile", "FutureActivationDate" };

        

    }

    public static class PowerToolStaticUsers
    {
       public static string[] universalUserID = new string[] {"guestuser","user","PowerUser","Admin","PowerAdmin","PTAdmin","powertool" };
       public static string[] universalPassword = new string[] { "Guest@12","Landis@123", "Landis@123", "Landis@123", "Landis@123", "Cabcon", "powertool@12" };

      
    }
   

   /* public static class DLMSPTXmlConstants
    {
        public static string[] pgmname1p = new string[] { "DemandIP,IntegrationPeriod", "BillingDateTime", "BillingCycle,BillingDateTime", "TamperThreshold,TamperThresholds", "TOU,TimeofUse", "SurveyIP,IntegrationPeriod" };
        public static string[] pgmname3p = new string[] { "TamperThreshold,TamperThresholds", "IntegrationPeriod", "BillingDateTime", "BillingCycle,BillingDateTime", "TOU,TimeofUse", "SurveyIP,IntegrationPeriod" };

        public static string[] TamperThreshold1p = new string[] { "TamperThresholdOverVoltage", "TamperThresholdLowVoltage", "TamperThresholdOverLoad", "TamperThresholdOverCurrent" };
        public static string[] TamperThreshold3p = new string[] { "OverVoltage", "LowVoltage", "OverLoad", "VeryLowPF", "MissingPotential", "NeutralDisturbance", "MagneticInfluence", "CurrentReversal", "CurrentImbalance", "OverCurrent", "CTOpen", "CTShort", "VoltageUnb", "PowerOnOff" };
    }*/
    
}
