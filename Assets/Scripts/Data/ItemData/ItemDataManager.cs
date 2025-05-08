using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ItemDataManager
{
    private static string remainsItemDataPath = "Data/Item/Remains";

    public static List<RemainsItemData> RemainsItems = new List<RemainsItemData>();

    public static void Init()
    {
        var remainsItems = Resources.LoadAll<RemainsItemData>(remainsItemDataPath);
        RemainsItems = remainsItems.ToList();
    }

    public static RemainsItemData GetRemainsItem(int id)
    {
        foreach (var item in RemainsItems)
        {
            if (item.ID == id) { return GameObject.Instantiate(item); }
        }
        return null;
    }
}
