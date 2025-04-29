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
        var tempBlock = new Block(BaseBlock);
        int baseBlockValue = BaseBlock.GetBlockValue();
        int blockValue = BattleManager.Instance.Player.CalculateBlock(tempBlock).GetBlockValue();
        string damageStr = baseBlockValue > blockValue ? $"</color=#FF0000>{blockValue}</color>" : baseBlockValue == blockValue ? $"{blockValue}" : $"<color=#00FF00>{blockValue}</color>";
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
