using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameTools
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
}
