namespace WebApi.Seeders.PostOfficeHelpers.Response;

class AggregateResponse {
	public List<AreaData> Areas { get; set; } = null!;
	public List<SettlementData> Settlements { get; set; } = null!;
	public List<PostOfficeData> PostOffices { get; set; } = null!;
}
