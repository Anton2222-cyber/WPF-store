using AutoMapper;
using Data.Context;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.PostOffice;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PostOfficeController(
	DataContext context,
	IMapper mapper
	) : ControllerBase {

	[HttpGet("{settlementId}")]
	[Authorize(Roles = "Admin,User")]
	public async Task<IActionResult> GetBySettlementId(long settlementId) {
		var postOffices = await context.PostOffices
			.Where(po => po.SettlementId == settlementId)
			.Select(po => mapper.Map<PostOfficeItemViewModel>(po))
			.ToArrayAsync();

		return Ok(postOffices);
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Create([FromForm] CreatePostOfficeViewModel model) {
		try {
			var postOfice = mapper.Map<PostOffice>(model);
			await context.AddAsync(postOfice);
			await context.SaveChangesAsync();

			return Ok();
		}
		catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Delete(long id) {
		var postOffice = await context.PostOffices
			.FirstOrDefaultAsync(po => po.Id == id);

		if (postOffice is null)
			return Ok();

		context.PostOffices.Remove(postOffice);
		await context.SaveChangesAsync();

		return Ok();
	}
}
