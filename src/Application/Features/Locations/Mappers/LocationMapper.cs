using CleanArchitecture.Blazor.Application.Features.Locations.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.Locations.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Locations.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class LocationMapper
{
    public static partial LocationDto ToDto(Location location);
    public static partial Location FromDto(LocationDto dto);
    public static partial Location FromEditCommand(AddEditLocationCommand command);
    public static partial void ApplyChangesFrom(AddEditLocationCommand command, Location location);
    [MapperIgnoreSource(nameof(LocationDto.Id))]
    public static partial AddEditLocationCommand CloneFromDto(LocationDto dto);
    public static partial AddEditLocationCommand ToEditCommand(LocationDto dto);
    public static partial IQueryable<LocationDto> ProjectTo(this IQueryable<Location> q);
}
