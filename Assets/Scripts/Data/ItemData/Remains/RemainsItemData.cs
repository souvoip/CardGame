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
    /// 获得该道具时调用
    /// </summary>
    public virtual void OnAcquire(bool isFirstGet = true)
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
