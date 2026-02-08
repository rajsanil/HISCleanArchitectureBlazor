namespace CleanArchitecture.Blazor.Application.Features.BloodGroups.EventHandlers;

public class BloodGroupUpdatedEventHandler : INotificationHandler<BloodGroupUpdatedEvent>
{
    private readonly ILogger<BloodGroupUpdatedEventHandler> _logger;

    public BloodGroupUpdatedEventHandler(ILogger<BloodGroupUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(BloodGroupUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Blood Group Updated: {Name}", notification.Item.Name);
        return Task.CompletedTask;
    }
}
