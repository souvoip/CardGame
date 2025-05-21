using System;

[Serializable]
public class GameEventTriggerEffectJumpOther : GameEventTriggerEffect
{
    public override EEventEffectType EffectType => EEventEffectType.JumpOtherRoom;

    public int BattleId;

    public override void TriggerEffect()
    {
        BattleManager.Instance.StartBattle(BattleId);
        // TODO：隐藏事件界面，在战斗结束后显示事件界面
    }
}
