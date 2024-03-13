namespace WebApi.Models.Settlement;

public class CreateSettlementViewModel {
	public string Name { get; set; } = null!;
	public long AreaId { get; set; }
}
