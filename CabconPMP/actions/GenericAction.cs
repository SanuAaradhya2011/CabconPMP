using IntegratedCalibration.DataLayer;
using LNG.Communication.SerialCommunication;
using SmartCalibration.Communication;
using SmartCalibration.DataLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;

namespace SmartCalibration.Actions
{
    /// <summary>
    /// This is class contains generic methods common to meters. Works as Parent class for all action classes i.e SM110, SM310
    /// Author : Mohsin Raza
    /// </summary>
    public class GenericAction
    {
        // Variable  

        //public int mAct { get; set; }
        Object thisLock = new Object();
        public object mpArg { get; set; }
        public object meterdetail { get; set; }
        //public int mpos { get; set; }
        public List<ConfigureData> csvconfigureData { get; set; }
        public int cmddelay { get; set; }

        public UInt16 m_temprature;

        public double mresulterror;

        public GenericAction()
        {
            cmddelay = 3000;
        }


        #region SETCLOCK
        /// <summary>
        /// Method Name: SetClock
        /// Description: Update RTC of the meter
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public virtual Constants.GlobalConstants.Result SETCLOCK(int mpos, int mAct)
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

        #region GETMSN
        /// <summary>
        /// Method Name: GETMSN
        /// Description: Get PCBA number from meter and write in imp.res, 01 Means - GetPCBA, 02- Get Meter Serial Number (Owner Number)
        /// Author:      Mohsin Raza
        /// Date:        30-Oct-2015
        /// </summary>
        public virtual Constants.GlobalConstants.Result GETMSN(int mpos, int mAct)
        {
            // TODO : Variables declaration and initialisation here

            string tagname = "[ManufacturerNo] = \"";
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            int indexvalue = 18;

            //File.WriteAllText(mpos + ".txt", "Port" + mpos.ToString());

            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);

            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalGenericMeterObject[Constants.DLMSConstants.GetNumberCommandMapper[(byte)mAct]].mclobisatt);

            if(Constants.DLMSConstants.GetNumberCommandMapper[(byte)mAct] == Constants.DLMSConstants.DLMSCommand.GETMETERID) tagname = "[OwnerNo] = \"";

            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                    return _result;


                if (_dlmsmanager.NormalGetPacket(cosempkt, Constants.DLMSConstants.GetNumberCommandMapper[(byte)mAct]) != Constants.GlobalConstants.Result.Pass)
                   return _result;

                byte[] bdata = (byte[])basehdlc.dlmsparser(_dlmsmanager.ValidData.ToArray(), ref indexvalue);

                meterdetail = System.Text.Encoding.UTF8.GetString(bdata);

                string strdata = "[Pos] = " + mpos.ToString() + "\n";

                meterdetail = strdata + tagname + meterdetail + "\"";

                Constants.GlobalConstants.MSNdata[mpos] = (string)meterdetail;

                //lock(thisLock)
                //{
                //    DoIt(null, Constants.GlobalConstants.IMPORTFILE, (string)meterdetail);
                //}

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
        public virtual Constants.GlobalConstants.Result GETTRAVELLER(int mpos, int mAct)
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

                if(_dlmsmanager.serialcom.ReceiveBuffer[28] == 0x02 && (_dlmsmanager.serialcom.ReceiveBuffer[29] == 1 || _dlmsmanager.serialcom.ReceiveBuffer[29] == 17))
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
        public virtual Constants.GlobalConstants.Result SETTRAVELLER(int mpos, int mAct)
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

            if(_amitraveller.ReadTravellerStatus(serialcomm.mpos) == true)
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

        #region RESETDATA
        /// <summary>
        /// Method Name: RESETDATA
        /// Description: Reset Meter Data
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public virtual Constants.GlobalConstants.Result RESETDATA(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            return _result;
        }
        #endregion

        #region CONFIGURE
        /// <summary>
        /// Method Name: Configuration
        /// Description: Write csv file in to the meters 
        /// Author:      Mohsin Raza
        /// </summary>
        public virtual Constants.GlobalConstants.Result CONFIGURE(int mpos, int mAct)
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
                    serialcomm.CommandTimeout = 3000;

