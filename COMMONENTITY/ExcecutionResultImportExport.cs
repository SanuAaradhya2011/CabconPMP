/*  FlexibleMessageBox – A flexible replacement for the .NET MessageBox

  *   - Initial Version
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Serialization;

namespace COMMONENTITY
{
    /// <summary>
    /// Create serializable copy of EntityExecutionResult
    /// </summary>
    [Serializable]
    public class SerializableEntityExecutionResult 
    {
        public SerializableEntityExecutionResult(EntityExecutionResult c)
        {
            foreach (PropertyInfo prop in c.GetType().GetProperties())
            {
                PropertyInfo prop2 = this.GetType().GetProperty(prop.Name);
                prop2.SetValue(this, prop.GetValue(c, null), null);
            }
            foreach (FieldInfo field in c.GetType().GetFields())
            {
                FieldInfo field2 = this.GetType().GetField(field.Name);
                field2.SetValue(this, field.GetValue(c));
            }
        }

        public SerializableEntityExecutionResult()
        {
        }

        string meterID;
        string pcbaID;
        string exemeterType;
        string exeprocedureType;
        string exetestID;
        string exeprogramName;
        string exeparametersName;
        string exeparaDefaultValue;
        string exeparaMinValue;
        string exeparaMaxValue;
        string executionRemarks;
        string executionStatus;
        DateTime executionDate;
        DateTime executionStatusStart;
        DateTime executionstatusEnd;
        string executionStatusStartpcba;
        string executionstatusEndpcba;
        string customerName;
        string finalResult;
        string workStationID;
        string logedUserID;
        int latestStatus;

        string missingMeterRangeFrom;
        string missingMeterRangeTo;
        string missingMeterAlphaFileld;

        public string MeterID
        {
            get { return meterID; }
            set { meterID = value; }
        }
        public string PCBAID
        {
            get { return pcbaID; }
            set { pcbaID = value; }
        }
        public string ExecutionMeterType
        {
            get { return exemeterType; }
            set { exemeterType = value; }
        }
        public string ExecutionProcedureType
        {
            get { return exeprocedureType; }
            set { exeprocedureType = value; }
        }
        public string ExecutionTestID
        {
            get { return exetestID; }
            set { exetestID = value; }
        }
        public string ExecutionProgramName
        {
            get { return exeprogramName; }
            set { exeprogramName = value; }
        }
        public string ExecutionParametersName
        {
            get { return exeparametersName; }
            set { exeparametersName = value; }
        }
        public string ExecutionParaDefaultValue
        {
            get { return exeparaDefaultValue; }
            set { exeparaDefaultValue = value; }
        }
        public string ExecutionParaMinValue
        {
            get { return exeparaMinValue; }
            set { exeparaMinValue = value; }
        }
        public string ExecutionParaMaxValue
        {
            get { return exeparaMaxValue; }
            set { exeparaMaxValue = value; }
        }
        public string ExecutionRemarks
        {
            get { return executionRemarks; }
            set { executionRemarks = value; }
        }
        public string ExecutionStatus
        {
            get { return executionStatus; }
            set { executionStatus = value; }
        }
        public DateTime ExecutionDate
        {
            get { return executionDate; }
            set { executionDate = value; }
        }
        public DateTime ExecutionStatusStart
        {
            get { return executionStatusStart; }
            set { executionStatusStart = value; }
        }
        public DateTime ExecutionstatusEnd
        {
            get { return executionstatusEnd; }
            set { executionstatusEnd = value; }
        }
        public string ExecutionStatusStartPCBA
        {
            get { return executionStatusStartpcba; }
            set { executionStatusStartpcba = value; }
        }
        public string ExecutionstatusEndPCBA
        {
            get { return executionstatusEndpcba; }
            set { executionstatusEndpcba = value; }
        }
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }
        public string FinalResult
        {
            get { return finalResult; }
            set { finalResult = value; }
        }
        public string WorkStationID
        {
            get { return workStationID; }
            set { workStationID = value; }
        }
        public string LogedUserID
        {
            get { return logedUserID; }
            set { logedUserID = value; }
        }
        public int LatestStatus
        {
            get { return latestStatus; }
            set { latestStatus = value; }
        }
        public string MissingMeterRangeFrom
        {
            get { return missingMeterRangeFrom; }
            set { missingMeterRangeFrom = value; }
        }
        public string MissingMeterRangeTo
        {
            get { return missingMeterRangeTo; }
            set { missingMeterRangeTo = value; }
        }
        public string MissingMeterAlphaFileld
        {
            get { return missingMeterAlphaFileld; }
            set { missingMeterAlphaFileld = value; }
        }
    }

    /// <summary>
    /// A class containing methods to save and read data from execution data files
    /// </summary>
    public class ExcecutionResultImportExport
    {
        public static string saveDir = AppDomain.CurrentDomain.BaseDirectory + "Configuration\\ResultFilesToSync\\";    
        public static string archiveDir = AppDomain.CurrentDomain.BaseDirectory + "Configuration\\ResultFilesToSync\\Archive\\";
        public static string latestFilesDir = AppDomain.CurrentDomain.BaseDirectory + "Configuration\\ResultFilesToSync\\LatestFiles\\";
        public static string fileSeperator = "\r\n\r\n\r\n";   
        private List<string> fileList = new List<string>();

        public List<string> PendingFileList
        {
            get { return fileList; }
        }

        public ExcecutionResultImportExport()
        {
            Directory.CreateDirectory(saveDir);
            Directory.CreateDirectory(archiveDir);
            Directory.CreateDirectory(latestFilesDir);
            DirectoryInfo dirInfo = new DirectoryInfo(saveDir);
            foreach (FileInfo fileInfo in dirInfo.GetFiles("*.exxml"))
            {
                string path = fileInfo.FullName;
                int idxend = path.LastIndexOf('.');
                fileList.Add(path.Substring(0, idxend));
            }
        }

 
        /// <summary>
        /// Save to merged file EntityExecutionResult and DataTable executionResultClone
        /// </summary>
        /// <param name="objexeresult"></param>
        /// <param name="executionResultClone"></param>
        /// <returns></returns>
        public bool SaveToFileExcecutionDataMerge(EntityExecutionResult objexeresult, DataTable executionResultClone)
        {
            bool isSuccess = false;
            string savePathResult = "";
            string name = "";
            switch (objexeresult.ExecutionProcedureType)
            {
                case StaticVariables.TestType_SR:
                    name = objexeresult.MeterID;
                    break;
                case StaticVariables.TestType_EMS:
                case StaticVariables.TestType_FT:
                case StaticVariables.TestType_Cal:
                case StaticVariables.TestType_OT:
                    name = objexeresult.PCBAID;
                    break;
                default:
                    break;
            }
            try
            {
                SerializableEntityExecutionResult serialobj = new SerializableEntityExecutionResult(objexeresult);

                XmlSerializer serializer = new XmlSerializer(typeof(SerializableEntityExecutionResult));
                savePathResult = String.Format("{0}{2}_{1}", saveDir, name, objexeresult.ExecutionProcedureType, objexeresult.ExecutionDate.ToString("yyyyMMddhhmm"));

                TextWriter writer = new StreamWriter(savePathResult + ".exxml");
                serializer.Serialize(writer, serialobj);


                writer.Write(fileSeperator);
                
                executionResultClone.WriteXml(writer, XmlWriteMode.WriteSchema);
                writer.Close();

                if (File.Exists(savePathResult + ".exxml"))
                    isSuccess = true;
            }

            catch (Exception ex)
            {
                isSuccess = false;
                DialogResult result = MessageBox.Show
                    (String.Format("Please Rescan Meter No.:{0}!\nCannot save Files for Meter due to error \n{1}", objexeresult.MeterID, ex.Message),
                    "Error saving results! Please rescan!",
                    MessageBoxButtons.OK);
            }
            finally
            {
                if (!isSuccess)
                {
                    File.Delete(savePathResult + ".exxml");
                    File.Delete(savePathResult + ".exrpt");
                }
            }
            return isSuccess;
        }

 
        /// <summary>
        /// Deserialize XML string to SerializableEntityExecutionResult
        /// </summary>
        /// <param name="xmlText"></param>
        /// <returns></returns>
        public SerializableEntityExecutionResult DeserializeExecutionReportObject(string[] xmlText)
        {
            SerializableEntityExecutionResult serialobj;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SerializableEntityExecutionResult));
                using (var reader = new StringReader(xmlText[0]))
                {
                    serialobj = (SerializableEntityExecutionResult)serializer.Deserialize(reader);
                }
                

                return serialobj;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DeserializeExecutionReportObject", MessageBoxButtons.OK);
                return null;
            }
        }


        /// <summary>
        /// Deserialize XML string to Datatable executionResultClone
        /// </summary>
        /// <param name="xmlText"></param>
        /// <returns></returns>
        public DataTable ImportExecutionResultTable(string[] xmlText)
        {
            
            try
            {
                DataTable importTable = new DataTable();
                
                importTable.ReadXml(new StringReader(xmlText[1]));

                return importTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ImportExecutionResultTable", MessageBoxButtons.OK);
                return null;
            }
        }
        /// <summary>
        /// Convert SerializableEntityExecutionResult to EntityExecutionResult
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public EntityExecutionResult ConverttoEntityExecutionResult(SerializableEntityExecutionResult c)
        {
            EntityExecutionResult exeresult = new EntityExecutionResult();

            foreach (PropertyInfo prop in c.GetType().GetProperties())
            {
                PropertyInfo prop2 = exeresult.GetType().GetProperty(prop.Name);
                prop2.SetValue(exeresult, prop.GetValue(c, null), null);
            }
            foreach (FieldInfo field in c.GetType().GetFields())
            {
                FieldInfo field2 = exeresult.GetType().GetField(field.Name);
                field2.SetValue(exeresult, field.GetValue(c));
            }
            return exeresult;
        }

        /// <summary>
        /// Delete Files of Given serial number
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        public bool DeleteRPTFile(string filePath)
        {
            try
            {
                File.Delete(filePath + ".exrpt");
                File.Delete(filePath + ".exxml");
                return true;
            }
            catch (Exception ex)
            {

                DialogResult result  = MessageBox.Show
                    (String.Format("Cannot Delete Files '{0}' at '{1}' due to error \n{2}", filePath.Substring(filePath.LastIndexOf("\\") + 1), saveDir, ex.Message),
                    "Unable To delete File! Retry or Delete Manually!",
                    MessageBoxButtons.RetryCancel);
                if (result == DialogResult.Retry)
                    return DeleteRPTFile(filePath);
                else
                    return false;
            }
        }

        


        /// <summary>
        /// Save Execution Data
        /// </summary>
        /// <param name="DGVParaLists"></param>
        /// <param name="executionResultClone"></param>
        /// <param name="objentityer"></param>
        /// <returns></returns>
        public bool SaveExecutionData(DataGridView DGVParaLists, DataTable executionResultClone, EntityExecutionResult objentityer)
        {
            executionResultClone.Clear();
            //---------------Fill Datatable--------------------
            //if (objentityer.PCBAID.Trim().Length < 5 || objentityer.PCBAID.Trim().Length > 16) return "Invalid PCBA ID :" + objentityer.PCBAID;
            int recCount = 0;
            DataRow dr;
            string executionRemark = "";
            while (recCount < DGVParaLists.Rows.Count)
            {
                if (DGVParaLists.Rows[recCount].Cells["colStatus"].Value.ToString().Length <= 0) break;
                dr = executionResultClone.NewRow();
                dr["MeterID"] = objentityer.MeterID;
                dr["PCBAID"] = objentityer.PCBAID;
                dr["MeterType"] = objentityer.ExecutionMeterType;
                dr["ProcedureType"] = objentityer.ExecutionProcedureType;
                dr["TestID"] = objentityer.ExecutionTestID;
                dr["ProgramName"] = objentityer.ExecutionProgramName;
                dr["ParametersName"] = DGVParaLists.Rows[recCount].Cells["colParaName"].Value.ToString();
                dr["ParaDefaultValue"] = DGVParaLists.Rows[recCount].Cells["colDefaultValue"].Value.ToString();
                dr["ParaMinValue"] = DGVParaLists.Rows[recCount].Cells["ColMinVal"].Value.ToString();
                dr["ParaMaxValue"] = DGVParaLists.Rows[recCount].Cells["ColMaxValue"].Value.ToString();
                executionRemark = DGVParaLists.Rows[recCount].Cells["colRemarks"].Value.ToString().Trim();
                if (executionRemark.Length >= 245) executionRemark = "** " + executionRemark.Substring(0, 244);
                dr["ExecutionRemarks"] = executionRemark.Trim();
                dr["ExecutionStatus"] = DGVParaLists.Rows[recCount].Cells["colStatus"].Value.ToString();
                dr["executionDate"] = objentityer.ExecutionDate;
                dr["CustomerName"] = objentityer.CustomerName;
                dr["FinalStatus"] = objentityer.FinalResult;
                dr["UpdateStatus"] = objentityer.ExecutionDate;
                dr["WorkStationID"] = System.Environment.MachineName;
                dr["LogedUserID"] = objentityer.LogedUserID;
                dr["LatestStatus"] = recCount + 1;

                if (DGVParaLists.Rows[recCount].Cells["colParaName"].Value == null) break;
                executionResultClone.Rows.Add(dr);
                recCount++;
            }
            //return SavetoFileExecutionData(objentityer, executionResultClone);
            return SaveToFileExcecutionDataMerge(objentityer, executionResultClone);
        }



        /// <summary>
        /// Log Sync Results
        /// </summary>
        /// <param name="resultText"></param>
        public void LogResults(StringBuilder resultText)
        {
            string logPath = archiveDir + DateTime.Now.ToString("yyyyMMdd") + ".log";

            TextWriter writer = File.AppendText(logPath);
            writer.Write("\r\n----------------------------- ");
            writer.Write(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
            writer.Write(" -----------------------------\r\n");
            writer.WriteLine(resultText.ToString());
            writer.Close();
        }

        public FileInfo[] GetSyncLogList()
        {
            FileInfo[] filelist = new DirectoryInfo(archiveDir).GetFiles("*.log");
            return filelist;
        }
    }
}
