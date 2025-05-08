using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItemData : ItemDataBase
{
    public override EItemType ItemType => EItemType.Potion;

    public BattleAnimData useAnimData;

    public EUseType UseType;

    public EPotionType PotionType;

    /// <summary>
    /// 非指向性
    /// </summary>
    public virtual void UseItem()
    {
    }
    /// <summary>
    /// 指向性
    /// </summary>
    /// <param name="target"></param>
    public virtual void UseItem(CharacterBase target)
    {
    }

    //protected void AddBuffs(EAddBuffTime addTime, CharacterBase characterTarget = null)
    //{
    //    for (int i = 0; i < Buffs.Count; i++)
    //    {
    //        if (Buffs[i].AddBuffTime == addTime)
    //        {
    //            if (Buffs[i].Target == ETargetRole.Self)
    //            {
    //                BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
    //            }
    //            else if (Buffs[i].Target == ETargetRole.Enemy)
    //            {
    //                if (characterTarget == null) { return; }
    //                if (characterTarget.IsDie) { return; }
    //                characterTarget.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
    //            }
    //            else if (Buffs[i].Target == ETargetRole.AllEnemy)
    //            {
    //                for (int j = 0; j < BattleManager.Instance.EnemyRoles.Count; j++)
    //                {
    //                    if (BattleManager.Instance.EnemyRoles[j].IsDie) { continue; }
    //                    BattleManager.Instance.EnemyRoles[j].AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
    //                }
    //            }
    //            else if (Buffs[i].Target == ETargetRole.All)
    //            {
    //                BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
    //                for (int j = 0; j < BattleManager.Instance.EnemyRoles.Count; j++)
    //                {
    //                    if (BattleManager.Instance.EnemyRoles[j].IsDie) { continue; }
    //                    BattleManager.Instance.EnemyRoles[j].AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
    //                }
    //            }
    //        }
    //    }
    //}

    public virtual DetailInfo GetDetailInfo()
    {
        DetailInfo info = new DetailInfo();
        info.Title = Name;
        info.Description = Description;
        return info;
    }
}

public enum EPotionType
{
    Drink,
    Throw
}
