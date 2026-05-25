using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using BALLAYER;
using System.ComponentModel;

namespace COMMONENTITY
{
    public class ErrorOperations
    {

        public ErrorOperations()
        {
            
        }

        public void SaveErrorLog(EntityError errorEntity, string saveDir)
        {
            string filepath = saveDir + "ErrorLog_" + DateTime.Now.ToString("yyyyMMdd") + ".xml";
            ErrorList dayErrList;
            try
            {
                if (!File.Exists(filepath))
                {
                    dayErrList = new ErrorList();
                    dayErrList.listError.Add(errorEntity);
                    XmlSerializer serializer = new XmlSerializer(typeof(ErrorList));

                    TextWriter writer = new StreamWriter(filepath);
                    serializer.Serialize(writer, dayErrList);
                    writer.Close();
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ErrorList));
  

                    FileStream fs = new FileStream(filepath, FileMode.Open);
                    dayErrList = (ErrorList)serializer.Deserialize(fs);

                    fs.Close();

                    dayErrList.listError.Add(errorEntity);

                    TextWriter writer = new StreamWriter(filepath);
                    serializer.Serialize(writer, dayErrList);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                CommonMethods.LogPMPMessage("Unable To Save Results : " + ex.Message);
            }
        }

        public ErrorList ReadErrorEntity(string filepath)
        {
            if (!File.Exists(filepath))
                return null;

            ErrorList dayErrList;
            XmlSerializer serializer = new XmlSerializer(typeof(ErrorList));


            FileStream fs = new FileStream(filepath, FileMode.Open);
            dayErrList = (ErrorList)serializer.Deserialize(fs);

            fs.Close();
            return dayErrList;
        }

        
        //public Dictionary<string, int> GetParameterErrorDictionary()
        //{
        //    Dictionary<string, int> errorDict = new Dictionary<string, int>();

