using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameEventTriggerEffectGetItem : GameEventTriggerEffect
{
    public override EEventEffectType EffectType => EEventEffectType.GetItem;

    /// <summary>
    /// 获取的物品ID，-1表示随机
    /// </summary>
    public int itemID;

    /// <summary>
    /// 如果为空，则使用默认随机物品库
    /// </summary>
    public List<int> randomItemIDs;

    public int randomCount = 1;

    public override void TriggerEffect()
    {
        if (itemID == -1)
        {
            // TODO：随机获取物品，需要保证是玩家未拥有的物品
            if(randomItemIDs == null || randomItemIDs.Count == 0)
            {
                // 使用默认随机物品库
            }
            else
            {
                BattleManager.Instance.Player.AddRandomItem(randomItemIDs, randomCount);
            }
        }
        else
        {
            BattleManager.Instance.Player.AddItem(itemID);
        }
    }
}
