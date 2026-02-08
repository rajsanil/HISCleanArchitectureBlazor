namespace CleanArchitecture.Blazor.Application.Features.Visits.Commands.Register;

public class RegisterVisitCommandValidator : AbstractValidator<RegisterVisitCommand>
{
    public RegisterVisitCommandValidator()
    {
        RuleFor(v => v.PatientId).GreaterThan(0).WithMessage("Patient is required.");
        RuleFor(v => v.VisitType).MaximumLength(50).NotEmpty().WithMessage("Visit type is required.");
        RuleFor(v => v.FacilityId).GreaterThan(0).WithMessage("Facility is required.");
    }
}
