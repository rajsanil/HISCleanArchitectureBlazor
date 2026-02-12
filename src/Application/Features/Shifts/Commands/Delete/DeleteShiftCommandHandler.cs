// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Shifts.Commands.Delete;

/// <summary>
/// Handler for DeleteShiftCommand.
/// </summary>
public class DeleteShiftCommandHandler : IRequestHandler<DeleteShiftCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public DeleteShiftCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(DeleteShiftCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Shifts
            .Where(x => request.Id.Contains(x.Id))
            .ToListAsync(cancellationToken);

        if (items.Count == 0)
        {
            return await Result<int>.FailureAsync("No shifts found to delete.");
        }

        foreach (var item in items)
        {
            // Raise domain event
            item.AddDomainEvent(new DeletedEvent<Shift>(item));
            _context.Shifts.Remove(item);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(items.Count);
    }
}
