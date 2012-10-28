using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Inner
{
    internal enum SecurityKind
    {
        SHARE,
        BOND,
        FUT,
        OPT,
        GKO,
        FOB,

        IDX,
        QUOTES,
        CURRENCY,
        ADR,
        NYSE,
        METAL,
        OIL,
        ERROR
    }
}
