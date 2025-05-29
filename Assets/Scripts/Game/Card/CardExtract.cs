using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardExtract : ISaveLoad
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

    public void Load(JSONObject data)
    {
        origin = (ECardRegion)data.GetField("origin").i;
        target = (ECardRegion)data.GetField("target").i;
        Count = (int)data.GetField("count").i;
        cardType = (EExtractCardType)data.GetField("cardType").i;
        mode = (EExtractMode)data.GetField("mode").i;
    }

    public JSONObject Save()
    {
        JSONObject data = JSONObject.Create();
        data.AddField("origin", (int)origin);
        data.AddField("target", (int)target);
        data.AddField("count", Count);
        data.AddField("cardType", (int)cardType);
        data.AddField("mode", (int)mode);
        return data;
    }
}

[System.Flags]
public enum EExtractCardType
{
    Atkack = 1 << 0,
    Skill = 1 << 1,
    Ability = 1 << 2,
    State = 1 << 3,
    Other = 1 << 4,
    All = Atkack | Skill | Ability | State | Other
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