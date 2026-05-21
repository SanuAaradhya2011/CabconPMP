using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace COMMONENTITY
{
  
    public partial class inputBox : Form
    {
        public inputBox(string textvalue)
        {
           InitializeComponent();
           this.Text = textvalue;
           Application.DoEvents();
        }

        private void bthOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void inputBox_Load(object sender, EventArgs e)
        {
            txtInputBox.Focus();
        }

        private void txtInputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter || e.KeyChar == (char)Keys.Space) bthOK_Click(this, e);
        }

        
    }
}
