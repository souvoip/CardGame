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

    public Damage(int damageValue, float damageRate)
    {
        DamageValue = damageValue;
        DamageRate = damageRate;
    }

    public Damage(Damage damage)
    {
        DamageValue = damage.DamageValue;
        DamageRate = damage.DamageRate;
    }

    public int GetDamageValue()
    {
        return Mathf.CeilToInt(DamageValue * DamageRate);
    }
}