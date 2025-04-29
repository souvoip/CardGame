using System;
using UnityEngine;

[Serializable]
public class EnemyAttackAction : EnemyDoAction
{
    [SerializeField, DisplayOnly]
    private string actionName= "攻击";

    public override EEnemyActionType ActionType => EEnemyActionType.Attack;

    public Damage BaseDamage;

    public int AtkCount;

    private ActionInfo actionInfo;

    public override void DoAction()
    {
        // 攻击玩家
        int damageValue = GameTools.CalculateDamage(self, BattleManager.Instance.Player, BaseDamage);
        for (int i = 0; i < AtkCount; i++)
        {
            self.AtkTarget(BattleManager.Instance.Player, damageValue);
            //BattleManager.Instance.Player.ChangeAttribute(ERoleAttribute.HP, -damageValue);
            BattleAnimManager.Instance.PlayAnim(BattleManager.Instance.Player.transform.position, actionAnim);
        }
    }

    public override ActionInfo GetActionInfo()
    {
        if (actionInfo != null)
        {
            UpdateActionInfo();
            return actionInfo;
        }
        actionInfo = new ActionInfo();
        actionInfo.icon = Resources.Load<Sprite>("Image/EnemyIntention/001");
        int dv = GameTools.CalculateDamage(self, BattleManager.Instance.Player, BaseDamage);
        actionInfo.text = dv.ToString();
        actionInfo.detailInfo = new DetailInfo();
        actionInfo.detailInfo.Title = "策略";
        actionInfo.detailInfo.Icon = actionInfo.icon;
        actionInfo.detailInfo.Description = $"将会对玩家造成{dv}点伤害。";
        return actionInfo;
    }

    private void UpdateActionInfo()
    {
        int dv = GameTools.CalculateDamage(self, BattleManager.Instance.Player, BaseDamage);

        actionInfo.text = dv.ToString();
        actionInfo.detailInfo.Description = $"将会对玩家造成{dv}点伤害。";
    }

}

public class DisplayOnlyAttribute : PropertyAttribute
{
}