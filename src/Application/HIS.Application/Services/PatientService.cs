using HIS.Application.DTOs;
using HIS.Application.Interfaces;
using HIS.Domain.Entities;
using HIS.Domain.Interfaces;

namespace HIS.Application.Services;

public class PatientService : IPatientService
{
    private readonly IRepository<Patient> _patientRepository;

    public PatientService(IRepository<Patient> patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync(CancellationToken cancellationToken = default)
    {
        var patients = await _patientRepository.GetAllAsync(cancellationToken);
        return patients.Select(MapToDto);
    }

    public async Task<PatientDto?> GetPatientByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var patient = await _patientRepository.GetByIdAsync(id, cancellationToken);
        return patient != null ? MapToDto(patient) : null;
    }

    public async Task<PatientDto> CreatePatientAsync(CreatePatientDto dto, CancellationToken cancellationToken = default)
    {
        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            MedicalRecordNumber = GenerateMedicalRecordNumber(),
            CreatedAt = DateTime.UtcNow
        };

        var createdPatient = await _patientRepository.AddAsync(patient, cancellationToken);
        return MapToDto(createdPatient);
    }

    public async Task UpdatePatientAsync(Guid id, CreatePatientDto dto, CancellationToken cancellationToken = default)
    {
        var patient = await _patientRepository.GetByIdAsync(id, cancellationToken);
        if (patient == null)
            throw new InvalidOperationException($"Patient with ID {id} not found.");

        patient.FirstName = dto.FirstName;
        patient.LastName = dto.LastName;
        patient.DateOfBirth = dto.DateOfBirth;
        patient.Gender = dto.Gender;
        patient.Email = dto.Email;
        patient.PhoneNumber = dto.PhoneNumber;
        patient.Address = dto.Address;
        patient.UpdatedAt = DateTime.UtcNow;

        await _patientRepository.UpdateAsync(patient, cancellationToken);
    }

    public async Task DeletePatientAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _patientRepository.DeleteAsync(id, cancellationToken);
    }

    private static PatientDto MapToDto(Patient patient)
    {
        return new PatientDto
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = patient.DateOfBirth,
            Gender = patient.Gender,
            Email = patient.Email,
            PhoneNumber = patient.PhoneNumber,
            Address = patient.Address,
            MedicalRecordNumber = patient.MedicalRecordNumber
        };
    }

    private static string GenerateMedicalRecordNumber()
    {
        return $"MRN{DateTime.UtcNow:yyyyMMdd}{Random.Shared.Next(1000, 9999)}";
    }
}
