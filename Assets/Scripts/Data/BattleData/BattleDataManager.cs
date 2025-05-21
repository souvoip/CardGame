using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleDataManager
{
    public static List<BattleData> battles = new List<BattleData>();

    public static void Init()
    {
        battles.AddRange(Resources.LoadAll<BattleData>(ResourcesPaths.BattletDataPath));
    }

    public static BattleData GetBattle(int id)
    {
        foreach (BattleData battleData in battles)
        {
            if (battleData.ID == id) { return battleData; }
        }
        return null;
    }

    public static BattleData GetRandomBattle()
    {
        return battles[Random.Range(0, battles.Count)];
    }

    public static BattleData GetRandomBattle(int Level, EBattleType type)
    {
        var tempDatas = battles.FindAll(b => b.Level == Level && b.BattleType == type);
        if (tempDatas.Count == 0)
        {
            Debug.LogError("No battle found with level " + Level + " and type " + type);
            return null;
        }
        return tempDatas[Random.Range(0, tempDatas.Count)];
    }
}
