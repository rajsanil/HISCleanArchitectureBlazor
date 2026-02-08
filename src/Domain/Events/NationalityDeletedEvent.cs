namespace CleanArchitecture.Blazor.Domain.Events;

public class NationalityDeletedEvent : DomainEvent
{
    public NationalityDeletedEvent(Nationality item)
    {
        Item = item;
    }

    public Nationality Item { get; }
}
