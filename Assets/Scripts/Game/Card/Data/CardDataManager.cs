using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardDataManager
{
    private static string cardDataPath = "Data/CardData";

    public static List<CardBase> Cards = new List<CardBase>();

    public static void Init()
    {
        var cds = Resources.LoadAll<CardBase>(cardDataPath);
        for (int i = 0; i < cds.Length; i++)
        {
            Cards.Add(cds[i]);
        }
    }

    public static CardBase GetCard(int id)
    {
        foreach (var item in Cards)
        {
            if (item.ID == id) { return GameObject.Instantiate(item); }
        }
        return null;
    }


    #region Test

    public static AtkCard GetTestData()
    {
        AtkCard atkCard = new AtkCard();
        atkCard.ID = 1;
        atkCard.Name = "斩击";
        atkCard.Desc = "造成5点伤害";
        atkCard.ImagePath = "001";
        atkCard.CardType = ECardType.Atk;
        atkCard.UseType = EUseType.Directivity;
        atkCard.Rare = ECardRare.Common;
        atkCard.Fee = 1;
        atkCard.BaseDamage = new Damage(5, 1);
        atkCard.Buffs = new List<BuffItem>();
        atkCard.Buffs.Add(new BuffItem(1, 1, EBuffTarget.Enemy, EAddBuffTime.AfterAttack));
        return atkCard;
    }

    public static void TestAtkData()
    {
        JSONObject testDatas = JSONObject.Create(JSONObject.Type.ARRAY);
        for (int i = 0; i < 2; i++)
        {
            var ta = GetTestData();
            testDatas.Add(ta.ToJSONObject());
        }
        Debug.Log(testDatas.ToString());
    }

    public static JSONObject ToJSONObject(this AtkCard atkCard)
    {
        JSONObject atkCardJson = JSONObject.Create();
        atkCardJson.AddField("ID", atkCard.ID);
        atkCardJson.AddField("Name", atkCard.Name);
        atkCardJson.AddField("Desc", atkCard.Desc);
        atkCardJson.AddField("ImagePath", atkCard.ImagePath);
        atkCardJson.AddField("Type", (int)atkCard.CardType);
        atkCardJson.AddField("UseType", (int)atkCard.UseType);
        atkCardJson.AddField("Rare", (int)atkCard.Rare);
        atkCardJson.AddField("Fee", atkCard.Fee);
        atkCardJson.AddField("BaseDamage", atkCard.BaseDamage.ToJSONObject());
        atkCardJson.AddField("Buffs", atkCard.Buffs.ToJSONObject());
        return atkCardJson;
    }

    public static JSONObject ToJSONObject(this Damage damage) 
    {
        JSONObject djson = JSONObject.Create();
        djson.AddField("DamageValue", damage.DamageValue);
        djson.AddField("DamageRate", damage.DamageRate);
        return djson;
    }

    public static JSONObject ToJSONObject(this List<BuffItem> buffItems)
    {
        JSONObject bjson = JSONObject.Create(JSONObject.Type.ARRAY);
        foreach (var item in buffItems)
        {
            bjson.Add(item.ToJSONObject());
        }
        return bjson;

    }

    public static JSONObject ToJSONObject(this BuffItem buffItem)
    {
        JSONObject bjson = JSONObject.Create();
        bjson.AddField("BuffID", buffItem.BuffID);
        bjson.AddField("Stacks", buffItem.Stacks);
        bjson.AddField("Target", (int)buffItem.Target);
        bjson.AddField("AddBuffTime", (int)buffItem.AddBuffTime);
        return bjson;
    }
    #endregion
}