namespace HIS.MasterData.Application.Features.Rooms.Queries.GetAll;

/// <summary>
/// Temporary stub query for getting all rooms.
/// TODO: Implement cross-module service to fetch rooms from core application.
/// </summary>
public class GetAllRoomsQuery : IRequest<IEnumerable<HIS.MasterData.Application.Features.Rooms.DTOs.RoomDto>>
{
}

public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, IEnumerable<HIS.MasterData.Application.Features.Rooms.DTOs.RoomDto>>
{
    public Task<IEnumerable<HIS.MasterData.Application.Features.Rooms.DTOs.RoomDto>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement cross-module communication to fetch rooms
        return Task.FromResult(Enumerable.Empty<HIS.MasterData.Application.Features.Rooms.DTOs.RoomDto>());
    }
}
