using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkCard : CardBase
{
    public int baseDamage;

    public override void LoadData(JSONObject data)
    {
        base.LoadData(data);
        baseDamage = (int)data.GetField("baseDamage").i;
    }

    public AtkCard(JSONObject data)
    {
        LoadData(data);
    }
}
