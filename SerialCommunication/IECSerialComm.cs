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
    public class IECSerialComm
    {
        public ASCIIEncoding encoding;
        #region Manager Variables
        //Command constant
        public int commHexCount = 0;
        public  string _SOH = "01";
        public  string _STX = "02";
        public  string _ETX = "03";
        public  string _EOT = "04";
        public  string _ACK = "06";
        public string _NACK = "15";
        public  string _LF = "0A";
        public  string _CR = "0D";
        public  string _NAK = "15";
        public  string _ZERO = "30";
        public string _R1 = "5231";
        public string _R2 = "5232";
        public string _R4 = "5234";
        public string _W1 = "5731";
        public string _W2 = "5732";
        public string _W4 = "5734";
        public string _D40 = "443430";
        public string _E00 = "4530";
        public  string _IEC_READ = "30";
        public  string _IEC_WRITE = "31";
        public string _IEC_MANF = "36";
        public  string _IEC_PRG = "63";
        public string _BStart = "28";
        public string _BEnd = "29";

        public string _IEC_SIGNON = "2F3F210D0A";
        public string _BreakCommand = "0142300371";
        public List<byte> ReceiveBuffer = new List<byte>();
        public SerialPort comPort;
     
        public string selectedCOM = string.Empty;
        public string selectedBaud = string.Empty;
        public string selectedBaudIndex = string.Empty;
        public string selectedSignOnMode = string.Empty;
        private string _baudRate;
        private string _parity;
        private string _stopBits;
        private string _dataBits;
        private string _portName;
        private string _command;
        private long  _cmriWaitTimeout;
        private int _cmriPktTimeout;
        private int _commandTimeout;
        private int _intercharacterDelay;
        private int _intercommandDelay=0;
        public int Pktflg = 0;
        public string strOutBuff ="";
        public int commandIndex = 0;
        public long commCount = 0;
        public bool flgReadFlag = false;
        public bool flgDataReceived;
        public string GenRead = string.Empty;
        public string GLoadRead = string.Empty;
        //public bool TimeoutFlag = false;
        public int pktCount = 0;
        public string strMeterpass = "";
        public bool flgBCC = false;
        public string PasswordSignonResponse = string.Empty;
        public DateTime TimeStamp;
        public string strParity = "Even";
        public int iDataBits = 7;
        public enum COMMANDINDEX{FWUPGRADE = 15,RFPAYLOAD=5};
        public string StopResponseByte = "\x03";
        #endregion

        #region Manager Constructors

        public IECSerialComm()
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
            encoding=new ASCIIEncoding();
            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
        }

        #endregion

        #region Manager Properties
        public void SetSerialPortSettings(String Port, String baud, String parity, String dataBits, String nStopBits, int CmdTimeout, int intercharDelay,int selectedbaudIndex)
        {
           
           this.selectedCOM = Port;
           this.selectedBaud = baud;
           this.selectedBaudIndex = (30 + selectedbaudIndex).ToString();         
           this.Parity = parity;
           this.DataBits = dataBits;
           this.StopBits = nStopBits;
           this.CommandTimeout = CmdTimeout;
           this.InterchatracterDelay = intercharDelay;
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
  
        #endregion
       
        #region WriteData
        internal void WriteData(string msg)
        {
            if (!(comPort.IsOpen == true)) comPort.Open();
                comPort.Write(msg);
        }
        #endregion

        #region HexToByte
        public byte[] HexToByte(string msg)
        {
            byte[] comBuffer = new byte[msg.Length / 2];
            for (int i = 0; i < msg.Length; i += 2)
                comBuffer[i / 2] = (byte)Convert.ToByte(msg.Substring(i, 2), 16);
            return comBuffer;
        }
        #endregion

        #region ByteToHex
        internal string ByteToHex(byte[] comByte)
        {
            StringBuilder builder = new StringBuilder(comByte.Length * 3);
            foreach (byte data in comByte)
                builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));
            return builder.ToString().ToUpper();
        }
        #endregion

        #region OpenPort
        public bool OpenPort()
        {
            try
            {
                if (comPort.IsOpen == true) comPort.Close();
                comPort.BaudRate = int.Parse(selectedBaud);
                comPort.DataBits =Convert.ToInt16(this.DataBits);
                comPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBits);
                comPort.Parity = (Parity)Enum.Parse(typeof(Parity), Parity);
                comPort.PortName = this.selectedCOM; 
                comPort.RtsEnable = true;
                comPort.DtrEnable = true;
                comPort.Open();
                return true;
            }
            catch (Exception  )
            {
                return false;
            }
        }
        #endregion

        #region ClosePort
        public bool ClosePort()
        {
            try
            {
                if (comPort.IsOpen == true) comPort.Close();
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }
        #endregion

        #region SetParity
        internal string[] SetParity()
        {
            return (Enum.GetNames(typeof(Parity)));
        }
        #endregion

        #region StopBit
        internal string[] StopBit()
        {
            return(Enum.GetNames(typeof(StopBits))) ;
        }
        #endregion
       
        #region GetPort
        internal string[] GetPort()
        {
            return (SerialPort.GetPortNames());   
        }
        #endregion

        #region comPort_DataReceived
        /// <summary>
        /// method that will be called when theres data waiting in the buffer
        /// </summary>

        public void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string strdata = "";
            byte[] tempBytes;
            flgDataReceived = true;
            switch (commandIndex)
            {
                case 0:
                    {
                        TimeStamp = DateTime.Now;
                        commCount = commCount + comPort.BytesToRead;
                        strdata = comPort.ReadExisting();
                        strOutBuff = strOutBuff + strdata;                         
                        break;
                    }                
                    
                case (int)COMMANDINDEX.FWUPGRADE:
                    TimeStamp = DateTime.Now;
                    commCount = commCount + comPort.BytesToRead;
                    strdata = comPort.ReadExisting();
                    strOutBuff = strOutBuff + strdata;
                    if (flgBCC == false)
                    {
                        if (strOutBuff.IndexOf("\x06") >= 0)
                        {
                            strOutBuff = "06";
                            flgReadFlag = true;
                            flgBCC = false;
                            return;
                           
                        }
                        else if (strOutBuff.IndexOf("ER27") >= 0)
                        {
                            strOutBuff = "NOP";
                            flgReadFlag = true;
                            flgBCC = false;
                            return;
                        }
                        else if (strOutBuff.IndexOf("\x15") >= 0)
                        {
                            strOutBuff = "15";
                            flgReadFlag = true;
                            flgBCC = false;
                            return;
                        }
                    }
                    else
                    {
                        flgReadFlag = true;
                        flgBCC = false;
                        return;
                    }
                    break;
                case (int)COMMANDINDEX.RFPAYLOAD:
                    commHexCount = comPort.BytesToRead;
                    tempBytes = new byte[commHexCount];
                    comPort.Read(tempBytes, 0, commHexCount);
                    ReceiveBuffer.AddRange(tempBytes);
                    strdata = Encoding.UTF7.GetString(tempBytes, 0, tempBytes.Length);
                    strOutBuff += strdata;
                    TimeStamp = DateTime.Now;
                    break;
                default:
                    {
                        break;
                    }
            }
        }

        #endregion

        # region SendCommand FOR IEC

        public bool SendCommand()
        {
            int i = 0;
            flgBCC = false;
            char CH;
            try
            {
           
                TimeStamp = DateTime.Now;
                while (i < Command.Length)
                {
                    CH = Convert.ToChar(Convert.ToUInt16((this.Command.Substring(i, 2)), 16));
                    WriteData(CH.ToString());
                    i = i + 2;
                    Thread.Sleep(20);
                }
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }

        # endregion

        #region Send Command For F/W Upgrade
        public bool SendFWUCommand(string Command)
        {
            int i = 0;
            flgBCC = false;
            char CH;
            byte[] bArr = new byte[Command.Length / 2];

            try
            {
                for (int j = 0; j < Command.Length; j++, i++)
                {
                    CH = Convert.ToChar(Convert.ToUInt16((this.Command.Substring(j, 2)), 16));
                    bArr[i] = (byte)CH;
                    j++;
                }
                WriteByteData(bArr);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }

        }
#endregion

        #region WriteByteData
        internal void WriteByteData(byte[] bArr)
        {
            if (!comPort.IsOpen)
                comPort.Open();
            comPort.Write(bArr, 0, bArr.Count());
        }

        public bool SendRFBuffCommand(byte[] bBuff)
        {

            try
            {

                TimeStamp = DateTime.Now;
                comPort.Write(bBuff, 0, bBuff.Length);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        # region Delay

        public void DelayExecution()
        {
            DateTime end = DateTime.UtcNow.AddMilliseconds(this.IntercommandDelay);
            while (DateTime.UtcNow < end)
            {
            }
        }

        # endregion

        # region Timeout

        public bool Timeout()
        {
            long elapsedTime;
            int timeout;

                elapsedTime = DateTime.Now.Ticks - TimeStamp.Ticks;
                TimeSpan objTimeSpan = new TimeSpan(elapsedTime);
                long elapsedMilliseconds = Convert.ToInt64(objTimeSpan.TotalMilliseconds);
                if (!flgDataReceived)
                    timeout = this.CommandTimeout;
                else
                    timeout = this.InterchatracterDelay;
                if (elapsedMilliseconds > timeout)
                    return true;
                else
                    return false;

        }

        # endregion

        # region CommandDelay()

        public bool CommandDelay(int intdelay)
        {
            long elapsedTime;
            int timeout;

            elapsedTime = DateTime.Now.Ticks - TimeStamp.Ticks;
            TimeSpan objTimeSpan = new TimeSpan(elapsedTime);
            long elapsedMilliseconds = Convert.ToInt64(objTimeSpan.TotalMilliseconds);
            if (!flgDataReceived)
                timeout = this.CommandTimeout;
            else
                timeout = this.InterchatracterDelay;
            if (elapsedMilliseconds > timeout)
                return true;
            else
                return false;

        }

        # endregion

  /******************************************************************************
  *  Function Name   : SignOn
  *  Description     : Send SignOn Command and return its response
  *                                  
  *******************************************************************************/
        public bool SignOn()
        {
            int cntout = 3;
            commandIndex = 0;
            strOutBuff = "";
            try
            {
                flgReadFlag = false;
                Command = _IEC_SIGNON;
                do
                {
                   if (!SendCommand()) return false;
                   TimeStamp = DateTime.Now;
                    do
                    {
                       Thread.Sleep(20);
                       if (strOutBuff.IndexOf(StopResponseByte) >= 0) return true;  
                    } while (!Timeout());
                    if (strOutBuff.IndexOf(StopResponseByte) >= 0) return true; 
                    cntout--;
                    Thread.Sleep(200);
                } while (cntout != 0);
                return false;

            }
            catch (Exception)
            {
                return false; 
            }           
        }

        public string SendMotFileDataToMeter(string strcmdHEXString)
        {
            commandIndex = 0;
            int retry = 2;
            flgReadFlag = false;
            Command = strcmdHEXString;
            do
            {
                strOutBuff = "";
                if (!SendFWUCommand(strcmdHEXString)) return "";
                Thread.Sleep(200);
                TimeStamp = DateTime.Now;
                do
                {
                    Thread.Sleep(20);
                    if (strOutBuff.IndexOf(StopResponseByte) >= 0) return strOutBuff;
                } while (!Timeout());
                if (strOutBuff.IndexOf(StopResponseByte) >= 0) return strOutBuff;
                else if(strOutBuff.IndexOf("\x15") < 0 && strOutBuff.Length>0) return strOutBuff;
            } while (retry-- > 0);
            return strOutBuff;
        }
        /******************************************************************************
        *  Function Name   : GetPortSettings
        *  Description     : 
        *                    
        *******************************************************************************/
       public bool GetPortSettings()
        {
            try
            {
                XmlTextReader TextReader = new XmlTextReader(AppDomain.CurrentDomain.BaseDirectory + @"\Configuration\SystemSettings.xml");
                int index = 1;
                while (TextReader.Read())                            //Read all the texts in the elements
                {
                    XmlNodeType NType = TextReader.NodeType;


                    if (NType == XmlNodeType.Text)
                    {
                        if (index == 1)
                        {
                            selectedCOM = TextReader.Value;

                        }
                        if (index == 2)
                        {
                            selectedBaud = TextReader.Value;
                        }
                        if (index == 3)
                        {
                            selectedBaudIndex = TextReader.Value;
                        }
                        if (index == 4)
                        {
                            selectedSignOnMode = TextReader.Value;
                        }
                        
                        index = index + 1;

                    }
                }
                TextReader.Close();
                return true;
                
            }
            catch (Exception )
            {
                return false;
            }
        }
       public string[] GetSystemSettings()
       {
           string[] systemsetting =new string[20];
           XmlTextReader TextReader = new XmlTextReader(AppDomain.CurrentDomain.BaseDirectory + @"\Configuration\SystemSettings.xml");
           try
           {               
               int index = 1;
               while (TextReader.Read())                            //Read all the texts in the elements
               {
                   XmlNodeType NType = TextReader.NodeType;

                   if (NType == XmlNodeType.Text)
                   {
                       if (index == 1)
                       {
                           systemsetting[0] = TextReader.Value;

                       }
                       if (index == 2)
                       {
                           systemsetting[1] = TextReader.Value;
                       }
                       if (index == 3)
                       {
                           systemsetting[2] = TextReader.Value;
                       }
                       if (index == 4)
                       {
                           systemsetting[3] = TextReader.Value;
                       }

                       index = index + 1;

                   }
               }
               TextReader.Close();
               return systemsetting;

           }
           catch (Exception)
           {
               
               TextReader.Close();
               return systemsetting;
           }
       }
        public bool SetSystemSettings(string[] strSysSetting)
        {

            try
            {
                string filename = AppDomain.CurrentDomain.BaseDirectory + @"\Configuration\SystemSettings.xml";

                XmlTextWriter TextWriter = new XmlTextWriter(filename, null);       //Use the text writer for the file writing
                TextWriter.Formatting = Formatting.Indented;
                TextWriter.WriteStartDocument();                                    //Start the document
                TextWriter.WriteStartElement("CfgSettings");
                if (strSysSetting[0]!=null) TextWriter.WriteElementString("PortName", strSysSetting[0]);   //Pass the property value as the Parameter
                else TextWriter.WriteElementString("PortName", "COM1"); 
                if (strSysSetting[1] != null) TextWriter.WriteElementString("BaudRate", strSysSetting[1]);   //Pass the property value as the Parameter  
                else TextWriter.WriteElementString("BaudRate", "9600"); 
                if (strSysSetting[2] != null) TextWriter.WriteElementString("BaudRateIndex", strSysSetting[2]);   //Pass the property value as the Parameter 
                else TextWriter.WriteElementString("BaudRateIndex", "5"); 
                if (strSysSetting[3] != null) TextWriter.WriteElementString("SignOnMode", strSysSetting[3]);   //Pass the property value as the Parameter
                else TextWriter.WriteElementString("SignOnMode", "0"); 
                TextWriter.WriteEndElement();                                       //Ending the element
                TextWriter.WriteEndDocument();	                                    //closing the document		
                TextWriter.Flush();                                                 //flush the writer
                TextWriter.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
      /******************************************************************************
      *  Function Name   : TeaAlgorithm()
      *  Description     : TeaAlgorithm is used for creating the algorithemic Password .  
      *                    Command based on command recevied from Meter               
      *******************************************************************************/
       public string TeaAlgorithm(string strseed)
       {
           uint y = 0;
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
               
           }
           catch (Exception )
           {
               return "";

           }
           return y.ToString();
       }
   /******************************************************************************
   *    Function Name    : CalBcc
   *    Description      : This function calculate the CheckSum of the passing   
   *                       String and match the Bcc with passed checksum character
   *******************************************************************************/
       public string CalBcc(string RecInpData)
       {
           try
           {
               char[] _hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
               int Bcc = 0;
               int countbyt = 0;
               string TempStr = "";
               while (countbyt < RecInpData.Length)
               {

                   TempStr += System.Convert.ToChar(System.Convert.ToUInt32(RecInpData.Substring(countbyt, 2), 16)).ToString(); ;
                   countbyt += 2;
               }
               countbyt = 0;
               System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
               Byte[] bytes = encoding.GetBytes(TempStr);
               foreach (byte b in bytes)
               {
                   if (countbyt <= RecInpData.Length) Bcc = Bcc ^ b;
                   countbyt++;
               }
               string retval = _hexChars[Bcc / 16].ToString() + _hexChars[Bcc % 16].ToString();

               return retval;
           }
           catch (Exception )
           {
               
               return "False";
           }
       }

       /******************************************************************************
       *    Function Name    : CalculateBcc()
       *    Description      : This function calculate the CheckSum of the recevied read data from meter by passing   
       *                       String and match the Bcc with passed checksum character
       *******************************************************************************/
       public bool CalculateBcc(string RecInpData, int endLen, string bccChar)
       {
           try
           {
               int Bcc = 0;
               System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
               Byte[] bytesBcc = encoding.GetBytes(RecInpData.Substring(0, 1));
               foreach (byte b in bytesBcc)
                   if (b == 2) { Bcc = 1; }

               long countbyt = 0;
               Byte[] bytes = encoding.GetBytes(RecInpData);
               foreach (byte b in bytes)
               {
                   if (countbyt <= endLen) Bcc = Bcc ^ b;
                   countbyt++;
               }
               bytes = encoding.GetBytes(bccChar);
               foreach (byte b in bytes)
                   if (Bcc != b) { return false; }
               return true;
           }
           catch (Exception )
           {
                
               return false;
           }
       }
       
       public bool ManfCommand(string Selectedbaudrate)
       {
           string commandResponse = string.Empty;
           SetBaudrateIndex(Selectedbaudrate);
           commandIndex = 0; 
           Command = _ACK + _IEC_PRG + selectedBaudIndex.ToString() + _IEC_WRITE + _CR + _LF;// "066335310D0A"; 
           strOutBuff = "";
           flgReadFlag = false;
           Thread.Sleep(200);
           if (!SendCommand()) return false;           
               Thread.Sleep(200);
               comPort.BaudRate = int.Parse(Selectedbaudrate);          
               TimeStamp = DateTime.Now;
               do
               {
                Thread.Sleep(20);
                if (strOutBuff.IndexOf(StopResponseByte) >= 0) break;
               } while (!Timeout());
               commandResponse = strOutBuff;
               if (commandResponse.IndexOf("(") >= 0)
               {
                   PasswordSignonResponse = commandResponse.Substring(commandResponse.IndexOf("(") + 1, commandResponse.IndexOf(")") - (commandResponse.IndexOf("(") + 1));
                   return true;
               }
               else return false;
         }
       private void SetBaudrateIndex(string baudrate)
       {
           selectedBaudIndex = "30";
           switch (baudrate)
           {
               case "300":
                   selectedBaudIndex = "30";
                   break;
               case "600":
                   selectedBaudIndex = "31";
                   break;
               case "1200":
                   selectedBaudIndex = "32";
                   break;
               case "2400":
                   selectedBaudIndex = "33";
                   break;
               case "4800":
                   selectedBaudIndex = "34";
                   break;
               case "9600":
                   selectedBaudIndex = "35";
                   break;                
           }
       }

       public string ManfModeCommand()
       {
           string commandResponse = string.Empty;

           //Initialize commandIndex
           commandIndex = 2;
           Command = _ACK + _IEC_PRG + selectedBaudIndex + _IEC_MANF + _CR + _LF;// "066335350D0A";

           strOutBuff = "";
           flgReadFlag = false;

           Thread.Sleep(300);
           if (SendCommand())
           {
               Thread.Sleep(200);
               comPort.BaudRate = int.Parse(selectedBaud);

               TimeStamp = DateTime.Now;
               do
               {
                   Thread.Sleep(20);
                   if (Timeout())return "";                   
                   else
                   {
                       if (flgReadFlag)
                       {
                           commandResponse = strOutBuff;
                           if (commandResponse.IndexOf("(") >= 0)
                           {
                               commandResponse = commandResponse.Substring(commandResponse.IndexOf("(") + 1, commandResponse.IndexOf(")") - (commandResponse.IndexOf("(") + 1));
                           }
                           commCount = 0;
                           strOutBuff = "";
                           return commandResponse;
                       }
                   }
               } while (true);
           }
           else
           {

               return "";
           }
       }

       public bool PasswordSignOn(string pass)
       {            
           string hexPass = "";
           if (pass == "") return false;
           foreach (char c in pass)
           {
               int tmp = c;

               hexPass += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
           }
           string strPassCmd = _SOH + "5031" + _STX + _BStart + hexPass + _BEnd + _ETX;
           string BccChar = ReturnBcc(strPassCmd.Substring(2));
           strPassCmd += BccChar;
           if (SendDataToMeterandGetResponse(strPassCmd).Length>=0) return true;
           else return false;

         

       }
        

       public bool PasswordModeChecking(bool flgmode)
       {
           bool Flg_ComSetting = GetPortSettings();
           if (flgmode)
           {
               if (selectedSignOnMode != "1") return (AlgorithemicSignOn(PasswordSignonResponse));
               else return (PasswordSignOn(strMeterpass));
           }
           else
           {
               return (AlgorithemicSignOn(PasswordSignonResponse));
           }

       }

       public bool AlgorithemicSignOn(string strSeed)
       {
           try
           {
               string CalculatedSeed = TeaAlgorithm(strSeed);
               uint ass = uint.Parse(CalculatedSeed);
               string Hexcmd = string.Format("{0:X}", ass);
               while (Hexcmd.Length < 8) Hexcmd = "0" + Hexcmd;
               Hexcmd = StrToHex(Hexcmd);
               string strPassCmd = _SOH + "5032" + _STX + _BStart + Hexcmd + _BEnd + _ETX;
               string BccChar = ReturnBcc(strPassCmd.Substring(2));
               strPassCmd += BccChar;
               strPassCmd=SendDataToMeterandGetResponse(strPassCmd);
               if (strPassCmd.Length > 0)
               {
                   if(strPassCmd.IndexOf(StopResponseByte)>=0)return true;
               } 
               return false;
               

           }
           catch (Exception)
           {
               return false;
           }
           finally
           {
               commCount = 0;
               strOutBuff = "";
           }
           

       }

       public string SendDataToMeterandGetResponse(string strPassCmd)
       {
           commandIndex = 0;          
           int retry=2;
           flgReadFlag = false;
           Command = strPassCmd;
           do
           {
               strOutBuff = "";
               if (!SendCommand()) return "";
               Thread.Sleep(200);
               TimeStamp = DateTime.Now;
               do
               {
                   Thread.Sleep(20);
                   if (strOutBuff.IndexOf(StopResponseByte) >= 0) return strOutBuff;
                   if (strOutBuff.IndexOf("(ER") >= 0) break;
               } while (!Timeout());
               if (strOutBuff.IndexOf(StopResponseByte) >= 0) return strOutBuff;
               else if (strOutBuff.IndexOf("\x15") < 0 && strOutBuff.Length > 0) return strOutBuff;             
           } while (retry-- > 0);
           return strOutBuff;
       }
       public string SendByteDataToMeterandGetResponse(string strPassCmd)
       {
           commandIndex = 0;
           int retry = 2;
           flgReadFlag = false;
           Command = strPassCmd;
           do
           {
               strOutBuff = "";
               if (!SendCommand()) return "";
               Thread.Sleep(200);
               TimeStamp = DateTime.Now;
               do
               {
                   Thread.Sleep(20);
                   if (strOutBuff.IndexOf(StopResponseByte) >= 0) return strOutBuff;
                   if (strOutBuff.IndexOf("(ER") >= 0) break;
               } while (!Timeout());
               if (strOutBuff.IndexOf(StopResponseByte) >= 0) return strOutBuff;
               else if (strOutBuff.IndexOf("\x15") < 0 && strOutBuff.Length > 0) return strOutBuff;
           } while (retry-- > 0);
           return strOutBuff;
       }

       public List<byte> ReadByteResponse(byte[] RequestCommand, int byteToread,int commandIDX)
       {
           try
           {
               commandIndex = commandIDX;
               strOutBuff = "";
               flgReadFlag = false;
               Thread.Sleep(200);
               ReceiveBuffer = new List<byte>();
               int retry = 1;              
               do
               {
                   strOutBuff = "";
                   ReceiveBuffer = new List<byte>();
                   if(!SendRFBuffCommand(RequestCommand))return null;
                   TimeStamp = DateTime.Now;
                   do
                   {
                       if (ReceiveBuffer.Count >= byteToread)break;                       
                       Thread.Sleep(20);
                       if (Timeout()) break;                       
                   } while (true);
                   if (ReceiveBuffer.Count >= byteToread) break;      
               } while (retry-- > 0);
               return ReceiveBuffer;              

           }
           catch (Exception)
           {
               return null;
           }

       }
       /******************************************************************************
  *    Function Name    : ReturnBcc
  *    Description      : This function calculate the CheckSum of the passing   
  *                       String and match the Bcc with passed checksum character
  *******************************************************************************/
       public string ReturnBcc(string RecInpData)
       {
           try
           {
               char[] _hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
               int Bcc = 0;
               int countbyt = 0;
               string TempStr = "";
               while (countbyt < RecInpData.Length)
               {

                   TempStr += System.Convert.ToChar(System.Convert.ToUInt32(RecInpData.Substring(countbyt, 2), 16)).ToString(); ;
                   countbyt += 2;
               }
               countbyt = 0;
               System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
               Byte[] bytes = encoding.GetBytes(TempStr);
               foreach (byte b in bytes)
               {
                   if (countbyt <= RecInpData.Length) Bcc = Bcc ^ b;
                   countbyt++;
               }
               string retval = _hexChars[Bcc / 16].ToString() + _hexChars[Bcc % 16].ToString();
               return retval;
           }
           catch (Exception )
           {
               
               return "False";
           }
       }
       /******************************************************************************
      *  Function Name   : StrToHex()
      *  Description     : Create the Command in nibblel Hex format for Sending To Meter.  
      *                               
      *******************************************************************************/
       public string StrToHex(string GetStr)
       {
           string tempstr = "";
           try
           {
               int indecount = 0;
               char AsciiCh;
               int chrascii;
               char[] _hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
               while (indecount < GetStr.Length)
               {
                   AsciiCh = Convert.ToChar(GetStr.Substring(indecount, 1));
                   if ((AsciiCh >= 48) && AsciiCh <= 57)
                   {

                       chrascii = (Convert.ToInt16(AsciiCh) - 48) + 30;
                       tempstr += chrascii.ToString();

                   }
                   else
                   {

                       if (AsciiCh != 32)
                       {
                           chrascii = Convert.ToInt16(AsciiCh);
                           AsciiCh = _hexChars[chrascii / 16];
                           tempstr += (_hexChars[chrascii / 16]).ToString() + (_hexChars[chrascii % 16]).ToString();
                       }
                       else
                       {
                           tempstr += "20";       //Space
                       }
                   }
                   indecount++;

               }
           }
           catch (Exception )
           {

               return "";
           }
           return tempstr;
       }
       public string HexToASciiConversion(string strHex)
       {
           try
           {
               int charcount = 0;
               string strascii = "";
               while (charcount < strHex.Length)
               {
                   if (Convert.ToInt32(strHex, 16) < 48) { strascii += Convert.ToInt16(strHex).ToString(); }
                   //if (Convert.ToInt32(strHex, 16) < 48) { strascii += Convert.ToInt16(Convert.ToInt32(strHex, 16)).ToString(); }
                   else strascii += Convert.ToChar(Convert.ToInt32(strHex.Substring(charcount, 2), 16)).ToString();
                   charcount += 2;
                   
               }
               return strascii;
           }
           catch (Exception)
           {
               return "";
           }
       }
       public string HexToDecimalConversion(string strHex)
       {
           try
           {
               int bytecnt = 0;
               bytecnt = strHex.Length;
               string strtemp = "";
               while (bytecnt > 0)
               {
                   strtemp += strHex.Substring(bytecnt - 2, 2);
                   bytecnt -= 2;
               }
               long aa = Int64.Parse(strtemp, System.Globalization.NumberStyles.HexNumber);
               return aa.ToString();
           }
           catch (Exception)
           {
               return "";
           }
       }
       /******************************************************************************
    *    Event Name    : StrToHexCmd ()
    *    Description   : This Function get a string of AsciiCharactor and return a string 
    *                  : of Hex to the corresponding Ascii Charactor.
    *                     
    *******************************************************************************/
       public string StrToHexCmd(string GetStr)
       {
           try
           {
               int indecount = 0;
               char AsciiCh;
               int chrascii;
               string tempstr = "";
               char[] _hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
               while (indecount < GetStr.Length)
               {
                   AsciiCh = Convert.ToChar(GetStr.Substring(indecount, 1));
                   if ((AsciiCh >= 48) && AsciiCh <= 57)
                   {

                       chrascii = (Convert.ToInt16(AsciiCh) - 48) + 30;
                       tempstr += chrascii.ToString();

                   }
                   else
                   {

                       if (AsciiCh != 32)
                       {
                           chrascii = Convert.ToInt16(AsciiCh);
                           AsciiCh = _hexChars[chrascii / 16];
                           tempstr += (_hexChars[chrascii / 16]).ToString() + (_hexChars[chrascii % 16]).ToString();
                       }
                       else
                       {
                           tempstr += "3230";       //Space
                       }
                   }
                   indecount++;

               }
               return tempstr;
           }
           catch (Exception)
           {
               return "";
           }
       }
       /******************************************************************************
 *    Event Name    : StrToHexForSerial()
 *    Description   : This Function get a string of AsciiCharactor and return a string 
 *                  : of Hex to the corresponding Ascii Charactor.
 *                     
 *******************************************************************************/
       public string StrToHexForSerial(string GetStr)
       {
           try
           {
               int indecount = 0;
               char AsciiCh;
               int chrascii;
               string strgetval = "";
               string tempstr = "";
               char[] _hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
               while (indecount < GetStr.Length)
               {
                   AsciiCh = Convert.ToChar(GetStr.Substring(indecount, 1));
                   if ((AsciiCh >= 48) && AsciiCh <= 57)
                   {

                       chrascii = (Convert.ToInt16(AsciiCh) - 48) + 30;
                       tempstr += "33" + chrascii.ToString();

                   }
                    else
                   {

                       if (AsciiCh != 32)
                       {
                           chrascii = Convert.ToInt16(AsciiCh);
                           AsciiCh = _hexChars[chrascii / 16];
                           strgetval = (_hexChars[chrascii / 16]).ToString() + (_hexChars[chrascii % 16]).ToString();
                           tempstr += StrToHexCmd(strgetval);
                       }
                       else
                       {
                           tempstr += "3230";       //Space
                       }
                   }
                   indecount++;

               }
               return tempstr;
           }
           catch (Exception)
           {
               return "";
           }
       }

       public string GetASCII(string strRec)
       {
           try
           {
               if (strRec == "") return "";
               int charcnt = 0;
               string strvalue = "";
               string strtemp = "";
               while (charcnt < strRec.Length)
               {
                   strtemp = strRec.Substring(charcnt, 2);
                   if (strtemp == "20") strvalue += " ";
                   else strvalue += HexToASciiConversion(strtemp);
                   charcnt += 2;

               }
               return strvalue;
           }
           catch (Exception)
           {
               return "";
           }
       }
        /// <summary>
        /// Decimal To Binary Conversion
        /// </summary>
        /// <param name="hexvalue"></param>
        /// <returns></returns>
       public string Decimal2binary(string hexvalue) 
       {
           string binaryval = ""; 
           binaryval = Convert.ToString(Convert.ToInt32(hexvalue, 10), 2); 
           return binaryval; 
       }

       public string BinarytoHex(string binData)
       {
           try
           {
               string data = "";
               data = Convert.ToInt64(binData, 2).ToString("X");//binary to hex
               while (data.Length < 2) data = "0" + data;
               data = StrToHexCmd(data);
               return data;
           }
           catch (Exception)
           {
               return "";
           }
       }

        /// <summary>
        /// Get The Number of days in particulat month of the year
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int GetDaysOfMonth(int month, int year)
        {
        if (month < 1 || month > 12)return 0;         
        if (1 == month || 3 == month || 5 == month || 7 == month || 8 == month ||10 == month || 12 == month)
        {
            return 31;
        }
        else if (2 == month)
        {
            // Check for leap year
             if (0 == (year % 4))
            {
             // If Year is divisible by 400, it's a leap year.
             // Otherwise, if it's divisible by 100 it's not.
                 if (0 == (year % 400)) return 29;
                 else if (0 == (year % 100))return 28;
                 // IF Divisible by 4 but not by 100 or 400 so it leaps
                 return 29;
            }
        // Not a leap year
        return 28;
        }
        return 30;
        }

        public string GetMeterIDORSeries(string getinp, int getType)
        {
            try
            {
                string returnStr = "";
                int startIndex = strOutBuff.IndexOf("LGC110") + 13;
                int endIndex = strOutBuff.IndexOf("\r\n");
                if (getType == 2) { if (strOutBuff.Length>=10) returnStr = strOutBuff.Substring(8, 2).Trim(); return returnStr; }
                returnStr = strOutBuff.Substring(startIndex, endIndex - (startIndex + 1)).Trim();
                if (getType == 1) returnStr = strOutBuff.Substring(startIndex, endIndex - (startIndex + 1)).Trim();
                else returnStr = strOutBuff.Substring(8, 2).Trim();
                return returnStr;
            }
            catch (Exception)
            {
                return "";
            }
        }
      
  //-----------------------------------------------END OF CLASS----------------------------------------------------------  
    }


}
