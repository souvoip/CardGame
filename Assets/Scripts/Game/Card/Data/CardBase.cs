using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class CardBase : ScriptableObject
{
    /// <summary>
    /// 卡片ID
    /// </summary>
    public int ID;
    /// <summary>
    /// 卡片名字
    /// </summary>
    public string Name;
    /// <summary>
    /// 卡片描述
    /// </summary>
    public string Desc;
    /// <summary>
    /// 卡片图片
    /// </summary>
    public string ImagePath;
    /// <summary>
    /// 卡片类型
    /// </summary>
    public ECardType CardType;
    /// <summary>
    /// 卡片使用类型
    /// </summary>
    public EUseType UseType;
    /// <summary>
    /// 卡片稀有度
    /// </summary>
    public ECardRare Rare;
    /// <summary>
    /// 卡片费用
    /// </summary>
    public int Fee;

    public List<BuffItem> Buffs;

    public List<CardExtract> Extract;

    public ECardFeatures Features;

    /// <summary>
    /// 非指向性
    /// </summary>
    public virtual void UseCard()
    {
        UseOver();
    }
    /// <summary>
    /// 指向性
    /// </summary>
    /// <param name="target"></param>
    public virtual void UseCard(CharacterBase target)
    {
        UseOver();
    }

    /// <summary>
    /// 抽区触发
    /// </summary>
    public virtual void Draw()
    {

    }

    /// <summary>
    /// 回合结束触发
    /// </summary>
    public virtual void TurnOver() { }

    protected virtual void UseOver()
    {
        // TODO：触发抽取或移除卡牌相关功能

        // TODO：使用结束，将卡牌移动到弃牌堆或者其它地方

    }

    protected void AddBuffs(EAddBuffTime addTime, CharacterBase characterTarget = null)
    {
        for (int i = 0; i < Buffs.Count; i++)
        {
            if (Buffs[i].AddBuffTime == addTime)
            {
                if (Buffs[i].Target == EBuffTarget.Self)
                {
                    BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                }
                else if (Buffs[i].Target == EBuffTarget.Enemy)
                {
                    if (characterTarget == null) { return; }
                    characterTarget.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                }
                else if (Buffs[i].Target == EBuffTarget.AllEnemy)
                {
                    for (int j = 0; j < BattleManager.Instance.EnemyRoles.Count; j++)
                    {
                        BattleManager.Instance.EnemyRoles[j].AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                    }
                }
                else if (Buffs[i].Target == EBuffTarget.All)
                {
                    BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                    for (int j = 0; j < BattleManager.Instance.EnemyRoles.Count; j++)
                    {
                        BattleManager.Instance.EnemyRoles[j].AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                    }
                }
            }
        }
    }
}

public enum ECardRare
{
    /// <summary>
    /// 普通
    /// </summary>
    Common = 1,
    /// <summary>
    /// 稀有
    /// </summary>
    Rare = 2,
    /// <summary>
    /// 史诗
    /// </summary>
    Epic = 3,
}

public enum ECardRegion
{
    /// <summary>
    /// 抽卡区
    /// </summary>
    Draw = 1,
    /// <summary>
    /// 手牌区
    /// </summary>
    Hand = 2,
    /// <summary>
    /// 弃牌区
    /// </summary>
    Discard = 3,
    /// <summary>
    /// 消耗区
    /// </summary>
    Cost = 4,
    /// <summary>
    /// 移除区
    /// </summary>
    Remove = 5,
}

[System.Flags]
public enum ECardFeatures
{
    /// <summary>
    /// 消耗
    /// </summary>
    Cost = 1 << 0,
    /// <summary>
    /// 固有
    /// </summary>
    Permanent = 1 << 1,
    /// <summary>
    /// 虚无
    /// </summary>
    Void = 1 << 2,
}