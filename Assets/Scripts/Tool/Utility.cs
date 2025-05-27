using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    /// <summary>
    /// 通过 Scale 调整图片大小，确保不超出原始显示区域
    /// </summary>
    /// <param name="image">目标 RawImage</param>
    public static void AutoAdjustImageSize(this RawImage image)
    {
        if (image.texture == null) return;
        float scaleX = 1, scaleY = 1;
        if(image.texture.width > image.texture.height)
        {
            scaleY = (float)image.texture.height / image.texture.width;
        }
        else
        {
            scaleX = (float)image.texture.width / image.texture.height;
        }
        image.rectTransform.localScale = new Vector3(scaleX, scaleY, 1);
    }

    public static string V3ToString(this Vector3 v3)
    {
        return v3.x + "," + v3.y + "," + v3.z;
    }

    public static Vector3 V3FromString(this string str)
    {
        string[] strs = str.Split(',');
        return new Vector3(float.Parse(strs[0]), float.Parse(strs[1]), float.Parse(strs[2]));
    }
}
