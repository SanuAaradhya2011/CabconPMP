using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using ManagedMath;
namespace Utilities
{
    public static class DLMSDataStracture
    {
        public enum METERTYPE { SM110, SM310 };
        public enum DisplayConfigStruct 
        { 
            AutoScroll, 
            PushButton, 
            DisplayConfig, 
            TamperConfig, 
            TamperPersistance, 
            DemandIP, 
            BillingDateTime, 
            BillingCycle, 
            LoadControl, 
            TOUPriceSlab, 
            Resolution, 
            TamperThresholdOverVoltage, 
            TamperThresholdLowVoltage, 
            TamperThresholdOverLoad, 
            TamperThresholdOverCurrent, 
            EventLog, 
            EventAlarm, 
            SURVEYIP, 
            ARMBUTTON, 
            RS485STATUS, 
            RS485ADDRESS,
            PREPAYMENT,
            DispHR,
            LSCaptureobject,
            ESWFConfig,
            LoadLimitValuekW,
            PaymentMode,
            MeteringMode,
            OpticalPort,
            RJPort,
            BatteryMode,
            FSModeLock,
            PulseEnergy,

               THROverVoltagePersistence=101,
               THROverVoltageThreshold = 102,
               THRLowVoltagePersistence = 103,
               THRLowVoltageThreshold = 104,
               THROverLoadPersistence = 105,
               THROverCurrentPersistence = 106,
               THROverCurrentThreshold = 107,
               THRTemperatureRisePersistence = 108,
               THRTemperatureRiseThreshold = 109,
               THRCurrentReversalPersistence = 110,
               THRCurrentReversalThreshold = 111
        };
        public enum TOUConfigStruct { ActiveDayProfile, FutureDayProfile, WeekProfile, SeasonProfile, FutureActivationDate };
        public enum DisplayConfigStruct_3Ph
        {
            INTERFRAMETIMEOUT, INACTIVITYTIMEOUT, KVAHSELECTION, REFVOLT, RESOLUTION, BILLINGDATETIME, BILLINGCYCLE, MDRESETLOCKOUTTIME, LSCAPTUREOBJECT, LSCAPTUREPERIOD, INTEGRATIONPERIOD, DISPPUSH, DISPSCROLL, DISPHR, DISPTIMEOUT, TAMPCOMPARTEMENT, TAMPTHRESHOULD, NOOFBILLONLCD, MFGYEAR, LOADCONTROL, COPENSTATUS, OVERVOLTAGE, TAMPERCOMPCONG,
            LOWVOLTAGE,
            OVERLOAD,
            TOUSLAB,
            EVENTLOG,
            ALARMLOG,
            VERYLOWPF,
            MISSINGPOTENTIAL,
            NEUTRALDISTURBANCE,
            MAGNETICINFLUENCE,
            CURRENTREVERSAL,
            CURRENTIMBALANCE,
            OVERCURRENT,
            CTOPEN,
            CTSHORT,
            VOLTAGEUNB,
            POWERONOFF,
            ARMBUTTON,
            RS485DeviceAddress,
            MeterType,
            DisConnectOnMagnet,
            LCDBacklight,
            AutoBilling,
            RS232,
            FactoryConfigurabelity,
            CTRation,
            PTRatio,
            INTEGRATIONPERIODSliding,
            RJPortConfiguration,
            OpticalPortConfiguration,
            METERINGMODE,
            ESWFCONFIG,
            LOADLIMITVALUEKW,
            DEMANDMETHOD,
            PAYMENTMODE,
            FSModeLock,

            THRMissingPotentioalPersistence = 101,
            THRMissingPotentioalThreshold = 102,
            THRVoltageUnbalancePersistence = 103,
            THRVoltageUnbalanceThreshold = 104,
            THROverVoltagePersistence = 105,
            THROverVoltageThreshold = 106,
            THRLowVoltagePersistence = 107,
            THRLowVoltageThreshold = 108,
            THRCurrentReversalPersistence = 109,
            THRCurrentReversalThreshold = 110,
            THRCTOpenPersistence = 111,
            THRCTOpenThreshold = 112,
            THRCTByPassPersistence = 113,
            THRCTByPassThreshold = 114,
            THROverCurrentPersistence = 115,
            THROverCurrentThreshold = 116,
            THRCurrentUnbalancePersistence = 117,
            THRCurrentUnbalanceThreshold = 118,
            THRPowerONoffPersistence = 119,
            THRMangeneticInfluence = 120,
            THRNEUTRALDISTURBANCE = 121,
            THRVeryLowPFPersistence = 122,
            THRVeryLowPFThreshold = 123,
            THROverLoad = 124,
            THRHighNeutralCurrentPersistence = 125,
            THRHighNeutralCurrentThreshold = 126,
            THRTemperatureRisePersistence = 127,
            THRTemperatureRiseThreshold = 128,
            THRInvalidPhaseAssociation = 129,

        };
        public enum TOUConfigStruct_3Ph { FutureDayProfile, WeekProfile, SeasonProfile,SpecialDaysProfile, FutureActivationDate };
        
        public enum TAMPERTHRESHOLD
        {
            MISSINGPOTPERSISTENCE,
            MISSINGPOTENTIAL,
            VOLTUNBALANCEPERSISTENCE,
            VOLTAGEUNB,
            OVERVOLTPERSISTENCE,
            OVERVOLTAGE,
            LOWVOLTPERSISTENCE,
            LOWVOLTAGE,
            CURRENTREVERSALPERSISTENCE,
            CURRENTREVERSAL,
            CTOPENPERSISTENCE,
            CTOPEN,
            CTBYPASSPERSISTENCE,
            CTBYPASS,
            OVERCURRENTPERSISTENCE,
            OVERCURRENT,
            CURRENTUNBALPERSISTENCE,
            CURRENTUNBALANCE,
            POWERONOFF,
            MAGNETICINFLUENCE,
            NEUTRALDISTURBANCE,
            VERYLOWPFPERSISTENCE,
            VERYLOWPF,
            OVERLOAD,
            HIGHNEUTRALPERSISTENCE,
            HIGHNEUTRALCURRENT,
            TEMPRISEPERSISTENCE,
            TEMPRATURERISE,
            INVALIDPHASEASSOCIATION,
              

        };
        public enum TAMPERTHRESHOLD1PH
        {

            OVERVOLTPERSISTENCE,
            OVERVOLTAGE,
            LOWVOLTPERSISTENCE,
            LOWVOLTAGE,
            OVERLOAD,
            OVERCURRENTPERSISTENCE,
            OVERCURRENT,
            TEMPRISEPERSISTENCE,
            TEMPRATURERISE,
            CURRENTMISMATCHPERSISTENCE,
            CURRENTMISMATCHTHRESHOLD,
};

        public enum CPP_Scalar { FF, FE, FD }; //FF-> 0, FE-> -1 , FD -> -2
        public enum CPP_Unit { A }; //0A-> INR
      
        public const byte Null_Type = 0x00;
        public const byte Array_Type = 0x01;
        public const byte Structure = 0x02;
        public const byte Boolean_Type = 0x03;
        public const byte BitString_Type = 0x04;
        public const byte DoubleLong_Type = 0x05;
        public const byte DoubleLongUnsigned_Type = 0x06;
        public const byte OctetString_Type = 0x09;
        public const byte VisibleString_Type = 10;
        public const byte BCD_Type = 13;
        public const byte Integer_Type = 15;
        public const byte Long_Type = 16;
        public const byte Unsigned_Type = 17;
        public const byte LongUnsigned_Type = 18;
        public const byte CompactArray_Type = 19;
        public const byte Long64_Type = 20;
        public const byte Long64Unsigned_Type = 21;
        public const byte Enum_Type = 22;
        public const byte Float32_Type = 23;
        public const byte Float64_Type = 24;
        public const byte DateTime_Type = 25;
        public const byte Date_Type = 26;
        public const byte Time_Type = 27;
        public const byte ExtendedOctetString = 0x82;        
        

        public struct DataStractureAccessSelector
        {
            public static byte Null_descriptor = 0x00;
            public static byte Range_descriptor = 0x01;
            public static byte Entry_descriptor = 0x02;
        }

        public struct DataStractureRequest
        {
            public static byte[] GetRequest_Normal = new byte[] { 0xC0, 0x01 };
            public static byte[] SetRequest_Normal = new byte[] { 0xC1, 0x01 };
            public static byte[] SetRequest_Block = new byte[] { 0xC1, 0x02 };
            public static byte[] GetRequest_Block = new byte[] { 0xC1, 0x03 };
            public static byte[] ActionRequest_Normal = new byte[] { 0xC3, 0x01 };
            public static byte[] ActionRequest_FirstBlock = new byte[] { 0xC3, 0x04 };
            public static byte[] ActionRequest_pBlock = new byte[] { 0xC3, 0x06 };

        }
        //---------------------------------------------Programing---------------------------------------------------------------------------------------
        public struct PushButtonlDisplayDataStracture
        {
            public static byte[] PushButtonDisplayOBIS_Falcon2 = new byte[] { 0x00, 0x00, 0x60, 0x80, 0x01, 0xFF };
            public static byte[] PushButtonDisplayOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x86, 0xFF };
            public static byte PushButtonDisplayClassID = 0x01;
            public static byte PushButtonDisplayValueAttribute = 0x02;
            public static byte PushButtonDisplayDataType = 0x09;
            public static byte PushButtonDisplayDataLength = 0x78;
            public static byte PushButtonDisplayDataLength_1PNONAMI = 0x40;
        }
        
