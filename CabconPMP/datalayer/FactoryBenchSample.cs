using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartCalibration.Constants.GlobalConstants;

namespace SmartCalibration.DataLayer
{
    public class FactoryBenchSample
    {
        static public Dictionary<MeterType, IBenchSampleHandler> BenchSampleMapper = new Dictionary<MeterType, IBenchSampleHandler>()
        {
            { MeterType.SM110, new SM110BenchSample()},
            { MeterType.SM310, new SM310BenchSample()},
            { MeterType.SPS2, new SM310BenchSample()},
        };

        public static IBenchSampleHandler GetBenchSampleObject(MeterType metertype)
        {
            try
            {
                return BenchSampleMapper[metertype];
            }
            catch(Exception ex)
            {
                // Defualt Three Phased Samples
                return null;
            }

            
        }
    }
}
