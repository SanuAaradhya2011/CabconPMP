using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMMONENTITY
{
   public class EntityExecutionResult
    {
       string meterID;
       string pcbaID;
       string exemeterType;
       string exeprocedureType;
       string exetestID;
       string exeprogramName;
       string exeparametersName;
       string exeparaDefaultValue;
       string exeparaMinValue;
       string exeparaMaxValue;    
       string executionRemarks;
       string executionStatus;
       DateTime executionDate;
       DateTime executionStatusStart;
       DateTime executionstatusEnd;
       string executionStatusStartpcba;
       string executionstatusEndpcba;
       string customerName;
       string finalResult;
       string workStationID;
       string logedUserID;
       int latestStatus;

        string missingMeterRangeFrom;
        string missingMeterRangeTo;
        string missingMeterAlphaFileld;
        public enum SelectionType { PCBAID = 0, MeterID = 1 , DATE=2};
       public string MeterID
       {
           get { return meterID; }
           set { meterID = value; }
       }
       public string PCBAID
       {
           get { return pcbaID; }
           set { pcbaID = value; }
       }
        public string ExecutionMeterType
        {
            get { return exemeterType; }
            set { exemeterType = value; }
        }
        public string ExecutionProcedureType
        {
            get { return exeprocedureType; }
            set { exeprocedureType = value; }
        }
        public string ExecutionTestID
        {
            get { return exetestID; }
            set { exetestID = value; }
        }
        public string ExecutionProgramName
        {
            get { return exeprogramName; }
            set { exeprogramName = value; }
        }
        public string ExecutionParametersName
        {
            get { return exeparametersName; }
            set { exeparametersName = value; }
        }
        public string ExecutionParaDefaultValue
        {
            get { return exeparaDefaultValue; }
            set { exeparaDefaultValue = value; }
        }
        public string ExecutionParaMinValue
        {
            get { return exeparaMinValue; }
            set { exeparaMinValue = value; }
        }
        public string ExecutionParaMaxValue
        {
            get { return exeparaMaxValue; }
            set { exeparaMaxValue = value; }
        }
        public string ExecutionRemarks
        {
            get { return executionRemarks; }
            set { executionRemarks = value; }
        }
        public string ExecutionStatus
        {
            get { return executionStatus; }
            set { executionStatus = value; }
        }
        public DateTime ExecutionDate
        {
            get { return executionDate; }
            set { executionDate = value; }
        }
        public DateTime ExecutionStatusStart
        {
            get { return executionStatusStart; }
            set { executionStatusStart = value; }
        }
        public DateTime ExecutionstatusEnd
        {
            get { return executionstatusEnd; }
            set { executionstatusEnd = value; }
        }
        public string ExecutionStatusStartPCBA
        {
            get { return executionStatusStartpcba; }
            set { executionStatusStartpcba = value; }
        }
        public string ExecutionstatusEndPCBA
        {
            get { return executionstatusEndpcba; }
            set { executionstatusEndpcba = value; }
        }
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }
        public string FinalResult
        {
            get { return finalResult; }
            set { finalResult = value; }
        }
        public string WorkStationID
        {
            get { return workStationID; }
            set { workStationID = value; }
        }
        public string LogedUserID
        {
            get { return logedUserID; }
            set { logedUserID = value; }
        }
        public int LatestStatus
        {
            get { return latestStatus; }
            set { latestStatus = value; }
        }
        public string MissingMeterRangeFrom
        {
            get { return missingMeterRangeFrom; }
            set { missingMeterRangeFrom = value; }
        }
        public string MissingMeterRangeTo
        {
            get { return missingMeterRangeTo; }
            set { missingMeterRangeTo = value; }
        }
        public string MissingMeterAlphaFileld
        {
            get { return missingMeterAlphaFileld; }
            set { missingMeterAlphaFileld = value; }
        }
         
    }
}
