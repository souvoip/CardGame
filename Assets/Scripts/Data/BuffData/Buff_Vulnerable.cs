using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 受到伤害倍率变化buff
/// </summary>
[CreateAssetMenu(fileName = "VulnerableBuff", menuName = "Data/Buff/VulnerableBuff")]
public class Buff_Vulnerable : BuffBase
{
    public float changeRate = 1.5f; // 50% more damage taken

    public override void OnTurnEnd()
    {
        AddStacks(-1);
        base.OnTurnEnd();
    }

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
        damage.DamageRate *= changeRate;
    }
}
