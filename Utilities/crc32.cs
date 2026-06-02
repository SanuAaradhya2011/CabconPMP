using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Xml.XPath;
namespace Utilities
{
    public class crc32
    {
        static uint[] table;

        ushort[] table16 = {
        //  Crc table for poly: $1021 using reversed calculations
        0x0000,0x1189,0x2312,0x329B,
        0x4624,0x57AD,0x6536,0x74BF,
        0x8C48,0x9DC1,0xAF5A,0xBED3,
        0xCA6C,0xDBE5,0xE97E,0xF8F7,
        0x1081,0x0108,0x3393,0x221A,
        0x56A5,0x472C,0x75B7,0x643E,
        0x9CC9,0x8D40,0xBFDB,0xAE52,
        0xDAED,0xCB64,0xF9FF,0xE876,
        0x2102,0x308B,0x0210,0x1399,
        0x6726,0x76AF,0x4434,0x55BD,
        0xAD4A,0xBCC3,0x8E58,0x9FD1,
        0xEB6E,0xFAE7,0xC87C,0xD9F5,
        0x3183,0x200A,0x1291,0x0318,
        0x77A7,0x662E,0x54B5,0x453C,
        0xBDCB,0xAC42,0x9ED9,0x8F50,
        0xFBEF,0xEA66,0xD8FD,0xC974,
        0x4204,0x538D,0x6116,0x709F,
        0x0420,0x15A9,0x2732,0x36BB,
        0xCE4C,0xDFC5,0xED5E,0xFCD7,
        0x8868,0x99E1,0xAB7A,0xBAF3,
        0x5285,0x430C,0x7197,0x601E,
        0x14A1,0x0528,0x37B3,0x263A,
        0xDECD,0xCF44,0xFDDF,0xEC56,
        0x98E9,0x8960,0xBBFB,0xAA72,
        0x6306,0x728F,0x4014,0x519D,
        0x2522,0x34AB,0x0630,0x17B9,
        0xEF4E,0xFEC7,0xCC5C,0xDDD5,
        0xA96A,0xB8E3,0x8A78,0x9BF1,
        0x7387,0x620E,0x5095,0x411C,
        0x35A3,0x242A,0x16B1,0x0738,
        0xFFCF,0xEE46,0xDCDD,0xCD54,
        0xB9EB,0xA862,0x9AF9,0x8B70,
        0x8408,0x9581,0xA71A,0xB693,
        0xC22C,0xD3A5,0xE13E,0xF0B7,
        0x0840,0x19C9,0x2B52,0x3ADB,
        0x4E64,0x5FED,0x6D76,0x7CFF,
        0x9489,0x8500,0xB79B,0xA612,
        0xD2AD,0xC324,0xF1BF,0xE036,
        0x18C1,0x0948,0x3BD3,0x2A5A,
        0x5EE5,0x4F6C,0x7DF7,0x6C7E,
        0xA50A,0xB483,0x8618,0x9791,
        0xE32E,0xF2A7,0xC03C,0xD1B5,
        0x2942,0x38CB,0x0A50,0x1BD9,
        0x6F66,0x7EEF,0x4C74,0x5DFD,
        0xB58B,0xA402,0x9699,0x8710,
        0xF3AF,0xE226,0xD0BD,0xC134,
        0x39C3,0x284A,0x1AD1,0x0B58,
        0x7FE7,0x6E6E,0x5CF5,0x4D7C,
        0xC60C,0xD785,0xE51E,0xF497,
        0x8028,0x91A1,0xA33A,0xB2B3,
        0x4A44,0x5BCD,0x6956,0x78DF,
        0x0C60,0x1DE9,0x2F72,0x3EFB,
        0xD68D,0xC704,0xF59F,0xE416,
        0x90A9,0x8120,0xB3BB,0xA232,
        0x5AC5,0x4B4C,0x79D7,0x685E,
        0x1CE1,0x0D68,0x3FF3,0x2E7A,
        0xE70E,0xF687,0xC41C,0xD595,
        0xA12A,0xB0A3,0x8238,0x93B1,
        0x6B46,0x7ACF,0x4854,0x59DD,
        0x2D62,0x3CEB,0x0E70,0x1FF9,
        0xF78F,0xE606,0xD49D,0xC514,
        0xB1AB,0xA022,0x92B9,0x8330,
        0x7BC7,0x6A4E,0x58D5,0x495C,
        0x3DE3,0x2C6A,0x1EF1,0x0F78
        };

        public ushort ComputeChecksum(byte[] bytes)
        {
            ushort crc = 0xffff;
            for (int i = 0; i < bytes.Length - 2; ++i)
            {
                byte index = (byte)(crc ^ bytes[i]);
                crc = (ushort)((crc >> 8) ^ table[index]);
            }
            crc = (ushort)~crc;
            return crc;
        }

        public byte[] ComputeChecksumBytes(byte[] bytes)
        {
            ushort crc = ComputeChecksum(bytes);
            return BitConverter.GetBytes(crc);
        }

