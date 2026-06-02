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
    public class SerialComm
    {

        public const int ReceiveBuffer_Size = 2048;
        public byte[] ReceiveBuffer = new byte[ReceiveBuffer_Size];
        public int bufferIndex = 0;
        public byte bCommType = 0;
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
       
        public SerialComm()
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
            int idxCount=0;
            if (!(comPort.IsOpen == true)) comPort.Open();
            comPort.Write(msg, idxCount, count);             
        }
     
        public bool OpenPort()
        {

            if (comPort.IsOpen) comPort.Close();
            
            comPort.BaudRate = int.Parse(this.BaudRate);    //BaudRate
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
            if (nCount < 0 ) return;
          
            comPort.Read(ReceiveBuffer,bufferIndex,nCount);
            if (bCommType == 0x00)
            {

               
               int lengetz = (byte)(ReceiveBuffer[1] & 0x7) * 0x100 + (byte)(ReceiveBuffer[2]);
               if (lengetz < ReceiveBuffer_Size)
               {
                   if (ReceiveBuffer[lengetz + 1] == 0x7E) flgReadFlag = true;
               }
            }
            else  if (nCount >= 5)
            {
                flgReadFlag = true; bCommType = 0x00;                
            }
            bufferIndex = bufferIndex + nCount;

            if (NoOfBytesToBeReceive3PHDLMSCalibCoeff > 0)
            {
                if (bufferIndex >= NoOfBytesToBeReceive3PHDLMSCalibCoeff) flgReadFlag = true;
            }
            
        }

        public bool fSendDataToPort(byte[] Buffer, int nLength)
        {
            
            int nRetries = 1;
            int discCommandByte = 8;  
            while (nRetries-- > 0)
            {
                
                flgReadFlag = false;
                bufferIndex = 0;
                strOutBuff = string.Empty;
                ReceiveBuffer = new byte[ReceiveBuffer_Size];
                flgDataReceived = false;               
                WriteData(Buffer, 0, nLength);             
                TimeStamp = DateTime.Now;
                timeout = CommandTimeout;                
                do
                {
                   //if (Buffer[discCommandByte] == 83) return true;
                    Thread.Sleep(2);
                    if (flgReadFlag) return true; ;
                    if (Timeout() == 1)break;
                } while (true);

                //*************************************************************************
                SerialComm result = null;
                byte[] destination = new byte[nLength];

                Array.Copy(Buffer, destination, nLength);
                string hexString = BitConverter.ToString(destination).Replace("-", " ");
                string filePath = "Cabcon_output.txt";
                File.AppendAllText(filePath, "Send " + DateTime.Now.ToShortDateString() + ":->" + hexString + "\n");

                //result = ReceiveBuffer;
                //string abc1 = result1.ToString();

                // hexString = BitConverter.ToString(abc1.Replace("-", " ");
                //File.AppendAllText(filePath, "Receive " + DateTime.Now.ToShortTimeString() + ":->" + hexString + "\n");
                //*************************************************************************

                if (Buffer[discCommandByte] == 83) break;
            }
            return false;

        }


        public bool fSendIrDADataToPort(byte[] Buffer, int nLength)
        {
            int nRetries = 1;
            int discCommandByte = 8;
            int lenDatatobeReceive = 0;
            while (nRetries-- > 0)
            {

                flgReadFlag = false;
                bufferIndex = 0;
                strOutBuff = string.Empty;
                ReceiveBuffer = new byte[ReceiveBuffer_Size];
                flgDataReceived = false;
                WriteData(Buffer, 0, nLength);
                TimeStamp = DateTime.Now;
                timeout = CommandTimeout;
                do
                {
                    //if (Buffer[discCommandByte] == 83) return true;
                    Thread.Sleep(2);
                    if (flgReadFlag) return true;
                    if (Timeout() == 1) break;
                    if (bufferIndex >= 5) lenDatatobeReceive = ReceiveBuffer[5];
                    if (lenDatatobeReceive > 0 && bufferIndex >= lenDatatobeReceive) return true;

                } while (true);
                if (Buffer[discCommandByte] == 83) break;
            }
            return false;

        }

        public bool fSendIrDADataToPort_1P(byte[] Buffer, int nLength)
        {
            int nRetries = 2;         
            int lenDatatobeReceive = 0;
            while (nRetries-- > 0)
            {

                flgReadFlag = false;
                bufferIndex = 0;
                strOutBuff = string.Empty;
                ReceiveBuffer = new byte[ReceiveBuffer_Size];
                flgDataReceived = false;
                WriteData(Buffer, 0, nLength);
                TimeStamp = DateTime.Now;
                timeout = CommandTimeout;
                do
                {
                    //if (Buffer[discCommandByte] == 83) return true;
                    Thread.Sleep(2);
                    if (flgReadFlag) return true;
                    if (Timeout() == 1) break;
                    if (bufferIndex >= 5)
                    {
                        //lenDatatobeReceive = (ReceiveBuffer[1] - 48) * 16 + (ReceiveBuffer[2] - 48) + 10;
                        lenDatatobeReceive = Convert.ToInt16(ASCIIHexToDecimalConversion(ReceiveBuffer,1,2))+ 10;// (ReceiveBuffer[1] - 48) * 16 + (ReceiveBuffer[2] - 48) + 10;
                        if (ReceiveBuffer[lenDatatobeReceive] == 0x0A) return true;
                        //if (lenDatatobeReceive > 0 && bufferIndex >= lenDatatobeReceive) return true;
                    }
                } while (true);
                 
            }
            return false;

        }
        public int ASCIIHexToDecimalConversion(byte[] recBuffer,int startdata,int enddata)
        {
            try
            {
                string hexString = "";
                while (startdata <= enddata)
                {
                   char AsciiCh = Convert.ToChar(recBuffer[startdata++]);
                    if ((AsciiCh >= 48) && AsciiCh <= 57) hexString += (Convert.ToInt16(AsciiCh) - 48).ToString();
                    else hexString +=  (AsciiCh).ToString();
                }
                return Convert.ToInt32(hexString,16);
                 
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public void SetSerialPortSettings(String Port,String baud, String parity, String dataBits, String nStopBits,int CmdTimeout,int intercharDelay)
        {
            PortName = Port;
            BaudRate = baud;
            Parity = parity;
            DataBits = dataBits;
            StopBits = nStopBits;
            CommandTimeout = CmdTimeout;
            InterchatracterDelay = intercharDelay;
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
 
    }

   
    
}