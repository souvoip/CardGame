using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItem_HardBattle : MapItemBase
{
    public override void Init(EMapItemType itemType, EMapState state, int layer)
    {
        base.Init(itemType, state, layer);
        Type = EMapItemType.HardBattle;
        mapItemImage.sprite = Resources.Load<Sprite>("Image/MapImg/M_HardBattle");
    }
}
