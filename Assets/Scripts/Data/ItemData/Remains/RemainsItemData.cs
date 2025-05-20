using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 提供被动效果的道具
/// </summary>
public class RemainsItemData : ItemDataBase
{
    public override EItemType ItemType => EItemType.Remains;

    /// <summary>
    /// 是否是第一次获取，而不是通过读取游戏存档获得
    /// </summary>
    protected bool IsFirstAcquire = true;

    /// <summary>
    /// 获得该道具时调用
    /// </summary>
    public virtual void OnAcquire()
    {

    }
    /// <summary>
    /// 战斗开始时调用
    /// </summary>
    public virtual void OnBattleStart() { }

    public override DetailInfo GetDetailInfo()
    {
        DetailInfo info = new DetailInfo();
        info.Title = Name;
        info.Description = Description;
        //info.Icon = IconPath;
        return info;
    }
}
