using CleanArchitecture.Blazor.Application.Features.Departments.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.Departments.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Departments.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class DepartmentMapper
{
    public static partial DepartmentDto ToDto(Department department);
    public static partial Department FromDto(DepartmentDto dto);
    public static partial Department FromEditCommand(AddEditDepartmentCommand command);
    public static partial void ApplyChangesFrom(AddEditDepartmentCommand command, Department department);
    [MapperIgnoreSource(nameof(DepartmentDto.Id))]
    public static partial AddEditDepartmentCommand CloneFromDto(DepartmentDto dto);
    public static partial AddEditDepartmentCommand ToEditCommand(DepartmentDto dto);
    public static partial IQueryable<DepartmentDto> ProjectTo(this IQueryable<Department> q);
}
