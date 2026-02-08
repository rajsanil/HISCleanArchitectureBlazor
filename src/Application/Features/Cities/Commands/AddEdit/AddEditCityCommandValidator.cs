namespace CleanArchitecture.Blazor.Application.Features.Cities.Commands.AddEdit;

public class AddEditCityCommandValidator : AbstractValidator<AddEditCityCommand>
{
    private readonly IApplicationDbContext _context;

    public AddEditCityCommandValidator(IApplicationDbContext context)
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
            .WithMessage("The Name '{PropertyValue}' already exists for this country.");

        RuleFor(v => v.NameArabic)
            .MaximumLength(200);
    }

    private async Task<bool> BeUniqueCode(AddEditCityCommand command, string code, CancellationToken cancellationToken)
    {
        return !await _context.Cities
            .Where(x => x.Id != command.Id)
            .AnyAsync(x => x.Code == code, cancellationToken);
    }

    private async Task<bool> BeUniqueName(AddEditCityCommand command, string name, CancellationToken cancellationToken)
    {
        return !await _context.Cities
            .Where(x => x.Id != command.Id && x.CountryId == command.CountryId)
            .AnyAsync(x => x.Name == name, cancellationToken);
    }
}
