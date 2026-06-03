using SmartCalibration.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SmartCalibration.Constants.GlobalConstants;

namespace SmartCalibration.Error
{
    public static class factoryErrorCode
    {
        public static Result GetErrorObject(int ival)
        {
            if (DLMSConstants.ErrorObject.ContainsKey(GlobalConstants.GlobalMeterType))
            {
                return DLMSConstants.ErrorObject[GlobalMeterType].GetErrorCode(ival);
            }
            else
            {
                return DLMSConstants.ErrorObject[MeterType.SM310].GetErrorCode(ival);
            }
        }
    }
}
