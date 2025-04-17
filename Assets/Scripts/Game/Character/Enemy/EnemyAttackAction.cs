using System;

[Serializable]
public class EnemyAttackAction : EnemyDoAction
{
    public override EEnemyActionType ActionType => EEnemyActionType.Attack;

    public Damage BaseDamage;

    public int AtkCount;

    public override void DoAction()
    {
        // 攻击玩家
        int damageValue = GameTools.CalculateDamage(self, BattleManager.Instance.Player, BaseDamage);
        for (int i = 0; i < AtkCount; i++)
        {
            BattleManager.Instance.Player.ChangeAttribute(ERoleAttribute.HP, -damageValue);
        }
    }
}
