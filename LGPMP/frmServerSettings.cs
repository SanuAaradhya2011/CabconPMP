using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using COMMONENTITY;
using Utilities;
using SystemSecurityLibrary;
namespace LGPMP
{
    public partial class frmServerSettings : Form
    {
        XMLExportImport obgxml = new XMLExportImport();
        AESEncryption objaes = new AESEncryption();
        public frmServerSettings(bool isopenModeisAdmin)
        {
            InitializeComponent();
            txtTimeout.Enabled = isopenModeisAdmin;
        }

        private void frmServerSettings_Load(object sender, EventArgs e)
        {
            string connectionString = obgxml.GetConnectionString();
            if (connectionString.IndexOf("INDELNB") >= 0) cmbServerLoc.SelectedIndex = 0;
            else if (connectionString.IndexOf("INDEL") >= 0) cmbServerLoc.SelectedIndex = 1;
           // else if (connectionString.IndexOf("INBDI") >= 0) cmbServerLoc.SelectedIndex = 2;
            else if (connectionString.IndexOf("INCCU") >= 0) cmbServerLoc.SelectedIndex = 2;
            else cmbServerLoc.SelectedIndex = 0;
        }

        private bool SetConnection()
        {
            try
            {
                string dbServer = "INDELVS01";
                if (cmbServerLoc.SelectedIndex == 1) dbServer = "INDELVS01";
               // else if (cmbServerLoc.SelectedIndex == 2) dbServer = "INBDISV06";//"INBDISV04\\FRSPMP";//"INBDISV06";//--Factory Closed Server removed
                else if (cmbServerLoc.SelectedIndex == 2) dbServer = "INCCUSV05";//"INCCUSV03";
                else if (cmbServerLoc.SelectedIndex == 3) dbServer = txtServerName.Text.Trim();//Bangladesh Production Server
                else dbServer = "INDELNB688";
                DataTable dt = new DataTable("SQLCONN");
                dt.Columns.Add("ConnectionClient", typeof(string));
                dt.Columns.Add("ConnectionString", typeof(string));
                DataRow dr = dt.NewRow();
                dr[0] = "SQL Client";
                string connString = "Server=" + dbServer + ";Database=LGPMPDB;Integrated Security=fals;User id=lgpmp;Password=pass@1234;Trusted_Connection=False; Connection Timeout=" + txtTimeout.Text.Trim() + ";";
                dr[1] = objaes.GenerateCipherText(connString, StaticVariables.PublicKeyEncryption);
                dt.Rows.Add(dr);
                obgxml.ExportXMLFromDatatable(dt, StaticVariables.DataBaseConnectionFile);
                return true;
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Unable To Save Connection Settings !" + "\n\n" + Ex.ToString(), "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int tempTimeOut=0;
            if (!int.TryParse(txtTimeout.Text.Trim(), out tempTimeOut)) 
            {
                MessageBox.Show("Please Provide Valid Connection TimeOut " + "\n Default Value Could be 10 Sec.", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtTimeout.Focus();
                return;
                 
            }
            if (cmbServerLoc.SelectedIndex == cmbServerLoc.Items.Count - 1)
            {
                if (txtServerName.Text.Trim().Length <= 0)
                {
                    MessageBox.Show("Please Provide Valid Data Base Server Name !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            if (SetConnection())
            {
                MessageBox.Show("Server Settings Saved !", "L+G PMP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbServerLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbServerLoc.SelectedIndex == cmbServerLoc.Items.Count - 1) { lblServerName.Visible = true; txtServerName.Visible = true; txtServerName.Text = ""; }
            else {txtServerName.Visible = false; lblServerName.Visible = false;}
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
