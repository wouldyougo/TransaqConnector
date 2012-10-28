using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;

using Ecng.Collections;

using StockSharp.BusinessEntities;
using StockSharp.Algo;
using StockSharp.Algo.Candles;

namespace StockSharp.Transaq
{
	using StockSharp.Transaq.Callback;
	using StockSharp.Transaq.Command;
	using StockSharp.Transaq.Inner;

	public partial class TransaqTrader : BaseTrader
    {
        private const string _invalidPeriodString = "Not valid candle period.";
        
        private readonly SynchronizedDictionary<string, Exchange> _exchanges = new SynchronizedDictionary<string,Exchange>();
        private readonly SynchronizedDictionary<string, TimeSpan> _candlePeriods = new SynchronizedDictionary<string,TimeSpan>();
        
        //SynchronizedDictionary<string, string> _exchangesNameIndex;
        //SynchronizedDictionary<TimeSpan, string> _candlePeriodsIndex;

		private readonly SynchronizedList<ClientTrade> _tradesnoorders;
		private readonly Dictionary<string, CandleToken> _tokens;
		private readonly Timer _checkConnectionTimer = new Timer();
		private readonly Timer _checkServerTimeTimer = new Timer();
		private readonly object _tradesNoOrdersSync = new object();

        public TransaqTrader()
        {
            _tradesnoorders = new SynchronizedList<ClientTrade>();
            //_exchanges = new SynchronizedDictionary<string, Exchange>();
            //_exchangesNameIndex = new SynchronizedDictionary<string, string>();
            //_candlePeriods = new SynchronizedDictionary<string, TimeSpan>();
            //_candlePeriodsIndex = new SynchronizedDictionary<TimeSpan, string>();
            _tokens = new Dictionary<string, CandleToken>();
            _checkConnectionTimer.AutoReset = true;
            _checkConnectionTimer.Elapsed += _checkConnectionTimer_Elapsed;
            
            _checkServerTimeTimer.Interval = TimeSpan.FromMinutes(5).TotalMilliseconds;
            _checkServerTimeTimer.AutoReset = true;
            _checkServerTimeTimer.Elapsed += _checkServerTimeTimer_Elapsed;
            _checkConnectionTimer.Interval = TimeSpan.FromMinutes(1).TotalMilliseconds;


            TransaqConnector.Instance.UnrecognizedResponseHandler += Instance_UnrecognizedResponseHandler;
            TransaqConnector.Instance.SendingCommandErrorHandler += Instance_SendingCommandErrorHandler;
            TransaqConnector.Instance.ErrorResponseHandler += Instance_ErrorResponseHandler;

            TransaqConnector.Instance.ServerStatusResponseHandler += Instance_ServerStatusResponseHandler;
            TransaqConnector.Instance.MarketsResponseHandler += Instance_MarketsResponseHandler;
            TransaqConnector.Instance.CandleKindsResponseHandler += Instance_CandleKindsResponseHandler;
            TransaqConnector.Instance.SecuritiesResponseHandler += Instance_SecuritiesResponseHandler;
            TransaqConnector.Instance.ClientResponseHandler += Instance_ClientResponseHandler;
            TransaqConnector.Instance.PositionsResponseHandler += Instance_PositionsResponseHandler;
            TransaqConnector.Instance.OvernightResponseHandler += Instance_OvernightResponseHandler;
            TransaqConnector.Instance.AllTradesResponseHandler += Instance_AllTradesResponseHandler;

			TransaqConnector.Instance.CandlesCallbackHandler += Instance_CandlesCallbackHandler;
            TransaqConnector.Instance.ClientTradesCallbackHandler += Instance_ClientTradesCallbackHandler;
            TransaqConnector.Instance.LeverageControlCallbackHandler += Instance_LeverageControlCallbackHandler;
            TransaqConnector.Instance.MarketOrderPermitCallbackHandler += Instance_MarketOrderPermitCallbackHandler;
            TransaqConnector.Instance.OrdersCallbackHandler += Instance_OrdersCallbackHandler;
            TransaqConnector.Instance.QuotationsCallbackHandler += Instance_QuotationsCallbackHandler;
            TransaqConnector.Instance.QuotesCallbackHandler += Instance_QuotesCallbackHandler;


            TransaqConnector.Instance.InitializeComplete();
        }




       
        #region Public Properties, Fields

