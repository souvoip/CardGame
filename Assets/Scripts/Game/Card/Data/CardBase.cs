using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardBase
{
    /// <summary>
    /// 卡片ID
    /// </summary>
    public int ID;
    /// <summary>
    /// 卡片名字
    /// </summary>
    public string Name;
    /// <summary>
    /// 卡片描述
    /// </summary>
    public string Desc;
    /// <summary>
    /// 卡片图片
    /// </summary>
    public string ImagePath;
    /// <summary>
    /// 卡片类型
    /// </summary>
    public ECardType Type;
    /// <summary>
    /// 卡片使用类型
    /// </summary>
    public EUseType UseType;
    /// <summary>
    /// 卡片稀有度
    /// </summary>
    public int Rare;


    public virtual void LoadData(JSONObject data)
    {
        ID = (int)data.GetField("ID").i;
        Name = data.GetField("Name").str;
        Desc = data.GetField("Desc").str;
        ImagePath = data.GetField("ImagePath").str;
        Type = (ECardType)data.GetField("Type").i;
        UseType = (EUseType)data.GetField("UseType").i;
        Rare = (int)data.GetField("Rare").i;
    }
}

