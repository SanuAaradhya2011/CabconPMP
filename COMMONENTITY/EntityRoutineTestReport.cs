using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace COMMONENTITY
{
    public class EntityRoutineTestReport
    {   
        public string mcustomername { get; set; }
        public string morderno { get; set; }
        public string mmeterrating { get; set; }
        public string mstandard { get; set; }
        public string mmetertype { get; set; }
        public string mtestid { get; set; }
        public string mclass { get; set; }
        public string mstartmeterid { get; set; }
        public string mendmeterid { get; set; }
        public bool isselected { get; set; }
        public DateTime mfromdate{get; set;}
        public DateTime mtodate { get; set; }
        public string morderdate { get; set; }
        public List<MeterReportData> mdatalist = new List<MeterReportData>();

        public DataTable mdatatable { get; set; }

        public EntityRoutineTestReport()
        {
     
        }
    }
    public class MeterReportData
    {
        public string mmtereid { get; set; }
        public List<string> mcolvalues { get; set; }

        public MeterReportData()
        {
            mcolvalues = new List<string>();
        }
    }
}