        public event Action<CandleToken, TimeFrameCandle> NewHistoryCandle;


        public String Password
        {
            get;
            set;
        }

        public String Login
        {
            get;
            set;
        }

        public String Host
        {
            get;
            set;
        }

        public String Port
        {
            get;
            set;
        }

        public Proxy Proxy
        {
            get;
            set;
        }

       /* public String ClientCode
        {
            get;
            set;
        }
        */
        public TimeSpan CheckConnectionInterval
        {
            get
            {
                return TimeSpan.FromMilliseconds(_checkConnectionTimer.Interval);
            }
            set
            {
                _checkConnectionTimer.Interval = value.TotalMilliseconds;
            }
        }

        /// <summary>
        /// Список доступных торговых площадок.
        /// </summary>
        public IEnumerable<Exchange> Exchanges
        {
            get
            {
                return _exchanges.Values;
            }
        }

        /// <summary>
        /// Список доступных периодов свечей.
        /// </summary>
        public IEnumerable<TimeSpan> CandleTimeFrames
        {
            get
            {
                return _candlePeriods.Values;
            }
        }

    
        #endregion

        #region Protected Methods

        protected override void OnConnect()
        {
            _candlePeriods.Clear();
            //_candlePeriodsIndex.Clear();

            CommandResult result = TransaqConnector.Instance.SendCommand(new ConnectCommand()
            {
                Login = this.Login,
                Password = this.Password,
                Proxy = this.Proxy,
                Port = this.Port,
                Host = this.Host
            });
        }

        protected virtual void OnConnected()
        {
            RaiseConnected();
        }


        protected override void OnDisconnect()
        {
            if (IsConnected)
            {
                CommandResult result = TransaqConnector.Instance.SendCommand(new DisconnectCommand());
                if (!result.Success)
                    RaiseConnectionError(new Exception(result.Message));
                else
                {
                    _checkConnectionTimer.Enabled = false;
                    _checkServerTimeTimer.Enabled = false;
                    // CheckConnection();
                }
            }
        }

        protected virtual void OnDisconnected()
        {
            RaiseDisconnected();
        }

        protected override void OnCancelOrder(Order order)
        {
            CommandResult result = TransaqConnector.Instance.SendCommand(new CancelOrderCommand()
            {
                TransactionId = order.TransactionId.ToString()
            });

            ProcessResult(result);
        }


        protected override void OnRegisterOrder(Order order)
        {
            CommandResult result = TransaqConnector.Instance.SendCommand(TransaqTraderHelper.ToNewOrderBaseCommand(order, this));

            long trId;
            if (result.Success)
            {
                if (long.TryParse(result.TransactionId, out trId)) order.TransactionId = trId;
            }
            else
            {
                RaiseOrderFailed(order, new Exception(result.Message));
            }


        }



        protected void RaiseNewHistoryCandle(CandleToken token, TimeFrameCandle timeFrame)
        {
            if (NewHistoryCandle != null) NewHistoryCandle(token, timeFrame);
        }
        #endregion

        #region Public Methods

        public void CheckConnection()
        {
            var command = new ServerStatusCommand();
            ProcessResult(TransaqConnector.Instance.SendCommand(command));
        }

        public void CheckServerTime()
        {
            CommandResult   result = TransaqConnector.Instance.SendCommand(new GetServTimeDifferenceCommand());
            if (result.Success && result.Difference != null)
            {
                MarketTimeOffset = TimeSpan.FromSeconds((int)result.Difference);
            }
        }
        /// <summary>
        /// Не реализован. Для Transaq нет необходимости,
        /// поскольку данные приходят сами после подключения и нет команды для их повторного запроса.
        /// </summary>
        /// <param name="portfolio"></param>
        public void RegisterPortfolio(Portfolio portfolio)
        {

        }

