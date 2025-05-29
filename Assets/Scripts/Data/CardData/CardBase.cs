using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class CardBase : ScriptableObject, ISaveLoad
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

    private int Level = 1;
    [DisplayOnly]
    public int MaxLevel = 1;

    public ECardFeatures Features;
    public BattleAnimData CardAnimData;
    [SerializeReference]
    public List<CardAction> CardActions;

    public List<CardLevelData> LevelDatas;

    /// <summary>
    /// 非指向性
    /// </summary>
    public virtual void UseCard()
    {
        UseCard(null);
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
                        CardActions[i].Execute(characterTarget, CardAnimData);
                    }
                }
                break;
            case ECardActionType.Defend:
                for (int i = 0; i < CardActions.Count; i++)
                {
                    if (CardActions[i].ActionType == ECardActionType.Defend)
                    {
                        CardActions[i].Execute(characterTarget, CardAnimData);
                    }
                }
                break;
            case ECardActionType.Buff:
                for (int i = 0; i < CardActions.Count; i++)
                {
                    if ((CardActions[i].ActionType == ECardActionType.Buff) && (CardActions[i] as CardBuffAction).Buff.AddBuffTime == triggerTime)
                    {
                        CardActions[i].Execute(characterTarget, CardAnimData);
                    }
                }
                break;
            case ECardActionType.CardExtract:
                for (int i = 0; i < CardActions.Count; i++)
                {
                    if (CardActions[i].ActionType == ECardActionType.CardExtract)
                    {
                        CardActions[i].Execute(characterTarget, CardAnimData);
                    }
                }
                break;
        }
    }

    public void UpgradeCard()
    {
        if(!IsUpgrade()) { return; }
        Level++;
        SetLevelData(Level);
    }

    public void SetLevelData(int level)
    {
        if (level > MaxLevel) { return; }
        Level = level;
        var cld = LevelDatas[level - 1];
        Name = cld.UName;
        Desc = cld.UDesc;
        ImagePath = cld.UImagePath;
        Price = cld.UPrice;
        Fee = cld.UFee;
        Features |= cld.UAddFeatures;
        Features &= ~cld.URemoveFeatures;
        CardAnimData = cld.UCardAnimData;
        CardActions = cld.UCardActions;
    }
    /// <summary>
    /// 是否可以升级
    /// </summary>
    /// <returns></returns>
    public bool IsUpgrade()
    {
        return Level < MaxLevel;
    }

    public virtual JSONObject Save()
    {
        JSONObject data = JSONObject.Create();
        data.AddField("CardType", (int)CardType);
        data.AddField("ID", ID);
        data.AddField("Name", Name);
        data.AddField("Desc", Desc);
        data.AddField("ImagePath", ImagePath);
        data.AddField("UseType", (int)UseType);
        data.AddField("Rare", (int)Rare);
        data.AddField("Price", Price);
        data.AddField("Fee", Fee);
        data.AddField("GetWay", (int)GetWay);
        data.AddField("Level", Level);
        data.AddField("MaxLevel", MaxLevel);
        data.AddField("Features", (int)Features);
        data.AddField("CardAnimData", CardAnimData.Save());
        JSONObject actionsData = JSONObject.Create(JSONObject.Type.ARRAY);
        for (int i = 0; i < CardActions.Count; i++)
        {
            actionsData.Add(CardActions[i].Save());
        }
        data.AddField("CardActions", actionsData);
        JSONObject levelDatas = JSONObject.Create(JSONObject.Type.ARRAY);
        for (int i = 0; i < LevelDatas.Count; i++)
        {
            levelDatas.Add(LevelDatas[i].Save());
        }
        data.AddField("LevelDatas", levelDatas);
        return data;
    }

    public virtual void Load(JSONObject data)
    {
        ID = (int)data.GetField("ID").i;
        Name = data.GetField("Name").str;
        Desc = data.GetField("Desc").str;
        ImagePath = data.GetField("ImagePath").str;
        UseType = (EUseType)data.GetField("UseType").i;
        Rare = (ECardRare)data.GetField("Rare").i;
        Price = (int)data.GetField("Price").i;
        Fee = (int)data.GetField("Fee").i;
        GetWay = (EGetWay)data.GetField("GetWay").i;
        Level = (int)data.GetField("Level").i;
        MaxLevel = (int)data.GetField("MaxLevel").i;
        Features = (ECardFeatures)data.GetField("Features").i;
        CardAnimData = new BattleAnimData();
        CardAnimData.Load(data.GetField("CardAnimData"));
        CardActions = new List<CardAction>();
        for (int i = 0; i < data.GetField("CardActions").Count; i++)
        {
            CardActions.Add(CardAction.Create(data.GetField("CardActions")[i]));
        }
        LevelDatas = new List<CardLevelData>();
        for (int i = 0; i < data.GetField("LevelDatas").Count; i++)
        {
            var levelData = new CardLevelData();
            levelData.Load(data.GetField("LevelDatas")[i]);
            LevelDatas.Add(levelData);
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


[Serializable]
public class CardLevelData : ISaveLoad
{
    public string UName;
    public string UDesc;
    public string UImagePath;
    public int UPrice;
    public int UFee;

    public ECardFeatures UAddFeatures;
    public ECardFeatures URemoveFeatures;
    public BattleAnimData UCardAnimData;
    [SerializeReference]
    public List<CardAction> UCardActions;

    public void Load(JSONObject data)
    {
        UName = data.GetField("UName").str;
        UDesc = data.GetField("UDesc").str;
        UImagePath = data.GetField("UImagePath").str;
        UPrice = (int)data.GetField("UPrice").i;
        UFee = (int)data.GetField("UFee").i;
        UAddFeatures = (ECardFeatures)data.GetField("UAddFeatures").i;
        URemoveFeatures = (ECardFeatures)data.GetField("URemoveFeatures").i;
        UCardAnimData = new BattleAnimData();
        UCardAnimData.Load(data.GetField("UCardAnimData"));
        UCardActions = new List<CardAction>();
        for (int i = 0; i < data.GetField("UCardActions").Count; i++)
        {
            UCardActions.Add(CardAction.Create(data.GetField("UCardActions")[i]));
        }
    }

    public JSONObject Save()
    {
        JSONObject data = JSONObject.Create();
        data.AddField("UName", UName);
        data.AddField("UDesc", UDesc);
        data.AddField("UImagePath", UImagePath);
        data.AddField("UPrice", UPrice);
        data.AddField("UFee", UFee);
        data.AddField("UAddFeatures", (int)UAddFeatures);
        data.AddField("URemoveFeatures", (int)URemoveFeatures);
        data.AddField("UCardAnimData", UCardAnimData.Save());
        JSONObject actionsData = JSONObject.Create(JSONObject.Type.ARRAY);
        for (int i = 0; i < UCardActions.Count; i++)
        {
            actionsData.Add(UCardActions[i].Save());
        }
        data.AddField("UCardActions", actionsData);
        return data;
    }
}