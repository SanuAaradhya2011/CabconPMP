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
using PDFLibraryCabcon;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.IO.Pipes;

namespace CabconPMPREJECTIONTOOL
{
   

    public partial class frmErrorList : Form
    {
        private static frmErrorList openForm = null;
        private bool bAll=false;
        private string pcbaSource = "";
        private BALErrorOperations balerrorobj = new BALErrorOperations();
        private DataTable tableRejectionMaster;
        private DataTable tableextinfo;
        private EntityUserManagement objetyusermgt;
        private Dictionary<string, string> param = new Dictionary<string,string>();
        private const string SearchText_Instant = "\n(Live Search)";
        private const string SearchText_Manual = "\n(Manual Search)";
        private string Current_Table = string.Empty;

        /// <summary>
        /// Set this form and its controls as double-buffered.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        /// <summary>
        /// Enum for Excel predefined cell locations
        /// </summary>
        public enum EXCELCELL
        {
            STARTDATE_ROW = 7,
            STARTDATE_COL = 3,
            ENDDATE_ROW = 8,
            ENDDATE_COL = 3,
            STAGE_ROW = 7,
            STAGE_COL = 6,
            METERTYPE_ROW = 8,
            METERTYPE_COL = 6,
            ERRSTATUS_ROW = 7,
            ERRSTATUS_COL = 9,
            CUSTOMER_ROW = 8,
            CUSTOMER_COL = 9,
            HEADER1_ROW = 11,
            HEADER1_COL = 2,
            TABLEDATA_ROW = 12,
            TABLEDATA_COL = 2,

            ERRTYPE_ROW = 2,
            ERRTYPE_COL = 1,
            ERRTYPECOUNT_ROW = 2,
            ERRTYPECOUNT_COL = 2,

            CUMPERCENT_ROW = 2,
            CUMPERCENT_COL = 3,
            TOTAL_ROW = 2,
            TOTAL_COL = 4,
        }

        /// <summary>
        /// Constructor function for ErrorList Form, always maintains only one instance
        /// </summary>
        /// <param name="objetyusermgt"></param>
        /// <returns></returns>
        public static frmErrorList GetInstance(EntityUserManagement objetyusermgt)
        {
            if (openForm == null)
            {
                if (objetyusermgt == null)
                {
                    objetyusermgt = new EntityUserManagement();
                    objetyusermgt.LoginuserID = "operator";
                    objetyusermgt.LogType = "operator";
                }
                openForm = new frmErrorList(objetyusermgt);
                openForm.FormClosing += (s, e) => { e.Cancel = true; openForm.Hide(); };
            }
            return openForm;
        }

        public static void ShowInstance(EntityUserManagement objetyusermgt)
        {
            GetInstance(objetyusermgt).Show();
            openForm.BringToFront();
        }

        /// <summary>
        /// Createinstance of this form frmErrorList using Login details.
        /// </summary>
        /// <param name="objetyusermgt"></param>
        public frmErrorList(EntityUserManagement objetyusermgt)
        {
            this.objetyusermgt = objetyusermgt;
            InitializeComponent(); COMMONENTITY.FormStyleHelper.Apply(this);
            //
            panelFilters.Height = 34;
            dtpStart.Value = dtpEnd.Value.AddMonths(-1);
            this.dgvRejectionList.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvRejectionList_RowPostPaint);
            updateRecordToolStripMenuItem.Enabled = false;
            saveToolStripMenuItemExcel.Enabled = false;
            savePDFToolStripMenuItem.Enabled = false;
            deleteRejectionRecordToolStripMenuItem.Enabled = false;
            
        }

        /// <summary>
        /// Set up initial data on Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmErrorList_Load(object sender, EventArgs e)
        {
            this.chkListProdStage.DataSource = new List<string>(){
               ErrorUtility.TestType_ALL,
               StaticVariables.TestType_Cal,
               StaticVariables.TestType_EMS,
               StaticVariables.TestType_FT,
               StaticVariables.TestType_SR,
               ErrorUtility.TestType_Sealing,
               ErrorUtility.TestType_Assembly,
               StaticVariables.TestType_OT,
            };

            this.chkListMeterType.DataSource = new List<string>(){
                ErrorUtility.MeterType_ALL,
                StaticVariables.MeterType_1PH_E150_ES,
                StaticVariables.MeterType_1PH_E150_MS,
                StaticVariables.MeterType_1PH_E150_MSD,
                StaticVariables.MeterType_3PH_DLMS,
                StaticVariables.MeterType_3PH_Sapphire,
                StaticVariables.MeterType_1PH_SM,
                StaticVariables.MeterType_3PH_SM,
                StaticVariables.MeterType_1PH_ZCE

            };

            this.chkListErrorStatus.DataSource = new List<string>(){
                ErrorStateConstants.StateAll,
                ErrorStateConstants.StatePassed,
                ErrorStateConstants.StateInitial,
                ErrorStateConstants.StateAnalysis,
                ErrorStateConstants.StateProcessed

            };

            this.chkListSearchBy.DataSource = new List<string>(){
                ErrorUtility.SearchTypeALL,
                RejectionTable.colPCBAID,
                RejectionTable.colCustomer
            };

            this.chkListShift.DataSource = new List<string>(){
                ErrorUtility.SearchTypeALL,
                ProductionShifts.Shift1,
                ProductionShifts.Shift2,
                ProductionShifts.Shift3,
            };

            this.chkListSource.DataSource = new List<string>(){
                PCBASourceNames.PCBSourceAll,
                PCBASourceNames.PCBSourceSGS,
                PCBASourceNames.PCBSourceKAYNES,
                PCBASourceNames.PCBSourceOTHER
            };

            panelSearchText.Controls.SetChildIndex(txtSearchPCBAID, 0);
            panelSearchText.Controls.SetChildIndex(txtSearchCustomer, 1);

            SetLabels(SearchText_Manual, SearchText_Instant);
        }

        private void SetLabels(string existingText, string replaceText)
        {
            lblErrorState.Text = lblErrorState.Text.Contains(replaceText) ? lblErrorState.Text : lblErrorState.Text.Contains(existingText) ? lblErrorState.Text.Replace(existingText, replaceText) : lblErrorState.Text + replaceText;
            lblMeterType.Text = lblMeterType.Text.Contains(replaceText) ?  lblMeterType.Text : lblMeterType.Text.Contains(existingText) ? lblMeterType.Text.Replace(existingText, replaceText) : lblMeterType.Text + replaceText;
            lblProdStage.Text = lblProdStage.Text.Contains(replaceText) ? lblProdStage.Text : lblProdStage.Text.Contains(existingText) ? lblProdStage.Text.Replace(existingText, replaceText) : lblProdStage.Text + replaceText;
            lblSearchBy.Text = lblSearchBy.Text.Contains(replaceText) ?  lblSearchBy.Text : lblSearchBy.Text.Contains(existingText) ? lblSearchBy.Text.Replace(existingText, replaceText) : lblSearchBy.Text + replaceText;
            lblPCBSource.Text = lblPCBSource.Text.Contains(replaceText) ?  lblPCBSource.Text : lblPCBSource.Text.Contains(existingText) ? lblPCBSource.Text.Replace(existingText, replaceText) : lblPCBSource.Text + replaceText;

            chkListSource.Visible = replaceText == SearchText_Instant;
            lblPCBSource.Visible = replaceText == SearchText_Instant;
        }

