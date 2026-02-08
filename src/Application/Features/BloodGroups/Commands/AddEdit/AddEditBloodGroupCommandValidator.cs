namespace CleanArchitecture.Blazor.Application.Features.BloodGroups.Commands.AddEdit;

public class AddEditBloodGroupCommandValidator : AbstractValidator<AddEditBloodGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public AddEditBloodGroupCommandValidator(IApplicationDbContext context)
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
            .WithMessage("The Blood Group '{PropertyValue}' already exists.");

        RuleFor(v => v.NameArabic)
            .MaximumLength(200);

        RuleFor(v => v.DisplayOrder)
            .GreaterThanOrEqualTo(0);
    }

    private async Task<bool> BeUniqueCode(AddEditBloodGroupCommand command, string code, CancellationToken cancellationToken)
    {
        return !await _context.BloodGroups
            .Where(x => x.Id != command.Id)
            .AnyAsync(x => x.Code == code, cancellationToken);
    }

    private async Task<bool> BeUniqueName(AddEditBloodGroupCommand command, string name, CancellationToken cancellationToken)
    {
        return !await _context.BloodGroups
            .Where(x => x.Id != command.Id)
            .AnyAsync(x => x.Name == name, cancellationToken);
    }
}
