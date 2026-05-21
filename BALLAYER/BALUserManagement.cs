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
   public class BALUserManagement
    {
       DALGolbalAdapter objprodal = new DALGolbalAdapter();
       public DataSet Select_LoginUseronUserID(EntityUserManagement objentitypro)
       {
           SqlParameter[] param = { new SqlParameter("@UserID", objentitypro.LoginuserID) };
           return (objprodal.ExecuteDataSet("Pro_GetLoginUseronUserID", param));

       }

       public DataSet Select_LoginUseronUserIDandPWD(EntityUserManagement objentitypro)
       {
           SqlParameter[] param = { new SqlParameter("@UserID", objentitypro.LoginuserID)
                                  ,new SqlParameter("@UserPassword", objentitypro.Loginpassword)};
           return (objprodal.ExecuteDataSet("Pro_GetLoginUseronUserIDandPwd", param));

       }

        public DataSet Select_UserDetails()
       {
           SqlParameter[] param = { };
           return (objprodal.ExecuteDataSet("Pro_GetLoginUserDetails", param));

       }

       public string Insert_UserDetails(EntityUserManagement objentitypro)
       {
           SqlParameter[] param = { new SqlParameter("@UserID", objentitypro.LoginuserID) 
                                   ,new SqlParameter("@UserPassword", objentitypro.Loginpassword)
                                   ,new SqlParameter("@UserType", objentitypro.LogType)
                                   ,new SqlParameter("@Reserved1", "")
                                   ,new SqlParameter("@Reserved2", "")
                                   ,new SqlParameter("@Reserved3", "")
                                   ,new SqlParameter("@Reserved4", "")
                                   ,new SqlParameter("@Reserved5", "")
       };
           return (objprodal.ExecuteInsertQuery("Pro_InsTabUserManagement", param));

       }
    }
}
