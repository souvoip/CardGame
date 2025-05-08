using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemDataBase : ScriptableObject
{
    public int ID;

    public string Name;

    public string Description;

    public string IconPath;

    public virtual EItemType ItemType { get; }
}


public enum EItemType
{
    Remains,
    Potion,
}