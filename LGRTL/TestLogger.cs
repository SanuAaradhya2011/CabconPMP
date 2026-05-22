using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
namespace CabconRTL
{
    public class TestLogger
    {
        // member variables declare here
        private int mintrunid = -1;
        private string mstrtestprocstepno = string.Empty;
        private string mstrtestprocname = string.Empty;
        private string mstrlowerlimit = string.Empty;
        private string mstrupperlimit = string.Empty;
        private string mstrtestname = string.Empty;
        private string mstrtestid = "0";
        private string mstrmetertype="";
        string duplicatePCBAIDList = "";
        List<CommonEntity> mserverdatalist = new List<CommonEntity>();
        ConnectionClass objconn = new ConnectionClass();
        bool isMTE = false;
        Version appversion;
              /// <summary>
        /// Construtor
        /// </summary>
        public TestLogger()
        {
            appversion = Assembly.GetEntryAssembly().GetName().Version;
             
                do
                {                   
                    ExecuteTestLogger(mintrunid);
                    mintrunid--;

               } while (mintrunid >= mintrunid + 1);   //------------For Missing Data Upload set the Min Run ID And Condition is : while (mintrunid == 21397 for specefic run id);
               // } while (mintrunid == 3272);
            
        }

        public void ExecuteTestLogger(int runID)
        {
            isMTE = false;
            if (AppDomain.CurrentDomain.BaseDirectory.ToUpperInvariant().Contains("CCS")) isMTE = true;
            string loggerMessage="";
            duplicatePCBAIDList = "Duplicate PCBAID :";
            try
            {
                Console.Clear();
                AESEncryption objaes = new AESEncryption();
                System.Console.WriteLine("              LG Rouiten Test Logger V" + objaes.ProductVersion());
                System.Console.WriteLine("\n=============================================================\n");
                string srverLabel = GetServer();
                System.Console.WriteLine(srverLabel);

                if (srverLabel.IndexOf("Error") >= 0)
                {
                    System.Console.WriteLine("\n\nPress Any Key To Exit !");
                    System.Console.ReadKey();
                    return;

                }
                System.Console.WriteLine("\n=============================================================\n");
                if (!GetRunID(runID))
                {
                    System.Console.WriteLine("\n\nError : Unable to Get Test Run ID ! \nPress Any Key To Exit !");
                    System.Console.ReadKey();
                    return;
                }

                if (!GetMyprocedure())
                {
                    //--------------------In Case of MTE Test Bench , Ignore Below Message & Continue-----------------------------
                    if (!isMTE)
                    {
                        System.Console.WriteLine("\n\nError : Unable to Get Procedure From Selected Run ID !  \nPress Any Key To Exit !");
                        System.Console.ReadKey();
                        return;
                    }
                }
                mserverdatalist = new List<CommonEntity>();
                if (!GetResults())
                {
                    System.Console.WriteLine("\n\nError : Unable To Get Test Results From Source Database ! \nPress Any Key To Exit !");
                    System.Console.ReadKey();
                    return;
                }
                DataTable dt = new DataTable();
                CommonEntity objtemp = new CommonEntity();
                System.Console.WriteLine("\n\nUploading Test Results To Server !\nPlease wait...........");
                for (int idCnt = 0; idCnt <= 5; idCnt++)
                {
                    if (mserverdatalist.Count <= idCnt) break;
                    objtemp = mserverdatalist[idCnt];
                    dt = objconn.Select_GetExecutionResult_onPCBAID(objtemp);
                    if (dt != null && dt.Rows.Count > 0) break;
                }
                if (dt == null)
                {
                    System.Console.WriteLine("\n\nError : Unable To Create Server Structure For Result Database ! \nPress Any Key To Exit !");
                    System.Console.ReadKey();
                    return;
                }
                if (dt.Rows.Count < 1)
                {
                    frmUserAction objaction = new frmUserAction();
                    if (objaction.ShowDialog() != System.Windows.Forms.DialogResult.Yes) return;
                    mstrmetertype = objaction.MeterType;
                }
                else mstrmetertype = dt.Rows[0]["MeterType"].ToString();
                DataTable dtcali = GetCalibrationdata();
                if (dtcali == null)
                {
                    System.Console.WriteLine("\n\nError : Unable To Create Server Structure For Calibration Test Database ! \nPress Any Key To Exit !");
                    System.Console.ReadKey();
                    return;
                }
                DataTable dtexec = GetExecutionResultdata(dt);

                if (dtexec == null)
                {
                    System.Console.WriteLine("\n\nError : Unable To Create Server Structure For Result Database ! \nPress Any Key To Exit !");
                    System.Console.ReadKey();
                    return;
                }


                objtemp.MeterType = dtexec.Rows[0]["MeterType"].ToString();
                //===========================Fail Meters Warning Display Message=================================
                DataRow[] drFailMeters = dtexec.Select("FinalStatus='FAIL'");              
                if (drFailMeters.Length > 0)
                {
                    int meterCount = 1;
                    string metersList = "Below Meters are Fail at Calibration Stage !" + "\n" ;
                    metersList += "-------------------------------------------------" + "\n";
                   
                    foreach (DataRow row in drFailMeters)
                    {
                        metersList += (meterCount).ToString("d2") + ": " + row[1] + ", ";
                        if (meterCount++ % 5 == 0) metersList += "\n";
                    }
                    System.Windows.Forms.MessageBox.Show("\n" + metersList, "L+G RTL", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                if (!objconn.UploadRoutineTestResultd(objtemp, dtexec, dtcali))
                {
                    System.Console.WriteLine("\n\nError : Unable To Upload Test Results To Server ! \nPress Any Key To Exit !");
                    System.Console.ReadKey();
                    return;

                }
                int effectedRowsCount = dtexec.Rows.Count;
                loggerMessage = dtcali.Rows[0]["ExecutionDate"] + "--->CabconPMP:" + appversion.ToString() + ", Run ID: " + dtexec.Rows[0]["ParaDefaultValue"] + ", Work Station ID :" + dtexec.Rows[0]["WorkStationID"] + ", Uploaded Meter Counts :" + effectedRowsCount.ToString() + ", " + duplicatePCBAIDList;
                System.Console.WriteLine("\n\nTotal Uploaded Meter Counts : " + effectedRowsCount.ToString());
               
                //System.Threading.Thread.Sleep(2000);
                System.Console.WriteLine("\n\nData Uploaded Successfully !");
                // System.Console.ReadKey();
            }
            catch (Exception Ex)
            {
                string dattimeFormatted = DateTime.Now.Day.ToString("d2") + "/" + DateTime.Now.Month.ToString("d2") + "/" + DateTime.Now.Year.ToString("d2") + " " + DateTime.Now.Hour.ToString("d2") + ":" + DateTime.Now.Minute.ToString("d2") + ":" + DateTime.Now.Second.ToString("d2");
                loggerMessage = dattimeFormatted + "--->CabconPMP:" + appversion.ToString() + ", " + Ex.Message;
                System.Console.WriteLine("\n\nError : Unable To Upload Test Results To Server ! \n " + Ex.Message + "\nPress Any Key To Exit !");
                System.Console.ReadKey();
            }
            finally
            {
              
                WriteTextLog(loggerMessage);
            }

        }
        
        public static void WriteTextLog(string msg)
        {
            try
            {
              
                using (StreamWriter w = File.AppendText("LGRTLlog.txt"))
                {
                    LogToFile(msg, w);
                }
            }
            catch (Exception)
            {
            }

        }

        public static void LogToFile(string logMessage, TextWriter w)
        {
             w.WriteLine(logMessage);     
            
        }

        /// <summary>
        /// Method Name : GetRunID
        /// Purpose     : To get test ID/run id and test procedure name from the Aemcal database and display in console
        /// Date        : 20-01-2015
        /// Author      : Mohsin Raza
        /// </summary>
        /// <returns></returns>
        private bool GetRunID(int runID)
        {
            try
            {
                //runID = 3272;//--------------For Debuging only
                string strcmd ="";
                if (runID <= 0) strcmd = "select RunID, Name from Run where RunID=(select max(RunID) from Run)";
                else strcmd = strcmd = "select RunID, Name from Run where RunID=" + runID ;
                //string strcmd = "select Max(RunID) from Run;";
                DataTable dttemp = GetAemCalData(strcmd);
                if (dttemp == null || dttemp.Rows.Count < 1) return false;
                mintrunid = int.Parse(dttemp.Rows[0][0].ToString());                
                mstrtestid = dttemp.Rows[0][1].ToString();
                System.Console.WriteLine("RunID: " + mintrunid);
                if (isMTE) System.Console.WriteLine("Procedure Name: " + "MTE156POSITION");
                else System.Console.WriteLine("Procedure Name: " + dttemp.Rows[0][1]);
                return true;
            }
            catch (Exception ex)
            {
                string dattimeFormatted = DateTime.Now.Day.ToString("d2") + "/" + DateTime.Now.Month.ToString("d2") + "/" + DateTime.Now.Year.ToString("d2") + " " + DateTime.Now.Hour.ToString("d2") + ":" + DateTime.Now.Minute.ToString("d2") + ":" + DateTime.Now.Second.ToString("d2");
                string loggerMessage = dattimeFormatted + "--->CabconPMP:" + appversion.ToString() + ", " + ex.Message;
                WriteTextLog(loggerMessage);
                System.Console.WriteLine(ex.Message);
                //System.Console.ReadKey();
                return false;

            }
            
        }

        /// <summary>
        /// Method Name     : GetResults()
        /// Purpose         : get meter data and result data from the Aemcal database and prepare the final data for main database
        /// Date            : 20-01-2015
        /// Author          : Mohsin Raza  
        /// </summary>
        /// <returns>boolean</returns>
        private bool GetResults()
        {
            try
            {
                // initialise local variables here
                string strresults = string.Empty;
                string strvalues = string.Empty;
                string strmetername = string.Empty;

                // get meterdetail data from aemcal database based on the RunID
                DataTable dtMeterDetails = GetMeterdetails();

                // return false if table is null or empty
                if (dtMeterDetails == null || dtMeterDetails.Rows.Count < 1) return false;
                //------------------If test bench is MTE then Set Default test ID Name------------------
                if (isMTE) {mstrtestid = "MTE-" + string.Format("{0:yyyyMMddhhmmss}", DateTime.Now); }
                // get the current date time for timestamp
                DateTime executiondatetime = DateTime.Now;                
                // create a list of record for each meter based on Execution
                for (int icount = 0; icount < dtMeterDetails.Rows.Count; icount++)
                {
                    if (dtMeterDetails.Rows[icount]["MSN"].ToString().Trim().Length < 5) continue;
                    string strfinalstatus = "FAIL";
                    strresults = "";

                    // create local object of CommonEntity
                    CommonEntity entityserverdata = new CommonEntity();

                    // get result data based on run id and postion number
                    string strcmd = "select  PositionNo,StepNo, RValue from RResult where RunID = " + mintrunid.ToString() + " AND PositionNo = " + dtMeterDetails.Rows[icount][1].ToString() + " Order by StepNo ASC";
                    DataTable dttemp = GetAemCalData(strcmd);
                    strvalues = "";
                    // prepare a list of serverdata
                    for (int i = 0; i < dttemp.Rows.Count; i++)
                    {
                        strvalues += dttemp.Rows[i]["RValue"].ToString() + ",";

                        if (dttemp.Rows[i]["RValue"].ToString().Trim().Length <=0 || dttemp.Rows[i]["RValue"].ToString().Contains(">") || dttemp.Rows[i]["RValue"].ToString().Contains("<") || dttemp.Rows[i]["RValue"].ToString().ToUpperInvariant().Contains("FAIL"))
                            strresults += "FAIL";
                        else if (dttemp.Rows[i]["RValue"].ToString().Length < 1)
                            strresults += "";
                        else
                            strresults += "PASS";

                        strresults += ",";
                    }

                    // in contains in out of range indication, put Fail otherwise put Pass
                    if (strresults.ToUpperInvariant().Contains("FAIL"))strfinalstatus = "FAIL";
                    else if (strresults.Trim().Length <=0) strfinalstatus = "FAIL";
                    else strfinalstatus = "PASS";
                    //-------------In Case of MTE No Error is Logged by CCS, Only Pass Meters is available in Table RResult, set default as Pass-------------
                    if (isMTE) { strfinalstatus = "PASS"; }
                    // fieled are updated 
                    entityserverdata.MeterType = mstrmetertype;//dtMeterDetails.Rows[0]["MeterName"].ToString();
                    entityserverdata.MeteriD = " ";
                    entityserverdata.PCBAID = dtMeterDetails.Rows[icount]["MSN"].ToString();
                    entityserverdata.ExecutionStatus = strresults;
                    entityserverdata.ProcedureType = "Calibration"; //mstrtestprocname;
                    entityserverdata.TestID = mstrtestid;
                    entityserverdata.ProgramName = AppDomain.CurrentDomain.FriendlyName;
                    entityserverdata.ParametersName = mstrtestprocname;
                    entityserverdata.ParametersDefaultValue = dtMeterDetails.Rows[icount]["RunID"].ToString();
                    entityserverdata.Max = mstrupperlimit;
                    entityserverdata.Min = mstrlowerlimit;
                    entityserverdata.ExecutionRemarks = strvalues;
                    entityserverdata.ExecutionStatus = strresults;
                    entityserverdata.ExecutionDate = executiondatetime;
                    entityserverdata.CustomerName = dtMeterDetails.Rows[icount]["ClientName"].ToString();
                    entityserverdata.FinalStatus = strfinalstatus;
                    entityserverdata.UpdateStatus = executiondatetime;
                    entityserverdata.WorkstationID = Environment.MachineName;
                    entityserverdata.logedUserID = Environment.UserName;
                    mserverdatalist.Add(entityserverdata);
                }

                // return true
                return true;
            }
            catch (Exception ex)
            {
                string dattimeFormatted = DateTime.Now.Day.ToString("d2") + "/" + DateTime.Now.Month.ToString("d2") + "/" + DateTime.Now.Year.ToString("d2") + " " + DateTime.Now.Hour.ToString("d2") + ":" + DateTime.Now.Minute.ToString("d2") + ":" + DateTime.Now.Second.ToString("d2");
                string loggerMessage = dattimeFormatted + "--->CabconPMP:" + appversion.ToString() + ", " + ex.Message;
                WriteTextLog(loggerMessage);
                System.Console.WriteLine(ex.Message);
                //System.Console.ReadKey();
                return false;
            }
        }

        /// <summary>
        /// Method Name     : getmeterdetails()
        /// Purpose         : Get meter details from Aemcal database based on runid
        /// Date            : 20-01-15
        /// Author          : Mohsin Raza
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable GetMeterdetails()
        {
            try
            {
                string strcmd = "select * from RMeter where RunID = " + mintrunid.ToString();

                DataTable dttemp = GetAemCalData(strcmd);

                if (dttemp == null || dttemp.Rows.Count < 1) return null;

                return dttemp;
            }
            catch (Exception ex)
            {
                string dattimeFormatted = DateTime.Now.Day.ToString("d2") + "/" + DateTime.Now.Month.ToString("d2") + "/" + DateTime.Now.Year.ToString("d2") + " " + DateTime.Now.Hour.ToString("d2") + ":" + DateTime.Now.Minute.ToString("d2") + ":" + DateTime.Now.Second.ToString("d2");
                string loggerMessage = dattimeFormatted + "--->CabconPMP:" + appversion.ToString() + ", " + ex.Message;
                WriteTextLog(loggerMessage);
                System.Console.WriteLine(ex.Message);
                //System.Console.ReadKey();
                return null;
            }

        }

        /// <summary>
        /// Method Name : Getmyprocedure()
        /// Purpose     : get the procedure names, Upper and Lower limits and combined with comma seperated for the server use based on run id
        /// Date        : 20-01-15
        /// Author      : Mohsin Raza
        /// </summary>
        /// <returns></returns>
       
        private bool GetMyprocedure()
        {
            try
            {
                //initialise local variables here
               
                string strtestprocname = "";
                string strlowerlimit = "";
                string strupperlimit = "";

                // get data from Aemcal database
                string strcmd = "select StepNo, Name, ULIMIT, LLIMIT from RStep where RunID = " + (mintrunid).ToString() + " Order by StepNo ASC";
                DataTable dttemp = GetAemCalData(strcmd);
                strcmd = "select distinct StepNo  from RResult where RunID = " + (mintrunid).ToString() + " Order by StepNo ASC";
                DataTable dttempresult = GetAemCalData(strcmd);

                // if teable is null or empty return false
                if (dttemp == null || dttemp.Rows.Count < 1) return false;
                if (dttempresult == null || dttempresult.Rows.Count < 1) return false;
                
                    // prepare test procedures, upper and lower limits
                foreach (DataRow rw in dttempresult.Rows)
                {
                     
                        var selectedMeterTest = (from myRow in dttemp.AsEnumerable()
                                                 where myRow.Field<Int16>("StepNo") == Convert.ToInt16(rw[0]) 
                                                 select myRow).ToList();
                        if (selectedMeterTest.Count < 1) continue;
                        if (selectedMeterTest[0].ItemArray[2].ToString().Length < 1) continue;



                        strtestprocname += selectedMeterTest[0].ItemArray[1].ToString() + ",";
                        strlowerlimit += selectedMeterTest[0].ItemArray[3].ToString() + ",";
                        strupperlimit += selectedMeterTest[0].ItemArray[2].ToString() + ",";
                        
                    }
                   
                    mstrtestprocname = strtestprocname;
                    mstrupperlimit = strupperlimit;
                    mstrlowerlimit = strlowerlimit;
                
                return true;
            }
            catch (Exception ex)
            {
                string dattimeFormatted = DateTime.Now.Day.ToString("d2") + "/" + DateTime.Now.Month.ToString("d2") + "/" + DateTime.Now.Year.ToString("d2") + " " + DateTime.Now.Hour.ToString("d2") + ":" + DateTime.Now.Minute.ToString("d2") + ":" + DateTime.Now.Second.ToString("d2");
                string loggerMessage = dattimeFormatted + "--->CabconPMP:" + appversion.ToString() + ", " + ex.Message;
                WriteTextLog(loggerMessage);
                System.Console.WriteLine(ex.Message);
                //System.Console.ReadKey();
                return false;
            }
        }

      
        /// <summary>
        /// Method Name : InsertExecutionResultdata()
        /// Purpose     : insert ExecutionResultdata to the server
        /// Date        : 20-01-15  
        /// Author      : Mohsin Raza
        /// </summary>
        /// <returns>boolean</returns>
        private DataTable GetExecutionResultdata(DataTable dt)
        {
            
            try
            {
                CommonEntity objtemp = mserverdatalist[0];     
                DataTable mydt = dt.Clone();
                mydt.Clear();
                int datarCnt = 0;  
                for (int icount = 0; icount < mserverdatalist.Count; icount++)
                {
                    objtemp = mserverdatalist[icount];
                    //-----------------------Filtering Duplicate Data------------------------------------
                    var selectedMeterTest = (from myRow in mydt.AsEnumerable()
                                             where myRow.Field<string>("PCBAID") == objtemp.PCBAID.Trim()
                                             select myRow).ToList();
                    //--------------------------If PCBAID Already Exist No Need to add Duplicate ID-----
                    if (selectedMeterTest.Count >= 1) { duplicatePCBAIDList += objtemp.PCBAID.Trim() + ","; continue; } 
                    //-------------------------------------------------------------------------------------
                    objtemp.Max = "";
                    objtemp.Min = "";
                    objtemp.ParametersName = "";
                    objtemp.ExecutionRemarks = "";
                    objtemp.ExecutionStatus = "";
                    mydt.Rows.Add();
                    mydt.Rows[datarCnt]["PCBAID"] = objtemp.PCBAID.Trim();
                    mydt.Rows[datarCnt]["MeterType"] = mstrmetertype;
                    mydt.Rows[datarCnt]["ProcedureType"] = objtemp.ProcedureType;
                    mydt.Rows[datarCnt]["TestID"] = objtemp.TestID;
                    mydt.Rows[datarCnt]["ProgramName"] = objtemp.ProgramName;
                    mydt.Rows[datarCnt]["ParametersName"] = objtemp.ParametersName;
                    mydt.Rows[datarCnt]["ParaDefaultValue"] = objtemp.ParametersDefaultValue;
                    mydt.Rows[datarCnt]["ParaMinValue"] = objtemp.Min;
                    mydt.Rows[datarCnt]["ParaMaxValue"] = objtemp.Max;
                    mydt.Rows[datarCnt]["ExecutionRemarks"] = objtemp.ExecutionRemarks;
                    mydt.Rows[datarCnt]["ExecutionStatus"] = objtemp.ExecutionStatus;
                    mydt.Rows[datarCnt]["executionDate"] = objtemp.ExecutionDate;
                    mydt.Rows[datarCnt]["CustomerName"] = objtemp.CustomerName;
                    mydt.Rows[datarCnt]["FinalStatus"] = objtemp.FinalStatus;
                    mydt.Rows[datarCnt]["UpdateStatus"] = objtemp.UpdateStatus;
                    mydt.Rows[datarCnt]["WorkStationID"] = objtemp.WorkstationID;
                    mydt.Rows[datarCnt]["LogedUserID"] = objtemp.logedUserID;
                    mydt.Rows[datarCnt]["LatestStatus"] = 1;
                    datarCnt++;
                }
                return mydt;
            }
            catch (Exception ex)
            {
                string dattimeFormatted = DateTime.Now.Day.ToString("d2") + "/" + DateTime.Now.Month.ToString("d2") + "/" + DateTime.Now.Year.ToString("d2") + " " + DateTime.Now.Hour.ToString("d2") + ":" + DateTime.Now.Minute.ToString("d2") + ":" + DateTime.Now.Second.ToString("d2");
                string loggerMessage = dattimeFormatted + "--->CabconPMP:" + appversion.ToString() + ", " + ex.Message;
                WriteTextLog(loggerMessage);
                System.Console.WriteLine("\nError: " + ex.Message + "!");
                System.Console.WriteLine("Unable To Connect To Server !");
                //System.Console.ReadKey();
                return null;
            }
            
        }

        /// <summary>
        /// Method Name : insertCalibrationdata()
        /// Purpose     : insert CalibrationData to the server
        /// Date        : 20-01-15  
        /// Author      : Mohsin Raza
        /// </summary>
        /// <returns>boolean</returns>
        private DataTable GetCalibrationdata()
        {
            
            try
            {
                DataTable dt = objconn.GetCalibrationDataTable();

                DataTable mydt = dt.Clone();
                mydt.Clear();
                int datarCnt = 0;
                CommonEntity objtemp = mserverdatalist[0];
                for (int icount = 0; icount < mserverdatalist.Count; icount++)
                {
                    objtemp = mserverdatalist[icount];
                    //-----------------------Filtering Duplicate Data------------------------------------
                    var selectedMeterTest = (from myRow in mydt.AsEnumerable()
                                             where myRow.Field<string>("PCBAID") == objtemp.PCBAID.Trim()
                                             select myRow).ToList();
                    //--------------------------If PCBAID Already Exist No Need to add Duplicate ID-----
                    if (selectedMeterTest.Count >= 1)  continue; 
                    //-------------------------------------------------------------------------------------
                   
                    mydt.Rows.Add();
                    mydt.Rows[datarCnt]["PCBAID"] = objtemp.PCBAID.Trim();
                    mydt.Rows[datarCnt]["ExecutionDate"] = objtemp.ExecutionDate;
                    mydt.Rows[datarCnt]["ParamList"] = objtemp.ParametersName;
                    mydt.Rows[datarCnt]["ResultList"] = objtemp.ExecutionRemarks;
                    mydt.Rows[datarCnt]["UpperLimit"] = objtemp.Max;
                    mydt.Rows[datarCnt]["LowerLimit"] = objtemp.Min;
                    mydt.Rows[datarCnt]["MeterType"] = mstrmetertype;
                    mydt.Rows[datarCnt]["ProcedureType"] = objtemp.ProcedureType;
                    mydt.Rows[datarCnt]["LatestStatus"] = 1;
                    mydt.Rows[datarCnt]["TestID"] = objtemp.TestID;
                    datarCnt++;
                }

                return mydt;

            }
            catch (Exception ex)
            {
                string dattimeFormatted = DateTime.Now.Day.ToString("d2") + "/" + DateTime.Now.Month.ToString("d2") + "/" + DateTime.Now.Year.ToString("d2") + " " + DateTime.Now.Hour.ToString("d2") + ":" + DateTime.Now.Minute.ToString("d2") + ":" + DateTime.Now.Second.ToString("d2");
                string loggerMessage = dattimeFormatted + "--->CabconPMP:" + appversion.ToString() + ", " + ex.Message;
                WriteTextLog(loggerMessage);
                System.Console.WriteLine("\nError: " + ex.Message + "!");
                System.Console.WriteLine("Unable To Connect To Server !");
                //System.Console.ReadKey();
                return null;
            }
            
        }

        //////////// AemCal DB Commands /////////////

        private DataTable GetAemCalData(string strcmd)
        {
            DataTable dt = new DataTable();

            try
            {
                objconn.OpenConn();
                OleDbCommand Comm = new OleDbCommand(strcmd, objconn.Connection);
                OleDbDataAdapter Adap = new OleDbDataAdapter(Comm);
                Adap.Fill(dt);
                objconn.CloseConn();
            }
            catch (Exception ex)
            {
                string dattimeFormatted = DateTime.Now.Day.ToString("d2") + "/" + DateTime.Now.Month.ToString("d2") + "/" + DateTime.Now.Year.ToString("d2") + " " + DateTime.Now.Hour.ToString("d2") + ":" + DateTime.Now.Minute.ToString("d2") + ":" + DateTime.Now.Second.ToString("d2");
                string loggerMessage = dattimeFormatted + "--->CabconPMP:" + appversion.ToString() + ", " + ex.Message;
                WriteTextLog(loggerMessage);
                System.Console.WriteLine(ex.Message);
                //System.Console.ReadKey();
                return null;
            }
            finally
            {
                objconn.CloseConn();

            }

            return dt;

        }

        private string GetServer()
        {
            string lblServerLocation = "";
            XMLExportImport objixml = new XMLExportImport();
            string connectionString = objixml.GetConnectionString();
            if (connectionString.IndexOf("INDELNB") >= 0) lblServerLocation    = "              Server Location : Local System";
            else if (connectionString.IndexOf("INDEL") >= 0) lblServerLocation = "              Server Location : GDC Noida";
            else if (connectionString.IndexOf("INBDI") >= 0) lblServerLocation = "              Server Location : Baddi Factory";
            else if (connectionString.IndexOf("INCCU") >= 0) lblServerLocation = "              Server Location : Joka Factory";
            else lblServerLocation = "Error : Data Base Server Not Found ! \n\n        Please Contact Your Administrator !";
            lblServerLocation += " ( " + connectionString.Substring(connectionString.IndexOf('=') + 1, connectionString.IndexOf(';') - connectionString.IndexOf('=') - 1) + " )"; 

            return lblServerLocation;
        }
    }
}
