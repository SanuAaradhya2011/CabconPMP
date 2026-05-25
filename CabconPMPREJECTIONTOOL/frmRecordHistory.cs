using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BALLAYER;
using COMMONENTITY;
using Utilities;

namespace CabconPMPREJECTIONTOOL
{
    /// <summary>
    /// Class that shows record history of pcbaid.
    /// </summary>
    public partial class frmRecordHistory : Form
    {
        private DataTable historyTable, tableextinfo;
        private BALErrorOperations balerrorobj = new BALErrorOperations();
        private EntityUserManagement objetyusermgt;
        private string PCBAID, productionStage;
        /// <summary>
        /// Default constructor.
        /// </summary>
        public frmRecordHistory()
        {
            InitializeComponent(); COMMONENTITY.FormStyleHelper.Apply(this);
        }
        /// <summary>
        /// Creates an instance of this class using pcbaid, production stage and login details.
        /// </summary>
        /// <param name="PCBAID"></param>
        /// <param name="productionStage"></param>
        /// <param name="objusermgmt"></param>
        public frmRecordHistory(string PCBAID, string productionStage, EntityUserManagement objusermgmt)
        {
            this.objetyusermgt = objusermgmt;
            this.PCBAID = PCBAID;
            this.productionStage = productionStage;
            InitializeComponent(); COMMONENTITY.FormStyleHelper.Apply(this);
            this.dgvRecordHistory.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvRecordHistory_RowPostPaint);
       
        }

        /// <summary>
        /// Put Entry No. at each row of datagrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRecordHistory_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dgvRecordHistory.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// Action to take on double click event of cell.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRecordHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OpenHistoryItem();
        }

        /// <summary>
        /// Open selected History entry in Update/Insert Form.
        /// </summary>
        private void OpenHistoryItem()
        {
            if (dgvRecordHistory.DataSource != null && dgvRecordHistory.SelectedRows.Count == 1)
            {
                int idx = dgvRecordHistory.CurrentCell.RowIndex;
                //int idx = dgvRecordHistory.Rows.Count-1;
                long idxreject = long.Parse(dgvRecordHistory.Rows[idx].Cells[RejectionTable.colIDReject].Value.ToString());
                tableextinfo = balerrorobj.GetExtendedInformation(idxreject);
                if (tableextinfo == null) { MessageBox.Show("Unable to Load Data!", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

                StartUpdateEntryForm(idx);
            }
        }
        /// <summary>
        /// Action to take on form shown event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRecordHistory_Shown(object sender, EventArgs e)
        {
            LoadHistoryDataToGrid();
        }
        /// <summary>
        /// Loads History data to grid for pcbaid/pcbaid-productionStage.
        /// </summary>
        public void LoadHistoryDataToGrid()
        {
            if (chkPCBAID.Checked)
                historyTable = balerrorobj.GetHistoryTable_PCBID(PCBAID);
            else
                historyTable = balerrorobj.GetHistoryTable_PCBID_Stage(PCBAID, productionStage);

            historyTable.TableName = "HistoryTable";
            if (historyTable == null) { MessageBox.Show("Unable to Load Data!", "Cabcon PMP", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            dgvRecordHistory.DataSource = historyTable;
            dgvRecordHistory.Columns[RejectionTable.colIDReject].Visible = false;
            dgvRecordHistory.Columns[ErrorTypesTable.colErrorType].Visible = false;

            dgvRecordHistory.Columns[ErrorTypesTable.colErrorname].Width = 200;
            dgvRecordHistory.Columns[RejectionTable.colMeterType].Width = 150;
            dgvRecordHistory.Columns[RejectionTable.colCustomer].Width = 200;

            dgvRecordHistory.Columns[RejectionTable.colErrorDate].DefaultCellStyle.Format = "dd/MM/yyyy hh:mm tt";
            dgvRecordHistory.Columns[RejectionTable.colActionDate].DefaultCellStyle.Format = "dd/MM/yyyy hh:mm tt";

            dgvRecordHistory.Rows[0].Selected = true;
            if (historyTable.Rows.Count > 0)
            {
                dgvRecordHistory.CurrentCell = dgvRecordHistory[1, 0];
            }
        }
        /// <summary>
        /// Creates instance of frmUpdateEntry class.
        /// </summary>
        /// <param name="rowidx"></param>
        public void StartUpdateEntryForm(int rowidx)
        {
            if (dgvRecordHistory.DataSource != null && dgvRecordHistory.SelectedRows.Count == 1)
            {
                DataRow dRow = ((DataRowView)dgvRecordHistory.Rows[rowidx].DataBoundItem).Row;
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
        /// Refreshes data in datagrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadHistoryDataToGrid();
        }
        /// <summary>
        /// Closes form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Loads history for pcbaid if checkbox is checked, else loads history for pcbaid-productionStage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPCBAID_CheckedChanged(object sender, EventArgs e)
        {
             LoadHistoryDataToGrid();
        }
        /// <summary>
        /// Opens an item for Update.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenHistoryItem();
        }

        /// <summary>
        /// Save as Excel sheet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataSet EXCELLDS = new DataSet();
            DataTable dt = dgvRecordHistory.DataSource as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.TableName.Equals("HistoryTable"))
                {
                    dt.Columns.Remove(RejectionTable.colIDReject);
                    dt.Columns.Remove(RejectionTable.colIDError);
                }
                EXCELLDS.Tables.Add(dt);
                ServiceClass.ServiceInstance.ExportDataSetToExcell(EXCELLDS);
            }
        }
        /// <summary>
        /// Opens new instance of  frmHardError class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newHWErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmHardError frmobj = new frmHardError(objetyusermgt, PCBAID);
            frmobj.Show();
        }
    }
}
