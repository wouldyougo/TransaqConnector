using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Inner
{
   internal class CallbackEventArgs:EventArgs
    {
        public CallbackEventArgs(String str)
        {
            CallbackResult = str;
        }

        public String CallbackResult
        {
            get;
            private set;
        }
    }
}
