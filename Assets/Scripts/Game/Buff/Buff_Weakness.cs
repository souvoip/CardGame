using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 造成伤害倍率变化buff
/// </summary>
[CreateAssetMenu(fileName = "WeaknessBuff", menuName = "Data/Buff/WeaknessBuff")]
public class Buff_Weakness : BuffBase
{
    public float changeRate = 0.75f; // 25% less damage dealt

    public override void OnTurnEnd()
    {
        AddStacks(-1);
        base.OnTurnEnd();
    }

    public override void AddEvents()
    {
        base.AddEvents();
        target.ChangeAtkDamageEvent.Add(buffID, ChangeHitDamage);
    }

    public override void RemoveEvents()
    {
        base.RemoveEvents();
        target.ChangeAtkDamageEvent.Remove(buffID);
    }

    private void ChangeHitDamage(Damage damage)
    {
        damage.DamageRate *= changeRate;
    }
}
