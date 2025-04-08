using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "VulnerableBuff", menuName = "Data/Buff/VulnerableBuff")]
public class Buff_Vulnerable : BuffBase
{

    public float changeRate = 1.5f; // 50% more damage taken

    public override void OnTurnEnd()
    {
        AddStacks(-1);
        base.OnTurnEnd();
        Debug.Log("Vulnerable End");
    }

    //public override float DamageRateChange(float baseRate)
    //{
    //    return baseRate * changeRate;
    //}
}
