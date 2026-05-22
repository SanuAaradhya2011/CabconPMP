///****************************************************************************
//'*
//'*  Projet       : Smart Meter
//'*
//'*  Component    : DLMS-Power Tool
//'*
//'*  Module       : Association
//'*
//'*  Environment  : Visual Studio 2008 - C#.net
//'*
//'*------+----------+------------------------------------------------------------
//'*Vers |   Date    |    Programmer and Comments
//'*------+----------+------------------------------------------------------------
//'* 0.20 | 10/08/13 | Balgovind Gupta: creation.
//'*------+----------+------------------------------------------------------------
//'*      |          | XXXXX: Change Details
//'******************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utilities;
using SerialCommunication;
using ApplicationInterface;
namespace CabconPMP
{
    public partial class Association : Form
    {
        SerialComm objSerialComm = new SerialComm();
        string existingconfblock = string.Empty;
        AppSettings objApps = new AppSettings();
        
        public Association()
        {
            InitializeComponent();
        }

        private void Association_Load(object sender, EventArgs e)
        {
            //-------------------Get Avilable COM Port---------------
            string[] PortNames = objSerialComm.GetAvailablePorts();          
            Array.Reverse(PortNames);
            foreach (string Port in PortNames) cmbPort.Items.Add(Port);
           //--------------------Set Default Settings-----------------
            DefaultSettings();
            //-------------------Show Custom Setting-----------------------
            ShowDefaultSettings();
            chlSelectAll.Checked = true;            
            CheckAllAssociation();

        }
        private void ShowDefaultSettings()
        {
            try
            {
                List<string> valueList = objApps.GetSettings();
                int valIDX = 0;
                //------------------------Serial Port Settings----------------------------------------
                cmbPort.Text = valueList[valIDX++];// SerialPortSettings.Default.SerialPort;
                cmbParity.Text = valueList[valIDX++];// SerialPortSettings.Default.Parity;
                cmbDatabits.Text = valueList[valIDX++];// SerialPortSettings.Default.DataBits;
                cmbStopBits.Text= valueList[valIDX++];//SerialPortSettings.Default.StopBits;
                cmbBaudRate.Text = valueList[valIDX++];// SerialPortSettings.Default.CommandBaudRate;
                txtInterFrameTimeout.Text = valueList[valIDX++];// SerialPortSettings.Default.IntercharacterDelay.ToString();
                txtResponsTimeout.Text = valueList[valIDX++];// SerialPortSettings.Default.CommandTimeOut.ToString();

                //--------------------------HDLC Settings---------------------------------------------
                txtInformationSize.Text = valueList[valIDX++];// SerialPortSettings.Default.InformationSize.ToString();
                cmbWindowSize.Text = valueList[valIDX++];// SerialPortSettings.Default.WindowSize.ToString();
                cmbHDLCAddressing.SelectedIndex = Convert.ToInt32(valueList[valIDX++]);//Convert.ToInt32(SerialPortSettings.Default.AddressingSchem);
                txtPhysicalID.Text = valueList[valIDX++];// SerialPortSettings.Default.ServerPhysicalID.ToString();
                txtCosemBufferSize.Text = valueList[valIDX++];// SerialPortSettings.Default.CosemBufferSize.ToString();
                txtDLLBufferSize.Text = valueList[valIDX++];// SerialPortSettings.Default.DLLBufferSize.ToString();
                //----------------------------Association Settings----------------------------------------
                valIDX++;//For baudrate Index
                string CliientSAP = Convert.ToInt32(valueList[valIDX++]).ToString("X");
               
                txtServerUAddress.Text = valueList[valIDX++];// SerialPortSettings.Default.ServerSAP.ToString();
                txtServerLAddress.Text = valueList[valIDX++];// SerialPortSettings.Default.ServerLowerMacAddress.ToString();
                txtDLMSVersion.Text = valueList[valIDX++];// SerialPortSettings.Default.DLMSVersion.ToString();
                string ApplicationContext = Convert.ToInt32(valueList[valIDX++]).ToString("X");
                if (ApplicationContext == "2") cmbApplicationContext.SelectedIndex = 0; //Short Name
                else if (ApplicationContext == "1") cmbApplicationContext.SelectedIndex = 1; //Logical Name without ciphering
                else if (ApplicationContext == "3") cmbApplicationContext.SelectedIndex = 2; //Logical Name with ciphering  
                string SecurityMechanism = Convert.ToInt32(valueList[valIDX++]).ToString("X");
                if (SecurityMechanism == "0") cmbAuthenticationLevel.SelectedIndex = 0;//No Security
                else if (SecurityMechanism == "1") cmbAuthenticationLevel.SelectedIndex = 1;  //Low Level               
                else cmbAuthenticationLevel.SelectedIndex = 2; //High Level
                if (cmbAuthenticationLevel.SelectedIndex <= 1) txtPassword.Text = valueList[valIDX];
                else txtPassword.Text = valueList[valIDX + 1];
                valIDX += 2;
                txtMaxPDUSize.Text = valueList[valIDX++];

                if (CliientSAP == "10") cmbClientType.SelectedIndex = 0;//PC
                else if (CliientSAP == "20") cmbClientType.SelectedIndex = 1;//MR
                else if (CliientSAP == "30") cmbClientType.SelectedIndex = 2;//US
                else if (CliientSAP == "40" || CliientSAP == "7E") cmbClientType.SelectedIndex = 3;//FS
                else if (CliientSAP == "50") cmbClientType.SelectedIndex = 4;//FU
                //-----------------------------Conformance Block Settings-------------------------------------------
                SetConformanceBlock(valueList[valIDX++]);
                valIDX++; //---------------File Path
                cmbSignonBaudRate.Text = valueList[valIDX++]; //--------Signon Baud Rate
                if (cmbDatabits.SelectedIndex == 1) cmbSettingCategory.SelectedIndex = 1;
                else cmbSettingCategory.SelectedIndex = 0;
                //-----------------------------For Cyphering-------------------------------
                
             //  cmbCyphering.Text = valueList[valIDX++];//Non Cyphering
               txtClientSystem.Text =valueList[valIDX++];
               string securitysoot = Convert.ToInt32(valueList[valIDX++]).ToString("X");
               if (securitysoot == "10") cmbSecuritysuit.SelectedIndex = 0;//Authentication
               else if (securitysoot == "20") cmbSecuritysuit.SelectedIndex = 1;//Encryption
               else if (securitysoot == "30") cmbSecuritysuit.SelectedIndex = 2;//Authentication+Encryption
                cmbSecuritysuit.Text = securitysoot;
                   txtEncryKey.Text =valueList[valIDX++];
                  // txtAAD.Text = valueList[valIDX++];
                   string DedicaKey = Convert.ToInt32(valueList[valIDX++]).ToString("X");
                   if (DedicaKey == "01") cmbDedicatedKey.SelectedIndex = 0;//Dedicated key
                   else if (DedicaKey == "00") cmbDedicatedKey.SelectedIndex = 1;//Dedicated key
                   cmbDedicatedKey.Text = DedicaKey;
                 txtAuthentication.Text = valueList[valIDX++];
               }
            catch (Exception)
            {
            }
        }
        private void DefaultSettings()
        {
            try
            {
                if (cmbPort.Items.Count > 0) cmbPort.SelectedIndex = 0;
                cmbBaudRate.SelectedIndex = 5;
                cmbSignonBaudRate.SelectedIndex = 0;
                cmbParity.SelectedIndex = 0;
                cmbStopBits.SelectedIndex = 0;
                cmbDatabits.SelectedIndex = 1;

                cmbWindowSize.SelectedIndex = 0;
                cmbHDLCAddressing.SelectedIndex = 2;

                cmbClientType.SelectedIndex = 0;
                cmbApplicationContext.SelectedIndex = 1;
                cmbSecuritysuit.SelectedIndex = 1;//Security soot
                cmbDedicatedKey.SelectedIndex = 1; 
            }
            catch (Exception)
            {
            }
        }
      
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                 
                if (!IsValidFields()) return;
                List<string> DataValueList = new List<string>();
                DataValueList.Add(cmbPort.Text.Trim());
                DataValueList.Add(cmbParity.Text.Trim());
                DataValueList.Add(cmbDatabits.Text.Trim());
                DataValueList.Add(cmbStopBits.Text.Trim());
                DataValueList.Add(cmbBaudRate.Text.Trim());
                DataValueList.Add(txtInterFrameTimeout.Text);
                DataValueList.Add(txtResponsTimeout.Text);

