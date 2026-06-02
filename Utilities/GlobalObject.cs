
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialCommunication;
using System.Windows.Forms;
using DLMSLIB;
using System.Runtime.InteropServices;
using ManagedMath;
using SystemSecurityLibrary;
using System.IO;

namespace Utilities
{
     

//static public extern void DisposeClassName(IntPtr pClassNameObject);
 

    public partial class GlobalObjects
    {
       public  static SerialComm objSerialComm = new SerialComm();
        public static IECSerialComm objIECSerialComm = new IECSerialComm();
        public static IECMeterSerialCommunication objIECMeterSerialComm = new IECMeterSerialCommunication();
        public static HDLCLIB objHDLCLIB = new HDLCLIB();
        public static COSEMLIB objCOSEMLIB = new COSEMLIB();
        public static GlobalFunctions objGlobalFunctions = new GlobalFunctions();
       
    
 
         
    }

    public class GlobalFunctions
    {
        public int[] ucFcs = new int[2];
        const int FIXLENGTH = 16;
        string randamPlanText = string.Empty;
        string HLSpublicPwd = string.Empty;
        byte[] HDLCCommand = new byte[200];
        int HDLCIndex = 0;
        byte Encryptionmethod = 5;
        byte[] EncryKey = new byte[16];
        Class1 ManageObj = new Class1();
        byte[] plainText = new byte[31];
        byte[] cyphertext = new byte[30];
        byte[] InitVector = new byte[12];
        byte[] AuthenticationTag = new byte[12];
        uint AuthTagLen = 12;
        byte ChannelNum = 0;
        byte[] AAD = new byte[17];
        byte[] DecyplainText;
        byte[] CypherDataDecypt;
          byte[] AADDecypt;
          byte[] InitVectorDecypt;
          byte[] ClientInitVector = new byte[12];
          byte[] AuthTagDecypt;
          byte[] serverSystemTitle = new byte[12];
          string clientSystemTitle;
          byte[] UserInfo = new byte[22];
          int UserInfoCounter = 0;
          byte[] ConfBlock = new byte[7];
          int ConfBlockCounter = 0;
          byte[] PDUSize = new byte[2];
          int PDUSizeCounter = 0;
          byte clientSecuritymechanism = 0x00;
         
