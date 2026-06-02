using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
   public class CalibrationConstants_1Phase
    {
            /* Default Calibration Factors*/


            public string Fact_V = "69937";
            public string Fact_Phase_Current = "66200";
            public string Fact_Neutral_Current = "77600";
            public string Fact_Phase_Energy = "315331";
            public string Fact_Neutral_Energy = "278125";
            public string Fact_Phase_Phase = "70";
            public string Fact_Neutral_Phase = "60";
            public string Fact_Neutral_P_MagSlope_Low = "0";
            public string Fact_Neutral_P_MagSlope_High = "110";
            public string Fact_Neutral_P_PhaseSlope_Low = "20";
            public string Fact_Neutral_P_PhaseSlope_High = "13";

            /*public string Fact_V = "69550";
            public string Fact_Phase_Current = "66200";
            public string Fact_Neutral_Current = "77600";
            public string Fact_Phase_Energy = "326170";
            public string Fact_Neutral_Energy = "278656";
            public string Fact_Phase_Phase = "67";
            public string Fact_Neutral_Phase = "62";
            public string Fact_Neutral_P_MagSlope_Low = "0";
            public string Fact_Neutral_P_MagSlope_High = "110";
            public string Fact_Neutral_P_PhaseSlope_Low = "20";
            public string Fact_Neutral_P_PhaseSlope_High = "13";*/


            //public string Fact_V = "77666";
            //public string Fact_Phase_Current = "77082";
            //public string Fact_Neutral_Current = "108422";
            //public string Fact_Phase_Energy = "249966";
            //public string Fact_Neutral_Energy = "177875";
            //public string Fact_Phase_Phase = "76";
            //public string Fact_Neutral_Phase = "69";
            //public string Fact_Neutral_P_MagSlope_Low = "0";
            //public string Fact_Neutral_P_MagSlope_High = "-110";
            //public string Fact_Neutral_P_PhaseSlope_Low = "20";
            //public string Fact_Neutral_P_PhaseSlope_High = "55";
            public string Fact_Offset_I_Phase = "20";
            public string Fact_Offset_I_Neutral = "73";
            public string Fact_Offset_P_Phase = "20";
            public string Fact_Offset_P_Neutral = "14";
            public string Fact_Creep_Phase_Energy = "";
            public string Fact_Creep_Neutral_Energy = "";
            /* Calibration Reference Constants*/
            public string Ref_Voltage = "230";  //Volt
            public string Ref_Frequency = "50";            //Hz
            public string Ref_PulseConstant = "3200";
            public string Ref_Ical = "10";            //Amp (for both phase & neutral)
            public string LOG_10 = "1";
            public string LOG_2 = "0.3010";
            public string Const_Phase_Phase = "0.26555078125";
            public string Const_Neutral_Phase = "0.26555078125";
            public int Const_Temp_Offset = 338;
           public string CalVoltageFactor(string getVoltage)
            {
                try
                {
                    string calculatedVFactor = string.Empty;
                    // Factor_V= (Default Factor_V * Reference Voltage * 100)/measured RMS voltage
                    double Factor_V = (Convert.ToDouble(Fact_V) * Convert.ToDouble(Ref_Voltage) * 100) / (Convert.ToDouble(getVoltage) * 100);

                    return Factor_V.ToString("0");
                }
                catch (Exception)
                {
                    return "";
                }
            }
            public string CalPhaseCurrent(string getPhaseCurrent)
            {
                try
                {
                    string calculatedVFactor = string.Empty;
                    // Calibrated Factor_I _Phase = (Default Factor_I_Phase * Ical * 1000)/measured RMS current
                    double Factor_Ph_Current = (Convert.ToDouble(Fact_Phase_Current) * Convert.ToDouble(Ref_Ical) * 1000) / (Convert.ToDouble(getPhaseCurrent) * 1000);

                    return Factor_Ph_Current.ToString("0");
                }
                catch (Exception)
                {
                    return "";
                }
            }
            public string CalPhaseEnergy(string getPhaseEnergy)
            {
                try
                {
                string calculatedVFactor = string.Empty;
                double Error_PE = Convert.ToDouble(getPhaseEnergy);
                // Calibrated Phase_P_1WH = Default Phase_P_1WH + (Default Phase_P_1WH * error/100)
                Error_PE = ((Convert.ToDouble(Fact_Phase_Energy) * Error_PE) / 100);
                double Factor_Ph_Energy = Convert.ToDouble(Fact_Phase_Energy) + Error_PE;
                return Factor_Ph_Energy.ToString("0");
                }
                catch (Exception)
                {
                    return "";
                }
            }
            public string CalPhasePhase(string getPhasePhase)
            {
                try
                {
                    string calculatedVFactor = string.Empty;
                    double Error_PP = Convert.ToDouble(getPhasePhase);
                    // Calibrated Phase_IS_Delay = default Fact_Phase_Phase + error/ Const_Phase_Phase
                    Error_PP = Error_PP / Convert.ToDouble(Const_Phase_Phase);
                    double Factor_Ph_Ph = Convert.ToDouble(Fact_Phase_Phase) - Error_PP;
                    return Factor_Ph_Ph.ToString("0");
                }
                catch (Exception)
                {
                    return "";
                }
            }
            public string CalNeutralCurrent(string getNeuCurrent)
            {
                try
                {
                string calculatedNCFactor = string.Empty;
                // Calibrated Factor_I _Neutral = (Default Factor_I_Neutral * Ical * 1000)/measured RMS 
                double Factor_VNC = (Convert.ToDouble(Fact_Neutral_Current) * Convert.ToDouble(Ref_Ical) * 1000) / (Convert.ToDouble(getNeuCurrent)*1000);
                return Factor_VNC.ToString("0");
                }
                catch (Exception)
                {
                    return "";
                }
            }
            public string CalNeutralEnergy(string getNeuEnergy)
            {
                try
                {
                string calNeuEnerguFactor = string.Empty;
                double Error_NE = Convert.ToDouble(getNeuEnergy);
                // Calibrated Neutral_P_1WH = Default Neutral_P_1WH + (Default Neutral_P_1WH * error/100)
                Error_NE = ((Convert.ToDouble(Fact_Neutral_Energy) * Error_NE) / 100);
                double Factor_Ph_Energy = Convert.ToDouble(Fact_Neutral_Energy) + Error_NE;
                return Factor_Ph_Energy.ToString("0");
                }
                catch (Exception)
                {
                    return "";
                }
            }
            public string CalNeuLR(string getNLR)
            {
                try
                {
                string calNeuEnerguFactor = string.Empty;
                double Error_NLR = Convert.ToDouble(getNLR);
                // Neutral_P_MagSlope_Low  =  (0 -  error)/(Log(10) – Log(2)) * 10000
                Error_NLR = ((0 - Error_NLR) / (Convert.ToDouble(LOG_10) - (Convert.ToDouble(LOG_2)))) * 100;
                return Error_NLR.ToString("0");
                }
                catch (Exception)
                {
                    return "";
                }
            }
            public string CalNeuPhase(string getNeuPhase)
            {
                try
                {
                string calculatedNPFactor = string.Empty;
                double Error_NP = Convert.ToDouble(getNeuPhase);
                // Calibrated Neutral_IC_Delay = default Neutral_IC_Delay + error/ 0.26555078125
                Error_NP = + Error_NP/Convert.ToDouble(Const_Neutral_Phase);
                double Factor_Neu_Ph =Convert.ToDouble(Fact_Neutral_Phase) - Error_NP  ;
                return Factor_Neu_Ph.ToString("0");
                }
                catch (Exception)
                {
                    return "";
                }
            }
    }
}
