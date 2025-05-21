using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItem_Shop : MapItemBase
{
    public override void Init(EMapItemType itemType, EMapState state, int layer)
    {
        base.Init(itemType, state, layer);
        Type = EMapItemType.Shop;
        mapItemImage.sprite = Resources.Load<Sprite>("Image/MapImg/M_Shop");
    }

    protected override void EnterEvent()
    {
        UIManager.Instance.shopUI.Show();
    }
}
