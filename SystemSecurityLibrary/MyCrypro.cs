using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Management;
using System.Reflection;
using System.Globalization;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Forms;
namespace SystemSecurityLibrary
{
    public class MyCrypro
    {
        
        private const string CRYPTKEY = "RZFKGEHN$GCNV@LXJLKMEKDYYURTKJLE";
        private const string CRYPTIV = "ILMCFACBXYKPWGPW";
        public RijndaelManaged _Rijndael = new RijndaelManaged();
        public string CalFileBcc(string RecInpData)
        {
            long countbyt = 0;
            long Bcc = 0;
            string strBcc = "";
            try
            {
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                Byte[] bytesBcc = encoding.GetBytes(RecInpData);

                Byte[] bytes = encoding.GetBytes(RecInpData);
                foreach (byte b in bytes)
                {
                    if (countbyt <= RecInpData.Length) Bcc = Bcc ^ b;
                    countbyt++;
                }

                strBcc = Convert.ToChar(Bcc).ToString();
                return strBcc;
            }
            catch (Exception)
            {
                return "";
            }

        }

        public string EncryptString(string plainText)
        {
            _Rijndael.Key = Encoding.ASCII.GetBytes(CRYPTKEY);
            _Rijndael.IV = Encoding.ASCII.GetBytes(CRYPTIV);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            ICryptoTransform transform = _Rijndael.CreateEncryptor();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);
            cs.Write(plainTextBytes, 0, plainTextBytes.Length);

            cs.FlushFinalBlock();

            byte[] cipherText = ms.ToArray();

            return Convert.ToBase64String(cipherText);



        }

        /// <summary>
        /// Decrypt a string
        /// </summary>
        /// <param name="encryptedString"></param>
        /// <returns></returns>
        public string DecryptString(string cipherText)
        {
            try
            {
                _Rijndael.Key = Encoding.ASCII.GetBytes(CRYPTKEY);
                _Rijndael.IV = Encoding.ASCII.GetBytes(CRYPTIV);
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                ICryptoTransform transform = _Rijndael.CreateDecryptor();
                MemoryStream ms = new MemoryStream(cipherTextBytes);
                CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Read);

                byte[] plainTextBuffer = new byte[cipherTextBytes.Length];
                int plainTextLength = cs.Read(plainTextBuffer, 0, cipherTextBytes.Length);

                byte[] plainText = new byte[plainTextLength];
                Array.Copy(plainTextBuffer, 0, plainText, 0, plainTextLength);
                return Encoding.UTF8.GetString(plainText);
            }
            catch (Exception)
            {
                return "";
            }


        }
        
        public bool IsDomainAutherizedUser()
        {
            try
            {
            List<string> autherizedUserList = new List<string> { "GUPTABG", "RAZAM", "YADAVD", "TIWARIRA" };
            string DomainName = Environment.UserDomainName.ToUpperInvariant();
            string userName = Environment.UserName;
            if (DomainName == "AP" && autherizedUserList.Contains(userName.ToUpperInvariant())) return true;
            else return false;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public bool IsSystemConnectWithDomain()
        {
            try
            {
                string DomainIP = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
                Ping myPing = new Ping();
                PingReply reply = myPing.Send(DomainIP, 5000);
                if (reply != null) return true;
                else return false;
                   // string message = "Status :  " + reply.Status + " \n Time : " + reply.RoundtripTime.ToString() + " \n Address : " + reply.Address;
            }
            catch (Exception) //--If not Connected.
            {
                return false;
            }

        }

         
    }
}
