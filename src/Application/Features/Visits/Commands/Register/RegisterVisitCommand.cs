using CleanArchitecture.Blazor.Application.Features.Visits.Caching;
using CleanArchitecture.Blazor.Application.Features.Visits.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Visits.Commands.Register;

/// <summary>
/// Registers a new visit for a patient. Generates a unique visit number.
/// </summary>
public class RegisterVisitCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string VisitType { get; set; } = string.Empty;
    public int FacilityId { get; set; }
    public int? DepartmentId { get; set; }
    public int? AttendingDoctorId { get; set; }

    public string CacheKey => VisitCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => VisitCacheKey.Tags;
}

public class RegisterVisitCommandHandler : IRequestHandler<RegisterVisitCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IVisitNumberGenerator _visitNumberGenerator;

    public RegisterVisitCommandHandler(IApplicationDbContext context, IVisitNumberGenerator visitNumberGenerator)
    {
        _context = context;
        _visitNumberGenerator = visitNumberGenerator;
    }

    public async Task<Result<int>> Handle(RegisterVisitCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            // Update existing visit registration details
            var item = await _context.Visits.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (item == null)
                return await Result<int>.FailureAsync($"Visit with id: [{request.Id}] not found.");
            VisitMapper.ApplyChangesFrom(request, item);
            item.AddDomainEvent(new UpdatedEvent<Visit>(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var visit = VisitMapper.FromRegisterCommand(request);
            visit.VisitNumber = await _visitNumberGenerator.GenerateNextVisitNumberAsync(cancellationToken);
            visit.VisitStatus = "Registered";
            visit.RegistrationDate = DateTime.UtcNow;
            visit.AddDomainEvent(new CreatedEvent<Visit>(visit));
            _context.Visits.Add(visit);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(visit.Id);
        }
    }
}
