using Data.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Category;
using WebApi.Services;

namespace WebApi.Validators.Product;

public class CategoryCreateValidator : AbstractValidator<CategoryCreateViewModel> {
	private readonly DataContext _context;

	public CategoryCreateValidator(DataContext context) {
		_context = context;

		RuleFor(c => c.Name)
			.NotNull()
				.WithMessage("Enter the name")
			.NotEmpty()
				.WithMessage("Enter the name")
			.MaximumLength(100)
				.WithMessage("Name is too long")
			.MustAsync(IsUniqueNameAsync)
				.WithMessage("Category with this name is already exists");

		RuleFor(c => c.Image)
			.NotNull()
				.WithMessage("Image is not selected")
			.NotEmpty()
				.WithMessage("Image is not valid")
			.Must(ImageValidator.IsValidImage)
				.WithMessage("The file is not an image or is corrupted");

		RuleFor(c => c.Description)
			.MaximumLength(4000)
				.WithMessage("Description is too long");
	}

	private async Task<bool> IsUniqueNameAsync(string name, CancellationToken cancellationToken) {
		return !await _context.Categories
			.Where(c => c.Name.ToLower().Equals(name.ToLower()))
			.AnyAsync(cancellationToken);
	}
}
