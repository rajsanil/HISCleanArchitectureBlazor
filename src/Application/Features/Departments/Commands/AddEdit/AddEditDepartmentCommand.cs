using CleanArchitecture.Blazor.Application.Features.Departments.Caching;
using CleanArchitecture.Blazor.Application.Features.Departments.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Departments.Commands.AddEdit;

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
    private readonly IApplicationDbContext _context;
    public AddEditDepartmentCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(AddEditDepartmentCommand request, CancellationToken cancellationToken)
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
}
