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
    public ECardType CardType;
    /// <summary>
    /// 卡片使用类型
    /// </summary>
    public EUseType UseType;
    /// <summary>
    /// 卡片稀有度
    /// </summary>
    public ECardRare Rare;
    /// <summary>
    /// 卡片费用
    /// </summary>
    public int Fee;

    public virtual void LoadData(JSONObject data)
    {
        ID = (int)data.GetField("ID").i;
        Name = data.GetField("Name").str;
        Desc = data.GetField("Desc").str;
        ImagePath = data.GetField("ImagePath").str;
        CardType = (ECardType)data.GetField("Type").i;
        UseType = (EUseType)data.GetField("UseType").i;
        Rare = (ECardRare)data.GetField("Rare").i;
        Fee = (int)data.GetField("Fee").i;
    }
}

public enum ECardRare
{
    /// <summary>
    /// 普通
    /// </summary>
    Common = 1,
    /// <summary>
    /// 稀有
    /// </summary>
    Rare = 2,
    /// <summary>
    /// 史诗
    /// </summary>
    Epic = 3,
}