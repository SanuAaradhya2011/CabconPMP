using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using SystemSecurityLibrary;

namespace Utilities
{
    public  class XMLFileHandle
    {
        private string metertype;      
        private string configcrc;
        private string meterid;
        private string timestamp;
        private string pathdetail;
        private List<MeterReadoutData> listmeterdata = new List<MeterReadoutData>();
        private List<MeterScalarData> listscalardata = new List<MeterScalarData>();
        private string DATANODENAME = "MeterData";

        public string MeterID
        {
            set { meterid = value; }
        }
        public string TimeStamp
        {
            set { timestamp = value; }
        }
        public string MeterType
        {
            set { metertype = value; }
        }
        public string FilePath
        {
            set { pathdetail = value; }
        }
        public string ConfigCRC
        {
            set { configcrc = value; }
            get { return configcrc;  }
        }


        public List<MeterReadoutData> MeterDataList
        {
            get { return listmeterdata; }
            set { listmeterdata = value; }
        }


        public List<MeterScalarData> ScalarDataList
        {
            get { return listscalardata; }
            set { listscalardata = value; }
        }


        public bool GenerateXML()
        {
            if (pathdetail.Length < 1) return false;

            return CreateNewFile();
            
        }

        public XMLFileHandle()
        {
        }

        private static Int32 GetInt32HashCode(byte[] hashText)
        {
            Int32 hashCode = 0;
            if (hashText.Length > 0)
            {
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

        public static string ComputeChecksumFromXml(string xml)
        {
            MemoryStream filestream = new MemoryStream();
            XmlSerializer xsSubmit = new XmlSerializer(typeof(XMLFileHandle));
            StringWriter sww = new StringWriter();
            XMLFileHandle objxmlhandle = new XMLFileHandle();
            XmlWriter writer = XmlWriter.Create(sww);
            XMLFileHandle objxml = new XMLFileHandle();
            List<string> configDescList = new List<string>();
           
            System.Xml.XmlDocument XMLDoc = new System.Xml.XmlDocument();
            XMLDoc.Load(xml);

            // Changes the Program CRC to 0 in the XML Document
            XmlNodeList progCRCNode = XMLDoc.GetElementsByTagName("ProgramCRC");
            progCRCNode[0].InnerText = "0";

            XmlNodeList meterDataNodeList = XMLDoc.GetElementsByTagName("MeterData");
            string timestamp = meterDataNodeList[0].Attributes[1].Value;
            meterDataNodeList[0].Attributes[1].Value = "0";

            XMLDoc.Save(filestream);

            byte[] bytes = Encoding.Default.GetBytes(XMLDoc.OuterXml);
            progCRCNode[0].InnerText = ComputeInt32CRC_Xml(bytes);
            meterDataNodeList[0].Attributes[1].Value = timestamp;
            
            XMLDoc.Save(xml);

            return sww.ToString();
        }

        private bool CreateNewFile()
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-16", null);
                doc.AppendChild(docNode);

                XmlNode startNode = doc.CreateElement("DLMSPT");
                doc.AppendChild(startNode);

                XmlNode producttypeNode = doc.CreateElement("MeterType");
                producttypeNode.AppendChild(doc.CreateTextNode(metertype));
                startNode.AppendChild(producttypeNode);

                XmlNode dataNodecrc = doc.CreateElement("ProgramCRC");
                dataNodecrc.AppendChild(doc.CreateTextNode("102.99"));
                startNode.AppendChild(dataNodecrc);

                XmlNode dataNodeconfigcrc = doc.CreateElement("ConfigCRC");
                dataNodeconfigcrc.AppendChild(doc.CreateTextNode(configcrc));
                startNode.AppendChild(dataNodeconfigcrc);

                XmlNode dataStartNode = doc.CreateElement(DATANODENAME);
                XmlAttribute nodeAttribute = doc.CreateAttribute("ID");
                nodeAttribute.Value = meterid;
                dataStartNode.Attributes.Append(nodeAttribute);

                nodeAttribute = doc.CreateAttribute("TimeStamp");
                nodeAttribute.Value = timestamp;
                dataStartNode.Attributes.Append(nodeAttribute);

                startNode.AppendChild(dataStartNode);

                for (int inodecount = 0; inodecount < listmeterdata.Count; inodecount++)
                {
                    XmlNode dataNameNode = doc.CreateElement(listmeterdata[inodecount].NodeName);
                    dataStartNode.AppendChild(dataNameNode);

                    for (int iobjectcount = 0; iobjectcount < listmeterdata[inodecount].MeterObjectData.Count; iobjectcount++)
                    {
                        XmlNode subdataNameNode = doc.CreateElement("Data");
                        nodeAttribute = doc.CreateAttribute("Name");
                        nodeAttribute.Value = listmeterdata[inodecount].MeterObjectData[iobjectcount].NodeName;
                        subdataNameNode.Attributes.Append(nodeAttribute);

                        nodeAttribute = doc.CreateAttribute("Count");
                        nodeAttribute.Value = listmeterdata[inodecount].MeterObjectData[iobjectcount].DataCount.ToString();
                        subdataNameNode.Attributes.Append(nodeAttribute);

                        for (int idatacount = 0; idatacount < listmeterdata[inodecount].MeterObjectData[iobjectcount].ObjectData.Count; idatacount++)
                        {
                            if (listmeterdata[inodecount].MeterObjectData[iobjectcount].DataValue == null || listmeterdata[inodecount].MeterObjectData[iobjectcount].DataValue.Count == 0) continue;
                            if (listmeterdata[inodecount].MeterObjectData[iobjectcount].DataValue[idatacount] == null ) continue;

                            XmlNode dataNode = doc.CreateElement("Object");
                            dataNode.AppendChild(doc.CreateTextNode(Convert.ToBase64String(listmeterdata[inodecount].MeterObjectData[iobjectcount].ObjectData[idatacount])));
                            subdataNameNode.AppendChild(dataNode);

                            dataNode = doc.CreateElement("Value");

                            dataNode.AppendChild(doc.CreateTextNode(Convert.ToBase64String(listmeterdata[inodecount].MeterObjectData[iobjectcount].DataValue[idatacount])));
                            subdataNameNode.AppendChild(dataNode);
                        }

                        dataNameNode.AppendChild(subdataNameNode);
                    }

                    if (listscalardata == null || listscalardata.Count == 0) continue;

                    XmlNode subdataNameNode1 = doc.CreateElement("Scalar");

                    for (int iobjectcount = 0; iobjectcount < listscalardata[inodecount].ScalarData.Count; iobjectcount++)
                    {
                        string strname = listscalardata[inodecount].NodeName[iobjectcount];
                        if (strname.Length < 1) break;

                        XmlNode dataNode = doc.CreateElement("Object");// doc.CreateElement(strname);
                        dataNode.AppendChild(doc.CreateTextNode(Convert.ToBase64String(listscalardata[inodecount].ScalarObjct[iobjectcount])));
                        subdataNameNode1.AppendChild(dataNode);
                        dataNode = doc.CreateElement("Value");
                        dataNode.AppendChild(doc.CreateTextNode(Convert.ToBase64String(listscalardata[inodecount].ScalarData[iobjectcount])));
                        subdataNameNode1.AppendChild(dataNode);
                    }
                    dataNameNode.AppendChild(subdataNameNode1);
                }
                doc.Save(pathdetail);
                return true;
            }
            catch (Exception)
            {
            }

            return false;
        }

