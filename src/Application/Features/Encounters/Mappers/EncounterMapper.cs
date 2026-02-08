using CleanArchitecture.Blazor.Application.Features.Encounters.Commands.Start;
using CleanArchitecture.Blazor.Application.Features.Encounters.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Encounters.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class EncounterMapper
{
    public static partial EncounterDto ToDto(Encounter encounter);
    public static partial Encounter FromDto(EncounterDto dto);
    public static partial Encounter FromStartCommand(StartEncounterCommand command);
    public static partial void ApplyChangesFrom(StartEncounterCommand command, Encounter encounter);
    public static partial StartEncounterCommand ToStartCommand(EncounterDto dto);
    public static partial IQueryable<EncounterDto> ProjectTo(this IQueryable<Encounter> q);
}
