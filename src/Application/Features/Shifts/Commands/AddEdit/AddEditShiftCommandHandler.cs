// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Shifts.Commands.AddEdit;

/// <summary>
/// Handler for AddEditShiftCommand.
/// </summary>
public class AddEditShiftCommandHandler : IRequestHandler<AddEditShiftCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public AddEditShiftCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(AddEditShiftCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            // Update existing shift
            var item = await _context.Shifts.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                return await Result<int>.FailureAsync($"Shift with ID {request.Id} not found.");
            }

            item.Code = request.ShiftCode;
            item.Name = request.ShiftName;
            item.FromTime = request.FromTime;
            item.ToTime = request.ToTime;
            item.IsActive = request.IsActive;

            // Raise domain event
            item.AddDomainEvent(new UpdatedEvent<Shift>(item));

            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            // Create new shift
            var item = new Shift
            {
                Code = request.ShiftCode,
                Name = request.ShiftName,
                FromTime = request.FromTime,
                ToTime = request.ToTime,
                IsActive = request.IsActive
            };

            // Raise domain event
            item.AddDomainEvent(new CreatedEvent<Shift>(item));

            _context.Shifts.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
    }
}
