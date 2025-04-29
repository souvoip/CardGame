using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Damage
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

    public int GetDamageValue()
    {
        return Mathf.CeilToInt(DamageValue * DamageRate);
    }
}