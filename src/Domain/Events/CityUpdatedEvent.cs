namespace CleanArchitecture.Blazor.Domain.Events;

public class CityUpdatedEvent : DomainEvent
{
    public CityUpdatedEvent(City item)
    {
        Item = item;
    }

    public City Item { get; }
}
