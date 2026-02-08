namespace CleanArchitecture.Blazor.Application.Features.Rooms.Commands.AddEdit;

public class AddEditRoomCommandValidator : AbstractValidator<AddEditRoomCommand>
{
    public AddEditRoomCommandValidator()
    {
        RuleFor(v => v.Code).MaximumLength(20).NotEmpty();
        RuleFor(v => v.Name).MaximumLength(200).NotEmpty();
        RuleFor(v => v.RoomType).MaximumLength(50).NotEmpty();
        RuleFor(v => v.LocationId).GreaterThan(0).WithMessage("Location is required.");
    }
}
