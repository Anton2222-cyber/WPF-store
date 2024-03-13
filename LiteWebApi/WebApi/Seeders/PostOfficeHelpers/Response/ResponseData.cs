using Newtonsoft.Json;

namespace WebApi.Seeders.PostOfficeHelpers.Response;

class ResponseData<T> {
	[JsonProperty(PropertyName = "success")]
	public string Success { get; set; } = null!;

	[JsonProperty(PropertyName = "data")]
	public ICollection<T> Data { get; set; } = null!;
}