        public void RegisterQuotes(Security security)
        {
            var command = new SubscribeCommand();
            command.Quotes.Add(security.Id);
            ProcessResult(TransaqConnector.Instance.SendCommand(command));

        }

        public  void RegisterSecurity(Security security)
        {
            var command = new SubscribeCommand();
            command.Quotations.Add(security.Id);
            ProcessResult(TransaqConnector.Instance.SendCommand(command));
        }

        public void RegisterTrades(Security security)
        {
            var command = new SubscribeCommand();
            command.AllTrades.Add(security.Id);
            ProcessResult(TransaqConnector.Instance.SendCommand(command));
        }

        /// <summary>
        /// Не реализован. Аналогично RegisterPortfolio.
        /// </summary>
        /// <param name="portfolio"></param>
        public void UnRegisterPortfolio(Portfolio portfolio)
        {

        }

        public  void UnRegisterQuotes(Security security)
        {
            var command = new UnsubscribeCommand();
            command.Quotes.Add(security.Id);
            ProcessResult(TransaqConnector.Instance.SendCommand(command));
        }

        public void UnRegisterSecurity(Security security)
        {
            var command = new UnsubscribeCommand();
            command.Quotations.Add(security.Id);
            ProcessResult(TransaqConnector.Instance.SendCommand(command));
        }

        public void UnRegisterTrades(Security security)
        {
            var command = new UnsubscribeCommand();
            command.AllTrades.Add(security.Id);
            ProcessResult(TransaqConnector.Instance.SendCommand(command));
        }


        public CandleToken RegisterHistoryData(Security sec, TimeSpan timeframe, int amount)
        {

        	var getCandles = new GetHistoryDataCommand {SecID = sec.Id};
        	//if (!_candlePeriodsIndex.ContainsKey(timeframe))
            //{
            //    RaiseProcessDataError(new Exception(_invalidPeriodString));
            //    return null;
            //}

            var pair = _candlePeriods.SingleOrDefault(s => s.Value == timeframe);
            if (pair.Key == null)
            {
                RaiseProcessDataError(new Exception(_invalidPeriodString));
                return null;
            }

            //getCandles.PeriodID = _candlePeriodsIndex.TryGetValue(timeframe);
            getCandles.PeriodID = pair.Key;
            getCandles.Count = amount;
            getCandles.Reset = false;
            CommandResult result = TransaqConnector.Instance.SendCommand(getCandles);
            if (result.Success)
            {
                //return GetCandleToken(sec.Id,_candlePeriodsIndex.TryGetValue(timeframe));
                return GetCandleToken(sec.Id, pair.Key);
            }
            return null;

        }

        #endregion



        #region Responses from Transaq

        #region обработка ошибок
        /// <summary>
        /// неудалось разобрать ответ от сервера
        /// </summary>
        /// <param name="str"></param>
        private void Instance_UnrecognizedResponseHandler(string str)
        {
            RaiseProcessDataError(new Exception(str));
        }

        /// <summary>
        /// ошибка в ответе от сервера
        /// </summary>
        /// <param name="obj"></param>
        private void Instance_SendingCommandErrorHandler(CommandResult obj)
        {
            RaiseProcessDataError(new Exception(obj.Message));
        }

        /// <summary>
        /// сервер вернул сообщение об ошибке
        /// </summary>
        /// <param name="obj"></param>
        private void Instance_ErrorResponseHandler(Response obj)
        {
            //CheckConnection();
            RaiseProcessDataError(new Exception(((ErrorResponse)obj).Error));
        }
        #endregion


