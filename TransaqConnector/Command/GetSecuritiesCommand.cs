using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Command
{
   internal class GetSecuritiesCommand:TXmlCommand
    {
        public GetSecuritiesCommand()
            : base()
        {
            ID = "get_securities";
        }
    }
}
