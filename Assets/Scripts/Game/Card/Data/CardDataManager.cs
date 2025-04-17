using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardDataManager
{
    private static string cardDataPath = "Data/CardData";

    public static List<CardBase> Cards = new List<CardBase>();

    public static void Init()
    {
        var cds = Resources.LoadAll<CardBase>(cardDataPath);
        for (int i = 0; i < cds.Length; i++)
        {
            Cards.Add(cds[i]);
        }
    }

    public static CardBase GetCard(int id)
    {
        foreach (var item in Cards)
        {
            if (item.ID == id) { return GameObject.Instantiate(item); }
        }
        return null;
    }
}