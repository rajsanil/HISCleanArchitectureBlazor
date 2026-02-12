using HIS.MasterData.Application.Features.Countries.Commands.AddEdit;
using HIS.MasterData.Application.Features.Countries.DTOs;

namespace HIS.MasterData.Application.Features.Countries.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class CountryMapper
{
    public static partial CountryDto ToDto(Country country);
    public static partial Country FromDto(CountryDto dto);
    public static partial Country FromEditCommand(AddEditCountryCommand command);
    public static partial void ApplyChangesFrom(AddEditCountryCommand command, Country country);
    [MapperIgnoreSource(nameof(CountryDto.Id))]
    public static partial AddEditCountryCommand CloneFromDto(CountryDto dto);
    public static partial AddEditCountryCommand ToEditCommand(CountryDto dto);
    public static partial IQueryable<CountryDto> ProjectTo(this IQueryable<Country> q);
}
