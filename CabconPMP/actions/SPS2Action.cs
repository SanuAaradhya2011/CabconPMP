using IntegratedCalibration.Constants;
using IntegratedCalibration.DataLayer;
using LNG.Communication.SerialCommunication;
using SmartCalibration.Communication;
using SmartCalibration.Constants;
using SmartCalibration.DataLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartCalibration.Actions
{
    /// <summary>
    /// Action class for THREE PHASE SMART METER - SM310
    /// 
    /// </summary>
    public class SPS2Action : GenericAction
    {
        // AT PRESENT MAX PARAM COUNT IS 8 AS ON 07-FEB-2020
        byte MAXPARAMCOUNT = 8;
        UInt32 neutralcurrent = 0;
        bool isncalib = false;
        // EIGHT TYPE OF PARAM TO CALIBRATE IN SM310
        enum SPS2COMMAND
        {
            RESET = 1,
            SET = 1,
            INIT = 0,
            CODE_VOLTAGE = 0x01,
            CODE_CURRENT = 0x02,
            CODE_ACTIVE = 0x03,
            CODE_REACTIVE = 0x04,
            CODE_NEUTRAL = 0x0A,
            CODE_TEMPERATURE = 0x0C,
            CODE_SAMPLES = 0x31,

        }

        #region RESET CALIBRATION
        /// <summary>
        /// Method Name: CALIBRESET
        /// Description: Reset Calibration, Value of mAct is 1 for reset calibration
        /// Author:      Mohsin Raza
        /// Date:        07-FEB- 2020
        /// </summary>        
        public override Constants.GlobalConstants.Result CALIBRESET(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            // At Volatge – 240V, Current = 0A then Reset Calibration.
            Logger.Logger.WriteCalibLog("CALIB RESET Command ---", mpos);
            int maxcurrent = 20;
            // Handle Aemcal situaton, in case of 0 amp, bench generate microamp current that why we devided by 1000 
            // Same can be removed after fixing in aemcal to handle th emili and micro amp current with unit
            BenchData.rcurrent = BenchData.rcurrent / 1000;
            BenchData.ycurrent = BenchData.ycurrent / 1000;
            BenchData.bcurrent = BenchData.bcurrent / 1000;


            if (BenchData.rcurrent > maxcurrent ||
                BenchData.ycurrent > maxcurrent ||
                 BenchData.bcurrent > maxcurrent
                )
            {
                Logger.Logger.WriteCalibLog("Leakage Current ---", mpos);
                return GlobalConstants.Result.OUT;
            }

            return WriteCommand(mpos, (byte)SPS2COMMAND.RESET);
        }
        #endregion

        #region Active CALIBRATION
        /// <summary>
        /// Method Name: CALIBACTIVE
        /// Description: CALIBRATE VOLTAGE, CURRENT, ACTIVE POWER;  Value of mAct is 02 for active calibration
        /// Author:      Mohsin Raza
        /// Date:        03-JULY-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result CALIBACTIVE(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            // At UPF with input Parameter Voltage – 240V, current - 5A, PF – 1 UPF (zero Deg) then calibrate the Voltage, Current & Active Power.
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            neutralcurrent = 0;

            if(BenchData.rreactivepower < 0 ||
               BenchData.yreactivepower < 0 ||
               BenchData.breactivepower < 0)
            {
                BenchData.rreactivepower = 0;
                BenchData.yreactivepower = 0;
                BenchData.breactivepower = 0;
            }

            if (BenchData.rreactivepower > 0x00ffffff ||
               BenchData.yreactivepower > 0x00ffffff ||
               BenchData.breactivepower > 0x00ffffff)
            {
                BenchData.rreactivepower = 0;
                BenchData.yreactivepower = 0;
                BenchData.breactivepower = 0;
            }

            if (WriteCommand(mpos, (byte)SPS2COMMAND.INIT) != GlobalConstants.Result.Pass)
            {
                Logger.Logger.WriteCalibLog("CALIB INIT Command -> Fail", mpos);
                return _result;
            }

            Logger.Logger.WriteCalibLog("CALIB INIT Command -> Pass", mpos);

            Logger.Logger.WriteCalibLog("Waiting for time (msec) - " + GlobalConstants.SM310_MAXSAMPLECOLLECTIONTIME.ToString(), mpos);

            System.Threading.Thread.Sleep(GlobalConstants.SM310_MAXSAMPLECOLLECTIONTIME);
            
            return WriteCommand(mpos, (byte)SPS2COMMAND.SET);
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
            // At 0.5 Lag with input Parameter Voltage – 240V, Current - 5A, PF – 0.5 Lag (60 Deg) then calibrate the Reactive Power
            neutralcurrent = 0;

            if (WriteCommand(mpos, (byte)SPS2COMMAND.INIT) != GlobalConstants.Result.Pass)
            {
                Logger.Logger.WriteCalibLog("CALIB INIT Command -> Fail", mpos);
                return _result;
            }

            Logger.Logger.WriteCalibLog("CALIB INIT Command -> Pass", mpos);

            Logger.Logger.WriteCalibLog("Waiting for time (msec) - " + GlobalConstants.SM310_MAXSAMPLECOLLECTIONTIME.ToString(), mpos);

            System.Threading.Thread.Sleep(GlobalConstants.SM310_MAXSAMPLECOLLECTIONTIME);

            return WriteCommand(mpos, (byte)SPS2COMMAND.SET);
        }
        #endregion
          
        #region WriteCommand
        /// <summary>
        /// Method Name: WriteCommand
        /// Description: Generic Method for RESET, INIT amd SET Command
        /// Author:      Mohsin Raza
        /// Date:        29-JUNE-2020
        /// </summary>        
        private Constants.GlobalConstants.Result WriteCommand(int mpos, byte commandtype)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            cmddelay = 2000;
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalSPS2MeterObject[Constants.DLMSConstants.DLMSCommand.SPS2SET].mclobisatt);

            cosempkt.AddRange(GetCosemPacket(commandtype, mpos));

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

        #region GetCosemPacket
        private List<byte> GetCosemPacket(byte cmdtype, int mpos)
        {
            List<byte> cosempkt = new List<byte>();

            cosempkt.Add(0x00);

            cosempkt.Add(0x02);
            cosempkt.Add(MAXPARAMCOUNT);
            
            // Command Type
            cosempkt.Add(0x11);
            cosempkt.Add(cmdtype); // SET = 1, INIT = 0, RESET = 1

            cosempkt.Add(0x02);
            cosempkt.Add(0x04);


            //MessageBox.Show(BenchData.rvoltage.ToString() + "-" + BenchData.yvoltage.ToString() + "-" + BenchData.bvoltage.ToString());

            // Command Type for Voltage
            cosempkt.Add(0x11);
            cosempkt.Add((byte)SPS2COMMAND.CODE_VOLTAGE);

            // R
            cosempkt.Add(0x06);
            cosempkt.AddRange(GetBytesFmValue(BenchData.rvoltage, mpos));

            // Y
            cosempkt.Add(0x06);
            cosempkt.AddRange(GetBytesFmValue(BenchData.yvoltage, mpos));

            // B
            cosempkt.Add(0x06);
            cosempkt.AddRange(GetBytesFmValue(BenchData.bvoltage, mpos));

            cosempkt.Add(0x02);
            cosempkt.Add(0x04);

            // Command Type for Current
            cosempkt.Add(0x11);
            cosempkt.Add((byte)SPS2COMMAND.CODE_CURRENT);

            // R
            cosempkt.Add(0x06);
            if(isncalib == true)
                cosempkt.AddRange(GetBytesFmValue(0, mpos));
            else
                cosempkt.AddRange(GetBytesFmValue(BenchData.rcurrent, mpos));


            // Y
            cosempkt.Add(0x06);
            cosempkt.AddRange(GetBytesFmValue(BenchData.ycurrent, mpos));

            // B
            cosempkt.Add(0x06);
            cosempkt.AddRange(GetBytesFmValue(BenchData.bcurrent, mpos));

            cosempkt.Add(0x02);
            cosempkt.Add(0x04);

            // Command Type for Active Power
            cosempkt.Add(0x11);
            cosempkt.Add((byte)SPS2COMMAND.CODE_ACTIVE);

            // R
            cosempkt.Add(0x06);
            cosempkt.AddRange(GetBytesFmValue(BenchData.ractivepower, mpos));

            // Y
            cosempkt.Add(0x06);
            cosempkt.AddRange(GetBytesFmValue(BenchData.yactivepower, mpos));

            // B
            cosempkt.Add(0x06);
            cosempkt.AddRange(GetBytesFmValue(BenchData.bactivepower, mpos));

            cosempkt.Add(0x02);
            cosempkt.Add(0x04);


            // Command Type for Reactive Power
            cosempkt.Add(0x11);
            cosempkt.Add((byte)SPS2COMMAND.CODE_REACTIVE);

            // R
            cosempkt.Add(0x06);
            cosempkt.AddRange(GetBytesFmValue(BenchData.rreactivepower, mpos));

            // Y
            cosempkt.Add(0x06);
            cosempkt.AddRange(GetBytesFmValue(BenchData.yreactivepower, mpos));

            // B
            cosempkt.Add(0x06);
            cosempkt.AddRange(GetBytesFmValue(BenchData.breactivepower, mpos));

            cosempkt.Add(0x02);
            cosempkt.Add(0x02);


            // Command Type for Neutral Current
            cosempkt.Add(0x11);
            cosempkt.Add((byte)SPS2COMMAND.CODE_NEUTRAL);

            // R
            cosempkt.Add(0x06);
            if (isncalib == true)
                cosempkt.AddRange(GetBytesFmValue(BenchData.rcurrent, mpos));
            else
                cosempkt.AddRange(GetBytesFmValue(0, mpos));


            cosempkt.Add(0x02);
            cosempkt.Add(0x02);

            // Command Type for Temperature
            cosempkt.Add(0x11);
            cosempkt.Add((byte)SPS2COMMAND.CODE_TEMPERATURE);
            
            // Temperature 
            cosempkt.Add(0x06);
            UInt32 temperature = m_temprature;
            cosempkt.AddRange(GetBytesFmValue(temperature, mpos));

            cosempkt.Add(0x02);
            cosempkt.Add(0x02);


            // Command Type for Samples
            cosempkt.Add(0x11);
            cosempkt.Add((byte)SPS2COMMAND.CODE_SAMPLES);
            
            //Samples
            cosempkt.Add(0x11);
            cosempkt.Add(Constants.GlobalConstants.MAXSM310SAMPLES);

            return cosempkt;
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
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
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

                serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM310RESET);

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
            const byte CALIBSTAGE = 0x02;
            

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

                //if (_dlmsmanager.serialcom.ReceiveBuffer[20] == CALIBSTAGE && 
                //    _dlmsmanager.serialcom.ReceiveBuffer[30] != 0x00 && 
                //    (_dlmsmanager.serialcom.ReceiveBuffer[21] == 0x01 || _dlmsmanager.serialcom.ReceiveBuffer[21] == 0x02
                //    || _dlmsmanager.serialcom.ReceiveBuffer[21] == 0x03) && // FT
                //    _dlmsmanager.serialcom.ReceiveBuffer[22] != 0x00)   // FT Must not fail
                if ((_dlmsmanager.serialcom.ReceiveBuffer[20] >= CALIBSTAGE) &&
                    _dlmsmanager.serialcom.ReceiveBuffer[30] != 0x00 &&
                    _dlmsmanager.serialcom.ReceiveBuffer[21] == 0x01 && // FT
                    _dlmsmanager.serialcom.ReceiveBuffer[22] != 0x00)   // FT Must not fail
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
            byte TRAVELLERSTAG = 0x03;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);

            AMITraveller _amitraveller = new AMITraveller();
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalGenericMeterObject[Constants.DLMSConstants.DLMSCommand.SETTRAVELER].mclobisatt);
            
            cosempkt.Add(0x09);
            cosempkt.Add(0x08);
            cosempkt.Add(TRAVELLERSTAG);

            if (_amitraveller.ReadTravellerStatus(serialcomm.mpos) == true)
                cosempkt.Add(1);
            else
                cosempkt.Add(0);


            cosempkt.Add(Convert.ToByte(DateTime.Now.Day));
            cosempkt.Add(Convert.ToByte(DateTime.Now.Month));
            cosempkt.Add(Convert.ToByte(String.Format("{0:yy}", DateTime.Now)));
            cosempkt.Add(Convert.ToByte(DateTime.Now.Hour));
            cosempkt.Add(Convert.ToByte(DateTime.Now.Minute));
            cosempkt.Add(Convert.ToByte(DateTime.Now.Second));



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

        #region GetBytesFmValue
        ////////////////////// ///////////////////////////////////////////////////////
        /*
        Method Name: GetBytesFmValue with one argument
        Purpose:     Convert Data to byte array
        Date:		 29-06-2020
        Author:		 Mohsin Raza		
        */
        /////////////////////////////////////////////////////////////////////////////
        private byte[] GetBytesFmValue(UInt32 benchvalue, int mpos)
        {
            byte[] databytes = new byte[4];

            try
            { 
                databytes = BitConverter.GetBytes(benchvalue);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(databytes);

                // Write Log

            }
            catch (Exception ex)
            {
                Logger.Logger.WriteCalibLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + "->"
                                      + System.Reflection.MethodBase.GetCurrentMethod().Name + "->" + ex.Message, mpos);
            }

            return databytes;

        }
        #endregion

        #region BOOTLOADER Not used
        /// <summary>
        /// Method Name: BOOTLOADER
        /// Description: Write TXT file in to the meters 
        /// Author:      Mohsin Raza
        /// </summary>
        //public override Constants.GlobalConstants.Result BOOTLOADER(int mpos, int mAct)
        //{
        //    // TODO : Variables declaration and initialisation here
        //    Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
        //    NonDLMSSerialComm serialcomm = new NonDLMSSerialComm();
        //    cmddelay = 3000;
        //    serialcomm.SetSerialPortSettings(mpos, cmddelay);
        //    List<byte> cosempkt = new List<byte>();
        //    BSLCommandPacketList[] datalist = (BSLCommandPacketList[])mpArg;

        //    NonDLMSManager _nondlmsmanager = new NonDLMSManager(serialcomm);

        //    try
        //    {
        //        return _nondlmsmanager.WriteBotFile(datalist.ToList(), true);
        //    }
        //    catch (Exception ex)
        //    {

        //        Logger.Logger.WriteCalibLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + "->"
        //                               + System.Reflection.MethodBase.GetCurrentMethod().Name + "->" + ex.Message, mpos);
        //    }


        //    return _result;
        //}
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
            NonDLMSSerialComm serialcomm = new NonDLMSSerialComm();
            cmddelay = 3000;
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            List<byte> cosempkt = new List<byte>();
            BSLCommandPacketList[] datalist = (BSLCommandPacketList[])mpArg;

            NonDLMSManager _nondlmsmanager = new NonDLMSManager(serialcomm);

            try
            {
                return _nondlmsmanager.WriteBotFileRepair(datalist.ToList(), false);
            }
            catch (Exception ex)
            {

                Logger.Logger.WriteCalibLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + "->"
                                       + System.Reflection.MethodBase.GetCurrentMethod().Name + "->" + ex.Message, mpos);
            }


            return _result;
        }
        #endregion

        #region BOOTLOADERREPAIR Not Used
        /// <summary>
        /// Method Name: BOOTLOADER
        /// Description: Write TXT file in to the meters 
        /// Author:      Mohsin Raza
        /// </summary>
        //public override Constants.GlobalConstants.Result BOOTLOADERREPAIRRETRY(int mpos, int mAct)
        //{
        //    // TODO : Variables declaration and initialisation here
        //    Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
        //    NonDLMSSerialComm serialcomm = new NonDLMSSerialComm();
        //    cmddelay = 3000;
        //    serialcomm.SetSerialPortSettings(mpos, cmddelay);
        //    List<byte> cosempkt = new List<byte>();
        //    BSLCommandPacketList[] datalist = (BSLCommandPacketList[])mpArg;

        //    NonDLMSManager _nondlmsmanager = new NonDLMSManager(serialcomm);

        //    try
        //    {
        //        return _nondlmsmanager.WriteBotFileRepairRetry(datalist.ToList(), false);
        //    }
        //    catch (Exception ex)
        //    {

        //        Logger.Logger.WriteCalibLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + "->"
        //                               + System.Reflection.MethodBase.GetCurrentMethod().Name + "->" + ex.Message, mpos);
        //    }


        //    return _result;
        //}
        #endregion

        #region CALIBNEUTRAL
        /// <summary>
        /// Method Name: CALIBNEUTRAL
        /// Description: 
        /// Author:      Mohsin Raza
        /// Date:        03-JULY-2020
        /// </summary>        
        public override Constants.GlobalConstants.Result CALIBNEUTRAL(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            // At UPF with input Parameter Voltage – 240V, current - 5A, PF – 1 UPF (zero Deg) then calibrate the Voltage, Current & Active Power.
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            UInt32 _ncurrent = BenchData.rcurrent;
            neutralcurrent = _ncurrent;
            isncalib = true;
            //BenchData.rcurrent = 0;
            BenchData.ycurrent = 0;
            BenchData.bcurrent = 0;
            BenchData.ractivepower = 0;
            BenchData.yactivepower = 0;
            BenchData.bactivepower = 0;
            BenchData.rreactivepower = 0;
            BenchData.yreactivepower = 0;
            BenchData.breactivepower = 0;

            if (WriteCommand(mpos, (byte)SPS2COMMAND.INIT) != GlobalConstants.Result.Pass)
            {
                Logger.Logger.WriteCalibLog("NEUTRAL INIT Command -> Fail", mpos);
                return _result;
            }

            Logger.Logger.WriteCalibLog("NEUTRAL INIT Command -> Pass", mpos);

            Logger.Logger.WriteCalibLog("Waiting for time (msec) - " + GlobalConstants.SM310_MAXSAMPLECOLLECTIONTIME.ToString(), mpos);

            System.Threading.Thread.Sleep(GlobalConstants.SM310_MAXSAMPLECOLLECTIONTIME);

            return WriteCommand(mpos, (byte)SPS2COMMAND.SET);


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
            int idev = 100;
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




                strTime += " -> Neutral Current: " + (val / idev).ToString() + "." + (val % idev).ToString("d2") + " A" ;

                meterdetail = strTime;

                string filename  = AppDomain.CurrentDomain.BaseDirectory + "Neutral Current" + mpos.ToString("d2") + ".lg";
                               
                
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
        
        #region CONFIGURE
        /// <summary>
        /// Method Name: Configuration
        /// Description: Write csv file in to the meters 
        /// Author:      Mohsin Raza
        /// </summary>
        public override Constants.GlobalConstants.Result CONFIGURE(int mpos, int mAct)
        {
            // TODO : Variables declaration and initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            List<byte> cosempkt = new List<byte>();
            string[] strarr = (string[])mpArg;
            string mytagname = "";
            int MAXPACKETSIZE = Constants.DLMSConstants.PacketSize;
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
                    serialcomm.CommandTimeout = 1200;

                    // For these tags, delay should be at least 15 Sec
                    if (mytagname.Contains("SurveyIP") || mytagname.Contains("LSCaptureobject") || mytagname.Contains("MeteringMode")) serialcomm.CommandTimeout = Constants.DLMSConstants.MAXTIMEOUT;

                    // check size of the command
                    if(mytagname.Contains("AutoScroll") || mytagname.Contains("PushButton"))
                    {
                        _result = _dlmsmanager.WriteBlock(strarr[i]);
                    }
                    else if (strdata.Length < (2 * MAXPACKETSIZE) || mytagname.Contains("FactoryConfigurabelity")
                       || mytagname.Contains("TamperThreshold"))
                    {
                        _result = _dlmsmanager.SetWritePacket(strdata);
                    }
                    else
                    {
                        _result = _dlmsmanager.WriteBlock(strarr[i]);
                    }

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
            if (setdatetime.DayOfWeek == DayOfWeek.Sunday) RTCbyte.Add(0x07);
            else RTCbyte.Add(Convert.ToByte(setdatetime.DayOfWeek));
            
            RTCbyte.Add(Convert.ToByte(setdatetime.Hour));
            RTCbyte.Add(Convert.ToByte(setdatetime.Minute));
            RTCbyte.Add(Convert.ToByte(setdatetime.Second));

            RTCbyte.Add(0xFF);
            RTCbyte.Add(0x80);
            RTCbyte.Add(0x00);
            RTCbyte.Add(0xFF);

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

        #region CALIBVERIFY
        /// <summary>
        /// CALIBVERIFY - Verfy calibraion called after calibration and before verification in YC.
        /// </summary>
        /// <param name="mpos"></param>
        /// <param name="mAct"></param>
        /// <returns></returns>

        public override Constants.GlobalConstants.Result CALIBVERIFY(int mpos, int mAct)
        {
            // TODO : Variables declaration and initialisation here
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            List<byte> databytes = new List<byte>();
            DateTime setdatetime = System.DateTime.Now;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);

            databytes.AddRange(Constants.DLMSConstants.GlobalSPS2MeterObject[Constants.DLMSConstants.DLMSCommand.SPS2CALIBVERIFY].mclobisatt);
          
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);

            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                    return _result;

                if (_dlmsmanager.NormalGetPacket(databytes, Constants.DLMSConstants.DLMSCommand.SPS2CALIBVERIFY) != Constants.GlobalConstants.Result.Pass)
                    return _result;

                _result = IsValidCalibrationData(_dlmsmanager.serialcom.ReceiveBuffer, mpos, mAct);
                    
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

        private Constants.GlobalConstants.Result IsValidCalibrationData(byte[] receivedData,int mpos,int mAct )
        {
            string MeterCalibrationCofficient = "";
            string DefaultCalibrationCofficient = "020a020411011234411234411234410204110212253a12253a12253a02041103121f13121f13121f13020411041278b41278b41278b402041108120161120161120161020411091200001200001200000202110a12be780202110b1200000202110c120000".ToUpperInvariant();
            Dictionary<string, string> dicDefaultCalibrationCofficient = new Dictionary<string, string>();
             
            dicDefaultCalibrationCofficient.Add("Voltage", "02041101123441123441123441");
            dicDefaultCalibrationCofficient.Add("Current", "0204110212253a12253a12253a");
            dicDefaultCalibrationCofficient.Add("Active Power", "02041103121f13121f13121f13");
            dicDefaultCalibrationCofficient.Add("Reactive Power", "020411041278b41278b41278b4");
            dicDefaultCalibrationCofficient.Add("Neu Current", "0202110a12be78");

            int startDataindx = 18;
            string[] datavalue = new string[2];
            int strctureStartIndex = startDataindx;
            if (receivedData[startDataindx] == 0x09) //STRUCT
            {
                int lengthodstruct = receivedData[startDataindx + 1];//length of stract
                byte[] parameterlist = new byte[lengthodstruct + 2];
                Array.Copy(receivedData, startDataindx, parameterlist, 0, parameterlist.Length);
                MeterCalibrationCofficient = GenericMethods.GetByteToHexString(parameterlist);
            }
            //---Verify the complete string received from meter with default cofficient--------------
            Logger.Logger.WriteCalibLog("Meter data: " + MeterCalibrationCofficient, mpos);
            Logger.Logger.WriteCalibLog("Default data: " + DefaultCalibrationCofficient, mpos);

            if(MeterCalibrationCofficient.Length < 1)
            {
                return Constants.GlobalConstants.Result.Fail;
            }

            if (MeterCalibrationCofficient.Contains(DefaultCalibrationCofficient))
            {
                Logger.Logger.WriteCalibLog("Meter is not calibrated", mpos);
                return Constants.GlobalConstants.Result.Fail;
            }
            //---Re-Verify Parameters Wise Default Value with default cofficient list---------------------    

            Constants.GlobalConstants.Result _result = GlobalConstants.Result.Pass;

            foreach (var item in dicDefaultCalibrationCofficient)
            {                                 
                if (MeterCalibrationCofficient.Contains(item.Value.ToUpper()))
                {
                    _result = GlobalConstants.Result.Fail;
                    Logger.Logger.WriteCalibLog(item.Key + " Failed -> Default data " + item.Value.ToUpper(), mpos);
                }
            }

            return _result;
            
        }

        #endregion
    }

}
