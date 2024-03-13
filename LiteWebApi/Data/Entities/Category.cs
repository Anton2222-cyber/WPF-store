namespace Data.Entities;

public class Category {
	public long Id { get; set; }

	public bool IsDeleted { get; set; } = false;

	public DateTime DateCreated { get; set; }

	public string Name { get; set; } = null!;

	public string Image { get; set; } = null!;

	public string Description { get; set; } = null!;

	public ICollection<Product> Products { get; set; } = null!;
}