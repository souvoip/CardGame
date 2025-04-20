using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItem_Event : MapItemBase
{

    public override void Init(EMapItemType itemType, EMapState state, int layer)
    {
        base.Init(itemType, state, layer);
        Type = EMapItemType.Event;
        mapItemImage.sprite = Resources.Load<Sprite>("Image/MapImg/M_Event");
    }
}
