namespace HIS.Application.DTOs;

public class CreateAppointmentDto
{
    public Guid PatientId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
}
