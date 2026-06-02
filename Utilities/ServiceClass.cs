using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Office.Interop;
using System.IO; 
using System.Management;
using System.Reflection;
using System.Globalization;
using Microsoft.Office.Interop.Excel;
using System.Security.Cryptography;
namespace Utilities
{
   
    public sealed class ServiceClass
    {
        private static volatile ServiceClass _ServiceInstance;
        private static Object syncRoot = new object();
        private ServiceClass()
        {
        }        
        public static ServiceClass ServiceInstance
        {
            get
            {
                if (_ServiceInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (_ServiceInstance == null)
                        {
                            _ServiceInstance = new ServiceClass();
                        }
                    }
                }

                return _ServiceInstance;
            }
        }


        /// <summary>
        /// Gets unit name defined in CosemEnumeratedUnits.xml for enum code
        /// </summary>
        /// <param name="filePath">Path for CosemEnumeratedUnits.xml</param>
        /// <param name="enumCode">Enum code defined in COSEM</param>
        /// <returns>Unit Name</returns>
        public string GetUnitNameFromCosemEnum(string filePath, byte enumCode)
        {
            try
            {
                string tempStr = string.Empty;
                //string fileName = "data.xml";
                XPathDocument doc = new XPathDocument(filePath);
                XPathNavigator nav = doc.CreateNavigator();

                //Compile a standard XPath expression

                XPathExpression expr;
                ///catalog/cd[price=10.90]
                string expression = "/COSEMEnums/Enum[@ID='" + enumCode.ToString() + "']/Unit";
                expr = nav.Compile(expression);
                XPathNodeIterator iterator = nav.Select(expr);

                //Iterate on the node set

                //listBox1.Items.Clear();
                string untiName = iterator.Current.HasAttributes.ToString();

                while (iterator.MoveNext())
                {
                    XPathNavigator nav2 = iterator.Current.Clone();
                    tempStr = nav2.Value;
                    //listBox1.Items.Add("price: " + nav2.Value);
                }
                return tempStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string AllignData(string val, int scale)
        {
            String Data;
            try
            {
                if (scale == 0)
                {
                    Data = val;
                }
                else
                {
                    Data = (Convert.ToDecimal(val) * Convert.ToDecimal(Math.Pow(10.0, Convert.ToDouble(scale)))).ToString();
                }
            }
            catch (Exception ex)
            {
                Data = val;
            }

            return Data;
        }

        /// <summary>
        /// Returns OBIS parameter name.
        /// </summary>
        /// <param name="classID">Class ID for the Obis code</param>
        /// <param name="ObisCode">Obis Code in HEX format eg(00.60.07.5E.3F.FF)</param>
        /// <param name="attribute">Attribute for the Obis cdoe</param>
        /// <returns>Obis name</returns>
        public string GetObisNameFromObisCode(int classID, string ObisCode, int attribute)
        {
            try
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Configuration\" + "DLMSInterfaceClass.xml";

                string tempStr = string.Empty;
                //string fileName = "data.xml";
                XPathDocument doc = new XPathDocument(filePath);
                XPathNavigator nav = doc.CreateNavigator();
                //Compile a standard XPath expression
                XPathExpression expr;
                //DLMS/LTCT_3Phase/Class[@ID="1"]/OBIS[@ParamCode="01.00.00.01.02.FF"]
                string expression = @"DLMS/LTCT_3Phase/Class[@ID=" + classID.ToString() + "]/OBIS[@ParamCode='" + ObisCode + "']";
                expr = nav.Compile(expression);
                XPathNodeIterator nodes = nav.Select(expr);
                nodes.MoveNext();                
                return nodes.Current.GetAttribute("ParamName", nodes.Current.NamespaceURI);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string GetKeysFromMeterID(string meterID)
        {
            try
            {
               // string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Configuration\" + "DLMSInterfaceClass.xml";
                string filePath = "C:/Users/guptabg/Desktop/ResponseXML_New.xml";
                string tempStr = string.Empty;
                //string fileName = "data.xml";
                XPathDocument doc = new XPathDocument(filePath);
                XPathNavigator nav = doc.CreateNavigator();
                //Compile a standard XPath expression
                XPathExpression expr;
                //DLMS/LTCT_3Phase/Class[@ID="1"]/OBIS[@ParamCode="01.00.00.01.02.FF"]
                string expression = @"EndDeviceSecurityConfig/EndDeviceSecurity/Names/[@name='" + meterID + "']";
                expr = nav.Compile(expression);
                XPathNodeIterator nodes = nav.Select(expr);
                nodes.MoveNext();
                return nodes.Current.GetAttribute("meterGlobalKey", nodes.Current.NamespaceURI);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public List<string> GetSecurityKeys(string SecurityKeyFilePath, string meterid)
        {

            string source = string.Empty;
            string result = string.Empty;
            string errorcode = string.Empty;
            int rowindex = 0;
            XmlDocument doc = new XmlDocument();
            List<string> SecurityKeyDetails = new List<string>();
            try
            {
                SecurityKeyFilePath = SecurityKeyFilePath + "\\EndDeviceSecurityKey.Xml";
                doc.Load(SecurityKeyFilePath);
                foreach (XmlNode node in doc.ChildNodes)
                {
                    if (node.Name == "ResponseMessage")
                        foreach (XmlNode node1 in node.ChildNodes)
                        {
                            if (node1.Name == "Header")
                            {
                                foreach (XmlNode node2 in node1.ChildNodes)
                                {
                                    if (node2.Name == "Source")
                                    {
                                        source = node2.InnerText;

                                    }

                                }
                            }
                            if (node1.Name == "Reply")
                            {
                                foreach (XmlNode node2 in node1.ChildNodes)
                                {
                                    if (node2.Name == "Result")
                                    {
                                        result = node2.InnerText;

                                    }
                                    if (node2.Name == "Error")
                                    {
                                        foreach (XmlNode node3 in node2.ChildNodes)
                                        {
                                            if (node3.Name == "code")
                                            {

                                                errorcode = node2.InnerText;
                                            }

                                        }


                                    }

                                }
                            }
                        }
                }
                if (source == "command center" && result == "OK" && errorcode == "0.0")
                {
                    var doc2 = XDocument.Load(SecurityKeyFilePath);
                   
                    var itemsList = (from c in doc2.Descendants("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity")
                                     select new
                                     {
                                         // item = query1.ElementAt(0),
                                     //    meterid = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}Names").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}name"),
                                     //    llsvalue = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}CustomAttributes").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}value"),
                                     //    secGlobalKey = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}meterGlobalKey"),
                                     //}).ToList()

                                         meterid = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}Names").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}name"),
                                         llsvalue = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}CustomAttributes").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}value"),
                                         secTempGlobalKey = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}CustomAttributes").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}value"),
                                         secGlobalKey = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}meterGlobalKey"),

                                     }).ToList();

                    int itemindex = 0;
                    int idualkeymultiplier = 1;
                    if (itemsList[0].llsvalue.Count() >= 2 * itemsList.Count) idualkeymultiplier = 2;

                    foreach (var item in itemsList)
                    {
                        SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).meterid.ElementAt(itemindex).Value);
                        SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).llsvalue.ElementAt(itemindex * idualkeymultiplier).Value);
                        SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).secGlobalKey.ElementAt(itemindex).Value);
                        if (itemsList.ElementAt(itemindex).secTempGlobalKey.Count() > 1)
                            SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).secTempGlobalKey.ElementAt(idualkeymultiplier * itemindex + 1).Value);
                    }
                    return SecurityKeyDetails;

                  
                }
                return null;

            }
            catch (UnauthorizedAccessException ex)
            {
                //MessageBox.Show("Access permission issue. Please run as administrator.");
               return null;
            }
             
        }


        public List<string> GetSecurityKeys(string SecurityKeyFilePath, string meterid, string strprivatekey)
        {
            string source = string.Empty;
            string result = string.Empty;
            string errorcode = string.Empty;
             XmlDocument doc = new XmlDocument();
            List<string> SecurityKeyDetails = new List<string>();
            try
            {
                //SecurityKeyFilePath = SecurityKeyFilePath + "\\EndDeviceSecurityKey.Xml";
                doc.Load(SecurityKeyFilePath);
                foreach (XmlNode node in doc.ChildNodes)
                {
                    if (node.Name == "ResponseMessageType")
                        foreach (XmlNode node1 in node.ChildNodes)
                        {
                            if (node1.Name == "Header")
                            {
                                foreach (XmlNode node2 in node1.ChildNodes)
                                {
                                    if (node2.Name == "Source")
                                    {
                                        source = node2.InnerText;

                                    }

                                }
                            }
                            if (node1.Name == "Reply")
                            {
                                foreach (XmlNode node2 in node1.ChildNodes)
                                {
                                    if (node2.Name == "Result")
                                    {
                                        result = node2.InnerText;

                                    }
                                    if (node2.Name == "Error")
                                    {
                                        foreach (XmlNode node3 in node2.ChildNodes)
                                        {
                                            if (node3.Name == "code")
                                            {

                                                errorcode = node2.InnerText;
                                            }

                                        }

                                    }

                                }
                            }
                        }
                }
                if (source == "command center" && result != "FAILED" && errorcode == "0.0")
                {
                    var doc2 = XDocument.Load(SecurityKeyFilePath);

                    var itemsList = (from c in doc2.Descendants("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity")
                                     select new
                                     {
                                         // item = query1.ElementAt(0),
                                         meterid = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}Names").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}name"),
                                         llsvalue = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}CustomAttributes").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}value"),
                                         secTempGlobalKey = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}CustomAttributes").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}value"),
                                         secGlobalKey = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}meterGlobalKey"),
                                        
                                     }).ToList();

                    int itemindex = 0;
                    int idualkeymultiplier = 1;
                    if (itemsList[0].llsvalue.Count() >= 2 * itemsList.Count) idualkeymultiplier = 2;

                    foreach (var item in itemsList)
                    {
                        if (itemsList.ElementAt(itemindex).meterid.ElementAt(itemindex).Value == meterid)
                        {
                            SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).meterid.ElementAt(itemindex).Value);
                            SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).llsvalue.ElementAt(itemindex * idualkeymultiplier).Value);
                            SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).secGlobalKey.ElementAt(itemindex).Value);
                            if (itemsList.ElementAt(itemindex).secTempGlobalKey.Count() > 1)
                                SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).secTempGlobalKey.ElementAt(idualkeymultiplier * itemindex + 1).Value);

                            break;
                        }

                        itemindex++;
                    }
                    //0: Meter ID
                    //1: LLS
                    //2: Global Key
                    //3: Temp Global Key
                    return GetDecryptedSecurityKeys(SecurityKeyDetails, strprivatekey);


                }
                return null;

            }
            catch (UnauthorizedAccessException ex)
            {
                //MessageBox.Show("Access permission issue. Please run as administrator.");
                return null;
            }

        }
        
        /// <summary>
        /// Convert OBIS code in Hexadecimal to Decimal format or vice versa.
        /// </summary>
        /// <param name="ObisCode">Obis Code in string format.</param>
        /// <param name="fromBase">The base of the number in value, which must be either 10 or 16.</param>
        /// <returns>Returns Obis code in either decimal or hexadecimal format depending upon </returns>
        public string ConvertObisCode(string ObisCode, int fromBase)
        {
            string[] hexCodes = ObisCode.Split('.');
            string decCode = string.Empty;
            string hexFormat = string.Empty;
            if (fromBase == 10) hexFormat = "X2";
            else hexFormat = "d";
            foreach (string hexCode in hexCodes)
            {
                decCode += Convert.ToInt32(hexCode, fromBase).ToString(hexFormat) + ".";
            }
            return decCode.Remove(decCode.LastIndexOf('.'));
        }


        /// <summary>
        /// Returns Scale and Unit from given Class ID, Obis code and Attribute.
        /// </summary>
        /// <param name="classID">Class ID for the Obis code</param>
        /// <param name="ObisCode">Obis Code in HEX format eg(00.60.07.5E.3F.FF)</param>
        /// <param name="attribute">Attribute for the Obis cdoe</param>
        /// <param name="filePath">Full path in string for XML file</param>
        ///<returns>String array containing Scale and Unit at index 0 and 1 respectively.</returns>

        public string[] GetScaleAndUnitsFromXMLFile(string classID, string ObisCode, string attribute, string filePath)
        {
            try
            {
                string[] strArray = new string[2];

                //string fileName = "data.xml";
                XPathDocument doc = new XPathDocument(filePath);
                XPathNavigator nav = doc.CreateNavigator();

                //Compile a standard XPath expression

                XPathExpression expr;
                string expression = @"//DLMS[Class = " + classID + "][ObisCode = '" + ObisCode + "']/Scale | //DLMS[Class = " + classID + "][ObisCode = '" + ObisCode + "']/Unit";
                expr = nav.Compile(expression);
                XPathNodeIterator nodes = nav.Select(expr);               
                nodes.MoveNext();
                if (nodes.Count >= 1)
                strArray[0] = nodes.Current.Value; //("Scale", nodes.Current.NamespaceURI);

                nodes.MoveNext();
                if (nodes.Count >= 2)
                strArray[1] = nodes.Current.Value; //nodes.Current.GetAttribute("Unit", nodes.Current.NamespaceURI);

                return strArray;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetDisplayParametersValue(string varDisplayParams,string varDescription, string varDescriptionVal,string returnPara, string filePath)
        {
            try
            {              
                string expression=string.Empty;
                XPathDocument doc = new XPathDocument(filePath);
                XPathNavigator nav = doc.CreateNavigator();              
                XPathExpression expr;
                XPathNodeIterator nodes;
                //Changes to support synchronization of display tags between BCS and PowerTool, and to maintain compatibility with earlier released display configuration files
                //which have the earlier generic tags   //20191216 A.S.
                expression = @"//" + varDisplayParams;
                expr = nav.Compile(expression);
                nodes = nav.Select(expr);
                if (nodes.Count == 0)
                {
                    switch (varDisplayParams)
                    {
                        case "DisplayParams":
                            varDisplayParams = "PushDisplayParams";
                            break;
                        case "DisplayParams1":
                            varDisplayParams = "ScrollDisplayParams";
                            break;
                        default:
                            break;
                    }
                }

                expression = @"//" + varDisplayParams + "[" + varDescription + "= '" + varDescriptionVal + "']/" + returnPara;               
                expr = nav.Compile(expression);
                nodes = nav.Select(expr);            
                nodes.MoveNext();
                return nodes.Current.Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string fGetTamperDescription(string sTamperID)
        {
            try
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Configuration\" + "TamperDescription.xml";

                string tempStr = string.Empty;
                //string fileName = "data.xml";
                XPathDocument doc = new XPathDocument(filePath);
                XPathNavigator nav = doc.CreateNavigator();

                //Compile a standard XPath expression

                XPathExpression expr;
                ///catalog/cd[price=10.90]
                string expression = "/TamperDescription/Enum[@ID='" + sTamperID + "']/Desc";
                expr = nav.Compile(expression);
                XPathNodeIterator iterator = nav.Select(expr);

                //Iterate on the node set

                //listBox1.Items.Clear();
                string untiName = iterator.Current.HasAttributes.ToString();

                while (iterator.MoveNext())
                {
                    XPathNavigator nav2 = iterator.Current.Clone();
                    tempStr = nav2.Value;
                }
                return tempStr;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Validate a specified string against given regular expression 
        /// </summary>
        /// <param name="toValidate">String to be validated</param>
        /// <param name="regEx">Regular expression for validation</param>
        /// <returns>Returns true if validation is performed successfully otherwise false.</returns>
        public bool ValidateRegEx(string toValidate, string regEx)
        {
            if (Regex.Match(toValidate, regEx).Success == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Returns minimum and maximum value of tamper threshold parameters.
        /// </summary>
        /// <param name="key">Key to be used for searching XML file</param>
        /// <param name="minValue">Minimum value</param>
        /// <param name="maxValue">Maximum value</param>
        /// <returns>Returns true if min and max values are found else false.</returns>
        public void GetTamperThresholdMinMaxParameters(string key, out Single minValue, out Single maxValue, string filePath)
        {
            try
            {
                //string filePath = AppDomain.CurrentDomain.BaseDirectory + @"Configuration\" + "E450_TamperThresholdValidation.xml";
              

                string tempStr = string.Empty;
                //string fileName = "data.xml";
                XPathDocument doc = new XPathDocument(filePath);
                XPathNavigator nav = doc.CreateNavigator();

                //Compile a standard XPath expression

                XPathExpression expr;

                //DLMS/LTCT_3Phase/Class[@ID="1"]/OBIS[@ParamCode="01.00.00.01.02.FF"]

                //string expression = @"TamperThreshold/MissingPotential/Occurence/Param[@key=""1""]";


                expr = nav.Compile(key + @"/*");
                XPathNodeIterator nodes = nav.Select(expr);
                
                nodes.MoveNext();

                if (Single.TryParse(nodes.Current.Value, out minValue) == false)
                {
                    throw new Exception("Key not found", null);
                }

                nodes.MoveNext();
                if (Single.TryParse(nodes.Current.Value, out maxValue) == false)
                {
                    throw new Exception("Key not found", null);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

         

        #region Export DataGridView To Excel
        /// <summary>  
        /// Export data from DataGridView to an Excel worksheet using Threading concepts.  
        /// </summary>  
        /// <param name="dGV">DataGridView name.</param>  
        /// <param name="initialRow">Number of Excel row where to start copying DataGridView titles.</param>  
        /// <param name="initialCol">Number of Excel column where to start copying DataGridView titles.</param>  
        /// <param name="exportTitles">True if you want to export DGV titles.</param>  
        /// <param name="wksName">How do you want to name the new worksheet.</param>  

        public void ExportDataGridViewToExcel(DataGridView dGV, int initialRow, int initialCol, bool exportTitles, string wksName)
        {
            Thread t1 = new Thread(

                delegate()
                {
                    OnWorkStart(dGV, new EventArgs());

                    // If exportTitles is set to false, change initial row value.  
                    if (!exportTitles) { initialRow = initialRow - 1; }

                    // Export Data.  
                    GetExcelReady(dGV, initialRow, initialCol, exportTitles, wksName);

                    OnWorkFinished(dGV, new EventArgs());
                }

            );

            t1.Start();
        }

        /// <summary>  
        /// Sets a new Excel object where to export DataGridView.  
        /// This method should be used with the methods:  
        /// ExportDataGridViewToExcel().  
        /// ToExcel().  
        /// </summary>  
        /// <param name="dGV">DataGridView name.</param>  
        /// <param name="initialRow">Number of Excel row where to start copying DataGridView titles.</param>  
        /// <param name="initialCol">Number of Excel column where to start copying DataGridView titles.</param>  
        /// <param name="exportTitles">True if you want to export DGV titles i.e column names.</param>  
        /// <param name="wksName">How do you want to name the new worksheet.</param>  
        void GetExcelReady(DataGridView dGV, int initialRow, int initialCol, bool exportTitles, string wksName)
        {
            // Declare missing object.  
            Object oMissing = System.Reflection.Missing.Value;
            
            // Change current thread culture to ("en-US").  
            // System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");  

            // Create a new Excel instance.  

            Microsoft.Office.Interop.Excel.Application oExcel = new Microsoft.Office.Interop.Excel.Application();
            oExcel.Visible = false;
            // Set Excel workbook to open with only 1 worsheet.  
            oExcel.SheetsInNewWorkbook = 1;

            // Set the UserControl property so Excel won't shut down.  
            oExcel.UserControl = true;

            // Add a workbook.  
            //Microsoft.Office.Interop.Excel.Workbook oBook = (Microsoft.Office.Interop.Excel.Workbook)(oExcel.Workbooks.Add("dhirendra"));
            Microsoft.Office.Interop.Excel.Workbook oBook = oExcel.Workbooks.Add(oMissing);
            //Microsoft.Office.Interop.Excel.Workbook oBook = oExcel.Workbooks.Add("wqasewr2eefs.xlsx");
            //oBook.Name.Replace(oBook.FullName, "dhire"); 
            //oBook.Names.Application.Name = "";
            
            // Get worksheets collection   
            Microsoft.Office.Interop.Excel.Sheets oSheetsColl = oExcel.Worksheets;

            // Get Worksheet number 1  

            Microsoft.Office.Interop.Excel.Worksheet oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oBook.ActiveSheet;
            //Microsoft.Office.Interop.Excel.Worksheet oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oSheetsColl.get_Item(1);
            oSheet.Name = wksName;

            //SaveFileDialog dialog = new SaveFileDialog();
            //dialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
            //dialog.FileName = oSheet.Name;
            //dialog.ValidateNames = true;

            //if (dialog.ShowDialog(dGV) == DialogResult.Cancel) { oSheet.Name = null; }
            //else { oSheet.Name = dialog.FileName; }

            //if (oSheet.Name == null) {return;}

            // Export dGV columns To Excel worksheet.  
            ToExcel(dGV, initialRow, initialCol, exportTitles, oSheet);

            // Make Excel visible to the user.  
            oExcel.Visible = true;
            oExcel.UserControl = true;
            //oExcel.GetSaveAsFilename(oSheet.Name + "-" + System.DateTime.Now.ToString("ddMMyyyyHHmmss"), "Excel Workbook (*.xlsx),*.xlsx,Excel 97-2003 Workbook (*.xls),*xls",1,"dhiru","savesdfkjg");
            //oExcel.Save(oSheet.Name);
            //oBook.SaveCopyAs("df");

            // Release the variables.  
            //oBook.Close(false, oMissing, oMissing);  
            oBook = null;

            //oExcel.Quit();  
            oExcel = null;

            // Collect garbage.  
            GC.Collect();


            //////////////////////////////////////////////
            
    //Dim objExcel As Object
    //Dim objWorkBook As Object
    //Set objExcel = CreateObject("EXCEL.APPLICATION")
    //Set objWorkBook = objExcel.Workbooks.Open("C:\test\test.xls")
    //objWorkBook.SaveAs ("C:\test\test2.xls")
    //objWorkBook.Close True
    //Set objWorkBook = Nothing
    //Set objExcel = Nothing

            
        }

        /// <summary>  
        /// Export from DataGridView to Excel worksheet.  
        /// This method should be used in combination with the methods:  
        /// ExportDataGridViewToExcel().  
        /// GetExcelReady().  
        /// </summary>  
        /// <param name="dGV">DataGridView name.</param>  
        /// <param name="initialRow">Number of Excel row where to start copying DataGridView titles.</param>  
        /// <param name="initialCol">Number of Excel column where to start copying DataGridView titles.</param>  
        /// <param name="exportTitles">True if you want to export DGV titles.</param>  
        /// <param name="wksName">How do you want to name the new worksheet.</param>  
        /// <param name="oSheet"></param>  
        void ToExcel(DataGridView dGV, int initialRow, int initialCol, bool exportTitles, Microsoft.Office.Interop.Excel.Worksheet oSheet)
        {
            int colIndex = 0;
            foreach (DataGridViewColumn column in dGV.Columns)
            {
                // Export only visible columns.  
                //if (dGV. )
                //{
               
                if (column.Visible)
                {
                    // Export.  
                    if (exportTitles)
                    { 
                        oSheet.Cells[initialRow, colIndex + initialCol] = column.HeaderText;
                    }
                    for (int row = initialRow; row <= initialRow + dGV.Rows.Count - 1; row++)
                    {
                        if (column.HeaderText.ToUpper().IndexOf("DATE") > 0) oSheet.Cells[row + 1, colIndex + initialCol] = "\"" + dGV[colIndex, row - initialRow].Value + "\"";
                        else oSheet.Cells[row + 1, colIndex + initialCol] =  dGV[colIndex, row - initialRow].Value ;
                    }
                    colIndex++;
                }
                 
            }
        }
        #endregion

        #region Export To CSV file
        /// <summary>  
        /// Manages a new thread used to Export from DataGridView to a csv (comma separated value) file.  
        /// This method must be used in combination with the methods:  
        /// GetCsvReady().  
        /// ToCsV().  
        /// </summary>  
        /// <param name="dGV">Extended DataGridView.</param>  
        /// <param name="filename">Name of csv file.</param>  
        public void CsvThread(DataGridView dGV, string filename)
        {
            Thread t1 = new Thread
                (
                delegate()
                {
                    OnWorkStart(dGV, new EventArgs());

                    // Export Data.  
                    GetCsvReady(dGV, filename);

                    OnWorkFinished(dGV, new EventArgs());
                }
                );
            t1.Start();
        }

        /// <summary>  
        /// Gets the new csv file name and path.  
        /// This method should be used with the methods:  
        /// CsvThread().  
        /// ToCsV().  
        /// </summary>  
        /// <param name="dGV">Extended DataGridView.</param>  
        /// <param name="filename">Name of csv file.</param>  
        void GetCsvReady(DataGridView dGV, string filename)
        {
            dGV.Invoke
            (
                new MethodInvoker
                (
                    delegate
                    {
                        SaveFileDialog dialog = new SaveFileDialog();
                        dialog.Filter = "Comma Separated Value (*.csv)|*.csv";
                        dialog.FileName = filename;
                        dialog.ValidateNames = true;

                        if (dialog.ShowDialog(dGV) == DialogResult.Cancel) { filename = null; }
                        else { filename = dialog.FileName; }

                        if (filename == null) { return; }

                    }
                )
            );

            // Export data.  
            ToCsV(dGV, filename);
        }

        /// <summary>  
        /// Export the DataGridView to Comma Separated Value file.  
        /// This method should be used with the methods:  
        /// CsvThread().  
        /// GetCsvReady().  
        /// </summary>  
        /// <param name="dGV">Extended DataGridView.</param>  
        /// <param name="filename">Full path & name of  the Name of csv file.</param>  
        void ToCsV(DataGridView dGV, string filename)
        {
            using (StreamWriter myFile = new StreamWriter(filename, false, Encoding.Default))
            {
                // Export only visible columns.  
                //if (dGV.ExportVisibleColumnsOnly)
                //{
                // Export titles:  
                string sHeaders = "";

                for (int j = 0; j < dGV.Columns.Count; j++)
                { 
                    if (dGV.Columns[j].Visible) 
                    { 
                        sHeaders = sHeaders.ToString() + dGV.Columns[j].HeaderText + ", "; 
                    } 
                }

                myFile.WriteLine(sHeaders);

                // Export data.  
                for (int i = 0; i < dGV.RowCount - 1; i++)
                {
                    string stLine = "";
                    for (int j = 0; j < dGV.Rows[i].Cells.Count; j++)
                    {
                        if (dGV.Columns[j].Visible)
                        {
                            stLine = stLine.ToString() + dGV.Rows[i].Cells[j].Value + ", ";
                        }
                    }
                    myFile.WriteLine(stLine);
                }
                
            }
        }
        #endregion

        private event EventHandler WorkStart;
        private event EventHandler WorkFinished;

        // Events  
        private void OnWorkStart(object sender, EventArgs e)
        {
            if (WorkStart != null) 
            {
                WorkStart(sender, e); 
            }
        }

        private void OnWorkFinished(object sender, EventArgs e)
        {
            if (WorkFinished != null) 
            {
                WorkFinished(sender, e); 
            }
        }

        public void ExportDataSetToExcell(DataSet dataSet)
        {
            Microsoft.Office.Interop.Excel.ApplicationClass ExcelApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
            Workbook xlWorkbook = ExcelApp.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);

            DataTableCollection collection = dataSet.Tables;
            
            for (int i = collection.Count; i > 0; i--)
            {
               // objLI.DisplayStatusMsg("Exporting " + dataSet.Tables[tabCount++].TableName.ToString() + "Data...", false);
                System.Windows.Forms.Application.DoEvents();
                System.Windows.Forms.Application.DoEvents();
                System.Windows.Forms.Application.DoEvents();
                Sheets xlSheets = null;
                Worksheet xlWorksheet = null;
                //Create Excel Sheets
                xlSheets = ExcelApp.Sheets;
                xlWorksheet = (Worksheet)xlSheets.Add(xlSheets[1], Type.Missing, Type.Missing, Type.Missing);
                System.Data.DataTable table = collection[i - 1];
                xlWorksheet.Name = table.TableName;
                for (int j = 1; j < table.Columns.Count + 1; j++)
                {
                    ExcelApp.Cells[1, j] = table.Columns[j - 1].ColumnName;

                }
                // Storing Each row and column value to excel sheet
                for (int k = 0; k < table.Rows.Count; k++)
                {
                    for (int l = 0; l < table.Columns.Count; l++)
                    {
                        if (table.Columns[l].ColumnName.ToUpper().IndexOf("DATE") > 0) ExcelApp.Cells[k + 2, l + 1] = "\"" +  table.Rows[k].ItemArray[l].ToString() + "\"";
                        else ExcelApp.Cells[k + 2, l + 1] = table.Rows[k].ItemArray[l].ToString();
                    }
                }
                ExcelApp.Columns.AutoFit();
            }

            ExcelApp.Visible = true;


            //objLI.DisplayStatusMsg("Data Exported !", false);


        }

        public System.Data.DataTable GetGridDataTable(string tableName, DataGridView dv)
        {
            System.Data.DataTable tabDT = new System.Data.DataTable(tableName);
            foreach (DataGridViewColumn col in dv.Columns)
            {
                tabDT.Columns.Add(col.HeaderText);
            }
            foreach (DataGridViewRow gridRow in dv.Rows)
            {
                DataRow dtRow = tabDT.NewRow();
                for (int i1 = 0; i1 < dv.Columns.Count; i1++)
                    dtRow[i1] = (gridRow.Cells[i1].Value == null ? DBNull.Value : gridRow.Cells[i1].Value);
                tabDT.Rows.Add(dtRow);
            }
            return tabDT;
        }

        private List<string> GetDecryptedSecurityKeys(List<string> arglistData, string argstrPrivateKey)
        {
            
            try
            {
                // Local varuables Initialised here
                string meterTempGlobalKey = "";
                const int LLSKEYINDEX = 1;
                const int GLBALKEYINDEX = 2;
                const int TEMPGLBALKEYINDEX = 3;
                byte[] globalkey = null;
                byte[] tempglobalkey = null;

                // Verify input data not null
                if (arglistData == null || arglistData.Count < 3)
                    return null;
               
                // Get Values
                string meterLLSKey = arglistData[LLSKEYINDEX];
                string meterGlobalKey = arglistData[GLBALKEYINDEX];

                // RSA objects creation
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                // Load private key
                rsa.FromXmlString(argstrPrivateKey);
                // Check valid temp global key
                if (arglistData.Count > TEMPGLBALKEYINDEX && arglistData[TEMPGLBALKEYINDEX] != null && arglistData[TEMPGLBALKEYINDEX].Length > 1)
                {
                    meterTempGlobalKey = arglistData[TEMPGLBALKEYINDEX];
                    tempglobalkey = rsa.Decrypt(Convert.FromBase64String(meterTempGlobalKey), false);
                    arglistData[TEMPGLBALKEYINDEX] = BitConverter.ToString(tempglobalkey);
                    arglistData[TEMPGLBALKEYINDEX] = arglistData[TEMPGLBALKEYINDEX].Replace("-", "");
                }

                // Check Global Key
                if (meterGlobalKey != null && meterGlobalKey.Length > 1)
                {
                    globalkey = rsa.Decrypt(Convert.FromBase64String(meterGlobalKey), false);
                    arglistData[GLBALKEYINDEX] = BitConverter.ToString(globalkey);
                    arglistData[GLBALKEYINDEX] = arglistData[GLBALKEYINDEX].Replace("-", "");
                }

                // Get LLS key
                byte[] llskey = rsa.Decrypt(Convert.FromBase64String(meterLLSKey), false);
                arglistData[LLSKEYINDEX] = new string(System.Text.Encoding.UTF8.GetString(llskey).ToCharArray());
                // Return List
                return arglistData;

            }
            catch (Exception ex)
            {
                
            }

            return null;
        }

    }

   

}
