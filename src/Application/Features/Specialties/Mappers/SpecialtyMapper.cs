using CleanArchitecture.Blazor.Application.Features.Specialties.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.Specialties.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Specialties.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class SpecialtyMapper
{
    public static partial SpecialtyDto ToDto(Specialty specialty);
    public static partial Specialty FromDto(SpecialtyDto dto);
    public static partial Specialty FromEditCommand(AddEditSpecialtyCommand command);
    public static partial void ApplyChangesFrom(AddEditSpecialtyCommand command, Specialty specialty);
    [MapperIgnoreSource(nameof(SpecialtyDto.Id))]
    public static partial AddEditSpecialtyCommand CloneFromDto(SpecialtyDto dto);
    public static partial AddEditSpecialtyCommand ToEditCommand(SpecialtyDto dto);
    public static partial IQueryable<SpecialtyDto> ProjectTo(this IQueryable<Specialty> q);
}
