// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Shifts.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.Shifts.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Shifts.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class ShiftMapper
{
    public static partial ShiftDto ToDto(Shift shift);
    public static partial Shift FromDto(ShiftDto dto);
    public static partial Shift FromEditCommand(AddEditShiftCommand command);
    public static partial void ApplyChangesFrom(AddEditShiftCommand command, Shift shift);
    [MapperIgnoreSource(nameof(ShiftDto.Id))]
    public static partial AddEditShiftCommand CloneFromDto(ShiftDto dto);
    public static partial AddEditShiftCommand ToEditCommand(ShiftDto dto);
    public static partial IQueryable<ShiftDto> ProjectTo(this IQueryable<Shift> q);
}
