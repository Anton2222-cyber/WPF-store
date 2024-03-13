using AutoMapper;
using Data.Context;
using Data.Entities;
using Data.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Order;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class OrderController(
	DataContext context,
	IIdentityService identityService,
	IMapper mapper
	) : ControllerBase {

	[HttpGet]
	[Authorize(Roles = "Admin,User")]
	public async Task<IActionResult> GetFiltered([FromQuery] OrderFilterViewModel filter) {
		User user = await identityService.GetCurrentUserAsync(this);

		IQueryable<Order> query = context.Orders
			.Include(o => o.Status)
			.Include(o => o.PostOffice)
				.ThenInclude(po => po.Settlement)
					.ThenInclude(s => s.Area)
			.Include(o => o.OrderedProducts)
				.ThenInclude(op => op.Product)
					.ThenInclude(p => p.Category)
			.Include(o => o.OrderedProducts)
				.ThenInclude(op => op.Product)
					.ThenInclude(p => p.Images)
			.Where(o => o.UserId == user.Id)
			.OrderByDescending(o => o.TimeOfCreation);

		long availableQuantity = query.Count();

		if (filter.Offset is not null)
			query = query.Skip((int)filter.Offset);

		if (filter.Limit is not null)
			query = query.Take((int)filter.Limit);

		var ordersList = await query
			.Select(o => mapper.Map<OrderItemViewModel>(o))
			.ToArrayAsync();

		return Ok(new FilteredOrdersViewModel {
			FilteredOrders = ordersList,
			AvailableQuantity = availableQuantity
		});
	}

	[HttpGet]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> GetCustomersOrdersFiltered([FromQuery] OrderFilterViewModel filter) {
		IQueryable<Order> query = context.Orders
			.Include(o => o.Status)
			.Include(o => o.PostOffice)
				.ThenInclude(po => po.Settlement)
					.ThenInclude(s => s.Area)
			.Include(o => o.OrderedProducts)
				.ThenInclude(op => op.Product)
					.ThenInclude(p => p.Category)
			.Include(o => o.OrderedProducts)
				.ThenInclude(op => op.Product)
					.ThenInclude(p => p.Images)
			.Include(o => o.User)
			.OrderByDescending(o => o.TimeOfCreation);

		long availableQuantity = query.Count();

		if (filter.Offset is not null)
			query = query.Skip((int)filter.Offset);

		if (filter.Limit is not null)
			query = query.Take((int)filter.Limit);

		var ordersList = await query
			.Select(o => mapper.Map<CustomerOrderItemViewModel>(o))
			.ToArrayAsync();

		return Ok(new FilteredCustomersOrdersViewModel {
			FilteredOrders = ordersList,
			AvailableQuantity = availableQuantity
		});
	}

	[HttpPost]
	[Authorize(Roles = "Admin,User")]
	public async Task<IActionResult> Order([FromForm] MakeOrderViewModel model) {
		try {
			if (!await context.PostOffices.AnyAsync(po => po.Id == model.PostOfficeId))
				throw new Exception("PostOffices with this id is not exists");

			User user = await identityService.GetCurrentUserAsync(this);

			if (!await context.BasketProducts.AnyAsync(bp => bp.UserId == user.Id))
				throw new Exception("Basket is empty");

			using (var transaction = await context.Database.BeginTransactionAsync()) {
				var status = new OrderStatus {
					Status = "It is being processed",
					TimeOfCreation = DateTime.Now
				};

				await context.OrderStatuses.AddAsync(status);
				await context.SaveChangesAsync();

				var order = new Order {
					UserId = user.Id,
					TimeOfCreation = DateTime.Now,
					PostOfficeId = model.PostOfficeId,
					StatusId = status.Id
				};

				await context.Orders.AddAsync(order);
				await context.SaveChangesAsync();

				var orderedProducts = await context.BasketProducts
					.Include(bp => bp.Product)
					.Where(bp => bp.UserId == user.Id)
					.Select(bp => mapper.Map<OrderedProduct>(bp))
					.ToListAsync();

				orderedProducts.ForEach(op => op.OrderId = order.Id);

				await context.OrderedProducts.AddRangeAsync(orderedProducts);
				await context.SaveChangesAsync();

				var basketProductsForDelete = await context.BasketProducts
					.Where(bp => bp.UserId == user.Id)
					.ToArrayAsync();

				context.BasketProducts.RemoveRange(basketProductsForDelete);
				await context.SaveChangesAsync();

				await transaction.CommitAsync();
			}

			return Ok();
		}
		catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}
}
