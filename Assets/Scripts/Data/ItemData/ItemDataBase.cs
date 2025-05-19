using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemDataBase : ScriptableObject
{
    public int ID;

    public string Name;

    public string Description;

    public string IconPath;
    /// <summary>
    /// 物品购买价格
    /// </summary>
    public int Price;
    /// <summary>
    /// 物品是否独一无二
    /// </summary>
    public bool IsUnique = true;

    /// <summary>
    /// 获取途径
    /// </summary>
    public EGetWay GetWay;

    public virtual EItemType ItemType { get; }
}


public enum EItemType
{
    Remains,
    Potion,
}