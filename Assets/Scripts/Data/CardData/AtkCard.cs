using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AtkCard", menuName = "Data/Card/AtkCard")]
public class AtkCard : CardBase
{
    public Damage BaseDamage;

    /// <summary>
    /// 攻击次数
    /// </summary>
    public int HitCount;

    public AtkCard()
    {
        CardType = ECardType.Atk;
    }

    public override void UseCard(CharacterBase target)
    {
        // 造成伤害前
        AddBuffs(EAddBuffTime.BeforeAttack, target);

        // 造成伤害
        int damageValue = GameTools.CalculateDamage(BattleManager.Instance.Player, target, BaseDamage);
        for (int i = 0; i < HitCount; i++)
        {
            if(target.IsDie) { break; }
            BattleManager.Instance.Player.AtkTarget(target, damageValue);
            //target.ChangeAttribute(ERoleAttribute.HP, -damageValue);
            BattleAnimManager.Instance.PlayAnim(target.transform.position, cardAnimData);
        }

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
        damage = BattleManager.Instance.Player.CalculateCauseDamage(damage);
        for (int i = BattleManager.Instance.EnemyRoles.Count - 1; i >= 0; i--)
        {
            if(BattleManager.Instance.EnemyRoles[i].IsDie) { continue; }
            var tempDamage = new Damage(damage);
            tempDamage = BattleManager.Instance.EnemyRoles[i].CalculateTakeDamage(tempDamage);
            for (int j = 0; j < HitCount; j++)
            {
                BattleManager.Instance.Player.AtkTarget(BattleManager.Instance.EnemyRoles[i], tempDamage.GetDamageValue());
                //BattleManager.Instance.EnemyRoles[i].ChangeAttribute(ERoleAttribute.HP, -tempDamage.GetDamageValue());
                BattleAnimManager.Instance.PlayAnim(BattleManager.Instance.EnemyRoles[i].transform.position, cardAnimData);
            }
        }

        // 造成伤害后
        AddBuffs(EAddBuffTime.AfterAttack);

        base.UseCard();
    }

    public override string GetDesc()
    {
        string desc = GetFeaturesDesc();
        if(UseType == EUseType.Directivity) { desc += "造成"; }
        else if(UseType == EUseType.NonDirectivity) { desc += "对所有敌人造成"; }
        // 计算实际伤害
        int baseDamageValue = BaseDamage.GetDamageValue();
        int damageValue = GameTools.CalculateDamage(BattleManager.Instance.Player, BattleManager.Instance.nowSelectEnemy, BaseDamage);
        string damageStr = baseDamageValue > damageValue ? $"<color=#FF0000>{damageValue}</color>" : baseDamageValue == damageValue ? $"{damageValue}" : $"<color=#00FF00>{damageValue}</color>";
        if(HitCount == 1)
        {
            desc += damageStr + "点伤害";
        }
        else
        {
            desc += damageStr + "点伤害" + HitCount + "次";
        }
        desc += "\n" + GetBuffsDesc() + Desc;
        return desc;
    }
}
