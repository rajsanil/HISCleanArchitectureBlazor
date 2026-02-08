namespace CleanArchitecture.Blazor.Application.Features.Nationalities.EventHandlers;

public class NationalityUpdatedEventHandler : INotificationHandler<NationalityUpdatedEvent>
{
    private readonly ILogger<NationalityUpdatedEventHandler> _logger;

    public NationalityUpdatedEventHandler(ILogger<NationalityUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(NationalityUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Nationality Updated: {Name}", notification.Item.Name);
        return Task.CompletedTask;
    }
}
