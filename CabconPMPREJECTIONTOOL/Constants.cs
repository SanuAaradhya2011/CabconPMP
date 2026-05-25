using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CabconPMPREJECTIONTOOL
{
    public struct MeterSources
    {
        public const string txt_SGS="SGS";
        public const string txt_KAYNES="KAYNES";
    }
    public enum MeterSrc1PHCharEnum
    {
        SGS=4,
        KAYNES=6,
    }
    public enum MeterSrc3PHCharEnum
    {
        SGS = 2,
        KAYNES = 4,
    }
}
