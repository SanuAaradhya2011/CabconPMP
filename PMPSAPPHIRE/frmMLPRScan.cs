using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PMPSAPPHIRE
{
    public partial class frmMLPRScan : Form
    {
        public frmMLPRScan()
        {
            InitializeComponent();
            strLPR_ID = "";
        }
        public string strLPR_ID;

        private void textBox_LPR_TextChanged(object sender, EventArgs e)
        {
            if (textBox_LPR.Text.Length == 8)
            {
                strLPR_ID = textBox_LPR.Text.Trim().ToUpperInvariant().PadRight(8,'0');
                this.Close();
            }
        }
    }
}
