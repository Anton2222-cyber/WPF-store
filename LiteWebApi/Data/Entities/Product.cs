namespace Data.Entities;

public class Product {
	public long Id { get; set; }

	public bool IsDeleted { get; set; } = false;

	public DateTime DateCreated { get; set; }

	public string Name { get; set; } = null!;

	public string Description { get; set; } = null!;

	public double Price { get; set; }

	public long CategoryId { get; set; }
	public Category Category { get; set; } = null!;

	public ICollection<Image> Images { get; set; } = null!;

	public ICollection<BasketProduct> BasketProducts { get; set; } = null!;

	public ICollection<OrderedProduct> OrderedProducts { get; set; } = null!;
}