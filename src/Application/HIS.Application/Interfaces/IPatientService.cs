using HIS.Application.DTOs;

namespace HIS.Application.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllPatientsAsync(CancellationToken cancellationToken = default);
    Task<PatientDto?> GetPatientByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PatientDto> CreatePatientAsync(CreatePatientDto dto, CancellationToken cancellationToken = default);
    Task UpdatePatientAsync(Guid id, CreatePatientDto dto, CancellationToken cancellationToken = default);
    Task DeletePatientAsync(Guid id, CancellationToken cancellationToken = default);
}
