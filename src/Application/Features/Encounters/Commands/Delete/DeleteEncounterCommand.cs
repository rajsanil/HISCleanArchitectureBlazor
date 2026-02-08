using CleanArchitecture.Blazor.Application.Features.Encounters.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Encounters.Commands.Delete;

public class DeleteEncounterCommand : ICacheInvalidatorRequest<Result<int>>
{
    public DeleteEncounterCommand(int[] id) { Id = id; }
    public int[] Id { get; }
    public string CacheKey => EncounterCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => EncounterCacheKey.Tags;
}

public class DeleteEncounterCommandHandler : IRequestHandler<DeleteEncounterCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    public DeleteEncounterCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(DeleteEncounterCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Encounters.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            item.AddDomainEvent(new DeletedEvent<Encounter>(item));
            _context.Encounters.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }
}
