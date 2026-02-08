namespace CleanArchitecture.Blazor.Application.Features.MaritalStatuses.EventHandlers;

public class MaritalStatusDeletedEventHandler : INotificationHandler<MaritalStatusDeletedEvent>
{
    private readonly ILogger<MaritalStatusDeletedEventHandler> _logger;

    public MaritalStatusDeletedEventHandler(ILogger<MaritalStatusDeletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(MaritalStatusDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Marital Status Deleted: {Name}", notification.Item.Name);
        return Task.CompletedTask;
    }
}
