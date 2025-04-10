using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AtkCard", menuName = "Data/Card/AtkCard")]
public class AtkCard : CardBase
{
    public Damage BaseDamage;

    public AtkCard()
    {
        CardType = ECardType.Atk;
    }

    public override void UseCard(CharacterBase target)
    {
        // 造成伤害前
        AddBuffs(EAddBuffTime.BeforeAttack, target);

        // 造成伤害
        var damage = new Damage(BaseDamage);
        damage = BattleManager.Instance.Player.CalculateAtkDamage(damage);
        damage = target.CalculateHitDamage(damage);
        target.GetHit(damage);

        // 造成伤害后
        AddBuffs(EAddBuffTime.AfterAttack, target);

        base.UseCard(target);
    }

    public override void UseCard()
    {
        // 造成伤害前
        AddBuffs(EAddBuffTime.BeforeAttack);

        // 对所有敌人造成伤害
        var damage = new Damage(BaseDamage);
        damage = BattleManager.Instance.Player.CalculateAtkDamage(damage);
        for (int i = 0; i < BattleManager.Instance.EnemyRoles.Count; i++)
        {
            var tempDamage = new Damage(damage);
            tempDamage = BattleManager.Instance.EnemyRoles[i].CalculateAtkDamage(tempDamage);
            BattleManager.Instance.EnemyRoles[i].GetHit(tempDamage);
        }

        // 造成伤害后
        AddBuffs(EAddBuffTime.AfterAttack);

        base.UseCard();
    }
}
