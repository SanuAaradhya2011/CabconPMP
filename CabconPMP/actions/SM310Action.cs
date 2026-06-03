using IntegratedCalibration.Constants;
using LNG.Communication.SerialCommunication;
using SmartCalibration.Communication;
using SmartCalibration.Constants;
using SmartCalibration.DataLayer;
using SmartCalibration.Error;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCalibration.Actions
{
    /// <summary>
    /// Action class for THREE PHASE SMART METER - SM310
    /// 
    /// </summary>
    public class SM310Action : GenericAction
    {
        // AT PRESENT MAX PARAM COUNT IS 8 AS ON 07-FEB-2020
        byte MAXPARAMCOUNT = 8;
        bool isoutofrange = false;

        // EIGHT TYPE OF PARAM TO CALIBRATE IN SM310
        enum SM310CALIBCOMMAND
        {
            VOLTAGE = 1,
            CURRENT,
            ACTPOWER,
            REACTPOWER,
            FVOLTAGE,
            FCURRENT,
            NEUTRAL,
            TEMPERATURE,
        }

        #region RESET CALIBRATION
        /// <summary>
        /// Method Name: CALIBRESET
        /// Description: Reset Calibration, Value of mAct is 1 for reset calibration
        /// Author:      Mohsin Raza
        /// Date:        07-FEB-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result CALIBRESET(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            mAct = 1;
            return CalibInitialise(mpos, mAct);
        }
        #endregion

        #region Active CALIBRATION
        /// <summary>
        /// Method Name: CALIBACTIVE
        /// Description: CALIBRATE VOLTAGE, CURRENT, ACTIVE POWER;  Value of mAct is 02 for active calibration
        /// Author:      Mohsin Raza
        /// Date:        07-FEB-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result CALIBACTIVE(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;

            mAct = 02;
                    
            if (CalibInitialise(mpos, mAct) != GlobalConstants.Result.Pass)
            {
                Logger.Logger.WriteCalibLog("INIT Command -> Fail", mpos);
                return _result;
            }

            Logger.Logger.WriteCalibLog("INIT Command -> Pass", mpos);

            Logger.Logger.WriteCalibLog("Waiting for time (msec) - " + GlobalConstants.SM310_MAXSAMPLECOLLECTIONTIME.ToString(), mpos);

            System.Threading.Thread.Sleep(GlobalConstants.SM310_MAXSAMPLECOLLECTIONTIME);
            
            return CalibrateMeter(mpos, mAct);
        }
        #endregion

        #region Reactive CALIBRATION
        /// <summary>
        /// Method Name: CALIBREACTIVE
        /// Description: Reactive Calibration, Value of mAct is 04 for reset calibration
        /// Author:      Mohsin Raza
        /// Date:        07-FEB-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result CALIBREACTIVE(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            mAct = 04;

            if (CalibInitialise(mpos, mAct) != GlobalConstants.Result.Pass)
                return _result;

            System.Threading.Thread.Sleep(GlobalConstants.SM310_MAXSAMPLECOLLECTIONTIME);

            return CalibrateMeter(mpos, mAct);
        }
        #endregion

        #region Neutral CALIBRATION
        /// <summary>
        /// Method Name: CALIBNEUTRAL
        /// Description: Neutral Calibration, Value of mAct is 16 for reset calibration, Current applied in R Phase
        /// Author:      Mohsin Raza
        /// Date:        07-FEB-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result CALIBNEUTRAL(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            mAct = 16;

            if (CalibInitialise(mpos, mAct) != GlobalConstants.Result.Pass)
                return _result;

            System.Threading.Thread.Sleep(GlobalConstants.SM310_MAXSAMPLECOLLECTIONTIME);

            return CalibrateMeter(mpos, mAct);
        }
        #endregion

        #region FUNDAMENTAL CALIBRATION VI
        /// <summary>
        /// Method Name: CALIBFVI
        /// Description: VI Fundamental Calibration, Value of mAct is 16 for reset calibration, Current applied in R Phase
        /// Author:      Mohsin Raza
        /// Date:        07-FEB-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result CALIBFVI(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            mAct = 32;

            if (CalibInitialise(mpos, mAct) != GlobalConstants.Result.Pass)
                return _result;

            System.Threading.Thread.Sleep(GlobalConstants.SM310_MAXSAMPLECOLLECTIONTIME);

            return CalibrateMeter(mpos, mAct);
        }
        #endregion

        #region Temperature CALIBRATION
        /// <summary>
        /// Method Name: CALIBRESET
        /// Description: Reset Calibration
        /// Author:      Mohsin Raza
        /// Date:        07-FEB-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result CALIBTEMP(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            mAct = 64;

            if (CalibInitialise(mpos, mAct) != GlobalConstants.Result.Pass)
                return _result;

            System.Threading.Thread.Sleep(2000);

            return CalibrateMeter(mpos, mAct);
        }
        #endregion

        #region CALIBACTIVE
        /// <summary>
        /// Method Name: CALIBACTIVE
        /// Description: Active Power Calibration
        /// Author:      Mohsin Raza
        /// Date:        07-FEB-2020
        /// </summary>        
        public Constants.GlobalConstants.Result CALIBRATE(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;

            if (CalibInitialise(mpos, mAct) != GlobalConstants.Result.Pass)
                return _result;

            System.Threading.Thread.Sleep(GlobalConstants.SM310_MAXSAMPLECOLLECTIONTIME);

            return CalibrateMeter(mpos, mAct);

        }
        #endregion
        
        #region INIT
        /// <summary>
        /// Method Name: INIT
        /// Description: Initialise Calibration for Active or Reactive based on argument pass
        /// RESET - 01
        /// ACTIVE - 02
        /// REACTIVE - 04
        /// NEUTRAL - 16
        /// VIFUNDAMENTAL - 32
        /// TEMPERATURE - 64
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        private Constants.GlobalConstants.Result CalibInitialise(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            cmddelay = 2000;
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM310MeterObject[Constants.DLMSConstants.DLMSCommand.SM310INIT].mclobisatt);
            cosempkt.Add(0x00); // access selector
            cosempkt.Add(0x02); // DataType struct
            cosempkt.Add(0x02); // Struct Len
            cosempkt.Add(0x11); // DataType - Unsigned one byte
            cosempkt.Add((byte)mAct); // DataType - Unsigned one byte
            cosempkt.Add(0x11);     // DataType - Unsigned one byte
            cosempkt.Add((byte)Constants.GlobalConstants.MAXSM310SAMPLES); 
           
            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                    return _result;

                if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET) != Constants.GlobalConstants.Result.Pass)
                    return _result;

                _result = Constants.GlobalConstants.Result.Pass;

            }
            catch (Exception ex)
            {
                _result = Constants.GlobalConstants.Result.Fail;

            }
            finally
            {
                // disconnect the connection
                _dlmsmanager.DisConnect();
            }

            return _result;
        }
        #endregion

        #region CalibrateMeter
        /// <summary>
        /// Method Name: INIT
        /// Description: Reset Meter Data
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        private Constants.GlobalConstants.Result CalibrateMeter(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();
            int startindex = 20;
            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM310MeterObject[Constants.DLMSConstants.DLMSCommand.SM310GETSAMPLECALIBRATE].mclobisatt);
            cosempkt.Add(0x00); // access selector

            Logger.Logger.WriteCalibLog("Get Sample Command---", mpos);

            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                    return _result;

                if (_dlmsmanager.NormalGetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALGET) != Constants.GlobalConstants.Result.Pass)
                    return _result;

                if (_dlmsmanager.ValidData == null)
                {
                    Logger.Logger.WriteCalibLog("Get Sample Command Invalid Data - > Fail", mpos);
                    return _result;
                }

                if (_dlmsmanager.ValidData[startindex - 1] != MAXPARAMCOUNT)
                {
                    Logger.Logger.WriteCalibLog("Param Count Mismatched - > Fail", mpos);
                    return _result;
                }


                // Updated Log on 22 feb 2018
                /*
                 7E A0 95 FD 00 02 04 01 96 1A 7A E6 E7 00 C4 01   ~ •ý....–.zæç.Ä.
                 C1 00 02 08 02 04 11 00 06 00 00 00 00 06 00 00   Á...............
                 00 00 06 00 00 00 00 02 04 11 00 06 00 00 00 00   ................
                 06 00 00 00 00 06 00 00 00 00 02 04 11 00 06 00   ................
                 00 00 00 06 00 00 00 00 06 00 00 00 00 02 04 11   ................
                 00 06 00 00 00 00 06 00 00 00 00 06 00 00 00 00   ................
                 02 04 11 00 06 00 00 00 00 06 00 00 00 00 06 00   ................
                 00 00 00 02 04 11 00 06 00 00 00 00 06 00 00 00   ................
                 00 06 00 00 00 00 02 02 11 00 12 00 00 02 02 11   ................
                 08 12 01 34 A1 4D 7E                              ...4¡M~

                */

                Logger.Logger.WriteCalibLog("Get Sample Command - > Pass", mpos);

                List<UInt32> meterparams = new List<UInt32>();

                startindex = 22;

                while (startindex < _dlmsmanager.ValidData[2]-1)
                {

                    byte[] barray = (byte[])basehdlc.dlmsparser(_dlmsmanager.ValidData.ToArray(), ref startindex);

                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(barray);

                    if (barray.Count() == 4)
                        meterparams.Add(BitConverter.ToUInt32(barray, 0));
                    else if (barray.Count() == 2)
                        meterparams.Add(BitConverter.ToUInt16(barray, 0));
                    else if (barray.Count() == 1)
                        meterparams.Add(barray[0]);

                }

                if (meterparams == null || meterparams.Count < 16)
                    return _result;

                cosempkt.Clear();
                cosempkt.AddRange(Constants.DLMSConstants.GlobalSM310MeterObject[Constants.DLMSConstants.DLMSCommand.SM310SETCALIBRATE].mclobisatt);
                cosempkt.Add(0x00);
                cosempkt.AddRange(GetCosemCoefficient(meterparams, mpos));

                if(isoutofrange)
                {
                    return Constants.GlobalConstants.Result.OUT;
                }

                return _dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET);

            }
            catch (Exception ex)
            {
                _result = Constants.GlobalConstants.Result.Fail;

            }
            finally
            {
                // disconnect the connection
                _dlmsmanager.DisConnect();
            }

            return _result;
        }
        #endregion

        #region GetCosemCoefficient
        private List<byte> GetCosemCoefficient(List<UInt32> meterparams, int mpos)
        {
            List<byte> cosempkt = new List<byte>();
            const byte VOLTAGEPARAM = 0;
            const byte CURRENTPARAM = 4;
            const byte ACTPOWERPARAM = 8;
            const byte REACTPOWERPARAM = 12;
            const byte VFPARAM = 16;
            const byte IFPARAM = 20;
            const byte NEUTRALPARAM = 24;
            const byte TEMPERATUREPARAM = 26;

            try
            {
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.STRUCT);
                cosempkt.Add(MAXPARAMCOUNT);
                Logger.Logger.WriteCalibLog("ParamCount -> " + MAXPARAMCOUNT.ToString(), mpos);
                /*
                 7E A0 73 00 02 04 01 FD 98 4A AD E6 E6 00 C1 01
                 C1 00 01 00 00 60 02 88 FF 02 00 02 07 02 04 11
                 01 12 6E 17 12 00 00 12 00 00 02 04 11 02 12 00
                 00 12 00 00 12 00 00 02 04 11 03 12 00 00 12 00
                 00 12 00 00 02 04 11 00 12 00 00 12 00 00 12 00
                 00 02 04 11 00 12 00 00 12 00 00 12 00 00 02 04
                 11 00 12 00 00 12 00 00 12 00 00 02 02 11 00 12
                 00 00 3E BE 7E

                */

                // Fill Voltage Samples - R,Y,B
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.STRUCT);
                cosempkt.Add(0x04);
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.UNSIGNED);
                cosempkt.Add((byte)meterparams[VOLTAGEPARAM]);
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.rvoltage, meterparams[VOLTAGEPARAM + 1], SM310CALIBCOMMAND.VOLTAGE, mpos, (byte)meterparams[VOLTAGEPARAM]));
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.yvoltage, meterparams[VOLTAGEPARAM + 2], SM310CALIBCOMMAND.VOLTAGE, mpos, (byte)meterparams[VOLTAGEPARAM]));
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.bvoltage, meterparams[VOLTAGEPARAM + 3], SM310CALIBCOMMAND.VOLTAGE, mpos, (byte)meterparams[VOLTAGEPARAM]));

                // Fill Current Samples - R,Y,B
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.STRUCT);
                cosempkt.Add(0x04);
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.UNSIGNED);
                cosempkt.Add((byte)meterparams[CURRENTPARAM]);
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.rcurrent, meterparams[CURRENTPARAM + 1], SM310CALIBCOMMAND.CURRENT, mpos, (byte)meterparams[CURRENTPARAM]));
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.ycurrent, meterparams[CURRENTPARAM + 2], SM310CALIBCOMMAND.CURRENT, mpos, (byte)meterparams[CURRENTPARAM]));
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.bcurrent, meterparams[CURRENTPARAM + 3], SM310CALIBCOMMAND.CURRENT, mpos, (byte)meterparams[CURRENTPARAM]));

                // Fill ACTIVE POWER Samples - R,Y,B
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.STRUCT);
                cosempkt.Add(0x04);
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.UNSIGNED);
                cosempkt.Add((byte)meterparams[ACTPOWERPARAM]);
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.ractivepower, meterparams[ACTPOWERPARAM + 1], SM310CALIBCOMMAND.ACTPOWER, mpos, (byte)meterparams[ACTPOWERPARAM]));
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.yactivepower, meterparams[ACTPOWERPARAM + 2], SM310CALIBCOMMAND.ACTPOWER, mpos, (byte)meterparams[ACTPOWERPARAM]));
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.bactivepower, meterparams[ACTPOWERPARAM + 3], SM310CALIBCOMMAND.ACTPOWER, mpos, (byte)meterparams[ACTPOWERPARAM]));

                // Fill REACTIVE Samples - R,Y,B
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.STRUCT);
                cosempkt.Add(0x04);
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.UNSIGNED);
                cosempkt.Add((byte)meterparams[REACTPOWERPARAM]);
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.rreactivepower, meterparams[REACTPOWERPARAM + 1], SM310CALIBCOMMAND.REACTPOWER, mpos, (byte)meterparams[REACTPOWERPARAM]));
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.yreactivepower, meterparams[REACTPOWERPARAM + 2], SM310CALIBCOMMAND.REACTPOWER, mpos, (byte)meterparams[REACTPOWERPARAM]));
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.breactivepower, meterparams[REACTPOWERPARAM + 3], SM310CALIBCOMMAND.REACTPOWER, mpos, (byte)meterparams[REACTPOWERPARAM]));

                // Fill FUNDAMENTAL Voltage Samples - R,Y,B
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.STRUCT);
                cosempkt.Add(0x04);
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.UNSIGNED);
                cosempkt.Add((byte)meterparams[VFPARAM]);
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.rvoltage, meterparams[VFPARAM + 1], SM310CALIBCOMMAND.FVOLTAGE, mpos, (byte)meterparams[VFPARAM]));
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.yvoltage, meterparams[VFPARAM + 2], SM310CALIBCOMMAND.FVOLTAGE, mpos, (byte)meterparams[VFPARAM]));
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.bvoltage, meterparams[VFPARAM + 3], SM310CALIBCOMMAND.FVOLTAGE, mpos, (byte)meterparams[VFPARAM]));

                // Fill FUNDAMENTAL CURRENT Samples - R,Y,B
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.STRUCT);
                cosempkt.Add(0x04);
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.UNSIGNED);
                cosempkt.Add((byte)meterparams[IFPARAM]);
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.rcurrent, meterparams[IFPARAM + 1], SM310CALIBCOMMAND.FCURRENT, mpos, (byte)meterparams[IFPARAM]));
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.ycurrent, meterparams[IFPARAM + 2], SM310CALIBCOMMAND.FCURRENT, mpos, (byte)meterparams[IFPARAM]));
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.bcurrent, meterparams[IFPARAM + 3], SM310CALIBCOMMAND.FCURRENT, mpos, (byte)meterparams[IFPARAM]));

                // Fill NEUTRAL CURRENT Samples - R
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.STRUCT);
                cosempkt.Add(0x02);
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.UNSIGNED);
                cosempkt.Add((byte)meterparams[NEUTRALPARAM]);
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(DataLayer.BenchData.rcurrent, meterparams[NEUTRALPARAM + 1], SM310CALIBCOMMAND.NEUTRAL, mpos, (byte)meterparams[NEUTRALPARAM]));

                // Fill TEMPERATURE Samples 
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.STRUCT);
                cosempkt.Add(0x02);
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.UNSIGNED);
                cosempkt.Add((byte)meterparams[TEMPERATUREPARAM]);
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONG);
                cosempkt.AddRange(GetCalculatedCoeff(m_temprature, meterparams[TEMPERATUREPARAM + 1], SM310CALIBCOMMAND.TEMPERATURE, mpos, (byte)meterparams[TEMPERATUREPARAM]));

            }
            catch(Exception ex)
            {
                Logger.Logger.WriteCalibLog("SM310Action -> GetCosemCoefficient > Parser Error", mpos);
            }

            return cosempkt;
        }

        #endregion

        #region GetCalculatedCoeff
        ////////////////////// ///////////////////////////////////////////////////////
        /*
        Method Name: GetCalculatedCoeff with one argument
        Purpose:     Get Coefficient based on thier values
        Date:		 28-Sept-2017
        Author:		 Mohsin Raza		
        */
        /////////////////////////////////////////////////////////////////////////////
        private byte[] GetCalculatedCoeff(UInt32 benchvalue, UInt32 metercommvalue, SM310CALIBCOMMAND commandtype, int mpos, byte paramstatus)
        {
            const int IDEVFACTOR = 4096;
            UInt16 uiival = 0;
            byte[] coeffdata = null;
            List<byte> cosempkt = new List<byte>();
            const UInt32 MAXPARAMLIMIT = 60000;
            const UInt32 MINPARAMLIMIT = 1;
            string strlog = commandtype.ToString() + " Coeffcient -> Status -> " + paramstatus.ToString("D2");

            cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.STRUCT);
            cosempkt.Add(0x02);
            cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.UNSIGNED);
            cosempkt.Add((byte)SM310CALIBCOMMAND.CURRENT);

            if (paramstatus !=0 && metercommvalue < MINPARAMLIMIT)
            {
                isoutofrange = true;
                strlog += " Out of range -> MeterComValue ->" + metercommvalue.ToString();
                Logger.Logger.WriteCalibLog(strlog, mpos);
                coeffdata = new byte[] { 0, 0};
                return coeffdata;
            }

            try
            {
                if (metercommvalue > 0)
                {
                    if (commandtype == SM310CALIBCOMMAND.TEMPERATURE)
                    {
                        uiival = (UInt16)(metercommvalue - m_temprature);
                    }
                    else
                    {
                        uiival = (UInt16)((benchvalue * IDEVFACTOR) / metercommvalue);

                    }

                    strlog += " Calculated- " + uiival.ToString() + " Benchvalue- " + benchvalue.ToString() + " MeterCommvalue: " + metercommvalue.ToString();
                    Logger.Logger.WriteCalibLog(strlog, mpos);
                    
                    if (uiival > MAXPARAMLIMIT || uiival < MINPARAMLIMIT)
                    {
                        isoutofrange = true;
                        strlog += " Out of range -> CalulatedValue ->" + uiival.ToString();
                        Logger.Logger.WriteCalibLog(strlog, mpos);
                    }
                }

                coeffdata = BitConverter.GetBytes(uiival);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(coeffdata);
            }
            catch(Exception ex)
            {
                Logger.Logger.WriteCalibLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + "->"
                                      + System.Reflection.MethodBase.GetCurrentMethod().Name + "->" + ex.Message, mpos);
            }

            return coeffdata;

        }
        #endregion

        #region ResetData
        /// <summary>
        /// Method Name: ResetData
        /// Description: Reset Data from Meter full
        /// </summary>        
        public override Constants.GlobalConstants.Result RESETDATA(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM310RESET);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();
            
            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM310MeterObject[Constants.DLMSConstants.DLMSCommand.DATARESET].mclobisatt);
            cosempkt.Add(0x00);
            cosempkt.Add(0x12);
            cosempkt.Add(0x00);
            cosempkt.Add(0x01);

            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                {
                    return _result;
                }

                if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET) != Constants.GlobalConstants.Result.Pass)
                    return _result;

                _result = Constants.GlobalConstants.Result.Pass;

            }
            catch (Exception ex)
            {
                _result = Constants.GlobalConstants.Result.Fail;
                Logger.Logger.WriteCalibLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + "->"
                                       + System.Reflection.MethodBase.GetCurrentMethod().Name + "->" + ex.Message, mpos);
            }
            finally
            {
                // disconnect the connection
                _dlmsmanager.DisConnect();
            }

            return _result;
        }
        #endregion

        #region GETTRAVELLER
        /// <summary>
        /// Method Name: DLMS_GETMSN
        /// Description: Get PCBA number from meter and write in imp.res, 01 Means - GetPCBA, 02- Get Meter Serial Number (Owner Number)
        /// Author:      Mohsin Raza
        /// Date:        30-Oct-2015
        /// </summary>
        public override Constants.GlobalConstants.Result GETTRAVELLER(int mpos, int mAct)
        {
            // TODO : Variables declaration and initialisation here
            // double unsigned

            AMITraveller _amitraveller = new AMITraveller();
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();
            cosempkt.AddRange(Constants.DLMSConstants.GlobalGenericMeterObject[Constants.DLMSConstants.DLMSCommand.GETTRAVELER].mclobisatt);

            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                    return _result;

                if (_dlmsmanager.NormalGetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALGET) != Constants.GlobalConstants.Result.Pass)
                    return _result;

                if (_dlmsmanager.ValidData == null || _dlmsmanager.ValidData.Count < 50)
                {
                    Logger.Logger.WriteCalibLog("Valid Data error", serialcomm.mpos);
                    return _result;
                }
                //rpacket[28] == 2 && (rpacket[29] == 1 || rpacket[29] == 17)
                if (_dlmsmanager.serialcom.ReceiveBuffer[28] == 0x02 && (_dlmsmanager.serialcom.ReceiveBuffer[29] == 1 || _dlmsmanager.serialcom.ReceiveBuffer[29] == 17))
                {
                    _amitraveller.WriteTravellerStatus(serialcomm.mpos, "PASS");
                    meterdetail = "PASS";
                    _result = Constants.GlobalConstants.Result.Pass;
                    Logger.Logger.WriteCalibLog("Traveller Status written - pass", serialcomm.mpos);
                }
                else
                {
                    _amitraveller.WriteTravellerStatus(serialcomm.mpos, "FAIL");
                    meterdetail = "FAIL";
                    _result = Constants.GlobalConstants.Result.OUT;
                    Logger.Logger.WriteCalibLog("Traveller Status written - fail", serialcomm.mpos);
                }

            }
            catch (Exception ex)
            {
                _result = Constants.GlobalConstants.Result.Fail;
                Logger.Logger.WriteCalibLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + "->"
                                       + System.Reflection.MethodBase.GetCurrentMethod().Name + "->" + ex.Message, mpos);

            }
            finally
            {
                // disconnect the connection
                _dlmsmanager.DisConnect();


            }

            return _result;
        }
        #endregion

        #region SETTRAVELLER
        /// <summary>
        /// Method Name: DLMS_GETMSN
        /// Description: Get PCBA number from meter and write in imp.res, 01 Means - GetPCBA, 02- Get Meter Serial Number (Owner Number)
        /// Author:      Mohsin Raza
        /// Date:        30-Oct-2015
        /// </summary>
        public override Constants.GlobalConstants.Result SETTRAVELLER(int mpos, int mAct)
        {
            // TODO : Variables declaration and initialisation here
            // double unsigned
            const int STARTADDRESS = 0x10;
            const int LENGTHBYTETOREAD = 16;
            const byte DATATYPE_OCTETSTRING = 0x09;
            byte TRAVELLERSTAG = 0x03;
            byte CALIBSTATUS = 1; // 1- PASS 0- Fail
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);

            AMITraveller _amitraveller = new AMITraveller();
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalGenericMeterObject[Constants.DLMSConstants.DLMSCommand.SETTRAVELER].mclobisatt);
            cosempkt.Add(DATATYPE_OCTETSTRING);
            cosempkt.Add(LENGTHBYTETOREAD + 2);
            cosempkt.Add(STARTADDRESS >> 8);
            cosempkt.Add(STARTADDRESS);
            cosempkt.Add(TRAVELLERSTAG);

            if (_amitraveller.ReadTravellerStatus(serialcomm.mpos) == true)
                cosempkt.Add(1);
            else
                cosempkt.Add(0);

            string timestamp = DateTime.Now.Year.ToString("d4") +
                                DateTime.Now.Month.ToString("d2") +
                                DateTime.Now.Day.ToString("d2") +
                                DateTime.Now.Hour.ToString("d2") +
                                DateTime.Now.Minute.ToString("d2") +
                                DateTime.Now.Second.ToString("d2");

            // 6 bytes for time stamp
            Int64 uinttimestamp = Int64.Parse(timestamp);
            cosempkt.Add((byte)(uinttimestamp >> 40));
            cosempkt.Add((byte)(uinttimestamp >> 32));
            cosempkt.Add((byte)(uinttimestamp >> 24));
            cosempkt.Add((byte)(uinttimestamp >> 16));
            cosempkt.Add((byte)(uinttimestamp >> 8));
            cosempkt.Add((byte)uinttimestamp);

            // 8 bytes reserve
            cosempkt.Add(0x00);
            cosempkt.Add(0x00);
            cosempkt.Add(0x00);
            cosempkt.Add(0x00);
            cosempkt.Add(0x00);
            cosempkt.Add(0x00);
            cosempkt.Add(0x00);
            cosempkt.Add(0x00);


            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                    return _result;

                if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET) != Constants.GlobalConstants.Result.Pass)
                    return _result;

                if (_dlmsmanager.serialcom.ReceiveBuffer[17] == 0)
                {
                    _result = Constants.GlobalConstants.Result.Pass;
                    Logger.Logger.WriteCalibLog("Traveller Status write - pass", serialcomm.mpos);
                }
                else
                {
                    _result = Constants.GlobalConstants.Result.Fail;
                    Logger.Logger.WriteCalibLog("Traveller Status write - fail", serialcomm.mpos);
                }

            }
            catch (Exception ex)
            {
                _result = Constants.GlobalConstants.Result.Fail;
                Logger.Logger.WriteCalibLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + "->"
                                       + System.Reflection.MethodBase.GetCurrentMethod().Name + "->" + ex.Message, mpos);

            }
            finally
            {
                // disconnect the connection
                _dlmsmanager.DisConnect();


            }

            return _result;
        }
        #endregion

        #region CALIBVERIFYN
        /// <summary>
        /// Method Name: CALIBVERIFYN
        /// Description: Neutral Current Verification, Apply 2 Amp
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        //public override Constants.GlobalConstants.Result CALIBVERIFYN(int mpos, int mAct)
        //{
        //    // TODO  : Variables declaration and Initialisation here
        //    Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
        //    SerialComm serialcomm = new SerialComm();
        //    serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTOTHERSCMD);
        //    DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
        //    List<byte> cosempkt = new List<byte>();
        //    cosempkt.AddRange(Constants.DLMSConstants.GlobalSM310MeterObject[Constants.DLMSConstants.DLMSCommand.SM310VERIFYNEUTRAL].mclobisatt);
        //    cosempkt.Add(0x00);
        //    int startindex = 18;
        //    int idev = 1000;
        //    const double MINVAL = 4.85;
        //    const double MAXVAL = 5.25;




        //    try
        //    {
        //        if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
        //        {
        //            return _result;
        //        }

        //        if (_dlmsmanager.NormalGetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALGET) != Constants.GlobalConstants.Result.Pass)
        //            return _result;

        //        byte[] barray = (byte[])basehdlc.dlmsparser(_dlmsmanager.ValidData.ToArray(), ref startindex);

        //        if (BitConverter.IsLittleEndian)
        //            Array.Reverse(barray);

        //        UInt32 val = BitConverter.ToUInt32(barray, 0);

        //        double neutralval = val * 1.00 / idev;

        //        string strTime = @DateTime.Now.Day.ToString() + "/" +
        //                DateTime.Now.Month.ToString() + "/" +
        //                DateTime.Now.Year.ToString() + "-" +
        //                DateTime.Now.Hour.ToString("d2") + ":" +
        //                DateTime.Now.Minute.ToString("d2") + ":" +
        //                DateTime.Now.Second.ToString("d2") + "-> Position: " + mpos.ToString();


        //        if (neutralval > MAXVAL || neutralval < MINVAL)
        //        {
        //            strTime += " -> Neutral Current: Fail \n";
        //            meterdetail = strTime;
        //            return Constants.GlobalConstants.Result.OUT;
        //        }


        //        strTime += " -> Neutral Current: " + (val/idev).ToString() + "." + (val%idev).ToString("d3") + " A \n";

        //        meterdetail = strTime;

        //        _result = Constants.GlobalConstants.Result.Pass;

        //    }
        //    catch (Exception ex)
        //    {
        //        _result = Constants.GlobalConstants.Result.Fail;
        //        Logger.Logger.WriteCalibLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + "->"
        //                               + System.Reflection.MethodBase.GetCurrentMethod().Name + "->" + ex.Message, mpos);
        //    }
        //    finally
        //    {
        //        // disconnect the connection
        //        _dlmsmanager.DisConnect();
        //    }

        //    return _result;
        //}
        //#endregion


        
        /// <summary>
        /// Method Name: CALIBVERIFYN
        /// Description: Neutral Current Verification, Apply 2 Amp
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result CALIBVERIFYN(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTOTHERSCMD);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();
            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM310MeterObject[Constants.DLMSConstants.DLMSCommand.SM310VERIFYNEUTRAL].mclobisatt);
            cosempkt.Add(0x00);
            int startindex = 18;
            int idev = 1000;
            const double MINVAL = 4.85;
            const double MAXVAL = 5.25;
            string status = " Pass";


            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                {
                    return _result;
                }

                if (_dlmsmanager.NormalGetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALGET) != Constants.GlobalConstants.Result.Pass)
                    return _result;

                byte[] barray = (byte[])basehdlc.dlmsparser(_dlmsmanager.ValidData.ToArray(), ref startindex);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(barray);

                UInt32 val = BitConverter.ToUInt32(barray, 0);

                double neutralval = val * 1.00 / idev;

                string strTime = @DateTime.Now.Day.ToString() + "/" +
                        DateTime.Now.Month.ToString() + "/" +
                        DateTime.Now.Year.ToString() + "-" +
                        DateTime.Now.Hour.ToString("d2") + ":" +
                        DateTime.Now.Minute.ToString("d2") + ":" +
                        DateTime.Now.Second.ToString("d2") + "-> Position: " + mpos.ToString();




                strTime += " -> Neutral Current: " + (val / idev).ToString() + "." + (val % idev).ToString("d3") + " A";

                meterdetail = strTime;

                string filename = AppDomain.CurrentDomain.BaseDirectory + "Neutral Current" + mpos.ToString("d2") + ".lg";


                _result = Constants.GlobalConstants.Result.Pass;

                if (neutralval > MAXVAL || neutralval < MINVAL)
                {
                    //strTime += " -> Neutral Current: Fail \n";
                    //meterdetail = strTime;
                    status = " Fail";
                    _result = Constants.GlobalConstants.Result.OUT;
                }

                File.WriteAllText(filename, strTime + status + Environment.NewLine);

                return _result;

            }
            catch (Exception ex)
            {
                _result = Constants.GlobalConstants.Result.Fail;
                Logger.Logger.WriteCalibLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + "->"
                                       + System.Reflection.MethodBase.GetCurrentMethod().Name + "->" + ex.Message, mpos);
            }
            finally
            {
                // disconnect the connection
                _dlmsmanager.DisConnect();
            }

            return _result;
        }
        #endregion


        #region CALIBVERIFY
        /// <summary>
        /// Method Name: CALIBVERIFY
        /// Description: Verify Calibration, Phase - true, Neutral - false
        /// Author:      Ravi Tiwari
        /// Date:        12-JAN-2021
        /// </summary> 
        public override Constants.GlobalConstants.Result CALIBVERIFY(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
         //   /*
	 
	        // 7e a0 1c 00 02 04 01 fd 54 5b 1c e6 e6 00 c0 01   ~.......T[......
	        // c1 00 01 00 00 60 02 88 ff 02 00 f3 75 7e         .....`......u~

	        // 7e a0 45 fd 00 02 04 01 74 d4 4a e6 e7 00 c4 01   ~.E.....t.J.....
	        // c1 00 09 30 89 b0 00 00 e6 c0 00 00 66 0d 01 00   ...0........f...
	        // 82 11 00 00 8a 0c 00 00 82 11 00 00 8a 0c 00 00   ................
	        // be 00 96 00 52 01 00 00 00 00 00 00 00 00 00 00   ....R...........
	        // 00 00 ab 17 a5 f6 7e                              ......
	
	        //*/

         //   /*
         //   Voltage					45628	41065.2		50190.8	10
         //   Phase Current			49000	39200		58800	20
         //   Neutral Current			69488	62539.2		76436.8	10
         //   Phase Active Power		4474	3579.2		5368.8	20
         //   Neutral Active Power	3154	2838.6		3469.4	10
         //   Phase Reactive Power	4474	3579.2		5368.8	20
         //   Neutral Reactive Power	3154	2838.6		3469.4	10
         //   Phase Delay				185		166.5		203.5	10
         //   Neutral Delay			145		116			174	20
         //   */
         //   byte VPER = 10;
         //   byte PHCUR = 20;
         //   byte NCUR = 10;
         //   byte PHACT = 20;
         //   byte NACT = 10;
         //   byte PHREACT = 20;
         //   byte NREACT = 10;
         //   byte PDELAY = 10;
         //   byte NDELAY = 20;

         //   uint defVoltage = 45628;
         //   uint defPhaseCurrent = 49000;
         //   uint defNeutralCurrent = 69488;
         //   uint defPhaseActive = 4474;
         //   uint defNeutralActive = 3154;
         //   uint defPhaseReactive = 4474;
         //   uint defNeutralReactive = 3154;
         //   uint defPhaseDelay = 0;//     
         //   uint defNeutralDelay = 0;//     
         //   string strlogdata = "\nParam      Default        Max         Min             Actual";


         //   uint VoltageMin = defVoltage - (uint)(defVoltage * VPER / 100.0);
         //   uint PhaseCurrentMin = defPhaseCurrent - (uint)(defPhaseCurrent * PHCUR / 100.0);
         //   uint NeutralCurrentMin = defNeutralCurrent - (uint)(defNeutralCurrent * NCUR / 100.0);
         //   uint PhaseActiveMin = defPhaseActive - (uint)(defPhaseActive * PHACT / 100.0);
         //   uint NeutralActiveMin = defNeutralActive - (uint)(defNeutralActive * NACT / 100.0);
         //   uint PhaseReactiveMin = defPhaseReactive - (uint)(defPhaseReactive * PHREACT / 100.0);
         //   uint NeutralReactiveMin = defNeutralReactive - (uint)(defNeutralReactive * NREACT / 100.0);
         //   uint PhaseDelayMin = 1;//PhaseDelay - (uint)(PhaseDelay * PDELAY / 100.0);
         //   uint NeutralDelayMin = 1;//NeutralDelay - (uint)(NeutralDelay * NDELAY / 100.0);

         //   uint VoltageMax = defVoltage + (uint)(defVoltage * VPER / 100.0);
         //   uint PhaseCurrentMax = defPhaseCurrent + (uint)(defPhaseCurrent * PHCUR / 100.0);
         //   uint NeutralCurrentMax = defNeutralCurrent + (uint)(defNeutralCurrent * NCUR / 100.0);
         //   uint PhaseActiveMax = defPhaseActive + (uint)(defPhaseActive * PHACT / 100.0);
         //   uint NeutralActiveMax = defNeutralActive + (uint)(defNeutralActive * NACT / 100.0);
         //   uint PhaseReactiveMax = defPhaseReactive + (uint)(defPhaseReactive * PHREACT / 100.0);
         //   uint NeutralReactiveMax = defNeutralReactive + (uint)(defNeutralReactive * NREACT / 100.0);
         //   uint PhaseDelayMax = 417;//PhaseDelay + (uint)(PhaseDelay * PDELAY / 100.0);
         //   uint NeutralDelayMax = 417;// NeutralDelay + (uint)(NeutralDelay * NDELAY / 100.0);

            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM310MeterObject[Constants.DLMSConstants.DLMSCommand.SM310CALIBVERIFY].mclobisatt);

            // Access Selector
            cosempkt.Add(0x00);

            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                {

                    return _result;
                }



                _result = _dlmsmanager.NormalGetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALGET);

                if (_dlmsmanager.serialcom.ReceiveBuffer[17] == 0 && _result == GlobalConstants.Result.Pass)
                {
                    string[] result=IsValidCalibrationData_3Phase(_dlmsmanager.serialcom.ReceiveBuffer);
                    if((Int32.Parse(result[0])) <=0)
                    {
                        _result = Constants.GlobalConstants.Result.Fail;
                        Logger.Logger.WriteCalibLog(">> Uncalibrated Meter "+ result[1], mpos);
                        return _result;
                    }


                }
                else
                {
                    _result = factoryErrorCode.GetErrorObject(_dlmsmanager.serialcom.ReceiveBuffer[17]);

                    Logger.Logger.WriteCalibLog(">> Failed By Meter " + _result.ToString(), mpos);
                }
            }
            catch (Exception ex)
            {
                _result = Constants.GlobalConstants.Result.Fail;
                Logger.Logger.WriteCalibLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + "->"
                                       + System.Reflection.MethodBase.GetCurrentMethod().Name + "->" + ex.Message, mpos);

            }
            finally
            {
                // disconnect the connection
                _dlmsmanager.DisConnect();
            }

            return _result;
        }
        #endregion

        private string[] IsValidCalibrationData_3Phase(byte[] Blockdata)
        {
            string resultResponse = "";
            string[] resultdata = new string[2]; 
            try
            {

                List<string> parsedDataList = new List<string>();
                int startDataindx = 18;
                string[] datavalue = new string[2];
                int textValIndex = 0;
                int paraCode = 0;
                byte Resultflag = 0;
                if (Blockdata[startDataindx++] == 0x02) //srtact
                {
                    int lengthodstruct = Blockdata[startDataindx++];//length of stract
                    int objCnt = 0;
                    while (objCnt < lengthodstruct)
                    {
                        parsedDataList = new List<string>();
                        startDataindx++; //------Coff Structure
                        int coffStructLen = Blockdata[startDataindx++]; //-----Coff Len
                        startDataindx++; //-----Para data type
                        paraCode = Blockdata[startDataindx++];//------Para Code
                        textValIndex = 0;

                        int byteIndex = startDataindx;
                        //------------------------R Phase Value--------------------
                        datavalue = basehdlc.DLMSDataFormator(Blockdata, byteIndex, false);
                        if (datavalue == null) { parsedDataList.Add(""); objCnt++; continue; }
                        string data = datavalue[0];
                        parsedDataList.Add(data);
                        textValIndex++;
                        byteIndex = Convert.ToInt32(datavalue[1]);
                        //------------------------Y Phase Value--------------------
                        if (coffStructLen == 2) break;
                        datavalue = basehdlc.DLMSDataFormator(Blockdata, byteIndex, false);
                        if (datavalue == null) { parsedDataList.Add(""); objCnt++; continue; }
                        data = datavalue[0];
                        parsedDataList.Add(data);
                        textValIndex++;
                        byteIndex = Convert.ToInt32(datavalue[1]);
                        //------------------------B Phase Value--------------------
                        datavalue = basehdlc.DLMSDataFormator(Blockdata, byteIndex, false);
                        if (datavalue == null) { parsedDataList.Add(""); objCnt++; continue; }
                        data = datavalue[0];
                        parsedDataList.Add(data);
                        textValIndex++;
                        startDataindx = Convert.ToInt32(datavalue[1]);
                        objCnt++;



                        if (paraCode == 1)
                        {
                            string tempRes = "Voltage R =" + parsedDataList[0] + ",Y=" + parsedDataList[1] + ",B=" + parsedDataList[2];
                            string[] Distvalue = parsedDataList.Distinct().ToArray();
                            //if (Distvalue.Length <= 1 && (parsedDataList[0] == "26900" || parsedDataList[0] == "27006")) resultResponse = StaticVariables.ERRORPreFix + tempRes;
                            if (Distvalue.Length <= 1 && (SmartMeter3PSMCalibrationCofficient[0].Contains(parsedDataList[0]))) resultResponse =  tempRes;
                            else { resultResponse = tempRes; Resultflag++; }
                        }
                        if (paraCode == 2)
                        {
                            string tempRes = ", Current R=" + parsedDataList[0] + ",Y=" + parsedDataList[1] + ",B=" + parsedDataList[2];
                            string[] Distvalue = parsedDataList.Distinct().ToArray();
                            // if (Distvalue.Length <= 1 && (parsedDataList[0] == "27100" || parsedDataList[0] == "4434")) resultResponse += StaticVariables.ERRORPreFix + tempRes;
                            if (Distvalue.Length <= 1 && (SmartMeter3PSMCalibrationCofficient[1].Contains(parsedDataList[0]))) resultResponse +=  tempRes;
                            else { resultResponse += tempRes; Resultflag++; }
                        }
                        if (paraCode == 3)
                        {
                            string tempRes = ", Active Power R=" + parsedDataList[0] + ",Y=" + parsedDataList[1] + ",B=" + parsedDataList[2];
                            string[] Distvalue = parsedDataList.Distinct().ToArray();
                            // if (Distvalue.Length <= 1 && (parsedDataList[0] == "45500" || parsedDataList[0] == "7481")) resultResponse += StaticVariables.ERRORPreFix + tempRes;
                            if (Distvalue.Length <= 1 && (SmartMeter3PSMCalibrationCofficient[2].Contains(parsedDataList[0]))) resultResponse +=  tempRes;
                            else { resultResponse += tempRes; Resultflag++; }
                        }
                        if (paraCode == 4)
                        {
                            string tempRes = ", ReActive Power R=" + parsedDataList[0] + ",Y=" + parsedDataList[1] + ",B=" + parsedDataList[2];
                            string[] Distvalue = parsedDataList.Distinct().ToArray();
                            //if (Distvalue.Length <= 1 && (parsedDataList[0] == "6900" || parsedDataList[0] == "1135")) resultResponse += StaticVariables.ERRORPreFix + tempRes;
                            if (Distvalue.Length <= 1 && (SmartMeter3PSMCalibrationCofficient[3].Contains(parsedDataList[0]))) resultResponse +=  tempRes;
                            else { resultResponse += tempRes; Resultflag++; }
                        }
                    }

                }
                resultdata[0] = Resultflag.ToString();
                resultdata[1] = resultResponse;
                return  resultdata;
               
            }
            catch (Exception ex)
            {
                resultdata[1] = ex.ToString() + resultdata[1];
                return resultdata;

            }



        }

        public static List<List<string>> SmartMeter3PSMCalibrationCofficient = new List<List<string>>
        {
            //---------------Default Voltage Cofficient List---------------------- 
             new List<string>{"26836",//--SmartMeterWCM05-30
                              "27006",//--SmartMeterLTCT05-10, 05-06
                              "26900"},//--SmartMeterWCM10-60, 10-40 and 20-100
             //---------------Default Current Cofficient List--------------------- 
             new List<string>{"22222",//--SmartMeterWCM 20MHz 10-60, 10-40
                              "27453",//--SmartMeterWCM 24MHz 10-60, 10-40,20-100
                              "27100",//--SmartMeterWCM 20MHz 20-100
                              "11122",//--SmartMeterWCM 20 & 24 MHz 05-30
                              "4434",//--SmartMeterLTCT20Mhz 05-10, 05-06
                              "4464"},//--SmartMeterLTCT24Mhz 05-10, 05-06
              //---------------Default Active Power Cofficient List----------------
             new List<string>{"37350",//--SmartMeterWCM 20MHz 10-60, 10-40
                              "46121",//--SmartMeterWCM 24MHz 10-60, 10-40, 20-100
                              "45500",//--SmartMeterWCM 20MHz 20-100
                              "18651",//--SmartMeterWCM 20 & 24 MHz 05-30
                              "7481",//--SmartMeterLTCT20Mhz 05-10, 05-06
                              "7530"},//--SmartMeterLTCT24Mhz 05-10, 05-06
               //---------------Default Reactive Power Cofficient List--------------- 
             new List<string>{"5670",//--SmartMeterWCM 20MHz 10-60, 10-40
                              "14000",//--SmartMeterWCM 24MHz 10-60, 10-40,20-100
                              "6900",//--SmartMeterWCM 20MHz 20-100
                              "2843",//--SmartMeterWCM 20 & 24 MHz 05-30
                              "1135",//--SmartMeterLTCT20Mhz 05-10, 05-06
                              "2283"},//--SmartMeterLTCT24Mhz 05-10, 05-06
            
        };
        public override Constants.GlobalConstants.Result CONFIGURE(int mpos, int mAct)
        {
            // TODO : Variables declaration and initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            List<byte> cosempkt = new List<byte>();
            string[] strarr = (string[])mpArg;
            string mytagname = "";
            int MAXPACKETSIZE = Constants.DLMSConstants.PacketSize + 1;
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);

            if (strarr == null || strarr.Count() < 1)
            {
                Logger.Logger.WriteCalibLog("No config data found", serialcomm.mpos);
                return Constants.GlobalConstants.Result.Fail;
            }

            if (!strarr[0].Contains("Info"))
            {
                Logger.Logger.WriteCalibLog("Invalid tag found", serialcomm.mpos);
                return Constants.GlobalConstants.Result.Fail;
            }

            if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass) return Constants.GlobalConstants.Result.Fail;

            try
            {
                for (int i = 0; i < strarr.Count(); i++)
                {
                    string strtemp = (string)strarr[i];
                    string[] strdataArr = strtemp.Split(',');
                    mytagname = strdataArr[0].Replace("<", "");
                    mytagname = mytagname.Replace(">", "");
                    if (strdataArr[0].Contains("Info")) continue;
                    if (strdataArr[0].Contains("/END")) continue;
                    string strdata = strdataArr[1];
                    serialcomm.CommandTimeout = 35000;

                    // For these tags, delay should be at least 15 Sec
                    if (mytagname.Contains("SurveyIP") || mytagname.Contains("LSCaptureobject") || mytagname.Contains("MeteringMode") || mytagname.Contains("TamperComp") || mytagname.Contains("FactoryConfigurabelity")) serialcomm.CommandTimeout = Constants.DLMSConstants.MAXTIMEOUT;

                    // check size of the command
                    if (strdata.Length < (2 * MAXPACKETSIZE) || mytagname.Contains("AutoScroll") || mytagname.Contains("PushButton")
                    || mytagname.Contains("FactoryConfigurabelity")
                    )
                    {
                        _result = _dlmsmanager.SetWritePacket(strdata);
                    }
                    else
                    {
                        _result = _dlmsmanager.WriteBlock(strarr[i]);
                    }

                    Thread.Sleep(300);

                    if (_result == Constants.GlobalConstants.Result.Fail)
                    {
                        Logger.Logger.WriteCalibLog(mytagname + "-> Fail", serialcomm.mpos);
                        break;
                    }

                    Logger.Logger.WriteCalibLog(mytagname + "-> Pass", serialcomm.mpos);
                }


            }
            catch (Exception ex)
            {
                _result = Constants.GlobalConstants.Result.Fail;
                Logger.Logger.WriteCalibLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + "->"
                                       + System.Reflection.MethodBase.GetCurrentMethod().Name + "->" + ex.Message, mpos);
            }
            finally
            {
                _dlmsmanager.DisConnect();
            }

            return _result;
        }

        public override Constants.GlobalConstants.Result GETCONFIGURE(int mpos, int mAct)
        {
            // TODO : Variables declaration and initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            List<byte> cosempkt = new List<byte>();
            string[] strarr = (string[])mpArg;
            string mytagname = "";
            
            int MAXPACKETSIZE = Constants.DLMSConstants.PacketSize + 1;
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);

            if (strarr == null || strarr.Count() < 1)
            {
                Logger.Logger.WriteCalibLog("No config data found", serialcomm.mpos);
                return Constants.GlobalConstants.Result.Fail;
            }

            if (!strarr[0].Contains("Info"))
            {
                Logger.Logger.WriteCalibLog("Invalid tag found", serialcomm.mpos);
                return Constants.GlobalConstants.Result.Fail;
            }

            if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass) return Constants.GlobalConstants.Result.Fail;

            try
            {
                for (int i = 0; i < strarr.Count(); i++)
                {
                    cosempkt = new List<byte>();
                    string strtemp = (string)strarr[i];
                    string[] strdataArr = strtemp.Split(',');
                    mytagname = strdataArr[0].Replace("<", "");
                    mytagname = mytagname.Replace(">", "");
                    if (strdataArr[0].Contains("Info")) continue;
                    if (strdataArr[0].Contains("/END")) continue;
                    string strdata = strdataArr[1];
                    //serialcomm.CommandTimeout = 35000;
                    cosempkt.Add(0xC0);
                    cosempkt.Add(0x01);
                    cosempkt.Add(0xC1);
                    cosempkt.Add(0x00);
                    string CommandBytes = strdata.Substring(0, 18);
                    List<byte> tempBytes = GenericMethods.ConvertStrToByte(CommandBytes);
                    cosempkt.AddRange(tempBytes);
                    // check size of the command
                    if (strdata.Length < (2 * MAXPACKETSIZE) || mytagname.Contains("AutoScroll") || mytagname.Contains("PushButton")
                    || mytagname.Contains("FactoryConfigurabelity")
                    )
                    {
                        _result = _dlmsmanager.NormalGetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALGET);
                        if (_result == Constants.GlobalConstants.Result.Pass)
                        {
                            byte[] bdata = new byte[_dlmsmanager.serialcom.bufferIndex - (18 + 3)];
                            Array.Copy(_dlmsmanager.ValidData.ToArray(),18, bdata,0, bdata.Length );
                            CommandBytes = CommandBytes +  GenericMethods.GetByteToHexString(bdata);
                            if(mytagname == "DispTimeout") CommandBytes = CommandBytes.Substring(0, CommandBytes.Length - 10) + strdata.Substring(46, 4) + CommandBytes.Substring(CommandBytes.Length - 6, 6);
                            if (CommandBytes != strdata)
                            {
                                Logger.Logger.WriteCalibLog(mytagname + "-> Not Matched ->Fail : " + CommandBytes, serialcomm.mpos);
                                _result = Constants.GlobalConstants.Result.Fail;
                            }
                        }
                    }
                    else
                    {
                        
                        _result = _dlmsmanager.BlockGetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALGET);
                         if (_result == Constants.GlobalConstants.Result.Pass)
                        {
                            int endIDX = ((strdata.Length - 22) / 2);  
                            if (_dlmsmanager.serialcom.BlockBuffer[0] == 0x09) endIDX -= 2;
                            byte[] bdata = new byte[endIDX];
                            Array.Copy(_dlmsmanager.serialcom.BlockBuffer,0, bdata,0, bdata.Length );
                            CommandBytes = GenericMethods.GetByteToHexString(bdata);
                            if (!strdata.Contains(CommandBytes))
                            {
                                Logger.Logger.WriteCalibLog(mytagname + "-> Not Matched ->Fail : " + CommandBytes, serialcomm.mpos);
                                _result = Constants.GlobalConstants.Result.Fail;
                            }
                        }
                    }

                    Thread.Sleep(300);

                    if (_result == Constants.GlobalConstants.Result.Fail)
                    {
                        Logger.Logger.WriteCalibLog(mytagname + "-> Fail", serialcomm.mpos);
                        break;
                    }

                    Logger.Logger.WriteCalibLog(mytagname + "-> Pass", serialcomm.mpos);
                }


            }
            catch (Exception ex)
            {
                _result = Constants.GlobalConstants.Result.Fail;
                Logger.Logger.WriteCalibLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + "->"
                                       + System.Reflection.MethodBase.GetCurrentMethod().Name + "->" + ex.Message, mpos);
            }
            finally
            {
                _dlmsmanager.DisConnect();
            }

            return _result;
        }

    }
}
