using System.Collections.Generic;
using System;

[Serializable]
public class EnemyGiveBuffAction : EnemyDoAction
{
    public override EEnemyActionType ActionType => EEnemyActionType.GiveBuff;
    public List<BuffItem> buffs;
    public override void DoAction()
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(buffs[i].BuffID), buffs[i].Stacks);
        }
    }
}
