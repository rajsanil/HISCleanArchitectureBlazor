namespace CleanArchitecture.Blazor.Application.Features.Cities.EventHandlers;

public class CityDeletedEventHandler : INotificationHandler<CityDeletedEvent>
{
    private readonly ILogger<CityDeletedEventHandler> _logger;

    public CityDeletedEventHandler(ILogger<CityDeletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CityDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("City Deleted: {Name}", notification.Item.Name);
        return Task.CompletedTask;
    }
}
