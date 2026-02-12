namespace HIS.MasterData.Application.Features.Nationalities.Commands.AddEdit;

public class AddEditNationalityCommandValidator : AbstractValidator<AddEditNationalityCommand>
{
    public AddEditNationalityCommandValidator()
    {
        RuleFor(v => v.Code).MaximumLength(10).NotEmpty();
        RuleFor(v => v.Name).MaximumLength(200).NotEmpty();
        RuleFor(v => v.NameArabic).MaximumLength(200);
    }
}
