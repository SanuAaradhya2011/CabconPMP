using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Utilities;
using System.Diagnostics;
using ApplicationInterface;
using COMMONENTITY;
using System.Globalization;
using BALLAYER;
using SystemSecurityLibrary;
using LGPMPREJECTIONTOOL;
using LGPMPSYNCSERVICE;
namespace CabconPMP
{
    public partial class frmMain : Form
    {
        GlobalMethods objsv = new GlobalMethods();
        AppSettings objappSettings = new AppSettings();
        MyCrypro objcrypt = new MyCrypro();
        XMLExportImport obgxml = new XMLExportImport();
        EntityUserManagement objetyusermgt = new EntityUserManagement();
        string logedUserID = string.Empty;
        string logedUserType = string.Empty;
        int getUserIndex = 0;
        public frmMain(EntityUserManagement objetyum)
        {
            InitializeComponent();
            objetyusermgt.LoginuserID = objetyum.LoginuserID;
            objetyusermgt = objetyum;
            logedUserType = objetyum.LogType;
        }
        public void MainForm_UpdateMsg(object sender, UpdateEventArgs e)
        {
            if (e.isError) dlmsCommStatusmsh.ForeColor = Color.Red;
            else dlmsCommStatusmsh.ForeColor = Color.Green;
            dlmsCommStatusmsh.Text = e.msg;
            Application.DoEvents();

        }  
        private void tsm_Association_Click(object sender, EventArgs e)
        {
            Association frmasso = new Association();
            frmasso.ShowDialog();
        }
 

        private void SM110frmMain_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
           // getUserIndex = StaticVariables.ApplicationUserType.IndexOf(objetyusermgt.LogType);
           // objetyusermgt.LoginTypeIndex = (byte)getUserIndex;
           // DisplayMainStatus();
           // SetUserPermission();
			//SetUserPermissionRejectionList();
        }
        public void DisplayMainStatus()
        {
            try
            {
                dlmsCommStatusmsh.Text = "";
                dlmsCommStatusmsh.Width = (DLMSStas.Width / 6) ;
                dlmsCommStatusmsh2.Text = "";
                dlmsCommStatusmsh2.Width = (DLMSStas.Width / 7);
                //------------------------------DLMS Mode Settings----------------------
                string ClientSAP = Convert.ToInt32(objappSettings.GetClientSAP(), 10).ToString("X");
                string SerialPort = objappSettings.GetPortName() + ", " + objappSettings.GetDatabits() + ", " + objappSettings.GetParity() + ", " + objappSettings.GetStopBits();
                string dlmscommmode = "";

                if (ClientSAP == "10") dlmscommmode += " PC ";                   
                else if (ClientSAP == "20") dlmscommmode += " MR ";
                else if (ClientSAP == "30") dlmscommmode += " US ";
                else if (ClientSAP == "40") dlmscommmode += " FS ";

                dlmscommmode += "| " + SerialPort;
                //--------------------------------------------------------------------


                string Communication = string.Empty;
                string Company_Profile = string.Empty;
                string filename = AppDomain.CurrentDomain.BaseDirectory + @"\CabconPMP.exe";
                string strbuilton = "Built On: " + File.GetCreationTime(filename).Day.ToString("00") + "/" + File.GetCreationTime(filename).Month.ToString("00") + "/" + File.GetCreationTime(filename).Year.ToString("00");

                string strtemp = "";
                string connectionString = strtemp = obgxml.GetConnectionString();
                
                if (connectionString.IndexOf("INDELNB") >= 0) connectionString = "Location : Local System";
                else if (connectionString.IndexOf("INDEL") >= 0) connectionString = "Location : GDC Noida" ;
                else if (connectionString.IndexOf("INBDI") >= 0) connectionString = "Location : Baddi Factory";
                else if (connectionString.IndexOf("INCCU") >= 0) connectionString = "Location : Joka Factory";
                else connectionString = "Location : Others Factory";

                connectionString += " ( " + strtemp.Substring(strtemp.IndexOf('=') + 1, strtemp.IndexOf(';') - strtemp.IndexOf('=') - 1) + " )"; 

                bool Flg_ComSetting = true; 
                if (Flg_ComSetting)
                {
                    Communication = dlmscommmode;

                }
                string Prduct_Version = SystemInfo.ProductVersion();                 
                string strpath = AppDomain.CurrentDomain.BaseDirectory + "CabconPMP.exe";
                Company_Profile = SystemInfo.CopyRightsDetail();
                lblMainScreenMsg.Text = "L+G  Product Management Program Ver. " + Prduct_Version +  "\n" + "            " + connectionString;
                Prduct_Version = Application.ProductName.ToString() + " Ver. " + Prduct_Version;
                stsReady.Text = "Association : " + dlmscommmode;
                lblversion.Text = Prduct_Version + " | " + strbuilton ;
                lblversion.Text = lblversion.Text ;
                lblLoginInfo.Text = "Login Mode : " + objetyusermgt.LogType.ToUpper() + " | " + connectionString;
                
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Setting Main Form Status Failed !" + "\r\n" + "\r\n" + Ex.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
            }
        }

