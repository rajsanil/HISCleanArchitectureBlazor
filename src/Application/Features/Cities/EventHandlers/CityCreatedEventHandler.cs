namespace CleanArchitecture.Blazor.Application.Features.Cities.EventHandlers;

public class CityCreatedEventHandler : INotificationHandler<CityCreatedEvent>
{
    private readonly ILogger<CityCreatedEventHandler> _logger;

    public CityCreatedEventHandler(ILogger<CityCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CityCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("City Created: {Name}", notification.Item.Name);
        return Task.CompletedTask;
    }
}
