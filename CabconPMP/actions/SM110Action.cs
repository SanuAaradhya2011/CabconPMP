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
using static SmartCalibration.Constants.DLMSConstants;

namespace SmartCalibration.Actions
{
    /// <summary>
    /// This class is dedicated for the method for 1P Smart Meters to execute calibration
    /// </summary>
    public class SM110Action : GenericAction
    {

        #region CALIBPHASE
        /// <summary>
        /// Method Name: CALIBPHASE
        /// Description: Reset Meter Data
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result CALIBPHASE(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;

            _result = RESETPHASE(mpos, mAct);

            if (_result != Constants.GlobalConstants.Result.Pass)
                return _result;

            _result = INITPHASE(mpos, mAct);

            if (_result != Constants.GlobalConstants.Result.Pass)
                return _result;

            Thread.Sleep(GlobalConstants.SM110_MAXSAMPLECOLLECTIONTIME);

            _result = CalibratePhase(mpos, mAct);

            if (_result != Constants.GlobalConstants.Result.Pass)
                return _result;

            return _result;
        }
        #endregion

        #region CALIBNEUTRAL
        /// <summary>
        /// Method Name: CALIBPHASE
        /// Description: Reset Meter Data
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>   
        ///  Object thisLock = new Object();
        Object thisLock = new Object();
        public override Constants.GlobalConstants.Result CALIBNEUTRAL(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;

            _result = RESETNEUTRAL(mpos, mAct);

            if (_result != Constants.GlobalConstants.Result.Pass)
                return _result;

            _result = INITNEUTRAL(mpos, mAct);

            if (_result != Constants.GlobalConstants.Result.Pass)
                return _result;

            Thread.Sleep(GlobalConstants.SM110_MAXSAMPLECOLLECTIONTIME);

            _result = CalibrateNeutral(mpos, mAct);

            if (_result != Constants.GlobalConstants.Result.Pass)
                return _result;

            return _result;

        }
        #endregion

