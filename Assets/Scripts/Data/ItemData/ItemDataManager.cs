using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ItemDataManager
{
    private static string remainsItemDataPath = "Data/Item/Remains";

    private static string potionItemDataPath = "Data/Item/Potion";

    public static List<RemainsItemData> RemainsItems = new List<RemainsItemData>();

    public static List<PotionItemData> PotionItems = new List<PotionItemData>();

    public static void Init()
    {
        var remainsItems = Resources.LoadAll<RemainsItemData>(remainsItemDataPath);
        RemainsItems = remainsItems.ToList();

        var potionItems = Resources.LoadAll<PotionItemData>(potionItemDataPath);
        PotionItems = potionItems.ToList();

    }

    public static ItemDataBase GetItem(int id)
    {
        foreach (var item in RemainsItems) { if (item.ID == id) { return item; } }
        foreach (var item in PotionItems) { if (item.ID == id) { return item; } }
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

    public static PotionItemData GetPotionItem(int id)
    {
        foreach (var item in PotionItems)
        {
            if (item.ID == id) { return GameObject.Instantiate(item); }
        }
        return null;
    }
}
