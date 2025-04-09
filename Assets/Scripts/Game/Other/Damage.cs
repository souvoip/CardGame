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

    public Damage(JSONObject json)
    {
        DamageValue = (int)json.GetField("DamageValue").i;
        DamageRate = json.GetField("DamageRate").f;
    }

    public Damage(Damage damage)
    {
        DamageValue = damage.DamageValue;
        DamageRate = damage.DamageRate;
    }

    public int GetDamage()
    {
        return Mathf.CeilToInt(DamageValue * DamageRate);
    }
}