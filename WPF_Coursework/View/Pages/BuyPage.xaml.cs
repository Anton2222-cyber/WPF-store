using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using View.Models.Api;
using View.Services;

namespace View.Pages {
	/// <summary>
	/// Interaction logic for BuyPage.xaml
	/// </summary>
	public partial class BuyPage : INotifyPropertyChanged {
		private ApiRepository _apiRepository = new ApiRepository();

		private string _totalPrice = "Not loaded";
		public string TotalPrice {
			get => _totalPrice;
			set {
				_totalPrice = value;
				OnPropertyChanged(nameof(TotalPrice));
			}
		}

		private ObservableCollection<AreaVm> _areas;
		public ObservableCollection<AreaVm> Areas {
			get => _areas;
			set {
				_areas = value;
				OnPropertyChanged(nameof(Areas));
			}
		}

		private ObservableCollection<SettlementVm> _settlements;
		public ObservableCollection<SettlementVm> Settlements {
			get => _settlements;
			set {
				_settlements = value;
				OnPropertyChanged(nameof(Settlements));
			}
		}

		private ObservableCollection<PostOfficeVm> _postOffices;
		public ObservableCollection<PostOfficeVm> PostOffices {
			get => _postOffices;
			set {
				_postOffices = value;
				OnPropertyChanged(nameof(PostOffices));
			}
		}

		public BuyPage() {
			InitializeComponent();

			_ = LoadTotalPriceAsync();
			_ = LoadAreasAsync();
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private async Task LoadTotalPriceAsync() {
			try {
				var totalPrice = await _apiRepository.GetBasketTotalPriceAsync();
				TotalPrice = $"{totalPrice} UAH";
			}
			catch { }
		}

		private async Task LoadAreasAsync() {
			try {
				var areas = await _apiRepository.GetAreasAsync();
				Areas = new ObservableCollection<AreaVm>(areas);
			}
			catch {
				MessageBox.Show(
					"Areas were not loaded successfully",
					"Error",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
			}
		}

		private async Task LoadSettlementsAsync(long areaId) {
			try {
				var settlements = await _apiRepository.GetSettlementsAsync(areaId);
				Settlements = new ObservableCollection<SettlementVm>(settlements);
			}
			catch {
				MessageBox.Show(
					"Settlements were not loaded successfully",
					"Error",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
			}
		}

		private async Task LoadPostOfficesAsync(long settlementId) {
			try {
				var postOffices = await _apiRepository.GetPostOfficesAsync(settlementId);
				PostOffices = new ObservableCollection<PostOfficeVm>(postOffices);
			}
			catch {
				MessageBox.Show(
					"Post offices were not loaded successfully",
					"Error",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
			}
		}

		private async Task OrderAsync(long postOfficeId) {
			try {
				await _apiRepository.MakeOrderAsync(postOfficeId);
				await Views.BasketPage.LoadBasketAsync();
				if (Views.OrdersPage != null)
					await Views.OrdersPage.LoadOrdersAsync();
				Views.MainWindow.NavigateToSuccessfulOrder();
			}
			catch {
				MessageBox.Show(
					"Order error",
					"Error",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
			}
		}

		private void AreasComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			long areaId = Convert.ToInt64(((ComboBox)sender).SelectedValue);

			_ = LoadSettlementsAsync(areaId);
		}

		private void SettlementsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			long settlementId = Convert.ToInt64(((ComboBox)sender).SelectedValue);

			_ = LoadPostOfficesAsync(settlementId);
		}

		private void OrderButton_Click(object sender, RoutedEventArgs e) {
			if (PostOfficesComboBox.SelectedValue == null) {
				MessageBox.Show(
					"Address is not selected",
					"Error",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);

				return;
			}

			long postOfficeId = Convert.ToInt64(PostOfficesComboBox.SelectedValue);
			_ = OrderAsync(postOfficeId);
		}
	}
}
