using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 添加buff的遗物
/// </summary>
[CreateAssetMenu(fileName = "Remains_AddBuff", menuName = "Data/Item/Remains/AddBuffRemains")]
public class RemainsItemData_AddBuff : RemainsItemData
{

    public List<RemainsBuffItem> Buffs;

    public override void OnAcquire(bool isFirstGet = true)
    {
        foreach (var buff in Buffs)
        {
            switch (buff.TriggerTime)
            {
                case ETriggerTime.StartBattle:
                    TurnManager.OnStartBattle += () => { AddBuff(buff); };
                    break;
                case ETriggerTime.PlayerTurnStart:
                    TurnManager.OnPlayerTurnStart += () => { AddBuff(buff); };
                    break;
                case ETriggerTime.PlayerTurnEnd:
                    TurnManager.OnPlayerTurnEnd += () => { AddBuff(buff); };
                    break;
                case ETriggerTime.EnemyTurnStart:
                    TurnManager.OnEnemyTurnStart += () => { AddBuff(buff); };
                    break;
                case ETriggerTime.EnemyTurnEnd:
                    TurnManager.OnEnemyTurnEnd += () => { AddBuff(buff); };
                    break;
                case ETriggerTime.EndBattle:
                    break;
                case ETriggerTime.PlayerDead:
                    break;
                case ETriggerTime.EnemyDead:
                    break;
                case ETriggerTime.PlayerHit:
                    BattleManager.Instance.Player.TakeDamageEvent += (enemy, _) => { AddBuff(enemy, buff); };
                    break;
                case ETriggerTime.PlayerAttack:
                    BattleManager.Instance.Player.CauseDamageEvent += (enemy, _) => { AddBuff(enemy, buff); };
                    break;
                default:
                    break;
            }
        }
    }

    public override void OnBattleStart()
    {
        foreach (var buff in Buffs)
        {
            switch (buff.TriggerTime)
            {
                case ETriggerTime.EnemyDead:
                    BattleManager.Instance.EnemyRoles.ForEach(enemy => enemy.DieEvent += () => { AddBuff(enemy, buff); });
                    break;
                default:
                    break;
            }
        }
    }

    private void AddBuff(RemainsBuffItem buffItem)
    {
        switch (buffItem.Target)
        {
            case ETargetRole.Self:
                BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(buffItem.BuffID), buffItem.Stacks);
                break;
            case ETargetRole.AllEnemy:
                BattleManager.Instance.EnemyRoles.ForEach(enemy => enemy.AddBuff(BuffDataManager.GetBuff(buffItem.BuffID), buffItem.Stacks));
                break;
            case ETargetRole.All:
                BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(buffItem.BuffID), buffItem.Stacks);
                BattleManager.Instance.EnemyRoles.ForEach(enemy => enemy.AddBuff(BuffDataManager.GetBuff(buffItem.BuffID), buffItem.Stacks));
                break;
        }
    }

    private void AddBuff(CharacterBase targetEnemy, RemainsBuffItem buffItem)
    {
        switch (buffItem.Target)
        {
            case ETargetRole.Self:
                AddBuff(buffItem);
                break;
            case ETargetRole.Enemy:
                targetEnemy.AddBuff(BuffDataManager.GetBuff(buffItem.BuffID), buffItem.Stacks);
                break;
            case ETargetRole.AllEnemy:
                AddBuff(buffItem);
                break;
            case ETargetRole.All:
                AddBuff(buffItem);
                break;
        }
    }
}

[Serializable]
public class RemainsBuffItem : BuffItem
{
    public ETriggerTime TriggerTime;
}

public enum ETriggerTime
{
    GetThis,          // 获得此遗物时
    StartBattle,      // 战斗开始
    PlayerTurnStart,  // 玩家回合开始
    PlayerTurnEnd,    // 玩家回合结束
    EnemyTurnStart,  // 敌人回合开始
    EnemyTurnEnd,    // 敌人回合结束
    EndBattle,       // 战斗胜利
    PlayerDead,     // 玩家死亡
    EnemyDead,      // 敌人死亡
    PlayerHit,      // 玩家被攻击
    PlayerAttack,    // 玩家攻击
}