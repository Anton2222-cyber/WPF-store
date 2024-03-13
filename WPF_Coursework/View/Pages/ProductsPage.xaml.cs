using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using View.Auth;
using View.Models;
using View.Models.Api;
using View.Services;
using Wpf.Ui.Controls;

namespace View.Pages {
	/// <summary>
	/// Interaction logic for ProductsPage.xaml
	/// </summary>
	public partial class ProductsPage : INotifyPropertyChanged {
		private const int _itemsPerPage = 3;

		private ApiRepository _apiRepository = new ApiRepository();

		private ObservableCollection<ProductCardVm> _productsList;
		public ObservableCollection<ProductCardVm> ProductsList {
			get => _productsList;
			set {
				_productsList = value;
				OnPropertyChanged(nameof(ProductsList));
			}
		}

		private long _categoryId;
		public long CategoryId {
			get => _categoryId;
			set {
				if (_categoryId == value)
					return;

				_categoryId = value;
				CurrentPage = 1;
				_ = LoadProductsAsync();
			}
		}

		private int _availablePages;
		public int AvailablePages {
			get => _availablePages;
			set {
				_availablePages = value;
				OnPropertyChanged(nameof(AvailablePages));
			}
		}

		private int _currentPage = 1;
		public int CurrentPage {
			get => _currentPage;
			set {
				if (!IsValidPageNumber(value))
					return;

				if (value != PageNumberBox.Value)
					PageNumberBox.Value = value;

				_currentPage = value;
				OnPropertyChanged(nameof(_currentPage));
				_ = LoadProductsAsync();
			}
		}

		private Visibility _visibleIsAdminOrUser = Visibility.Visible;
		public Visibility VisibleIsAdminOrUser {
			get => _visibleIsAdminOrUser;
			set {
				if (_visibleIsAdminOrUser == value)
					return;

				_visibleIsAdminOrUser = value;
				OnPropertyChanged(nameof(VisibleIsAdminOrUser));
			}
		}

		private Visibility _visibleIsAdmin = Visibility.Visible;
		public Visibility VisibleIsAdmin {
			get => _visibleIsAdmin;
			set {
				if (_visibleIsAdmin == value)
					return;

				_visibleIsAdmin = value;
				OnPropertyChanged(nameof(VisibleIsAdmin));
			}
		}



		public ProductsPage() {
			InitializeComponent();

			Views.ProductsPage = this;

			AuthManager.PropertyChanged += (object sender, EventArgs e) => {
				VisibleIsAdmin = AuthManager.VisibleIsAdmin;
			};
			VisibleIsAdmin = AuthManager.VisibleIsAdmin;

			AuthManager.PropertyChanged += (object sender, EventArgs e) => {
				VisibleIsAdminOrUser = AuthManager.VisibleIsAdminOrUser;
			};
			VisibleIsAdminOrUser = AuthManager.VisibleIsAdminOrUser;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private bool IsValidPageNumber(int pageNumber) => (1 <= pageNumber && pageNumber <= _availablePages);

		private async Task LoadProductsAsync() {
			try {
				var response = await _apiRepository.GetFilteredProductsAsync(new ProductFilterVm {
					CategoryId = CategoryId,
					Limit = _itemsPerPage,
					Offset = (CurrentPage - 1) * _itemsPerPage
				});

				var products = response.FilteredProducts
					.Select(p => new ProductCardVm {
						Id = p.Id,
						DateCreated = p.DateCreated,
						Name = p.Name,
						Description = p.Description,
						Price = p.Price,
						Category = p.Category,
						Image = _apiRepository.GetImageUrlByPart(p.Images.First())
					})
					.ToArray();

				ProductsList = new ObservableCollection<ProductCardVm>(products);
				AvailablePages = (int)Math.Ceiling((double)response.AvailableQuantity / _itemsPerPage);
			}
			catch (Exception e) {
				System.Windows.MessageBox.Show(e.Message);
			}
		}

		private void NumberBox_ValueChanged(object sender, RoutedEventArgs e) {
			if (PageNumberBox.Value is null)
				PageNumberBox.Value = CurrentPage;

			CurrentPage = (int)PageNumberBox.Value;
		}

		private void PrevButton_Click(object sender, RoutedEventArgs e) {
			CurrentPage--;
		}

		private void NextButton_Click(object sender, RoutedEventArgs e) {
			CurrentPage++;
		}

		private void BuyButton_Click(object sender, RoutedEventArgs e) {
			var productId = Convert.ToInt32(((System.Windows.Controls.Button)sender).Tag);
			_ = AddToBasketAsync(productId);
		}

		private async Task AddToBasketAsync(int productId) {
			try {
				await _apiRepository.AddProductToBasketAsync(productId);
				if (Views.BasketPage != null)
					await Views.BasketPage.LoadBasketAsync();
			}
			catch {
				System.Windows.MessageBox.Show("Error, maybe the product is already in the cart");
			}
		}

		private void NotCompletedButton_Click(object sender, RoutedEventArgs e) {
			Views.MainWindow.NavigateToOperationNotAvailable();
		}
	}
}
