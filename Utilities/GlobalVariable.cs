using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialCommunication;

namespace Utilities
{
    public sealed class GlobalVariable
    {

        public int seqno = 0;
        private static volatile GlobalVariable instance;
        private static object syncRoot = new Object();   
        public int gMeterIDLen = 9;
        public enum METERTYPE { E350, E450,E150 };
                                   //SM110      SM310   
        public enum XmlMeterType { GSDLMSE350, RFE350SM310, RFE250 };
       public enum CommandExecutionType { ReadData = 3, ReadProfile = 2, };
       public enum ScalarUnitCommandType { InstantScalarUnit = 0, BillingScalarUnit = 1, LoadSurveyScalarUnit = 2, TamperScalarUnit = 3, DailySurveyScalarUnit = 4 };
               private GlobalVariable() { }

        public static GlobalVariable Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        instance = new GlobalVariable();
                    }
                }

                return instance;
            }
        }
       

    }
}