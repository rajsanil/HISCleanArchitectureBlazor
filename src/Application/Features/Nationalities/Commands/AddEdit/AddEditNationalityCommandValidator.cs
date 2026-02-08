namespace CleanArchitecture.Blazor.Application.Features.Nationalities.Commands.AddEdit;

public class AddEditNationalityCommandValidator : AbstractValidator<AddEditNationalityCommand>
{
    private readonly IApplicationDbContext _context;

    public AddEditNationalityCommandValidator(IApplicationDbContext context)
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
            .WithMessage("The Name '{PropertyValue}' already exists.");

        RuleFor(v => v.NameArabic)
            .MaximumLength(200);
    }

    private async Task<bool> BeUniqueCode(AddEditNationalityCommand command, string code, CancellationToken cancellationToken)
    {
        return !await _context.Nationalities
            .Where(x => x.Id != command.Id)
            .AnyAsync(x => x.Code == code, cancellationToken);
    }

    private async Task<bool> BeUniqueName(AddEditNationalityCommand command, string name, CancellationToken cancellationToken)
    {
        return !await _context.Nationalities
            .Where(x => x.Id != command.Id)
            .AnyAsync(x => x.Name == name, cancellationToken);
    }
}
