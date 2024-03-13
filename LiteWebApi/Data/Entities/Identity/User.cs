using Microsoft.AspNetCore.Identity;

namespace Data.Entities.Identity;

public class User : IdentityUser<long> {
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public string Photo { get; set; } = null!;
	public virtual ICollection<UserRole> UserRoles { get; set; } = null!;
	public virtual ICollection<BasketProduct> BasketProducts { get; set; } = null!;
	public virtual ICollection<Order> Orders { get; set; } = null!;
}
