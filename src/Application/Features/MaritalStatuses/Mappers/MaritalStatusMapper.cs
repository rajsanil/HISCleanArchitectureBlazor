#nullable disable

using CleanArchitecture.Blazor.Application.Features.MaritalStatuses.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.MaritalStatuses.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.MaritalStatuses.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class MaritalStatusMapper
{
    public static partial MaritalStatusDto ToDto(MaritalStatus source);
    public static partial MaritalStatus FromDto(MaritalStatusDto dto);
    public static partial MaritalStatus FromEditCommand(AddEditMaritalStatusCommand command);
    public static partial AddEditMaritalStatusCommand CloneFromDto(MaritalStatusDto dto);
    public static partial void ApplyChangesFrom(AddEditMaritalStatusCommand source, MaritalStatus target);
    public static partial IQueryable<MaritalStatusDto> ProjectTo(this IQueryable<MaritalStatus> q);
}