        /// <summary>
        /// Add numeric index to rows in datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRejectionList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dgvRejectionList.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 4);
            }
        }

        

        /// <summary>
        /// Function on ItemClicked event for initializing and disabling controls depending on item clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name.Equals("displayRejectionListToolStripMenuItem"))
            {
                this.chkShowZeroCountRecords.Visible = false;
                this.chkUniquePCBAID.Visible = true;
                this.updateRecordToolStripMenuItem.Enabled = true;
                this.saveToolStripMenuItemExcel.Enabled = true;
                this.dgvRejectionList.DataSource = null;
                this.savePDFToolStripMenuItem.Enabled = true;
                this.deleteRejectionRecordToolStripMenuItem.Enabled = true;

                if (!groupBoxErrDetails.Visible)
                {
                    this.dgvRejectionList.Height -= this.groupBoxErrDetails.Height;
                    this.groupBoxErrDetails.Visible = true;
                }
            }
            else if (e.ClickedItem.Name.Equals("viewErrorTypesTableToolStripMenuItem"))
            {
                this.chkUniquePCBAID.Visible = false;
                this.chkShowZeroCountRecords.CheckedChanged -= new System.EventHandler(this.chkShowZeroCountRecords_CheckedChanged);
                this.chkShowZeroCountRecords.Visible = true;
                this.chkShowZeroCountRecords.Checked = true;
                this.chkShowZeroCountRecords.CheckedChanged += new System.EventHandler(this.chkShowZeroCountRecords_CheckedChanged);

                txtParamName.Text = string.Empty;
                txtErrorDate.Text = string.Empty;
                txtActionDate.Text = string.Empty;
                txtProbDesc.Text = string.Empty;
                txtErrorName.Text = string.Empty;
                txtRejectCause.Text = string.Empty;
                txtRejectAction.Text = string.Empty;
                txtLoggedUserID.Text = string.Empty;

                if (this.groupBoxErrDetails.Visible)
                {
                    this.groupBoxErrDetails.Visible = false;
                    this.dgvRejectionList.Height += this.groupBoxErrDetails.Height;
                }

                this.savePDFToolStripMenuItem.Enabled = true;
                this.updateRecordToolStripMenuItem.Enabled = false;
                this.deleteRejectionRecordToolStripMenuItem.Enabled = false;
                this.saveToolStripMenuItemExcel.Enabled = true;
                this.dgvRejectionList.DataSource = null;
            }
        }

        /// <summary>
        /// Remove Zero Count records from ErrorTypes table in datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkShowZeroCountRecords_CheckedChanged(object sender, EventArgs e)
        {
            DataTable sourceTable = this.dgvRejectionList.DataSource as DataTable;
            if (!this.chkShowZeroCountRecords.Checked)
            {          
                sourceTable.DefaultView.RowFilter = ("Count <> 0");
            }
            else
            {
                sourceTable.DefaultView.RowFilter = "";
            }

        }

        /// <summary>
        /// Gets Rejection List Table Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displayRejectionListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            param.Remove(ParamDictionaryKeys.ErrorType);
            SetLabels(SearchText_Manual, SearchText_Instant);
            lblLoggedUserID.Text = ErrorUtility.SearchTypeALL;
            //Dictionary<string, string> param = null; 
            //DateTime startDate = DateTime.Now; DateTime endDate = DateTime.Now;
            FillParamsForQuery(ref param, RejectionTable.dbRejectionMasterTableName);
            DisplayRejectionList(param);
        }
        /// <summary>
        /// Display Rejection table in Datagridview after applying all filters.
        /// </summary>
        /// <param name="param"></param>
        private void DisplayRejectionList(Dictionary<string, string> param)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                txtParamName.Text = string.Empty;
                txtErrorDate.Text = string.Empty;
                txtActionDate.Text = string.Empty;
                txtProbDesc.Text = string.Empty;
                txtErrorName.Text = string.Empty;
                txtRejectCause.Text = string.Empty;
                txtRejectAction.Text = string.Empty;
                txtLoggedUserID.Text = string.Empty;

                this.chkShowZeroCountRecords.Visible = false;
                this.chkUniquePCBAID.Visible = true;

                param.Remove(ParamDictionaryKeys.SearchParams);
                tableRejectionMaster = balerrorobj.GetRejectionMasterTable(param);
                lblQueryDateTime.Text = DateTime.Now.ToString(DateTimeFormats.DATETIME_FORMAT_DISPLAY, CultureInfo.InvariantCulture);
                if (tableRejectionMaster == null) { MessageBox.Show("Unable to Load Data!", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (tableRejectionMaster.Columns.Contains("N")) tableRejectionMaster.Columns.Remove("N");

                DataColumn col = new DataColumn(RejectionTable.colPCBSource, typeof(string));
                tableRejectionMaster.Columns.Add(col);
                foreach (DataRow row in tableRejectionMaster.Rows)
                {
                    row[RejectionTable.colPCBSource] = ErrorUtility.GetSourceFromPCBAID_MeterType(row[RejectionTable.colPCBAID].ToString(), row[RejectionTable.colMeterType].ToString());
                }

                col = new DataColumn(param[ParamDictionaryKeys.ErrorDateType] == RejectionTable.colErrorDate ? RejectionTable.colShiftEDate : RejectionTable.colShiftADate, typeof(string));
                tableRejectionMaster.Columns.Add(col);
                foreach (DataRow row in tableRejectionMaster.Rows)
                {
                    row[col.ColumnName] = ErrorUtility.GetShiftFromDate((DateTime)row[param[ParamDictionaryKeys.ErrorDateType]]);
                }

                tableRejectionMaster.Locale = System.Globalization.CultureInfo.GetCultureInfo("en-IN");
                tableRejectionMaster.Columns[RejectionTable.colErrorDate].DataType = typeof(DateTime);
                tableRejectionMaster.Columns[RejectionTable.colActionDate].DataType = typeof(DateTime); 

                tableRejectionMaster.AcceptChanges();
                string valueText = string.Empty;

                SetDataGridViewDataSource(dgvRejectionList, tableRejectionMaster);

                this.dgvRejectionList.SelectionChanged += new System.EventHandler(this.dgvRejectionList_SelectionChanged);

                //dgvRejectionList.Columns[RejectionTable.colLoggedUserID].ContextMenuStrip = contextMenuUserID;
                //dgvRejectionList.ContextMenuStrip = contextMenuUserID;

                dgvRejectionList.Columns[RejectionTable.colIDError].Visible = false;
                dgvRejectionList.Columns[ErrorTypesTable.colErrorType].Visible = false;
                dgvRejectionList.Columns["S.NO"].Visible = false;

                dgvRejectionList.Columns[ErrorTypesTable.colErrorname].Width = 200;
                dgvRejectionList.Columns[RejectionTable.colMeterType].Width = 150;
                dgvRejectionList.Columns[RejectionTable.colCustomer].Width = 200;
                //dgvRejectionList.Columns[RejectionTable.colHistoryStatus].Width = 100;

                dgvRejectionList.Columns[RejectionTable.colErrorDate].DefaultCellStyle.Format = "dd/MM/yyyy hh:mm tt";
                dgvRejectionList.Columns[RejectionTable.colActionDate].DefaultCellStyle.Format = "dd/MM/yyyy hh:mm tt";
              
                if (tableRejectionMaster.Rows.Count > 0)
                    dgvRejectionList.CurrentCell = dgvRejectionList[2, 0];
                txtRecordCount.Text = tableRejectionMaster.Rows.Count.ToString();

                SetDataGridViewRowFilter(BuildSearchQuery(), RejectionTable.dbRejectionMasterTableName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Set PCB Source from PCBAID; 
        /// ***************************
        /// 1PH
        /// 1st char '4' = SGS
        /// 1st char '6' = Kaynes
        /// ***************************
        /// 3PH
        /// 6th char '2' = SGS
        /// 6th char '4' = Kaynes
        /// ***************************
        /// Smart Meter all SGS
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string GetSourceFromPCBAID_MeterType(string pcbID, string meterType)
        {
            try
            {
                if (meterType.Equals(StaticVariables.MeterType_1PH_SM) || meterType.Equals(StaticVariables.MeterType_3PH_SM))
                    return PCBASourceNames.PCBSourceSGS;
                else if (pcbID[0] == ( (int)MeterSrc1PHCharEnum.SGS ).ToString()[0])
                    return PCBASourceNames.PCBSourceSGS;
                else if (pcbID[0] == ( (int)MeterSrc1PHCharEnum.KAYNES ).ToString()[0])
                    return PCBASourceNames.PCBSourceKAYNES;
                else if (pcbID[5] == ( (int)MeterSrc3PHCharEnum.SGS ).ToString()[0])
                    return PCBASourceNames.PCBSourceSGS;
                else if (pcbID[5] == ( (int)MeterSrc3PHCharEnum.KAYNES ).ToString()[0])
                    return PCBASourceNames.PCBSourceKAYNES;
                else
                    return PCBASourceNames.PCBSourceOTHER;
            }
            catch (Exception ex)
            {
                return PCBASourceNames.PCBSourceOTHER;
            }

        }

        /// <summary>
        /// Gets parameter array for search criteria
        /// </summary>
        /// <param name="param"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        private void FillParamsForQuery(ref Dictionary<string,string> param, string tableName)
        {
            param.Clear();
            param[ParamDictionaryKeys.ErrorDateStart] = dtpStart.Value.ToString(DateTimeFormats.DATETIME_FORMAT_SQL, CultureInfo.InvariantCulture);  //need to provide in MM/dd/yyyy culture format  due to SQL server culture.
            param[ParamDictionaryKeys.ErrorDateEnd] = dtpEnd.Value.ToString(DateTimeFormats.DATETIME_FORMAT_SQL, CultureInfo.InvariantCulture);

            param[ParamDictionaryKeys.ErrorDateType] = rdbErrorDate.Checked ? RejectionTable.colErrorDate : RejectionTable.colActionDate;
            
            string valueText=string.Empty;
            //***********************************************************************
            //Search Params
            if (chkListSearchBy.CheckedItems.Count > 0 && tableName == ErrorTypesTable.dbErrorTypeTableName)
            {
                for (int i = 0; i < chkListSearchBy.CheckedItems.Count; i++)
                {
                    if (((string)chkListSearchBy.CheckedItems[i]).ToUpper().Contains("ALL")) {  continue; }
                    valueText += string.Format("{0}={1}|", (string)chkListSearchBy.CheckedItems[i], panelSearchText.Controls.Find("txtSearch" + (string)chkListSearchBy.CheckedItems[i], true)[0].Text);
                }
                valueText = valueText.Trim('|');
                param[ParamDictionaryKeys.SearchParams] = valueText;
            }
            //***********************************************************************
            //Meter Type
            if (chkListMeterType.CheckedItems.Count > 0 && tableName == ErrorTypesTable.dbErrorTypeTableName)
            {
                valueText = string.Empty;
                for (int i = 0; i < chkListMeterType.CheckedItems.Count; i++)
                {
                    if (((string)chkListMeterType.CheckedItems[i]).ToUpper().Contains("ALL")) { break; }
                    valueText += string.Format("'{0}',", (string)chkListMeterType.CheckedItems[i]);
                }
                valueText = valueText.Trim(',');
                if(!string.IsNullOrEmpty(valueText)) param[ParamDictionaryKeys.MeterType] = valueText;
            }
            //***********************************************************************
            //Production Stage
            if (chkListProdStage.CheckedItems.Count > 0 && tableName == ErrorTypesTable.dbErrorTypeTableName)
            {
                valueText = string.Empty;
                for (int i = 0; i < chkListProdStage.CheckedItems.Count; i++)
                {
                    if (((string)chkListProdStage.CheckedItems[i]).ToUpper().Contains("ALL")) break;
                    valueText += string.Format("'{0}',", (string)chkListProdStage.CheckedItems[i]);
                }
                valueText = valueText.Trim(',');
                if (!string.IsNullOrEmpty(valueText)) param[ParamDictionaryKeys.ProductionStage] = valueText;
            }
            //***********************************************************************
            //Error State
            if (chkListErrorStatus.CheckedItems.Count > 0 && tableName == ErrorTypesTable.dbErrorTypeTableName)
            {
                valueText = string.Empty;
                for (int i = 0; i < chkListErrorStatus.CheckedItems.Count; i++)
                {
                    if (((string)chkListErrorStatus.CheckedItems[i]).ToUpper().Contains("ALL")) break;
                    valueText += string.Format("'{0}',", (string)chkListErrorStatus.CheckedItems[i]);
                }
                valueText = valueText.Trim(',');
                if (!string.IsNullOrEmpty(valueText)) param[ParamDictionaryKeys.ErrorStatus] = valueText;
            }
            //***********************************************************************
            //Unique PCBA
            if (chkUniquePCBAID.Checked)
            {
                param[ParamDictionaryKeys.UniquePCBA] = bool.TrueString;
            }
        }

        /// <summary>
        /// Get Error List Table Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewErrorTypesTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.dgvRejectionList.SelectionChanged -= new System.EventHandler(this.dgvRejectionList_SelectionChanged);
                SetLabels(SearchText_Instant, SearchText_Manual);
                param.Remove(ParamDictionaryKeys.ErrorType);
                FillParamsForQuery(ref param, ErrorTypesTable.dbErrorTypeTableName);

                DataTable errortable = balerrorobj.GetErrorTypesTableWithCount(param);

                lblQueryDateTime.Text = DateTime.Now.ToString(DateTimeFormats.DATETIME_FORMAT_DISPLAY, CultureInfo.InvariantCulture);
                if (errortable == null) { MessageBox.Show("Unable to Load Data!", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
                errortable.TableName = ErrorTypesTable.dbErrorTypeTableName;
                SetDataGridViewDataSource(dgvRejectionList, errortable);
                dgvRejectionList.Columns[ErrorTypesTable.colErrorType].Width = 200;
                dgvRejectionList.Columns[ErrorTypesTable.colErrorname].Width = 200;
                dgvRejectionList.Columns[ErrorTypesTable.colErrorDescription].Width = 400;
                dgvRejectionList.Columns[ErrorTypesTable.colIdError].Width = 50;
                dgvRejectionList.Columns["Count"].Width = 50;

                txtRecordCount.Text = errortable.AsEnumerable().Sum(x => Int32.Parse(x["Count"].ToString())).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        /// <summary>
        /// Show record History of selected items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRejectionList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                frmRecordHistory frmobj;
                if (Current_Table == RejectionTable.dbRejectionMasterTableName && dgvRejectionList.SelectedRows.Count == 1)
                {
                    DataRow dRow = ((DataRowView)dgvRejectionList.Rows[dgvRejectionList.CurrentCell.RowIndex].DataBoundItem).Row;
                    frmobj = new frmRecordHistory(dRow[RejectionTable.colPCBAID].ToString(), dRow[RejectionTable.colProductionStage].ToString(), objetyusermgt);
                    frmobj.ShowDialog();
                }
                else if (dgvRejectionList.DataSource != null && ((DataTable)dgvRejectionList.DataSource).TableName.Equals(ErrorTypesTable.dbErrorTypeTableName) && dgvRejectionList.SelectedRows.Count == 1)
                {
                    if (!groupBoxErrDetails.Visible)
                    {
                        this.dgvRejectionList.Height -= this.groupBoxErrDetails.Height;
                        this.groupBoxErrDetails.Visible = true;
                    }
                    FillParamsForQuery(ref param, RejectionTable.dbRejectionMasterTableName);
                    param[ParamDictionaryKeys.ErrorType] = ((DataRowView)dgvRejectionList.Rows[dgvRejectionList.CurrentCell.RowIndex].DataBoundItem).Row[ErrorTypesTable.colIdError].ToString();
                    SetLabels(SearchText_Manual, SearchText_Instant);
                    lblLoggedUserID.Text = ErrorUtility.SearchTypeALL;
                    DisplayRejectionList(param);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Update Text in Lower pane when Rejection meter selection changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRejectionList_SelectionChanged(object sender, EventArgs e)
        {
            if (Current_Table == RejectionTable.dbRejectionMasterTableName && dgvRejectionList.SelectedRows.Count == 1)
            {
                int idx = dgvRejectionList.CurrentCell.RowIndex;
                long idxreject = long.Parse(dgvRejectionList.Rows[idx].Cells[0].Value.ToString());
                
                txtParamName.Text = dgvRejectionList.Rows[idx].Cells[RejectionTable.colParameterName].Value.ToString();
                txtErrorDate.Text = ((DateTime)dgvRejectionList.Rows[idx].Cells[RejectionTable.colErrorDate].Value).ToString(DateTimeFormats.DATETIME_FORMAT_DISPLAY, CultureInfo.InvariantCulture);
                txtActionDate.Text = ((DateTime)dgvRejectionList.Rows[idx].Cells[RejectionTable.colActionDate].Value).ToString(DateTimeFormats.DATETIME_FORMAT_DISPLAY, CultureInfo.InvariantCulture);
                txtProbDesc.Text = dgvRejectionList.Rows[idx].Cells[RejectionTable.colProblemDescription].Value.ToString();
                txtErrorName.Text = dgvRejectionList.Rows[idx].Cells[ErrorTypesTable.colErrorname].Value.ToString();
                txtRejectCause.Text = dgvRejectionList.Rows[idx].Cells[RejectionTable.colRejectionCause].Value.ToString();
                txtRejectAction.Text = dgvRejectionList.Rows[idx].Cells[RejectionTable.colRejectionAction].Value.ToString();
                txtLoggedUserID.Text = dgvRejectionList.Rows[idx].Cells[RejectionTable.colLoggedUserID].Value.ToString();
            }

        }

        /// <summary>
        /// Open Update/Insert Entry form
        /// </summary>
        public void StartUpdateEntryForm(long idxreject)
        {
            if (Current_Table == RejectionTable.dbRejectionMasterTableName && dgvRejectionList.SelectedRows.Count == 1)
            {
                tableextinfo = balerrorobj.GetExtendedInformation(idxreject);
                DataRow dRow = ((DataRowView)dgvRejectionList.Rows[dgvRejectionList.CurrentCell.RowIndex].DataBoundItem).Row;
                frmUpdateEntry frmobj = new frmUpdateEntry(dRow, tableextinfo, objetyusermgt);
                frmobj.ShowDialog();
            }
            else
            {
                frmUpdateEntry frmobj = new frmUpdateEntry();
                frmobj.ShowDialog();
            }
        }


        

        /// <summary>
        /// Normalize Camelcase words with spaces for a given table
        /// </summary>
        /// <param name="saveTable"></param>
        /// <returns></returns>
        private bool GenerateNormalizedRejectionTable(DataTable saveTable)
        {
            //if (!saveTable.TableName.Equals(RejectionTable.dbRejectionMasterTableName)) { return; }
            if (saveTable.TableName.Equals(RejectionTable.dbRejectionMasterTableName))
            {
                saveTable.Columns.Remove("S.No");
                saveTable.Columns.Remove(RejectionTable.colIDError);

                frmChooseSaveColumns formobj = new frmChooseSaveColumns(saveTable.Columns);
                formobj.ShowDialog();
                if (formobj.bIsCancel) return false;
            }

            foreach (DataColumn col in saveTable.Columns)
            {
                col.ColumnName = ErrorUtility.SplitStringCamelCase(col.ColumnName);
            }

            foreach (DataRow drow in saveTable.Rows)
            {
                foreach (DataColumn col in saveTable.Columns)
                {
                    if (col.ColumnName.ToLower().Contains("date")) continue;
                    drow[col] = ErrorUtility.NormalizeWhiteSpace(ErrorUtility.SplitStringCamelCase(drow[col].ToString()));
                }
            }
            return true;
        }

        /// <summary>
        /// Generate PDF for visible table in datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void asPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRejectionList.DataSource == null) return;

                bool bLandscape = false;
                DataTable saveTable = new DataTable();
                saveTable = ((DataTable)dgvRejectionList.DataSource).DefaultView.ToTable();

                saveTable.Columns.Remove(ErrorTypesTable.colErrorType);

                if (!GenerateNormalizedRejectionTable(saveTable)) return;

                if (saveTable.TableName.Equals(RejectionTable.dbRejectionMasterTableName)) { bLandscape = true; }

                SavePDF(saveTable, bLandscape);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Create PDF from given Datatable
        /// </summary>
        /// <param name="saveTable"></param>
        /// <param name="bLandscape"></param>
        public void SavePDF(DataTable saveTable, bool bLandscape)
        {
            try
            {
                PDFUtility pdfUtility = new PDFUtility();
                DataSet ds = new DataSet();


                ds.Tables.Add(saveTable);

                string filepath = string.Empty;
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.AddExtension = true;
                dialog.Filter = "pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
                dialog.InitialDirectory = @"C:\Users\SarkaArj\Desktop\";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    filepath = dialog.FileName;
                }
                else
                {
                    return;
                }

                Dictionary<string, string> headerDict = new Dictionary<string, string>();
                headerDict.Add(ParamDictionaryKeys.ErrorDateStart, dtpStart.Value.ToString(DateTimeFormats.DATETIME_FORMAT_DISPLAY, CultureInfo.InvariantCulture));
                headerDict.Add(ParamDictionaryKeys.ErrorDateEnd, dtpEnd.Value.ToString(DateTimeFormats.DATETIME_FORMAT_DISPLAY, CultureInfo.InvariantCulture));
                //***********************************************************************
                //ProductionStage
                headerDict.Add(ParamDictionaryKeys.ProductionStage, "\n" + 
                               (param.ContainsKey(ParamDictionaryKeys.ProductionStage) ? 
                               param[ParamDictionaryKeys.ProductionStage].Replace("'", "").Replace(",", ";\n") : ErrorUtility.SearchTypeALL));
                //***********************************************************************
                //Meter Type
                headerDict.Add(ParamDictionaryKeys.MeterType, "\n" +
                               (param.ContainsKey(ParamDictionaryKeys.MeterType) ?
                               param[ParamDictionaryKeys.MeterType].Replace("'", "").Replace(",", ";\n") : ErrorUtility.SearchTypeALL));
                //***********************************************************************
                //Error State
                headerDict.Add(ParamDictionaryKeys.ErrorStatus, "\n" +
                              (param.ContainsKey(ParamDictionaryKeys.ErrorStatus) ?
                              param[ParamDictionaryKeys.ErrorStatus].Replace("'", "").Replace(",", ";\n") : ErrorUtility.SearchTypeALL));
                //***********************************************************************
                //Search params
                headerDict.Add(ParamDictionaryKeys.SearchParams, "\n" +
                               (param.ContainsKey(ParamDictionaryKeys.SearchParams) ?
                               param[ParamDictionaryKeys.SearchParams].Replace("'", "").Replace("|", ";\n") : ErrorUtility.SearchTypeALL));

                pdfUtility.createAndSavePDF(ds, filepath, bLandscape, headerDict);
                OpenSavedFile(filepath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Excel from table in Datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItemExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRejectionList.DataSource == null) { MessageBox.Show("No data to Save!", "Cabcon PMP",MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
                bool bLandscape = false;

                DataTable saveTable = new DataTable();
                saveTable = ((DataTable)dgvRejectionList.DataSource).DefaultView.ToTable();

                if (saveTable.TableName.Equals(RejectionTable.dbRejectionMasterTableName))
                {
                    bLandscape = true;
                    var errorQuery = from row in saveTable.AsEnumerable()
                                     group row by row.Field<string>((ErrorTypesTable.colErrorname)) into error
                                     orderby error.Key
                                     select new
                                     {
                                         ErrorType = error.Key,
                                         CountOfErrors = error.Count()
                                     };

                    var dict = errorQuery.ToDictionary(t => t.ErrorType, t => t.CountOfErrors);

                    saveTable.Columns.Remove(ErrorTypesTable.colErrorType);

                    if (!GenerateNormalizedRejectionTable(saveTable)) return;



                    string filepath = string.Empty;
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.AddExtension = true;
                    dialog.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
                    dialog.InitialDirectory = @"C:\Users\SarkaArj\Desktop\";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        filepath = dialog.FileName;
                    }
                    else
                    {
                        return;
                    }

                    //SaveExcel(saveTable, bLandscape, filepath, dict);
                    SaveExcelPerf(saveTable, bLandscape, filepath, dict);
                    OpenSavedFile(filepath);
                }

                else if (saveTable.TableName.Equals(ErrorTypesTable.dbErrorTypeTableName))
                {
                    DataSet EXCELLDS = new DataSet();
                    if (saveTable != null && saveTable.Rows.Count > 0)
                    {
                        saveTable.TableName = "Error Types";
                        EXCELLDS.Tables.Add(saveTable);
                        ExportDataSetToExcell(EXCELLDS);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Create Excel from Rejection table using template excel file in /Configurations
        /// </summary>
        /// <param name="dataSet"></param>
        private void ExportDataSetToExcell(DataSet dataSet)
        {
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook xlWorkbook = ExcelApp.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);

            DataTableCollection collection = dataSet.Tables;
            try
            {
                this.Cursor = Cursors.WaitCursor;

                for (int i = collection.Count; i > 0; i--)
                {
                    Excel.Sheets xlSheets = null;
                    Excel.Worksheet xlWorksheet = null;
                    //Create Excel Sheets
                    xlSheets = ExcelApp.Sheets;
                    //xlWorksheet = (Excel.Worksheet)xlSheets.Add(xlSheets[1], Type.Missing, Type.Missing, Type.Missing);
                    xlWorksheet = (Excel.Worksheet)xlSheets[1];

                    System.Data.DataTable table = collection[i - 1];
                    xlWorksheet.Name = table.TableName;

                    int currXrow = 1;
                    int currYcol = 1;

                    xlWorksheet.Cells[currXrow + 1, currYcol + 0] = ParamDictionaryKeys.ErrorDateStart;
                    xlWorksheet.Cells[currXrow + 1, currYcol + 1] = "\"" + dtpStart.Value.ToString(DateTimeFormats.DATETIME_FORMAT_DISPLAY, CultureInfo.InvariantCulture) + "\"";

                    xlWorksheet.Cells[currXrow + 2, currYcol + 0] = ParamDictionaryKeys.ErrorDateEnd;
                    xlWorksheet.Cells[currXrow + 2, currYcol + 1] = "\"" + dtpEnd.Value.ToString(DateTimeFormats.DATETIME_FORMAT_DISPLAY, CultureInfo.InvariantCulture) + "\"";

                    //***********************************************************************
                    //ProductionStage
                    xlWorksheet.Cells[currXrow + 1, currYcol + 2] = ParamDictionaryKeys.ProductionStage;
                    xlWorksheet.Cells[currXrow + 1, currYcol + 3] = "\n" + 
                               (param.ContainsKey(ParamDictionaryKeys.ProductionStage) ? 
                               param[ParamDictionaryKeys.ProductionStage].Replace("'", "").Replace(",", ";\n") : ErrorUtility.SearchTypeALL);
                    //***********************************************************************
                    //Meter Type
                    xlWorksheet.Cells[currXrow + 2, currYcol + 2] = ParamDictionaryKeys.MeterType;
                    xlWorksheet.Cells[currXrow + 2, currYcol + 3] = "\n" +
                               (param.ContainsKey(ParamDictionaryKeys.MeterType) ?
                               param[ParamDictionaryKeys.MeterType].Replace("'", "").Replace(",", ";\n") : ErrorUtility.SearchTypeALL);
                    //***********************************************************************
                    //Error State
                    xlWorksheet.Cells[currXrow + 1, currYcol + 4] = ParamDictionaryKeys.ErrorStatus;
                    xlWorksheet.Cells[currXrow + 1, currYcol + 5] = "\n" +
                              (param.ContainsKey(ParamDictionaryKeys.ErrorStatus) ?
                              param[ParamDictionaryKeys.ErrorStatus].Replace("'", "").Replace(",", ";\n") : ErrorUtility.SearchTypeALL);
                    //***********************************************************************
                    //Search params

                    xlWorksheet.Cells[currXrow + 2, currYcol + 4] = ParamDictionaryKeys.SearchParams;
                    xlWorksheet.Cells[currXrow + 2, currYcol + 5] = "\n" +
                           (param.ContainsKey(ParamDictionaryKeys.SearchParams) ?
                           param[ParamDictionaryKeys.SearchParams].Replace("'", "").Replace("|", ";\n") : ErrorUtility.SearchTypeALL);
                    

                    currXrow = currXrow + 2 + 2;
                    currYcol = 1;

                    for (int j = 1; j < table.Columns.Count + 1; j++)
                    {
                        ExcelApp.Cells[currXrow, j] = table.Columns[j - 1].ColumnName;

                    }
                    // Storing Each row and column value to excel sheet
                    for (int k = 0; k < table.Rows.Count; k++)
                    {
                        for (int l = 0; l < table.Columns.Count; l++)
                        {
                            if (table.Columns[l].ColumnName.ToUpper().IndexOf("DATE") > 0) ExcelApp.Cells[currXrow + k + 2, l + 1] = "\"" + table.Rows[k].ItemArray[l].ToString() + "\"";
                            else ExcelApp.Cells[currXrow + k + 2, l + 1] = table.Rows[k].ItemArray[l].ToString();
                        }
                    }
                    ExcelApp.Columns.AutoFit();
                }

                ExcelApp.Visible = true;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        /// <summary>
        /// Open Exported File.
        /// </summary>
        /// <param name="filepath"></param>
        private void OpenSavedFile(string filepath)
        {
            if (MessageBox.Show("File Saved Successfully! Open File?", "Cabcon PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                System.Diagnostics.Process.Start(filepath);
        }

        /// <summary>
        /// Save Excel file using block of cells method (more performant than saving each cell individually).
        /// </summary>
        /// <param name="table"></param>
        /// <param name="bLandscape"></param>
        /// <param name="strFileName"></param>
        /// <param name="dict"></param>
        private void SaveExcelPerf(DataTable table, bool bLandscape, string strFileName, Dictionary<string, int> dict)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                Excel.Application xlApp = null;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet, xlWorkSheet2;
                object misValue = System.Reflection.Missing.Value;
                string refFileFormat = AppDomain.CurrentDomain.BaseDirectory + "\\Configuration\\" + "RTRFileFormatRejection" + ".xls";

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(refFileFormat, 0, true, misValue, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                xlWorkSheet.Cells[EXCELCELL.STARTDATE_ROW, EXCELCELL.STARTDATE_COL - 1] = ParamDictionaryKeys.ErrorDateStart;
                xlWorkSheet.Cells[EXCELCELL.STARTDATE_ROW, EXCELCELL.STARTDATE_COL] = dtpStart.Value.ToString(DateTimeFormats.DATETIME_FORMAT_DISPLAY, CultureInfo.InvariantCulture);
                xlWorkSheet.Cells[EXCELCELL.ENDDATE_ROW, EXCELCELL.ENDDATE_COL - 1] = ParamDictionaryKeys.ErrorDateEnd;
                xlWorkSheet.Cells[EXCELCELL.ENDDATE_ROW, EXCELCELL.ENDDATE_COL] = dtpEnd.Value.ToString(DateTimeFormats.DATETIME_FORMAT_DISPLAY, CultureInfo.InvariantCulture);
                //***********************************************************************
                //ProductionStage
                xlWorkSheet.Cells[EXCELCELL.STAGE_ROW, EXCELCELL.STAGE_COL-1] = ParamDictionaryKeys.ProductionStage;
                xlWorkSheet.Cells[EXCELCELL.STAGE_ROW, EXCELCELL.STAGE_COL] = (param.ContainsKey(ParamDictionaryKeys.ProductionStage) ?
                               param[ParamDictionaryKeys.ProductionStage].Replace("'", "").Replace(",", ";\n") : ErrorUtility.SearchTypeALL); ;
                //***********************************************************************
                //Meter Type
                xlWorkSheet.Cells[EXCELCELL.METERTYPE_ROW, EXCELCELL.METERTYPE_COL-1] = ParamDictionaryKeys.MeterType;
                xlWorkSheet.Cells[EXCELCELL.METERTYPE_ROW, EXCELCELL.METERTYPE_COL] = (param.ContainsKey(ParamDictionaryKeys.MeterType) ?
                               param[ParamDictionaryKeys.MeterType].Replace("'", "").Replace(",", ";\n") : ErrorUtility.SearchTypeALL); ;
                //***********************************************************************
                //Error State
                xlWorkSheet.Cells[EXCELCELL.ERRSTATUS_ROW, EXCELCELL.ERRSTATUS_COL-1] = ParamDictionaryKeys.ErrorStatus;
                xlWorkSheet.Cells[EXCELCELL.ERRSTATUS_ROW, EXCELCELL.ERRSTATUS_COL] = (param.ContainsKey(ParamDictionaryKeys.ErrorStatus) ?
                              param[ParamDictionaryKeys.ErrorStatus].Replace("'", "").Replace(",", ";\n") : ErrorUtility.SearchTypeALL); ;
                //***********************************************************************
                //Search params
                xlWorkSheet.Cells[EXCELCELL.CUSTOMER_ROW, EXCELCELL.CUSTOMER_COL-1] = ParamDictionaryKeys.SearchParams;
                xlWorkSheet.Cells[EXCELCELL.CUSTOMER_ROW, EXCELCELL.CUSTOMER_COL] = (param.ContainsKey(ParamDictionaryKeys.SearchParams) ?
                       param[ParamDictionaryKeys.SearchParams].Replace("'", "").Replace("|", ";\n") : ErrorUtility.SearchTypeALL); 
                

                //dataArray[(int)EXCELCELL.STARTDATE_ROW-1, (int)EXCELCELL.STARTDATE_COL-1] = dtpStart.Value.ToShortDateString() + " 00:00:00";
                //dataArray[(int)EXCELCELL.ENDDATE_ROW-1, (int)EXCELCELL.ENDDATE_COL-1] = dtpEnd.Value.ToShortDateString() + " 23:59:59";

                //dataArray[(int)EXCELCELL.STAGE_ROW-1, (int)EXCELCELL.STAGE_COL-1] = cmbStage.SelectedValue.ToString();
                //dataArray[(int)EXCELCELL.METERTYPE_ROW-1, (int)EXCELCELL.METERTYPE_COL-1] = cmbMeterType.SelectedValue.ToString();

                //dataArray[(int)EXCELCELL.ERRSTATUS_ROW-1, (int)EXCELCELL.ERRSTATUS_COL-1] = cmbErrorStatus.SelectedValue.ToString();
                //dataArray[(int)EXCELCELL.CUSTOMER_ROW-1, (int)EXCELCELL.CUSTOMER_COL-1] = txtSrCriteria2.Text;

                int rows = table.Rows.Count+1;
                int columns = 11;

                string[,] dataArray = new string[rows, columns];

                for (int icolcount = 0; icolcount < table.Columns.Count; icolcount++)
                {
                    string testpointname = table.Columns[icolcount].ColumnName;
                    dataArray[0, icolcount] = testpointname;
                }

                for (int irowcount = 0; irowcount < table.Rows.Count; irowcount++)
                {
                    for (int icolcount = 0; icolcount < table.Columns.Count; icolcount++)
                    {
                        var testpointdata = table.Rows[irowcount][icolcount];
                        if (table.Columns[icolcount].DataType == typeof(DateTime))
                        {
                            dataArray[irowcount + 1, icolcount] = "\"" + ((DateTime)testpointdata).ToString(DateTimeFormats.DATETIME_FORMAT_DISPLAY, CultureInfo.InvariantCulture) + "\"";
                        }
                        else
                        {
                            dataArray[ irowcount+1,  icolcount] = testpointdata.ToString();
                        }
                    }
                }

                //xlWorkSheet.Rows[rows, columns] = dataArray;

                xlWorkSheet.get_Range((Excel.Range)(xlWorkSheet.Cells[11, 2]), (Microsoft.Office.Interop.Excel.Range)(xlWorkSheet.Cells[rows+10, columns+1])).Value2 = dataArray;


                int rows2 = dict.Count;
                int columns2 = 4;
                object[,] dataArray2 = new object[rows, columns];

                xlWorkSheet2 = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2);
                var sorteddict = dict.OrderByDescending(i => i.Value).ToDictionary(i => i.Key, i => i.Value);
                int count = 0;
                int total = sorteddict.Sum(x => x.Value);
                for (count = 0; count < sorteddict.Count; count++)
                {
                    dataArray2[count, (int)EXCELCELL.ERRTYPE_COL-1] = sorteddict.ElementAt(count).Key.Replace('_', ' ');
                    dataArray2[count, (int)EXCELCELL.ERRTYPECOUNT_COL-1] = sorteddict.ElementAt(count).Value;

                    float percent = 0;
                    float subtotal = 0;
                    for (int item = 0; item <= count; item++)
                    {
                        subtotal = subtotal + sorteddict.ElementAt(item).Value;
                    }
                    percent = subtotal / total * 100;
                    dataArray2[count, (int)EXCELCELL.CUMPERCENT_COL-1] = String.Format("{0:000.00}%", percent);
                }

                dataArray2[0, (int)EXCELCELL.TOTAL_COL-1] = total.ToString();    //String.Format("=SUM(B2:B{0})", (EXCELCELL.ERRTYPECOUNT_ROW + count));

                //xlWorkSheet2.Rows[rows2, columns2] = dataArray2;

                xlWorkSheet2.get_Range((Excel.Range)(xlWorkSheet2.Cells[2, 1]), (Excel.Range)(xlWorkSheet2.Cells[rows2+1, columns2])).Value2 = dataArray2;


                Excel.ChartObject chart = (Excel.ChartObject)xlWorkSheet2.ChartObjects(1);

                Excel.Range datarange = xlWorkSheet2.get_Range(xlWorkSheet2.Cells[EXCELCELL.ERRTYPE_ROW - 1, EXCELCELL.ERRTYPE_COL], xlWorkSheet2.Cells[EXCELCELL.CUMPERCENT_ROW + count - 1, EXCELCELL.CUMPERCENT_COL]);
                chart.Chart.SetSourceData(datarange, Excel.XlRowCol.xlColumns);




                xlWorkBook.Close(true, strFileName, misValue);
                xlApp.Quit();

                ReleaseObject(xlWorkSheet);
                ReleaseObject(xlWorkSheet2);
                ReleaseObject(xlWorkBook);
                ReleaseObject(xlApp);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                throw ex;
            }
        }


      
        /// <summary>
        /// Release Held Resource.
        /// </summary>
        /// <param name="obj"></param>
        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + "\n" + ex.ToString(), "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
              
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// Open form to insert item to rejection list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdateEntry frmobj = new frmUpdateEntry(objetyusermgt);
            frmobj.ShowDialog();
        }
        /// <summary>
        /// Open form to update item to rejection list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvRejectionList_CellDoubleClick(new object(), new DataGridViewCellEventArgs(0, 0));
        }

        /// <summary>
        /// Deletes the selected record and all associated history for that PCBAID & Production Stage entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteRejectionRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CabconPMPREJECTIONTOOL.AccessPassword frmobj = new CabconPMPREJECTIONTOOL.AccessPassword();
            try
            {
                string password = balerrorobj.GetPasswordsForEntryModification(ErrorUtility.KeywordDeleteEntry);
                 frmobj.ShowDialog();
                if (frmobj.Password.Equals(password))
                {
                    if (!(MessageBox.Show("Are you sure you want to Delete this record? \n This Record and all associated history will be deleted!", "Cabcon PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)) { return; }
               
                    string pcbaid = dgvRejectionList.Rows[dgvRejectionList.CurrentCell.RowIndex].Cells[RejectionTable.colPCBAID].Value.ToString();
                    string stage = dgvRejectionList.Rows[dgvRejectionList.CurrentCell.RowIndex].Cells[RejectionTable.colProductionStage].Value.ToString();
                    if (balerrorobj.DeleteRecord_PCBAID_Stage(pcbaid, stage))
                    {
                        MessageBox.Show("Record Deleted Successfully!", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        displayRejectionListToolStripMenuItem_Click(new object(), new EventArgs());
                    }
                    else
                        MessageBox.Show("Record could not be deleted!", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Wrong password!", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkUniquePCBAID_CheckedChanged(object sender, EventArgs e)
        {
            //displayRejectionListToolStripMenuItem_Click(new object(), EventArgs.Empty);
        }
        /// <summary>
        /// Paint Row No. on left side of dataGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRejectionList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1) //It's header cell
            {
                e.PaintBackground(e.CellBounds, false);

                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);

                if (dgvRejectionList.SortedColumn != null && dgvRejectionList.SortedColumn.Index == e.ColumnIndex)
                {
                    var sortIcon = dgvRejectionList.SortOrder == SortOrder.Ascending ? "▲" : "▼";


                    TextRenderer.DrawText(e.Graphics, sortIcon,
                        e.CellStyle.Font, e.CellBounds, Color.IndianRed,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
                }


                e.Handled = true;
            }
        }
        /// <summary>
        /// Check-uncheck logibc for checkList boxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkListSource_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox box = sender as CheckedListBox;
            if(box==null) box = sender as MyCheckedListBox;
            box.ItemCheck -= new ItemCheckEventHandler(chkListSource_ItemCheck);
            if (e.Index == 0)
            {
                
                if (e.NewValue == CheckState.Checked)
                {
                    for (int i = 1; i < box.Items.Count; i++)
                    {
                        box.SetItemChecked(i, true);
                    }
                }
                else
                {
                    for (int i = 1; i < box.Items.Count; i++)
                    {
                        box.SetItemChecked(i, false);
                    }
                }
                
            }
            else
            {
                if (e.NewValue == CheckState.Unchecked)
                {
                    box.SetItemChecked(0, false);
                }
                else
                {
                    if(box.CheckedItems.Count == box.Items.Count-2)
                        box.SetItemChecked(0, true);
                }
            }
            box.SetItemCheckState(e.Index, e.NewValue);
            box.ItemCheck += new ItemCheckEventHandler(chkListSource_ItemCheck);

            if(Current_Table == RejectionTable.dbRejectionMasterTableName)
                SetDataGridViewRowFilter(BuildSearchQuery(), RejectionTable.dbRejectionMasterTableName);
        }

        private string AppendUserToSearchQuery(string query, string value)
        {
            query += string.IsNullOrEmpty(query) ? string.Empty : " AND ";
            query += string.Format(" ( {0} = '{2}' OR {1} = '{2}' ) ", RejectionTable.colLoggedUserID, RejectionTable.colLastRepairID, value );
            return query;
        }
        /// <summary>
        /// Build a Search query
        /// </summary>
        /// <returns></returns>
        private string BuildSearchQuery()
        {
            StringBuilder query = new StringBuilder();
            string valueText = string.Empty;
            //***********************************************************************
            //Meter Type
            if (chkListMeterType.CheckedItems.Count > 0)
            {
                valueText = string.Empty;
                for (int i = 0; i < chkListMeterType.CheckedItems.Count; i++)
                {
                    if (((string)chkListMeterType.CheckedItems[i]).ToUpper().Contains("ALL")) { continue; }
                    valueText += string.Format("'{0}',", (string)chkListMeterType.CheckedItems[i]);
                }
                valueText = valueText.Trim(',');
                if (!string.IsNullOrEmpty(valueText)) param[ParamDictionaryKeys.MeterType] = valueText;

                query.AppendFormat(" AND {0} IN ({1}) ", RejectionTable.colMeterType, valueText);
            }
            else
            {
                param.Remove(ParamDictionaryKeys.MeterType);
            }
            //***********************************************************************
            //Production Stage
            if (chkListProdStage.CheckedItems.Count > 0)
            {
                valueText = string.Empty;
                for (int i = 0; i < chkListProdStage.CheckedItems.Count; i++)
                {
                    if (((string)chkListProdStage.CheckedItems[i]).ToUpper().Contains("ALL")) continue;
                    valueText += string.Format("'{0}',", (string)chkListProdStage.CheckedItems[i]);
                }
                valueText = valueText.Trim(',');
                if (!string.IsNullOrEmpty(valueText)) param[ParamDictionaryKeys.ProductionStage] = valueText;

                query.AppendFormat(" AND {0} IN ({1}) ", RejectionTable.colProductionStage, valueText);
            }
            else
            {
                param.Remove(ParamDictionaryKeys.ProductionStage);
            }
            //***********************************************************************
            //Error State
            if (chkListErrorStatus.CheckedItems.Count > 0)
            {
                valueText = string.Empty;
                for (int i = 0; i < chkListErrorStatus.CheckedItems.Count; i++)
                {
                    if (((string)chkListErrorStatus.CheckedItems[i]).ToUpper().Contains("ALL")) continue;
                    valueText += string.Format("'{0}',", (string)chkListErrorStatus.CheckedItems[i]);
                }
                valueText = valueText.Trim(',');
                if (!string.IsNullOrEmpty(valueText)) param[ParamDictionaryKeys.ErrorStatus] = valueText;

                query.AppendFormat(" AND {0} IN ({1}) ", RejectionTable.colErrorState, valueText);
            }
            else
            {
                param.Remove(ParamDictionaryKeys.ErrorStatus);
            }
            //***********************************************************************
            //Meter Search Criteria
            if (chkListSearchBy.CheckedItems.Count > 0)
            {
                valueText = string.Empty;
                for (int i = 0; i < chkListSearchBy.CheckedItems.Count; i++)
                {
                    if (((string)chkListSearchBy.CheckedItems[i]).ToUpper().Contains("ALL")) { continue; }
                    valueText += string.Format("{0}={1}|", (string)chkListSearchBy.CheckedItems[i], panelSearchText.Controls.Find("txtSearch" + (string)chkListSearchBy.CheckedItems[i], true)[0].Text);
                }
                valueText = valueText.Trim('|');

                param[ParamDictionaryKeys.SearchParams] = valueText;

                string[] searchParams = param[ParamDictionaryKeys.SearchParams].Split('|');
                if (searchParams != null && !string.IsNullOrEmpty(searchParams[0]))
                {
                    foreach (string item in searchParams)
                    {
                        string[] keyValue = item.Split('=');
                        query.AppendFormat(" AND {0} LIKE '%{1}%' ", keyValue[0], keyValue[1]);
                    }
                }
            }
            else
            {
                param.Remove(ParamDictionaryKeys.SearchParams);
            }
            //***********************************************************************
            //Meter PCB Source
            if (chkListSource.CheckedItems.Count > 0)
            {
                valueText = string.Empty;
                for (int i = 0; i < chkListSource.CheckedItems.Count; i++)
                {
                    if (((string)chkListSource.CheckedItems[i]).ToUpper().Contains("ALL")) continue;
                    valueText += string.Format("'{0}',", (string)chkListSource.CheckedItems[i]);
                }
                valueText = valueText.Trim(',');

                param[ParamDictionaryKeys.PCBASource] = valueText;

                query.AppendFormat(" AND {0} IN ({1}) ", RejectionTable.colPCBSource, valueText);
            }
            else
            {
                param.Remove(ParamDictionaryKeys.PCBASource);
            }
            //***********************************************************************
            //Production Shift
            if (chkListShift.CheckedItems.Count > 0)
            {
                valueText = string.Empty;
                for (int i = 0; i < chkListShift.CheckedItems.Count; i++)
                {
                    if (((string)chkListShift.CheckedItems[i]).ToUpper().Contains("ALL")) continue;
                    valueText += string.Format("'{0}',", (string)chkListShift.CheckedItems[i]);
                }
                valueText = valueText.Trim(',');
                if (!string.IsNullOrEmpty(valueText)) param[ParamDictionaryKeys.ProductionShift] = valueText;

                query.AppendFormat(" AND {0} IN ({1}) ", param[ParamDictionaryKeys.ErrorDateType] == RejectionTable.colErrorDate ? RejectionTable.colShiftEDate : RejectionTable.colShiftADate
                                                       , valueText);
            }
            else
            {
                param.Remove(ParamDictionaryKeys.ProductionShift);
            }

            return query.Length > 4 ? query.Replace("AND", "", 0, 6).ToString() : string.Empty;
        }
        /// <summary>
        /// Apply searchquery to DataTable
        /// </summary>
        /// <param name="query"></param>
        private void SetDataGridViewRowFilter(string query, string tableName)
        {
            DataTable table = dgvRejectionList.DataSource as DataTable;
            if (Current_Table == tableName)
            {
                table.DefaultView.RowFilter = query;
                txtRecordCount.Text = table.DefaultView.Count.ToString();
            }
            if (!query.Contains(RejectionTable.colLoggedUserID))
            {
                lblLoggedUserID.Text = ErrorUtility.SearchTypeALL;
            }
        }

         private void txtSearchPCBAID_TextChanged(object sender, EventArgs e)
        {
            SetDataGridViewRowFilter(BuildSearchQuery(), RejectionTable.dbRejectionMasterTableName);
        }

        /// <summary>
        /// Expander button logic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCollapse_Click(object sender, EventArgs e)
        {
            ///***************************************************
            ///panelFilter minimized height = 34
            ///panelFilter expanded height = 254
            
            Button button = sender as Button;
            if (button.Text.Equals("---------▼---------"))
            {
                while (panelFilters.Height < 254)
                {
                    panelFilters.Height+=20;
                    Application.DoEvents();
                }
                button.Text = "---------▲---------";
            }
            else
            {
                while (panelFilters.Height > 34)
                {
                    panelFilters.Height-=20;
                    Application.DoEvents();
                }
                button.Text = "---------▼---------";
            }
        }

       /// <summary>
        ///  Context menu logic for Logged in users for click event.
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void contextMenuUserID_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            DataTable table = dgvRejectionList.DataSource as DataTable;
            if (e.ClickedItem.Text == ErrorUtility.SearchTypeALL)
            {
                if (table.DefaultView.RowFilter.Contains(RejectionTable.colLoggedUserID))
                {
                    table.DefaultView.RowFilter = table.DefaultView.RowFilter.Contains("AND") ? table.DefaultView.RowFilter.Remove(table.DefaultView.RowFilter.LastIndexOf("AND")) : string.Empty;
                    txtRecordCount.Text = table.DefaultView.Count.ToString();
                }
            }
            else
            {
                string query = BuildSearchQuery();
                query = AppendUserToSearchQuery(query, e.ClickedItem.Text);
                SetDataGridViewRowFilter(query, RejectionTable.dbRejectionMasterTableName);
            }
            lblLoggedUserID.Text = e.ClickedItem.Text;
        }
        /// <summary>
        /// ReApply double bufferred to datagridview on  Resize end event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmErrorList_ResizeEnd(object sender, EventArgs e)
        {
            if (!System.Windows.Forms.SystemInformation.TerminalServerSession)
            {
                Type dgvType = dgvRejectionList.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(dgvRejectionList, true, null);
            }
        }
        /// <summary>
        /// Apply datasource for Datagridview
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="table"></param>
        private void SetDataGridViewDataSource(DataGridView dataGridView, DataTable table)
        {
            if (!System.Windows.Forms.SystemInformation.TerminalServerSession)
            {
                Type dgvType = dataGridView.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(dataGridView, true, null);
            }

            dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView.RowHeadersVisible = false;

            Current_Table = table.TableName;
            dataGridView.DataSource = table;
            
            dataGridView.Refresh();

            dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGridView.RowHeadersVisible = true;
            this.Focus();
        }

        private void contextMenuUserID_Opening(object sender, CancelEventArgs e)
        {
            if (Current_Table == RejectionTable.dbRejectionMasterTableName)
            {
                e.Cancel = false;
                //contextMenuUserID.Items.Clear();
                //var header = new ToolStripMenuItem(RejectionTable.colLoggedUserID);
                //header.Enabled = false;
                //contextMenuUserID.Items.Add(header);
                //contextMenuUserID.Items.Add(new ToolStripSeparator());
                //contextMenuUserID.Items.Add(ErrorUtility.SearchTypeALL);
                //foreach (DataRow item in tableRejectionMaster.DefaultView.ToTable(true, RejectionTable.colLoggedUserID).Rows)
                //{
                //    if (!contextMenuUserID.Items.ContainsKey(item[RejectionTable.colLoggedUserID].ToString().ToLower()))
                //        contextMenuUserID.Items.Add(new ToolStripMenuItem(item[RejectionTable.colLoggedUserID].ToString(),null,null,item[RejectionTable.colLoggedUserID].ToString().ToLower()));
                //}
                //lblLoggedUserID.Text = contextMenuUserID.Items[2].Text;
            }
            else
            {
                //contextMenuUserID.Items.Clear();
                e.Cancel = true;
            }
        }
        /// <summary>
        /// Open context menu on mouseclick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRejectionList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int currentMouseColumn = dgvRejectionList.HitTest(e.X, e.Y).ColumnIndex;

                if ( Current_Table == RejectionTable.dbRejectionMasterTableName
                                //&& (dgvRejectionList.Columns[currentMouseColumn].Name == RejectionTable.colLoggedUserID)
                    )
                {
                    contextMenuUserID.Items.Clear();
                    string headerName = RejectionTable.colLoggedUserID; //dgvRejectionList.Columns[currentMouseColumn].Name
                    var header = new ToolStripMenuItem(headerName);
                    header.Enabled = false;
                    contextMenuUserID.Items.Add(header);
                    contextMenuUserID.Items.Add(new ToolStripSeparator());
                    contextMenuUserID.Items.Add(ErrorUtility.SearchTypeALL);
                    contextMenuUserID.Items.Add(new ToolStripSeparator());
                    var userTable = tableRejectionMaster.DefaultView.ToTable(true, headerName);
                    var rows = userTable.Rows;
                    if (rows.Count < 50)
                    {
                        foreach (DataRow item in rows)
                        {
                            if (!string.IsNullOrEmpty(item[headerName].ToString().Trim()) && !contextMenuUserID.Items.ContainsKey(item[headerName].ToString().ToLower()))
                                contextMenuUserID.Items.Add(new ToolStripMenuItem(item[headerName].ToString(), null, null, item[headerName].ToString().ToLower()));
                        }
                        lblLoggedUserID.Text = contextMenuUserID.Items[2].Text;
                    }
                    headerName = RejectionTable.colLastRepairID;
                    userTable = tableRejectionMaster.DefaultView.ToTable(true, headerName);
                    rows = userTable.Rows;
                    if (rows.Count < 50)
                    {
                        foreach (DataRow item in rows)
                        {
                            if (!string.IsNullOrEmpty(item[headerName].ToString().Trim()) && !contextMenuUserID.Items.ContainsKey(item[headerName].ToString().ToLower()))
                                contextMenuUserID.Items.Add(new ToolStripMenuItem(item[headerName].ToString(), null, null, item[headerName].ToString().ToLower()));
                        }
                    }
                }
                else
                {
                    contextMenuUserID.Items.Clear();
                }

                contextMenuUserID.Show(dgvRejectionList, new Point(e.X, e.Y));
            }
        }

        private void liveChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
           FillParamsForQuery(ref param, RejectionTable.dbRejectionMasterTableName);
           frmDisplayChart.ShowInstance(param, BuildSearchQuery());
        }

    }
}
