///****************************************************************************
//'*
//'*  Projet       : Falcon
//'*
//'*  Component    : MMP
//'*
//'*  Module       : Serial Communication
//'*
//'*  Environment  : Visual Studio 2008 - C#.net
//'*
//'*------+----------+------------------------------------------------------------
//'*Vers |   Date    |    Programmer and Comments
//'*------+----------+------------------------------------------------------------
//'* 1.00 | 10/08/13 | Bal Govind Gupta : creation.
//'*------+----------+------------------------------------------------------------
//'*      |          | XXXXX: Change Details
//'******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Xml;
using System.Threading;

namespace SerialCommunication
{
    public class IECMeterSerialCommunication
    {

        public const int ReceiveBuffer_Size = 100000;
        public byte[] ReceiveBuffer = new byte[ReceiveBuffer_Size];
        public int bufferIndex = 0;
        public byte bCommType = 0;
        int nCount = 0;        
        public SerialPort comPort;
        private string _baudRate;
        private string _signonbaudRate;
        private string _parity;
        private string _stopBits;
        private string _dataBits;
        private string _portName;
        private string _command;
        private long _cmriWaitTimeout;
        private int _cmriPktTimeout;
        private int _commandTimeout;
        private int _intercharacterDelay;
        private int _intercommandDelay = 0;
        private byte _responseTerminationByte;
        private byte _2ndresponseTerminationByte;
        private byte[] _commandDataBytes;

        public int Pktflg = 0;
        public string strOutBuff = "";
        public int commandIndex = 0;
        public long commCount = 0;
        public bool flgReadFlag = false;
        public bool flgDataReceived;
        public long elapsedMilliseconds;
        public long timeout;
        public int pktCount = 0;
        public DateTime TimeStamp;

        private byte _iecSignonType;
       

        public int NoOfBytesToBeReceive3PHDLMSCalibCoeff = 0;

        public IECMeterSerialCommunication()
        {
            _baudRate = string.Empty;
            _parity = string.Empty;
            _stopBits = string.Empty;
            _dataBits = string.Empty;
            _command = string.Empty;
            _cmriWaitTimeout = 0;
            _cmriPktTimeout = 0;
            _commandTimeout = 0;
            _intercharacterDelay = 0;
            _intercommandDelay = 0;
            flgDataReceived = false;
            comPort = new SerialPort();            
            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
        }
              
        public string BaudRate
        {
            get { return _baudRate; }
            set { _baudRate = value; }
        }

        public string SignonbaudRate
        {
            get { return _signonbaudRate; }
            set { _signonbaudRate = value; }
        }

        public string Parity
        {
            get { return _parity; }
            set { _parity = value; }
        }

        public string StopBits
        {
            get { return _stopBits; }
            set { _stopBits = value; }
        }

        public string DataBits
        {
            get { return _dataBits; }
            set { _dataBits = value; }
        }

        public string PortName
        {
            get { return _portName; }
            set { _portName = value; }
        }

        public string Command
        {
            get { return _command; }
            set { _command = value; }
        }

        public int CommandTimeout
        {
            get { return _commandTimeout; }
            set { _commandTimeout = value; }
        }

        public int InterchatracterDelay
        {
            get { return _intercharacterDelay; }
            set { _intercharacterDelay = value; }
        }

        public int IntercommandDelay
        {
            get { return _intercommandDelay; }
            set { _intercommandDelay = value; }
        }

        public long CMRIWaitTimeout
        {
            get { return _cmriWaitTimeout; }
            set { _cmriWaitTimeout = value; }
        }

        public int CMRIPktTimeout
        {
            get { return _cmriPktTimeout; }
            set { _cmriPktTimeout = value; }
        }

       
        public byte[] _CommandDataBytes 
        {
            get { return _commandDataBytes; }
            set { _commandDataBytes = value; }
        
        }        
        public byte   _CommandResponseStopByte
        {
            get { return _responseTerminationByte; }
            set { _responseTerminationByte = value; }
        }
        public byte _2NdCommandResponseStopByte
        {
            get { return _2ndresponseTerminationByte; }
            set { _2ndresponseTerminationByte = value; }
        }
        public byte IECSignonType
        {
            get { return _iecSignonType; }
            set { _iecSignonType = value; }
        }
 
