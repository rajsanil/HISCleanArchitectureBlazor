// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Shifts.Commands.AddEdit;

/// <summary>
/// Validator for AddEditShiftCommand.
/// </summary>
public class AddEditShiftCommandValidator : AbstractValidator<AddEditShiftCommand>
{
    private readonly IApplicationDbContext _context;

    public AddEditShiftCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.ShiftCode)
            .MaximumLength(200)
            .NotEmpty()
            .WithMessage("Shift code is required.")
            .MustAsync(BeUniqueShiftCode)
            .WithMessage("The specified shift code already exists.");


        RuleFor(v => v.ShiftName)
            .MaximumLength(200)
            .NotEmpty()
            .WithMessage("Shift name is required.");

        RuleFor(v => v.ToTime)
            .Must((command, toTime) => toTime > command.FromTime)
            .WithMessage("End time must be after start time.");
    }

    /// <summary>
    /// Validates that the shift code is unique within the tenant.
    /// </summary>
    private async Task<bool> BeUniqueShiftCode(AddEditShiftCommand command, string shiftCode, CancellationToken cancellationToken)
    {
        return await _context.Shifts
            .Where(x => x.Id != command.Id)
            .AllAsync(x => x.Code != shiftCode, cancellationToken);
    }
}
