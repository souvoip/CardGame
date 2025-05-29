using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuffItem : ISaveLoad
{
    public int BuffID;
    public int Stacks;
    public ETargetRole Target;
    public EBuffTriggerTime AddBuffTime;

    public DetailInfo GetDetailInfo()
    {
        return BuffDataManager.GetBuff(BuffID).GetDetailInfo();
    }

    public void Load(JSONObject data)
    {
        BuffID = (int)data.GetField("BuffID").i;
        Stacks = (int)data.GetField("Stacks").i;
        Target = (ETargetRole)data.GetField("Target").i;
        AddBuffTime = (EBuffTriggerTime)data.GetField("AddBuffTime").i;
    }

    public JSONObject Save()
    {
        JSONObject data = JSONObject.Create();
        data.AddField("BuffID", BuffID);
        data.AddField("Stacks", Stacks);
        data.AddField("Target", (int)Target);
        data.AddField("AddBuffTime", (int)AddBuffTime);
        return data;
    }
}

/// <summary>
/// buff对象
/// </summary>
public enum ETargetRole
{
    Self,
    Enemy,
    AllEnemy,
    All,
}

/// <summary>
/// 添加buff的时间
/// </summary>
public enum EBuffTriggerTime
{
    None,
    /// <summary>
    /// 攻击造成伤害前
    /// </summary>
    BeforeAttack,
    /// <summary>
    /// 攻击造成伤害后
    /// </summary>
    AfterAttack,
    ///// <summary>
    ///// 受到伤害时
    ///// </summary>
    //DuringAttack,
}