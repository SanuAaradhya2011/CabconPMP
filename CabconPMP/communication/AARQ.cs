using SmartCalibration.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AARQ : basecosem
{
    public override List<byte> GetPacket(ConfigureData configureData)
    {
        List<byte> datalist = new List<byte>();

        List<byte> AARQTAG = new List<byte>()
                { 0x60, 0x3e };

        List<byte> AARQCONTEXT = new List<byte>()
                {0xA1, 0x09, 0x06, 0x07, 0x60, 0x85, 0x74, 0x05, 0x08, 0x01, 0x01 };

        List<byte> SECURITYTAG = new List<byte>()
                { 0x8A, 0X02, 0X07, 0X80, 0X8B, 0X07, 0X60, 0X85, 0X74, 0X05, 0X08, 0X02, 0X02, 0XAC, 0X12, 0X80 };

        List<byte> SEEDDATA = new List<byte>()
                { 
                    //SEED Length, SEED Data
                    0x10, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31
                };

        List<byte> OTHERS = new List<byte>()
                { 0xBE, 0x10, 0x04, 0x0e, 0x01, 0x00, 0x00, 0x00, 0x06, 0x5f, 0x1f, 0x04, 0x00, 0x1c};

        List<byte> ENDTAGS = new List<byte>()
                {
                    0xff, 0x3f, 0x27, 0x0f,
                };

        datalist.AddRange(AARQTAG);
        datalist.AddRange(AARQCONTEXT);
        datalist.AddRange(SECURITYTAG);
        datalist.AddRange(SEEDDATA);
        datalist.AddRange(OTHERS);
        datalist.AddRange(ENDTAGS);
        return datalist;
    }
}
