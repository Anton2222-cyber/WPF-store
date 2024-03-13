using WebApi.Models.PostOffice;

namespace WebApi.Models.Order;

public class CustomerOrderItemViewModel {
	public long Id { get; set; }

	public DateTime TimeOfCreation { get; set; }

	public OrderStatusItemViewModel Status { get; set; } = null!;

	public PostOfficeItemViewModel PostOffice { get; set; } = null!;

	public ICollection<OrderedProductItemViewModel> OrderedProducts { get; set; } = null!;

	public CustomerInfo Customer { get; set; } = null!;
}
