using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LNG.Communication.SerialCommunication;
using SmartCalibration.Communication;
using SmartCalibration.Constants;
using SmartCalibration.DataLayer;

namespace SmartCalibration.Actions
{
    public class NonAMI1PAction : GenericAction
    {
        UInt32 PHASECURRENT = 77082;
        UInt32 NEUTRALCURRENT = 108422;
        UInt32 PHASEENERGY = 249966;
        UInt32 NEUTRALENERGY = 177875;
        UInt32 MAXCURRENT = 11000;
        UInt32 MINCURRENT = 9000;
        int MAXPHENERGY = 25;
        int MINPHENERGY = -25;
        //string aesdata = "01000060029BFF02000112637C777BF26B6FC53001672BFED7AB76CA82C97DFA5947F0ADD4A2AF9CA472C0B7FD9326363FF7CC34A5E5F171D8311504C723C31896059A071280E2EB27B27509832C1A1B6E5AA0523BD6B329E32F8453D100ED20FCB15B6ACBBE394A4C58CFD0EFAAFB434D338545F9027F503C9FA851A3408F929D38F5BCB6DA2110FFF3D2CD0C13EC5F974417C4A77E3D645D197360814FDC222A908846EEB814DE5E0BDBE0323A0A4906245CC2D3AC629195E479E7C8376D8DD54EA96C56F4EA657AAE08BA78252E1CA6B4C6E8DD741F4BBD8B8A703EB5664803F60E613557B986C11D9EE1F8981169D98E949B1E87E9CE5528DF8CA1890DBFE6426841992D0FB054BB1652096AD53036A538BF40A39E81F3D7FB7CE339829B2FFF87348E4344C4DEE9CB547B9432A6C2233DEE4C950B42FAC34E082EA16628D924B2765BA2496D8BD12572F8F66486689816D4A45CCC5D65B6926C704850FDEDB9DA5E154657A78D9D8490D8AB008CBCD30AF7E45805B8B34506D02C1E8FCA3F0F02C1AFBD0301138A6B3A9111414F67DCEA97F2CFCEF0B4E67396AC7422E7AD3585E2F937E81C75DF6E47F11A711D29C5896FB7620EAA18BE1BFC563E4BC6D279209ADBC0FE78CD5AF41FDDA8338807C731B11210592780EC5F60517FA919B54A0D2DE57A9F93C99CEFA0E03B4DAE2AF5B0C8EBBB3C83539961172B047EBA77D626E169146355210C7D";

        Dictionary<DLMSConstants.DLMSCommand, byte> CommandFactorMapper = new Dictionary<DLMSConstants.DLMSCommand, byte>()
        {
            { DLMSConstants.DLMSCommand.CALCURRENTN, 05},
            { DLMSConstants.DLMSCommand.CALCURRENTPH, 02},
        };

        #region CALIBRESET
        public override Constants.GlobalConstants.Result CALIBRESET(int mpos, int mAct)
        {
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            Logger.Logger.WriteCalibLog("CAIBRESET --->", mpos);

            try
            {
                cosempkt.Clear();
                cosempkt.AddRange(Constants.DLMSConstants.GlobalNonAMI1PMeterObject[Constants.DLMSConstants.DLMSCommand.NONAMI1PCALIB].mclobisatt);
                cosempkt.Add(0x00);
                cosempkt.Add(0x02);
                cosempkt.Add(0x02);
                cosempkt.Add(0x0F);
                cosempkt.Add(00);
                cosempkt.Add(0x06);
                cosempkt.Add(00);
                cosempkt.Add(00);
                cosempkt.Add(00);
                cosempkt.Add(01);

                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                {
                    return _result;
                }

                return _dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET);
            }
            catch (Exception ex)
            {
                Logger.Logger.WriteCalibLog(ex.Message, mpos);

                return Constants.GlobalConstants.Result.Fail;
            }
            finally
            {
                _dlmsmanager.DisConnect();

            }
        }
        #endregion

