using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ItemDataManager
{
    public static List<RemainsItemData> RemainsItems = new List<RemainsItemData>();

    public static List<PotionItemData> PotionItems = new List<PotionItemData>();

    public static void Init()
    {
        var remainsItems = Resources.LoadAll<RemainsItemData>(ResourcesPaths.RemainsItemDataPath);
        RemainsItems = remainsItems.ToList();

        var potionItems = Resources.LoadAll<PotionItemData>(ResourcesPaths.PotionItemDataPath);
        PotionItems = potionItems.ToList();

    }

    public static ItemDataBase GetItem(int id)
    {
        foreach (var item in RemainsItems) { if (item.ID == id) { return GameObject.Instantiate(item); } }
        foreach (var item in PotionItems) { if (item.ID == id) { return GameObject.Instantiate(item); } }
        return null;
    }

    public static string GetItemName(int id)
    {
        foreach (var item in RemainsItems) { if (item.ID == id) { return item.Name; } }
        foreach (var item in PotionItems) { if (item.ID == id) { return item.Name; } }
        return null;
    }

    public static RemainsItemData GetRemainsItem(int id)
    {
        foreach (var item in RemainsItems)
        {
            if (item.ID == id) { return GameObject.Instantiate(item); }
        }
        return null;
    }

    private static List<int> shopRemainsItemIDs;

    public static RemainsItemData GetRandomShopRemainsItem()
    {
        if (shopRemainsItemIDs == null)
        {
            shopRemainsItemIDs = new List<int>();
            foreach (var item in RemainsItems)
            {
                if ((item.GetWay & EGetWay.Shop) == EGetWay.Shop) { shopRemainsItemIDs.Add(item.ID); }
            }
        }
        return GetRemainsItem(shopRemainsItemIDs[Random.Range(0, shopRemainsItemIDs.Count)]);
    }

    public static PotionItemData GetPotionItem(int id)
    {
        foreach (var item in PotionItems)
        {
            if (item.ID == id) { return GameObject.Instantiate(item); }
        }
        return null;
    }

    private static List<int> shopPotionItemIDs;
    public static PotionItemData GetRandomShopPotionItem()
    {
        if (shopPotionItemIDs == null)
        {
            shopPotionItemIDs = new List<int>();
            foreach (var item in PotionItems)
            {
                if ((item.GetWay & EGetWay.Shop) == EGetWay.Shop) { shopPotionItemIDs.Add(item.ID); }
            }
        }
        return GetPotionItem(shopPotionItemIDs[Random.Range(0, shopPotionItemIDs.Count)]);
    }
}
