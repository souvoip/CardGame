using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkCard : CardBase
{
    public Damage BaseDamage;

    public AtkCard(JSONObject data)
    {
        LoadData(data);
    }

    public AtkCard()
    {
        CardType = ECardType.Atk;
    }

    public override void LoadData(JSONObject data)
    {
        base.LoadData(data);
        BaseDamage = new Damage(data.GetField("BaseDamage"));
    }

    public override void UseCard(CharacterBase target)
    {
        // 造成伤害前
        for (int i = 0; i < Buffs.Count; i++)
        {
            if (Buffs[i].AddBuffTime == EAddBuffTime.BeforeAttack)
            {
                if (Buffs[i].Target == EBuffTarget.Self)
                {
                    BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                }
                else if (Buffs[i].Target == EBuffTarget.Enemy)
                {
                    target.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                }
                else if (Buffs[i].Target == EBuffTarget.AllEnemy)
                {
                    for (int j = 0; j < BattleManager.Instance.EnemyRoles.Count; j++)
                    {
                        BattleManager.Instance.EnemyRoles[j].AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                    }
                }
                else if (Buffs[i].Target == EBuffTarget.All)
                {
                    BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                    for (int j = 0; j < BattleManager.Instance.EnemyRoles.Count; j++)
                    {
                        BattleManager.Instance.EnemyRoles[j].AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                    }
                }
            }
        }

        // 造成伤害
        var damage = BaseDamage;
        damage = BattleManager.Instance.Player.CalculateAtkDamage(damage);
        Debug.Log("P = " + damage.DamageRate);
        damage = target.CalculateHitDamage(damage);
        Debug.Log("E = " + damage.DamageRate);
        target.GetHit(damage);

        // 造成伤害后
        for (int i = 0; i < Buffs.Count; i++)
        {
            if (Buffs[i].AddBuffTime == EAddBuffTime.AfterAttack)
            {
                if (Buffs[i].Target == EBuffTarget.Self)
                {
                    BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                }
                else if (Buffs[i].Target == EBuffTarget.Enemy)
                {
                    target.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                }
                else if (Buffs[i].Target == EBuffTarget.AllEnemy)
                {
                    for (int j = 0; j < BattleManager.Instance.EnemyRoles.Count; j++)
                    {
                        BattleManager.Instance.EnemyRoles[j].AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                    }
                }
                else if (Buffs[i].Target == EBuffTarget.All)
                {
                    BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                    for (int j = 0; j < BattleManager.Instance.EnemyRoles.Count; j++)
                    {
                        BattleManager.Instance.EnemyRoles[j].AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                    }
                }
            }
        }
    }

    public override void UseCard()
    {
        // 造成伤害前
        for (int i = 0; i < Buffs.Count; i++)
        {
            if (Buffs[i].AddBuffTime == EAddBuffTime.BeforeAttack)
            {
                if (Buffs[i].Target == EBuffTarget.Self)
                {
                    BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                }
                else if (Buffs[i].Target == EBuffTarget.AllEnemy)
                {
                    for (int j = 0; j < BattleManager.Instance.EnemyRoles.Count; j++)
                    {
                        BattleManager.Instance.EnemyRoles[j].AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                    }
                }
                else if (Buffs[i].Target == EBuffTarget.All)
                {
                    BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                    for (int j = 0; j < BattleManager.Instance.EnemyRoles.Count; j++)
                    {
                        BattleManager.Instance.EnemyRoles[j].AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                    }
                }
            }
        }

        // 对所有敌人造成伤害
        var damage = BaseDamage;
        damage = BattleManager.Instance.Player.CalculateAtkDamage(damage);
        for (int i = 0; i < BattleManager.Instance.EnemyRoles.Count; i++)
        {
            damage = BattleManager.Instance.EnemyRoles[i].CalculateAtkDamage(damage);
            BattleManager.Instance.EnemyRoles[i].GetHit(damage);
        }

        // 造成伤害后
        for (int i = 0; i < Buffs.Count; i++)
        {
            if (Buffs[i].AddBuffTime == EAddBuffTime.AfterAttack)
            {
                if (Buffs[i].Target == EBuffTarget.Self)
                {
                    BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                }
                else if (Buffs[i].Target == EBuffTarget.AllEnemy)
                {
                    for (int j = 0; j < BattleManager.Instance.EnemyRoles.Count; j++)
                    {
                        BattleManager.Instance.EnemyRoles[j].AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                    }
                }
                else if (Buffs[i].Target == EBuffTarget.All)
                {
                    BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                    for (int j = 0; j < BattleManager.Instance.EnemyRoles.Count; j++)
                    {
                        BattleManager.Instance.EnemyRoles[j].AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                    }
                }
            }
        }
    }
}
