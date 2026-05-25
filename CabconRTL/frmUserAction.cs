using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace CabconRTL
{
    public partial class frmUserAction : Form
    {
        public string MeterType { get; set; }

        public frmUserAction()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           
            if (MessageBox.Show("Do You Want To Proceed with Selected Meter Type : " + cmbmetertype.Text, "Calibration", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
            MeterType = cmbmetertype.Text;
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void frmuseraction_Load(object sender, EventArgs e)
        {
            XMLExportImport objixml = new XMLExportImport();
            string connectionString = objixml.GetConnectionString();            
            if (connectionString.IndexOf("INDELNB") >= 0) lblServerLocation.Text = "Server Location : Local System";
            else if (connectionString.IndexOf("INDEL") >= 0) lblServerLocation.Text = "Server Location : GDC Noida";
            else if (connectionString.IndexOf("INBDI") >= 0) lblServerLocation.Text = "Server Location : Baddi Factory";
            else if (connectionString.IndexOf("INCCU") >= 0) lblServerLocation.Text = "Server Location : Joka Factory";
            else 
            {
                MessageBox.Show("Data Base Server Not Found ! \n Please Contact Your Administrator !", "Calibration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.DialogResult = DialogResult.No;
                this.Close();
            
            }
            Loadfromxml();
        }

        /// <summary>
        /// Method Name : Loadfromxml()
        /// Purpose     : Load the MeterTypeList.xml in combo box for meter type selection
        /// Date        : 23-01-15
        /// Author      : Mohsin Raza
        /// </summary>
        private void Loadfromxml()
        {
            List<string>metertyprlist =   StaticVariables.GetMeterType();

            if (metertyprlist.Count < 1)
            {
                MessageBox.Show("Meter Type information not available\nUnable to proceed", "Calibration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            cmbmetertype.Items.Clear();

            for (int icount = 0; icount < metertyprlist.Count; icount++)
            {
                cmbmetertype.Items.Add(metertyprlist[icount]);
            }

            cmbmetertype.SelectedIndex = 0;
         
        }
    }
}
