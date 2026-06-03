using SmartCalibration.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartCalibration.Constants.GlobalConstants;

namespace SmartCalibration.Constants
{
    public class DLMSConstants
    {
        public static int PacketSize = 0x4d;
        public static int MAXTIMEOUT = 16000;

        public enum COMMANDCODE
        {
            RESETPHASE = 1,
            INITPHASE,
            CALIBPHASE,
            INITPHASELAG,
            PHASELAGCALIB,
            RESETNEUTRAL,
            INITNEUTRAL,
            CALIBNEUTRAL,
            INITNEUTRALLAG,
            NEUTRALLAGCALIB,
            CALIBTEMP,
            WRITEDEFAULTCALIB,
            WRITEDEFAULTPHASE,
            WRITEDEFUALTNEUTRAL,
            VERIFYCALIB,

        }

        public enum DLMSCommand
        {
            SNRM,
            AARQ,
            NONAMI1PAARQ,
            HIGHLEVELSECURITYPASS,
            NORMALGET,
            NORMALSET,
            NORMASETACTION,
            RTCSET,
            GETMETERID,
            GETPCBANUMBER,
            WRITEFIRSTBLOCK,
            WRITENEXTBLOCK,
            WRITELASTBLOCK,
            WRITESET,
            GETTRAVELER,
            SETTRAVELER,
            DATARESET,
            SM110CALIB,
            SM110CALIBVERIFY,
            SMSETMRPWD,
            SMSETUSPWD,
            SM310INIT,
            SM310GETSAMPLECALIBRATE,
            SM310SETCALIBRATE,
            SM310VERIFYNEUTRAL,
            SM310CALIBVERIFY,
            CALCURRENTPH,
            CALCURRENTN,
            CALCURRENT,
            NONAMI1PCALCURRENTN,
            NONAMI1PCALCURRENTPH,
            NONAMI1PCALIB,
            NONAMI1PCALIBPHASE,
            NONAMI1PCALIBNEUTRAL,
            WRITEAES,
            SPS2SET,
            SPS2CALIBVERIFY,
            SM110GETIMAGEBLOCKSIZE,
            SM110GETIMAGETRANSFERENABLESTATUS,
            SM110SETIMAGETRANSFERENABLESTATUS,
            SM110INITIMAGETRANSFER,
            SM110IMAGEBLOCKTRANSFER,
            SM110IMAGEFIRSTBLOCKNOTTRANSFERBLOKNUMBER,
            SM110IMAGEVERIFY,
            SM110IMAGEACTIVATION,
            SM110IMAGESTATUS
        }

        public enum HDLC
        {
            TAG = 0x7E,
            DESTADDBYTE0 = 0X00,
            DESTADDBYTE1 = 0X02,
            DESTADDBYTE2 = 0X04,
            DESTADDBYTE3 = 0X01,
            SM_ClientAdd = 0xFD,
            SNRM = 0x93,
            DISC = 0x53,
            FRAMETYPE = 0xA0,
        }

        public enum COSEM
        {
            LLC0 = 0xE6,
            LLC1 = 0xE6,
            LLC2 = 0x00,
            NORMALGETREQUEST0 = 0xC0,
            NORMALGETREQUEST1 = 0x01,
            NORMALSETREQUEST0 = 0xC1,
            NORMALSETREQUEST1 = 0x01,
            BLOCKSETREQUEST0 = 0xC1,
            BLOCKLSETREQUEST1 = 0x02,
            BLOCKGETREQUEST0 = 0xC1,
            BLOCKLGETREQUEST1 = 0x03,
            ACTIONNORMALREQUEST0 = 0xC3,
            ACTIONNORMALREQUEST1 = 0x01,
            ACTIONFIRSTBLOCK0 = 0xC3,
            ACTIONFIRSTBLOCK1 = 0x06,

        }

        public enum DLMSDATATYPE
        {
            NULL = 0x00,
            ARRAY = 0x01,
            STRUCT = 0x02,
            BOOLEAN = 0x03,
            BITSTRING = 0x04,
            DOUBLELONG = 0x05,
            DOUBLEUNSIGNEDLONG = 0x06,
            OCTETSTRING = 0x09,
            VISIBLESTRING = 10,
            BCD = 13,
            INTEGER = 15,
            LONG = 16,
            UNSIGNED = 17,
            LONGUNSIGNED = 18,
            COMPACTARRAY = 19,
            LONG64 = 20,
            LONGUNSIGNED64 = 21,
            ENUM = 22,
            FLOAT32 = 23,
            FLOAT64 = 24,
            DATETIME = 25,
            DATE = 26,
            TIME = 27,
            EXTOCTETSTRING = 0x82,
        }

