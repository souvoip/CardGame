using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityCard", menuName = "Data/Card/AbilityCard")]
public class AbilityCard : CardBase
{
    public override ECardType CardType => ECardType.Ability;

    //public override void UseCard()
    //{
    //    AddBuffs(EBuffTriggerTime.None);
    //    BattleAnimManager.Instance.PlayAnim(BattleManager.Instance.Player.transform.position, cardAnimData);
    //    base.UseCard();
    //}
}
