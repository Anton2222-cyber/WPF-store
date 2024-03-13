using System;
using System.ComponentModel;
using System.Windows;
using View.Auth;
using View.Pages;
using Wpf.Ui.Appearance;

namespace View {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : INotifyPropertyChanged {
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

		public MainWindow() {
			DataContext = this;

			SystemThemeWatcher.Watch(this);

			InitializeComponent();

			Views.MainWindow = this;

			AuthManager.PropertyChanged += (object sender, EventArgs e) => {
				VisibleIsAdminOrUser = AuthManager.VisibleIsAdminOrUser;
			};
			VisibleIsAdminOrUser = AuthManager.VisibleIsAdminOrUser;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void FluentWindow_Loaded(object sender, RoutedEventArgs e) {
			RootNavigation.Navigate(typeof(CategoriesPage));
		}


		public void NavigateToProducts() {
			RootNavigation.Navigate(typeof(ProductsPage));
		}

		public void NavigateToBuyMenu() {
			RootNavigation.Navigate(typeof(BuyPage));
		}

		public void NavigateToSuccessfulOrder() {
			if (RootNavigation.SelectedItem.TargetPageType != typeof(BuyPage))
				throw new InvalidOperationException("Allowed to use only from BuyPage");

			RootNavigation.GoBack();
			RootNavigation.Navigate(typeof(SuccessfulOrderPage));
		}

		public void NavigateToOperationNotAvailable() {
			RootNavigation.Navigate(typeof(OperationNotAvailablePage));
		}
	}
}
