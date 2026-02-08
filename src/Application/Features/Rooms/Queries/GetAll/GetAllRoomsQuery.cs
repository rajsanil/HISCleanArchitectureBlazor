using CleanArchitecture.Blazor.Application.Features.Rooms.Caching;
using CleanArchitecture.Blazor.Application.Features.Rooms.DTOs;
using CleanArchitecture.Blazor.Application.Features.Rooms.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Rooms.Queries.GetAll;

public class GetAllRoomsQuery : ICacheableRequest<IEnumerable<RoomDto>>
{
    public string CacheKey => RoomCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => RoomCacheKey.Tags;
}

public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, IEnumerable<RoomDto>>
{
    private readonly IApplicationDbContext _context;
    public GetAllRoomsQueryHandler(IApplicationDbContext context) { _context = context; }

    public async Task<IEnumerable<RoomDto>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Rooms.ProjectTo().ToListAsync(cancellationToken);
    }
}
