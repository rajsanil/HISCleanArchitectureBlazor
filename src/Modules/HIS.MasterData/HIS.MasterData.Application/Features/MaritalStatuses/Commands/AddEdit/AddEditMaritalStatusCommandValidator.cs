namespace HIS.MasterData.Application.Features.MaritalStatuses.Commands.AddEdit;

public class AddEditMaritalStatusCommandValidator : AbstractValidator<AddEditMaritalStatusCommand>
{
    public AddEditMaritalStatusCommandValidator()
    {
        RuleFor(v => v.Code).MaximumLength(10).NotEmpty();
        RuleFor(v => v.Name).MaximumLength(100).NotEmpty();
        RuleFor(v => v.NameArabic).MaximumLength(100);
    }
}
