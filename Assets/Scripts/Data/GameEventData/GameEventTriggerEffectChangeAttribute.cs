using System;
using UnityEngine;

[Serializable]
public class GameEventTriggerEffectChangeAttribute : GameEventTriggerEffect
{
    public override EEventEffectType EffectType => EEventEffectType.ChangeAttribute;

    public ERoleAttribute ChangeAttribute;

    /// <summary>
    /// 直接改变属性
    /// </summary>
    public int ChangeValue;

    /// <summary>
    /// 按照百分比来改变, 百分比的计算属性
    /// </summary>
    public ERoleAttribute CalculateAttribute;
    /// <summary>
    /// 计算属性改变后的值
    /// </summary>
    public float Rate;

    public override void TriggerEffect()
    {
        int value = ChangeValue + Mathf.CeilToInt(BattleManager.Instance.Player.GetAttributeValue(CalculateAttribute) * Rate);
        BattleManager.Instance.Player.ChangeAttribute(ChangeAttribute, ChangeValue);
    }
}
