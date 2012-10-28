namespace SampleTransaq
{
	using System.ComponentModel;
	using System.Windows;
	using System.Windows.Controls;

	using Ecng.Common;

	using StockSharp.BusinessEntities;
    using StockSharp.Transaq;
	using StockSharp.Transaq.Command;

	public partial class NewStopOrderWindow
	{
		private readonly bool _initialized;

		public NewStopOrderWindow()
		{
			InitializeComponent();
			this.Portfolio.Trader = MainWindow.Instance.Trader;
			_initialized = true;
			RefreshControls();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			this.Portfolio.Trader = null;
			base.OnClosing(e);
		}

		public Security Security { get; set; }

		private void Send_Click(object sender, RoutedEventArgs e)
		{
			var stopOrder = new Order
			{
				Portfolio = this.Portfolio.SelectedPortfolio,
				Type = OrderTypes.Conditional,
				Volume = this.Volume.Text.To<int>(),
				Price = this.Price.Text.To<decimal>(),
				Security = this.Security,
				Direction = this.IsBuy.IsChecked == true ? OrderDirections.Buy : OrderDirections.Sell,
				StopCondition = new TransaqStopConditions
				{
					ConditionPrice= this.StopPrice.Text.To<double>(),
                    StopConditionKind=TransaqStopConditionKinds.None
				},
			};

			MainWindow.Instance.Trader.RegisterOrder(stopOrder);
			base.DialogResult = true;
		}

		private void StopOrderType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			RefreshControls();
		}

		private void RefreshControls()
		{
			if (_initialized)
			{
				this.Price.IsEnabled = this.StopOrderType.SelectedIndex == 1;
			}
		}

		private void Portfolio_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.Send.IsEnabled = this.Portfolio.SelectedPortfolio != null;
		}
	}
}