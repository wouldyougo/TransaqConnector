using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Command
{
    internal  class  MakeOrDownCommand:CancelOrderCommand
    {
        public MakeOrDownCommand()
        {
            ID = "makeordown";
        }
    }
}
