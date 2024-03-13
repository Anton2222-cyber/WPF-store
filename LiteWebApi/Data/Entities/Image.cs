namespace Data.Entities;

public class Image {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public int Order { get; set; }

	public long ProductId { get; set; }
	public Product Product { get; set; } = null!;
}