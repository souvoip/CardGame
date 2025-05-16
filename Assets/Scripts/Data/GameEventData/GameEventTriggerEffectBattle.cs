using System;

[Serializable]
public class GameEventTriggerEffectBattle : GameEventTriggerEffect
{
    public override EEventEffectType EffectType => EEventEffectType.JumpOtherRoom;

    public override void TriggerEffect()
    {
        BattleManager.Instance.StartBattleTest();
        // TODO：隐藏事件界面，在战斗结束后显示事件界面
    }
}