        public struct DataStractureRequest
        {
            public static byte[] SequrityRequest_Normal = new byte[] { 0xC1, 0x00 };
            public static byte[] GetRequest_Normal = new byte[] { 0xC0, 0x01 };
            public static byte[] SetRequest_Normal = new byte[] { 0xC1, 0x01 };
            public static byte[] SetRequest_Block = new byte[] { 0xC1, 0x02 };
            public static byte[] SetNextRequest_Block = new byte[] { 0xC1, 0x03 };
            public static byte[] ActionRequest_Normal = new byte[] { 0xC3, 0x01 };
            public static byte[] ActionRequest_FirstBlock = new byte[] { 0xC3, 0x04 };
            public static byte[] ActionRequest_pBlock = new byte[] { 0xC3, 0x06 };

        }

        public class DLMSDataStructure
        {
            public string mParamName;
            public byte[] mclobisatt;
            public long muiRxDelay;
            public byte[] mbRequestType;


            public DLMSDataStructure(string nParam, byte[] bArr, long nuiRxDelay, byte[] bRequesttype)
            {
                mParamName = nParam;
                mclobisatt = bArr;
                muiRxDelay = nuiRxDelay;
                mbRequestType = bRequesttype;
            }
        }

        public static Dictionary<byte, DLMSCommand> GetNumberCommandMapper = new Dictionary<byte, DLMSCommand>()
        {
            { 01, DLMSCommand.GETPCBANUMBER},
            { 02, DLMSCommand.GETMETERID},
        };

        public static Dictionary<byte, DLMSCommand> CalCurrentCommandMapper = new Dictionary<byte, DLMSCommand>()
        {
            { 01, DLMSCommand.NONAMI1PCALCURRENTN},
            { 02, DLMSCommand.NONAMI1PCALCURRENTPH},
        };

        public static Dictionary<DLMSCommand, DLMSDataStructure> GlobalGenericMeterObject = new Dictionary<DLMSCommand, DLMSDataStructure>
        {
            // Command Name              // Request Type Class ID OBIS Att
            {
                DLMSCommand.HIGHLEVELSECURITYPASS,
                new DLMSDataStructure("HIGHLEVELPASS",
                new byte[]{0xC3, 0x01, 0xC1, 0x00, 0x0F, 0x00, 0x00, 0x28, 0x00, 0x00, 0xFF, 0x01 }, 3000, DataStractureRequest.SequrityRequest_Normal)
            },

            {
                DLMSCommand.RTCSET,
                new DLMSDataStructure("RTCSET",
                new byte[]{0xC1, 0x01, 0xC1, 0x00, 0x08, 0x00, 0x00, 0x01, 0x00, 0x00, 0xFF, 0x02 }, 3000, DataStractureRequest.SetRequest_Normal)
            },

            {
                DLMSCommand.AARQ,
                new DLMSDataStructure("AARQ",
                new byte[]{ }, 3000, DataStractureRequest.SetRequest_Normal)
            },

            {
                DLMSCommand.GETMETERID,
                new DLMSDataStructure("METERID",
                new byte[]{0xC0, 0x01, 0xC1, 0x00, 0x01, 0x00, 0x00, 0x60, 0x01, 0x00, 0xFF, 0x02, 0x00 }, 3000, DataStractureRequest.SetRequest_Normal)
            },

            {
                DLMSCommand.GETPCBANUMBER,
                new DLMSDataStructure("GETPCBANUMBER",
                new byte[]{0xC0, 0x01, 0xC1, 0x00, 0x01, 0x00, 0x00, 0x60, 0x01, 0x8b, 0xFF, 0x02, 0x00}, 3000, DataStractureRequest.SetRequest_Normal)
            },

            {
                DLMSCommand.GETTRAVELER,
                new DLMSDataStructure("GETTRAVELLERSTATUS",
                new byte[]{0xC0, 0x01, 0xC1, 0x00, 0x01, 0x00, 0x00, 0x60, 0x02, 0x9b, 0xFF, 0x02, 0x00}, 3000, DataStractureRequest.SetRequest_Normal)
            },

            {
                DLMSCommand.SETTRAVELER,
                new DLMSDataStructure("SETTRAVELLERSTATUS",
                new byte[]{0xC1, 0x01, 0xC1, 0x00, 0x01, 0x00, 0x00, 0x60, 0x02, 0x9b, 0xFF, 0x02, 0x00}, 3000, DataStractureRequest.SetRequest_Normal)
            },

            {
                DLMSCommand.DATARESET,
                new DLMSDataStructure("DATARESET",
                new byte[]{0xC3, 0x01, 0xC1, 0x00,
                           0x09,    // Class ID
                           0x00, 0x01, 0x0A, 0x08, 0x00, 0xFF},// OBIS
                           3000,
                           DataStractureRequest.ActionRequest_Normal)
            },

             {
                DLMSCommand.SMSETMRPWD,
                new DLMSDataStructure("SETMRPWD",
                new byte[]{0xC1, 0x01, 0xC1, 0x00,
                           0x0F,    // Class ID
                           0x00, 0x00, 0x28, 0x00, 0x02, 0xFF, 0x07},// OBIS & AttID
                           3000,
                           DataStractureRequest.SetRequest_Normal)
            },

            {
                DLMSCommand.SMSETUSPWD,
                new DLMSDataStructure("SETUSPWD",
                new byte[]{0xC3, 0x01, 0xC1, 0x00,
                           0x0F,    // Class ID
                           0x00, 0x00, 0x28, 0x00, 0x03, 0xFF, 0x02},// OBIS & AttID
                           3000,
                           DataStractureRequest.ActionRequest_Normal)
            },
        };