		 private void SetUserPermissionRejectionList()
        {
            try
            {
                ToolStripItem[] ts = new ToolStripItem[] { tsm_rejectlist, tsm_newrejection, tsm_newrejectioncompact, tss_Reject, tss_Error };
                List<int> permissionGivenIdx = new List<int>();

                switch (getUserIndex)
                {
                    case (int)COMMONENTITY.StaticVariables.userCategory.UserRework:
                    case (int)COMMONENTITY.StaticVariables.userCategory.Useradministrator:
                    case (int)COMMONENTITY.StaticVariables.userCategory.Usersupervisor:
                    case (int)COMMONENTITY.StaticVariables.userCategory.UserPoweradministrator:
                        break;
                    case (int)COMMONENTITY.StaticVariables.userCategory.Uservendor:
                    case (int)COMMONENTITY.StaticVariables.userCategory.Useroperator:
                    case (int)COMMONENTITY.StaticVariables.userCategory.UserValidation:
                    case (int)COMMONENTITY.StaticVariables.userCategory.Userreader:
                        permissionGivenIdx.Add(0);
                        permissionGivenIdx.Add(1);
                        permissionGivenIdx.Add(2);
                        permissionGivenIdx.Add(3);
                        permissionGivenIdx.Add(4);
                        break;
                }
                int displayIdx = 0;
                while (displayIdx < permissionGivenIdx.Count)
                {
                    ts[permissionGivenIdx[displayIdx]].Visible = false;
                    switch (permissionGivenIdx[displayIdx])
                    {
                        case 0:
                            toolStripLabelReject.Visible = false;
                            toolStripLabelError.Visible = false;
                            break;
                        case 2:
                            break;
                    }
                    displayIdx++;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Setting Main Form Status Failed !" + "\r\n" + "\r\n" + ex.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
            }
        }
		
        private void SetUserPermission()
        {
            try
            {
                
                ToolStripMenuItem[] ts = new ToolStripMenuItem[] { runProcedureToolStripMenuItem, tsm_createProcedure, tsm_programList, tsm_procedureopen, programListToolStripMenuItem, tsm_Import_TestProcedureFile, tsm_Export_TestProcedureFile, tsm_Association, tsm_userManagement, tsm_changePassword, tsm_executionReports,tsm_ServerSettings };
                List<int> permissionGivenIdx = new List<int>();

                switch (getUserIndex)
                {                  
                        
                    case (int)COMMONENTITY.StaticVariables.userCategory.Uservendor:
                    case (int)COMMONENTITY.StaticVariables.userCategory.Useroperator:
                    case (int)COMMONENTITY.StaticVariables.userCategory.UserRework: 
                        permissionGivenIdx.Add(1);
                        permissionGivenIdx.Add(2);
                        permissionGivenIdx.Add(3);
                        permissionGivenIdx.Add(5);
                        permissionGivenIdx.Add(6);
                        permissionGivenIdx.Add(4);
                        permissionGivenIdx.Add(8);
                        permissionGivenIdx.Add(11);
                        break;
                    case (int)COMMONENTITY.StaticVariables.userCategory.Usersupervisor:

                        permissionGivenIdx.Add(1);
                        permissionGivenIdx.Add(2);
                        permissionGivenIdx.Add(8);
                        permissionGivenIdx.Add(11);
                        break;
                    case (int)COMMONENTITY.StaticVariables.userCategory.Userreader:
                        permissionGivenIdx.Add(0);
                        permissionGivenIdx.Add(1);
                        permissionGivenIdx.Add(2);
                        permissionGivenIdx.Add(3);
                        permissionGivenIdx.Add(4);
                        permissionGivenIdx.Add(5);
                        permissionGivenIdx.Add(6);
                        permissionGivenIdx.Add(7);
                        permissionGivenIdx.Add(8);
                        permissionGivenIdx.Add(9);
                        permissionGivenIdx.Add(10);
                        permissionGivenIdx.Add(11);
                        permissionGivenIdx.Add(12);
                        break;
                    case (int)COMMONENTITY.StaticVariables.userCategory.Useradministrator:
                    case (int)COMMONENTITY.StaticVariables.userCategory.UserValidation:
                        permissionGivenIdx.Add(8);
                        //permissionGivenIdx.Add(11);//Server setting
                        break;
                    case (int)COMMONENTITY.StaticVariables.userCategory.UserPoweradministrator:
                        break;
                }
                int displayIdx = 0;
                while (displayIdx < permissionGivenIdx.Count)
                {
                    ts[permissionGivenIdx[displayIdx]].Visible = false;
                    switch (permissionGivenIdx[displayIdx])
                    {
                        case 0:
                            cms_RunProcedure.Visible = false;
                            ts_ico_TestRun.Visible = false;
                            tss_RunProcedure.Visible = false;
                            break;
                        case 1:
                            cms_CreateProcedure.Visible = false;
                            break;
                        case 2:
                            cms_CreateProgram.Visible = false;
                            break;
                        case 3:
                            cms_openProcedure.Visible = false;
                            break;
                        case 4:
                            cms_OpenProgram.Visible = false;
                            break;                                           
                        case 8:
                            ts_ico_Association.Visible = false;
                            tss_Association.Visible = false;
                            break;                      
                        case 11:
                            ts_ico_report.Visible = false;
                            tss_Report.Visible = false;
                            break;
                        case 12:
                            tsm_ServerSettings.Visible = false;
                            break;

                    }
                    displayIdx++;
                }
            }
            catch (Exception)
            {
            }

        }
        private void DefaultPermission()
        {
            tsm_New.Visible = false;
            tsm_openProcedure.Visible = false;
             

        }
        private void ts_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SM110frmMain_Activated(object sender, EventArgs e)
        {
           // DisplayMainStatus();
			FormCollection fc = Application.OpenForms;
            foreach (Form frm in fc)
            {
                //iterate through
                string[] formNames = { "frmChooseSaveColumns", "frmRecordHistory", "frmUpdateEntry", "frmHardError" };
                if (formNames.Any(c => frm.Name.Contains(c)))
                    frm.BringToFront();
            }										   
        }

        private void tsm_createProcedure_Click(object sender, EventArgs e)
        {

            frmProcedure objprcreat = new frmProcedure("", objetyusermgt);
            objprcreat.UpdateMsg += new frmProcedure.UpdateMainMsgHandler(MainForm_UpdateMsg);
            objprcreat.ShowDialog();
        }

        private void tsm_procedureopen_Click(object sender, EventArgs e)
        {
            frmProcedureOpen objpropen = new frmProcedureOpen("Open", objetyusermgt);
            objpropen.UpdateMsg += new frmProcedureOpen.UpdateMainMsgHandler(MainForm_UpdateMsg);
            objpropen.ShowDialog();
        }

        private void runProcedureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProcedureOpen objpropen = new frmProcedureOpen("Run", objetyusermgt);
            objpropen.UpdateMsg += new frmProcedureOpen.UpdateMainMsgHandler(MainForm_UpdateMsg);
            objpropen.ShowDialog();
        }

