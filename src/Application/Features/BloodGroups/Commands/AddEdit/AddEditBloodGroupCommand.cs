#nullable disable

using CleanArchitecture.Blazor.Application.Features.BloodGroups.Caching;
using CleanArchitecture.Blazor.Application.Features.BloodGroups.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.BloodGroups.Commands.AddEdit;

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
    private readonly IApplicationDbContext _context;
    
    public AddEditBloodGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<int>> Handle(AddEditBloodGroupCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.BloodGroups.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                return await Result<int>.FailureAsync($"Blood Group with id: [{request.Id}] not found.");
            }
            
            BloodGroupMapper.ApplyChangesFrom(request, item);
            item.AddDomainEvent(new BloodGroupUpdatedEvent(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = BloodGroupMapper.FromEditCommand(request);
            item.AddDomainEvent(new BloodGroupCreatedEvent(item));
            _context.BloodGroups.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
    }
}
