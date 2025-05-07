
using UnityEngine;
/// <summary>
/// 抵消debuff
/// </summary>
[CreateAssetMenu(fileName = "ArtifactBuff", menuName = "Data/Buff/ArtifactBuff")]
public class Buff_Artifact : BuffBase
{
    public override void AddEvents()
    {
        base.AddEvents();
        target.AddBuffEvent += TryBlockDebuff;
    }

    public override void RemoveEvents()
    {
        base.RemoveEvents();
        target.AddBuffEvent -= TryBlockDebuff;
    }


    // 当尝试施加减益时触发
    public void TryBlockDebuff(BuffBase buff)
    {
        if (buff.isDebuff)
        {
            // 抵消debuff
            buff.buffID = -1;
            target.AddBuff(this, -1);
        }
    }
}