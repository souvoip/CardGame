public class GameEventTriggerEffectChangeAttribute : GameEventTriggerEffect
{
    public override EEventEffectType EffectType => EEventEffectType.ChangeAttribute;

    public ERoleAttribute Attribute;

    public int ChangeValue;

    public override void TriggerEffect()
    {
        BattleManager.Instance.Player.ChangeAttribute(Attribute, ChangeValue);
    }
}
