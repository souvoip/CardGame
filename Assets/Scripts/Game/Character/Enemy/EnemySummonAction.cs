using System;

[Serializable]
public class EnemySummonAction : EnemyDoAction
{
    public override EEnemyActionType ActionType => EEnemyActionType.Summon;
    //public List<EnemyRoleData> enemies;
    public override void DoAction()
    {
    }
}