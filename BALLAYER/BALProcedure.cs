using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using COMMONENTITY;
using DALLAYER;
namespace BALLAYER
{
   public class BALProcedure
    {
      
       DALGolbalAdapter objprodal = new DALGolbalAdapter();
       
       public string InsertinToTabProcedureMasterByBatch(EntityProcedure objentitypro, DataTable executionResultClone)
       {  
            if (executionResultClone.Rows.Count <= 0) return "Nothing To Save !" + "\n" + "Please Create a Valid Test Procedure and Try Again !";
            SqlParameter[] param ={             
            new SqlParameter("@ProcedureMaster", executionResultClone),              
            new SqlParameter("@ProcedureName",objentitypro.ProcedureName),
            new SqlParameter("@ProgramName",objentitypro.ProgramName)
            };
            string resultmsg = objprodal.ExecuteInsertQuery("Pro_Insert_tabProcedureMasterBatch", param);
            if (resultmsg != "") return "Unable To Save !" + "\n" + resultmsg;
            return "";
       }

       //public string InsertinToTabProgramMasterByBatch(DataTable programMasterClone)
       //{
       //    SqlParameter[] param ={             
       //     new SqlParameter("@ProgramMaster", programMasterClone)             
                         
       //     };
       //    return objprodal.ExecuteInsertQuery("Pro_Insert_tabProgramMasterBatch", param);
       //}
       public DataSet SelectDistinctFromTabProcedureMaster(EntityProcedure objentitypro)
       {
          // SqlParameter[] param ={ };
           // return (objprodal.ExecuteDataSet("Pro_SelecttabProMaster_DistinctProcedureName", param));
           SqlParameter[] param = { new SqlParameter("@ProcedureName", objentitypro.ProcedureName) };
           return (objprodal.ExecuteDataSet("Pro_SelecttabProMaster_DistinctProcedureNameLike", param));
       }

       public DataSet SelectFromTabProcedureMasteronProcedureNAme(EntityProcedure objentitypro)
       {
           SqlParameter[] param = { new SqlParameter("@ProcedureName", objentitypro.ProcedureName) };
           return (objprodal.ExecuteDataSet("Pro_SelecttabProMaster_onProcedureName", param));

       }

       //public DataSet Select_frm_TProcedureMaster_NOTin_TProgramMaster_ON_ProgName(EntityProcedure objentitypro)
       //{
       //    SqlParameter[] param = { new SqlParameter("@ProgramName", objentitypro.ProgramName) };
       //    return (objprodal.ExecuteDataSet("Pro_GetParaName_frm_TProcedureMaster_NOTin_TProgramMaster_ON_ProgName", param));

       //}

       public string DeleteTabProcedureMasteronProcedureName(EntityProcedure objentitypro)
       {
           SqlParameter[] param = { new SqlParameter("@ProcedureName", objentitypro.ProcedureName) };
           return (objprodal.ExecuteInsertQuery("Pro_DeltabProMaster_onProcedureNam", param));

       }

    }
}