        public static void filltable()
        {
            uint poly = 0xedb88320;
            table = new uint[256];
            uint temp = 0;
            for (uint i = 0; i < table.Length; ++i)
            {
                temp = i;
                for (int j = 8; j > 0; --j)
                {
                    if ((temp & 1) == 1)
                    {
                        temp = (uint)((temp >> 1) ^ poly);
                    }
                    else
                    {
                        temp >>= 1;
                    }
                }
                table[i] = temp;
            }
        }

        public static uint ComputeCRC(byte[] bytes)
        {
            filltable();

            uint crc = 0xffffffff;

            for (int i = 0; i < bytes.Length; ++i)
            {
                byte index = (byte)(((crc) & 0xff) ^ bytes[i]);
                crc = (uint)((crc >> 8) ^ table[index]);
            }

            return ~crc;
        }
     
       

        /// <summary>
        /// ComputeStepCRC
        /// this function is used to calculate crc32 of each packet for three phase
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="crc"></param>
        /// <returns></returns>
        public static uint ComputeStepCRC(byte[] bytes, uint crc)
        {
            if (crc != 0xffffffff)
                crc = ~crc;

            for (int i = 0; i < bytes.Length; ++i)
            {
                byte index = (byte)(((crc) & 0xff) ^ bytes[i]);
                crc = (uint)((crc >> 8) ^ table[index]);
            }
            return ~crc;
        }

       
    }

    public static class Crc8
    {
        static byte[] table = new byte[256];
        // x8 + x7 + x6 + x4 + x2 + 1
        const byte poly = 0x18;

        public static byte ComputeChecksum(params byte[] bytes)
        {
            byte crc = 0xaa;

            if (bytes != null && bytes.Length > 0)
            {
                foreach (byte b in bytes)
                {
                    byte mubyte = b;

                    for (int i = 0; i < 8; i++)
                    {
                        byte bval = (byte)((mubyte ^ crc) & 0x01);

                        if (bval > 0)
                        {
                            /* Shift and subtract poly */
                            crc = (byte)(((crc ^ poly) >> 1) | 0x80);
                        }
                        else
                        {
                            /* Transparent shift */
                            crc >>= 1;
                        }

                        mubyte = (byte)(mubyte >> 1);

                    }

                    //crc = table[crc ^ b];
                }
            }
            return crc;
        }

        static Crc8()
        {
            for (int i = 0; i < 256; ++i)
            {
                int temp = i;
                for (int j = 0; j < 8; ++j)
                {
                    if ((temp & 0x80) != 0)
                    {
                        temp = (temp << 1) ^ poly;
                    }
                    else
                    {
                        temp <<= 1;
                    }
                }
                table[i] = (byte)temp;
            }
        }
    }

    public class CRCGenerator
    {
         
        /// <summary>
        /// Computes the Checksum for XML string after removal of Optional XML nodes.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        /// 

        public static string ComputeChecksumFromXml(string xml)
        {
            MemoryStream filestream = new MemoryStream();
            XmlSerializer xsSubmit = new XmlSerializer(typeof(ModifyMeterProgramIndiaAMI));
            StringWriter sww = new StringWriter();
            XmlWriter writer = XmlWriter.Create(sww);
            ModifyMeterProgramIndiaAMI mpr = new ModifyMeterProgramIndiaAMI();
            List<string> configDescList = new List<string>();
            int i = 0;

            System.Xml.XmlDocument XMLDoc = new System.Xml.XmlDocument();
            XMLDoc.LoadXml(xml);

            // Changes the Program CRC to 0 in the XML Document
            XmlNodeList progCRCNode = XMLDoc.GetElementsByTagName(StaticConstantsCommon.ProgramCRC);
            progCRCNode[0].InnerText = "0";

            // Removes the Optional nodes from XML Document
            XmlNodeList descNode = XMLDoc.GetElementsByTagName(StaticConstantsCommon.ProgramDescription);
            string programDescription = descNode[0].InnerText;
            descNode[0].RemoveAll();

            XmlNodeList descNodes = XMLDoc.GetElementsByTagName(StaticConstantsCommon.ConfigDescription);

            foreach (XmlNode configNode in descNodes)
            {
                configDescList.Add(configNode.InnerText);
                configNode.RemoveAll();
            }

            XMLDoc.Save(filestream);
            filestream.Seek(0, SeekOrigin.Begin);

            mpr = (ModifyMeterProgramIndiaAMI)xsSubmit.Deserialize(filestream);

            mpr.ProgramCRC = CRCGenerator.ComputeInt32HashCode(mpr);
            mpr.ProgramDescription = programDescription;
            foreach (var configDesc in configDescList)
            {
                mpr.MeterOperation[i++].ConfigDescription = configDesc;
            }

            xsSubmit.Serialize(writer, mpr);

            return sww.ToString();
        }
       
        /// <summary>
        /// Computes the Integer HashCode for the object. 
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="isFullRegReadFLG"></param>
        /// <param name="isSnapSupportedFLG"></param>
        /// <returns></returns>
        public static Int32 ComputeInt32HashCode(object sourceObject)
        {
            return GetInt32HashCode(GetHashCodeByteArray(sourceObject));
        }

