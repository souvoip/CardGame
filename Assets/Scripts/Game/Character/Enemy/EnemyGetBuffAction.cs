using System.Collections.Generic;
using System;

[Serializable]
public class EnemyGetBuffAction : EnemyDoAction
{
    public override EEnemyActionType ActionType => EEnemyActionType.GetBuff;
    public List<BuffItem> buffs;

    public override void DoAction()
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            self.AddBuff(BuffDataManager.GetBuff(buffs[i].BuffID), buffs[i].Stacks);
        }
    }
}