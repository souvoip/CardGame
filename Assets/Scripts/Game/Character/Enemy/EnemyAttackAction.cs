using System;

[Serializable]
public class EnemyAttackAction : EnemyDoAction
{
    public override EEnemyActionType ActionType => EEnemyActionType.Attack;

    public Damage baseDamage;

    public override void DoAction()
    {
        // ¹¥»÷Íæ¼Ò
        Damage tempDamage = new Damage(baseDamage);
        self.CalculateAtkDamage(tempDamage);
        BattleManager.Instance.Player.CalculateHitDamage(tempDamage);
        BattleManager.Instance.Player.ChangeAttribute(ERoleAttribute.HP, -tempDamage.GetDamage());
    }
}