        public static Dictionary<DLMSCommand, DLMSDataStructure> GlobalSM110MeterObject = new Dictionary<DLMSCommand, DLMSDataStructure>
        {
            {
                 DLMSCommand.DATARESET,
                 new DLMSDataStructure("DATARESET",
                 new byte[]{0xC3, 0x01, 0xC1, 0x00,
                           0x09,    // Class ID
                           0x00, 0x01, 0x0A, 0x08, 0x00, 0xFF, 0x01},// CLID, OBIS, ATTID
                           3000,
                           DataStractureRequest.ActionRequest_Normal)
            },

            {
                 DLMSCommand.SM110CALIB,
                 new DLMSDataStructure("SM110CALIB",
                 new byte[]{0xC1, 0x01, 0xC1, 0x00,
                           0x01,    // Class ID
                           0x00, 0x00, 96, 0x02, 132, 0xFF, 0x02},// CLID, OBIS, ATTID
                           3000,
                           DataStractureRequest.ActionRequest_Normal)
            },

            {
                 DLMSCommand.SM110CALIBVERIFY,
                 new DLMSDataStructure("SM110CALIBVERIFY",
                 new byte[]{0xC0, 0x01, 0xC1, 0x00,
                           0x01,    // Class ID
                           0x00, 0x00, 0x60, 0x02, 0x88, 0xFF, 0x02},// CLID, OBIS, ATTID
                           3000,
                           DataStractureRequest.ActionRequest_Normal)
            },
            {
                 DLMSCommand.SM110GETIMAGEBLOCKSIZE,
                 new DLMSDataStructure("SM110GETIMAGEBLOCKSIZE",
                 new byte[]{0xC0, 0x01, 0xC1, 0x00,
                           0x12,    // Class ID
                           0x00, 0x00, 0x2C, 0x00, 0x00, 0xFF, 0x02, 0x00},// CLID, OBIS, ATTID
                           3000,
                           DataStractureRequest.GetRequest_Normal)
            },
            {
                 DLMSCommand.SM110GETIMAGETRANSFERENABLESTATUS,
                 new DLMSDataStructure("GETIMAGETRANSFERENABLESTATUS",
                 new byte[]{0xC0, 0x01, 0xC1, 0x00,
                           0x12,    // Class ID
                           0x00, 0x00, 0x2C, 0x00, 0x00, 0xFF, 0x05, 0x00},// CLID, OBIS, ATTID
                           3000,
                           DataStractureRequest.GetRequest_Normal)
            },
            {
                 DLMSCommand.SM110SETIMAGETRANSFERENABLESTATUS,
                 new DLMSDataStructure("SETIMAGETRANSFERENABLESTATUS",
                 new byte[]{0xC3, 0x01, 0xC1, 0x00,
                           0x12,    // Class ID
                           0x00, 0x00, 0x2C, 0x00, 0x00, 0xFF, 0x05, 0x00},// CLID, OBIS, ATTID
                           3000,
                           DataStractureRequest.SetRequest_Normal)
            },
             {
                 DLMSCommand.SM110INITIMAGETRANSFER,
                 new DLMSDataStructure("INITIMAGETRANSFER",
                 new byte[]{0xC3, 0x01, 0xC1, 0x00,
                           0x12,    // Class ID
                           0x00, 0x00, 0x2C, 0x00, 0x00, 0xFF, 0x01, 0x01},// CLID, OBIS, ATTID
                           3000,
                           DataStractureRequest.ActionRequest_Normal)
            },
             {
                 DLMSCommand.SM110IMAGEBLOCKTRANSFER,
                 new DLMSDataStructure("IMAGEBLOCKTRANSFER",
                 new byte[]{0xC3, 0x01, 0xC1, 0x00,
                           0x12,    // Class ID
                           0x00, 0x00, 0x2C, 0x00, 0x00, 0xFF, 0x02, 0x01},// CLID, OBIS, ATTID
                           3000,
                           DataStractureRequest.ActionRequest_Normal)
            },
             {
                 DLMSCommand.SM110IMAGEFIRSTBLOCKNOTTRANSFERBLOKNUMBER,
                 new DLMSDataStructure("IMAGEFIRSTBLOCKNOTTRANSFERBLOKNUMBER",
                 new byte[]{0xC0, 0x01, 0xC1, 0x00,
                           0x12,    // Class ID
                           0x00, 0x00, 0x2C, 0x00, 0x00, 0xFF, 0x04, 0x00},// CLID, OBIS, ATTID
                           3000,
                           DataStractureRequest.GetRequest_Normal)
            },
              {
                 DLMSCommand.SM110IMAGEVERIFY,
                 new DLMSDataStructure("IMAGEVERIFY",
                 new byte[]{0xC3, 0x01, 0xC1, 0x00,
                           0x12,    // Class ID
                           0x00, 0x00, 0x2C, 0x00, 0x00, 0xFF, 0x03, 0x01},// CLID, OBIS, ATTID
                           120000,
                           DataStractureRequest.ActionRequest_Normal)
            },
              
              {
                 DLMSCommand.SM110IMAGEACTIVATION,
                 new DLMSDataStructure("IMAGEACTIVATION",
                 new byte[]{0xC3, 0x01, 0xC1, 0x00,
                           0x12,    // Class ID
                           0x00, 0x00, 0x2C, 0x00, 0x00, 0xFF, 0x04, 0x01},// CLID, OBIS, ATTID
                           120000,
                           DataStractureRequest.ActionRequest_Normal)
            },
              {
                 DLMSCommand.SM110IMAGESTATUS,
                 new DLMSDataStructure("IMAGESTATUS",
                 new byte[]{0xC0, 0x01, 0xC1, 0x00,
                           0x12,    // Class ID
                           0x00, 0x00, 0x2C, 0x00, 0x00, 0xFF, 0x06, 0x00},// CLID, OBIS, ATTID
                           5000,
                           DataStractureRequest.GetRequest_Normal)
            },

        };

