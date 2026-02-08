namespace CleanArchitecture.Blazor.Domain.Events;

public class MaritalStatusCreatedEvent : DomainEvent
{
    public MaritalStatusCreatedEvent(MaritalStatus item)
    {
        Item = item;
    }

    public MaritalStatus Item { get; }
}
