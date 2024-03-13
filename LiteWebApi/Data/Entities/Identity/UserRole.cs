using Microsoft.AspNetCore.Identity;

namespace Data.Entities.Identity;

public class UserRole : IdentityUserRole<long> {
	public virtual User User { get; set; } = null!;
	public virtual Role Role { get; set; } = null!;
}
