using HIS.MasterData.Application.Features.Specialties.Caching;
using HIS.MasterData.Application.Features.Specialties.Mappers;
using CleanArchitecture.Blazor.Application.Common.ExceptionHandlers;

namespace HIS.MasterData.Application.Features.Specialties.Commands.AddEdit;

public class AddEditSpecialtyCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? NameArabic { get; set; }
    public int DepartmentId { get; set; }
    public bool IsActive { get; set; } = true;

    public string CacheKey => SpecialtyCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => SpecialtyCacheKey.Tags;
}

public class AddEditSpecialtyCommandHandler : IRequestHandler<AddEditSpecialtyCommand, Result<int>>
{
    private readonly IMasterDataDbContext _context;
    public AddEditSpecialtyCommandHandler(IMasterDataDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(AddEditSpecialtyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id > 0)
            {
                var item = await _context.Specialties.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (item == null)
                    return await Result<int>.FailureAsync($"Specialty with id: [{request.Id}] not found.");
                SpecialtyMapper.ApplyChangesFrom(request, item);
                item.AddDomainEvent(new UpdatedEvent<Specialty>(item));
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
            else
            {
                var item = SpecialtyMapper.FromEditCommand(request);
                item.AddDomainEvent(new CreatedEvent<Specialty>(item));
                _context.Specialties.Add(item);
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
                "IX_Specialties_Code" => await Result<int>.FailureAsync($"Code|A specialty with code '{request.Code}' already exists."),
                "IX_Specialties_Name" => await Result<int>.FailureAsync($"Name|A specialty with name '{request.Name}' already exists."),
                _ => await Result<int>.FailureAsync(UniqueConstraintExceptionHandler.GetUserFriendlyMessage(constraintName))
            };
        }
    }
}
