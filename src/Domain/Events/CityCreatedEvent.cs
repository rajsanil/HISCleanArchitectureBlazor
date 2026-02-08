namespace CleanArchitecture.Blazor.Domain.Events;

public class CityCreatedEvent : DomainEvent
{
    public CityCreatedEvent(City item)
    {
        Item = item;
    }

    public City Item { get; }
}
