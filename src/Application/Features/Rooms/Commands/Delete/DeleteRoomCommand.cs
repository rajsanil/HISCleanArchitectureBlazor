using CleanArchitecture.Blazor.Application.Features.Rooms.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Rooms.Commands.Delete;

public class DeleteRoomCommand : ICacheInvalidatorRequest<Result<int>>
{
    public DeleteRoomCommand(int[] id) { Id = id; }
    public int[] Id { get; }
    public string CacheKey => RoomCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => RoomCacheKey.Tags;
}

public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    public DeleteRoomCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Rooms.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            item.AddDomainEvent(new DeletedEvent<Room>(item));
            _context.Rooms.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }
}
