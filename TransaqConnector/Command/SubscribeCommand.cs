using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Command
{
    internal class SubscribeCommand:TXmlCommand
    {
        private List<String> _allTrades = new List<string>();
        private List<String> _quotations = new List<string>();
        private List<String> _quotes = new List<string>();

        public SubscribeCommand()
            : base()
        {
            ID = "subscribe";
        }

        public IList<String> AllTrades
        {
            get
            {
                return _allTrades;
            }
        }

        public IList<String> Quotations
        {
            get
            {
                return _quotations;
            }
        }

        public IList<String> Quotes
        {
            get
            {
                return _quotes;
            }
        }

        public override string ToXmlString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(base.GetXmlBegin());
            int i;
            result.Append("<alltrades>");
            for(i=0;i<AllTrades.Count;i++)
            {
                result.Append(String.Format("<secid>{0}</secid>",AllTrades[i]));
            }
            result.Append( "</alltrades>");

            result.Append("<quotations>");
            for (i = 0; i < Quotations.Count; i++)
            {
                result.Append(String.Format("<secid>{0}</secid>", Quotations[i]));
            }
            result.Append("</quotations>");

            result.Append("<quotes>");
            for (i = 0; i < Quotes.Count; i++)
            {
                result.Append( String.Format("<secid>{0}</secid>", Quotes[i]));
            }
            result.Append("</quotes>");

            result.Append( base.GetXmlEnd());
            return result.ToString();
        }
    }
}