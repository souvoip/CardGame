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
}

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

public class GameEventTriggerEffect
{
    public virtual EEventEffectType EffectType{ get; }

    public virtual void TriggerEffect() { }
}

public class GameEventTriggerEffectChangeAttribute : GameEventTriggerEffect
{
    public override EEventEffectType EffectType => EEventEffectType.ChangeAttribute;

    public ERoleAttribute Attribute;

    public int ChangeValue;

    public override void TriggerEffect()
    {
        BattleManager.Instance.Player.ChangeAttribute(Attribute, ChangeValue);
    }
}

public class GameEventTriggerEffectChangeCard : GameEventTriggerEffect
{
    public override EEventEffectType EffectType => EEventEffectType.ChangeCard;
}

public class GameEventTriggerEffectBattle : GameEventTriggerEffect
{
    public override EEventEffectType EffectType => EEventEffectType.Battle;

    public override void TriggerEffect()
    {
        BattleManager.Instance.StartBattleTest();
        // TODO：隐藏事件界面，在战斗结束后显示事件界面
    }
}

public class GameEventTriggerEffectGetItem : GameEventTriggerEffect
{
    public override EEventEffectType EffectType => EEventEffectType.GetItem;

    /// <summary>
    /// 获取的物品ID，-1表示随机
    /// </summary>
    public int itemID;

    public override void TriggerEffect()
    {

    }
}

public enum EEventEffectType
{
    ChangeAttribute,
    ChangeCard,
    Battle,
    GetItem,
}