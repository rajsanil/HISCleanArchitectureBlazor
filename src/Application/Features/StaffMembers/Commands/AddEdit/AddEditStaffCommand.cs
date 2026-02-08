using CleanArchitecture.Blazor.Application.Features.StaffMembers.Caching;
using CleanArchitecture.Blazor.Application.Features.StaffMembers.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.StaffMembers.Commands.AddEdit;

public class AddEditStaffCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int Id { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string? ApplicationUserId { get; set; }
    public string StaffType { get; set; } = string.Empty;
    public int? DepartmentId { get; set; }
    public int? SpecialtyId { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Title { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public string CacheKey => StaffCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => StaffCacheKey.Tags;
}

public class AddEditStaffCommandHandler : IRequestHandler<AddEditStaffCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    public AddEditStaffCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(AddEditStaffCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.Staff.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (item == null)
                return await Result<int>.FailureAsync($"Staff with id: [{request.Id}] not found.");
            StaffMapper.ApplyChangesFrom(request, item);
            item.AddDomainEvent(new UpdatedEvent<Staff>(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = StaffMapper.FromEditCommand(request);
            item.AddDomainEvent(new CreatedEvent<Staff>(item));
            _context.Staff.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
    }
}