        /// <summary>
        /// Изменился статус сервера 
        /// </summary>
        /// <param name="obj">ответ от сервера</param>
        private void Instance_ServerStatusResponseHandler(Response response)
        {
            ServerStatusResponse obj = (ServerStatusResponse)response;
            if (obj.IsError)
            {
                RaiseProcessDataError(new Exception(obj.ErrorMessage));
                return;
            }

            if (IsConnected != obj.IsConnected)
            {
                IsConnected = obj.IsConnected;

                if (IsConnected)
                    OnConnected();
                else
                    OnDisconnected();
            }
        }

        private void Instance_MarketsResponseHandler(Response response)
        {
            MarketsResponse obj = (MarketsResponse)response;

            for (int i = 0; i < obj.Markets.Count; i++)
            {
                Market market = obj.Markets[i];
                Exchange exchange = GetExchangeByName(market);
            }
        }

        private void Instance_CandleKindsResponseHandler(Response response)
        {
            CandleKindsResponse obj = (CandleKindsResponse)response;

            CandleKind kind;
            TimeSpan period;
            for (int i = 0; i < obj.CandleKinds.Length; i++)
            {
                kind = obj.CandleKinds[i];
                if (!_candlePeriods.ContainsKey(kind.ID))
                {
                    period = TimeSpan.FromSeconds(kind.Period);
                    _candlePeriods.Add(kind.ID, period);
                    //_candlePeriodsIndex.Add(period, kind.ID);
                }
            }
        }

        private void Instance_SecuritiesResponseHandler(Response response)
        {
            SecuritiesResponse obj = (SecuritiesResponse)response;

            Action actionAddingSecurities = () =>
            {
                List<Security> securities = new List<Security>();
                for (int i = 0; i < obj.Securities.Length; i++)
                {
                    Security security = TransaqTraderHelper.GetMockSecurity(obj.Securities[i].ID);
                    TransaqTraderHelper.CopyData(security, obj.Securities[i]);
                    security.Exchange = GetExchange(obj.Securities[i].Market);
                    securities.Add(security);
                }

                RaiseNewSecurities(securities);
            };
            
            //if (NewSecurities != null) NewSecurities(newsecurities);
            ProcessEvents(actionAddingSecurities);
        }

        private void Instance_ClientResponseHandler(Response response)
        {
            ClientResponse obj = (ClientResponse)response;
            Action addportfolios = delegate()
            {
                if (!obj.Remove)
                    TransaqTraderHelper.CopyData(GetPortfolio(obj.ID), obj);

            };
            ProcessEvents(addportfolios);
        }

        private void Instance_PositionsResponseHandler(Response response)
        {
            PositionsResponse obj = (PositionsResponse)response;

            Action addPositions = delegate()
            {
                int i;
                for (i = 0; i < obj.FortsCollaterals.Count; i++)
                {

                }

                for (i = 0; i < obj.FortsMoney.Count; i++)
                {
                }

                for (i = 0; i < obj.FortsPositions.Count; i++)
                {
                }

                for (i = 0; i < obj.MoneyPositions.Count; i++)
                {
                    MoneyPosition pos = obj.MoneyPositions[i];
                    TransaqTraderHelper.CopyData(GetPortfolio(pos.Client), pos);
                }

                for (i = 0; i < obj.SecurityPositions.Count; i++)
                {
                    SecurityPosition pos = obj.SecurityPositions[i];
                    TransaqTraderHelper.CopyData(
						GetPosition(GetPortfolio(pos.Client),
									TransaqTraderHelper.GetMockSecurity(pos.SecurityID)),
						pos);
                }

            };

            ProcessEvents(addPositions);
        }

        private void Instance_OvernightResponseHandler(Response response)
        {
            OvernightResponse obj = (OvernightResponse)response;

            #warning Не имплементирован
        }