                    // For these tags, delay should be at least 15 Sec
                    if (mytagname.Contains("SurveyIP") || mytagname.Contains("LSCaptureobject") || mytagname.Contains("MeteringMode")) serialcomm.CommandTimeout = Constants.DLMSConstants.MAXTIMEOUT;

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

                    if (_result == Constants.GlobalConstants.Result.Fail)
                    {
                        Logger.Logger.WriteCalibLog(mytagname + "-> Fail", serialcomm.mpos);
                        break;
                    }

                    Logger.Logger.WriteCalibLog(mytagname + "-> Pass", serialcomm.mpos);
                }

                
            }
            catch(Exception ex)
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

        #region RESETPHASE
        /// <summary>
        /// Method Name: RESETPHASE
        /// Description: 
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public virtual Constants.GlobalConstants.Result RESETPHASE(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            return _result;
        }
        #endregion

        #region INITPHASE
        /// <summary>
        /// Method Name: INITPHASE
        /// Description: 
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public virtual Constants.GlobalConstants.Result INITPHASE(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            return _result;
        }
        #endregion

        #region CALIBPHASE
        /// <summary>
        /// Method Name: CALIBPHASE
        /// Description: 
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public virtual Constants.GlobalConstants.Result CALIBPHASE(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            return _result;
        }
        #endregion

        #region RESETNEUTRAL
        /// <summary>
        /// Method Name: RESETNEUTRAL
        /// Description: 
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public virtual Constants.GlobalConstants.Result RESETNEUTRAL(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            return _result;
        }
        #endregion

        #region INITNEUTRAL
        /// <summary>
        /// Method Name: INITNEUTRAL
        /// Description: 
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public virtual Constants.GlobalConstants.Result INITNEUTRAL(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            return _result;
        }
        #endregion

        #region CALIBNEUTRAL
        /// <summary>
        /// Method Name: CALIBNEUTRAL
        /// Description: 
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public virtual Constants.GlobalConstants.Result CALIBNEUTRAL(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            return _result;
        }
        #endregion

        #region CALIBTEMP
        /// <summary>
        /// Method Name: CALIBTEMP
        /// Description: 
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public virtual Constants.GlobalConstants.Result CALIBTEMP(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            return _result;
        }
        #endregion

        #region CALIBVERIFYPH
        /// <summary>
        /// Method Name: CALIBVERIFYP
        /// Description: 
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public virtual Constants.GlobalConstants.Result CALIBVERIFYPH(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            return _result;
        }
        #endregion

        #region CALIBVERIFYN
        /// <summary>
        /// Method Name: CALIBVERIFYP
        /// Description: 
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public virtual Constants.GlobalConstants.Result CALIBVERIFYN(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            return _result;
        }
        #endregion

        #region WRITEDEFAULT
        /// <summary>
        /// Method Name: RESETPHASE
        /// Description: 
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public virtual Constants.GlobalConstants.Result WRITEDEFAULT(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            return _result;
        }
        #endregion

        #region WRITEDEFAULTPHASE
        /// <summary>
        /// Method Name: RESETPHASE
        /// Description: 
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public virtual Constants.GlobalConstants.Result WRITEDEFAULTPHASE(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            return _result;
        }
        #endregion

        #region WRITEDEFAULTNEUTRAL
        /// <summary>
        /// Method Name: RESETPHASE
        /// Description: 
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public virtual Constants.GlobalConstants.Result WRITEDEFAULTNEUTRAL(int mpos, int mAct)
        {
            // TODO  : Variables declaration and Initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            return _result;
        }
        #endregion

        #region SETPWD
        /// <summary>
        /// Method Name: SETPWD
        /// Description: Write US and MR default Password
        /// Author:      Mohsin Raza
        /// Date:        22-JAN-2020
        /// </summary>        
        public virtual Constants.GlobalConstants.Result SETPWD(int mpos, int mAct)
        {
            // TODO : Variables declaration and initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            SerialComm serialcomm = new SerialComm();
            DLMSManager _dlmsmanager = new DLMSManager(serialcomm);
            serialcomm.SetSerialPortSettings(mpos, cmddelay);
            List<byte> cosempkt = new List<byte>();

            cosempkt.AddRange(Constants.DLMSConstants.GlobalGenericMeterObject[Constants.DLMSConstants.DLMSCommand.SMSETMRPWD].mclobisatt);

            cosempkt.AddRange(GetMRPWD((string)mpArg));

            try
            {
                if (_dlmsmanager.ConnectToMeter() != Constants.GlobalConstants.Result.Pass)
                    return _result;

                if (_dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET) != Constants.GlobalConstants.Result.Pass)
                    return _result;

                cosempkt.Clear();

                cosempkt.AddRange(Constants.DLMSConstants.GlobalGenericMeterObject[Constants.DLMSConstants.DLMSCommand.SMSETUSPWD].mclobisatt);

                cosempkt.AddRange(GetUSPWD((string)mpArg));

                return _dlmsmanager.NormalSetPacket(cosempkt, Constants.DLMSConstants.DLMSCommand.NORMALSET);
         
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

        #region GetMRPWD
        private List<byte> GetMRPWD(string strpwd)
        {
            List<byte> pwdbytes = new List<byte>();
            string[] meterpwd = strpwd.Split(':');
            if (meterpwd == null || meterpwd.Count() < 2) return pwdbytes;
            pwdbytes.Add(0x00);
            pwdbytes.Add(0x09);
            pwdbytes.Add((byte)meterpwd[0].Length);
            pwdbytes.AddRange(ASCIIEncoding.ASCII.GetBytes(meterpwd[0]));
            return pwdbytes;
        }
        #endregion

        #region GetUSPWD
        private List<byte> GetUSPWD(string strpwd)
        {
            strpwd = strpwd.ToUpper();
            List<byte> pwdbytes = new List<byte>();
            string[] meterpwd = strpwd.Split(':');
            if (meterpwd == null || meterpwd.Count() < 2) return pwdbytes;
            Dictionary<string, byte> asciitohexmapper = new Dictionary<string, byte>()
            {
                {"00", 0x00 },
                {"01", 0x01 },
                {"02", 0x02 },
                {"03", 0x03 },
                {"04", 0x04 },
                {"05", 0x05 },
                {"06", 0x06 },
                {"07", 0x07 },
                {"08", 0x08},
                {"09", 0x09 },
                {"0A", 0x0A },
                {"0B", 0x0B },
                {"0C", 0x0C },
                {"0D", 0x0D },
                {"0E", 0x0E },
                {"0F", 0x0F },
                
            };

            pwdbytes.Add(0x01);
            pwdbytes.Add(0x09);
            pwdbytes.Add((byte)((byte)meterpwd[1].Length/2));
            for (int icount = 0; icount < (byte)meterpwd[1].Length; icount++)
            {
                pwdbytes.Add(asciitohexmapper[meterpwd[1].Substring(icount, 2)]);
                icount++;
            }
            return pwdbytes;
        }
        #endregion

        // SM310 Method to Override
        #region CALIBRESET
        public virtual Constants.GlobalConstants.Result CALIBRESET(int mpos, int mAct)
        {
            return Constants.GlobalConstants.Result.Fail;
        }
        #endregion

        // SM310 Method to Override
        #region CALIBACTIVE
        public virtual Constants.GlobalConstants.Result CALIBACTIVE(int mpos, int mAct)
        {
            return Constants.GlobalConstants.Result.Pass;
        }
        #endregion

        // SM310 Method to Override
        #region CALIBREACTIVE
        public virtual Constants.GlobalConstants.Result CALIBREACTIVE(int mpos, int mAct)
        {
            return Constants.GlobalConstants.Result.Pass;
        }
        #endregion

        // SM310 Method to Override
        #region CALIBFVI
        public virtual Constants.GlobalConstants.Result CALIBFVI(int mpos, int mAct)
        {
            return Constants.GlobalConstants.Result.Pass;
        }
        #endregion
        
        #region DoIt
        /// <summary>
        /// Method Name: DoIt
        /// Description: Update Files for GetMSN method, Thread safe
        /// Author:      Mohsin Raza
        /// Date:        30-Oct-2015
        /// </summary>
        protected void DoIt(object p, string FileName, string stddata)
        {
            using (FileStream fs = new FileStream(FileName, FileMode.Open, FileSystemRights.AppendData,
                FileShare.Write, 4096, FileOptions.None))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.AutoFlush = true;
                    writer.WriteLine(stddata);
                }
            }
        }
        #endregion

        #region CALIBCURRENT
        public virtual Constants.GlobalConstants.Result CALIBCURRENT(int mpos, int mAct)
        {
            return Constants.GlobalConstants.Result.Fail;
        }
        #endregion

        // SM310 Method to Override
        #region WRITEAES VIM
        public virtual Constants.GlobalConstants.Result WRITEAESTABLE(int mpos, int mAct)
        {
            return Constants.GlobalConstants.Result.Pass;
        }
        #endregion

        #region BOOTLOADER
        /// <summary>
        /// Method Name: BOOTLOADER
        /// Description: Write TXT file in to the meters 
        /// Author:      Mohsin Raza
        /// </summary>
        public virtual Constants.GlobalConstants.Result BOOTLOADER(int mpos, int mAct)
        {
            // TODO : Variables declaration and initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            return _result;
        }
        #endregion

        #region BOOTLOADER
        /// <summary>
        /// Method Name: BOOTLOADER
        /// Description: Write TXT file in to the meters 
        /// Author:      Mohsin Raza
        /// </summary>
        public virtual Constants.GlobalConstants.Result BOOTLOADERREPAIR(int mpos, int mAct)
        {
            // TODO : Variables declaration and initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            return _result;
        }
        #endregion
                
        #region SETKVARH
        /// <summary>
        /// Method Name: SETKVARH
        /// Description: SETKVARH 
        /// Author:      Mohsin Raza
        /// </summary>
        public virtual Constants.GlobalConstants.Result SETKVARH(int mpos, int mAct)
        {
            // TODO : Variables declaration and initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            return _result;
        }
        #endregion

        #region RESETKVARH
        /// <summary>
        /// Method Name: RESETKVARH
        /// Description: RESETKVARH 
        /// Author:      Mohsin Raza
        /// </summary>
        public virtual Constants.GlobalConstants.Result RESETKVARH(int mpos, int mAct)
        {
            // TODO : Variables declaration and initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            return _result;
        }
        #endregion

        public virtual Constants.GlobalConstants.Result CALIBVERIFY(int mpos, int mAct)
        {
            // TODO : Variables declaration and initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            return _result;
        }

        public virtual Constants.GlobalConstants.Result GETCONFIGURE(int mpos, int mAct)
        {
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
                    List<byte> tempBytes = IntegratedCalibration.Constants.GenericMethods.ConvertStrToByte(CommandBytes);
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
                            Array.Copy(_dlmsmanager.ValidData.ToArray(), 18, bdata, 0, bdata.Length);
                            CommandBytes = CommandBytes + IntegratedCalibration.Constants.GenericMethods.GetByteToHexString(bdata);
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
                            Array.Copy(_dlmsmanager.serialcom.BlockBuffer, 0, bdata, 0, bdata.Length);
                            CommandBytes = IntegratedCalibration.Constants.GenericMethods.GetByteToHexString(bdata);
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

        #region BOOTLOADERVERIFY for BIN
        /// <summary>
        /// Method Name: BOOTLOADER
        /// Description: Write TXT file in to the meters 
        /// Author:      Mohsin Raza
        /// </summary>
        public virtual Constants.GlobalConstants.Result IMAGEVERIFY(int mpos, int mAct)
        {
            // TODO : Variables declaration and initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            return _result;
        }
        #endregion

        #region IMAGEACTIVATION for BIN
        /// <summary>
        /// Method Name: BOOTLOADER
        /// Description: Write TXT file in to the meters 
        /// Author:      Mohsin Raza
        /// </summary>
        public virtual Constants.GlobalConstants.Result IMAGEACTIVATION(int mpos, int mAct)
        {
            // TODO : Variables declaration and initialisation here
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Fail;
            return _result;
        }
        #endregion
    }
}
