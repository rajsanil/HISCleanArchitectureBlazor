using CleanArchitecture.Blazor.Application.Features.Encounters.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Encounters.Commands.End;

/// <summary>
/// Ends an active encounter by setting its EndDate and status to "Completed".
/// </summary>
public class EndEncounterCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int EncounterId { get; set; }
    public string? Notes { get; set; }

    public string CacheKey => EncounterCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => EncounterCacheKey.Tags;
}

public class EndEncounterCommandValidator : AbstractValidator<EndEncounterCommand>
{
    public EndEncounterCommandValidator()
    {
        RuleFor(v => v.EncounterId).GreaterThan(0);
    }
}

public class EndEncounterCommandHandler : IRequestHandler<EndEncounterCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    public EndEncounterCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(EndEncounterCommand request, CancellationToken cancellationToken)
    {
        var encounter = await _context.Encounters.SingleOrDefaultAsync(x => x.Id == request.EncounterId, cancellationToken);
        if (encounter == null)
            return await Result<int>.FailureAsync($"Encounter with id: [{request.EncounterId}] not found.");
        if (encounter.EncounterStatus == "Completed")
            return await Result<int>.FailureAsync("Encounter is already completed.");

        encounter.EndDate = DateTime.UtcNow;
        encounter.EncounterStatus = "Completed";
        if (!string.IsNullOrEmpty(request.Notes))
            encounter.Notes = request.Notes;
        encounter.AddDomainEvent(new UpdatedEvent<Encounter>(encounter));

        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(encounter.Id);
    }
}
