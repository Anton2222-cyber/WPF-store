using LocalData.Context;
using LocalData.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Windows;
using View.Constants;
using View.Models;

namespace View.Auth {
	public static class AuthManager {
		private static string _token;
		public static string Token {
			get => _token;
			set {
				using (LocalDataContext context = new LocalDataContext()) {
					var tokenItem = context.Settings.FirstOrDefault(s => s.Name == "token");

					if (value == null) {
						if (tokenItem != null) {
							context.Settings.Remove(tokenItem);
							context.SaveChanges();
							_token = value;
							OnPropertyChanged();
						}

						return;
					}

					if (tokenItem != null) {
						tokenItem.Value = value;
					}
					else {
						context.Settings.Add(new Setting {
							Name = "token",
							Value = value
						});
					}

					context.SaveChanges();
					_token = value;
					OnPropertyChanged();
				}
			}
		}

		static AuthManager() {
			using (LocalDataContext context = new LocalDataContext()) {
				var tokenItem = context.Settings.FirstOrDefault(s => s.Name == "token");

				_token = tokenItem?.Value;
			}
		}

		private static JwtSecurityToken ConvertJwtStringToJwtSecurityToken(string jwt) {
			var handler = new JwtSecurityTokenHandler();
			var token = handler.ReadJwtToken(jwt);

			return token;
		}

		private static JwtSecurityToken ActualJwtSecurityToken {
			get => ConvertJwtStringToJwtSecurityToken(_token);
		}

		public static UserInfoVm UserInfo {
			get {
				if (Token == null)
					return null;

				var tokenClaims = ActualJwtSecurityToken.Claims
					.ToArray();


				var email = tokenClaims
					.Where(c => c.Type == "email")
					.Select(c => c.Value)
					.First();

				var name = tokenClaims
					.Where(c => c.Type == "name")
					.Select(c => c.Value)
					.First();

				var photo = tokenClaims
					.Where(c => c.Type == "photo")
					.Select(c => c.Value)
					.First();

				return new UserInfoVm {
					Email = email,
					Name = name,
					Photo = photo
				};
			}
		}

		private static bool HasAnyRole(params string[] searchedRoles) {
			if (Token == null)
				return false;

			var token = ActualJwtSecurityToken;
			var roles = token.Claims
				.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
				.Select(c => c.Value)
				.ToArray();

			return searchedRoles.Any(sr => roles.Contains(sr));
		}

		private static Visibility BoolToVisibility(bool isVisible) => isVisible ? Visibility.Visible : Visibility.Hidden;

		public static bool IsAuthorized {
			get => Token != null;
			set => Token = null;
		}
		public static bool IsUser => HasAnyRole(ApiRoles.User);
		public static bool IsAdmin => HasAnyRole(ApiRoles.Admin);
		public static bool IsAdminOrUser => HasAnyRole(ApiRoles.Admin, ApiRoles.User);

		public static Visibility VisibleIsAuthorized => BoolToVisibility(IsAuthorized);
		public static Visibility VisibleIsUser => BoolToVisibility(IsUser);
		public static Visibility VisibleIsAdmin => BoolToVisibility(IsAdmin);
		public static Visibility VisibleIsAdminOrUser => BoolToVisibility(IsAdminOrUser);

		public static event EventHandler PropertyChanged;
		private static void OnPropertyChanged() {
			PropertyChanged?.Invoke(null, EventArgs.Empty);
		}
	}
}
