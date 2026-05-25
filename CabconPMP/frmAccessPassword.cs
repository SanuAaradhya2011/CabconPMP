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

            // Modern lock icon
            picLock.Image = SystemIcons.Shield.ToBitmap();

            // Rounded buttons
            btnOK.FlatAppearance.MouseOverBackColor = Color.FromArgb(29, 78, 216);
            btnCancel.FlatAppearance.MouseOverBackColor = Color.Gainsboro;
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
