
using System;

[Serializable]
public class GameEventTriggerEffectChangeCard : GameEventTriggerEffect
{
    public override EEventEffectType EffectType => EEventEffectType.ChangeCard;
}