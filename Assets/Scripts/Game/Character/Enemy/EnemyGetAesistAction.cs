using System;
using UnityEngine;

[Serializable]
public class EnemyGetAesistAction : EnemyDoAction
{
    [SerializeField, DisplayOnly]
    private string actionName = "获取抵抗";
    public override EEnemyActionType ActionType => EEnemyActionType.GetAesist;

    public Block baseBlock;

    private ActionInfo actionInfo;

    public override void DoAction()
    {
        // 获取抵抗 TODO: 需要计算相关Buff
        self.ChangeAttribute(ERoleAttribute.Aesist, baseBlock.GetBlockValue());
        BattleAnimManager.Instance.PlayAnim(self.transform.position, actionAnim);
    }

    public override ActionInfo GetActionInfo()
    {
        if(actionInfo != null) { return actionInfo; }
        actionInfo = new ActionInfo();
        actionInfo.icon = Resources.Load<Sprite>("Image/EnemyIntention/002");
        actionInfo.text = baseBlock.GetBlockValue().ToString();
        actionInfo.detailInfo = new DetailInfo();
        actionInfo.detailInfo.Title = "策略";
        actionInfo.detailInfo.Icon = actionInfo.icon;
        actionInfo.detailInfo.Description = $"将会获得{baseBlock.GetBlockValue()}点抵抗。";
        return actionInfo;
    }
}