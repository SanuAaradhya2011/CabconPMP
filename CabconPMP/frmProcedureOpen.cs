using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using COMMONENTITY;
using BALLAYER;
using ApplicationInterface;
using Utilities;
using PMPSM110;
using System.IO;
using System.Diagnostics;
using SystemSecurityLibrary;
namespace CabconPMP
{
    public partial class frmProcedureOpen : Form
    {
        public delegate void UpdateMainMsgHandler(object sender, UpdateEventArgs e);
        public event UpdateMainMsgHandler UpdateMsg;
        UpdateEventArgs args = null;
        FileEncryptionMethod objenc = new FileEncryptionMethod();
        EntityProcedure objentitypro = new EntityProcedure();
        StandardDateTime stddt = new StandardDateTime();
        BALProcedure objbalprocedure = new BALProcedure();
        BALDBConnectionTest objdbcon = new BALDBConnectionTest();
        string proactionTypr = string.Empty;
        string logedUserID = string.Empty;
        EntityUserManagement objentyUM;
        public frmProcedureOpen(string ptype, EntityUserManagement objetyusermgtref)
        {
            InitializeComponent();
            proactionTypr = ptype;
            logedUserID = objetyusermgtref.LoginuserID;
            if (objetyusermgtref.LoginTypeIndex == (int)StaticVariables.userCategory.Useradministrator || objetyusermgtref.LoginTypeIndex == (int)StaticVariables.userCategory.UserPoweradministrator) btnDelete.Enabled = true;
            else btnDelete.Enabled = false;
            btnopen.Text = proactionTypr;
            objentyUM = objetyusermgtref;
        }
        public void AddressForm_PingLed(object sender, UpdateEventArgs e)
        {
            args = new UpdateEventArgs(e.msg, e.isError);
            UpdateMsg(this, args);
        }
        private void SM110frmProcedureOpen_Load(object sender, EventArgs e)
        {
            GetProcedureList();
        }
        private void GetProcedureList()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (!objdbcon.IsDBConnected())
                {
                    MessageBox.Show("Unable To Connect DataBase, Server May Down" + "\n" + "Plase Contact System Administrator !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.DoEvents();
                    this.Close();
                    Application.DoEvents();
                    return;
                }
                objentitypro.ProcedureName = txtSearch.Text.Trim();
                DataSet ds = objbalprocedure.SelectDistinctFromTabProcedureMaster(objentitypro);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    lstProcedureName.Items.Clear();
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        lstProcedureName.Items.Add(dr[0].ToString());
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnopen_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (btnopen.Text.Trim() == "Open")
                {
                    if (lstProcedureName.SelectedIndex >= 0)
                    {

                        if (MessageBox.Show("Do You Want To open Selected Procedure ?? ", "Cabcon PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                        frmProcedure objfrmpro = new frmProcedure(lstProcedureName.SelectedItem.ToString(), objentyUM);
                        objfrmpro.ShowDialog();

                    }
                }
                else if (btnopen.Text.Trim() == "Run")
                {
                    if (lstProcedureName.SelectedIndex >= 0)
                    {

                        if (MessageBox.Show("Do You Want To Run Selected Procedure ?? ", "Cabcon PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

                        ExportProcedureDetails(lstProcedureName.SelectedItem.ToString());
                        this.Close();

                    }
                }
                else if (btnopen.Text.Trim() == "Import")
                {
                    ImportFile();
                }
                else if (btnopen.Text.Trim() == "Export")
                {
                    ExportFile();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void ExportProcedureDetails(string procedureName)
        {
            try
            {
                AppSettings objApps = new AppSettings();
                List<string> valueList = objApps.GetSettings();
                valueList.RemoveAt(valueList.Count - 7);
                valueList.Add(logedUserID);
                int assCount = 0;
                string assList = string.Empty;
                while (assCount < valueList.Count) { assList += valueList[assCount] + ","; assCount++; }
                if (assList.Length > 0) assList = assList.Substring(0, assList.Length - 1);
                if (procedureName.Length > 0)
                {
                    objentitypro.ProcedureName = procedureName;
                    string xmlFileName = procedureName.Replace(" ", "");
                    DataSet ds = objbalprocedure.SelectFromTabProcedureMasteronProcedureNAme(objentitypro);
                    //---------------Genearate MeterID XML-------------
                    if (ds.Tables[0].Rows[0]["ProceduteType"].ToString().ToUpperInvariant() ==StaticVariables.TestType_SR.ToUpperInvariant())
                    {
                    DataSet dsMID = new DataSet("XMLMeterID");
                    string meterIDlist = ds.Tables[0].Rows[0]["MeterIDRange"].ToString();
                    string[] meterIDlistitems = meterIDlist.Split(',');
                    if (meterIDlist != null && meterIDlist.Length > 3)
                    {
                        string preFix = meterIDlistitems[1].Trim();
                        string meterIDFrom = meterIDlistitems[2].Trim();
                        string meterIDTo = meterIDlistitems[3].Trim();
                        int maxlen = Convert.ToInt32(meterIDlistitems[0].Trim());
                        long mIDcnt;
                        long maxIDcnt;
                        if (!long.TryParse(meterIDlistitems[2].Trim(), out mIDcnt)) { mIDcnt = 0; };
                        if (!long.TryParse(meterIDlistitems[3].Trim(), out maxIDcnt)) { mIDcnt = 0; };

                        DataTable dt1 = new System.Data.DataTable("MeterIDList");
                        DataRow dr = dt1.NewRow();
                        dt1.Columns.Add("MeterID");
                        dr = dt1.NewRow();
                        while (mIDcnt <= maxIDcnt)
                        {
                            Application.DoEvents();
                            dr = dt1.NewRow();
                            dr[0] = preFix.Trim() + (mIDcnt++).ToString().PadLeft(maxlen - preFix.Length, '0');
                            dt1.Rows.Add(dr);
                        }                        
                        dt1.WriteXml(AppDomain.CurrentDomain.BaseDirectory + "\\Configuration\\" + StaticVariables.FilePrefixMeterIDList + xmlFileName + ".xml");
                    }
                    }
            
                    //-------------------------------------------------
                    XMLExportImport objexpxml = new XMLExportImport();
                   
                    objexpxml.ExportXMLFromDatatable(ds.Tables[0], xmlFileName);
                    string progNme = ds.Tables[0].Rows[0]["ProgramName"].ToString();
                    string procedureType = ds.Tables[0].Rows[0]["proceduteType"].ToString();
                    procedureType = procedureType.Replace(" ", "");
                    assList = assList + "," + objentyUM.LoginTypeIndex.ToString();
                    assList = assList + "," + AppDomain.CurrentDomain.BaseDirectory + "\\" + progNme;
                    //Serialization,Falcon2SM110SerializationTEST,COM1,None,8,1,9600,2500,3500,128,1,2,1,9999,9999,5,126,1,256,6,1,2,00000000,000102030405060708090A0B0C0D0E0F,9999,1CFF3F,9600,12345678,32,000102030405060708090A0B0C0D0E0F,0,000102030405060708090A0B0C0D0E0F,indel7063,5,D:\TFS\AMP\India\1P & 3P Common Tools\CabconPMP\DEV\CabconPMP-DEV\SRC\CabconPMP\bin\x86\Release\\PMPSM110
                    System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\" + progNme, procedureType + "," + xmlFileName + "," + assList);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Program Not Found/ Unable To Execte Selected Test Procedure !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstProcedureName.SelectedIndex >= 0)
                {

                    if (MessageBox.Show("Do You Want To Delete Selected Procedure ?? ", "Cabcon PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                    objentitypro.ProcedureName = lstProcedureName.SelectedItem.ToString();
                    string resultmsg = objbalprocedure.DeleteTabProcedureMasteronProcedureName(objentitypro);
                    if (resultmsg != "")
                    {
                        MessageBox.Show("Unable To Delete !" + "\n" + resultmsg, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else lstProcedureName.Items.RemoveAt(lstProcedureName.SelectedIndex);
                    MessageBox.Show("Selected Procedure Deleted !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Unable To Delete Selected Procedure !" + "\n" + Ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void ExportFile()
        {

            try
            {

                if (lstProcedureName.SelectedIndex >= 0)
                {
                    if (MessageBox.Show("Do You Want To Export Selected Procedure ?? ", "Cabcon PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                }
                if (lstProcedureName.SelectedIndex >= 0)
                {
                    SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
                    SaveFileDialog1.Filter = "Test Procedure Files (*.tpf)|*.tpf|All Files (*.*)|*.*";
                    SaveFileDialog1.FileName = lstProcedureName.SelectedItem.ToString();
                    DialogResult result = SaveFileDialog1.ShowDialog();

                    if (result != DialogResult.OK) return;
                    string strFileName = SaveFileDialog1.FileName;
                    string ipFileName = strFileName.Replace("tpf", "xml");
                    this.Cursor = Cursors.WaitCursor;
                    //-------------------------------
                    AppSettings objApps = new AppSettings();
                    List<string> valueList = objApps.GetSettings();
                    int assCount = 0;
                    string assList = string.Empty;
                    while (assCount < valueList.Count) { assList += valueList[assCount] + ","; assCount++; }
                    if (assList.Length > 0) assList = assList.Substring(0, assList.Length - 1);

                    objentitypro.ProcedureName = lstProcedureName.SelectedItem.ToString();
                    DataSet ds = objbalprocedure.SelectFromTabProcedureMasteronProcedureNAme(objentitypro);
                    XMLExportImport objexpxml = new XMLExportImport();
                    string xmlFileName = objentitypro.ProcedureName.ToString().Replace(" ", "");
                    string ipfilename = AppDomain.CurrentDomain.BaseDirectory + "\\Export\\" + xmlFileName + ".pro";
                    ds.WriteXml(ipFileName, XmlWriteMode.IgnoreSchema);
                    objenc.EncryptFile(ipFileName, strFileName);
                    File.Delete(ipFileName);
                    MessageBox.Show("Procedure File Exported !" + "\n" + strFileName, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Procedure Not Found/ Unable To Export Selected Test Procedure !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }




        private void ImportFile()
        {
            try
            {

                //decimal objdec;
                DataTable executionResultClone=new DataTable();
                openFileDialog1.Filter = "Test Procedure Files (*.tpf)|*.tpf|All Files (*.*)|*.*";
                openFileDialog1.Title = "Select Procedure File";
                openFileDialog1.FileName = "";
                string fileName = "";
                if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
                fileName = openFileDialog1.FileName;
                string FileNameFromProcedure = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                FileNameFromProcedure = FileNameFromProcedure.Substring(0, FileNameFromProcedure.Length - 4);
                string opFileName = fileName.Replace("tpf", "xml");
                if (!File.Exists(fileName))
                {
                    MessageBox.Show("Invalid File Selected, File Not Exist!", "Cabcon Technologies", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                objenc.DecryptFile(fileName, opFileName);
                string strFileData = DLMSDataStracture.ReadUserFileData(fileName);
                DataSet ds = new DataSet();
                ds.ReadXml(opFileName);
                if (!File.Exists(opFileName))
                {
                    MessageBox.Show("Invalid File Selected, File Not Exist!", "Cabcon Technologies", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objentitypro.ProcedureID = stddt.DateTimeToLong(DateTime.Now);
                    objentitypro.MeterType = ds.Tables[0].Rows[0][1].ToString();
                    objentitypro.ProcedureType = ds.Tables[0].Rows[0][2].ToString();
                    objentitypro.ProcedureName = FileNameFromProcedure;
                    objentitypro.ProgramName = ds.Tables[0].Rows[0][5].ToString();
                    objentitypro.ParaUpdatedDateTime = DateTime.Now;
                    DataSet dsexist = objbalprocedure.SelectFromTabProcedureMasteronProcedureNAme(objentitypro);
                    executionResultClone = dsexist.Tables[0].Clone();
                    if (dsexist != null && dsexist.Tables.Count > 0 && dsexist.Tables[0].Rows.Count > 0)
                    {                        
                        objentitypro.ProcedureName = dsexist.Tables[0].Rows[0][3].ToString() + "_" + String.Format("{0:ddMMyyyyHHmmss}", DateTime.Now);

                    }
                     //---------------Fill Datatable--------------------
                 
                int recCount = 0;
                DataRow dr;               
                while (recCount < ds.Tables[0].Rows.Count)
                {
                     
                    dr = executionResultClone.NewRow();
                    dr["ProcedureID"] = recCount+1;
                    dr["MeterType"] = objentitypro.MeterType;
                    dr["ProceduteType"] =objentitypro.ProcedureType;
                    dr["ProcedureName"] = objentitypro.ProcedureName;
                    dr["SerializationWindow"] =0;
                    dr["ProgramName"] = objentitypro.ProgramName;
                    dr["ParametersName"] = ds.Tables[0].Rows[recCount]["ParametersName"].ToString();
                    dr["ParaDefaultValue"] = ds.Tables[0].Rows[recCount]["ParaDefaultValue"].ToString();
                    dr["ParaMinValue"] = ds.Tables[0].Rows[recCount]["ParaMinValue"].ToString();
                    dr["ParaMaxValue"] = ds.Tables[0].Rows[recCount]["ParaMaxValue"].ToString();
                    dr["ParaActivationStatus"] = ds.Tables[0].Rows[recCount]["ParaActivationStatus"];
                    dr["ParaUpdatedDateTime"] = objentitypro.ParaUpdatedDateTime;
                    executionResultClone.Rows.Add(dr);
                    recCount++;
                }
                string resultmsg = objbalprocedure.InsertinToTabProcedureMasterByBatch(objentitypro, executionResultClone);
                if (resultmsg != "")
                {
                    MessageBox.Show(resultmsg, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }                 
                       
                File.Delete(opFileName);
                MessageBox.Show("Procedure Imported As : " + objentitypro.ProcedureName, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GetProcedureList();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid/Currupted File Selected" + "\n" + "Unable To Import Selected Test Procedure !", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnRefrsh_Click(object sender, EventArgs e)
        {
            GetProcedureList();
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            GetProcedureList();
        }
    }
}
