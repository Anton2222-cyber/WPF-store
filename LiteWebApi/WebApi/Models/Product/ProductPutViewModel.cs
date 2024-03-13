namespace WebApi.Models.Product;

public class ProductPutViewModel {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public string Description { get; set; } = null!;

	public double Price { get; set; }

	public long CategoryId { get; set; }

	public ICollection<IFormFile> Images { get; set; } = null!;
}