        private bool UpdateExistingFile()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(pathdetail);
                XmlElement elmRoot = doc.DocumentElement;
                XmlElement elmNew = doc.CreateElement(DATANODENAME);
                XmlAttribute nodeAttribute = doc.CreateAttribute("ID");
                nodeAttribute.Value = meterid;
                elmNew.Attributes.Append(nodeAttribute);

                nodeAttribute = doc.CreateAttribute("TimeStamp");
                nodeAttribute.Value = timestamp;
                elmNew.Attributes.Append(nodeAttribute);

                elmRoot.AppendChild(elmNew);
                elmRoot = doc.DocumentElement;

                for (int inodecount = 0; inodecount < listmeterdata.Count; inodecount++)
                {
                    XmlNode dataNameNode = doc.CreateElement(listmeterdata[inodecount].NodeName);

                    for (int iobjectcount = 0; iobjectcount < listmeterdata[inodecount].MeterObjectData.Count; iobjectcount++)
                    {
                        XmlNode subdataNameNode = doc.CreateElement("Data");
                        nodeAttribute = doc.CreateAttribute("Name");
                        nodeAttribute.Value = listmeterdata[inodecount].MeterObjectData[iobjectcount].NodeName;
                        subdataNameNode.Attributes.Append(nodeAttribute);

                        nodeAttribute = doc.CreateAttribute("Count");
                        nodeAttribute.Value = listmeterdata[inodecount].MeterObjectData[iobjectcount].DataCount.ToString();
                        subdataNameNode.Attributes.Append(nodeAttribute);

                        for (int idatacount = 0; idatacount < listmeterdata[inodecount].MeterObjectData[iobjectcount].ObjectData.Count; idatacount++)
                        {
                            if (listmeterdata[inodecount].MeterObjectData[iobjectcount].DataValue == null || listmeterdata[inodecount].MeterObjectData[iobjectcount].DataValue.Count == 0) continue;

                            XmlNode dataNode = doc.CreateElement("Object");
                            dataNode.AppendChild(doc.CreateTextNode(Convert.ToBase64String(listmeterdata[inodecount].MeterObjectData[iobjectcount].ObjectData[idatacount])));
                            subdataNameNode.AppendChild(dataNode);

                            dataNode = doc.CreateElement("Value");
                            dataNode.AppendChild(doc.CreateTextNode(Convert.ToBase64String(listmeterdata[inodecount].MeterObjectData[iobjectcount].DataValue[idatacount])));
                            subdataNameNode.AppendChild(dataNode);
                        }

                        dataNameNode.AppendChild(subdataNameNode);
                        elmRoot.LastChild.AppendChild(dataNameNode);

                    }

                    if (listscalardata == null || listscalardata.Count == 0) continue;

                    XmlNode subdataNameNode1 = doc.CreateElement("Scalar");

                    for (int iobjectcount = 0; iobjectcount < listscalardata[inodecount].ScalarData.Count; iobjectcount++)
                    {
                        string strname = listscalardata[inodecount].NodeName[iobjectcount];
                        if (strname.Length < 1) break;

                        XmlNode dataNode = doc.CreateElement(strname);
                        dataNode.AppendChild(doc.CreateTextNode(Convert.ToBase64String(listscalardata[inodecount].ScalarObjct[iobjectcount])));
                        subdataNameNode1.AppendChild(dataNode);

                        dataNode = doc.CreateElement("Value");
                        dataNode.AppendChild(doc.CreateTextNode(Convert.ToBase64String(listscalardata[inodecount].ScalarData[iobjectcount])));
                        subdataNameNode1.AppendChild(dataNode);
                    }

                    dataNameNode.AppendChild(subdataNameNode1);

                    elmRoot.LastChild.AppendChild(dataNameNode); ;
                }

