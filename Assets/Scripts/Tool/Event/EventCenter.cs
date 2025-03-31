using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#region EventInfo T1,T2,...
public class EventInfo {
    /// <summary>
    /// 事件委托
    /// </summary>
    public UnityAction actions;

    /// <summary>
    /// 构造函数
    /// </summary>
    public EventInfo(UnityAction action) {
        actions += action;
    }
}
public class EventInfo<T> {
    /// <summary>
    /// 事件委托
    /// </summary>
    public UnityAction<T> actions;

    /// <summary>
    /// 构造函数
    /// </summary>
    public EventInfo(UnityAction<T> action) {
        actions += action;
    }
}
public class EventInfo<T1, T2> {
    /// <summary>
    /// 事件委托
    /// </summary>
    public UnityAction<T1, T2> actions;

    /// <summary>
    /// 构造函数
    /// </summary>
    public EventInfo(UnityAction<T1, T2> action) {
        actions += action;
    }
}

public class EventInfo<T1, T2, T3>
{
    /// <summary>
    /// 事件委托
    /// </summary>
    public UnityAction<T1, T2, T3> actions;

    /// <summary>
    /// 构造函数
    /// </summary>
    public EventInfo(UnityAction<T1, T2, T3> action)
    {
        actions += action;
    }
}
#endregion


#region EventCenter T1,T2,...
/// <summary>
/// 事件分发中心
/// </summary>
public class EventCenter : BaseManager<EventCenter> {
    private Dictionary<string, EventInfo> eventDic = new Dictionary<string, EventInfo>();

    public void AddEventListener(string name, UnityAction action) {
        if (eventDic.ContainsKey(name)) {
            eventDic[name].actions += action;
        } else {
            eventDic.Add(name, new EventInfo(action));
        }
    }

    public void RemoveEventListener(string name, UnityAction action) {
        if (eventDic.Count == 0) return;
        if (eventDic.ContainsKey(name)) { eventDic[name].actions -= action; }
    }

    public void EventTrigger(string name) {
        if (eventDic.ContainsKey(name)) {
            eventDic[name].actions?.Invoke();
        }
    }

    public void Clear() {
        eventDic.Clear();
    }
}



/// <summary>
/// 事件分发中心
/// </summary>
public class EventCenter<T> : BaseManager<EventCenter<T>> {
    /// <summary>
    /// 事件字典
    /// </summary>
    private Dictionary<string, EventInfo<T>> eventDic = new Dictionary<string, EventInfo<T>>();

    /// <summary>
    /// 添加监听
    /// </summary>
    public void AddEventListener(string name, UnityAction<T> action) {
        if (eventDic.ContainsKey(name)) {
            eventDic[name].actions += action;
        } else {
            eventDic.Add(name, new EventInfo<T>(action));
        }
    }

    /// <summary>
    /// 移除监听
    /// </summary>
    public void RemoveEventListener(string name, UnityAction<T> action) {
        if (eventDic.Count == 0) return;
        if (eventDic.ContainsKey(name)) { eventDic[name].actions -= action; }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    public void EventTrigger(string name, T info) {
        if (eventDic.ContainsKey(name)) {
            eventDic[name].actions?.Invoke(info);
        }
    }

    /// <summary>
    /// 清除事件
    /// </summary>
    public void Clear() {
        eventDic.Clear();
    }
}



/// <summary>
/// 事件分发中心
/// </summary>
public class EventCenter<T1, T2> : BaseManager<EventCenter<T1, T2>> {
    /// <summary>
    /// 事件字典
    /// </summary>
    private Dictionary<string, EventInfo<T1>> eventDicT1 = new Dictionary<string, EventInfo<T1>>();
    private Dictionary<string, EventInfo<T1, T2>> eventDicT1T2 = new Dictionary<string, EventInfo<T1, T2>>();

    /// <summary>
    /// 添加监听
    /// </summary>
    public void AddEventListener(string name, UnityAction<T1> action) {
        if (eventDicT1.ContainsKey(name)) {
            eventDicT1[name].actions += action;
        } else {
            eventDicT1.Add(name, new EventInfo<T1>(action));
        }
    }
    public void AddEventListener(string name, UnityAction<T1,T2> action) {
        if (eventDicT1T2.ContainsKey(name)) {
            eventDicT1T2[name].actions += action;
        } else {
            eventDicT1T2.Add(name, new EventInfo<T1, T2>(action));
        }
    }

    /// <summary>
    /// 移除监听
    /// </summary>
    public void RemoveEventListener(string name, UnityAction<T1> action) {
        if (eventDicT1.Count == 0) return;
        if (eventDicT1.ContainsKey(name)) { eventDicT1[name].actions -= action; }
    }
    public void RemoveEventListener(string name, UnityAction<T1,T2> action) {
        if (eventDicT1T2.Count == 0) return;
        if (eventDicT1T2.ContainsKey(name)) { eventDicT1T2[name].actions -= action; }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    public void EventTrigger(string name, T1 info) {
        if (eventDicT1.ContainsKey(name)) {
            eventDicT1[name].actions?.Invoke(info);
        }
    }
    public void EventTrigger(string name, T1 t1,T2 t2) {
        if (eventDicT1T2.ContainsKey(name)) {
            eventDicT1T2[name].actions?.Invoke(t1,t2);
        }
    }

    /// <summary>
    /// 清除事件
    /// </summary>
    public void Clear() {
        eventDicT1.Clear();
        eventDicT1T2.Clear();
    }
}

/// <summary>
/// 事件分发中心
/// </summary>
public class EventCenter<T1, T2, T3> : BaseManager<EventCenter<T1, T2, T3>>
{
    /// <summary>
    /// 事件字典
    /// </summary>
    private Dictionary<string, EventInfo<T1, T2, T3>> eventDic = new Dictionary<string, EventInfo<T1, T2, T3>>();

    /// <summary>
    /// 添加监听
    /// </summary>
    public void AddEventListener(string name, UnityAction<T1, T2, T3> action)
    {
        if (eventDic.ContainsKey(name))
        {
            eventDic[name].actions += action;
        }
        else
        {
            eventDic.Add(name, new EventInfo<T1, T2, T3>(action));
        }
    }

    /// <summary>
    /// 移除监听
    /// </summary>
    public void RemoveEventListener(string name, UnityAction<T1, T2, T3> action)
    {
        if (eventDic.Count == 0) return;
        if (eventDic.ContainsKey(name)) { eventDic[name].actions -= action; }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    public void EventTrigger(string name, T1 info1, T2 info2, T3 info3)
    {
        if (eventDic.ContainsKey(name))
        {
            eventDic[name].actions?.Invoke(info1, info2, info3);
        }
    }

    /// <summary>
    /// 清除事件
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
#endregion