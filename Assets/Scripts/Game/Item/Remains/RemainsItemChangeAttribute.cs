using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Remains_ChangeAttribute", menuName = "Data/Remains/ChangeAttributeRemains")]
public class RemainsItemChangeAttribute : RemainsItem
{
    public ETriggerTime TriggerTime;

    public ERoleAttribute Attribute;

    public ETargetRole Target;

    public int ChangeValue;

    public override void OnAcquire()
    {
        switch (TriggerTime)
        {
            case ETriggerTime.StartBattle:
                TurnManager.OnStartBattle += OnChange;
                break;
            case ETriggerTime.PlayerTurnStart:
                TurnManager.OnPlayerTurnStart += OnChange;
                break;
            case ETriggerTime.PlayerTurnEnd:
                TurnManager.OnPlayerTurnEnd += OnChange;
                break;
            case ETriggerTime.EnemyTurnStart:
                TurnManager.OnEnemyTurnStart += OnChange;
                break;
            case ETriggerTime.EnemyTurnEnd:
                TurnManager.OnEnemyTurnEnd += OnChange;
                break;
            case ETriggerTime.EndBattle:
                TurnManager.OnBattleVictory += OnChange;
                break;
            case ETriggerTime.PlayerDead:
                BattleManager.Instance.Player.DieEvent += OnChange;
                break;
            case ETriggerTime.EnemyDead:
                break;
            case ETriggerTime.PlayerHit:
                BattleManager.Instance.Player.GetHitEvent += OnChange;
                break;
            case ETriggerTime.PlayerAttack:
                BattleManager.Instance.Player.AttackEvent += OnChange;
                break;
            default:
                break;
        }
    }

    public override void OnBattleStart()
    {
        switch (TriggerTime)
        {
            case ETriggerTime.EnemyDead:
                BattleManager.Instance.EnemyRoles.ForEach(enemy => enemy.DieEvent += OnChange);
                break;
            default:
                break;
        }
    }

    private void OnChange()
    {
        if (Target == ETargetRole.Self)
        {
            BattleManager.Instance.Player.ChangeAttribute(Attribute, ChangeValue);
        }
        else if (Target == ETargetRole.AllEnemy)
        {
            foreach (var enemy in BattleManager.Instance.EnemyRoles)
            {
                enemy.ChangeAttribute(Attribute, ChangeValue);
            }
        }
        else if (Target == ETargetRole.All)
        {
            BattleManager.Instance.Player.ChangeAttribute(Attribute, ChangeValue);
            foreach (var enemy in BattleManager.Instance.EnemyRoles)
            {
                enemy.ChangeAttribute(Attribute, ChangeValue);
            }
        }
    }

    private void OnChange(CharacterBase character, int value)
    {
        if (Target == ETargetRole.Self)
        {
            BattleManager.Instance.Player.ChangeAttribute(Attribute, ChangeValue);
        }
        else if (Target == ETargetRole.Enemy)
        {
            character.ChangeAttribute(Attribute, ChangeValue);
        }
        else if (Target == ETargetRole.AllEnemy)
        {
            foreach (var enemy in BattleManager.Instance.EnemyRoles)
            {
                enemy.ChangeAttribute(Attribute, ChangeValue);
            }
        }
        else if (Target == ETargetRole.All)
        {
            BattleManager.Instance.Player.ChangeAttribute(Attribute, ChangeValue);
            foreach (var enemy in BattleManager.Instance.EnemyRoles)
            {
                enemy.ChangeAttribute(Attribute, ChangeValue);
            }
        }
    }
}
