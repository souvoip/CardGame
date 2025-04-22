using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class EnemyGetBuffAction : EnemyDoAction
{
    public override EEnemyActionType ActionType => EEnemyActionType.GetBuff;
    public List<BuffItem> buffs;

    private ActionInfo actionInfo;

    public override void DoAction()
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            self.AddBuff(BuffDataManager.GetBuff(buffs[i].BuffID), buffs[i].Stacks);
        }
    }

    public override ActionInfo GetActionInfo()
    {
        if (actionInfo != null) { return actionInfo; }
        actionInfo = new ActionInfo();
        actionInfo.icon = Resources.Load<Sprite>("Image/EnemyIntention/003");
        actionInfo.text = "";
        actionInfo.detailInfo = new DetailInfo();
        actionInfo.detailInfo.Title = "策略";
        actionInfo.detailInfo.Icon = actionInfo.icon;
        actionInfo.detailInfo.Description = "这名敌人将强化自己";
        return actionInfo;
    }
}