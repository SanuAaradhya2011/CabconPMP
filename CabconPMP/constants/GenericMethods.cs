using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntegratedCalibration.Constants
{
    public class GenericMethods
    {
        
        public static string GetByteToHexString(byte[] dataBytes)
        {
            string hexString = string.Empty;

            foreach (var item in dataBytes)
            {
                hexString += item.ToString("X2");
            }
            return hexString;
        }

        public static List<byte> ConvertStrToByte(string srddata)
        {
            List<byte> myretlist = new List<byte>();
            srddata = srddata.Replace("\r", "");
            for (int i = 0; i < srddata.Length;)
            {
                byte bval = byte.Parse(srddata.Substring(i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                i++; i++;
                myretlist.Add(bval);
            }
            return myretlist;
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
                    return sb.ToString();
            }
        }

    }
}
