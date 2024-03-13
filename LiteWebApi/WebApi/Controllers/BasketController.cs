using AutoMapper;
using Data.Context;
using Data.Entities;
using Data.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Basket;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = "Admin,User")]
public class BasketController(
	DataContext context,
	IIdentityService identityService,
	IMapper mapper
	) : ControllerBase {

	[HttpGet]
	public async Task<IActionResult> Get() {
		try {
			long userId = (await identityService.GetCurrentUserAsync(this)).Id;

			ICollection<BasketItemViewModel> basketItems = await context.BasketProducts
				.Include(bp => bp.Product)
					.ThenInclude(p => p.Images)
				.Include(bp => bp.Product)
					.ThenInclude(p => p.Category)
				.Where(bp => bp.UserId == userId)
				.Select(bp => mapper.Map<BasketItemViewModel>(bp))
				.ToArrayAsync();

			return Ok(basketItems);
		}
		catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpPost("{productId}")]
	public async Task<IActionResult> Create(long productId) {
		try {
			if (!await context.Products.Where(p => !p.IsDeleted).AnyAsync(p => p.Id == productId))
				throw new Exception("Product with this id is not exists");

			long userId = (await identityService.GetCurrentUserAsync(this)).Id;

			bool isProductAlreadyInBasket = await context.BasketProducts
				.AnyAsync(bp => bp.ProductId == productId && bp.UserId == userId);

			if (isProductAlreadyInBasket)
				throw new Exception("Product is already in basket");

			BasketProduct basketProduct = new() {
				ProductId = productId,
				UserId = userId,
				Quantity = 1
			};

			await context.BasketProducts
				.AddAsync(basketProduct);

			await context.SaveChangesAsync();

			return Ok();
		}
		catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpPatch("{productId} {quantity}")]
	public async Task<IActionResult> SetQuantity(long productId, int quantity) {
		try {
			if (quantity < 1)
				throw new Exception("Invalid quantity value");

			long userId = (await identityService.GetCurrentUserAsync(this)).Id;

			var basketProduct = await context.BasketProducts
				.FirstOrDefaultAsync(bp => bp.ProductId == productId && bp.UserId == userId)
				?? throw new Exception("Product with this id is not contains in basket");

			basketProduct.Quantity = quantity;

			await context.SaveChangesAsync();

			return Ok();
		}
		catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpDelete("{productId}")]
	public async Task<IActionResult> Delete(long productId) {
		try {
			long userId = (await identityService.GetCurrentUserAsync(this)).Id;

			var itemForDelete = await context.BasketProducts
				.FirstOrDefaultAsync(bp => bp.ProductId == productId && bp.UserId == userId);

			if (itemForDelete is null)
				return Ok();

			context.BasketProducts.Remove(itemForDelete);

			await context.SaveChangesAsync();

			return Ok();
		}
		catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpGet]
	public async Task<IActionResult> GetTotalPrice() {
		User user = await identityService.GetCurrentUserAsync(this);

		var totalPrice = await context.BasketProducts
			.Include(bp => bp.Product)
			.Where(bp => bp.UserId == user.Id)
			.SumAsync(bp => bp.Product.Price * bp.Quantity);

		return Ok(new TotalPriceViewModel { TotalPrice = totalPrice });
	}
}
