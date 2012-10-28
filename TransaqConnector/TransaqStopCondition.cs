using System;
using StockSharp.BusinessEntities;

namespace StockSharp.Transaq
{
	using StockSharp.Transaq.Command;

	public class TransaqStopConditions :StopCondition
    {
        public TransaqStopConditions() 
        {
            IsValidBeforeCancelled = false;
        }

        public TransaqStopConditionKinds StopConditionKind
        {
            get;
            set;
        }

        public double? ConditionPrice
        {
            get;
            set;
        }

        public DateTime? ConditionTime
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

        public override StopCondition Clone()
        {
            return new TransaqStopConditions()
            {
                ConditionPrice = this.ConditionPrice,
                ConditionTime = this.ConditionTime,
                IsValidBeforeCancelled = this.IsValidBeforeCancelled,
                StopConditionKind = this.StopConditionKind,
                ValidAfter = this.ValidAfter,
                ValidBefore = this.ValidBefore
            };
        }
    }
}
