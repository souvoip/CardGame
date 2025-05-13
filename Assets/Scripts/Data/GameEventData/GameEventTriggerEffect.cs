
public class GameEventTriggerEffect
{
    public virtual EEventEffectType EffectType { get; }

    public virtual void TriggerEffect() { }
}

public enum EEventEffectType
{
    ChangeAttribute,
    ChangeCard,
    Battle,
    GetItem,
}