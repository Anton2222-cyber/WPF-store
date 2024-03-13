namespace WebApi.Models.Order;

public class CustomerInfo {
	public long Id { get; set; }

	public string FirstName { get; set; } = null!;

	public string LastName { get; set; } = null!;

	public string Photo { get; set; } = null!;
}