namespace Data.Entities;

public class Settlement {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public ICollection<PostOffice> PostOffices { get; set; } = null!;

	public long AreaId { get; set; }
	public Area Area { get; set; } = null!;
}
