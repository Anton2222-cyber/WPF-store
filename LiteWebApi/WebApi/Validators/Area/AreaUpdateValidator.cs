using Data.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Areas;

namespace WebApi.Validators.Area;

public class AreaUpdateValidator : AbstractValidator<UpdateAreaViewModel> {
	private readonly DataContext _context;

	public AreaUpdateValidator(DataContext context) {
		_context = context;

		RuleFor(a => a.Id)
			.MustAsync(IsIdValid)
				.WithMessage("Area with this id is not exists");

		RuleFor(a => a.Name)
			.NotNull()
				.WithMessage("Enter the name")
			.NotEmpty()
				.WithMessage("Enter the name")
			.MaximumLength(100)
				.WithMessage("Name is too long");
	}

	private async Task<bool> IsIdValid(long id, CancellationToken cancellationToken) =>
		await _context.Areas.AnyAsync(a => a.Id == id, cancellationToken);
}