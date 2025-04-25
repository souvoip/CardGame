using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敏捷
/// </summary>
[CreateAssetMenu(fileName = "AgileBuff", menuName = "Data/Buff/AgileBuff")]
public class Buff_Agile : BuffBase
{
    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
    }

    public override void AddEvents()
    {
        base.AddEvents();
        target.ChangeBlockEvent.Add(buffID, ChangeBlock);
    }

    public override void RemoveEvents()
    {
        base.RemoveEvents();
        target.ChangeBlockEvent.Remove(buffID);
    }

    private void ChangeBlock(Block block)
    {
        block.Value += currentStacks;
    }
}
