using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using COMMONENTITY;
namespace CabconPMP
{
    public partial class frmMeterExecutionState : Form
    {
        XMLExportImport objexpimp = new XMLExportImport();
        List<string> TestTypeList = new List<string>();
        List<bool> FinalStatusList = new List<bool>();
        public frmMeterExecutionState(List<string> GetTestTypeList, List<bool> GetFinalStatusList,string pcba)
        {
            TestTypeList = GetTestTypeList;
            FinalStatusList = GetFinalStatusList;
            InitializeComponent(); COMMONENTITY.FormStyleHelper.Apply(this);
            lblFinalmsg.Text = pcba;
        }

        private void frmMeterExecutionState_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            pbEMSTst.Image = CabconPMP.Properties.Resources.StateUnknown;
            pbFT.Image = CabconPMP.Properties.Resources.StateUnknown;
            pbCalibration.Image = CabconPMP.Properties.Resources.StateUnknown;
            pbSerialization.Image = CabconPMP.Properties.Resources.StateUnknown;
            int stateCount=0;
            PictureBox objpb = new PictureBox();
            bool status = false;
            int finalstate = 0;
            while (stateCount < TestTypeList.Count)
            {
               switch (TestTypeList[stateCount])
               {
                   case "EMS Test":
                       objpb = pbEMSTst;
                       status =  FinalStatusList[stateCount];
                       break;
                   case "Functional Test":
                       objpb = pbFT;
                       status = FinalStatusList[stateCount];
                       break;
                   case "Calibration Test":
                       objpb = pbCalibration;
                       status = FinalStatusList[stateCount];
                       break;
                   case "Serialization":
                       objpb = pbSerialization;
                       status = FinalStatusList[stateCount];
                       break;
               }
                if (status) objpb.Image = CabconPMP.Properties.Resources.StatePass;
                else { objpb.Image = CabconPMP.Properties.Resources.StateFail; finalstate++; }
                stateCount++;
             }
            if( pbEMSTst.Image == CabconPMP.Properties.Resources.StatePass &&
                pbFT.Image == CabconPMP.Properties.Resources.StatePass &&
                pbCalibration.Image == CabconPMP.Properties.Resources.StatePass &&
                pbSerialization.Image == CabconPMP.Properties.Resources.StatePass
               )
            {
                lblFinalmsg.Text = "Meter PCBA =" + lblFinalmsg.Text + " : is PASSED All the Production Stage";
            }
            else lblFinalmsg.Text = "Meter PCBA =" + lblFinalmsg.Text + " : is under Production";
            this.Cursor = Cursors.Default;
        }
    }
}
