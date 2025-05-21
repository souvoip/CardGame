using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItem_Battle : MapItemBase
{
    public override void Init(EMapItemType itemType, EMapState state, int layer)
    {
        base.Init(itemType, state, layer);
        Type = EMapItemType.Battle;
        mapItemImage.sprite = Resources.Load<Sprite>("Image/MapImg/M_Battle");
    }

    protected override void EnterEvent()
    {
        BattleManager.Instance.StartBattle(BattleDataManager.GetRandomBattle(1, EBattleType.Normal));
    }
}
