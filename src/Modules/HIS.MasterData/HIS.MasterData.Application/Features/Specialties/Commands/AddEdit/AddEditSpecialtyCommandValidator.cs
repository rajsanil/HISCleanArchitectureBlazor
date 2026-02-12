namespace HIS.MasterData.Application.Features.Specialties.Commands.AddEdit;

public class AddEditSpecialtyCommandValidator : AbstractValidator<AddEditSpecialtyCommand>
{
    public AddEditSpecialtyCommandValidator()
    {
        RuleFor(v => v.Code).MaximumLength(20).NotEmpty();
        RuleFor(v => v.Name).MaximumLength(200).NotEmpty();
        RuleFor(v => v.DepartmentId).GreaterThan(0).WithMessage("Department is required.");
    }
}
