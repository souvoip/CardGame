using System.Collections.Generic;
using System;

[Serializable]
public class EnemyGiveCardAction : EnemyDoAction
{
    public override EEnemyActionType ActionType => EEnemyActionType.GiveCard;
    public List<int> cardIDs;
    public override void DoAction()
    {

    }
}