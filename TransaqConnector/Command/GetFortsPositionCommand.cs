using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Command
{
    internal class GetFortsPositionCommand:TXmlCommand
    {
        public GetFortsPositionCommand()
        {
            ID = "get_forts_position";
        }

        public String Client
        {
            get;
            set;
        }

        public override string ToXmlString()
        {
            return String.Format("<command id=\"{0}\" client=\"{1}\"/>",ID,Client);
        }
    }
}
