using CleanArchitecture.Blazor.Application.Features.Beds.Caching;
using CleanArchitecture.Blazor.Application.Features.Beds.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Beds.Commands.AddEdit;

public class AddEditBedCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public int RoomId { get; set; }
    public string BedStatus { get; set; } = "Available";
    public bool IsActive { get; set; } = true;

    public string CacheKey => BedCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => BedCacheKey.Tags;
}

public class AddEditBedCommandHandler : IRequestHandler<AddEditBedCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    public AddEditBedCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(AddEditBedCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.Beds.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (item == null)
                return await Result<int>.FailureAsync($"Bed with id: [{request.Id}] not found.");
            BedMapper.ApplyChangesFrom(request, item);
            item.AddDomainEvent(new UpdatedEvent<Bed>(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = BedMapper.FromEditCommand(request);
            item.AddDomainEvent(new CreatedEvent<Bed>(item));
            _context.Beds.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
    }
}
