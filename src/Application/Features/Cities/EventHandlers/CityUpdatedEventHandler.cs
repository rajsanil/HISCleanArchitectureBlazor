namespace CleanArchitecture.Blazor.Application.Features.Cities.EventHandlers;

public class CityUpdatedEventHandler : INotificationHandler<CityUpdatedEvent>
{
    private readonly ILogger<CityUpdatedEventHandler> _logger;

    public CityUpdatedEventHandler(ILogger<CityUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CityUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("City Updated: {Name}", notification.Item.Name);
        return Task.CompletedTask;
    }
}
