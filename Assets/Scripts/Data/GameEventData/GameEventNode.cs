using System.Collections.Generic;
using System;
using System.Numerics;

[Serializable]
public class GameEventNode
{
    public string StoryText;

    public string ImgPath;

    /// <summary>
    /// 进入节点时触发
    /// </summary>
    public List<GameEventTriggerEffect> EnterEffects = new List<GameEventTriggerEffect>();

    public List<GameEventChoice> Choices = new List<GameEventChoice>();

    public void EnterThisNode()
    {
        for (int i = 0; i < EnterEffects.Count; i++)
        {
            // TODO: 触发效果
            EnterEffects[i].TriggerEffect();
        }
    }

#if UNITY_EDITOR
    public Vector2 EditorPos;
#endif
}