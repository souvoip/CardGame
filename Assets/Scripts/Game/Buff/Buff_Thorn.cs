using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 荆棘:每受到一次伤害，对攻击者造成buff层数的伤害
/// </summary>
[CreateAssetMenu(fileName = "ThornBuff", menuName = "Data/Buff/ThornBuff")]
public class Buff_Thorn : BuffBase
{
    public override void AddEvents()
    {
        base.AddEvents();
        target.GetHitEvent += ThornDamage;
    }

    public override void RemoveEvents()
    {
        base.RemoveEvents();
        target.GetHitEvent -= ThornDamage;
    }

    private void ThornDamage(CharacterBase target, int damage)
    {
        target.ChangeAttribute(ERoleAttribute.HP, -currentStacks);
    }
}
