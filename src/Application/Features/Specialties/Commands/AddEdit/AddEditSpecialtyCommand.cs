using CleanArchitecture.Blazor.Application.Features.Specialties.Caching;
using CleanArchitecture.Blazor.Application.Features.Specialties.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Specialties.Commands.AddEdit;

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
    private readonly IApplicationDbContext _context;
    public AddEditSpecialtyCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(AddEditSpecialtyCommand request, CancellationToken cancellationToken)
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
}
