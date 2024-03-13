using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using View.Auth;
using View.Models.Api;

namespace View.Services {
	public class ApiRepository {
		public const string ApiUrl = "http://localhost:5114";
		public const string ImagesUrlPart = "Data/images";

		private const string CategoriesControllerName = "categories";
		private const string ProductsControllerName = "products";
		private const string AccountControllerName = "account";
		private const string BasketControllerName = "basket";
		private const string AreasControllerName = "areas";
		private const string SettlementsControllerName = "settlements";
		private const string PostOfficeControllerName = "postOffice";
		private const string OrderControllerName = "order";

		public ApiRepository() { }

		public string GetImageUrlByPart(string part) {
			return $"{ApiUrl}/{ImagesUrlPart}/{part}";
		}

		private string BuildUrl(string controller, string action) {
			return $"{ApiUrl}/api/{controller}/{action}";
		}

		public async Task<FilteredCategoriesVm> GetFilteredCategoriesAsync(CategoryFilterVm filter) {
			using (HttpClient client = new HttpClient()) {
				string url = BuildUrl(CategoriesControllerName, "GetFiltered");

				QueryParametersBulder parametersBulder = new QueryParametersBulder();
				parametersBulder.AddParameter("name", filter.Name);
				parametersBulder.AddParameter("limit", filter.Limit);
				parametersBulder.AddParameter("offset", filter.Offset);

				Stream stream = await client.GetStreamAsync($"{url}{parametersBulder}");

				using (StreamReader streamReader = new StreamReader(stream)) {
					string jsonText = streamReader.ReadToEnd();
					var response = JsonConvert.DeserializeObject<FilteredCategoriesVm>(jsonText);
					return response;
				}
			}
		}

		public async Task<FilteredProductsVm> GetFilteredProductsAsync(ProductFilterVm filter) {
			using (HttpClient client = new HttpClient()) {
				string url = BuildUrl(ProductsControllerName, "GetFiltered");

				QueryParametersBulder parametersBulder = new QueryParametersBulder();
				parametersBulder.AddParameter("categoryId", filter.CategoryId);
				parametersBulder.AddParameter("name", filter.Name);
				parametersBulder.AddParameter("description", filter.Description);
				parametersBulder.AddParameter("minPrice", filter.MinPrice);
				parametersBulder.AddParameter("maxPrice", filter.MaxPrice);
				parametersBulder.AddParameter("limit", filter.Limit);
				parametersBulder.AddParameter("offset", filter.Offset);

				Stream stream = await client.GetStreamAsync($"{url}{parametersBulder}");

				using (StreamReader streamReader = new StreamReader(stream)) {
					string jsonText = streamReader.ReadToEnd();
					var response = JsonConvert.DeserializeObject<FilteredProductsVm>(jsonText);
					return response;
				}
			}
		}

		public async Task<string> LoginAsync(LoginVm model) {
			using (HttpClient client = new HttpClient()) {
				string url = BuildUrl(AccountControllerName, "Login");

				MultipartFormDataContent form = new MultipartFormDataContent {
					{ new StringContent(model.Email), "email" },
					{ new StringContent(model.Password), "password" }
				};

				var response = await client.PostAsync(url, form);

				if (!response.IsSuccessStatusCode)
					throw new Exception($"Status: {response.StatusCode}");

				using (StreamReader streamReader = new StreamReader(await response.Content.ReadAsStreamAsync())) {
					string jsonText = streamReader.ReadToEnd();
					var tokenVm = JsonConvert.DeserializeObject<TokenVm>(jsonText);
					return tokenVm.Token;
				}
			}
		}

		public async Task<string> RegistrationAsync(RegisterVm model) {
			using (HttpClient client = new HttpClient()) {
				string url = BuildUrl(AccountControllerName, "Registration");

				var image = new FileInfo(model.Image);

				MultipartFormDataContent form = new MultipartFormDataContent {
					{ new StringContent(model.FirstName), "firstName" },
					{ new StringContent(model.LastName), "lastName" },
					{ new StringContent(model.Email), "email" },
					{ new StringContent(model.UserName), "userName" },
					{ new ByteArrayContent(System.IO.File.ReadAllBytes(model.Image)), "image", image.Name },
					{ new StringContent(model.Password), "password" }
				};

				var response = await client.PostAsync(url, form);

				if (!response.IsSuccessStatusCode)
					throw new Exception($"Error: {await response.Content.ReadAsStringAsync()}");

				using (StreamReader streamReader = new StreamReader(await response.Content.ReadAsStreamAsync())) {
					string jsonText = streamReader.ReadToEnd();
					var tokenVm = JsonConvert.DeserializeObject<TokenVm>(jsonText);
					return tokenVm.Token;
				}
			}
		}

		private static void AddAuthHeader(HttpClient client) => client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AuthManager.Token}");

