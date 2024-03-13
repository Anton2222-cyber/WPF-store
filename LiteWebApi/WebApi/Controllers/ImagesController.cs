using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ImagesController : ControllerBase {
	[HttpGet("{name}")]
	public async Task<IActionResult> Get(string name) {
		if (!ImageWorker.IsImageExists(name)) {
			return BadRequest("File with this name is not exists");
		}

		var bytes = await ImageWorker.LoadBytesAsync(name);

		return File(bytes, "application/octet-stream");
	}
}
