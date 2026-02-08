using CleanArchitecture.Blazor.Application.Features.Facilities.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.Facilities.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Facilities.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class FacilityMapper
{
    public static partial FacilityDto ToDto(Facility facility);
    public static partial Facility FromDto(FacilityDto dto);
    public static partial Facility FromEditCommand(AddEditFacilityCommand command);
    public static partial void ApplyChangesFrom(AddEditFacilityCommand command, Facility facility);
    [MapperIgnoreSource(nameof(FacilityDto.Id))]
    public static partial AddEditFacilityCommand CloneFromDto(FacilityDto dto);
    public static partial AddEditFacilityCommand ToEditCommand(FacilityDto dto);
    public static partial IQueryable<FacilityDto> ProjectTo(this IQueryable<Facility> q);
}
