using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkCard : CardBase
{
    public int BaseDamage;

    public override void LoadData(JSONObject data)
    {
        base.LoadData(data);
        BaseDamage = (int)data.GetField("BaseDamage").i;
    }

    public AtkCard(JSONObject data)
    {
        LoadData(data);
    }

    public AtkCard()
    {
        CardType = ECardType.Atk;
    }
}
