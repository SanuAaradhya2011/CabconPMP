using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace COMMONENTITY
{
   public class CommonMethods
    {
       decimal objdec;
       
       public string GetTravelerAddress(int TravelerType)
       {            
           string StartAddress = string.Empty;
           try
           {

               switch (TravelerType)
               {
                   case (int)StaticVariables.ProduTraveler.EMSTest:
                       StartAddress = StaticVariables.Traveler_EMS_Add;
                       break;
                   case (int)StaticVariables.ProduTraveler.FunctionTest:
                       StartAddress = StaticVariables.Traveler_FT_Add;
                       break;
                   case (int)StaticVariables.ProduTraveler.CalibrationTest:
                       StartAddress = StaticVariables.Traveler_CALI_Add;
                       break;
                   case (int)StaticVariables.ProduTraveler.SirializationTest:
                       StartAddress = StaticVariables.Traveler_SERIAL_Add;
                       break;
               }
           }
           catch (Exception)
           {
               return "";
           }
           return StartAddress;
       }

       public string GetTravelerAddress_3Phase(int TravelerType)
       {
           
           string StartAddress = string.Empty;
           try
           {

               switch (TravelerType)
               {
                   case (int)StaticVariables.ProduTraveler.EMSTest:
                       StartAddress = StaticVariables.Traveler_EMS_3PhAdd;
                       break;
                   case (int)StaticVariables.ProduTraveler.FunctionTest:
                       StartAddress = StaticVariables.Traveler_FT_3PhAdd;
                       break;
                   case (int)StaticVariables.ProduTraveler.CalibrationTest:
                       StartAddress = StaticVariables.Traveler_CALI_3PhAdd;
                       break;
                   case (int)StaticVariables.ProduTraveler.SirializationTest:
                       StartAddress = StaticVariables.Traveler_SERIAL_3PhAdd;
                       break;
               }
           }
           catch (Exception)
           {
               return "";
           }
           return StartAddress;
       }

       public byte GetTravelerBytes(int TravelerType)
       {
           GlobalMethods objstvar = new GlobalMethods();
           byte byteLen = 0x08;
           try
           {

               switch (TravelerType)
               {
                   case (int)StaticVariables.ProduTraveler.EMSTest:
                       byteLen = 0x20;
                       break;
                   case (int)StaticVariables.ProduTraveler.FunctionTest:
                       byteLen = 0x18;
                       break;
                   case (int)StaticVariables.ProduTraveler.CalibrationTest:
                       byteLen = 0x10;
                       break;
                   case (int)StaticVariables.ProduTraveler.SirializationTest:
                       byteLen = 0x08;
                       break;
               }
           }
           catch (Exception)
           {
               return 0x08;
           }
           return byteLen;
       }

       public string HexToDecimalConversion(string strHex)
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

       public bool isValidReadParameters(string defValue, string minVal, string maxVal, decimal meterValue)
       {
           bool isValiddata = true;
           if (defValue != "")
           {
               if (defValue != meterValue.ToString()) isValiddata = false;
           }
           else
           {
               if (minVal != "" && maxVal != "")
               {
                   if (meterValue < Convert.ToDecimal(minVal) || meterValue > Convert.ToDecimal(maxVal)) isValiddata = false;
               }
               else if (minVal != "")
               {
                   if (meterValue < Convert.ToDecimal(minVal)) isValiddata = false;
               }
               else if (maxVal != "")
               {
                   if (meterValue > Convert.ToDecimal(maxVal)) isValiddata = false;
               }
           }
           return isValiddata;

       }

     
       public string CheckingRangeValueForDecimal(string msgValue, string defaultVal, string minVal, string maxVal, string meterValue)
       {
           try
           {
               decimal RefMinValue = 0;
               decimal RefMaxValue = 0;
               bool isValiddata = true;
               if (minVal != "") RefMinValue = Convert.ToDecimal(minVal);
               if (maxVal != "") RefMaxValue = Convert.ToDecimal(maxVal);
               //------------------If Default Comparision set then No Min or Max Done --------------------------------------------
               if (defaultVal != "")
               {
                   if (decimal.TryParse(defaultVal, out objdec))
                   {
                       if ((Convert.ToDecimal(defaultVal)).ToString("0.00") != (Convert.ToDecimal(meterValue)).ToString("0.00")) isValiddata = false;
                   }
                   else
                   {
                       if (defaultVal != meterValue) isValiddata = false;
                   }
               }
               else
               {
                   decimal meterFormattedValue = 0;
                   if (decimal.TryParse(meterValue, out objdec))
                   {
                       meterFormattedValue = Convert.ToDecimal(meterValue);
                   }
                   if (minVal != "" && maxVal != "")
                   {
                       if (meterFormattedValue < RefMinValue || meterFormattedValue > RefMaxValue) isValiddata = false;
                   }
                   else if (minVal != "")
                   {
                       if (meterFormattedValue < RefMinValue) isValiddata = false;
                   }
                   else if (maxVal != "")
                   {
                       if (meterFormattedValue > RefMaxValue) isValiddata = false;
                   }
               }
               string MemoryStstFlg = msgValue + meterValue;
               if (!isValiddata) MemoryStstFlg =StaticVariables.ERRORPreFix + MemoryStstFlg;
               return MemoryStstFlg;
           }
           catch (Exception ex)
           {
               return StaticVariables.ERRORPreFix + ex.Message;
           }
       }

       public string GetData(string data)
       {
           try
           {
               string result = "";
               string binDataValue = Convert.ToString(Convert.ToInt32(data, 16), 2);
               while (binDataValue.Length < 16) { binDataValue = "0" + binDataValue; }
               if (binDataValue.Substring(0, 1) == "1")
               {
                   int decData = Convert.ToInt32(data, 16);
                   decData = 0 - decData;
                   result = decData.ToString("X").Substring(decData.ToString("X").Length - 4, 4);
                   result = "-" + Convert.ToInt32(result, 16).ToString();
               }
               else
               {
                   result = Convert.ToInt32(data, 16).ToString(); ;
               }
               return result;
           }
           catch (Exception)
           {
               return "";
           }
       }

       public string ReverseString(string response, int format)
       {
           try
           {
           int count = 0;
           string revResponse = "";

           for (count = response.Length - 2; count >= 0; count -= 2)
           {
               revResponse += response.Substring(count, 2);
           }
           //return revResponse.TrimStart('0');
           if (format == 0)
           { revResponse = Convert.ToInt32(revResponse, 16).ToString(); }
           return revResponse;
           }
           catch (Exception)
           {
               return "";
           }
       }

       public string ASCIIToHexForSerial(string GetStr)
       {
           try
           {
               int indecount = 0;
               char AsciiCh;
               int chrascii;
               string strgetval = "";
               string tempstr = "";
               char[] _hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
               while (indecount < GetStr.Length)
               {
                   AsciiCh = Convert.ToChar(GetStr.Substring(indecount, 1));
                   if ((AsciiCh >= 48) && AsciiCh <= 57)
                   {

                       chrascii = (Convert.ToInt16(AsciiCh) - 48) + 30;
                       tempstr += chrascii.ToString();

                   }
                   else
                   {

                       if (AsciiCh != 32)
                       {
                           chrascii = Convert.ToInt16(AsciiCh);
                           AsciiCh = _hexChars[chrascii / 16];
                           strgetval = (_hexChars[chrascii / 16]).ToString() + (_hexChars[chrascii % 16]).ToString();
                           tempstr += (strgetval);
                       }
                       else
                       {
                           tempstr += "20";       //Space
                       }
                   }
                   indecount++;

               }
               return tempstr;
           }
           catch (Exception)
           {
               return "";
           }
       }

       public string GetManufacturingDataWriteCommand(string defaultVal, string minVal, string maxVal, int meterType)
       {
           int objint;
           string meterDetailsCommand = meterType.ToString("X2");
           if (int.TryParse(minVal, out objint)) meterDetailsCommand = meterDetailsCommand + Convert.ToInt32(minVal).ToString("X2");
           if (int.TryParse(maxVal, out objint)) meterDetailsCommand = meterDetailsCommand + Convert.ToInt32(maxVal).ToString("X2");
           if (int.TryParse(defaultVal, out objint)) meterDetailsCommand = meterDetailsCommand + Convert.ToInt32(defaultVal).ToString("X2");
           return meterDetailsCommand += SetTimestamp();

       }

       private string SetTimestamp()
       {
           string data = string.Format("{0:ddMMyyHHmmss}", System.DateTime.Now);
           string getYear = Convert.ToInt32(data.Substring(4, 2)).ToString("X2");


           string getMonth = Convert.ToString(Convert.ToInt32(data.Substring(2, 2), 10), 2);
           while (getMonth.Length < 8) { getMonth = "0" + getMonth; }
           getMonth = "0" + getMonth.Substring(1);
           getMonth = Convert.ToInt32(getMonth, 2).ToString("X2");
           getMonth = getMonth + getYear;


           string getDate = Convert.ToString(Convert.ToInt32(data.Substring(0, 2), 10), 2);
           while (getDate.Length < 8) { getDate = "0" + getDate; }
           getDate = getDate.Substring(3);

           string gethrs = Convert.ToString(Convert.ToInt32(data.Substring(6, 2), 10), 2);
           while (gethrs.Length < 8) { gethrs = "0" + gethrs; }
           gethrs = gethrs.Substring(3);

           string getmin = Convert.ToString(Convert.ToInt32(data.Substring(8, 2), 10), 2);
           while (getmin.Length < 8) { getmin = "0" + getmin; }
           getmin = getmin.Substring(2);

           string tmpdttime = getDate + gethrs + getmin;
           string dt1 = Convert.ToInt32(tmpdttime, 2).ToString("X4");

           string getDateTime = getMonth + dt1.Substring(2, 2) + dt1.Substring(0, 2) + "00"; //------00 as dummy byte
           return getDateTime;

       }

       public DateTime ConvertIECMeterResponseTodate(String strdate)
       {
           try
           {
               int Year = Convert.ToInt32(strdate.Substring(0, 2)) + 2000;
               int Month = Convert.ToInt32(strdate.Substring(2, 2));
               int Day = Convert.ToInt32(strdate.Substring(4, 2));
               int Hour = Convert.ToInt32(strdate.Substring(6, 2));
               int min = Convert.ToInt32(strdate.Substring(8, 2));
               int sec = Convert.ToInt32(strdate.Substring(10, 2));
               if (Hour == 24) Hour = 0;
               return new DateTime(Year, Month, Day, Hour, min, sec);
           }
           catch (Exception)
           {

               return new DateTime();
           }

       }

       public DateTime ConvertIECMeterResponseTodateZCE(String strdate)
       {
           try
           {
               int Year = Convert.ToInt32(strdate.Substring(10, 2)) + 2000;
               int Month = Convert.ToInt32(strdate.Substring(8, 2));
               int Day = Convert.ToInt32(strdate.Substring(6, 2));
               int Hour = Convert.ToInt32(strdate.Substring(4, 2));
               int min = Convert.ToInt32(strdate.Substring(2, 2));
               int sec = Convert.ToInt32(strdate.Substring(0, 2));
               if (Hour == 24) Hour = 0;
               return new DateTime(Year, Month, Day, Hour, min, sec);
           }
           catch (Exception)
           {

               return new DateTime();
           }

       }

       public static void LogPMPMessage(string logMessage)
       {
           try
           {
               string sLogStampFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
               using (StreamWriter w = File.AppendText("PMPErrorlog.txt"))
               {                   
                   w.WriteLine("  :{0}", sLogStampFormat + logMessage);
                   w.WriteLine("-------------------------------");
               }
           }
           catch (Exception)
           { 

           }


       }
    }
}
