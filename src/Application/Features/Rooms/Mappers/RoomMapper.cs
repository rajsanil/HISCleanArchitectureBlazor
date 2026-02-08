using CleanArchitecture.Blazor.Application.Features.Rooms.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.Rooms.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Rooms.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class RoomMapper
{
    public static partial RoomDto ToDto(Room room);
    public static partial Room FromDto(RoomDto dto);
    public static partial Room FromEditCommand(AddEditRoomCommand command);
    public static partial void ApplyChangesFrom(AddEditRoomCommand command, Room room);
    [MapperIgnoreSource(nameof(RoomDto.Id))]
    public static partial AddEditRoomCommand CloneFromDto(RoomDto dto);
    public static partial AddEditRoomCommand ToEditCommand(RoomDto dto);
    public static partial IQueryable<RoomDto> ProjectTo(this IQueryable<Room> q);
}
