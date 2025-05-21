using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Battle_", menuName = "Data/BattleData/Battle")]
public class BattleData : ScriptableObject
{
    public int ID;

    public string BattleName;

    public int Level;

    public EBattleType BattleType;

    public List<BattleEnemyData> Enemies = new List<BattleEnemyData>();
}

[Serializable]
public class BattleEnemyData
{
    public int EnemyID;
    public Vector3 Position;
}

public enum EBattleType
{
    Normal,
    Hard,
    Boss,
}