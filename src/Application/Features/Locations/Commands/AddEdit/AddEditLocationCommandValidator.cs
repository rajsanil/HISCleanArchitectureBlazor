namespace CleanArchitecture.Blazor.Application.Features.Locations.Commands.AddEdit;

public class AddEditLocationCommandValidator : AbstractValidator<AddEditLocationCommand>
{
    public AddEditLocationCommandValidator()
    {
        RuleFor(v => v.Code).MaximumLength(20).NotEmpty();
        RuleFor(v => v.Name).MaximumLength(200).NotEmpty();
        RuleFor(v => v.LocationType).MaximumLength(50).NotEmpty();
        RuleFor(v => v.FacilityId).GreaterThan(0).WithMessage("Facility is required.");
        RuleFor(v => v.DepartmentId).GreaterThan(0).WithMessage("Department is required.");
    }
}
