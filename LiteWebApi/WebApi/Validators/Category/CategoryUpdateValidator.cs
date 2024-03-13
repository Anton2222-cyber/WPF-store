using Data.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using WebApi.Models.Category;

namespace WebApi.Validators.Product;

public class CategoryUpdateValidator : AbstractValidator<CategoryUpdateViewModel> {
	private readonly DataContext _context;

	public CategoryUpdateValidator(DataContext context) {
		_context = context;

		RuleFor(c => c.Id)
			.MustAsync(IsCorrectId)
				.WithMessage("Id is not found");

		RuleFor(c => c)
			.Must(IsAnyNotNullValue)
				.WithMessage("No data to update")
			.MustAsync(IsUniqueNameAsync)
				.WithMessage("Category with this name is already exists");

		RuleFor(c => c.Name)
			.MaximumLength(100)
				.WithMessage("Name is too long");

		RuleFor(c => c.Image)
			.Must(IsValidImage)
				.WithMessage("The file is not an image or is corrupted");

		RuleFor(c => c.Description)
			.MaximumLength(4000)
				.WithMessage("Description is too long");
	}

	private bool IsAnyNotNullValue(CategoryUpdateViewModel model) {
		return AnyNotNull(model.Name, model.Image, model.Description);
	}

	private static bool AnyNotNull(params object?[] values) {
		return values.Any(v => v is not null);
	}

	private async Task<bool> IsCorrectId(int id, CancellationToken token) {
		return await _context.Categories
			.Where(c => !c.IsDeleted)
			.Where(c => c.Id == id)
			.AnyAsync(token);
	}

	private async Task<bool> IsUniqueNameAsync(CategoryUpdateViewModel model, CancellationToken cancellationToken) {
		if (model.Name is null) {
			return true;
		}

		return !await _context.Categories
			.Where(c => c.Id != model.Id)
			.Where(c => c.Name.ToLower().Equals(model.Name.ToLower()))
			.AnyAsync(cancellationToken);
	}

	private bool IsValidImage(IFormFile? image) {
		if (image is null)
			return true;

		using var stream = image.OpenReadStream();

		try {
			using var imageInstance = Image.Load(stream);
		}
		catch {
			return false;
		}
		return true;
	}
}