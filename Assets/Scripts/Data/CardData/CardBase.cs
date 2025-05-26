using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class CardBase : ScriptableObject
{
    /// <summary>
    /// 卡片类型
    /// </summary>
    public abstract ECardType CardType { get; }
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

    public int Level;

    public int MaxLevel;

    public ECardFeatures Features;
    public BattleAnimData cardAnimData;
    [SerializeReference]
    public List<CardAction> CardActions;


    /// <summary>
    /// 非指向性
    /// </summary>
    public virtual void UseCard()
    {
        UseCard(null);
        UseOver();
    }
    /// <summary>
    /// 指向性
    /// </summary>
    /// <param name="target"></param>
    public virtual void UseCard(CharacterBase target)
    {
        TargetAction(ECardActionType.Buff, EBuffTriggerTime.BeforeAttack, target);
        TargetAction(ECardActionType.Damage, EBuffTriggerTime.None, target);
        TargetAction(ECardActionType.Defend, EBuffTriggerTime.None, target);
        TargetAction(ECardActionType.Buff, EBuffTriggerTime.AfterAttack, target);
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
        TargetAction(ECardActionType.CardExtract, EBuffTriggerTime.None, null);
        // 消耗费用
        BattleManager.Instance.Player.ChangeAttribute(ERoleAttribute.AP, -Fee);
    }

    //protected void AddBuffs(EBuffTriggerTime addTime, CharacterBase characterTarget = null)
    //{
    //    for (int i = 0; i < Buffs.Count; i++)
    //    {
    //        if (Buffs[i].AddBuffTime == addTime)
    //        {
    //            if (Buffs[i].Target == ETargetRole.Self)
    //            {
    //                BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
    //            }
    //            else if (Buffs[i].Target == ETargetRole.Enemy)
    //            {
    //                if (characterTarget == null) { return; }
    //                if (characterTarget.IsDie) { return; }
    //                characterTarget.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
    //            }
    //            else if (Buffs[i].Target == ETargetRole.AllEnemy)
    //            {
    //                for (int j = 0; j < BattleManager.Instance.EnemyRoles.Count; j++)
    //                {
    //                    if (BattleManager.Instance.EnemyRoles[j].IsDie) { continue; }
    //                    BattleManager.Instance.EnemyRoles[j].AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
    //                }
    //            }
    //            else if (Buffs[i].Target == ETargetRole.All)
    //            {
    //                BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
    //                for (int j = 0; j < BattleManager.Instance.EnemyRoles.Count; j++)
    //                {
    //                    if (BattleManager.Instance.EnemyRoles[j].IsDie) { continue; }
    //                    BattleManager.Instance.EnemyRoles[j].AddBuff(BuffDataManager.GetBuff(Buffs[i].BuffID), Buffs[i].Stacks);
    //                }
    //            }
    //        }
    //    }
    //}

    public virtual List<DetailInfo> GetDetailInfos()
    {
        List<DetailInfo> infos = new List<DetailInfo>();
        GameTools.GetDetailInfosWithCardFeatures(Features, ref infos);
        for(int i = 0; i < CardActions.Count; i++)
        {
            if(CardActions[i].ActionType == ECardActionType.Buff)
            {
                infos.Add((CardActions[i] as CardBuffAction).Buff.GetDetailInfo());
            }
        }
        return infos;
    }

    public virtual string GetDesc()
    {
        return GetFeaturesDesc() + GetActionsDesc() + Desc;
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

    protected string GetActionsDesc()
    {
        string damageStr = "";
        string defendStr = "";
        string buffStr = "";
        foreach (var item in CardActions)
        {
            switch (item.ActionType)
            {
                case ECardActionType.Damage:
                    damageStr += item.GetDesc() + "\n";
                    break;
                case ECardActionType.Defend:
                    defendStr += item.GetDesc() + "\n";
                    break;
                case ECardActionType.Buff:
                    buffStr += item.GetDesc() + "\n";
                    break;
                case ECardActionType.CardExtract:
                    break;
            }
        }
        return damageStr + defendStr + buffStr;
    }

    protected void TargetAction(ECardActionType actionType, EBuffTriggerTime triggerTime, CharacterBase characterTarget = null)
    {
        switch (actionType)
        {
            case ECardActionType.Damage:
                for (int i = 0; i < CardActions.Count; i++)
                {
                    if (CardActions[i].ActionType == ECardActionType.Damage)
                    {
                        CardActions[i].Execute(characterTarget, cardAnimData);
                    }
                }
                break;
            case ECardActionType.Defend:
                for (int i = 0; i < CardActions.Count; i++)
                {
                    if (CardActions[i].ActionType == ECardActionType.Defend)
                    {
                        CardActions[i].Execute(characterTarget, cardAnimData);
                    }
                }
                break;
            case ECardActionType.Buff:
                for (int i = 0; i < CardActions.Count; i++)
                {
                    if ((CardActions[i].ActionType == ECardActionType.Buff) && (CardActions[i] as CardBuffAction).Buff.AddBuffTime == triggerTime)
                    {
                        CardActions[i].Execute(characterTarget, cardAnimData);
                    }
                }
                break;
        }
    }

    public void UpgradeCard()
    {

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
