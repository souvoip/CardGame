using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRoleData", menuName = "Data/Character/PlayerRoleData")]
public class PlayerRoleData : RoleData
{
    public int MaxAP;

    public int AP;

    /// <summary>
    /// 每回抽取的卡牌数量
    /// </summary>
    public int DrawCardCount;
    /// <summary>
    /// 最大卡牌数量
    /// </summary>
    public int MaxCardCount;
}
