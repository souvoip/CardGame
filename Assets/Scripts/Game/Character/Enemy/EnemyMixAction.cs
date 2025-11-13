using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMixAction : EnemyDoAction
{
    [SerializeField, DisplayOnly]
    private string actionName = "Mix";
    public override EEnemyActionType ActionType => EEnemyActionType.Mix;

    public override void DoAction()
    {
        base.DoAction();
    }
}
