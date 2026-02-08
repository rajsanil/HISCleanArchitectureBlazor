namespace CleanArchitecture.Blazor.Domain.Events;

public class BloodGroupDeletedEvent : DomainEvent
{
    public BloodGroupDeletedEvent(BloodGroup item)
    {
        Item = item;
    }

    public BloodGroup Item { get; }
}
