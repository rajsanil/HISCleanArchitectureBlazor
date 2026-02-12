namespace HIS.MasterData.Application.Features.Cities.Commands.AddEdit;

public class AddEditCityCommandValidator : AbstractValidator<AddEditCityCommand>
{
    public AddEditCityCommandValidator()
    {
        RuleFor(v => v.Code).MaximumLength(10).NotEmpty();
        RuleFor(v => v.Name).MaximumLength(200).NotEmpty();
        RuleFor(v => v.NameArabic).MaximumLength(200);
        RuleFor(v => v.CountryId).NotNull().WithMessage("Country is required.");
    }
}
