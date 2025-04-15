using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterBase : MonoBehaviour
{
    #region Events
    /// <summary>
    /// 角色死亡事件
    /// </summary>
    public event Action DieEvent;

    public Dictionary<int, Action<Damage>> ChangeAtkDamageEvent = new Dictionary<int, Action<Damage>>();

    public Dictionary<int, Action<Damage>> ChangeHitDamageEvent = new Dictionary<int, Action<Damage>>();
    /// <summary>
    /// 角色受伤事件, 造成伤害的来源,伤害值
    /// </summary>
    public event Action<CharacterBase, int> GetHitEvent;
    /// <summary>
    /// 角色攻击事件, 攻击的目标,伤害值
    /// </summary>
    public event Action<CharacterBase, int> AttackEvent;
    #endregion

    protected BuffControl buffControl;

    protected virtual void Init() { }

    private void Awake()
    {
        buffControl = GetComponent<BuffControl>();
        Init();
    }

    public void AddBuff(BuffBase buff, int stracks)
    {
        buffControl.ApplyBuff(buff, stracks);
    }

    public void UpdateBuffIcon(int buffId)
    {
        buffControl.UpdateBuffUI(buffId);
    }

    public void RemoveBuff(int buffId)
    {
        buffControl.RemoveBuff(buffId);
    }

    public Damage CalculateAtkDamage(Damage damage)
    {
        foreach (var i in ChangeAtkDamageEvent.Keys)
        {
            ChangeAtkDamageEvent[i].Invoke(damage);
            Debug.Log(damage.DamageValue + " " + damage.DamageRate);
        }
        return damage;
    }

    public Damage CalculateHitDamage(Damage damage)
    {
        foreach (var i in ChangeHitDamageEvent.Keys)
        {
            ChangeHitDamageEvent[i].Invoke(damage);
        }
        return damage;
    }

    public Block CalculateBlock(Block block) 
    {
        return block;
    }

    public virtual void ChangeAttribute(ERoleAttribute attribute, int value) { }
}

public enum ERoleAttribute
{
    HP,
    MP,
    AP,
    Aesist
}