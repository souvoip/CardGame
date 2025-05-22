using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItem_Treasures : MapItemBase
{
    public override void Init(EMapItemType itemType, EMapState state, int layer)
    {
        base.Init(itemType, state, layer);
        Type = EMapItemType.Treasures;
        mapItemImage.sprite = Resources.Load<Sprite>("Image/MapImg/M_Treasures");
    }

    protected override void EnterEvent()
    {
        // 生成奖励
        List<PrizeItemData> prizeList = new List<PrizeItemData>()
        {
            new PrizeItemData(EPrizeType.Gold, Random.Range(40, 80)),
            new PrizeItemData(EPrizeType.Item, 2),
        };

        UIManager.Instance.prizeUI.Show(prizeList);
    }
}
