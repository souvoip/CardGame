public class Buff_Poison : BuffBase
{
    public override void OnTurnEnd()
    {
        // 造成伤害
        //target.TakeDamage(currentStacks);

        // 减少1层
        AddStacks(-1);

        base.OnTurnEnd(); // 处理持续时间
    }
}