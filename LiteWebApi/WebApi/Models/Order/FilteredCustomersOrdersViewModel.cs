namespace WebApi.Models.Order;

public class FilteredCustomersOrdersViewModel {
	public ICollection<CustomerOrderItemViewModel> FilteredOrders { get; set; } = null!;
	public long AvailableQuantity { get; set; }
}