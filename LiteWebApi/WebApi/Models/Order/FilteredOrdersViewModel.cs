namespace WebApi.Models.Order;

public class FilteredOrdersViewModel {
	public ICollection<OrderItemViewModel> FilteredOrders { get; set; } = null!;
	public long AvailableQuantity { get; set; }
}