		public async Task<List<BasketItemVm>> GetBasketItemsAsync() {
			using (HttpClient client = new HttpClient()) {
				AddAuthHeader(client);

				Stream stream = await client.GetStreamAsync(BuildUrl(BasketControllerName, "Get"));

				using (StreamReader streamReader = new StreamReader(stream)) {
					string jsonText = streamReader.ReadToEnd();
					var response = JsonConvert.DeserializeObject<List<BasketItemVm>>(jsonText);
					return response;
				}
			}
		}

		public async Task AddProductToBasketAsync(long productId) {
			using (HttpClient client = new HttpClient()) {
				AddAuthHeader(client);

				string url = BuildUrl(BasketControllerName, "Create");

				var response = await client.PostAsync($"{url}/{productId}", null);

				if (!response.IsSuccessStatusCode)
					throw new Exception($"Status: {response.StatusCode}");
			}
		}

		public async Task SetBasketQuantityAsync(long productId, int quantity) {
			using (HttpClient client = new HttpClient()) {
				AddAuthHeader(client);

				string url = BuildUrl(BasketControllerName, "SetQuantity");

				var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{url}/{productId} {quantity}");

				await client.SendAsync(request);
			}
		}

		public async Task DeleteProductFromBasketAsync(long productId) {
			using (HttpClient client = new HttpClient()) {
				AddAuthHeader(client);

				string url = BuildUrl(BasketControllerName, "Delete");

				var response = await client.DeleteAsync($"{url}/{productId}");

				if (!response.IsSuccessStatusCode)
					throw new Exception($"Status: {response.StatusCode}");
			}
		}

		public async Task<double> GetBasketTotalPriceAsync() {
			using (HttpClient client = new HttpClient()) {
				AddAuthHeader(client);

				string url = BuildUrl(BasketControllerName, "GetTotalPrice");

				Stream stream = await client.GetStreamAsync(url);

				using (StreamReader streamReader = new StreamReader(stream)) {
					string jsonText = streamReader.ReadToEnd();
					var response = JsonConvert.DeserializeObject<TotalPriceVm>(jsonText);
					return response.TotalPrice;
				}
			}
		}

		public async Task<List<AreaVm>> GetAreasAsync() {
			using (HttpClient client = new HttpClient()) {
				AddAuthHeader(client);

				string url = BuildUrl(AreasControllerName, "GetAll");

				Stream stream = await client.GetStreamAsync(url);

				using (StreamReader streamReader = new StreamReader(stream)) {
					string jsonText = streamReader.ReadToEnd();
					var response = JsonConvert.DeserializeObject<List<AreaVm>>(jsonText);
					return response;
				}
			}
		}

		public async Task<List<SettlementVm>> GetSettlementsAsync(long areaId) {
			using (HttpClient client = new HttpClient()) {
				AddAuthHeader(client);

				string url = BuildUrl(SettlementsControllerName, "GetByAreaId");

				Stream stream = await client.GetStreamAsync($"{url}/{areaId}");

				using (StreamReader streamReader = new StreamReader(stream)) {
					string jsonText = streamReader.ReadToEnd();
					var response = JsonConvert.DeserializeObject<List<SettlementVm>>(jsonText);
					return response;
				}
			}
		}

		public async Task<List<PostOfficeVm>> GetPostOfficesAsync(long settlementId) {
			using (HttpClient client = new HttpClient()) {
				AddAuthHeader(client);

				string url = BuildUrl(PostOfficeControllerName, "GetBySettlementId");

				Stream stream = await client.GetStreamAsync($"{url}/{settlementId}");

				using (StreamReader streamReader = new StreamReader(stream)) {
					string jsonText = streamReader.ReadToEnd();
					var response = JsonConvert.DeserializeObject<List<PostOfficeVm>>(jsonText);
					return response;
				}
			}
		}

		public async Task MakeOrderAsync(long postOfficeId) {
			using (HttpClient client = new HttpClient()) {
				AddAuthHeader(client);

				string url = BuildUrl(OrderControllerName, "Order");

				MultipartFormDataContent form = new MultipartFormDataContent {
					{ new StringContent(postOfficeId.ToString()), "postOfficeId" }
				};

				var response = await client.PostAsync(url, form);

				if (!response.IsSuccessStatusCode)
					throw new Exception($"Error: {await response.Content.ReadAsStringAsync()}");
			}
		}

		public async Task<FilteredOrdersVm> GetOrdersAsync() {
			using (HttpClient client = new HttpClient()) {
				AddAuthHeader(client);

				string url = BuildUrl(OrderControllerName, "GetFiltered");

				Stream stream = await client.GetStreamAsync(url);

				using (StreamReader streamReader = new StreamReader(stream)) {
					string jsonText = streamReader.ReadToEnd();
					var response = JsonConvert.DeserializeObject<FilteredOrdersVm>(jsonText);
					return response;
				}
			}
		}
	}
}
