using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartCalibration.Constants;
using static SmartCalibration.Constants.GlobalConstants;

namespace SmartCalibration.Error
{
    public class SM310Error : IErrorCode
    {
        public GlobalConstants.Result GetErrorCode(int ival)
        {
            Result retresult = Result.Fail;

            Dictionary<int, Result> resultmapper = new Dictionary<int, Result>()
            {
                { 0, Result.Init},
                { 1, Result.Pass},
                { 2, Result.Fail},
                { 3, Result.OUT},
                { 4, Result.NA},
                { 5, Result.Retry},
           };

            try
            {
                retresult = resultmapper[ival];
            }
            catch (Exception ex)
            {
                retresult = Result.UNDEFINED;
            }

            return retresult;
        }
    }
}
