using HIS.MasterData.Application.Features.Cities.Commands.AddEdit;
using HIS.MasterData.Application.Features.Cities.DTOs;

namespace HIS.MasterData.Application.Features.Cities.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class CityMapper
{
    [MapProperty(new[] { nameof(City.Country), nameof(Country.Name) }, new[] { nameof(CityDto.CountryName) })]
    public static partial CityDto ToDto(City entity);
    public static partial City FromEditCommand(AddEditCityCommand command);
    public static partial void ApplyChangesFrom(AddEditCityCommand command, City entity);
    [MapperIgnoreSource(nameof(CityDto.Id))]
    [MapperIgnoreSource(nameof(CityDto.CountryName))]
    public static partial AddEditCityCommand CloneFromDto(CityDto dto);
    [MapperIgnoreSource(nameof(CityDto.CountryName))]
    public static partial AddEditCityCommand ToEditCommand(CityDto dto);
    [MapProperty(new[] { nameof(City.Country), nameof(Country.Name) }, new[] { nameof(CityDto.CountryName) })]
    public static partial IQueryable<CityDto> ProjectTo(this IQueryable<City> q);
}
