using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CabconRTL
{
    public class CommonEntity
    {
        // Meter ID Details : Blank
        public string MeteriD { get; set; }
        // PCBA ID : Get from database
        public string PCBAID {get; set;}
        // Meter Type : 1P or 3P
        // fill by User
        public string MeterType { get; set; }
        // Function Type : Calibration
        public string ProcedureType { get; set; }
        // Procedure Name Created By AEMCAL
        public string TestID { get; set; }
        // Defualt Exe Name : CabconRTL.exe
        public string ProgramName { get; set; }
        // Test point name details comma seperated
        public string ParametersName { get; set; }
        // blank
        public string ParametersDefaultValue { get; set; }
        // Limits with Max value
        public string Max { get; set; }
        // Limits with Min value
        public string Min { get; set; }
        // Execution Results : Values
        public string ExecutionRemarks { get; set; }
        // Results of each step with comma seperated
        public string ExecutionStatus { get; set; }
        // Execution Date
        public DateTime ExecutionDate { get; set; }
        // Customer Name
        public string CustomerName { get; set; }
        // Final Status : Pass or Fail
        public string FinalStatus { get; set; }
        // date of updtation : Blank
        public DateTime UpdateStatus { get; set; }
        // PC Name
        public string WorkstationID { get; set; }
        // Blank
        public string logedUserID { get; set; }

        // Blank
        public int LatestStatus { get; set; }

    }
}
