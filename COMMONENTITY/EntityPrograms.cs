using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMMONENTITY
{
   public class EntityPrograms
    {
       string programName;
       string programParameterName;
       string testType;
       public string ProgramName
       {
           get { return programName; }
           set { programName = value ; }
       }
       public string ProgramParameterName
       {
           get { return programParameterName; }
           set { programParameterName = value; }
       }
       public string TestType
       {
           get { return testType; }
           set { testType = value; }
       }
    }
}
