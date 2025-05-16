using System;

[Serializable]
public class GameEventTriggerEffect
{
    public virtual EEventEffectType EffectType { get; }

    public virtual void TriggerEffect() { }
}

public enum EEventEffectType
{
    ChangeAttribute, // 属性相关事件
    ChangeCard, // 卡牌相关事件, 获得，移除，升级等
    JumpOtherRoom, // 跳转其他场景，如触发战斗， 普通战斗，精英战斗，特殊指定战斗，进入商店等等
    GetItem, // 获得道具
}