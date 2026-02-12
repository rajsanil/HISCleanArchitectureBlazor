namespace HIS.MasterData.Application.Features.Countries.Commands.AddEdit;

public class AddEditCountryCommandValidator : AbstractValidator<AddEditCountryCommand>
{
    public AddEditCountryCommandValidator()
    {
        RuleFor(v => v.Code).MaximumLength(10).NotEmpty();
        RuleFor(v => v.Name).MaximumLength(200).NotEmpty();
        RuleFor(v => v.NameArabic).MaximumLength(200);
        RuleFor(v => v.Iso2Code).MaximumLength(2);
        RuleFor(v => v.Iso3Code).MaximumLength(3);
        RuleFor(v => v.PhoneCode).MaximumLength(10);
    }
}
