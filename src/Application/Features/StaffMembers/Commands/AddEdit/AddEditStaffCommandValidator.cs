namespace CleanArchitecture.Blazor.Application.Features.StaffMembers.Commands.AddEdit;

public class AddEditStaffCommandValidator : AbstractValidator<AddEditStaffCommand>
{
    public AddEditStaffCommandValidator()
    {
        RuleFor(v => v.EmployeeCode).MaximumLength(50).NotEmpty();
        RuleFor(v => v.StaffType).MaximumLength(50).NotEmpty();
        RuleFor(v => v.FirstName).MaximumLength(100).NotEmpty();
        RuleFor(v => v.LastName).MaximumLength(100).NotEmpty();
        RuleFor(v => v.LicenseNumber).MaximumLength(50);
        RuleFor(v => v.Title).MaximumLength(20);
    }
}
