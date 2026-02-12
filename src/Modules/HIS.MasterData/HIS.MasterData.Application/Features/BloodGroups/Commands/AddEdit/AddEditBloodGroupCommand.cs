using HIS.MasterData.Application.Features.BloodGroups.Caching;
using HIS.MasterData.Application.Features.BloodGroups.Mappers;
using HIS.MasterData.Application.Common.Exceptions;

namespace HIS.MasterData.Application.Features.BloodGroups.Commands.AddEdit;

public class AddEditBloodGroupCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }

    [Description("Code")]
    public string Code { get; set; } = string.Empty;

    [Description("Name")]
    public string Name { get; set; } = string.Empty;

    [Description("Name (Arabic)")]
    public string? NameArabic { get; set; }

    [Description("Display Order")]
    public int DisplayOrder { get; set; }

    [Description("Is Active")]
    public bool IsActive { get; set; } = true;

    public string CacheKey => BloodGroupCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => BloodGroupCacheKey.Tags;
}

public class AddEditBloodGroupCommandHandler : IRequestHandler<AddEditBloodGroupCommand, Result<int>>
{
    private readonly IMasterDataDbContext _context;

    public AddEditBloodGroupCommandHandler(IMasterDataDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(AddEditBloodGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id > 0)
            {
                var item = await _context.BloodGroups.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (item == null) return await Result<int>.FailureAsync($"Blood group with id: [{request.Id}] not found.");
                BloodGroupMapper.ApplyChangesFrom(request, item);
                item.AddDomainEvent(new UpdatedEvent<BloodGroup>(item));
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
            else
            {
                var item = BloodGroupMapper.FromEditCommand(request);
                item.AddDomainEvent(new CreatedEvent<BloodGroup>(item));
                _context.BloodGroups.Add(item);
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
                "IX_BloodGroups_Code" => await Result<int>.FailureAsync($"Code|A blood group with code '{request.Code}' already exists."),
                "IX_BloodGroups_Name" => await Result<int>.FailureAsync($"Name|A blood group with name '{request.Name}' already exists."),
                _ => await Result<int>.FailureAsync(UniqueConstraintExceptionHandler.GetUserFriendlyMessage(constraintName))
            };
        }
    }
}
