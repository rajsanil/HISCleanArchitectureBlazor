using HIS.MasterData.Application.Features.MaritalStatuses.Caching;
using HIS.MasterData.Application.Features.MaritalStatuses.Mappers;
using HIS.MasterData.Application.Common.Exceptions;

namespace HIS.MasterData.Application.Features.MaritalStatuses.Commands.AddEdit;

public class AddEditMaritalStatusCommand : ICacheInvalidatorRequest<Result<int>>
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

    public string CacheKey => MaritalStatusCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => MaritalStatusCacheKey.Tags;
}

public class AddEditMaritalStatusCommandHandler : IRequestHandler<AddEditMaritalStatusCommand, Result<int>>
{
    private readonly IMasterDataDbContext _context;

    public AddEditMaritalStatusCommandHandler(IMasterDataDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(AddEditMaritalStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id > 0)
            {
                var item = await _context.MaritalStatuses.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (item == null) return await Result<int>.FailureAsync($"Marital status with id: [{request.Id}] not found.");
                MaritalStatusMapper.ApplyChangesFrom(request, item);
                item.AddDomainEvent(new UpdatedEvent<MaritalStatus>(item));
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
            else
            {
                var item = MaritalStatusMapper.FromEditCommand(request);
                item.AddDomainEvent(new CreatedEvent<MaritalStatus>(item));
                _context.MaritalStatuses.Add(item);
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
                "IX_MaritalStatuses_Code" => await Result<int>.FailureAsync($"Code|A marital status with code '{request.Code}' already exists."),
                "IX_MaritalStatuses_Name" => await Result<int>.FailureAsync($"Name|A marital status with name '{request.Name}' already exists."),
                _ => await Result<int>.FailureAsync(UniqueConstraintExceptionHandler.GetUserFriendlyMessage(constraintName))
            };
        }
    }
}
