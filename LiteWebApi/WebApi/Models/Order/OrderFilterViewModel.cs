using WebApi.Models.Product;

namespace WebApi.Models.Order;

public class OrderFilterViewModel {
	public int? Offset { get; set; }
	public int? Limit { get; set; }
}