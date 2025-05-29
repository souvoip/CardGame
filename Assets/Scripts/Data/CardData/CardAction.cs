using System;

[Serializable]
public abstract class CardAction : ICloneable, ISaveLoad
{
    public abstract ECardActionType ActionType { get; }

    public abstract object Clone();

    public abstract void Execute(CharacterBase target, BattleAnimData anim = null);

    public abstract string GetDesc();

    public virtual void Load(JSONObject data)
    {
    }

    public virtual JSONObject Save()
    {
        JSONObject data = JSONObject.Create();
        data.AddField("ActionType", (int)ActionType);
        return data;
    }

    public static CardAction Create(JSONObject data)
    {
        ECardActionType type = (ECardActionType)data.GetField("ActionType").i;
        switch (type)
        {
            case ECardActionType.Damage:
                CardDamageAction act = new CardDamageAction();
                act.Load(data);
                return act;
            case ECardActionType.Defend:
                CardDefendAction act2 = new CardDefendAction();
                act2.Load(data);
                return act2;
            case ECardActionType.Buff:
                CardBuffAction act3 = new CardBuffAction();
                act3.Load(data);
                return act3;
            case ECardActionType.CardExtract:
                CardExtractAction act4 = new CardExtractAction();
                act4.Load(data);
                return act4;
            default:
                return null;
        }
    }
}

[Serializable]
public class CardDamageAction : CardAction
{
    public Damage BaseDamage;

    public ETargetRole TargetRole;
    /// <summary>
    /// 攻击次数
    /// </summary>
    public int HitCount;

    public override ECardActionType ActionType => ECardActionType.Damage;

    public override object Clone()
    {
        CardDamageAction clone = new CardDamageAction();
        clone.BaseDamage = new Damage(BaseDamage);
        clone.TargetRole = TargetRole;
        clone.HitCount = HitCount;
        return clone;
    }

    public override void Execute(CharacterBase target, BattleAnimData anim = null)
    {
        // 造成伤害
        if(target == null)
        {
            Execute(anim);
            return;
        }
        int damageValue = GameTools.CalculateDamage(BattleManager.Instance.Player, target, BaseDamage);
        for (int i = 0; i < HitCount; i++)
        {
            if (target.IsDie) { break; }
            BattleManager.Instance.Player.AtkTarget(target, damageValue);
            if (anim != null)
            {
                BattleAnimManager.Instance.PlayAnim(target.transform.position, anim);
            }
        }

    }

    public void Execute(BattleAnimData anim = null)
    {
        // 对所有敌人造成伤害
        var damage = new Damage(BaseDamage);
        damage = BattleManager.Instance.Player.CalculateCauseDamage(damage);
        for (int i = BattleManager.Instance.EnemyRoles.Count - 1; i >= 0; i--)
        {
            if (BattleManager.Instance.EnemyRoles[i].IsDie) { continue; }
            var tempDamage = new Damage(damage);
            tempDamage = BattleManager.Instance.EnemyRoles[i].CalculateTakeDamage(tempDamage);
            for (int j = 0; j < HitCount; j++)
            {
                BattleManager.Instance.Player.AtkTarget(BattleManager.Instance.EnemyRoles[i], tempDamage.GetDamageValue());
                //BattleManager.Instance.EnemyRoles[i].ChangeAttribute(ERoleAttribute.HP, -tempDamage.GetDamageValue());
                BattleAnimManager.Instance.PlayAnim(BattleManager.Instance.EnemyRoles[i].transform.position, anim);
            }
        }
    }

