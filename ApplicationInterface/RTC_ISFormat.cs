using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationInterface;
using Utilities;
namespace ApplicationInterface
{
    public class RTC_ISFormat
    {
        public byte yearhighbyte { get; set; }
        public byte yearlowbyte { get; set; }
        public byte month { get; set; }
        public byte dayofmonth { get; set; }
        public byte dayofweek { get; set; }
        public byte hour { get; set; }
        public byte minute { get; set; }
        public byte second { get; set; }
        public byte Hundreds { get; set; }
        public byte deviationhighbyte { get; set; }
        public byte deviationlowbyte { get; set; }
        public byte clockstatus { get; set; }
    }

    public class GenericRTC
    {
       
        public static RTC_ISFormat RTCWRITE(int metermodel, int yearval, int monthVal, int DayVal)
        {
            RTC_ISFormat rtc = new RTC_ISFormat() {dayofweek =0xFF, clockstatus = 0x00 };

            if (metermodel == (int)LayerInterface.MeterTypeInfo.SAPPHIRE_S2 || metermodel == (int)LayerInterface.MeterTypeInfo.MicroStar_DLMS || metermodel == (int)LayerInterface.MeterTypeInfo.Smart_Meter_1PH || metermodel == (int)LayerInterface.MeterTypeInfo.Smart_Meter_3PH)
            {
                rtc.dayofweek = GetDayOfWeek(yearval, monthVal, DayVal);
            }

            if (metermodel == (int)LayerInterface.MeterTypeInfo.Smart_Meter_1PH || metermodel == (int)LayerInterface.MeterTypeInfo.Smart_Meter_3PH || metermodel == (int)LayerInterface.MeterTypeInfo.SAPPHIRE_S2)
            {
                rtc.clockstatus = 0xFF;
            }
            return rtc;
        }

        public static RTC_ISFormat ClockWRITEBytes(int metermodel, int yearval, int monthVal, int DayVal, int HrsVal, int MinVal)
        {
            RTC_ISFormat rtc = new RTC_ISFormat() {dayofweek=0xFF, hour = 0xFF, minute = 0xFF, second = 0xFF, Hundreds = 0xFF, clockstatus = 0x00 };
            switch (metermodel)
            {
                case (int)LayerInterface.MeterTypeInfo.SAPPHIRE_S2:
                case (int)LayerInterface.MeterTypeInfo.SAPPHIRE:
                case (int)LayerInterface.MeterTypeInfo.MicroStar_DLMS:
                case (int)LayerInterface.MeterTypeInfo.Smart_Meter_1PH:
                case (int)LayerInterface.MeterTypeInfo.Smart_Meter_3PH:
                    {
                        if (IsRTCAsPerIS15959A5())
                        {
                            rtc.dayofweek = GetDayOfWeek(yearval, monthVal, DayVal);
                            rtc.hour = HrsVal < 24 ? (byte)HrsVal : (byte)00; //To be valid Hrs & Min for IS15959 clock format supported meters with default DIP, incase user not selected the valid value
                            rtc.minute = MinVal < 60 ? (byte)MinVal : (byte)30;
                            rtc.second = 0x00;
                            rtc.Hundreds = 0x00;
                            rtc.clockstatus = 0xFF;
                        }
                        break;
                    }
            }

            //if (metermodel == (int)LayerInterface.MeterTypeInfo.SAPPHIRE_S2 || (metermodel == (int)LayerInterface.MeterTypeInfo.SAPPHIRE && IsRTCAsPerIS15959A5()) ||  (metermodel == (int)LayerInterface.MeterTypeInfo.MicroStar_DLMS && IsRTCAsPerIS15959A5()))
            //{
            //    rtc.dayofweek = GetDayOfWeek(yearval, monthVal, DayVal);
            //    rtc.hour = HrsVal < 24 ? (byte)HrsVal : (byte)00; //To be valid Hrs & Min for IS15959 clock format supported meters with default DIP, incase user not selected the valid value
            //    rtc.minute = MinVal < 60 ? (byte)MinVal : (byte)30;
            //    rtc.second = 0x00;
            //    rtc.Hundreds = 0x00;
            //    rtc.clockstatus = 0xFF;
            //}
            return rtc;
        }
        public static Byte GetDayOfWeek(int yearval, int monthVal, int DayVal)
        {
            DateTime setdatetime = new DateTime(yearval, monthVal, DayVal);
            if (setdatetime.DayOfWeek == DayOfWeek.Sunday) return 0x07;
            else return Convert.ToByte(setdatetime.DayOfWeek);
        }
        public static RTC_ISFormat SEASONPROFILEWRITE(int metermodel)
        {
            RTC_ISFormat rtc = new RTC_ISFormat() { clockstatus = 0x00 };
            switch (metermodel)
            {
                case (int)LayerInterface.MeterTypeInfo.SAPPHIRE_S2:
                case (int)LayerInterface.MeterTypeInfo.SAPPHIRE:
                case (int)LayerInterface.MeterTypeInfo.MicroStar_DLMS:
                case (int)LayerInterface.MeterTypeInfo.Smart_Meter_1PH:
                case (int)LayerInterface.MeterTypeInfo.Smart_Meter_3PH:
                    {
                        if (IsRTCAsPerIS15959A5())
                        {
                            rtc.clockstatus = 0xFF;
                        }
                    
                    break;
                    }
            }
            //if (metermodel == (int)LayerInterface.MeterTypeInfo.SAPPHIRE_S2 || (metermodel == (int)LayerInterface.MeterTypeInfo.SAPPHIRE && IsRTCAsPerIS15959A5()) || (metermodel == (int)LayerInterface.MeterTypeInfo.MicroStar_DLMS && IsRTCAsPerIS15959A5()))
            //{
            //    rtc.clockstatus = 0xFF;
            //}

            return rtc;
        }

        public static RTC_ISFormat DAYPROFILEWRITE(int metermodel)
        {
            RTC_ISFormat rtc = new RTC_ISFormat() { second = 0x00, Hundreds = 0x00 };

            //if (metermodel == (int)LayerInterface.MeterTypeInfo.SAPPHIRE_S2)// value must be 0x00 as per IS15959 A5 changes, so no need to implement check here
            //{
            //    rtc.second = 0x00;
            //    rtc.Hundreds = 0x00;
            //}

            return rtc;
        }
        private static bool IsRTCAsPerIS15959A5()
        {
            try 
	        {
                SystemSettingsAccess objSysSettings = new SystemSettingsAccess();
                List<string> RTCasPerA5 = new List<string> { "W0","L0"};
                foreach (var item in RTCasPerA5)
                {
                    if (LayerInterface.MeterSignature.Contains(item)) return true;
                }
                if (objSysSettings.IS15959PART1Amendment5Clock) return true;
        		return true;//false
	        }
	        catch (Exception)
	        {
        		
		       return false;
	        }
        }


    }
}
