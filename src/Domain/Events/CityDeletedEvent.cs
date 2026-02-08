namespace CleanArchitecture.Blazor.Domain.Events;

public class CityDeletedEvent : DomainEvent
{
    public CityDeletedEvent(City item)
    {
        Item = item;
    }

    public City Item { get; }
}
