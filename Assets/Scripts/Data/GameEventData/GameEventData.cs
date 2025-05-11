using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventData : ScriptableObject
{
    public int ID;

    public string EventName;


}

[Serializable]
public class GemeEventSelectItem
{
    public string Text;

    public List<string> Select;
}