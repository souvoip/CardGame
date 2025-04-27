using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillCard", menuName = "Data/Card/SkillCard")]
public class SkillCard : CardBase
{
    /// <summary>
    /// 给予的护盾值
    /// </summary>
    public Block BaseBlock;
    /// <summary>
    /// 次数
    /// </summary>
    public int SkillCount = 1;

    public SkillCard()
    {
        CardType = ECardType.Skill;
    }

    public override void UseCard()
    {
        AddBuffs(EAddBuffTime.BeforeAttack);

        // 添加护盾
        if(BaseBlock.Value != 0)
        {
            var tempBlock = new Block(BaseBlock);
            BattleManager.Instance.Player.ChangeAttribute(ERoleAttribute.Aesist, BattleManager.Instance.Player.CalculateBlock(tempBlock).GetBlockValue());
            BattleAnimManager.Instance.PlayAnim(BattleManager.Instance.Player.transform.position, cardAnimData);
        }
        AddBuffs(EAddBuffTime.AfterAttack);

        base.UseCard();
    }

    public override void UseCard(CharacterBase target)
    {
        AddBuffs(EAddBuffTime.BeforeAttack, target);

        // 添加护盾
        if(BaseBlock.Value != 0)
        {
            var tempBlock = new Block(BaseBlock);
            BattleManager.Instance.Player.ChangeAttribute(ERoleAttribute.Aesist, BattleManager.Instance.Player.CalculateBlock(tempBlock).GetBlockValue());
            BattleAnimManager.Instance.PlayAnim(BattleManager.Instance.Player.transform.position, cardAnimData);
        }
        AddBuffs(EAddBuffTime.AfterAttack, target);

        base.UseCard(target);
    }

    public override string GetDesc()
    {
        string desc = GetFeaturesDesc();
        // 计算实际防护值
        int baseBlockValue = BaseBlock.GetBlockValue();
        //int damageValue = GameTools.CalculateDamage(BattleManager.Instance.Player, BattleManager.Instance.CardManager.NowSelectEnemy, BaseDamage);
        //string damageStr = baseDamageValue > damageValue ? $"</color=#FF0000>{damageValue}</color>" : baseDamageValue == damageValue ? $"{damageValue}" : $"<color=#00FF00>{damageValue}</color>";
        if(baseBlockValue != 0)
        {
            if (SkillCount == 1)
            {
                desc += "获取" + baseBlockValue + "抵抗\n";
            }
            else
            {
                desc += "获取" + baseBlockValue + "抵抗" + SkillCount + "次\n";
            }
        }
        desc += GetBuffsDesc() + Desc;
        return desc;
    }
}
