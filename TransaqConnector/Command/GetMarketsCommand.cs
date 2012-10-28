using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Command
{
   internal class GetMarketsCommand:TXmlCommand
    {
        public GetMarketsCommand()
        {
            ID = "get_markets";
        }
    }
}
