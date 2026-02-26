// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Shifts.Commands.AddEdit;

/// <summary>
/// Validator for AddEditShiftCommand.
/// </summary>
public class AddEditShiftCommandValidator : AbstractValidator<AddEditShiftCommand>
{
    public AddEditShiftCommandValidator()
    {
        RuleFor(v => v.ShiftCode)
            .MaximumLength(200)
            .NotEmpty()
            .WithMessage("Shift code is required.");

        RuleFor(v => v.ShiftName)
            .MaximumLength(200)
            .NotEmpty()
            .WithMessage("Shift name is required.");

        RuleFor(v => v.ToTime)
            .Must((command, toTime) => toTime > command.FromTime)
            .WithMessage("End time must be after start time.");
    }
}
