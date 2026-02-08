using CleanArchitecture.Blazor.Application.Features.StaffMembers.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.StaffMembers.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.StaffMembers.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class StaffMapper
{
    public static partial StaffDto ToDto(Staff staff);
    public static partial Staff FromDto(StaffDto dto);
    public static partial Staff FromEditCommand(AddEditStaffCommand command);
    public static partial void ApplyChangesFrom(AddEditStaffCommand command, Staff staff);
    [MapperIgnoreSource(nameof(StaffDto.Id))]
    [MapperIgnoreSource(nameof(StaffDto.FullName))]
    public static partial AddEditStaffCommand CloneFromDto(StaffDto dto);
    public static partial AddEditStaffCommand ToEditCommand(StaffDto dto);
    public static partial IQueryable<StaffDto> ProjectTo(this IQueryable<Staff> q);
}
