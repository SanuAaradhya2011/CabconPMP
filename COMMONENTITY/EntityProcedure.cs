using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMMONENTITY
{
   public class EntityProcedure
    {

       long procedureID;
       string meterType;
       string procedureType;
       string procedureName;
       int serializationwindow;
       string programName;
       string parametersName;
       string paraDefaultValue;
       string paraMinValue;
       string paraMaxValue;
       bool paraActivationStatus;
       string meterIDList;
       DateTime paraUpdatedDateTime;      

       public long ProcedureID
       {
           get { return procedureID; }
           set { procedureID = value; }
       }
       public string MeterType
       {
           get { return meterType; }
           set { meterType = value; }
       }
       public string ProcedureType
       {
           get { return procedureType; }
           set { procedureType = value; }
       }
       public string ProcedureName
       {
           get { return procedureName; }
           set { procedureName = value; }
       }
       public int SerializationWindow
       {
           get { return serializationwindow; }
           set { serializationwindow = value; }
       }
       public string ProgramName
       {
           get { return programName; }
           set { programName = value; }
       }
       public string ParametersName
       {
           get { return parametersName; }
           set { parametersName = value; }
       }
       public string ParaDefaultValue
       {
           get { return paraDefaultValue; }
           set { paraDefaultValue = value; }
       }
       public string ParaMinValue
       {
           get { return paraMinValue; }
           set { paraMinValue = value; }
       }
       public string ParaMaxValue
       {
           get { return paraMaxValue; }
           set { paraMaxValue = value; }
       }
       public bool ParaActivationStatus
       {
           get { return paraActivationStatus; }
           set { paraActivationStatus = value; }
       }
       public DateTime ParaUpdatedDateTime
       {
           get { return paraUpdatedDateTime; }
           set { paraUpdatedDateTime = value; }
       }
       public string MeterIDList
       {
           get { return meterIDList; }
           set { meterIDList = value; }
       }
    }
}
