using System;
using System.Text;

namespace StockSharp.Transaq.Command
{
	using StockSharp.Transaq.Inner;

	internal class NewCondOrderCommand : NewOrderBaseCommand
    {
        public NewCondOrderCommand()
        {
            ID = "newcondorder";
            IsValidBeforeCancelled = false;
        }

        public TransaqStopConditionKinds OrderCondition
        {
            get;
            set;
        }

        public double? CondPrice
        {
            get;
            set;
        }

        public DateTime? CondTime
        {
            get;
            set;
        }

        public DateTime? ValidAfter
        {
            get;
            set;
        }

        public bool IsValidBeforeCancelled
        {
            get;
            set;
        }

        public DateTime? ValidBefore
        {
            get;
            set;
        }

        public override string ToXmlString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("<cond_type>{0}</cond_type>",OrderCondition));
            String condValue=String.Empty;
            if(OrderCondition==TransaqStopConditionKinds.Time)
            {
                if(CondTime!=null)
                condValue=((DateTime)CondTime).ToString(Formats.DateTimeFormat);
            }
            else
                if(CondPrice!=null) 
                    condValue=((double)CondPrice).ToString(Formats.NumericProvider);
            sb.Append(String.Format("<cond_value>{0}</cond_value>",condValue));
            String validAfter;
            if(ValidAfter==null) validAfter="0";
            else validAfter=((DateTime)ValidAfter).ToString(DateTimeFormat);
            sb.Append("<validafter>"+validAfter+"</validafter>");
            String validBefore;
            if (IsValidBeforeCancelled) validBefore = "till_cancelled";
            else
                if (ValidBefore == null) validBefore = "0";
                else
                    validBefore = ((DateTime)ValidBefore).ToString(DateTimeFormat);
            sb.Append("<validbefore>" + validBefore + "</validbefore>");
            return base.ToXmlString(sb.ToString());
        }
    }

    public enum TransaqStopConditionKinds
    {
        None,
        //= лучшая цена покупки
        Bid,
        // = лучшая цена покупки или сделка по заданной цене и выше
        BidOrLast,
        //= лучшая цена продажи
        Ask,
        //= лучшая цена продажи или сделка по заданной цене и ниже
        AskOrLast,
        //= время выставления заявки на Биржу
        Time,
        //= обеспеченность ниже заданной
        CovDown,
        //= обеспеченность выше заданной
        CovUp,
        //= сделка на рынке по заданной цене или выше
        LastUp,
        // = сделка на рынке по заданной цене или ниже
        LastDown
    }
}
