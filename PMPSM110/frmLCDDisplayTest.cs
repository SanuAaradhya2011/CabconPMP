using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using COMMONENTITY;
namespace PMPSM110
{
    public partial class frmLCDDisplayTest : Form
    {
        GlobalMethods objtemp;

        public frmLCDDisplayTest(GlobalMethods StaticVariablesvar, string mesgdefault, string mesgmin, string mesgmax)
        {
            InitializeComponent();
            rdPhaseOK.Checked = true;
            rdRelayOK.Checked = true;
            rdActiveOK.Checked = true;
            objtemp = StaticVariablesvar;
            lblPushButton.Text = "";
            string mesg = mesgdefault;
            if (mesgdefault != "") mesg = mesgdefault;
            else if (mesgmin != "" && mesgmax != "") mesg = ">= " + mesgmin + " And <= " + mesgmax;
            else if (mesgmin != "") mesg = ">= " + mesgmin;
            else if (mesgmax != "") mesg = "<= " + mesgmax;            
            lblPushButton.Text = "Press Push Button =" +  mesg  + " Time And Verify Display Segment !";
            btnOK.Focus();
        }

        private void rdPhaseOK_CheckedChanged(object sender, EventArgs e)
        {
            if (rdPhaseOK.Checked) rdPhaseOK.BackColor = Color.Green;
            else { rdPhaseOK.BackColor = lblphasetest.BackColor; }
        }

        private void rdRelayOK_CheckedChanged(object sender, EventArgs e)
        {
            if (rdRelayOK.Checked) rdRelayOK.BackColor = Color.Green;
            else rdRelayOK.BackColor = lblphasetest.BackColor;
        }

        private void rdActiveOK_CheckedChanged(object sender, EventArgs e)
        {
            if (rdActiveOK.Checked) rdActiveOK.BackColor = Color.Green;
            else rdActiveOK.BackColor = lblphasetest.BackColor;
        }

        private void rdPhaseFail_CheckedChanged(object sender, EventArgs e)
        {
            if (rdPhaseFail.Checked) rdPhaseFail.BackColor = Color.Red;
            else rdPhaseFail.BackColor = lblphasetest.BackColor;
        }

        private void rdRelayFail_CheckedChanged(object sender, EventArgs e)
        {
            if (rdRelayFail.Checked) rdRelayFail.BackColor = Color.Red;
            else rdRelayFail.BackColor = lblphasetest.BackColor;
        }

        private void rdActiveFail_CheckedChanged(object sender, EventArgs e)
        {
            if (rdActiveFail.Checked) rdActiveFail.BackColor = Color.Red;
            else rdActiveFail.BackColor = lblphasetest.BackColor;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
             
            string pasvalue = string.Empty;
            if (rdPhaseOK.Checked) pasvalue = "Push Button Icon =" + StaticVariables.DisplayWorking;
            else pasvalue = "Push Button Icon =" + StaticVariables.DisplayNotWorking;
            if (rdRelayOK.Checked) pasvalue += ",  Relay ON/OFF =" + StaticVariables.DisplayWorking;
            else pasvalue += ",  Relay ON/OFF =" + StaticVariables.DisplayNotWorking;
            if (rdActiveOK.Checked) pasvalue += ",  All Segment LCD =" + StaticVariables.DisplayWorking;
            else pasvalue += ",  All Segment LCD =" + StaticVariables.DisplayNotWorking;

            objtemp.LCDSegmentTestResponse = pasvalue;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
