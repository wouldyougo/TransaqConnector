namespace StockSharp.Transaq
{
	using System;
	using System.Collections.Generic;

	using StockSharp.BusinessEntities;
	using StockSharp.Algo.Candles;
	using StockSharp.Transaq.Callback;
	using StockSharp.Transaq.Command;
	using StockSharp.Transaq.Inner;

	public partial class TransaqTrader
    {
        static class TransaqTraderHelper
        {


            public static Security GetMockSecurity(string secID)
            {
                throw new NotImplementedException();
            }

            public static UnfilledAction ToUnfilledAction(OrderExecutionConditions cond)
            {
                switch (cond)
                {
                    case OrderExecutionConditions.CancelBalance:
                        return UnfilledAction.CancelBalance;

                    case OrderExecutionConditions.MatchOrCancel:
                        return UnfilledAction.ImmOrCancel;
                    case OrderExecutionConditions.PutInQueue:
                        return UnfilledAction.PutInQueue;
                    default: throw new ArgumentOutOfRangeException();


                }
            }

            public static TradeOperation ToTradeOperation(OrderDirections dir)
            {
                switch (dir)
                {
                    case OrderDirections.Buy:
                        return TradeOperation.B;
                    case OrderDirections.Sell:
                        return TradeOperation.S;
                    default: throw new ArgumentOutOfRangeException();

                }
            }

            public static OrderDirections ToOrderDirections(TradeOperation op)
            {
                switch (op)
                {
                    case TradeOperation.B:
                        return OrderDirections.Buy;
                    case TradeOperation.S:
                        return OrderDirections.Sell;
                    default: throw new ArgumentOutOfRangeException();
                }
            }

            public static Quote ToQuote(TransaqQuote tquote, MarketDepth md)
            {
                return new Quote()
                 {
                     OrderDirection = tquote.Buy != -1 && tquote.Buy != null ? OrderDirections.Buy : OrderDirections.Sell,
                     Price = tquote.Price != null ? (decimal)tquote.Price : 0,
                     Security = md.Security,
                     Volume = tquote.Buy != -1 && tquote.Buy != null ? (int)tquote.Buy : (int)tquote.Sell,
                 };
            }

            public static NewOrderBaseCommand ToNewOrderBaseCommand(Order order, TransaqTrader trader)
            {

                if (order.Type != OrderTypes.Conditional)
                {
                    return new NewOrderCommand()
                    {
                        ByMarket = order.Type == OrderTypes.Market,
                        Client = order.Portfolio != null ? order.Portfolio.Name : String.Empty,
                        LotsNumber = (int) order.Volume,
                        UnfilledAction = TransaqTraderHelper.ToUnfilledAction(order.ExecutionCondition),
                        Operation = TransaqTraderHelper.ToTradeOperation(order.Direction),
                        Price = order.Price,
                        SecID = order.Security.Id,
                        Reference = order.Comment

                    };
                }
                else
                {
                    NewCondOrderCommand result =
                     new NewCondOrderCommand()
                    {
                        Reference = order.Comment,
                        ByMarket = order.Type == OrderTypes.Market,
                        Client = order.Portfolio != null ? order.Portfolio.Name : String.Empty,
                        LotsNumber = (int) order.Volume,
                        Operation = TransaqTraderHelper.ToTradeOperation(order.Direction),
                        Price = order.Price,
                        SecID = order.Security.Id,


                    };

                    TransaqStopConditions cond = order.StopCondition as TransaqStopConditions;
                    if (cond != null)
                    {
                        result.CondPrice = cond.ConditionPrice;
                        result.CondTime = cond.ConditionTime;
                        result.IsValidBeforeCancelled = cond.IsValidBeforeCancelled;
                        result.OrderCondition = cond.StopConditionKind;
                        result.ValidAfter = cond.ValidAfter;
                        result.ValidBefore = cond.ValidBefore;
                    }
                    return result;
                }
            }

            public static void CopyData(Security tsec, TransaqSecurity sec)
            {
                if (tsec != null && sec != null)
                {
                    tsec.Name = sec.ShortName;
                    tsec.Code = sec.Code;
                    tsec.Class = sec.Type.ToString();
                    tsec.Decimals = sec.Decimals;
                    if (sec.LotSize != null)
                        tsec.MinLotSize = (int)sec.LotSize;
                    if (sec.MinStep != null) tsec.MinStepSize = (decimal)sec.MinStep;
                    tsec.ShortName = sec.ShortName;
                    if (sec.Active) tsec.State = SecurityStates.Trading;
                    else
                        tsec.State = SecurityStates.Stoped;
                    switch (sec.Type)
                    {
                        case SecurityKind.FUT:
                            tsec.Type = SecurityTypes.Future;
                            break;
                        case SecurityKind.FOB:
                            tsec.Type = SecurityTypes.Future;
                            break;
                        case SecurityKind.OPT:
                            tsec.Type = SecurityTypes.Option;
                            break;
                        default:
                            tsec.Type = SecurityTypes.Equity;
                            break;
                    }

                }
            }

            public static void UpdateMarketDepth(MarketDepth md, IList<TransaqQuote> quotes)
            {
                TransaqQuote q = null;
                Quote newQuote = null;
                decimal price;
                int i;

                Dictionary<decimal, Quote> buy = new Dictionary<decimal, Quote>();
                Dictionary<decimal, Quote> sell = new Dictionary<decimal, Quote>();
                Quote[] buyArray = md.GetQuotes(OrderDirections.Buy);
                Quote[] sellArray = md.GetQuotes(OrderDirections.Sell);

                for (i = 0; i < buyArray.Length; i++)
                {
                    buy.Add(buyArray[i].Price, buyArray[i]);
                }
                for (i = 0; i < sellArray.Length; i++)
                {
                    sell.Add(sellArray[i].Price, sellArray[i]);
                }
                for (i = 0; i < quotes.Count; i++)
                {
                    q = quotes[i];
                    if (q.Price == null) continue;
                    price = (decimal)q.Price;
                    if (q.Sell == -1d && q.Buy == -1d)
                    {
                        if (sell.ContainsKey(price))
                            sell.Remove(price);
                        if (buy.ContainsKey(price))
                            buy.Remove(price);
                    }
                    else
                    {
                        newQuote = ToQuote(quotes[i], md);
                        if (newQuote.OrderDirection == OrderDirections.Sell)
                            if (sell.ContainsKey(price))
                                sell[price] = newQuote;
                            else
                                sell.Add(price, newQuote);
                        else
                            if (buy.ContainsKey(price))
                                buy[price] = newQuote;
                            else
                                buy.Add(price, newQuote);
                    }
                }
                md.Update(sell.Values, buy.Values);
                //md.FinishUpdate();
            }

            public static void CopyData(Security tsec, Quotation qt)
            {
                
                if (qt.Close != null)
                    tsec.ClosePrice = (decimal)qt.Close;
                if (qt.Open != null)
                    tsec.OpenPrice = (decimal)qt.Open;
                if (qt.High != null)
                    tsec.HighPrice = (decimal)qt.High;
                if (qt.Low != null)
                    tsec.LowPrice = (decimal)qt.Low;
                if (tsec.BestAsk == null)
                    tsec.BestAsk = new Quote { Security = tsec };
               if (tsec.BestBid == null)
                    tsec.BestBid = new Quote { Security = tsec };
                if (qt.Bid != null)
                    tsec.BestBid.Price = (decimal)qt.Bid;
                if (qt.BidsNumber != null)
                    tsec.BestBid.Volume = (int)qt.BidsNumber;
                if (qt.Offer != null)
                    tsec.BestAsk.Price = (decimal)qt.Offer;
                if (qt.OffersNumber != null)
                    tsec.BestAsk.Volume = (int)qt.OffersNumber;
                if (qt.Status != null)
                    tsec.State = qt.Status == QuotationStatusKind.S ? SecurityStates.Stoped : SecurityStates.Trading;
                if (tsec.LastTrade == null) tsec.LastTrade = new Trade { Security = tsec };
                if (qt.Last != null) tsec.LastTrade.Price = (decimal)qt.Last;
                if (qt.Quantity != null) tsec.LastTrade.Volume = (int)qt.Quantity;
                if (qt.Time != null) tsec.LastTrade.Time = (DateTime)qt.Time;


            }

            public static void CopyData(Position tpos, SecurityPosition pos)
            {
                if (pos.SaldoIn != null)
                    tpos.BeginValue = (int)pos.SaldoIn;
                if (pos.Saldo != null)
                    tpos.CurrentValue = (int)pos.Saldo;
                if (pos.SaldoMin != null)
                    tpos.BlockedValue = (int)pos.SaldoMin;
            }

            public static void CopyData(Portfolio portfolio, MoneyPosition pos)
            {
                if (pos.SaldoIn != null)
                    portfolio.BeginValue = (decimal)pos.SaldoIn;
                if (pos.Comission != null)
                    portfolio.Commission = (decimal)pos.Comission;
                if (pos.Saldo != null)
                    portfolio.CurrentValue = (decimal)pos.Saldo;
            }

            public static void CopyData(Order torder, TransaqOrder order)
            {
                if (order.Balance != null)
                    torder.Balance = (int)order.Balance;
                if (order.Time != null)
                    torder.Time = (DateTime)order.Time;
                if (order.WithDrawTime != null)
                    torder.ExpiryDate = order.WithDrawTime;
                if (order.Reference != null)
                    torder.Comment = order.Reference;
                if (order.Operation != null)
                    torder.Direction = order.Operation == TradeOperation.B ? OrderDirections.Buy : OrderDirections.Sell;
                if (order.Number != null)
                    torder.Id = (long)order.Number;
                if (order.Result != null)
                    torder.Messages.Add(order.Result);
                if (order.Price != null)
                    torder.Price = (decimal)order.Price;
                if (order.Status != null)
                {
                    var status = (OrderStatusKind)order.Status;
                    CopyOrderState(torder, status);
                    CopyOrderStatus(torder, status);
                }
                if (order.Quantity != null)
                    torder.Volume = (int)order.Quantity;
                if (order.Condition != null && order.Condition != TransaqStopConditionKinds.None && torder.StopCondition == null)
                {
                	var stopCond = new TransaqStopConditions
                	               {
                	               	ValidBefore = order.ValidBeforeAsDateTime,
                	               	ValidAfter = order.ValidAfterAsDateTime,
                	               	ConditionPrice = order.ConditionValueAsDouble,
                	               	ConditionTime = order.ConditionValueAsDateTime
                	               };
                }
                if (order.Operation != null)
                    torder.Direction = ToOrderDirections((TradeOperation)order.Operation);



            }

            public static void CopyData(Trade ttrade, ClientTrade trade)
            {
                ttrade.Id = trade.Number;
                ttrade.OrderDirection = ToOrderDirections(trade.Operation);
                ttrade.Price = trade.Price;
                ttrade.Time = trade.Time;
                ttrade.Volume = trade.Quantity;
            }

            public static void CopyData(Portfolio portfolio, ClientResponse clientAcc)
            {
                portfolio.Name = clientAcc.ID;
                if (clientAcc.Currency != null)
                {
                    switch (clientAcc.Currency)
                    {
                        case CurrencyKind.EUR:
                            portfolio.Currency = CurrencyTypes.EUR;
                            break;
                        case CurrencyKind.RUB:
                            portfolio.Currency = CurrencyTypes.RUB;
                            break;
                        case CurrencyKind.USD:
                            portfolio.Currency = CurrencyTypes.USD;
                            break;

                    }
                }
            }

            public static void CopyData(Candle tcandle, TransaqCandle candle)
            {
                tcandle.ClosePrice = candle.Close;
                tcandle.CloseVolume = candle.Volume;
                tcandle.HighPrice = candle.High;
                tcandle.HighVolume = candle.Volume;
                tcandle.LowPrice = candle.Low;
                tcandle.LowVolume = candle.Volume;
                tcandle.OpenPrice = candle.Open;
                tcandle.OpenVolume = candle.Volume;
                tcandle.TotalVolume = candle.Volume;
                tcandle.OpenTime = candle.Date;
            }

            public static void CopyData(Trade ttrade, TransaqTrade trade)
            {
                ttrade.Id = trade.Number;
                ttrade.OrderDirection = null;
                ttrade.Price = trade.Price;
                ttrade.Time = trade.Time;
                ttrade.Volume = trade.Quantity;

            }

            private static void CopyOrderState(Order order, OrderStatusKind status)
            {
                switch (status)
                {
                    case OrderStatusKind.None:
                        order.State = OrderStates.None;
                        break;

                    case OrderStatusKind.Active:
                        order.State = OrderStates.Active;
                        break;

                    case OrderStatusKind.Wait:
                        order.State = OrderStates.Active;
                        break;

                    case OrderStatusKind.Forwarding:
                        order.State = OrderStates.None;
                        break;

                    case OrderStatusKind.Watching:
                        order.State = OrderStates.Active;
                        break;

                    default:
                        order.State = OrderStates.Done;
                        break;
                }
            }

            private static void CopyOrderStatus(Order torder, OrderStatusKind status)
            {
                switch (status)
                {
                    case OrderStatusKind.Forwarding:
                        torder.Status = OrderStatus.ReceiveByServer;
                        break;
                    case OrderStatusKind.Rejected:
                        torder.Status = OrderStatus.NotDone;
                        break;
                    case OrderStatusKind.Refused:
                        torder.Status = OrderStatus.NotAcceptedByManager;
                        break;
                    case OrderStatusKind.Failed:
                        torder.Status = OrderStatus.GateError;
                        break;
                    case OrderStatusKind.Denied:
                        torder.Status = OrderStatus.NotValidated;
                        break;
                    case OrderStatusKind.Removed:
                        torder.Status = OrderStatus.CanceledByManager;
                        break;
                    case OrderStatusKind.None:
                        break;
                    default:
                        torder.Status = OrderStatus.Accepted;
                        break;
                }
            }
        }
        
    }
}
