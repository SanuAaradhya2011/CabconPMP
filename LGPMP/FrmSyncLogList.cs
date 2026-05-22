using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using COMMONENTITY;

namespace CabconPMP
{
    public partial class FrmSyncLogList : Form
    {
        System.IO.FileInfo[] filelist;
        public FrmSyncLogList()
        {
            ExcecutionResultImportExport objexeimprt = new ExcecutionResultImportExport();
            filelist = objexeimprt.GetSyncLogList();
            if (filelist.Count() == 0)
            {
                MessageBox.Show("No Logs present!", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            InitializeComponent();
        }

        private void FrmSyncLogList_Load(object sender, EventArgs e)
        {
            foreach (System.IO.FileInfo fi in filelist)
            {
                listBoxLog.Items.Add(fi);
            }
            listBoxLog.SelectedIndex = 0;
        }

        private void btnShowLog_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(filelist.ElementAt(listBoxLog.SelectedIndex).FullName);
        }

        private void listBoxLog_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Process.Start(filelist.ElementAt(listBoxLog.SelectedIndex).FullName);
        }

        


    }
}
