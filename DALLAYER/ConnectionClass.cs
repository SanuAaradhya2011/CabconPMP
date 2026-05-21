using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Utilities;
using COMMONENTITY;
namespace DALLAYER
{
    public class ConnectionClass
    {
        string connectionString = "";
        XMLExportImport objxml = new XMLExportImport();
      
        public SqlConnection conn;
        // string connectionString = @"Server=INDELNB126\SQLEXPRESS1;Database=LGPMPDB;Integrated Security=fals;User id=sa;Password=Password12;Trusted_Connection=False;";
        //string connectionString = @"Server=INDELVS01;Database=LGPMPDB;Integrated Security=fals;User id=lgpmp;Password=pass@1234;Trusted_Connection=False;";
        // string  connectionString = @"Server=INBDISV04\FRSPMP;Database=LGPMPDB;Integrated Security=fals;User id=lgpmp;Password=pass@1234;Trusted_Connection=False;";
       
        #region DataBaseConnection

        public bool OpenConn()
        {
            try
            {
                connectionString = objxml.GetConnectionString();
                if (connectionString.Length > 0)
                {
                    conn = new SqlConnection(connectionString);
                    if (conn.State.ToString() == "Closed") conn.Open();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public void CloseConn()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
            }
        }
       
        #endregion

        #region DataBaseInterface_Used_Functions

        public DataSet ExecuteDataSet(string sp_name, SqlParameter[] param)
        {
            DataSet ds = new DataSet();
            try
            {
                if (!OpenConn()) return null;
                SqlCommand cmd = new SqlCommand(sp_name, conn);
                cmd.CommandTimeout = 300;
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

        public string ExecuteScaler(string sp_name, SqlParameter[] param)
        {

            try
            {
                OpenConn();
                SqlCommand cmd = new SqlCommand(sp_name, conn);
                cmd.CommandTimeout = 300;
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }
                return (Convert.ToString(cmd.ExecuteScalar()));

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                CloseConn();
            }

        }


        #endregion

        #region DataBaseInterface_UnUsed_Functions
        
       
        //public void ExecuteNonQuery(string sp_name, SqlParameter[] param)
        //{
        //    try
        //    {
        //        OpenConn();
        //        SqlCommand cmd = new SqlCommand(sp_name, conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        for (int i = 0; i < param.Length; i++)
        //        {
        //            cmd.Parameters.Add(param[i]);
        //        }

        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    finally
        //    {
        //        CloseConn();
        //    }
        //}

        //public DataSet ExecuteDataSetonPara(string sp_name, SqlParameterCollection param)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        OpenConn();
        //        SqlCommand cmd = new SqlCommand(sp_name, conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        for (int i = 0; i < param.Count; i++)
        //        {
        //            cmd.Parameters.Add(param[i]);
        //        }
        //        SqlDataAdapter adp = new SqlDataAdapter(cmd);


        //        adp.Fill(ds);
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        CloseConn();
        //    }
        //    return ds;
        //}
       

        //public void ExecuteNonQuery(string sp_name, SqlParameter[] param, DataTable table)
        //{
        //    try
        //    {
        //        OpenConn();
        //        SqlCommand cmd = new SqlCommand(sp_name, conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        for (int i = 0; i < param.Length - 1; i++)
        //        {
        //            cmd.Parameters.Add(param[i]);
        //        }
        //        //SqlParameter tvpParam = cmd.Parameters.AddWithValue("@tvpNewCategories", table);

        //        param[param.Length - 1].Value = table;

        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    finally
        //    {
        //        CloseConn();
        //    }
        //}
        
       
 
        //public object ExecuteScalar(string sp_name, SqlParameter[] param)
        //{
        //    object result = "";
        //    try
        //    {
        //        OpenConn();
        //        SqlCommand cmd = new SqlCommand(sp_name, conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        for (int i = 0; i < param.Length; i++)
        //        {
        //            cmd.Parameters.Add(param[i]);
        //        }
        //        result = cmd.ExecuteScalar();
        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //        {
        //            CloseConn();
        //        }
        //    }
        //}
        
        //public SqlDataReader ExecuteReader(string sp_name, SqlParameter[] param)
        //{
        //    try
        //    {
        //        OpenConn();
        //        SqlCommand cmd = new SqlCommand(sp_name, conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        for (int i = 0; i < param.Length; i++)
        //        {
        //            cmd.Parameters.Add(param[i]);
        //        }

        //        SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //        return reader;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        public bool ExecuteNonQuery(string query)
        {
            if (OpenConn() == true)
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandTimeout = 300;
                cmd.ExecuteNonQuery();

            }
            else
                return false;
            CloseConn();
            return true;
        }

        public DataTable GetQueryResult(string query, string dataBase)
        {
            DataTable table = new DataTable();
            if (OpenConn() == true)
            {
                conn.ChangeDatabase(dataBase);
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandTimeout = 300;
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = cmd;
                dataAdapter.Fill(table);

            }
            else
                return null;
            CloseConn();
            return table;
        }

        public DataTable GetQueryResult(string query)
        {
            DataTable table = new DataTable();
            if (OpenConn() == true)
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandTimeout = 300;
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = cmd;
                dataAdapter.Fill(table);

            }
            else
                return null;
            CloseConn();
            return table;
        }
        #endregion
    }
}
