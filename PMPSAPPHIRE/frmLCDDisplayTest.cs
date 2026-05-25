using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using COMMONENTITY;
namespace PMPSAPPHIRE
{
    public partial class frmLCDDisplayTest : Form
    {
        GlobalMethods objtemp;

        public frmLCDDisplayTest(GlobalMethods StaticVariablesvar)
        {
            InitializeComponent(); COMMONENTITY.FormStyleHelper.Apply(this);
            rdPhaseOK.Checked = true;
            rdEvenOK.Checked = true;
            rdOddOK.Checked = true;
            rdLedActiveOK.Checked = true;
            rdLedReActiveOK.Checked = true;
            objtemp = StaticVariablesvar;  
            btnOK.Focus();
        }

        private void rdPhaseOK_CheckedChanged(object sender, EventArgs e)
        {
            if (rdPhaseOK.Checked) rdPhaseOK.BackColor = Color.Green;
            else { rdPhaseOK.BackColor = lblphasetest.BackColor; }
        }

        private void rdRelayOK_CheckedChanged(object sender, EventArgs e)
        {
            if (rdEvenOK.Checked) rdEvenOK.BackColor = Color.Green;
            else rdEvenOK.BackColor = lblphasetest.BackColor;
        }

        private void rdActiveOK_CheckedChanged(object sender, EventArgs e)
        {
            if (rdOddOK.Checked) rdOddOK.BackColor = Color.Green;
            else rdOddOK.BackColor = lblphasetest.BackColor;
        }

        private void rdPhaseFail_CheckedChanged(object sender, EventArgs e)
        {
            if (rdPhaseFail.Checked) rdPhaseFail.BackColor = Color.Red;
            else rdPhaseFail.BackColor = lblphasetest.BackColor;
        }

        private void rdRelayFail_CheckedChanged(object sender, EventArgs e)
        {
            if (rdEvenFail.Checked) rdEvenFail.BackColor = Color.Red;
            else rdEvenFail.BackColor = lblphasetest.BackColor;
        }

        private void rdActiveFail_CheckedChanged(object sender, EventArgs e)
        {
            if (rdOddFail.Checked) rdOddFail.BackColor = Color.Red;
            else rdOddFail.BackColor = lblphasetest.BackColor;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                string pasvalue = "";
                if (!rdPhaseOK.Checked) pasvalue = "All Segment LCD =" + StaticVariables.DisplayNotWorking;
                if (!rdEvenOK.Checked) pasvalue += ",  Even Segment LCD =" + StaticVariables.DisplayNotWorking;
                if (!rdOddOK.Checked) pasvalue += ",  odd Segment LCD =" + StaticVariables.DisplayNotWorking;
                if (!rdLedActiveOK.Checked) pasvalue += ",  Active LED =" + StaticVariables.DisplayNotWorking;
                if (!rdLedReActiveOK.Checked) pasvalue += ",  ReActive LED =" + StaticVariables.DisplayNotWorking;
                if (pasvalue.Length > 0) objtemp.LCDSegmentTestResponse = StaticVariables.ERRORPreFix + pasvalue;
                else objtemp.LCDSegmentTestResponse = pasvalue;
                this.Close();
            }
            catch (Exception Ex)
            {
                objtemp.LCDSegmentTestResponse = StaticVariables.ERRORPreFix + Ex.Message;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            objtemp.LCDSegmentTestResponse = StaticVariables.ERRORPreFix + "Window Closed Manualy";
            this.Close();
        }

        private void rdLedActiveOK_CheckedChanged(object sender, EventArgs e)
        {
            if (rdLedActiveOK.Checked) rdLedActiveOK.BackColor = Color.Green;
            else rdLedActiveOK.BackColor = lblphasetest.BackColor;
        }

        private void rdLedActiveFail_CheckedChanged(object sender, EventArgs e)
        {
            if (rdLedActiveFail.Checked) rdLedActiveFail.BackColor = Color.Red;
            else rdLedActiveFail.BackColor = lblphasetest.BackColor;
        }

        private void rdLedReActiveOK_CheckedChanged(object sender, EventArgs e)
        {
            if (rdLedReActiveOK.Checked) rdLedReActiveOK.BackColor = Color.Green;
            else rdLedReActiveOK.BackColor = lblphasetest.BackColor;
        }

        private void rdLedReActiveFail_CheckedChanged(object sender, EventArgs e)
        {
            if (rdLedReActiveFail.Checked) rdLedReActiveFail.BackColor = Color.Red;
            else rdLedReActiveFail.BackColor = lblphasetest.BackColor;
        }
    }
}
