using UnityEngine.Events;
public static class EventsPool
{

    public readonly static UnityEvent<AttackDirection> UserSwipedEvent = new UnityEvent<AttackDirection>();

    public readonly static UnityEvent CharacterAttackedEvent = new UnityEvent();

    public readonly static UnityEvent<int> CharacterDamagedEvent = new UnityEvent<int>();

    public readonly static UnityEvent<int> ComboLevelEvent = new UnityEvent<int>();

    public readonly static UnityEvent<bool> FinishedHitEvent = new UnityEvent<bool>();

    public readonly static UnityEvent<bool> GameFinishedEvent = new UnityEvent<bool>();
    
    public readonly static UnityEvent<SkinItem> UpdateSkinEvent = new UnityEvent<SkinItem>();

    public readonly static UnityEvent ClearPoolsEvent = new UnityEvent();

    public readonly static UnityEvent UpdateUIEvent = new UnityEvent();

    public readonly static UnityEvent GameStartedEvent = new UnityEvent();


}