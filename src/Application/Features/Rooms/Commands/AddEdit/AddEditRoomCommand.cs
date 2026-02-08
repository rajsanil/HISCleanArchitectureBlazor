using CleanArchitecture.Blazor.Application.Features.Rooms.Caching;
using CleanArchitecture.Blazor.Application.Features.Rooms.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Rooms.Commands.AddEdit;

public class AddEditRoomCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public int LocationId { get; set; }
    public bool IsActive { get; set; } = true;

    public string CacheKey => RoomCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => RoomCacheKey.Tags;
}

public class AddEditRoomCommandHandler : IRequestHandler<AddEditRoomCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    public AddEditRoomCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(AddEditRoomCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.Rooms.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (item == null)
                return await Result<int>.FailureAsync($"Room with id: [{request.Id}] not found.");
            RoomMapper.ApplyChangesFrom(request, item);
            item.AddDomainEvent(new UpdatedEvent<Room>(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = RoomMapper.FromEditCommand(request);
            item.AddDomainEvent(new CreatedEvent<Room>(item));
            _context.Rooms.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
    }
}
