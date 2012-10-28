using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Command
{
    internal class NewOrderBaseCommand : TXmlCommand
    {
        public NewOrderBaseCommand()
        {
            ID = "neworder";
            ByMarket = false;
        }


        public String SecID
        {
            get;
            set;
        }

        public String Client
        {
            get;
            set;
        }

        public decimal Price
        {
            get;
            set;
        }

        public int LotsNumber
        {
            get;
            set;
        }

        public TradeOperation Operation
        {
            get;
            set;
        }

        public bool ByMarket
        {
            get;
            set;
        }

        public bool UseCredit
        {
            get;
            set;
        }

        public bool NoSplit
        {
            get;
            set;
        }

        /// <summary>
        /// Примечание
        /// </summary>
        public String Reference
        {
            get;
            set;

        }

        public String ToXmlString(String body)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.GetXmlBegin());
            sb.Append(String.Format("<secid>{0}</secid>", SecID));
            sb.Append(String.Format("<client>{0}</client>", Client));
            sb.Append(String.Format("<price>{0}</price>", Price));
            sb.Append(String.Format("<quantity>{0}</quantity>", LotsNumber));
            String oper = "B";
            if (Operation == TradeOperation.S)
                oper = "S";
            sb.Append(String.Format("<buysell>{0}</buysell>", oper));
            if (ByMarket) sb.Append("<bymarket/>");
            if (Reference != null) sb.Append(String.Format("<brokerref>{0}</brokerref>", Reference));
            sb.Append(body);
            if (UseCredit) sb.Append("<usecredit/>");
            if (NoSplit) sb.Append("<nosplit/>");
            sb.Append(base.GetXmlEnd());
            return sb.ToString();
        }

        public override string ToXmlString()
        {
            return base.ToXmlString();
        }
    }
}