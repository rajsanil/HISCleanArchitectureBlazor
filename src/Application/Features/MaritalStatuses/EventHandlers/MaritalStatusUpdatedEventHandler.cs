namespace CleanArchitecture.Blazor.Application.Features.MaritalStatuses.EventHandlers;

public class MaritalStatusUpdatedEventHandler : INotificationHandler<MaritalStatusUpdatedEvent>
{
    private readonly ILogger<MaritalStatusUpdatedEventHandler> _logger;

    public MaritalStatusUpdatedEventHandler(ILogger<MaritalStatusUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(MaritalStatusUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Marital Status Updated: {Name}", notification.Item.Name);
        return Task.CompletedTask;
    }
}
