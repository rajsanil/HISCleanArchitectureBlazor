#nullable disable

using CleanArchitecture.Blazor.Application.Features.Nationalities.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.Nationalities.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Nationalities.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class NationalityMapper
{
    public static partial NationalityDto ToDto(Nationality source);    public static partial Nationality FromDto(NationalityDto dto);
    public static partial Nationality FromEditCommand(AddEditNationalityCommand command);
    public static partial AddEditNationalityCommand CloneFromDto(NationalityDto dto);
    public static partial void ApplyChangesFrom(AddEditNationalityCommand source, Nationality target);
    public static partial IQueryable<NationalityDto> ProjectTo(this IQueryable<Nationality> q);
}