		private void Instance_AllTradesResponseHandler(Response response)
		{
			AllTradesResponse obj = (AllTradesResponse)response;

			Action newAllTrades = delegate()
			{
				TransaqTrade trade;
				for (int i = 0; i < obj.AllTrades.Length; i++)
				{
					trade = obj.AllTrades[i];
					GetTrade(TransaqTraderHelper.GetMockSecurity(trade.SecurityId),
					   trade.Number,
					   delegate(long id)
					   {
						   Trade t = new Trade() { Id = id };
						   TransaqTraderHelper.CopyData(t, trade);
						   t.Security = TransaqTraderHelper.GetMockSecurity(trade.SecurityId);
						   return t;
					   }
					   );
				}
			};
			ProcessEvents(newAllTrades);
		}










        void _checkServerTimeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CheckServerTime();
        }

        void _checkConnectionTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CheckConnection();
        }





        void Instance_QuotesCallbackHandler(QuotesCallback obj)
        {
            Action addQuotes = delegate()
            {
                TransaqQuote q;
                Dictionary<String, List<TransaqQuote>> quotes = new Dictionary<string, List<TransaqQuote>>();
                for (int i = 0; i < obj.Quotes.Length; i++)
                {
                    q = obj.Quotes[i];
                    if (!quotes.ContainsKey(q.SecurityID)) 
                        quotes.Add(q.SecurityID, new List<TransaqQuote>());
                    quotes[q.SecurityID].Add(q);
                }
                foreach (String key in quotes.Keys)
                {
                    TransaqTraderHelper.UpdateMarketDepth(GetMarketDepth(TransaqTraderHelper.GetMockSecurity(key)), quotes[key]);
				}


			};

            ProcessEvents(addQuotes);
        }

        void Instance_QuotationsCallbackHandler(QuotationsCallback obj)
        {
            Action addQuotations = delegate()
            {
                Quotation quot;
                List<Security> changedSecurity = new List<Security>();
                for (int i = 0; i < obj.Quotations.Length; i++)
                {
                    quot = obj.Quotations[i];
                    TransaqTraderHelper.CopyData(TransaqTraderHelper.GetMockSecurity(quot.SecurityID), quot);
                    changedSecurity.Add(TransaqTraderHelper.GetMockSecurity(quot.SecurityID));
                }
                
                //RaiseSecuritiesChanged(changedSecurity);
            };
            
            ProcessEvents(addQuotations);
        }




        void Instance_OrdersCallbackHandler(OrdersCallback obj)
        {
            Action updateOrders = delegate()
            {
                TransaqOrder order;
                Order torder=null;
                for (int i = 0; i < obj.Orders.Length; i++)
                {
                    order = obj.Orders[i];

                    if (!String.IsNullOrEmpty(order.TransactionId))
                        torder = GetOrderByTransactionId(long.Parse(order.TransactionId));

                    if(torder==null)
                        torder = GetOrder(null,(long)order.Number, id => new Order { Id = id }, null);

                    TransaqTraderHelper.CopyData(torder, order);
                    if (!String.IsNullOrEmpty(order.SecurityID)) torder.Security = TransaqTraderHelper.GetMockSecurity(order.SecurityID);
                    if (!String.IsNullOrEmpty(order.Client)) torder.Portfolio = GetPortfolio(order.Client);
                    if (torder.StopCondition != null && torder.DerivedOrder == null && order.Number != null)
                    {
                    	AddDerivedOrder(null,(long)order.Number, torder, (stopOrder, limitOrder) => stopOrder.DerivedOrder = limitOrder);
                    }

                }
            };
            ProcessEvents(updateOrders);
            ProcessClientTrades(_tradesnoorders);
        }


        /// <summary>
        /// Не имплементирован
        /// </summary>
        /// <param name="obj"></param>
        void Instance_MarketOrderPermitCallbackHandler(MarketOrderPermitCallback obj)
        {

        }

        /// <summary>
        /// Не имплементирован
        /// </summary>
        /// <param name="obj"></param>
        void Instance_LeverageControlCallbackHandler(LeverageControlCallback obj)
        {

        }


        void Instance_ClientTradesCallbackHandler(ClientTradesCallback obj)
        {
            ProcessClientTrades(obj.Trades);
        }




        void Instance_CandlesCallbackHandler(CandlesCallback obj)
        {
            CandleToken token = GetCandleToken(obj.SecurityID, obj.PeriodID);
            List<Candle> candles=new List<Candle>(obj.Candles.Count);
            TimeFrameCandle newCandle;
            for (int i = 0; i < obj.Candles.Count; i++)
            {
                newCandle=new TimeFrameCandle();
                TransaqTraderHelper.CopyData(newCandle,obj.Candles[i]);
                newCandle.Security=TransaqTraderHelper.GetMockSecurity(obj.SecurityID);
                newCandle.TimeFrame=_candlePeriods.TryGetValue(obj.PeriodID);
                candles.Add(newCandle);
                RaiseNewHistoryCandle(token, newCandle);
            }
            
        }



        #endregion

        #region Private methods

        private void ProcessResult(CommandResult result)
        {
            if (!result.Success)
                RaiseProcessDataError(new Exception(result.Message));
        }

        private CandleToken GetCandleToken(String secId, String periodId)
        {
            if(String.IsNullOrEmpty(secId) || String.IsNullOrEmpty(periodId)) 
                return null;
           
            
            string key=secId+"|"+periodId;
            if (_tokens.ContainsKey(key))
            {
                return _tokens[key];
            }
            else
            {
                //CandleToken token = new CandleToken(typeof(TimeFrameCandle), TransaqTraderHelper.GetMockSecurity(secId), _candlePeriods[periodId]);
                var token = new CandleToken(TransaqTraderHelper.GetMockSecurity(secId), _candlePeriods[periodId]);
                _tokens.Add(key,token);
                return token;
            }
        }

        private void RemoveCandleToken(CandleToken token)
        {
            var pair = _candlePeriods.SingleOrDefault(s => s.Value == (TimeSpan)token.Arg);
            //String key = token.Security.Id + "|" + _candlePeriodsIndex.TryGetValue((TimeSpan)token.Arg);
            string key = token.Security.Id + "|" + pair.Key;
            _tokens.Remove(key);
        }

        private void ProcessClientTrades(IList<ClientTrade> trades)
        {
            lock (_tradesNoOrdersSync)
            {
                Action addTrades = delegate
                                   {
                    foreach (var trade in trades)
                    {
                    	ClientTrade trade1 = trade;
                    	AddMyTrade(TransaqTraderHelper.GetMockSecurity(trade.SecurityId),
                                   trade.OrderNumber,0,trade.Number,
                    	           id => new Trade { Id = id },
                    	           myTrade =>
                    	           {
                    	           	TransaqTraderHelper.CopyData(myTrade.Trade, trade1);
                    	           	myTrade.Trade.Security = TransaqTraderHelper.GetMockSecurity(trade1.SecurityId);
                    	           });
                    }
                };
                ProcessEvents(addTrades);
            }
        }
        #endregion


        #region Helpers
        private Exchange GetExchange(string id)
        {
            if (!_exchanges.ContainsKey(id))
                _exchanges.Add(id, new Exchange());

            return _exchanges.TryGetValue(id);
        }

        private Exchange GetExchangeByName(Market market)
        {
            if (!_exchanges.ContainsKey(market.ID))
            {
                Exchange exchange = CreateExchange(market);
                //_exchanges.Add(market.ID, exchange);
                return exchange;
            }

            return _exchanges[market.ID];
        }

        private Exchange CreateExchange(Market market)
        {
            Exchange exchange;

            if (market.Name.ToLower().Trim() == Exchange.Micex.Name.ToLower())
            {
                exchange = Exchange.Micex;
                _exchanges.Add(market.ID, Exchange.Micex);
            }
            else if (market.Name.ToLower().Trim() == Exchange.Rts.Name)
            {
                exchange = Exchange.Rts;
                _exchanges.Add(market.ID, Exchange.Rts);
            }
            else
            {
                exchange = GetExchange(market.ID);
                exchange.Name = market.Name;
            }

            return exchange;
        }
        #endregion
    }
}
