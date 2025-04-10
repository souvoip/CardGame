using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    /// <summary>
    /// 随机打乱数组
    /// </summary>
    /// <param name="array"></param>
    public static void ShuffleArray<T>(this T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            T temp = array[i];
            int randomIndex = Random.Range(i, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    /// <summary>
    /// 随机打乱列表元素
    /// </summary>
    /// <param name="array"></param>
    public static void ShuffleList<T>(this List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public static string GetCardTypeeString(this CardBase card)
    {
        switch (card.CardType)
        {
            case ECardType.Atk:
                return "攻击";
            case ECardType.Skill:
                return "技能";
            case ECardType.Ability:
                return "能力";
            case ECardType.State:
                return "状态";
            default:
                return "其它";
        }
    }
}
