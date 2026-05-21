using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using COMMONENTITY;
using System.Data.SqlClient;
using DALLAYER;

namespace BALLAYER
{
    public class BALErrorOperations
    {
        CommonCommandMethods objccmdmethod = new CommonCommandMethods();
        BALExecutionResults balexresobj = new BALExecutionResults();
        DALErrorOperations dalerrorobj = new DALErrorOperations();

        public DataTable GetProductionDataAutoFRS(string dboTableName, string startDate, string endDate)
        {
            DataTable table = null;
            try
            {
                table = dalerrorobj.GetProductionDataAutoFRS( dboTableName,  startDate,  endDate);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "PMPTool", System.Windows.Forms.MessageBoxButtons.OK);
            }

            return table;
        }

        public DataTable GetProductionDataPMPDB(string procedure, string startDate, string endDate)
        {
            DataTable table = null;
            try
            {
                table = dalerrorobj.GetProductionDataPMPDB(procedure, startDate, endDate);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "PMPTool", System.Windows.Forms.MessageBoxButtons.OK);
            }

            return table;
        }

        /// <summary>
        /// Get Error Types Details for Manual Errors only.
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetHWErrorTypesTable()
        {
            DataTable table = null;
            try
            {
                 table = dalerrorobj.GetHWErrorTypesTable();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "PMPTool", System.Windows.Forms.MessageBoxButtons.OK);
            }

            return table;
        }

        /*
        /// <summary>
        /// Returns Error Types Detailed DataTable. 
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetErrorTypesTable()
        {
            DataTable table = null;
            try
            {
                table = dalerrorobj.GetErrorTypesTable();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "PMPTool", System.Windows.Forms.MessageBoxButtons.OK);
            }

            return table;
        }
         */

        /// <summary>
        /// Returns DataTable for Rejection Details.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="param"></param>
        /// <returns>DataTable</returns>
        public DataTable GetRejectionMasterTable(Dictionary<string, string> param)
        {
            DataTable table = null;
            try
            {
                 table = dalerrorobj.GetRejectionMasterTable(param);
               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "PMPTool", System.Windows.Forms.MessageBoxButtons.OK);
            }

            return table;
        }
        /// <summary>
        /// Insert Data to Rejection Table.
        /// </summary>
        /// <param name="errorEntity"></param>
        /// <returns>boolean</returns>
        public bool InsertToRejectionTable(EntityError errorEntity)
        {
            bool bSuccess = false;
            try
            {
                    DataTable historytable = dalerrorobj.GetRejectionMastertableFrom_PCBA_Stage(errorEntity.PCBAID, errorEntity.ProcedureStage);
                    if (historytable != null && historytable.Rows.Count>0)
                    {
                        errorEntity.ErrorDate = Convert.ToDateTime(historytable.Rows[historytable.Rows.Count - 1][RejectionTable.colErrorDate]);
                    }
                    bSuccess = dalerrorobj.InsertToRejectionTable(errorEntity);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "PMPTool", System.Windows.Forms.MessageBoxButtons.OK);
            }
            return bSuccess;
        }

        /// <summary>
        /// Get Execution Details for selected PCBAID
        /// </summary>
        /// <param name="errentity"></param>
        /// <returns>DataSet</returns>
        public DataSet Select_GetExecutionResult_onPCBAIDandProcedureType(EntityError  errentity)
        {
            DataSet dataset = null;
            try
            {
                SqlParameter[] param = { new SqlParameter("@PCBAID", errentity.PCBAID)
                                        ,new SqlParameter("@ProcedureType", errentity.ProcedureStage)
                                         ,new SqlParameter("@MeterType", errentity.MeterType)
                                       };
                
                dataset = (dalerrorobj.ExecuteDataSet("Pro_GetExecutionResult_onPCBAIDandProcedureType", param));
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "PMPTool", System.Windows.Forms.MessageBoxButtons.OK);
            }
            return dataset;

        }

        /*
        /// <summary>
        /// Get Execution Status using given parameters.
        /// </summary>
        /// <param name="errentity"></param>
        /// <returns>DataSet</returns>
        public DataSet VerifyExecutionStatusInDatabase(EntityError errentity)
        {
            DataSet ds = null;
            try
            {
                ds = Select_GetExecutionResult_onPCBAIDandProcedureType(errentity);
                
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "PMPTool", System.Windows.Forms.MessageBoxButtons.OK);
            }
            return ds;
        }
        */

        /// <summary>
        /// Get Error Details for a given Rejection Entry.
        /// </summary>
        /// <param name="idReject"></param>
        /// <returns>DataTable</returns>
        public DataTable GetExtendedInformation(long idReject)
        {
            DataTable table = null;
            try
            {
                table = dalerrorobj.GetExtendedInformation(idReject);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "PMPTool", System.Windows.Forms.MessageBoxButtons.OK);
            }
            return table;
        }

        /*
        /// <summary>
        /// Update Data to Rejection Table for given pcbaID in parameter.
        /// </summary>
        /// <param name="errorEntity"></param>
        /// <returns>boolean</returns>
        public bool UpdateToRejectionTable(EntityError errentity)
        {
            bool bSuccess=false;
            try
            {
                bSuccess = dalerrorobj.UpdateToRejectionTableRejectID(errentity);
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "PMPTool", System.Windows.Forms.MessageBoxButtons.OK);
            }
            return bSuccess;
        }
         */

        /// <summary>
        /// Get Error Details History for a given Rejection PCBAID and production stage.
        /// </summary>
        /// <param name="PCBAID"></param>
        /// <param name="productionStage"></param>
        /// <returns></returns>
        public DataTable GetHistoryTable_PCBID_Stage(string PCBAID, string productionStage)
        {
            DataTable table = null;
            try
            {
                table = dalerrorobj.GetRejectHistoryTable_PCBID_Stage(PCBAID, productionStage);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "PMPTool", System.Windows.Forms.MessageBoxButtons.OK);
            }
            return table;
        }

        /// <summary>
        /// Get Error Details History for a given Rejection PCBAID .
        /// </summary>
        /// <param name="PCBAID"></param>
        /// <returns>DataTable</returns>
        public DataTable GetHistoryTable_PCBID(string PCBAID)
        {
            DataTable table = null;
            try
            {
                table = dalerrorobj.GetRejectHistoryTable_PCBID(PCBAID);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "PMPTool", System.Windows.Forms.MessageBoxButtons.OK);
            }
            return table;
        }

        /// <summary>
        /// Insert new error type.
        /// </summary>
        /// <param name="errEntityType"></param>
        /// <returns>boolean</returns>
        public bool InsertToErrorTypesTable(EntityErrorType errEntityType)
        {
            bool bSuccess = false;
            try
            {
                bSuccess = dalerrorobj.InsertToErrorTypesTable(errEntityType);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "PMPTool", System.Windows.Forms.MessageBoxButtons.OK);
            }
            return bSuccess;
        }

        /// <summary>
        /// Get Error types details with count of errors.
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetErrorTypesTableWithCount()
        {
            DataTable table = null;
            try
            {
                 table = dalerrorobj.GetErrorTypesTableWithCount();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "PMPTool", System.Windows.Forms.MessageBoxButtons.OK);
            }

            return table;
        }

        /// <summary>
        /// Get Error Types Table With Count for given parameters.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="param"></param>
        /// <returns>DataTable</returns>
        public DataTable GetErrorTypesTableWithCount(Dictionary<string, string> param)
        {
            DataTable table = null;
            try
            {
                 table = dalerrorobj.GetErrorTypesTableWithCount(param);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "PMPTool", System.Windows.Forms.MessageBoxButtons.OK);
            }

            return table;
        }

        /// <summary>
        /// Delete Record for given PCBAID and production stage.
        /// </summary>
        /// <param name="pcbaid"></param>
        /// <param name="stage"></param>
        /// <returns>boolean</returns>
        public bool DeleteRecord_PCBAID_Stage(string pcbaid, string stage)
        {
            bool bSuccess = false;
            try
            {
                bSuccess = dalerrorobj.DeleteRecord_PCBAID_Stage(pcbaid, stage);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "PMPTool", System.Windows.Forms.MessageBoxButtons.OK);
            }
            return bSuccess;
        }

        /// <summary>
        /// Gets Password string from database using given keyword.
        /// </summary>
        /// <returns>string</returns>
        public string GetPasswordsForEntryModification(string keyword)
        {
            string result = string.Empty;
            try
            {
                var table = dalerrorobj.GetPasswordsForEntryModification();
                if (table != null && table.Rows.Count > 0)
                {
                    var key = table.Rows[0][0].ToString().Split(';').First(s=>s.Contains(keyword));
                    if (!string.IsNullOrEmpty(key)) result = key.Split('=')[1];
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }
}