        private void tsm_programList_Click(object sender, EventArgs e)
        {
            frmProgramLists objprlist = new frmProgramLists("", objetyusermgt);
            objprlist.ShowDialog();
        }

        private void programListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProgramListOpen objprlistop = new frmProgramListOpen(objetyusermgt);
            objprlistop.ShowDialog();
        }
 

        private void tsm_executionReports_Click(object sender, EventArgs e)
        {
            frmResultsReport objrr = new frmResultsReport();
            objrr.ShowDialog();
        }

        
        private void toolStripLabel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                cms_New.Show(PointToScreen(e.Location));
            }
        }

        private void cms_openProcedure_Click(object sender, EventArgs e)
        {
            frmProcedureOpen objpropen = new frmProcedureOpen("Open", objetyusermgt);
            objpropen.UpdateMsg += new frmProcedureOpen.UpdateMainMsgHandler(MainForm_UpdateMsg);
            objpropen.ShowDialog();
        }

        private void cms_OpenProgram_Click(object sender, EventArgs e)
        {
            frmProgramListOpen objprlistop = new frmProgramListOpen(objetyusermgt);
            objprlistop.ShowDialog();
        }

        private void toolStripLabel2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                cms_Open.Show(PointToScreen(e.Location));
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmProcedureOpen objpropen = new frmProcedureOpen("Run", objetyusermgt);
            objpropen.UpdateMsg += new frmProcedureOpen.UpdateMainMsgHandler(MainForm_UpdateMsg);
            objpropen.ShowDialog();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmProcedure objprcreat = new frmProcedure("", objetyusermgt);
            objprcreat.UpdateMsg += new frmProcedure.UpdateMainMsgHandler(MainForm_UpdateMsg);
            objprcreat.ShowDialog();
        }

        private void createProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProgramLists objprlist = new frmProgramLists("", objetyusermgt);
            objprlist.ShowDialog();
        }

        private void ts_FunctionalTest_Click(object sender, EventArgs e)
        {
            frmProcedureOpen objpropen = new frmProcedureOpen("Run", objetyusermgt);
            objpropen.UpdateMsg += new frmProcedureOpen.UpdateMainMsgHandler(MainForm_UpdateMsg);
            objpropen.ShowDialog();
        }

        
        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
           
            frmResultsReport objrr = new frmResultsReport();
            objrr.ShowDialog();
        }

        private void ts_Association_Click(object sender, EventArgs e)
        {
            Association frmasso = new Association();
            frmasso.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        public void RemoveInstances()
        {
            try
            {
            List<string> currPrsName = objsv.ProgramNameList.ToList();
            Process currentappProcess = Process.GetCurrentProcess();
            System.Diagnostics.Process[] prs = System.Diagnostics.Process.GetProcesses();
            /*-------------------Dependency process Kill---------------------*/
            foreach (System.Diagnostics.Process proces in prs)
            {
                if (currPrsName.Contains(proces.ProcessName.ToUpper()))
                {                   
                    proces.Refresh();
                    if (!proces.HasExited)
                        proces.Kill();
                }
            }
            //------------------Main Application Process Kill-----------------*/            
                currentappProcess.Kill();
            
            }
            catch (Exception)
            {
            }
        }
        private void RemovingTepFiles()
        {
            try
            {
                string[] parmanentFiles = new string[] { "DbConnection" };
                string directorypath = AppDomain.CurrentDomain.BaseDirectory + "Configuration\\";
                string[] filePathList = Directory.GetFiles(directorypath, "*.xml");
                int filepathCounts = 0;
                for (filepathCounts = 0; filepathCounts < filePathList.Length; filepathCounts++ )
                {
                    string readfileName = filePathList[filepathCounts].Substring(filePathList[filepathCounts].LastIndexOf("\\") + 1).ToUpperInvariant();
                    if (readfileName.IndexOf(parmanentFiles[0].ToUpperInvariant()) >= 0)continue;                 
                    File.Delete(filePathList[filepathCounts]);
                }
            }
            catch (Exception)
            {
            }

        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            RemovingTepFiles();
            RemoveInstances();
            
        }

        private void tsm_Import_MeterIDLists_Click(object sender, EventArgs e)
        {
            frmMeterRange objipmid = new frmMeterRange();
            objipmid.ShowDialog();
        }

       
        private void tsm_Run_Click(object sender, EventArgs e)
        {
            frmProcedureOpen objpropen = new frmProcedureOpen("Run", objetyusermgt);
            objpropen.UpdateMsg += new frmProcedureOpen.UpdateMainMsgHandler(MainForm_UpdateMsg);
            objpropen.ShowDialog();
        }

        private void tsm_Import_TestProcedureFile_Click(object sender, EventArgs e)
        {
            frmProcedureOpen objpropen = new frmProcedureOpen("Import", objetyusermgt);
            objpropen.UpdateMsg += new frmProcedureOpen.UpdateMainMsgHandler(MainForm_UpdateMsg);
            objpropen.ShowDialog();
        }

        private void tsm_Export_TestProcedureFile_Click(object sender, EventArgs e)
        {
            frmProcedureOpen objpropen = new frmProcedureOpen("Export", objetyusermgt);
            objpropen.UpdateMsg += new frmProcedureOpen.UpdateMainMsgHandler(MainForm_UpdateMsg);
            objpropen.ShowDialog();
        }        
 
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            frmProcedureOpen objpropen = new frmProcedureOpen("Import", objetyusermgt);
            objpropen.UpdateMsg += new frmProcedureOpen.UpdateMainMsgHandler(MainForm_UpdateMsg);
            objpropen.ShowDialog();
        }

        private void ts_ico_ImportID_Click(object sender, EventArgs e)
        {
            frmMeterRange objipmid = new frmMeterRange();
            objipmid.ShowDialog();
        }
 
        private void userManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserManagement objum = new frmUserManagement("New", objetyusermgt);
            objum.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserManagement objum = new frmUserManagement("PWD", objetyusermgt);
            objum.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutLGPMP objabout = new AboutLGPMP();
            objabout.ShowDialog();
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Help.ShowHelp(this, AppDomain.CurrentDomain.BaseDirectory + "\\Configuration\\L+G_PMP.chm");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable To open Help File !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ts_Help_Click(object sender, EventArgs e)
        {
            try
            {
                Help.ShowHelp(this, AppDomain.CurrentDomain.BaseDirectory + "\\Configuration\\L+G_PMP.chm");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable To open Help File !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tsm_ServerSettings_Click(object sender, EventArgs e)
        {
            frmServerSettings objserversett = new frmServerSettings(true);
            objserversett.ShowDialog();
        }

        private void tsm_productionStageReport_Click(object sender, EventArgs e)
        {
            frmProductionStausReport objpsr = new frmProductionStausReport();
            objpsr.ShowDialog();
        }

        private void routineTestReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report.frmRoutineTestReport objroutinetest = new CabconPMP.Report.frmRoutineTestReport(objetyusermgt);
            objroutinetest.ShowDialog();
        }

        private void tsm_missingMeterReport_Click(object sender, EventArgs e)
        {
            frmMissingMeterReport objmissmeterrpt = new frmMissingMeterReport();
            objmissmeterrpt.ShowDialog();
        }

        private void tsm_parametersWiseReport_Click(object sender, EventArgs e)
        {
            frmReportTestWise objrtw = new frmReportTestWise();
            objrtw.ShowDialog();
        }

        private void tsm_rutineTestReportFileFormat_Click(object sender, EventArgs e)
        {
            frmImportRTRFormat objrtr = new frmImportRTRFormat();
            objrtr.ShowDialog();
            
        }

        private void backupDataReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBackupDataResultsReport objbdr = new frmBackupDataResultsReport();
            objbdr.ShowDialog();
            
        }

        private void toolStripLabelSync_Click(object sender, EventArgs e)
        {
            FrmSyncLogList frmobj = new FrmSyncLogList();
            if (!frmobj.IsDisposed)
                frmobj.ShowDialog();
            //PMPExSyncService service = new PMPExSyncService();
            //service.Do_Work(new object());
        }
 
 
        private void toolStripLabelReject_Click(object sender, EventArgs e)
        {
            frmErrorList.ShowInstance(objetyusermgt);
            //frmErrorList objfrmErrList = frmErrorList.GetInstance(objetyusermgt);
            //objfrmErrList.Show();
            //objfrmErrList.Activate();
        }

        private void toolStripLabelError_Click(object sender, EventArgs e)
        {
            LGPMPREJECTIONTOOL.AccessPassword frmobj = new LGPMPREJECTIONTOOL.AccessPassword(objetyusermgt);
            frmobj.ShowDialog();
        }

        private void tsm_newrejection_Click(object sender, EventArgs e)
        {
            frmUpdateEntry objfrm = new frmUpdateEntry(objetyusermgt);
            objfrm.ShowDialog();
        }

        private void tsm_rejectlist_Click(object sender, EventArgs e)
        {
            toolStripLabelReject_Click(new object(), EventArgs.Empty);
        }

        private void rejectionEntryCompactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripLabelError_Click(new object(), EventArgs.Empty);
        }
    }
}
