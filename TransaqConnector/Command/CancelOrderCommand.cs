using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Command
{
    internal class CancelOrderCommand:TXmlCommand
    {
        public CancelOrderCommand()
        {
            ID = "cancelorder";
        }

        public String TransactionId
        {
            get;
            set;
        }

        public override string ToXmlString()
        {
            return base.GetXmlBegin() + "<transactionid>" + TransactionId +
                "</transactionid>" + base.GetXmlEnd();
        }
    }
}