    public override string GetDesc()
    {
        string desc = "";
        if (TargetRole == ETargetRole.Enemy) { desc += "对目标造成"; }
        else if (TargetRole == ETargetRole.AllEnemy) { desc += "对所有敌人造成"; }
        else if (TargetRole == ETargetRole.Self) { desc += "受到"; }
        else if (TargetRole == ETargetRole.All) { desc += "对所有单位造成"; }
        // 计算实际伤害
        int baseDamageValue = BaseDamage.GetDamageValue();
        int damageValue = GameTools.CalculateDamage(BattleManager.Instance.Player, BattleManager.Instance.nowSelectEnemy, BaseDamage);
        string damageStr = baseDamageValue > damageValue ? $"<color=#FF0000>{damageValue}</color>" : baseDamageValue == damageValue ? $"{damageValue}" : $"<color=#00FF00>{damageValue}</color>";
        if (HitCount == 1)
        {
            desc += damageStr + "点伤害";
        }
        else
        {
            desc += damageStr + "点伤害" + HitCount + "次";
        }

        return desc;
    }

    public override JSONObject Save()
    {
        JSONObject data = base.Save();
        data.AddField("BaseDamage", BaseDamage.Save());
        data.AddField("TargetRole", (int)TargetRole);
        data.AddField("HitCount", HitCount);
        return data;
    }

    public override void Load(JSONObject data)
    {
        base.Load(data);
        BaseDamage = new Damage(data.GetField("BaseDamage"));
        TargetRole = (ETargetRole)data.GetField("TargetRole").i;
        HitCount = (int)data.GetField("HitCount").i;
    }
}

[Serializable]
public class CardDefendAction : CardAction
{
    /// <summary>
    /// 给予的护盾值
    /// </summary>
    public Block BaseBlock;
    /// <summary>
    /// 次数
    /// </summary>
    public int SkillCount = 1;
    public override ECardActionType ActionType => ECardActionType.Defend;

    public override object Clone()
    {
        CardDefendAction clone = new CardDefendAction();
        clone.BaseBlock = new Block(BaseBlock);
        clone.SkillCount = SkillCount;
        return clone;
    }

    public override void Execute(CharacterBase target, BattleAnimData anim = null)
    {
        Execute(anim);
    }
    public void Execute(BattleAnimData anim = null)
    {
        // 添加护盾
        if (BaseBlock.Value != 0)
        {
            var tempBlock = new Block(BaseBlock);
            BattleManager.Instance.Player.ChangeAttribute(ERoleAttribute.Aesist, BattleManager.Instance.Player.CalculateBlock(tempBlock).GetBlockValue());
            BattleAnimManager.Instance.PlayAnim(BattleManager.Instance.Player.transform.position, anim);
        }
    }

    public override string GetDesc()
    {
        string desc = "";
        // 计算实际防护值
        var tempBlock = new Block(BaseBlock);
        int baseBlockValue = BaseBlock.GetBlockValue();
        int blockValue = BattleManager.Instance.Player.CalculateBlock(tempBlock).GetBlockValue();
        string damageStr = baseBlockValue > blockValue ? $"</color=#FF0000>{blockValue}</color>" : baseBlockValue == blockValue ? $"{blockValue}" : $"<color=#00FF00>{blockValue}</color>";
        if (baseBlockValue != 0)
        {
            if (SkillCount == 1)
            {
                desc += "获取" + damageStr + "抵抗\n";
            }
            else
            {
                desc += "获取" + damageStr + "抵抗" + SkillCount + "次\n";
            }
        }
        return desc;
    }

    public override JSONObject Save()
    {
        JSONObject data = base.Save();
        data.AddField("BaseBlock", BaseBlock.Save());
        data.AddField("SkillCount", SkillCount);
        return data;
    }

    public override void Load(JSONObject data)
    {
        base.Load(data);
        BaseBlock = new Block(data.GetField("BaseBlock"));
        SkillCount = (int)data.GetField("SkillCount").i;
    }
}

[Serializable]
public class CardBuffAction : CardAction
{
    public BuffItem Buff;

    public override ECardActionType ActionType => ECardActionType.Buff;
    public override void Execute(CharacterBase target, BattleAnimData anim = null)
    {
        AddBuff(target);
    }