        #region CALIBPHASE
        public override GlobalConstants.Result CALIBPHASE(int mpos, int mAct)
        {
            byte factorid = 0x03;
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            if(mresulterror >= 0xFFFFFFFF)
            {
                return Constants.GlobalConstants.Result.ERRFILE;
            }

            Logger.Logger.WriteCalibLog("CALIBPHASE ---", mpos);

            Logger.Logger.WriteCalibLog("Error -> " + mresulterror.ToString(), mpos);

            try
            {
                if (mresulterror < MINPHENERGY || mresulterror > MAXPHENERGY)
                {
                    Logger.Logger.WriteCalibLog("Error Out of range +25/-25", mpos);
                    return Constants.GlobalConstants.Result.OUT;
                }

                UInt32 dwVal = (UInt32)(PHASEENERGY + (UInt32)(PHASEENERGY * (mresulterror / 100.0)));

                Logger.Logger.WriteCalibLog("PHASEENERGY -> " +  PHASEENERGY.ToString() + " CalCoeff -> " + dwVal.ToString(), mpos);

                byte[] coeffdata = BitConverter.GetBytes(dwVal);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(coeffdata);

                cosempkt.Clear();
                cosempkt.AddRange(Constants.DLMSConstants.GlobalNonAMI1PMeterObject[Constants.DLMSConstants.DLMSCommand.NONAMI1PCALIB].mclobisatt);
                cosempkt.Add(0x00);
                cosempkt.Add(0x02);
                cosempkt.Add(0x02);
                cosempkt.Add(0x0F);
                cosempkt.Add(factorid);
                cosempkt.Add(0x06);
                cosempkt.AddRange(coeffdata);

                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                {
                   return _result;
                }

                return _dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET);
            }
            catch(Exception ex)
            {
                Logger.Logger.WriteCalibLog(ex.Message, mpos);

                return Constants.GlobalConstants.Result.Fail;
            }
            finally
            {
                _dlmsmanager.DisConnect();

            }
        }
        #endregion

        #region CALIBNEUTRAL
        public override GlobalConstants.Result CALIBNEUTRAL(int mpos, int mAct)
        {
            byte factorid = 0x06;
            UInt32 dwVal = 0;
            byte[] factoridArray = new byte[] { 0x06, 0x07, 0x08};
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            if (mresulterror >= 0xFFFFFFFF)
            {
                return Constants.GlobalConstants.Result.ERRFILE;
            }

            Logger.Logger.WriteCalibLog("CALIBNEUTRAL --->", mpos);

            Logger.Logger.WriteCalibLog("Error -> " + mresulterror.ToString(), mpos);

            try
            {
                if (mresulterror < MINPHENERGY || mresulterror > MAXPHENERGY)
                {
                    Logger.Logger.WriteCalibLog("Error Out of range +25/-25", mpos);
                    return Constants.GlobalConstants.Result.OUT;
                }

                // Calibrated Neutral_P_1WH = Default Neutral_P_1WH + (Default Neutral_P_1WH * error/100)
                //dwVal=(DWORD) (177875.0+(177875.0*(percentErr/100.0))); // md_NeutralEnergyConstant
                //dwVal = (DWORD)(md_NeutralEnergyConstant + (md_NeutralEnergyConstant * (percentErr / 100.0)));
                dwVal = (UInt32)(NEUTRALENERGY + (UInt32)(NEUTRALENERGY * (mresulterror / 100.0)));
                
                Logger.Logger.WriteCalibLog("NEUTRALENERGY -> " + NEUTRALENERGY.ToString() + " CalCoeff -> " + dwVal.ToString(), mpos);

                byte[] coeffdata = BitConverter.GetBytes(dwVal);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(coeffdata);

                cosempkt.Clear();
                cosempkt.AddRange(Constants.DLMSConstants.GlobalNonAMI1PMeterObject[Constants.DLMSConstants.DLMSCommand.NONAMI1PCALIB].mclobisatt);
                cosempkt.Add(0x00);
                cosempkt.Add(0x02);
                cosempkt.Add(0x02);
                cosempkt.Add(0x0F);
                cosempkt.Add(factorid);
                cosempkt.Add(0x06);
                cosempkt.AddRange(coeffdata);

                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                {
                    return _result;
                }

                return _dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET);
            }
            catch (Exception ex)
            {
                Logger.Logger.WriteCalibLog(ex.Message, mpos);

                return Constants.GlobalConstants.Result.Fail;
            }
            finally
            {
                _dlmsmanager.DisConnect();

            }
        }
        #endregion

        #region CALIBCURRENT
        public override GlobalConstants.Result CALIBCURRENT(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();
            int startindex = 20;
            UInt32 ICAL = 10;
            byte factorid = 02;

            if (mresulterror >= 0xFFFFFFFF)
            {
                return Constants.GlobalConstants.Result.ERRFILE;
            }

            // 01 - > Neutral
            // 02 - > Phase
            cosempkt.AddRange(Constants.DLMSConstants.GlobalNonAMI1PMeterObject[Constants.DLMSConstants.CalCurrentCommandMapper[(byte)mAct]].mclobisatt);
            cosempkt.Add(0x00); //access selector

            Logger.Logger.WriteCalibLog(Constants.DLMSConstants.CalCurrentCommandMapper[(byte)mAct].ToString() + " -->", mpos);

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

                startindex = 18;

                UInt32 valRead = 0;

                byte[] barray = (byte[])basehdlc.dlmsparser(_dlmsmanager.ValidData.ToArray(), ref startindex);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(barray);

                valRead = BitConverter.ToUInt32(barray, 0);

                Logger.Logger.WriteCalibLog("MeterValue - > " + valRead.ToString(), mpos);

                if (valRead < MINCURRENT || valRead > MAXCURRENT)
                {
                    Logger.Logger.WriteCalibLog("MeterValue Out of range -> " + MINCURRENT.ToString() + "/" + MAXCURRENT.ToString(), mpos);
                    return Constants.GlobalConstants.Result.OUT;
                }

                UInt32 valCal = 0;

                if (Constants.DLMSConstants.CalCurrentCommandMapper[(byte)mAct] == DLMSConstants.DLMSCommand.NONAMI1PCALCURRENTN)
                {
                    valCal = (UInt32)((NEUTRALCURRENT) * ICAL * 1000) / (valRead);  

                    factorid = 05;

                }
                else
                {
                    valCal = (UInt32)((PHASECURRENT * ICAL * 1000) / (valRead));

                    factorid = 02;

                }

                Logger.Logger.WriteCalibLog("CalCoeff - > " + valCal.ToString(), mpos);

                cosempkt.Clear();
                cosempkt.AddRange(Constants.DLMSConstants.GlobalNonAMI1PMeterObject[Constants.DLMSConstants.DLMSCommand.NONAMI1PCALIB].mclobisatt);
                cosempkt.Add(0x00);

                byte[] coeffdata = BitConverter.GetBytes(valCal);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(coeffdata);

                cosempkt.Add(0x02);
                cosempkt.Add(0x02);
                cosempkt.Add(0x0F);
                cosempkt.Add(factorid);
                cosempkt.Add(0x06);
                cosempkt.AddRange(coeffdata);
                                
                return _dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET);

            }
            catch (Exception ex)
            {
                Logger.Logger.WriteCalibLog(ex.Message, mpos);
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

            //7e a0 1f 00 02 04 01 81 32 61 eb e6 e6 00 c3 01   ~.......2a......
            //c1 00 09 00 01 0a 08 00 ff 01 01 09 01 00 f3 98................7e


            cosempkt.AddRange(Constants.DLMSConstants.GlobalSM310MeterObject[Constants.DLMSConstants.DLMSCommand.DATARESET].mclobisatt);
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

        #region SETKVARH
        /// <summary>
        /// Method Name: SETKVARH
        /// Description: SETKVARH for BYPL
        /// </summary>        
        public override Constants.GlobalConstants.Result SETKVARH(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            NonDLMSSerialComm serialcomm = new NonDLMSSerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM310RESET);
            NonDLMSManager _nondlmsmanager = new NonDLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();

            /*pack[j++] = 0x7E;
            pack[j++] = 0x30;
            pack[j++] = 0x31;
            pack[j++] = 0x32;
            pack[j++] = 0x33;
            pack[j++] = 0x34;
            pack[j++] = 0x44;
            pack[j++] = 0x35;
            pack[j++] = 0x7e*/

            cosempkt.Add(0x7E);
            cosempkt.Add(0x30);
            cosempkt.Add(0x31);
            cosempkt.Add(0x32);
            cosempkt.Add(0x33);
            cosempkt.Add(0x34);
            cosempkt.Add(0x44);
            cosempkt.Add(0x35);
            cosempkt.Add(0x7e);

            try
            {
                if (!_nondlmsmanager.serialcom.OpenPort())
                {
                    Logger.Logger.WriteCalibLog("Port open error", mpos);
                    return _result;
                }

                if (_nondlmsmanager.serialcom.BYPLSendDataToPort(cosempkt.ToArray(), cosempkt.Count) == false)
                {
                    Logger.Logger.WriteCalibLog("Data not received", mpos);
                    return _result;
                }

                if (_nondlmsmanager.serialcom.ReceiveBuffer[0] == 0x7E
                    && _nondlmsmanager.serialcom.ReceiveBuffer[0] == 0x30
                    && _nondlmsmanager.serialcom.ReceiveBuffer[0] == 0x31
                    && _nondlmsmanager.serialcom.ReceiveBuffer[0] == 0x32
                    && _nondlmsmanager.serialcom.ReceiveBuffer[0] == 0x33
                    && _nondlmsmanager.serialcom.ReceiveBuffer[0] == 0x34
                    && _nondlmsmanager.serialcom.ReceiveBuffer[0] == 0x44
                    && _nondlmsmanager.serialcom.ReceiveBuffer[0] == 0x4F
                    && _nondlmsmanager.serialcom.ReceiveBuffer[0] == 0x7E)
                {
                    _result = Constants.GlobalConstants.Result.Pass;
                }

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
                _nondlmsmanager.serialcom.ClosePort();
            }

            return _result;
        }
        #endregion

        #region SETKVARH
        /// <summary>
        /// Method Name: ResetData
        /// Description: Reset Data from Meter full
        /// </summary>        
        public override Constants.GlobalConstants.Result RESETKVARH(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            NonDLMSSerialComm serialcomm = new NonDLMSSerialComm();
            serialcomm.SetSerialPortSettings(mpos, GlobalConstants.MAXCMDTIMEOUTSM310RESET);
            NonDLMSManager _nondlmsmanager = new NonDLMSManager(serialcomm);
            List<byte> cosempkt = new List<byte>();


            /*pack[j++]=0x7E;
		pack[j++]=0x30;
		pack[j++]=0x31;
		pack[j++]=0x32;
		pack[j++]=0x33;
		pack[j++]=0x34;
		pack[j++]=0x42;
		pack[j++]=0x35;
		pack[j++]=0x7E;*/

            cosempkt.Add(0x7E);
            cosempkt.Add(0x30);
            cosempkt.Add(0x31);
            cosempkt.Add(0x32);
            cosempkt.Add(0x33);
            cosempkt.Add(0x34);
            cosempkt.Add(0x42);
            cosempkt.Add(0x35);
            cosempkt.Add(0x7e);

            try
            {
                if (!_nondlmsmanager.serialcom.OpenPort())
                {
                    Logger.Logger.WriteCalibLog("Port open error", mpos);
                    return _result;
                }

                if (_nondlmsmanager.serialcom.BYPLSendDataToPort(cosempkt.ToArray(), cosempkt.Count) == false)
                {
                    Logger.Logger.WriteCalibLog("Data not received", mpos);
                    return _result;
                }

                if (_nondlmsmanager.serialcom.ReceiveBuffer[0] == 0x7E
                    && _nondlmsmanager.serialcom.ReceiveBuffer[0] == 0x30
                    && _nondlmsmanager.serialcom.ReceiveBuffer[0] == 0x31
                    && _nondlmsmanager.serialcom.ReceiveBuffer[0] == 0x32
                    && _nondlmsmanager.serialcom.ReceiveBuffer[0] == 0x33
                    && _nondlmsmanager.serialcom.ReceiveBuffer[0] == 0x34
                    && _nondlmsmanager.serialcom.ReceiveBuffer[0] == 0x42
                    && _nondlmsmanager.serialcom.ReceiveBuffer[0] == 0x4F
                    && _nondlmsmanager.serialcom.ReceiveBuffer[0] == 0x7E)
                {
                    _result = Constants.GlobalConstants.Result.Pass;
                }

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
                _nondlmsmanager.serialcom.ClosePort();
            }

            return _result;
        }
        #endregion


        public override Constants.GlobalConstants.Result WRITEAESTABLE(int mpos, int mAct)
        {
                                //classid obis attid
            string aesdata = @"<AESTable>,637c777bf26b6fc53001672bfed7ab76 
						ca82c97dfa5947f0add4a2af9ca472c0 
						b7fd9326363ff7cc34a5e5f171d83115 
						04c723c31896059a071280e2eb27b275 
						09832c1a1b6e5aa0523bd6b329e32f84 
						53d100ed20fcb15b6acbbe394a4c58cf 
						d0efaafb434d338545f9027f503c9fa8 
						51a3408f929d38f5bcb6da2110fff3d2 
						cd0c13ec5f974417c4a77e3d645d1973 
						60814fdc222a908846eeb814de5e0bdb 
						e0323a0a4906245cc2d3ac629195e479 
						e7c8376d8dd54ea96c56f4ea657aae08
						ba78252e1ca6b4c6e8dd741f4bbd8b8a 
						703eb5664803f60e613557b986c11d9e 
						e1f8981169d98e949b1e87e9ce5528df 
						8ca1890dbfe6426841992d0fb054bb16
						52096ad53036a538bf40a39e81f3d7fb
						7ce339829b2fff87348e4344c4dee9cb
						547b9432a6c2233dee4c950b42fac34e
						082ea16628d924b2765ba2496d8bd125
						72f8f66486689816d4a45ccc5d65b692
						6c704850fdedb9da5e154657a78d9d84
						90d8ab008cbcd30af7e45805b8b34506
						d02c1e8fca3f0f02c1afbd0301138a6b
						3a9111414f67dcea97f2cfcef0b4e673
						96ac7422e7ad3585e2f937e81c75df6e
						47f11a711d29c5896fb7620eaa18be1b
						fc563e4bc6d279209adbc0fe78cd5af4
						1fdda8338807c731b11210592780ec5f
						60517fa919b54a0d2de57a9f93c99cef
						a0e03b4dae2af5b0c8ebbb3c83539961
						172b047eba77d626e169146355210c7d
						8d01020408102040801b366cd8ab4d9a
						2f5ebc63c697356ad4b37dfaefc59139
						72e4d3bd61c29f254a943366cc831d3a
						74e8cb8d01020408102040801b366cd8
						ab4d9a2f5ebc63c697356ad4b37dfaef
						c5913972e4d3bd61c29f254a943366cc
						831d3a74e8cb8d01020408102040801b
						366cd8ab4d9a2f5ebc63c697356ad4b3
						7dfaefc5913972e4d3bd61c29f254a94
						3366cc831d3a74e8cb8d010204081020
						40801b366cd8ab4d9a2f5ebc63c69735
						6ad4b37dfaefc5913972e4d3bd61c29f
						254a943366cc831d3a74e8cb8d010204
						08102040801b366cd8ab4d9a2f5ebc63
						c697356ad4b37dfaefc5913972e4d3bd
						61c29f254a943366cc831d3a74e8cb,</AESTable>";

            aesdata = aesdata.Replace("\r\n", "");

            aesdata = aesdata.Replace(" ", "");

            aesdata = aesdata.Replace("\t", "");



            // TODO : Variables declaration and initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);

            
            if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass) return Constants.GlobalConstants.Result.Fail;

            try
            {

                _result = _dlmsmanager.WriteSetAesTable(aesdata);
                    
                 Logger.Logger.WriteCalibLog("AESTABLE  -> Pass", serialcomm.mpos);
          
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
