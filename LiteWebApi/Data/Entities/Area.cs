namespace Data.Entities;

public class Area {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public ICollection<Settlement> Settlements { get; set; } = null!;
}