using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DALLAYER;
using System.Data.SqlClient;
using System.Data;
using COMMONENTITY;
namespace BALLAYER
{
   public class BALPrograms
    {
        DALGolbalAdapter objprodal = new DALGolbalAdapter();
         
        public string InsertinToTabProgramMasterByBatch(EntityPrograms objentityprog, DataTable executionResultClone)
        {
            if (executionResultClone.Rows.Count <= 0) return "Nothing To Save !" + "\n" + "Please Create a Valid Test Procedure and Try Again !";
            SqlParameter[] param ={             
            new SqlParameter("@ProgramMaster", executionResultClone),              
            new SqlParameter("@ProgramName",objentityprog.ProgramName)           
            };
            string resultmsg = objprodal.ExecuteInsertQuery("Pro_Insert_tabProgramMasterBatch", param);
            if (resultmsg != "") return "Unable To Save !" + "\n" + resultmsg;
            return "";
        }

        public DataSet SelectDistinctFromTabProgramMaster()
        {
            SqlParameter[] param = { };
            return (objprodal.ExecuteDataSet("Pro_SelecttabProgramMaster_DistinctProgName", param));

        }

        public DataSet SelectFromTabProcedureMasteronProcedureNameonProgramName(EntityPrograms objentityprog)
        {
            SqlParameter[] param = { new SqlParameter("@ProgramName", objentityprog.ProgramName) };
            return (objprodal.ExecuteDataSet("Pro_SelecttabProgramMaster_DistinctParaName_onProgramName", param));

        }

        public DataSet Pro_Select_tabProgramMaster_onMeterType(EntityPrograms objentityprog)
        {
            SqlParameter[] param = { new SqlParameter("@MeterType", objentityprog.TestType) };
            return (objprodal.ExecuteDataSet("Pro_Select_tabProgramMaster_onMeterType", param));

        }

        public DataSet SelecttabProgramMaster_onParametersName(EntityPrograms objentityprog)
        {
            SqlParameter[] param = { new SqlParameter("@ParametersName", objentityprog.ProgramParameterName) };
            return (objprodal.ExecuteDataSet("Pro_SelecttabProgramMaster_onParametersName", param));

        }
        
        public string DeleteTabProgramMaster_onProgramNameOnly(EntityPrograms objentityprog)
        {
            SqlParameter[] param = { new SqlParameter("@ProgramName", objentityprog.ProgramName)};
            return (objprodal.ExecuteInsertQuery("Pro_DelTabProgramMaster_onProgNameonly", param));

        }
    }
}
