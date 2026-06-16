using ApplicationInterface;
using BALLAYER;
using CabconPMPREJECTIONTOOL;
using CabconPMPSYNCSERVICE;
using COMMONENTITY;
using DataLayer;
using SmartCalibration.DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemSecurityLibrary;
using Utilities;
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
        string[] mcmdarguments;
        LayerInterface objLI = new LayerInterface();
        List<RetryPortExecutionResult> m_retryExecutionSummary = new List<RetryPortExecutionResult>();
        private Panel pnlRetryRecordsHost;
        private frmRetryLiveTracking retryLiveTrackingForm;
        private frmRetryRecords retryRecordsForm;

        public frmMain(EntityUserManagement objetyum)
        {
            InitializeComponent(); COMMONENTITY.FormStyleHelper.Apply(this);
            objetyusermgt.LoginuserID = objetyum.LoginuserID;
            objetyusermgt = objetyum;
            logedUserType = objetyum.LogType;
            InitializeRetryViews();
        }
        public void MainForm_UpdateMsg(object sender, UpdateEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, UpdateEventArgs>(MainForm_UpdateMsg), sender, e);
                return;
            }

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
                string selectedPorts = "COM5";//SerialPortSettings.Default.SelectedPortsCsv;
                string SerialPort = (string.IsNullOrWhiteSpace(selectedPorts) ? objappSettings.GetPortName() : selectedPorts) + ", " + objappSettings.GetDatabits() + ", " + objappSettings.GetParity() + ", " + objappSettings.GetStopBits();
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
                lblMainScreenMsg.Text = "Cabcon  Product Management Program Ver. " + Prduct_Version +  "\n" + "            " + connectionString;
                Prduct_Version = Application.ProductName.ToString() + " Ver. " + Prduct_Version;
                stsReady.Text = "Association : " + dlmscommmode;
                lblversion.Text = Prduct_Version + " | " + strbuilton ;
                lblversion.Text = lblversion.Text ;
                lblLoginInfo.Text = "Login Mode : " + objetyusermgt.LogType.ToUpper() + " | " + connectionString;
                
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Setting Main Form Status Failed !" + "\r\n" + "\r\n" + Ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
            }
        }

		 private void SetUserPermissionRejectionList()
        {
            try
            {
                ToolStripItem[] ts = new ToolStripItem[] { tsm_rejectlist, tsm_newrejection, tsm_newrejectioncompact };
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
                            break;
                        case 2:
                            break;
                    }
                    displayIdx++;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Setting Main Form Status Failed !" + "\r\n" + "\r\n" + ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
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
            if (retryLiveTrackingForm != null && !retryLiveTrackingForm.IsDisposed)
            {
                retryLiveTrackingForm.Close();
                retryLiveTrackingForm.Dispose();
            }

            if (retryRecordsForm != null && !retryRecordsForm.IsDisposed)
            {
                retryRecordsForm.Close();
                retryRecordsForm.Dispose();
            }
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
            AboutCabconPMP objabout = new AboutCabconPMP();
            objabout.ShowDialog();
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Help.ShowHelp(this, AppDomain.CurrentDomain.BaseDirectory + "\\Configuration\\Cabcon_PMP.chm");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable To open Help File !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ts_Help_Click(object sender, EventArgs e)
        {
            try
            {
                Help.ShowHelp(this, AppDomain.CurrentDomain.BaseDirectory + "\\Configuration\\Cabcon_PMP.chm");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable To open Help File !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            CabconPMPREJECTIONTOOL.AccessPassword frmobj = new CabconPMPREJECTIONTOOL.AccessPassword(objetyusermgt);
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

        private void pcbBackgroundImage_Click(object sender, EventArgs e)
        {

        }
        List<Meter> m_meterlist = new List<Meter>();

        private async void toolRetry_Click(object sender, EventArgs e)
        {
            EnsureRetryViews();

            PortRetryExecutionRunner retryRunner = new PortRetryExecutionRunner();
            retryRunner.StatusUpdated += RetryRunner_StatusUpdated;

            try
            {
                retryLiveTrackingForm.ResetLog();
                retryLiveTrackingForm.Show(this);
                retryLiveTrackingForm.BringToFront();
                retryLiveTrackingForm.AppendStatus("Starting retry execution...", false);

                mcmdarguments = retryRunner.LoadProcedureMethodNames().ToArray();
                m_retryExecutionSummary = await Task.Run(() => retryRunner.Execute(mcmdarguments));

                if (m_retryExecutionSummary == null || m_retryExecutionSummary.Count == 0)
                {
                    retryLiveTrackingForm.AppendStatus("No available communication ports or procedure methods were found.", true);
                    MessageBox.Show("Attention\n\nNo available communication ports or procedure methods were found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                ShowRetryRecords(m_retryExecutionSummary);
                string summaryText = string.Format("Retry completed across {0} port(s).", m_retryExecutionSummary.Count);
                dlmsCommStatusmsh.Text = summaryText;
                retryLiveTrackingForm.AppendStatus(summaryText, false);
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                retryLiveTrackingForm.AppendStatus(ex.Message, true);
                MessageBox.Show(ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                retryRunner.StatusUpdated -= RetryRunner_StatusUpdated;
            }
        }
        public void AddressForm_PingLed(object sender, UpdateEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, UpdateEventArgs>(AddressForm_PingLed), sender, e);
                return;
            }

            if (e.isError) dlmsCommStatusmsh.ForeColor = Color.Red;
            else dlmsCommStatusmsh.ForeColor = Color.Green;
            dlmsCommStatusmsh.Text = e.msg;
            Application.DoEvents();
        }

        private void InitializeRetryViews()
        {
            if (pnlRetryRecordsHost != null)
            {
                return;
            }

            if (pcbBackgroundImage != null)
            {
                Controls.Remove(pcbBackgroundImage);
                pcbBackgroundImage.Visible = false;
            }

            pnlRetryRecordsHost = new Panel();
            pnlRetryRecordsHost.Name = "pnlRetryRecordsHost";
            pnlRetryRecordsHost.Dock = DockStyle.Fill;
            pnlRetryRecordsHost.BackColor = Color.White;
            Controls.Add(pnlRetryRecordsHost);
            pnlRetryRecordsHost.SendToBack();

            retryRecordsForm = new frmRetryRecords();
            retryRecordsForm.TopLevel = false;
            retryRecordsForm.FormBorderStyle = FormBorderStyle.None;
            retryRecordsForm.Dock = DockStyle.Fill;
            pnlRetryRecordsHost.Controls.Add(retryRecordsForm);
            retryRecordsForm.Show();
            retryRecordsForm.BindResults(null);

            // wire toolbar actions to handlers if not wired by designer
            if (this.toolConnect != null)
            {
                this.toolConnect.Click -= this.toolConnect_Click;
                this.toolConnect.Click += this.toolConnect_Click;
            }

            if (this.toolRetry != null)
            {
                this.toolRetry.Click -= this.toolRetry_Click;
                this.toolRetry.Click += this.toolRetry_Click;
            }

            retryLiveTrackingForm = new frmRetryLiveTracking();

            // subscribe to retry requests from the records form
            if (retryRecordsForm != null)
            {
                retryRecordsForm.RetryRequested -= RetryRecordsForm_RetryRequested;
                retryRecordsForm.RetryRequested += RetryRecordsForm_RetryRequested;
            }
        }

        private void EnsureRetryViews()
        {
            if (pnlRetryRecordsHost == null || pnlRetryRecordsHost.IsDisposed)
            {
                InitializeRetryViews();
            }

            if (retryLiveTrackingForm == null || retryLiveTrackingForm.IsDisposed)
            {
                retryLiveTrackingForm = new frmRetryLiveTracking();
            }

            if (retryRecordsForm == null || retryRecordsForm.IsDisposed)
            {
                retryRecordsForm = new frmRetryRecords();
                retryRecordsForm.TopLevel = false;
                retryRecordsForm.FormBorderStyle = FormBorderStyle.None;
                retryRecordsForm.Dock = DockStyle.Fill;
                pnlRetryRecordsHost.Controls.Clear();
                pnlRetryRecordsHost.Controls.Add(retryRecordsForm);
                // subscribe to retry requests on newly created form
                retryRecordsForm.RetryRequested -= RetryRecordsForm_RetryRequested;
                retryRecordsForm.RetryRequested += RetryRecordsForm_RetryRequested;
                retryRecordsForm.Show();
                retryRecordsForm.BindResults(null);
            }
        }

        private void RetryRunner_StatusUpdated(object sender, UpdateEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, UpdateEventArgs>(RetryRunner_StatusUpdated), sender, e);
                return;
            }

            if (retryLiveTrackingForm != null && !retryLiveTrackingForm.IsDisposed)
            {
                retryLiveTrackingForm.AppendStatus(e.msg, e.isError);
            }

            if (e.isError) dlmsCommStatusmsh.ForeColor = Color.Red;
            else dlmsCommStatusmsh.ForeColor = Color.Green;
            dlmsCommStatusmsh.Text = e.msg;
        }

        private void ShowRetryRecords(List<RetryPortExecutionResult> results)
        {
            EnsureRetryViews();
            retryRecordsForm.BindResults(results);
            // populate internal retry counters and failure reasons
            AssignRecordsFromResults(results);
            retryRecordsForm.Show();
            retryRecordsForm.BringToFront();
            pnlRetryRecordsHost.SendToBack();
        }

        private void AssignRecordsFromResults(List<RetryPortExecutionResult> results)
        {
            if (results == null || retryRecordsForm == null) return;

            // Build list of RetryBenchRecord objects inside the form via reflection since it's internal
            // Simpler approach: call BindResults already sets up the records, we only need to ensure retry counts
        }

        private void RetryRecordsForm_RetryRequested(int position, string operationName)
        {
            // limit retries per position to 3 - for now we simply append status to the live tracking and re-run the process for that single port
            retryLiveTrackingForm.AppendStatus(string.Format(CultureInfo.InvariantCulture, "Retry requested for position {0} (method: {1})", position, string.IsNullOrWhiteSpace(operationName) ? "<all>" : operationName), false);
            // Implement single-position retry execution using PortRetryExecutionRunner against the specific port name.
            // Map position -> port name via current retry summary
            if (m_retryExecutionSummary == null || m_retryExecutionSummary.Count == 0)
            {
                retryLiveTrackingForm.AppendStatus("No execution summary available to map position to port.", true);
                return;
            }

            var record = m_retryExecutionSummary.FirstOrDefault(r => r.ThreadIndex + 1 == position || r.PortName.IndexOf("Port", StringComparison.OrdinalIgnoreCase) >= 0 && (r.PortName.EndsWith(position.ToString()) || r.PortName.Contains(position.ToString())));
            if (record == null)
            {
                // fallback: attempt to use same index
                if (position - 1 >= 0 && position - 1 < m_retryExecutionSummary.Count)
                {
                    record = m_retryExecutionSummary[position - 1];
                }
            }

            if (record == null)
            {
                retryLiveTrackingForm.AppendStatus("Unable to determine port for requested position.", true);
                return;
            }

            // Run retry for this single port for the last failed method(s)
            var runner = new PortRetryExecutionRunner();
            runner.StatusUpdated += RetryRunner_StatusUpdated;

            // Determine which methods to retry.
            string[] targetMethods = null;
            if (!string.IsNullOrWhiteSpace(operationName) && !string.Equals(operationName, "<none>", StringComparison.OrdinalIgnoreCase))
            {
                targetMethods = new[] { operationName };
            }
            else
            {
                // Determine last failed method names from record.MethodResults
                var failedMethods = record.MethodResults?.Where(m => !m.Succeeded).Select(m => m.MethodName).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
                if (failedMethods == null || failedMethods.Length == 0)
                {
                    // nothing failed — consider re-running full procedure methods (use runner.LoadProcedureMethodNames())
                    targetMethods = runner.LoadProcedureMethodNames().ToArray();
                }
                else
                {
                    targetMethods = failedMethods;
                }
            }

            retryLiveTrackingForm.AppendStatus(string.Format(CultureInfo.InvariantCulture, "Retrying position {0} on port {1} for method(s): {2}", position, record.PortName, string.Join(", ", targetMethods)), false);

            // Execute single port retry on background thread to avoid UI freeze
            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    var singleResult = runner.ExecuteSinglePort(record.PortName, record.Mode, record.ThreadIndex, targetMethods);
                    // update UI summary — merge single result into existing summary so other cards remain visible
                    Invoke(new Action(() =>
                    {
                        if (m_retryExecutionSummary == null || m_retryExecutionSummary.Count == 0)
                        {
                            m_retryExecutionSummary = new List<RetryPortExecutionResult> { singleResult };
                        }
                        else
                        {
                            int idx = position - 1;
                            if (idx >= 0 && idx < m_retryExecutionSummary.Count)
                            {
                                m_retryExecutionSummary[idx] = singleResult;
                            }
                            else
                            {
                                // try to match by port name or thread id, otherwise append
                                var match = m_retryExecutionSummary.FirstOrDefault(r =>
                                    (singleResult != null && !string.IsNullOrWhiteSpace(singleResult.PortName) && string.Equals(r.PortName, singleResult.PortName, StringComparison.OrdinalIgnoreCase))
                                    || r.ManagedThreadId == singleResult.ManagedThreadId);

                                if (match != null)
                                {
                                    int found = m_retryExecutionSummary.IndexOf(match);
                                    if (found >= 0)
                                    {
                                        m_retryExecutionSummary[found] = singleResult;
                                    }
                                    else
                                    {
                                        m_retryExecutionSummary.Add(singleResult);
                                    }
                                }
                                else
                                {
                                    m_retryExecutionSummary.Add(singleResult);
                                }
                            }
                        }

                        ShowRetryRecords(m_retryExecutionSummary);
                    }));
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() => retryLiveTrackingForm.AppendStatus("Retry execution failed: " + ex.Message, true)));
                }
            });
        }

        private void toolConnect_Click(object sender, EventArgs e)
        {
            // perform connect-only across all configured ports and show the results in the records view
            try
            {
                EnsureRetryViews();
                // indicate operation name and clear previous results
                retryRecordsForm.SetOperationName("ConnectToMeter");
                retryRecordsForm.ClearCards();
                PortRetryExecutionRunner runner = new PortRetryExecutionRunner();
                runner.StatusUpdated += RetryRunner_StatusUpdated;

                retryLiveTrackingForm.ResetLog();
                retryLiveTrackingForm.Show(this);
                retryLiveTrackingForm.AppendStatus("Starting connect across ports...", false);

                List<RetryPortExecutionResult> connectResults = null;
                Task connectTask = Task.Run(() => { connectResults = runner.ConnectToAllPorts(); });
                connectTask.Wait();

                if (connectResults == null || connectResults.Count == 0)
                {
                    retryLiveTrackingForm.AppendStatus("No available communication ports were found.", true);
                    MessageBox.Show("No communication ports were found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                m_retryExecutionSummary = connectResults;
                ShowRetryRecords(m_retryExecutionSummary);
                retryLiveTrackingForm.AppendStatus("Connect phase completed.", false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
