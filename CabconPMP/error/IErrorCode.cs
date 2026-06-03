using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SmartCalibration.Constants.GlobalConstants;

namespace SmartCalibration.Error
{
    public interface IErrorCode
    {
        Result GetErrorCode(int ival);
    }
}
