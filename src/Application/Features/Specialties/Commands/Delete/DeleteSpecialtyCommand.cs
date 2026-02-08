using CleanArchitecture.Blazor.Application.Features.Specialties.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Specialties.Commands.Delete;

public class DeleteSpecialtyCommand : ICacheInvalidatorRequest<Result<int>>
{
    public DeleteSpecialtyCommand(int[] id) { Id = id; }
    public int[] Id { get; }
    public string CacheKey => SpecialtyCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => SpecialtyCacheKey.Tags;
}

public class DeleteSpecialtyCommandHandler : IRequestHandler<DeleteSpecialtyCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    public DeleteSpecialtyCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(DeleteSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Specialties.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            item.AddDomainEvent(new DeletedEvent<Specialty>(item));
            _context.Specialties.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }
}
