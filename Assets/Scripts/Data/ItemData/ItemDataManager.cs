using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ItemDataManager
{
    private static string remainsItemDataPath = "Data/Item/Remains";

    public static List<RemainsItem> RemainsItems = new List<RemainsItem>();

    public static void Init()
    {
        var remainsItems = Resources.LoadAll<RemainsItem>(remainsItemDataPath);
        RemainsItems = remainsItems.ToList();
    }

    public static RemainsItem GetRemainsItem(int id)
    {
        foreach (var item in RemainsItems)
        {
            if (item.ID == id) { return GameObject.Instantiate(item); }
        }
        return null;
    }
}
