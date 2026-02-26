// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Common.ExceptionHandlers;
using CleanArchitecture.Blazor.Application.Features.Shifts.Caching;
using CleanArchitecture.Blazor.Application.Features.Shifts.Mappers;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Blazor.Application.Features.Shifts.Commands.AddEdit;

/// <summary>
/// Command to add or edit a shift.
/// </summary>
public class AddEditShiftCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }

    [Description("Code")]
    public string ShiftCode { get; set; } = string.Empty;

    [Description("Name")]
    public string ShiftName { get; set; } = string.Empty;

    [Description("From Time")]
    public TimeSpan FromTime { get; set; }

    [Description("To Time")]
    public TimeSpan ToTime { get; set; }

    [Description("Is Active")]
    public bool IsActive { get; set; } = true;

    public string CacheKey => ShiftCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => ShiftCacheKey.Tags;
}

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
        try
        {
            if (request.Id > 0)
            {
                var item = await _context.Shifts.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (item == null)
                    return await Result<int>.FailureAsync($"Shift with id: [{request.Id}] not found.");

                ShiftMapper.ApplyChangesFrom(request, item);
                item.AddDomainEvent(new UpdatedEvent<Shift>(item));
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
            else
            {
                var item = ShiftMapper.FromEditCommand(request);
                item.AddDomainEvent(new CreatedEvent<Shift>(item));
                _context.Shifts.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
        }
        catch (DbUpdateException ex) when (UniqueConstraintExceptionHandler.IsUniqueConstraintViolation(ex))
        {
            // Clear the change tracker to remove any tracked entities with invalid state
            _context.ChangeTracker.Clear();

            var constraintName = UniqueConstraintExceptionHandler.ExtractConstraintName(ex);
            // Format: "FieldName|User-friendly message" — the UI parses the prefix to highlight the correct field
            return constraintName switch
            {
                "IX_Shifts_Code" => await Result<int>.FailureAsync($"ShiftCode|A shift with code '{request.ShiftCode}' already exists."),
                "IX_Shifts_Name" => await Result<int>.FailureAsync($"ShiftName|A shift with name '{request.ShiftName}' already exists."),
                _ => await Result<int>.FailureAsync(UniqueConstraintExceptionHandler.GetUserFriendlyMessage(constraintName))
            };
        }
    }
}

