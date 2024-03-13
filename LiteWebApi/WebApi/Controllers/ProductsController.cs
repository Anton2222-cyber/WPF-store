using AutoMapper;
using Data.Context;
using Data.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Product;
using WebApi.Services;

namespace WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ProductsController(
	DataContext context,
	IMapper mapper,
	IValidator<ProductCreateViewModel> createValidator,
	IValidator<ProductPutViewModel> putValidator
	) : Controller {

	[HttpGet]
	public async Task<IActionResult> GetAll() {
		var productEntities = await context.Products
			.Include(p => p.Category)
			.Include(p => p.Images.OrderBy(i => i.Order))
			.Where(p => !p.IsDeleted)
			.OrderBy(p => p.Id)
			.Select(p => mapper.Map<ProductItemViewModel>(p))
			.ToArrayAsync();

		return Ok(productEntities);
	}

	[HttpGet]
	public async Task<IActionResult> GetFiltered([FromQuery] ProductFilterViewModel filter) {
		var products = context.Products
			.Include(p => p.Category)
			.Include(p => p.Images.OrderBy(i => i.Order))
			.OrderBy(p => p.Id)
			.Where(p => !p.IsDeleted);

		if (filter.CategoryId is not null) {
			products = products
				.Where(p => p.CategoryId == filter.CategoryId);
		}

		if (filter.Name is not null) {
			products = products
				.Where(p => p.Name.ToLower().Contains(filter.Name.ToLower()));
		}

		if (filter.Description is not null) {
			products = products
				.Where(p => p.Description.ToLower().Contains(filter.Description.ToLower()));
		}

		if (filter.MinPrice is not null) {
			products = products
				.Where(p => p.Price >= filter.MinPrice);
		}

		if (filter.MaxPrice is not null) {
			products = products
				.Where(p => p.Price <= filter.MaxPrice);
		}

		long availableQuantity = products.Count();

		if (filter.Offset is not null) {
			products = products.Skip((int)filter.Offset);
		}

		if (filter.Limit is not null) {
			products = products.Take((int)filter.Limit);
		}

		var list = await products
			.Select(p => mapper.Map<ProductItemViewModel>(p))
			.ToListAsync();

		return Ok(new FilteredProductsViewModel {
			FilteredProducts = list,
			AvailableQuantity = availableQuantity
		});
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(long id) {
		if (!await context.Products.AnyAsync(p => p.Id == id))
			return BadRequest("Product with this id is not exists");

		var productEntities = await context.Products
			.Include(p => p.Category)
			.Include(p => p.Images.OrderBy(i => i.Order))
			.Where(p => !p.IsDeleted)
			.OrderBy(p => p.Id)
			.FirstAsync(p => p.Id == id);

		var productVm = mapper.Map<ProductItemViewModel>(productEntities);

		return Ok(productVm);
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Create([FromForm] ProductCreateViewModel model) {
		var validationResult = await createValidator.ValidateAsync(model);

		if (!validationResult.IsValid)
			return BadRequest(validationResult.Errors.First().ErrorMessage);

		var product = mapper.Map<Product>(model);

		product.DateCreated = DateTime.Now;

		var pathOfImages = await ImageWorker.SaveImagesAsync(model.Images);
		int order = 0;
		product.Images = pathOfImages
			.Select(i => new Image {
				Name = i,
				ProductId = product.Id,
				Order = ++order
			})
			.ToArray();

		try {
			await context.Products.AddAsync(product);
			await context.SaveChangesAsync();
		}
		catch (Exception) {
			ImageWorker.DeleteImagesIfExists(pathOfImages);
			throw;
		}

		return Ok();
	}

	[HttpPut]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Put([FromForm] ProductPutViewModel model) {
		var validationResult = await putValidator.ValidateAsync(model);

		if (!validationResult.IsValid)
			return BadRequest(validationResult.Errors.First().ErrorMessage);

		var product = mapper.Map<Product>(model);

		var pathOfNewImages = await ImageWorker.SaveImagesAsync(model.Images);
		int order = 0;
		product.Images = pathOfNewImages
			.Select(i => new Image {
				Name = i,
				ProductId = product.Id,
				Order = ++order
			})
			.ToArray();

		try {
			var productEntity = await context.Products
			.Include(p => p.Images)
			.FirstAsync(p => p.Id == model.Id && !p.IsDeleted);

			var pathOfOldImages = productEntity.Images.Select(i => i.Name).ToArray();

			productEntity.Name = product.Name;
			productEntity.Description = product.Description;
			productEntity.Price = product.Price;
			productEntity.CategoryId = product.CategoryId;
			productEntity.Images.Clear();
			foreach (var image in product.Images)
				productEntity.Images.Add(image);

			await context.SaveChangesAsync();

			ImageWorker.DeleteImagesIfExists(pathOfOldImages);
		}
		catch (Exception) {
			ImageWorker.DeleteImagesIfExists(pathOfNewImages);
			throw;
		}

		return Ok();
	}
}
