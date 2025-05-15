using System;

[Serializable]
public class GameEventTriggerEffectGetItem : GameEventTriggerEffect
{
    public override EEventEffectType EffectType => EEventEffectType.GetItem;

    /// <summary>
    /// 获取的物品ID，-1表示随机
    /// </summary>
    public int itemID;

    public override void TriggerEffect()
    {

    }
}
