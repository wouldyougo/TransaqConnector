using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Command
{
    internal class NewOrderCommand : NewOrderBaseCommand
    {

        public UnfilledAction UnfilledAction
        {
            get;
            set;
        }



        public override string ToXmlString()
        {

            return ToXmlString(String.Format("<unfilled>{0}</unfilled>", UnfilledAction));

        }
    }

    internal enum TradeOperation
    {
        B,
        S
    }

    internal enum UnfilledAction
    {
        PutInQueue,
        CancelBalance,
        ImmOrCancel
    }


}
