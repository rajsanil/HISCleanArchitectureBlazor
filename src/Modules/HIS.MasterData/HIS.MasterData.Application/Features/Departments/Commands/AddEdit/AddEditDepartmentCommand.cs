using HIS.MasterData.Application.Features.Departments.Caching;
using HIS.MasterData.Application.Features.Departments.Mappers;
using HIS.MasterData.Application.Common.Exceptions;

namespace HIS.MasterData.Application.Features.Departments.Commands.AddEdit;

public class AddEditDepartmentCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? NameArabic { get; set; }
    public int FacilityId { get; set; }
    public int? ParentDepartmentId { get; set; }
    public bool IsActive { get; set; } = true;

    public string CacheKey => DepartmentCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => DepartmentCacheKey.Tags;
}

public class AddEditDepartmentCommandHandler : IRequestHandler<AddEditDepartmentCommand, Result<int>>
{
    private readonly IMasterDataDbContext _context;
    public AddEditDepartmentCommandHandler(IMasterDataDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(AddEditDepartmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id > 0)
            {
                var item = await _context.Departments.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (item == null)
                    return await Result<int>.FailureAsync($"Department with id: [{request.Id}] not found.");
                DepartmentMapper.ApplyChangesFrom(request, item);
                item.AddDomainEvent(new UpdatedEvent<Department>(item));
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
            else
            {
                var item = DepartmentMapper.FromEditCommand(request);
                item.AddDomainEvent(new CreatedEvent<Department>(item));
                _context.Departments.Add(item);
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
                "IX_Departments_Code_FacilityId" => await Result<int>.FailureAsync($"Code|A department with code '{request.Code}' already exists in this facility."),
                "IX_Departments_Name_FacilityId" => await Result<int>.FailureAsync($"Name|A department with name '{request.Name}' already exists in this facility."),
                _ => await Result<int>.FailureAsync(UniqueConstraintExceptionHandler.GetUserFriendlyMessage(constraintName))
            };
        }
    }
}
