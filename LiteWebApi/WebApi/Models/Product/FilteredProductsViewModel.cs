namespace WebApi.Models.Product;

public class FilteredProductsViewModel {
	public ICollection<ProductItemViewModel> FilteredProducts { get; set; } = null!;
	public long AvailableQuantity { get; set; }
}
