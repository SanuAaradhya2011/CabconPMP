using DataLayer;
using LNG.Communication.SerialCommunication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SmartCalibration.Actions
{
    public class MeterAction
    {

        // TODO :   Member variables are declare and initialise here
        public object mpArg;
        public string margument;
        public Constants.GlobalConstants.Result results;
        public int m_temperature;
        public double mresulterror;

        #region OnReDo
        /// <summary>
        /// Method Name: OnReDo
        /// Description: Find method name and execute the action
        /// Author:      Mohsin Raza
        /// Date:        07-Feb-2020
        /// </summary>
        //public virtual void OnReDo(object sender, DoWorkEventArgs e)
        public virtual void OnReDo(object sender)
        {
            Constants.GlobalConstants.Result _result = Constants.GlobalConstants.Result.Pass;
            GenericAction _action = Constants.GlobalConstants.GlobalActionMapper[Constants.GlobalConstants.GlobalMeterType];
            UpdateEventArgs arg = null;
            //Meter _meter = e.Argument as Meter;
            Meter _meter = sender as Meter;
            string _meterdetail = string.Empty;

            try
            {
                 MethodInfo _method = typeof(GenericAction).GetMethod(_meter.margument);
                _action.mpArg = mpArg;
                _action.mresulterror = _meter.Result_Error;
                _action.m_temprature = (UInt16)m_temperature;
                Logger.Logger.WriteCalibLog("Method Name: " + _method.Name, _meter.mpos);
                _result = (Constants.GlobalConstants.Result)_method.Invoke(_action, new object[] { _meter.mpos, _meter.mact });
                if(_result == Constants.GlobalConstants.Result.Pass)
                    Logger.Logger.WriteCalibLog("Executed Successfully" , _meter.mpos);
                Logger.Logger.WriteCalibLog("*******************************************************" , _meter.mpos);
                _meterdetail = (string)_action.meterdetail;
                arg = new UpdateEventArgs("", false, _meter.mpos, _result, _meter.mact, _meterdetail, _meter.margument);
                //sender.Result = arg;
                
            }
            catch (Exception ex)
            {
                _result = Constants.GlobalConstants.Result.Fail;
                arg = new UpdateEventArgs("", false, _meter.mpos, _result, _meter.mact, _meterdetail, _meter.margument);
                //e.Result = arg;
                
            }

            Thread.Sleep(10);

            Constants.GlobalConstants.ActiveTask++;

           // updateMasterView(arg);

            results = _result;

            
        }
        #endregion

        public void UpdateMaster(UpdateEventArgs arg)
        {

        }
        

    }
}
