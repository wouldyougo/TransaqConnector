namespace SampleTransaq
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Windows;
	using MessageBox = System.Windows.MessageBox;

	using Ecng.Collections;
	using Ecng.Common;
	using Ecng.Xaml;

	using StockSharp.BusinessEntities;
    using StockSharp.Transaq;
    
	public partial class MainWindow
	{
		private bool _isConnected;

		public TransaqTrader Trader;         

		private readonly SecuritiesWindow _securitiesWindow = new SecuritiesWindow();
		private readonly TradesWindow _tradesWindow = new TradesWindow();
		private readonly MyTradesWindow _myTradesWindow = new MyTradesWindow();
		private readonly OrdersWindow _ordersWindow = new OrdersWindow();
		private readonly PortfolioWindow _portfolioWindow = new PortfolioWindow();
		private readonly StopOrdersWindow _stopOrdersWindow = new StopOrdersWindow();

		public MainWindow()
		{
			InitializeComponent();
			MainWindow.Instance = this;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			_ordersWindow.RealClose = _myTradesWindow.RealClose =
			_tradesWindow.RealClose = _securitiesWindow.RealClose =
			_stopOrdersWindow.RealClose = _portfolioWindow.RealClose = true;
			
			_securitiesWindow.Close();
			_tradesWindow.Close();
			_myTradesWindow.Close();
			_stopOrdersWindow.Close();
			_ordersWindow.Close();
			_portfolioWindow.Close();

			if (this.Trader != null)
				this.Trader.Dispose();

			base.OnClosing(e);
		}

		public static MainWindow Instance { get; private set; }

		private void Connect_Click(object sender, RoutedEventArgs e)
		{
			if (!_isConnected)
			{
				if (this.Trader == null)
				{
					if (this.Login.Text.IsEmpty())
					{
						MessageBox.Show(this, "Не указан логин.");
						return;
					}
					else if (this.Password.Password.IsEmpty())
					{
						MessageBox.Show(this, "Не указан пароль.");
						return;
					}

					// создаем шлюз
					this.Trader = new TransaqTrader()
                    {
                       Login=this.Login.Text, 
                       Password=this.Password.Password, 
                       Host= this.textAddress.Text,
                       Port=this.textPort.Text
                    };

					// очищаем из текстового поля в целях безопасности
					this.Password.Clear();

					// инициализируем механизм переподключения (будет автоматически соединяться
					// каждые 10 секунд, если шлюз потеряется связь с сервером)
					this.Trader.ReConnectionSettings.Interval = TimeSpan.FromSeconds(10);
					//this.Trader.ReConnectionSettings.TimeBounds = Exchange.Rts.WorkingTime;
					this.Trader.ReConnectionSettings.ConnectionRestored += () => this.GuiAsync(() =>
					{
						// разблокируем кнопку Экспорт (соединение было восстановлено)
						ChangeConnectStatus(true);
						MessageBox.Show(this, "Соединение восстановлено.");
					});

					// подписываемся на событие успешного соединения
					this.Trader.Connected += () =>
					{
						// возводим флаг, что соединение установлено
						_isConnected = true;

						// разблокируем кнопку Экспорт
						this.GuiAsync(() => ChangeConnectStatus(true));
					};

					// подписываемся на событие разрыва соединения
					this.Trader.ConnectionError += error => this.GuiAsync(() =>
					{
						// заблокируем кнопку Экспорт (так как соединение было потеряно)
						ChangeConnectStatus(false);

						MessageBox.Show(this, error.ToString(), "Ошибка соединения");	
					});

                    this.Trader.Disconnected+= ()=>this.GuiAsync(() =>
                    {
                        // заблокируем кнопку Экспорт (так как соединение было потеряно)
                        ChangeConnectStatus(false);
       
                    });

					this.Trader.ProcessDataError += error => this.GuiAsync(() => MessageBox.Show(this, error.ToString(), "Ошибка обработки данных"));

                    this.Trader.NewSecurities += new Action<IEnumerable<Security>>(Trader_NewSecurities);
                      
					this.Trader.NewMyTrades += trades => this.GuiAsync(() => _myTradesWindow.Trades.AddRange(trades));
					this.Trader.NewTrades += trades => this.GuiAsync(() => _tradesWindow.Trades.AddRange(trades));
					this.Trader.NewOrders += orders => this.GuiAsync(() => _ordersWindow.Orders.AddRange(orders));
					this.Trader.NewStopOrders += orders => this.GuiAsync(() => _stopOrdersWindow.Orders.AddRange(orders));
					this.Trader.NewPortfolios += portfolios => portfolios.ForEach(this.Trader.RegisterPortfolio);
					this.Trader.NewPositions += positions => this.GuiAsync(() => _portfolioWindow.Positions.AddRange(positions));

					// подписываемся на событие о неудачной регистрации заявок
					this.Trader.OrdersRegisterFailed += OrdersFailed;

					// подписываемся на событие о неудачной регистрации стоп-заявок
					this.Trader.StopOrdersRegisterFailed += OrdersFailed;

					this.ShowSecurities.IsEnabled = this.ShowTrades.IsEnabled =
					this.ShowMyTrades.IsEnabled = this.ShowOrders.IsEnabled = 
					this.ShowPortfolio.IsEnabled = this.ShowStopOrders.IsEnabled = true;
				}
                //this.Trader. Login = this.Login.Text;
                //this.Trader.Password = this.Password.Password;
                //this.Trader.Host = this.textAddress.Text;
                //this.Trader.Port = this.textPort.Text;
                this.Trader.Connect();
			}
			else
			{
				this.Trader.Disconnect();
			}
		}

        void Trader_NewSecurities(IEnumerable<Security> securities)
        {
            this.GuiAsync(() => _securitiesWindow.AddSecurities(securities));
        }

		private void OrdersFailed(IEnumerable<OrderFail> fails)
		{
			this.GuiAsync(() =>
			{
				foreach (var fail in fails)
					MessageBox.Show(this, fail.Error.ToString(), "Ошибка регистрации заявки");
			});
		}

		private void ChangeConnectStatus(bool isConnected)
		{
			_isConnected = isConnected;
			this.ConnectBtn.Content = isConnected ? "Отключиться" : "Подключиться";
			this.Export.IsEnabled = isConnected;
		}

		private void ShowSecurities_Click(object sender, RoutedEventArgs e)
		{
			ShowOrHide(_securitiesWindow);
		}

		private void ShowTrades_Click(object sender, RoutedEventArgs e)
		{
			ShowOrHide(_tradesWindow);
		}

		private void ShowMyTrades_Click(object sender, RoutedEventArgs e)
		{
			ShowOrHide(_myTradesWindow);
		}

		private void ShowOrders_Click(object sender, RoutedEventArgs e)
		{
			ShowOrHide(_ordersWindow);
		}

		private void ShowPortfolio_Click(object sender, RoutedEventArgs e)
		{
			ShowOrHide(_portfolioWindow);
		}

		private void ShowStopOrders_Click(object sender, RoutedEventArgs e)
		{
			ShowOrHide(_stopOrdersWindow);
		}

		private static void ShowOrHide(Window window)
		{
			if (window == null)
				throw new ArgumentNullException("window");

			if (window.Visibility == Visibility.Visible)
				window.Hide();
			else
				window.Show();
		}

		private void Export_Click(object sender, RoutedEventArgs e)
		{
			this.Trader.StartExport();
		}
	}
}
