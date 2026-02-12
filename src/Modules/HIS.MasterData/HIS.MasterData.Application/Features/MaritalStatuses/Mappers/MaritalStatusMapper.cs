using HIS.MasterData.Application.Features.MaritalStatuses.Commands.AddEdit;
using HIS.MasterData.Application.Features.MaritalStatuses.DTOs;

namespace HIS.MasterData.Application.Features.MaritalStatuses.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class MaritalStatusMapper
{
    public static partial MaritalStatusDto ToDto(MaritalStatus entity);
    public static partial MaritalStatus FromDto(MaritalStatusDto dto);
    public static partial MaritalStatus FromEditCommand(AddEditMaritalStatusCommand command);
    public static partial void ApplyChangesFrom(AddEditMaritalStatusCommand command, MaritalStatus entity);
    [MapperIgnoreSource(nameof(MaritalStatusDto.Id))]
    public static partial AddEditMaritalStatusCommand CloneFromDto(MaritalStatusDto dto);
    public static partial AddEditMaritalStatusCommand ToEditCommand(MaritalStatusDto dto);
    public static partial IQueryable<MaritalStatusDto> ProjectTo(this IQueryable<MaritalStatus> q);
}
