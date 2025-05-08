using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potion_TakeDamage", menuName = "Data/Item/Potion/PotionItemData_TakeDamage")]
public class PotionItemData_TakeDamage : PotionItemData
{
    public Damage damage;

    public override void UseItem()
    {
        // 对所有敌人造成伤害
        for(int i = 0; i < BattleManager.Instance.EnemyRoles.Count; i++)
        {
            BattleManager.Instance.EnemyRoles[i].TakeDamage(null, damage.GetDamageValue());
        }
    }

    public override void UseItem(CharacterBase target)
    {
        // 对指定目标造成伤害
        target.TakeDamage(null, damage.GetDamageValue());
    }
}
