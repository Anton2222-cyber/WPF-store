using Data.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Settlement;

namespace WebApi.Validators.Settlement;

public class SettlementCreateValidator : AbstractValidator<CreateSettlementViewModel> {
	private readonly DataContext _context;

	public SettlementCreateValidator(DataContext context) {
		_context = context;

		RuleFor(s => s.Name)
			.NotNull()
				.WithMessage("Enter the name")
			.NotEmpty()
				.WithMessage("Enter the name")
			.MaximumLength(100)
				.WithMessage("Name is too long");

		RuleFor(s => s.AreaId)
			.MustAsync(IsAreaIdExists)
				.WithMessage("Area with this id is not exists");
	}

	private async Task<bool> IsAreaIdExists(long areaId, CancellationToken cancellationToken) =>
		await _context.Settlements.AnyAsync(s => s.AreaId == areaId, cancellationToken);
}
