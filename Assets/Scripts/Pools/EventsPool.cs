using UnityEngine.Events;
public static class EventsPool
{
    public readonly static UnityEvent<AttackDirection> UserSwipedEvent = new UnityEvent<AttackDirection>();

    public readonly static UnityEvent ClearPoolsEvent = new UnityEvent();

}
