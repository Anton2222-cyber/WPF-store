namespace WebApi.Models.Order;

public class OrderStatusItemViewModel {
	public long Id { get; set; }

	public string Status { get; set; } = null!;

	public DateTime TimeOfCreation { get; set; }
}
