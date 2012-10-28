namespace StockSharp.Transaq.Inner
{
	using System;
	using System.Collections.Generic;

	using StockSharp.Transaq.Callback;
	using StockSharp.Transaq.Command;

	internal class TransaqConnector
    {
        private static TransaqConnector _instance;

        #region служебные события
        
        // событие вызывающееся при каждом ответе от сервера
        public event Action<object> ResponseHandler;
        
        // событие после отправки команды серверу
        public event Action<CommandResult> SendingCommandHandler;
        
        // ошибка при отправке команды
        public event Action<CommandResult> SendingCommandErrorHandler;
        
        // не удалось разобрать ответ от сервера
        public event Action<String> UnrecognizedResponseHandler;
        #endregion

        //public delegate void ResponseHandlerDelagate(Response obj);

        public event Action<Response> ErrorResponseHandler;             // ErrorResponse
        public event Action<Response> ServerStatusResponseHandler;      // ServerStatusResponse
        public event Action<Response> MarketsResponseHandler;           //MarketsResponse
        public event Action<Response> CandleKindsResponseHandler;       //CandleKindsResponse
        public event Action<Response> SecuritiesResponseHandler;        //SecuritiesResponse
        public event Action<Response> ClientResponseHandler;            //ClientResponse
        public event Action<Response> PositionsResponseHandler;         //PositionsResponse
        public event Action<Response> OvernightResponseHandler;         //OvernightResponse
		public event Action<Response> AllTradesResponseHandler;			//AllTradesResponse

        public event Action<CandlesCallback> CandlesCallbackHandler;
        public event Action<QuotationsCallback> QuotationsCallbackHandler;

        public event Action<QuotesCallback> QuotesCallbackHandler;
        public event Action<OrdersCallback> OrdersCallbackHandler;
        public event Action<ClientTradesCallback> ClientTradesCallbackHandler;
        public event Action<MarketOrderPermitCallback> MarketOrderPermitCallbackHandler;
        public event Action<LeverageControlCallback> LeverageControlCallbackHandler;


        private Dictionary<Type, Action<Response>> handlerBunch = new Dictionary<Type, Action<Response>>();



        public static TransaqConnector Instance
        {
            get
            {
                return _instance;
            }
        }

        static TransaqConnector()
        {
            _instance = new TransaqConnector();
        }



        private TransaqConnector()
        {
            Initialize();
        }

        private void Initialize()
        {
            TXMLConnector.Instance.ResponseHandler += new EventHandler<CallbackEventArgs>(Instance_ResponseHandler);
        }


        public void InitializeComplete()
        {
            handlerBunch.Add(typeof(ErrorResponse), ErrorResponseHandler);
            handlerBunch.Add(typeof(ServerStatusResponse), ServerStatusResponseHandler);
            handlerBunch.Add(typeof(MarketsResponse), MarketsResponseHandler);
            handlerBunch.Add(typeof(CandleKindsResponse), CandleKindsResponseHandler);
            handlerBunch.Add(typeof(SecuritiesResponse), SecuritiesResponseHandler);
            handlerBunch.Add(typeof(ClientResponse), ClientResponseHandler);
            handlerBunch.Add(typeof(PositionsResponse), PositionsResponseHandler);
            handlerBunch.Add(typeof(OvernightResponse), OvernightResponseHandler);
			handlerBunch.Add(typeof(AllTradesResponse), AllTradesResponseHandler);
        }

        public CommandResult SendCommand(TXmlCommand command)
        {
            if (command == null)
                return null;

            string resultString = TXMLConnector.Instance.SendCommand(command.ToXmlString());
            return OnSendingCommand(resultString);
        }

        protected void Instance_ResponseHandler(object sender, CallbackEventArgs e)
        {
            if (e == null || string.IsNullOrEmpty(e.CallbackResult))
                throw new ArgumentException("CallbackResult is empty or null;");

            string responseString = e.CallbackResult.Trim();
            string startString = responseString.Substring(1);
            startString = startString.TrimStart();
            startString = startString.Substring(0, startString.IndexOf('>'));
            
            if(startString.Contains(" "))
                startString = startString.Substring(0, startString.IndexOf(' '));

            Response response = null;
            try
            {
                response = Response.Instance(responseString);

                // вызываем общее событие для всех команд
                OnResponse(response);

                // вызываем событие закрепленное за данным ответом сервера
                OnHandlerRedirector(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return;

            switch (startString)
            {
                //case "server_status":
                //    response = Response.Instance<ServerStatusResponse>(responseString);
                //    //ServerStatusResponse response = ServerStatusResponse.FromXmlString(callback);
                //    OnResponse(response);
                //    if (ServerStatusResponseHandler != null)
                //        ServerStatusResponseHandler((ServerStatusResponse)response);
                //    break;

                //case "error":
                //    response = Response.Instance<ErrorResponse>(responseString);
                //    OnResponse(response);
                //    if (ErrorResponseHandler != null)
                //        ErrorResponseHandler((ErrorResponse)response);
                //    break;




                case "candles":
                    CandlesCallback ccallback = CandlesCallback.FromXmlString(responseString);
                    OnResponse(ccallback);
                    if (CandlesCallbackHandler != null) CandlesCallbackHandler(ccallback);
                    break;
                //case "client":
                //    ClientAccountCallback clcallback = ClientAccountCallback.FromXmlString(responseString);
                //    OnResponse(clcallback);
                //    if (ClientAccountCallbackHandler != null) ClientAccountCallbackHandler(clcallback);
                //    break;
                //case "markets":
                //    MarketsCallback mcallback = MarketsCallback.FromXmlString(responseString);
                //    OnResponse(mcallback);
                //    if (MarketsCallbackHandler != null) MarketsCallbackHandler(mcallback);
                //    break;
                //case "candlekinds":
                //    CandleKindsCallback ckcallback = CandleKindsCallback.FromXmlString(responseString);
                //    OnResponse(ckcallback);
                //    if (CandleKindsCallbackHandler != null) CandleKindsCallbackHandler(ckcallback);
                //    break;
                //case "securities":
                //    SecuritiesCallback sccallback = SecuritiesCallback.FromXmlString(responseString);
                //    OnResponse(sccallback);
                //    if (SecuritiesCallbackHandler != null) SecuritiesCallbackHandler(sccallback);
                //    break;
                case "quotations":
                    QuotationsCallback qcallback = QuotationsCallback.FromXmlString(responseString);
                    OnResponse(qcallback);
                    if (QuotationsCallbackHandler != null) QuotationsCallbackHandler(qcallback);
                    break;
				//case "alltrades":
				//    AllTradesCallback acallback = AllTradesCallback.FromXmlString(responseString);
				//    OnResponse(acallback);
				//    if (AllTradesCallbackHandler != null) AllTradesCallbackHandler(acallback);
				//    break;
                case "quotes":
                    QuotesCallback qscallback = QuotesCallback.FromXmlString(responseString);
                    OnResponse(qscallback);
                    if (QuotesCallbackHandler != null) QuotesCallbackHandler(qscallback);
                    break;
                case "orders":
                    OrdersCallback ocallback = OrdersCallback.FromXmlString(responseString);
                    OnResponse(ocallback);
                    if (OrdersCallbackHandler != null) OrdersCallbackHandler(ocallback);
                    break;
                case "trades":
                    ClientTradesCallback ctcallback = ClientTradesCallback.FromXmlString(responseString);
                    OnResponse(ctcallback);
                    if (ClientTradesCallbackHandler != null) ClientTradesCallbackHandler(ctcallback);
                    break;
                //case "positions":
                //    PositionsCallback pcallback = PositionsCallback.FromXmlString(responseString);
                //    OnResponse(pcallback);
                //    if (PositionsCallbackHandler != null) PositionsCallbackHandler(pcallback);
                //    break;
                //case "overnight":
                //    OvernightCallback oncallback = OvernightCallback.FromXmlString(responseString);
                //    OnResponse(oncallback);
                //    if (OvernightCallbackHandler != null) OvernightCallbackHandler(oncallback);
                //    break;
                case "marketord":
                    MarketOrderPermitCallback mopcallback = MarketOrderPermitCallback.FromXmlString(responseString);
                    OnResponse(mopcallback);
                    if (MarketOrderPermitCallbackHandler != null) MarketOrderPermitCallbackHandler(mopcallback);
                    break;
                case "leverage_control":
                    LeverageControlCallback lccallback = LeverageControlCallback.FromXmlString(responseString);
                    OnResponse(lccallback);
                    if (LeverageControlCallbackHandler != null) LeverageControlCallbackHandler(lccallback);
                    break;
                default:
                    OnUnrecognizedCommand(responseString);
                    break;
            }

        }

        /// <summary>
        /// Вызов необходимого хендлера
        /// </summary>
        /// <param name="response">объект ответа сервера</param>
        protected virtual void OnHandlerRedirector(Response response)
        {
            if (!handlerBunch.ContainsKey(response.GetType()))
                throw new ArgumentException("response");


            Action<Response> handler = handlerBunch[response.GetType()];
            if (handler != null)
                handler(response);
        }



        /// <summary>
        /// результат отправки команды серверу
        /// </summary>
        /// <param name="serverResponse">ответ сервера о приеме команды</param>
        protected virtual CommandResult OnSendingCommand(string serverResponse)
        {
            CommandResult result = CommandResult.FromXmlString(serverResponse);
            if (result == null)
            {
                OnUnrecognizedCommand(serverResponse);
                return null;
            }

            if (SendingCommandHandler != null)
                SendingCommandHandler(result);
            
            if (!result.Success)
            {
                if (SendingCommandErrorHandler != null)
                    SendingCommandErrorHandler(result);
            }

            return result;
        }


        /// <summary>
        /// обработка сообщения от сервера
        /// </summary>
        /// <param name="response"></param>
        protected virtual void OnResponse(object response)
        {
#warning Сменить тип параметра на Response
            if (ResponseHandler != null)
                ResponseHandler(response);
        }


        /// <summary>
        /// Обработка ситуации в случае если команда не распознана
        /// </summary>
        /// <param name="serverResponse">ответ сервера о приеме команды</param>
        protected void OnUnrecognizedCommand(string serverResponse)
        {
            if (UnrecognizedResponseHandler != null)
                UnrecognizedResponseHandler(serverResponse);
        }





    }
}