        public struct AutoScrollDisplayDataStracture
        {
            public static byte[] AutoDisplayOBIS_Falcon2 = new byte[] { 0x00, 0x00, 0x60, 0x80, 0x00, 0xFF };
            public static byte[] AutoDisplayOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x87, 0xFF };
            public static byte AutoDisplayClassID = 0x01;
            public static byte AutoDisplayValueAttribute = 0x02;
            public static byte AutoDisplayDataType = 0x09;
            public static byte AutoDisplayDataLength = 0x78;
            public static byte AutoDisplayDataLength_1PNONAMI = 0x40;
        }

        public struct DisplayHRDataStracture
        {
            public static byte[] DisplayHROBIS_Falcon2 = new byte[] { 0x00, 0x00, 0x60, 0x80, 0x02, 0xFF };
            public static byte[] DisplayHROBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x88, 0xFF };
            public static byte DisplayHRClassID = 0x01;
            public static byte DisplayHRValueAttribute = 0x02;
            public static byte DisplayHRDataType = 0x09;
            public static byte DisplayHRDataLength = 0x0A;
        }


        public struct DisplayConfigurationDataStracture
        {
            public static byte[] DisplayConfigurationOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x83, 0xFF };
            public static byte DisplayConfigurationClassID = 0x01;
            public static byte DisplayConfigurationValueAttribute = 0x02;
            public static byte DisplayConfigurationDataType = 0x09;
            public static byte DisplayConfigurationDataLength = 0x20;
            public static byte DisplayConfigurationDataLength_NONAMI = 0x10;
        }

        public struct TamperConfigurationDataStracture
        {
            public static byte[] TamperConfigurationOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x8D, 0xFF };
            public static byte[] TamperConfigurationOBIS_Event = new byte[] { 0x00, 0x00, 0x60, 0x02, 0xA0, 0xFF };
            public static byte[] TamperConfigurationOBIS_EventLogFalcon2 = new byte[] { 0x00, 0x00, 0x60, 0x80, 0x07, 0xFF };
            public static byte[] TamperConfigurationOBIS_Alarm = new byte[] { 0x00, 0x00, 0x60, 0x01, 0xCE, 0xFF };
            public static byte TamperConfigurationClassID = 0x01;
            public static byte TamperConfigurationValueAttribute = 0x02;
            public static byte TamperConfigurationDataType = 0x09;
            public static byte TamperConfigurationDataType_EventLog = 0x04;//--Bit String
            public static byte TamperConfigurationDataLength = 0x0A;
            public static byte TamperConfigurationDataLength_NONAMI = 0x03;
            //public static byte TamperConfigurationDataLength_3Phase = 0x06;
            //public static byte TamperConfigurationDataLength_1Phase = 0x04;
        }

        public struct TamperPersistanceDataStracture
        {
            public static byte[] TamperPersistanceOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x83, 0xFF };
            public static byte TamperPersistanceClassID = 0x01;
            public static byte TamperPersistanceValueAttribute = 0x02;
            public static byte TamperPersistanceDataType = 0x02;
            public static byte TamperPersistanceDataLength = 0x07;
            public static byte TamperPersistanceDataLength_E150DLMS = 0x0C;
        }

        public struct DemandIPDataStracture
        {
            public static byte[] DemandIPOBIS = new byte[] { 0x01, 0x00, 0x00, 0x08, 0x00, 0xFF };
            public static byte[] SurveyIPOBIS = new byte[] { 0x01, 0x00, 0x00, 0x08, 0x04, 0xFF };
            public static byte DemandIPClassID = 0x01;
            public static byte DemandIPValueAttribute = 0x02;
            public static byte DemandIPDataType = 0x12;
            public static byte DemandIPDataLength = 0x00;
        }

        public struct DemandIPSlidingDataStracture
        {
            public static byte[] DemandIPSlidingOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x99, 0xFF };
            public static byte[] DemandIPSlidingOBIS_Falcon2 = new byte[] { 0x01, 0x00, 0x60, 0x32, 0x03, 0xFF };
            public static byte DemandIPSlidingClassID = 0x01;
            public static byte DemandIPSlidingValueAttribute = 0x02;
            public static byte DemandIPSlidingDataType = 0x12;
            public static byte DemandIPSlidingDataLength = 0x00;
            public static byte DemandIPSlidingDataType_DemandMethod = 0x02;
            public static byte DemandIPSlidingDataLength_DemandMethod = 0x02;
        }

        public struct BillingDateTimeDataStracture
        {
            public static byte[] BillingDateTimeOBIS = new byte[] { 0x00, 0x00, 0x0F, 0x00, 0x00, 0xFF };
            public static byte BillingDateTimeClassID = 0x16;
            public static byte BillingDateTimeValueAttribute = 0x04;
            public static byte BillingDateTimeDataType = 0xFF;
            public static byte BillingDateTimeDataLength = 0xFF;
        }

        public struct BillingCycleDataStracture
        {
            public static byte[] BillingCycleOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x8B, 0xFF };
            //public static byte[] BillingCycleOBIS_3Phase = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x00, 0xFF };
            public static byte BillingCycleClassID = 0x01;
            public static byte BillingCycleValueAttribute = 0x02;
            public static byte BillingCycleDataType = 0x11;
            public static byte BillingCycleDataLength = 0x00;
        }

        public struct PulseEnergyDataStracture
        {
            public static byte[] PulseEnergyOBIS = new byte[] { 0x01, 0x00, 0x60, 0x80, 0x10, 0xFF };
            public static byte PulseEnergyClassID = 0x01;
            public static byte PulseEnergyValueAttribute = 0x02;
            public static byte PulseEnergyDataType = 0x11;
            public static byte PulseEnergyDataLength = 0x00;
            public static byte PulseEnergyValueByteActive = 0x00;
            public static byte PulseEnergyValueByteApparent = 0x01;
            public static byte PulseEnergyValueByteReactive = 0x02;
        }

        public struct LoadControlDataStracture
        {
            public static byte[] LoadControlOBIS = new byte[] { 0x00, 0x00, 0x60, 0x03, 0x80, 0xFF };
            public static byte LoadControlClassID = 0x01;
            public static byte LoadControlValueAttribute = 0x02;
            public static byte LoadControlDataType = 0x02;
            public static byte LoadControlDataLength = 0x06;
            public static byte LoadControlDataLength_3Phase = 0x05;
        }

        public struct TouPriceSlabDataStracture
        {
            public static byte[] TouPriceSlabOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x80, 0xFF };
            public static byte TouPriceSlabClassID = 0x01;
            public static byte TouPriceSlabValueAttribute = 0x02;
            public static byte TouPriceSlabDataType = 0x02;
            public static byte TouPriceSlabDataLength = 0x06;
            public static byte TouPriceSlabDataLength_3Phase = 0x08;
            
        }

        public struct ResolutionDataStracture
        {
            public static byte[] ResolutionOBIS_Falcon2 = new byte[] { 0x00, 0x00, 0x60, 0x80, 0x02, 0xFF };
            public static byte[] ResolutionOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x84, 0xFF };
            public static byte ResolutionClassID = 0x01;
            public static byte ResolutionValueAttribute = 0x02;
            public static byte ResolutionDataType = 0x11;
            public static byte ResolutionDataLength = 0x00;

            public static byte ResolutionDataType_3Phase = 0x09;
            public static byte ResolutionDataLength_3Phase = 0x03;
        }
        public struct ARMButtonDataStracture
        {
            public static byte[] ARMButtonOBIS = new byte[] { 0x00, 0x00, 0x60, 0x03, 0x81, 0xFF };
            public static byte ARMButtonClassID = 0x01;
            public static byte ARMButtonValueAttribute = 0x02;
            public static byte ARMButtonDataType = 0x11;
            public static byte ARMButtonDataLength = 0x00; 
        }
        public struct ConnectControlDataStracture
        {
            public static byte[] ConnectControlOBIS = new byte[] { 0x00, 0x00, 0x60, 0x03, 0x0A, 0xFF };
            public static byte ConnectControlClassID = 0x46;
            public static byte DisconnectControlValueAttribute = 0x01;
            public static byte ConnectControlValueAttribute = 0x02;           
            public static byte DisconnectControlControlStateValueAttribute = 0x03;
            
            public static byte ConnectControlDataType = 0x0F;
            public static byte ConnectControlDataLength = 0x00;
                    
        }


        public struct TOUConfigurationDataStracture
        {
            public static byte[] TOUConfigurationOBIS = new byte[] { 0x00, 0x00, 0x0D, 0x00, 0x00, 0xFF };
            public static byte TOUConfigurationClassID = 0x14;
            public static byte TOUConfigurationValueAttribute_ActiveSeason = 0x03;
            public static byte TOUConfigurationValueAttribute_ActiveWeek = 0x04;
            public static byte TOUConfigurationValueAttribute_ActiveDays = 0x05;

            public static byte TOUConfigurationValueAttribute_FutureSeason = 0x07;
            public static byte TOUConfigurationValueAttribute_FutureWeek = 0x08;
            public static byte TOUConfigurationValueAttribute_FutureDays = 0x09;

            public static byte TOUConfigurationValueAttribute_FutureActivationDate = 0x0A;

            public static byte TOUConfigurationDataType_FutureDays = 0x01;
            public static byte TOUConfigurationDataLength_FutureDays = 0x04;
            public static byte TOUConfigurationDataLength_FutureDays_3Phase = 0x18;

            public static byte TOUConfigurationDataType_FutureWeek = 0x01;
            public static byte TOUConfigurationDataLength_FutureWeek = 0x04;

            public static byte TOUConfigurationDataType_FutureSeason = 0x01;
            public static byte TOUConfigurationDataLength_FutureSeason = 0x04;

            public static byte TOUConfigurationDataType_FutureActivationDate = 0x09;
            public static byte TOUConfigurationDataLength_FutureActivationDate = 0x0C;

            public static byte[] TOUConfiguration_SpecialDaysOBIS = new byte[] { 0x00, 0x00, 0x0B, 0x00, 0x00, 0xFF };
            public static byte TOUConfiguration_SpecialDaysClassID = 0x0B;
            public static byte TOUConfiguration_SpecialDaysValueAttribute = 0x02;
            public static byte TOUConfiguration_SpecialDaysDataType = 0x01;
            public static byte TOUConfiguration_SpecialDaysDataLength = 0x64;

        }

        public struct CalibrationDataStracture
        {
            public static byte[] CalibrationVoltageReadOBIS = new byte[] { 0x01, 0x00, 0x0C, 0x07, 0x00, 0xFF };
            public static byte[] CalibrationPhaseCurrentReadOBIS = new byte[] { 0x01, 0x00, 0x0B, 0x07, 0x00, 0xFF };
            public static byte[] CalibrationNeutralCurrentReadOBIS = new byte[] { 0x01, 0x00, 0x5B, 0x07, 0x00, 0xFF };
            public static byte[] CalibrationTemperatureOBIS = new byte[] { 0x00, 0x00, 0x60, 0x09, 0x80, 0xFF };

            public static byte[] CalibrationOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x84, 0xFF };
            public static byte CalibrationNonPowerReadClassID = 0x03;
            public static byte CalibrationClassID = 0x01;

            public static byte CalibrationValueAttribute = 0x02;
            public static byte CalibrationValueAttribute_Scalar = 0x03;
            public static byte CalibrationDataType = 0x02;
            public static byte CalibrationDataLength = 0x02;

            public static byte CalibrationFactorIentifier_ResetAll = 0x00;
            public static byte CalibrationFactorIentifier_Voltage = 0x01;
            public static byte CalibrationFactorIentifier_PhaseCurrent = 0x02;
            public static byte CalibrationFactorIentifier_NeutralCurrent = 0x05;

            public static byte CalibrationFactorIentifier_PhaseEnergy = 0x03;
            public static byte CalibrationFactorIentifier_PhaseAngle = 0x04;

            public static byte CalibrationFactorIentifier_Neutral_P_1WH = 0x06;
            public static byte CalibrationFactorIentifier_Neutral_P_MagSlope_Low = 0x07;
            public static byte CalibrationFactorIentifier_Neutral_IS_Delay = 0x08;

            public static byte CalibrationFactorIentifier_Magnet = 0x09;
            public static byte CalibrationFactorIentifier_MagnetThreshold = 0x0B;
            public static byte CalibrationFactorIentifier_Temperature = 0x0C;

            public static byte CalibrationDataByte_ResetAllCalibration = 0x01;
            public static byte CalibrationDataByte_ResetVoltageCalibration = 0x02;
            public static byte CalibrationDataByte_ResetPhaseCalibration = 0x03;
            public static byte CalibrationDataByte_ResetNeutralCalibration = 0x04;
            public static byte CalibrationDataByte_ResetTemperatureCalibration = 0x05;

            public static byte[] CalibrationInitiateOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x91, 0xFF };
            public static byte[] CalibrationGetOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x84, 0xFF };
            public static byte[] CalibrationSetOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x88, 0xFF };

            public static byte CalibrationInitiate_ResetCalibration = 0x01;
            public static byte CalibrationInitiate_ActiveCalibration = 0x02;
            public static byte CalibrationInitiate_ResetandActiveCalibration = 0x03;
            public static byte CalibrationInitiate_ReactiveCalibration = 0x04;
            public static byte CalibrationInitiate_CToffsetCalibration = 0x08;
            public static byte CalibrationInitiate_CToffsetandReactiveCalibration = 0x0C;
            public static byte CalibrationInitiate_Neutral = 0x10;
            public static byte CalibrationInitiate_Temperature = 0x40;
            public static byte CalibrationInitiate_VoltageTHD = 0x20;
            public static byte CalibrationInitiate_CurrentTHD = 0x40;

            public static byte CalibrationSet_VoltageValue = 0x01;
            public static byte CalibrationSet_CurrentValue = 0x02;
            public static byte CalibrationSet_ActiveValue = 0x03;
            public static byte CalibrationSet_ReactiveValue = 0x04;
            public static byte CalibrationSet_FundamentalVoltage = 0x05;
            public static byte CalibrationSet_FundamentalCurrent = 0x06;

            public static byte CalibrationSet_CToffsetCofficient = 0x05;
            public static byte CalibrationSet_VoltageCofficient = 0x06;
            public static byte CalibrationSet_CurrentCofficient = 0x07;
            public static byte CalibrationSet_Falcon2NeutralCofficient = 0x07;
            public static byte CalibrationSet_Falcon2TemperatureCofficient = 0x08;

            public static byte CalibrationSet_ActiveCofficient = 0x08;
            public static byte CalibrationSet_ReactiveCofficient = 0x09;
            public static byte CalibrationSet_NeutralCofficient = 0x0C;
         

            public static byte CalibrationSet_THDVoltageCoefficient = 0x0F;
            public static byte CalibrationSet_THDCurrentCoefficient = 0x10;

            public static byte CalibrationSet_DataType = 0x02;
            public static byte CalibrationSet_DataLength = 0x08;           


        }
       
        public struct PmaxDataStracture
        {
            public static byte[] PmaxOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x85, 0xFF };
            public static byte PmaxClassID = 0x01;
            public static byte PmaxValueAttribute = 0x02;
            public static byte PmaxDataType = 0x11;
            public static byte PmaxDataLength = 0x00;
        }
        public struct BuildVersionDataStracture
        {
            public static byte[] BuildVersionOBIS = new byte[] { 0x00, 0x00, 0x60, 0x80, 0x08, 0xFF };
            public static byte BuildVersionClassID = 0x01;
            public static byte BuildVersionValueAttribute = 0x02;
            public static byte BuildVersionDataType = 0x0A;
            public static byte BuildVersionDataLength = 0x23;//--35 Bytes
        }
        public struct MagnetThreshouldDataStracture
        {
            public static byte[] MagnetThreshouldOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x86, 0xFF };
            public static byte MagnetThreshouldClassID = 0x01;
            public static byte MagnetThreshouldValueAttribute = 0x02;
            public static byte MagnetThreshouldDataType = 0x11;
            public static byte MagnetThreshouldDataLength = 0x00;
        }

        public struct MeterIDDataStracture
        {
            public static byte[] MeterIDOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x00, 0xFF };
            public static byte MeterIDClassID = 0x01;
            public static byte MeterIDValueAttribute = 0x02;
            public static byte MeterIDDataType = 0x0A;
            public static byte MeterIDDataLength = 0x10;
            public static byte MeterIDDataType_3Phase = 0x09;
            public static byte MeterIDDataLength_3Phase = 0x08;
        }
        public struct MeteringModeDataStracture
        {
            public static byte[] MeteringModeOBIS = new byte[] { 0x00, 0x00, 0x5E, 0x60, 0x13, 0xFF }; //0.0.94.96.19.255
            public static byte MeteringModeClassID = 0x01;
            public static byte MeteringModeValueAttribute = 0x02;
            public static byte MeteringModeDataType = 0x11;
            public static byte MeteringModeDataLength = 0x00;
           
        }
        public struct PCBAIDDataStracture
        {
            public static byte[] PCBAIDOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x8b, 0xFF };
            public static byte PCBAIDClassID = 0x01;
            public static byte PCBAIDValueAttribute = 0x02;
            public static byte PCBAIDDataType = 0x0A;
            public static byte PCBAIDDataLength = 0x0D;
            public static byte PCBAIDDataType_3Phase = 0x09;
            public static byte PCBAIDDataLength_E150DLMS = 0x0A;
        }
        public struct MeterRTCDataStracture
        {
            public static byte[] MeterRTCOBIS = new byte[] { 0x00, 0x00, 0x01, 0x00, 0x00, 0xFF };
            public static byte MeterRTCClassID = 0x08;
            public static byte MeterRTCValueAttribute = 0x02;
            public static byte MeterRTCDataType = 0x09;
            public static byte MeterRTCDataLength = 0x0C;
        }
        public struct ActivePowerDataStracture
        {
            public static byte[] ActivePowerOBIS_R = new byte[] { 0x01, 0x00, 0x15, 0x07, 0x00, 0xFF };
            public static byte[] ActivePowerOBIS_Y = new byte[] { 0x01, 0x00, 0x29, 0x07, 0x00, 0xFF };
            public static byte[] ActivePowerOBIS_B = new byte[] { 0x01, 0x00, 0x3D, 0x07, 0x00, 0xFF };
            public static byte ActivePowerClassID = 0x03;
            public static byte ActivePowerValueAttribute = 0x02;
           
        }
        public struct PaymentOpertationDataStracture
        {
            public static byte[] PaymentModeOBIS = new byte[]               { 0x00, 0x00, 0x5E, 0x60, 0x14, 0xFF };
            public static byte[] LastTokenRechargeAmountOBIS = new byte[]   { 0x00, 0x00, 0x5E, 0x60, 0x15, 0xFF };
            public static byte[] LastTokenRechargeTimeOBIS = new byte[]     { 0x00, 0x00, 0x5E, 0x60, 0x16, 0xFF };
            public static byte[] TotalAmountAtLastRechargeOBIS = new byte[] { 0x00, 0x00, 0x5E, 0x60, 0x17, 0xFF };
            public static byte[] CurrentBalanceAmountOBIS = new byte[]      { 0x00, 0x00, 0x5E, 0x60, 0x18, 0xFF };
            public static byte[] CurrentBalanceTimeOBIS = new byte[]        { 0x00, 0x00, 0x5E, 0x60, 0x19, 0xFF };

            public static byte PaymentClassID = 0x01;
            public static byte PaymentValueAttribute = 0x02;
          
            public static byte PaymentModeDataType = 0x011;
            public static byte PaymentModeDataLength = 0x00;

            public static byte LastTokenRechargeAmountDataType = 0x06;
            public static byte LastTokenRechargeAmountDataType_1PSM = 0x05;
            public static byte LastTokenRechargeAmountDataLength = 0x00;

            public static byte LastTokenRechargeTimeDataType = 0x09;
            public static byte LastTokenRechargeTimeDataLength = 0x0C;

            public static byte TotalAmountAtLastRechargeDataType = 0x06;
            public static byte TotalAmountAtLastRechargeDataType_1PSM = 0x05;
            public static byte TotalAmountAtLastRechargeDataLength = 0x00;

            public static byte CurrentBalanceAmountDataType = 0x06;
            public static byte CurrentBalanceAmountDataType_1PSM = 0x05;
            public static byte CurrentBalanceAmountDataLength = 0x00;

            public static byte CurrentBalanceTimeDataType = 0x09;
            public static byte CurrentBalanceTimeDataLength = 0x0C;


        }
        public struct CPPRateDataStracture
        {
            public static byte[] CPPRateOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x8C, 0xFF };
            public static byte[] CPPDurationOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x99, 0xFF };

            public static byte CPPRateDataType_1Phase = 0x06;
            public static byte CPPRateClassID_1Phase = 0x03;

            public static byte CPPRateClassID = 0x04;
            public static byte CPPRateValueAttribute = 0x02;
            public static byte CPPRateValueAttribute_ScalarUnit = 0x03;
            public static byte CPPRateDataType = 0x12;
            public static byte CPPRateDataLength = 0x00;
            public static byte CPPRateDataType_ScalarUnit = 0x02;
            public static byte CPPRateDataLength_ScalarUnit = 0x02;
            public static byte CPPDurationClassID = 0x03;
            public static byte CPPDurationDataType = 0x06;    
            public static byte CPPDurationValueAttribute = 0x02;
            public static byte CPPDurationDataLength = 0x00;
            public static byte CPPStarttimeAttribute = 0x05;
            public static byte CPPStarttimeClassID = 0x04;
            public static byte CPPStarttimeDataType = 0x09;
            public static byte CPPStarttimeDataLength = 0x0C;

        }
        public struct CPPActivationDeactivationDataStracture
        {
            public static byte[] CPPActivationDeactivationOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x9F, 0xFF };
            public static byte CPPActivationDeactivationClassID = 0x01;
            public static byte CPPActivationDeactivationValueAttribute = 0x02;
            public static byte CPPActivationDeactivationDataType = 0x02;
            public static byte CPPActivationDeactivationDataLength = 0x02;
        }
        public struct MeterSoftwareLockDataStracture
        {
            public static byte[] MeterSoftwareLockOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x92, 0xFF };
            public static byte MeterSoftwareLockClassID = 0x01;
            public static byte MeterSoftwareLockValueAttribute = 0x02;
            public static byte MeterSoftwareLockDataType = 0x11;
            public static byte MeterSoftwareLockDataLength = 0x00;
        }
        //---------------------------------------Readout Data and Object------------------------------------------------------       
        public struct ScalUnitDataStracture
        {
            public static byte[] InstantScalUnitOBIS = new byte[] { 0x01, 0x00, 0x5E, 0x5B, 0x03, 0xFF };
            public static byte[] BillingScalUnitOBIS = new byte[] { 0x01, 0x00, 0x5E, 0x5B, 0x06, 0xFF };
            public static byte[] TamperScalUnitOBIS = new byte[] { 0x01, 0x00, 0x5E, 0x5B, 0x07, 0xFF };
            public static byte[] LoadSurveyScalUnitOBIS = new byte[] { 0x01, 0x00, 0x5E, 0x5B, 0x04, 0xFF };
            public static byte[] DailyProfileScalUnitOBIS = new byte[] { 0x01, 0x00, 0x5E, 0x5B, 0x05, 0xFF };
            public static byte[] LoadSwitchScalUnitOBIS = new byte[] { 0x01, 0x00, 0x5E, 0x5B, 0x83, 0xFF };
            public static byte[] MeterAccuracyCheckOBIS = new byte[] { 0x01, 0x00, 0x5E, 0x5B, 0x81, 0xFF };
            public static byte[] MeterAccuracyCheckOBIS_NONAMI = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x9C, 0xFF };
            public static byte[] TamperScalUnitOBIS_UtilitySpecificEvents = new byte[] { 0x01, 0x00, 0x5E, 0x5B, 0x82, 0xFF };
            public static byte[] PhasorScalarProfileOBIS_Falcon2 = new byte[] { 0x01, 0x00, 0x5E, 0x5B, 0x80, 0xFF };
            public static byte[] HRProfileScalUnitOBIS = new byte[] { 0x01, 0x00, 0x5E, 0x5B, 0x85, 0xFF };
            public static byte ScalUnitClassID = 0x07;
            public static byte ScalUnitValueAttribute_Object = 0x03;
            public static byte ScalUnitValueAttribute_Data = 0x02;
        }

        public struct ReadoutDataStracture
        {
            public static byte[] InstantReadoutOBIS = new byte[] { 0x01, 0x00, 0x5E, 0x5B, 0x00, 0xFF };
            public static byte[] BillingReadoutOBIS = new byte[] { 0x01, 0x00, 0x62, 0x01, 0x00, 0xFF };
            public static byte[] TamperReadoutOBIS_Compartment1 = new byte[] { 0x00, 0x00, 0x63, 0x62, 0x00, 0xFF };
            public static byte[] TamperReadoutOBIS_Compartment2 = new byte[] { 0x00, 0x00, 0x63, 0x62, 0x01, 0xFF };
            public static byte[] TamperReadoutOBIS_Compartment3 = new byte[] { 0x00, 0x00, 0x63, 0x62, 0x02, 0xFF };
            public static byte[] TamperReadoutOBIS_Compartment4 = new byte[] { 0x00, 0x00, 0x63, 0x62, 0x03, 0xFF };
            public static byte[] TamperReadoutOBIS_Compartment5 = new byte[] { 0x00, 0x00, 0x63, 0x62, 0x04, 0xFF };
            public static byte[] TamperReadoutOBIS_Compartment6 = new byte[] { 0x00, 0x00, 0x63, 0x62, 0x05, 0xFF };
            public static byte[] TamperReadoutOBIS_Compartment7 = new byte[] { 0x00, 0x00, 0x63, 0x62, 0x06, 0xFF };
            public static byte[] TamperReadoutOBIS_Compartment8 = new byte[] { 0x00, 0x00, 0x63, 0x62, 0x07, 0xFF };
            public static byte[] TamperReadoutOBIS_Compartment9 = new byte[] { 0x00, 0x00, 0x63, 0x62, 0x08, 0xFF };
            public static byte[] TamperReadoutOBIS_Compartment10_OTA = new byte[] { 0x00, 0x00, 0x63, 0x62, 0x80, 0xFF };
            public static byte[] TamperReadoutOBIS_Compartment11_DigitalInput = new byte[] { 0x00, 0x00, 0x63, 0x62, 0x82, 0xFF };
            public static byte[] TamperReadoutOBIS_Compartment12_UtilitySpecefic = new byte[] { 0x00, 0x00, 0x63, 0x62, 0x80, 0xFF };
            public static byte[] LoadSurveyReadoutOBIS = new byte[] { 0x01, 0x00, 0x63, 0x01, 0x00, 0xFF };
            public static byte[] DailySurveyReadoutOBIS = new byte[] { 0x01, 0x00, 0x63, 0x02, 0x00, 0xFF };
            public static byte[] LoadSwitchReadoutOBIS = new byte[] { 0x00, 0x00, 0x63, 0x62, 0x81, 0xFF };
            public static byte[] NamePlateReadoutOBIS = new byte[] { 0x00, 0x00, 0x5E, 0x5B, 0x0A, 0xFF };
            public static byte[] PhasorProfileOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0xBD, 0xFF };
            public static byte[] PhasorScalarOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0xBE, 0xFF };
            public static byte[] PhasorProfileOBIS_Falcon2 = new byte[] { 0x01, 0x00, 0x63, 0x80, 0x80, 0xFF };
            public static byte[] PhasorScalarProfileOBIS_Falcon2 = new byte[] { 0x01, 0x00, 0x5E, 0x5B, 0x80, 0xFF };
            public static byte[] MeterAccuracyCheckOBIS_Value = new byte[] { 0x01, 0x00, 0x63, 0x80, 0x81, 0xFF };
            public static byte[] MeterAccuracyCheckOBIS_Value_NONAMI = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x9B, 0xFF };
            public static byte[] PeriodicPushReadoutOBIS = new byte[] { 0x00, 0x00, 0x19, 0x09, 0x00, 0xFF };
            public static byte[] EventPushReadoutOBIS = new byte[]    { 0x00, 0x04, 0x19, 0x09, 0x00, 0xFF };
            public static byte[] HRProfileOBIS = new byte[] { 0x01, 0x00, 0x63, 0x80, 0x82, 0xFF };

            public static byte PeriodicandEventPushClassID = 0x28;
            public static byte ReadoutClassID = 0x07;
            public static byte Readout3PhaseNamePlateDataClassID = 0x01;
            public static byte ReadoutValueAttribute_Object = 0x03;
            public static byte ReadoutValueAttribute_Data = 0x02;
          
        }
        public struct ReadMeterInfoDataStracture
        {
            public static byte[] ReadMeterInfoOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x9A, 0xFF };
            public static byte[] ReadMeterInfoOBIS_3PHDLMS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0xBC, 0xFF };
            public static byte ReadMeterInfoClassID = 0x01;
            public static byte ReadMeterInfoValueAttribute = 0x02;
            public static byte ReadMeterInfoDataType = 0x09;
            public static byte ReadMeterInfoDataLength = 0x10;
        }
       
        public struct ReadInstantkWhDataStracture
        {
            public static byte[] ReadInstantkWhOBIS = new byte[] { 0x01, 0x00, 0x01, 0x08, 0x00, 0xFF };
            public static byte ReadInstantkWhClassID = 0x03;
            public static byte ReadInstantkWhValueAttribute = 0x02;
            public static byte ReadInstantkWhValueAttributeScalar = 0x03;
        }
        public struct ReadMeterBufferDataStracture
        {
            public static byte[] ReadMeterBufferOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x87, 0xFF };
            public static byte ReadMeterBufferClassID = 0x01;
            public static byte ReadMeterBufferValueAttribute = 0x02;
            public static byte ReadMeterBufferDataType = 0x09;
            public static byte ReadMeterBufferDataLength = 0x2A;
        }

        public struct ReadStatusFlagDataStracture
        {
            public static byte[] ReadStatusFlagOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x9C, 0xFF };
            public static byte ReadStatusFlagClassID = 0x01;
            public static byte ReadStatusFlagValueAttribute = 0x02;
            public static byte ReadStatusFlagDataType = 0x09;
            public static byte ReadStatusFlagDataLength = 0x02;
        }

        public struct ReadEEPROMDataStracture
        {
            public static byte[] ReadEEPROMOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x9B, 0xFF };
            public static byte ReadEEPROMClassID = 0x01;
            public static byte ReadEEPROMValueAttribute = 0x02;
            public static byte ReadEEPROMDataType = 0x02;
            public static byte ReadEEPROMDataLength = 0x02;
        }

        public struct ReadCalibrationDataStracture
        {
            public static byte[] ReadCalibrationOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x88, 0xFF };
            public static byte ReadCalibrationClassID = 0x01;
            public static byte ReadCalibrationValueAttribute = 0x02;
            public static byte ReadCalibrationDataType = 0x09;
            public static byte ReadCalibrationDataLength = 0x2C;
        }
        public struct AnomalyDataStracture
        {
            public static byte[] AnomalyOBIS = new byte[] { 0x00, 0x00, 0x61, 0x61, 0x80, 0xFF };
            public static byte[] AnomalyOBIS_Sapphire = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x9D, 0xFF };
            public static byte AnomalyClassID = 0x01;
            public static byte AnomalyValueAttribute = 0x02;
            public static byte AnomalyDataType = 0x09;
            public static byte AnomalyDataLength = 0x10;
        }
      
        public struct ReadFlashStatusDataStracture
        {
            public static byte[] ReadFlashStatusOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x94, 0xFF };
            public static byte ReadFlashStatusClassID = 0x01;
            public static byte ReadFlashStatusValueAttribute = 0x02;
            public static byte ReadFlashStatusDataType = 0x09;
            public static byte ReadFlashStatusDataLength = 0x0F;
        }

        public struct ReadEnggBufferDataStracture
        {
            public static byte[] ReadEnggBufferOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x9D, 0xFF };
            public static byte ReadEnggBufferClassID = 0x01;
            public static byte ReadEnggBufferValueAttribute = 0x02;
            public static byte ReadEnggBufferDataType = 0x09;
            public static byte ReadEnggBufferDataLength = 0x27;
        }
        public struct ReadBatteryStatusDataStracture
        {
            public static byte[] ReadBatteryStatusOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x9D, 0xFF }; // Same OBIS is also used for data structure AnomalyOBIS_Sapphire
            public static byte ReadBatteryStatusClassID = 0x01;
            public static byte ReadBatteryStatusValueAttribute = 0x02;

        }
        public struct ReadCTWireDataStracture
        {
            public static byte[] ReadCTWireOBIS = new byte[] { 0x01, 0x00, 0xAC, 0x07, 0x00, 0xFF };
            public static byte ReadCTWireClassID = 0x03;
            public static byte ReadCTWireValueAttribute = 0x02;

        }
        public struct ReadSignedActivePowerDataStracture
         {
            public static byte[] SignedActivePowerOBIS = new byte[] { 0x01, 0x00, 0x01, 0x07, 0x00, 0xFF };
            public static byte SignedActivePowerClassID = 0x03;
            public static byte SignedActivePowerValueAttribute = 0x02;

        }
        public struct ReadVoltageDataStracture_1Phase
        {
            public static byte[] ReadVoltageOBIS = new byte[] { 0x01, 0x00, 0x0C, 0x07, 0x00, 0xFF };
            public static byte ReadVoltageClassID = 0x03;
            public static byte ReadVoltageValueAttribute = 0x02;
            public static byte ReadVoltageDataType = 0x09;
            public static byte ReadVoltageDataLength = 0x27;
        }
        //-----------------------------------------3Phase Additional--------------------------------------------------------
        
        public struct HDLCTIMEOUTDataStracture
        {
            public static byte[] HDLCTIMEOUTOBIS = new byte[] { 0x00, 0x00, 0x16, 0x00, 0x00, 0xFF };
            public static byte HDLCTIMEOUTClassID = 0x17;
            public static byte InterFrameTIMEOUTValueAttribute = 0x07;
            public static byte InActivityTIMEOUTValueAttribute = 0x08;
            public static byte HDLCTIMEOUTDataType = 0x12;
            public static byte HDLCTIMEOUTDataLength = 0x00;
        }
        public struct KVAHSelectionDataStracture
        {
            public static byte[] KVAHSelectionOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x8F, 0xFF };
            public static byte KVAHSelectionClassID = 0x01;
            public static byte KVAHSelectionValueAttribute = 0x02;        
            public static byte KVAHSelectionDataType = 0x11;
            public static byte KVAHSelectionDataLength = 0x00;
        }
        /// <summary>
        /// Combined structure for Sapphire S2,supported programabable parameters Are
        /// KVAH SELECTION                Enable 01   , Disable 00, Para Code 01
        /// MANUAL BUTTON MD RESET        Enable 01   , Disable 00, Para Code 02
        /// AUTO BILL                     Enable 01   , Disable 00, Para Code 03
        /// SOFTWARE BILLING              Enable 01   , Disable 00, Para Code 04
        /// </summary>
        public struct CombinedProgrammableParameterCommandDataStracture
        {
            public static byte[] OBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x8E, 0xFF };
            public static byte ClassID = 0x01;
            public static byte Attribute = 0x02;
            public static byte DataType = 0x01;
            public static byte DataLength = 0x01;
        }
        public struct CTRatioDataStracture
        {
            public static byte[] CTRatioOBIS = new byte[] { 0x01, 0x00, 0x00, 0x04, 0x02, 0xFF };
            public static byte CTRatioClassID = 0x01;
            public static byte CTRatioValueAttribute = 0x02;
            public static byte CTRatioDataType = 0x12;
            public static byte CTRatioDataLength = 0x00;
        }
        public struct PTRatioDataStracture
        {
            public static byte[] PTRatioOBIS = new byte[] { 0x01, 0x00, 0x00, 0x04, 0x03, 0xFF };
            public static byte PTRatioClassID = 0x01;
            public static byte PTRatioValueAttribute = 0x02;
            public static byte PTRatioDataType = 0x12;
            public static byte PTRatioDataLength = 0x00;
        }
        public struct LoadlimitKWDataStracture
        {
            public static byte[] LoadlimitKWOBIS = new byte[] { 0x00, 0x00, 0x11, 0x00, 0x00, 0xFF };
            public static byte LoadlimitKWClassID = 0x47;
            public static byte LoadlimitKWValueAttribute_monitored_value = 0x02;
            public static byte LoadlimitKWValueAttribute_threshold_active = 0x03;
            public static byte LoadlimitKWValueAttribute_threshold_normal = 0x04;
            public static byte LoadlimitKWValueAttribute_threshold_emergency = 0x05;
            public static byte LoadlimitKWValueAttribute_min_over_threshold_duration = 0x06;
            public static byte LoadlimitKWValueAttribute_min_under_threshold_duration = 0x7;
            public static byte LoadlimitKWDataType_threshold_normal = 0x06;
            public static byte LoadlimitKWDataLength = 0x00;
        }
       

        public struct MDResetLockoutDataStracture
        {
            public static byte[] MDResetLockoutOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x89, 0xFF };
            public static byte MDResetLockoutClassID = 0x01;
            public static byte MDResetLockoutValueAttribute = 0x02;
            public static byte MDResetLockoutDataType = 0x12;
            public static byte MDResetLockoutDataLength = 0x00;
        }

        public struct LSCapturePeriodDataStracture
        {
            public static byte[] LSCapturePeriodOBIS = new byte[] { 0x01, 0x00, 0x00, 0x08, 0x04, 0xFF };
            public static byte LSCapturePeriodClassID = 0x01;
            public static byte LSCapturePeriodValueAttribute = 0x02;
            public static byte LSCapturePeriodDataType = 0x12;
            public static byte LSCapturePeriodDataLength = 0x00;
        }
        public struct ESWFDataStracture
        {
            public static byte[] ESWFOBIS = new byte[] { 0x00, 0x00, 0x5E, 0x5B, 0x1A, 0xFF };
            public static byte ESWFClassID = 0x01;
            public static byte ESWFValueAttribute = 0x02;
            public static byte ESWFDataType = 0x04;
            public static byte ESWFDataLength = 0x80; //--16 Byte that is 128 Bits
        }
        public struct LSCaptureobjectDataStracture
        {
            public static byte[] LSCaptureobjectOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x85, 0xFF };
            public static byte[] LSCaptureobjectOBIS_Falcon2 = new byte[] { 0x01, 0x00, 0x60, 0x80, 0x02, 0xFF };
            public static byte LSCaptureobjectClassID = 0x01;
            public static byte LSCaptureobjectValueAttribute = 0x02;
            public static byte LSCaptureobjectDataType = 0x12;
            public static byte LSCaptureobjectDataType_NetMetering = 0x06;
            public static byte LSCaptureobjectDataType_Falcon2 = 0x04;
            public static byte LSCaptureobjectDataLength_Falcon2 = 0x28; //40 Bits
            public static byte LSCaptureobjectDataLength = 0x00;
        }

        public struct DisplayTimeoutDataStracture
        {
            public static byte[] DisplayTimeoutOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x80, 0xFF };
            public static byte DisplayTimeoutClassID = 0x01;
            public static byte DisplayTimeoutValueAttribute = 0x02;
            public static byte DisplayTimeoutDataType = 0x02;
            public static byte DisplayTimeoutDataLength = 0x06;
            public static byte DisplayTimeoutDataLength_3PhaseNonSmart = 0x04;
        }

        public struct DisplayCoverOpenDataStracture
        {
            public static byte[] COpenOBIS = new byte[] { 0x01, 0x00, 0x00, 0x02, 0x01, 0x9C };
            public static byte COpenClassID = 0x01;
            public static byte COpenAttribute = 0x02;
            public static byte COpentDataType = 0x02;
            public static byte COpenDataLength = 0x01;
        }

        public struct LCDBacklightDataStracture
        {
            public static byte[] LCDBacklightOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x8A, 0xFF };
            public static byte LCDBacklightClassID = 0x01;
            public static byte LCDBacklightValueAttribute = 0x02;
            public static byte LCDBacklightDataType = 0x02;
            public static byte LCDBacklightDataLength = 0x02;
        }

        public struct BillingonLCDDataStracture
        {
            public static byte[] BillingonLCDOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x97, 0xFF };
            public static byte BillingonLCDClassID = 0x01;
            public static byte BillingonLCDValueAttribute = 0x02;
            public static byte BillingonLCDDataType = 0x09;
            public static byte BillingonLCDDataLength = 0x02;
        }
        public struct ManufactureYearDataStracture
        {
            public static byte[] ManufactureYearOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x04, 0xFF };
            public static byte ManufactureYearClassID = 0x01;
            public static byte ManufactureYearValueAttribute = 0x02;      
            public static byte ManufactureYearDataType = 0x12;
            public static byte ManufactureYearDataLength = 0x00;
        }

        public struct ManufactureMonthDataStracture
        {
            public static byte[] ManufactureMonthOBIS = new byte[] { 0x00, 0x00, 0x60, 0x80, 0x11, 0xFF };
            public static byte ManufactureMonthClassID = 0x01;
            public static byte ManufactureMonthValueAttribute = 0x02;
            public static byte ManufactureMonthDataType = 0x11;
            public static byte ManufactureMonthDataLength = 0x00;
        }

        public struct TamperCompartmentDataStracture
        {
            public static byte[] TamperCompartmentOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x82, 0xFF };
            public static byte TamperCompartmentClassID = 0x01;
            public static byte TamperCompartmentValueAttribute = 0x02;
            public static byte TamperCompartmentDataType = 0x09;
            public static byte TamperCompartmentDataLength = 0x12;
            public static byte TamperCompartmentDataLength_3PhaseDLMS = 0x0C;
        }

        public struct TamperCompartmentConfigDataStracture
        {
            // 01	00	00	02	01	9F

            public static byte[] TamperCompartmentOBIS = new byte[] { 0x01, 0x00, 0x00, 0x02, 0x01, 0x9F };
            public static byte TamperCompartmentClassID = 0x01;
            public static byte TamperCompartmentValueAttribute = 0x02;
            public static byte TamperCompartmentDataType = 0x04;
            public static byte TamperCompartmentDataLength = 0x01;
        }

        public struct TamperThresholdDataStracture
        {
            public static byte[] TamperThresholdOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x81, 0xFF };

            public static byte[][] ThresholdOBIS = new byte[][] {
                                                                         new byte[]{01, 00, 12, 130, 131, 255},/* Missing Pot. Persist*/
                                                                         new byte[]{01, 00, 12, 129, 131, 255},/* Missing poten. Threshold*/
                                                                         new byte[]{01, 00, 12, 130, 128, 255},/* voltage Unbal. Persist */
                                                                         new byte[]{01, 00, 12, 129, 128, 255},/* voltage Unbalance Threshold*/
                                                                         new byte[]{01, 00, 12, 130, 129, 255},/* Over voltage Persist */
                                                                         new byte[]{01, 00, 12, 129, 129, 255},/* Over voltage Threshold*/
                                                                         new byte[]{01, 00, 12, 130, 130, 255},/* Low voltage Persist */
                                                                         new byte[]{01, 00, 12, 129, 130, 255},/* Low voltage Threshold*/
                                                                         new byte[]{01, 00, 11, 130, 128, 255},/* Current Reversal. Persist */
                                                                         new byte[]{01, 00, 11, 129, 128, 255},/* Current Reverseal Threshold*/
                                                                         new byte[]{01, 00, 11, 130, 129, 255},/* CT open Persist */
                                                                         new byte[]{01, 00, 11, 129, 129, 255},/* CT Open Threshold*/
                                                                         new byte[]{01, 00, 11, 130, 131, 255},/* CT bypass Persist */
                                                                         new byte[]{01, 00, 11, 129, 131, 255},/* CT bypass Threshold*/
                                                                         new byte[]{01, 00, 11, 130, 132, 255},/* Over Current Persist*/
                                                                         new byte[]{01, 00, 11, 129, 132, 255},/* Over current Threshold*/
                                                                         new byte[]{01, 00, 11, 130, 130, 255},/* Current Unbal. Persist */
                                                                         new byte[]{01, 00, 11, 129, 130, 255},/* Current Unbal. Threshold*/
                                                                         new byte[]{00, 00, 96, 128, 04, 255},/* Power On-off */
                                                                         new byte[]{00, 00, 96, 128, 05, 255},/* Magnetic Influence */
                                                                         new byte[]{01, 00, 96, 128, 00, 255},/* Neutral Distb. */
                                                                         new byte[]{01, 00, 13, 130, 128, 255},/* Very Low PF Persist*/
                                                                         new byte[]{01, 00, 13, 129, 128, 255},/* very low PF Threshold*/
                                                                         new byte[]{01, 00, 01, 130, 128, 255},/* Over load persist */
                                                                         new byte[]{01, 00, 91, 130, 128, 255},/* High Neu. curr. Persist*/
                                                                         new byte[]{01, 00, 91, 129, 128, 255},/* High Neutral Curr. Threshold*/
                                                                         new byte[]{00, 00, 96, 128, 06, 255},/* Temp. Rise Persist*/
                                                                         new byte[]{00, 00, 96, 128, 03, 255},/* Temp Rise */
                                                                         new byte[]{01, 00, 96, 128, 04, 255},/* Invalid Phase Association */
                                                                                                           
                                                                  };
            public static byte[][] ThresholdOBIS_1phFalcon2 = new byte[][] {
                                                                        
                                                                         new byte[]{01, 00, 12, 130, 129, 255},/* Over voltage Persist */
                                                                         new byte[]{01, 00, 12, 129, 129, 255},/* Over voltage */
                                                                         new byte[]{01, 00, 12, 130, 130, 255},/* Low voltage Persist */
                                                                         new byte[]{01, 00, 12, 129, 130, 255},/* Low voltage */
                                                                         new byte[]{01, 00, 01, 130, 128, 255},/* Over load persist */
                                                                         new byte[]{01, 00, 11, 130, 132, 255},/* Over Current Persist*/
                                                                         new byte[]{01, 00, 11, 129, 132, 255},/* Over current*/
                                                                         new byte[]{00, 00, 96, 128, 06, 255},/* Temp. Rise Persist*/
                                                                         new byte[]{00, 00, 96, 128, 03, 255},/* Temp Rise */
                                                                         new byte[]{01, 00, 11, 130, 133, 255},/* Curr.Mismatch Persist */
                                                                         new byte[]{01, 00, 11, 129, 133, 255},/* Current Mismatch threshold.*/
                                                                                                           
                                                                  };

            public static byte TamperThresholdClassID = 0x01;
            public static byte TamperThresholdValueAttribute = 0x02;
            public static byte TamperThresholdDataType = 0x02;
            public static byte TamperThresholdDataType_3PNONDLMS = 0x09;
            public static byte[] TamperThresholdDataLength = new byte[] {   0x02,/* Missing Pot. Persist*/
                                                                            0x04,/* Missing Pot. Threshold*/
                                                                            0x02,/* voltage Unbal. Persist */
                                                                            0x08,/* voltage Unbal. Threshold */
                                                                            0x02,/* Over voltage Persist */
                                                                            0x02,/* Over voltage Threshold */
                                                                            0x02,/* Low voltage Persist */
                                                                            0x02,/* Low voltage Threshold */
                                                                            0x02,/* Current Reversal. Persist */
                                                                            0x04,/* Current Reversal. Threshold */
                                                                            0x02,/* CT open Persist */
                                                                            0x04,/* CT open Threshold */
                                                                            0x02,/* CT bypass Persist */
                                                                            0x04,/* CT bypass Threshold */
                                                                            0x02,/* Over Current Persist*/
                                                                            0x02,/* Over Current Threshold*/
                                                                            0x02,/* Current Unbal. Persist */
                                                                            0x06,/* Current Unbal. Threshold */
                                                                            0x00,/* Power On-off */
                                                                            0x02,/* Magnetic Influence */
                                                                            0x02,/* Neutral Distb. */
                                                                            0x02,/* Very Low PF Persist*/
                                                                            0x06,/* Very Low PF Threshold*/
                                                                            0x02,/* Over load persist */
                                                                            0x02,/* High Neu. curr. Persist*/
                                                                            0x02,/* High Neu. curr. Threshold*/
                                                                            0x02,/* Temp. Rise Persist*/
                                                                            0x02, /* Temp. Rise Persist*/
                                                                            0x02 /* Invalid Phase Association Persist*/
                                                                        };
        
            public static byte[] TamperThresholdDataLength1pH = new byte[] { 0x02, 0x02, 0x02, 0x04, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02 };

        
            public static byte TamperThresholdDataType_Value = 0x12;
            public static byte TamperThresholdDataLength_Value = 0x00;
        }

        public struct TamperThresholdOverVoltageDataStracture
        {
            public static byte[] TamperThresholdOverVoltageOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0xCA, 0xFF };
            public static byte TamperThresholdOverVoltageClassID = 0x01;
            public static byte TamperThresholdOverVoltageValueAttribute = 0x02;
            public static byte TamperThresholdOverVoltageDataType = 0x09;
            public static byte TamperThresholdOverVoltageDataLength = 0x06;
        }
        public struct TamperThresholdLowVoltageDataStracture
        {
            public static byte[] TamperThresholdLowVoltageOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0xCB, 0xFF };
            public static byte TamperThresholdLowVoltageClassID = 0x01;
            public static byte TamperThresholdLowVoltageValueAttribute = 0x02;
            public static byte TamperThresholdLowVoltageDataType = 0x09;
            public static byte TamperThresholdLowVoltageDataLength = 0x06;
        }
        public struct TamperThresholdOverLoadDataStracture
        {
            public static byte[] TamperThresholdOverLoadOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0xCC, 0xFF };
            public static byte TamperThresholdOverLoadClassID = 0x01;
            public static byte TamperThresholdOverLoadValueAttribute = 0x02;
            public static byte TamperThresholdOverLoadDataType = 0x09;
            public static byte TamperThresholdOverLoadDataLength = 0x06;
            public static byte TamperThresholdOverLoadDataLength_3PhaseDLMS_TPDDL = 0x0C;
        }
        public struct TamperThresholdVeryLowPFDataStracture
        {
            public static byte[] TamperThresholdVeryLowPFOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0xCD, 0xFF };
            public static byte TamperThresholdVeryLowPFClassID = 0x01;
            public static byte TamperThresholdVeryLowPFValueAttribute = 0x02;
            public static byte TamperThresholdVeryLowPFDataType = 0x09;
            public static byte TamperThresholdVeryLowPFDataLength = 0x06;
        }
        public struct TamperThresholdOverCurrentDataStracture
        {
            public static byte[] TamperThresholdOverCurrentOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0xD4, 0xFF };
            public static byte TamperThresholdOverCurrentClassID = 0x01;
            public static byte TamperThresholdOverCurrentValueAttribute = 0x02;
            public static byte TamperThresholdOverCurrentDataType = 0x09;
            public static byte TamperThresholdOverCurrentDataLength = 0x06;
        }
        public struct BaudRateDataStracture
        {
            public static byte[] BaudRateOBIS = new byte[] { 0x00, 0x00, 0x16, 0x00, 0x00, 0xFF };
            public static byte BaudRateClassID = 0x17;
            public static byte BaudRateValueAttribute = 0x02;
            public static byte BaudRateDataType = 0x16;
            public static byte BaudRateDataLength = 0x00;
        }
        public struct BatterymodeDisplayDataStructure
        {
            public static byte[] BatterymodeDisplayOBIS = new byte[] { 0x00, 0x00, 0x60, 0x80, 0x0C, 0xFF };
            public static byte BatterymodeDisplayClassID = 0x01;
            public static byte BatterymodeDisplayValueAttribute = 0x02;
            public static byte BatterymodeDisplayDataType = 0x09;
            public static byte BatterymodeDisplayDataLength = 0x40;
        }
        public struct IECHDLCSETUPDataStracture
        {
            public static byte[] IECHDLCSETUPOBIS = new byte[] { 0x00, 0x00, 0x16, 0x00, 0x00, 0xFF };
            public static byte IECHDLCSETUPClassID = 0x17;
            public static byte IECHDLCSETUP_BaudRateValueAttribute = 0x02;
            public static byte BaudRate_DataType = 0x16;
            public static byte BaudRate_DataLength = 0x00;
        }

        public struct LLSKeyDataStracture
        {
           // public static byte[] LLSKeyOBIS = new byte[] { 0x00, 0x00, 0x28, 0x00, 0x02, 0xFF };
            public static byte[] LLSKeyOBIS = new byte[] { 0x00, 0x00, 0x2B, 0x01, 0x02, 0xFF };//MR For smart meter
            public static byte LLSKeyClassID = 0x0F;
            public static byte LLSKeyValueAttribute = 0x07;
            public static byte LLSKeyDataType = 0x0A;
            public static byte LLSKeyDataLength = 0x08;
        }
        
        public struct HLSKeyDataStracture
        {
            //public static byte[] HLSKeyOBIS = new byte[] { 0x00, 0x00, 0x28, 0x00, 0x03, 0xFF };
            public static byte[] HLSKeyOBIS = new byte[] { 0x00, 0x00, 0x2B, 0x01, 0x03, 0xFF };//US For smart meter
            public static byte HLSKeyClassID = 0x0F;
            public static byte HLSKeyValueAttribute_Write = 0x02;
            public static byte HLSKeyValueAttribute_Read = 0x07;
            public static byte HLSKeyDataType = 0x09;
            public static byte HLSKeyDataLength = 0x10;
        }
        public struct LNAssociationDataStracture
        {
            public static byte[] LNAssociationOBIS_Current = new byte[]     { 0x00, 0x00, 0x28, 0x00, 0x00, 0xFF };
            public static byte[] LNAssociationOBIS_PC = new byte[]          { 0x00, 0x00, 0x28, 0x00, 0x01, 0xFF };
            public static byte[] LNAssociationOBIS_MR = new byte[]          { 0x00, 0x00, 0x28, 0x00, 0x02, 0xFF };
            public static byte[] LNAssociationOBIS_US = new byte[]          { 0x00, 0x00, 0x28, 0x00, 0x03, 0xFF };
            public static byte[] LNAssociationOBIS_Push = new byte[]        { 0x00, 0x00, 0x28, 0x00, 0x04, 0xFF };
            public static byte[] LNAssociationOBIS_FU = new byte[]          { 0x00, 0x00, 0x28, 0x00, 0x05, 0xFF };

            public static byte LNAssociationClassID = 0x0F;
            public static byte LNAssociationValueAttribute_Method = 0x02;
            public static byte LNAssociationValueAttribute = 0x07;
            public static byte LNAssociationDataType_HLS = 0x09;
            public static byte LNAssociationDataLength_HLS = 0x10;
            public static byte LNAssociationDataType_LLS = 0x09;
            public static byte LNAssociationDataLength_LLS = 0x08;
        }
        public struct SecuritySetupDataStracture
        {
            public static byte[] SecuritySetupOBIS_GlobalKey = new byte[] { 0x00, 0x00, 0x2B, 0x00, 0x00, 0xFF };
           // public static byte[] SecuritySetupOBIS_PC = new byte[]      { 0x00, 0x00, 0x2B, 0x00, 0x01, 0xFF };
            public static byte[] SecuritySetupOBIS_MR = new byte[]      { 0x00, 0x00, 0x2B, 0x00, 0x02, 0xFF };
            public static byte[] SecuritySetupOBIS_US = new byte[]      { 0x00, 0x00, 0x2B, 0x00, 0x03, 0xFF };
            public static byte[] SecuritySetupOBIS_Push = new byte[]    { 0x00, 0x00, 0x2B, 0x00, 0x04, 0xFF };
            public static byte[] SecuritySetupOBIS_FU = new byte[]      { 0x00, 0x00, 0x2B, 0x00, 0x05, 0xFF };

            public static byte SecuritySetupClassID = 0x40;
            public static byte SecuritySetupValueAttribute_Write = 0x02;
            public static byte SecuritySetupValueAttribute_Read = 0x07;
            public static byte SecuritySetupDataType = 0x01;
            public static byte SecuritySetupDataLength = 0x01;
        }
        public struct SingleActionScheduleDataStracture
        {
            public static byte[] SingleActionScheduleOBIS_ImageActivation = new byte[] { 0x00, 0x00, 0x0F, 0x00, 0x02, 0xFF };
            public static byte SingleActionScheduleClassID = 0x16;
            public static byte SingleActionScheduleValueAttribute_ExecutionTime = 0x04;
            public static byte SingleActionScheduleDataType = 0xFF;
            public static byte SingleActionScheduleDataLength = 0xFF;
           
        }
        public struct DigitalIOConfigurationDataStracture
        {
            public static byte[] DigitalIOConfigurationOBIS = new byte[] { 0x01, 0x00, 0x60, 0x80, 0x03, 0xFF };
            public static byte DigitalIOConfigurationClassID = 0x01;
            public static byte DigitalIOConfigurationValueAttribute = 0x02;
            public static byte DigitalIOConfigurationDataType = 0x03;
            public static byte DigitalIOConfigurationDataLength = 0x00;

        }
        public struct ResetDataStracture
        {
            public static byte[] MgtTamperIconOBIS = new byte[]   { 0x00, 0x01, 0x0A, 0x01, 0x00, 0xFF };
            public static byte[] ResetEnergyOBIS = new byte[]     { 0x00, 0x01, 0x0A, 0x02, 0x00, 0xFF };
            public static byte[] ResetMDOBIS = new byte[]         { 0x00, 0x01, 0x0A, 0x03, 0x00, 0xFF };
            public static byte[] ResetLoadSurveyOBIS = new byte[] { 0x00, 0x01, 0x0A, 0x04, 0x00, 0xFF };
            public static byte[] ResetTamperOBIS = new byte[]     { 0x00, 0x01, 0x0A, 0x05, 0x00, 0xFF };
            public static byte[] ResetBillingOBIS = new byte[]    { 0x00, 0x01, 0x0A, 0x06, 0x00, 0xFF };           
            public static byte[] ResetDailySurveyOBIS = new byte[]{ 0x00, 0x01, 0x0A, 0x07, 0x00, 0xFF };
            public static byte[] ResetALLOBIS = new byte[]        { 0x00, 0x01, 0x0A, 0x08, 0x00, 0xFF };
            public static byte[] MDResetsOBIS = new byte[]        { 0x00, 0x01, 0x0A, 0x09, 0x00, 0xFF };
            public static byte[] MDResetsOBIS_Falcon = new byte[] { 0x00, 0x00, 0x0A, 0x00, 0x01, 0xFF };
            public static byte[] ResetLowBattOBIS = new byte[] { 0x00, 0x01, 0x0A, 0x0A, 0x00, 0xFF };
            public static byte[] ResetBatteryCounterResetOBIS = new byte[] { 0x00, 0x01, 0x0A, 0x0B, 0x00, 0xFF };
            public static byte ResetClassID = 0x09;
            public static byte ResetValueAttribute = 0x01;
            public static byte ResetDataType = 0x09;
            public static byte ResetDataLength = 0x01;
            public static byte ResetDataType_Falcon2 = 0x12; 
            public static byte ResetDataLength_Falcon2 = 0x00;
        }

        public struct CalibrationStractureDataStracture
        {
            public static byte[] CalibrationStractureOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x91, 0xFF };
            public static byte CalibrationStractureClassID = 0x01;
            public static byte CalibrationStractureValueAttribute = 0x02;
            public static byte CalibrationStractureDataType = 0x09;
            public static byte CalibrationStractureDataLength = 0x01;
        }
        public struct MeterLOCKDataStracture
        {
            public static byte[] MeterLOCKOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x92, 0xFF };
            public static byte MeterLOCKClassID = 0x01;
            public static byte MeterLOCKValueAttribute = 0x02;
            public static byte MeterLOCKDataType = 0x11;
            public static byte MeterLOCKDataLength = 0x00;
        }
        public struct ReadNeutralCurrentDataStracture
        {
            public static byte[] ReadNeutralCurrentOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x93, 0xFF };
            public static byte ReadNeutralCurrentClassID = 0x01;
            public static byte ReadNeutralCurrentValueAttribute = 0x02;
            public static byte ReadNeutralCurrentDataType = 0x09;
            public static byte ReadNeutralCurrentDataLength = 0x01;
        }
        public struct ReadCaseTamperCounterDataStracture
        {
            public static byte[] ReadCaseTamperCounterOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x93, 0xFF };
            public static byte ReadCaseTamperCounterClassID = 0x01;
            public static byte ReadCaseTamperCounterValueAttribute = 0x02;          
        }
        public struct ReferenceVoltageDataStracture
        {
            public static byte[] ReferenceVoltageOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x9E, 0xFF };
            public static byte RefVoltageClassID = 0x01;
            public static byte ReadRefVoltageValueAttribute = 0x02;
            public static byte ReferenceVoltageDataType = 0x09;          
            public static byte ReferenceVoltageDataLength = 0x01;          

        }
        public struct CurrentRatingDataStracture
        {
            public static byte[] CurrentRatingOBIS = new byte[] { 0x00, 0x00, 0x5E, 0x5B, 0x0C, 0xFF };
            public static byte CurrentRatingClassID = 0x01;
            public static byte CurrentRatingValueAttribute = 0x02;
            public static byte CurrentRatingDataType = 0x0A;            
            public static byte CurrentRatingDataLength = 0x07;
            public static byte CurrentRatingDataLength_3Phase = 0x08;
       }
               
        public struct ImageTransfer
        {
            public static byte[] ImageTransferOBIS = new byte[] { 0x00, 0x00, 0x2C, 0x00, 0x00, 0xFF };
            public static byte ImageTransferClassID = 0x12;
            public static byte ImageBlockSize = 0x02;
            public static byte ImageTransferBlockStatus = 0x03;
            public static byte ImageFirstNotTransferedBlockNumber = 0x04;
            public static byte ImageTrabsferEnabled = 0x05;
            public static byte ImageTransferStatus = 0x06;
            public static byte ImageToActivateInfo = 0x07;
            public static byte ImageTransferInitMethodID = 0x01;
            public static byte ImageTransferInittypeofdata = 0x02;
            public static byte ImageTransferInitdatalength = 0x02;
            public static byte ImageTransferblockmethodid = 0x02;
            public static byte Imageverifymethodid = 0x03;
            public static byte Imageactivatearray = 0x01;
            public static byte Imageactivatearraylength = 0x01;



            //public static byte[] BootModeEnableOBIS = new byte[] { 0x00, 0x00, 0x2C, 0x00, 0x00, 0xFF };
           // public static byte BootModeEnableClassID = 0x12;
            public static byte BootModeEnableAttribute = 0x01;
            public static byte BootModeEnableAttribute_Activate = 0x04;
            public static byte BootModeEnableDataType = 0x09;
            public static byte BootModeEnableDataType_Activate = 0x09;
            public static byte BootModeEnableDataLength = 0x10;
            public static byte BootModeEnableDataLength_Activate = 0x01;

        }
        public struct EngineeringCommandDataStracture_3Phase
        {
            public static byte[] VoltageOBIS_RPhase = new byte[] { 0x01, 0x00, 0x20, 0x07, 0x00, 0xFF };
            public static byte[] VoltageOBIS_YPhase = new byte[] { 0x01, 0x00, 0x34, 0x07, 0x00, 0xFF };
            public static byte[] VoltageOBIS_BPhase = new byte[] { 0x01, 0x00, 0x48, 0x07, 0x00, 0xFF };

            public static byte[] CurrentOBIS_RPhase = new byte[] { 0x01, 0x00, 0x1F, 0x07, 0x00, 0xFF };
            public static byte[] CurrentOBIS_YPhase = new byte[] { 0x01, 0x00, 0x33, 0x07, 0x00, 0xFF };
            public static byte[] CurrentOBIS_BPhase = new byte[] { 0x01, 0x00, 0x47, 0x07, 0x00, 0xFF };

            public static byte[] PFOBIS_RPhase= new byte[] { 0x01, 0x00, 0x21, 0x07, 0x00, 0xFF };
            public static byte[] PFOBIS_YPhase = new byte[] { 0x01, 0x00, 0x35, 0x07, 0x00, 0xFF };
            public static byte[] PFOBIS_BPhase = new byte[] { 0x01, 0x00, 0x49, 0x07, 0x00, 0xFF };

            public static byte EngineeringCommand_3Phase_ClassID = 0x03;
            public static byte EngineeringCommand_3Phase_Attribute_Value = 0x02;
            public static byte EngineeringCommand_3Phase_Attribute_Scalar = 0x03;
        }
        public struct CTDirectionDataStracture
        {
            public static byte[] CTDirectionOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x90, 0xFF };
            public static byte CTDirectionClassID = 0x01;
            public static byte CTDirectionValueAttribute = 0x02;
            public static byte CTDirectionDataType = 0x09;
            public static byte CTDirectionDataLength = 0x01;
        }
        public struct CasteTamperDataStracture
        {
            public static byte[] CasteTamperOBIS = new byte[] { 0x00, 0x00, 0x60, 0x0B, 0x05, 0xFF };
            public static byte CasteTamperClassID = 0x01;
            public static byte CasteTamperValueAttribute = 0x02;
            public static byte CasteTamperDataType = 0x12;
           
        }
        public struct MeterFWVersionDataStracture
        {
            public static byte[] MeterFWVersionOBIS = new byte[] { 0x01, 0x00, 0x00, 0x02, 0x00, 0xFF };
            public static byte MeterFWVersionClassID = 0x01;
            public static byte MeterFWVersionValueAttribute = 0x02;
            public static byte MeterFWVersionDataType = 0x09;
            public static byte MeterFWVersionDataLength = 0x03;
        }

        public struct RS485DeviceAddressDataStracture
        {
            public static byte[] RS485DeviceaddressOBIS = new byte[] { 0x00, 0x00, 0x16, 0x00, 0x00, 0xFF };
            public static byte RS485DeviceaddressClassID = 0x17;
            public static byte RS485DeviceaddressValueAttribute = 0x09;
            public static byte RS485DeviceaddressDataType = 0x12;
            public static byte RS485DeviceaddressDataLength = 0x00;
        }
        public struct PortConfigurationDataStracture
        {
            public static byte[] PortConfigurationRJOBIS = new byte[]      { 0x00, 0x00, 0x60, 0x32, 0x07, 0xFF };
            public static byte[] PortConfigurationOPTICALOBIS = new byte[] { 0x00, 0x00, 0x60, 0x32, 0x06, 0xFF };
            public static byte PortConfigurationClassID = 0x01;
            public static byte PortConfigurationValueAttribute = 0x02;
            public static byte PortConfigurationDataType = 0x16;
            public static byte PortConfigurationDataLength = 0x00;
        }
        public struct MeterTypeDataStructure
        {
            // 01	00	00	02	01	9E
            public static byte[] MeterTypeOBIS = new byte[] { 0x01, 0x00, 0x00, 0x02, 0x01, 0x9E };
            public static byte[] MeterTypeOBIS_Falcon2 = new byte[] { 0x00, 0x00, 0x60, 0x80, 0x09, 0xFF };
            public static byte MeterTypeClassID = 0x1;
            public static byte MeterTypeValueAttribute = 0x02;
            public static byte MeterTypeDataType = 0x3;
            public static byte MeterTypeDataLength = 0x00;

        }
        public struct FSModeLockingDataStructure
        {
            public static byte[] FSModeLockingOBIS = new byte[] { 0x00, 0x00, 0x60, 0x80, 0x0A, 0xFF };
            public static byte FSModeLockingClassID = 0x1;
            public static byte FSModeLockingValueAttribute = 0x02;
            public static byte FSModeLockingDataType = 0x11;
            public static byte FSModeLockingDataLength = 0x00;

        }
        public struct JTAGLockingDataStructure
        {
            public static byte[] JTAGLockingOBIS = new byte[] { 0x00, 0x00, 0x60, 0x80, 0x0E, 0xFF };
            public static byte JTAGLockingClassID = 0x1;
            public static byte JTAGLockingValueAttribute = 0x02;
            public static byte JTAGLockingDataType = 0x11;
            public static byte JTAGLockingDataLength = 0x00;

        }
        public struct LANIDDataStructure
        {
            public static byte[] LANIDOBIS = new byte[] { 0x00, 0x00, 0x60, 0x80, 0x0B, 0xFF };
            public static byte LANIDClassID = 0x1;
            public static byte LANIDValueAttribute = 0x02;
            public static byte LANIDDataType = 0x09;
            public static byte LANIDDataLength = 0x04;

        }
        public struct DisconnectonMagnetDataStracture
        {
            public static byte[] DisconnectonMagnetOBIS = new byte[] { 0x01, 0x00, 0x00, 0x02, 0x01, 0x9D };
            public static byte DisconnectonMagnetClassID = 0x01;
            public static byte DisconnectonMagnetValueAttribute = 0x02;
            public static byte DisconnectonMagnetDataType = 0x11;
            public static byte DisconnectonMagnetDataLength = 0x00;
        }

        public struct ConfigCRC
        {
            public static byte[] ConfigCRCOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0x9E, 0xFF };
            public static byte ConfigCRCClassID = 0x1;
            public static byte ConfigCRCAttribute = 0x02;
            public static byte ConfigCRCDataType = 0x12;
        }
        public struct CalibrationSpeceficDataStruct_3Phase
        {             
            public static byte[] Calibration_DLMSOBIS = new byte[] { 0xEE, 0x25, 0x00, 0x00, 0x13};
            public static byte[] Calibration_DLMSRUBYOBIS = new byte[] { 0xEE, 0xAA, 0x00, 0x00, 0x98 };
           
        }
        public struct AutoBillingEnableDisableDataStracture
        {
            public static byte[] AutoBillingEnableDisableOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0xC3, 0xFF };
            public static byte AutoBillingEnableDisableClassID = 0x01;
            public static byte AutoBillingEnableDisableValueAttribute = 0x02;
            public static byte AutoBillingEnableDisableDataType = 0x11;
            public static byte AutoBillingEnableDisableDataLength = 0x00;

        }
        public struct RS232LockUnlockDataStracture
        {
            public static byte[] RS232LockUnlockOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0xC4, 0xFF };
            public static byte RS232LockUnlockClassID = 0x01;
            public static byte RS232LockUnlockValueAttribute = 0x02;
            public static byte RS232LockUnlockDataType = 0x11;
            public static byte RS232LockUnlockDataLength = 0x00;
        }
        public struct FactoryConfigurabelityDataStracture
        {
            public static byte[] FactoryConfigurabelityOBIS = new byte[] { 0x00, 0x00, 0x60, 0x01, 0x9F, 0xFF };
            public static byte FactoryConfigurabelityClassID = 0x01;
            public static byte FactoryConfigurabelityValueAttribute = 0x02;
            public static byte FactoryConfigurabelityDataType = 0x09;
            public static byte FactoryConfigurabelityDataLength = 0x40;
        }
          
        public struct BootModeEnableDataStracture
        {
            public static byte[] BootModeEnableOBIS = new byte[] { 0x00, 0x00, 0x60, 0x02, 0xBF, 0xFF };
            public static byte BootModeEnableClassID = 0x01;
            public static byte BootModeEnableAttribute = 0x02;
            public static byte BootModeEnableDataType = 0x11;  
        }
        public struct FirmwareUpgradeLogDataStracture
        {
            public static byte[] FirmwareUpgradeLogOBIS = new byte[] { 0x00, 0x00, 0x60, 0x80, 0x0F, 0xFF };
            public static byte FirmwareUpgradeLogClassID = 0x01;
            public static byte FirmwareUpgradeLogAttribute = 0x02;
            public static byte FirmwareUpgradeLogDataType = 0x11;
        }

        public struct BillingDateDataStracture
        {
            public static byte[] BillingDateOBIS = new byte[] { 0x00, 0x00, 0x00, 0x01, 0x02, 0xFF };
            public static byte BillingDateClassID = 0x03;
            public static byte BillingDateAttribute = 0x02;
            public static byte BillingDateDataType = 0x09;
            public static byte BillingDateDataLength = 0x0C;
        }
        public struct MDResetCounterDataStracture
        {
            public static byte[] MDResetCounterOBIS = new byte[] { 0x00, 0x00, 0x00, 0x01, 0x00, 0xFF };
            public static byte MDResetCounterClassID = 0x01;
            public static byte MDResetCounterAttribute = 0x02;
            public static byte MDResetCounterDataType = 0x12;
            public static byte MDResetCounterDataLength = 0x00;
        }

        //------------------------------------------Generic Functions--------------------------------------------------------

        public static string GetHexStringPatternByte(byte[] dataCollection)
        {
            string dataValue = string.Empty;
            int byteCnt = 0;
            while (byteCnt < dataCollection.Length)
            {
                dataValue += dataCollection[byteCnt].ToString("X").PadLeft(2, '0');
                byteCnt++;
            }
            return dataValue;
        }

        public static string GetHexStringPatternByte(byte dataCollection)
        {
            string dataValue = dataCollection.ToString("X").PadLeft(2, '0');
            return dataValue;
        }

        public static byte[] GetByteFromHexStringPattern(string dataCollection)
        {

            byte[] dataVal = new byte[dataCollection.Length / 2];

            int datCount = 0;
            int byteCnt = 0;

            while (byteCnt < dataCollection.Length)
            {
                dataVal[datCount++] = Convert.ToByte(Convert.ToInt32(dataCollection.Substring(byteCnt, 2), 16));
                byteCnt += 2;
            }

            return dataVal;
        }

        public static byte[] GetByteFromHexStringPattern(string dataCollection, string strtagname)
        {
            const int iSTARTOBIS = 0x02;
            const int iOBISLENGTH = 12;
            const int iSTARTATTID = 14;
            const int iATTIDLENGTH = 2;
            const int iSTARTDATA = 18;
            const int iUNUSEDBYTES = 19;
            const int iUNUSEDBYTESCOUNT = 4;
            const int invokeIDandPriority = 0xC1;
            const int CmdWriteType = 0xC1;
            const int CmdWriteSubType = 0x01;
            if (strtagname.Contains("FutureDayProfile") || strtagname.Contains("SeasonProfile") || strtagname.Contains("WeekProfile"))
                dataCollection = dataCollection.Remove(iUNUSEDBYTES, iUNUSEDBYTESCOUNT);

            string _obiscode = dataCollection.Substring(iSTARTOBIS, iOBISLENGTH);
            int _classid = Convert.ToByte(Convert.ToInt32(dataCollection.Substring(0, 2), 16));//int.Parse(dataCollection.Substring(iSTARTCLASSID, iCLASSIDLENGTH));
            string _attid = dataCollection.Substring(iSTARTATTID, iATTIDLENGTH);//--Att ID 1 Byte
            string _accSelector = dataCollection.Substring(iSTARTATTID+2, iATTIDLENGTH);//-- Access Selector 1 Byte
            string _data = dataCollection.Substring(iSTARTDATA, dataCollection.Length - iSTARTDATA);

           // string _cmddata = (_data.Length / 2).ToString("X4") + _obiscode + _attid + _classid.ToString("X4")+ _data; //---Falcon 1 Implementation
            string CosemPacket = CmdWriteSubType.ToString("X2") + invokeIDandPriority.ToString("X2") + _classid.ToString("X4") + _obiscode + _attid + _accSelector + _data;
            string _cmddata = CmdWriteType.ToString("X2") + (CosemPacket.Length / 2).ToString("X4") + CosemPacket;//---Falcon 2 Implementation
            byte[] dataVal = new byte[_cmddata.Length / 2];

            int datCount = 0;
            int byteCnt = 0;
            //_cmddata = dataCollection; //--Temp for CC testing
            //dataVal = new byte[_cmddata.Length / 2];
            while (byteCnt < _cmddata.Length)
            {
                dataVal[datCount++] = Convert.ToByte(Convert.ToInt32(_cmddata.Substring(byteCnt, 2), 16));
                byteCnt += 2;
            }

            return dataVal;
        }
        
        public static string ReadUserFileData(string fileName)
        {
            try
            {
                string strTempData = "";
                string strFileData = "";
                StreamReader SR = File.OpenText(fileName);
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fs.Length <= 0) return "";
                if ((strTempData = SR.ReadToEnd()) != null) strFileData = strTempData;
                SR.Close();
                fs.Close();
                return strFileData;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static bool WriteUserFileData(string fileName, string fileContent)
        {
            try
            {
                FileStream file1 = new FileStream(fileName, FileMode.Create);
                StreamWriter wr1 = new StreamWriter(file1);
                wr1.Write(fileContent);
                wr1.Close();
                file1.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string HexToDecimalConversion(string strHex)
        {
            try
            {
                int bytecnt = 0;
                bytecnt = strHex.Length;
                string strtemp = "";
                while (bytecnt > 0)
                {
                    strtemp += strHex.Substring(bytecnt - 2, 2);
                    bytecnt -= 2;
                }
                long aa = Int64.Parse(strtemp, System.Globalization.NumberStyles.HexNumber);
                return aa.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string[] DLMSDataFormator(byte[] Blockdata, int nByteIndex,bool IsASCII)
        {
            try
            {
            bool bUnsignFlag = false;
            byte[] buffer=new byte[1];
            string data = "";
            string[] dataValue = new string[2];
            bool isASCIIString = false;
            int startdataIDX = nByteIndex;           
            byte indexedDataType = (byte)Blockdata[nByteIndex];
            SWITCHAGAIN:
            switch (indexedDataType)
            {
                case (int)DLMSDataStracture.Null_Type:                                    //0- NULL                   
                    break;
                case (int)DLMSDataStracture.Array_Type:                                   //1- Array
                    buffer = new byte[Blockdata[nByteIndex + 1]]; nByteIndex += 2; 
                    break;
                case (int)DLMSDataStracture.Structure:                                    //2-Structure
                    buffer = new byte[Blockdata[nByteIndex + 1]]; nByteIndex += 2; 
                    break;
                case (int)DLMSDataStracture.Boolean_Type:                                 //3- Boolean
                    buffer = new byte[1]; nByteIndex += 1; 
                    break;
                case (int)DLMSDataStracture.BitString_Type:                              //4- Bit String
                    int bitLength = Blockdata[nByteIndex + 1];
                    if (Blockdata[nByteIndex + 1] == 0x81) nByteIndex += (Blockdata[nByteIndex + 2 ] / 8) + 3;
                    else nByteIndex += (Blockdata[nByteIndex + 1] / 8) + 2;
                    data = GetBitString(Blockdata);
                    buffer = null;
                    break;
                case (int)DLMSDataStracture.DoubleLong_Type:                             //5- Double Long -- 4Byte
                    bUnsignFlag = true;
                    buffer = new byte[0x4]; nByteIndex++;
                    break;
                case (int)DLMSDataStracture.DoubleLongUnsigned_Type:                     //6- Double Long Unsigned -- 4Byte
                    buffer = new byte[0x4]; nByteIndex++;
                    break;
                case (int)DLMSDataStracture.OctetString_Type:                           //9- Oct String     
                    isASCIIString = true;
                    buffer = new byte[Blockdata[nByteIndex + 1]]; nByteIndex += 2; 
                    break;
                case (int)DLMSDataStracture.VisibleString_Type:                        //10- Sequence of ASCII String 
                    isASCIIString = true;
                    buffer = new byte[Blockdata[nByteIndex + 1]]; nByteIndex += 2; 
                    break;
                case (int)DLMSDataStracture.BCD_Type:                                  //13 - BCD
                    buffer = new byte[Blockdata[nByteIndex + 1]]; nByteIndex += 2; 
                    break;
                case (int)DLMSDataStracture.Integer_Type:                              //15- Integer 1Byte
                    bUnsignFlag = true;
                    buffer = new byte[0x1]; nByteIndex++;
                    break;
                case (int)DLMSDataStracture.Unsigned_Type:                             //17- Unsigned 1Byte
                    buffer = new byte[0x1]; nByteIndex++; 
                    break;
                case (int)DLMSDataStracture.Long_Type:                                //16- Long Signed 2 byte
                    bUnsignFlag = true;
                    buffer = new byte[0x2]; nByteIndex++; 
                    break;
                case (int)DLMSDataStracture.LongUnsigned_Type:                        //18- Unsigned 2 Byte
                    buffer = new byte[0x2]; nByteIndex++;
                    break;
                case (int)DLMSDataStracture.CompactArray_Type:                        //19- Unsigned 2 Byte
                    indexedDataType = Blockdata[nByteIndex + 1];
                    nByteIndex++;
                    goto SWITCHAGAIN;
                   
                case (int)DLMSDataStracture.Long64_Type:                             //20- Integer64   8 Byte
                    bUnsignFlag = true;
                    buffer = new byte[0x8]; nByteIndex++;
                    break;
                case (int)DLMSDataStracture.Long64Unsigned_Type:                     //21- Unsigned64  8 Byte
                    buffer = new byte[0x8]; nByteIndex++; 
                    break;
                case (int)DLMSDataStracture.Enum_Type:                               //22- Enum
                    buffer = new byte[0x1]; nByteIndex++;  
                    break;
                case (int)DLMSDataStracture.Float32_Type:                           //23- OCT String Len 4 
                    buffer = new byte[0x4]; nByteIndex++; 
                    break;
                case (int)DLMSDataStracture.Float64_Type:                           //24- OCT String Len 8 
                    buffer = new byte[0x8]; nByteIndex++; 
                    break;
                case (int)DLMSDataStracture.DateTime_Type:                         //25- OCT String Len 12 
                    buffer = new byte[0x0C]; nByteIndex++; 
                    break;
                case (int)DLMSDataStracture.Date_Type:                            //26- OCT String Len 5 
                    buffer = new byte[0x5]; nByteIndex++;
                    break;
                case (int)DLMSDataStracture.Time_Type:                            //27- OCT String Len 4 
                    buffer = new byte[0x4]; nByteIndex++;
                    break;             
             }
            if (buffer !=null && buffer.Length > 0)
            {
                int valuestart = nByteIndex;
                
                Array.Copy(Blockdata, valuestart, buffer, 0, buffer.Length);
                nByteIndex += buffer.Length;
                if (Blockdata[nByteIndex - (buffer.Length + 1)] == 0x0C && !IsASCII) data = FormatDate(buffer);
                else if (IsASCII && isASCIIString) data = FormatASCIIData(buffer);
                else data = FormatData(buffer, bUnsignFlag);
                //else if (!IsASCII) data = FormatData(buffer, bUnsignFlag);
                //else data = FormatASCIIData(buffer);
                
            }
            dataValue[0] = data;
            dataValue[1] = nByteIndex.ToString();
            return dataValue;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private static string GetBitString(byte[] ReceiveBuffer)
        {
            int dataindexByte = 18;
            int recBytelen = ReceiveBuffer[dataindexByte + 1];
            if (ReceiveBuffer[dataindexByte + 1] == 0x81) { recBytelen = ReceiveBuffer[dataindexByte + 2]; dataindexByte += 3; }
            else dataindexByte += 2;
            byte[] lsobjectData = new byte[recBytelen / 8];
            Array.Copy(ReceiveBuffer, dataindexByte, lsobjectData, 0, lsobjectData.Length);
            List<byte> convertedByteList = DLMSDataStracture.ReverseBitsofByteList(lsobjectData);
            BitArray myarra = new BitArray(convertedByteList.ToArray());

            var builder = new StringBuilder();
            foreach(var bit in myarra.Cast<bool>())
                builder.Append(bit ? "1": "0");
             return builder.ToString();
        }
        private static string GetManufactureYear(byte[] buffer)
        {
            
                int compValue = 0;
                compValue = (compValue | (int)buffer[0]) << 8;
                compValue = (compValue | (int)buffer[1]);
                string mfgYear = Convert.ToString(compValue);
                return mfgYear;
            
        }

        private static string FormatASCIIData(byte[] buffer)
        {
            string  dataVal=string.Empty;
            int startDataindx=0;
            string asciival=string.Empty;
            
                int stractcount = 0;
                int lengthodstruct = buffer.Length;//length of stract
                while (stractcount < lengthodstruct)
                {
                    dataVal = buffer[startDataindx++].ToString("X");
                    if (dataVal != "0") asciival = asciival + ((char)(Convert.ToInt32((dataVal), 16)));
                    stractcount++;
                }

                return asciival;
        }
          

        public static string FormatDate(byte[] buffer)
        {
            if (buffer[0] != 0xFF)
            {
                int nYear = 0;

                nYear = (nYear | (int)buffer[0]) << 8;
                nYear = (nYear | (int)buffer[1]);
                string year = nYear.ToString("d4");

                string month = buffer[2].ToString("d2");
                string day = buffer[3].ToString("d2");
                string time = "";
                if (buffer[7] == 0xFF)
                    time = buffer[5].ToString("d2") + ":" + buffer[6].ToString("d2");
                else
                    time = buffer[5].ToString("d2") + ":" + buffer[6].ToString("d2") + ":" + buffer[7].ToString("d2");

                string date = day + "/" + month + "/" + year + " " + time;
                return date;
            }
            else
                return "00/00/0000 00:00:00";


        }

     
        /// <summary>
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="isSignedDataType"></param>
        /// <returns></returns>
        public static string FormatData(byte[] buffer, bool isSignedDataType)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte item in buffer) sb.Append(item.ToString("X2"));
            //----------Directly Convert To Unsigned Int64 and return if data type is DLMS Unsigned-------------
            if (!isSignedDataType) return Convert.ToUInt64(sb.ToString(), 16).ToString();
            //----------To Get Signed Value, Convert the data to the Desired Type Signed Value -----------------
            switch (buffer.Length)
            {
                case 1://One Byte signed integer
                    return sbyte.Parse(sb.ToString(), System.Globalization.NumberStyles.HexNumber).ToString();
                case 2://Two Byte signed integer
                    return Int16.Parse(sb.ToString(), System.Globalization.NumberStyles.HexNumber).ToString();
                case 4://Four Byte signed integer
                    return Int32.Parse(sb.ToString(), System.Globalization.NumberStyles.HexNumber).ToString();
                case 8://Eight Byte signed integer
                    return Int64.Parse(sb.ToString(), System.Globalization.NumberStyles.HexNumber).ToString();
                default://---Do not Parse if not a appropriate numeric type
                    return  sb.ToString();
            }
        }
        public static UInt32 FormatData(byte[] buffer,int startIndex,int noOfBytes, bool isSignedDataType)
        {
            byte[] temByte = new byte[noOfBytes];
            Array.Copy(buffer, startIndex, temByte, 0, noOfBytes);
            return Convert.ToUInt32(FormatData(temByte, isSignedDataType));
        }
        /// <summary>
        ///  Method Name:GetRecordsCount
        ///  Purpose: To get number of records for Load survy and Tamper
        /// </summary>
        /// <param name="Blockdata"></param>
        /// <param name="nByteIndex"></param>
        /// <returns>no_of_records</returns>
        public static int GetRecordsCount(byte[] Blockdata, ref int nByteIndex)
        {
            nByteIndex = 1;
            int no_of_records = 0;
            int itemp = 0;

            try
            {
                if (Blockdata[nByteIndex] < 0x80)
                {
                    no_of_records = (int)Blockdata[nByteIndex];
                    nByteIndex++;
                }
                else
                {
                    itemp = Blockdata[nByteIndex] & 0x0F;
                    nByteIndex++;       // variable length encoding data type

                    for (int ind = 0; ind < itemp; ind++)
                    {
                        no_of_records = no_of_records << 8;
                        no_of_records = no_of_records | (int)Blockdata[nByteIndex];
                        nByteIndex++;
                    }
                }
            }
            catch (Exception)
            {
            }

            return no_of_records;
            
        }

        /// <summary>
        /// Method to reverse the bit of each byte and return the new byte list
        /// </summary>
        /// <param name="recByteList"></param>
        /// <returns></returns>
        public static List<byte> ReverseBitsofByteList(byte[] recByteList)
        {
            List<byte> convertedlist = new List<byte>();
            try
            {
                foreach (byte item in recByteList)
                {
                    char[] bitarr = Convert.ToString(item, 2).PadLeft(8, '0').ToCharArray();
                    Array.Reverse(bitarr);
                    convertedlist.Add((byte)Convert.ToInt32(new string(bitarr), 2));
                }
                return convertedlist;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static List<byte> ConvertStrToByte(string srddata)
        {
            List<byte> myretlist = new List<byte>();
            srddata = srddata.Replace("\r", "");
            for (int i = 0; i < srddata.Length; )
            {
                byte bval = byte.Parse(srddata.Substring(i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                i++; i++;
                myretlist.Add(bval);
            }
            return myretlist;
        }

        public static List<byte> GetPasswordBytes(string refpwd, string refSecuritykey, string refMasterKey)
        {
            Class1 ManageObj = new Class1();
            string passwordstr = refpwd;
            string Masterkey = refMasterKey;
            if (Masterkey.Length <= 0) Masterkey = "F5FD25E41C546E2098405A0F19E5502A"; //---Default Key
            string Securitykey = refSecuritykey;
            byte[] MasterKeyArr = new byte[16];
            byte[] SecuritykeyArr = new byte[16];
            byte[] Wraptext = new byte[24];
            string KeyCipher = string.Empty;
            List<byte> pwdbyte = new List<byte>();
            int countlen = 0;
            while (countlen < passwordstr.Length)
            {
                pwdbyte.Add(Convert.ToByte(passwordstr.Substring(countlen, 2), 16));
                countlen += 2;
            }
            int IndexLen = 0;
            int Enckcount = 0;
            while (IndexLen < Masterkey.Length)
            {
                MasterKeyArr[Enckcount++] = Convert.ToByte(Masterkey.Substring(IndexLen, 2), 16);
                IndexLen += 2;
            }
            IndexLen = 0;
            Enckcount = 0;
            while (IndexLen < Securitykey.Length)
            {
                SecuritykeyArr[Enckcount++] = Convert.ToByte(Securitykey.Substring(IndexLen, 2), 16);
                IndexLen += 2;
            }
            if (Securitykey.Trim().Length >=16)
            {
                ManageObj.p_securityAESWrap(MasterKeyArr, SecuritykeyArr, ref Wraptext);
                countlen = 0;
                pwdbyte = new List<byte>();
                pwdbyte.Add(0x02);
                pwdbyte.Add(0x02);
                pwdbyte.Add(0x16);
                pwdbyte.Add(0x00);
                pwdbyte.Add(0x09);
                pwdbyte.Add(0x18);
                while (countlen < Wraptext.Length)
                {
                    pwdbyte.Add(Wraptext[countlen++]);

                }

            }

            return pwdbyte;
        }

        public static List<List<string>> GetProfileData(byte[] Buffer, int actualDataStartIndex)
        {
            try
            {
                List<string> profileList = new List<string>();
                List<List<string>> profileData = new List<List<string>>();
               
                if (Buffer[actualDataStartIndex++] == 0x01)
                {
                    int recordsCount = Buffer[actualDataStartIndex];
                    if (Buffer[actualDataStartIndex] == 0x82) { recordsCount = ((byte)(Buffer[actualDataStartIndex + 1] & 0x1F) * 0x100 + (byte)(Buffer[actualDataStartIndex + 2])); actualDataStartIndex += 2; }
                    else if (Buffer[actualDataStartIndex] == 0x81) { recordsCount = ((byte)(Buffer[actualDataStartIndex + 1])); actualDataStartIndex += 1; }
                    else actualDataStartIndex++;
                    int recordsIndex = 0;
                    while (recordsIndex < recordsCount)
                    {
                        if (Buffer[actualDataStartIndex++] == 0x02)
                        {
                            int paraMetersCount = Buffer[actualDataStartIndex];
                            if (Buffer[actualDataStartIndex] == 0x82) { paraMetersCount = ((byte)(Buffer[actualDataStartIndex + 1] & 0x1F) * 0x100 + (byte)(Buffer[actualDataStartIndex + 2])); actualDataStartIndex += 2; }
                            else if (Buffer[actualDataStartIndex] == 0x81) { paraMetersCount = ((byte)(Buffer[actualDataStartIndex + 1])); actualDataStartIndex += 1; }
                            else actualDataStartIndex++;
                            int parameterIndex = 0;
                            profileList = new List<string>();
                            while (parameterIndex < paraMetersCount)
                            {
                                string[] datavalue = DLMSDataStracture.DLMSDataFormator(Buffer, actualDataStartIndex, false);
                                if (datavalue == null) { profileList.Add("");}// parseData += ","; }
                                { profileList.Add(datavalue[0]); }// { parseData += datavalue[0] + ","; }
                                actualDataStartIndex = Convert.ToInt32(datavalue[1]);
                                parameterIndex++;
                            }
                            profileData.Add(profileList);
                        }
                        recordsIndex++;
                    }
                }
                return profileData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static byte[] GetHexStringToByteList(string hexString)
        {
            if (hexString.Length < 2) return null;
            byte[] ByteList = new byte[hexString.Length / 2];
            int listIndex = 0;
            for (int i = 0; i < hexString.Length; i += 2)
            {
                ByteList[listIndex++] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return ByteList;
        }

        public static string GetByteToHexString(byte[] dataBytes)
        {
            string hexString = string.Empty;

            foreach (var item in dataBytes)
            {
                hexString += item.ToString("X2");
            }
            return hexString;
        }

        public static string GetASCIIStringToHex(string input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
                sb.AppendFormat("{0:X2}", (int)c);
            return sb.ToString().Trim();
        }
        //-----------------------------------------------END Of Functions--------------------------------------------------
    }
}
