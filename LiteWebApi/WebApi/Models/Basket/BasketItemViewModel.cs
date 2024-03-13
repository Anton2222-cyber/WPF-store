using WebApi.Models.Product;

namespace WebApi.Models.Basket;

public class BasketItemViewModel {
	public long Id { get; set; }

	public ProductItemViewModel Product { get; set; } = null!;

	public int Quantity { get; set; }
}
