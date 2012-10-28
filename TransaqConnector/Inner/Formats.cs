using System;
using System.Collections.Generic;

using System.Text;
using System.Globalization;

namespace StockSharp.Transaq.Inner
{
    internal static class Formats
    {
        public static readonly String DateTimeFormat = "dd.mm.yyyy hh:mm:ss";
        public static readonly IFormatProvider NumericProvider = CultureInfo.InvariantCulture.NumberFormat;
        public static readonly IFormatProvider DateTimeProvider = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat;
    }
}
