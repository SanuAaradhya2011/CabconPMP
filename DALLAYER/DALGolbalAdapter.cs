using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
namespace DALLAYER
{
    public class DALGolbalAdapter
    {
        ConnectionClass objcon = new ConnectionClass();
        public string ExecuteInsertQuery(string InsertProcedureName, SqlParameter[] param)
        {
            return(objcon.ExecuteScaler(InsertProcedureName, param));             
        }
        public DataSet ExecuteDataSet(string selectProcedureName, SqlParameter[] param)
        {
            return (objcon.ExecuteDataSet(selectProcedureName, param));             
        }       
         
    }
}
