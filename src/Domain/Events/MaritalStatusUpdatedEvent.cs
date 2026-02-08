namespace CleanArchitecture.Blazor.Domain.Events;

public class MaritalStatusUpdatedEvent : DomainEvent
{
    public MaritalStatusUpdatedEvent(MaritalStatus item)
    {
        Item = item;
    }

    public MaritalStatus Item { get; }
}
