namespace CleanArchitecture.Blazor.Application.Features.Patients.Commands.AddEdit;

public class AddEditPatientCommandValidator : AbstractValidator<AddEditPatientCommand>
{
    public AddEditPatientCommandValidator()
    {
        RuleFor(v => v.FirstName)
            .MaximumLength(100)
            .NotEmpty();
        RuleFor(v => v.LastName)
            .MaximumLength(100)
            .NotEmpty();
        RuleFor(v => v.DateOfBirth)
            .NotEmpty()
            .LessThan(DateTime.Today.AddDays(1))
            .WithMessage("Date of birth must be in the past.");
        RuleFor(v => v.Gender)
            .MaximumLength(20)
            .NotEmpty();
        RuleFor(v => v.Phone)
            .MaximumLength(20);
        RuleFor(v => v.Email)
            .MaximumLength(100)
            .EmailAddress()
            .When(v => !string.IsNullOrEmpty(v.Email));
        RuleFor(v => v.EmiratesId)
            .MaximumLength(20);
        RuleFor(v => v.PassportNumber)
            .MaximumLength(20);
        RuleFor(v => v.Address)
            .MaximumLength(500);
    }
}
