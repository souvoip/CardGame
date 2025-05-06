using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class EnemyGiveCardAction : EnemyDoAction
{
    [SerializeField, DisplayOnly]
    private string actionName = "给予玩家卡牌";
    public override EEnemyActionType ActionType => EEnemyActionType.Mix;
    public List<int> cardIDs;
    private ActionInfo actionInfo;
    public override void DoAction()
    {
        BattleAnimManager.Instance.PlayAnim(BattleManager.Instance.Player.transform.position, actionAnim);
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
        actionInfo.detailInfo.Description = "这名敌人将施加负面状态";
        return actionInfo;
    }
}