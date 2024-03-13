using Data.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Product;
using WebApi.Services;

namespace WebApi.Validators.Product;

public class ProductPutValidator : AbstractValidator<ProductPutViewModel> {
	private readonly DataContext _context;

	public ProductPutValidator(DataContext context) {
		_context = context;

		RuleFor(p => p.Id)
			.Must(id => id != 0)
				.WithMessage("Id must not be equals 0")
			.MustAsync(IsIdExists)
				.WithMessage("Product with this id is not exists");

		RuleFor(p => p.Name)
			.NotNull()
				.WithMessage("Enter the name")
			.NotEmpty()
				.WithMessage("Enter the name")
			.MaximumLength(100)
				.WithMessage("Name is too long");

		RuleForEach(p => p.Images)
			.NotNull()
				.WithMessage("Images is not selected")
			.NotEmpty()
				.WithMessage("Image is not selected")
			.Must(ImageValidator.IsValidImage)
				.WithMessage("There is files that are not an image or are corrupted");

		RuleFor(p => p.Description)
			.MaximumLength(4000)
				.WithMessage("Description is too long");

		RuleFor(p => p.Price)
			.GreaterThanOrEqualTo(0)
				.WithMessage("Price cannot be negative");

		RuleFor(p => p.CategoryId)
			.Must(categoryId => categoryId != 0)
				.WithMessage("Category id must not be equals 0")
			.MustAsync(IsCategoryIdExists)
				.WithMessage("Category with this id is not exists");
	}

	private async Task<bool> IsIdExists(long id, CancellationToken cancellationToken) =>
		await _context.Products.AnyAsync(p => p.Id == id && !p.IsDeleted, cancellationToken);

	private async Task<bool> IsCategoryIdExists(long categoryId, CancellationToken cancellationToken) =>
		await _context.Categories.AnyAsync(c => c.Id == categoryId && !c.IsDeleted, cancellationToken);
}