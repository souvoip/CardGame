public class RoleData
{
    #region Base
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
    #endregion

    #region Battle

    public int MaxHP;

    public int HP;

    public int MaxMP;

    public int MP;

    public int MaxAct;

    public int Act;
    /// <summary>
    /// 抵抗
    /// </summary>
    public int Aesist;
    /// <summary>
    /// 护盾
    /// </summary>
    public int Shield;
    #endregion
}
