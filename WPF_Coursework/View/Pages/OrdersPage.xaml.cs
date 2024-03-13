using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using View.Models;
using View.Models.Api;
using View.Services;

namespace View.Pages {
	/// <summary>
	/// Interaction logic for OrdersPage.xaml
	/// </summary>
	public partial class OrdersPage : INotifyPropertyChanged {
		private ApiRepository _apiRepository = new ApiRepository();

		private ObservableCollection<OrderCardVm> _orderCards;
		public ObservableCollection<OrderCardVm> OrderCards {
			get => _orderCards;
			set {
				_orderCards = value;
				OnPropertyChanged(nameof(OrderCards));
			}
		}

		public OrdersPage() {
			InitializeComponent();

			Views.OrdersPage = this;

			_ = LoadOrdersAsync();
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public async Task LoadOrdersAsync() {
			try {
				var filteredOrders = await _apiRepository.GetOrdersAsync();

				var orders = filteredOrders.FilteredOrders;

				var orderCards = orders
					.Select(o => new OrderCardVm {
						Id = o.Id,
						TimeOfCreation = o.TimeOfCreation.ToShortDateString(),
						Status = new OrderStatusCardVm {
							Id = o.Status.Id,
							Status = $"Status: {o.Status.Status}",
							TimeOfCreation = $"Status changed: {o.Status.TimeOfCreation.ToShortDateString()}"
						},
						PostOffice = new PostOfficeVm {
							Id = o.PostOffice.Id,
							Name = $"Shipping to: {o.PostOffice.Name}",
						},
						OrderedProducts = o.OrderedProducts
							.Select(op => new OrderedProductCardVm {
								Id = op.Id,
								Product = new ProductCardVm {
									Id = op.Product.Id,
									DateCreated = op.Product.DateCreated,
									Name = op.Product.Name,
									Description = op.Product.Description,
									Price = op.Product.Price,
									Category = op.Product.Category,
									Image = _apiRepository.GetImageUrlByPart(op.Product.Images.First())
								},
								UnitPrice = $"Unit price: {op.UnitPrice} UAH",
								Quantity = $"Quantity: {op.Quantity}"
							})
							.ToArray()
					})
					.ToArray();

				OrderCards = new ObservableCollection<OrderCardVm>(orderCards);
			}
			catch {
				MessageBox.Show(
					"Orders were not loaded successfully",
					"Error",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
			}
		}
	}
}
