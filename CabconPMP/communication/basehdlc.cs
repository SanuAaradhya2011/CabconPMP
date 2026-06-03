using SmartCalibration.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCalibration.Communication
{
    public class basehdlc
    {
        public int dmlsblocksize = 0x4d;
        public int[] ucFcs = new int[2];
        int PPPINITFCS16 = 0xFFFF;
        int PPPGOODFCS16 = 0xF0B8;
        protected byte sourceaddress;
        public byte nCMDByte;
        const byte Null_Type = 0x00;
        const byte Array_Type = 0x01;
        const byte Structure = 0x02;
        const byte Boolean_Type = 0x03;
        const byte BitString_Type = 0x04;
        const byte DoubleLong_Type = 0x05;
        const byte DoubleLongUnsigned_Type = 0x06;
        const byte OctetString_Type = 0x09;
        const byte VisibleString_Type = 10;
        const byte BCD_Type = 13;
        const byte Integer_Type = 15;
        const byte Long_Type = 16;
        const byte Unsigned_Type = 17;
        const byte LongUnsigned_Type = 18;
        const byte CompactArray_Type = 19;
        const byte Long64_Type = 20;
        const byte Long64Unsigned_Type = 21;
        const byte Enum_Type = 22;
        const byte Float32_Type = 23;
        const byte Float64_Type = 24;
        const byte DateTime_Type = 25;
        const byte Date_Type = 26;
        const byte Time_Type = 27;
        const byte ExtendedOctetString = 0x82;
        private object syncObj = new object();
        public int mpos;
        public Dictionary<int, byte> cmddicbyte = new Dictionary<int, byte>();

        int[] uifcstab = {  0x0000, 0x1189,0x2312,  0x329b, 0x4624, 0x57ad, 0x6536, 0x74bf,
                            0x8c48, 0x9dc1, 0xaf5a, 0xbed3, 0xca6c, 0xdbe5, 0xe97e, 0xf8f7,
                            0x1081, 0x0108, 0x3393, 0x221a, 0x56a5, 0x472c, 0x75b7, 0x643e,
                            0x9cc9, 0x8d40, 0xbfdb, 0xae52, 0xdaed, 0xcb64, 0xf9ff, 0xe876,
                            0x2102, 0x308b, 0x0210, 0x1399, 0x6726, 0x76af, 0x4434, 0x55bd,
                            0xad4a, 0xbcc3, 0x8e58, 0x9fd1, 0xeb6e, 0xfae7, 0xc87c, 0xd9f5,
                            0x3183, 0x200a, 0x1291, 0x0318, 0x77a7, 0x662e, 0x54b5, 0x453c,
                            0xbdcb, 0xac42, 0x9ed9, 0x8f50, 0xfbef, 0xea66, 0xd8fd, 0xc974,
                            0x4204, 0x538d, 0x6116, 0x709f, 0x0420, 0x15a9, 0x2732, 0x36bb,
                            0xce4c, 0xdfc5, 0xed5e, 0xfcd7, 0x8868, 0x99e1, 0xab7a, 0xbaf3,
                            0x5285, 0x430c, 0x7197, 0x601e, 0x14a1, 0x0528, 0x37b3, 0x263a,
                            0xdecd, 0xcf44, 0xfddf, 0xec56, 0x98e9, 0x8960, 0xbbfb, 0xaa72,
                            0x6306, 0x728f, 0x4014, 0x519d, 0x2522, 0x34ab, 0x0630, 0x17b9,
                            0xef4e, 0xfec7, 0xcc5c, 0xddd5, 0xa96a, 0xb8e3, 0x8a78, 0x9bf1,
                            0x7387, 0x620e, 0x5095, 0x411c, 0x35a3, 0x242a, 0x16b1, 0x0738,
                            0xffcf, 0xee46, 0xdcdd, 0xcd54, 0xb9eb, 0xa862, 0x9af9, 0x8b70,
                            0x8408, 0x9581, 0xa71a, 0xb693, 0xc22c, 0xd3a5, 0xe13e, 0xf0b7,
                            0x0840, 0x19c9, 0x2b52, 0x3adb, 0x4e64, 0x5fed, 0x6d76, 0x7cff,
                            0x9489, 0x8500, 0xb79b, 0xa612, 0xd2ad, 0xc324, 0xf1bf, 0xe036,
                            0x18c1, 0x0948, 0x3bd3, 0x2a5a, 0x5ee5, 0x4f6c, 0x7df7, 0x6c7e,
                            0xa50a, 0xb483, 0x8618, 0x9791, 0xe32e, 0xf2a7, 0xc03c, 0xd1b5,
                            0x2942, 0x38cb, 0x0a50, 0x1bd9, 0x6f66, 0x7eef, 0x4c74, 0x5dfd,
                            0xb58b, 0xa402, 0x9699, 0x8710, 0xf3af, 0xe226, 0xd0bd, 0xc134,
                            0x39c3, 0x284a, 0x1ad1, 0x0b58, 0x7fe7, 0x6e6e, 0x5cf5, 0x4d7c,
                            0xc60c, 0xd785, 0xe51e, 0xf497, 0x8028, 0x91a1, 0xa33a, 0xb2b3,
                            0x4a44, 0x5bcd, 0x6956, 0x78df, 0x0c60, 0x1de9, 0x2f72, 0x3efb,
                            0xd68d, 0xc704, 0xf59f, 0xe416, 0x90a9, 0x8120, 0xb3bb, 0xa232,
                            0x5ac5, 0x4b4c, 0x79d7, 0x685e, 0x1ce1, 0x0d68, 0x3ff3, 0x2e7a,
                            0xe70e, 0xf687, 0xc41c, 0xd595, 0xa12a, 0xb0a3, 0x8238, 0x93b1,
                            0x6b46, 0x7acf, 0x4854, 0x59dd, 0x2d62, 0x3ceb, 0x0e70, 0x1ff9,
                            0xf78f, 0xe606, 0xd49d, 0xc514, 0xb1ab, 0xa022, 0x92b9, 0x8330,
                            0x7bc7, 0x6a4e, 0x58d5, 0x495c, 0x3de3, 0x2c6a, 0x1ef1, 0x0f78
                         };


       
        public void fSetPFBit(byte nStatus)
        {
            if (nStatus == 0x01) nCMDByte = Convert.ToByte(nCMDByte | 0x10);
            else nCMDByte = Convert.ToByte(nCMDByte & 0xEF);
        }
        /// <summary>
        /// Set last bit
        /// </summary>
        /// <param name="nStatus"></param>
        public void fSetLastBit(byte nStatus)
        {
            if (nStatus == 0x01) nCMDByte = Convert.ToByte(nCMDByte | 0x01);
            else nCMDByte = Convert.ToByte(nCMDByte & 0xFE);
        }
        /// <summary>
        /// Increment recieve Sequence number
        /// </summary>
        public void fIncRecieve()
        {
            int nSeqCounter = Convert.ToByte(nCMDByte & 0xE0);
            nSeqCounter = Convert.ToByte(nSeqCounter >> 5);
            nSeqCounter = Convert.ToByte(nSeqCounter + 1);
            nSeqCounter = nSeqCounter << 5;
            nSeqCounter = nSeqCounter & 0x00FF;
            nCMDByte = Convert.ToByte(nCMDByte & 0x1F);
            nCMDByte = Convert.ToByte(nCMDByte | nSeqCounter);

            
        }

        /// <summary>
        /// Increment Send Sequence number
        /// </summary>
        public void fIncSend()
        {
            //nCMDByte = 0xB8;
            try
            {
                Monitor.Enter(syncObj);
                {
                    byte tempncmdbyte = nCMDByte;
                    int nSeqCounter = Convert.ToByte(tempncmdbyte & 0x0E);

                    nSeqCounter = Convert.ToByte(nSeqCounter >> 1);
                    nSeqCounter = Convert.ToByte(nSeqCounter + 1);

                    nSeqCounter = nSeqCounter << 1;
                    nSeqCounter = nSeqCounter & 0x00FF;

                    tempncmdbyte = Convert.ToByte(tempncmdbyte & 0xF1);
                    tempncmdbyte = Convert.ToByte(tempncmdbyte | nSeqCounter);
                    nCMDByte = tempncmdbyte;
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                Monitor.Exit(syncObj);
            }
        }

        /// <summary>
        /// Increment Send Sequence number
        /// </summary>
        public void fIncSend(int mpos)
        {
            //nCMDByte = 0xB8;
            try
            {
                Monitor.Enter(syncObj);
                {
                    byte tempncmdbyte = cmddicbyte[mpos];
                    int nSeqCounter = Convert.ToByte(tempncmdbyte & 0x0E);

                    nSeqCounter = Convert.ToByte(nSeqCounter >> 1);
                    nSeqCounter = Convert.ToByte(nSeqCounter + 1);

                    nSeqCounter = nSeqCounter << 1;
                    nSeqCounter = nSeqCounter & 0x00FF;

                    tempncmdbyte = Convert.ToByte(tempncmdbyte & 0xF1);
                    tempncmdbyte = Convert.ToByte(tempncmdbyte | nSeqCounter);
                    cmddicbyte[mpos] = tempncmdbyte;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Monitor.Exit(syncObj);
            }
        }
        /// <summary>
        /// Set Commmand Byte as SNRM
        /// </summary>
        public void fSetSNRM()
        {
            nCMDByte = 0x93;
        }
        /// <summary>
        /// Set Commmand Byte as UA
        /// </summary>
        public void fSetUA()
        {
            nCMDByte = 0x73;
        }
        /// <summary>
        /// Set Commmand Byte as DISC
        /// </summary>
        public void fSetDISC()
        {
            nCMDByte = 0x53;
        }
        /// <summary>
        /// Set Commmand Byte as UI
        /// </summary>
        public void fSetUI()
        {
            nCMDByte = 0x13;
        }
        /// <summary>
        /// Set Commmand Byte as Initial I
        /// </summary>
        public void fSetInitialI()
        {
            nCMDByte = 0x10;
        }


        /// <summary>
        /// Calculate a new FCS given the current FCS and New data
        /// </summary>
        /// <param name="uiLocal_fcs"></param>
        /// <param name="Buffer"></param>
        /// <param name="endlen"></param>
        /// <returns></returns>
        public int fPPPfcs16(int uiLocal_fcs, byte[] Buffer, int endlen)
        {
            int i = 1;

            while (endlen > 0)
            {
                uiLocal_fcs = (uiLocal_fcs >> 8) ^ uifcstab[(uiLocal_fcs ^ Buffer[i++]) & 0xff];
                endlen--;
            }
            return (uiLocal_fcs);
        }

        /// <summary>
        /// Check FCS is correct or Not
        /// </summary>
        /// <param name="RecvBuffer"></param>
        /// <returns></returns>
        public bool CheckFCS(byte[] RecvBuffer)
        {
            int nHDLCPktLength = (byte)(RecvBuffer[1] & 0x1F) * 0x100 + (byte)(RecvBuffer[2]);
            int nHCSindex = 8;                          //Depends on Address Byte Supported Need Change
            if (fGenerateFCS(RecvBuffer, 1, nHCSindex))    //hcs
            {
                if ((RecvBuffer[nHCSindex + 1] == ucFcs[0]) && (RecvBuffer[nHCSindex + 2] == ucFcs[1]))
                {
                    if (fGenerateFCS(RecvBuffer, 1, (nHDLCPktLength - 2)))    //FCS
                    {
                        if ((RecvBuffer[nHDLCPktLength - 1] == ucFcs[0]) && (RecvBuffer[nHDLCPktLength] == ucFcs[1])) return true;

                    }
                }
            }
            return false;


        }
        /// <summary>
        /// Generate a new FCS for Buffer data
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="stlen"></param>
        /// <param name="endlen"></param>
        /// <returns></returns>
        public bool fGenerateFCS(byte[] Buffer, int stlen, int endlen)
        {
            int uitrialfcs;
            int[] ucCalcFcs = new int[2];
            int uiLocal_fcs;
            int i = 1;
            ucFcs[0] = 0x00;
            ucFcs[1] = 0x00;
            uiLocal_fcs = PPPINITFCS16;

            uitrialfcs = fPPPfcs16(PPPINITFCS16, Buffer, endlen);
            uitrialfcs ^= 0xffff;


            ucCalcFcs[0] = Convert.ToByte(uitrialfcs & 0x00ff);
            ucCalcFcs[1] = Convert.ToByte((uitrialfcs >> 8) & 0x00ff);

            while (endlen > 0)
            {
                uiLocal_fcs = (uiLocal_fcs >> 8) ^ uifcstab[(uiLocal_fcs ^ Buffer[i++]) & 0xff];
                endlen--;
            }

            uiLocal_fcs = (uiLocal_fcs >> 8) ^ uifcstab[(uiLocal_fcs ^ ucCalcFcs[0]) & 0xff];
            uitrialfcs = (uiLocal_fcs >> 8) ^ uifcstab[(uiLocal_fcs ^ ucCalcFcs[1]) & 0xff];


            if (uitrialfcs == PPPGOODFCS16)
            {
                ucFcs[0] = ucCalcFcs[0];
                ucFcs[1] = ucCalcFcs[1];
                return true;
            }
            else
            {
                return false;
            }
        }

        
        /// <summary>
        /// Checking the Start and End tag
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nCommandType"></param>
        /// <returns></returns>
        public bool CheckCommand(byte[] Buffer, byte nCommandType)
        {
            if (Buffer[8] == nCommandType) return true;
            if (Buffer[8] == 0x1F) return false;  //Secandry Device Busy     
            return false;
        }

        public bool dlmsFrameValidation(List<byte> mlist, int commandtype)
        {
            if (!CheckFCS(mlist.ToArray())) return false;
            if (!CheckStartEndTag(mlist.ToArray())) return false;
            //nCMDByte = mlist[8];
            return true;
        }

       
        /// <summary>
        /// Checking the Start and End tag
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public bool CheckStartEndTag(byte[] Buffer)
        {
            int lengetz = (byte)(Buffer[1] & 0x1F) * 0x100 + (byte)(Buffer[2]);
            if (Buffer[0] == (byte)Constants.DLMSConstants.HDLC.TAG
                && Buffer[lengetz + 1] == (byte)Constants.DLMSConstants.HDLC.TAG)
                return true;
            else return false;
        }

        /// <summary>
        /// Conversion of Client Address Values and Filling into Buffer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        public byte[] FillLength(byte tagval, int listcount)
        {
            byte[] barrret = new byte[2];
            UInt16 ilength = (UInt16)(listcount - 1);
            UInt16 ilength1 = (UInt16)(tagval * 0x100);
            ilength = (UInt16)(ilength1 | ilength);

            barrret[0] = (byte)(ilength >> 8);
            barrret[1] = (byte)(ilength);      //Filling length Byte
            return barrret;
        }


        static public object dlmsparser(byte[] Blockdata, ref int nByteIndex)
        {
            //object obj = new object();

            try
            {
                bool bUnsignFlag = false;
                byte[] buffer = new byte[1];
                string data = "";
                string[] dataValue = new string[2];
                bool isASCIIString = false;
                int startdataIDX = nByteIndex;
                byte indexedDataType = (byte)Blockdata[nByteIndex];

            SWITCHAGAIN:
                switch (indexedDataType)
                {
                    case (int)Null_Type:                                    //0- NULL                   
                        break;
                    case (int)Array_Type:                                   //1- Array
                        buffer = new byte[Blockdata[nByteIndex + 1]]; nByteIndex += 2;
                        break;
                    case (int)Structure:                                    //2-Structure
                        buffer = new byte[Blockdata[nByteIndex + 1]]; nByteIndex += 2;
                        indexedDataType = (byte)Blockdata[nByteIndex];
                        goto SWITCHAGAIN;
                    //case (int)Boolean_Type:                                 //3- Boolean
                    //    buffer =  byte[1]; nByteIndex += 1;
                    //    break;
                    case (int)BitString_Type:                              //4- Bit String
                        buffer = new byte[Blockdata[nByteIndex + 1]]; nByteIndex += 2;
                        break;
                    case (int)DoubleLong_Type:                             //5- Double Long -- 4Byte
                        bUnsignFlag = true;
                        buffer = new byte[0x4]; nByteIndex++;
                        break;
                    case (int)DoubleLongUnsigned_Type:                     //6- Double Long Unsigned -- 4Byte
                        buffer = new byte[0x4]; nByteIndex++;
                        break;
                    case (int)OctetString_Type:                           //9- Oct String     
                        isASCIIString = true;
                        buffer = new byte[Blockdata[nByteIndex + 1]]; nByteIndex += 2;
                        break;
                    case (int)VisibleString_Type:                        //10- Sequence of ASCII String 
                        isASCIIString = true;
                        buffer = new byte[Blockdata[nByteIndex + 1]]; nByteIndex += 2;
                        break;
                    case (int)BCD_Type:                                  //13 - BCD
                        buffer = new byte[Blockdata[nByteIndex + 1]]; nByteIndex += 2;
                        break;
                    case (int)Integer_Type:                              //15- Integer 1Byte
                    case (int)Unsigned_Type:                             //17- Unsigned 1Byte
                        buffer = new byte[0x1]; nByteIndex++;
                        break;
                    case (int)Long_Type:                                //16- Long Signed 2 byte
                        bUnsignFlag = true;
                        buffer = new byte[0x2]; nByteIndex++;
                        break;
                    case (int)LongUnsigned_Type:                        //18- Unsigned 2 Byte
                        buffer = new byte[0x2]; nByteIndex++;
                        break;
                    case (int)CompactArray_Type:                        //19- Unsigned 2 Byte
                        indexedDataType = Blockdata[nByteIndex + 1];
                        nByteIndex++;
                        goto SWITCHAGAIN;

                    case (int)Long64_Type:                             //20- Integer64   8 Byte
                        bUnsignFlag = true;
                        buffer = new byte[0x8]; nByteIndex++;
                        break;
                    case (int)Long64Unsigned_Type:                     //21- Unsigned64  8 Byte
                        buffer = new byte[0x8]; nByteIndex++;
                        break;
                    case (int)Enum_Type:                               //22- Enum
                        buffer = new byte[Blockdata[nByteIndex + 1]]; nByteIndex += 2;
                        break;
                    case (int)Float32_Type:                           //23- OCT String Len 4 
                        buffer = new byte[0x4]; nByteIndex++;
                        break;
                    case (int)Float64_Type:                           //24- OCT String Len 8 
                        buffer = new byte[0x8]; nByteIndex++;
                        break;
                    case (int)DateTime_Type:                         //25- OCT String Len 12 
                        buffer = new byte[0x0C]; nByteIndex++;
                        break;
                    case (int)Date_Type:                            //26- OCT String Len 5 
                        buffer = new byte[0x5]; nByteIndex++;
                        break;
                    case (int)Time_Type:                            //27- OCT String Len 4 
                        buffer = new byte[0x4]; nByteIndex++;
                        break;
                }

                if (buffer.Length > 0)
                {
                    int valuestart = nByteIndex;

                    Array.Copy(Blockdata, valuestart, buffer, 0, buffer.Length);
                    nByteIndex += buffer.Length;
                    //for (int bCnt = 0; bCnt < buffer.Length; bCnt++)
                    //{
                    //    buffer[bCnt] = Blockdata[bCnt + valuestart];
                    //    nByteIndex++;
                    //}
                    //data = FormatData(buffer, bUnsignFlag);
                    return buffer;
                }

                dataValue[0] = data;
                dataValue[1] = nByteIndex.ToString();
                return dataValue;
            }
            catch (Exception)
            {
                return null;
            }

        }
            
          public static string[] DLMSDataFormator(byte[] Blockdata, int nByteIndex, bool IsASCII)
        {
            try
            {
                bool bUnsignFlag = false;
                byte[] buffer = new byte[1];
                string data = "";
                string[] dataValue = new string[2];
                bool isASCIIString = false;
                int startdataIDX = nByteIndex;
                byte indexedDataType = (byte)Blockdata[nByteIndex];
            SWITCHAGAIN:
                switch (indexedDataType)
                {
                    case (int)Null_Type:                                    //0- NULL                   
                        break;
                    case (int)Array_Type:                                   //1- Array
                        buffer = new byte[Blockdata[nByteIndex + 1]]; nByteIndex += 2;
                        break;
                    case (int)Structure:                                    //2-Structure
                        buffer = new byte[Blockdata[nByteIndex + 1]]; nByteIndex += 2;
                        break;
                    case (int)Boolean_Type:                                 //3- Boolean
                        buffer = new byte[1]; nByteIndex += 1;
                        break;
                    case (int)BitString_Type:                              //4- Bit String
                        int bitLength = Blockdata[nByteIndex + 1];
                        if (Blockdata[nByteIndex + 1] == 0x81) nByteIndex += (Blockdata[nByteIndex + 2] / 8) + 3;
                        else nByteIndex += (Blockdata[nByteIndex + 1] / 8) + 2;
                        data = GetBitString(Blockdata);
                        buffer = null;
                        break;
                    case (int)DoubleLong_Type:                             //5- Double Long -- 4Byte
                        bUnsignFlag = true;
                        buffer = new byte[0x4]; nByteIndex++;
                        break;
                    case (int)DoubleLongUnsigned_Type:                     //6- Double Long Unsigned -- 4Byte
                        buffer = new byte[0x4]; nByteIndex++;
                        break;
                    case (int)OctetString_Type:                           //9- Oct String     
                        isASCIIString = true;
                        buffer = new byte[Blockdata[nByteIndex + 1]]; nByteIndex += 2;
                        break;
                    case (int)VisibleString_Type:                        //10- Sequence of ASCII String 
                        isASCIIString = true;
                        buffer = new byte[Blockdata[nByteIndex + 1]]; nByteIndex += 2;
                        break;
                    case (int)BCD_Type:                                  //13 - BCD
                        buffer = new byte[Blockdata[nByteIndex + 1]]; nByteIndex += 2;
                        break;
                    case (int)Integer_Type:                              //15- Integer 1Byte
                        bUnsignFlag = true;
                        buffer = new byte[0x1]; nByteIndex++;
                        break;
                    case (int)Unsigned_Type:                             //17- Unsigned 1Byte
                        buffer = new byte[0x1]; nByteIndex++;
                        break;
                    case (int)Long_Type:                                //16- Long Signed 2 byte
                        bUnsignFlag = true;
                        buffer = new byte[0x2]; nByteIndex++;
                        break;
                    case (int)LongUnsigned_Type:                        //18- Unsigned 2 Byte
                        buffer = new byte[0x2]; nByteIndex++;
                        break;
                    case (int)CompactArray_Type:                        //19- Unsigned 2 Byte
                        indexedDataType = Blockdata[nByteIndex + 1];
                        nByteIndex++;
                        goto SWITCHAGAIN;

                    case (int)Long64_Type:                             //20- Integer64   8 Byte
                        bUnsignFlag = true;
                        buffer = new byte[0x8]; nByteIndex++;
                        break;
                    case (int)Long64Unsigned_Type:                     //21- Unsigned64  8 Byte
                        buffer = new byte[0x8]; nByteIndex++;
                        break;
                    case (int)Enum_Type:                               //22- Enum
                        buffer = new byte[0x1]; nByteIndex++;
                        break;
                    case (int)Float32_Type:                           //23- OCT String Len 4 
                        buffer = new byte[0x4]; nByteIndex++;
                        break;
                    case (int)Float64_Type:                           //24- OCT String Len 8 
                        buffer = new byte[0x8]; nByteIndex++;
                        break;
                    case (int)DateTime_Type:                         //25- OCT String Len 12 
                        buffer = new byte[0x0C]; nByteIndex++;
                        break;
                    case (int)Date_Type:                            //26- OCT String Len 5 
                        buffer = new byte[0x5]; nByteIndex++;
                        break;
                    case (int)Time_Type:                            //27- OCT String Len 4 
                        buffer = new byte[0x4]; nByteIndex++;
                        break;
                }
                if (buffer != null && buffer.Length > 0)
                {
                    int valuestart = nByteIndex;

                    Array.Copy(Blockdata, valuestart, buffer, 0, buffer.Length);
                    nByteIndex += buffer.Length;
                    if (Blockdata[nByteIndex - (buffer.Length + 1)] == 0x0C && !IsASCII) data = FormatDate(buffer);
                    else if (IsASCII && isASCIIString) data = FormatASCIIData(buffer);
                    else data = FormatData(buffer, bUnsignFlag);
                    //else if (!IsASCII) data = FormatData(buffer, bUnsignFlag);
                    //else data = FormatASCIIData(buffer);

                }
                dataValue[0] = data;
                dataValue[1] = nByteIndex.ToString();
                return dataValue;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string FormatDate(byte[] buffer)
        {
            if (buffer[0] != 0xFF)
            {
                int nYear = 0;

                nYear = (nYear | (int)buffer[0]) << 8;
                nYear = (nYear | (int)buffer[1]);
                string year = nYear.ToString("d4");

                string month = buffer[2].ToString("d2");
                string day = buffer[3].ToString("d2");
                string time = "";
                if (buffer[7] == 0xFF)
                    time = buffer[5].ToString("d2") + ":" + buffer[6].ToString("d2");
                else
                    time = buffer[5].ToString("d2") + ":" + buffer[6].ToString("d2") + ":" + buffer[7].ToString("d2");

                string date = day + "/" + month + "/" + year + " " + time;
                return date;
            }
            else
                return "00/00/0000 00:00:00";


        }

        private static string GetBitString(byte[] ReceiveBuffer)
        {
            int dataindexByte = 18;
            int recBytelen = ReceiveBuffer[dataindexByte + 1];
            if (ReceiveBuffer[dataindexByte + 1] == 0x81) { recBytelen = ReceiveBuffer[dataindexByte + 2]; dataindexByte += 3; }
            else dataindexByte += 2;
            byte[] lsobjectData = new byte[recBytelen / 8];
            Array.Copy(ReceiveBuffer, dataindexByte, lsobjectData, 0, lsobjectData.Length);
            List<byte> convertedByteList = ReverseBitsofByteList(lsobjectData);
            BitArray myarra = new BitArray(convertedByteList.ToArray());

            var builder = new StringBuilder();
            foreach (var bit in myarra.Cast<bool>())
                builder.Append(bit ? "1" : "0");
            return builder.ToString();
        }
        public static List<byte> ReverseBitsofByteList(byte[] recByteList)
        {
            List<byte> convertedlist = new List<byte>();
            try
            {
                foreach (byte item in recByteList)
                {
                    char[] bitarr = Convert.ToString(item, 2).PadLeft(8, '0').ToCharArray();
                    Array.Reverse(bitarr);
                    convertedlist.Add((byte)Convert.ToInt32(new string(bitarr), 2));
                }
                return convertedlist;
            }
            catch (Exception)
            {
                return null;
            }
        }



        private static string FormatASCIIData(byte[] buffer)
        {
            string dataVal = string.Empty;
            int startDataindx = 0;
            string asciival = string.Empty;

            int stractcount = 0;
            int lengthodstruct = buffer.Length;//length of stract
            while (stractcount < lengthodstruct)
            {
                dataVal = buffer[startDataindx++].ToString("X");
                if (dataVal != "0") asciival = asciival + ((char)(Convert.ToInt32((dataVal), 16)));
                stractcount++;
            }

            return asciival;
        }

        public static string FormatData(byte[] buffer, bool bUnsignFlag)
        {
            StringBuilder sb = new StringBuilder();

            bool bSignFlag = false;
            Int64 tempVal = 0;
            for (int i = 0; i < buffer.Length; i++)
            {

                if (buffer[0] > 127)
                {

                    if (buffer.Length > 1)
                    {
                        if (bUnsignFlag) bSignFlag = true;

                    }
                }
                sb.Append(buffer[i].ToString("X2"));
            }

            if (bSignFlag == true)
            {
                if (buffer.Length == 4)
                {
                    tempVal = Convert.ToInt64("FFFFFFFF", 16) - (Convert.ToInt64(sb.ToString(), 16) - 1);
                    return "-" + tempVal.ToString();
                }
                else
                {
                    tempVal = Convert.ToInt32("FFFF", 16) - (Convert.ToInt64(sb.ToString(), 16) - 1);
                    return "-" + tempVal.ToString();
                }

            }
            else
            {
                return Convert.ToInt64(sb.ToString(), 16).ToString();
            }
        }

        /// <summary>
        /// Method Name : convertStringtobyteArr
        /// Purpose : This method is used to convert string data hex format to byte array
        /// </summary>
        /// <param name="strdata"></param>
        /// <returns></returns>
        public byte[] convertStringtobyteArr(string strdata)
        {
            List<byte> bretdata = new List<byte>();
            if (strdata == null || strdata.Length == 0) return null;
            if ((strdata.Length % 2) != 0) return null;
            int icount = 0;
            while (true)
            {
                string str = strdata.Substring(icount, 2);
                icount += 2;
                bretdata.Add(byte.Parse(str, System.Globalization.NumberStyles.AllowHexSpecifier));
                if (icount >= strdata.Length) break;
            }

            return bretdata.ToArray();

        }

        public List<string> splitmystring(string strdata, int isize)
        {
            List<string> a = strdata.ToCharArray().Select((c, i) => new { Char = c, Index = i }).GroupBy(o => o.Index / isize).Select(g => new String(g.Select(o => o.Char).ToArray())).ToList();
            return a;
        }

        // SNRM
        /// <summary>
        /// Send SNRM packet and Recieve and Check UA response
        /// </summary>
        /// <param name="nServerSAP"></param>
        /// <param name="nServerLowerMacAddress"></param>
        /// <param name="nClientSAP"></param>
        /// <returns></returns>
        public List<byte> GetCmdSNRM()
        {
            try
            {
                const byte SETSNRM = 0x93;
                const byte SETUA = 0x73;
                List<byte> datalist = new List<byte>();
                datalist.Add((byte)Constants.DLMSConstants.HDLC.TAG);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.FRAMETYPE);
                datalist.Add(0x00);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE0);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE1);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE2);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE3);
                datalist.Add(sourceaddress);
                datalist.Add(SETSNRM);
                datalist.Add(0);  // FCS
                datalist.Add(0);  // FCS
                byte[] barrLen = FillLength(datalist[1], datalist.Count);
                datalist[1] = barrLen[0];
                datalist[2] = barrLen[1];
                fGenerateFCS(datalist.ToArray(), 1, 8);
                datalist[datalist.Count - 2] = (byte)ucFcs[0];
                datalist[datalist.Count - 1] = (byte)ucFcs[1];
                datalist.Add((byte)Constants.DLMSConstants.HDLC.TAG);
                nCMDByte = SETUA;
                return datalist;

            }
            catch (Exception)
            {
                return null;
            }
        }

        // SNRM
        /// <summary>
        /// Send SNRM packet and Recieve and Check UA response
        /// </summary>
        /// <param name="nServerSAP"></param>
        /// <param name="nServerLowerMacAddress"></param>
        /// <param name="nClientSAP"></param>
        /// <returns></returns>
        public List<byte> GetAARQ(List<byte> cosempacket)
        {
            try
            {
                List<byte> datalist = new List<byte>();
                datalist.Add((byte)Constants.DLMSConstants.HDLC.TAG);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.FRAMETYPE);
                datalist.Add(0x00);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE0);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE1);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE2);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE3);
                datalist.Add(sourceaddress);
                datalist.Add(nCMDByte);
                datalist.Add(0);  // FCS
                datalist.Add(0);  // FCS
                datalist.AddRange(new byte[] { (byte)Constants.DLMSConstants.COSEM.LLC0, (byte)Constants.DLMSConstants.COSEM.LLC1, (byte)Constants.DLMSConstants.COSEM.LLC2 });
                datalist.AddRange(cosempacket);
                // Add two byte for FCS in Length Count
                byte[] barrLen = FillLength(datalist[1], datalist.Count + 2);
                datalist[1] = barrLen[0];
                datalist[2] = barrLen[1];
                fGenerateFCS(datalist.ToArray(), 1, 8);
                datalist[9] = (byte)ucFcs[0];
                datalist[10] = (byte)ucFcs[1];
                fGenerateFCS(datalist.ToArray(), 1, datalist.Count - 1);
                datalist.Add((byte)ucFcs[0]);
                datalist.Add((byte)ucFcs[1]);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.TAG);
                return datalist;

            }
            catch (Exception)
            {
                return null;
            }
        }

        // SNRM
        /// <summary>
        /// Send SNRM packet and Recieve and Check UA response
        /// </summary>
        /// <param name="nServerSAP"></param>
        /// <param name="nServerLowerMacAddress"></param>
        /// <param name="nClientSAP"></param>
        /// <returns></returns>
        public List<byte> GetDisConnect()
        {
            try
            {
                const byte SETDISC = 0x53;
                List<byte> datalist = new List<byte>();
                datalist.Add((byte)Constants.DLMSConstants.HDLC.TAG);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.FRAMETYPE);
                datalist.Add(0x00);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE0);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE1);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE2);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE3);
                datalist.Add(sourceaddress);
                datalist.Add(SETDISC);
                datalist.Add(0);  // FCS
                datalist.Add(0);  // FCS
                byte[] barrLen = FillLength(datalist[1], datalist.Count);
                datalist[1] = barrLen[0];
                datalist[2] = barrLen[1];
                fGenerateFCS(datalist.ToArray(), 1, 8);
                datalist[datalist.Count - 2] = (byte)ucFcs[0];
                datalist[datalist.Count - 1] = (byte)ucFcs[1];
                datalist.Add((byte)Constants.DLMSConstants.HDLC.TAG);
                nCMDByte = SETDISC;
                return datalist;

            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<byte> GetNormalHDLCPacket(List<byte>cosempacket, byte prenCMDByte)
        {
            try
            {
                nCMDByte = prenCMDByte;
                List<byte> datalist = new List<byte>();
                datalist.Add((byte)Constants.DLMSConstants.HDLC.TAG);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.FRAMETYPE);
                datalist.Add(0x00);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE0);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE1);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE2);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE3);
                datalist.Add(sourceaddress);
                fIncSend();
                datalist.Add(nCMDByte);
                datalist.Add(0);  // FCS
                datalist.Add(0);  // FCS
                datalist.AddRange(new byte[] { (byte)Constants.DLMSConstants.COSEM.LLC0, (byte)Constants.DLMSConstants.COSEM.LLC1 , (byte)Constants.DLMSConstants.COSEM.LLC2 });
                datalist.AddRange(cosempacket);
                // Add two byte for FCS in Length Count
                byte[] barrLen = FillLength(datalist[1], datalist.Count+2);
                datalist[1] = barrLen[0];
                datalist[2] = barrLen[1];
                fGenerateFCS(datalist.ToArray(), 1, 8);
                datalist[9] = (byte)ucFcs[0];
                datalist[10] = (byte)ucFcs[1];
                fGenerateFCS(datalist.ToArray(), 1, datalist.Count-1);
                datalist.Add((byte)ucFcs[0]);
                datalist.Add((byte)ucFcs[1]);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.TAG);
                return datalist;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<byte> GetNormalHDLCPacket(List<byte> cosempacket, int mpos)
        {
            try
            {   
                List<byte> datalist = new List<byte>();
                datalist.Add((byte)Constants.DLMSConstants.HDLC.TAG);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.FRAMETYPE);
                datalist.Add(0x00);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE0);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE1);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE2);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE3);
                datalist.Add(sourceaddress);
                fIncSend(mpos);
                datalist.Add(cmddicbyte[mpos]);
                datalist.Add(0);  // FCS
                datalist.Add(0);  // FCS
                datalist.AddRange(new byte[] { (byte)Constants.DLMSConstants.COSEM.LLC0, (byte)Constants.DLMSConstants.COSEM.LLC1, (byte)Constants.DLMSConstants.COSEM.LLC2 });
                datalist.AddRange(cosempacket);
                // Add two byte for FCS in Length Count
                byte[] barrLen = FillLength(datalist[1], datalist.Count + 2);
                datalist[1] = barrLen[0];
                datalist[2] = barrLen[1];
                fGenerateFCS(datalist.ToArray(), 1, 8);
                datalist[9] = (byte)ucFcs[0];
                datalist[10] = (byte)ucFcs[1];
                fGenerateFCS(datalist.ToArray(), 1, datalist.Count - 1);
                datalist.Add((byte)ucFcs[0]);
                datalist.Add((byte)ucFcs[1]);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.TAG);
                return datalist;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<byte> GetBlockHDLCPacket(List<byte> cosempacket, byte prenCMDByte)
        {
            try
            {
                nCMDByte = prenCMDByte;
                List<byte> datalist = new List<byte>();
                datalist.Add((byte)Constants.DLMSConstants.HDLC.TAG);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.FRAMETYPE);
                datalist.Add(0x00);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE0);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE1);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE2);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.DESTADDBYTE3);
                datalist.Add(sourceaddress);
                fIncSend();
                datalist.Add(nCMDByte);
                datalist.Add(0);  // FCS
                datalist.Add(0);  // FCS
                datalist.AddRange(new byte[] { (byte)Constants.DLMSConstants.COSEM.LLC0, (byte)Constants.DLMSConstants.COSEM.LLC1, (byte)Constants.DLMSConstants.COSEM.LLC2 });
                datalist.AddRange(cosempacket);
                // Add two byte for FCS in Length Count
                byte[] barrLen = FillLength(datalist[1], datalist.Count + 2);
                datalist[1] = barrLen[0];
                datalist[2] = barrLen[1];
                fGenerateFCS(datalist.ToArray(), 1, 8);
                datalist[9] = (byte)ucFcs[0];
                datalist[10] = (byte)ucFcs[1];
                fGenerateFCS(datalist.ToArray(), 1, datalist.Count - 1);
                datalist.Add((byte)ucFcs[0]);
                datalist.Add((byte)ucFcs[1]);
                datalist.Add((byte)Constants.DLMSConstants.HDLC.TAG);
                return datalist;

            }
            catch (Exception)
            {
                return null;
            }
        }


    }
}
