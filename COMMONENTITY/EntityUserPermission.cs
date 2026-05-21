using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMMONENTITY
{
   public class EntityUserPermission
    {
       bool newRunProcedure;
       bool newCreateProcedure;
       bool newCreateProgram;
       bool openProcedure;
       bool openProgram; 
       bool importMeterIDList;
       bool importProcedureFile;
       bool exportProcedureFile;
       bool settingsAssociation;
       bool settingUserManagement;
       bool settingChangePassword;
       bool executionReports;

       public bool WewRunProcedure
       {
           get { return newRunProcedure; }
           set { newRunProcedure = value; }
       }
       public bool NewCreateProcedure
       {
           get { return newCreateProcedure; }
           set { newCreateProcedure = value; }
       }
       public bool NewCreateProgram
       {
           get { return newCreateProgram; }
           set { newCreateProgram = value; }
       }
       public bool OpenProcedure
       {
           get { return openProcedure; }
           set { openProcedure = value; }
       }
       public bool OpenProgram
       {
           get { return openProgram; }
           set { openProgram = value; }
       }

       public bool ImportMeterIDList
       {
           get { return importMeterIDList; }
           set { importMeterIDList = value; }
       }
       public bool ImportProcedureFile
       {
           get { return importProcedureFile; }
           set { importProcedureFile = value; }
       }
       public bool ExportProcedureFile
       {
           get { return exportProcedureFile; }
           set { exportProcedureFile = value; }
       }
       public bool SettingsAssociation
       {
           get { return settingsAssociation; }
           set { settingsAssociation = value; }
       }
       public bool SettingUserManagement
       {
           get { return settingUserManagement; }
           set { settingUserManagement = value; }
       }
       public bool SettingChangePassword
       {
           get { return settingChangePassword; }
           set { settingChangePassword = value; }
       }
       public bool ExecutionReports
       {
           get { return executionReports; }
           set { executionReports = value; }
       }    

    }
}
