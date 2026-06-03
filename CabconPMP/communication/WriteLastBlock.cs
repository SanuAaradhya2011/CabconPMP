using SmartCalibration.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WriteLastBlock : basecosem
{
    public override List<byte> GetPacket(ConfigureData configureData)
    {
        List<byte> datalist = new List<byte>();

        if (configureData == null) return datalist;

        //datalist.AddRange(Constants.DLMSConstants.GlobalMeterObject[configureData.commandname].mclobisatt);
        datalist.AddRange(configureData.setdata);

        return datalist;
    }
}
