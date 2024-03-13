using AutoMapper;
using Data.Context;
using Data.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Settlement;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class SettlementsController(
	DataContext context,
	IMapper mapper,
	IValidator<CreateSettlementViewModel> createValidator,
	IValidator<UpdateSettlementViewModel> updateValidator
	) : ControllerBase {

	[HttpGet("{areaId}")]
	[Authorize(Roles = "Admin,User")]
	public async Task<IActionResult> GetByAreaId(long areaId) {
		var settlement = await context.Settlements
			.Where(s => s.AreaId == areaId)
			.Select(s => mapper.Map<SettlementItemViewModel>(s))
			.ToArrayAsync();

		return Ok(settlement);
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Create([FromForm] CreateSettlementViewModel model) {
		var validationResult = await createValidator.ValidateAsync(model);

		if (!validationResult.IsValid)
			return BadRequest(validationResult.Errors.First().ErrorMessage);

		var settlement = mapper.Map<Settlement>(model);
		await context.Settlements.AddAsync(settlement);
		await context.SaveChangesAsync();

		return Ok();
	}

	[HttpPut]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Update(UpdateSettlementViewModel model) {
		var validationResult = await updateValidator.ValidateAsync(model);

		if (!validationResult.IsValid)
			return BadRequest(validationResult.Errors.First().ErrorMessage);

		var settlement = await context.Settlements
			.FirstAsync(s => s.Id == model.Id);

		settlement.Name = model.Name;
		settlement.AreaId = model.AreaId;
		await context.SaveChangesAsync();

		return Ok();
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Delete(long id) {
		var settlements = await context.Settlements
			.FirstOrDefaultAsync(s => s.Id == id);

		if (settlements is null)
			return Ok();

		context.Settlements.Remove(settlements);
		await context.SaveChangesAsync();

		return Ok();
	}
}