        /// <summary>
        /// Computes the 16 bit hash for
        /// byte array
        /// </summary>
        /// <param name="objectAsBytes"></param>
        /// <returns></returns>
        public static string ComputeHash(object sourceObject)
        {

            //Catch unuseful parameter values
            if (sourceObject == null)
            {
                throw new ArgumentNullException("Null as parameter is not allowed");
            }
            else
            {
                //We determine if the passed object is really serializable.
                try
                {
                    // Build the final string by converting each byte
                    // into hex and appending it to a StringBuilder
                    StringBuilder sb = new StringBuilder();

                    byte[] result = GetHashCodeByteArray(sourceObject);


                    for (int i = 0; i < result.Length; i++)
                    {
                        sb.Append(result[i].ToString("X2"));
                    }

                    // And return it
                    return sb.ToString();
                }
                catch (System.Reflection.AmbiguousMatchException ex)
                {
                    throw new ApplicationException("Could not definitely decide if object is serializable. Message:" + ex.Message);
                }
            }
        }

         

        private static byte[] GetHashCodeByteArray(object sourceObject)
        {
            // Build the final string by converting each byte
            // into hex and appending it to a StringBuilder
            StringBuilder sb = new StringBuilder();
            DataContractSerializer serializedSourceobject = new DataContractSerializer(sourceObject.GetType());
            byte[] result;
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                serializedSourceobject.WriteObject(memoryStream, sourceObject);
                System.Security.Cryptography.MD5 hash = new System.Security.Cryptography.MD5CryptoServiceProvider();
                //System.Security.Cryptography.SHA256 hash =
                //                 new System.Security.Cryptography.SHA256CryptoServiceProvider();
                result = hash.ComputeHash(memoryStream.ToArray());
            }
            return result;
        }

        private static Int32 GetInt32HashCode(byte[] hashText)
        {
            Int32 hashCode = 0;
            if (hashText.Length > 0)
            {
                //hashCodeStart = 0~7  8Byte
                //hashCodeMedium = 8~23  8Byte
                //and Fold
                Int32 hashCodeStart = BitConverter.ToInt32(hashText, 0);
                Int32 hashCodeMedium = BitConverter.ToInt32(hashText, 8);
                hashCode = hashCodeStart ^ hashCodeMedium;
                if (hashCode < 0)
                    hashCode = -hashCode;
            }

            return (hashCode);
        }

        public static string ComputeInt32CRC_Xml(byte[] arr)
        {
            System.Security.Cryptography.MD5 hash = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = hash.ComputeHash(arr);
            return (GetInt32HashCode(bytes)).ToString();
        }

       
         
    }

    /// <summary>
    /// crc16 is specefic for Bootloaded
    /// </summary>

    public class CRC16
    {
       
        public enum Crc16Mode : ushort { Standard = 0xA001, CcittKermit = 0x8408 } 

        readonly ushort[] table = new ushort[256];
        
        public  ushort ComputeChecksum(params byte[] bytes)
        {
            ushort crc = 0;
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte index = (byte)(crc ^ bytes[i]);
                crc = (ushort)((crc >> 8) ^ table[index]);
            }
            return crc;
        }

        public  byte[] ComputeChecksumBytes(params byte[] bytes)
        {
            ushort crc = ComputeChecksum(bytes);
            return BitConverter.GetBytes(crc);
        }

        public CRC16(Crc16Mode mode)
        {
            ushort polynomial = (ushort)mode;
            ushort value;
            ushort temp;
            for (ushort i = 0; i < table.Length; ++i)
            {
                value = 0;
                temp = i;
                for (byte j = 0; j < 8; ++j)
                {
                    if (((value ^ temp) & 0x0001) != 0)
                    {
                        value = (ushort)((value >> 1) ^ polynomial);
                    }
                    else
                    {
                        value >>= 1;
                    }
                    temp >>= 1;
                }
                table[i] = value;
            }
        }

        public string CalcCRC16(byte[] data)
        {
            ushort crc = 0xFFFF;
            for (int i = 0; i < data.Length; i++)
            {
                crc ^= (ushort)(data[i] << 8);
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x8000) > 0)
                        crc = (ushort)((crc << 1) ^ 0x1021);
                    else
                        crc <<= 1;
                }
            }
            return crc.ToString("X4");
        }
    }

    public static class CheckSum
    {
        public static string GetCheckSum(string strdata,bool isTwosComplement)
        {
            string retstr = "";

            if (strdata.Length == 0) return strdata;

            List<byte> myrawdata = DLMSDataStracture.ConvertStrToByte(strdata);

            UInt16 mychksum = 0x00;

            for (int i = 0; i < myrawdata.Count; i++)
            {
                mychksum += myrawdata[i];
            }

            // Two's complement
            if (isTwosComplement) mychksum = (byte)~mychksum;

            byte mybchk = (byte)mychksum;
            //Incase of Two's complement
            if (isTwosComplement) mybchk += 1;

            //// end
            retstr = mybchk.ToString("X2");

            return retstr;
        }
    }
}
