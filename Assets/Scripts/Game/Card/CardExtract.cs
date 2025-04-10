using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardExtract
{
    /// <summary>
    /// 原目标
    /// </summary>
    public ECardRegion origin;
    /// <summary>
    /// 抽取到的目标
    /// </summary>
    public ECardRegion target;
    /// <summary>
    /// 抽取数量
    /// </summary>
    public int Count;
    /// <summary>
    /// 可以抽取的卡牌类型
    /// </summary>
    public EExtractCardType cardType;
    /// <summary>
    /// 抽取模式
    /// </summary>
    public EExtractMode mode;
}

[System.Flags]
public enum EExtractCardType
{
    Atkack = 1 << 0,
    Skill = 1 << 1,
    Ability = 1 << 2,
    State = 1 << 3,
    Other = 1 << 4,
}

public enum EExtractMode
{
    /// <summary>
    /// 随机
    /// </summary>
    Random,
    /// <summary>
    /// 顺序
    /// </summary>
    Order,
    /// <summary>
    /// 指定
    /// </summary>
    Specified,
}