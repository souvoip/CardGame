using System;

[Serializable]
public class EnemyGetAesistAction : EnemyDoAction
{
    public override EEnemyActionType ActionType => EEnemyActionType.GetAesist;

    public Block baseBlock;

    public override void DoAction()
    {
        // 获取抵抗 TODO: 需要计算相关Buff
        self.ChangeAttribute(ERoleAttribute.Aesist, baseBlock.GetBlockValue());
    }
}