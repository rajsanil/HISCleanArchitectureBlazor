namespace HIS.MasterData.Application.Features.Departments.Commands.AddEdit;

public class AddEditDepartmentCommandValidator : AbstractValidator<AddEditDepartmentCommand>
{
    public AddEditDepartmentCommandValidator()
    {
        RuleFor(v => v.Code).MaximumLength(20).NotEmpty();
        RuleFor(v => v.Name).MaximumLength(200).NotEmpty();
        RuleFor(v => v.FacilityId).GreaterThan(0).WithMessage("Facility is required.");
    }
}
