using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Command
{
    internal class ServerStatusCommand:TXmlCommand
    {
        public ServerStatusCommand()
            : base()
        {
            ID = "server_status";
        }
    }
}
