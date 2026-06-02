using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemSecurityLibrary
{
  public class StandardDateTime
    {
        /// <summary>
        /// This method is used to convert the Date time to long integer.
        /// </summary>
        /// <param name="value">DateTime</param>
        /// <returns>long</returns>
        public long DateTimeToLong(DateTime value)
        {
            string number = string.Empty;
            number = string.Concat(number, ConvertToDigit(value.Year));
            number = string.Concat(number, ConvertToDigit(value.Month));
            number = string.Concat(number, ConvertToDigit(value.Day));
            number = string.Concat(number, ConvertToDigit(value.Hour));
            number = string.Concat(number, ConvertToDigit(value.Minute));
            number = string.Concat(number, ConvertToDigit(value.Second));
            return Convert.ToInt64(number);
        }

        public long DateTimeToLongWithoutSecond(DateTime value)
        {
            string number = string.Empty;

           if (value.Hour == 0 && value.Minute == 0)
            {
                value = value.AddDays(-1);
                number = string.Concat(number, ConvertToDigit(value.Year));
                number = string.Concat(number, ConvertToDigit(value.Month));
                number = string.Concat(number, ConvertToDigit(value.Day));
                number = string.Concat(number, "2400");
            }
            else
            {
                number = string.Concat(number, ConvertToDigit(value.Year));
                number = string.Concat(number, ConvertToDigit(value.Month));
                number = string.Concat(number, ConvertToDigit(value.Day));
                number = string.Concat(number, ConvertToDigit(value.Hour));
                number = string.Concat(number, ConvertToDigit(value.Minute));
            }
            return Convert.ToInt64(number);
        }
        public long DateTimeToLongWithoutSecondLS(DateTime value)
        {
            string number = string.Empty;             
            number = string.Concat(number, ConvertToDigit(value.Year));
            number = string.Concat(number, ConvertToDigit(value.Month));
            number = string.Concat(number, ConvertToDigit(value.Day));
            number = string.Concat(number, ConvertToDigit(value.Hour));
            number = string.Concat(number, ConvertToDigit(value.Minute));
            return Convert.ToInt64(number);
        }
      /// <summary>
        ///  This method is used to convert the Date time to long integer.
      /// </summary>
      /// <param name="value">Datetine as string</param>
      /// <returns>Long</returns>
        public long StringDateTimeToLong(string value)
        {
            string number = string.Empty;
            number = string.Concat(number, ConvertToDigit(Convert.ToInt32(value.Substring(0,4))));
            number = string.Concat(number, ConvertToDigit(Convert.ToInt32(value.Substring(4, 2))));
            number = string.Concat(number, ConvertToDigit(Convert.ToInt32(value.Substring(6, 2))));
            number = string.Concat(number, ConvertToDigit(Convert.ToInt32(value.Substring(8, 2))));
            number = string.Concat(number, ConvertToDigit(Convert.ToInt32(value.Substring(10, 2))));
            number = string.Concat(number, ConvertToDigit(Convert.ToInt32(value.Substring(12, 2))));
            return Convert.ToInt64(number);
        }
        public DateTime ConvertStringTodate(String strdate)
        {
            try
            {
                int Year = Convert.ToInt32(strdate.Substring(0, 4));
                int Month = Convert.ToInt32(strdate.Substring(4, 2));
                int Day = Convert.ToInt32(strdate.Substring(6, 2));
                int Hour = Convert.ToInt32(strdate.Substring(8, 2));
                int min = Convert.ToInt32(strdate.Substring(10, 2));
                if (Hour == 24) Hour = 0;
                return new DateTime(Year, Month, Day, Hour, min, 0);
            }
            catch (Exception)
            {

                return new DateTime();
            }

        }
        public long DateToLong(DateTime value)
        {
            string number = string.Empty;
            number = string.Concat(number, ConvertToDigit(value.Year));
            number = string.Concat(number, ConvertToDigit(value.Month));
            number = string.Concat(number, ConvertToDigit(value.Day));
            number = string.Concat(number, "00");
            number = string.Concat(number, "00");
            
            return Convert.ToInt64(number);
        }
        public long DateToLongwithSec(DateTime value)
        {
            string number = string.Empty;
            number = string.Concat(number, ConvertToDigit(value.Year));
            number = string.Concat(number, ConvertToDigit(value.Month));
            number = string.Concat(number, ConvertToDigit(value.Day));
            number = string.Concat(number, "00"); //hours
            number = string.Concat(number, "00");//Minutes
            number = string.Concat(number, "00");//Seconds
            return Convert.ToInt64(number);
        }
        public long DateToLongForMonth(DateTime value)
        {
            string number = string.Empty;
            number = string.Concat(number, ConvertToDigit(value.Year));
            number = string.Concat(number, ConvertToDigit(value.Month));
            number = string.Concat(number, "00");
            number = string.Concat(number, "00");
            number = string.Concat(number, "00");

            return Convert.ToInt64(number);
        }
        /// <summary>
        /// This method is used to convert the DateTime for start time(000000 hours) and last time(235959 hours)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startEndDateValue"></param>
        /// <returns></returns>
        public  long ConvertSearchDateTimeToLong(long date, string timeString)
        {
            DateTime value = LongToDateTime(date);
            string number = string.Empty;
            number = string.Concat(number, ConvertToDigit(value.Year));
            number = string.Concat(number, ConvertToDigit(value.Month));
            number = string.Concat(number, ConvertToDigit(value.Day));
            number = string.Concat(number, timeString);
            return Convert.ToInt64(number);
        }

        /// <summary>
        /// This method is used to convert the long integer value to datetime.
        /// </summary>
        /// <param name="value">long</param>
        /// <returns>DateTime</returns>
        public  DateTime LongToDateTime(long value)
        {
            try
            {
                bool is24hrs = false;
                string number = value.ToString();
                if (number.Length < 14)
                    number = number + "00";
                string year = number.Substring(0, 4);
                if (year.Trim().ToUpper().Equals("9999"))
                    year = "0000";

                string month = number.Substring(4, 2);
                if (month.Trim().ToUpper().Equals("99"))
                    month = "00";

                string day = number.Substring(6, 2);
                if (day.Trim().ToUpper().Equals("99"))
                    day = "00";


                string hour = number.Substring(8, 2);
                if (hour.Trim().ToUpper().Equals("99"))
                    hour = "00";


                string minute = number.Substring(10, 2);
                if (minute.Trim().ToUpper().Equals("99"))
                    minute = "00";
                string second = number.Substring(12, 2);
                if (second.Trim().ToUpper().Equals("99"))
                    second = "00";
                if (hour.Trim().ToUpper().Equals("24"))
                { hour = "00"; minute = "00"; second = "00"; is24hrs = true; }

                DateTime dateTime = new DateTime(
                 Convert.ToInt32(year),
                 Convert.ToInt32(month),
                 Convert.ToInt32(day),
                 Convert.ToInt32(hour),
                 Convert.ToInt32(minute),
                 Convert.ToInt32(second));
                if (is24hrs) return dateTime.AddDays(1);
                else return dateTime;
            }
            catch (Exception)
            {
               
                return new DateTime();;
            }
        }
        public DateTime LongDatetimeToSystemDateTime(string value)
        {
            try
            {

                string number = value.ToString();
                if (number.Length < 14)
                    number = number + "00";
                string year = number.Substring(0, 4);
                if (year.Trim().ToUpper().Equals("9999"))
                    year = "0000";

                string month = number.Substring(4, 2);
                if (month.Trim().ToUpper().Equals("99"))
                    month = "00";

                string day = number.Substring(6, 2);
                if (day.Trim().ToUpper().Equals("99"))
                    day = "00";


                string hour = number.Substring(8, 2);
                if (hour.Trim().ToUpper().Equals("99"))
                    hour = "00";

                string minute = number.Substring(10, 2);
                if (minute.Trim().ToUpper().Equals("99"))
                    minute = "00";

                string second = "00";
                if (number.Length >= 14)
                {
                    second = number.Substring(12, 2);
                    if (second.Trim().ToUpper().Equals("99"))
                        second = "00";
                }

                DateTime dateTime = new DateTime(
                 Convert.ToInt32(year),
                 Convert.ToInt32(month),
                 Convert.ToInt32(day),
                 Convert.ToInt32(hour),
                 Convert.ToInt32(minute),
                 Convert.ToInt32(second));
                return dateTime;
            }
            catch (Exception)
            {
                return new DateTime();
            }
        }
       
      public DateTime MeterDateTimeToSystemDateTime(string value)
        {
            try 
            {
                
            string number = value.ToString();
            if (number.Length < 14)
                number = number + "00";
            string year ="20" + number.Substring(6, 2);
            if (year.Trim().ToUpper().Equals("9999"))
                year = "0000";

            string month = number.Substring(3, 2);
            if (month.Trim().ToUpper().Equals("99"))
                month = "00";

            string day = number.Substring(0, 2);
            if (day.Trim().ToUpper().Equals("99"))
                day = "00";


            string hour = number.Substring(9, 2);
            if (hour.Trim().ToUpper().Equals("99"))
                hour = "00";

            string minute = number.Substring(12, 2);
            if (minute.Trim().ToUpper().Equals("99"))
                minute = "00";

            string second = "00";
            if (number.Length > 14)
            {
                number.Substring(15, 2);
                if (second.Trim().ToUpper().Equals("99"))
                    second = "00";
            }

            DateTime dateTime = new DateTime(
             Convert.ToInt32(year),
             Convert.ToInt32(month),
             Convert.ToInt32(day),
             Convert.ToInt32(hour),
             Convert.ToInt32(minute),
             Convert.ToInt32(second));
            return dateTime;
            }
            catch (Exception)
            {                 
                return new DateTime();
            }
        }

      public DateTime strSystemDateTimetoDateTime(string value)
      {
          try
          {

              string number = value.ToString();
              if (number.Length < 16)
                  number = number + "00";
              string year =  number.Substring(6, 4);
              if (year.Trim().ToUpper().Equals("9999"))
                  year = "0000";

              string month = number.Substring(3, 2);
              if (month.Trim().ToUpper().Equals("99"))
                  month = "00";

              string day = number.Substring(0, 2);
              if (day.Trim().ToUpper().Equals("99"))
                  day = "00";


              string hour = number.Substring(11, 2);
              if (hour.Trim().ToUpper().Equals("99"))
                  hour = "00";

              string minute = number.Substring(14, 2);
              if (minute.Trim().ToUpper().Equals("99"))
                  minute = "00";

              string second = "00";
             

              DateTime dateTime = new DateTime(
               Convert.ToInt32(year),
               Convert.ToInt32(month),
               Convert.ToInt32(day),
               Convert.ToInt32(hour),
               Convert.ToInt32(minute),
               Convert.ToInt32(second));
              return dateTime;
          }
          catch (Exception)
          {
              return new DateTime();
          }
      }
      /// <summary>
      /// get the diffrence of two dates in minute
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// 
      public long getDateTimeDiff(string dt1, string dt2)
      {
          try
          {
               
              DateTime currentSysDateTime1 = LongToDateTime(Convert.ToInt64(dt1));
              if (currentSysDateTime1 == new DateTime()) return -1;
              DateTime currentSysDateTime2 = LongToDateTime(Convert.ToInt64(dt2));
              if (currentSysDateTime2 == new DateTime()) return -1;
                          
              TimeSpan TS = new TimeSpan();
              TS = currentSysDateTime1 - currentSysDateTime2;
              long minutes =  (TS.Days * 24 * 60) + (TS.Hours * 60) + TS.Minutes ;

              return minutes;
          }
          catch (Exception)
          {
              return -1;
          }
      }

        public DateTime StringDateTimeToSystemDateTime(string value)
        {
            try
            {

                string number = value.ToString();
                if (number.Length < 16)
                    number = number + "00";
                string year = "20" + number.Substring(8, 2);
                if (year.Trim().ToUpper().Equals("9999"))
                    year = "0000";

                string month = number.Substring(3, 2);
                if (month.Trim().ToUpper().Equals("99"))
                    month = "00";

                string day = number.Substring(0, 2);
                if (day.Trim().ToUpper().Equals("99"))
                    day = "00";


                string hour = number.Substring(11, 2);
                if (hour.Trim().ToUpper().Equals("99"))
                    hour = "00";

                string minute = number.Substring(14, 2);
                if (minute.Trim().ToUpper().Equals("99"))
                    minute = "00";

                string second = "00";
                if (number.Length > 16)
                {
                    number.Substring(17, 2);
                    if (second.Trim().ToUpper().Equals("99"))
                        second = "00";
                }

                DateTime dateTime = new DateTime(
                 Convert.ToInt32(year),
                 Convert.ToInt32(month),
                 Convert.ToInt32(day),
                 Convert.ToInt32(hour),
                 Convert.ToInt32(minute),
                 Convert.ToInt32(second));
                return dateTime;
            }
            catch (Exception)
            {
                return new DateTime();
            }
        }
        public long AddDurationinDateTime(long value, int nDays, int nHrs, int nMin)
        {
            string number = value.ToString();
            if (number.Length < 12) return 200000000000;
            DateTime OccDate = LongToDateTime(value);
            OccDate = OccDate.AddDays(nDays);
            OccDate = OccDate.AddHours(nHrs);
            OccDate = OccDate.AddMinutes(nMin);
            long recValue = DateTimeToLongWithoutSecondLS(OccDate);
            return recValue;

        }             

        public string LongToStringDateFormat(string value)
        {
            string dmeter = "/";
            
               
            string number = value.ToString();
            string year1 = number.Substring(0, 4).Trim();
            string month1 = number.Substring(4, 2).Trim();
            string day1 = number.Substring(6, 2).Trim();
            year1 = (year1.ToString().Length == 2) ? string.Concat("20", year1.ToString()) : year1.ToString();
            month1 = (month1.ToString().Length == 1) ? string.Concat("0", month1.ToString()) : month1.ToString();
            day1 = (day1.ToString().Length == 1) ? string.Concat("0", day1.ToString()) : day1.ToString();
            return string.Concat(year1, dmeter, month1, dmeter, day1);
             
        }
        public string LongToStringDateTimeWithSecFormat(string value)
        {
            string data = "---------";
            if (value.Length<14)
                return data;
            string dmeter = "/";            
            string number = value.ToString();
            string year1 = number.Substring(0, 4).Trim();
            if (year1.Equals("1900"))
                return data;
            string month1 = number.Substring(4, 2).Trim();
            string day1 = number.Substring(6, 2).Trim();
            string hour = "0";
            string minute = "0";
            string second = "0";
            if (number.Length >= 10)
                hour = number.Substring(8, 2).Trim();
            if (number.Length >= 12)
                minute = number.Substring(10, 2).Trim();
            if (number.Length >= 14)
                second = number.Substring(12, 2).Trim();
            year1 = (year1.Length == 2) ? string.Concat("20", year1) : year1;
            month1 = (month1.Length == 1) ? string.Concat("0", month1) : month1;
            day1 = (day1.Length == 1) ? string.Concat("0", day1) : day1;
            hour = (hour.Length == 1) ? string.Concat("0", hour) : hour;
            minute = (minute.Length == 1) ? string.Concat("0", minute) : minute;
            second = (second.Length == 1) ? string.Concat("0", second) : second;
            if (month1 != "00" && day1 != "00")
            {
                //if (sequence[0].ToLower().Equals("dd") && sequence[1].ToLower().Equals("mm") && sequence[2].ToLower().Equals("yyyy"))
                    data = string.Concat(day1, dmeter, month1, dmeter, year1, " ", hour, ":", minute, ":", second);
               // if (sequence[0].ToLower().Equals("mm") && sequence[1].ToLower().Equals("dd") && sequence[2].ToLower().Equals("yyyy"))
                //    data = string.Concat(month1, dmeter, day1, dmeter, year1, " ", hour, ":", minute, ":", second);
            }
            return data;
        }
        public string LongToStringDateTimeFormat(long value)
        {
            string data = "---------";
            if (value.ToString().Length < 12)
                return "00/00/0000 00:00";
            string dmeter = "/";

            string number = value.ToString();
            string year1 = number.Substring(0, 4).Trim();
            if (year1.Equals("1900"))
                return data;
            string month1 = number.Substring(4, 2).Trim();
            string day1 = number.Substring(6, 2).Trim();
            string hour = "0";
            string minute = "0";
            string second = "0";
            if (number.Length >= 10)
                hour = number.Substring(8, 2).Trim();
            if (number.Length >= 12)
                minute = number.Substring(10, 2).Trim();
            if (number.Length >= 14)
                second = number.Substring(12, 2).Trim();
            year1 = (year1.Length == 2) ? string.Concat("20", year1) : year1;
            month1 = (month1.Length == 1) ? string.Concat("0", month1) : month1;
            day1 = (day1.Length == 1) ? string.Concat("0", day1) : day1;
            hour = (hour.Length == 1) ? string.Concat("0", hour) : hour;
            minute = (minute.Length == 1) ? string.Concat("0", minute) : minute;
            //second = (second.Length == 1) ? string.Concat("0", second) : second;
            if (month1 != "00" && day1 != "00")
            {
                //if (sequence[0].ToLower().Equals("dd") && sequence[1].ToLower().Equals("mm") && sequence[2].ToLower().Equals("yyyy"))
                data = string.Concat(day1, dmeter, month1, dmeter, year1, " ", hour, ":", minute);
                // if (sequence[0].ToLower().Equals("mm") && sequence[1].ToLower().Equals("dd") && sequence[2].ToLower().Equals("yyyy"))
                //    data = string.Concat(month1, dmeter, day1, dmeter, year1, " ", hour, ":", minute, ":", second);
            }
            return data;
        }
        public string LongToStringDateFormat(long value)
        {
            string data = "---------";
            if (value.ToString().Length < 12)
                return "00/00/0000";
            string dmeter;            
            dmeter = "/";
            string number = value.ToString();
            string year1 = number.Substring(0, 4).Trim();
            if (year1.Equals("1900"))
                return data;
            string month1 = number.Substring(4, 2).Trim();
            string day1 = number.Substring(6, 2).Trim();          
            year1 = (year1.Length == 2) ? string.Concat("20", year1) : year1;
            month1 = (month1.Length == 1) ? string.Concat("0", month1) : month1;
            day1 = (day1.Length == 1) ? string.Concat("0", day1) : day1;
            if (month1 != "00" && day1 != "00") data = string.Concat(day1, dmeter, month1, dmeter, year1);
            return data;
        }
        public string LongToStringTimeFormat(long value)
        {
            string data = "---------";
            if (value.ToString().Length < 12)
                return "00:00";
            string number = value.ToString();           
            string hour = "0";
            string minute = "0";           
            if (number.Length >= 10)
                hour = number.Substring(8, 2).Trim();
            if (number.Length >= 12)
                minute = number.Substring(10, 2).Trim();         
            hour = (hour.Length == 1) ? string.Concat("0", hour) : hour;
            minute = (minute.Length == 1) ? string.Concat("0", minute) : minute;
           data = string.Concat(hour, ":", minute);             
            return data;
        }
        public string LongToMeterDateTimeFormat(string value)
        {
            string data = "---------";
            if (value.Length < 12)
                return "00-00-00    00:00";
            string dmeter;
            dmeter = "-";

            string number = value;
            string year1 = number.Substring(2, 2).Trim();
            if (year1.Equals("1900"))
                return data;
            string month1 = number.Substring(4, 2).Trim();
            string day1 = number.Substring(6, 2).Trim();
            string hour = "0";
            string minute = "0";
            string second = "0";
            if (number.Length >= 10)
                hour = number.Substring(8, 2).Trim();
            if (number.Length >= 12)
                minute = number.Substring(10, 2).Trim();
            if (number.Length >= 14)
                second = number.Substring(12, 2).Trim();
            year1 = (year1.Length == 1) ? string.Concat("0", year1) : year1;
            month1 = (month1.Length == 1) ? string.Concat("0", month1) : month1;
            day1 = (day1.Length == 1) ? string.Concat("0", day1) : day1;
            hour = (hour.Length == 1) ? string.Concat("0", hour) : hour;
            minute = (minute.Length == 1) ? string.Concat("0", minute) : minute;
        
            if (month1 != "00" && day1 != "00")
            {
                data = string.Concat(day1, dmeter, month1, dmeter, year1, "    ", hour, ":", minute);
            }
            return data;
        }
        public long StringToLongDateTimeFormat(string value)
        {
            long lng = new long();
            string data = "0";
            if (value.Length <19)
                return 0;
            string year1 = value.Substring(6, 4).Trim();
            string month1 = value.Substring(3, 2).Trim();
            string day1 = value.Substring(0, 2).Trim();
            string hour = "0";
            string minute = "0";
            string second = "0";
            if (value.Length>= 11)
                hour = value.Substring(11, 2).Trim();
            if (value.Length>= 16)
                minute = value.Substring(14, 2).Trim();
            if (value.Length >=19)
                second = value.Substring(17, 2).Trim();
            year1 = (year1.Length == 2) ? string.Concat("20", year1) : year1;
            month1 = (month1.Length == 1) ? string.Concat("0", month1) : month1;
            day1 = (day1.Length == 1) ? string.Concat("0", day1) : day1;
            hour = (hour.Length == 1) ? string.Concat("0", hour) : hour;
            minute = (minute.Length == 1) ? string.Concat("0", minute) : minute;
            second = (second.Length == 1) ? string.Concat("0", second) : second;
            data = string.Concat(year1, month1, day1, hour, minute, second);           
            bool isLong = long.TryParse(data, out lng);
            if (isLong) return Convert.ToInt64(data);
            else return 0;
        }
        public long StringToLongDateTimeFormatForTamper(string value)
        {
            long lng = new long();
            value = value.Replace(" ", "");
            string data = "0";
            if (value.Length < 13)
                return 0;
            string year1 = value.Substring(6, 2).Trim();
            string month1 = value.Substring(3, 2).Trim();
            string day1 = value.Substring(0, 2).Trim();
            string hour = "0";
            string minute = "0";           
            if (value.Length >= 8)
                hour = value.Substring(8, 2).Trim();
            if (value.Length >= 11)
                minute = value.Substring(11, 2).Trim();        
                
            year1 = (year1.Length == 2) ? string.Concat("20", year1) : year1;
            month1 = (month1.Length == 1) ? string.Concat("0", month1) : month1;
            day1 = (day1.Length == 1) ? string.Concat("0", day1) : day1;
            hour = (hour.Length == 1) ? string.Concat("0", hour) : hour;
            minute = (minute.Length == 1) ? string.Concat("0", minute) : minute;
            
            data = string.Concat(year1, month1, day1, hour, minute);
            bool isLong = long.TryParse(data, out lng);
            if (isLong) return Convert.ToInt64(data);
            else return 0;
        }
        public static long DateToLong(string value, bool IsStartTime)
        {
            string startTime = "";
            if (IsStartTime)
                startTime = "000000";
            else
                startTime = "235959";
            if (value.IndexOf('-') > 0)
                value = value.Replace('-', '/');
            string[] number = value.Split('/');
            string longDate = "";
            string formate = "";// ConfigInfo.DateFormat().ToUpper();
            if (formate.IndexOf('-') > 0)
                formate = formate.Replace('-', '/');
            if (formate.Equals("MM/DD/YYYY"))
                longDate = string.Concat(number[2], number[0], number[1], startTime);
            else
                longDate = string.Concat(number[2], number[1], number[0], startTime);
            return Convert.ToInt64(longDate);
        }
        public string MeterdateToSystemDate(string value)
        {
            if (value == "----") return value;
            if (value.IndexOf('-') > 0)
                value = value.Replace('-', '/');
            if (value.Length <= 14) value = value.Insert(value.LastIndexOf("/")+1, "20");
                return value;
        }

        /// <summary>
        /// This method is used to convert the single digit number to double digit.
        /// </summary>
        /// <param name="value">int</param>
        /// <returnsstring></returns>
        private static string ConvertToDigit(int value)
        {
            string number = string.Empty;
            if (value <= 9)
                number = string.Concat("0", value.ToString());
            else
                number = value.ToString();
            return number;
        }

        public static DateTime ConvertIntToDate(int day, int month, int year)
        {
            DateTime dt = new DateTime(year, month, day);
            return dt;
        }

    }
}