                DataValueList.Add(txtInformationSize.Text.Trim());
                DataValueList.Add(cmbWindowSize.Text.Trim());
                DataValueList.Add(cmbHDLCAddressing.SelectedIndex.ToString());
                DataValueList.Add(txtPhysicalID.Text.Trim());
                DataValueList.Add(txtCosemBufferSize.Text.Trim());
                DataValueList.Add(txtDLLBufferSize.Text.Trim());
                if (cmbBaudRate.SelectedIndex < 0) DataValueList.Add("0");
                else DataValueList.Add(cmbBaudRate.SelectedIndex.ToString());

                if (cmbClientType.SelectedIndex == 0) DataValueList.Add(Convert.ToInt32("10",16).ToString());//PC
                else if (cmbClientType.SelectedIndex == 1) DataValueList.Add(Convert.ToInt32("20", 16).ToString());//MR
                else if (cmbClientType.SelectedIndex == 2) DataValueList.Add(Convert.ToInt32("30", 16).ToString());//US
                else if (cmbClientType.SelectedIndex == 3 && cmbAuthenticationLevel.SelectedIndex <= 1) DataValueList.Add(Convert.ToInt32("40", 16).ToString());//FS
                else if (cmbClientType.SelectedIndex == 3 && cmbAuthenticationLevel.SelectedIndex == 2) DataValueList.Add(Convert.ToInt32("7E", 16).ToString());//FS
                else if (cmbClientType.SelectedIndex == 4) DataValueList.Add(Convert.ToInt32("50", 16).ToString());//FU
                DataValueList.Add(txtServerUAddress.Text.Trim());
                DataValueList.Add(txtServerLAddress.Text.Trim());
                DataValueList.Add(txtDLMSVersion.Text.Trim());
                if (cmbApplicationContext.SelectedIndex == 0) DataValueList.Add("02"); //Short Name
                else if (cmbApplicationContext.SelectedIndex == 2) DataValueList.Add("03"); //Logical Name with ciphering
                else DataValueList.Add("01"); //Logical Name without ciphering               
                if (cmbAuthenticationLevel.SelectedIndex == 0) DataValueList.Add("00");//No Security
                else if (cmbAuthenticationLevel.SelectedIndex == 1) DataValueList.Add("01");  //Low Level               
                else DataValueList.Add("02"); //High Level
                if (cmbAuthenticationLevel.SelectedIndex <= 1) { DataValueList.Add(txtPassword.Text.Trim()); DataValueList.Add(""); }
                else { DataValueList.Add(""); DataValueList.Add(txtPassword.Text.Trim()); }
                DataValueList.Add(txtMaxPDUSize.Text.Trim());
                string confblk = GetConformanceBlockConfig();
                DataValueList.Add(confblk.ToUpper());
                DataValueList.Add( Application.StartupPath + @"\Configuration"); //Set as App Default Path
                DataValueList.Add(cmbSignonBaudRate.Text);//------Signon Baudrate Applicable for IEC Communication
               // DataValueList.Add(cmbCyphering.Text);// Cypher/Non Cypher -- for smart meter
                DataValueList.Add(txtClientSystem.Text);// Client system title byte -- for smart meter
                if (cmbSecuritysuit.SelectedIndex == 0) DataValueList.Add(Convert.ToInt32("10", 16).ToString());//Authentication
                else if (cmbSecuritysuit.SelectedIndex == 1) DataValueList.Add(Convert.ToInt32("20", 16).ToString());//Encryption
                else if (cmbSecuritysuit.SelectedIndex == 2) DataValueList.Add(Convert.ToInt32("30", 16).ToString());//Authentication + Encryption
                DataValueList.Add(txtEncryKey.Text);// Global Encryption Key
            
