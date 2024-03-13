using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using View.Auth;
using View.Models.Api;
using View.Services;

namespace View.Pages {
	/// <summary>
	/// Interaction logic for CategoriesPage.xaml
	/// </summary>
	public partial class CategoriesPage : INotifyPropertyChanged {
		private const int _itemsPerPage = 3;

		private ApiRepository _apiRepository = new ApiRepository();

		private ObservableCollection<CategoryVm> _categoriesList;
		public ObservableCollection<CategoryVm> CategoriesList {
			get => _categoriesList;
			set {
				_categoriesList = value;
				OnPropertyChanged(nameof(CategoriesList));
			}
		}

		private long _availableCategories;

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
				_ = LoadCategoriesAsync();
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



		public CategoriesPage() {
			InitializeComponent();

			_ = LoadCategoriesAsync();

			Views.CategoriesPage = this;

			AuthManager.PropertyChanged += (object sender, EventArgs e) => {
				VisibleIsAdmin = AuthManager.VisibleIsAdmin;
			};
			VisibleIsAdmin = AuthManager.VisibleIsAdmin;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private bool IsValidPageNumber(int pageNumber) => (1 <= pageNumber && pageNumber <= _availablePages);

		private async Task LoadCategoriesAsync() {
			try {
				var response = await _apiRepository.GetFilteredCategoriesAsync(new CategoryFilterVm {
					Limit = _itemsPerPage,
					Offset = (CurrentPage - 1) * _itemsPerPage
				});

				var categories = response.FilteredCategories
					.Select(c => new CategoryVm {
						Id = c.Id,
						Name = c.Name,
						Image = _apiRepository.GetImageUrlByPart(c.Image),
						Description = c.Description
					})
					.ToArray();

				CategoriesList = new ObservableCollection<CategoryVm>(categories);
				_availableCategories = response.AvailableCategories;
				AvailablePages = (int)Math.Ceiling((double)_availableCategories / _itemsPerPage);
			}
			catch (Exception e) {
				MessageBox.Show(e.Message);
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			var tag = ((Button)sender).Tag;
			long categoryId = Convert.ToInt64(tag);
			OpenProducts(categoryId);
		}

		private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var tag = ((Grid)sender).Tag;
			long categoryId = Convert.ToInt64(tag);
			OpenProducts(categoryId);
		}

		private void OpenProducts(long categoryId) {
			Views.MainWindow.NavigateToProducts();
			Views.ProductsPage.CategoryId = categoryId;
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

		private void NotCompletedButton_Click(object sender, RoutedEventArgs e) {
			Views.MainWindow.NavigateToOperationNotAvailable();
		}
	}
}
