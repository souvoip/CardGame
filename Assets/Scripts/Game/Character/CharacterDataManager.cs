using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public static class CharacterDataManager
{
    private static string characterDataPath = "Data/Character";

    private static List<PlayerRoleData> playerRoleDatas = new List<PlayerRoleData>();

    private static List<EnemyRoleData> enemyRoleDatas = new List<EnemyRoleData>();

    public static void Init()
    {
        // 加载玩家角色数据
        var pds = Resources.LoadAll<PlayerRoleData>(characterDataPath + "/Player");
        foreach (var item in pds)
        {
            playerRoleDatas.Add(item);
        }
        // 加载敌人角色数据
        var eds = Resources.LoadAll<EnemyRoleData>(characterDataPath + "/Enemy");
        foreach (var item in eds)
        {
            enemyRoleDatas.Add(item);
        }
    }

    public static PlayerRoleData GetPlayerRoleData(int id)
    {
        foreach (var item in playerRoleDatas)
        {
            if (item.ID == id) { return GameObject.Instantiate(item); }
        }
        return null;
    }

    public static EnemyRoleData GetEnemyRoleData(int id)
    {
        foreach (var item in enemyRoleDatas)
        {
            if (item.ID == id) { return GameObject.Instantiate(item); }
        }
        return null;
    }
}
