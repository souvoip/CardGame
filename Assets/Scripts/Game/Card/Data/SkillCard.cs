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

    public SkillCard()
    {
        CardType = ECardType.Skill;
    }

    public override void UseCard()
    {
        AddBuffs(EAddBuffTime.BeforeAttack);

        // TODO：添加护盾

        AddBuffs(EAddBuffTime.AfterAttack);
    }

    public override void UseCard(CharacterBase target)
    {
        AddBuffs(EAddBuffTime.BeforeAttack, target);

        // TODO：添加护盾

        AddBuffs(EAddBuffTime.AfterAttack, target);
    }
}
