namespace StockSharp.Transaq
{
    using StockSharp.Algo.Candles;
    using StockSharp.BusinessEntities;

    public class CandleToken : CandleSeries
    {
        public CandleToken(Security security, object timeFrame)
            : base(typeof(TimeFrameCandle), security, timeFrame)
        {
        }
    }
}