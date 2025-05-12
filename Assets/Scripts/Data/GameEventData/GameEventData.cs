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
}

[Serializable]
public class GameEventNode
{
    public string StoryText;

    public string ImgPath;

    public List<GameEventChoice> Choices = new List<GameEventChoice>();
}

[Serializable]
public class GameEventChoice
{
    public string ChoiceText;

    /// <summary>
    /// 如果为空，则表示该选项是结束事件
    /// </summary>
    public GameEventNode NextNode;
}