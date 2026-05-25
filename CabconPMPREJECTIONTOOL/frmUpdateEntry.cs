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

namespace CabconPMPREJECTIONTOOL
{
    /// <summary>
    /// Class for Inserting/Updating Existing Rejection Entry.
    /// </summary>
    public partial class frmUpdateEntry : Form
    {
        BALErrorOperations balerrorobj = new BALErrorOperations();
        DataTable tableErrorTypes;
        long rowid = 0;
        string loggeduserid;
        string loggedusertype;
        /// <summary>
        /// Create new instance using login details.
        /// </summary>
        /// <param name="objetyusermgt"></param>
        public frmUpdateEntry(EntityUserManagement objetyusermgt)
        {
            InitializeComponent();
            loggeduserid = objetyusermgt.LoginuserID;
            loggedusertype = objetyusermgt.LogType;
            PerformStartupActions();
            btnNew_Click(new object(), EventArgs.Empty);
        }
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public frmUpdateEntry()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Draw table borders for layout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            Pen borderPen = new Pen(Color.FromArgb(170,170,170));

            e.Graphics.DrawLine(borderPen, e.CellBounds.Location, new Point(e.CellBounds.Right, e.CellBounds.Top));

            if (e.Row == tableLayoutPanel1.RowCount - 1)
            {
                Point tempPoint = e.CellBounds.Location;
                tempPoint.Offset(0, e.CellBounds.Height-1);
                e.Graphics.DrawLine(borderPen, tempPoint, new Point(e.CellBounds.Right, e.CellBounds.Bottom-1));
            }

            if(e.Column==0)
                e.Graphics.DrawLine(borderPen, e.CellBounds.Location, new Point(e.CellBounds.Left, e.CellBounds.Bottom));

