namespace CleanArchitecture.Blazor.Domain.Events;

public class NationalityUpdatedEvent : DomainEvent
{
    public NationalityUpdatedEvent(Nationality item)
    {
        Item = item;
    }

    public Nationality Item { get; }
}
