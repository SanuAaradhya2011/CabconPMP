using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
 
using System.Xml;

namespace LGRTL
{
    public class ConnectionClass
    {
        string connectionString = "";
        XMLExportImport objxml = new XMLExportImport();
        public OleDbConnection Connection;
        public OleDbConnection Conn;
        public SqlConnection Sqlconn;
       
        public void OpenConn()
        {

            Connection = new OleDbConnection();
            //The Following connection string is for the Access 2007 only if the format is also for the MS Access 2007.
            //Connection.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\BCSStarlight.accdb; Jet OLEDB:Database Password=Password12;";

            //The following string is for the access 2002-2003 format.
            //-----------For Production---------------------------
             Connection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "db" + "\\AemCalData.mdb;Jet OLEDB:Database Password=;";
            //------------------For Testing-------------------------------
             // Connection.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "db" + "\\AemCalData.mdb;Jet OLEDB:Database Password=;";
             //Connection.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "db" + "\\AemCalData.accdb;Jet OLEDB:Database Password=;";
            // Connection.ConnectionString = ConfigurationSettings.AppSettings["ConnectionString"].ToString();
            if (Connection.State.ToString() == "Closed") Connection.Open();
        }

        public void CloseConn()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
                Connection.Dispose();
            }
        }

        public bool sqlOpenConn()
        {
            try
            {
                connectionString = objxml.GetConnectionString();
                if (connectionString.Length > 0)
                {
                    Sqlconn = new SqlConnection(connectionString);
                    if (Sqlconn.State.ToString() == "Closed") Sqlconn.Open();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                Console.WriteLine("Error : Unable To Connect DataBase Server !");
                return false;
            }

        }

        public void sqlCloseConn()
        {
            if (Sqlconn.State == ConnectionState.Open)
            {
                Sqlconn.Close();
                Sqlconn.Dispose();
            }
        }

       

        #region ExecuteNonQuery
        public bool ExecuteNonQuery(string sp_name, SqlParameter[] param)
        {
            try

            {
                if (!sqlOpenConn()) return false;
                SqlCommand cmd = new SqlCommand(sp_name, Sqlconn);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

               int rr= cmd.ExecuteNonQuery();
              
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                sqlCloseConn();
            }
        }
        #endregion

        #region ExecuteDataSet
        public DataSet ExecuteDataSet(string sp_name, SqlParameter[] param)
        {
            DataSet ds = new DataSet();
            try
            {
                if (!sqlOpenConn()) return null;
                SqlCommand cmd = new SqlCommand(sp_name, Sqlconn);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }
                SqlDataAdapter adp = new SqlDataAdapter(cmd);


                adp.Fill(ds);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                CloseConn();
            }
            return ds;
        }

        public DataSet ExecuteDataSetonPara(string sp_name, SqlParameterCollection param)
        {
            DataSet ds = new DataSet();
            try
            {
                sqlOpenConn();
                SqlCommand cmd = new SqlCommand(sp_name, Sqlconn);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < param.Count; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }
                SqlDataAdapter adp = new SqlDataAdapter(cmd);


                adp.Fill(ds);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                sqlCloseConn();
            }
            return ds;
        }
        #endregion

        #region ExecuteNonQuery with TableValued Parameters

        public bool ExecuteNonQuery(string sp_name, SqlParameter[] param, DataTable table)
        {
            try
            {
                sqlOpenConn();
                SqlCommand cmd = new SqlCommand(sp_name, Sqlconn);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < param.Length - 1; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }
                //SqlParameter tvpParam = cmd.Parameters.AddWithValue("@tvpNewCategories", table);

                param[param.Length - 1].Value = table;

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                sqlCloseConn();
            }
        }
        #endregion

        #region ExecuteScaler
        public string ExecuteScaler(string sp_name, SqlParameter[] param)
        {
           
            try
            {
                sqlOpenConn();
                SqlCommand cmd = new SqlCommand(sp_name, Sqlconn);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }
                return(Convert.ToString(cmd.ExecuteScalar()));
                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                sqlCloseConn();
            }
           
        }

        public string ExecuteScaleronDataTable(DataTable mytable)
        {
             SqlBulkCopy bulkcopy = new SqlBulkCopy(connectionString);
            //I assume you have created the table previously          
            bulkcopy.DestinationTableName = mytable.TableName;
            try
            {
                bulkcopy.WriteToServer(mytable);
                return "";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }          
            finally
            {
                sqlCloseConn();
            }
           
        }


        #endregion

        #region ExecuteScalar return object
        public object ExecuteScalar(string sp_name, SqlParameter[] param)
        {
            object result = "";
            try
            {
                sqlOpenConn();
                SqlCommand cmd = new SqlCommand(sp_name, Sqlconn);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }
                result = cmd.ExecuteScalar();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (Sqlconn.State == ConnectionState.Open)
                {
                    sqlCloseConn();
                }
            }
        }
        #endregion

        #region ExecuteReader return SqlDataReader
        public SqlDataReader ExecuteReader(string sp_name, SqlParameter[] param)
        {
            try
            {
                sqlOpenConn();
                SqlCommand cmd = new SqlCommand(sp_name, Sqlconn);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                sqlCloseConn();
            }
        }
        #endregion

         
        public bool UploadRoutineTestResultd(LGRTL.CommonEntity objentitypro, DataTable dtexr, DataTable dtcal)
        {
            SqlParameter[] param ={             
            new SqlParameter("@CalibrationResult",dtcal),
            new SqlParameter("@ExecutionResult",dtexr),           
            new SqlParameter("@MeterType",objentitypro.MeterType),
            new SqlParameter("@ProcedureType",objentitypro.ProcedureType),
            new SqlParameter("@executionDate",objentitypro.ExecutionDate),

            };

            
            return (ExecuteNonQuery("Pro_Upload_RoutineTestResultByBatch", param));

        }
      
        public DataTable GetCalibrationDataTable()
        {
             try
            {
                DataSet ds = new DataSet();
                SqlParameter[] param ={             
               
                                      };
                ds = ExecuteDataSet("[Pro_GetCalibrationDetails]", param);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public DataTable Select_GetExecutionResult_onPCBAID(CommonEntity objtemp)
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = { new SqlParameter("@PCBAID",objtemp.PCBAID.Trim())
                                   };
            ds = ExecuteDataSet("[Pro_GetExecutionResult_onPCBAID]", param);
            return ds.Tables[0];

        }
    }
}
