#nullable disable

using CleanArchitecture.Blazor.Application.Features.MaritalStatuses.Caching;
using CleanArchitecture.Blazor.Application.Features.MaritalStatuses.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.MaritalStatuses.Commands.AddEdit;

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
    private readonly IApplicationDbContext _context;
    
    public AddEditMaritalStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<int>> Handle(AddEditMaritalStatusCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.MaritalStatuses.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                return await Result<int>.FailureAsync($"Marital Status with id: [{request.Id}] not found.");
            }
            
            MaritalStatusMapper.ApplyChangesFrom(request, item);
            item.AddDomainEvent(new MaritalStatusUpdatedEvent(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = MaritalStatusMapper.FromEditCommand(request);
            item.AddDomainEvent(new MaritalStatusCreatedEvent(item));
            _context.MaritalStatuses.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
    }
}
