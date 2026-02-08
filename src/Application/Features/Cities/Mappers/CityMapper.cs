#nullable disable

using CleanArchitecture.Blazor.Application.Features.Cities.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.Cities.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Cities.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class CityMapper
{
    public static partial CityDto ToDto(City source);
    public static partial City FromDto(CityDto dto);
    public static partial City FromEditCommand(AddEditCityCommand command);
    public static partial AddEditCityCommand CloneFromDto(CityDto dto);
    public static partial void ApplyChangesFrom(AddEditCityCommand source, City target);
    public static partial IQueryable<CityDto> ProjectTo(this IQueryable<City> q);
}
