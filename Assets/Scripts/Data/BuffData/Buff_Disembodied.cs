using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 无实体: 受到的所有伤害变为1
/// </summary>
[CreateAssetMenu(fileName = "DisembodiedBuff", menuName = "Data/Buff/DisembodiedBuff")]
public class Buff_Disembodied : BuffBase
{
    public override void AddEvents()
    {
        base.AddEvents();
        target.ChangeTakeDamageEvent.Add(buffID, ChangeHitDamage);
    }

    public override void RemoveEvents()
    {
        base.RemoveEvents();
        target.ChangeTakeDamageEvent.Remove(buffID);
    }

    private void ChangeHitDamage(Damage damage)
    {
        damage.DamageValue = 1;
        damage.DamageRate = 1;
        damage.isNext = false;
    }
}
