using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMMONENTITY
{
    class EntityDefaultoptionsCheck
    {
       
        int checkRTCDrift;
        int checkRTCDriftValue;
        int writeMeterMfgDetails;
        string manufacturingData_CurrentIB;
        string manufacturingData_CurrentIMax;
        int checkCalibration;      
        int meterLock;
        string firmWareVersion;      
        int pushbuttonMinCount;
        int pushbuttonMaxCount;
        int ledMinCount;
        int ledMaxCount;
        int caseTamperMinCount;
        int caseTamperMaxCount;
        int acMagnetMinCount;
        int acMagnetMaxCount;

        int meterVarient;
        bool rememberUser;
        string loginUserID;
        string loginUserPassword;

        public int CheckRTCDrift
        {
            get { return checkRTCDrift; }
            set { checkRTCDrift = value; }
        }       
        public int CheckRTCDriftValue
        {
            get { return checkRTCDriftValue; }
            set { checkRTCDriftValue = value; }
        }

        public int WriteMeterMfgDetails
        {
            get { return writeMeterMfgDetails; }
            set { writeMeterMfgDetails = value; }
        }
        public string ManufacturingData_CurrentIB
        {
            get { return manufacturingData_CurrentIB; }
            set { manufacturingData_CurrentIB = value; }
        }
        public string ManufacturingData_CurrentIMax
        {
            get { return manufacturingData_CurrentIMax; }
            set { manufacturingData_CurrentIMax = value; }
        }
        public int CheckCalibration
        {
            get { return checkCalibration; }
            set { checkCalibration = value; }
        }
        public int MeterLock
        {
            get { return meterLock; }
            set { meterLock = value; }
        }
        public string FirmWareVersion
        {
            get { return firmWareVersion; }
            set { firmWareVersion = value; }
        }

        public int PushbuttonMinCount
        {
            get { return pushbuttonMinCount; }
            set { pushbuttonMinCount = value; }
        }
        public int PushbuttonMaxCount
        {
            get { return pushbuttonMaxCount; }
            set { pushbuttonMaxCount = value; }
        }

        public int LEDMinCount
        {
            get { return ledMinCount; }
            set { ledMinCount = value; }
        }
        public int LEDMaxCount
        {
            get { return ledMaxCount; }
            set { ledMaxCount = value; }
        }
        public int CaseTamperMinCount
        {
            get { return caseTamperMinCount; }
            set { caseTamperMinCount = value; }
        }
        public int CaseTamperMaxCount
        {
            get { return caseTamperMaxCount; }
            set { caseTamperMaxCount = value; }
        }
        public int ACMagnetMinCount
        {
            get { return acMagnetMinCount; }
            set { acMagnetMinCount = value; }
        }
        public int ACMagnetMaxCount
        {
            get { return acMagnetMaxCount; }
            set { acMagnetMaxCount = value; }
        }


        public int MeterVarient
        {
            get { return meterVarient; }
            set { meterVarient = value; }
        }
        public bool RememberUser
        {
            get { return rememberUser; }
            set { rememberUser = value; }
        }
        public string LoginUserID
        {
            get { return loginUserID; }
            set { loginUserID = value; }
        }
        public string LoginUserPassword
        {
            get { return loginUserPassword; }
            set { loginUserPassword = value; }
        }
        
    }
}
