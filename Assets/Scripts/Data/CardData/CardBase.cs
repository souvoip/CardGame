using System.Collections.Generic;
using UnityEngine;

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
    /// 卡片购买价格
    /// </summary>
    public int Price;
    /// <summary>
    /// 卡片费用
    /// </summary>
    public int Fee;

    public EGetWay GetWay;

    public List<BuffItem> Buffs;

    public List<CardExtract> Extract;

    public ECardFeatures Features;

    public BattleAnimData cardAnimData;

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
    /// 抽到后触发
    /// </summary>
    public virtual void Draw()
    {

    }

    /// <summary>
    /// 回合结束触发
    /// </summary>
    public virtual void TurnOver() { }

    public bool IsCanUse()
    {
        if (UseType == EUseType.CannotUse) { return false; }
        if (BattleManager.Instance.Player.RoleData.AP < Fee) { return false; }
        return true;
    }

    protected virtual void UseOver()
    {
        // 触发抽取或移除卡牌相关功能
        for (int i = 0; i < Extract.Count; i++)
        {
            EventCenter<CardExtract>.GetInstance().EventTrigger(EventNames.EXTRACT_CARD, Extract[i]);
        }
        // 消耗费用
        BattleManager.Instance.Player.ChangeAttribute(ERoleAttribute.AP, -Fee);
    }

    protected void AddBuffs(EAddBuffTime addTime, CharacterBase characterTarget = null)
    {
        for (int i = 0; i < Buffs.Count; i++)
        {
            if (Buffs[i].AddBuffTime == addTime)
            {
                if (Buffs[i].Target == ETargetRole.Self)
                {
                    BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                }
                else if (Buffs[i].Target == ETargetRole.Enemy)
                {
                    if (characterTarget == null) { return; }
                    if (characterTarget.IsDie) { return; }
                    characterTarget.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                }
                else if (Buffs[i].Target == ETargetRole.AllEnemy)
                {
                    for (int j = 0; j < BattleManager.Instance.EnemyRoles.Count; j++)
                    {
                        if (BattleManager.Instance.EnemyRoles[j].IsDie) { continue; }
                        BattleManager.Instance.EnemyRoles[j].AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                    }
                }
                else if (Buffs[i].Target == ETargetRole.All)
                {
                    BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                    for (int j = 0; j < BattleManager.Instance.EnemyRoles.Count; j++)
                    {
                        if (BattleManager.Instance.EnemyRoles[j].IsDie) { continue; }
                        BattleManager.Instance.EnemyRoles[j].AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
                    }
                }
            }
        }
    }

    public virtual List<DetailInfo> GetDetailInfos()
    {
        List<DetailInfo> infos = new List<DetailInfo>();
        GameTools.GetDetailInfosWithCardFeatures(Features, ref infos);
        foreach (var item in Buffs)
        {
            infos.Add(item.GetDetailInfo());
        }
        return infos;
    }

    public virtual string GetDesc()
    {
        return GetFeaturesDesc() + GetBuffsDesc() + Desc;
    }

    protected string GetFeaturesDesc()
    {
        if (Features == ECardFeatures.None) { return ""; }
        string str = "<color=#FFDA00>";
        if ((Features & ECardFeatures.Fixed) == ECardFeatures.Fixed) { str += "固有，"; }
        if ((Features & ECardFeatures.Hold) == ECardFeatures.Hold) { str += "保留，"; }
        if ((Features & ECardFeatures.Cost) == ECardFeatures.Cost) { str += "消耗，"; }
        if ((Features & ECardFeatures.Void) == ECardFeatures.Void) { str += "虚无，"; }
        str = str.Remove(str.Length - 1);
        str += "</color>\n";
        return str;
    }

    protected string GetBuffsDesc()
    {
        string str = "";
        foreach (var item in Buffs)
        {
            switch (item.Target)
            {
                case ETargetRole.Self:
                    str += $"给予自身{item.Stacks}层<color=#FFDA00>{BuffDataManager.GetBuffName(item.BuffID)}</color>\n";
                    break;
                case ETargetRole.Enemy:
                    str += $"给予目标{item.Stacks}层<color=#FFDA00>{BuffDataManager.GetBuffName(item.BuffID)}</color>\n";
                    break;
                case ETargetRole.AllEnemy:
                    str += $"给予所有敌人{item.Stacks}层<color=#FFDA00>{BuffDataManager.GetBuffName(item.BuffID)}</color>\n";
                    break;
                case ETargetRole.All:
                    str += $"给予所有单位{item.Stacks}层<color=#FFDA00>{BuffDataManager.GetBuffName(item.BuffID)}</color>\n";
                    break;
            }
        }
        return str;
    }

    //protected string GetCardExtractDesc()
    //{
    //    string str = "";
    //    foreach (var item in Extract)
    //    {
    //        switch (item.origin)
    //        {
    //            case ECardRegion.Draw:

    //                break;
    //        }
    //    }
    //    return str;
    //}
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
    None = 0,
    /// <summary>
    /// 消耗
    /// </summary>
    Cost = 1 << 0,
    /// <summary>
    /// 固有
    /// </summary>
    Fixed = 1 << 1,
    /// <summary>
    /// 虚无
    /// </summary>
    Void = 1 << 2,
    /// <summary>
    /// 保留
    /// </summary>
    Hold = 1 << 3,
}


public abstract class CardAction
{
    public abstract ECardActionType ActionType { get; }

    public virtual void Execute() { }

    public virtual void Execute(CharacterBase target) { }
}

public class CardAction_Damage : CardAction
{
    public override ECardActionType ActionType => ECardActionType.Damage;

    public Damage BaseDamage;

    /// <summary>
    /// 攻击次数
    /// </summary>
    public int HitCount;
}

public class CardAction_Defend : CardAction
{
    public override ECardActionType ActionType => ECardActionType.Defend;

    /// <summary>
    /// 给予的护盾值
    /// </summary>
    public Block BaseBlock;
    /// <summary>
    /// 次数
    /// </summary>
    public int SkillCount = 1;
}

public class CardAction_Buff : CardAction
{
    public override ECardActionType ActionType => ECardActionType.Buff;

    public BuffItem Buff;
}

public class CardAction_CardExtract : CardAction
{
    public override ECardActionType ActionType => ECardActionType.CardExtract;

    public List<CardExtract> Extract;
}

public enum ECardActionType
{
    Damage = 1,
    Defend = 2,
    Buff = 3,
    CardExtract = 4,
}