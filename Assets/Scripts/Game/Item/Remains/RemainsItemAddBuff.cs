using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 添加buff的遗物
/// </summary>
[CreateAssetMenu(fileName = "Remains_AddBuff", menuName = "Data/Remains/AddBuffRemains")]
public class RemainsItemAddBuff : RemainsItem
{
    public List<RemainsBuffItem> Buffs;

    public override void OnAcquire()
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
                    break;
                case ETriggerTime.PlayerAttack:
                    break;
                default:
                    break;
            }
        }
    }

    private void AddBuff(RemainsBuffItem buffItem)
    {
        if(buffItem.Target == ETargetRole.Self)
        {
            BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(buffItem.BuffID), buffItem.Stacks);
        }
    }
}


public class RemainsBuffItem : BuffItem
{
    public ETriggerTime TriggerTime;
}

public enum ETriggerTime
{
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