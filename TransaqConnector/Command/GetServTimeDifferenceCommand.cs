using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Command
{
    internal class GetServTimeDifferenceCommand:TXmlCommand
    {
        public GetServTimeDifferenceCommand()
        {
            ID = "get_servtime_difference";
        }
    }
}
