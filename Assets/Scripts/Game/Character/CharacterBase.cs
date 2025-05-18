using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterBase : MonoBehaviour
{
    protected static string BaseRolePath = "Image/Role/";

    #region Events
    /// <summary>
    /// 角色死亡事件
    /// </summary>
    public event Action DieEvent;

    public Dictionary<int, Action<Damage>> ChangeCauseDamageEvent = new Dictionary<int, Action<Damage>>();

    public Dictionary<int, Action<Damage>> ChangeTakeDamageEvent = new Dictionary<int, Action<Damage>>();

    public Dictionary<int, Action<Block>> ChangeBlockEvent = new Dictionary<int, Action<Block>>();
    /// <summary>
    /// 角色受伤事件, 造成伤害的来源,伤害值
    /// </summary>
    public event Action<CharacterBase, int> TakeDamageEvent;
    /// <summary>
    /// 角色攻击事件, 攻击的目标,伤害值
    /// </summary>
    public event Action<CharacterBase, int> CauseDamageEvent;
    /// <summary>
    ///  角色获得buff事件，获得的buff
    /// </summary>
    public event Action<BuffBase> AddBuffEvent;
    #endregion

    protected BuffControl buffControl;

    protected bool isDie = false;
    public bool IsDie { get { return isDie; } }

    public virtual bool IsPlayer { get; }

    protected virtual void Init() { }

    private void Start()
    {
        buffControl = GetComponent<BuffControl>();
        Init();
    }

    public void AddBuff(BuffBase buff, int stracks)
    {
        if(stracks > 0)
        {
            AddBuffEvent?.Invoke(buff);
        }
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

    public Damage CalculateCauseDamage(Damage damage)
    {
        foreach (var i in ChangeCauseDamageEvent.Keys)
        {
            if (!damage.isNext)
            {
                break;
            }
            ChangeCauseDamageEvent[i].Invoke(damage);
        }
        return damage;
    }

    public Damage CalculateTakeDamage(Damage damage)
    {
        foreach (var i in ChangeTakeDamageEvent.Keys)
        {
            if (!damage.isNext)
            {
                break;
            }
            ChangeTakeDamageEvent[i].Invoke(damage);
        }
        return damage;
    }

    public Block CalculateBlock(Block block)
    {
        foreach (var i in ChangeBlockEvent.Keys)
        {
            ChangeBlockEvent[i].Invoke(block);
        }
        return block;
    }

    /// <summary>
    /// 攻击目标
    /// </summary>
    /// <param name="target"> 目标 </param>
    /// <param name="damage"> 伤害值 </param>
    public void AtkTarget(CharacterBase target, int damage)
    {
        target.TakeDamage(this, damage);
        CauseDamageEvent?.Invoke(target, damage);
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="from"> 伤害来原 </param>
    /// <param name="damage"> 数值 </param>
    public void TakeDamage(CharacterBase from, int damage)
    {
        ChangeAttribute(ERoleAttribute.HP, -damage);
        TakeDamageEvent?.Invoke(from, damage);
    }

    public virtual int GetAttributeValue(ERoleAttribute attribute) { return 0; }

    public virtual void ChangeAttribute(ERoleAttribute attribute, int value) { }

    public virtual void Die()
    {
        DieEvent?.Invoke();
    }
}

public enum ERoleAttribute
{
    HP,
    MaxHP,
    MP,
    MaxMP,
    AP,
    MaxAP,
    Aesist,
    Shield,
    DrawCardCount,
    MaxCardCount,
    Gold,

    Age,
    Level,
    Exp,
}