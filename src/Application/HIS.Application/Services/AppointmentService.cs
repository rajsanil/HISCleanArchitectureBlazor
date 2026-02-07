using HIS.Application.DTOs;
using HIS.Application.Interfaces;
using HIS.Domain.Entities;
using HIS.Domain.Interfaces;

namespace HIS.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IRepository<Appointment> _appointmentRepository;
    private readonly IRepository<Patient> _patientRepository;

    public AppointmentService(IRepository<Appointment> appointmentRepository, IRepository<Patient> patientRepository)
    {
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync(CancellationToken cancellationToken = default)
    {
        var appointments = await _appointmentRepository.GetAllAsync(cancellationToken);
        var appointmentDtos = new List<AppointmentDto>();

        foreach (var appointment in appointments)
        {
            appointmentDtos.Add(await MapToDtoAsync(appointment, cancellationToken));
        }

        return appointmentDtos;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientIdAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        var appointments = await _appointmentRepository.GetAllAsync(cancellationToken);
        var patientAppointments = appointments.Where(a => a.PatientId == patientId);
        var appointmentDtos = new List<AppointmentDto>();

        foreach (var appointment in patientAppointments)
        {
            appointmentDtos.Add(await MapToDtoAsync(appointment, cancellationToken));
        }

        return appointmentDtos;
    }

    public async Task<AppointmentDto?> GetAppointmentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id, cancellationToken);
        return appointment != null ? await MapToDtoAsync(appointment, cancellationToken) : null;
    }

    public async Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto dto, CancellationToken cancellationToken = default)
    {
        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            PatientId = dto.PatientId,
            DoctorName = dto.DoctorName,
            AppointmentDate = dto.AppointmentDate,
            Status = "Scheduled",
            Reason = dto.Reason,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        var createdAppointment = await _appointmentRepository.AddAsync(appointment, cancellationToken);
        return await MapToDtoAsync(createdAppointment, cancellationToken);
    }

    public async Task UpdateAppointmentAsync(Guid id, CreateAppointmentDto dto, CancellationToken cancellationToken = default)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id, cancellationToken);
        if (appointment == null)
            throw new InvalidOperationException($"Appointment with ID {id} not found.");

        appointment.PatientId = dto.PatientId;
        appointment.DoctorName = dto.DoctorName;
        appointment.AppointmentDate = dto.AppointmentDate;
        appointment.Reason = dto.Reason;
        appointment.Notes = dto.Notes;
        appointment.UpdatedAt = DateTime.UtcNow;

        await _appointmentRepository.UpdateAsync(appointment, cancellationToken);
    }

    public async Task DeleteAppointmentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _appointmentRepository.DeleteAsync(id, cancellationToken);
    }

    private async Task<AppointmentDto> MapToDtoAsync(Appointment appointment, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetByIdAsync(appointment.PatientId, cancellationToken);
        var patientName = patient != null ? $"{patient.FirstName} {patient.LastName}" : "Unknown";

        return new AppointmentDto
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            PatientName = patientName,
            DoctorName = appointment.DoctorName,
            AppointmentDate = appointment.AppointmentDate,
            Status = appointment.Status,
            Reason = appointment.Reason,
            Notes = appointment.Notes
        };
    }
}
