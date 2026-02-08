#nullable disable

using CleanArchitecture.Blazor.Application.Features.BloodGroups.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.BloodGroups.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.BloodGroups.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class BloodGroupMapper
{
    public static partial BloodGroupDto ToDto(BloodGroup source);
    public static partial BloodGroup FromDto(BloodGroupDto dto);
    public static partial BloodGroup FromEditCommand(AddEditBloodGroupCommand command);
    public static partial AddEditBloodGroupCommand CloneFromDto(BloodGroupDto dto);
    public static partial void ApplyChangesFrom(AddEditBloodGroupCommand source, BloodGroup target);
    public static partial IQueryable<BloodGroupDto> ProjectTo(this IQueryable<BloodGroup> q);
}
