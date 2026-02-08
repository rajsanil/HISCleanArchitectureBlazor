using CleanArchitecture.Blazor.Application.Features.Patients.Caching;
using CleanArchitecture.Blazor.Application.Features.Patients.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Patients.Commands.AddEdit;

public class AddEditPatientCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int Id { get; set; }
    public string? MRN { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? FirstNameArabic { get; set; }
    public string? LastNameArabic { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public int? NationalityId { get; set; }
    public string? EmiratesId { get; set; }
    public string? PassportNumber { get; set; }
    public int? BloodGroupId { get; set; }
    public int? MaritalStatusId { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public int? CityId { get; set; }
    public int? CountryId { get; set; }
    public string? PhotoUrl { get; set; }
    public bool IsVIP { get; set; }
    public bool IsActive { get; set; } = true;

    public string CacheKey => PatientCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => PatientCacheKey.Tags;
}

public class AddEditPatientCommandHandler : IRequestHandler<AddEditPatientCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMrnGenerator _mrnGenerator;

    public AddEditPatientCommandHandler(IApplicationDbContext context, IMrnGenerator mrnGenerator)
    {
        _context = context;
        _mrnGenerator = mrnGenerator;
    }

    public async Task<Result<int>> Handle(AddEditPatientCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.Patients.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (item == null)
            {
                return await Result<int>.FailureAsync($"Patient with id: [{request.Id}] not found.");
            }
            PatientMapper.ApplyChangesFrom(request, item);
            item.AddDomainEvent(new UpdatedEvent<Patient>(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = PatientMapper.FromEditCommand(request);
            item.MRN = await _mrnGenerator.GenerateNextMrnAsync(cancellationToken);
            item.AddDomainEvent(new CreatedEvent<Patient>(item));
            _context.Patients.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
    }
}
