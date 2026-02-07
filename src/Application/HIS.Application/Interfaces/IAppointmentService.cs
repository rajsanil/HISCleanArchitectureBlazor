using HIS.Application.DTOs;

namespace HIS.Application.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientIdAsync(Guid patientId, CancellationToken cancellationToken = default);
    Task<AppointmentDto?> GetAppointmentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto dto, CancellationToken cancellationToken = default);
    Task UpdateAppointmentAsync(Guid id, CreateAppointmentDto dto, CancellationToken cancellationToken = default);
    Task DeleteAppointmentAsync(Guid id, CancellationToken cancellationToken = default);
}
