using System;

[Serializable]
public abstract class CardAction
{
    public abstract ECardActionType ActionType { get; }

    public abstract void Execute(CharacterBase target, BattleAnimData anim = null);

    public abstract string GetDesc();
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
}

[Serializable]
public class CardExtractAction : CardAction
{
    public CardExtract Extract;
    public override ECardActionType ActionType => ECardActionType.CardExtract;
    public override void Execute(CharacterBase _, BattleAnimData __ = null)
    {
        EventCenter<CardExtract>.GetInstance().EventTrigger(EventNames.EXTRACT_CARD, Extract);
    }

    public override string GetDesc() { return ""; }
}

public enum ECardActionType
{
    Damage,
    Defend,
    Buff,
    CardExtract
}