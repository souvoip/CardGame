using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRoleData", menuName = "Data/Character/EnemyRoleData")]
public class EnemyRoleData : RoleData
{
    [SerializeReference]
    public List<EnemyDoAction> Actions = new List<EnemyDoAction>();

    // 当前正在编辑的动作索引
    public int SelectedActionIndex = -1;

    [SerializeReference]
    public EnemyDoAction EditAction;

    public void InitActions(CharacterBase self)
    {
        for (int i = 0; i < Actions.Count; i++)
        {
            Actions[i].self = self;
        }
    }
}

[Serializable]
public abstract class EnemyDoAction
{
    public abstract EEnemyActionType ActionType { get; }

    [NonSerialized]
    public CharacterBase self;

    public virtual void DoAction() { }
}

[Serializable]
public class EnemyAttackAction : EnemyDoAction
{
    public override EEnemyActionType ActionType => EEnemyActionType.Attack;

    public Damage baseDamage;

    public override void DoAction()
    {
        // 攻击玩家
        Damage tempDamage = new Damage(baseDamage);
        self.CalculateAtkDamage(tempDamage);
        BattleManager.Instance.Player.CalculateHitDamage(tempDamage);
        BattleManager.Instance.Player.ChangeAttribute(ERoleAttribute.HP, -tempDamage.GetDamage());
    }
}

[Serializable]
public class EnemyGetAesistAction : EnemyDoAction
{
    public override EEnemyActionType ActionType => EEnemyActionType.GetAesist;

    public Block baseBlock;

    public override void DoAction()
    {
        // 获取抵抗 TODO: 需要计算相关Buff
        self.ChangeAttribute(ERoleAttribute.Aesist, baseBlock.GetBlockValue());
    }
}

[Serializable]
public class EnemyGetBuffAction : EnemyDoAction
{
    public override EEnemyActionType ActionType => EEnemyActionType.GetBuff;
    public List<BuffItem> buffs;

    public override void DoAction()
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            self.AddBuff(BuffDataManager.GetBuff(buffs[i].BuffID), buffs[i].Stacks);
        }
    }
}
[Serializable]
public class EnemyGiveBuffAction : EnemyDoAction
{
    public override EEnemyActionType ActionType => EEnemyActionType.GiveBuff;
    public List<BuffItem> buffs;
    public override void DoAction()
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            BattleManager.Instance.Player.AddBuff(BuffDataManager.GetBuff(buffs[i].BuffID), buffs[i].Stacks);
        }
    }
}

[Serializable]
public class EnemySummonAction : EnemyDoAction
{
    public override EEnemyActionType ActionType => EEnemyActionType.Summon;
    //public List<EnemyRoleData> enemies;
    public override void DoAction()
    {
    }
}

[Serializable]
public class EnemyGiveCardAction : EnemyDoAction
{
    public override EEnemyActionType ActionType => EEnemyActionType.GiveCard;
    public List<int> cardIDs;
    public override void DoAction()
    {

    }
}

[System.Flags]
public enum EEnemyActionType
{
    /// <summary>
    /// 攻击玩家
    /// </summary>
    Attack = 1 << 0,
    /// <summary>
    /// 获取抵抗
    /// </summary>
    GetAesist = 1 << 1,
    /// <summary>
    /// 获取buff
    /// </summary>
    GetBuff = 1 << 2,
    /// <summary>
    /// 给予debuff
    /// </summary>
    GiveBuff = 1 << 3,
    /// <summary>
    /// 召唤敌人
    /// </summary>
    Summon = 1 << 4,
    /// <summary>
    /// 给予玩家卡牌
    /// </summary>
    GiveCard = 1 << 5,
}