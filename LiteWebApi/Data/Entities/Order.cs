using Data.Entities.Identity;

namespace Data.Entities;

public class Order {
	public long Id { get; set; }

	public long UserId { get; set; }
	public User User { get; set; } = null!;

	public DateTime TimeOfCreation { get; set; }

	public long StatusId { get; set; }
	public OrderStatus Status { get; set; } = null!;

	public long PostOfficeId { get; set; }
	public PostOffice PostOffice { get; set; } = null!;

	public ICollection<OrderedProduct> OrderedProducts { get; set; } = null!;
}
