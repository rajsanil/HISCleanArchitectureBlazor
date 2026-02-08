using CleanArchitecture.Blazor.Application.Features.Patients.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Patients.Commands.Delete;

public class DeletePatientCommand : ICacheInvalidatorRequest<Result<int>>
{
    public DeletePatientCommand(int[] id)
    {
        Id = id;
    }

    public int[] Id { get; }
    public string CacheKey => PatientCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => PatientCacheKey.Tags;
}

public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public DeletePatientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Patients.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            item.AddDomainEvent(new DeletedEvent<Patient>(item));
            _context.Patients.Remove(item);
        }

        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }
}
