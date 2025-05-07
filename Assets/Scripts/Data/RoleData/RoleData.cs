using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RoleData : ScriptableObject
{
    #region Base
    public int ID;
    /// <summary>
    /// 名字
    /// </summary>
    public string Name;
    /// <summary>
    /// 性别
    /// </summary>
    public string Sex;
    /// <summary>
    /// 年龄
    /// </summary>
    public int Age;
    /// <summary>
    /// 等级
    /// </summary>
    public int Level;
    /// <summary>
    /// 经验
    /// </summary>
    public int Exp;
    /// <summary>
    /// 角色图片路径
    /// </summary>
    public string RoleImgPath;
    #endregion

    #region Battle

    public int MaxHP;

    public int HP;

    public int MaxMP;

    public int MP;

    /// <summary>
    /// 抵抗
    /// </summary>
    public int Aesist;
    /// <summary>
    /// 护盾
    /// </summary>
    public int Shield;
    /// <summary>
    /// 开始战斗时，添加的buff
    /// </summary>
    public List<BuffItem> FixedBattleBuffs = new List<BuffItem>();
    #endregion
}
