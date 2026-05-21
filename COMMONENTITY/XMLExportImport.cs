using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utilities;
using SystemSecurityLibrary;
namespace COMMONENTITY
{
    public class XMLExportImport
    {
        AESEncryption objaes = new AESEncryption();

        public bool ExportXMLFromDatatable(DataTable dtValue, string XMLFileName)
        {
            try
            {
                  dtValue.WriteXml(AppDomain.CurrentDomain.BaseDirectory + "\\Configuration\\" + XMLFileName + ".xml", XmlWriteMode.IgnoreSchema);
                  return true;
            }
            catch (Exception)
            {
                
                return false;
            }
        }

        public DataTable ImportXMLToDatatable(string XMLFileName)
        {
            try
            {
                  string xmlFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Configuration\\" + XMLFileName + ".xml";
                  DataSet ds = new DataSet();
                  ds.ReadXml(xmlFilePath);
                  return ds.Tables[0];
            }
            catch (Exception)
            {                
                return null;
            }
        }

        public string GetConnectionString()
        {
            try
            {
                string constring = "";
                DataTable dt = ImportXMLToDatatable(StaticVariables.DataBaseConnectionFile);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow rw in dt.Rows)
                    {
                        constring = dt.Rows[0]["ConnectionString"].ToString();
                        break;
                    }
                }
                return objaes.GeneratePlainText(constring, StaticVariables.PublicKeyEncryption);  
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
