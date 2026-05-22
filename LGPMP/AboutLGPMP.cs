using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utilities;
using SystemSecurityLibrary;
namespace CabconPMP
{
    public partial class AboutLGPMP : Form
    {
        public AboutLGPMP()
        {
            InitializeComponent();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void AboutLGPMP_Load(object sender, EventArgs e)
        {
            MyCrypro oblcrypto = new MyCrypro();
            lblProductVersion.Text = SystemInfo.ProductVersion();
            lblproductname.Text = Application.ProductName.ToString() + " (Product Management Program)";
            label1.Text = SystemInfo.CopyRightsDetail() + "  " + "All Rights Reserved.";
            lblliciencedto.Text = "This Product is Licensed To : " +  System.Environment.MachineName.ToString(); 
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
