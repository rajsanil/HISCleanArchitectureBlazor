namespace CleanArchitecture.Blazor.Domain.Events;

public class MaritalStatusDeletedEvent : DomainEvent
{
    public MaritalStatusDeletedEvent(MaritalStatus item)
    {
        Item = item;
    }

    public MaritalStatus Item { get; }
}
