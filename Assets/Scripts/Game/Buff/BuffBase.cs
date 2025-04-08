using System;
using UnityEngine;

public abstract class BuffBase : ScriptableObject
{
    [Header("基础配置")]
    public string buffID;          // 唯一标识
    public string displayName;     // 显示名称
    public string iconPath;        // 图标
    public Color tintColor = Color.white; // 图标颜色

    [Header("层数设置")]
    public int minStacks = 0;      // 最小层数
    public int maxStacks = 99;     // 最大层数
    public bool isDebuff;          // 是否为负面效果

    [Header("持续时间")]
    public int duration = -1;      // -1表示永久

    // 运行时数据
    [HideInInspector]
    public int currentStacks;
    [HideInInspector]
    public int remainingTurns;

    protected CharacterBase target;

    public virtual void Initialize(CharacterBase owner, int initStacks = 1)
    {
        target = owner;
        currentStacks = initStacks;
        remainingTurns = duration;

        // 自动注册回合事件
        if (duration != 0)
        {

            AddEvents();
        }
    }

    // 每回合结束时调用(敌人回合结束)
    public virtual void OnTurnEnd()
    {
        if (remainingTurns > 0) remainingTurns--;
        if (remainingTurns == 0) RemoveBuff();
    }

    // 增加层数
    public virtual void AddStacks(int amount)
    {
        currentStacks = Mathf.Clamp(currentStacks + amount, minStacks, maxStacks);
        if (currentStacks == 0) { RemoveBuff(); }
    }

    // 移除Buff
    public virtual void RemoveBuff()
    {
        RemoveEvents();
    }

    // 获取描述文本（用于UI显示）
    public virtual string GetDescription()
    {
        return $"{displayName} ({currentStacks})";
    }

    public virtual void AddEvents()
    {
        TurnManager.OnEnemyTurnEnd += OnTurnEnd;
    }

    public virtual void RemoveEvents()
    {
        TurnManager.OnEnemyTurnEnd -= OnTurnEnd;
    }
}