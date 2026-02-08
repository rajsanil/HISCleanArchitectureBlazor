namespace CleanArchitecture.Blazor.Domain.Events;

public class NationalityCreatedEvent : DomainEvent
{
    public NationalityCreatedEvent(Nationality item)
    {
        Item = item;
    }

    public Nationality Item { get; }
}