    protected void AddBuff(CharacterBase characterTarget = null)
    {
        if (Buff.Target == ETargetRole.Self)
        {
            BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(Buff.BuffID), Buff.Stacks);
        }
        else if (Buff.Target == ETargetRole.Enemy)
        {
            if (characterTarget == null) { return; }
            if (characterTarget.IsDie) { return; }
            characterTarget.AddBuff(BuffDataManager.GetBuff(Buff.BuffID), Buff.Stacks);
        }
        else if (Buff.Target == ETargetRole.AllEnemy)
        {
            for (int j = 0; j < BattleManager.Instance.EnemyRoles.Count; j++)
            {
                if (BattleManager.Instance.EnemyRoles[j].IsDie) { continue; }
                BattleManager.Instance.EnemyRoles[j].AddBuff(BuffDataManager.GetBuff(Buff.BuffID), Buff.Stacks);
            }
        }
        else if (Buff.Target == ETargetRole.All)
        {
            BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(Buff.BuffID), Buff.Stacks);
            for (int j = 0; j < BattleManager.Instance.EnemyRoles.Count; j++)
            {
                if (BattleManager.Instance.EnemyRoles[j].IsDie) { continue; }
                BattleManager.Instance.EnemyRoles[j].AddBuff(BuffDataManager.GetBuff(Buff.BuffID), Buff.Stacks);
            }
        }
    }

    public override string GetDesc()
    {
        string str = "";
        switch (Buff.Target)
        {
            case ETargetRole.Self:
                str += $"给予自身{Buff.Stacks}层<color=#FFDA00>{BuffDataManager.GetBuffName(Buff.BuffID)}</color>\n";
                break;
            case ETargetRole.Enemy:
                str += $"给予目标{Buff.Stacks}层<color=#FFDA00>{BuffDataManager.GetBuffName(Buff.BuffID)}</color>\n";
                break;
            case ETargetRole.AllEnemy:
                str += $"给予所有敌人{Buff.Stacks}层<color=#FFDA00>{BuffDataManager.GetBuffName(Buff.BuffID)}</color>\n";
                break;
            case ETargetRole.All:
                str += $"给予所有单位{Buff.Stacks}层<color=#FFDA00>{BuffDataManager.GetBuffName(Buff.BuffID)}</color>\n";
                break;
        }
        return str;
    }

    public override object Clone()
    {
        CardBuffAction clone = new CardBuffAction();
        clone.Buff = new BuffItem(){ BuffID = Buff.BuffID, AddBuffTime = Buff.AddBuffTime, Stacks = Buff.Stacks, Target = Buff.Target };
        return clone;
    }

    public override JSONObject Save()
    {
        JSONObject data = base.Save();
        data.AddField("Buff", Buff.Save());
        return data;
    }

    public override void Load(JSONObject data)
    {
        base.Load(data);
        Buff = new BuffItem();
        Buff.Load(data.GetField("Buff"));
    }
}

[Serializable]
public class CardExtractAction : CardAction
{
    public CardExtract Extract;
    public override ECardActionType ActionType => ECardActionType.CardExtract;

    public override object Clone()
    {
        CardExtractAction clone = new CardExtractAction();
        clone.Extract = new CardExtract(){ origin = Extract.origin, target = Extract.target, Count = Extract.Count, cardType = Extract.cardType, mode = Extract.mode };
        return clone;
    }

    public override void Execute(CharacterBase _, BattleAnimData __ = null)
    {
        EventCenter<CardExtract>.GetInstance().EventTrigger(EventNames.EXTRACT_CARD, Extract);
    }

    public override string GetDesc() { return ""; }

    public override JSONObject Save()
    {
        JSONObject data = base.Save();
        data.AddField("Extract", Extract.Save());
        return data;
    }

    public override void Load(JSONObject data)
    {
        base.Load(data);
        Extract = new CardExtract();
        Extract.Load(data.GetField("Extract"));
    }
}

public enum ECardActionType
{
    Damage,
    Defend,
    Buff,
    CardExtract
}