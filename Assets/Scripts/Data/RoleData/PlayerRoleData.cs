using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRoleData", menuName = "Data/Character/PlayerRoleData")]
public class PlayerRoleData : RoleData, ISaveLoad
{
    public int MaxAP;

    public int AP;

    public int Gold;

    /// <summary>
    /// 每回抽取的卡牌数量
    /// </summary>
    public int DrawCardCount;
    /// <summary>
    /// 最大卡牌数量
    /// </summary>
    public int MaxCardCount;
    /// <summary>
    /// 初始携带的物品, 开始游戏，读取时使用
    /// </summary>
    public List<int> FixedItemIDs = new List<int>();
    [NonSerialized]
    public List<ItemDataBase> Items = new List<ItemDataBase>();

    public void Load(JSONObject data)
    {
        ID = (int)data.GetField("ID").i;
        Name = data.GetField("Name").str;
        Sex = data.GetField("Sex").str;
        Age = (int)data.GetField("Age").i;
        Level = (int)data.GetField("Level").i;
        Exp = (int)data.GetField("Exp").i;
        RoleImgPath = data.GetField("RoleImgPath").str;
        MaxHP = (int)data.GetField("MaxHP").i;
        HP = (int)data.GetField("HP").i;
        MaxMP = (int)data.GetField("MaxMP").i;
        MP = (int)data.GetField("MP").i;
        Aesist = (int)data.GetField("Aesist").i;
        Shield = (int)data.GetField("Shield").i;
        MaxAP = (int)data.GetField("MaxAP").i;
        AP = (int)data.GetField("AP").i;
        Gold = (int)data.GetField("Gold").i;
        DrawCardCount = (int)data.GetField("DrawCardCount").i;
        MaxCardCount = (int)data.GetField("MaxCardCount").i;
        JSONObject fixedItemArray = data.GetField("FixedItemIDs");
        if(FixedItemIDs == null)
        {
            FixedItemIDs = new List<int>();
        }
        else
        {
            FixedItemIDs.Clear();
        }
        for(int i = 0; i < fixedItemArray.Count; i++)
        {
            FixedItemIDs.Add((int)fixedItemArray[i].i);
        }
        JSONObject fixedBattleBuffsArray = data.GetField("FixedBattleBuffs");
        if (FixedBattleBuffs == null)
        {
            FixedBattleBuffs = new List<BuffItem>();
        }
        else
        {
            FixedBattleBuffs.Clear();
        }
        for (int i = 0; i < fixedBattleBuffsArray.Count; i++)
        {
            BuffItem buffItem = new BuffItem();
            buffItem.Load(fixedBattleBuffsArray[i]);
            FixedBattleBuffs.Add(buffItem);
        }
        JSONObject itemsData = data.GetField("Items");
        if (Items == null)
        {
            Items = new List<ItemDataBase>();
        }
        else
        {
            Items.Clear();
        }
        for (int i = 0; i < itemsData.Count; i++)
        {
            ItemDataBase item = ItemDataManager.GetItem((int)itemsData[i].GetField("ID").i);
            item.Load(itemsData[i]);
            Items.Add(item);
        }
    }

    public JSONObject Save()
    {
        JSONObject data = JSONObject.Create();
        data.AddField("ID", ID);
        data.AddField("Name", Name);
        data.AddField("Sex", Sex);
        data.AddField("Age", Age);
        data.AddField("Level", Level);
        data.AddField("Exp", Exp);
        data.AddField("RoleImgPath", RoleImgPath);
        data.AddField("MaxHP", MaxHP);
        data.AddField("HP", HP);
        data.AddField("MaxMP", MaxMP);
        data.AddField("MP", MP);
        data.AddField("Aesist", Aesist);
        data.AddField("Shield", Shield);
        data.AddField("MaxAP", MaxAP);
        data.AddField("AP", AP);
        data.AddField("Gold", Gold);
        data.AddField("DrawCardCount", DrawCardCount);
        data.AddField("MaxCardCount", MaxCardCount);
        JSONObject fixedItemArray = JSONObject.Create(JSONObject.Type.ARRAY);
        foreach (var item in FixedItemIDs)
        {
            fixedItemArray.Add(item);
        }
        data.AddField("FixedItemIDs", fixedItemArray);
        JSONObject fixedBattleBuffsArray = JSONObject.Create(JSONObject.Type.ARRAY);
        foreach (var item in FixedBattleBuffs)
        {
            fixedBattleBuffsArray.Add(item.Save());
        }
        data.AddField("FixedBattleBuffs", fixedBattleBuffsArray);
        JSONObject itemsData = JSONObject.Create(JSONObject.Type.ARRAY);
        for (int i = 0; i < Items.Count; i++)
        {
            itemsData.Add(Items[i].Save());
        }
        data.AddField("Items", itemsData);
        return data;
    }
}