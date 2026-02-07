using HIS.Domain.Common;

namespace HIS.Domain.Entities;

public class Patient : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string MedicalRecordNumber { get; set; } = string.Empty;
    
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