                doc.Save(pathdetail);

                return true;
            }
            catch (Exception)
            {
            }

            return false;
        }
      /// <summary>
      /// Method Name : uploadfromfile
      /// Details : This method load the xml file and construct the object list as per class
      /// </summary>
      /// <param name="strfilename"></param>
      /// <returns>List<MeterReadoutData></returns>

        public List<MeterReadoutData> uploadfromfile(string strfilename)
        {
            List<MeterReadoutData> MeterNodeData = new List<MeterReadoutData>();

            try
            {  
                XmlDocument xml = new XmlDocument();
                xml.Load(strfilename);

                // Finding node for data
                XmlNodeList xnList = xml.SelectNodes("DLMSPT/MeterData");

                // go through the each node of data
                foreach (XmlNode xn in xnList)
                {
                    MeterReadoutData _objmeternode = new MeterReadoutData();

                    // get meter id and timestamp from xml
                    _objmeternode.MeterID = xn.Attributes["ID"].Value;
                    _objmeternode.TimeStamp = xn.Attributes["TimeStamp"].Value;

                    // create list for meter data and scalar data
                    List<MeterobjectData> _listmeterdata = new List<MeterobjectData>();
                    List<MeterScalarData> _listscalardata = new List<MeterScalarData>();

                    // populate data for each child
                    for (int icount = 0; icount < xn.ChildNodes.Count; icount++)
                    {

                        for (int ichildcount = 0; ichildcount < xn.ChildNodes[icount].ChildNodes.Count; ichildcount++)
                        {
                            MeterobjectData _meterdata = new MeterobjectData();
                            MeterScalarData _scalardata = new MeterScalarData();

                            // file node name of data
                            _meterdata.NodeName = (xn.ChildNodes[icount].Name.ToString());

                            // find the node is data or scalar
                            if (!xn.ChildNodes[icount].ChildNodes[ichildcount].Name.Contains("Scalar"))
                            {
                                // if no data then continue
                                if (!xn.ChildNodes[icount].ChildNodes[ichildcount].HasChildNodes)
                                  continue;

                                // populate the data in byte array format using Base64 for Object and data
                                byte[] badata = Convert.FromBase64String(xn.ChildNodes[icount].ChildNodes[ichildcount].ChildNodes[0].InnerText.ToString());
                                _meterdata.DataCount = byte.Parse(xn.ChildNodes[icount].ChildNodes[ichildcount].Attributes[1].Value);
                                _meterdata.DataNodeName = xn.ChildNodes[icount].ChildNodes[ichildcount].Attributes[0].Value;
                                _meterdata.ObjectData.Add(badata);
                                badata = Convert.FromBase64String(xn.ChildNodes[icount].ChildNodes[ichildcount].ChildNodes[1].InnerText.ToString());
                                _meterdata.DataValue.Add(badata);
                                _listmeterdata.Add(_meterdata);
                            }
                            else
                            {
                                if (!xn.ChildNodes[icount].ChildNodes[ichildcount].HasChildNodes)
                                    continue;

                                // populate the data in byte array format using UTF-16 encoding for Scalar object and Scalar data
                                byte[] badata = Convert.FromBase64String(xn.ChildNodes[icount].ChildNodes[ichildcount].ChildNodes[0].InnerText.ToString());
                                _scalardata.ScalarObjct.Add(badata);
                                _scalardata.ScalarNodeName = (xn.ChildNodes[icount].Name.ToString());
                                badata = Convert.FromBase64String(xn.ChildNodes[icount].ChildNodes[ichildcount].ChildNodes[1].InnerText.ToString());
                                _scalardata.ScalarData.Add(badata);
                                _listscalardata.Add(_scalardata);
                            }
                        }

                        // file the list for meterdata and scalar data
                        _objmeternode.MeterObjectData = _listmeterdata.GetRange(0, _listmeterdata.Count);
                        _objmeternode.MeterScalarData = _listscalardata.GetRange(0, _listscalardata.Count);

                    }

                    // update Meternodedata list
                    MeterNodeData.Add(_objmeternode);
                }

                return MeterNodeData;

            }
            catch (Exception)
            {
                return null;
            }
       
        }

        public bool uploadfrom_IEC_file(string strfilename, IECMeterReadoutData _objmeternode)
        {
          //  List<IECMeterReadoutData> MeterNodeData = new List<IECMeterReadoutData>();

            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(strfilename);
                // Finding node for data
                XmlNodeList xnList = xml.SelectNodes("IEC/MeterReadout");
                // go through the each node of data
                foreach (XmlNode xn in xnList)
                {                   

                    // populate data for each child
                    for (int icount = 0; icount < xn.ChildNodes.Count; icount++)
                    {
                        
                        for (int ichildcount = 0; ichildcount < xn.ChildNodes[icount].ChildNodes.Count; ichildcount++)
                        {                           
                                // if no data then continue
                                if (!xn.ChildNodes[icount].ChildNodes[ichildcount].HasChildNodes)
                                    continue;
                                _objmeternode.DataNodes.Add((xn.ChildNodes[icount].Name.ToString()));
                                // populate the data in byte array format using Base64 for Object and data
                                _objmeternode.MeterConfiguration.Add(xn.ChildNodes[icount].ChildNodes[ichildcount].InnerText.ToString());
                                _objmeternode.MeterData.Add(xn.ChildNodes[icount].ChildNodes[ichildcount + 1].InnerText.ToString());
                                break;

                        }
                    }
 
                }

                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }

        public static int GetObjectIndex(List<MeterReadoutData> listMeterReadoutData, string paramname, int inodecount)
        {
            int iretval = -1;

            for (int icount = 0; icount < listMeterReadoutData[inodecount].MeterObjectData.Count; icount++)
            {
                if (listMeterReadoutData[inodecount].MeterObjectData[icount].NodeName == paramname)
                {
                    iretval = icount;
                    break;
                }
            }

            return iretval;
        }

        public static int GetScalarIndex(List<MeterReadoutData> listMeterReadoutData, string paramname, int inodecount)
        {
            int iretval = -1;

            for (int icount = 0; icount < listMeterReadoutData[inodecount].MeterScalarData.Count; icount++)
            {
                if (listMeterReadoutData[inodecount].MeterScalarData[icount].ScalarNodeName == paramname)
                {
                    iretval = icount;
                    break;
                }
            }

            return iretval;
        }

        public static bool ComputeReadoutChecksumFromXml(string xml)
        {
            try
            {
                MemoryStream filestream = new MemoryStream();
                XmlSerializer xsSubmit = new XmlSerializer(typeof(XMLFileHandle));
                StringWriter sww = new StringWriter();
                XMLFileHandle objxmlhandle = new XMLFileHandle();
                XmlWriter writer = XmlWriter.Create(sww);
                XMLFileHandle objxml = new XMLFileHandle();
                List<string> configDescList = new List<string>();
              

                System.Xml.XmlDocument XMLDoc = new System.Xml.XmlDocument();
                XMLDoc.Load(xml);

                // Changes the Program CRC to 0 in the XML Document
                XmlNodeList progCRCNode = XMLDoc.GetElementsByTagName("ProgramCRC");
                string filecrc = progCRCNode[0].InnerText;
                progCRCNode[0].InnerText = "0";
                //----------------------As HES is not Consedering------------------------------------------------------------------------------------
                // Changes the Program CRC to 0 in the XML Document
                //XmlNodeList configCRCNode = XMLDoc.GetElementsByTagName("ConfigCRC");
                //string dataconfigrc = configCRCNode[0].InnerText;
                //dataconfigrc[0].InnerText = "0";
                //----------------------As HES is not Consedering build 0.0.1.8 Program CRC Mismatch issue resolved by commenting this------------------
                //XmlNodeList meterDataNodeList = XMLDoc.GetElementsByTagName("MeterData");
                //string timestamp = meterDataNodeList[0].Attributes[1].Value;
                //meterDataNodeList[0].Attributes[1].Value = "0";
                //---------------------------------------------------------------------------------------------------------------------------------------
                XMLDoc.Save(filestream);

                byte[] bytes = Encoding.Default.GetBytes(XMLDoc.OuterXml);

                progCRCNode[0].InnerText = CRCGenerator.ComputeInt32CRC_Xml(bytes);

                if (filecrc != progCRCNode[0].InnerText) return false;

                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }

        public static string UpdateChecksumFromXml(string xml)
        {
            try
            {
                MemoryStream filestream = new MemoryStream();
                XmlSerializer xsSubmit = new XmlSerializer(typeof(XMLFileHandle));
                StringWriter sww = new StringWriter();
                XMLFileHandle objxmlhandle = new XMLFileHandle();
                XmlWriter writer = XmlWriter.Create(sww);
                XMLFileHandle objxml = new XMLFileHandle();
                List<string> configDescList = new List<string>();
                string strfilename="";
             
                System.Xml.XmlDocument XMLDoc = new System.Xml.XmlDocument();
                XMLDoc.Load(xml);

                // Changes the Program CRC to 0 in the XML Document
                XmlNodeList progCRCNode = XMLDoc.GetElementsByTagName("ProgramCRC");
                progCRCNode[0].InnerText = "0";
                //----------------------As HES is not Consedering build 0.0.1.8 Program CRC Mismatch issue resolved by commenting this------------------
                //XmlNodeList meterDataNodeList = XMLDoc.GetElementsByTagName("MeterData");
                //string timestamp = meterDataNodeList[0].Attributes[1].Value;
                //meterDataNodeList[0].Attributes[1].Value = "0";
                //---------------------------------------------------------------------------------------------------------------------------------------
                XMLDoc.Save(filestream);
                byte[] bytes = Encoding.Default.GetBytes(XMLDoc.OuterXml);
                progCRCNode[0].InnerText = ComputeInt32CRC_Xml(bytes);
                //meterDataNodeList[0].Attributes[1].Value = timestamp;// as above build 0.0.1.8 Program CRC Mismatch issue resolved by commenting this
                strfilename = xml + "_" + progCRCNode[0].InnerText + ".xml";
                XMLDoc.Save(strfilename);
                File.Delete(xml);
                return strfilename ;
            }
            catch (Exception)
            {
                return "";
            }

        }
     


    }



    /// <summary>
    /// Class Name: MeterobjectData
    /// Details: This class contains attributes for object data and object value
    /// </summary>
   public  class MeterobjectData
    {
        private List<byte[]> objectdata = new List<byte[]>();
        private List<byte[]> datavalue = new List<byte[]>();
        private byte datacount;
        private string nodename;
        private string datanodename;

        public string DataNodeName
        {
            get { return datanodename; }
            set { datanodename = value; }
        }

        public MeterobjectData()
        {
            datacount = 0;
        }

        public byte DataCount
        {
            get { return datacount; }
            set { datacount = value; }
        }

        public string NodeName
        {
            get { return nodename; }
            set { nodename = value; }
        }

        public List<byte[]> ObjectData
        {
            get { return objectdata; }
            set { objectdata = value; }
        }

        public List<byte[]> DataValue
        {
            get { return datavalue; }
            set { datavalue = value; }
        }
    }

    /// <summary>
    /// Class Name: MeterScalarData
    /// Details: This class contains the attributes for scalar data (Scakar Object and Scalar Data) handling
    /// </summary>
    public class MeterScalarData
    {
        private List<byte[]> scalarobject = new List<byte[]>();
        private List<byte[]> scalardata = new List<byte[]>();
        private List<string> nodename;
        private string scalarnodename;

        public string ScalarNodeName
        {
            get { return scalarnodename; }
            set { scalarnodename = value; }
        }

        public List<byte[]> ScalarObjct
        {
            get { return scalarobject; }
            set { scalarobject = value; }
        }

        public List<byte[]> ScalarData
        {
            get { return scalardata; }
            set { scalardata = value; }
        }

        public List<string> NodeName
        {
            get { return nodename; }
            set { nodename = value; }
        }

    }

    /// <summary>
    /// Class Name: MeterReadoutData
    /// Details: This class represents the architecture of of readout data. In each readout tag there will be MeterID, TimeStamp, NodeName, List of object data and list of scalar data
    /// </summary>
    public class MeterReadoutData
    {
        private string nodename;
        private string subnodename;
        private string meterid;
        private string timestamp;

        private List<MeterScalarData> meterscalardata = new List<MeterScalarData>();
        private List<MeterobjectData> meterobjectdata = new List<MeterobjectData>();

        public List<MeterScalarData> MeterScalarData
        {
            get { return meterscalardata; }
            set { meterscalardata = value; }
        }

        public List<MeterobjectData> MeterObjectData
        {
            get { return meterobjectdata; }
            set { meterobjectdata = value; }
        }
      
      
        public string SubNodeName
        {
            get { return subnodename; }
            set { subnodename = value; }
        }

      
        public string NodeName
        {
            get { return nodename; }
            set { nodename = value; }
        }

        public string MeterID
        {
            get { return meterid; }
            set { meterid = value; }
        }

        public string TimeStamp
        {
            get {
                if (timestamp.Length == 14)
                {
                    StandardDateTime objstddt = new StandardDateTime();
                    timestamp = objstddt.LongToStringDateTimeWithSecFormat(timestamp);
                }
                return timestamp; 
            }
            set { timestamp = value; }
        }
        
    }

    public class IECMeterReadoutData
    {
        private List<string> meterConfiguration = new List<string>();
        private List<string> meterData = new List<string>();
        private List<string> dataNodes = new List<string>();


        public List<string> MeterConfiguration
        {
            get { return meterConfiguration; }
            set { meterConfiguration = value; }
        }

        public List<string> MeterData
        {
            get { return meterData; }
            set { meterData = value; }
        }

        public List<string> DataNodes
        {
            get { return dataNodes; }
            set { dataNodes = value; }
        }    


    }

     
}
