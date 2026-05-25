using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using COMMONENTITY;
using System.IO;
using BALLAYER;
using System.Windows.Forms;
using ZipArchiveLibrary;

namespace CabconPMPSYNCSERVICE
{
    partial class PMPExSyncService : ServiceBase
    {
        private System.Threading.Timer IntervalTimer;
        private bool isAbort, isOccupied;
        object objlock = new object();

        public PMPExSyncService()
        {
            InitializeComponent();
        }

        public void StartService()
        {
            OnStart(new string[] { });
        }


        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.

            isAbort = false;
            ChangeLock(false);
            Do_Work(new object());
            TimeSpan tsInterval = new TimeSpan(1, 0, 0);
            IntervalTimer = new System.Threading.Timer(new System.Threading.TimerCallback(Do_Work), null, tsInterval, tsInterval);
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            isAbort = true;
            ChangeLock(false);
            IntervalTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            IntervalTimer.Dispose();
            IntervalTimer = null;
        }

       

        private void Do_Work(object state)
        {
            if (!isOccupied)
            {
                ChangeLock(true);
                SyncRemainingExecutionReportsServ();
                ChangeLock(false);
            }
        }

        private void ChangeLock(bool value)
        {
            lock (objlock)
            {
                isOccupied = value;
            }
        }

        /// <summary>
        /// Returns Result String after Syncing Remaining Execution Result files
        /// </summary>
        /// <returns></returns>
        public string SyncRemainingExecutionReportsServ()
        {


            ExcecutionResultImportExport exeRptImportExport = new ExcecutionResultImportExport();
            BALExecutionResults objexere = new BALExecutionResults();
            string resultmsg = "";
            StringBuilder resultText = new StringBuilder("Batch Sync operation report:\r\n\r\n");
            int pos = resultText.Length;
            int count = 0;
            bool bLatestFile = false;
            foreach (var filePath in exeRptImportExport.PendingFileList)
            {
                if (isAbort)
                    return resultText.ToString();
                string sourceFilePath = filePath + ".exxml";
                string dataText = "";
                string[] strarray;
                try
                {
                    resultText.Append(" - File : " + filePath.Substring(filePath.LastIndexOf("\\") + 1) + " ");
                    count++;
                    dataText = File.ReadAllText(sourceFilePath);
                    strarray = dataText.Split(new string[] { ExcecutionResultImportExport.fileSeperator }, StringSplitOptions.None);

                    DataTable executionResultClone = null;

                    SerializableEntityExecutionResult serialObjExeResult = exeRptImportExport.DeserializeExecutionReportObject(strarray);

                    if (serialObjExeResult == null)
                    {
                        resultText.Append("cannot be read. Please Retry!\r\n");
                        continue;
                    }
                    EntityExecutionResult objexeresult = exeRptImportExport.ConverttoEntityExecutionResult(serialObjExeResult);

                    DataSet ds = objexere.Select_GetExecutionResult_onPCBAID_ProType_ExeDate(objexeresult);

                    string archiveFilePath = ExcecutionResultImportExport.archiveDir + DateTime.Now.ToString("yyyyMMdd") + ".zip";
                    string latestFilePath = ExcecutionResultImportExport.latestFilesDir + sourceFilePath.Substring(sourceFilePath.LastIndexOf("\\") + 1);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ArchivalMethods.ArchiveAppendFile(sourceFilePath,archiveFilePath);
                        File.Copy(sourceFilePath, latestFilePath, true);
                        File.Delete(sourceFilePath);
                        resultText.Append("already exists in DataBase.\r\n");
                        continue;
                    }
                    else
                    {
                        executionResultClone = exeRptImportExport.ImportExecutionResultTable(strarray);

                        objexere.InsertinToTabExecutionResultsPending(executionResultClone, objexeresult);
                        resultmsg = objexere.InsertinToTabExecutionResultsPending(executionResultClone, objexeresult);

                        ds = objexere.Select_GetExecutionResult_onPCBAID_ProType_ExeDate(objexeresult);

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            resultText.Append("was uploaded successfully to DataBase.\r\n");
                            if (!bLatestFile)
                            {
                                bLatestFile = true;
                                FileInfo[] arrFiles = new DirectoryInfo(ExcecutionResultImportExport.latestFilesDir).GetFiles("*.exxml");
                                foreach (FileInfo file in arrFiles)
                                {
                                    if (file.CreationTime.Date < DateTime.Now.Date)
                                        file.Delete();
                                }
                            }
                            ArchivalMethods.ArchiveAppendFile(sourceFilePath, archiveFilePath);
                            File.Copy(sourceFilePath, latestFilePath, true);
                            File.Delete(sourceFilePath);
                        }
                        else
                        {
                            resultText.Append("was not uploaded. Retrying in 1 hr!\r\n");
                        }
                    }
                    
                }
                catch (Exception Ex)
                {
                    count++;
                    resultmsg = Ex.Message;
                    resultText.AppendLine("\n" + resultmsg);
                }
                finally
                {
                    if (resultmsg != "") CommonMethods.LogPMPMessage("Unable To Save Results : " + resultmsg);
                }
            }
            
            if (count > 0)
            {
                resultText.Insert(pos, String.Format(" - {0} Files Processed!\r\n\r\n", count));
                exeRptImportExport.LogResults(resultText);
            }
            return resultText.ToString();
        }
    }
    
}
