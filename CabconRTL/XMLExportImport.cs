using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace CabconRTL
{
    public class XMLExportImport
    {
        public const string DataBaseConnectionFile = "DbConnection";
        public bool ExportXMLFromDatatable(DataTable dtValue, string XMLFileName)
        {
            try
            {
                  dtValue.WriteXml(AppDomain.CurrentDomain.BaseDirectory + "\\" + XMLFileName + ".xml", XmlWriteMode.IgnoreSchema);
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
                  string xmlFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + XMLFileName + ".xml";
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
            AESEncryption objaes = new AESEncryption();
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
