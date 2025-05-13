using System.Collections.Generic;
using System;

[Serializable]
public class GameEventChoice
{
    public string ChoiceText;

    public List<GameEventTriggerEffect> TriggerEffects = new List<GameEventTriggerEffect>();

    public List<GameEventChoiceNextNode> NextNodes = new List<GameEventChoiceNextNode>();

    public void SelectThisChoice()
    {
        for (int i = 0; i < TriggerEffects.Count; i++)
        {
            // TODO: 触发效果
            TriggerEffects[i].TriggerEffect();
        }
        // 显示下一个节点，或者结束
        if (NextNodes.Count > 0)
        {
            // TODO: 显示下一个节点
            if (NextNodes.Count == 1)
            {
                // TODO: 直接显示下一个节点
            }
            else
            {
                // TODO: 显示随机节点
                int allValue = 0;
                for (int i = 0; i < NextNodes.Count; i++)
                {
                    allValue += NextNodes[i].RandomRatio;
                }
                int random = UnityEngine.Random.Range(0, allValue);
                int tempValue = 0;
                for (int i = 0; i < NextNodes.Count; i++)
                {
                    if (random < NextNodes[i].RandomRatio + tempValue)
                    {
                        // TODO: 显示下一个节点
                    }
                    tempValue += NextNodes[i].RandomRatio;
                }
            }
        }
        else
        {
            // TODO: 结束事件
        }
    }
}


public class GameEventChoiceNextNode
{
    public int RandomRatio = 1;
    public GameEventNode NextNode;
}