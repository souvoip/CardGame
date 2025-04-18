using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuffItem
{
    public int BuffID;
    public int Stacks;
    public EBuffTarget Target;
    public EAddBuffTime AddBuffTime;

    public BuffItem(int buffID, int stacks, EBuffTarget target, EAddBuffTime addBuffTime)
    {
        BuffID = buffID;
        Stacks = stacks;
        Target = target;
        AddBuffTime = addBuffTime;
    }

    public BuffItem(JSONObject jSONObject)
    {
        BuffID = (int)jSONObject.GetField("BuffID").i;
        Stacks = (int)jSONObject.GetField("Stacks").i;
        Target = (EBuffTarget)jSONObject.GetField("Target").i;
        AddBuffTime = (EAddBuffTime)jSONObject.GetField("AddBuffTime").i;
    }

    public DetailInfo GetDetailInfo()
    {
        return BuffDataManager.GetBuff(BuffID).GetDetailInfo();
    }
}

/// <summary>
/// buff对象
/// </summary>
public enum EBuffTarget
{
    Self,
    Enemy,
    AllEnemy,
    All,
}

/// <summary>
/// 添加buff的时间
/// </summary>
public enum EAddBuffTime
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
    /// <summary>
    /// 受到伤害时
    /// </summary>
    DuringAttack,
}