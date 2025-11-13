using System;
using UnityEngine;

[Serializable]
public class EnemySummonAction : EnemyDoAction
{
    [SerializeField, DisplayOnly]
    private string actionName = "召唤其它敌人，战斗场地需要有空位才会执行";
    public override EEnemyActionType ActionType => EEnemyActionType.Summon;
    //public List<EnemyRoleData> enemies;
    public override void DoAction()
    {
        base.DoAction();
    }
}