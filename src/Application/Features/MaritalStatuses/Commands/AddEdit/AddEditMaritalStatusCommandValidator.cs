namespace CleanArchitecture.Blazor.Application.Features.MaritalStatuses.Commands.AddEdit;

public class AddEditMaritalStatusCommandValidator : AbstractValidator<AddEditMaritalStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public AddEditMaritalStatusCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Code)
            .MaximumLength(10)
            .NotEmpty()
            .MustAsync(BeUniqueCode)
            .WithMessage("The Code '{PropertyValue}' already exists.");

        RuleFor(v => v.Name)
            .MaximumLength(200)
            .NotEmpty()
            .MustAsync(BeUniqueName)
            .WithMessage("The Marital Status '{PropertyValue}' already exists.");

        RuleFor(v => v.NameArabic)
            .MaximumLength(200);

        RuleFor(v => v.DisplayOrder)
            .GreaterThanOrEqualTo(0);
    }

    private async Task<bool> BeUniqueCode(AddEditMaritalStatusCommand command, string code, CancellationToken cancellationToken)
    {
        return !await _context.MaritalStatuses
            .Where(x => x.Id != command.Id)
            .AnyAsync(x => x.Code == code, cancellationToken);
    }

    private async Task<bool> BeUniqueName(AddEditMaritalStatusCommand command, string name, CancellationToken cancellationToken)
    {
        return !await _context.MaritalStatuses
            .Where(x => x.Id != command.Id)
            .AnyAsync(x => x.Name == name, cancellationToken);
    }
}
