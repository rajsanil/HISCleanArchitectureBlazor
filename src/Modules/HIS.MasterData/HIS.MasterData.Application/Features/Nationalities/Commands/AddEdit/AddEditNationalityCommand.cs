using HIS.MasterData.Application.Features.Nationalities.Caching;
using HIS.MasterData.Application.Features.Nationalities.Mappers;
using HIS.MasterData.Application.Common.Exceptions;

namespace HIS.MasterData.Application.Features.Nationalities.Commands.AddEdit;

public class AddEditNationalityCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }

    [Description("Code")]
    public string Code { get; set; } = string.Empty;

    [Description("Name")]
    public string Name { get; set; } = string.Empty;

    [Description("Name (Arabic)")]
    public string? NameArabic { get; set; }

    [Description("Is Active")]
    public bool IsActive { get; set; } = true;

    public string CacheKey => NationalityCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => NationalityCacheKey.Tags;
}

public class AddEditNationalityCommandHandler : IRequestHandler<AddEditNationalityCommand, Result<int>>
{
    private readonly IMasterDataDbContext _context;

    public AddEditNationalityCommandHandler(IMasterDataDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(AddEditNationalityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id > 0)
            {
                var item = await _context.Nationalities.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (item == null) return await Result<int>.FailureAsync($"Nationality with id: [{request.Id}] not found.");
                NationalityMapper.ApplyChangesFrom(request, item);
                item.AddDomainEvent(new UpdatedEvent<Nationality>(item));
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
            else
            {
                var item = NationalityMapper.FromEditCommand(request);
                item.AddDomainEvent(new CreatedEvent<Nationality>(item));
                _context.Nationalities.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
        }
        catch (DbUpdateException ex) when (UniqueConstraintExceptionHandler.IsUniqueConstraintViolation(ex))
        {
            var constraintName = UniqueConstraintExceptionHandler.ExtractConstraintName(ex);
            return constraintName switch
            {
                "IX_Nationalities_Code" => await Result<int>.FailureAsync($"Code|A nationality with code '{request.Code}' already exists."),
                "IX_Nationalities_Name" => await Result<int>.FailureAsync($"Name|A nationality with name '{request.Name}' already exists."),
                _ => await Result<int>.FailureAsync(UniqueConstraintExceptionHandler.GetUserFriendlyMessage(constraintName))
            };
        }
    }
}
