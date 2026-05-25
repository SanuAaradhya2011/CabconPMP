using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COMMONENTITY;
using DALLAYER;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using SystemSecurityLibrary;
using System.IO;
using System.Diagnostics;
namespace BALLAYER
{
    public class BALExecutionResults
    {
        CommonCommandMethods objccmdmethod = new CommonCommandMethods();
        DALGolbalAdapter objprodal = new DALGolbalAdapter();
        FileVersionInfo PMPVersion = FileVersionInfo.GetVersionInfo(AppDomain.CurrentDomain.BaseDirectory + @"\CabconPMP.exe");
        public DataSet VerifyExecutionStatusInDatabase(EntityExecutionResult objexeresult)
        {
            DataSet ds = new DataSet();
            
                if (objexeresult.ExecutionProcedureType.IndexOf(StaticVariables.TestType_SR) >= 0)
                {
                    string tempMsg = objccmdmethod.IsSerialIDinRange(objexeresult.PCBAID, objexeresult.ExecutionTestID);
                    if (tempMsg != "")
                    {
                        MessageBox.Show(tempMsg, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return null;
                    }
                   ds = Select_GetExecutionResult_onMeterIDandProcedureType(objexeresult);               
                }
                else ds = Select_GetExecutionResult_onPCBAIDandProcedureType(objexeresult);
                if (ds == null) { MessageBox.Show("Unable To Connect To DataBase/ Column Value Not Found !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop); return null; }

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) { if (MessageBox.Show("Meter Details Already Found in DataBase!" + "\n" + "Do You Want To Continue ??", "Cabcon PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return null; }
                
                return ds;                
            
            
        }

        public string InsertinToTabExecutionResults(DataGridView DGVParaLists, DataTable executionResultClone, EntityExecutionResult objentityer)
        {
            executionResultClone.Clear();
            //---------------Fill Datatable--------------------
            if (objentityer.PCBAID.Trim().Length < 5 || objentityer.PCBAID.Trim().Length > 16) return "Invalid PCBA ID :" + objentityer.PCBAID;
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
                dr["LogedUserID"] = objentityer.LogedUserID + " ," + SystemInfo.ProductVersion() + " ," + PMPVersion.FileVersion; //--Logged user // Meter specefic exe version // CabconPMP used version
                dr["LatestStatus"] = recCount + 1;

                if (DGVParaLists.Rows[recCount].Cells["colParaName"].Value == null) break;
                executionResultClone.Rows.Add(dr);
                recCount++;
            }
            //------Removing Duploicate Rows for debuging only , Not Required -----------------------
           // DataTable dt2 = executionResultClone.DefaultView.ToTable(true, "MeterType", "ProcedureType", "ParametersName", "executionDate", "LatestStatus");
            //if (dt2.Rows.Count < executionResultClone.Rows.Count) CommonMethods.LogPMPMessage("Removed Rows Count : " + dt2.Rows.Count.ToString() + ",  Orginal Rows Count : " + executionResultClone.Rows.Count.ToString());
            //--------------------------------Execute Procedure-----------------------------------
            SqlParameter[] param ={             
            new SqlParameter("@ExecutionResult", executionResultClone),              
            new SqlParameter("@PCBAID",objentityer.PCBAID),
            new SqlParameter("@MeterType",objentityer.ExecutionMeterType),
            new SqlParameter("@ProcedureType",objentityer.ExecutionProcedureType),           
            new SqlParameter("@executionDate",objentityer.ExecutionDate)              
           };
            return objprodal.ExecuteInsertQuery("Pro_Insert_tabExecutionResultBatch", param);
        }
         
        public DataSet SelectFromtabExecutionResult_TestID(EntityExecutionResult objentityer)
        {
            SqlParameter[] param = { new SqlParameter("@MeterType", objentityer.ExecutionMeterType)
                                     ,new SqlParameter("@TestID", objentityer.ExecutionTestID)
                                     ,new SqlParameter("@ProcedureType", objentityer.ExecutionProcedureType)
                                     ,new SqlParameter("@FinalStatus", objentityer.FinalResult)
                                     ,new SqlParameter("@LatestStatus", objentityer.LatestStatus)
                                   };
            return (objprodal.ExecuteDataSet("Pro_GetExecutionResult_LIKE_TestID", param));

        }

        public DataSet SelectFromtabExecutionResult_MissingMeterList(EntityExecutionResult objentityer)
        {
            SqlParameter[] param = { new SqlParameter("@MeterType", objentityer.ExecutionMeterType)
                                     ,new SqlParameter("@rangeFrom", objentityer.MissingMeterRangeFrom)
                                     ,new SqlParameter("@rangeTo", objentityer.MissingMeterRangeTo)
                                     ,new SqlParameter("@alphaFileld", objentityer.MissingMeterAlphaFileld)                                 
                                   };
            return (objprodal.ExecuteDataSet("Pro_GetMeterDetailsByMeterIDRange", param));

        }
        public DataSet SelectFromtabExecutionResult_ParaMeterWiseReports(EntityExecutionResult objentityer, EntityExecutionResult.SelectionType selectionType)
        {
            SqlParameter[] param = { new SqlParameter("@MeterType", objentityer.ExecutionMeterType)
                                     ,new SqlParameter("@rangeFrom", objentityer.MissingMeterRangeFrom)
                                     ,new SqlParameter("@rangeTo", objentityer.MissingMeterRangeTo)
                                     ,new SqlParameter("@alphaFileld", objentityer.MissingMeterAlphaFileld)     
                                      ,new SqlParameter("@ParametersName", objentityer.ExecutionParametersName) 
                                       ,new SqlParameter("@ProcedureType", objentityer.ExecutionProcedureType) 
                                       ,new SqlParameter("@FinalStatus", objentityer.FinalResult) 
                                   };
           if(selectionType == EntityExecutionResult.SelectionType.MeterID) return (objprodal.ExecuteDataSet("Pro_GetParametersByMeterIDRange", param));
           else if (selectionType == EntityExecutionResult.SelectionType.PCBAID) return (objprodal.ExecuteDataSet("Pro_GetParametersValuesByPCBAIDRange", param));
           else return (objprodal.ExecuteDataSet("Pro_GetParametersValuesByPCBAIDRange", param)); //---return PCBA based if noc ase found

        }
       
        public DataSet SelectFromtabExecutionResult_onDateRang_ProductStatus(EntityExecutionResult objentityer)
        {
             SqlParameter[] param = { new SqlParameter("@MeterType", objentityer.ExecutionMeterType)
                                     ,new SqlParameter("@startdt", objentityer.ExecutionStatusStart)
                                     ,new SqlParameter("@enddt", objentityer.ExecutionstatusEnd)                            
                                   };           
            return (objprodal.ExecuteDataSet("Pro_GetExecutionResult_onDateRange_ProductionStatus", param));
        }

        public DataSet SelectFromtabExecutionResult_onPCBARang_ProductStatus(EntityExecutionResult objentityer)
        {
            SqlParameter[] param = { new SqlParameter("@MeterType", objentityer.ExecutionMeterType)
                                     ,new SqlParameter("@pcbaIDValue", objentityer.ExecutionStatusStartPCBA)                                                    
                                   };
            return (objprodal.ExecuteDataSet("Pro_GetExecutionResult_onPCBARange_ProductionStatus", param));
        }

        public DataSet SelectFromtabExecutionResult_CustomerName(EntityExecutionResult objentityer)
        {
            SqlParameter[] param = { new SqlParameter("@MeterType", objentityer.ExecutionMeterType)
                                    ,new SqlParameter("@CustomerName", objentityer.CustomerName)
                                    ,new SqlParameter("@ProcedureType", objentityer.ExecutionProcedureType)
                                    ,new SqlParameter("@FinalStatus", objentityer.FinalResult)
                                    ,new SqlParameter("@LatestStatus", objentityer.LatestStatus)
                                   };
            return (objprodal.ExecuteDataSet("Pro_GetExecutionResult_LIKE_CustomerName", param));

        }
        public DataSet SelectFromtabExecutionResult_ExecutionDate(EntityExecutionResult objentityer)
        {
            SqlParameter[] param = {new SqlParameter("@MeterType", objentityer.ExecutionMeterType)                                  
                                   ,new SqlParameter("@ProcedureType", objentityer.ExecutionProcedureType)
                                   ,new SqlParameter("@FinalStatus", objentityer.FinalResult)
                                   ,new SqlParameter("@LatestStatus", objentityer.LatestStatus)
                                   ,new SqlParameter("@startdt", objentityer.ExecutionStatusStart)
                                   ,new SqlParameter("@enddt", objentityer.ExecutionstatusEnd)
                                   };
            return (objprodal.ExecuteDataSet("Pro_GetExecutionResult_LIKE_UpdatedDate", param));

        }
        public DataSet SelectFromtabExecutionResult_ParameterWise(EntityExecutionResult objentityer)
        {
            SqlParameter[] param = {new SqlParameter("@MeterType", objentityer.ExecutionMeterType)                                  
                                   ,new SqlParameter("@ParametersName", objentityer.ExecutionProgramName)
                                   ,new SqlParameter("@FinalStatus", objentityer.FinalResult)
                                   ,new SqlParameter("@LatestStatus", objentityer.LatestStatus)
                                   ,new SqlParameter("@startdt", objentityer.ExecutionStatusStart)
                                   ,new SqlParameter("@enddt", objentityer.ExecutionstatusEnd)
                                   ,new SqlParameter("@ProcedureType", objentityer.ExecutionProcedureType)
                                   };
            return (objprodal.ExecuteDataSet("Pro_GetExecutionResult_ParameterWise", param));

        }

        public DataSet Select_GetExecutionResult_LIKE_PCBAID(EntityExecutionResult objentityer)
        {
            SqlParameter[] param = { new SqlParameter("@MeterType", objentityer.ExecutionMeterType)
                                    ,new SqlParameter("@PCBAID", objentityer.PCBAID)
                                    ,new SqlParameter("@ProcedureType", objentityer.ExecutionProcedureType)
                                    ,new SqlParameter("@FinalStatus", objentityer.FinalResult)
                                    ,new SqlParameter("@LatestStatus", objentityer.LatestStatus)
                                   };
            return (objprodal.ExecuteDataSet("Pro_GetExecutionResult_LIKE_PCBAID", param));

        }

        public DataSet Select_GetExecutionResult_LIKE_MeterID(EntityExecutionResult objentityer)
        {
            SqlParameter[] param = { new SqlParameter("@MeterType", objentityer.ExecutionMeterType)
                                    ,new SqlParameter("@MeterID", objentityer.MeterID)
                                    ,new SqlParameter("@ProcedureType", objentityer.ExecutionProcedureType)
                                    ,new SqlParameter("@FinalStatus", objentityer.FinalResult)
                                    ,new SqlParameter("@LatestStatus", objentityer.LatestStatus)
                                   };
            return (objprodal.ExecuteDataSet("Pro_GetExecutionResult_LIKE_MeterID", param));

        }
        public DataSet Select_Backup_GetExecutionResult_LIKE_PCBAID(EntityExecutionResult objentityer)
        {
            SqlParameter[] param = { new SqlParameter("@MeterType", objentityer.ExecutionMeterType)
                                    ,new SqlParameter("@PCBAID", objentityer.PCBAID)
                                    ,new SqlParameter("@ProcedureType", objentityer.ExecutionProcedureType)
                                    ,new SqlParameter("@FinalStatus", objentityer.FinalResult)
                                    ,new SqlParameter("@LatestStatus", objentityer.LatestStatus)
                                   };
            return (objprodal.ExecuteDataSet("Pro_FromBackupTable_GetExecutionResult_LIKE_PCBAID", param));

        }

        public DataSet Select_Backup_GetExecutionResult_LIKE_MeterID(EntityExecutionResult objentityer)
        {
            SqlParameter[] param = { new SqlParameter("@MeterType", objentityer.ExecutionMeterType)
                                    ,new SqlParameter("@MeterID", objentityer.MeterID)
                                    ,new SqlParameter("@ProcedureType", objentityer.ExecutionProcedureType)
                                    ,new SqlParameter("@FinalStatus", objentityer.FinalResult)
                                    ,new SqlParameter("@LatestStatus", objentityer.LatestStatus)
                                   };
            return (objprodal.ExecuteDataSet("Pro_FromBackupTable_GetExecutionResult_LIKE_MeterID", param));

        }

        public DataSet Select_GetExecutionResult_LIKE_LogedUserID(EntityExecutionResult objentityer)
        {
            SqlParameter[] param = { new SqlParameter("@MeterType", objentityer.ExecutionMeterType)
                                    ,new SqlParameter("@LogedUserID", objentityer.LogedUserID)
                                    ,new SqlParameter("@ProcedureType", objentityer.ExecutionProcedureType)
                                    ,new SqlParameter("@FinalStatus", objentityer.FinalResult)
                                    ,new SqlParameter("@LatestStatus", objentityer.LatestStatus)
                                   };
            return (objprodal.ExecuteDataSet("Pro_GetExecutionResult_LIKE_UserID", param));

        }
        
       
        //-----------------------------Report--------------------------------
        

        public DataSet SelectFromtabExecutionReport_onPCBAIDSpecefic(EntityExecutionResult objentityer)
        {
            SqlParameter[] param = { new SqlParameter("@PCBAID", objentityer.PCBAID)
                                    ,new SqlParameter("@executionDate", objentityer.ExecutionDate)
                                   };
            return (objprodal.ExecuteDataSet("Pro_GetExecutionResult_onPCBAIDSpecefic", param));

        }
        
        public DataSet Select_GetExecutionResult_onMeterIDandProcedureType(EntityExecutionResult objentityer)
        {
            SqlParameter[] param = { new SqlParameter("@MeterID", objentityer.MeterID) 
                                    ,new SqlParameter("@ProcedureType", objentityer.ExecutionProcedureType)
                                    ,new SqlParameter("@MeterType", objentityer.ExecutionMeterType)
                                   };
            return (objprodal.ExecuteDataSet("Pro_GetExecutionResult_onMeterIDandProcedureType", param));

        }
        
        public DataSet Select_GetExecutionResult_onPCBAIDandProcedureType(EntityExecutionResult objentityer)
        {
            SqlParameter[] param = { new SqlParameter("@PCBAID", objentityer.PCBAID)
                                    ,new SqlParameter("@ProcedureType", objentityer.ExecutionProcedureType)
                                     ,new SqlParameter("@MeterType", objentityer.ExecutionMeterType)
                                   };
            return (objprodal.ExecuteDataSet("Pro_GetExecutionResult_onPCBAIDandProcedureType", param));

        }
        public DataSet Select_GetExecutionResult_onPCBAID_ProType_ExeDate(EntityExecutionResult objentityer)
        {
            SqlParameter[] param = { new SqlParameter("@PCBAID", objentityer.PCBAID)
                                    ,new SqlParameter("@ProcedureType", objentityer.ExecutionProcedureType)
                                    ,new SqlParameter("@MeterType", objentityer.ExecutionMeterType)
                                    ,new SqlParameter("@executionDate", objentityer.ExecutionDate)
                                    {
                                        SqlDbType = SqlDbType.DateTime2
                                    }
                                   };
            return (objprodal.ExecuteDataSet("Pro_GetExecutionResult_onPCBAID_ProType_ExeDate", param));

        }

        public bool IsCaseTamper_3Phase_AlreadyPass(EntityExecutionResult objentityer)
        {
            try
            {
                //var results = from myRow in dt.AsEnumerable()
                //              where myRow.Field<int>("LatestStatus") == 1 && myRow.Field<string>("ParametersName") == "CASE TAMPER TEST" && myRow.Field<string>("PCBAID") == objentityer.PCBAID && myRow.Field<string>("MeterType") == objentityer.ExecutionMeterType && myRow.Field<string>("ProcedureType") == objentityer.ExecutionProcedureType
                //              select myRow;
                //if (results.ToArray().Length <= 0) { return false; }
                //foreach (var EmpID in results)
                //{
                //    if (EmpID["ExecutionStatus"].ToString().ToUpperInvariant() == "PASS") return true;
                //    break;
                //}
                //return false;

                SqlParameter[] param = { new SqlParameter("@PCBAID", objentityer.PCBAID)
                                        ,new SqlParameter("@MeterType", objentityer.ExecutionMeterType)
                                        ,new SqlParameter("@ProcedureType", objentityer.ExecutionProcedureType)
                                        ,new SqlParameter("@ParametersName", "CASE TAMPER TEST")
                                   };
                DataSet ds = objprodal.ExecuteDataSet("Pro_GetExecutionResultDetails_ParameterWise", param);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["ExecutionStatus"].ToString().ToUpperInvariant() == "PASS") return true;                    
                }
                return false;
               
            }
            catch (Exception)
            {
                return false;
            }
        }
        

        public DataSet SelectFromtabExecutionReport_onMeterTypeandPCBAID(EntityExecutionResult objentityer)
        {
            SqlParameter[] param = { new SqlParameter("@PCBAID", objentityer.PCBAID)
                                    ,new SqlParameter("@MeterType", objentityer.ExecutionMeterType)
                                   };
            return (objprodal.ExecuteDataSet("GetExecutionResult_MeterTypeandPCBAID", param));

        }

        //S.A. code change 20180701 start
        public string InsertinToTabExecutionResultsPending(DataTable executionResultClone, EntityExecutionResult objentityer)
        {
            SqlParameter[] param ={             
            new SqlParameter("@ExecutionResult", executionResultClone),              
            new SqlParameter("@PCBAID",objentityer.PCBAID),
            new SqlParameter("@MeterType",objentityer.ExecutionMeterType),
            new SqlParameter("@ProcedureType",objentityer.ExecutionProcedureType),           
            new SqlParameter("@executionDate",objentityer.ExecutionDate)              
           };
            return objprodal.ExecuteInsertQuery("Pro_Insert_tabExecutionResultBatch", param);
        }
        //S.A. code change 20180701 end
         
    }
}
