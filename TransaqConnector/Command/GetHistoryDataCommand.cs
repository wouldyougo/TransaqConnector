using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Command
{
    internal class GetHistoryDataCommand:TXmlCommand
    {
        public GetHistoryDataCommand()
        {
            ID = "gethistorydata";
            Reset = true;
        }

        public String SecID
        {
            get;
            set;
        }

        public String PeriodID
        {
            get;
            set;
        }

        public int Count
        {
            get;
            set;
        }

        public bool Reset
        {
            get;
            set;
        }

        public override String ToXmlString()
        {
            return String.Format("<command id=\"{0}\" secid=\"{1}\" period=\"{2}\"" +
            "count=\"{3}\" reset=\"{4}\"/>", ID, SecID, PeriodID, Count, Reset);
        }

    }
}