        internal void WriteData(string msg)
        {
            if (!(comPort.IsOpen == true)) comPort.Open();
            comPort.Write(msg);
        }

        internal void WriteData(byte[] msg, int offset, int count)
        {
            int idxCount=0;
            if (!(comPort.IsOpen == true)) comPort.Open();
            comPort.Write(msg, idxCount, count);             
        }
     
        public bool OpenPort()
        {

            if (comPort.IsOpen) comPort.Close();

            comPort.BaudRate = int.Parse(this.SignonbaudRate);    //BaudRate
            comPort.DataBits = int.Parse(this.DataBits);          //DataBits
            comPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), this.StopBits);    //StopBits
            comPort.Parity = (Parity)Enum.Parse(typeof(Parity), this.Parity);    //Parity
            comPort.PortName = this.PortName;  //PortName
            //now open the port
            comPort.RtsEnable = true;
            comPort.DtrEnable = true;
            try
            {
                comPort.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
     
        public bool ClosePort()
        {
            try
            {
                if (comPort.IsOpen == true) comPort.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
      
        internal string[] SetParity()
        {
            return (Enum.GetNames(typeof(Parity)));
        }
     
        internal string[] StopBit()
        {
            return (Enum.GetNames(typeof(StopBits)));
        }
      
        public string[] GetAvailablePorts()
        {
            return (SerialPort.GetPortNames());
        }
                
        public void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {          

            flgDataReceived = true;
            timeout = this.InterchatracterDelay;
            TimeStamp = DateTime.Now;
            nCount = comPort.BytesToRead;
            if (nCount < 0) return;
            comPort.Read(ReceiveBuffer,bufferIndex,nCount);
            bufferIndex = bufferIndex + nCount;
            if (bCommType == 0x00)
            {
                if (bufferIndex >= 2) { if (ReceiveBuffer[bufferIndex - 2] == _responseTerminationByte || ReceiveBuffer[bufferIndex - 2] == _2NdCommandResponseStopByte) flgReadFlag = true; }
                else if (bufferIndex == 1) { if (ReceiveBuffer[bufferIndex - 1] == _responseTerminationByte || ReceiveBuffer[bufferIndex - 1] == _2NdCommandResponseStopByte) flgReadFlag = true; }
               
            }
            
        }       
      
        public bool fSendDataToPort()
        {
            try
            {
                int nRetries = 1;
                while (nRetries-- > 0)
                {

                    flgReadFlag = false;
                    bufferIndex = 0;
                    strOutBuff = string.Empty;
                    ReceiveBuffer = new byte[ReceiveBuffer_Size];
                    flgDataReceived = false;
                    WriteData(_CommandDataBytes, 0, _CommandDataBytes.Length);
                    TimeStamp = DateTime.Now;
                    timeout = CommandTimeout;
                    do
                    {

                        Thread.Sleep(2);
                        if (Timeout() == 1) break;
                        if (flgReadFlag) return true;
                        if (ReceiveBuffer[0] == _responseTerminationByte && ReceiveBuffer[1] == _2NdCommandResponseStopByte) flgReadFlag = true;

                    } while (true);

                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                int countbytes = 0;
                while (countbytes < bufferIndex)
                {
                    byte tembyte = (byte)(ReceiveBuffer[countbytes++] & 0x7F);
                    strOutBuff += (char)tembyte;
                }
            }

        }

        public bool ManfCommandAccess()
        {
            int nRetries = 1;
            while (nRetries-- > 0)
            {

                flgReadFlag = false;
                bufferIndex = 0;
                strOutBuff = string.Empty;
                ReceiveBuffer = new byte[ReceiveBuffer_Size];
                flgDataReceived = false;
                WriteData(_CommandDataBytes, 0, _CommandDataBytes.Length);
                Thread.Sleep(200);
                comPort.BaudRate = int.Parse(_baudRate);
                Thread.Sleep(200);
                TimeStamp = DateTime.Now;
                timeout = CommandTimeout;
                do
                {

                    Thread.Sleep(2);
                   
                    if (flgReadFlag)
                    {
                        int countbytes = 0;
                        while (countbytes < bufferIndex)
                        {
                            byte tembyte = (byte)(ReceiveBuffer[countbytes++] & 0x7F);
                            strOutBuff += (char)tembyte;
                        }

                        if (GetBcc(ReceiveBuffer, 1, bufferIndex - 1) != ReceiveBuffer[countbytes - 1]) return false; ;
                        return true;
                    }

                } while (Timeout() != 1);

            }
            return false;

        }

        public void SetSerialPortSettings(String Port, String baud, String parity, String dataBits, String nStopBits, int CmdTimeout, int intercharDelay, String signonbaud)
        {
            PortName = Port;
            BaudRate = baud;
            Parity = parity;
            DataBits = dataBits;
            StopBits = nStopBits;
            CommandTimeout = CmdTimeout;
            InterchatracterDelay = intercharDelay;
            SignonbaudRate = signonbaud;
        }
       
        public void DelayExecution(int millisecondsTime)
        {
            DateTime end = DateTime.UtcNow.AddMilliseconds(millisecondsTime);
            while (DateTime.UtcNow < end)
            {
            }
        }

        public int Timeout()
        {

            long elapsedTime;
            elapsedTime = DateTime.Now.Ticks - TimeStamp.Ticks;
            TimeSpan objTimeSpan = new TimeSpan(elapsedTime);
            elapsedMilliseconds = Convert.ToInt64(objTimeSpan.TotalMilliseconds); 
            if (elapsedMilliseconds > timeout) return 1;
            else return 0; 
               
        }

        public bool TeaAlgorithm(string strseed)
        {
            uint y = 0;
            
            strseed =strseed.Substring(strseed.IndexOf("(")+1 , 8);
            try
            {
                //strseed = "8C1FE0DA";
                /* this key is specific to the CCR meter */
                uint seed = Convert.ToUInt32(strseed, 16);//0xC903CA6C;// 
                uint[] Key = { 0x9c178e52, 0x1ec2a690, 0xfb34508d, 0x359e2697 };    //Algo Standard value
                uint Delta = 0x9e3779b9;
                uint z;
                uint Sum = 0;
                uint n = 32;
                //char n = Convert.ToChar(32);  /* a key schedule constant */
                uint[] EncryptData = new uint[2];
                y = seed;
                z = 100 - seed;

                while (n-- > 0)
                {
                    /* basic cycle start */
                    y += ((z << 4) ^ (z >> 5)) + z ^ Sum + Key[Sum & 3];
                    Sum += Delta;
                    z += ((y << 4) ^ (y >> 5)) + y ^ Sum + Key[(Sum >> 11) & 3];
                    /* end cycle */
                }
                EncryptData[0] = y;
                EncryptData[1] = z;
                string Hexcmd = y.ToString("X");
                Hexcmd = Hexcmd.PadLeft(8,'0');
                byte[] asciiBytes = Encoding.ASCII.GetBytes(Hexcmd);
                _CommandDataBytes[5] = asciiBytes[0];
                _CommandDataBytes[6] = asciiBytes[1];
                _CommandDataBytes[7] = asciiBytes[2];
                _CommandDataBytes[8] = asciiBytes[3];
                _CommandDataBytes[9] = asciiBytes[4];
                _CommandDataBytes[10] = asciiBytes[5];
                _CommandDataBytes[11] = asciiBytes[6];
                _CommandDataBytes[12] = asciiBytes[7];
 
                return true;
            }
            catch (Exception)
            {
                return false;

            }
            
        }

        public Byte GetBcc(byte[] RecInpData, int staridex, int endidex)
        {
            try
            {
                int Bcc = 0;
                int bCount=0;
                foreach (byte b in RecInpData)
                {
                    if (bCount++ >= staridex) Bcc = Bcc ^ b;
                    if (bCount >= endidex) break;
                }
                return (byte)Bcc;
            }
            catch (Exception)
            {

                return 0xFF;
            }
        }
 
    }

   
    
}