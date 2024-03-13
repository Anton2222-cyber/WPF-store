namespace WebApi.Seeders.PostOfficeHelpers.ViewModels;

class PostOfficeVm {
	public string Description { get; set; } = null!;

	public string CityRef { get; set; } = null!;

	public float Longitude { get; set; }

	public float Latitude { get; set; }
}