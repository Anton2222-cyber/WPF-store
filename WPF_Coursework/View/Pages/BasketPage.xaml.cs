using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using View.Models.Api;
using View.Services;
using Wpf.Ui.Controls;

namespace View.Pages {
	/// <summary>
	/// Interaction logic for BasketPage.xaml
	/// </summary>
	public partial class BasketPage : INotifyPropertyChanged {
		private ApiRepository _apiRepository = new ApiRepository();

		private ObservableCollection<BasketItemCardVm> _basketItems;
		public ObservableCollection<BasketItemCardVm> BasketItems {
			get => _basketItems;
			set {
				_basketItems = value;
				OnPropertyChanged(nameof(BasketItems));
			}
		}

		public BasketPage() {
			InitializeComponent();

			_ = LoadBasketAsync();

			Views.BasketPage = this;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public async Task LoadBasketAsync() {
			try {
				var basketItems = await _apiRepository.GetBasketItemsAsync();

				var items = basketItems
					.Select(bi => new BasketItemCardVm {
						Id = bi.Id,
						Product = new Models.BasketProductCardVm {
							Id = bi.Product.Id,
							DateCreated = bi.Product.DateCreated,
							Name = bi.Product.Name,
							Description = bi.Product.Description,
							TotalPrice = $"{bi.Quantity * bi.Product.Price} UAH",
							Category = bi.Product.Category,
							Image = _apiRepository.GetImageUrlByPart(bi.Product.Images.First())
						},
						Quantity = bi.Quantity
					})
					.ToArray();

				BasketItems = new ObservableCollection<BasketItemCardVm>(items);
			}
			catch (Exception e) {
				System.Windows.MessageBox.Show(e.Message);
			}
		}

		private void QuantityNumberBox_ValueChanged(object sender, RoutedEventArgs e) {
			var value = (int?)((NumberBox)sender).Value;

			if (value == null) {
				((NumberBox)sender).Value = value = 1;
			}

			var productId = Convert.ToInt32(((NumberBox)sender).Tag);

			try {
				_ = SetQuantityAndReload(productId, (int)value);
			}
			catch (Exception ex) {
				System.Windows.MessageBox.Show(ex.Message);
			}
		}

		private async Task SetQuantityAndReload(long productId, int quantity) {
			await _apiRepository.SetBasketQuantityAsync(productId, quantity);
			await LoadBasketAsync();
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e) {
			var productId = Convert.ToInt32(((Button)sender).Tag);

			_ = DeleteAndReload(productId);
		}

		private async Task DeleteAndReload(long productId) {
			await _apiRepository.DeleteProductFromBasketAsync(productId);
			await LoadBasketAsync();
		}

		private void BuyButton_Click(object sender, RoutedEventArgs e) {
			if (BasketItems == null || BasketItems.Count == 0) {
				System.Windows.MessageBox.Show("Basket is empty");
				return;
			}

			Views.MainWindow.NavigateToBuyMenu();
		}
	}
}
