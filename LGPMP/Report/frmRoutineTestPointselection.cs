using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CabconPMP.Report
{
    public partial class frmRoutineTestPointselection : Form
    {
        private List<string> mparamdetails;
        public List<string> mparamlist { get; set; }
        public List<string> mparamindexlist { get; set; }

        public frmRoutineTestPointselection(List<string> _param)
        {
            InitializeComponent();
            mparamdetails = _param;
           
        }

        private void frmreportcolselection_Load(object sender, EventArgs e)
        {   
           Ppopulatelist();
        }

        private void Ppopulatelist()
        {
            var _SelectedtestPoints = Properties.Settings.Default.selectedparam;           
            chklist.Items.Clear();
            int itemCnt = 0;          
            foreach (string testPoint in mparamdetails)
            {
       
                chklist.Items.Add(testPoint);
                if (_SelectedtestPoints != null)
                {
                    if (_SelectedtestPoints.IndexOf(testPoint) >= 0) chklist.SetItemChecked(itemCnt, true);
                    
                }
                Application.DoEvents();
                itemCnt++;
            }

            if (chklist.Items.Count > 2) chklist.SelectedIndex = chklist.Items.Count/2;
        }

        private void btnselect_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.selectedparam = new System.Collections.Specialized.StringCollection();
           mparamindexlist = new List<string>();

            
            foreach (string iteminList in chklist.CheckedItems)
            {
                Properties.Settings.Default.selectedparam.Add(iteminList);
                mparamindexlist.Add(iteminList);
            }            
            if (chklist.CheckedItems.Count <= 0)
            {
                if (MessageBox.Show("No Routine Test Point Selected !" + "\n" + "Do You Want To Select ??", "L+G PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes) return;
                else { this.DialogResult = DialogResult.Cancel; this.Close(); }
            }
            else
            {
                if (MessageBox.Show("Total Selected Routine Test Point is : " + chklist.CheckedItems.Count.ToString() + "\n" + "Do You Want To Continue  ??", "L+G PMP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                this.DialogResult = DialogResult.OK;
            }
         
            
         }

        private void btnclose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public void MoveListItem(int direction)
        {
            // Checking selected item
            if (chklist.SelectedItem == null || chklist.SelectedIndex < 0)
                return; // No selected item - nothing to do

            // Calculate new index using move direction
            int newIndex = chklist.SelectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= chklist.Items.Count)
                return; // Index out of range - nothing to do

            object selected = chklist.SelectedItem;

            // Removing removable element
            chklist.Items.Remove(selected);
            // Insert it in new position
            chklist.Items.Insert(newIndex, selected);
            // Restore selection
            chklist.SetSelected(newIndex, true);
        }

        private void btnUpScroll_Click(object sender, EventArgs e)
        {
            MoveListItem(-1);
        }

        private void btnDownScroll_Click(object sender, EventArgs e)
        {
            MoveListItem(1);
        }

        private void chklist_SelectedValueChanged(object sender, EventArgs e)
        {
            lblSelectedCount.Text = "Total Selected Points : " + chklist.CheckedItems.Count.ToString();
        }

        private void chklist_DoubleClick(object sender, EventArgs e)
        {
            lblSelectedCount.Text = "Total Selected Points : " + chklist.CheckedItems.Count.ToString();
        }
    }
}
