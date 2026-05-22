/*
 * Work File      : frmMeterRange.cs
 * Date Started   : 15/Feb/2010
 * Date Completed : 15/Feb/2010
 * Project        : PMP Software
 * Copyright      : Landis + Gyr
 * Revision       : 1.0
 * Author         : BalGovind
 * Last Modified  :
 * Discription    : For Defining Meter Range
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using COMMONENTITY;
namespace LGPMP
{
    public partial class frmMeterRange : Form
    {
        //DataAccessDAL objChangePass = new DataAccessDAL();
        public frmMeterRange()
        {
            InitializeComponent();
        }
        /// <summary>
        ///  Button Close event handeler for closing the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Form Load execured
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMeterRange_Load(object sender, EventArgs e)
        {
            try
            {
                cmbMeterType.DataSource = StaticVariables.GetMeterType();
                if (cmbMeterType.SelectedIndex < 0 && cmbMeterType.Items.Count > 0) cmbMeterType.SelectedIndex = 0;     
                lblmsg.Visible = false;
            
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

            }
        }
        /// <summary>
        /// Button Upload file event handeler for read and save the file in database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                if(cmbMeterType.SelectedIndex < 0)
                {
                      MessageBox.Show("Please Select A Valid Meter Type !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                      return;
                }
                grpFileupload.Enabled = false;
                DataSet ds = new DataSet("XMLMeterID");
                btn_Close.Enabled = false;
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Multiselect = true;
                openFile.DefaultExt = ".xlsx"; // Default file extension
                openFile.Filter = "Excel documents (.xlsx)|*.xlsx|Excel 2003 documents (.xls)|*.xls"; // Filter files by       extension
                txtFileName.Text = "";
                this.Cursor = Cursors.WaitCursor;
                DialogResult result = openFile.ShowDialog();
                if (result == DialogResult.OK)
                {
                    int msgindex =Convert.ToInt16(MessageBox.Show("Do You Want To Upload Selected File ?", "L+G PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning));
                    if (msgindex != 6) return;
                    txtFileName.Text = openFile.FileName;
                    lblmsg.Visible = true;
                    System.Windows.Forms.Application.DoEvents();
                    System.Windows.Forms.Application.DoEvents();
                    System.Windows.Forms.Application.DoEvents(); 
                    ds = ReadExcelFile(txtFileName.Text);
                    ds.WriteXml(AppDomain.CurrentDomain.BaseDirectory + "\\Configuration\\" + StaticVariables.FilePrefixMeterIDList + cmbMeterType.Text.Trim() + ".xml");                     
                    MessageBox.Show("Meter ID List Saved !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Invalid File Selected or No Data To Save !" + "\n\n" + Ex.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

            }
            finally
            {
                lblmsg.Visible = false;
                grpFileupload.Enabled = true;
                this.Cursor = Cursors.Default;
                btn_Close.Enabled = true;
                RemoveExcellInstances();
            }
        }
        /// <summary>
        /// Function to read selected Excell file and creating a dataset
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        private DataSet ReadExcelFile(string FilePath)
        {
               string Path = FilePath;

                Excel.Application app = new Excel.Application();
                Excel.Workbook workBook = app.Workbooks.Open(Path, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Excel.Worksheet workSheet =  (Excel.Worksheet)workBook.ActiveSheet;
                DataSet dsExcelData = new DataSet("XMLMeterID");
                System.Data.DataTable dt1 = new System.Data.DataTable("MeterRangeMaster");
                DataRow dr = dt1.NewRow();
                int index = 1;
                object rowIndex = 1;
                object colIndex1 = 1;
                object colIndex2 = 2;
                int totalrecord = 0;
                dt1 = new System.Data.DataTable("MeterIDList");
               // dt1.Columns.Add("FileName");
                dt1.Columns.Add("MeterID");
                dr = dt1.NewRow();
                while (((Excel.Range)workSheet.Cells[rowIndex, colIndex1]).Value2 != null)
                {
                    totalrecord++;
                    string MeterID = ((Excel.Range)workSheet.Cells[rowIndex, colIndex1]).Value2.ToString();
                    if (MeterID != "")
                    {
                        dr = dt1.NewRow();
                        //dr[0] = txtFileName.Text;
                        dr[0] = MeterID;
                        dt1.Rows.Add(dr);
                    }
                    index++;
                    System.Windows.Forms.Application.DoEvents();
                    lblmsg.Text = "Uploading Records : " + index.ToString();
                    rowIndex = index;

                }
                dsExcelData.Tables.Add(dt1);         
                return dsExcelData;            
        }
        /// <summary>
        /// Removing Excel Instances after File Uploaded
        /// </summary>
        public void RemoveExcellInstances()
        {
            string currPrsName = "EXCEL";
            if (currPrsName == string.Empty) return;
            System.Diagnostics.Process[] prs = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process proces in prs)
            {
                if (proces.ProcessName == currPrsName)
                {
                    proces.Refresh();
                    if (!proces.HasExited)
                        proces.Kill();
                }
            }
        }

       
    }
}
