using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardDataManager
{
    private static string cardDataPath = "Data/CardData";

    public static List<CardBase> Cards = new List<CardBase>();

    /// <summary>
    /// 临时测试数据，可以在战斗中获取的卡牌ID TODO：完善
    /// </summary>
    public readonly static int[] getCardIds = new int[] {
        2, 3, 4, 102, 103, 201, 202
    };

    /// <summary>
    /// 获取随机卡牌ID, TODO：临时测试
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public static int[] GetRandomCardIds(int count)
    {
        if (count > getCardIds.Length)
        {
            return getCardIds;
        }

        int[] ids = new int[count];
        int n = getCardIds.Length;
        int[] indices = new int[n];

        // 初始化索引数组
        for (int i = 0; i < n; i++)
        {
            indices[i] = i;
        }
        System.Random rnd = new System.Random();
        for (int i = 0; i < count; i++)
        {
            // 生成随机位置（范围逐渐缩小）
            int j = rnd.Next(i, n);

            // 交换当前索引和随机位置的索引
            int temp = indices[i];
            indices[i] = indices[j];
            indices[j] = temp;

            // 获取对应的卡片ID
            ids[i] = getCardIds[indices[i]];
        }

        return ids;
    }

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