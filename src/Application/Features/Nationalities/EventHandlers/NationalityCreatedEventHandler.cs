namespace CleanArchitecture.Blazor.Application.Features.Nationalities.EventHandlers;

public class NationalityCreatedEventHandler : INotificationHandler<NationalityCreatedEvent>
{
    private readonly ILogger<NationalityCreatedEventHandler> _logger;

    public NationalityCreatedEventHandler(ILogger<NationalityCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(NationalityCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Nationality Created: {Name}", notification.Item.Name);
        return Task.CompletedTask;
    }
}
