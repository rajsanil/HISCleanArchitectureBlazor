namespace CleanArchitecture.Blazor.Application.Features.BloodGroups.EventHandlers;

public class BloodGroupDeletedEventHandler : INotificationHandler<BloodGroupDeletedEvent>
{
    private readonly ILogger<BloodGroupDeletedEventHandler> _logger;

    public BloodGroupDeletedEventHandler(ILogger<BloodGroupDeletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(BloodGroupDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Blood Group Deleted: {Name}", notification.Item.Name);
        return Task.CompletedTask;
    }
}
