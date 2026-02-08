#nullable disable

using CleanArchitecture.Blazor.Application.Features.Nationalities.Caching;
using CleanArchitecture.Blazor.Application.Features.Nationalities.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Nationalities.Commands.AddEdit;

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
    private readonly IApplicationDbContext _context;
    
    public AddEditNationalityCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<int>> Handle(AddEditNationalityCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.Nationalities.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                return await Result<int>.FailureAsync($"Nationality with id: [{request.Id}] not found.");
            }
            
            NationalityMapper.ApplyChangesFrom(request, item);
            item.AddDomainEvent(new NationalityUpdatedEvent(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = NationalityMapper.FromEditCommand(request);
            item.AddDomainEvent(new NationalityCreatedEvent(item));
            _context.Nationalities.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
    }
}
