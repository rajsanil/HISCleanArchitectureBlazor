namespace CleanArchitecture.Blazor.Domain.Events;

public class BloodGroupUpdatedEvent : DomainEvent
{
    public BloodGroupUpdatedEvent(BloodGroup item)
    {
        Item = item;
    }

    public BloodGroup Item { get; }
}