        /// <summary>
        /// Send SNRM packet and Recieve and Check UA response
       /// </summary>
       /// <param name="nServerSAP"></param>
       /// <param name="nServerLowerMacAddress"></param>
       /// <param name="nClientSAP"></param>
       /// <returns></returns>
        //public bool fSendSNRM(int nServerSAP, int nServerLowerMacAddress, int nClientSAP)
        //{
        //    try
        //    { 
        //        HDLCIndex = 0;
        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, nServerSAP, nServerLowerMacAddress);
        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, nClientSAP);
        //        GlobalObjects.objHDLCLIB.fSetSNRM();
        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
        //        GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
        //        GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
        //        GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
        //        if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex))return false;                 
        //        GlobalObjects.objHDLCLIB.fSetUA();//Setting Response Command type
        //        if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer, nClientSAP))return false;
        //        return true;                    
        //         //objWrapper.m_securityAESUnwrap(
        //    }
        //    catch (Exception )
        //    {
        //        return false;
        //    }
        //}
          public bool fSendSNRM(int nServerSAP, int nServerLowerMacAddress, int nClientSAP, int maxInfoFieldLenTransmit, int maxInfoFieldLenReceive, int windowSizeTransmit, int windowSizeReceive)
          {
              try
              {
                  HDLCIndex = 0;
                  HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                  HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                  HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, nServerSAP, nServerLowerMacAddress);
                  HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, nClientSAP);
                  GlobalObjects.objHDLCLIB.fSetSNRM();
                  HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                  HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex); //Add HCS blank bytes
                  HDLCIndex = GlobalObjects.objHDLCLIB.fAddInfoFieldNegotiationBytes(HDLCCommand, HDLCIndex, maxInfoFieldLenTransmit, maxInfoFieldLenReceive, windowSizeTransmit, windowSizeReceive);
                  HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);//Add FCS blank bytes
                  GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                  GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8); //Generate HCS, Same method fGenerateFCS will be used
                  GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10); //Add calculated HCS bytes
                  GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 33); //Generate FCS
                  // GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                  GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 34, 35);//Add calculated FCS bytes
                  HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                  if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex)) return false;
                  GlobalObjects.objHDLCLIB.fSetUA();//Setting Response Command type
                 
                if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer, nClientSAP))
                 {
                    return false;
                 }
                 else 
                 {
                    
                    return true;
                 }
                  //objWrapper.m_securityAESUnwrap(
              }
              catch (Exception)
              {
                  return false;
              }
          }
        /// <fCheckHDLCResponse>
        ///  Check Start/end tag, Check FCS , Check destination Address and Check command Byte
       /// </summary>
       /// <param name="Buffer"></param>
       /// <param name="nClientSAP"></param>
       /// <returns></returns>
        private bool fCheckHDLCResponse(byte[] Buffer,int nClientSAP)
        {
            
            if (!GlobalObjects.objHDLCLIB.fCheckStartEndTag(Buffer))return false;
            if (!GlobalObjects.objHDLCLIB.fCheckFCS(Buffer))return false;
            if (!GlobalObjects.objHDLCLIB.fCheckServerSAP(Buffer, nClientSAP)) return false;
            if (!GlobalObjects.objHDLCLIB.fCheckCommand(Buffer, GlobalObjects.objHDLCLIB.nCMDByte))return false;            
            return true;                 
        }

        /// <fSendAARQ>
        ///  Send AARQ packet and Recieve and Check AARE response
       /// </summary>
       /// <param name="nServerSAP"></param>
       /// <param name="nServerLowerMacAddress"></param>
       /// <param name="nClientSAP"></param>
       /// <param name="nSecurityMechanism"></param>
       /// <param name="nPassword"></param>
       /// <param name="HLSKey"></param>
       /// <returns></returns>
        public bool fSendAARQ(int nServerSAP, int nServerLowerMacAddress, int nClientSAP, byte nSecurityMechanism, string nPassword, string HLSKey, string HLSPwd, string conformanceBlock, int nPDUSize,byte applicationcontext )
        {
            try
            {
               
                HLSpublicPwd = HLSPwd;
                byte[] cnfBlock = new byte[3];
                cnfBlock[0] = (byte)Convert.ToInt16((conformanceBlock.Substring(0, 2)), 16);// 0x00;
                cnfBlock[1] = (byte)Convert.ToInt16((conformanceBlock.Substring(2, 2)), 16);//0x12;
                cnfBlock[2] = (byte)Convert.ToInt16((conformanceBlock.Substring(4, 2)), 16);//0x1A;
                //Change Needed
                //7EA02E0002002321107ECBE6E600601DA109060760857405080101BE10040E01000000065F1F040000121AFFFFF4FF7E
                //7EA047000200234110974BE6E6006036A1090607608574050801018A0207808B0760857405080201AC0A80083132333435363738BE10040E01000000065F1F040000121AFFFFEEE07E

                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, nServerSAP, nServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, nClientSAP);
                GlobalObjects.objHDLCLIB.fSetInitialI();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
                if (nSecurityMechanism == 0x00)
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddAARQTAG(HDLCCommand, HDLCIndex, 0x1D);
                else if (nSecurityMechanism == 0x01)
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddAARQTAG(HDLCCommand, HDLCIndex, 0x36);
                else
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddAARQTAG(HDLCCommand, HDLCIndex, 0x3E);
                //byte nApplicationContext = 0x01;
                //HDLCIndex = GlobalObjects.objCOSEMLIB.fAddContext(HDLCCommand, HDLCIndex, nApplicationContext);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddContext(HDLCCommand, HDLCIndex, applicationcontext);
                if (nSecurityMechanism == 0x01)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddSecMechanism(HDLCCommand, HDLCIndex, nSecurityMechanism);
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddPassword(HDLCCommand, HDLCIndex, nPassword);
                }
                else if (nSecurityMechanism == 0x02)
                {
                    randamPlanText = "1111111111111111";//Generate16ByteRandomHLSState();//
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddSecMechanism(HDLCCommand, HDLCIndex, nSecurityMechanism);
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddRandomKey(HDLCCommand, HDLCIndex, randamPlanText);
                }
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddUserInf(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddCnfBlock(HDLCCommand, HDLCIndex, cnfBlock);
                
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddPDUSize(HDLCCommand, HDLCIndex, nPDUSize);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex)) return false;
                GlobalObjects.objHDLCLIB.fIncRecieve(); 
                if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer, nClientSAP))return false;
                if (!GlobalObjects.objCOSEMLIB.fCheckAARQResponse(GlobalObjects.objSerialComm.ReceiveBuffer)) return false;
                if (nSecurityMechanism != 0x02) return true;
                if (!fSendRLRQ(nServerSAP, nServerLowerMacAddress, nClientSAP, HLSKey)) return false;
                return true;       
                
            }
            catch (Exception)
            {
                return false;
            }
        }

     
        public bool fSendAARQ_Cyphered(int nServerSAP, int nServerLowerMacAddress, int nClientSAP, byte nSecurityMechanism, string nPassword, string HLSKey, string HLSPwd, string conformanceBlock, int nPDUSize, string sYstemTitle, int Securitysuit,string GlobalEncyptKey,string AuthenticationKey,int DedicatedInfo)
        {
            try
            {
                GlobalObjects.objHDLCLIB.SecuritysuitByte = Securitysuit;
                clientSecuritymechanism = nSecurityMechanism;
                clientSystemTitle = sYstemTitle;
                HLSpublicPwd = HLSPwd;
                byte[] cnfBlock = new byte[3];
                cnfBlock[0] = (byte)Convert.ToInt16((conformanceBlock.Substring(0, 2)), 16);// 0x00;
                cnfBlock[1] = (byte)Convert.ToInt16((conformanceBlock.Substring(2, 2)), 16);//0x12;
                cnfBlock[2] = (byte)Convert.ToInt16((conformanceBlock.Substring(4, 2)), 16);//0x1A;
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, nServerSAP, nServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, nClientSAP);
                GlobalObjects.objHDLCLIB.fSetInitialI();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
                if (clientSecuritymechanism == 0x00)
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddAARQTAG(HDLCCommand, HDLCIndex, 0x1D);
                else if (clientSecuritymechanism == 0x01)
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddAARQTAG(HDLCCommand, HDLCIndex, 0x36);
                else
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddAARQTAG(HDLCCommand, HDLCIndex, 0x3E);
                byte nApplicationContext = 0x03;//Allen refrencing with cyphering byte for smart meter
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddContext_Cyphered(HDLCCommand, HDLCIndex, nApplicationContext);// Cyphered byte data
                HDLCIndex = GlobalObjects.objCOSEMLIB.fBeforeSysTitle(HDLCCommand, HDLCIndex, sYstemTitle);//A60A0408
                HDLCIndex = GlobalObjects.objCOSEMLIB.fSystemTitle(HDLCCommand, HDLCIndex, sYstemTitle);//System title byte
                if (clientSecuritymechanism == 0x01)//MR mode
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddSecMechanism(HDLCCommand, HDLCIndex, clientSecuritymechanism);
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddPassword(HDLCCommand, HDLCIndex, nPassword);
                }
                else if (clientSecuritymechanism == 0x02)//US mode
                {
                    randamPlanText = "1111111111111111";//Generate16ByteRandomHLSState();
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddSecMechanism(HDLCCommand, HDLCIndex, clientSecuritymechanism);
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddRandomKey(HDLCCommand, HDLCIndex, randamPlanText);//Random byte 16
                }

                HDLCIndex = GlobalObjects.objCOSEMLIB.SecuritySuitByte(HDLCCommand, HDLCIndex, Securitysuit, DedicatedInfo);//Security Suit byte
                HDLCIndex = GlobalObjects.objCOSEMLIB.fInvocationCounter(HDLCCommand, HDLCIndex, GlobalObjects.objHDLCLIB.InitializationCounter);//Invocation counter

                GlobalObjects.objCOSEMLIB.fAddUserInf_cypher(UserInfo, UserInfoCounter, DedicatedInfo);// User Information 5 byte
                GlobalObjects.objCOSEMLIB.fAddCnfBlock_Cyphered(ConfBlock, ConfBlockCounter, cnfBlock);//Confirmation Block
                GlobalObjects.objCOSEMLIB.fAddPDUSize_Cyphered(PDUSize, PDUSizeCounter, nPDUSize);//PDU Size
         //********************************* Create AES GCM Encryption ***************************************
                int countlen = 0;
                int EncCount = 0;
                int Aadcount = 0;
                AAD[0] = (byte)Securitysuit;
                while (countlen < GlobalEncyptKey.Length)
                {
                    EncryKey[EncCount++] = Convert.ToByte(GlobalEncyptKey.Substring(countlen, 2), 16);
                   // AAD[aadcount + 1] = Convert.ToByte(GlobalEncyptKey.Substring(countlen, 2), 16); ;
                    countlen += 2;
                  //  aadcount++;
                }
                countlen = 0;
                while (countlen < AuthenticationKey.Length)
                {
                    AAD[Aadcount + 1] = Convert.ToByte(AuthenticationKey.Substring(countlen, 2), 16); ;
                    countlen += 2;
                    Aadcount++;
                }
        
               var user_data = UserInfo.TakeWhile((v, index) => UserInfo.Skip(index).Any(w => w != 0x00)).ToArray();
               UserInfo = user_data;//Skip null values at MR mode
                System.Buffer.BlockCopy(UserInfo, 0, plainText, 0, UserInfo.Length);//Plain Text
                System.Buffer.BlockCopy(ConfBlock, 0, plainText, UserInfo.Length, ConfBlock.Length);
                System.Buffer.BlockCopy(PDUSize, 0, plainText, UserInfo.Length + ConfBlock.Length, PDUSize.Length);

                UserInfo = new byte[22];
            var Plain_data = plainText.TakeWhile((v, index) => plainText.Skip(index).Any(w => w != 0x00)).ToArray();
                plainText = Plain_data;//Skip null values at MR mode

                IntializationVector(sYstemTitle);;//Init Vector=System title + invocation counter
                
                cyphertext = new byte[plainText.Length];
                AuthenticationTag = new byte[AuthTagLen];

                if (clientSecuritymechanism == 0x01 && DedicatedInfo == 0x00)//MR mode Encryption Only
               {
                   AAD = new byte[17];
                   AuthenticationTag= new byte[12];
                   ManageObj.p_securityLibEncrypt(Encryptionmethod, EncryKey, (ushort)EncryKey.Length, plainText, (uint)plainText.Length, ref cyphertext, ClientInitVector, 12, null, 0, ref AuthenticationTag, 0, ChannelNum);
                   HDLCIndex = GlobalObjects.objCOSEMLIB.fAddCyphered_Tag(HDLCCommand, HDLCIndex, cyphertext);// Add Cypher Tag                   
                    plainText = new byte[31];
                }
                else if (clientSecuritymechanism == 0x02 || DedicatedInfo == 0x01)//US mode Encryption + Authentication only
                {
                   ManageObj.p_securityLibEncrypt(Encryptionmethod, EncryKey,(ushort)EncryKey.Length, plainText, (uint)plainText.Length, ref cyphertext, ClientInitVector, 12, AAD, 17, ref AuthenticationTag, AuthTagLen, ChannelNum);
                   HDLCIndex = GlobalObjects.objCOSEMLIB.fAddCyphered_Tag(HDLCCommand, HDLCIndex, cyphertext);// Add Cypher Tag
                   HDLCIndex = GlobalObjects.objCOSEMLIB.fAddAuthentication_Tag(HDLCCommand, HDLCIndex, AuthenticationTag);
                   plainText = new byte[31];
                }
              
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillAARQBufferLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex)) return false;
                GlobalObjects.objHDLCLIB.fIncRecieve();
                if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer, nClientSAP)) return false;
               
                if (!GlobalObjects.objCOSEMLIB.fCheckAARQResponse(GlobalObjects.objSerialComm.ReceiveBuffer)) return false;
                    
               //*********************************Call AES GCM Decryption ***************************************
                InitVectorDecypt = new byte[12];
                int InitIndex = 0;
                int buffIndex = 43;
                while (InitIndex < InitVectorDecypt.Length-4)
                {
                    InitVectorDecypt[InitIndex] = GlobalObjects.objSerialComm.ReceiveBuffer[buffIndex + InitIndex];
                    InitIndex++;
                }
               byte[] plaintextResponse = GetPlainTextFromCipheredTest(91);// GCM Decryption
               if (clientSecuritymechanism != 0x02) return true;
               if (!fSendRLRQ_Cyphered(nServerSAP, nServerLowerMacAddress, nClientSAP, HLSKey))
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //********************************* Create AES GCM Encryption start***************************************
       public int CreateCipherCommand(byte[] cmdplaintext, byte[] Buffer, int nBufferIndex)
        {
               
            IntializationVector(clientSystemTitle);
            cyphertext = new byte[cmdplaintext.Length];
            AuthenticationTag = new byte[AuthTagLen];
            if (clientSecuritymechanism == 0x01 && GlobalObjects.objCOSEMLIB.DedKeystr == "") //---MR Mode Encryption
            {
                ManageObj.p_securityLibEncrypt(Encryptionmethod, EncryKey, 16, cmdplaintext, (uint)cmdplaintext.Length, ref cyphertext, ClientInitVector, 12, null, 0, ref AuthenticationTag, 0, ChannelNum);
            }
            else if (clientSecuritymechanism == 0x02 || GlobalObjects.objCOSEMLIB.DedKeystr != "") //--US Mode Encryption with Dedicated key
            {
                if (GlobalObjects.objCOSEMLIB.DedKeystr != "")
                {
                    //***************Using Dedicated key in place of Encryption key*********
                    int IndexLen = 0;
                    int Enckcount = 0;
                    while (IndexLen < GlobalObjects.objCOSEMLIB.DedKeystr.Length)
                    {
                        // EncryKey = GlobalObjects.objCOSEMLIB.DedicatedKey;
                        EncryKey[Enckcount++] = Convert.ToByte(GlobalObjects.objCOSEMLIB.DedKeystr.Substring(IndexLen, 2), 16);
                        IndexLen += 2;
                    }
                }
               
                ManageObj.p_securityLibEncrypt(Encryptionmethod, EncryKey, 16, cmdplaintext, (uint)cmdplaintext.Length, ref cyphertext, ClientInitVector, 12, AAD, 17, ref AuthenticationTag, AuthTagLen, ChannelNum);
            }

            byte[] cypherData = cyphertext;
            byte[] AuthenData = AuthenticationTag;

            nBufferIndex = GlobalObjects.objCOSEMLIB.fAddCyphered_Tag(Buffer, nBufferIndex, cypherData);// Add Cypher Tag
            if (clientSecuritymechanism == 0x02) nBufferIndex = GlobalObjects.objCOSEMLIB.fAddAuthentication_Tag(Buffer, nBufferIndex, AuthenData);//Add Authentication Tag
            return nBufferIndex;
        }
       //********************************* Create AES GCM Encryption End***************************************


       //********************************* Create AES GCM Decryption start***************************************
       public byte[] GetPlainTextFromCipheredTest(int InitIndex)
            {
            int noofReceivedBytes = GlobalObjects.objSerialComm.ReceiveBuffer[15] - (5); //5--> tag 30 0ne byte & intcounter 4 byte
            int cipherDataStartIndex = 21;
            if (GlobalObjects.objSerialComm.ReceiveBuffer[15] == 0x81)
            {
                noofReceivedBytes = GlobalObjects.objSerialComm.ReceiveBuffer[16] - (5);
                cipherDataStartIndex++; 
            }
            else if (GlobalObjects.objSerialComm.ReceiveBuffer[15] == 0x82)
            {
                 noofReceivedBytes = ((byte)(GlobalObjects.objSerialComm.ReceiveBuffer[16] & 0x1F) * 0x100 + (byte)(GlobalObjects.objSerialComm.ReceiveBuffer[17]));  
                noofReceivedBytes = noofReceivedBytes - (5);
                cipherDataStartIndex+=2;
            }
                
            if (clientSecuritymechanism == 0x01)
            {
                DecyplainText = new byte[noofReceivedBytes];
                CypherDataDecypt = new byte[noofReceivedBytes];
            }
            else if (clientSecuritymechanism == 0x02)
            {
                DecyplainText = new byte[noofReceivedBytes - 12];
                CypherDataDecypt = new byte[noofReceivedBytes - 12];
            }
            System.Buffer.BlockCopy(GlobalObjects.objSerialComm.ReceiveBuffer, cipherDataStartIndex, CypherDataDecypt, 0, CypherDataDecypt.Length);

            AADDecypt = new byte[AAD.Length + CypherDataDecypt.Length];
            System.Buffer.BlockCopy(AAD, 0, AADDecypt, 0, AAD.Length);
            System.Buffer.BlockCopy(CypherDataDecypt, 0, AADDecypt, AAD.Length, CypherDataDecypt.Length);

            AuthTagDecypt = new byte[12];
            if (clientSecuritymechanism == 0x02)
            {
                System.Buffer.BlockCopy(GlobalObjects.objSerialComm.ReceiveBuffer, cipherDataStartIndex + CypherDataDecypt.Length, AuthTagDecypt, 0, 12);
            }
             long InvoDecrypt = DLMSDataStracture.FormatData(GlobalObjects.objSerialComm.ReceiveBuffer, InitIndex, 4, false);
             Array.Copy(GlobalObjects.objSerialComm.ReceiveBuffer, InitIndex, InitVectorDecypt, 8, 4);
             if (clientSecuritymechanism == 0x01)
            {
                ManageObj.p_securityLibDecrypt(Encryptionmethod, EncryKey, 16, ref DecyplainText, (ushort)DecyplainText.Length, CypherDataDecypt, InitVectorDecypt, 12, null, 0, ref AuthTagDecypt, 0, ChannelNum);
            }
            else if (clientSecuritymechanism == 0x02)
            {
                uint AADLength = Convert.ToUInt16(AAD.Length + CypherDataDecypt.Length);
                ManageObj.p_securityLibDecrypt(Encryptionmethod, EncryKey, 16, ref DecyplainText, (ushort)DecyplainText.Length, CypherDataDecypt, InitVectorDecypt, 12, AADDecypt, AADLength, ref AuthTagDecypt, AuthTagLen, ChannelNum);
            }
            return DecyplainText;
        }
            //********************************* Create AES GCM Decryption End***************************************
        

        public void IntializationVector(string systitle)
        {
            long clientinovationCount = GlobalObjects.objHDLCLIB.InitializationCounter;
            ClientInitVector[0] = Convert.ToByte(systitle[0]);
            ClientInitVector[1] = Convert.ToByte(systitle[1]);
            ClientInitVector[2] = Convert.ToByte(systitle[2]);
            ClientInitVector[3] = Convert.ToByte(systitle[3]);
            ClientInitVector[4] = Convert.ToByte(systitle[4]);
            ClientInitVector[5] = Convert.ToByte(systitle[5]);
            ClientInitVector[6] = Convert.ToByte(systitle[6]);
            ClientInitVector[7] = Convert.ToByte(systitle[7]);
            ClientInitVector[8] = Convert.ToByte((clientinovationCount & 0xFF000000) >> 24);
            ClientInitVector[9] = Convert.ToByte((clientinovationCount & 0xFF0000) >> 16);
            ClientInitVector[10] = Convert.ToByte((clientinovationCount & 0xFF00) >> 8);
            ClientInitVector[11] = Convert.ToByte(clientinovationCount & 0x00FF);
            
        }
       
        public string Generate16ByteRandomHLSState()
        {
            Random random = new Random();
            string randomString = "";
            int digitCount;
            for (digitCount = 1; digitCount <= 16; digitCount++)
            {
                randomString += random.Next(0, 9).ToString();
            }
            return randomString;
        } 

        /// <fSendRLRQ>
        /// This is Optional Function and is only Call in case of High level Sequrity Mechanism.
        /// </summary>
        /// <param name="nServerSAP"></param>
        /// <param name="nServerLowerMacAddress"></param>
        /// <param name="nClientSAP"></param>
        /// <param name="HLSKey"></param>
        /// <returns></returns>
        public bool fSendRLRQ(int nServerSAP, int nServerLowerMacAddress, int nClientSAP, string HLSKey)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, nServerSAP, nServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, nClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
                //C3 01 C1 00 0F 00 00 28 00 03 FF 01 00 09 10
                HDLCCommand[HDLCIndex++] = 0xC3;    //Get Action
                HDLCCommand[HDLCIndex++] = 0x01;    //Normal Block 
                HDLCCommand[HDLCIndex++] = 0xC1;    //Invoke ID
                HDLCCommand[HDLCIndex++] = 0x00;    //Class ID 1st Byte
                HDLCCommand[HDLCIndex++] = 0x0F;    //Class ID 2nd byte
                HDLCCommand[HDLCIndex++] = 0x00;    //OBIS Code 1
                HDLCCommand[HDLCIndex++] = 0x00;    //OBIS Code 2
                HDLCCommand[HDLCIndex++] = 0x28;    //OBIS Code 3
                HDLCCommand[HDLCIndex++] = 0x00;    //OBIS Code 4
                HDLCCommand[HDLCIndex++] = 0x00;//---For Current association. 0x03;    //OBIS Code 5
                HDLCCommand[HDLCIndex++] = 0xFF;    //OBIS Code 6
                HDLCCommand[HDLCIndex++] = 0x01;    //Attribute
                HDLCCommand[HDLCIndex++] = 0x01;    //Data Index //-----set to 1 for current association to support FS with HLS 
                HDLCCommand[HDLCIndex++] = 0x09;    //oct String
                HDLCCommand[HDLCIndex++] = 0x10;    //Len is 16 bytep
                //------------------------Generate Cipher Text From Server Seed--------------------------------------
                AESEncryption objaes = new AESEncryption();
                string ServerSeed = GetServerSeed(56);
                string ClientCipherText=objaes.GenerateCipherText(ServerSeed, HLSpublicPwd);
                //---------------------------------------------------------------------------------------------------
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddEncryptedKey(HDLCCommand, HDLCIndex, ClientCipherText);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex)) return false;                
                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer, nClientSAP)) return false;
                if (!IsValidSerevrHLS()) return false;             
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Verifying Serever HLS Seed with AES
        /// </summary>
        /// <returns></returns>
        public bool fSendRLRQ_Cyphered(int nServerSAP, int nServerLowerMacAddress, int nClientSAP, string HLSKey)
        {
            try
            {
                HDLCIndex = 0;
                HDLCCommand = new byte[200];
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, nServerSAP, nServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, nClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
                HDLCCommand[HDLCIndex++] = 0xCB;
                HDLCCommand[HDLCIndex++] = 0x30;
                HDLCCommand[HDLCIndex++] = 0x30;
                //HDLCCommand[HDLCIndex++] = 0x00;
                //HDLCCommand[HDLCIndex++] = 0x00;
                //HDLCCommand[HDLCIndex++] = 0x00;
                //HDLCCommand[HDLCIndex++] = 0x01;
                HDLCIndex = GlobalObjects.objCOSEMLIB.fInvocationCounter(HDLCCommand, HDLCIndex, GlobalObjects.objHDLCLIB.InitializationCounter);
                //C3 01 C1 00 0F 00 00 28 00 03 FF 01 00 09 10
                //HDLCCommand[HDLCIndex++] = 0xC3;    //Get Action
                //HDLCCommand[HDLCIndex++] = 0x01;    //Normal Block 
                //HDLCCommand[HDLCIndex++] = 0xC1;    //Invoke ID
                //HDLCCommand[HDLCIndex++] = 0x00;    //Class ID 1st Byte
                //HDLCCommand[HDLCIndex++] = 0x0F;    //Class ID 2nd byte
                //HDLCCommand[HDLCIndex++] = 0x00;    //OBIS Code 1
                //HDLCCommand[HDLCIndex++] = 0x00;    //OBIS Code 2
                //HDLCCommand[HDLCIndex++] = 0x28;    //OBIS Code 3
                //HDLCCommand[HDLCIndex++] = 0x00;    //OBIS Code 4
                //HDLCCommand[HDLCIndex++] = 0x00;//---For Current association. 0x03;    //OBIS Code 5
                //HDLCCommand[HDLCIndex++] = 0xFF;    //OBIS Code 6
                //HDLCCommand[HDLCIndex++] = 0x01;    //Attribute
                //HDLCCommand[HDLCIndex++] = 0x01;    //Data Index //-----set to 1 for current association to support FS with HLS 
                //HDLCCommand[HDLCIndex++] = 0x09;    //oct String
                //HDLCCommand[HDLCIndex++] = 0x10;    //Len is 16 bytep
                //------------------------Generate Cipher Text From Server Seed--------------------------------------
                AESEncryption objaes = new AESEncryption();
                string ServerSeed = GetServerSeed(68);
                string ClientCipherText = objaes.GenerateCipherText(ServerSeed, HLSpublicPwd);
                //---------------------------------------------------------------------------------------------------
                // HDLCIndex = GlobalObjects.objCOSEMLIB.fAddEncryptedKey(HDLCCommand, HDLCIndex, ClientCipherText);
                byte[] RLRQData = new byte[16];
              //  RLRQData = Encoding.ASCII.GetBytes(ClientCipherText);

                int aadcount = 0;
                int countlen1 = 0;
                while (countlen1 < ClientCipherText.Length)
                {
                    RLRQData[aadcount++] = Convert.ToByte(ClientCipherText.Substring(countlen1, 2), 16);
                    countlen1 += 2;
                }
                string HDLCstr = "C301C1000F0000280000FF01010910";
                byte[] HDLCdata = new byte[15];
                //HDLCdata = Encoding.ASCII.GetBytes(HDLCstr);
                //System.Buffer.BlockCopy(UserInfo, 0, plainText, 0, UserInfo.Length);//Plain Text
                 aadcount = 0;
                 countlen1 = 0;
                while (countlen1 < HDLCstr.Length)
                {
                    HDLCdata[aadcount++] = Convert.ToByte(HDLCstr.Substring(countlen1, 2), 16);
                    countlen1 += 2;
                }

                byte[] RLRQplainText = new byte[31];
                System.Buffer.BlockCopy(HDLCdata, 0, RLRQplainText, 0, HDLCdata.Length);
                System.Buffer.BlockCopy(RLRQData, 0, RLRQplainText, HDLCdata.Length, RLRQData.Length);

                cyphertext = new byte[31];
                AuthenticationTag = new byte[12];
                IntializationVector(clientSystemTitle);
                ManageObj.p_securityLibEncrypt(Encryptionmethod, EncryKey, 16, RLRQplainText, 31, ref cyphertext, ClientInitVector, 12, AAD, 17, ref AuthenticationTag, AuthTagLen, ChannelNum);

                System.Buffer.BlockCopy(cyphertext, 0, HDLCCommand, HDLCIndex, cyphertext.Length);
                HDLCIndex += 31;
                System.Buffer.BlockCopy(AuthenticationTag, 0, HDLCCommand, HDLCIndex, AuthenticationTag.Length);
                HDLCIndex += 12;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex)) return false;
                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer, nClientSAP)) return false;
                CypherDataDecypt = new byte[24];
                System.Buffer.BlockCopy(GlobalObjects.objSerialComm.ReceiveBuffer, 21, CypherDataDecypt, 0, 24);
                cyphertext = new byte[31];
                AuthTagDecypt = new byte[12];
                System.Buffer.BlockCopy(GlobalObjects.objSerialComm.ReceiveBuffer, 45, AuthTagDecypt, 0, 12);
                DecyplainText = new byte[24];
                long InvoDecrypt = DLMSDataStracture.FormatData(GlobalObjects.objSerialComm.ReceiveBuffer, 17, 4, false);
                Array.Copy(GlobalObjects.objSerialComm.ReceiveBuffer, 17, InitVectorDecypt, 8, 4);
                ManageObj.p_securityLibDecrypt(Encryptionmethod, EncryKey, 16, ref DecyplainText, 24, CypherDataDecypt, InitVectorDecypt, 12, AADDecypt, 31, ref AuthTagDecypt, AuthTagLen, ChannelNum);

                if (!CipherIsValidSerevrHLS(DecyplainText)) return false;
                return true;  
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool IsValidSerevrHLS()
        {
            try
            {
                int StarByteCnt = 21;
                AESEncryption objaes = new AESEncryption();
                string ReceivedSeed = string.Empty;
                if (GlobalObjects.objSerialComm.ReceiveBuffer[19] == 0x00) StarByteCnt++; //--as per Greenbook & Kali stack this is the Data bits, same was missing with TI Stack.
                int icnt = StarByteCnt;
                while (icnt < StarByteCnt + 16)
                {
                    string tempcipher = GlobalObjects.objSerialComm.ReceiveBuffer[icnt++].ToString("X");
                    while (tempcipher.Length < 2) tempcipher = "0" + tempcipher;
                    ReceivedSeed += tempcipher;
                }
               // ReceivedSeed="232D0CC194881C4291528EEA93A551A0";
                string plaintext = objaes.GeneratePlainText(ReceivedSeed, HLSpublicPwd);
                if (plaintext == randamPlanText) return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool CipherIsValidSerevrHLS(byte[] cipheredvalue)
        {
            try
            {
                int StarByteCnt = 8;
                AESEncryption objaes = new AESEncryption();
                string ReceivedSeed = string.Empty;
                //if (GlobalObjects.objSerialComm.ReceiveBuffer[19] == 0x00) StarByteCnt++; //--as per Greenbook & Kali stack this is the Data bits, same was missing with TI Stack.
                int icnt = StarByteCnt;
                while (icnt < StarByteCnt + 16)
                {
                    string tempcipher = cipheredvalue[icnt++].ToString("X");
                    while (tempcipher.Length < 2) tempcipher = "0" + tempcipher;
                    ReceivedSeed += tempcipher;
                }
                // ReceivedSeed="232D0CC194881C4291528EEA93A551A0";
                string plaintext = objaes.GeneratePlainText(ReceivedSeed, HLSpublicPwd);
                if (plaintext == randamPlanText) return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Extracting Server Seed From Received Buffer
        /// </summary>
        /// <param name="StarBytePos"></param>
        /// <returns></returns>
        private string GetServerSeed(int StarBytePos)
        {
            try
            {
                //string ReceivedSeed = string.Empty;
                //int icnt = StarBytePos;
                //char[] chararray = System.Text.Encoding.UTF8.GetString(GlobalObjects.objSerialComm.ReceiveBuffer).ToCharArray();
                //while (icnt < StarBytePos + 16) ReceivedSeed += chararray[icnt++].ToString();                
                //return ReceivedSeed;

                string ReceivedSeed = string.Empty;
                int icnt = StarBytePos;
                char[] chararray = System.Text.Encoding.UTF8.GetString(GlobalObjects.objSerialComm.ReceiveBuffer).ToCharArray();
                while (icnt < StarBytePos + 16) ReceivedSeed += Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[icnt++]).ToString();
                return ReceivedSeed;
            }
            catch (Exception)
            {
                return "";
            }
        }
        /// <fSendDISC>
        /// Send DISC packet and Recieve and Check UA response
       /// </summary>
       /// <param name="nServerSAP"></param>
       /// <param name="nServerLowerMacAddress"></param>
       /// <param name="nClientSAP"></param>
       /// <returns></returns>
        public bool fSendDISC(int nServerSAP, int nServerLowerMacAddress, int nClientSAP)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, nServerSAP, nServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, nClientSAP);
                GlobalObjects.objHDLCLIB.fSetDISC();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                if (!GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex))return false;
                GlobalObjects.objHDLCLIB.fSetUA();//Setting Response Command type
                if (!fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer, nClientSAP))  return false;
                return true;                     
                 
            }
            catch (Exception)
            {
                return false;
            }
        }


       

    }

    
    
}
