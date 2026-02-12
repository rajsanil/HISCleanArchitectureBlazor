namespace HIS.MasterData.Application.Features.BloodGroups.Commands.AddEdit;

public class AddEditBloodGroupCommandValidator : AbstractValidator<AddEditBloodGroupCommand>
{
    public AddEditBloodGroupCommandValidator()
    {
        RuleFor(v => v.Code).MaximumLength(10).NotEmpty();
        RuleFor(v => v.Name).MaximumLength(100).NotEmpty();
        RuleFor(v => v.NameArabic).MaximumLength(100);
    }
}
