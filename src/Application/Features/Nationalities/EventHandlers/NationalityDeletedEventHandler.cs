namespace CleanArchitecture.Blazor.Application.Features.Nationalities.EventHandlers;

public class NationalityDeletedEventHandler : INotificationHandler<NationalityDeletedEvent>
{
    private readonly ILogger<NationalityDeletedEventHandler> _logger;

    public NationalityDeletedEventHandler(ILogger<NationalityDeletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(NationalityDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Nationality Deleted: {Name}", notification.Item.Name);
        return Task.CompletedTask;
    }
}
