using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ApplicationInterface;
using COMMONENTITY;
using BALLAYER;
using System.Globalization;

namespace CabconPMPREJECTIONTOOL
{
    /// <summary>
    /// Class for entering Errors that cannot be auto captured through software.
    /// </summary>
    public partial class frmHardError : Form
    {
        private static frmHardError openForm = null;
        private string meterType = "";
        private string stage = "";
        private string loggeduser;
        private string loggedusertype;
        private LayerInterface objLI = new LayerInterface();
        private IECLayerInterface objIECLI = new IECLayerInterface();
        private AppSettings objappSettings = new AppSettings();
       // MyCrypro objcrypt = new MyCrypro();
        private CommonCommandMethods objccmdmethod = new CommonCommandMethods();
        private BALErrorOperations balerrorobj = new BALErrorOperations();

        /// <summary>
        /// Ensure only one instance of this form exists
        /// </summary>
        /// <param name="objetyusermgt"></param>
        /// <returns></returns>
        public static frmHardError GetInstance(EntityUserManagement objetyusermgt)
        {
            if (openForm == null)
            {
                openForm = new frmHardError(objetyusermgt, null);
                openForm.FormClosed += delegate { openForm = null; };
            }
            return openForm;
        }
        /// <summary>
        /// Buils a new instance of this class using login details and pcbaid.
        /// </summary>
        /// <param name="objetyusermgt"></param>
        /// <param name="pcbaid"></param>
        public frmHardError(EntityUserManagement objetyusermgt, string pcbaid)
        {
            InitializeComponent();
            this.panelErrorType.Paint += (s, e) => PaintCustomBorderBox(s, e, true, true, true, true);
            this.panelRejectDetails.Paint += (s, e) => PaintCustomBorderBox(s, e, true, true, true, true);
            this.groupMeterType.Paint += (s, e) => PaintCustomBorderBox(s, e, true, true, true, true);
            this.groupStage.Paint += (s, e) => PaintCustomBorderBox(s, e, true, false, true, true);
            this.groupMeterDetails.Paint += (s, e) => PaintCustomBorderBox(s, e, true, true, true, true);
            this.groupStatus.Paint += (s, e) => PaintCustomBorderBox(s, e, true, true, true, true);
            //this.TopMost = true;
            loggeduser = objetyusermgt.LoginuserID;
            loggedusertype = objetyusermgt.LogType;

            txtPCBAID.Text = string.IsNullOrEmpty(pcbaid) ? string.Empty : pcbaid.Trim();

            rdbFT.Text = StaticVariables.TestType_FT;
            rdbCalibration.Text= StaticVariables.TestType_Cal;
            rdbSerialization.Text = StaticVariables.TestType_SR;
            rdbSealing.Text = "Sealing";
            //rdbFT.Checked = true;

            rdbMSNDLMS.Text = StaticVariables.MeterType_1PH_E150_MS;
            rdbMSDLMS.Text = StaticVariables.MeterType_1PH_E150_MSD;
            rdbSAPPHIRE.Text = StaticVariables.MeterType_3PH_Sapphire;
            rdb1PHSMART.Text = StaticVariables.MeterType_1PH_SM;
            rdb3PHSMART.Text = StaticVariables.MeterType_3PH_SM;
            //rdbMSDLMS.Checked = true;   
        }
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public frmHardError()
        {
            //this.TopMost = true;
            InitializeComponent();
        }
        /// <summary>
        /// Gets PCBAID through dlms communication on serial port.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button15_Click(object sender, EventArgs e)
        {
            objappSettings.SetMeterMode((int)LayerInterface.MeterTypeInfo.MicroStar_DLMS);
            if (objLI.ConnectToMeter()) //&& objIECLI.ConnectToIECMeter())
                    return;

            string getpcbaResponse = objccmdmethod.ReadPCBAID();
            if(!getpcbaResponse.Contains("Error"))
                txtPCBAID.Text = getpcbaResponse.Trim();
        }
        /// <summary>
        /// Action to take on click of button "UPDATE"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateItems())
            {
                if (MessageBox.Show(string.Format("Are you sure you want to insert this new Rejection Entry? \n\nPCBAID - {0}\n\nProduction Stage - {1}\n\nMeter Type - {2}\n\nError Type - {3}\n\nCustomer Name - {4}"
                                                                ,txtPCBAID.Text.Trim()
                                                                , groupStage.Controls.OfType<RadioButton>().First(s => s.Checked).Text
                                                                , groupMeterType.Controls.OfType<RadioButton>().First(s => s.Checked).Text
                                                                ,((DataRowView)listBoxErrorItems.SelectedItem)[ErrorTypesTable.colErrorname].ToString()
                                                                ,string.IsNullOrEmpty(txtCustomerID.Text.Trim()) ? "None" : txtCustomerID.Text), "Rejection Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                    == DialogResult.Yes)
                {

                    if (ReportError(listBoxErrorItems.SelectedIndex))
                        SetStatusText("Rejection Entry Uploaded Successfully!", Color.Green);
                    else
                        SetStatusText("Rejection Entry could not be Uploaded !", Color.Red);

                }
            }
        }
        /// <summary>
        /// Validates if relevant items in form have valid data.
        /// </summary>
        /// <returns></returns>
        private bool ValidateItems()
        {
            if (string.IsNullOrEmpty(txtPCBAID.Text.Trim())
                || txtPCBAID.Text.Trim().Any(s => !Char.IsLetterOrDigit(s))
                )
            {
                SetStatusText("Please Enter Correct PCBAID!", Color.Red);
                return false;
            }
            if (listBoxErrorItems.SelectedIndex < 0)
            {
                SetStatusText("Please Select Error Type!", Color.Red);
                return false;
            }
            if (groupMeterType.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked) == null)
            {
                SetStatusText("Please Select Meter Type!", Color.Red);
                return false;
            }
            if (groupStage.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked) == null)
            {
                SetStatusText("Please Select Stage!", Color.Red);
                return false;
            }

