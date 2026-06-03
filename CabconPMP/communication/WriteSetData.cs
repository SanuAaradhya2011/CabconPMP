using SmartCalibration.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WriteSetData : basecosem
{
    public override List<byte> GetPacket(ConfigureData configureData)
    {
        List<byte> datalist = new List<byte>();

        if (configureData == null) return datalist;

        datalist.Add(SmartCalibration.Constants.DLMSConstants.DataStractureRequest.SetRequest_Normal[0]);
        datalist.Add(SmartCalibration.Constants.DLMSConstants.DataStractureRequest.SetRequest_Normal[1]);
        datalist.Add(0xC1);
        datalist.Add(0x00);
        datalist.AddRange(configureData.setdata);

        return datalist;
    }
}