               if (cmbDedicatedKey.SelectedIndex == 0) DataValueList.Add(Convert.ToInt16("01", 16).ToString());//Dedicated key true
                else if (cmbDedicatedKey.SelectedIndex == 1) DataValueList.Add(Convert.ToInt16("00", 16).ToString());//Dedicated key false
                DataValueList.Add(txtAuthentication.Text);     // Auth. Key
               if (!objApps.SaveSettings(DataValueList))
                {
                    MessageBox.Show("Unable To saved Settings !", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
               
                MessageBox.Show("Settings saved sucessfully !", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString() + "\n" + "Unable To Save Settings!", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private bool IsValidFields()
        {
            if (cmbClientType.SelectedIndex == 3)
            {
                AccessPassword objAccessPassword = new AccessPassword();
                objAccessPassword.ShowDialog();

                if (objAccessPassword.Password != "lng123#")
                {
                    MessageBox.Show("Please enter Correct password to save settings.", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return false;
                }
            }
            if (txtResponsTimeout.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please Enter Response Timeout.", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                txtResponsTimeout.Focus();
                return false;
            }
            if (txtInterFrameTimeout.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please Enter InterFrame Timeout.", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                txtInterFrameTimeout.Focus();
                return false;
            }

            if (txtInformationSize.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please Enter Information Size.", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                txtInformationSize.Focus();
                return false;
            }
            if (txtPhysicalID.Text.Trim().Length == 0)
            {
                if (cmbHDLCAddressing.SelectedIndex > 0)
                {
                    MessageBox.Show("Please Enter Server Physical ID.", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    txtPhysicalID.Focus();
                    return false;
                }
            }
            if (txtCosemBufferSize.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please Enter Cosem Buffer Size Size.", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                txtCosemBufferSize.Focus();
                return false;
            }
            if (txtDLLBufferSize.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please Enter Data Link Layer Buffer Size.", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                txtDLLBufferSize.Focus();
                return false;
            }           
            if (txtServerUAddress.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please Enter Server Upper Address.", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                txtServerUAddress.Focus();
                return false;
            }
            if (txtServerLAddress.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please Enter Server Lower Address.", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                txtServerLAddress.Focus();
                return false;
            }
            if (txtDLMSVersion.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please Enter DLMS Version Number.", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                txtDLMSVersion.Focus();
                return false;
            }
          
                if (cmbAuthenticationLevel.SelectedIndex==1 && txtPassword.Text.Trim().Length < 8)
                {
                    MessageBox.Show("Please Enter 8 Digit Password.", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    txtPassword.Focus();
                    return false;
                }
                if (cmbAuthenticationLevel.SelectedIndex == 2 && txtPassword.Text.Trim().Length < 16)
                {
                    MessageBox.Show("Please Enter 16 Digit HLS Secret Key.", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    txtPassword.Focus();
                    return false;
                }
            

            if (txtMaxPDUSize.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please Enter Client MAX PDU Size.", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                txtMaxPDUSize.Focus();
                return false;
            }
            Control.ControlCollection objcfgBlock = this.grpConformanceBlock.Controls;
            int flgcnt = 0;
            foreach (Control objcontrol in objcfgBlock) if (((CheckBox)objcontrol).Checked) flgcnt += 1;
            if (flgcnt <= 0)
            {
                MessageBox.Show("Please Select Atleast one Service From Conformance Block List", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                grpConformanceBlock.Focus();
                return false;
            }
            return true;

        }

        
        private void cmbHDLCAddressing_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbHDLCAddressing.SelectedIndex == 0) txtPhysicalID.Enabled = false;
            else txtPhysicalID.Enabled = true;

        }

        private void cmbClientType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbClientType.SelectedIndex == 0)
            { 
                cmbAuthenticationLevel.SelectedIndex = 0;
                
            }
            else if (cmbClientType.SelectedIndex == 1)
            {
                cmbAuthenticationLevel.SelectedIndex = 1;
                cmbSecuritysuit.SelectedIndex = 1;
                cmbApplicationContext.SelectedIndex = 2;
            }
            else if (cmbClientType.SelectedIndex == 2)
            {
                cmbAuthenticationLevel.SelectedIndex = 2;
                cmbSecuritysuit.SelectedIndex = 2;
                cmbApplicationContext.SelectedIndex = 2;
            }
            else if (cmbClientType.SelectedIndex == 3)
            {
                cmbApplicationContext.SelectedIndex = 1;

            }
            else if (cmbClientType.SelectedIndex == 4)
            {
                cmbAuthenticationLevel.SelectedIndex = 2;
                cmbSecuritysuit.SelectedIndex = 2;
            }
            chlSelectAll.Checked = true;
            CheckAllAssociation();
        }

        private void cmbAuthenticationLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
             if (cmbAuthenticationLevel.SelectedIndex == 0){ txtPassword.Enabled = false;/*txtPassword.Text = "";*/}
             else txtPassword.Enabled = true;
            //{

             if (cmbAuthenticationLevel.SelectedIndex <= 1)
             {
                 txtPassword.MaxLength = 8;
                 txtPassword.Text = objApps.GetLLSPassword();// SerialPortSettings.Default.Password;
             }
             else
             {
                 txtPassword.MaxLength = 32;//16 for value in hex for cyphering
                 txtPassword.Text = objApps.GetHLSPassword();//SerialPortSettings.Default.HLSPWD;
             }
            //}
        }

        

        /// <summary>
        /// return the reverse of input string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
        /// <summary>
        /// Set the default settings on the screen as per configured value
        /// </summary>
        private void rdDefaultAccess()
        {
            try
            {
                                                   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }                              
        }

        private void cmbApplicationContext_SelectedIndexChanged(object sender, EventArgs e)
        {
            Control.ControlCollection objconf = this.grpConformanceBlock.Controls;
            foreach (Control objcontrollist in objconf) ((CheckBox)objcontrollist).Checked = false;

            if (cmbApplicationContext.SelectedIndex == 1)//Logical Name WO Ciphering
            {
                chkBlockTransferwithGetRead.Checked = true;
                chkBlockTransferwithWrite.Checked = true;
                chkGet.Checked = true;
                chkSet.Checked = true;
                chkSelectiveAccess.Checked = true;
                chkAction.Checked = true;                
            }
            else if (cmbApplicationContext.SelectedIndex == 2)//Logical Name with Ciphering
            {
                chkBlockTransferwithGetRead.Checked = true;
                chkBlockTransferwithWrite.Checked = true;
                chkGet.Checked = true;
                chkSet.Checked = true;
                chkSelectiveAccess.Checked = true;
                chkAction.Checked = true;
            }

            else
            {                
                chkread.Checked = true;
                chkWrite.Checked = true;
                chkParameterizedAccess.Checked = true;
            }
        }

        private string GetConformanceBlockConfig()
        {
            try
            {
                string confblk = string.Empty;                 
                string confbyte1 = string.Empty;
                string confbyte2 = string.Empty;
                string confbyte3 = string.Empty;

                if (Resered1.Checked) confbyte1 += "1";
                else confbyte1 += "0";
                if (Resered2.Checked) confbyte1 += "1";
                else confbyte1 += "0";
                if (Resered3.Checked) confbyte1 += "1";
                else confbyte1 += "0";
                if (chkread.Checked) confbyte1 += "1";
                else confbyte1 += "0";
                if (chkWrite.Checked) confbyte1 += "1";
                else confbyte1 += "0";
                if (chkUwrite.Checked) confbyte1 += "1";
                else confbyte1 += "0";
                if (Resered4.Checked) confbyte1 += "1";
                else confbyte1 += "0";
                if (Resered5.Checked) confbyte1 += "1";
                else confbyte1 += "0";

                if (chkAttributeWithSet.Checked) confbyte2 += "1";
                else confbyte2 += "0";
                if (chkPriority.Checked) confbyte2 += "1";
                else confbyte2 += "0";
                if (chkAttributewithGet.Checked) confbyte2 += "1";
                else confbyte2 += "0";
                if (chkBlockTransferwithGetRead.Checked) confbyte2 += "1";
                else confbyte2 += "0";
                if (chkBlockTransferwithWrite.Checked) confbyte2 += "1";
                else confbyte2 += "0";
                if (chkBlockTransferwithAction.Checked) confbyte2 += "1";
                else confbyte2 += "0";
                if (chkMultiReference.Checked) confbyte2 += "1";
                else confbyte2 += "0";
                if (chkInformationReport.Checked) confbyte2 += "1";
                else confbyte2 += "0";

                if (Resered6.Checked) confbyte3 += "1";
                else confbyte3 += "0";
                if (Resered7.Checked) confbyte3 += "1";
                else confbyte3 += "0";
                if (chkParameterizedAccess.Checked) confbyte3 += "1";
                else confbyte3 += "0";
                if (chkGet.Checked) confbyte3 += "1";
                else confbyte3 += "0";
                if (chkSet.Checked) confbyte3 += "1";
                else confbyte3 += "0";
                if (chkSelectiveAccess.Checked) confbyte3 += "1";
                else confbyte3 += "0";
                if (chkEventNotification.Checked) confbyte3 += "1";
                else confbyte3 += "0";
                if (chkAction.Checked) confbyte3 += "1";
                else confbyte3 += "0";
                
                confbyte1 = Convert.ToString(Convert.ToInt32(confbyte1, 2), 16);
                confbyte2 = Convert.ToString(Convert.ToInt32(confbyte2, 2), 16);
                confbyte3 = Convert.ToString(Convert.ToInt32(confbyte3, 2), 16);
                while (confbyte1.Length < 2) confbyte1 = "0" + confbyte1;
                while (confbyte2.Length < 2) confbyte2 = "0" + confbyte2;
                while (confbyte3.Length < 2) confbyte3 = "0" + confbyte3;
                confblk += (confbyte1 + confbyte2 + confbyte3);
                return confblk;               
            }
            catch (Exception)
            {
                return "";
            }
        }
        private void SetConformanceBlock(string ConformanceBlock)
        {
            try
            {
                string existingconfblock = ConformanceBlock;
                string cByte1 = Convert.ToString(Convert.ToInt32(existingconfblock.Substring(0, 2), 16), 2);
                string cByte2 = Convert.ToString(Convert.ToInt32(existingconfblock.Substring(2, 2), 16), 2);
                string cByte3 = Convert.ToString(Convert.ToInt32(existingconfblock.Substring(4, 2), 16), 2);
                while (cByte1.Length < 8) cByte1 = "0" + cByte1;
                while (cByte2.Length < 8) cByte2 = "0" + cByte2;
                while (cByte3.Length < 8) cByte3 = "0" + cByte3;

                char[] arr1 = cByte1.ToCharArray();
                char[] arr2 = cByte2.ToCharArray();
                char[] arr3 = cByte3.ToCharArray();

                if (arr1[0] == '1') Resered1.Checked = false;
                else Resered1.Checked = false;
                if (arr1[1] == '1') Resered2.Checked = false;
                else Resered2.Checked = false;
                if (arr1[2] == '1') Resered3.Checked = false;
                else Resered3.Checked = false;
                if (arr1[3] == '1') chkread.Checked = true;
                else chkread.Checked = false;
                if (arr1[4] == '1') chkWrite.Checked = true;
                else chkWrite.Checked = false;
                if (arr1[5] == '1') chkUwrite.Checked = true;
                else chkUwrite.Checked = false;
                if (arr1[6] == '1') Resered4.Checked = false;
                else Resered4.Checked = false;
                if (arr1[7] == '1') Resered5.Checked = false;
                else Resered5.Checked = false;


                if (arr2[0] == '1') chkAttributeWithSet.Checked = true;
                else chkAttributeWithSet.Checked = false;
                if (arr2[1] == '1') chkPriority.Checked = true;
                else chkPriority.Checked = false;
                if (arr2[2] == '1') chkAttributewithGet.Checked = true;
                else chkAttributewithGet.Checked = false;
                if (arr2[3] == '1') chkBlockTransferwithGetRead.Checked = true;
                else chkBlockTransferwithGetRead.Checked = false;
                if (arr2[4] == '1') chkBlockTransferwithWrite.Checked = true;
                else chkBlockTransferwithWrite.Checked = false;
                if (arr2[5] == '1') chkBlockTransferwithAction.Checked = true;
                else chkBlockTransferwithAction.Checked = false;
                if (arr2[6] == '1') chkMultiReference.Checked = true;
                else chkMultiReference.Checked = false;
                if (arr2[7] == '1') chkInformationReport.Checked = true;
                else chkInformationReport.Checked = false;

                if (arr3[0] == '1') Resered6.Checked = false;
                else Resered6.Checked = false;
                if (arr3[1] == '1') Resered7.Checked = false;
                else Resered7.Checked = false;
                if (arr3[2] == '1') chkParameterizedAccess.Checked = true;
                else chkParameterizedAccess.Checked = false;
                if (arr3[3] == '1') chkGet.Checked = true;
                else chkGet.Checked = false;
                if (arr3[4] == '1') chkSet.Checked = true;
                else chkSet.Checked = false;
                if (arr3[5] == '1') chkSelectiveAccess.Checked = true;
                else chkSelectiveAccess.Checked = false;
                if (arr3[6] == '1') chkEventNotification.Checked = true;
                else chkEventNotification.Checked = false;
                if (arr3[7] == '1') chkAction.Checked = true;
                else chkAction.Checked = false;               
            
             }
            catch (Exception)
            {
                MessageBox.Show("Unable To Display Conformance Block Services.", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
               
            }
        }
 
        private void chlSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckAllAssociation();
        }
        private void CheckAllAssociation()
        {
            Control.ControlCollection objselectedOption = this.grpConformanceBlock.Controls;
            foreach (Control C in objselectedOption)
            {
                try
                {
                    if (((CheckBox)C).Text != "Reserved") ((CheckBox)C).Checked = chlSelectAll.Checked;
                    //if (((CheckBox)C).Enabled) ((CheckBox)C).Checked = chlSelectAll.Checked;
                }
                catch (Exception)
                {
                }
            }
        }

        private void cmbSettingCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSettingCategory.SelectedIndex == 0)
            {
                //--7,E,1,300
                cmbDatabits.SelectedIndex = 0;
                cmbParity.SelectedIndex = 1;
                cmbStopBits.SelectedIndex = 0;
                cmbSignonBaudRate.SelectedIndex = 0;
            }
            else if (cmbSettingCategory.SelectedIndex == 1)
            {
                //--8,N,1,9600
                cmbDatabits.SelectedIndex = 1;
                cmbParity.SelectedIndex = 0;
                cmbStopBits.SelectedIndex = 0;
                cmbSignonBaudRate.SelectedIndex = 1;
            }
        }

        private void cmbPort_Click(object sender, EventArgs e)
        {
            try
            {
                cmbPort.Items.Clear();
                //-------------------Get Avilable COM Port---------------
                string[] PortNames = objSerialComm.GetAvailablePorts();
                Array.Reverse(PortNames);
                foreach (string Port in PortNames) cmbPort.Items.Add(Port);
                if (cmbPort.Items.Count > 0) cmbPort.SelectedIndex = 0;
            }
            catch (Exception)
            {
                 
            }
            
            
        }

       

       
        
    }
}
