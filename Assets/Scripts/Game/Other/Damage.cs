using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Damage: ISaveLoad
{
    public int DamageValue;
    public float DamageRate;
    /// <summary>
    /// 是否需要继续计算buff伤害
    /// </summary>
    public bool isNext = true;

    public Damage(int damageValue, float damageRate)
    {
        DamageValue = damageValue;
        DamageRate = damageRate;
    }

    public Damage(Damage damage)
    {
        DamageValue = damage.DamageValue;
        DamageRate = damage.DamageRate;
        isNext = damage.isNext;
    }

    public Damage(JSONObject data)
    {
        Load(data);
    }

    public int GetDamageValue()
    {
        return Mathf.CeilToInt(DamageValue * DamageRate);
    }

    public JSONObject Save()
    {
        JSONObject data = JSONObject.Create();
        data.AddField("DamageValue", DamageValue);
        data.AddField("DamageRate", DamageRate);
        data.AddField("isNext", isNext);
        return data;
    }

    public void Load(JSONObject data)
    {
        DamageValue = (int)data.GetField("DamageValue").i;
        DamageRate = data.GetField("DamageRate").f;
        isNext = data.GetField("isNext").b;
    }
}