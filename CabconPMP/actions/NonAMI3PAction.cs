using LNG.Communication.SerialCommunication;
using SmartCalibration.Communication;
using SmartCalibration.Constants;
using SmartCalibration.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCalibration.Actions
{
    public class NonAMI3PAction : GenericAction
    {
        // AT PRESENT MAX PARAM COUNT IS 8 AS ON 07-FEB-2020
        byte MAXPARAMCOUNT = 8;
        bool isoutofrange = false;
        List<UInt32> BenchData = new List<uint>();
        // EIGHT TYPE OF PARAM TO CALIBRATE IN SM310
        enum NONAMICALIBCOMMAND
        {
            VOLTAGE = 1,
            CURRENT = 2,
            ACTIVE = 3,
            REACTIVE = 4,
            VOLTAGECOEFF = 6,
            CURRENTCOEFF = 7,
            ACTPOWERCOEFF = 8,
            REACTPOWERCOEFF = 9,
            FVOLTAGE,
            FCURRENT,
            NEUTRAL = 16,
            TEMPERATURE = 20,
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

            BenchData = new List<uint>()
            {
                DataLayer.BenchData.rvoltage,
                DataLayer.BenchData.yvoltage,
                DataLayer.BenchData.bvoltage,
                DataLayer.BenchData.rcurrent,
                DataLayer.BenchData.ycurrent,
                DataLayer.BenchData.bcurrent,
                DataLayer.BenchData.ractivepower,
                DataLayer.BenchData.yactivepower,
                DataLayer.BenchData.bactivepower,
            

            };

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
            BenchData = new List<uint>()
            {
                DataLayer.BenchData.rreactivepower,
                DataLayer.BenchData.yreactivepower,
                DataLayer.BenchData.breactivepower,

            };
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
            BenchData = new List<uint>()
            {
                DataLayer.BenchData.rcurrent,
          
            };
            mAct = 16;

            if (CalibInitialise(mpos, mAct) != GlobalConstants.Result.Pass)
                return _result;

            System.Threading.Thread.Sleep(GlobalConstants.SM310_MAXSAMPLECOLLECTIONTIME);

            return CalibrateNeutral(mpos, mAct);
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
        /// TEMPERATURE - 20
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

                //_dlmsmanager.ValidData = new List<byte>();

                //// Active
                //_dlmsmanager.ValidData.AddRange(new byte[] {

                //     0x7E ,0xA0 ,0x4B ,0x81 ,0x00 ,0x02 ,0x04 ,0x01 ,0x52 ,0x34 ,0x7C ,0xE6 ,0xE7 ,0x00 ,0xC4 ,0x01
                //     ,0xC1 ,0x00 ,0x09 ,0x36 ,0x01 ,0x02 ,0x03 ,0x06 ,0x00 ,0x00 ,0x0F ,0xC3 ,0x06 ,0x00 ,0x00 ,0x0F
                //     ,0xBC ,0x06 ,0x00 ,0x00 ,0x0F ,0xBE ,0x02 ,0x02 ,0x03 ,0x06 ,0x00 ,0x00 ,0x00 ,0x00 ,0x06 ,0x00
                //     ,0x00 ,0x13 ,0xAF ,0x06 ,0x00 ,0x00 ,0x13 ,0xDF ,0x03 ,0x02 ,0x03 ,0x06 ,0x00 ,0x00 ,0x00 ,0x00
                //     ,0x06 ,0x00 ,0x01 ,0x17 ,0x27 ,0x06 ,0x00 ,0x01 ,0x19 ,0xE8 ,0x77 ,0x7E ,0x7E
                //});

                // Reactive
                /*_dlmsmanager.ValidData.AddRange(new byte[] {

                    0x7E, 0xA0 , 0x27 , 0x81 , 0x00 , 0x02 , 0x04 , 0x01 , 0x74 , 0x1F , 0x2D , 0xE6 , 0xE7 , 0x00 , 0xC4 , 0x01
 , 0xC1 , 0x00 , 0x09 , 0x12 , 0x04 , 0x02 , 0x03 , 0x06 , 0x00 , 0x02 , 0xA2 , 0x67 , 0x06 , 0x00 , 0x02 , 0xCE
 , 0x79 , 0x06 , 0x00 , 0x02 , 0xD5 , 0xB3 , 0x48 , 0xC2 , 0x7E
                });*/



                // Updated Log on 22 feb 2018
                /*
                 7E A0 66 FD 00 02 04 01 96 1A 7A E6 E7 00 C4 01   ~ •ý....–.zæç.Ä.
                 C1 00 02 24
                 01
                 02 03
                 12 00 00
                 12 00 00
                 12 00 00
                 02
                 02 03
                 12 00 00
                 12 00 00
                 12 00 00
                 03
                 02 03
                 12 00 00
                 12 00 00
                 12 00 00
                 A1 4D 7E 

                */

                Logger.Logger.WriteCalibLog("Get Sample Command - > Pass", mpos);

                List<UInt32> meterparams = new List<UInt32>();

                startindex = 20;
                                
                while (startindex < _dlmsmanager.ValidData[2] - 1)
                {
                    if(startindex == 20 || startindex == 38 || startindex == 56)
                        meterparams.Add(_dlmsmanager.ValidData[startindex++]);

                    
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

                if (meterparams == null || meterparams.Count < 1)
                    return _result;

                cosempkt.Clear();
                cosempkt.AddRange(Constants.DLMSConstants.GlobalSM310MeterObject[Constants.DLMSConstants.DLMSCommand.SM310SETCALIBRATE].mclobisatt);
                cosempkt.Add(0x00);
                cosempkt.AddRange(GetCosemCoefficient(meterparams, mpos));

                if (isoutofrange)
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
            
          
            try
            {
                
                // Fill Voltage Samples - R,Y,B
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.STRUCT);
                cosempkt.Add((byte)(meterparams.Count*3)); // 36

                for(int icount = 0, ibenchcount=0; icount < meterparams.Count; icount++)
                {
                    byte commandindex = (byte)meterparams[icount];
                    cosempkt.Add((byte)(meterparams[icount++] + 5));
                    cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.STRUCT);
                    cosempkt.Add(0x03);
                    cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                    cosempkt.AddRange(GetCalculatedCoeff(BenchData[ibenchcount++], meterparams[icount], commandindex, mpos)); icount++;
                    cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                    cosempkt.AddRange(GetCalculatedCoeff(BenchData[ibenchcount++], meterparams[icount], commandindex, mpos)); icount++;
                    cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                    cosempkt.AddRange(GetCalculatedCoeff(BenchData[ibenchcount++], meterparams[icount], commandindex, mpos)); 
                }


            }
            catch (Exception ex)
            {
                Logger.Logger.WriteCalibLog("SM310Action -> GetCosemCoefficient > Parser Error", mpos);
            }

            return cosempkt;
        }

        #endregion

        #region CalibrateNeutral
        /// <summary>
        /// Method Name: INIT
        /// Description: Reset Meter Data
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        private Constants.GlobalConstants.Result CalibrateNeutral(int mpos, int mAct)
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

                //_dlmsmanager.ValidData = new List<byte>();

                // Active
                /*_dlmsmanager.ValidData.AddRange(new byte[] {

                    0x7E ,0xA0 ,75 ,0xFD ,0x00 ,0x02 ,0x04 ,0x01 ,0x96 ,0x1A ,0x7A ,0xE6 ,0xE7 ,0x00 ,0xC4 ,0x01
                    ,0xC1 ,0x00 ,0x02 ,0x24
                     ,0x01
                     ,0x02 ,0x03
                     ,0x06 ,0x00 ,0x00, 0x00, 0x00
                     ,0x06 ,0x00 ,0x00, 0x00, 0x00
                     ,0x06 ,0x00 ,0x00, 0x00, 0x00
                     ,0x02
                     ,0x02 ,0x03
                     ,0x06 ,0x00 ,0x00, 0x00, 0x00
                     ,0x06 ,0x00 ,0x00, 0x00, 0x00
                     ,0x06 ,0x00 ,0x00, 0x00, 0x00
                     ,0x03
                     ,0x02 ,0x03
                     ,0x06 ,0x00 ,0x00, 0x00, 0x00
                     ,0x06 ,0x00 ,0x00, 0x00, 0x00
                     ,0x06 ,0x00 ,0x00, 0x00, 0x00
                     ,0xA1 ,0x4D ,0x7E
                });*/

                // Reactive
                //_dlmsmanager.ValidData.AddRange(new byte[] {

                //    0x7E ,0xA0 ,0x1B ,0x81 ,0x00 ,0x02 ,0x04 ,0x01 ,0x74 ,0xCF ,0x4A ,0xE6 ,0xE7 ,0x00 ,0xC4 ,0x01
                //    ,0xC1 ,0x00 ,0x09 ,0x06 ,0x0B ,0x02 ,0x01 ,0x12 ,0x07 ,0x34 ,0xD1 ,0x26 ,0x7E
                //});
                

              
                Logger.Logger.WriteCalibLog("Get Sample Command - > Pass", mpos);

                List<UInt32> meterparams = new List<UInt32>();

                startindex = 20;

                while (startindex < _dlmsmanager.ValidData[2] - 1)
                {
                    if (startindex == 20 )
                        meterparams.Add(_dlmsmanager.ValidData[startindex++]);


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

                if (meterparams == null || meterparams.Count < 1)
                    return _result;

                cosempkt.Clear();
                cosempkt.AddRange(Constants.DLMSConstants.GlobalSM310MeterObject[Constants.DLMSConstants.DLMSCommand.SM310SETCALIBRATE].mclobisatt);
                cosempkt.Add(0x00);
                cosempkt.AddRange(GetCosemNeutralCoefficient(meterparams, mpos));

                if (isoutofrange)
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

        #region GetCosemNeutralCoefficient
        private List<byte> GetCosemNeutralCoefficient(List<UInt32> meterparams, int mpos)
        {
            List<byte> cosempkt = new List<byte>();


            try
            {

                // Fill Neutral Val
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.STRUCT);
                cosempkt.Add((byte)(meterparams.Count * 3)); // 36
                byte commandindex = (byte)meterparams[0];
                cosempkt.Add(0x0c);
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.STRUCT);
                cosempkt.Add(0x01);
                cosempkt.Add((byte)DLMSConstants.DLMSDATATYPE.LONGUNSIGNED);
                cosempkt.AddRange(GetCalculatedCoeff(BenchData[0], meterparams[1], 5, mpos)); 
                   
            }
            catch (Exception ex)
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
        private byte[] GetCalculatedCoeff(UInt32 benchvalue, UInt32 metercommvalue, uint commandname, int mpos)
        {
            const int IDEVFACTOR = 4096;
            UInt16 uiival = 0;
            byte[] coeffdata = null;
            List<byte> cosempkt = new List<byte>();
            const UInt32 MAXPARAMLIMIT = 60000;
            const UInt32 MINPARAMLIMIT = 1;

            Dictionary<uint, string> diccommandname = new Dictionary<uint, string>()
            {
                { 1, "VOLTAGE"},
                { 2, "CURRENT"},
                { 3, "ACTIVE"},
                { 4, "REACTIVE"},
                { 5, "NEUTRAL"},

            };


            string strlog = diccommandname[commandname] + " Coeffcient -> Status -> ";

           
            if (metercommvalue < MINPARAMLIMIT)
            {
                isoutofrange = true;
                strlog += " Out of range -> MeterComValue ->" + metercommvalue.ToString();
                Logger.Logger.WriteCalibLog(strlog, mpos);
                coeffdata = new byte[] { 0, 0 };
                return coeffdata;
            }

            try
            {
                if (metercommvalue > 0)
                {
                    if (commandname == 20)
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
            catch (Exception ex)
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
            //cosempkt.Add(0x00);
            //cosempkt.Add(0x12);
            //cosempkt.Add(0x00);
            //cosempkt.Add(0x01);

            cosempkt.Add(0x01);
            cosempkt.Add(0x09);
            cosempkt.Add(0x01);
            cosempkt.Add(0x00);


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
            const double MINVAL = 1.85;
            const double MAXVAL = 2.25;




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


                if (neutralval > MAXVAL || neutralval < MINVAL)
                {
                    strTime += " -> Neutral Current: Fail \n";
                    meterdetail = strTime;
                    return Constants.GlobalConstants.Result.OUT;
                }


                strTime += " -> Neutral Current: " + (val / idev).ToString() + "." + (val % idev).ToString("d3") + " A \n";

                meterdetail = strTime;

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

        #region SETCLOCK
        /// <summary>
        /// Method Name: SetClock
        /// Description: Update RTC of the meter
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result SETCLOCK(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            List<byte> RTCbyte = new List<byte>();
            DateTime setdatetime = System.DateTime.Now;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);

            RTCbyte.AddRange(Constants.DLMSConstants.GlobalGenericMeterObject[Constants.DLMSConstants.DLMSCommand.RTCSET].mclobisatt);
            // Prepare RTC data list
            RTCbyte.Add(0x0);
            RTCbyte.Add(0x9);
            RTCbyte.Add(0xc);
            RTCbyte.Add(Convert.ToByte((setdatetime.Year & 0xFF00) >> 8));
            RTCbyte.Add(Convert.ToByte(setdatetime.Year & 0x00FF));

            RTCbyte.Add(Convert.ToByte(setdatetime.Month));
            RTCbyte.Add(Convert.ToByte(setdatetime.Day));

            // Need to check

            RTCbyte.Add(0xFF);

            RTCbyte.Add(Convert.ToByte(setdatetime.Hour));
            RTCbyte.Add(Convert.ToByte(setdatetime.Minute));
            RTCbyte.Add(Convert.ToByte(setdatetime.Second));

            RTCbyte.Add(0xFF);
            RTCbyte.Add(0x80);
            RTCbyte.Add(0x00);
            RTCbyte.Add(0x00);

            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);

            Logger.Logger.WriteCalibLog("DateTime Value -> " + setdatetime, serialcomm.mpos);

            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                    return _result;

                if (_dlmsmanager.NormalSetPacket(RTCbyte, Constants.DLMSConstants.DLMSCommand.RTCSET) != Constants.GlobalConstants.Result.Pass)
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
    }
}

/*
 * Reset Calib
 *  7e a0 22 00 02 04 01 81 97 c3 e1 e6 e6 00 c1 01   
    c1 00 01 00 00 60 01 91 ff 02 00 02 02 11 01 11   
    0a 5b 4a 7e
 */

/*
 * Init Active Calib
 *  7e a0 22 00 02 04 01 81 99 bd 08 e6 e6 00 c1 01   ~.".............
    c1 00 01 00 00 60 01 91 ff 02 00 02 02 11 02 11   .....`..........
    0a 3f a5 7e 
 */

/*
* Read Active Samples
*   7e a0 1c 00 02 04 01 81 9b c4 7b e6 e6 00 c0 01   ~.........{.....
    c1 00 01 00 00 60 02 84 ff 02 00 c7 e2 7e
*/

/*
* Set Samples
*  7e a0 1c 00 02 04 01 81 9b c4 7b e6 e6 00 c0 01   ~.........{.....
   c1 00 01 00 00 60 02 84 ff 02 00 c7 e2 7e 7e a0   .....`.......~~.
   42 00 02 04 01 81 9d 1c ea e6 e6 00 c1 01 c1 00   B...............
   01 00 00 60 02 88 ff 02 00 02 24 06 02 03 12 00   ...`......$.....
   00 12 00 00 12 00 00 07 02 03 12 00 00 12 00 00   ................
   12 00 00 08 02 03 12 00 00 12 00 00 12 00 00 d7   ................
   e2 7e 
*/



