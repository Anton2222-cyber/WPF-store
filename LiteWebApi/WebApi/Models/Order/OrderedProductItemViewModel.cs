using WebApi.Models.Product;

namespace WebApi.Models.Order;

public class OrderedProductItemViewModel {
	public long Id { get; set; }

	public ProductItemViewModel Product { get; set; } = null!;

	public double UnitPrice { get; set; }

	public int Quantity { get; set; }
}
