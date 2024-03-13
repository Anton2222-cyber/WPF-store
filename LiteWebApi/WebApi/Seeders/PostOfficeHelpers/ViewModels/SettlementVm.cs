namespace WebApi.Seeders.PostOfficeHelpers.ViewModels;

class SettlementVm {
	public string Description { get; set; } = null!;

	public string Area { get; set; } = null!;

	public string Ref { get; set; } = null!;

	public List<PostOfficeVm> PostOffices { get; set; } = null!;
}
