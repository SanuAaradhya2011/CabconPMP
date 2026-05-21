using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DALLAYER;
namespace BALLAYER
{
    public class BALRoutineTestReport
    {
        DALGolbalAdapter objprodal = new DALGolbalAdapter();

        public BALRoutineTestReport()
        { 
        }       

        
        public DataTable getExecutionResult(COMMONENTITY.EntityRoutineTestReport _entityroutinetest)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] param ={             
                                            new SqlParameter("@metertype", _entityroutinetest.mmetertype),
                                            new SqlParameter("@startdt", _entityroutinetest.mfromdate),
                                            new SqlParameter("@enddt", _entityroutinetest.mtodate)
                                      };
                ds = objprodal.ExecuteDataSet("Pro_GetCalibReport_MeterType_DateRange", param);
              //  ds = objprodal.ExecuteDataSet("Pro_GetCalibReport_MeterType_DateRange_FromBackUp", param);
                
                return ds.Tables[0];
            }
            catch (Exception)
            {
                return null;
            }
        }
         
    }
}
