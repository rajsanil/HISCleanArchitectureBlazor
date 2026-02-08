using CleanArchitecture.Blazor.Application.Features.Patients.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.Patients.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Patients.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class PatientMapper
{
    public static partial PatientDto ToDto(Patient patient);
    public static partial Patient FromDto(PatientDto dto);
    public static partial Patient FromEditCommand(AddEditPatientCommand command);
    public static partial void ApplyChangesFrom(AddEditPatientCommand command, Patient patient);
    [MapperIgnoreSource(nameof(PatientDto.Id))]
    [MapperIgnoreSource(nameof(PatientDto.MRN))]
    public static partial AddEditPatientCommand CloneFromDto(PatientDto dto);
    public static partial AddEditPatientCommand ToEditCommand(PatientDto dto);
    public static partial IQueryable<PatientDto> ProjectTo(this IQueryable<Patient> q);
}
