using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Data/GameEventData/GameEvent")]
public class GameEventData : ScriptableObject
{
    public int ID;

    public string EventName;

    public GameEventNode StartNode;

    public List<GameEventNode> AllNodes = new List<GameEventNode>();
}

