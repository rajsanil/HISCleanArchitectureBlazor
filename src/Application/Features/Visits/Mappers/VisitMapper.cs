using CleanArchitecture.Blazor.Application.Features.Visits.Commands.Register;
using CleanArchitecture.Blazor.Application.Features.Visits.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Visits.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class VisitMapper
{
    public static partial VisitDto ToDto(Visit visit);
    public static partial Visit FromDto(VisitDto dto);
    public static partial Visit FromRegisterCommand(RegisterVisitCommand command);
    public static partial void ApplyChangesFrom(RegisterVisitCommand command, Visit visit);
    public static partial RegisterVisitCommand ToRegisterCommand(VisitDto dto);
    public static partial IQueryable<VisitDto> ProjectTo(this IQueryable<Visit> q);
}
