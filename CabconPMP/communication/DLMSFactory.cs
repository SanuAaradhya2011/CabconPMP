using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCalibration.Communication
{
    public class DLMSFactory
    {
        public static Dictionary<Constants.GlobalConstants.MeterType, basehdlc> HDLCObjectMapper = new Dictionary<Constants.GlobalConstants.MeterType, basehdlc>();
        public static Dictionary<Constants.DLMSConstants.DLMSCommand, basecosem> CosemObjectMapper = new Dictionary<Constants.DLMSConstants.DLMSCommand, basecosem>();
        public static Dictionary<Constants.GlobalConstants.MeterType, basecosem> AARQMapper = new Dictionary<Constants.GlobalConstants.MeterType, basecosem>();

        static DLMSFactory()
        {

            HDLCObjectMapper.Add(Constants.GlobalConstants.MeterType.SM110, new HDLC(Constants.GlobalConstants.MeterType.SM110));
            HDLCObjectMapper.Add(Constants.GlobalConstants.MeterType.SM310, new HDLC(Constants.GlobalConstants.MeterType.SM310));
            HDLCObjectMapper.Add(Constants.GlobalConstants.MeterType.NONAMI1P, new HDLC(Constants.GlobalConstants.MeterType.NONAMI1P));
            HDLCObjectMapper.Add(Constants.GlobalConstants.MeterType.NONAMI3P, new HDLC(Constants.GlobalConstants.MeterType.NONAMI3P));
            HDLCObjectMapper.Add(Constants.GlobalConstants.MeterType.SPS2, new HDLC(Constants.GlobalConstants.MeterType.SPS2));

            AARQMapper.Add(Constants.GlobalConstants.MeterType.SM110, new AARQ());
            AARQMapper.Add(Constants.GlobalConstants.MeterType.SM310, new AARQ());
            AARQMapper.Add(Constants.GlobalConstants.MeterType.NONAMI1P, new NonAMI1PAARQ());
            AARQMapper.Add(Constants.GlobalConstants.MeterType.NONAMI3P, new NonAMI1PAARQ());
            AARQMapper.Add(Constants.GlobalConstants.MeterType.SPS2, new NonAMI1PAARQ());

            // Cosem Object Creation

            CosemObjectMapper.Add(Constants.DLMSConstants.DLMSCommand.AARQ, AARQMapper[Constants.GlobalConstants.GlobalMeterType]);
            CosemObjectMapper.Add(Constants.DLMSConstants.DLMSCommand.NONAMI1PAARQ, new NonAMI1PAARQ());
            CosemObjectMapper.Add(Constants.DLMSConstants.DLMSCommand.HIGHLEVELSECURITYPASS, new HighLevelSecurityPass());
            CosemObjectMapper.Add(Constants.DLMSConstants.DLMSCommand.NORMALGET, new NormalGet());
            CosemObjectMapper.Add(Constants.DLMSConstants.DLMSCommand.NORMALSET, new NormalSet());
            CosemObjectMapper.Add(Constants.DLMSConstants.DLMSCommand.WRITEFIRSTBLOCK, new WriteFirstBlock());
            CosemObjectMapper.Add(Constants.DLMSConstants.DLMSCommand.WRITENEXTBLOCK, new WriteNextBlock());
            CosemObjectMapper.Add(Constants.DLMSConstants.DLMSCommand.WRITESET, new WriteSetData());
        }
    }
}