        #region RESETMETERDATA
        /// <summary>
        /// Method Name: RESETDATA
        /// Description: Reset Meter Data
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result RESETDATA(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTOTHERSCMD);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.DATARESET].mclobisatt);
            cosempkt.Add(0x01);
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

        #region RESETPHASE
        /// <summary>
        /// Method Name: RESETDATA
        /// Description: Reset Meter Data
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result RESETPHASE(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            byte datastartindex = 17;
            Logger.Logger.WriteCalibLog("method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name, mpos);
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110CALIB].mclobisatt);
            cosempkt.Add(0x00);
            cosempkt.Add(0x02);
            cosempkt.Add(0x06);
            cosempkt.AddRange(CalibCommandMapper(COMMANDCODE.RESETPHASE, mpos));


            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                {
                    Logger.Logger.WriteCalibLog("Meter Connection Failed", mpos);
                    return _result;
                }

                if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET) != Constants.GlobalConstants.Result.Pass)
                {
                    Logger.Logger.WriteCalibLog("Response Error", mpos);
                    _result = factoryErrorCode.GetErrorObject(datastartindex);
                    Constants.GlobalConstants.Result _tempresult = factoryErrorCode.GetErrorObject(_dlmsmanager.serialcom.ReceiveBuffer[17]);
                    Logger.Logger.WriteCalibLog(">> Failed By Meter " + _tempresult.ToString(), mpos);
                    return _result;
                }

                _result = Constants.GlobalConstants.Result.Pass;
                Logger.Logger.WriteCalibLog("Executed Successfully", mpos);

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

        #region INITPHASE
        /// <summary>
        /// Method Name: INITPHASE
        /// Description: Init Phase samples
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result INITPHASE(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Logger.Logger.WriteCalibLog("method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name, mpos);
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110CALIB].mclobisatt);
            cosempkt.Add(0x00);
            cosempkt.Add(0x02);
            cosempkt.Add(0x06);
            cosempkt.AddRange(CalibCommandMapper(COMMANDCODE.INITPHASE, mpos));


            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                {
                    Logger.Logger.WriteCalibLog("Meter Connection Failed", mpos);
                    return _result;
                }

                if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET) != Constants.GlobalConstants.Result.Pass)
                {
                    Constants.GlobalConstants.Result _tempresult = factoryErrorCode.GetErrorObject(_dlmsmanager.serialcom.ReceiveBuffer[17]);
                    Logger.Logger.WriteCalibLog(">> Failed By Meter " + _tempresult.ToString(), mpos);
                    return _result;
                }

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

        #region CalibratePhase
        /// <summary>
        /// Method Name: CalibratePhase
        /// Description: Calibrate Phase
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        private Constants.GlobalConstants.Result CalibratePhase(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Logger.Logger.WriteCalibLog("method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name, mpos);
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110CALIB].mclobisatt);
            cosempkt.Add(0x00);
            cosempkt.Add(0x02);
            cosempkt.Add(0x06);
            cosempkt.AddRange(CalibCommandMapper(COMMANDCODE.CALIBPHASE, mpos));


            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                {
                    Logger.Logger.WriteCalibLog("Meter Connection Failed", mpos);
                    return _result;
                }

                if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET) != Constants.GlobalConstants.Result.Pass)
                {
                    Constants.GlobalConstants.Result _tempresult = factoryErrorCode.GetErrorObject(_dlmsmanager.serialcom.ReceiveBuffer[17]);
                    Logger.Logger.WriteCalibLog(">> Failed By Meter " + _tempresult.ToString(), mpos);
                    return _result;
                }

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

        #region RESETNEUTRAL
        /// <summary>
        /// Method Name: RESETNEUTRAL
        /// Description: Reset Meter Data
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result RESETNEUTRAL(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Logger.Logger.WriteCalibLog("method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name, mpos);
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110CALIB].mclobisatt);
            cosempkt.Add(0x00);
            cosempkt.Add(0x02);
            cosempkt.Add(0x06);
            cosempkt.AddRange(CalibCommandMapper(COMMANDCODE.RESETNEUTRAL, mpos));


            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                {
                    return _result;
                }

                if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET) != Constants.GlobalConstants.Result.Pass)
                {
                    Constants.GlobalConstants.Result _tempresult = factoryErrorCode.GetErrorObject(_dlmsmanager.serialcom.ReceiveBuffer[17]);
                    Logger.Logger.WriteCalibLog(">> Failed By Meter " + _tempresult.ToString(), mpos);
                    return _result;
                }

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

        #region INITNEUTRAL
        /// <summary>
        /// Method Name: INITNEUTRAL
        /// Description: Init Neutral Samples
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result INITNEUTRAL(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Logger.Logger.WriteCalibLog("method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name, mpos);
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110CALIB].mclobisatt);
            cosempkt.Add(0x00);
            cosempkt.Add(0x02);
            cosempkt.Add(0x06);
            cosempkt.AddRange(CalibCommandMapper(COMMANDCODE.INITNEUTRAL, mpos));


            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                    return _result;

                if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET) != Constants.GlobalConstants.Result.Pass)
                {
                    Constants.GlobalConstants.Result _tempresult = factoryErrorCode.GetErrorObject(_dlmsmanager.serialcom.ReceiveBuffer[17]);
                    Logger.Logger.WriteCalibLog(">> Failed By Meter " + _tempresult.ToString(), mpos);
                    return _result;
                }

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

        #region CalibrateNeutral
        /// <summary>
        /// Method Name: CalibrateNeutral
        /// Description: Calibrate Neutral
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        private Constants.GlobalConstants.Result CalibrateNeutral(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Logger.Logger.WriteCalibLog("method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name, mpos);
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110CALIB].mclobisatt);
            cosempkt.Add(0x00);
            cosempkt.Add(0x02);
            cosempkt.Add(0x06);
            cosempkt.AddRange(CalibCommandMapper(COMMANDCODE.CALIBNEUTRAL, mpos));

            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                {
                    Logger.Logger.WriteCalibLog("Meter Connection Failed", mpos);
                    return _result;
                }

                if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET) != Constants.GlobalConstants.Result.Pass)
                {
                    Constants.GlobalConstants.Result _tempresult = factoryErrorCode.GetErrorObject(_dlmsmanager.serialcom.ReceiveBuffer[17]);
                    Logger.Logger.WriteCalibLog(">> Failed By Meter " + _tempresult.ToString(), mpos);
                    return _result;
                }

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

        #region CALIBTemperature
        /// <summary>
        /// Method Name: CALIBTEMP
        /// Description: Reset Meter Data
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result CALIBTEMP(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110CALIB].mclobisatt);
            cosempkt.Add(0x00);
            cosempkt.Add(0x02);
            cosempkt.Add(0x06);
            cosempkt.AddRange(CalibCommandMapper(COMMANDCODE.CALIBTEMP, mpos));

            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                {
                    Logger.Logger.WriteCalibLog("Meter Connection Failed", mpos);
                    return _result;
                }

                if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET) != Constants.GlobalConstants.Result.Pass)
                {
                    Constants.GlobalConstants.Result _tempresult = factoryErrorCode.GetErrorObject(_dlmsmanager.serialcom.ReceiveBuffer[17]);
                    Logger.Logger.WriteCalibLog(">> Failed By Meter " + _tempresult.ToString(), mpos);
                    return _result;
                }

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

        #region CALIBVERIFYPH
        /// <summary>
        /// Method Name: CALIBVERIFYPH
        /// Description: Verify Calibration 
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result CALIBVERIFYPH(int mpos, int mAct)
        {
            return CALIBVERIFY(mpos, mAct, true);
        }
        #endregion

        #region CALIBVERIFYN
        /// <summary>
        /// Method Name: CALIBVERIFYN
        /// Description: Verify Calibration 
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result CALIBVERIFYN(int mpos, int mAct)
        {
            return CALIBVERIFY(mpos, mAct, false);
        }
        #endregion

        #region CALIBVERIFY
        /// <summary>
        /// Method Name: CALIBVERIFY
        /// Description: Verify Calibration, Phase - true, Neutral - false
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary> 
        private Constants.GlobalConstants.Result CALIBVERIFY(int mpos, int mAct, bool isphase)
        {
            // TODO  : Variables declaration and Initialisation here
            /*
	 
	         7e a0 1c 00 02 04 01 fd 54 5b 1c e6 e6 00 c0 01   ~.......T[......
	         c1 00 01 00 00 60 02 88 ff 02 00 f3 75 7e         .....`......u~

	         7e a0 45 fd 00 02 04 01 74 d4 4a e6 e7 00 c4 01   ~.E.....t.J.....
	         c1 00 09 30 89 b0 00 00 e6 c0 00 00 66 0d 01 00   ...0........f...
	         82 11 00 00 8a 0c 00 00 82 11 00 00 8a 0c 00 00   ................
	         be 00 96 00 52 01 00 00 00 00 00 00 00 00 00 00   ....R...........
	         00 00 ab 17 a5 f6 7e                              ......
	
	        */

            /*                     Default   Min         Max      % of Default
            Voltage					45628	41065.2		50190.8	    10%
            Phase Current			49000	39200		58800	    20%
            Neutral Current			69488	62539.2		76436.8	    10%
            Phase Active Power		4474	3579.2		5368.8	    20%
            Neutral Active Power	3154	2838.6		3469.4	    10%
            Phase Reactive Power	4474	3579.2		5368.8	    20%
            Neutral Reactive Power	3154	2838.6		3469.4	    10%
            Phase Delay				0		1		    417	        
            Neutral Delay			0		1			417	         
            */
            byte VPER = 10;
            byte PHCUR = 20;
            byte NCUR = 10;
            byte PHACT = 20;
            byte NACT = 10;
            byte PHREACT = 20;
            byte NREACT = 10;

            uint defVoltage = 45628;
            uint defPhaseCurrent = 49000;
            uint defNeutralCurrent = 69488;
            uint defPhaseActive = 4474;
            uint defNeutralActive = 3154;
            uint defPhaseReactive = 4474;
            uint defNeutralReactive = 3154;
            uint defPhaseDelay = 0;//     
            uint defNeutralDelay = 0;//     
            string strlogdata = "\nParam      Default        Max         Min             Actual";


            uint VoltageMin = defVoltage - (uint)(defVoltage * VPER / 100.0);
            uint PhaseCurrentMin = defPhaseCurrent - (uint)(defPhaseCurrent * PHCUR / 100.0);
            uint NeutralCurrentMin = defNeutralCurrent - (uint)(defNeutralCurrent * NCUR / 100.0);
            uint PhaseActiveMin = defPhaseActive - (uint)(defPhaseActive * PHACT / 100.0);
            uint NeutralActiveMin = defNeutralActive - (uint)(defNeutralActive * NACT / 100.0);
            uint PhaseReactiveMin = defPhaseReactive - (uint)(defPhaseReactive * PHREACT / 100.0);
            uint NeutralReactiveMin = defNeutralReactive - (uint)(defNeutralReactive * NREACT / 100.0);
            uint PhaseDelayMin = 1;//PhaseDelay - (uint)(PhaseDelay * PDELAY / 100.0);
            uint NeutralDelayMin = 1;//NeutralDelay - (uint)(NeutralDelay * NDELAY / 100.0);

            uint VoltageMax = defVoltage + (uint)(defVoltage * VPER / 100.0);
            uint PhaseCurrentMax = defPhaseCurrent + (uint)(defPhaseCurrent * PHCUR / 100.0);
            uint NeutralCurrentMax = defNeutralCurrent + (uint)(defNeutralCurrent * NCUR / 100.0);
            uint PhaseActiveMax = defPhaseActive + (uint)(defPhaseActive * PHACT / 100.0);
            uint NeutralActiveMax = defNeutralActive + (uint)(defNeutralActive * NACT / 100.0);
            uint PhaseReactiveMax = defPhaseReactive + (uint)(defPhaseReactive * PHREACT / 100.0);
            uint NeutralReactiveMax = defNeutralReactive + (uint)(defNeutralReactive * NREACT / 100.0);
            uint PhaseDelayMax = 417;//PhaseDelay + (uint)(PhaseDelay * PDELAY / 100.0);
            uint NeutralDelayMax = 417;// NeutralDelay + (uint)(NeutralDelay * NDELAY / 100.0);

            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110CALIBVERIFY].mclobisatt);

            // Access Selector
            cosempkt.Add(0x00);

            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                {

                    return _result;
                }

                int index = 20;

                _result = _dlmsmanager.NormalGetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALGET);

                if (_dlmsmanager.serialcom.ReceiveBuffer[17] == 0 && _result == GlobalConstants.Result.Pass)
                {
                    Logger.Logger.WriteCalibLog(">> Received Data ", mpos);

                    byte[] u32data = new byte[4];
                    byte[] u16data = new byte[2];

                    Array.Copy(_dlmsmanager.serialcom.ReceiveBuffer, index, u32data, 0, 4);

                    UInt32 voltage = BitConverter.ToUInt32(u32data, 0); index += 4;

                    strlogdata += "\n Voltage         >> " + defVoltage.ToString() + "     " + VoltageMax.ToString() + "   " + VoltageMin.ToString() + "   " + voltage.ToString();

                    Array.Copy(_dlmsmanager.serialcom.ReceiveBuffer, index, u32data, 0, 4);

                    UInt32 phcurrent = BitConverter.ToUInt32(u32data, 0); index += 4;

                    strlogdata += "\n PhaseCurrent    >> " + defPhaseCurrent.ToString() + "     " + PhaseCurrentMax.ToString() + "   " + PhaseCurrentMin.ToString() + "   " + phcurrent.ToString();

                    Array.Copy(_dlmsmanager.serialcom.ReceiveBuffer, index, u32data, 0, 4);

                    UInt32 ncurrent = BitConverter.ToUInt32(u32data, 0); index += 4;

                    strlogdata += "\n NeutralCurrent  >> " + defNeutralCurrent.ToString() + "     " + NeutralCurrentMax.ToString() + "   " + NeutralCurrentMin.ToString() + "   " + ncurrent.ToString();

                    Array.Copy(_dlmsmanager.serialcom.ReceiveBuffer, index, u32data, 0, 4);

                    UInt32 phaseactive = BitConverter.ToUInt32(u32data, 0); index += 4;

                    strlogdata += "\n PhaseActive     >> " + defPhaseActive.ToString() + "     " + PhaseActiveMax.ToString() + "   " + PhaseActiveMin.ToString() + "   " + phaseactive.ToString();

                    Array.Copy(_dlmsmanager.serialcom.ReceiveBuffer, index, u32data, 0, 4);

                    UInt32 neutralactive = BitConverter.ToUInt32(u32data, 0); index += 4;

                    strlogdata += "\n NeutralActive   >> " + defNeutralActive.ToString() + "     " + NeutralActiveMax.ToString() + "   " + NeutralActiveMin.ToString() + "   " + neutralactive.ToString();

                    Array.Copy(_dlmsmanager.serialcom.ReceiveBuffer, index, u32data, 0, 4);

                    UInt32 phasereactive = BitConverter.ToUInt32(u32data, 0); index += 4;

                    strlogdata += "\n PhaseReactive   >> " + defPhaseReactive.ToString() + "     " + PhaseReactiveMax.ToString() + "   " + PhaseReactiveMin.ToString() + "   " + phasereactive.ToString();

                    Array.Copy(_dlmsmanager.serialcom.ReceiveBuffer, index, u32data, 0, 4);

                    UInt32 neutralreactive = BitConverter.ToUInt32(u32data, 0); index += 4;

                    strlogdata += "\n NeutralReactive >> " + defNeutralReactive.ToString() + "     " + NeutralReactiveMax.ToString() + "   " + NeutralReactiveMin.ToString() + "   " + neutralreactive.ToString();

                    Array.Copy(_dlmsmanager.serialcom.ReceiveBuffer, index, u16data, 0, 2);

                    UInt16 phvtoicorr = BitConverter.ToUInt16(u16data, 0); index += 2;

                    strlogdata += "\n PhaseDelay      >> " + defPhaseDelay.ToString() + "     " + PhaseDelayMax.ToString() + "   " + PhaseDelayMin.ToString() + "   " + phvtoicorr.ToString();

                    Array.Copy(_dlmsmanager.serialcom.ReceiveBuffer, index, u16data, 0, 2);

                    UInt16 neuvtoicorr = BitConverter.ToUInt16(u16data, 0); index += 2;

                    strlogdata += "\n NeutralDelay    >> " + defNeutralDelay.ToString() + "     " + NeutralDelayMax.ToString() + "   " + NeutralDelayMin.ToString() + "   " + neuvtoicorr.ToString();

                    Array.Copy(_dlmsmanager.serialcom.ReceiveBuffer, index, u16data, 0, 2);

                    UInt16 temperature = BitConverter.ToUInt16(u16data, 0);

                    //strlogdata += "\n Temperature  >> " + defte.ToString() + "     " + NeutralDelayMax.ToString() + "   " + NeutralDelayMin.ToString() + "   " + neuvtoicorr.ToString();

                    Logger.Logger.WriteCalibLog(strlogdata, mpos);

                    if (isphase == true)
                    {
                        Logger.Logger.WriteCalibLog(">> Inside Phase Verify ", mpos);

                        if (voltage == defVoltage && phcurrent == defPhaseCurrent
                            && phaseactive == defPhaseActive && phasereactive == defPhaseReactive
                            )
                        {
                            _result = Constants.GlobalConstants.Result.Fail;
                            Logger.Logger.WriteCalibLog(">> Phase Meter Not Calibrated", mpos);
                            return _result;
                        }

                        if (voltage > VoltageMax || voltage < VoltageMin)
                        {
                            _result = Constants.GlobalConstants.Result.OUT;
                            Logger.Logger.WriteCalibLog(">> Voltage OUT", mpos);
                        }
                        if (phcurrent > PhaseCurrentMax || phcurrent < PhaseCurrentMin)
                        {
                            _result = Constants.GlobalConstants.Result.OUT;
                            Logger.Logger.WriteCalibLog(">> Phase Current OUT", mpos);
                        }
                        if (phaseactive > PhaseActiveMax || phaseactive < PhaseActiveMin)
                        {
                            _result = Constants.GlobalConstants.Result.OUT;
                            Logger.Logger.WriteCalibLog(">> Phase Active OUT", mpos);
                        }
                        if (phasereactive > PhaseReactiveMax || phasereactive < PhaseReactiveMin)
                        {
                            _result = Constants.GlobalConstants.Result.OUT;
                            Logger.Logger.WriteCalibLog(">> Phase Reactive OUT", mpos);
                        }
                        if (phvtoicorr > PhaseDelayMax || phvtoicorr < PhaseDelayMin)
                        {
                            _result = Constants.GlobalConstants.Result.OUT;
                            Logger.Logger.WriteCalibLog(">> Phase Delay OUT/Not Calibrated", mpos);
                        }

                    }
                    else
                    {
                        Logger.Logger.WriteCalibLog(">> Inside Neutral Verify ", mpos);

                        if (ncurrent == defNeutralCurrent && neutralactive == defNeutralActive && neutralreactive == defNeutralReactive)
                        {
                            _result = Constants.GlobalConstants.Result.Fail;
                            Logger.Logger.WriteCalibLog(">> Neutral Meter Not Calibrated", mpos);
                            return _result;
                        }

                        if (ncurrent > NeutralCurrentMax || ncurrent < NeutralCurrentMin)
                        {
                            _result = Constants.GlobalConstants.Result.OUT;
                            Logger.Logger.WriteCalibLog(">> Neutral Current OUT", mpos);
                        }
                        if (neutralactive > NeutralActiveMax || neutralactive < NeutralActiveMin)
                        {
                            _result = Constants.GlobalConstants.Result.OUT;
                            Logger.Logger.WriteCalibLog(">> Neutral Active OUT", mpos);
                        }
                        if (neutralreactive > NeutralReactiveMax || neutralreactive < NeutralReactiveMin)
                        {
                            _result = Constants.GlobalConstants.Result.OUT;
                            Logger.Logger.WriteCalibLog(">> Neutral Reactive OUT", mpos);
                        }
                        if (neuvtoicorr > NeutralDelayMax || neuvtoicorr < NeutralDelayMin)
                        {
                            _result = Constants.GlobalConstants.Result.OUT;
                            Logger.Logger.WriteCalibLog(">> Neutral Delay OUT/Not Calibrated", mpos);
                        }

                    }


                    return _result;
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

        #region WRITEDEFAULT
        /// <summary>
        /// Method Name: WRITEDEFAULT
        /// Description: Write default constant pahse and neutral both
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result WRITEDEFAULT(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Logger.Logger.WriteCalibLog("method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name, mpos);
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110CALIB].mclobisatt);
            cosempkt.Add(0x00);
            cosempkt.Add(0x02);
            cosempkt.Add(0x06);
            cosempkt.AddRange(CalibCommandMapper(COMMANDCODE.WRITEDEFAULTCALIB, mpos));


            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                {
                    Logger.Logger.WriteCalibLog("Meter Connection Failed", mpos);
                    return _result;
                }

                if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET) != Constants.GlobalConstants.Result.Pass)
                {
                    Constants.GlobalConstants.Result _tempresult = factoryErrorCode.GetErrorObject(_dlmsmanager.serialcom.ReceiveBuffer[17]);
                    Logger.Logger.WriteCalibLog(">> Failed By Meter " + _tempresult.ToString(), mpos);
                    return _result;
                }

                _result = Constants.GlobalConstants.Result.Pass;
                Logger.Logger.WriteCalibLog("Executed Successfully", mpos);

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

        #region WRITEDEFAULTPHASE
        /// <summary>
        /// Method Name: WRITEDEFAULTPHASE
        /// Description: Set default phase constant
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result WRITEDEFAULTPHASE(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Logger.Logger.WriteCalibLog("method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name, mpos);
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110CALIB].mclobisatt);
            cosempkt.Add(0x00);
            cosempkt.Add(0x02);
            cosempkt.Add(0x06);
            cosempkt.AddRange(CalibCommandMapper(COMMANDCODE.WRITEDEFAULTPHASE, mpos));


            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                {
                    Logger.Logger.WriteCalibLog("Meter Connection Failed", mpos);
                    return _result;
                }

                if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET) != Constants.GlobalConstants.Result.Pass)
                {
                    Constants.GlobalConstants.Result _tempresult = factoryErrorCode.GetErrorObject(_dlmsmanager.serialcom.ReceiveBuffer[17]);
                    Logger.Logger.WriteCalibLog(">> Failed By Meter " + _tempresult.ToString(), mpos);
                    return _result;
                }

                _result = Constants.GlobalConstants.Result.Pass;
                Logger.Logger.WriteCalibLog("Executed Successfully", mpos);

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

        #region WRITEDEFAULTNEUTRAL
        /// <summary>
        /// Method Name: WRITEDEFAULTNEUTRAL
        /// Description: Set default Neutral constant only
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result WRITEDEFAULTNEUTRAL(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Logger.Logger.WriteCalibLog("method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name, mpos);
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110CALIB].mclobisatt);
            cosempkt.Add(0x00);
            cosempkt.Add(0x02);
            cosempkt.Add(0x06);
            cosempkt.AddRange(CalibCommandMapper(COMMANDCODE.WRITEDEFUALTNEUTRAL, mpos));


            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                {
                    Logger.Logger.WriteCalibLog("Meter Connection Failed", mpos);
                    return _result;
                }

                if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET) != Constants.GlobalConstants.Result.Pass)
                {
                    Constants.GlobalConstants.Result _tempresult = factoryErrorCode.GetErrorObject(_dlmsmanager.serialcom.ReceiveBuffer[17]);
                    Logger.Logger.WriteCalibLog(">> Failed By Meter " + _tempresult.ToString(), mpos);
                    return _result;
                }

                _result = Constants.GlobalConstants.Result.Pass;
                Logger.Logger.WriteCalibLog("Executed Successfully", mpos);

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

        #region CommandMapper
        private List<byte> CalibCommandMapper(COMMANDCODE commandcode, int mpos)
        {
            List<byte> calibdata = new List<byte>();
            //m_temprature = mpArg as UInt16;
            UInt16 powerfactor = (UInt16)BenchData.rpowerfactor;
            try
            {
                Dictionary<COMMANDCODE, object[]> dictCalibCommandMapper = new Dictionary<COMMANDCODE, object[]>

                    {
                    // Command Code              // byte array
                    {
                        COMMANDCODE.RESETPHASE,
                        new object[]{0, 0, 0, 0, 0}
                    },

                    {
                         COMMANDCODE.RESETNEUTRAL,
                        new object[]{0, 0, 0, 0, 0}
                    },

                    {
                        COMMANDCODE.INITPHASE,
                        new object[]{Constants.GlobalConstants.MAXSM110SAMPLES, 0, 0, 0, 0}
                    },

                    {
                        COMMANDCODE.INITPHASELAG,
                        new object[]{ Constants.GlobalConstants.MAXSM110SAMPLES, 0, 0, 0, 0}
                    },

                    {
                        COMMANDCODE.INITNEUTRAL,
                        new object[]{ Constants.GlobalConstants.MAXSM110SAMPLES, 0, 0, 0, 0}
                    },
                    {
                        COMMANDCODE.INITNEUTRALLAG,
                        new object[]{ Constants.GlobalConstants.MAXSM110SAMPLES, 0, 0, 0, 0}
                    },
                     {
                        COMMANDCODE.CALIBPHASE,
                        new object[]{ BenchData.rvoltage, BenchData.rcurrent, BenchData.ractivepower, powerfactor, BenchData.rreactivepower }
                    },

                    {
                        COMMANDCODE.PHASELAGCALIB,
                       new object[]{ BenchData.rvoltage, BenchData.rcurrent, BenchData.ractivepower, powerfactor, BenchData.rreactivepower }
                    },
                    {
                        COMMANDCODE.NEUTRALLAGCALIB,
                        new object[]{ 0, BenchData.rcurrent, BenchData.ractivepower, powerfactor, BenchData.rreactivepower }
                    },
                     {
                        COMMANDCODE.CALIBNEUTRAL,
                        new object[]{ 0, BenchData.rcurrent, BenchData.ractivepower, powerfactor, BenchData.rreactivepower }
                    },
                    {
                        COMMANDCODE.CALIBTEMP,
                        new object[]{ m_temprature, 0, 0, 0, 0}
                    },

                    {
                        COMMANDCODE.WRITEDEFAULTCALIB,
                        new object[]{0, 0, 0, 0, 0}
                    },

                    {
                        COMMANDCODE.WRITEDEFAULTPHASE,
                        new object[]{0, 0, 0, 0, 0}
                    },

                    {
                        COMMANDCODE.WRITEDEFUALTNEUTRAL,
                        new object[]{0, 0, 0, 0, 0}
                    },
                };


                object[] objarray = dictCalibCommandMapper[commandcode];

                UInt16 data1 = Convert.ToUInt16(objarray[0]);
                UInt16 data2 = Convert.ToUInt16(objarray[1]);
                UInt32 data3 = Convert.ToUInt32(objarray[2]);
                UInt16 data4 = Convert.ToUInt16(objarray[3]);
                UInt32 data5 = Convert.ToUInt32(objarray[4]);

                string strdata = "Data1: " + data1 +
                                    " Data2: " + data2 +
                                    " Data3: " + data3 +
                                    " Data4: " + data4 +
                                    " Data5: " + data5;

                Logger.Logger.WriteCalibLog(strdata, mpos);

                byte[] bytearr1 = BitConverter.GetBytes(data1);
                byte[] bytearr2 = BitConverter.GetBytes(data2);
                byte[] bytearr3 = BitConverter.GetBytes(data3);
                byte[] bytearr4 = BitConverter.GetBytes(data4);
                byte[] bytearr5 = BitConverter.GetBytes(data5);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytearr1);
                    Array.Reverse(bytearr2);
                    Array.Reverse(bytearr3);
                    Array.Reverse(bytearr4);
                    Array.Reverse(bytearr5);
                }

                calibdata.Add(0x11);
                calibdata.Add((byte)commandcode);
                calibdata.Add(0x12);
                calibdata.AddRange(bytearr1);
                calibdata.Add(0x12);
                calibdata.AddRange(bytearr2);
                calibdata.Add(0x06);
                calibdata.AddRange(bytearr3);
                calibdata.Add(0x10);
                calibdata.AddRange(bytearr4);
                calibdata.Add(0x06);
                calibdata.AddRange(bytearr5);
            }
            catch (Exception ex)
            {
                Logger.Logger.WriteCalibLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + "->"
                                       + System.Reflection.MethodBase.GetCurrentMethod().Name + "->" + ex.Message, mpos);
            }

            return calibdata;
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

                if (_dlmsmanager.serialcom.ReceiveBuffer[49] == 0x02 && (_dlmsmanager.serialcom.ReceiveBuffer[50] == 1 || _dlmsmanager.serialcom.ReceiveBuffer[50] == 17))
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
            const int STARTADDRESS = 0x25;
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

        #region BOOTLOADER
        /// <summary>
        /// Method Name: BOOTLOADER
        /// Description: Write TXT file in to the meters 
        /// Author:      Mohsin Raza
        /// </summary>
        public override Constants.GlobalConstants.Result BOOTLOADER(int mpos, int mAct)
        {
            // TODO : Variables declaration and initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            IntegratedCalibration.Communication.OTAImageEntity objimageEntity = (IntegratedCalibration.Communication.OTAImageEntity)mpArg;

            LogBootLoaderStatus("0", serialcomm.mpos,true);

            try
            {
                cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110GETIMAGEBLOCKSIZE].mclobisatt);

                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                    return _result;
                //-----------------------------Get Image Transfer Status and Enable if Not--------------------------------------------------
                cosempkt = new List<byte>();
                cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110GETIMAGETRANSFERENABLESTATUS].mclobisatt);
                if (_dlmsmanager.NormalGetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.SM110GETIMAGETRANSFERENABLESTATUS) != Constants.GlobalConstants.Result.Pass)
                    return _result;
                if (_dlmsmanager.serialcom.ReceiveBuffer[17] != 0)
                {
                    _result = Constants.GlobalConstants.Result.Fail;
                    Logger.Logger.WriteCalibLog("Reading Block Size - fail", serialcomm.mpos);
                    return _result;
                }
                if (_dlmsmanager.serialcom.ReceiveBuffer[19] == (byte)IntegratedCalibration.Communication.OTAImageEntity.ImageTransferEnabeling.Disable) //---Enable incase it is disabled
                {
                    cosempkt = new List<byte>();
                    cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110SETIMAGETRANSFERENABLESTATUS].mclobisatt);
                    cosempkt.Add(0x03);
                    cosempkt.Add(0x00);
                    if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.SM110SETIMAGETRANSFERENABLESTATUS) != Constants.GlobalConstants.Result.Pass)
                        return _result;
                    if (_dlmsmanager.serialcom.ReceiveBuffer[17] != 0)
                    {
                        _result = Constants.GlobalConstants.Result.Fail;
                        Logger.Logger.WriteCalibLog("Reading Block Size - fail", serialcomm.mpos);
                        return _result;
                    }
                }
                //-------------------------Initiation of Image Transfer----------------------------------
                cosempkt = new List<byte>();
                cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110INITIMAGETRANSFER].mclobisatt);
                cosempkt.Add(0x02);
                cosempkt.Add(0x02);
                cosempkt.Add(0x09);
                cosempkt.Add((byte)(objimageEntity.ImageIdentifier.Length + objimageEntity.ImageCRC.Length));
                cosempkt.AddRange(objimageEntity.ImageCRC);
                cosempkt.AddRange(objimageEntity.ImageIdentifier);
                cosempkt.Add(0x06);
                cosempkt.AddRange(objimageEntity.ImageSize);
                if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.SM110INITIMAGETRANSFER) != Constants.GlobalConstants.Result.Pass)
                    return _result;
                if (_dlmsmanager.serialcom.ReceiveBuffer[17] != 0)
                {
                    _result = Constants.GlobalConstants.Result.Fail;
                    Logger.Logger.WriteCalibLog("Reading Block Size - fail", serialcomm.mpos);
                    return _result;
                }
                //-----------------------------Start of Image Packet transfer----------------------------------
                int imageBlockNumber = 0;
                foreach (var item in objimageEntity.FirmwareImagelists)
                {
                    if (TransferImagePacket(item, imageBlockNumber, _dlmsmanager, _result) != Constants.GlobalConstants.Result.Pass)
                    {
                        _result = Constants.GlobalConstants.Result.Fail;
                        Logger.Logger.WriteCalibLog("Block Transfer Re-try- fail", serialcomm.mpos);
                        return _result;
                    }
                    else imageBlockNumber++;
                }

                //-------------read 1st Not transfer blok number---------
                imageBlockNumber = 0;
                Int32 firstBlockNotTransferNumber = 0;
                do
                {
                    cosempkt = new List<byte>();
                    cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110IMAGEFIRSTBLOCKNOTTRANSFERBLOKNUMBER].mclobisatt);
                    if (_dlmsmanager.NormalGetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.SM110IMAGEFIRSTBLOCKNOTTRANSFERBLOKNUMBER) != Constants.GlobalConstants.Result.Pass)
                    {
                        _result = Constants.GlobalConstants.Result.Fail;
                        Logger.Logger.WriteCalibLog("Reading NormalGetPacket - fail", serialcomm.mpos);
                        return _result;
                    }
                    byte[] temValue = new byte[4];
                    Array.Copy(_dlmsmanager.serialcom.ReceiveBuffer, 19, temValue, 0, temValue.Length);
                    firstBlockNotTransferNumber = BitConverter.ToInt32(temValue.Reverse().ToArray(), 0);
                    if (firstBlockNotTransferNumber < objimageEntity.FirmwareImagelists.Count)
                    {
                        if (TransferImagePacket(objimageEntity.FirmwareImagelists[firstBlockNotTransferNumber], imageBlockNumber, _dlmsmanager, _result) != Constants.GlobalConstants.Result.Pass)
                        {
                            _result = Constants.GlobalConstants.Result.Fail;
                            Logger.Logger.WriteCalibLog("Reading Block Count Error - fail", serialcomm.mpos);
                            return _result;
                        }
                    }

                } while (firstBlockNotTransferNumber < objimageEntity.FirmwareImagelists.Count);
                
                _result = Constants.GlobalConstants.Result.Pass;

                LogBootLoaderStatus("1", serialcomm.mpos, true);
        
            }
            catch (Exception ex)
            {

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

        private Constants.GlobalConstants.Result TransferImagePacket(List<byte> item, int imageBlockNumber, DLMSManager _dlmsmanager, Constants.GlobalConstants.Result _result )
        {
            try
            {
                int retryCounts = 3;
                _result = Constants.GlobalConstants.Result.Fail;
                List<byte> cosempkt = new List<byte>();
                cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110IMAGEBLOCKTRANSFER].mclobisatt);
                cosempkt.Add(0x02);
                cosempkt.Add(0x02);
                cosempkt.Add(0x06);
                cosempkt.AddRange(BitConverter.GetBytes(imageBlockNumber).Reverse());
                cosempkt.Add(0x09);
                cosempkt.Add(0x82);
                cosempkt.Add(0x01);
                cosempkt.Add(0x00);
                cosempkt.AddRange(item);
                
                do
                {
                    //Logger.Logger.WriteCalibLog("Rx[8]->" + _dlmsmanager.serialcom.ReceiveBuffer[8].ToString("x2"), _dlmsmanager.serialcom.mpos);
                    
                    if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.SM110IMAGEBLOCKTRANSFER) == Constants.GlobalConstants.Result.Pass)
                    {
                        //Logger.Logger.WriteCalibLog("cmd->" + _dlmsmanager.nTestCMDByte.ToString("x2"), _dlmsmanager.serialcom.mpos);
                        _result = Constants.GlobalConstants.Result.Pass;
                        return _result;
                    }

                    Logger.Logger.WriteCalibLog("cmd->" + _dlmsmanager.nTestCMDByte.ToString("x2"), _dlmsmanager.serialcom.mpos);

                } while (retryCounts-- > 0);
            }
            catch (Exception ex)
            {
                Logger.Logger.WriteCalibLog(ex.Message, _dlmsmanager.serialcom.mpos);
                throw ex;
            }
            return _result;
        }

       
        public override Constants.GlobalConstants.Result IMAGEVERIFY(int mpos, int mAct)
        {
            // TODO : Variables declaration and initialisation here
           
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();
            int timeoutstep = 5000;
            int maxretey = 25;
            string strmsg = string.Empty;

            try
            {
                               
                if (LogBootLoaderStatus("0", serialcomm.mpos, false) != 1)
                    return _result;

                byte retry = 0;

                do
                {
                    cosempkt = new List<byte>();
                    cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110IMAGESTATUS].mclobisatt);
                    serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);

                    Logger.Logger.WriteCalibLog("Connecting -> Verifying Image Status . . . " + retry.ToString("%2d"), serialcomm.mpos);

                    if (_dlmsmanager.ConnectToMeter() == Constants.GlobalConstants.Result.Pass)
                    {

                        Logger.Logger.WriteCalibLog("Verifying Image Status . . . " + retry.ToString("%2d"), serialcomm.mpos);

                        if (_dlmsmanager.NormalGetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.SM110IMAGESTATUS) != Constants.GlobalConstants.Result.Pass)
                            return _result;

                        Logger.Logger.WriteCalibLog("Verifying Image Status Status Value -> " + _dlmsmanager.serialcom.ReceiveBuffer[19].ToString(), serialcomm.mpos);

                        if (_dlmsmanager.serialcom.ReceiveBuffer[19] == (byte)IntegratedCalibration.Communication.OTAImageEntity.ImageTransferStatus.Image_verification_successful)
                        {
                            _result = Constants.GlobalConstants.Result.Pass;
                            LogBootLoaderStatus("2", serialcomm.mpos, true);
                            Logger.Logger.WriteCalibLog("Verifying Image Status- Pass, retry -> " + retry.ToString(), serialcomm.mpos);
                            return _result;
                        }
                        if (_dlmsmanager.serialcom.ReceiveBuffer[19] == (byte)IntegratedCalibration.Communication.OTAImageEntity.ImageTransferStatus.Image_transfer_initiated)
                        {
                            serialcomm.SetSerialPortSettings(mpos, (int)Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110IMAGEVERIFY].muiRxDelay);
                            cosempkt = new List<byte>();
                            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110IMAGEVERIFY].mclobisatt);
                            cosempkt.Add(0x0F);
                            cosempkt.Add(0x00);

                            Logger.Logger.WriteCalibLog("Set Verify Image Status Status Value ...", serialcomm.mpos);

                            if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.SM110IMAGEVERIFY) != Constants.GlobalConstants.Result.Pass)
                            {
                                if (_dlmsmanager.serialcom.bufferIndex < 1)
                                {
                                    Logger.Logger.WriteCalibLog("Set Verify Image Status Status Value (rxbuffer = 0) -> Failed", serialcomm.mpos);
                                    return _result;
                                }
                            }
                        }

                    }

                    retry++;

                    serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);
                    
                    _dlmsmanager.DisConnect();

                    Thread.Sleep(timeoutstep);

                    //strmsg = "Verifying Image Status-fail, retry -
                    Logger.Logger.WriteCalibLog("Verifying Image Status- fail, retry -> " + retry.ToString(), serialcomm.mpos);



                } while (retry < maxretey);

            }
            catch (Exception ex)
            {

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

        public override Constants.GlobalConstants.Result IMAGEACTIVATION(int mpos, int mAct)
        {
            // TODO : Variables declaration and initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();
            int timeoutstep = 5000;
            int maxretey = 25;
            string strmsg = string.Empty;

            try
            {
                
                if (LogBootLoaderStatus("0", serialcomm.mpos, false) != 2)
                    return _result;

                byte retry = 0;

                do
                {
                    cosempkt = new List<byte>();
                    cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110IMAGESTATUS].mclobisatt);
                    serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);

                    Logger.Logger.WriteCalibLog("Connecting Meter -> Reading Image Activation Status...", serialcomm.mpos);

                    if (_dlmsmanager.ConnectToMeter() == Constants.GlobalConstants.Result.Pass)
                    {

                        Logger.Logger.WriteCalibLog("Reading Image Activation Status...", serialcomm.mpos);

                        if (_dlmsmanager.NormalGetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.SM110IMAGESTATUS) != Constants.GlobalConstants.Result.Pass)
                        {
                            Logger.Logger.WriteCalibLog("Reading Image Activation Status Failed", serialcomm.mpos);
                            return _result;
                        }

                        Logger.Logger.WriteCalibLog("Activation Image Status Status Value -> " + _dlmsmanager.serialcom.ReceiveBuffer[19].ToString(), serialcomm.mpos);

                        if (_dlmsmanager.serialcom.ReceiveBuffer[19] == (byte)IntegratedCalibration.Communication.OTAImageEntity.ImageTransferStatus.Image_activation_successful)
                        {
                            _result = Constants.GlobalConstants.Result.Pass;
                            LogBootLoaderStatus("3", serialcomm.mpos, true);
                            Logger.Logger.WriteCalibLog("Activation Image Status- Pass, retry -> " + retry.ToString(), serialcomm.mpos);
                            return _result;
                        }

                        if (_dlmsmanager.serialcom.ReceiveBuffer[19] == (byte)IntegratedCalibration.Communication.OTAImageEntity.ImageTransferStatus.Image_verification_successful)
                        {
                            cosempkt = new List<byte>();
                            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110IMAGEACTIVATION].mclobisatt);
                            cosempkt.Add(0x0F);
                            cosempkt.Add(0x00);

                            if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                                return _result;

                            serialcomm.SetSerialPortSettings(mpos, (int)Constants.DLMSConstants.GlobalSM110MeterObject[Constants.DLMSConstants.DLMSCommand.SM110IMAGEACTIVATION].muiRxDelay);

                            Logger.Logger.WriteCalibLog("Set Activation Image Status ...", serialcomm.mpos);

                            if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.SM110IMAGEACTIVATION) != Constants.GlobalConstants.Result.Pass)
                            {

                                if (_dlmsmanager.serialcom.bufferIndex < 1)
                                {
                                    Logger.Logger.WriteCalibLog("Set Activation Image Status : Failed", serialcomm.mpos);
                                    return _result;
                                }
                            }
                        }

                    }

                    retry++;

                    serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM110);

                    _dlmsmanager.DisConnect();

                    Thread.Sleep(timeoutstep);
                                        
                    Logger.Logger.WriteCalibLog("Activating Image Status- fail, retry -> " + retry.ToString(), serialcomm.mpos);


                } while (retry < maxretey);

            }
            catch (Exception ex)
            {

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


        private int LogBootLoaderStatus(string arg_strMsg, int mpos, bool isWrite)
        {
            //File open
            int retval = 0;
            string strfilename = AppDomain.CurrentDomain.BaseDirectory + "BootLoader" + mpos.ToString("D2") + ".log";
            if (isWrite)
                File.WriteAllText(strfilename, arg_strMsg + "\n");
            else
            {
                string strretval = File.ReadAllText(strfilename);
                int.TryParse(strretval, out retval);
                /*
                 * 1 BootLoader Pass
                 * 2 ImageVerify Pass
                 * 3 Image Activation Pass
                 */
            }

            return retval;
        }

    }
    
}