            return true;
        }
        /// <summary>
        /// Sets status text with given color.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="color"></param>
        private void SetStatusText(string status, Color color)
        {
            rtbStatus.Text = status;
            rtbStatus.ForeColor = color;
            rtbStatus.SelectAll();
            rtbStatus.SelectionAlignment = HorizontalAlignment.Center;
        }
        /// <summary>
        /// Inserts new rejection entry to Database using given error type.
        /// </summary>
        /// <param name="listIdx"></param>
        /// <returns>boolean</returns>
        private bool ReportError(int listIdx)
        {
            string errorID = "";

            var ds = listBoxErrorItems.DataSource as DataTable;
            errorID = ds.Rows[listIdx][ErrorTypesTable.colIdError].ToString();
            
            EntityError errentity = new EntityError();
            
            
            errentity.PCBAID = txtPCBAID.Text;
            errentity.Customer = string.IsNullOrEmpty(txtCustomerID.Text.Trim()) ? "None" : txtCustomerID.Text;
            
            errentity.ErrorID = errorID;
            errentity.ProcedureStage = groupStage.Controls.OfType<RadioButton>().First(s => s.Checked).Text;
            //errentity.ProcedureStage = stage;

            if (!string.IsNullOrEmpty(txtRejectAction.Text.Trim()))
                errentity.ErrorState = ErrorStateConstants.StateProcessed;
            else if (!string.IsNullOrEmpty(txtRejectCause.Text.Trim()) || !string.IsNullOrEmpty(txtProbDesc.Text.Trim()))
                errentity.ErrorState = ErrorStateConstants.StateAnalysis;
            else
                errentity.ErrorState = ErrorStateConstants.StateInitial;

            errentity.MeterType = groupMeterType.Controls.OfType<RadioButton>().First(s => s.Checked).Text;
            //errentity.MeterType = meterType;
            errentity.LoggedUserID = loggeduser;
            errentity.LastRepairID = loggeduser;
            errentity.ErrorDate = DateTime.Now; ;
            errentity.ActionDate = DateTime.Now;
            errentity.ParameterName = "";
            errentity.ProblemDescription = txtProbDesc.Text;
            errentity.RejectionCause = txtRejectCause.Text;
            errentity.ActionTaken = txtRejectAction.Text;
            errentity.MoreInformation = "";
            errentity.Customer = String.IsNullOrEmpty(txtCustomerID.Text.Trim()) ? "None" : txtCustomerID.Text;

            return balerrorobj.InsertToRejectionTable(errentity);
        }
        /// <summary>
        /// Sets Prduction stage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbFT_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdb = sender as RadioButton;
            stage = rdb.Text;

            
        }
        /// <summary>
        /// Sets Meter Type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbMSNDLMS_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdb = sender as RadioButton;
            meterType = rdb.Text;
        }
        /// <summary>
        /// Sets cursor to PCBAID textbox on form shown event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmHardError_Shown(object sender, EventArgs e)
        {
            this.txtPCBAID.Select();
        }
        /// <summary>
        /// Custom canvas draw logic for listbox items. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Rectangle Bounds = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height + 10);
            bool selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);
            int index = e.Index;
            Graphics g = e.Graphics;
            Color color;
            color = selected ? Color.DarkBlue : Color.White;
            /* Draw Background */
            g.FillRectangle(new SolidBrush(color), e.Bounds);

            Color borderColor = Color.DarkGray;
            g.DrawLine(new Pen(borderColor), e.Bounds.X, e.Bounds.Y, e.Bounds.X + e.Bounds.Width, e.Bounds.Y);

            /* Draw Item Text */
            g.DrawString(((DataRowView)listBoxErrorItems.Items[e.Index])[ErrorTypesTable.colErrorname].ToString(), e.Font, new SolidBrush(color = selected ? Color.White : Color.Black), e.Bounds, StringFormat.GenericTypographic);

            e.DrawFocusRectangle();
        }
        /// <summary>
        /// Sets default states on form load event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmHardError_Load(object sender, EventArgs e)
        {
            groupMeterType.Controls.OfType<RadioButton>().Select(r=>r.Checked=false);
            
            groupStage.Controls.OfType<RadioButton>().Select(r => r.Checked = false);

            var hwErrorSet = balerrorobj.GetHWErrorTypesTable();
            listBoxErrorItems.DataSource = hwErrorSet;
            listBoxErrorItems.DisplayMember = ErrorTypesTable.colErrorname;
            listBoxErrorItems.ValueMember = ErrorTypesTable.colErrorname;
        }
        /// <summary>
        /// Actions that occur on click of "NEW ERROR TYPE" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddNewErrorType_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewErrorType.Text.Trim())) { MessageBox.Show("Error Type Cannot be Blank"); return; };

            CabconPMPREJECTIONTOOL.AccessPassword frmobj = new CabconPMPREJECTIONTOOL.AccessPassword();
            EntityErrorType errorEntity = null;
            try
            {
                string password = balerrorobj.GetPasswordsForEntryModification(ErrorUtility.KeywordNewError);
                frmobj.ShowDialog();
                if (frmobj.Password.Equals(password))
                {
                    errorEntity = new EntityErrorType();
                    errorEntity.ErrorName = ErrorUtility.ConvertToPascalCase(txtNewErrorType.Text.Trim(), false).Replace("HW", "").Trim() + " (HW)";
                    errorEntity.ErrorType = "HW_" + ErrorUtility.ConvertToPascalCase(txtNewErrorType.Text.Trim(), true).Replace("HW", "") + "_FAIL";
                    errorEntity.ErrorDescription = errorEntity.ErrorName;
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
            

            if (MessageBox.Show("Are you sure you want to add this new Error Type? \n\n" + errorEntity.ErrorName, "HW failure ErrorType", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                == DialogResult.Yes)
            {
                if (balerrorobj.InsertToErrorTypesTable(errorEntity))
                {
                    var hwErrorSet = balerrorobj.GetHWErrorTypesTable();
                    listBoxErrorItems.DataSource = hwErrorSet;
                    listBoxErrorItems.DisplayMember = ErrorTypesTable.colErrorname;
                    listBoxErrorItems.ValueMember = ErrorTypesTable.colErrorname;
                    SetStatusText("Rejection Error Type Inserted Successfully", Color.SeaGreen);
                }
            }
        }


        private void PaintCustomBorderBox(object sender, PaintEventArgs e, bool bTop, bool bBottom, bool bRight, bool bLeft)
        {
            Control control = sender as Control;
            DrawControlBox(control, e.Graphics, Color.Black, Color.DarkGray, bTop, bBottom, bRight, bLeft);
        }


        private void DrawControlBox(Control box, Graphics g, Color textColor, Color borderColor, bool bTop, bool bBottom, bool bRight, bool bLeft)
        {
            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                               box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                               box.ClientRectangle.Width - 1,
                                               box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

                // Clear text and border
                g.Clear(this.BackColor);

                // Draw text
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                // Drawing Border
                //Left
                if (bLeft)
                    g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                //Right
                if (bRight)
                    g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Bottom
                if (bBottom)
                    g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Top1
                if (bTop)
                {
                    g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                    //Top2
                    g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
                }
            }
        }
    }
}
