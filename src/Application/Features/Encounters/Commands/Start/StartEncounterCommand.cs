using CleanArchitecture.Blazor.Application.Features.Encounters.Caching;
using CleanArchitecture.Blazor.Application.Features.Encounters.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Encounters.Commands.Start;

/// <summary>
/// Starts a new clinical encounter for a visit. Generates a unique encounter number.
/// </summary>
public class StartEncounterCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int Id { get; set; }
    public int VisitId { get; set; }
    public string EncounterType { get; set; } = string.Empty;
    public int? DoctorId { get; set; }
    public int? DepartmentId { get; set; }
    public int? LocationId { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? Notes { get; set; }

    public string CacheKey => EncounterCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => EncounterCacheKey.Tags;
}

public class StartEncounterCommandValidator : AbstractValidator<StartEncounterCommand>
{
    public StartEncounterCommandValidator()
    {
        RuleFor(v => v.VisitId).GreaterThan(0).WithMessage("Visit is required.");
        RuleFor(v => v.EncounterType).MaximumLength(50).NotEmpty().WithMessage("Encounter type is required.");
    }
}

public class StartEncounterCommandHandler : IRequestHandler<StartEncounterCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IEncounterNumberGenerator _encounterNumberGenerator;

    public StartEncounterCommandHandler(IApplicationDbContext context, IEncounterNumberGenerator encounterNumberGenerator)
    {
        _context = context;
        _encounterNumberGenerator = encounterNumberGenerator;
    }

    public async Task<Result<int>> Handle(StartEncounterCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            // Update existing encounter
            var item = await _context.Encounters.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (item == null)
                return await Result<int>.FailureAsync($"Encounter with id: [{request.Id}] not found.");
            EncounterMapper.ApplyChangesFrom(request, item);
            item.AddDomainEvent(new UpdatedEvent<Encounter>(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            // Verify visit exists
            var visit = await _context.Visits.SingleOrDefaultAsync(v => v.Id == request.VisitId, cancellationToken);
            if (visit == null)
                return await Result<int>.FailureAsync($"Visit with id: [{request.VisitId}] not found.");

            var encounter = EncounterMapper.FromStartCommand(request);
            encounter.EncounterNumber = await _encounterNumberGenerator.GenerateNextEncounterNumberAsync(cancellationToken);
            encounter.EncounterStatus = "InProgress";
            encounter.StartDate = DateTime.UtcNow;
            encounter.TenantId = visit.TenantId;
            encounter.AddDomainEvent(new CreatedEvent<Encounter>(encounter));
            _context.Encounters.Add(encounter);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(encounter.Id);
        }
    }
}
