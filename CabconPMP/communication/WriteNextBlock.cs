using SmartCalibration.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WriteNextBlock : basecosem
{
    public override List<byte> GetPacket(ConfigureData configureData)
    {
        List<byte> datalist = new List<byte>();

        if (configureData == null) return datalist;

        datalist.Add(SmartCalibration.Constants.DLMSConstants.DataStractureRequest.SetNextRequest_Block[0]);
        datalist.Add(SmartCalibration.Constants.DLMSConstants.DataStractureRequest.SetNextRequest_Block[1]);
        datalist.Add(0xC1);
        configureData.packetnumber += 1;
        datalist.Add(configureData.lastpacket);
        datalist.Add((byte)(configureData.packetnumber >> 24));
        datalist.Add((byte)(configureData.packetnumber >> 16));
        datalist.Add((byte)(configureData.packetnumber >> 8));
        datalist.Add((byte)(configureData.packetnumber));
        datalist.Add((byte)(configureData.setdata.Count));
        datalist.AddRange(configureData.setdata);
        return datalist;
    }
}
