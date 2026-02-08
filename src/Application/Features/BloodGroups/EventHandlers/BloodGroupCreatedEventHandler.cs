namespace CleanArchitecture.Blazor.Application.Features.BloodGroups.EventHandlers;

public class BloodGroupCreatedEventHandler : INotificationHandler<BloodGroupCreatedEvent>
{
    private readonly ILogger<BloodGroupCreatedEventHandler> _logger;

    public BloodGroupCreatedEventHandler(ILogger<BloodGroupCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(BloodGroupCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Blood Group Created: {Name}", notification.Item.Name);
        return Task.CompletedTask;
    }
}
