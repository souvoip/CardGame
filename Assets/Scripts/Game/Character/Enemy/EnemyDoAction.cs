using System.Collections.Generic;
using System;

[Serializable]
public abstract class EnemyDoAction
{
    public abstract EEnemyActionType ActionType { get; }

    [NonSerialized]
    public CharacterBase self;

    public abstract void DoAction();

    /// <summary>
    /// TODO: 获取图标
    /// </summary>
    public virtual void GetIcon() { }
}

public enum EEnemyActionType
{
    /// <summary>
    /// 攻击玩家
    /// </summary>
    Attack,
    /// <summary>
    /// 获取抵抗
    /// </summary>
    GetAesist,
    /// <summary>
    /// 获取buff
    /// </summary>
    GetBuff,
    /// <summary>
    /// 给予debuff
    /// </summary>
    GiveBuff,
    /// <summary>
    /// 召唤敌人
    /// </summary>
    Summon,
    /// <summary>
    /// 给予玩家卡牌
    /// </summary>
    GiveCards,
    /// <summary>
    /// 混合
    /// </summary>
    Mix,
}