            if (e.Column == tableLayoutPanel1.ColumnCount-1)
            {
                Point tempPoint = e.CellBounds.Location;
                tempPoint.Offset(e.CellBounds.Width-1, 0);
                e.Graphics.DrawLine(borderPen, tempPoint, new Point(e.CellBounds.Right-1, e.CellBounds.Bottom));
            }   
        }
        /// <summary>
        /// Perform Startup actions during initialization of new instance of frmUpdateEntry class.
        /// </summary>
        private void PerformStartupActions()
        {
            this.tableLayoutPanel1.CellPaint += tableLayoutPanel1_CellPaint;
            
            this.cmbProdStage.DataSource = new List<string>(){
               StaticVariables.TestType_Cal,
               StaticVariables.TestType_EMS,
               StaticVariables.TestType_FT,
               StaticVariables.TestType_SR,
               StaticVariables.TestType_OT,
               "Sealing",
               "Assembly",
            };

            this.cmbMeterType.DataSource = new List<string>(){
                StaticVariables.MeterType_1PH_E150_ES,
                StaticVariables.MeterType_1PH_E150_MS,
                StaticVariables.MeterType_1PH_E150_MSD,
                StaticVariables.MeterType_3PH_DLMS,
                StaticVariables.MeterType_3PH_Sapphire,
                StaticVariables.MeterType_1PH_SM,
                StaticVariables.MeterType_3PH_SM,
                StaticVariables.MeterType_1PH_ZCE
            };

            this.cmbErrorState.DataSource = new List<string>(){
                ErrorStateConstants.StatePassed,
                ErrorStateConstants.StateInitial,
                ErrorStateConstants.StateAnalysis,
                ErrorStateConstants.StateProcessed
            };

            BindErrorTypesToCmb();
        }
        /// <summary>
        /// Bind Error Types to relevant combobox.
        /// </summary>
        private void BindErrorTypesToCmb()
        {
            try
            {
                tableErrorTypes = balerrorobj.GetErrorTypesTableWithCount();
                cmbErrorType.DataSource = tableErrorTypes;
                //cmbErrorType.DisplayMember = ErrorTypesTable.colErrorType;
                cmbErrorType.DisplayMember = ErrorTypesTable.colErrorname;
                //cmbErrorType.ValueMember = ErrorTypesTable.colErrorname;
                cmbErrorType.BindingContext = this.BindingContext;
                //txtNewErrorType.ReadOnly = true;
                txtError_Name.ReadOnly = true;
                txtErrorDescription.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Create new instance of frmUpdateEntry class using login details and rejection entry information.
        /// </summary>
        /// <param name="dgvSelectedRow"></param>
        /// <param name="tableextinfo"></param>
        /// <param name="objetyusermgt"></param>
        public frmUpdateEntry(DataRow dgvSelectedRow, DataTable tableextinfo, EntityUserManagement objetyusermgt)
        {
            InitializeComponent();

            loggeduserid = objetyusermgt.LoginuserID;
            loggedusertype = objetyusermgt.LogType;
            rowid = long.Parse(dgvSelectedRow[RejectionTable.colIDReject].ToString());
            txtPCBAID.Text = dgvSelectedRow[RejectionTable.colPCBAID].ToString();
            txtCustomer.Text = dgvSelectedRow[RejectionTable.colCustomer].ToString();
            txtPCBAID.ReadOnly = true;
            cmbProdStage.Enabled = false;
            cmbMeterType.Enabled = false;
            txtCustomer.ReadOnly = true;
            dtpErrorDate.Value = (DateTime)dgvSelectedRow[RejectionTable.colErrorDate];
            dtpErrorDate.Enabled = false;
            txtError_Name.Text = tableextinfo.Rows[0][ErrorTypesTable.colErrorname].ToString();
            txtNewErrorType.Text = tableextinfo.Rows[0][ErrorTypesTable.colErrorType].ToString();
            txtErrorDescription.Text = tableextinfo.Rows[0][ErrorTypesTable.colErrorDescription].ToString();
            txtParam_Name.Text = tableextinfo.Rows[0][RejectionTable.colParameterName].ToString();
            txtProbDesc.Text = dgvSelectedRow[RejectionTable.colProblemDescription].ToString();
            txtRejectCause.Text = dgvSelectedRow[RejectionTable.colRejectionCause].ToString();
            txtRejectAction.Text = dgvSelectedRow[RejectionTable.colRejectionAction].ToString();
            dtpActionDate.Value = (DateTime)dgvSelectedRow[RejectionTable.colActionDate];
            txtMore_Info.Text = tableextinfo.Rows[0][RejectionTable.colMoreInformation].ToString();

            PerformStartupActions();
            
            this.cmbProdStage.SelectedItem = dgvSelectedRow[RejectionTable.colProductionStage].ToString();
            
            this.cmbMeterType.SelectedItem = dgvSelectedRow[RejectionTable.colMeterType].ToString();
          
            this.cmbErrorState.SelectedItem = dgvSelectedRow[RejectionTable.colErrorState].ToString();

            int id = Int32.Parse(dgvSelectedRow[RejectionTable.colIDError].ToString());

            var resultrow = from DataRow myRow in tableErrorTypes.Rows
                          where (int)myRow[ErrorTypesTable.colIdError] == id
                          select myRow;
            int idx = tableErrorTypes.Rows.IndexOf(resultrow.ElementAt(0));

            cmbErrorType.SelectedIndex = idx;
            //cmbErrorType.SelectedIndex = -1;
        }
        /// <summary>
        /// Action to occur on click of "NEW" Button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            rowid = 0;
            btnUpdate.Enabled = false;
            btnInsert.Enabled = true;
            //txtPCBAID.Text = "";
            //txtCustomer.Text = "";
            txtCustomer.ReadOnly = false;
            cmbMeterType.Enabled = true;
            cmbProdStage.Enabled = true;
            dtpErrorDate.Value = DateTime.Now;
            txtError_Name.Text = "";
            txtErrorDescription.Text = "";
            txtParam_Name.Text = "";
            txtProbDesc.Text = "";
            txtRejectCause.Text = "";
            txtRejectAction.Text = "";
            dtpActionDate.Value = DateTime.Now;
            txtMore_Info.Text = "";
            //txtNewErrorType.Text = "";
            txtError_Name.Text = "";
            txtErrorDescription.Text = "";
            cmbErrorState.SelectedIndex = -1;
            cmbErrorType.SelectedIndex = -1;
            cmbMeterType.SelectedIndex = -1;
            cmbProdStage.SelectedIndex = -1;
        }
        /// <summary>
        /// Builds a new object of EntityError class using relevant information.
        /// </summary>
        /// <returns></returns>
        private EntityError BuildErrorEntity()
        {
            EntityError errentity = new EntityError();
            errentity.Rejectid = rowid.ToString();
            errentity.PCBAID = txtPCBAID.Text;
            errentity.Customer = txtCustomer.Text;
           //string expr = String.Format("{0} = '{1}'", ErrorTypesTable.colErrorType, cmbErrorType.SelectedValue.ToString());
            errentity.ErrorID = tableErrorTypes.Rows[cmbErrorType.SelectedIndex][ErrorTypesTable.colIdError].ToString();
            errentity.ProcedureStage = cmbProdStage.SelectedValue.ToString();

            if (!string.IsNullOrEmpty(txtRejectAction.Text.Trim()))
                errentity.ErrorState = ErrorStateConstants.StateProcessed;
            else if (!string.IsNullOrEmpty(txtRejectCause.Text.Trim()) || !string.IsNullOrEmpty(txtProbDesc.Text.Trim()))
                errentity.ErrorState = ErrorStateConstants.StateAnalysis;
            else
                errentity.ErrorState = ErrorStateConstants.StateAnalysis;

            errentity.MeterType = cmbMeterType.SelectedValue.ToString();
            errentity.LoggedUserID = loggeduserid;
            errentity.LastRepairID = loggeduserid;
            errentity.ErrorDate = dtpErrorDate.Value;
            errentity.ActionDate = DateTime.Now;
            errentity.ParameterName = txtParam_Name.Text;
            errentity.ProblemDescription = txtProbDesc.Text;
            errentity.RejectionCause = txtRejectCause.Text;
            errentity.ActionTaken = txtRejectAction.Text;
            errentity.MoreInformation = txtMore_Info.Text;
            errentity.WorkStationID = System.Environment.MachineName;
            return errentity;
        }
        /// <summary>
        /// Actions that occur on changing Error Type combobox selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbErrorType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbErrorType.SelectedIndex != -1)
                {
                    txtError_Name.Text = ((DataRowView)cmbErrorType.SelectedValue)[ErrorTypesTable.colErrorname].ToString();
                    txtErrorDescription.Text = ((DataRowView)cmbErrorType.SelectedValue)[ErrorTypesTable.colErrorDescription].ToString();
                    txtNewErrorType.Text = ((DataRowView)cmbErrorType.SelectedValue)[ErrorTypesTable.colErrorType].ToString();
                    //txtNewErrorType.ReadOnly = true;
                    txtError_Name.ReadOnly = true;
                    txtErrorDescription.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Action to occur on click of "INSERT" Button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateItems()) return;
                
                bool bSuccess;

                if (txtError_Name.ReadOnly == false && !String.IsNullOrEmpty(txtError_Name.Text.Trim()))
                {
                    CabconPMPREJECTIONTOOL.AccessPassword frmobj = new CabconPMPREJECTIONTOOL.AccessPassword();
                    EntityErrorType errEntityType=null;
                    try
                    {
                        string password = balerrorobj.GetPasswordsForEntryModification(ErrorUtility.KeywordNewError);
                        frmobj.ShowDialog();
                        if (frmobj.Password.Equals(password))
                        {
                            errEntityType = BuildEntityErrorType();
                            bSuccess = balerrorobj.InsertToErrorTypesTable(errEntityType);
                            if (!bSuccess) { MessageBox.Show("Unable to Upload Data!", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
                        }
                        else
                        {
                            MessageBox.Show("Wrong password!", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    BindErrorTypesToCmb();
                    txtNewErrorType.Text = errEntityType.ErrorType;
                    txtError_Name.Text = errEntityType.ErrorName;
                    txtErrorDescription.Text = errEntityType.ErrorDescription;
                }

                var resultrow = from DataRow myRow in tableErrorTypes.Rows
                                where (string)myRow[ErrorTypesTable.colErrorType] == txtNewErrorType.Text
                                select myRow;
                int idx = tableErrorTypes.Rows.IndexOf(resultrow.ElementAt(0));
                cmbErrorType.SelectedIndex = idx;

                EntityError errentity = BuildErrorEntity();

                if (!(MessageBox.Show(string.Format("Do you want to upload this data?\n\nPCBAID - {0}\n\nProduction Stage - {1}\n\nMeter Type - {2}\n\nError Type - {3}\n\nError State - {4}\n\nCustomer Name - {5}"
                                                                , errentity.PCBAID
                                                                , errentity.ProcedureStage
                                                                , errentity.MeterType
                                                                , txtError_Name.Text
                                                                , errentity.ErrorState
                                                                , string.IsNullOrEmpty(txtCustomer.Text.Trim()) ? "None" : txtCustomer.Text), "Cabcon PMP Rejection", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)) { return; }

                bSuccess = balerrorobj.InsertToRejectionTable(errentity);
                if (bSuccess)
                { 
                    MessageBox.Show("Data Uploaded Successfully!","Cabcon PMP",MessageBoxButtons.OK,MessageBoxIcon.Information); 
                }
                else { MessageBox.Show("Unable to Upload Data!", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Validates if relevant data in form is valid.
        /// </summary>
        /// <returns></returns>
        private bool ValidateItems()
        {
            bool bvalid = true ;
            if (String.IsNullOrEmpty(txtPCBAID.Text.Trim()) || txtPCBAID.Text.Trim().Length < 6) { lblPCBAID.ForeColor = Color.Red; bvalid = false; } else lblPCBAID.ForeColor = Color.Black;
            if (String.IsNullOrEmpty(txtCustomer.Text.Trim())) { lblCustomer.ForeColor = Color.Red; bvalid = false; } else lblCustomer.ForeColor = Color.Black;
            if (cmbProdStage.SelectedIndex == -1) { lblProductionStage.ForeColor = Color.Red; bvalid = false; } else lblProductionStage.ForeColor = Color.Black;
            if (cmbMeterType.SelectedIndex == -1) { lblMeterType.ForeColor = Color.Red; bvalid = false; } else lblMeterType.ForeColor = Color.Black;
            if (cmbErrorType.SelectedIndex == -1) { lblErrorType.ForeColor = Color.Red; bvalid = false; } else lblErrorType.ForeColor = Color.Black;
            if (String.IsNullOrEmpty(txtError_Name.Text.Trim())) { lblErrorName.ForeColor = Color.Red; bvalid = false; } else lblErrorName.ForeColor = Color.Black;
            if (!bvalid) { MessageBox.Show("Please Enter All required Fields","Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Information); }

            return bvalid;
        }
        /// <summary>
        /// Build a new object of EntityErrorType class using relevant information in form.
        /// </summary>
        /// <returns>EntityErrorType</returns>
        private EntityErrorType BuildEntityErrorType()
        {
            EntityErrorType errTypeEntity = new EntityErrorType();
            errTypeEntity.ErrorName = ErrorUtility.ConvertToPascalCase(txtError_Name.Text.Trim(), false).Replace("HW", "").Trim() + " (HW)";
            errTypeEntity.ErrorType = "HW_" + ErrorUtility.ConvertToPascalCase(txtError_Name.Text.Trim(), true).Replace("HW", "") + "_FAIL";
            txtNewErrorType.Text = errTypeEntity.ErrorType;
            errTypeEntity.ErrorDescription = txtErrorDescription.Text;
            return errTypeEntity;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //cmbErrorType.SelectedIndex = -1;
            //txtNewErrorType.ReadOnly = false;
            txtError_Name.ReadOnly = false;
            txtErrorDescription.ReadOnly = false;
            txtNewErrorType.Text = "";
            txtError_Name.Text = "";
            txtErrorDescription.Text = "";
        }

        private void cmbProdStage_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            e.DrawBackground();

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.LightBlue), e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.Window), e.Bounds);
            }

            if (e.Index >= 0)
            {
                e.Graphics.DrawString(box.Items[e.Index].ToString(), e.Font,
                     new SolidBrush(Color.Black), e.Bounds, StringFormat.GenericDefault);
            }
            e.DrawFocusRectangle();
        }
    }
}
