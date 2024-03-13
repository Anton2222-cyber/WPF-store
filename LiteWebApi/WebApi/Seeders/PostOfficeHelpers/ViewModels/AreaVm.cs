namespace WebApi.Seeders.PostOfficeHelpers.ViewModels;

class AreaVm {
	public string Description { get; set; } = null!;

	public string Ref { get; set; } = null!;

	public List<SettlementVm> Settlements { get; set; } = null!;
}
