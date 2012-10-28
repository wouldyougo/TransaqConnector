using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Command
{
    internal class UnsubscribeCommand:SubscribeCommand
    {
        public UnsubscribeCommand()
            : base()
        {
            ID = "unsubscribe";
        }
    }
}
