using Data.Entities.Identity;

namespace Data.Entities;

public class BasketProduct {
	public long Id { get; set; }

	public long UserId { get; set; }
	public User User { get; set; } = null!;

	public long ProductId { get; set; }
	public Product Product { get; set; } = null!;

	public int Quantity { get; set; }
}
