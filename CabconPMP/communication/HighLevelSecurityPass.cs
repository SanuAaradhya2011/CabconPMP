using SmartCalibration.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HighLevelSecurityPass : basecosem
{
    public override List<byte> GetPacket(ConfigureData configureData)
    {
        List<byte> datalist = new List<byte>();

        List<byte> cosempkt = new List<byte>()
            {
                0x01, 0x09, 0x10, 0xfc, 0x91,
                0xae, 0x8c, 0x11, 0xd3, 0x8d,
                0x3e, 0x50, 0x19, 0x91, 0xcf,
                0xd4, 0x30, 0x79, 0xaf,
            };

        datalist.AddRange(SmartCalibration.Constants.DLMSConstants.GlobalGenericMeterObject[SmartCalibration.Constants.DLMSConstants.DLMSCommand.HIGHLEVELSECURITYPASS].mclobisatt);
        datalist.AddRange(cosempkt);

        return datalist;
    }
}