        //    errorDict.Add("VERIFYPRODUCTSTAGE", (int)ErrorConstants.ErrorID.VerifyProductStage_FAIL);
        //    errorDict.Add("VERIFYPCBAID",(int)ErrorConstants.ErrorID.VerifyPCBAID_FAIL);
        //    errorDict.Add("VERIFYMETERFWVERSION",(int)ErrorConstants.ErrorID.VerifyMeterFwVersion_FAIL);
        //    errorDict.Add("VERIFYDLMSFWVERSION",(int)ErrorConstants.ErrorID.VerifyDLMSFWVersion_FAIL);
        //    errorDict.Add("VERIFYMETERPASSWORD",(int)ErrorConstants.ErrorID.VerifyMeterPassword_FAIL);
        //    errorDict.Add("WRITEPCBAID",(int)ErrorConstants.ErrorID.WritePCBAID_FAIL);
        //    errorDict.Add("VERIFYCONFIGURATION",(int)ErrorConstants.ErrorID.VerifyConfiguration_FAIL);
        //    errorDict.Add("VERIFYENERGY",(int)ErrorConstants.ErrorID.VerifyEnergy_FAIL);
        //    errorDict.Add("VERIFYMAGNETCOUNTS",(int)ErrorConstants.ErrorID.VerifyMagnetCounts_FAIL);
        //    errorDict.Add("VERIFYCTWIRE",(int)ErrorConstants.ErrorID.VerifyCTWire_FAIL);
        //    errorDict.Add("VERIFYPHASECURRENT",(int)ErrorConstants.ErrorID.VerifyPhaseCurrent_FAIL);
        //    errorDict.Add("VERIFYNEUTRALCURRENT",(int)ErrorConstants.ErrorID.VerifyNeutralCurrent_FAIL);
        //    errorDict.Add("PUSHBUTTONTEST",(int)ErrorConstants.ErrorID.PushbuttonTest_FAIL);
        //    errorDict.Add("VERIFYBATTERYSTATUS",(int)ErrorConstants.ErrorID.VerifyBatteryStatus_FAIL);
        //    errorDict.Add("SETMETERRTC",(int)ErrorConstants.ErrorID.SetMeterRTC_FAIL);
        //    errorDict.Add("METROLOGYTESTPHASE",(int)ErrorConstants.ErrorID.MetrologyTestPhase_FAIL);
        //    errorDict.Add("METROLOGYTESTNEUTRAL",(int)ErrorConstants.ErrorID.MetrologyTestNeutral_FAIL);
        //    errorDict.Add("COMMUNICATIONTESTONBATTERY",(int)ErrorConstants.ErrorID.CommunicationTestOnBattery_FAIL);
        //    errorDict.Add("COMMUNICATIONTEST",(int)ErrorConstants.ErrorID.CommunicationTest_FAIL);
        //    errorDict.Add("RTCDRIFTTEST",(int)ErrorConstants.ErrorID.RTCDriftTest_FAIL);
        //    errorDict.Add("VERIFYCALIBRATIONDATA",(int)ErrorConstants.ErrorID.VerifyCalibrationData_FAIL);
        //    errorDict.Add("CASETAMPERTEST",(int)ErrorConstants.ErrorID.CaseTamperTest_FAIL);
        //    errorDict.Add("VERIFYCASETAMPER",(int)ErrorConstants.ErrorID.VerifyCaseTamper_FAIL);
        //    errorDict.Add("RESETBILLINGDATA",(int)ErrorConstants.ErrorID.ResetBillingData_FAIL);
        //    errorDict.Add("RESETALLDATA",(int)ErrorConstants.ErrorID.ResetAllData_FAIL);
        //    errorDict.Add("RESETLOWBATT",(int)ErrorConstants.ErrorID.ResetLowBatt_FAIL);
        //    errorDict.Add("READPCBAID",(int)ErrorConstants.ErrorID.ReadPCBAID_FAIL);
        //    errorDict.Add("WRITEMETERID",(int)ErrorConstants.ErrorID.WriteMeterID_FAIL);
        //    errorDict.Add("WRITEREFVOLTAGE",(int)ErrorConstants.ErrorID.WriteRefVoltage_FAIL);
        //    errorDict.Add("WRITEMANUFACTURINGYEAR",(int)ErrorConstants.ErrorID.WriteManufacturingYear_FAIL);
        //    errorDict.Add("WRITECURRENTRATING",(int)ErrorConstants.ErrorID.WriteCurrentRating_FAIL);
        //    errorDict.Add("VERIFYMANUFACTURINGYEAR",(int)ErrorConstants.ErrorID.VerifyManufacturingYear_FAIL);
        //    errorDict.Add("WRITEPMAX",(int)ErrorConstants.ErrorID.WritePmax_FAIL);
        //    errorDict.Add("LOCKINGMETER",(int)ErrorConstants.ErrorID.LockingMeter_FAIL);
        //    errorDict.Add("PCBASTATUSTEST",(int)ErrorConstants.ErrorID.PCBAStatusTest_FAIL);
        //    errorDict.Add("PCBASTATUSTESTLTCT",(int)ErrorConstants.ErrorID.PCBAStatusTestLtct_FAIL);
        //    errorDict.Add("MAGNETSENSORTESTLEFT",(int)ErrorConstants.ErrorID.MagnetSensorTestLeft_FAIL);
        //    errorDict.Add("MAGNETSENSORTESTRIGHT",(int)ErrorConstants.ErrorID.MagnetSensorTestRight_FAIL);
        //    errorDict.Add("VOLTAGETESTRPHASE",(int)ErrorConstants.ErrorID.VoltagePhaseR_FAIL);
        //    errorDict.Add("VOLTAGETESTYPHASE",(int)ErrorConstants.ErrorID.VoltagePhaseY_FAIL);
        //    errorDict.Add("VOLTAGETESTBPHASE",(int)ErrorConstants.ErrorID.VoltagePhaseB_FAIL);
        //    errorDict.Add("CURRENTTESTRPHASE",(int)ErrorConstants.ErrorID.CurrentPhaseR_FAIL);
        //    errorDict.Add("CURRENTTESTYPHASE",(int)ErrorConstants.ErrorID.CurrentPhaseY_FAIL);
        //    errorDict.Add("CURRENTTESTBPHASE",(int)ErrorConstants.ErrorID.CurrentPhaseB_FAIL);
        //    errorDict.Add("POWERFACTORRPHASE",(int)ErrorConstants.ErrorID.PowerFactorPhaseR_FAIL);
        //    errorDict.Add("POWERFACTORYPHASE",(int)ErrorConstants.ErrorID.PowerFactorPhaseY_FAIL);
        //    errorDict.Add("POWERFACTORBPHASE",(int)ErrorConstants.ErrorID.PowerFactorPhaseB_FAIL);
        //    errorDict.Add("NEUTRALCURRENTTEST",(int)ErrorConstants.ErrorID.NeutralCurrentTest_FAIL);
        //    errorDict.Add("CTDIRECTIONTEST",(int)ErrorConstants.ErrorID.CTDirectionTest_FAIL);
        //    errorDict.Add("VERIFYRS485DEVICEADDRESS",(int)ErrorConstants.ErrorID.RS485Device_FAIL);
        //    errorDict.Add("VERIFYLPR",(int)ErrorConstants.ErrorID.VerifyLPR_FAIL);
        //    errorDict.Add("VERIFYMLPR",(int)ErrorConstants.ErrorID.VerifyMLPR_FAIL);
        //    errorDict.Add("RESETBATTERYCOUNTER",(int)ErrorConstants.ErrorID.ResetBatteryCounter_FAIL);
        //    errorDict.Add("VERIFYTEMPERATURE",(int)ErrorConstants.ErrorID.VerifyTemperature_FAIL);
        //    errorDict.Add("METERUNLOCK",(int)ErrorConstants.ErrorID.MeterUnlock_FAIL);
        //    errorDict.Add("LOCKINGMETERALL",(int)ErrorConstants.ErrorID.LockingMeterAll_FAIL);
        //    errorDict.Add("LOCKINGMETERWOTOU",(int)ErrorConstants.ErrorID.LockingMeterWoTOUAndDisplay_FAIL);
        //    errorDict.Add("LOCKINGMETERWOTOUANDDISPLAY",(int)ErrorConstants.ErrorID.LockingMeterWoTOUAndDisplay_FAIL);
        //    errorDict.Add("SETPRODUCTSTAGE",(int)ErrorConstants.ErrorID.SetProductStage_FAIL);
        //    errorDict.Add("RELAYCIRCUITTEST",(int)ErrorConstants.ErrorID.RelayCiruitTest_FAIL);
        //    errorDict.Add("RELAYCIRCUITCONNECT",(int)ErrorConstants.ErrorID.RelayCircuitConnect_FAIL);
        //    errorDict.Add("RELAYCIRCUITDISCONNECT",(int)ErrorConstants.ErrorID.RelayCircuitDisconnect_FAIL);
        //    errorDict.Add("MAINBATTERYVOLTAGETEST",(int)ErrorConstants.ErrorID.MainBatteryVoltageTest_FAIL);
        //    errorDict.Add("RTCBATTERYVOLTAGETEST",(int)ErrorConstants.ErrorID.RTCBatteryVoltageTest_FAIL);
        //    errorDict.Add("RELAYMALFUNCTIONTEST",(int)ErrorConstants.ErrorID.RelayMalfunctionTest_FAIL);
        //    errorDict.Add("CASETAMPERSWITCHTEST",(int)ErrorConstants.ErrorID.CaseTamperSwitchTest_FAIL);
        //    errorDict.Add("ACMAGNETTEST",(int)ErrorConstants.ErrorID.ACMagnetTest_FAIL);
        //    errorDict.Add("LOCKFSMODE",(int)ErrorConstants.ErrorID.LockFSMode_FAIL);
        //    errorDict.Add("UNLOCKFSMODE",(int)ErrorConstants.ErrorID.UnlockFSMode_FAIL);
        //    errorDict.Add("JTAGUNLOCKING",(int)ErrorConstants.ErrorID.JTAGUnlocking_FAIL);
        //    errorDict.Add("JTAGLOCKING",(int)ErrorConstants.ErrorID.JTAGLocking_FAIL);
        //    errorDict.Add("DIDOCIRCUITTEST",(int)ErrorConstants.ErrorID.DIDOCircuitTest_FAIL);
            

