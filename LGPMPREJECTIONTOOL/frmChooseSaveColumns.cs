using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using COMMONENTITY;

namespace LGPMPREJECTIONTOOL
{
    /// <summary>
    /// Class for choosing columns to show in report.
    /// </summary>
    public partial class frmChooseSaveColumns : Form
    {
        public DataColumnCollection columnCollection;
        int maxcol = 11;
        int currentcols = 0;
        public bool bIsCancel=true;

        public frmChooseSaveColumns()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Instantiates class using given column collection.
        /// </summary>
        /// <param name="columnCollection"></param>
        public frmChooseSaveColumns(DataColumnCollection columnCollection)
        {
            this.columnCollection = columnCollection;
            InitializeComponent();
        }
        /// <summary>
        /// Sets initial column states.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmChooseSaveColumns_Shown(object sender, EventArgs e)
        {
            chkColumnsList.Items.Clear();
            currentcols = 0;
            foreach (DataColumn col in columnCollection)
            {
                //if (col.ColumnName.Contains(ErrorTypesTable.colErrorType)) { continue; }
                if (new string[] {
                                    RejectionTable.colPCBAID,
                                    //RejectionTable.colProductionStage, 
                                    ErrorTypesTable.colErrorType
                                  }.Any(s => col.ColumnName.Contains(s)))
                {
                    chkColumnsList.Items.Add(col.ColumnName, CheckState.Indeterminate);
                }
                else
                {
                    chkColumnsList.Items.Add(col.ColumnName);
                }
            }
        }
        /// <summary>
        /// Action to take when a column is checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkColumnsList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            currentcols = chkColumnsList.CheckedItems.Count;
            //skip mandatory columns
            if (e.CurrentValue == CheckState.Unchecked && e.NewValue == CheckState.Indeterminate) { currentcols++; }  
            if (e.CurrentValue == CheckState.Indeterminate) { e.NewValue = CheckState.Indeterminate; }
            else if (e.NewValue == CheckState.Unchecked) { --currentcols; }
            else if (e.NewValue == CheckState.Checked)
            {
                if (currentcols + 1 > maxcol) 
                { 
                    MessageBox.Show("Selection cannot exceed " + maxcol.ToString() + " columns", "L+G PMP",MessageBoxButtons.OK, MessageBoxIcon.Information); 
                    e.NewValue = CheckState.Unchecked; 
                }
                else currentcols++;
            }

            lblColumnsCount.Text = String.Format("{0} columns selected, {1} remaining", currentcols, maxcol-currentcols);
        }
        /// <summary>
        /// Sets final state of column collection and closes this instance.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            bIsCancel = false;
            foreach (object item in chkColumnsList.Items)
            {
                if (!chkColumnsList.CheckedItems.Contains(item))
                {
                    columnCollection.Remove(item.ToString());
                }
            }
            this.Close();
        }
        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
