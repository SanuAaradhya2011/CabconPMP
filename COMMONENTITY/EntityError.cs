using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Data;

namespace COMMONENTITY
{
    [XmlRoot("ErrorRoot",
    IsNullable = false)]
    public class ErrorList
    {
        public List<EntityError> listError = new List<EntityError>();
    }

    [Serializable]
    public class EntityError
    {

        private string errorID;
        private string parameterName;
        private string pCBAID;
        private string errorState;
        private string procedureStage;
        private DateTime errorDate;
        private DateTime actionDate;
        private string workStationID;
        private string loggedUserID;
        private string customer;
        private string programName;
        private string meterType;
        private string actionTaken;
        private string rejectionCause;
        private string moreInformation;
        private string problemDescription;
        private string rejectid;
        private string lastRepairID;
        //private string historyStatus;

        //public string HistoryStatus
        //{
        //    get { return historyStatus; }
        //    set { historyStatus = value; }
        //}

        public string LastRepairID
        {
            get { return lastRepairID; }
            set { lastRepairID = value; }
        }

        public string Rejectid
        {
            get { return rejectid; }
            set { rejectid = value; }
        }

        public EntityError()
        {
            this.workStationID = System.Environment.MachineName;
        }

        public EntityError(String errorMessage, string parameterName, string logedUserID, string PCBAID)
        {
            this.problemDescription = errorMessage;
            this.parameterName = parameterName;
            this.loggedUserID = logedUserID;
            this.pCBAID = PCBAID;
            this.workStationID = System.Environment.MachineName;
        }

        public DateTime ActionDate
        {
            get { return actionDate; }
            set { actionDate = value; }
        }

        public string ActionTaken
        {
            get { return actionTaken; }
            set { actionTaken = value; }
        }
        

        public string ProblemDescription
        {
            get { return problemDescription; }
            set { problemDescription = value; }
        }
        
        public string RejectionCause
        {
            get { return rejectionCause; }
            set { rejectionCause = value; }
        }
        
        public string MoreInformation
        {
            get { return moreInformation; }
            set { moreInformation = value; }
        }

        public string MeterType
        {
            get { return meterType; }
            set { meterType = value; }
        }

        public string ProgramName
        {
            get { return programName; }
            set { programName = value; }
        }

        public string Customer
        {
            get { return customer; }
            set { customer = value; }
        }

        public string ErrorID
        {
            get { return errorID; }
            set { errorID = value; }
        }
        

        public string ParameterName
        {
            get { return parameterName; }
            set { parameterName = value; }
        }
        

        public string PCBAID
        {
            get { return pCBAID; }
            set { pCBAID = value; }
        }
        

        public string ErrorState
        {
            get { return errorState; }
            set { errorState = value; }
        }
        

        public string ProcedureStage
        {
            get { return procedureStage; }
            set { procedureStage = value; }
        }
        

        public DateTime ErrorDate
        {
            get { return errorDate; }
            set { errorDate = value; }
        }
        

        public string WorkStationID
        {
            get { return workStationID; }
            set { workStationID = value; }
        }
        

        public string LoggedUserID
        {
            get { return loggedUserID; }
            set { loggedUserID = value; }
        }

    }

    public class EntityErrorType
    {
        private string errorID;
        private string errorType;
        private string errorName;
        private string errorDescription;

        public string ErrorID
        {
            get { return errorID; }
            set { errorID = value; }
        }
        
        public string ErrorType
        {
            get { return errorType; }
            set { errorType = value; }
        }
       
        public string ErrorName
        {
            get { return errorName; }
            set { errorName = value; }
        }
        
        public string ErrorDescription
        {
            get { return errorDescription; }
            set { errorDescription = value; }
        }
       
    }
}
