using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCard : CardBase
{
    /// <summary>
    /// 给予的护盾值
    /// </summary>
    public Block BaseBlock;

    public SkillCard(JSONObject data)
    {
        LoadData(data);
        BaseBlock = new Block(data.GetField("BaseBlock"));
    }

    public SkillCard()
    {
        CardType = ECardType.Skill;
    }

    public override void LoadData(JSONObject data)
    {
        base.LoadData(data);
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


public class Block
{
    public int Value;
    public float Rate;

    public Block(int value, int rate) { Value = value; Rate = rate; }

    public Block(JSONObject data)
    {
        Value = (int)data.GetField("Value").i;
        Rate = data.GetField("Rate").f;
    }

    public Block(Block bk)
    {
        Value = bk.Value;
        Rate = bk.Rate;
    }

    public int GetBlockValue() { return (int)(Value * Rate); }
}