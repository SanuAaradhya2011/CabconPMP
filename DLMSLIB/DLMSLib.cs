/**
     namespace DLMSLIB
     1- public class HDLCLIB - version 1.0
     2- public class COSEMLIB - version 1.0      
     10/04/2012
     copyright Landis+Gyr Ltd, GDC Noida.
     summary - DLMS Library Class for COSEM and HDLS Layers
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
namespace DLMSLIB
{
    public class HDLCLIB
    {
        

        #region Declaration
       
        public byte nCMDByte = 0x00;
        #endregion
        public long InitializationCounter = 0;
        public int SecuritysuitByte = 0;
        #region HDLCFCS
        public int[] ucFcs = new int[2];
        int PPPINITFCS16 = 0xFFFF;
        int PPPGOODFCS16 = 0xF0B8;
        #region FCSTab
        int[] uifcstab = {  0x0000, 0x1189,0x2312,	0x329b,	0x4624,	0x57ad,	0x6536,	0x74bf,
	                        0x8c48,	0x9dc1,	0xaf5a,	0xbed3,	0xca6c,	0xdbe5,	0xe97e,	0xf8f7,
	                        0x1081,	0x0108,	0x3393,	0x221a,	0x56a5,	0x472c,	0x75b7,	0x643e,
	                        0x9cc9,	0x8d40,	0xbfdb,	0xae52,	0xdaed,	0xcb64,	0xf9ff,	0xe876,
	                        0x2102,	0x308b,	0x0210,	0x1399,	0x6726,	0x76af,	0x4434,	0x55bd,
	                        0xad4a,	0xbcc3,	0x8e58,	0x9fd1,	0xeb6e,	0xfae7,	0xc87c,	0xd9f5,
	                        0x3183,	0x200a,	0x1291,	0x0318,	0x77a7,	0x662e,	0x54b5,	0x453c,
	                        0xbdcb,	0xac42,	0x9ed9,	0x8f50,	0xfbef,	0xea66,	0xd8fd,	0xc974,
	                        0x4204,	0x538d,	0x6116,	0x709f,	0x0420,	0x15a9,	0x2732,	0x36bb,
	                        0xce4c,	0xdfc5,	0xed5e,	0xfcd7,	0x8868,	0x99e1,	0xab7a,	0xbaf3,
	                        0x5285,	0x430c,	0x7197,	0x601e,	0x14a1,	0x0528,	0x37b3,	0x263a,
	                        0xdecd,	0xcf44,	0xfddf,	0xec56,	0x98e9,	0x8960,	0xbbfb,	0xaa72,
	                        0x6306,	0x728f,	0x4014,	0x519d,	0x2522,	0x34ab,	0x0630,	0x17b9,
	                        0xef4e,	0xfec7,	0xcc5c,	0xddd5,	0xa96a,	0xb8e3,	0x8a78,	0x9bf1,
	                        0x7387,	0x620e,	0x5095,	0x411c,	0x35a3,	0x242a,	0x16b1,	0x0738,
	                        0xffcf,	0xee46,	0xdcdd,	0xcd54,	0xb9eb,	0xa862,	0x9af9,	0x8b70,
	                        0x8408,	0x9581,	0xa71a,	0xb693,	0xc22c,	0xd3a5,	0xe13e,	0xf0b7,
	                        0x0840,	0x19c9,	0x2b52,	0x3adb,	0x4e64,	0x5fed,	0x6d76,	0x7cff,
	                        0x9489,	0x8500,	0xb79b,	0xa612,	0xd2ad,	0xc324,	0xf1bf,	0xe036,
	                        0x18c1,	0x0948,	0x3bd3,	0x2a5a,	0x5ee5,	0x4f6c,	0x7df7,	0x6c7e,
	                        0xa50a,	0xb483,	0x8618,	0x9791,	0xe32e,	0xf2a7,	0xc03c,	0xd1b5,
	                        0x2942,	0x38cb,	0x0a50,	0x1bd9,	0x6f66,	0x7eef,	0x4c74,	0x5dfd,
	                        0xb58b,	0xa402,	0x9699,	0x8710,	0xf3af,	0xe226,	0xd0bd,	0xc134,
	                        0x39c3,	0x284a,	0x1ad1,	0x0b58,	0x7fe7,	0x6e6e,	0x5cf5,	0x4d7c,
	                        0xc60c,	0xd785,	0xe51e,	0xf497,	0x8028,	0x91a1,	0xa33a,	0xb2b3,
	                        0x4a44,	0x5bcd,	0x6956,	0x78df,	0x0c60,	0x1de9,	0x2f72,	0x3efb,
	                        0xd68d,	0xc704,	0xf59f,	0xe416,	0x90a9,	0x8120,	0xb3bb,	0xa232,
	                        0x5ac5,	0x4b4c,	0x79d7,	0x685e,	0x1ce1,	0x0d68,	0x3ff3,	0x2e7a,
	                        0xe70e,	0xf687,	0xc41c,	0xd595,	0xa12a,	0xb0a3,	0x8238,	0x93b1,
	                        0x6b46,	0x7acf,	0x4854,	0x59dd,	0x2d62,	0x3ceb,	0x0e70,	0x1ff9,
	                        0xf78f,	0xe606,	0xd49d,	0xc514,	0xb1ab,	0xa022,	0x92b9,	0x8330,
	                        0x7bc7,	0x6a4e,	0x58d5,	0x495c,	0x3de3,	0x2c6a,	0x1ef1,	0x0f78
                         };
        #endregion

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
        public bool fCheckFCS(byte[] RecvBuffer)
        {

            int nHDLCPktLength = (byte)(RecvBuffer[1] & 0x7) * 0x100 + (byte)(RecvBuffer[2]);// (byte)(RecvBuffer[1] & 0x1F) * 0x100 + (byte)(RecvBuffer[2]);    
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
        ///  Filling FCS bytes to Buffer
      /// </summary>
      /// <param name="Buffer"></param>
      /// <param name="upIndex"></param>
      /// <param name="lowIndex"></param>
        public void fFillFCS(byte[] Buffer, int upIndex, int lowIndex)
        {
            Buffer[upIndex] = Convert.ToByte(ucFcs[0]);
            Buffer[lowIndex] = Convert.ToByte(ucFcs[1]);
        }
     
        #endregion

        #region HDLCChecks
     /// <summary>
        /// Checking the Start and End tag
     /// </summary>
     /// <param name="Buffer"></param>
     /// <returns></returns>
        public bool fCheckStartEndTag(byte[] Buffer)
        {
            //int lengetz = (byte)(Buffer[1] & 0x1F) * 0x100 + (byte)(Buffer[2]);
            int lengetz = (byte)(Buffer[1] & 0x7) * 0x100 + (byte)(Buffer[2]);
            if (Buffer[0] == 0x7E && Buffer[lengetz + 1] == 0x7E) return true;
            else  return false;
             
        }
        /// <summary>
        /// Checking the Start and End tag
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nClientSAP"></param>
        /// <returns></returns>
        public bool fCheckServerSAP(byte[] Buffer, int nClientSAP)
        {
            int tempBuffer = 0;

            tempBuffer = 0;
            tempBuffer = Convert.ToByte(nClientSAP & 0x00FF);
            tempBuffer = tempBuffer << 1;
            tempBuffer = Convert.ToByte(tempBuffer & 0x00FF);
            tempBuffer = Convert.ToByte(tempBuffer | 0x01);
            if (Buffer[3] == tempBuffer) return true;
            else return false;
            
        }
        /// <summary>
        /// Checking the Start and End tag
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nCommandType"></param>
        /// <returns></returns>
        public bool fCheckCommand(byte[] Buffer, byte nCommandType)
        {
            if (Buffer[8] == nCommandType) return true;
            if (Buffer[8] == 0x1F) return false;  //Secandry Device Busy     
            
            //if (nCommandType == 0x73)
            //{
            //    if (Buffer[8] == 0x1F) return true;                     
            //}
            return false;                  
             
        }
        #endregion

        #region HDLCSequence
    /// <summary>
        ///  Set poll Final bit
    /// </summary>
    /// <param name="nStatus"></param>
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

            InitializationCounter++;
        }
        
     /// <summary>
        /// Increment Send Sequence number
     /// </summary>
        public void fIncSend()
        {
            int nSeqCounter = Convert.ToByte(nCMDByte & 0x0E);

            nSeqCounter = Convert.ToByte(nSeqCounter >> 1);
            nSeqCounter = Convert.ToByte(nSeqCounter + 1);

            nSeqCounter = nSeqCounter << 1;
            nSeqCounter = nSeqCounter & 0x00FF;

            nCMDByte = Convert.ToByte(nCMDByte & 0xF1);
            nCMDByte = Convert.ToByte(nCMDByte | nSeqCounter);
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
        /// Generate a new FCS for Buffer data
        /// </summary>
        /// <param name="seqindex"></param>
        /// <returns></returns>
        public int IncSendRecvSeqno(int seqindex)
        {
            byte[] sendseqno = { 0x10, 0x32, 0x54, 0x76, 0x98, 0xBA, 0xDC, 0xFE };
            return sendseqno[seqindex];
        }
        #endregion

        #region FillHDLCPacket
       /// <summary>
        /// add 0x7E to Buffer
       /// </summary>
       /// <param name="Buffer"></param>
       /// <param name="nBufferIndex"></param>
       /// <returns></returns>
        public int fAdd7E(byte[] Buffer, int nBufferIndex)
        {
            Buffer[nBufferIndex++] = 0x7E;
            return nBufferIndex;
        }
   /// <summary>
        /// add 0xA0 and space for Length to Buffer
   /// </summary>
   /// <param name="Buffer"></param>
   /// <param name="nBufferIndex"></param>
   /// <returns></returns>
        public int fAddHDLCFrameTag(byte[] Buffer, int nBufferIndex)
        {
            Buffer[nBufferIndex++] = 0xA0;
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// Conversion of Destination Address Values and Filling into Buffer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="nServerSAP"></param>
        /// <param name="ServerLowerMACAddress"></param>
        /// <returns></returns>
        public int fAddServerSAP(byte[] Buffer, int nBufferIndex, int nServerSAP, int ServerLowerMACAddress)
        {
            int tempBuffer = 0;

            tempBuffer = 0;
            tempBuffer = Convert.ToByte(nServerSAP & 0x00FF);
            tempBuffer = tempBuffer << 1;
            Buffer[nBufferIndex + 1] = Convert.ToByte(tempBuffer & 0x00FF);

            nServerSAP = nServerSAP << 2;
            Buffer[nBufferIndex] = Convert.ToByte((nServerSAP >> 8) & 0x00FF);
            Buffer[nBufferIndex] = Convert.ToByte(Buffer[nBufferIndex] & 0x00FF);

            nBufferIndex = Convert.ToByte(nBufferIndex + 2);

            tempBuffer = 0;
            tempBuffer = Convert.ToByte(ServerLowerMACAddress & 0x00FF);
            tempBuffer = tempBuffer << 1;
            tempBuffer = Convert.ToByte(tempBuffer & 0x00FF);
            Buffer[nBufferIndex + 1] = Convert.ToByte(tempBuffer | 0x01);

            ServerLowerMACAddress = ServerLowerMACAddress << 2;
            Buffer[nBufferIndex] = Convert.ToByte((ServerLowerMACAddress >> 8) & 0x00FF);
            Buffer[nBufferIndex] = Convert.ToByte(Buffer[nBufferIndex] & 0x00FE);

            nBufferIndex = Convert.ToByte(nBufferIndex + 2);

            return nBufferIndex;

        }
       /// <summary>
        /// Conversion of Client Address Values and Filling into Buffer
       /// </summary>
       /// <param name="Buffer"></param>
       /// <param name="nBufferIndex"></param>
       /// <param name="nClientSAP"></param>
       /// <returns></returns>
        public int fAddClientSAP(byte[] Buffer, int nBufferIndex, int nClientSAP)
        {
            int tempBuffer = 0;
            tempBuffer = Convert.ToByte(nClientSAP & 0x00FF);
            tempBuffer = tempBuffer << 1;
            Buffer[nBufferIndex] = Convert.ToByte(tempBuffer & 0x00FF);
            Buffer[nBufferIndex] = Convert.ToByte(tempBuffer | 0x01);
            nBufferIndex++;
            return nBufferIndex;
        }
      /// <summary>
        /// Fill Space for FCS/HCS in Buffer
      /// </summary>
      /// <param name="Buffer"></param>
      /// <param name="nBufferIndex"></param>
      /// <returns></returns>
        public int fAddBlankFCS(byte[] Buffer, int nBufferIndex)
        {
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
       /// <summary>
        /// Fill Command Byte in buffer
       /// </summary>
       /// <param name="Buffer"></param>
       /// <param name="nBufferIndex"></param>
       /// <returns></returns>
        public int fAddCmdByte(byte[] Buffer, int nBufferIndex)
        {
            Buffer[nBufferIndex++] = nCMDByte;
            return nBufferIndex;
        }
        /// <summary>
        /// Fill HDLC (SNRM) command Negotiation bytes
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="maxInfoFieldLenTransmit"></param>
        /// <param name="maxInfoFieldLenReceive"></param>
        /// <param name="windowSizeTransmit"></param>
        /// <param name="windowSizeReceive"></param>
        /// <returns></returns>
        public int fAddInfoFieldNegotiationBytes(byte[] Buffer, int nBufferIndex, int maxInfoFieldLenTransmit, int maxInfoFieldLenReceive, int windowSizeTransmit, int windowSizeReceive)
        {
            Buffer[nBufferIndex++] = 0x81; //format identifie
            Buffer[nBufferIndex++] = 0x80; //group identifie
            Buffer[nBufferIndex++] = 0x14;//group length (20 octets); 
            Buffer[nBufferIndex++] = 0x05; //parameter identifier (HDLC Layer maximum information field length – transmit); 
            Buffer[nBufferIndex++] = 0x02; //parameter length(2 octet);
            Buffer[nBufferIndex++] = Convert.ToByte((maxInfoFieldLenTransmit & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(maxInfoFieldLenTransmit & 0x00FF);
            Buffer[nBufferIndex++] = 0x06; //parameter identifier (HDLC Layer maximum information field length – Receive); 
            Buffer[nBufferIndex++] = 0x02;//parameter length(2 octet);
            Buffer[nBufferIndex++] = Convert.ToByte((maxInfoFieldLenReceive & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(maxInfoFieldLenReceive & 0x00FF);
            Buffer[nBufferIndex++] = 0x07; //parameter identifier (window size, transmit); 
            Buffer[nBufferIndex++] = 0x04;//parameter length(4 octet);
            Buffer[nBufferIndex++] = Convert.ToByte((windowSizeTransmit & 0xFF000000) >> 24);
            Buffer[nBufferIndex++] = Convert.ToByte((windowSizeTransmit & 0xFF0000) >> 16);
            Buffer[nBufferIndex++] = Convert.ToByte((windowSizeTransmit & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(windowSizeTransmit & 0x00FF);
            Buffer[nBufferIndex++] = 0x08;//parameter identifier (window size, Receive); 
            Buffer[nBufferIndex++] = 0x04;//parameter length(4 octet);
            Buffer[nBufferIndex++] = Convert.ToByte((windowSizeReceive & 0xFF000000) >> 24);
            Buffer[nBufferIndex++] = Convert.ToByte((windowSizeReceive & 0xFF0000) >> 16);
            Buffer[nBufferIndex++] = Convert.ToByte((windowSizeReceive & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(windowSizeReceive & 0x00FF);
            return nBufferIndex;
        }
        /// <summary>
        /// Conversion of Client Address Values and Filling into Buffer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        public void ffillLength(byte[] Buffer, int nBufferIndex)
        {
          //  Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
        
            UInt16 ilength = (UInt16)(nBufferIndex - 1);
            UInt16 ilength1 = (UInt16)(Buffer[1] * 0x100);
            ilength = (UInt16)(ilength1 | ilength);

            Buffer[1] = (byte)(ilength >> 8);
            Buffer[2] = (byte)(ilength);      //Filling length Byte

        }
        /// <summary>
        /// AARQ Buffer Data Length
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        public void ffillAARQBufferLength(byte[] Buffer, int nBufferIndex)
        {
            //  Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte

            UInt16 ilength = (UInt16)(nBufferIndex - 18); //--15 + 3 
            Buffer[15] = (byte)(ilength);      //Filling length Byte

        }

      /// <summary>
        /// Filling 0x09 0x01 0x00 into Buffer
      /// </summary>
      /// <param name="Buffer"></param>
      /// <param name="nBufferIndex"></param>
      /// <returns></returns>
        public int ffillData(byte[] Buffer, int nBufferIndex)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// Filling 0x09 0x01 0x00 into Buffer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="meterID"></param>
        /// <returns></returns>
        public int ffillMeterID(byte[] Buffer, int nBufferIndex, string meterID)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte

            byte len = Convert.ToByte(meterID.Length);
            Buffer[nBufferIndex++] = len;
            foreach (char ch in meterID)
            {
                Buffer[nBufferIndex++] = Convert.ToByte(ch);
            }
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="energyResolution"></param>
        /// <param name="MDResolution"></param>
        /// <param name="highResolution"></param>
        /// <returns></returns>
        public int ffillResolution(byte[] Buffer, int nBufferIndex, byte ResolutionByte)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = ResolutionByte;            
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="PriceByte"></param>
        /// <returns></returns>
        public int FillWriteParameters(byte[] Buffer, int nBufferIndex,List<byte> dataByte)
        {
            int byteIDX = 0;
            while (byteIDX < dataByte.Count)
            {
                Buffer[nBufferIndex++] = dataByte[byteIDX++];
            }            
            return nBufferIndex;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="LSFirstByte"></param>
        /// <param name="LSSecondByte"></param>
        /// <returns></returns>
        public int ffillLSCaptureObject(byte[] Buffer, int nBufferIndex, byte LSFirstByte, byte LSSecondByte)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = LSSecondByte;
            Buffer[nBufferIndex++] = LSFirstByte;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="firstByte"></param>
        /// <param name="secondByte"></param>
        /// <returns></returns>
        public int ffillLCDBacklight(byte[] Buffer, int nBufferIndex, byte firstByte, byte secondByte)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x11;
            Buffer[nBufferIndex++] = firstByte;
            Buffer[nBufferIndex++] = 0x11;
            Buffer[nBufferIndex++] = secondByte;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="firstByte"></param>
        /// <returns></returns>
        public int ffillCalibMethod(byte[] Buffer, int nBufferIndex, byte firstByte)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = firstByte;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="firstByte"></param>
        /// <returns></returns>
        public int ffillMtrLock(byte[] Buffer, int nBufferIndex, byte firstByte)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = firstByte;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="resetLockoutDays"></param>
        /// <returns></returns>
        public int ffillResetLockoutDays(byte[] Buffer, int nBufferIndex, byte resetLockoutDays)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = 0x11;
            Buffer[nBufferIndex++] = resetLockoutDays;

            return nBufferIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="displayParams"></param>
        /// <returns></returns>
        public int ffillDisplayParameters(byte[] Buffer, int nBufferIndex, List<byte> displayParams)
        {
            Buffer[nBufferIndex++] = Convert.ToByte(displayParams.Count);


            foreach (byte b in displayParams)
            {
                Buffer[nBufferIndex++] = b;
            }
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="Compartment1"></param>
        /// <param name="Compartment2"></param>
        /// <param name="Compartment3"></param>
        /// <param name="Compartment4"></param>
        /// <param name="Compartment5"></param>
        /// <param name="Compartment6"></param>
        /// <returns></returns>
        public int ffillNumberOfEvents(byte[] Buffer, int nBufferIndex, int Compartment1, int Compartment2, int Compartment3, int Compartment4, int Compartment5, int Compartment6)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = Convert.ToByte((Compartment1 & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(Compartment1 & 0x00FF);

            Buffer[nBufferIndex++] = Convert.ToByte((Compartment2 & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(Compartment2 & 0x00FF);

            Buffer[nBufferIndex++] = Convert.ToByte((Compartment3 & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(Compartment3 & 0x00FF);

            Buffer[nBufferIndex++] = Convert.ToByte((Compartment4 & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(Compartment4 & 0x00FF);

            Buffer[nBufferIndex++] = Convert.ToByte((Compartment5 & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(Compartment5 & 0x00FF);

            Buffer[nBufferIndex++] = Convert.ToByte((Compartment6 & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(Compartment6 & 0x00FF);

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="CTRatio"></param>
        /// <returns></returns>
        public int ffillCTRatio(byte[] Buffer, int nBufferIndex, byte CTRatio)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = CTRatio;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <returns></returns>
        public int ffillRTC(byte[] Buffer, int nBufferIndex,DateTime setrtcvalue,bool isAuto)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            DateTime setdatetime = System.DateTime.Now;
            if (!isAuto) setdatetime = setrtcvalue;
            Buffer[nBufferIndex++] = Convert.ToByte((setdatetime.Year & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(setdatetime.Year & 0x00FF);

            //Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = Convert.ToByte(setdatetime.Year % 100);
            Buffer[nBufferIndex++] = Convert.ToByte(setdatetime.Month);
            Buffer[nBufferIndex++] = Convert.ToByte(setdatetime.Day);
            Buffer[nBufferIndex++] = Convert.ToByte(setdatetime.DayOfWeek);
            Buffer[nBufferIndex++] = Convert.ToByte(setdatetime.Hour);
            Buffer[nBufferIndex++] = Convert.ToByte(setdatetime.Minute);
            Buffer[nBufferIndex++] = Convert.ToByte(setdatetime.Second);

            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = 0x80;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="baudRate"></param>
        /// <returns></returns>

        public int ffillBaudRate(byte[] Buffer, int nBufferIndex, int baudRate)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte


            if (baudRate == 9600)
            {
                Buffer[nBufferIndex++] = 0x05;
            }
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="integrationPeriod"></param>
        /// <returns></returns>

        public int ffillIntegrationPeriod(byte[] Buffer, int nBufferIndex, int integrationPeriod)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = Convert.ToByte((integrationPeriod & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(integrationPeriod & 0x00FF);

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="capturePeriod"></param>
        /// <returns></returns>
        public int ffillLSCapturePeriod(byte[] Buffer, int nBufferIndex, int capturePeriod)
        {
            Buffer[nBufferIndex++] = Convert.ToByte((capturePeriod & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(capturePeriod & 0x00FF);
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="scrollTime"></param>
        /// <param name="pushTimeout"></param>
        /// <param name="autoScrollTime"></param>
        /// <param name="autoScrollModeSelected"></param>
        /// <returns></returns>

        public int ffillDisplayParamsTimeouts(byte[] Buffer, int nBufferIndex, int scrollTime, int pushTimeout, int autoScrollTime, int autoScrollModeSelected)
        {
            Buffer[nBufferIndex++] = 0x12;
            Buffer[nBufferIndex++] = Convert.ToByte((pushTimeout & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(pushTimeout & 0x00FF);

            Buffer[nBufferIndex++] = 0x12;
            Buffer[nBufferIndex++] = Convert.ToByte((scrollTime & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(scrollTime & 0x00FF);

            Buffer[nBufferIndex++] = 0x0F;
            Buffer[nBufferIndex++] = Convert.ToByte(autoScrollModeSelected);

            Buffer[nBufferIndex++] = 0x12;
            Buffer[nBufferIndex++] = Convert.ToByte((autoScrollTime & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(autoScrollTime & 0x00FF);
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="interFrameTimeout"></param>
        /// <returns></returns>
        public int ffillInterFrameTimeout(byte[] Buffer, int nBufferIndex, int interFrameTimeout)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte

            Buffer[nBufferIndex++] = Convert.ToByte((interFrameTimeout & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(interFrameTimeout & 0x00FF);

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="InactivityTimeout"></param>
        /// <returns></returns>

        public int ffillInterActivityTimeout(byte[] Buffer, int nBufferIndex, int InactivityTimeout)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte

            Buffer[nBufferIndex++] = Convert.ToByte((InactivityTimeout & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(InactivityTimeout & 0x00FF);

            return nBufferIndex;
        }
        #endregion

        #region IrDA Methods

        //======================================================IrDA Specefic Methods===========================================
        /// <summary>
        /// Byte Position offset is 0,len
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public bool IrDACheckSyncWord_1P(byte[] Buffer, int recPayloadLen)
        {
            if (Buffer[0] == 0x3A && Buffer[recPayloadLen + 10] == 0x0A) return true;
            else return false;
        }
        /// <summary>
        /// Byte Position offset is 0,1
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public bool IrDACheckSyncWord(byte[] Buffer)
        {
            if (Buffer[0] == 0x95 && Buffer[1] == 0x95) return true;
            else return false;
        }
        /// <summary>
        /// To Calculate BCC Make xor with reach byte.
        /// BBC of Data Part only
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public bool IrDACheckBCC(byte[] Buffer)
        {
            byte bcc = 0x00;
            int recPayloadLen = Buffer[5]-1;
            if (Buffer[recPayloadLen] == 0x00) return true; //-----If no Payload data
            int dataIndex = 10; //------Data Start from index position 10th 
            while (dataIndex < recPayloadLen)
            {
                bcc += (byte)~Buffer[dataIndex++];
            }
            if (bcc != Buffer[recPayloadLen])return false;
            return true;
        }
        /// <summary>
        /// To Calculate BCC Make xor with reach byte.
        /// BBC of Data Part only
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public bool IrDACheckBCC_1P(byte[] Buffer, int recPayloadLen)
        {
            byte bcc = 0x00;
            int connadTypeByteLen = 2;
            int DataLen = 2;
            recPayloadLen = recPayloadLen + connadTypeByteLen + DataLen;
            int dataIndex = 1; //------Data Start from index position 10th 
            while (dataIndex <= recPayloadLen)
            {
                bcc += (byte)Buffer[dataIndex++];
               // bcc += (byte)~Buffer[dataIndex++];
            }
            bcc = (byte)~bcc; //--2's Compliment
            byte BCC1 = (byte)(bcc + 1);//--1's Compliment of 2's Compliment
            byte BCC2 = (byte)~BCC1;  ////--2's Compliment of BCC1

            StringBuilder sb = new StringBuilder();
            StringBuilder datasb = new StringBuilder();
            string bccstr = BCC1.ToString("X2") + BCC2.ToString("X2");
            foreach (char c in bccstr)
            {
                if (Buffer[dataIndex++].ToString("X2") != ((int)c).ToString("X2")) return false;
            }
            
            
            return true;
        }
        /// <summary>
        /// Byte Position offset is 2,3,4
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="hhuID"></param>
        /// <returns></returns>
        public bool IrDACheckHHUIP(byte[] Buffer, int hhuID)
        {
            int compValue = 0;
            compValue = (compValue | (int)Buffer[2]) << 16;
            compValue = (compValue | (int)Buffer[3]) << 8;
            compValue = (compValue | (int)Buffer[4]);
            if (compValue == hhuID) return true;
            else return false;
        }
        /// <summary>
        /// Byte Position offset is 6
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public bool IrDACheckCommandID(byte[] Buffer, byte IrDAReadCommandbyte)
        {
            if (Buffer[6] == IrDAReadCommandbyte) return true;
            else return false;
        }

        /// <summary>
        /// Byte Position offset is 7,8,9
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="hhuID"></param>
        /// <returns></returns>
        public bool IrDACheckMeterIP(byte[] Buffer, int meterID)
        {
            int compValue = 0;
            compValue = (compValue | (int)Buffer[7]) << 16;
            compValue = (compValue | (int)Buffer[8]) << 8;
            compValue = (compValue | (int)Buffer[9]);
            if (compValue == meterID) return true;
            else return false;
        }
        /// <summary>
        /// Byte Position offset is 7,8,9
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="hhuID"></param>
        /// <returns> Meter ID that is came from Meter response</returns>
        public int IrDAGetMeterIP(byte[] Buffer)
        {
            int compValue = 0;
            compValue = (compValue | (int)Buffer[7]) << 16;
            compValue = (compValue | (int)Buffer[8]) << 8;
            compValue = (compValue | (int)Buffer[9]);
            return compValue;
        }
        #endregion

    } 
    
    public class COSEMLIB
    {
        public string DedKeystr = string.Empty;
        public byte[] DedicatedKey=new byte[16]; 
        public int nMaxBufferSize = 0x4d;
        public DataSet _obisQueryDSet;
        public byte[] Readout_CMD = new byte[80];  //statically fixed query length 
        public int nBlockTotalByteCount = 0x00;
        public int nBlockNumber = 0x00;
        public int nBlockIndex = 0x00;
        public int nTotalPacketSize = 0x00;
        //>>ID-LS - Reported by Harish Sharma, ALI on 13-Dec-2011
        public byte[] BlockBuffer = new byte[1000000];
        #region FillCOSEMPacket
      /// <summary>
        /// add LLC Byte to Buffer
      /// </summary>
      /// <param name="Buffer"></param>
      /// <param name="nBufferIndex"></param>
      /// <returns></returns>
        public int fAddLLCByte(byte[] Buffer, int nBufferIndex)
        {
            Buffer[nBufferIndex++] = 0xE6;
            Buffer[nBufferIndex++] = 0xE6;
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
       /// <summary>
        /// add AARQ Tag and  Length to Buffer
       /// </summary>
       /// <param name="Buffer"></param>
       /// <param name="nBufferIndex"></param>
       /// <param name="nLength"></param>
       /// <returns></returns>
        public int fAddAARQTAG(byte[] Buffer, int nBufferIndex,byte nLength)
        {
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = nLength;
            return nBufferIndex;
        }
    /// <summary>
        /// add Context tag and Context name to Buffer
    /// </summary>
    /// <param name="Buffer"></param>
    /// <param name="nBufferIndex"></param>
    /// <param name="nContextType"></param>
    /// <returns></returns>
        public int fAddContext(byte[] Buffer, int nBufferIndex, byte nContextType)
        {
            //A109060760857405080101
            Buffer[nBufferIndex++] = 0xA1;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x06;
            Buffer[nBufferIndex++] = 0x07;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x85;
            Buffer[nBufferIndex++] = 0x74;
            Buffer[nBufferIndex++] = 0x05;
            Buffer[nBufferIndex++] = 0x08;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = nContextType;
            return nBufferIndex;
        }
        public int fAddContext_Cyphered(byte[] Buffer, int nBufferIndex, byte nContextType)
        {
            //A109060760857405080103
            Buffer[nBufferIndex++] = 0xA1;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x06;
            Buffer[nBufferIndex++] = 0x07;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x85;
            Buffer[nBufferIndex++] = 0x74;
            Buffer[nBufferIndex++] = 0x05;
            Buffer[nBufferIndex++] = 0x08;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = nContextType;
            return nBufferIndex;
        }
        public int fBeforeSysTitle(byte[] Buffer, int nBufferIndex, string SystemTitle)
        {
            Buffer[nBufferIndex++] = 0xA6;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x08;
            
            return nBufferIndex;
        }
        public int fSystemTitle(byte[] Buffer, int nBufferIndex, string SystemTitle)
        {
           
            Buffer[nBufferIndex++] = Convert.ToByte(SystemTitle[0]);
            Buffer[nBufferIndex++] = Convert.ToByte(SystemTitle[1]);
            Buffer[nBufferIndex++] = Convert.ToByte(SystemTitle[2]);
            Buffer[nBufferIndex++] = Convert.ToByte(SystemTitle[3]);
            Buffer[nBufferIndex++] = Convert.ToByte(SystemTitle[4]);
            Buffer[nBufferIndex++] = Convert.ToByte(SystemTitle[5]);
            Buffer[nBufferIndex++] = Convert.ToByte(SystemTitle[6]);
            Buffer[nBufferIndex++] = Convert.ToByte(SystemTitle[7]);
            return nBufferIndex;
        }
        
        /// <summary>
        ///  add Security Mechanism tag and Security Mechanism to Buffer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="nSecMechanism"></param>
        /// <returns></returns>
        public int fAddSecMechanism(byte[] Buffer, int nBufferIndex, byte nSecMechanism)
        {
            //8A0207808B0760857405080201
            Buffer[nBufferIndex++] = 0x8A;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x07;
            Buffer[nBufferIndex++] = 0x80;
            Buffer[nBufferIndex++] = 0x8B;
            Buffer[nBufferIndex++] = 0x07;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x85;
            Buffer[nBufferIndex++] = 0x74;
            Buffer[nBufferIndex++] = 0x05;
            Buffer[nBufferIndex++] = 0x08;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = nSecMechanism;
            return nBufferIndex;
        }
        /// <summary>
        ///  add password tag and password to Buffer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int fAddPassword(byte[] Buffer, int nBufferIndex, string password)
        {
            //AC0A80083132333435363738
            Buffer[nBufferIndex++] = 0xAC;
            Buffer[nBufferIndex++] = (byte)(password.Length + 2);// 0x0A;
            Buffer[nBufferIndex++] = 0x80;
            Buffer[nBufferIndex++] = (byte)password.Length;
            int countPassLen=0;
            while (countPassLen < password.Length)
            {
                Buffer[nBufferIndex++] = Convert.ToByte(password[countPassLen++]);
            }
            //Buffer[nBufferIndex++] = Convert.ToByte(password[1]);
            //Buffer[nBufferIndex++] = Convert.ToByte(password[2]);
            //Buffer[nBufferIndex++] = Convert.ToByte(password[3]);
            //Buffer[nBufferIndex++] = Convert.ToByte(password[4]);
            //Buffer[nBufferIndex++] = Convert.ToByte(password[5]);
            //Buffer[nBufferIndex++] = Convert.ToByte(password[6]);
            //Buffer[nBufferIndex++] = Convert.ToByte(password[7]);

            return nBufferIndex;
        }
        /// <summary>
        /// add password tag and password to Buffer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int fAddRandomKey(byte[] Buffer, int nBufferIndex, string password)
        {
            //AC0A80083132333435363738
            Buffer[nBufferIndex++] = 0xAC;
            Buffer[nBufferIndex++] = 0x12;
            Buffer[nBufferIndex++] = 0x80;
            Buffer[nBufferIndex++] = 0x10;
            Buffer[nBufferIndex++] = Convert.ToByte(password[0]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[1]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[2]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[3]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[4]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[5]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[6]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[7]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[8]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[9]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[10]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[11]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[12]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[13]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[14]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[15]);

            return nBufferIndex;
        }
     /// <summary>
        ///  add password tag and password to Buffer
     /// </summary>
     /// <param name="Buffer"></param>
     /// <param name="nBufferIndex"></param>
     /// <param name="HLSKey"></param>
     /// <returns></returns>
        public int fAddEncryptedKey(byte[] Buffer, int nBufferIndex, string HLSKey)
        {
            for (int i = 0; i < HLSKey.Length; i = i + 2)
            {
                Buffer[nBufferIndex++] = Convert.ToByte(HLSKey.Substring(i, 2), 16);
            }
            return nBufferIndex;
        }
       /// <summary>
        /// add User Info Tags and proposed DLMS Ver Number to Buffer
       /// </summary>
       /// <param name="Buffer"></param>
       /// <param name="nBufferIndex"></param>
       /// <returns></returns>
        public int fAddUserInf(byte[] Buffer, int nBufferIndex)
        {
            //BE10040E0100000006
            Buffer[nBufferIndex++] = 0xBE;
            Buffer[nBufferIndex++] = 0x10;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x0E;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;      
            Buffer[nBufferIndex++] = 0x06;      //Proposed DLMS Ver Number
            return nBufferIndex;
        }
        public int fAddUserInf_cypher(byte[] Buffer, int nBufferIndex,int DedicationKey)
        {
            //BE10040E0100000006
            int countlen = 0;
            int dedcounter = 0;
            DedKeystr = "";
            Buffer[nBufferIndex++] = 0x01;
            if (DedicationKey == 1)
            {
             // DedKeystr = "3C0B155618776321585A62117D735D41";
               DedKeystr = RandomHexString();
               Buffer[nBufferIndex++] = 0x01;
               Buffer[nBufferIndex++] = 0x10;
               while (countlen < DedKeystr.Length)
                {
                  Buffer[nBufferIndex++] = Convert.ToByte(DedKeystr.Substring(countlen, 2), 16);
                  DedicatedKey[dedcounter++] = Convert.ToByte(DedKeystr.Substring(countlen, 2), 16);
                  countlen += 2;
                }
            }
            else
                Buffer[nBufferIndex++] = 0x00;
          
                Buffer[nBufferIndex++] = 0x00;
                Buffer[nBufferIndex++] = 0x00;
                Buffer[nBufferIndex++] = 0x06;      //Proposed DLMS Ver Number
                return nBufferIndex;
        }

        public string RandomHexString()
        {
            Random rdm = new Random();
            string RandomhexVal = string.Empty;
            int num;

            for (int i = 0; i < 4; i++)
            {
                num = rdm.Next(0, int.MaxValue);
                RandomhexVal += num.ToString("X8");
            }

            return RandomhexVal;
        }
        public int SecuritySuitByte(byte[] Buffer, int nBufferIndex, int sootByte, int Dedicatekey)
        {
            //BE230421211F
            Buffer[nBufferIndex++] = 0xBE;
           if (Dedicatekey == 1)
              Buffer[nBufferIndex++] = 0x34;
           else
              Buffer[nBufferIndex++] = 0x23;

            Buffer[nBufferIndex++] = 0x04;
            if (Dedicatekey == 1)
             Buffer[nBufferIndex++] = 0x32;
            else
             Buffer[nBufferIndex++] = 0x21;

            Buffer[nBufferIndex++] = 0x21;
            if (Dedicatekey == 1)
                Buffer[nBufferIndex++] = 0x30;
            else
            Buffer[nBufferIndex++] = 0x1F;
            Buffer[nBufferIndex++] = Convert.ToByte(Convert.ToString(sootByte));
            return nBufferIndex;
        }
       /// <summary>
        /// add Conformance Block Tags and proposed Conformance Block to Buffer
       /// </summary>
       /// <param name="Buffer"></param>
       /// <param name="nBufferIndex"></param>
       /// <param name="cnfBlock"></param>
       /// <returns></returns>
        public int fAddCnfBlock(byte[] Buffer, int nBufferIndex, byte[] cnfBlock)
        {
            //5F1F040000121A
            Buffer[nBufferIndex++] = 0x5F;
            Buffer[nBufferIndex++] = 0x1F;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = cnfBlock[0];
            Buffer[nBufferIndex++] = cnfBlock[1];
            Buffer[nBufferIndex++] = cnfBlock[2];
            return nBufferIndex;
        }

        public int fAddCnfBlock_Cyphered(byte[] Buffer, int nBufferIndex, byte[] cnfBlock)
        {
            //5F1F040000121A
            Buffer[nBufferIndex++] = 0x5F;
            Buffer[nBufferIndex++] = 0x1F;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = cnfBlock[0];
            Buffer[nBufferIndex++] = cnfBlock[1];
            Buffer[nBufferIndex++] = cnfBlock[2];
            return nBufferIndex;
        }
        public int fAddAuthentication_Tag(byte[] Buffer, int nBufferIndex, byte[] AuthTag)
        {
            int bytecount = 0;
            while (bytecount < AuthTag.Length)
            {
                Buffer[nBufferIndex++] = AuthTag[bytecount++];
            }
           
            return nBufferIndex;
        }
        public int fAddCyphered_Tag(byte[] Buffer, int nBufferIndex, byte[] CypherTag)
        {
            int bytecount = 0;
            while (bytecount < CypherTag.Length)
            {
                Buffer[nBufferIndex++] = CypherTag[bytecount++];
            }
           
            return nBufferIndex;
        }

        public int fAddAuth_Tag(byte[] Buffer, int nBufferIndex, byte[] AuthTag)
        {

            Buffer[nBufferIndex++] = AuthTag[0];
            Buffer[nBufferIndex++] = AuthTag[1];
            Buffer[nBufferIndex++] = AuthTag[2];
            Buffer[nBufferIndex++] = AuthTag[3];
            Buffer[nBufferIndex++] = AuthTag[4];
            Buffer[nBufferIndex++] = AuthTag[5];
            Buffer[nBufferIndex++] = AuthTag[6];
            Buffer[nBufferIndex++] = AuthTag[7];
            Buffer[nBufferIndex++] = AuthTag[8];
            Buffer[nBufferIndex++] = AuthTag[9];
            Buffer[nBufferIndex++] = AuthTag[10];
            Buffer[nBufferIndex++] = AuthTag[11];
            return nBufferIndex;
        }
        public int fAddCyph_Tag(byte[] Buffer, int nBufferIndex, byte[] CypherTag)
        {

            Buffer[nBufferIndex++] = CypherTag[0];
            Buffer[nBufferIndex++] = CypherTag[1];
            Buffer[nBufferIndex++] = CypherTag[2];
            Buffer[nBufferIndex++] = CypherTag[3];
            Buffer[nBufferIndex++] = CypherTag[4];
            Buffer[nBufferIndex++] = CypherTag[5];
            Buffer[nBufferIndex++] = CypherTag[6];
            Buffer[nBufferIndex++] = CypherTag[7];
            Buffer[nBufferIndex++] = CypherTag[8];
            Buffer[nBufferIndex++] = CypherTag[9];
            Buffer[nBufferIndex++] = CypherTag[10];
            Buffer[nBufferIndex++] = CypherTag[11];
            Buffer[nBufferIndex++] = CypherTag[12];
            Buffer[nBufferIndex++] = CypherTag[13];
            return nBufferIndex;
        }

        public int fInvocationCounter(byte[] Buffer, int nBufferIndex,long InvCount)
        {
            //00000018
            //int clientinovationCount = GlobalObjects.objHDLCLIB.InitializationCounter;
            Buffer[nBufferIndex++] = Convert.ToByte((InvCount & 0xFF000000) >> 24);
            Buffer[nBufferIndex++] = Convert.ToByte((InvCount & 0xFF0000) >> 16);
            Buffer[nBufferIndex++] = Convert.ToByte((InvCount & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(InvCount & 0x00FF);
            return nBufferIndex;
        }
        
      /// <summary>
        /// add PDU Size to Buffer
      /// </summary>
      /// <param name="Buffer"></param>
      /// <param name="nBufferIndex"></param>
      /// <param name="PDUSize"></param>
      /// <returns></returns>
        public int fAddPDUSize(byte[] Buffer, int nBufferIndex, int PDUSize)
        {
            Buffer[nBufferIndex++] = Convert.ToByte((PDUSize >> 8) & 0x00ff);
            Buffer[nBufferIndex++] = Convert.ToByte(PDUSize & 0x00ff);
            return nBufferIndex;
        }
        public int fAddPDUSize_Cyphered(byte[] Buffer, int nBufferIndex, int PDUSize)
        {
            Buffer[nBufferIndex++] = Convert.ToByte((PDUSize >> 8) & 0x00ff);
            Buffer[nBufferIndex++] = Convert.ToByte(PDUSize & 0x00ff);
            return nBufferIndex;
        }
        #endregion
        public DataSet ObisQueryDSet
        {
            get
            {
                return _obisQueryDSet;
            }
            set
            {
                _obisQueryDSet = value;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="iIndex"></param>
        /// <returns></returns>
        public int GetQuery(byte[] Buffer,int nBufferIndex, int iIndex)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            string ClassID = ObisQueryDSet.Tables["CLASS"].Rows[iIndex][0].ToString().Trim();
            Buffer[nBufferIndex++] = Convert.ToByte(ClassID);
            string ObisCode = ObisQueryDSet.Tables["OBISCODE"].Rows[iIndex][0].ToString().Trim();
            string[] ObisCodeClass = ObisCode.Split('.');
            Buffer[nBufferIndex++] = Convert.ToByte(ObisCodeClass[0].ToString().Trim(), 16);    //Convert.ToByte(ObisCodeClass[0]);//0x00;
            Buffer[nBufferIndex++] = Convert.ToByte(ObisCodeClass[1].ToString().Trim(), 16);    //Convert.ToByte(OBIS_LIST[iIndex, 1]);//0x00;
            Buffer[nBufferIndex++] = Convert.ToByte(ObisCodeClass[2].ToString().Trim(), 16);    //Convert.ToByte(OBIS_LIST[iIndex, 2]);//0x60;
            Buffer[nBufferIndex++] = Convert.ToByte(ObisCodeClass[3].ToString().Trim(), 16);    //Convert.ToByte(OBIS_LIST[iIndex, 3]);//0x01;
            Buffer[nBufferIndex++] = Convert.ToByte(ObisCodeClass[4].ToString().Trim(), 16);    //Convert.ToByte(OBIS_LIST[iIndex, 4]);//0x00;
            Buffer[nBufferIndex++] = Convert.ToByte(ObisCodeClass[5].ToString().Trim(), 16);    //Convert.ToByte(OBIS_LIST[iIndex, 5]);//0xFF;
            string AttributeID = ObisQueryDSet.Tables["ATTRIBUTE"].Rows[iIndex][0].ToString().Trim();
            Buffer[nBufferIndex++] = Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        public int GetQueryOBISList(byte[] Buffer, int nBufferIndex,int Mode)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0F;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; // 0000280000FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x28;
            Buffer[nBufferIndex++] = 0x00;
            if ( Mode == 0x10 )
                Buffer[nBufferIndex++] = 0x01;
            else if (Mode == 0x20)
                Buffer[nBufferIndex++] = 0x02;
            else if (Mode == 0x30)
                Buffer[nBufferIndex++] = 0x03;
            else
                Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = 0x02;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>

        public int GetQueryInstantProfile(byte[] Buffer, int nBufferIndex,byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x5E;
            Buffer[nBufferIndex++] = 0x5B;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <returns></returns>
        public int GetQuerySAPList(byte[] Buffer, int nBufferIndex)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x11;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x41;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = 0x02;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>

        public int GetQueryInstantScalarProfile(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //01 00 5E 5B 03 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x5E;
            Buffer[nBufferIndex++] = 0x5B;
            Buffer[nBufferIndex++] = 0x03;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryResetEnergy(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryResetDailyLog(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryResetSoftwareMD(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <returns></returns>
        public int fGetQueryKey(byte[] Buffer, int nBufferIndex)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0F;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x28;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x03;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = 0x02;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x10;
            return nBufferIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryResetLoadSurvey(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryResetMD(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x03;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryResetTamper(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x05;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>

        public int GetQueryResetBillingData(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x06;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>

        public int GetQueryResetMagneticTamper(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x07;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryResetOthers(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; // 00 01 0A 08 00 FF
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x08;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryBillingScalarProfile(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x5E;
            Buffer[nBufferIndex++] = 0x5B;
            Buffer[nBufferIndex++] = 0x06;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryLoadSurveyScalarProfile(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x5E;
            Buffer[nBufferIndex++] = 0x5B;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryTamperScalarProfile(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x5E;
            Buffer[nBufferIndex++] = 0x5B;
            Buffer[nBufferIndex++] = 0x07;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryDailySurveyScalarProfile(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x9B;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteMeterID(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 00 255
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0A;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWritePCBAID(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 8B 255
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x8B;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x0D;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadMeterID(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 00 255
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadPCBAID(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 00 255
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x8B;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadInterFrameTimeout(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x17;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 01 16 00 00 FF
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x16;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadInterActivityTimeout(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x17;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 01 16 00 00 FF
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x16;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteResolution(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 B1 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x84;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x11;
           // Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteToMeter(List<byte>databyte , byte[] Buffer, int nBufferIndex, byte atb, byte[] ParameterOBIS, byte ParaclassID, byte typeodData, byte lengthofData, byte[] DataRequestType)
        {
            Buffer[nBufferIndex++] = DataRequestType[0];
            Buffer[nBufferIndex++] = DataRequestType[1];
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = ParaclassID;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = ParameterOBIS[0]; //OBIS
            Buffer[nBufferIndex++] = ParameterOBIS[1];
            Buffer[nBufferIndex++] = ParameterOBIS[2];
            Buffer[nBufferIndex++] = ParameterOBIS[3];
            Buffer[nBufferIndex++] = ParameterOBIS[4];
            Buffer[nBufferIndex++] = ParameterOBIS[5]; 
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex] = 0x00;

            /*
             * This check is added to take care of optional data field as discuss with Priyanshu on 04-06-2015
             * 
             */
            if (DataRequestType[0] == 0xC3 && DataRequestType[1] == 0x01 && databyte.Count > 0)
                Buffer[nBufferIndex] = 0x01;

            nBufferIndex++;

            if (typeodData == 255 && lengthofData == 255) 
            {
                return nBufferIndex;
            }
            else if (lengthofData >=128) 
            {
                Buffer[nBufferIndex++] = typeodData;
                Buffer[nBufferIndex++] = 0x81;
                Buffer[nBufferIndex++] = lengthofData;
            }
            else 
            {
                Buffer[nBufferIndex++] = typeodData;
                if (lengthofData > 0) Buffer[nBufferIndex++] = lengthofData;
            }

            return nBufferIndex;
        }
        public byte GetQueryWriteMethodToMeter(byte[] Buffer, byte nBufferIndex, byte atb, byte[] ParameterOBIS, byte ParaclassID, byte typeodData, byte lengthofData, byte[] DataRequestType,byte AccessSelector)
        {
            Buffer[nBufferIndex++] = DataRequestType[0];
            Buffer[nBufferIndex++] = DataRequestType[1];
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = ParaclassID;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = ParameterOBIS[0]; //OBIS
            Buffer[nBufferIndex++] = ParameterOBIS[1];
            Buffer[nBufferIndex++] = ParameterOBIS[2];
            Buffer[nBufferIndex++] = ParameterOBIS[3];
            Buffer[nBufferIndex++] = ParameterOBIS[4];
            Buffer[nBufferIndex++] = ParameterOBIS[5];
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = AccessSelector;
            if (typeodData == 255 && lengthofData == 255) return nBufferIndex;
            else
            {
                Buffer[nBufferIndex++] = typeodData;
                if (lengthofData > 0) Buffer[nBufferIndex++] = lengthofData;
            }

            return nBufferIndex;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteKVAhSelection(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 8F FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x8F;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x01;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>

        public int GetQueryWriteLSCaptureObject(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 85 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x85;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x12;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteLCDBacklight(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;  //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;  //00	00	60	01	8A	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x8A;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;   //Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteCalibMethod(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	60	01	91	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x91;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteMtrLock(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	60	01	91	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x92;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteResetLockoutDays(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01; //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	60	01	89	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x89;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteTOU(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x14; //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	0D	00	00	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0D;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteTOUBlock(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x14; //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	0D	00	00	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0D;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }

        public int GetQueryToWriteBlockToMeter(byte[] Buffer, int nBufferIndex, byte atb, byte[] ParameterOBIS, byte ParaclassID, byte typeodData, int lengthofData, byte[] DataRequestType)
        {
            Buffer[nBufferIndex++] = DataRequestType[0];
            Buffer[nBufferIndex++] = DataRequestType[1];
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = ParaclassID;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = ParameterOBIS[0]; //OBIS
            Buffer[nBufferIndex++] = ParameterOBIS[1];
            Buffer[nBufferIndex++] = ParameterOBIS[2];
            Buffer[nBufferIndex++] = ParameterOBIS[3];
            Buffer[nBufferIndex++] = ParameterOBIS[4];
            Buffer[nBufferIndex++] = ParameterOBIS[5];
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
           
            return nBufferIndex;
        }

        public int GetQueryToWriteBlockToMeterWithoutAccessSelector(byte[] Buffer, int nBufferIndex, byte atb, byte[] ParameterOBIS, byte ParaclassID, byte typeodData, int lengthofData, byte[] DataRequestType)
        {
            Buffer[nBufferIndex++] = DataRequestType[0];
            Buffer[nBufferIndex++] = DataRequestType[1];
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = ParaclassID;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = ParameterOBIS[0]; //OBIS
            Buffer[nBufferIndex++] = ParameterOBIS[1];
            Buffer[nBufferIndex++] = ParameterOBIS[2];
            Buffer[nBufferIndex++] = ParameterOBIS[3];
            Buffer[nBufferIndex++] = ParameterOBIS[4];
            Buffer[nBufferIndex++] = ParameterOBIS[5];
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            return nBufferIndex;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadTOU(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x14; //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	0D	00	00	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0D;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="Buffer"></param>
       /// <param name="nBufferIndex"></param>
       /// <param name="atb"></param>
       /// <returns></returns>
        public int GetQueryWriteTamperThreshold(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x02;          //block transfer
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01; //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	60	01	89	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadTamperThreshold(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;          //block transfer
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01; //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	60	01	89	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWritePushDisplayParameter(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 86 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x86;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteScrollDisplayParameter(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 87 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x87;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteHighResolutionDisplayParameter(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 88 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x88;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteDisplayParameterTimeouts(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 80 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x80;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x04;
            //Buffer[nBufferIndex++] = 0x12;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteEvents(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 82 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x82;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x0C;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadResolution(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 84 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x84;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
       


        public int GetQueryReadByClassOBIS(byte[] Buffer, int nBufferIndex, byte atb,byte[] obisCode,byte classCode)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;//0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = classCode;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = obisCode[0]; //00 00 60 01 83 FF
            Buffer[nBufferIndex++] = obisCode[1];
            Buffer[nBufferIndex++] = obisCode[2];
            Buffer[nBufferIndex++] = obisCode[3];
            Buffer[nBufferIndex++] = obisCode[4];
            Buffer[nBufferIndex++] = obisCode[5];
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
             
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadKVAhSelection(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 8f FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x8F;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteRTC(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x08;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 01 00 00 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x0C;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWritePassword(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0F;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; ////00	00	28	00	00	FF
            Buffer[nBufferIndex++] = 0x00; 
            Buffer[nBufferIndex++] = 0x28;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadMeterPassword(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0F;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	28	00	00	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x28;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadRTC(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x08;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 01 00 00 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x0C;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteBaudRate(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x17;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 01 16 00 00 255
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x16;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x16;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadBaudRate(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x17;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 01 16 00 00 255
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x16;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x16;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteIntegrationPeriod(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //01 00 00 08 00 255
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x08;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x12;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteLSCapturePeriod(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //0x01,0x00,0x00,0x08,0x04,0xFF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x08;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x12;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteManuYear(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; 
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x12;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>

        public int GetQueryReadIntegrationPeriod(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //01 00 00 08 00 255
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x08;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x11;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteInterFrameTimeout(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x17;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 01 16 00 00 FF
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x16;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x12;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteInactivityTimeout(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x17;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 01 16 00 00 255
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x16;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x12;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryWriteCTRatio(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //01 00 00 04 02 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x11;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadCTRatio(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //01 00 00 04 02 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x11;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadLSCapturePeriod(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //0x01,0x00,0x00,0x08,0x04,0xFF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x08;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x11;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadManuYear(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //0x01,0x00,0x00,0x08,0x04,0xFF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x11;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <param name="paramDate"></param>
        /// <param name="paramHour"></param>
        /// <param name="paramMinute"></param>
        /// <returns></returns>
        public int GetQueryWriteBillingDatetime(byte[] Buffer, int nBufferIndex, byte atb, byte paramDate, byte paramHour, byte paramMinute)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x16;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 01 0F 00 00 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0F;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x02;
           
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = paramHour;
            Buffer[nBufferIndex++] = paramMinute;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = 0xFF;

            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x05;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = paramDate;
            Buffer[nBufferIndex++] = 0xFF;
            return nBufferIndex;
        }


        public int GetQueryWriteBillingCycle(byte[] Buffer, int nBufferIndex, byte atb, byte BillCycle)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 B0 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xB0;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x01;
            //Buffer[nBufferIndex++] = 0x01;
            //Buffer[nBufferIndex++] = 0x02;
            //Buffer[nBufferIndex++] = 0x02;

            Buffer[nBufferIndex++] = 0x11;
            //Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = BillCycle;
            //Buffer[nBufferIndex++] = paramMinute;
            //Buffer[nBufferIndex++] = 0xFF;
            //Buffer[nBufferIndex++] = 0xFF;

            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x05;
            //Buffer[nBufferIndex++] = 0xFF;
            //Buffer[nBufferIndex++] = 0xFF;
            //Buffer[nBufferIndex++] = 0xFF;
            //Buffer[nBufferIndex++] = paramDate;
            //Buffer[nBufferIndex++] = 0xFF;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadBillingDatetime(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x16;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 01 0F 00 00 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0F;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x01;
            //Buffer[nBufferIndex++] = 0x01;
            //Buffer[nBufferIndex++] = 0x02;
            //Buffer[nBufferIndex++] = 0x02;
            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x05;
            //Buffer[nBufferIndex++] = 0xFF;
            //Buffer[nBufferIndex++] = 0xFF;
            //Buffer[nBufferIndex++] = 0xFF;
            //Buffer[nBufferIndex++] = paramDate;
            //Buffer[nBufferIndex++] = 0xFF;
            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x04;
            //Buffer[nBufferIndex++] = paramHour;
            //Buffer[nBufferIndex++] = paramMinute;
            //Buffer[nBufferIndex++] = 0xFF;
            //Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        public int GetQueryReadBillingCycle(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //01 00 60 01 B0 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xB0;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;            
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadLSCaptureObject(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 85 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x85;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadTamperEvents(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 82 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x82;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadPushDisplayParameter(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 86 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x86;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadPushDisplayParameterTimeouts(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; ///00 00 60 01 80 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x80;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadLCDBacklight(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;  //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;  ///00 00 60 01 8A FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x8A;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadCalibMethod(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;  //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;  ///00 00 60 01 91 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x91;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadNeutralCurrent(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;  //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;  ///00 00 60 01 91 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x93;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadPCBAStatus(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;  //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;  ///00 00 60 01 91 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x94;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryMRUSWritePassword(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; ////00	00	28	00	00	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x8E;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x10;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryMRUSReadMeterPassword(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	28	00	00	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x8E;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadLockOutDays(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;  //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;  ///00 00 60 01 89 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x89;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadScrollDisplayParameter(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 87 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x87;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int GetQueryReadHighResolutionDisplayParameter(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 88 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x88;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }
        /// <summary>
                /// Check AARQ Response
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public bool fCheckAARQResponse(byte[] Buffer)
        {
            int nCosemIndex = 0x0E;
            if (Buffer[nCosemIndex] != 0x61) return false;//Fail   //AARQ.response
            nCosemIndex = nCosemIndex + 17;
            if (Buffer[nCosemIndex] == 0x00) return true;    /// Success   //Association response
            return false; ///fail 
        }
       /// <summary>
        /// add PDU Size to Buffer
       /// </summary>
       /// <param name="Buffer"></param>
       /// <param name="nBufferIndex"></param>
       /// <param name="nDataArray"></param>
       /// <param name="flgTransfer"></param>
       /// <returns></returns>
        public int fSetBlockTransferPacket(byte[] Buffer, int nBufferIndex, byte[] nDataArray, bool flgTransfer)
        {  

            if (!flgTransfer)
            {
                for (int i = 0; i < nTotalPacketSize; i++)
                {
                    BlockBuffer[i] = nDataArray[i];
                }
                nBlockNumber = 0x01;
                nBlockIndex = 0x00;
                nMaxBufferSize = 0x4d;
                
            }
            if ((nTotalPacketSize - nBlockIndex) <= nMaxBufferSize)
            {
                nMaxBufferSize = nTotalPacketSize - nBlockIndex;
                Buffer[nBufferIndex++] = 0x01;
            }
            else
                Buffer[nBufferIndex++] = 0x00;

            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = Convert.ToByte(nBlockNumber >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(nBlockNumber & 0xFF);

            if (nMaxBufferSize < 256)
            Buffer[nBufferIndex++] = Convert.ToByte(nMaxBufferSize);
            else
            {
                Buffer[nBufferIndex++] = 0x81;
                Buffer[nBufferIndex++] = (byte)(nMaxBufferSize >> 8);
                Buffer[nBufferIndex++] = (byte)(nMaxBufferSize);
            }
           
            for (int i = 0; i < nMaxBufferSize; i++)
            {
                Buffer[nBufferIndex++] = BlockBuffer[nBlockIndex++];
            }

            return nBufferIndex;
        }


        public int fSetImgBlockTransferPacket(byte[] Buffer, int nBufferIndex, byte[] nDataArray, bool flgTransfer, List<byte> imgfooter)
        {         
            if (!flgTransfer)
            {
                for (int i = 0; i < nTotalPacketSize; i++)
                {
                    BlockBuffer[i] = nDataArray[i];
                }
                nBlockNumber = 0x01;
                nBlockIndex = 0x00;

            }
            if ((nTotalPacketSize - nBlockIndex) <= nMaxBufferSize)
            {
                nMaxBufferSize = nTotalPacketSize - nBlockIndex;
                Buffer[nBufferIndex++] = 0x01;
               // islastpkt = true;
            }
            else
                Buffer[nBufferIndex++] = 0x00;

            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = Convert.ToByte(nBlockNumber >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(nBlockNumber & 0xFF);

            if (nMaxBufferSize < 128)
                Buffer[nBufferIndex++] = Convert.ToByte(nMaxBufferSize);
            else if (nMaxBufferSize > 127 && nMaxBufferSize < 256)
            {
                Buffer[nBufferIndex++] = 0x81;
                Buffer[nBufferIndex++] = (byte)(nMaxBufferSize);
            }
            else if (nMaxBufferSize >= 256)
            {
                Buffer[nBufferIndex++] = 0x82;
                Buffer[nBufferIndex++] = (byte)(nMaxBufferSize >> 8);
                Buffer[nBufferIndex++] = (byte)(nMaxBufferSize);
            }

            for (int i = 0; i < nMaxBufferSize; i++)
            {
                Buffer[nBufferIndex++] = BlockBuffer[nBlockIndex++];
            }



            return nBufferIndex;
        }

      /// <summary>
        /// add PDU Size to Buffer
      /// </summary>
      /// <param name="Buffer"></param>
      /// <returns></returns>
        public int fCheckCOSEMResponse(byte[] Buffer)
        {
            int nCosemIndex = 14;
            if (Buffer[nCosemIndex] != 0xC4 )  return 0x03;//Tag Mismatch  //Get.response
                nCosemIndex = nCosemIndex + 1 ;
                if (Buffer[nCosemIndex] == 0x02 )   //Get.response.Next
                {
                    nCosemIndex = nCosemIndex + 2;
                    if (Buffer[nCosemIndex] == 0x00)   //Get.response.Next
                    {
                        nCosemIndex = nCosemIndex + 3;
                     

                        nBlockNumber = 0;
                        nBlockNumber = nBlockNumber | (int)Buffer[nCosemIndex] << 8;
                        nCosemIndex++;
                        nBlockNumber = nBlockNumber | (int)Buffer[nCosemIndex];

                        nCosemIndex = nCosemIndex + 2;
                        int nBlockByteCount = Buffer[nCosemIndex];
                        if (Buffer[nCosemIndex] == 0x82) { nBlockByteCount = ((byte)(Buffer[nCosemIndex + 1] & 0x1F) * 0x100 + (byte)(Buffer[nCosemIndex + 2])); nCosemIndex += 2; }
                        else if (Buffer[nCosemIndex] == 0x81) { nBlockByteCount = ((byte)(Buffer[nCosemIndex + 1])); nCosemIndex += 1; }
                        nCosemIndex = nCosemIndex + 1;  //Skipping the block Count Byte
                        for (int i = 0; i < nBlockByteCount ; i++)
                        {
                            BlockBuffer[nBlockIndex++] = Buffer[nCosemIndex++];
                        }
                        return 0x02;
                    }
                    else 
                    {
                        nCosemIndex = nCosemIndex + 6;
                        int nBlockByteCount = Buffer[nCosemIndex];
                        if (Buffer[nCosemIndex] == 0x82)  { nBlockByteCount = ((byte)(Buffer[nCosemIndex + 1] & 0x1F) * 0x100 + (byte)(Buffer[nCosemIndex + 2])); nCosemIndex += 2; }
                        else if (Buffer[nCosemIndex] == 0x81) { nBlockByteCount = ((byte)(Buffer[nCosemIndex + 1])); nCosemIndex += 1; }
                        nCosemIndex = nCosemIndex + 1;  //Skipping the block Count Byte
                        for (int i = 0; i < nBlockByteCount; i++)
                        {
                            BlockBuffer[nBlockIndex++] = Buffer[nCosemIndex++];
                        }
                        
                        nBlockTotalByteCount = nBlockIndex;
                        nBlockNumber = 0x00;
                        nBlockIndex = 0x00;
                        return 0x01; ///Block Trasfer Completed
                    }
                }
                else 
                {
                     nCosemIndex = nCosemIndex + 2;
                     if (Buffer[nCosemIndex] == 0x00)   //Get.response.Normal
                     {
                         //code written to handle tamper compartment readout when data comes in normal transfer 
                         int nBlockByteCount = Buffer[nCosemIndex + 2];
                         if (nBlockByteCount == 0x00)
                         {
                             
                             return 0x07; ///Get.Request.normal
                         }
                         else
                         {
                             
                             nBlockByteCount = (byte)(Buffer[nCosemIndex - 16] & 0x1F) * 0x100 + (byte)(Buffer[nCosemIndex - 15]);
                             nBlockByteCount = nBlockByteCount - 20;

                             if (Buffer[nCosemIndex] == 0x82) { nBlockByteCount = ((byte)(Buffer[nCosemIndex + 1] & 0x1F) * 0x100 + (byte)(Buffer[nCosemIndex + 2])); nCosemIndex += 2; }
                             else if (Buffer[nCosemIndex] == 0x81) { nBlockByteCount = ((byte)(Buffer[nCosemIndex + 1])); nCosemIndex += 1; }

                             nCosemIndex = nCosemIndex + 1;
                             for (int i = 0; i <= nBlockByteCount; i++)  //included = in the condition for tamper compartment readout 
                             {
                                 BlockBuffer[nBlockIndex++] = Buffer[nCosemIndex++];
                             }
                             
                             nBlockTotalByteCount = nBlockIndex;
                             nBlockNumber = 0x00;
                             nBlockIndex = 0x00;
                             return 0x01; ///Get.Request.normal
                         }
                        
                     }
                     else
                     {
                         return 0x05; ///Get.Request.normal
                     }
                }
             
        }
        /// <summary>
                /// add PDU Size to Buffer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public int fCheckCOSEMResponseForSet(byte[] Buffer)
        {
            int nCosemIndex = 14;
            if (Buffer[nCosemIndex] == 0xC5) //Get.response Normal
            { //Fail 
                
                if (Buffer[++nCosemIndex] == 0x02)   //Get.response.Next
                {
                     nCosemIndex = nCosemIndex + 2;
                     if (Buffer[nCosemIndex] != 0x00) return 0x04;//Get.response.Next
                     //if ( (nTotalPacketSize > nBlockIndex)  ){ nBlockNumber++;  return 0x04;  }  /// Access Denied
                     else   return 0x01; ///Get.Request.normal
                }
                else
                {
                    nCosemIndex = nCosemIndex + 2;
                    if (Buffer[nCosemIndex] != 0x00)  return 0x02;    // Access Denied  //Get.response.Next
                    return 0x01; ///Get.Request.normal
                     
                }
            }
            else if (Buffer[nCosemIndex] == 0xC7)//Action.response Normal
            {
                
                if (Buffer[++nCosemIndex] == 0x01)   //Action.response.Next
                {
                    nCosemIndex = nCosemIndex + 2;
                    if (Buffer[nCosemIndex] != 0x00) { nBlockNumber++; return 0x04; }   //Action.response.Next
                    //if ((nTotalPacketSize > nBlockIndex)) { nBlockNumber++; return 0x04; }    /// Access Denied
                    else return 0x01; ///Action.Request.normal
                       
                }
                else
                {
                    nCosemIndex = nCosemIndex + 2;
                    if (Buffer[nCosemIndex] == 0x01) return 0x02;    // Access Denied  //Get.response.Next
                    return 0x01; ///Get.Request.normal

                }
            }
            else return 0x03;
            
        }

        /// <summary>
        /// add PDU Size to Buffer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public int fCheckCOSEMResponseForImageBlockSet(byte[] Buffer)
        {
            int nCosemIndex = 14;
            if (Buffer[nCosemIndex] == 0xC5) //Get.response Normal
            { //Fail 

                if (Buffer[++nCosemIndex] == 0x02)   //Get.response.Next
                {
                    nCosemIndex = nCosemIndex + 2;
                    if (Buffer[nCosemIndex] != 0x00) { return 0x02; }  //Get.response.Next
                    else if ((nTotalPacketSize > nBlockIndex)) { nBlockNumber++; return 0x04; }  /// Access Denied
                    else return 0x01; ///Get.Request.normal
                }
                else
                {
                    nCosemIndex = nCosemIndex + 2;
                    //if (Buffer[nCosemIndex] == 0x01) return 0x02;    // Access Denied  //Get.response.Next
                    if (Buffer[nCosemIndex] != 0x00) return 0x02;    // Access Denied  //Get.response.Next
                    return 0x01; ///Get.Request.normal

                }
               

            }
            else if (Buffer[nCosemIndex] == 0xC7)//Action.response Normal
            {
                ++nCosemIndex;
                if (Buffer[nCosemIndex] == 0x04)   //Action.response.Next
                {
                    nCosemIndex = nCosemIndex + 2;
                    if ((nTotalPacketSize > nBlockIndex)) { nBlockNumber++; return 0x04; }    /// Access Denied
                    else return 0x01; ///Action.Request.normal

                }
                else if (Buffer[nCosemIndex] == 0x01)
                {
                    nCosemIndex = nCosemIndex + 2;
                    if (Buffer[nCosemIndex] != 0x00) return 0x02;    // Access Denied  //Get.response.Next
                    return 0x01; ///Get.Request.normal

                }
                else 
                    return 0x01;

            }
            else return 0x03;

        }

       /// <summary>
        /// add PDU Size to Buffer
       /// </summary>
       /// <param name="Buffer"></param>
       /// <returns></returns>
        public int fCheckCOSEMResponseForGet(byte[] Buffer)
        {
            int nCosemIndex = 14;
            if (Buffer[nCosemIndex] != 0xC4)  return 0x00;//Fail  //Get.response
            nCosemIndex = nCosemIndex + 3;
                if (Buffer[nCosemIndex] == 0x01)   //Failure
                {
                    nCosemIndex = nCosemIndex + 1;
                    if (Buffer[nCosemIndex] == 0x03) return 0x03;    // Access Denied                       
                    else if (Buffer[nCosemIndex] == 0x0E) return 0x0E;    //Data block unavailable                       
                    else return 0x02;    // Access Denied                       
                }
                return 0x01; ///Get.Request.normal (Success)
                
        }
       /// <summary>
        /// add PDU Size to Buffer
       /// </summary>
       /// <param name="Buffer"></param>
       /// <returns></returns>
        public int fCheckCOSEMResponseForReset(byte[] Buffer)
        {
            int nCosemIndex = 14;
            if (Buffer[nCosemIndex] != 0xC7)   //Get.response
            {
                if (Buffer[nCosemIndex] == 0x0E)   return 0x02;//Fail //Get.response 
                return 0x03;//Fail
            }
            else
            {
                nCosemIndex = nCosemIndex + 3;
                if (Buffer[nCosemIndex] == 0x01) return 0x02;    /// Access Denied   //Get.response.Next
                return 0x01; ///Get.Request.normal
                 
            }
      
        }
      /// <summary>
        /// Create Block Tranfsfer packet
      /// </summary>
      /// <param name="Buffer"></param>
      /// <param name="nBufferIndex"></param>
      /// <returns></returns>
        public int fGetBlockTransferPacket(byte[] Buffer, int nBufferIndex)
        {
            //7EA01402232154 7E15 E6E600 C002C10000000151BE7E

            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x02;      //need to change
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;

            //Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = Convert.ToByte(nBlockNumber);

            Buffer[nBufferIndex++] = Convert.ToByte(nBlockNumber >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(nBlockNumber & 0xFF);
            return nBufferIndex;
        }
        /// <summary>
        /// Create Block Tranfsfer packet
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int fGetQueryBillingProfile(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x62;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="Buffer"></param>
       /// <param name="nBufferIndex"></param>
       /// <param name="atb"></param>
       /// <returns></returns>
        public int fGetQueryLoadSurveyProfile(byte[] Buffer, int nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x63;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <param name="compartment"></param>
        /// <returns></returns>
        public int fGetQueryTamperProfile(byte[] Buffer, int nBufferIndex, byte atb, byte compartment)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 63 62 compartment(00 to 05) ff
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x63;
            Buffer[nBufferIndex++] = 0x62;
            Buffer[nBufferIndex++] = compartment;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
       /// <summary>
        /// Create Block Tranfsfer packet
       /// </summary>
       /// <param name="Buffer"></param>
       /// <param name="nBufferIndex"></param>
       /// <param name="fromEntry"></param>
       /// <param name="toEntry"></param>
       /// <returns></returns>
        public int fGetSelectiveAccessByEntry(byte[] Buffer, int nBufferIndex,byte fromEntry,byte toEntry)
        {
            //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
            nBufferIndex--;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x02;      //need to change//
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x06;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = Convert.ToByte(fromEntry);
            Buffer[nBufferIndex++] = 0x06;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = Convert.ToByte(toEntry);
            Buffer[nBufferIndex++] = 0x12;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x12;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        public int FillCommandData(byte[] Buffer, int nBufferIndex, List<byte> DataValue)
        {
            int datavalCnt = 0;
            while (datavalCnt < DataValue.Count)
            {
                Buffer[nBufferIndex++] = DataValue[datavalCnt++];
            }
            return nBufferIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public int fGetSelectiveAccessByEntry(byte[] Buffer, int nBufferIndex, DateTime fromDate, DateTime toDate)
        {
            //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
            nBufferIndex--;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x02;      
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x02;      
            Buffer[nBufferIndex++] = 0x04;

            Buffer[nBufferIndex++] = 0x12;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x08;

            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x06;
            
            Buffer[nBufferIndex++] = 0x00; //obis code
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;

            Buffer[nBufferIndex++] = 0x0F;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x12;

            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;

            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x0C;

            Buffer[nBufferIndex++] = Convert.ToByte((fromDate.Year / 100) % 20); //year
            Buffer[nBufferIndex++] = Convert.ToByte( fromDate.Year % 100);

            Buffer[nBufferIndex++] = Convert.ToByte(fromDate.Month); //month

            Buffer[nBufferIndex++] = Convert.ToByte(fromDate.Day);
            Buffer[nBufferIndex++] = 0xFF;

            Buffer[nBufferIndex++] = Convert.ToByte(fromDate.Hour);
            Buffer[nBufferIndex++] = Convert.ToByte(fromDate.Minute);
            Buffer[nBufferIndex++] = Convert.ToByte(fromDate.Second); 

            Buffer[nBufferIndex++] = 0xFF;

            Buffer[nBufferIndex++] = 0x80;
            Buffer[nBufferIndex++] = 0x00;

            Buffer[nBufferIndex++] = 0x00;
            
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x0C;

            Buffer[nBufferIndex++] = Convert.ToByte((toDate.Year / 100) % 20); //year
            Buffer[nBufferIndex++] = Convert.ToByte(toDate.Year % 100);

            Buffer[nBufferIndex++] = Convert.ToByte(toDate.Month); //month

            Buffer[nBufferIndex++] = Convert.ToByte(toDate.Day);
            Buffer[nBufferIndex++] = 0xFF;

            Buffer[nBufferIndex++] = Convert.ToByte(toDate.Hour);
            Buffer[nBufferIndex++] = Convert.ToByte(toDate.Minute);
            Buffer[nBufferIndex++] = Convert.ToByte(toDate.Second); 

            Buffer[nBufferIndex++] = 0xFF;

            Buffer[nBufferIndex++] = 0x80;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;

            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
           
            return nBufferIndex;
        }

        public int fActionNormalImgBlockTransferPacket(byte[] Buffer, int nBufferIndex, byte[] nDataArray, List<byte> imgfooter)
        {
            nTotalPacketSize = nDataArray.Count();

            for (int i = 0; i < nDataArray.Count(); i++)
            {
                Buffer[nBufferIndex++] = nDataArray[i];
            }
            return nBufferIndex;
        }


    }
}
