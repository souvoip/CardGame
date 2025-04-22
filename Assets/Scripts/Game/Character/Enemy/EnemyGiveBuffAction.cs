using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class EnemyGiveBuffAction : EnemyDoAction
{
    public override EEnemyActionType ActionType => EEnemyActionType.GiveBuff;
    public List<BuffItem> buffs;
    private ActionInfo actionInfo;
    public override void DoAction()
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(buffs[i].BuffID), buffs[i].Stacks);
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
        actionInfo.detailInfo.Description = "这名敌人将施加负面效果";
        return actionInfo;
    }
}
