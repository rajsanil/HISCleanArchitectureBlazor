using HIS.MasterData.Application.Features.Beds.Commands.AddEdit;
using HIS.MasterData.Application.Features.Beds.DTOs;

namespace HIS.MasterData.Application.Features.Beds.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class BedMapper
{
    public static partial BedDto ToDto(Bed bed);
    public static partial Bed FromDto(BedDto dto);
    public static partial Bed FromEditCommand(AddEditBedCommand command);
    public static partial void ApplyChangesFrom(AddEditBedCommand command, Bed bed);
    [MapperIgnoreSource(nameof(BedDto.Id))]
    public static partial AddEditBedCommand CloneFromDto(BedDto dto);
    public static partial AddEditBedCommand ToEditCommand(BedDto dto);
    public static partial IQueryable<BedDto> ProjectTo(this IQueryable<Bed> q);
}
