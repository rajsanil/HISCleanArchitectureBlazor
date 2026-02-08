namespace CleanArchitecture.Blazor.Application.Features.Facilities.Commands.AddEdit;

public class AddEditFacilityCommandValidator : AbstractValidator<AddEditFacilityCommand>
{
    public AddEditFacilityCommandValidator()
    {
        RuleFor(v => v.Code).MaximumLength(20).NotEmpty();
        RuleFor(v => v.Name).MaximumLength(200).NotEmpty();
        RuleFor(v => v.NameArabic).MaximumLength(200);
        RuleFor(v => v.LicenseNumber).MaximumLength(50);
        RuleFor(v => v.Phone).MaximumLength(20);
        RuleFor(v => v.Email).MaximumLength(100).EmailAddress().When(v => !string.IsNullOrEmpty(v.Email));
    }
}
