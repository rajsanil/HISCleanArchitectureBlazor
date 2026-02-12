using HIS.MasterData.Application.Features.Beds.Caching;
using HIS.MasterData.Application.Features.Beds.Mappers;
using HIS.MasterData.Application.Common.Exceptions;

namespace HIS.MasterData.Application.Features.Beds.Commands.AddEdit;

public class AddEditBedCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public int RoomId { get; set; }
    public string BedStatus { get; set; } = "Available";
    public bool IsActive { get; set; } = true;

    public string CacheKey => BedCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => BedCacheKey.Tags;
}

public class AddEditBedCommandHandler : IRequestHandler<AddEditBedCommand, Result<int>>
{
    private readonly IMasterDataDbContext _context;
    public AddEditBedCommandHandler(IMasterDataDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(AddEditBedCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id > 0)
            {
                var item = await _context.Beds.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (item == null)
                    return await Result<int>.FailureAsync($"Bed with id: [{request.Id}] not found.");
                BedMapper.ApplyChangesFrom(request, item);
                item.AddDomainEvent(new UpdatedEvent<Bed>(item));
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
            else
            {
                var item = BedMapper.FromEditCommand(request);
                item.AddDomainEvent(new CreatedEvent<Bed>(item));
                _context.Beds.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
        }
        catch (DbUpdateException ex) when (UniqueConstraintExceptionHandler.IsUniqueConstraintViolation(ex))
        {
            // Clear the change tracker to remove any tracked entities with invalid state
            _context.ChangeTracker.Clear();
            
            var constraintName = UniqueConstraintExceptionHandler.ExtractConstraintName(ex);
            return constraintName switch
            {
                "IX_Beds_Code_RoomId" => await Result<int>.FailureAsync($"Code|A bed with code '{request.Code}' already exists in this room."),
                _ => await Result<int>.FailureAsync(UniqueConstraintExceptionHandler.GetUserFriendlyMessage(constraintName))
            };
        }
    }
}
