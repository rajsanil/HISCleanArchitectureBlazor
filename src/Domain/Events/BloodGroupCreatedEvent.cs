namespace CleanArchitecture.Blazor.Domain.Events;

public class BloodGroupCreatedEvent : DomainEvent
{
    public BloodGroupCreatedEvent(BloodGroup item)
    {
        Item = item;
    }

    public BloodGroup Item { get; }
}
