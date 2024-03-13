namespace WebApi.Models.Settlement;

public class UpdateSettlementViewModel {
	public long Id { get; set; }
	public string Name { get; set; } = null!;
	public long AreaId { get; set; }
}