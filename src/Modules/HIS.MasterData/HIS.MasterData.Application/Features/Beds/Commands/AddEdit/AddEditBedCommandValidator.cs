namespace HIS.MasterData.Application.Features.Beds.Commands.AddEdit;

public class AddEditBedCommandValidator : AbstractValidator<AddEditBedCommand>
{
    public AddEditBedCommandValidator()
    {
        RuleFor(v => v.Code).MaximumLength(20).NotEmpty();
        RuleFor(v => v.RoomId).GreaterThan(0).WithMessage("Room is required.");
        RuleFor(v => v.BedStatus).MaximumLength(50).NotEmpty();
    }
}
