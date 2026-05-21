using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using COMMONENTITY;
using System.Data.SqlClient;

namespace DALLAYER
{
    

   public class DALErrorOperations
    {
        ConnectionClass conn = new ConnectionClass();
        const string dbName = "LGPMPDB";
        const string dbRejectionMasterTableName = RejectionTable.dbRejectionMasterTableName;
        const string dbErrorTypeTableName = ErrorTypesTable.dbErrorTypeTableName;

        DALGolbalAdapter objprodal = new DALGolbalAdapter();

       /// <summary>
       /// Returns Error Types Detailed DataTable. 
       /// </summary>
       /// <returns>DataTable</returns>
        public DataTable GetErrorTypesTable()
        {
            try
            {
                //StringBuilder query = new StringBuilder();
                //query.AppendFormat("SELECT TOP 1000 ");
                
                //query.AppendFormat("[{0}]", ErrorTypesTable.colErrorType);
                //query.AppendFormat(",[{0}]", ErrorTypesTable.colErrorname);
                //query.AppendFormat(",[{0}]", ErrorTypesTable.colErrorDescription);
                //query.AppendFormat(",[{0}]", ErrorTypesTable.colIdError);
                //query.AppendFormat(" FROM [{0}].[dbo].[{1}] order by {2}", dbName, dbErrorTypeTableName, ErrorTypesTable.colIdError);


                //DataTable table = conn.GetQueryResult(query.ToString());
                //if (table != null) table.TableName = dbErrorTypeTableName;

                //return table;

                SqlParameter[] param = { };
                DataSet dsResult = objprodal.ExecuteDataSet("Pro_Rework_GetErrorTypesTable", param);
                if (dsResult != null && dsResult.Tables.Count > 0) return dsResult.Tables[0];
                else return null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
       /// <summary>
       /// Build query using given parameters.
       /// </summary>
       /// <param name="query"></param>
       /// <param name="param"></param>
        private void BuildSelectQuery(ref StringBuilder query, Dictionary<string,string> param)
        {
            query.AppendFormat("SELECT * FROM LGPMPDB.dbo.ViewRejectionTable ");

            query.Append(" WHERE ");

            query.AppendFormat(" {0} >= '{1}' AND {0} <= '{2}' ",
                                        param[ParamDictionaryKeys.ErrorDateType],
                                        param[ParamDictionaryKeys.ErrorDateStart],
                                        param[ParamDictionaryKeys.ErrorDateEnd]);

            if (param.ContainsKey(ParamDictionaryKeys.ProductionStage))
                query.AppendFormat(" AND {0} IN ({1}) ", RejectionTable.colProductionStage, param[ParamDictionaryKeys.ProductionStage].Equals(ErrorUtility.TestType_ALL) ? "%" : param[ParamDictionaryKeys.ProductionStage]);

            if (param.ContainsKey(ParamDictionaryKeys.MeterType))
                query.AppendFormat(" AND {0} IN ({1}) ", RejectionTable.colMeterType, param[ParamDictionaryKeys.MeterType].Equals(ErrorUtility.MeterType_ALL) ? "%" : param[ParamDictionaryKeys.MeterType]);

            if (param.ContainsKey(ParamDictionaryKeys.ErrorStatus))
                query.AppendFormat(" AND {0} IN ({1}) ", RejectionTable.colErrorState, param[ParamDictionaryKeys.ErrorStatus].Equals(ErrorStateConstants.StateAll) ? "%" : param[ParamDictionaryKeys.ErrorStatus]);

            if (param.ContainsKey(ParamDictionaryKeys.SearchParams))
            {
                string[] searchParams = param[ParamDictionaryKeys.SearchParams].Split('|');
                if (searchParams != null)
                {
                    foreach (string item in searchParams)
                    {
                        string[] keyValue = item.Split('=');
                        query.AppendFormat(" AND {0} LIKE '%{1}%' ", keyValue[0], keyValue[1]);
                    }
                }
            }

            if(param.ContainsKey(ParamDictionaryKeys.ErrorType))
                query.AppendFormat(" AND {0} = {1} ", RejectionTable.colIDError, param[ParamDictionaryKeys.ErrorType]);

            //Needs to be last query!
            if (param.ContainsKey(ParamDictionaryKeys.UniquePCBA) && bool.Parse(param[ParamDictionaryKeys.UniquePCBA]))
                query.Replace("LGPMPDB.dbo.ViewRejectionTable", @"(SELECT ROW_NUMBER() OVER (PARTITION BY [PCBAID] ORDER BY [ActionDate] DESC) as N ,* 
                                                                                                    FROM [LGPMPDB].[dbo].[ViewRejectionTable]) as A ")
                     .Append(" AND N=1");
        }
       /// <summary>
       /// Returns DataTable for Rejection Details.
       /// </summary>
       /// <param name="fromDate"></param>
       /// <param name="toDate"></param>
       /// <param name="param"></param>
       /// <returns>DataTable</returns>
        public DataTable GetRejectionMasterTable(Dictionary<string, string> param)
        {
            try
            {
                StringBuilder query = new StringBuilder();

                BuildSelectQuery(ref query, param);

                query.Append(";");
                DataTable table = conn.GetQueryResult(query.ToString());
                if (table != null) table.TableName = dbRejectionMasterTableName;
                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


       /// <summary>
       /// Returns Rejection details for given PCBAID, Production stage.
       /// </summary>
       /// <param name="PCBAID"></param>
       /// <param name="procedureStage"></param>
        /// <returns>DataTable</returns>
        public DataTable GetRejectionMastertableFrom_PCBA_Stage(string PCBAID, string procedureStage)
        {
            try
            {
                //StringBuilder query = new StringBuilder();

                //query.AppendFormat("SELECT TOP 1000 ");

                //query.AppendFormat("[{0}]", RejectionTable.colIDReject);
                //query.AppendFormat(",[{0}]", RejectionTable.colPCBAID);
                //query.AppendFormat(",[{0}]", RejectionTable.colProductionStage);
                //query.AppendFormat(",[{0}]", RejectionTable.colCustomer);
                //query.AppendFormat(",[{0}]", RejectionTable.colMeterType);
                //query.AppendFormat(",[{0}]", RejectionTable.colErrorDate);
                //query.AppendFormat(",[{0}]", RejectionTable.colActionDate);
                //query.AppendFormat(",[{0}]", RejectionTable.colParameterName);
                //query.AppendFormat(",[{0}]", RejectionTable.colErrorState);
                //query.AppendFormat(",[{0}]", RejectionTable.colRejectionAction);
                //query.AppendFormat(",[{0}]", RejectionTable.colProblemDescription);
                //query.AppendFormat(",[{0}]", RejectionTable.colRejectionCause);
                //query.AppendFormat(",[{0}]", RejectionTable.colMoreInformation);
                //query.AppendFormat(",[{0}]", RejectionTable.colIDError);

                //query.AppendFormat(" FROM [{0}].[dbo].[{1}] ", dbName, dbRejectionMasterTableName);
                //query.Append(" WHERE ");
                //query.AppendFormat(" [{0}] = '{1}' ", RejectionTable.colPCBAID, PCBAID);
                //query.Append(" AND ");
                //query.AppendFormat(" [{0}] = '{1}' ", RejectionTable.colProductionStage, procedureStage);
                //query.Append(";");

                //DataTable table = conn.GetQueryResult(query.ToString());

                //return table;

                SqlParameter[] param = { new SqlParameter("@PCBAID", PCBAID)
                                        ,new SqlParameter("@ProductionStage", procedureStage)
                                   };

                DataSet dsResult = (objprodal.ExecuteDataSet("Pro_Rework_GetRejectionMastertableFrom_PCBA_Stage", param));
                if (dsResult != null && dsResult.Tables.Count > 0) return dsResult.Tables[0];
                else return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
       /// <summary>
       /// Insert Data to Rejection Table.
       /// </summary>
       /// <param name="errorEntity"></param>
       /// <returns>boolean</returns>
        public bool InsertToRejectionTable(EntityError errorEntity)
        {
            try
            {
                //StringBuilder query = new StringBuilder();
                //query.AppendFormat(@"INSERT INTO [{0}].[dbo].[{1}]  ", dbName, dbRejectionMasterTableName);
                //query.Append("(");
                //query.AppendFormat("[{0}]", RejectionTable.colPCBAID);
                //query.AppendFormat(",[{0}]", RejectionTable.colCustomer);
                //query.AppendFormat(",[{0}]", RejectionTable.colMeterType);
                //query.AppendFormat(",[{0}]", RejectionTable.colErrorDate);
                //query.AppendFormat(",[{0}]", RejectionTable.colActionDate);
                //query.AppendFormat(",[{0}]", RejectionTable.colProductionStage);
                //query.AppendFormat(",[{0}]", RejectionTable.colParameterName);
                //query.AppendFormat(",[{0}]", RejectionTable.colErrorState);
                //query.AppendFormat(",[{0}]", RejectionTable.colRejectionAction);
                //query.AppendFormat(",[{0}]", RejectionTable.colProblemDescription);
                //query.AppendFormat(",[{0}]", RejectionTable.colRejectionCause);
                //query.AppendFormat(",[{0}]", RejectionTable.colMoreInformation);
                //query.AppendFormat(",[{0}]", RejectionTable.colLoggedUserID);
                //query.AppendFormat(",[{0}]", RejectionTable.colWorkstationID);
                //query.AppendFormat(",[{0}]", RejectionTable.colIDError);
                //query.AppendFormat(",[{0}]", RejectionTable.colLastRepairID);
                //query.Append(")");

                //query.Append(" VALUES ( ");
                //query.AppendFormat("'{0}'", errorEntity.PCBAID);
                //query.AppendFormat(",'{0}'", errorEntity.Customer);
                //query.AppendFormat(",'{0}'", errorEntity.MeterType);
                //query.AppendFormat(",'{0}'", errorEntity.ErrorDate.ToString("MM/dd/yyyy hh:mm:ss tt"));     //'7/19/2018 10:14:03 AM'
                //query.AppendFormat(",'{0}'", errorEntity.ActionDate.ToString("MM/dd/yyyy hh:mm:ss tt"));
                //query.AppendFormat(",'{0}'", errorEntity.ProcedureStage);
                //query.AppendFormat(",'{0}'", errorEntity.ParameterName);
                //query.AppendFormat(",'{0}'", errorEntity.ErrorState);
                //query.AppendFormat(",'{0}'", errorEntity.ActionTaken);
                //query.AppendFormat(",'{0}'", errorEntity.ProblemDescription);
                //query.AppendFormat(",'{0}'", errorEntity.RejectionCause);
                //query.AppendFormat(",'{0}'", errorEntity.MoreInformation);
                //query.AppendFormat(",'{0}'", errorEntity.LoggedUserID);
                //query.AppendFormat(",'{0}'", errorEntity.WorkStationID);
                //query.AppendFormat(",'{0}'", errorEntity.ErrorID);
                //query.AppendFormat(",'{0}'", errorEntity.LastRepairID);
                //query.Append(")");
                //query.Append(";");

                //return conn.ExecuteNonQuery(query.ToString());

                SqlParameter[] param ={             
                            new SqlParameter("@PCBAID", errorEntity.PCBAID),              
                            new SqlParameter("@Customer",errorEntity.Customer),
                            new SqlParameter("@MeterType",errorEntity.MeterType),
                            new SqlParameter("@ErrorDate",errorEntity.ErrorDate)
                                        {
                                            SqlDbType = SqlDbType.DateTime2
                                        },           
                            new SqlParameter("@ActionDate",errorEntity.ActionDate)
                                        {
                                            SqlDbType = SqlDbType.DateTime2
                                        }, 
                            new SqlParameter("@ProcedureStage", errorEntity.ProcedureStage),              
                            new SqlParameter("@ParameterName",errorEntity.ParameterName),
                            new SqlParameter("@ErrorState",errorEntity.ErrorState),                            
                            new SqlParameter("@ActionTaken", errorEntity.ActionTaken),              
                            new SqlParameter("@ProblemDescription",errorEntity.ProblemDescription),
                            new SqlParameter("@RejectionCause",errorEntity.RejectionCause),                            
                            new SqlParameter("@MoreInformation", errorEntity.MoreInformation),              
                            new SqlParameter("@LoggedUserID",errorEntity.LoggedUserID),
                            new SqlParameter("@WorkStationID",errorEntity.WorkStationID),                            
                            new SqlParameter("@ErrorID", errorEntity.ErrorID),              
                            new SqlParameter("@LastRepairID",errorEntity.LastRepairID),
                           };
                return objprodal.ExecuteInsertQuery("Pro_Rework_Insert_RejectionTable_ErrorEntity_Params", param) == "";
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

       /*
        /// <summary>
        /// Update Data to Rejection Table for given pcbaID in parameter.
        /// </summary>
        /// <param name="errorEntity"></param>
        /// <returns>boolean</returns>
        public bool UpdateToRejectionTablePCBAID(EntityError errorEntity)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendFormat("UPDATE [{0}].[dbo].[{1}] SET  ", dbName, dbRejectionMasterTableName);
                query.AppendFormat(" [{0}] = '{1}'", RejectionTable.colProductionStage, errorEntity.ProcedureStage);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colParameterName, errorEntity.ParameterName);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colErrorDate, errorEntity.ErrorDate);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colActionDate, errorEntity.ActionDate);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colErrorState, errorEntity.ErrorState);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colRejectionAction, errorEntity.ActionTaken);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colMoreInformation, errorEntity.MoreInformation);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colRejectionCause, errorEntity.RejectionCause);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colIDError, errorEntity.ErrorID);
                query.AppendFormat(" where [{0}] = {1}", RejectionTable.colPCBAID, errorEntity.PCBAID);
                query.Append(";");

                return conn.ExecuteNonQuery(query.ToString());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        */

       /*
       /// <summary>
        /// Update Data to Rejection Table for given RejectID in parameter.
       /// </summary>
       /// <param name="errorEntity"></param>
       /// <returns>boolean</returns>
        public bool UpdateToRejectionTableRejectID(EntityError errorEntity)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendFormat("UPDATE [{0}].[dbo].[{1}] SET  ", dbName, dbRejectionMasterTableName);
                query.AppendFormat(" [{0}] = '{1}'", RejectionTable.colPCBAID, errorEntity.PCBAID);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colCustomer, errorEntity.Customer);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colProductionStage, errorEntity.ProcedureStage);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colMeterType, errorEntity.MeterType);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colParameterName, errorEntity.ParameterName);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colErrorDate, errorEntity.ErrorDate);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colActionDate, errorEntity.ActionDate);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colErrorState, errorEntity.ErrorState);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colRejectionAction, errorEntity.ActionTaken);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colProblemDescription, errorEntity.ProblemDescription);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colMoreInformation, errorEntity.MoreInformation);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colRejectionCause, errorEntity.RejectionCause);
                query.AppendFormat(", [{0}] = '{1}'", RejectionTable.colIDError, errorEntity.ErrorID);
                query.AppendFormat(" where [{0}] = {1}", RejectionTable.colIDReject, errorEntity.Rejectid);
                query.Append(";");

                return conn.ExecuteNonQuery(query.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */

       /// <summary>
       /// Execute given SQL procedure.
       /// </summary>
       /// <param name="InsertProcedureName"></param>
       /// <param name="param"></param>
       /// <returns>string</returns>
        public string ExecuteInsertQuery(string InsertProcedureName, SqlParameter[] param)
        {
            return (conn.ExecuteScaler(InsertProcedureName, param));
        }
       /// <summary>
        /// Execute given SQL procedure.
       /// </summary>
       /// <param name="selectProcedureName"></param>
       /// <param name="param"></param>
       /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string selectProcedureName, SqlParameter[] param)
        {
            return (conn.ExecuteDataSet(selectProcedureName, param));
        }
       /// <summary>
       /// Get Error Details for a given Rejection Entry.
       /// </summary>
       /// <param name="idReject"></param>
       /// <returns>DataTable</returns>
        public DataTable GetExtendedInformation(long idReject)
        {
            try
            {
                //StringBuilder query = new StringBuilder();

                //query.AppendFormat("SELECT TOP 1000 ");
                //query.AppendFormat("[{0}]", RejectionTable.colParameterName);
                //query.AppendFormat(",[{0}]", RejectionTable.colMoreInformation);
                //query.AppendFormat(",[{0}]", ErrorTypesTable.colErrorType);
                //query.AppendFormat(",[{0}]", ErrorTypesTable.colErrorname);
                //query.AppendFormat(",[{0}]", ErrorTypesTable.colErrorDescription);

                //query.AppendFormat(" FROM [{0}].[dbo].[{1}] ", dbName, dbRejectionMasterTableName);
                //query.Append(" INNER JOIN ");
                //query.AppendFormat(" [{0}].[dbo].[{1}] ", dbName, dbErrorTypeTableName);
                //query.AppendFormat(" ON [{0}].[dbo].[{1}].[{3}] = [{0}].[dbo].[{2}].[{4}]", dbName, dbRejectionMasterTableName, dbErrorTypeTableName, RejectionTable.colIDError, ErrorTypesTable.colIdError);


                // query.Append(" WHERE ");
                   
                // query.AppendFormat(" {0} = {1} ", RejectionTable.colIDReject, idReject);


                        
                
                //query.Append(";");

                //DataTable table = conn.GetQueryResult(query.ToString());

                //return table;

                SqlParameter[] param = { new SqlParameter("@IDReject", idReject)
                                   };

                DataSet dsResult = (objprodal.ExecuteDataSet("Pro_Rework_GetExtendedInformation_RejectID", param));
                if (dsResult != null && dsResult.Tables.Count > 0) return dsResult.Tables[0];
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       /// <summary>
        /// Get Error Details History for a given Rejection PCBAID and production stage.
       /// </summary>
       /// <param name="PCBAID"></param>
       /// <param name="productionStage"></param>
       /// <returns></returns>
        public DataTable GetRejectHistoryTable_PCBID_Stage(string PCBAID, string productionStage)
        {
            try
            {
                //StringBuilder query = new StringBuilder();

                //query.AppendFormat("SELECT ");
                //query.AppendFormat("[{0}]", RejectionTable.colIDReject);
                //query.AppendFormat(",[{0}]", RejectionTable.colPCBAID);
                //query.AppendFormat(",[{0}]", RejectionTable.colProductionStage);
                //query.AppendFormat(",[{0}]", RejectionTable.colCustomer);
                //query.AppendFormat(",[{0}]", RejectionTable.colMeterType);
                //query.AppendFormat(",[{0}]", RejectionTable.colErrorDate);
                //query.AppendFormat(",[{0}]", RejectionTable.colActionDate);
                //query.AppendFormat(",Q.[{0}]", ErrorTypesTable.colErrorType);
                //query.AppendFormat(",Q.[{0}]", ErrorTypesTable.colErrorname);
                //query.AppendFormat(",[{0}]", RejectionTable.colParameterName);
                //query.AppendFormat(",[{0}]", RejectionTable.colErrorState);
                //query.AppendFormat(",[{0}]", RejectionTable.colProblemDescription);
                //query.AppendFormat(",[{0}]", RejectionTable.colRejectionCause);
                //query.AppendFormat(",[{0}]", RejectionTable.colRejectionAction);
                //query.AppendFormat(",[{0}]", RejectionTable.colLoggedUserID);
                //query.AppendFormat(",[{0}]", RejectionTable.colWorkstationID);
                //query.AppendFormat(",[{0}]", RejectionTable.colLastRepairID);
                //query.AppendFormat(",M.[{0}]", RejectionTable.colIDError);
                //query.AppendFormat(",[{0}]", RejectionTable.colMoreInformation);

                //query.AppendFormat(" FROM [{0}].[dbo].[{1}] AS M", dbName, dbRejectionMasterTableName);

                //query.Append(" INNER JOIN ");
                //query.AppendFormat(" [{0}].[dbo].[{1}]  AS Q ", dbName, dbErrorTypeTableName);
                //query.AppendFormat(" ON M.[{0}] = Q.[{1}]", RejectionTable.colIDError, ErrorTypesTable.colIdError);

                //query.AppendFormat(" WHERE ");
                //query.AppendFormat(" M.{0} = '{1}'", RejectionTable.colPCBAID,PCBAID);
                //query.AppendFormat(" AND ");
                //query.AppendFormat(" M.{0} = '{1}'", RejectionTable.colProductionStage, productionStage);
                //query.AppendFormat("ORDER BY M.{0}",RejectionTable.colActionDate);
                //query.Append(";");

                //DataTable table = conn.GetQueryResult(query.ToString());

                //return table;

                SqlParameter[] param = { new SqlParameter("@PCBAID", PCBAID)
                                        ,new SqlParameter("@ProductionStage", productionStage)
                                   };

                DataSet dsResult = (objprodal.ExecuteDataSet("Pro_Rework_GetRejectHistory_PCBID_Stage", param));
                if (dsResult != null && dsResult.Tables.Count > 0) return dsResult.Tables[0];
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       /// <summary>
       /// Insert new error type.
       /// </summary>
       /// <param name="errEntityType"></param>
       /// <returns>boolean</returns>
        public bool InsertToErrorTypesTable(EntityErrorType errEntityType)
        {
            try
            {
                //StringBuilder query = new StringBuilder();

                //query.AppendFormat("INSERT INTO [{0}].[dbo].[{1}]", dbName, dbErrorTypeTableName);
                //query.Append("(");
                //query.AppendFormat("[{0}]", ErrorTypesTable.colErrorType);
                //query.AppendFormat(",[{0}]", ErrorTypesTable.colErrorname);
                //query.AppendFormat(",[{0}]", ErrorTypesTable.colErrorDescription);
                //query.Append(")");
                //query.Append(" VALUES( ");
                //query.AppendFormat("'{0}'", errEntityType.ErrorType);
                //query.AppendFormat(",'{0}'", errEntityType.ErrorName);
                //query.AppendFormat(",'{0}'", errEntityType.ErrorDescription);
                //query.Append(")");
                //query.Append(";");

                //return conn.ExecuteNonQuery(query.ToString());

                SqlParameter[] param ={             
                            new SqlParameter("@ErrorType", errEntityType.ErrorType),              
                            new SqlParameter("@ErrorName",errEntityType.ErrorName),
                            new SqlParameter("@ErrorDescription",errEntityType.ErrorDescription)
                           };
                return objprodal.ExecuteInsertQuery("Pro_Rework_Insert_ErrorTypesTable_ErrorEntity_Params", param) == "";
            }
            catch ( Exception ex)
            {
                throw ex;
            }
        }
       /// <summary>
        /// Get Error Details History for a given Rejection PCBAID .
       /// </summary>
       /// <param name="PCBAID"></param>
       /// <returns>DataTable</returns>
        public DataTable GetRejectHistoryTable_PCBID(string PCBAID)
        {
            try
            {
                //StringBuilder query = new StringBuilder();

                //query.AppendFormat("SELECT ");
                //query.AppendFormat("[{0}]", RejectionTable.colIDReject);
                //query.AppendFormat(",[{0}]", RejectionTable.colPCBAID);
                //query.AppendFormat(",[{0}]", RejectionTable.colProductionStage);
                //query.AppendFormat(",[{0}]", RejectionTable.colCustomer);
                //query.AppendFormat(",[{0}]", RejectionTable.colMeterType);
                //query.AppendFormat(",[{0}]", RejectionTable.colErrorDate);
                //query.AppendFormat(",[{0}]", RejectionTable.colActionDate);
                //query.AppendFormat(",Q.[{0}]", ErrorTypesTable.colErrorType);
                //query.AppendFormat(",Q.[{0}]", ErrorTypesTable.colErrorname);
                //query.AppendFormat(",[{0}]", RejectionTable.colParameterName);
                //query.AppendFormat(",[{0}]", RejectionTable.colErrorState);
                //query.AppendFormat(",[{0}]", RejectionTable.colProblemDescription);
                //query.AppendFormat(",[{0}]", RejectionTable.colRejectionCause);
                //query.AppendFormat(",[{0}]", RejectionTable.colRejectionAction);
                //query.AppendFormat(",[{0}]", RejectionTable.colLoggedUserID);
                //query.AppendFormat(",[{0}]", RejectionTable.colWorkstationID);
                //query.AppendFormat(",[{0}]", RejectionTable.colLastRepairID);
                //query.AppendFormat(",M.[{0}]", RejectionTable.colIDError);
                //query.AppendFormat(",[{0}]", RejectionTable.colMoreInformation);

                //query.AppendFormat(" FROM [{0}].[dbo].[{1}] AS M", dbName, dbRejectionMasterTableName);

                //query.Append(" INNER JOIN ");
                //query.AppendFormat(" [{0}].[dbo].[{1}]  AS Q ", dbName, dbErrorTypeTableName);
                //query.AppendFormat(" ON M.[{0}] = Q.[{1}]", RejectionTable.colIDError, ErrorTypesTable.colIdError);

                //query.AppendFormat(" WHERE ");
                //query.AppendFormat(" M.{0} = '{1}'", RejectionTable.colPCBAID, PCBAID);
                //query.AppendFormat("ORDER BY M.{0}", RejectionTable.colProductionStage);
                //query.Append(";");

                //DataTable table = conn.GetQueryResult(query.ToString());

                //return table;

                SqlParameter[] param = { new SqlParameter("@PCBAID", PCBAID)
                                   };

                DataSet dsResult = (objprodal.ExecuteDataSet("Pro_Rework_GetRejectHistory_PCBID", param));
                if (dsResult != null && dsResult.Tables.Count > 0) return dsResult.Tables[0];
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       /// <summary>
       /// Get Error types details with count of errors.
       /// </summary>
       /// <returns>DataTable</returns>
        public DataTable GetErrorTypesTableWithCount()
        {
            try
            {
                //StringBuilder query = new StringBuilder();
                //query.AppendFormat(" Select ISNull(A.Count,0) as Count, E.{0}, E.{1},E.{2},E.{3} from ",
                //                                                                ErrorTypesTable.colIdError,
                //                                                                ErrorTypesTable.colErrorType,
                //                                                                ErrorTypesTable.colErrorname,
                //                                                                ErrorTypesTable.colErrorDescription);
                //query.AppendFormat(" (SELECT R.{0},Count(R.[{0}]) as Count FROM ", RejectionTable.colIDError);
                //query.Append("(");

                //Dictionary<string, string> param = new Dictionary<string, string>();
                //param.Add(ParamDictionaryKeys.ErrorDateType, RejectionTable.colActionDate);
                //param.Add(ParamDictionaryKeys.ErrorDateStart, DateTime.Now.AddMonths(-1).ToShortDateString() + " 00:00:00");
                //param.Add(ParamDictionaryKeys.ErrorDateEnd, DateTime.Now.ToShortDateString() + " 23:59:59");

                //BuildSelectQuery(ref query, param);

                //query.Append(")");
                //query.AppendFormat(" as R group by R.{0} ) as A ", RejectionTable.colIDError);
                //query.AppendFormat(" right join ");
                //query.AppendFormat(" LGPMPDB.dbo.{0} as E on A.{1}= E.{2} ", ErrorTypesTable.dbErrorTypeTableName,
                //                                                                                RejectionTable.colIDError,
                //                                                                                ErrorTypesTable.colIdError);

                //query.AppendFormat(" order by Count DESC, E.{2}", dbName, dbErrorTypeTableName, ErrorTypesTable.colIdError);


                //DataTable table = conn.GetQueryResult(query.ToString());
                //if (table != null) table.TableName = dbErrorTypeTableName;

                //return table;

                SqlParameter[] param = { };
                DataSet dsResult = objprodal.ExecuteDataSet("Pro_Rework_GetErrorTypesTableWithCount", param);
                if (dsResult != null && dsResult.Tables.Count > 0) return dsResult.Tables[0];
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       /// <summary>
       /// Get Error Types Table With Count for given parameters.
       /// </summary>
       /// <param name="fromDate"></param>
       /// <param name="toDate"></param>
       /// <param name="param"></param>
       /// <returns>DataTable</returns>
        public DataTable GetErrorTypesTableWithCount( Dictionary<string, string> param)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendFormat(" Select ISNull(A.Count,0) as Count, E.{0}, E.{1},E.{2},E.{3} from ",
                                                                                ErrorTypesTable.colIdError,
                                                                                ErrorTypesTable.colErrorType,
                                                                                ErrorTypesTable.colErrorname,
                                                                                ErrorTypesTable.colErrorDescription);
                query.AppendFormat(" (SELECT R.{0},Count(R.[{0}]) as Count FROM ", RejectionTable.colIDError);
                query.Append("(");

                BuildSelectQuery(ref query, param);

                query.Append(")");
                query.AppendFormat(" as R group by R.{0} ) as A ", RejectionTable.colIDError);
                query.AppendFormat(" right join ");
                query.AppendFormat(" LGPMPDB.dbo.{0} as E on A.{1}= E.{2} ", ErrorTypesTable.dbErrorTypeTableName,
                                                                                                RejectionTable.colIDError,
                                                                                                ErrorTypesTable.colIdError);

                query.AppendFormat(" order by Count DESC, E.{2}", dbName, dbErrorTypeTableName, ErrorTypesTable.colIdError);


                DataTable table = conn.GetQueryResult(query.ToString());
                if (table != null) table.TableName = dbErrorTypeTableName;

                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       /// <summary>
       /// Delete Record for given PCBAID and production stage.
       /// </summary>
       /// <param name="pcbaid"></param>
       /// <param name="stage"></param>
       /// <returns>boolean</returns>
        public bool DeleteRecord_PCBAID_Stage(string pcbaid, string stage)
        {
            try
            {
                //StringBuilder query = new StringBuilder();

                //query.AppendFormat("DELETE from {0}.dbo.{1} ", dbName, dbRejectionMasterTableName);
                //query.Append(" WHERE ");
                //query.AppendFormat("  {0} = '{1}' ", RejectionTable.colPCBAID, pcbaid);
                //query.Append(" AND ");
                //query.AppendFormat(" {0} = '{1}'", RejectionTable.colProductionStage, stage);


                //return conn.ExecuteNonQuery(query.ToString());

                SqlParameter[] param = { new SqlParameter("@PCBAID", pcbaid),
                                         new SqlParameter("@ProductionStage", stage),
                                   };

                return objprodal.ExecuteInsertQuery("Pro_Rework_DeleteRecord_PCBAID_Stage", param) == "";
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
       /// <summary>
       /// Get Error Types Details for Manual Errors only.
       /// </summary>
       /// <returns>DataTable</returns>
        public DataTable GetHWErrorTypesTable()
        {
            try
            {
                //StringBuilder query = new StringBuilder();
                //query.AppendFormat("SELECT TOP 1000 ");

                //query.AppendFormat("[{0}]", ErrorTypesTable.colErrorType);
                //query.AppendFormat(",[{0}]", ErrorTypesTable.colErrorname);
                //query.AppendFormat(",[{0}]", ErrorTypesTable.colErrorDescription);
                //query.AppendFormat(",[{0}]", ErrorTypesTable.colIdError);
                //query.AppendFormat(" FROM [{0}].[dbo].[{1}]", dbName, dbErrorTypeTableName);
                //query.AppendFormat(" where [{0}] like 'HW%'", ErrorTypesTable.colErrorType);
                //query.AppendFormat(" order by {0}", ErrorTypesTable.colIdError);

                //DataTable table = conn.GetQueryResult(query.ToString());
                //if (table != null) table.TableName = dbErrorTypeTableName;

                //return table;

                SqlParameter[] param = { };
                DataSet dsResult = objprodal.ExecuteDataSet("Pro_Rework_GetHWErrorTypesTable", param);
                if (dsResult != null && dsResult.Tables.Count > 0) return dsResult.Tables[0];
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       /// <summary>
       /// Gets Passwords table from database.
       /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetPasswordsForEntryModification()
        {
            try
            {
                //StringBuilder query = new StringBuilder();
                //query.AppendFormat("SELECT [Reserved5] FROM [LGPMPDB].[dbo].[tabUserManagement] where [UserID]='INDEL7063'");

                //DataTable table = conn.GetQueryResult(query.ToString());
                //if (table != null) table.TableName = dbErrorTypeTableName;

                //return table;

                SqlParameter[] param = { };
                DataSet dsResult = objprodal.ExecuteDataSet("Pro_Rework_GetPasswordsForEntryModification", param);
                if (dsResult != null && dsResult.Tables.Count > 0) return dsResult.Tables[0];
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetProductionDataAutoFRS(string dboTableName, string startDate, string endDate)
        {
                //BEGIN TRANSACTION
                //    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
                //    SELECT A.StationID, ISNULL(PassCount,'') as PassCount, ISNULL(FailCount,'') as FailCount From
                //    (SELECT StationID, count(distinct PCBID) AS PassCount
                //    FROM {0}
                //    where (EntryDate between '{1}' and '{2}')
                //    and Status='PASS'
                //    GROUP BY StationID) A
                //    left outer join
                //    (SELECT StationID, count(distinct PCBID) AS FailCount
                //    FROM {0}
                //    where (EntryDate between '{1}' and '{2}')
                //    and Status='FAIL'
                //    GROUP BY StationID) B
                //    ON A.StationID=B.StationID
                //    order by A.StationID ASC
                //  COMMIT TRANSACTION
                //  BEGIN TRANSACTION
                //    SET TRANSACTION ISOLATION LEVEL READ COMMITTED
                //COMMIT TRANSACTION
            try
            {
                //StringBuilder query = new StringBuilder();
                //query.AppendFormat(@"   
                //                    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
                //
                //                      WITH TempItem AS
                //                      (
                //                          SELECT A.StationID, ISNULL(PassCount,'') as PassCount, ISNULL(FailCount,'') as FailCount From
                //                              (SELECT StationID, PCBID AS PassCount
                //                              FROM {0}
                //                              where (EntryDate between '{1}' and '{2}')
                //                              and Status='PASS'
                //                              ) A
                //                                  left outer join
                //                              (SELECT StationID, PCBID AS FailCount
                //                              FROM {0}
                //                              where (EntryDate between '{1}' and '{2}')
                //                              and Status='FAIL'
                //                              ) B
                //                                  ON A.StationID=B.StationID
                //                                  and A.PassCount = B.FailCount
                //                      )
                //
                //                      Select A.StationID, PassCount, ISNULL(FailCount,'0') as FailCount from
                //                          (Select StationID, count(distinct PassCount) as PassCount from TempItem where not PassCount = FailCount group by StationID) A
                //                              full outer join
                //                          (Select StationID, count(distinct PassCount) as FailCount from TempItem where PassCount = FailCount group by StationID) B
                //                              on A.StationID = B.StationID
                //                      where A.StationID <> '' and A.StationID IS NOT NULL
                //                        order by A.StationID
                //
                //                    SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
                //                        ", dboTableName, startDate, endDate);

                //DataTable table = conn.GetQueryResult(query.ToString(), "AUTOFRS");
                //if (table != null) table.TableName = dboTableName;

                //return table;

                SqlParameter[] param = 
                {
                    new SqlParameter("@dboTableName", dboTableName),
                    new SqlParameter("@startDate", startDate),
                    new SqlParameter("@endDate", endDate)
                };
                DataSet dsResult = objprodal.ExecuteDataSet("Pro_Rework_GetProductionDataAutoFRS", param);
                if (dsResult != null && dsResult.Tables.Count > 0) return dsResult.Tables[0];
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetProductionDataPMPDB(string procedure, string startDate, string endDate)
        {
            try
            {
                // StringBuilder query = new StringBuilder();
                // query.AppendFormat(@"   
                //                        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
                //
                //                             SELECT A.WorkStationID as StationID, ISNULL(PassCount,'') as PassCount, ISNULL(FailCount,'') as FailCount From
                //                                 (SELECT WorkStationID, count(distinct PCBAID) AS PassCount
                //                                 FROM [LGPMPDB].[dbo].[tabExecutionResult]
                //                                 where (executionDate between '{1}' and '{2}')
                //                                 and FinalStatus='PASS'
                //                              and ProcedureType = '{0}'
                //                                 GROUP BY WorkStationID) A
                //                                     left outer join
                //                                 (SELECT WorkStationID, count(distinct PCBAID) AS FailCount
                //                                 FROM [LGPMPDB].[dbo].[tabExecutionResult]
                //                                 where (executionDate between '{1}' and '{2}')
                //                                 and FinalStatus='FAIL'
                //                              and ProcedureType = '{0}'
                //                                 GROUP BY WorkStationID) B
                //                                     ON A.WorkStationID=B.WorkStationID
                //                             order by A.WorkStationID ASC
                //
                //                         SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
                //                         ", procedure, startDate, endDate);

                // DataTable table = conn.GetQueryResult(query.ToString());
                // if (table != null) table.TableName = procedure;

                // return table;

                SqlParameter[] param = 
                {
                    new SqlParameter("@procedureName", procedure),
                    new SqlParameter("@startDate", startDate),
                    new SqlParameter("@endDate", endDate)
                };
                DataSet dsResult = objprodal.ExecuteDataSet("Pro_Rework_GetProductionDataPMPDB", param);
                if (dsResult != null && dsResult.Tables.Count > 0) return dsResult.Tables[0];
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
