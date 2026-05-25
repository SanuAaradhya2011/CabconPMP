/*
 * Work File      : frmMeterRange.cs
 * Date Started   : 25/05/2026
 * Date Completed : 25/05/2026
 * Project        : PMP Software
 * Copyright      : Cabcon Technologies
 * Revision       : 1.0
 * Author         : Piyush Singh
 * Last Modified  :
 * Discription    : For Defining Meter Range
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using COMMONENTITY;
namespace CabconPMP
{
    public partial class frmImportRTRFormat : Form
    {
        //DataAccessDAL objChangePass = new DataAccessDAL();
        public frmImportRTRFormat()
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
                    if (MessageBox.Show("Do You Want To Import Routine Test Report Refrence File Format For Meter Type : "  + cmbMeterType.Text , "L+G PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                     
                    string ipPath=AppDomain.CurrentDomain.BaseDirectory + "\\Configuration\\" + StaticVariables.FilePrefixRoutineTestReportFileFormat + cmbMeterType.Text.Trim() + ".xls";
                    openFileDialog1.Filter = "Routine Test Refrence File (*.xls)|*.xls|(*.xlsx)|*.xlsx";
                    openFileDialog1.Title = "Select Routine Test Refrence File";
                    openFileDialog1.FileName = "";
                    if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
                    txtFileName.Text = openFileDialog1.FileName;
                    File.Delete(ipPath);
                    File.Copy(openFileDialog1.FileName, ipPath);
                    MessageBox.Show("Routine Test Report Refrence File Format Imported !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
               }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to Select Routine Test Refrence File !" + "\n" + ex.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
               
               Excel.Worksheet workSheet = (Excel.Worksheet)workBook.ActiveSheet;
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
