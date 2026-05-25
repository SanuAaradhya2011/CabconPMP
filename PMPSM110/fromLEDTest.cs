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
    public partial class fromLEDTest : Form
    {
        GlobalMethods objtemp;
        public fromLEDTest(GlobalMethods StaticVariablesvar)
        {
            InitializeComponent(); COMMONENTITY.FormStyleHelper.Apply(this);
            rdPhaseOK.Checked = true;
            rdRelayOK.Checked = true;
            rdActiveOK.Checked = true;
            objtemp = StaticVariablesvar;
            btnOK.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdPhaseOK_CheckedChanged(object sender, EventArgs e)
        {
            if (rdPhaseOK.Checked) rdPhaseOK.BackColor = Color.Green;
            else { rdPhaseOK.BackColor = lblphasetest.BackColor;  }
        }

        private void rdRelayOK_CheckedChanged(object sender, EventArgs e)
        {
            if (rdRelayOK.Checked) rdRelayOK.BackColor = Color.Green;
            else rdRelayOK.BackColor = lblphasetest.BackColor;
        }

        private void rdActiveOK_CheckedChanged(object sender, EventArgs e)
        {
            if (rdActiveOK.Checked) rdActiveOK.BackColor = Color.Green;
            else  rdActiveOK.BackColor = lblphasetest.BackColor;
        }

        private void rdPhaseFail_CheckedChanged(object sender, EventArgs e)
        {
            if (rdPhaseFail.Checked) rdPhaseFail.BackColor = Color.Red;
            else   rdPhaseFail.BackColor = lblphasetest.BackColor;
        }

        private void rdRelayFail_CheckedChanged(object sender, EventArgs e)
        {
            if (rdRelayFail.Checked) rdRelayFail.BackColor = Color.Red;
            else  rdRelayFail.BackColor = lblphasetest.BackColor;
        }

        private void rdActiveFail_CheckedChanged(object sender, EventArgs e)
        {
            if (rdActiveFail.Checked) rdActiveFail.BackColor = Color.Red;
            else   rdActiveFail.BackColor = lblphasetest.BackColor;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
          
            string pasvalue = string.Empty;
            if (rdPhaseOK.Checked ) pasvalue = "Phase LED =" + StaticVariables.DisplayWorking;
            else pasvalue = "Phase LED =" + StaticVariables.DisplayNotWorking;
            if (rdRelayOK.Checked) pasvalue += ",  Relay LED =" + StaticVariables.DisplayWorking;
            else pasvalue += ",  Relay LED =" + StaticVariables.DisplayNotWorking;
            if (rdActiveOK.Checked) pasvalue += ",  Active LED =" + StaticVariables.DisplayWorking;
            else pasvalue += ",  Active LED =" + StaticVariables.DisplayNotWorking;

            objtemp.LEDTestResponse = pasvalue;
            this.Close();
        }
    }
}
