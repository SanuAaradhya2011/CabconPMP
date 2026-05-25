using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CabconPMP
{
    public partial class AccessPassword : Form
    {
        private string _password ; 

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public AccessPassword()
        {
            InitializeComponent(); COMMONENTITY.FormStyleHelper.Apply(this);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Password = txtBoxPassword.Text;
            this.Close();
        }

        private void AccessPassword_Load(object sender, EventArgs e)
        {

        }
    }
}
