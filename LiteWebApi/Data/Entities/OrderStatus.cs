namespace Data.Entities;

public class OrderStatus {
	public long Id { get; set; }

	public string Status { get; set; } = null!;

	public DateTime TimeOfCreation { get; set; }
}