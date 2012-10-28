using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Command
{
    internal class DisconnectCommand:TXmlCommand
    {
        public DisconnectCommand()
            : base()
        {
            base.ID = "disconnect";
        }
    }
}
