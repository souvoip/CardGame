using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Damage
{
    public int DamageValue;
    public float DamageRate;

    public Damage(int damageValue, float damageRate)
    {
        DamageValue = damageValue;
        DamageRate = damageRate;
    }

    public int GetDamage()
    {
        return Mathf.CeilToInt(DamageValue * DamageRate);
    }
}