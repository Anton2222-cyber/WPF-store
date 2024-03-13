namespace Data.Entities;

public class OrderedProduct {
	public long Id { get; set; }

	public long ProductId { get; set; }
	public Product Product { get; set; } = null!;

	public double UnitPrice { get; set; }

	public int Quantity { get; set; }

	public long OrderId { get; set; }
	public Order Order { get; set; } = null!;
}
