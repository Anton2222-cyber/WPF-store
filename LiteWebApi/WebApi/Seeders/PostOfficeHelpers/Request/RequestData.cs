using Newtonsoft.Json;

namespace WebApi.Seeders.PostOfficeHelpers.Request;

class RequestData {
	[JsonProperty(PropertyName = "apiKey")]
	public string ApiKey { get; set; } = null!;
	[JsonProperty(PropertyName = "modelName")]
	public string ModelName { get; set; } = null!;
	[JsonProperty(PropertyName = "calledMethod")]
	public string CalledMethod { get; set; } = null!;
	[JsonProperty(PropertyName = "methodProperties")]
	public Dictionary<string, string> MethodProperties { get; set; } = null!;
}
