using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityCard", menuName = "Data/Card/AbilityCard")]
public class AbilityCard : CardBase
{
    public override void UseCard()
    {
        AddBuffs(EAddBuffTime.None);
        base.UseCard();
    }
}
