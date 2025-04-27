using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public abstract class EnemyDoAction
{
    public abstract EEnemyActionType ActionType { get; }

    [NonSerialized]
    public CharacterBase self;

    public BattleAnimData actionAnim;

    public abstract void DoAction();

    /// <summary>
    /// TODO: 获取图标
    /// </summary>
    public virtual ActionInfo GetActionInfo() { return null; }
}


public class ActionInfo
{
    public Sprite icon;
    public string text;
    public DetailInfo detailInfo;
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