using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using View.Auth;
using View.Models;
using View.Models.Api;
using View.Services;

namespace View.Pages {
	/// <summary>
	/// Interaction logic for AccountPage.xaml
	/// </summary>
	public partial class AccountPage : INotifyPropertyChanged {
		private string _photoPath = null;
		private ApiRepository _apiRepository = new ApiRepository();

		private Visibility _visibleIsAuthorized = Visibility.Visible;
		public Visibility VisibleIsAuthorized {
			get => _visibleIsAuthorized;
			set {
				if (_visibleIsAuthorized == value)
					return;

				_visibleIsAuthorized = value;
				OnPropertyChanged(nameof(VisibleIsAuthorized));
			}
		}

		public UserInfoVm UserInfo {
			set {
				EmailValue = value?.Email;
				NameValue = value?.Name;
				PhotoValue = value?.Photo == null ? null : _apiRepository.GetImageUrlByPart(value.Photo);
			}
		}

		private string _email = null;
		public string EmailValue {
			get => _email;
			set {
				if (_email == value)
					return;

				_email = value;
				OnPropertyChanged(nameof(EmailValue));
			}
		}

		private string _name = null;
		public string NameValue {
			get => _name;
			set {
				if (_name == value)
					return;

				_name = value;
				OnPropertyChanged(nameof(NameValue));
			}
		}

		private string _photo = null;
		public string PhotoValue {
			get => _photo;
			set {
				if (_photo == value)
					return;

				_photo = value;
				OnPropertyChanged(nameof(PhotoValue));
			}
		}

		public AccountPage() {
			InitializeComponent();

			AuthManager.PropertyChanged += (object sender, EventArgs e) => {
				VisibleIsAuthorized = AuthManager.VisibleIsAuthorized;
				UserInfo = AuthManager.UserInfo;
			};
			VisibleIsAuthorized = AuthManager.VisibleIsAuthorized;
			UserInfo = AuthManager.UserInfo;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private async Task LoginAsync() {
			try {
				var token = await _apiRepository.LoginAsync(new LoginVm {
					Email = LoginEmail.Text,
					Password = LoginPassword.Password
				});

				AuthManager.Token = token;
				if (Views.BasketPage != null)
					await Views.BasketPage.LoadBasketAsync();
				if (Views.OrdersPage != null)
					await Views.OrdersPage.LoadOrdersAsync();
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private async Task RegistrationAsync() {
			try {
				if (RegistrationPassword.Password != RegistrationConfirmPassword.Password)
					throw new Exception("Passwords are different");

				var token = await _apiRepository.RegistrationAsync(new RegisterVm {
					FirstName = RegistrationFirstName.Text,
					LastName = RegistrationLastName.Text,
					Email = RegistrationEmail.Text,
					UserName = RegistrationUserName.Text,
					Image = _photoPath,
					Password = RegistrationPassword.Password
				});

				AuthManager.Token = token;
				if (Views.BasketPage != null)
					await Views.BasketPage.LoadBasketAsync();
				if (Views.OrdersPage != null)
					await Views.OrdersPage.LoadOrdersAsync();
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void LoginButton_Click(object sender, RoutedEventArgs e) {
			_ = LoginAsync();
		}

		private void LogoutButton_Click(object sender, RoutedEventArgs e) {
			AuthManager.IsAuthorized = false;
		}

		private void RegistrationButton_Click(object sender, RoutedEventArgs e) {
			_ = RegistrationAsync();
		}

		private void SelectPhotoButton_Click(object sender, RoutedEventArgs e) {
			OpenFileDialog openFileDialog = new OpenFileDialog();

			if (openFileDialog.ShowDialog() == true) {
				try {
					RegistrationPhoto.Source = new BitmapImage(new Uri(openFileDialog.FileName));
					_photoPath = openFileDialog.FileName;
				}
				catch {
					MessageBox.Show("File format is not valid or file is corrupted");
				}
			}
		}
	}
}
