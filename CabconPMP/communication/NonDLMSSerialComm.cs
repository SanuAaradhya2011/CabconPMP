using SmartCalibration.Constants;
using SmartCalibration.Logger;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SmartCalibration.Logger.Logger;

namespace LNG.Communication.SerialCommunication
{
    public class NonDLMSSerialComm
    {

        public const int ReceiveBuffer_Size = 2048;
        public byte[] ReceiveBuffer = new byte[ReceiveBuffer_Size];
        public int bufferIndex = 0;
        public byte bCommType = 0;
        private byte _responseTerminationByte;
        private byte _2ndresponseTerminationByte;

        public byte _CommandResponseStopByte
        {
            get { return _responseTerminationByte; }
            set { _responseTerminationByte = value; }
        }
        public byte _2NdCommandResponseStopByte
        {
            get { return _2ndresponseTerminationByte; }
            set { _2ndresponseTerminationByte = value; }
        }

        int nCount = 0;
        public SerialPort comPort;
        private string _baudRate;
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
        
        public int NoOfBytesToBeReceive3PHDLMSCalibCoeff = 0;

        public int mpos { get; set; }

        private object synobject = new object();

        public byte ncmdindex;

        public NonDLMSSerialComm()
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

        internal void WriteData(string msg)
        {
            if (!(comPort.IsOpen == true)) comPort.Open();
            comPort.Write(msg);
        }

        internal void WriteData(byte[] msg, int offset, int count)
        {
            int idxCount = 0;
            if (!(comPort.IsOpen == true)) comPort.Open();
            comPort.Write(msg, idxCount, count);
        }

        public bool OpenPort()
        {
            {
                if (comPort.IsOpen) comPort.Close();

                comPort.BaudRate = int.Parse(this.BaudRate);
                comPort.DataBits = int.Parse(this.DataBits);
                comPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), this.StopBits);
                comPort.Parity = (Parity)Enum.Parse(typeof(Parity), this.Parity);
                comPort.PortName = this.PortName;
                comPort.RtsEnable = true;
                comPort.DtrEnable = true;
                try
                {
                    comPort.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool ClosePort()
        {
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
            comPort.Read(ReceiveBuffer, bufferIndex, nCount);
            bufferIndex = bufferIndex + nCount;
            
            if (bufferIndex >= 2) { if (ReceiveBuffer[bufferIndex - 2] == _responseTerminationByte || 
                    ReceiveBuffer[bufferIndex - 2] == _2NdCommandResponseStopByte) flgReadFlag = true; }
            else if (bufferIndex == 1)
            {
                if (ReceiveBuffer[bufferIndex - 1] == 0x06 || ReceiveBuffer[bufferIndex - 1] == 0x15) flgReadFlag = true;

                if (ReceiveBuffer[bufferIndex - 1] == _responseTerminationByte || ReceiveBuffer[bufferIndex - 1] == _2NdCommandResponseStopByte) flgReadFlag = true;
            }


        }

        
        public bool SendDataToPort(byte[] Buffer, int nLength)
        {
            try
            {
                int nRetries = 3;
                while (nRetries-- > 0)
                {

                    flgReadFlag = false;
                    bufferIndex = 0;
                    strOutBuff = string.Empty;
                    ReceiveBuffer = new byte[ReceiveBuffer_Size];
                    flgDataReceived = false;
                    WriteData(Buffer, 0, nLength);
                    Logger.WriteComLog(Buffer, nLength, mpos, ActionMode.TX);
                    TimeStamp = DateTime.Now;
                    timeout = CommandTimeout;
                    do
                    {

                        Thread.Sleep(2);
                        if (Timeout() == 1) break;
                        if (flgReadFlag) return true;
                        if (ReceiveBuffer[0] == 0x06 || ReceiveBuffer[0] == 0x15) flgReadFlag = true;
                        if (ReceiveBuffer[0] == _responseTerminationByte && ReceiveBuffer[1] == _2NdCommandResponseStopByte) flgReadFlag = true;

                    } while (true);

                    Logger.WriteComLog(ReceiveBuffer, bufferIndex, mpos, ActionMode.RX);

                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                
            }
        }


        public bool BYPLSendDataToPort(byte[] Buffer, int nLength)
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
                    WriteData(Buffer, 0, nLength);
                    Logger.WriteComLog(Buffer, nLength, mpos, ActionMode.TX);
                    Thread.Sleep(150);
                    TimeStamp = DateTime.Now;
                    timeout = CommandTimeout;
                    do
                    {

                        Thread.Sleep(2);
                        if (Timeout() == 1) break;
                        if (flgReadFlag) return true;
                        if(bufferIndex >= 9 )
                        {
                            flgReadFlag = true;
                            break;
                        }

                    } while (true);

                    Logger.WriteComLog(ReceiveBuffer, bufferIndex, mpos, ActionMode.RX);
                }

               

                return flgReadFlag;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {

            }
        }

        public void SetSerialPortSettings(int Pos, int CmdTimeOut)
        {
            mpos = Pos;
            if (GlobalConstants.MeterPortMap != null && GlobalConstants.MeterPortMap.ContainsKey(Pos))
            {
                PortName = GlobalConstants.MeterPortMap[Pos];
            }
            else
            {
                PortName = "COM" + (Pos + 6).ToString();
            }
            BaudRate = "9600";
            Parity = "None";
            DataBits = "8";
            StopBits = "1";
            CommandTimeout = CmdTimeOut;
            InterchatracterDelay = 1000;
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

    }

}
