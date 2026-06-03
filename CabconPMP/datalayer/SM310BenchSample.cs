using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartCalibration.DataLayer
{
    public class SM310BenchSample : IBenchSampleHandler
    {
        public bool FillSamplefmfile()
        {
            if (!File.Exists(Constants.GlobalConstants.AEMCALSAMPLEFILE))
            {
                MessageBox.Show(Constants.GlobalConstants.AEMCALSAMPLEFILE + " not found", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            string[] samples = File.ReadAllLines(Constants.GlobalConstants.AEMCALSAMPLEFILE);
            const byte MAXPARAM = 17;
            int samplecount = 0;
            int iavgcount = 0;
            BenchData.rvoltage = 0;
            BenchData.yvoltage = 0;
            BenchData.bvoltage = 0;

            BenchData.rcurrent = 0;
            BenchData.ycurrent = 0;
            BenchData.bcurrent = 0;

            BenchData.rpowerfactor = 0;
            BenchData.ypowerfactor = 0;
            BenchData.bpowerfactor = 0;

            BenchData.ractivepower = 0;
            BenchData.yactivepower = 0;
            BenchData.bactivepower = 0;

            BenchData.rreactivepower = 0;
            BenchData.yreactivepower = 0;
            BenchData.breactivepower = 0;

            if (samples == null || samples.Count() < 1)
                return false;

            
            try
            {
                for (samplecount = 0; samplecount < samples.Count(); samplecount++)
                {
                    string[] paramarr = samples[samplecount].Split(',');

                    if (paramarr == null || paramarr.Count() < MAXPARAM)
                        continue;

                    double drvoltage = 0;
                    double dyvoltage = 0;
                    double dbvoltage = 0;

                    double drcurrent = 0;
                    double dycurrent = 0;
                    double dbcurrent = 0;

                    double drangle = 0;
                    double dyangle = 0;
                    double dbangle = 0;

                    double dractpower = 0;
                    double dyactpower = 0;
                    double dbactpower = 0;

                    double drreactpower = 0;
                    double dyreactpower = 0;
                    double dbreactpower = 0;


                    double.TryParse(paramarr[(int)BenchData.SAMPLEINDEX.VR], out drvoltage);
                    double.TryParse(paramarr[(int)BenchData.SAMPLEINDEX.VY], out dyvoltage);
                    double.TryParse(paramarr[(int)BenchData.SAMPLEINDEX.VB], out dbvoltage);

                    double.TryParse(paramarr[(int)BenchData.SAMPLEINDEX.IR], out drcurrent);
                    double.TryParse(paramarr[(int)BenchData.SAMPLEINDEX.IY], out dycurrent);
                    double.TryParse(paramarr[(int)BenchData.SAMPLEINDEX.IB], out dbcurrent);

                    double.TryParse(paramarr[(int)BenchData.SAMPLEINDEX.RANGLE], out drangle);
                    double.TryParse(paramarr[(int)BenchData.SAMPLEINDEX.YANGLE], out dyangle);
                    double.TryParse(paramarr[(int)BenchData.SAMPLEINDEX.BANGLE], out dbangle);

                    double.TryParse(paramarr[(int)BenchData.SAMPLEINDEX.RACTIVEPOWER], out dractpower);
                    double.TryParse(paramarr[(int)BenchData.SAMPLEINDEX.YACTIVEPOWER], out dyactpower);
                    double.TryParse(paramarr[(int)BenchData.SAMPLEINDEX.BACTIVEPOWER], out dbactpower);

                    double.TryParse(paramarr[(int)BenchData.SAMPLEINDEX.RREACTPOWER], out drreactpower);
                    double.TryParse(paramarr[(int)BenchData.SAMPLEINDEX.YREACTPOWER], out dyreactpower);
                    double.TryParse(paramarr[(int)BenchData.SAMPLEINDEX.BREACTPOWER], out dbreactpower);

                    if (drvoltage < 200) continue;
                    if (dyvoltage < 200) continue;
                    if (dbvoltage < 200) continue;

                    BenchData.rvoltage += (UInt32)(drvoltage * 100.0);
                    BenchData.yvoltage += (UInt32)(dyvoltage * 100.0);
                    BenchData.bvoltage += (UInt32)(dbvoltage * 100.0);

                    BenchData.rcurrent += (UInt32)(drcurrent * 1000.0);
                    BenchData.ycurrent += (UInt32)(dycurrent * 1000.0);
                    BenchData.bcurrent += (UInt32)(dbcurrent * 1000.0);

                    BenchData.rpowerfactor = (UInt32)(drangle);
                    BenchData.ypowerfactor = (UInt32)(dyangle);
                    BenchData.bpowerfactor = (UInt32)(dbangle);

                    BenchData.ractivepower += (UInt32)(dractpower * 100.0);
                    BenchData.yactivepower += (UInt32)(dyactpower * 100.0);
                    BenchData.bactivepower += (UInt32)(dbactpower * 100.0);

                    BenchData.rreactivepower += (UInt32)(drreactpower * 100.0);
                    BenchData.yreactivepower += (UInt32)(dyreactpower * 100.0);
                    BenchData.breactivepower += (UInt32)(dbreactpower * 100.0);
                    iavgcount++;
                }

                samplecount = iavgcount;
                BenchData.rvoltage = (UInt32)(BenchData.rvoltage / samplecount);
                BenchData.yvoltage = (UInt32)(BenchData.yvoltage / samplecount);
                BenchData.bvoltage = (UInt32)(BenchData.bvoltage / samplecount);

                BenchData.rcurrent = (UInt32)(BenchData.rcurrent / samplecount);
                BenchData.ycurrent = (UInt32)(BenchData.ycurrent / samplecount);
                BenchData.bcurrent = (UInt32)(BenchData.bcurrent / samplecount);

                //BenchData.rpowerfactor = (UInt32)(BenchData.rpowerfactor / samplecount);
                //BenchData.ypowerfactor = (UInt32)(BenchData.ypowerfactor / samplecount);
                //BenchData.bpowerfactor = (UInt32)(BenchData.bpowerfactor / samplecount);

                BenchData.rpowerfactor = (UInt32)(BenchData.rpowerfactor);
                BenchData.ypowerfactor = (UInt32)(BenchData.ypowerfactor );
                BenchData.bpowerfactor = (UInt32)(BenchData.bpowerfactor);

                BenchData.ractivepower = (UInt32)(BenchData.ractivepower / samplecount);
                BenchData.yactivepower = (UInt32)(BenchData.yactivepower / samplecount);
                BenchData.bactivepower = (UInt32)(BenchData.bactivepower / samplecount);

                BenchData.rreactivepower = (UInt32)(BenchData.rreactivepower / samplecount);
                BenchData.yreactivepower = (UInt32)(BenchData.yreactivepower / samplecount);
                BenchData.breactivepower = (UInt32)(BenchData.breactivepower / samplecount);

                // Write Log 
                string strlogdata = "\nBench Samples :-----";
                strlogdata += "\nVoltage:  " + BenchData.rvoltage.ToString() + ", " + BenchData.yvoltage.ToString() + ", " + BenchData.bvoltage.ToString();
                strlogdata += "\nCurrent:  " + BenchData.rcurrent.ToString() + ", " + BenchData.ycurrent.ToString() + ", " + BenchData.bcurrent.ToString();
                strlogdata += "\nPF:       " + BenchData.rpowerfactor.ToString() + ", " + BenchData.ypowerfactor.ToString() + ", " + BenchData.bpowerfactor.ToString();
                strlogdata += "\nActive:   " + BenchData.ractivepower.ToString() + ", " + BenchData.yactivepower.ToString() + ", " + BenchData.bactivepower.ToString();
                strlogdata += "\nReactive: " + BenchData.rreactivepower.ToString() + ", " + BenchData.yreactivepower.ToString() + ", " + BenchData.breactivepower.ToString();

                Logger.Logger.WriteCalibLog(strlogdata, 255);


            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        ////////////////////// ///////////////////////////////////////////////////////
        /*
        Method Name: GetCalculatedCoeff
        Purpose:     Get Coefficient based on thier values
        Date:		 28-Sept-2017
        Author:		 Mohsin Raza		
        */
        /////////////////////////////////////////////////////////////////////////////
        private UInt32 GetCalculatedCoeff(UInt32 argdwBenchValue, UInt32 argdwCommValue)
        {
            const int IDEVFACTOR = 4096;
            UInt32 dwCoeff = 0;

            if (argdwCommValue > 0)
                dwCoeff = (argdwBenchValue * IDEVFACTOR) / argdwCommValue;

            return dwCoeff;

        }
    }
}
