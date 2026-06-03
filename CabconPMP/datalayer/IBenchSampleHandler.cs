using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartCalibration.Constants.GlobalConstants;

namespace SmartCalibration.DataLayer
{
    public interface IBenchSampleHandler
    {
       bool FillSamplefmfile();
        
    }
}
