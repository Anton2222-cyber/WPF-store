using FluentValidation;
using WebApi.Models.Areas;

namespace WebApi.Validators.Area;

public class AreaCreateValidator : AbstractValidator<CreateAreaViewModel> {
	public AreaCreateValidator() {
		RuleFor(a => a.Name)
			.NotNull()
				.WithMessage("Enter the name")
			.NotEmpty()
				.WithMessage("Enter the name")
			.MaximumLength(100)
				.WithMessage("Name is too long");
	}
}