        //    return errorDict;
        //}

        //private int GetErrorID(string ParaToBeExecute)
        //{
        //    try
        //    {
        //        int errorID;

        //        var errorDict = GetParameterErrorDictionary();

        //        if (!errorDict.TryGetValue(ParaToBeExecute, out errorID))
        //            errorID = (int)ErrorConstants.ErrorID.Unknown_Error;

        //        return errorID;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable GetDataTableTemplateForErrorEntity(EntityError errorEntity)
        {

            DataTable table = new DataTable();

            table.Columns.Add("pcbid", typeof(string));
            table.Columns.Add("customer", typeof(string));
            table.Columns.Add("metertype", typeof(string));
            table.Columns.Add("errordate", typeof(string));
            table.Columns.Add("actiondate", typeof(string));
            table.Columns.Add("productionstage", typeof(string));
            table.Columns.Add("parametername", typeof(string));
            table.Columns.Add("errorstate", typeof(string));
            table.Columns.Add("problemdescription", typeof(string));
            table.Columns.Add("rejectioncause", typeof(string));
            table.Columns.Add("actiontaken", typeof(string));
            table.Columns.Add("moreinformation", typeof(string));
            table.Columns.Add("errormachineid", typeof(string));
            table.Columns.Add("errorloggeduserid", typeof(string));



            return table;
        }

        public DataTable GetDataTableForErrorEntity(DataTable table, EntityError errorEntity)
        {
            DataRow newRow = table.NewRow();
            try
            {
                newRow["pcbid"] = errorEntity.PCBAID;
                newRow["customer"] = errorEntity.Customer;
                newRow["metertype"] = errorEntity.MeterType;
                newRow["errordate"] = errorEntity.ErrorDate;
                newRow["actiondate"] = errorEntity.ActionDate;
                newRow["productionstage"] = errorEntity.ProcedureStage;
                newRow["parametername"] = errorEntity.ParameterName;
                newRow["errorstate"] = errorEntity.ErrorState;
                newRow["problemdescription"] = errorEntity.ProblemDescription;
                newRow["rejectioncause"] = errorEntity.RejectionCause;
                newRow["actiontaken"] = errorEntity.ActionTaken;
                newRow["moreinformation"] = errorEntity.MoreInformation;
                newRow["errormachineid"] = errorEntity.WorkStationID;
                newRow["errorloggeduserid"] = errorEntity.LoggedUserID;

                table.Rows.Add(newRow);
            }
            catch (Exception ex)
            {
                CommonMethods.LogPMPMessage("Unable To Save Results : " + ex.Message);
            }
            return table;
        }

    }
}
