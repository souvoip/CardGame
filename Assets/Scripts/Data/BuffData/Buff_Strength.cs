using UnityEngine;

[CreateAssetMenu(fileName = "StrengthBuff", menuName = "Data/Buff/StrengthBuff")]
public class Buff_Strength : BuffBase
{
    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
    }

    public override void AddEvents()
    {
        base.AddEvents();
        target.ChangeCauseDamageEvent.Add(buffID, ChangeAtkDamage);
    }

    public override void RemoveEvents()
    {
        base.RemoveEvents();
        target.ChangeCauseDamageEvent.Remove(buffID);
    }

    private void ChangeAtkDamage(Damage damage)
    {
        damage.DamageValue += currentStacks;
    }
}
