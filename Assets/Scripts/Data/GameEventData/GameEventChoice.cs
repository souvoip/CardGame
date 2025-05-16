using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class GameEventChoice
{
    public string ChoiceText;

    [SerializeReference]
    public List<GameEventTriggerEffect> TriggerEffects = new List<GameEventTriggerEffect>();

    public List<GameEventChoiceNextNode> NextNodes = new List<GameEventChoiceNextNode>();

    public void SelectThisChoice()
    {
        for (int i = 0; i < TriggerEffects.Count; i++)
        {
            // TODO: 触发效果
            TriggerEffects[i].TriggerEffect();
        }
    }
}

[Serializable]
public class GameEventChoiceNextNode
{
    public int RandomRatio = 1;
    public int NextNodeIndex;
    //public GameEventNode NextNode;
}