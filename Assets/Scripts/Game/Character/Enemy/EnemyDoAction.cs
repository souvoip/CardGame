using System.Collections.Generic;
using System;

[Serializable]
public abstract class EnemyDoAction
{
    public abstract EEnemyActionType ActionType { get; }

    [NonSerialized]
    public CharacterBase self;

    public virtual void DoAction() { }
}

[System.Flags]
public enum EEnemyActionType
{
    /// <summary>
    /// 攻击玩家
    /// </summary>
    Attack = 1 << 0,
    /// <summary>
    /// 获取抵抗
    /// </summary>
    GetAesist = 1 << 1,
    /// <summary>
    /// 获取buff
    /// </summary>
    GetBuff = 1 << 2,
    /// <summary>
    /// 给予debuff
    /// </summary>
    GiveBuff = 1 << 3,
    /// <summary>
    /// 召唤敌人
    /// </summary>
    Summon = 1 << 4,
    /// <summary>
    /// 给予玩家卡牌
    /// </summary>
    GiveCard = 1 << 5,
}