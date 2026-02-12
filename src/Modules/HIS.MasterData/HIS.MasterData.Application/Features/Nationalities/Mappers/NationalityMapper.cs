using HIS.MasterData.Application.Features.Nationalities.Commands.AddEdit;
using HIS.MasterData.Application.Features.Nationalities.DTOs;

namespace HIS.MasterData.Application.Features.Nationalities.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class NationalityMapper
{
    public static partial NationalityDto ToDto(Nationality entity);
    public static partial Nationality FromDto(NationalityDto dto);
    public static partial Nationality FromEditCommand(AddEditNationalityCommand command);
    public static partial void ApplyChangesFrom(AddEditNationalityCommand command, Nationality entity);
    [MapperIgnoreSource(nameof(NationalityDto.Id))]
    public static partial AddEditNationalityCommand CloneFromDto(NationalityDto dto);
    public static partial AddEditNationalityCommand ToEditCommand(NationalityDto dto);
    public static partial IQueryable<NationalityDto> ProjectTo(this IQueryable<Nationality> q);
}
