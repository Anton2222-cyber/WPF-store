namespace Data.Entities;

public class PostOffice {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public float Longitude { get; set; }

	public float Latitude { get; set; }

	public long SettlementId { get; set; }
	public Settlement Settlement { get; set; } = null!;
}
