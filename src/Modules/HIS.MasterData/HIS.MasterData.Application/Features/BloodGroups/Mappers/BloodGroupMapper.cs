using HIS.MasterData.Application.Features.BloodGroups.Commands.AddEdit;
using HIS.MasterData.Application.Features.BloodGroups.DTOs;

namespace HIS.MasterData.Application.Features.BloodGroups.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class BloodGroupMapper
{
    public static partial BloodGroupDto ToDto(BloodGroup entity);
    public static partial BloodGroup FromDto(BloodGroupDto dto);
    public static partial BloodGroup FromEditCommand(AddEditBloodGroupCommand command);
    public static partial void ApplyChangesFrom(AddEditBloodGroupCommand command, BloodGroup entity);
    [MapperIgnoreSource(nameof(BloodGroupDto.Id))]
    public static partial AddEditBloodGroupCommand CloneFromDto(BloodGroupDto dto);
    public static partial AddEditBloodGroupCommand ToEditCommand(BloodGroupDto dto);
    public static partial IQueryable<BloodGroupDto> ProjectTo(this IQueryable<BloodGroup> q);
}
