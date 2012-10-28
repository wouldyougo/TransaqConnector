namespace SampleTransaq
{
	using System.ComponentModel;

	using StockSharp.BusinessEntities;
	using Ecng.Xaml;

	public partial class PortfolioWindow
	{
		public PortfolioWindow()
		{
			this.Positions = new ThreadSafeObservableCollection<Position>();
			InitializeComponent();
		}

		public ThreadSafeObservableCollection<Position> Positions { get; private set; }

		protected override void OnClosing(CancelEventArgs e)
		{
			if (!this.RealClose)
			{
				base.Hide();
				e.Cancel = true;
			}

			base.OnClosing(e);
		}

		public bool RealClose { get; set; }
	}
}