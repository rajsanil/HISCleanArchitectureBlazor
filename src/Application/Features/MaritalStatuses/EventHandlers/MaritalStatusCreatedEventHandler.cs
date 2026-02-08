namespace CleanArchitecture.Blazor.Application.Features.MaritalStatuses.EventHandlers;

public class MaritalStatusCreatedEventHandler : INotificationHandler<MaritalStatusCreatedEvent>
{
    private readonly ILogger<MaritalStatusCreatedEventHandler> _logger;

    public MaritalStatusCreatedEventHandler(ILogger<MaritalStatusCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(MaritalStatusCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Marital Status Created: {Name}", notification.Item.Name);
        return Task.CompletedTask;
    }
}
