using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartCalibration.Constants;
using static SmartCalibration.Constants.GlobalConstants;

namespace SmartCalibration.Error
{
    public class SM110Error : IErrorCode
    {
        public GlobalConstants.Result GetErrorCode(int ival)
        {
            Result retresult = Result.Fail;

            Dictionary<int, Result> resultmapper = new Dictionary<int, Result>()
            {
                { 0, Result.Pass},
                { 1, Result.HF},
                { 3, Result.RWD},
                { 250, Result.OTHER},
                

            };

            try
            {
                retresult = resultmapper[ival];
            }
            catch(Exception ex)
            {
                retresult = Result.UNDEFINED;
            }

            return retresult;
        }
    }
}
