using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Net.NetworkInformation;
namespace SystemSecurityLibrary
{
    public static class SystemInfo
    {
        public static string ProductVersion()
        {
            string productversion = "";
            Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
            if (assembly != null)
            {
                object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if ((customAttributes != null) && (customAttributes.Length > 0))
                {
                    productversion = ((AssemblyTitleAttribute)customAttributes[0]).Title;
                }
                if (string.IsNullOrEmpty(productversion))
                {
                    productversion = string.Empty;
                }
            }
            return productversion;

        }
        public static string CopyRightsDetail()
        {
            string copyRightsDetail = "";
            Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
            if (assembly != null)
            {
                object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if ((customAttributes != null) && (customAttributes.Length > 0))
                {
                    copyRightsDetail = ((AssemblyCopyrightAttribute)customAttributes[0]).Copyright;
                }
                if (string.IsNullOrEmpty(copyRightsDetail))
                {
                    copyRightsDetail = string.Empty;
                }
            }
            return copyRightsDetail;

        }

        public static string GetLastBuildDate()
        {
            try
            {
                string f_Name = AppDomain.CurrentDomain.BaseDirectory + @"\\" +Application.ProductName.ToString() + ".exe";// PowerTool.exe";
                DateTime buiddate = File.GetLastWriteTime(f_Name);
                string getBuildDate = string.Format("{0:dd}", buiddate) + " " + string.Format("{0:MMM}", buiddate) + " " + string.Format("{0:yyyy}", buiddate);
                return getBuildDate;
            }
            catch (Exception)
            {
                return "";
            }

        }
        public static string GetSystemMACAddress()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.NetworkInterfaceType.ToString().ToLower() == "ethernet" && nic.Name.ToLower() == "ethernet")
                {
                    macAddresses = nic.GetPhysicalAddress().ToString();

                }
            }

            if (macAddresses == "" || macAddresses.Length < 1) macAddresses = "AABBCCDDEEFF";
            return macAddresses;
        }

        public static string GetDomainName()
        {
            return Environment.UserDomainName.ToUpperInvariant();
        }
        public static string GetLoggedUserName()
        {
            return Environment.UserName;
        }
        public static string GetCurrentSystemName()
        {
            return Environment.MachineName;
        }
        public static string GetCurrentProductName()
        {
            return Application.ProductName.ToString();
        }
    }
}
