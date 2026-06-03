using SmartCalibration.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WriteFirstBlock : basecosem
{
    public override List<byte> GetPacket(ConfigureData configureData)
    {
        List<byte> datalist = new List<byte>();

        if (configureData == null) return datalist;

        datalist.Add(SmartCalibration.Constants.DLMSConstants.DataStractureRequest.SetRequest_Block[0]);
        datalist.Add(SmartCalibration.Constants.DLMSConstants.DataStractureRequest.SetRequest_Block[1]);
        datalist.Add(0xC1);
        datalist.Add(0x0);

        for (int icount = 0; icount < 9; icount++)
            datalist.Add((byte)(configureData.setdata[icount]));

        configureData.packetnumber += 1;

        datalist.Add(configureData.lastpacket);
        datalist.Add((byte)(configureData.packetnumber >> 24));
        datalist.Add((byte)(configureData.packetnumber >> 16));
        datalist.Add((byte)(configureData.packetnumber >> 8));
        datalist.Add((byte)(configureData.packetnumber));
        datalist.Add((byte)(configureData.setdata.Count - 11));

        for (int icount = 11; icount < configureData.setdata.Count; icount++)
        {
            datalist.Add(configureData.setdata[icount]);
        }

        return datalist;
    }
}