        public static Dictionary<DLMSCommand, DLMSDataStructure> GlobalSM310MeterObject = new Dictionary<DLMSCommand, DLMSDataStructure>
        {
            // Command Name              // Request Type Class ID OBIS Att
            {
                DLMSCommand.SM310INIT,
                new DLMSDataStructure("INIT",
                new byte[]{0xC1, 0x01, 0xC1, 0x00, 0x01, 0x00, 0x00, 0x60, 0x01, 0x91, 0xFF, 0x02 }, 3000, DataStractureRequest.SetRequest_Normal)
            },

            // Command Name              // Request Type Class ID OBIS Att
            {
                DLMSCommand.SM310GETSAMPLECALIBRATE,
                new DLMSDataStructure("GETSAMPLE",
                new byte[]{0xC0, 0x01, 0xC1, 0x00, 0x01, 0x00, 0x00, 0x60, 0x02, 0x84, 0xFF, 0x02 }, 3000, DataStractureRequest.GetRequest_Normal)
            },

            // Command Name              // Request Type Class ID OBIS Att
            {
                DLMSCommand.SM310SETCALIBRATE,
                new DLMSDataStructure("SETCOEFF",
                new byte[]{0xC1, 0x01, 0xC1, 0x00, 0x01, 0x00, 0x00, 0x60, 0x02, 0x88, 0xFF, 0x02 }, 3000, DataStractureRequest.SetRequest_Normal)
            },

             // Command Name              // Request Type Class ID OBIS Att
            {
                DLMSCommand.DATARESET,
                new DLMSDataStructure("DATARESET",
                new byte[]{0xC3, 0x01, 0xC1, 0x00, 0x09, 0x00, 0x01, 0x0A, 0x08, 0x00, 0xFF, 0x01 }, 3000, DataStractureRequest.SetRequest_Normal)
            },

             // Command Name              // Request Type Class ID OBIS Att
            {
                DLMSCommand.SM310VERIFYNEUTRAL,
                new DLMSDataStructure("NEUTRALVERIFY",
                new byte[]{0xC0, 0x01, 0xC1, 0x00, 0x03, 0x01, 0x00, 0x5b, 0x07, 0x00, 0xFF, 0x02 }, 3000, DataStractureRequest.GetRequest_Normal)
            },
           // 0x01,0x00,5B,0x07,0x00,0xFF
           {
                 DLMSCommand.SM310CALIBVERIFY,
                 new DLMSDataStructure("SM310CALIBVERIFY",
                 new byte[]{0xC0, 0x01, 0xC1, 0x00,
                           0x01,    // Class ID
                           0x00, 0x00, 0x60, 0x02, 0x88, 0xFF, 0x02},// CLID, OBIS, ATTID
                           3000,
                           DataStractureRequest.ActionRequest_Normal)
            },

        };

        public static Dictionary<DLMSCommand, DLMSDataStructure> GlobalNonAMI1PMeterObject = new Dictionary<DLMSCommand, DLMSDataStructure>
        {
          
            // Command Name              // Request Type Class ID OBIS Att
            {
                DLMSCommand.NONAMI1PCALIB,
                new DLMSDataStructure("GETSAMPLE",
                new byte[]{0xC1, 0x01, 0xC1, 0x00, 0x01, 0x00, 0x00, 0x60, 0x02, 0x84, 0xFF, 0x02 }, 3000, DataStractureRequest.SetRequest_Normal)
            },
            {
                DLMSCommand.NONAMI1PCALCURRENTN,
                new DLMSDataStructure("GETSAMPLE",
                new byte[]{0xC0, 0x01, 0xC1,  0x00, 0x03, 0x01, 0x00, 0x5b, 0x07, 0x00, 0xFF, 0x02 }, 3000, DataStractureRequest.GetRequest_Normal)
            },
            {
                DLMSCommand.NONAMI1PCALCURRENTPH,
                new DLMSDataStructure("GETSAMPLE",
                new byte[]{0xC0, 0x01, 0xC1,  0x00, 0x03, 0x01, 0x00, 0x0b, 0x07, 0x00, 0xFF, 0x02 }, 3000, DataStractureRequest.GetRequest_Normal)
            },

             {
                DLMSCommand.WRITEAES,
                new DLMSDataStructure("GETSAMPLE",//02 00 02 02 12
                new byte[]{0xC1, 0x01, 0xC1, 0x00, 0x01, 0x00, 0x00, 0x60, 0x02, 0x9B, 0xFF, 0x02, 00, 02, 0x02, 0x12 }, 3000, DataStractureRequest.SetRequest_Normal)
            },



        };

        public static Dictionary<DLMSCommand, DLMSDataStructure> GlobalSPS2MeterObject = new Dictionary<DLMSCommand, DLMSDataStructure>
        {
            // Command Name              // Request Type Class ID OBIS Att
            {
                DLMSCommand.SPS2SET,
                new DLMSDataStructure("SET",
                new byte[]{0xC1, 0x01, 0xC1, 0x00, 0x01, 0x00, 0x00, 0x60, 0x02, 0x84, 0xFF, 0x02 }, 3000, DataStractureRequest.SetRequest_Normal)
            },

            {
                DLMSCommand.SPS2CALIBVERIFY,
                new DLMSDataStructure("GET",
                new byte[]{0xC0, 0x01, 0xC1, 0x00, 0x01, 0x00, 0x00, 0x60, 0x02, 0x88, 0xFF, 0x02, 0x00 }, 3000, DataStractureRequest.GetRequest_Normal)
            },


        };

        public static Dictionary<MeterType, IErrorCode> ErrorObject = new Dictionary<MeterType, IErrorCode>()
        {
            { MeterType.SM110, new SM110Error()},
            { MeterType.SM310, new SM310Error()},
        };

    }
}
